using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 主动销售模板查询QCModel
    /// </summary>
    public class SalesTemplateQueryQCModel : BaseQCModel
    {
        #region 公共属性-条件用
        /// <summary>
        /// 销售模板ID
        /// </summary>
        public String WHERE_SasT_ID { get; set; }
        /// <summary>
        /// 销售模板名称
        /// </summary>
        public String WHERE_SasT_Name { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_SasT_Org_ID { get; set; }
        /// <summary>
        /// 汽修商户ID
        /// </summary>
        public String WHERE_SasT_CustomerID { get; set; }
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String WHERE_SasT_CustomerName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String WHERE_SasT_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_SasT_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_SasT_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_SasT_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_SasT_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_SasT_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_SasT_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_SasT_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_SasT_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_SasT_TransID { get; set; }
        #endregion

        #region 其他属性

        /// <summary>
        /// 汽修商户组织ID
        /// </summary>
        public String WHERE_AutoFactoryOrgID { get; set; }
        /// <summary>
        /// 汽修商户组织名称
        /// </summary>
        public String WHERE_AutoFactoryOrgName { get; set; }
        #endregion
    }
}
