using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Warren.OrderService.Common.Configuration;
using Warren.OrderService.Infrastructure.Common;
using Warren.OrderService.Infrastructure.Hash;
using Warren.OrderService.Infrastructure.Mysql.Dtos;
using static Dapper.SqlMapper;

namespace Warren.OrderService.Infrastructure.Mysql.DbContext
{
    public class DbContext
    {
        internal readonly DatabaseConfiguration _databaseConfiguration;
        public readonly DBHelper.ShardingHelper _shardingHelper;

        public DbContext(IConfiguration configuration)
        {
            _databaseConfiguration = configuration.GetSection("mysql").Get<DatabaseConfiguration>();
            _shardingHelper = new DBHelper.ShardingHelper(_databaseConfiguration);
        }

        public MySqlConnection GetSqlConnection(string connectionString)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
            if (mySqlConnection.State == System.Data.ConnectionState.Closed)
            {
                mySqlConnection.Open();
            }
            return mySqlConnection;
        }

        protected async Task<List<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            List<T> result = new List<T>();
            var shradResults = ConstructSQL(sql, param);
            foreach (var shradResult in shradResults)
            {
                using (var conn = GetSqlConnection(shradResult.connectionString))
                {
                    result.AddRange(await conn.QueryAsync<T>(shradResult.sql, shradResult.param, transaction, commandTimeout, commandType));
                }
            }
            return result;
        }

        protected async Task<List<T>> QueryMultipleAsync<T>(Func<GridReader, T> func, string sql, object param = null)
        {
            List<T> result = new List<T>();
            var shradResults = ConstructSQL(sql, param);
            foreach (var shradResult in shradResults)
            {
                using (var conn = GetSqlConnection(shradResult.connectionString))
                {
                    result.Add(func(await conn.QueryMultipleAsync(shradResult.sql, shradResult.param)));
                }
            }
            return result;
        }

        protected async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var shradResult = ConstructSQL(sql, param).FirstOrDefault();
            using (var conn = GetSqlConnection(shradResult.connectionString))
            {
                return await conn.QueryFirstOrDefaultAsync<T>(shradResult.sql, shradResult.param, transaction, commandTimeout, commandType);
            }
        }

        protected async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var shradResults = ConstructSQL(sql, param);
            if (shradResults.Count != 1)
            {
                throw new ArgumentNullException("sql error");
            }
            var shradResult = shradResults.FirstOrDefault();
            using (var conn = GetSqlConnection(shradResult.connectionString))
            {
                return await conn.ExecuteAsync(shradResult.sql, shradResult.param, transaction, commandTimeout, commandType);
            }
        }

        protected async Task<T> ExecuteScalarAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var shradResults = ConstructSQL(sql, param);
            if (shradResults.Count != 1)
            {
                throw new ArgumentNullException("sql error");
            }
            var shradResult = shradResults.FirstOrDefault();

            using (var conn = GetSqlConnection(shradResult.connectionString))
            {
                return await conn.ExecuteScalarAsync<T>(shradResult.sql, shradResult.param, transaction, commandTimeout, commandType);
            }
        }

        protected async Task<bool> BatchExecuteScalarAsync(List<Dtos.BatchExecuteScalar> batchExecuteScalar, Func<string, IDbTransaction, Task<bool>> func = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            List<ShradResult> shradResults = new List<ShradResult>();
            foreach (var item in batchExecuteScalar)
            {
                shradResults.AddRange(ConstructSQL(item.sql, item.param));
            }
            if (shradResults.Select(o => o.connectionString).Distinct().Count() != 1)
            {
                throw new ArgumentNullException("batch sql error");
            }

            using (var conn = GetSqlConnection(shradResults.FirstOrDefault().connectionString))
            {
                using (var trans = conn.BeginTransaction(IsolationLevel.RepeatableRead))
                {
                    foreach (var shradResult in shradResults)
                    {
                        //await conn.ExecuteAsync(shradResult.sql, shradResult.param, trans, commandTimeout, commandType);
                        var result = await conn.ExecuteAsync(shradResult.sql, shradResult.param, trans, commandTimeout, commandType) > 0;
                        if (!result)
                        {
                            return false;
                        }
                    }
                    if (func != null)
                    {
                        if (!await func?.Invoke(shradResults.FirstOrDefault().connectionString, trans))
                        {
                            return false;
                        }
                    }
                    trans.Commit();
                }
            }
            return true;
        }

        protected async Task<PagingResponse<List<T>>> QueryByPagingAsync<T>(string sql, int page_index, int page_size, object param)
        {
            var result = new List<T>();
            var shradResults = ConstructSQL(sql, param);
            var tasks = new List<int>();
            foreach (var shradResult in shradResults)
            {
                using (var conn = GetSqlConnection(shradResult.connectionString))
                {
                    var count = await conn.QueryFirstOrDefaultAsync<int>(ConstructCountSQL(shradResult.sql), shradResult.param);


                    /** Redis缓存查询结果优化
                     * 
                     
                    var hash = MD5Hash.Md5Hash(JsonHelper.JsonSerializeInternal(shradResult)).ToString();

                    if(Key是否存在)
                    {
                        获取结果
                    }
                    else
                    {
                        查询SQL
                        设置过期时间,存入缓存
                    }

                     */

                    tasks.Add(count);
                }
            };
            //await Task.WhenAll(tasks);
            var total = tasks.Sum();
            List<ShradResult> selectShradResults = new List<ShradResult>();
            var tuples = new List<Tuple<int, int, int>>();
            //查询总数
            int remaining_count = page_size * page_index;
            for (var i = 0; i < tasks.Count; i++)
            {
                //当前页数据量
                var count = tasks[i];
                if (count <= 0)
                {
                    continue;
                }
                if (remaining_count <= 0)
                {
                    break;
                }
                //当页数据量超过查询总数
                if (count >= remaining_count)
                {
                    //查询总数小于单页容量
                    if (remaining_count < page_size)
                    {
                        tuples.Add(new Tuple<int, int, int>(i, 0, remaining_count));
                    }
                    else
                    {
                        tuples.Add(new Tuple<int, int, int>(i, remaining_count - page_size, page_size));
                    }
                    break;
                }
                //当页数据量小于查询总数 说明有需要待查询数量
                else
                {
                    //是否需要在此页查询
                    if (remaining_count - count <= page_size)
                    {
                        if (page_size - remaining_count > 0)
                        {
                            tuples.Add(new Tuple<int, int, int>(i, 0, count));
                        }
                        else
                        {
                            tuples.Add(new Tuple<int, int, int>(i, remaining_count - page_size, Math.Abs(page_size - remaining_count + count)));
                        }
                    }
                }
                remaining_count -= count;
            }

            for (var i = 0; i < tuples.Count; i++)
            {
                var shradResult = shradResults[tuples[i].Item1];
                selectShradResults.Add(new ShradResult()
                {
                    connectionString = shradResult.connectionString,
                    sql = $"{shradResult.sql} limit {tuples[i].Item2},{tuples[i].Item3}"
                });
            }
            var selectTasks = new List<Task<IEnumerable<T>>>();
            foreach (var shradResult in selectShradResults)
            {
                using (var conn = GetSqlConnection(shradResult.connectionString))
                {
                    selectTasks.Add(conn.QueryAsync<T>(shradResult.sql, param));
                }
            }
            await Task.WhenAll(selectTasks);
            selectTasks.ForEach(o => result.AddRange(o.Result));
            return new Dtos.PagingResponse<List<T>>()
            {
                total = total,
                Data = result
            };
        }


        /// <summary>
        ///  select * from[db-name]{order_id}.order_attach{order_id}
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private List<ShradResult> ConstructSQL(string sql, object param)
        {
            List<ShradResult> shradResults = new List<ShradResult>();
            var db_left = sql.IndexOf('[');
            var db_right = sql.IndexOf(']');
            if (db_left < 0 || db_right < 0 || db_left >= db_right)
            {
                throw new ArgumentNullException("invalid sql");
            }
            var database = sql.Substring(db_left + 1, db_right - db_left - 1);
            if (string.IsNullOrWhiteSpace(database))
            {
                throw new ArgumentNullException("database not empty in sql");
            }
            var left = sql.IndexOf('{');
            var right = sql.IndexOf('}');
            if (left < 0 || right < 0 || left >= right)
            {
                //throw new ArgumentNullException("invalid sql");
                return new List<ShradResult>() {
                new ShradResult (){
                     connectionString =_shardingHelper.GetDataBaseTable(null, database).ConnectionString,
                      param =param,
                       sql =sql.Replace("[" + database + "].", "")
                } };
            }

            var sharding_column = sql.Substring(left + 1, right - left - 1);
            if (string.IsNullOrWhiteSpace(sharding_column))
            {
                throw new ArgumentNullException("sharding_column not empty in sql");
            }
            List<Dtos.DataBaseTable> dataBaseTables = new List<Dtos.DataBaseTable>();
            if (param.GetType().IsClass)
            {
                if (param.GetType().IsArray || param is IEnumerable)
                {
                    List<object> obj = new List<object>();
                    foreach (var item in param as IEnumerable)
                    {
                        System.Reflection.PropertyInfo propertyInfo = item.GetType().GetProperty(sharding_column);
                        obj.Add(propertyInfo.GetValue(item, null));
                    }
                    if (obj.Distinct().Count() == 1)
                    {
                        dataBaseTables.AddRange(GetDataBaseTables(obj[0], database));
                    }
                    else
                    {
                        throw new ArgumentNullException("sharding_column error in sql");
                    }
                }
                else if (param is DynamicParameters)
                {
                    var dynamicParameters = param as DynamicParameters;
                    var value = dynamicParameters.Get<object>(sharding_column);
                    dataBaseTables.AddRange(GetDataBaseTables(value, database));
                }
                else
                {
                    System.Reflection.PropertyInfo propertyInfo = param.GetType().GetProperty(sharding_column);
                    var value = propertyInfo.GetValue(param, null);
                    dataBaseTables.AddRange(GetDataBaseTables(value, database));
                }
            }
            else
            {
                if (!sharding_column.Equals(param))
                {
                    throw new ArgumentNullException("sharding column not empty");
                }
                dataBaseTables.AddRange(GetDataBaseTables(param, database));
            }
            foreach (var dataBaseTable in dataBaseTables)
            {
                shradResults.Add(new ShradResult()
                {
                    connectionString = dataBaseTable.ConnectionString,
                    sql = sql.Replace("[" + database + "]" + "{" + sharding_column + "}.", "").Replace("{" + sharding_column + "}", $"_{dataBaseTable.TableShrading}"),
                    param = param
                });
            }

            return shradResults;

            List<Dtos.DataBaseTable> GetDataBaseTables(object value, string database)
            {
                List<Dtos.DataBaseTable> dataBaseTables = new List<Dtos.DataBaseTable>();
                if (value.GetType().IsArray || value is IEnumerable)
                {
                    foreach (var item in value as IEnumerable)
                    {
                        var databaseTable = _shardingHelper.GetDataBaseTable(item, database);
                        if (string.IsNullOrWhiteSpace(databaseTable?.ConnectionString))
                        {
                            throw new ArgumentNullException("ConnectionString not empty");
                        }
                        if (!dataBaseTables.Exists(o => o.TableShrading.Equals(databaseTable.TableShrading) && o.ConnectionString.Equals(databaseTable.ConnectionString)))
                        {
                            dataBaseTables.Add(databaseTable);
                        }
                    }
                }
                else
                {
                    var databaseTable = _shardingHelper.GetDataBaseTable(value, database);
                    if (string.IsNullOrWhiteSpace(databaseTable?.ConnectionString))
                    {
                        throw new ArgumentNullException("ConnectionString not empty");
                    }
                    dataBaseTables.Add(databaseTable);
                }

                return dataBaseTables;
            }
        }

        /// <summary>
        ///     根据SelectSQL构建CountSQL
        /// </summary>
        /// <param name="SelectSql">完整查询SQL</param>
        /// <returns></returns>
        private string ConstructCountSQL(string SelectSql)
        {
            int start = SelectSql.IndexOf("select", StringComparison.OrdinalIgnoreCase) + "select".Length;
            int end = SelectSql.LastIndexOf("from", StringComparison.OrdinalIgnoreCase);
            if (start < end)
            {
                return SelectSql.Remove(start, end - start).Insert(start, " COUNT(1) ");
            }
            else
            {
                throw new ArgumentNullException("sql count error");
            }
        }
    }

    public class ShradResult
    {
        public string connectionString { get; set; }

        public string sql { get; set; }

        public object param { get; set; }
    }
}
