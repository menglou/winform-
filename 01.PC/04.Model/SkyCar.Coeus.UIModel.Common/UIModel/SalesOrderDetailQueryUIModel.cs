using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.Common.UIModel
{
    /// <summary>
    /// 销售订单明细UIModel
    /// </summary>
    public class SalesOrderDetailQueryUIModel : BaseUIModel
    {
        #region 公共属性
        /// <summary>
        /// 销售订单明细ID
        /// </summary>
        public String SOD_ID { get; set; }
        /// <summary>
        /// 销售订单ID
        /// </summary>
        public String SOD_SO_ID { get; set; }

        private Decimal? _sod_SalePriceRate;
        /// <summary>
        /// 计价基准
        /// </summary>
        public Decimal? SOD_SalePriceRate
        {
            get { return _sod_SalePriceRate; }
            set
            {
                if (_sod_SalePriceRate != value)
                {
                    PropertyValueChanged = true;
                }
                _sod_SalePriceRate = value;
            }
        }

        private Boolean? _sod_SalePriceRateIsChangeable;
        /// <summary>
        /// 计价基准可改
        /// </summary>
        public Boolean? SOD_SalePriceRateIsChangeable
        {
            get { return _sod_SalePriceRateIsChangeable; }
            set
            {
                if (_sod_SalePriceRateIsChangeable != value)
                {
                    PropertyValueChanged = true;
                }
                _sod_SalePriceRateIsChangeable = value;
            }
        }

        private Boolean? _sod_PriceIsIncludeTax;
        /// <summary>
        /// 价格是否含税
        /// </summary>
        public Boolean? SOD_PriceIsIncludeTax
        {
            get { return _sod_PriceIsIncludeTax; }
            set
            {
                if (_sod_PriceIsIncludeTax != value)
                {
                    PropertyValueChanged = true;
                }
                _sod_PriceIsIncludeTax = value;
            }
        }

        private Decimal? _sod_TaxRate;
        /// <summary>
        /// 税率
        /// </summary>
        public Decimal? SOD_TaxRate
        {
            get { return _sod_TaxRate; }
            set
            {
                if (_sod_TaxRate != value)
                {
                    PropertyValueChanged = true;
                }
                _sod_TaxRate = value;
            }
        }
        /// <summary>
        /// 税额
        /// </summary>
        public Decimal? SOD_TotalTax { get; set; }

        private Decimal? _sod_Qty;
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? SOD_Qty
        {
            get { return _sod_Qty; }
            set
            {
                if (_sod_Qty != value)
                {
                    PropertyValueChanged = true;
                }
                _sod_Qty = value;
            }
        }

        private Decimal? _sod_UnitPrice;
        /// <summary>
        /// 单价
        /// </summary>
        public Decimal? SOD_UnitPrice
        {
            get { return _sod_UnitPrice; }
            set
            {
                if (_sod_UnitPrice != value)
                {
                    PropertyValueChanged = true;
                }
                _sod_UnitPrice = value;
            }
        }
        /// <summary>
        /// 总金额
        /// </summary>
        public Decimal? SOD_TotalAmount { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String SOD_Barcode { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String SOD_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String SOD_Specification { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public String SOD_UOM { get; set; }

        private String _sod_StockInOrgID;
        /// <summary>
        /// 入库组织ID
        /// </summary>
        public String SOD_StockInOrgID
        {
            get { return _sod_StockInOrgID; }
            set
            {
                if (_sod_StockInOrgID != value)
                {
                    PropertyValueChanged = true;
                }
                _sod_StockInOrgID = value;
            }
        }
        private String _sod_StockInOrgName;
        /// <summary>
        /// 入库组织名称
        /// </summary>
        public String SOD_StockInOrgName
        {
            get { return _sod_StockInOrgName; }
            set
            {
                if (_sod_StockInOrgName != value)
                {
                    PropertyValueChanged = true;
                }
                _sod_StockInOrgName = value;
            }
        }
        private String _sod_StockInWarehouseID;
        /// <summary>
        /// 入库仓库ID
        /// </summary>
        public String SOD_StockInWarehouseID
        {
            get { return _sod_StockInWarehouseID; }
            set
            {
                if (_sod_StockInWarehouseID != value)
                {
                    PropertyValueChanged = true;
                }
                _sod_StockInWarehouseID = value;
            }
        }
        private String _sod_StockInWarehouseName;
        /// <summary>
        /// 入库仓库名称
        /// </summary>
        public String SOD_StockInWarehouseName
        {
            get { return _sod_StockInWarehouseName; }
            set
            {
                if (_sod_StockInWarehouseName != value)
                {
                    PropertyValueChanged = true;
                }
                _sod_StockInWarehouseName = value;
            }
        }
        private String _sod_StockInBinID;
        /// <summary>
        /// 入库仓位ID
        /// </summary>
        public String SOD_StockInBinID
        {
            get { return _sod_StockInBinID; }
            set
            {
                if (_sod_StockInBinID != value)
                {
                    PropertyValueChanged = true;
                }
                _sod_StockInBinID = value;
            }
        }
        private String _sod_StockInBinName;
        /// <summary>
        /// 入库仓位名称
        /// </summary>
        public String SOD_StockInBinName
        {
            get { return _sod_StockInBinName; }
            set
            {
                if (_sod_StockInBinName != value)
                {
                    PropertyValueChanged = true;
                }
                _sod_StockInBinName = value;
            }
        }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String SOD_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String SOD_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String SOD_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String SOD_ApprovalStatusName { get; set; }

        private String _sod_Remark;
        /// <summary>
        /// 备注
        /// </summary>
        public String SOD_Remark
        {
            get { return _sod_Remark; }
            set
            {
                if (_sod_Remark != value)
                {
                    PropertyValueChanged = true;
                }
                _sod_Remark = value;
            }
        }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SOD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SOD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SOD_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String SOD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SOD_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? SOD_VersionNo { get; set; }
        #endregion

        #region 其他属性

        /// <summary>
        /// 销售订单明细ID
        /// </summary>
        public String WHERE_SOD_ID { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_SOD_VersionNo { get; set; }

        /// <summary>
        /// 原厂编码
        /// </summary>
        public String INV_OEMNo { get; set; }

        /// <summary>
        /// 第三方编码
        /// </summary>
        public String INV_ThirdNo { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public String INV_BatchNo { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String INV_SUPP_ID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String INV_WH_ID { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String INV_WHB_ID { get; set; }
        /// <summary>
        /// 采购单价
        /// </summary>
        public Decimal? INV_PurchaseUnitPrice { get; set; }

        /// <summary>
        /// 配件品牌
        /// </summary>
        public String APA_Brand { get; set; }
        /// <summary>
        /// 配件级别
        /// </summary>
        public String APA_Level { get; set; }
        /// <summary>
        /// 车辆品牌
        /// </summary>
        public String APA_VehicleBrand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String APA_VehicleInspire { get; set; }
        /// <summary>
        /// 排量
        /// </summary>
        public String APA_VehicleCapacity { get; set; }
        /// <summary>
        /// 年款
        /// </summary>
        public String APA_VehicleYearModel { get; set; }
        /// <summary>
        /// 变速类型
        /// </summary>
        public String APA_VehicleGearboxType { get; set; }

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
        #endregion

    }
}
