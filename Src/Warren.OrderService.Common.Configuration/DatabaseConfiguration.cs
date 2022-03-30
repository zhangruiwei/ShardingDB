using System.Collections.Generic;

namespace Warren.OrderService.Common.Configuration
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DatabaseConfiguration
    {
        /// <summary>
        ///     数据库
        /// </summary>
        public List<DataBase> databases { get; set; }
    }

    /// <summary>
    /// 物理库
    /// </summary>
    public class DataSource
    {
        /// <summary>
        ///     连接字符串
        /// </summary>
        public string connectionString { get; set; }
        /// <summary>
        ///     物理库分片名
        /// </summary>
        public string shardingName { get; set; }
    }

    /// <summary>
    /// 逻辑库
    /// </summary>
    public class DataBase
    {
        /// <summary>
        ///     逻辑库名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        ///     物理库
        /// </summary>
        public List<DataSource> datasources { get; set; }
        /// <summary>
        ///     表分片数
        /// </summary>
        public int tableShardingCount { get; set; }
    }
}
