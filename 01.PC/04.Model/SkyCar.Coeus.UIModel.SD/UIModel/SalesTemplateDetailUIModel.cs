using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SD.UIModel
{
    /// <summary>
    /// 销售模板明细UIModel
    /// </summary>
    public class SalesTemplateDetailUIModel : BaseNotificationUIModel
    {
        #region 公共属性
        /// <summary>
        /// 销售模板明细ID
        /// </summary>
        public String SasTD_ID { get; set; }
        /// <summary>
        /// 销售模板ID
        /// </summary>
        public String SasTD_SasT_ID { get; set; }
        /// <summary>
        /// 价格是否含税
        /// </summary>
        public Boolean? SasTD_PriceIsIncludeTax { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        public Decimal? SasTD_TaxRate { get; set; }
        /// <summary>
        /// 税额
        /// </summary>
        public Decimal? SasTD_TotalTax { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? SasTD_Qty { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public Decimal? SasTD_UnitPrice { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public Decimal? SasTD_TotalAmount { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String SasTD_Barcode { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String SasTD_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String SasTD_Specification { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public String SasTD_UOM { get; set; }
        /// <summary>
        /// 汽修商组织ID
        /// </summary>
        public String SasTD_AutoFactoryOrgID { get; set; }
        /// <summary>
        /// 汽修商组织名称
        /// </summary>
        public String SasTD_AutoFactoryOrgName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String SasTD_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SasTD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SasTD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SasTD_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String SasTD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SasTD_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? SasTD_VersionNo { get; set; }
        #endregion

        /// <summary>
        /// 销售模板明细ID
        /// </summary>
        public String WHERE_SasTD_ID { get; set; }

        private bool _isChecked = false;
        /// <summary>
        /// 选择
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                RaisePropertyChanged(() => IsChecked);
            }
        }
        /// <summary>
        /// 总记录条数（翻页用）
        /// </summary>
        public Int32? RecordCount { get; set; }
    }
}
