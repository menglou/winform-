using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 入库管理UIModel
    /// </summary>
    public class StockInBillQueryUIModel : BaseUIModel
    {
        #region 入库单

        /// <summary>
        /// 单号
        /// </summary>
        public String SIB_No { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String SIB_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String SIB_SourceNo { get; set; }
        /// <summary>
        /// 单据状态
        /// </summary>
        public String SIB_StatusCode { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public String SIB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SIB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SIB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SIB_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String SIB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SIB_UpdatedTime { get; set; }
        /// <summary>
        /// 入库单ID
        /// </summary>
        public String SIB_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String SIB_Org_ID { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String SIB_SourceTypeName { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String SIB_StatusName { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String SIB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? SIB_VersionNo { get; set; }
        #endregion

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
        
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String SID_SUPP_ID { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String SUPP_Name { get; set; }
    }
}
