using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 微信用户明细异动Model
    /// </summary>
    public class MDLWC_UserDetailTrans
    {
        #region 公共属性
        /// <summary>
        /// 明细异动ID
        /// </summary>
        public String WUDT_ID { get; set; }
        /// <summary>
        /// 开放平台ID
        /// </summary>
        public String WUDT_UnionID { get; set; }
        /// <summary>
        /// OpenID
        /// </summary>
        public String WUDT_OpenID { get; set; }
        /// <summary>
        /// 认证标识
        /// </summary>
        public String WUDT_Mark { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public String WUDT_Name { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String WUDT_UserID { get; set; }
        /// <summary>
        /// 异动类型
        /// </summary>
        public String WUDT_Type { get; set; }
        /// <summary>
        /// 认证类型
        /// </summary>
        public String WUDT_CertificationType { get; set; }
        /// <summary>
        /// 异动时间
        /// </summary>
        public DateTime? WUDT_Time { get; set; }
        /// <summary>
        /// 异动时间-开始（查询条件用）
        /// </summary>
        public DateTime? _TimeStart { get; set; }
        /// <summary>
        /// 异动时间-终了（查询条件用）
        /// </summary>
        public DateTime? _TimeEnd { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WUDT_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WUDT_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WUDT_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WUDT_CreatedTime { get; set; }
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
        public String WUDT_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WUDT_UpdatedTime { get; set; }
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
        public Int64? WUDT_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WUDT_TransID { get; set; }
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
        /// 明细异动ID
        /// </summary>
        public String WHERE_WUDT_ID { get; set; }
        /// <summary>
        /// 开放平台ID
        /// </summary>
        public String WHERE_WUDT_UnionID { get; set; }
        /// <summary>
        /// OpenID
        /// </summary>
        public String WHERE_WUDT_OpenID { get; set; }
        /// <summary>
        /// 认证标识
        /// </summary>
        public String WHERE_WUDT_Mark { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public String WHERE_WUDT_Name { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String WHERE_WUDT_UserID { get; set; }
        /// <summary>
        /// 异动类型
        /// </summary>
        public String WHERE_WUDT_Type { get; set; }
        /// <summary>
        /// 认证类型
        /// </summary>
        public String WHERE_WUDT_CertificationType { get; set; }
        /// <summary>
        /// 异动时间
        /// </summary>
        public DateTime? WHERE_WUDT_Time { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_WUDT_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_WUDT_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_WUDT_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_WUDT_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_WUDT_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_WUDT_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_WUDT_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_WUDT_TransID { get; set; }
        #endregion

    }
}
