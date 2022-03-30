using System;
using System.Collections.Generic;
using System.Linq;
using Warren.OrderService.Common.Configuration;
using Warren.OrderService.Infrastructure.Mysql.Dtos;

namespace Warren.OrderService.Infrastructure.Mysql.DBHelper
{
    public class ShardingHelper
    {
        private readonly DatabaseConfiguration _databaseConfiguration;

        public ShardingHelper(DatabaseConfiguration databaseConfiguration)
        {
            _databaseConfiguration = databaseConfiguration;
        }

        public DataBaseTable GetDataBaseTable(object value, string database)
        {
            var dataBase = _databaseConfiguration.databases.Find(o => o.name.Equals(database, StringComparison.OrdinalIgnoreCase));
            if (dataBase == null)
            {
                throw new ArgumentNullException("逻辑库配置不存在，检查配置文件 mysql.config");
            }
            return GetDataBaseTable(value, dataBase);
        }

        DataBaseTable GetDataBaseTable(object value, DataBase dataBase)
        {
            if (value is DateTime data)
            {
                var year_count = dataBase.datasources.FindAll(o => o.shardingName.StartsWith($"{data.Year}")).Count;
                if (year_count <= 0)
                {
                    throw new ArgumentNullException("year_count", "找不到对应的年份分片");
                }
                var shrading = $"{data.Year}_{data.Month % year_count}";
                var datasource = dataBase.datasources.Find(o => o.shardingName.Equals(shrading, StringComparison.OrdinalIgnoreCase));
                if (datasource == null)
                {
                    throw new ArgumentNullException("物理库配置不存在，检查配置文件 mysql.config");
                }
                return new DataBaseTable()
                {
                    ConnectionString = datasource.connectionString,
                    TableShrading = string.Empty
                };
            }
            else if (value is long || value is ulong)
            {
                ulong index = Convert.ToUInt64(value);
                var intermediate = index % Convert.ToUInt64(dataBase.datasources.Count * dataBase.tableShardingCount);

                var shrading = (intermediate / Convert.ToUInt64(dataBase.tableShardingCount)).ToString();
                var datasource = dataBase.datasources.Find(o => o.shardingName.Equals(shrading, StringComparison.OrdinalIgnoreCase));
                if (datasource == null)
                {
                    throw new ArgumentNullException("物理库配置不存在，检查配置文件 mysql.config");
                }
                return new DataBaseTable()
                {
                    ConnectionString = datasource.connectionString,
                    TableShrading = (intermediate % Convert.ToUInt64(dataBase.tableShardingCount)).ToString()
                };
            }
            else if (value == null)
            {
                var datasource = dataBase.datasources.FirstOrDefault();
                if (datasource == null)
                {
                    throw new ArgumentNullException("物理库配置不存在，检查配置文件 mysql.config");
                }
                return new DataBaseTable()
                {
                    ConnectionString = datasource.connectionString,
                    TableShrading = string.Empty
                };
            }
            throw new ArgumentNullException("invalid sharding");
        }

        public List<DateTime> DateTimeIntervalSplit(DateTime StartDate, DateTime EndDate)
        {
            if (EndDate < new DateTime(2022, 01, 01, 00, 00, 00))
            {
                return new List<DateTime>();
            }
            StartDate = StartDate > new DateTime(2022, 01, 01, 00, 00, 00) ? StartDate : new DateTime(2022, 01, 01, 00, 00, 00);

            var start_year = StartDate.Year;
            var end_year = EndDate.Year;

            var start_month = StartDate.Month;
            var end_month = EndDate.Month;
            var dateTimes = new List<DateTime>();
            for (var year = start_year; year <= end_year; year++)
            {
                var max_month = year != end_year ? 12 : end_month;
                for (var month = start_month; month <= max_month; month++)
                {
                    dateTimes.Add(new DateTime(year, month, 1));
                }
                start_month = 1;
            }
            return dateTimes;
        }
    }
}
