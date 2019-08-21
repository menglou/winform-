using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 出库管理UIModel
    /// </summary>
    public class StockOutBillQueryUIModel : BaseUIModel
    {
        #region 出库管理属性
       
        /// <summary>
        /// 单据编号
        /// </summary>
        public String SOB_No { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String SOB_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String SOB_SourceNo { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String SOB_SUPP_ID { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String SOB_SUPP_Name { get; set; }
        /// <summary>
        /// 单据状态
        /// </summary>
        public String SOB_StatusName { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public String SOB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String SOB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SOB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SOB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SOB_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String SOB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SOB_UpdatedTime { get; set; }
        /// <summary>
        /// 出库单ID
        /// </summary>
        public String SOB_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String SOB_Org_ID { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String SOB_SourceTypeCode { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String SOB_StatusCode { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String SOB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? SOB_VersionNo { get; set; }
        #endregion

        #region 其他属性

        private Boolean _isChecked = false;
        /// <summary>
        /// 选择
        /// </summary>
        public Boolean IsChecked
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
        /// 行标识
        /// </summary>
        public string RowID { get; set; }

        #endregion
    }
}
