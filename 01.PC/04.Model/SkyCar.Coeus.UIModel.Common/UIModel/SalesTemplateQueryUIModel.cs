using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 主动销售模板查询UIModel
    /// </summary>
    public class SalesTemplateQueryUIModel : BaseUIModel
    {
        /// <summary>
        /// 行标识
        /// </summary>
        public string RowID { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked { get; set; }

        #region 公共属性
        /// <summary>
        /// 销售模板ID
        /// </summary>
        public String SasT_ID { get; set; }
        /// <summary>
        /// 销售模板名称
        /// </summary>
        public String SasT_Name { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String SasT_Org_ID { get; set; }
        /// <summary>
        /// 汽修商户ID
        /// </summary>
        public String SasT_CustomerID { get; set; }
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String SasT_CustomerName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String SasT_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String SasT_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String SasT_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SasT_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SasT_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SasT_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String SasT_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SasT_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? SasT_VersionNo { get; set; }
        #endregion

        /// <summary>
        /// 汽修商组织ID
        /// </summary>
        public String AROrgID { get; set; }
        /// <summary>
        /// 汽修商组织名称
        /// </summary>
        public String AROrgName { get; set; }
    }
}
