using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 用户登录日志Model
    /// </summary>
    public class MDLSM_UserLoginLog
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String ULL_ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String ULL_User_ID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public String ULL_User_Name { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public String ULL_IPAdress { get; set; }
        /// <summary>
        /// MAC地址
        /// </summary>
        public String ULL_MACAdress { get; set; }
        /// <summary>
        /// 主机名称
        /// </summary>
        public String ULL_HostName { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String ULL_OrgID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String ULL_OrgName { get; set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public String ULL_LogType { get; set; }
        /// <summary>
        /// 终端类型
        /// </summary>
        public String ULL_TerminalType { get; set; }
        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime? ULL_LogTime { get; set; }
        /// <summary>
        /// 发生时间-开始（查询条件用）
        /// </summary>
        public DateTime? _LogTimeStart { get; set; }
        /// <summary>
        /// 发生时间-终了（查询条件用）
        /// </summary>
        public DateTime? _LogTimeEnd { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? ULL_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String ULL_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? ULL_CreatedTime { get; set; }
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
        public String ULL_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ULL_UpdatedTime { get; set; }
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
        public Int64? ULL_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String ULL_TransID { get; set; }
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

        #region 公共属性-条件用
        /// <summary>
        /// ID
        /// </summary>
        public String WHERE_ULL_ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String WHERE_ULL_User_ID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public String WHERE_ULL_User_Name { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public String WHERE_ULL_IPAdress { get; set; }
        /// <summary>
        /// MAC地址
        /// </summary>
        public String WHERE_ULL_MACAdress { get; set; }
        /// <summary>
        /// 主机名称
        /// </summary>
        public String WHERE_ULL_HostName { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_ULL_OrgID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String WHERE_ULL_OrgName { get; set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public String WHERE_ULL_LogType { get; set; }
        /// <summary>
        /// 终端类型
        /// </summary>
        public String WHERE_ULL_TerminalType { get; set; }
        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime? WHERE_ULL_LogTime { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_ULL_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_ULL_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_ULL_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_ULL_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_ULL_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_ULL_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_ULL_TransID { get; set; }
        #endregion

    }
}
