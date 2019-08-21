using System;

namespace SkyCar.Coeus.UIModel.Common.UIModel
{
    /// <summary>
    /// 商户服务器管理Model
    /// </summary>
    public class MerchantServerUIModel
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String MS_ID { get; set; }
        /// <summary>
        /// 商户ID
        /// </summary>
        public String MS_MCT_ID { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public String MS_PDT_ID { get; set; }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public String MS_ConnectString { get; set; }
        /// <summary>
        /// 数据库连接
        /// </summary>
        public String MS_DataSource { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        public String MS_DataBase { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public String MS_UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public String MS_PassWord { get; set; }
        /// <summary>
        /// 是否允许连接池
        /// </summary>
        public Boolean? MS_IsEnablePooling { get; set; }
        /// <summary>
        /// 最小连接池数
        /// </summary>
        public Int64? MS_MinPoolSize { get; set; }
        /// <summary>
        /// 最大连接池数
        /// </summary>
        public Int64? MS_MaxPoolSize { get; set; }
        /// <summary>
        /// 连接超时时间
        /// </summary>
        public Int64? MS_ConnectTimeout { get; set; }
        /// <summary>
        /// 缓存写服务器IP
        /// </summary>
        public String MS_RedisWriteServerIP { get; set; }
        /// <summary>
        /// 缓存写服务器端口号
        /// </summary>
        public String MS_RedisWriteServerPort { get; set; }
        /// <summary>
        /// 最大写缓存池大小
        /// </summary>
        public String MS_RedisMaxWritePoolSize { get; set; }
        /// <summary>
        /// 缓存读服务器IP
        /// </summary>
        public String MS_RedisReadServerIP { get; set; }
        /// <summary>
        /// 缓存读服务器端口号
        /// </summary>
        public String MS_RedisReadServerPort { get; set; }
        /// <summary>
        /// 最大读缓存池大小
        /// </summary>
        public String MS_RedisMaxReadPoolSize { get; set; }
        /// <summary>
        /// 是否自动启动
        /// </summary>
        public Boolean? MS_RedisIsAutoStart { get; set; }
        /// <summary>
        /// 本地缓存时间
        /// </summary>
        public String MS_RedisLocalCacheTime { get; set; }
        /// <summary>
        /// 是否记录日志
        /// </summary>
        public Boolean? MS_RedisIsRecordeLog { get; set; }
        /// <summary>
        /// 商户级缓存Key
        /// </summary>
        public String MS_RedisSystemKey { get; set; }
        /// <summary>
        /// 日志缓存写服务器IP
        /// </summary>
        public String MS_LogRedisWriteServerIP { get; set; }
        /// <summary>
        /// 日志缓存写服务器端口号
        /// </summary>
        public String MS_LogRedisWriteServerPort { get; set; }
        /// <summary>
        /// 日志最大写缓存池大小
        /// </summary>
        public String MS_LogRedisMaxWritePoolSize { get; set; }
        /// <summary>
        /// 日志缓存读服务器IP
        /// </summary>
        public String MS_LogRedisReadServerIP { get; set; }
        /// <summary>
        /// 日志缓存读服务器端口号
        /// </summary>
        public String MS_LogRedisReadServerPort { get; set; }
        /// <summary>
        /// 日志最大读缓存池大小
        /// </summary>
        public String MS_LogRedisMaxReadPoolSize { get; set; }
        /// <summary>
        /// 日志缓存服务是否自动启动
        /// </summary>
        public Boolean? MS_LogRedisIsAutoStart { get; set; }
        /// <summary>
        /// 日志本地缓存时间
        /// </summary>
        public String MS_LogRedisLocalCacheTime { get; set; }
        /// <summary>
        /// 日志缓存是否记录日志
        /// </summary>
        public Boolean? MS_LogRedisIsRecordeLog { get; set; }
        /// <summary>
        /// 日志商户级缓存Key
        /// </summary>
        public String MS_LogRedisSystemKey { get; set; }
        /// <summary>
        /// PC端消息队列通道
        /// </summary>
        public String MS_ChannelPC { get; set; }
        /// <summary>
        /// 微信端消息队列通道
        /// </summary>
        public String MS_ChannelWechat { get; set; }
        /// <summary>
        /// 备用消息队列通道
        /// </summary>
        public String MS_ChannelBK { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String MS_Remark { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public Boolean? MS_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String MS_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? MS_CreatedTime { get; set; }
        /// <summary>
        /// 创建时间-开始（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeStart { get; set; }
        /// <summary>
        /// 创建时间-终了（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeEnd { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String MS_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? MS_UpdatedTime { get; set; }
        /// <summary>
        /// 修改时间-开始（查询条件用）
        /// </summary>
        public DateTime? _UpdatedTimeStart { get; set; }
        /// <summary>
        /// 修改时间-终了（查询条件用）
        /// </summary>
        public DateTime? _UpdatedTimeEnd { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? MS_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String MS_TransID { get; set; }
        /// <summary>
        /// 总记录条数（翻页用）
        /// </summary>
        public Int32? RecordCount { get; set; }
        /// <summary>
        /// 当前页面索引/要跳转的页码（翻页用）
        /// </summary>
        public Int32? PageIndex { get; set; }
        /// <summary>
        /// 页面大小（翻页用）
        /// </summary>
        public Int32? PageSize { get; set; }
        #endregion

        /// <summary>
        /// 汽修商户编码
        /// </summary>
        public String AR_MCT_Code { get; set; }
    }
}
