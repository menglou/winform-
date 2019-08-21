using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 微信用户明细Model
    /// </summary>
    public class MDLWC_UserDetail
    {
        #region 公共属性
        /// <summary>
        /// 微信用户明细ID
        /// </summary>
        public String WUD_ID { get; set; }
        /// <summary>
        /// 开放平台ID
        /// </summary>
        public String WUD_UnionID { get; set; }
        /// <summary>
        /// OpenID
        /// </summary>
        public String WUD_OpenID { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public String WUD_Type { get; set; }
        /// <summary>
        /// 认证时间
        /// </summary>
        public DateTime? WUD_CertificationTime { get; set; }
        /// <summary>
        /// 认证时间-开始（查询条件用）
        /// </summary>
        public DateTime? _CertificationTimeStart { get; set; }
        /// <summary>
        /// 认证时间-终了（查询条件用）
        /// </summary>
        public DateTime? _CertificationTimeEnd { get; set; }
        /// <summary>
        /// 认证标识
        /// </summary>
        public String WUD_Mark { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public String WUD_Name { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String WUD_UserID { get; set; }
        /// <summary>
        /// 是否商户管理者
        /// </summary>
        public Boolean? WUD_IsManager { get; set; }
        /// <summary>
        /// 是否允许当前商户被多次认证
        /// </summary>
        public Boolean? WUD_AllowMultipleCertificate { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WUD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WUD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WUD_CreatedTime { get; set; }
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
        public String WUD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WUD_UpdatedTime { get; set; }
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
        public Int64? WUD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WUD_TransID { get; set; }
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
        /// 微信用户明细ID
        /// </summary>
        public String WHERE_WUD_ID { get; set; }
        /// <summary>
        /// 开放平台ID
        /// </summary>
        public String WHERE_WUD_UnionID { get; set; }
        /// <summary>
        /// OpenID
        /// </summary>
        public String WHERE_WUD_OpenID { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public String WHERE_WUD_Type { get; set; }
        /// <summary>
        /// 认证时间
        /// </summary>
        public DateTime? WHERE_WUD_CertificationTime { get; set; }
        /// <summary>
        /// 认证标识
        /// </summary>
        public String WHERE_WUD_Mark { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public String WHERE_WUD_Name { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String WHERE_WUD_UserID { get; set; }
        /// <summary>
        /// 是否商户管理者
        /// </summary>
        public Boolean? WHERE_WUD_IsManager { get; set; }
        /// <summary>
        /// 是否允许当前商户被多次认证
        /// </summary>
        public Boolean? WHERE_WUD_AllowMultipleCertificate { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_WUD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_WUD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_WUD_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_WUD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_WUD_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_WUD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_WUD_TransID { get; set; }
        #endregion

    }
}
