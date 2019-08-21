using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.BS
{
    /// <summary>
    /// 配件档案UIModel
    /// </summary>
    public class AutoPartsArchiveManagerUIModel : BaseUIModel
    {
        #region 配件档案

        /// <summary>
        /// 条形码
        /// </summary>
        public String APA_Barcode { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String APA_Name { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String APA_Brand { get; set; }
        /// <summary>
        /// 配件级别
        /// </summary>
        public String APA_Level { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String APA_OEMNo { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String APA_ThirdNo { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String APA_Specification { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public String APA_UOM { get; set; }
        /// <summary>
        /// 变速类型编码
        /// </summary>
        public String APA_VehicleGearboxTypeCode { get; set; }
        /// <summary>
        /// 排量
        /// </summary>
        public String APA_VehicleCapacity { get; set; }
        /// <summary>
        /// 年款
        /// </summary>
        public String APA_VehicleYearModel { get; set; }
        /// <summary>
        /// 安全库存是否预警
        /// </summary>
        public Boolean? APA_IsWarningSafeStock { get; set; }
        /// <summary>
        /// 安全库存
        /// </summary>
        public Int32? APA_SafeStockNum { get; set; }
        /// <summary>
        /// 呆滞件是否预警
        /// </summary>
        public Boolean? APA_IsWarningDeadStock { get; set; }
        /// <summary>
        /// 呆滞天数
        /// </summary>
        public Int32? APA_SlackDays { get; set; }
        /// <summary>
        /// 车辆品牌
        /// </summary>
        public String APA_VehicleBrand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String APA_VehicleInspire { get; set; }
        /// <summary>
        /// 销价系数
        /// </summary>
        public Decimal? APA_SalePriceRate { get; set; }
        /// <summary>
        /// 销价
        /// </summary>
        public Decimal? APA_SalePrice { get; set; }
        /// <summary>
        /// 车型代码
        /// </summary>
        public String APA_VehicleModelCode { get; set; }
        /// <summary>
        /// 互换码
        /// </summary>
        public String APA_ExchangeCode { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? APA_IsValid { get; set; }
        /// <summary>
        /// 配置档案ID
        /// </summary>
        public String APA_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String APA_Org_ID { get; set; }
        /// <summary>
        /// 变速类型名称
        /// </summary>
        public String APA_VehicleGearboxTypeName { get; set; }
        /// <summary>
        /// 默认供应商ID
        /// </summary>
        public String APA_SUPP_ID { get; set; }
        /// <summary>
        /// 默认仓库ID
        /// </summary>
        public String APA_WH_ID { get; set; }
        /// <summary>
        /// 默认仓位ID
        /// </summary>
        public String APA_WHB_ID { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? APA_VersionNo { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String APA_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? APA_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String APA_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APA_UpdatedTime { get; set; }
        #endregion

        #region 其他

        /// <summary>
        /// 默认供应商
        /// </summary>
        public String SUPP_Name { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        public String WH_Name { get; set; }
        /// <summary>
        /// 仓位
        /// </summary>
        public String WHB_Name { get; set; }

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
        #endregion
    }
}
