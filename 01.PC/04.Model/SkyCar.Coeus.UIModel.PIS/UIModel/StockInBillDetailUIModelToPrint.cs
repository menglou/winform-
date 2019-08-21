using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 入库管理明细UIModel
    /// </summary>
    public class StockInBillDetailUIModelToPrint : BaseNotificationUIModel
    {
        #region 入库单明细

        /// <summary>
        /// 销售订单明细ID
        /// </summary>
        public String WHERE_SID_ID { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_SID_VersionNo { get; set; }

        /// <summary>
        /// 入库单明细ID
        /// </summary>
        public String SID_ID { get; set; }
        /// <summary>
        /// 入库单ID
        /// </summary>
        public String SID_SIB_ID { get; set; }
        /// <summary>
        /// 入库单号
        /// </summary>
        public String SID_SIB_No { get; set; }
        /// <summary>
        /// 来源单明细ID
        /// </summary>
        public String SID_SourceDetailID { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String SID_Barcode { get; set; }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String SID_BatchNo { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String SID_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String SID_OEMNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String SID_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String SID_Specification { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String SID_SUPP_ID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String SID_WH_ID { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String SID_WHB_ID { get; set; }

        private Decimal? _sid_Qty;
        /// <summary>
        /// 入库数量
        /// </summary>
        public Decimal? SID_Qty
        {
            get { return _sid_Qty; }
            set
            {
                _sid_Qty = value;
                RaisePropertyChanged(() => SID_Qty);
            }
        }
        /// <summary>
        /// 单位
        /// </summary>
        public String SID_UOM { get; set; }

        private Decimal? _sid_UnitCostPrice;
        /// <summary>
        /// 入库单价
        /// </summary>
        public Decimal? SID_UnitCostPrice
        {
            get { return _sid_UnitCostPrice; }
            set
            {
                _sid_UnitCostPrice = value;
                RaisePropertyChanged(() => SID_UnitCostPrice);
            }
        }

        /// <summary>
        /// 入库金额
        /// </summary>
        public Decimal? SID_Amount { get; set; }
        /// <summary>
        /// 已结算标志
        /// </summary>
        public Boolean? SID_IsSettled { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SID_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SID_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SID_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String SID_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SID_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? SID_VersionNo { get; set; }
        #endregion

        #region 其他属性

        /// <summary>
        /// 配件品牌
        /// </summary>
        public String APA_Brand { get; set; }
        /// <summary>
        /// 配件级别
        /// </summary>
        public String APA_Level { get; set; }
        /// <summary>
        /// 品牌
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
        public String APA_VehicleGearboxTypeName { get; set; }
        /// <summary>
        /// 变速类型编码
        /// </summary>
        public String APA_VehicleGearboxTypeCode { get; set; }

        private String _supp_Name;
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String SUPP_Name
        {
            get { return _supp_Name; }
            set
            {
                _supp_Name = value;
                RaisePropertyChanged(() => SUPP_Name);
            }
        }
        private String _wh_Name;
        /// <summary>
        /// 仓库名称
        /// </summary>
        public String WH_Name
        {
            get { return _wh_Name; }
            set
            {
                _wh_Name = value;
                RaisePropertyChanged(() => WH_Name);
            }
        }
        private String _whb_Name;
        /// <summary>
        /// 仓位名称
        /// </summary>
        public String WHB_Name
        {
            get { return _whb_Name; }
            set
            {
                _whb_Name = value;
                RaisePropertyChanged(() => WHB_Name);
            }
        }

        /// <summary>
        /// 临时入库单明细ID
        /// </summary>
        public String Tmp_SID_ID { get; set; }

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
        #endregion
    }
}
