using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SD
{
    /// <summary>
    /// 销售订单明细UIModel
    /// </summary>
    public class SalesOrderDetailUIModelToPrint : BaseNotificationUIModel
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
                _sod_SalePriceRate = value;
                RaisePropertyChanged(() => SOD_SalePriceRate);
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
                _sod_SalePriceRateIsChangeable = value;
                RaisePropertyChanged(() => SOD_SalePriceRateIsChangeable);
            }
        }

        private String _sod_PriceIsIncludeTax;
        /// <summary>
        /// 价格是否含税
        /// </summary>
        public String SOD_PriceIsIncludeTax
        {
            get { return _sod_PriceIsIncludeTax; }
            set
            {
                _sod_PriceIsIncludeTax = value;
                RaisePropertyChanged(() => SOD_PriceIsIncludeTax);
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
                _sod_TaxRate = value;
                RaisePropertyChanged(() => SOD_TaxRate);
            }
        }
        /// <summary>
        /// 税额
        /// </summary>
        public Decimal? SOD_TotalTax { get; set; }

        private Decimal? _sod_Qty;
        /// <summary>
        /// 销售数量
        /// </summary>
        public Decimal? SOD_Qty
        {
            get { return _sod_Qty; }
            set
            {
                _sod_Qty = value;
                RaisePropertyChanged(() => SOD_Qty);
            }
        }


        private Decimal? _sodSignQty;
        /// <summary>
        /// 签收数量
        /// </summary>
        public Decimal? SOD_SignQty
        {
            get { return _sodSignQty; }
            set
            {
                _sodSignQty = value;
                RaisePropertyChanged(() => SOD_SignQty);
            }
        }
        private Decimal? _sodRejectQty;
        /// <summary>
        /// 拒收数量
        /// </summary>
        public Decimal? SOD_RejectQty
        {
            get { return _sodRejectQty; }
            set
            {
                _sodRejectQty = value;
                RaisePropertyChanged(() => SOD_RejectQty);
            }
        }
        private Decimal? _sodLoseQty;
        /// <summary>
        /// 丢失数量
        /// </summary>
        public Decimal? SOD_LoseQty
        {
            get { return _sodLoseQty; }
            set
            {
                _sodLoseQty = value;
                RaisePropertyChanged(() => SOD_LoseQty);
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
                _sod_UnitPrice = value;
                RaisePropertyChanged(() => SOD_UnitPrice);
            }
        }
        /// <summary>
        /// 总金额
        /// </summary>
        public Decimal? SOD_TotalAmount { get; set; }
        /// <summary>
        /// 配件批次号（汽修厂用）
        /// </summary>
        public String SOD_BatchNoNew { get; set; }
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
                _sod_StockInOrgID = value;
                RaisePropertyChanged(() => SOD_StockInOrgID);
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
                _sod_StockInOrgName = value;
                RaisePropertyChanged(() => SOD_StockInOrgName);
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
                _sod_StockInWarehouseID = value;
                RaisePropertyChanged(() => SOD_StockInWarehouseID);
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
                _sod_StockInWarehouseName = value;
                RaisePropertyChanged(() => SOD_StockInWarehouseName);
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
                _sod_StockInBinID = value;
                RaisePropertyChanged(() => SOD_StockInBinID);
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
                _sod_StockInBinName = value;
                RaisePropertyChanged(() => SOD_StockInBinName);
            }
        }
        private String _sod_Remark;
        /// <summary>
        /// 备注
        /// </summary>
        public String SOD_Remark
        {
            get { return _sod_Remark; }
            set
            {
                _sod_Remark = value;
                RaisePropertyChanged(() => SOD_Remark);
            }
        }
        #endregion

        #region 其他属性

        #region 库存

        /// <summary>
        /// 第三方编码
        /// </summary>
        public String INV_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String INV_OEMNo { get; set; }
        #endregion

        #region 配件档案

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
        /// 变速类型编码
        /// </summary>
        public String APA_VehicleGearboxTypeCode { get; set; }
        /// <summary>
        /// 变速类型名称
        /// </summary>
        public String APA_VehicleGearboxTypeName { get; set; }

        #endregion

        /// <summary>
        /// 出库仓位名称
        /// </summary>
        public String StockOutBinName { get; set; }
        #endregion
    }
}
