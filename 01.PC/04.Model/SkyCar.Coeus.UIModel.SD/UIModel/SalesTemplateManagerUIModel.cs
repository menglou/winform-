using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SD
{
    /// <summary>
    /// 主动销售模板管理UIModel
    /// </summary>
    public class SalesTemplateManagerUIModel : BaseUIModel
    {
        /// <summary>
        /// 销售模板名称
        /// </summary>
        public String SasT_Name { get; set; }
        /// <summary>
        /// 汽修商户编码
        /// </summary>
        public String SasT_AutoFactoryCode { get; set; }
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String SasT_AutoFactoryName { get; set; }
        /// <summary>
        /// 汽修商客户ID
        /// </summary>
        public String SasT_CustomerID { get; set; }
        /// <summary>
        /// 汽修商客户名称
        /// </summary>
        public String SasT_CustomerName { get; set; }
        /// <summary>
        /// 汽修商组织编码
        /// </summary>
        public String SasT_AutoFactoryOrgCode { get; set; }
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
        /// 销售模板ID
        /// </summary>
        public String SasT_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String SasT_Org_ID { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String SasT_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? SasT_VersionNo { get; set; }
        
        private bool _isChecked = false;
        /// <summary>
        /// 选择
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    PropertyValueChanged = true;
                }
                _isChecked = value;
            }
        }

        /// <summary>
        /// 组织简称
        /// </summary>
        public String Org_ShortName { get; set; }

        /// <summary>
        /// 汽修商组织ID
        /// </summary>
        public String AROrgID { get; set; }
        /// <summary>
        /// 汽修商户组织信息（平台内汽修商专用）
        /// </summary>
        public String AutoFactoryOrgInfo { get; set; }
       
    }
}
