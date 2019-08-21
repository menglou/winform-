using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 出库管理明细UIModel
    /// </summary>
    public class StockOutBillManagerDetailUIModel : BaseNotificationUIModel
    {
        #region 私有变量
        /// <summary>
        /// 选择
        /// </summary>
        private bool _ischecked;
        /// <summary>
        /// 配件名称
        /// </summary>
        private String _sobd_name;
        /// <summary>
        /// 原厂编码
        /// </summary>
        private String _sobd_oemno;
        /// <summary>
        /// 第三方编码
        /// </summary>
        private String _sobd_thirdno;
        /// <summary>
        /// 规格型号
        /// </summary>
        private String _sobd_specification;
        /// <summary>
        /// 单位
        /// </summary>
        private String _sobd_uom;
        /// <summary>
        /// 进货单价
        /// </summary>
        private Decimal? _sobd_unitcostprice;
        /// <summary>
        /// 销售单价
        /// </summary>
        private Decimal? _sobd_unitsaleprice;
        /// <summary>
        /// 出库数量
        /// </summary>
        private Decimal? _sobd_qty;
        /// <summary>
        /// 金额
        /// </summary>
        private Decimal? _sobd_amount;
        /// <summary>
        /// 仓库名称
        /// </summary>
        private String _wh_name;
        /// <summary>
        /// 仓位名称
        /// </summary>
        private String _whb_name;
        /// <summary>
        /// 配件条码
        /// </summary>
        private String _sobd_barcode;
        /// <summary>
        /// 配件批次号
        /// </summary>
        private String _sobd_batchno;
        /// <summary>
        /// 有效
        /// </summary>
        private Boolean? _sobd_isvalid;
        /// <summary>
        /// 创建人
        /// </summary>
        private String _sobd_createdby;
        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime? _sobd_createdtime;
        /// <summary>
        /// 修改人
        /// </summary>
        private String _sobd_updatedby;
        /// <summary>
        /// 修改时间
        /// </summary>
        private DateTime? _sobd_updatedtime;
        /// <summary>
        /// 出库单明细ID
        /// </summary>
        private String _sobd_id;
        /// <summary>
        /// 出库单ID
        /// </summary>
        private String _sobd_sob_id;
        /// <summary>
        /// 出库单号
        /// </summary>
        private String _sobd_sob_no;
        /// <summary>
        /// 来源单明细ID
        /// </summary>
        private String _sobd_sourcedetailid;
        /// <summary>
        /// 仓库ID
        /// </summary>
        private String _sobd_wh_id;
        /// <summary>
        /// 仓位ID
        /// </summary>
        private String _sobd_whb_id;
        /// <summary>
        /// 版本号
        /// </summary>
        private Int64? _sobd_versionno;
        #endregion

        #region 公共属性
        /// <summary>
        /// 选择
        /// </summary>
        public bool IsChecked
        {
            get { return _ischecked; }
            set
            {
                _ischecked = value;
                RaisePropertyChanged(() => IsChecked);
            }
        }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String SOBD_Name
        {
            get { return _sobd_name; }
            set
            {
                _sobd_name = value;
                RaisePropertyChanged(() => SOBD_Name);
            }
        }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String SOBD_OEMNo
        {
            get { return _sobd_oemno; }
            set
            {
                _sobd_oemno = value;
                RaisePropertyChanged(() => SOBD_OEMNo);
            }
        }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String SOBD_ThirdNo
        {
            get { return _sobd_thirdno; }
            set
            {
                _sobd_thirdno = value;
                RaisePropertyChanged(() => SOBD_ThirdNo);
            }
        }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String SOBD_Specification
        {
            get { return _sobd_specification; }
            set
            {
                _sobd_specification = value;
                RaisePropertyChanged(() => SOBD_Specification);
            }
        }
        /// <summary>
        /// 单位
        /// </summary>
        public String SOBD_UOM
        {
            get { return _sobd_uom; }
            set
            {
                _sobd_uom = value;
                RaisePropertyChanged(() => SOBD_UOM);
            }
        }
        /// <summary>
        /// 进货单价
        /// </summary>
        public Decimal? SOBD_UnitCostPrice
        {
            get { return _sobd_unitcostprice; }
            set
            {
                _sobd_unitcostprice = value;
                RaisePropertyChanged(() => SOBD_UnitCostPrice);
            }
        }
        /// <summary>
        /// 销售单价
        /// </summary>
        public Decimal? SOBD_UnitSalePrice
        {
            get { return _sobd_unitsaleprice; }
            set
            {
                _sobd_unitsaleprice = value;
                RaisePropertyChanged(() => SOBD_UnitSalePrice);
            }
        }
        /// <summary>
        /// 出库数量
        /// </summary>
        public Decimal? SOBD_Qty
        {
            get { return _sobd_qty; }
            set
            {
                _sobd_qty = value;
                RaisePropertyChanged(() => SOBD_Qty);
            }
        }
        /// <summary>
        /// 金额
        /// </summary>
        public Decimal? SOBD_Amount
        {
            get { return _sobd_amount; }
            set
            {
                _sobd_amount = value;
                RaisePropertyChanged(() => SOBD_Amount);
            }
        }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public String WH_Name
        {
            get { return _wh_name; }
            set
            {
                _wh_name = value;
                RaisePropertyChanged(() => WH_Name);
            }
        }
        /// <summary>
        /// 仓位名称
        /// </summary>
        public String WHB_Name
        {
            get { return _whb_name; }
            set
            {
                _whb_name = value;
                RaisePropertyChanged(() => WHB_Name);
            }
        }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String SOBD_Barcode
        {
            get { return _sobd_barcode; }
            set
            {
                _sobd_barcode = value;
                RaisePropertyChanged(() => SOBD_Barcode);
            }
        }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String SOBD_BatchNo
        {
            get { return _sobd_batchno; }
            set
            {
                _sobd_batchno = value;
                RaisePropertyChanged(() => SOBD_BatchNo);
            }
        }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SOBD_IsValid
        {
            get { return _sobd_isvalid; }
            set
            {
                _sobd_isvalid = value;
                RaisePropertyChanged(() => SOBD_IsValid);
            }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SOBD_CreatedBy
        {
            get { return _sobd_createdby; }
            set
            {
                _sobd_createdby = value;
                RaisePropertyChanged(() => SOBD_CreatedBy);
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SOBD_CreatedTime
        {
            get { return _sobd_createdtime; }
            set
            {
                _sobd_createdtime = value;
                RaisePropertyChanged(() => SOBD_CreatedTime);
            }
        }
        /// <summary>
        /// 修改人
        /// </summary>
        public String SOBD_UpdatedBy
        {
            get { return _sobd_updatedby; }
            set
            {
                _sobd_updatedby = value;
                RaisePropertyChanged(() => SOBD_UpdatedBy);
            }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SOBD_UpdatedTime
        {
            get { return _sobd_updatedtime; }
            set
            {
                _sobd_updatedtime = value;
                RaisePropertyChanged(() => SOBD_UpdatedTime);
            }
        }
        /// <summary>
        /// 出库单明细ID
        /// </summary>
        public String SOBD_ID
        {
            get { return _sobd_id; }
            set
            {
                _sobd_id = value;
                RaisePropertyChanged(() => SOBD_ID);
            }
        }
        /// <summary>
        /// 出库单ID
        /// </summary>
        public String SOBD_SOB_ID
        {
            get { return _sobd_sob_id; }
            set
            {
                _sobd_sob_id = value;
                RaisePropertyChanged(() => SOBD_SOB_ID);
            }
        }
        /// <summary>
        /// 出库单号
        /// </summary>
        public String SOBD_SOB_No
        {
            get { return _sobd_sob_no; }
            set
            {
                _sobd_sob_no = value;
                RaisePropertyChanged(() => SOBD_SOB_No);
            }
        }
        /// <summary>
        /// 来源单明细ID
        /// </summary>
        public String SOBD_SourceDetailID
        {
            get { return _sobd_sourcedetailid; }
            set
            {
                _sobd_sourcedetailid = value;
                RaisePropertyChanged(() => SOBD_SourceDetailID);
            }
        }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String SOBD_WH_ID
        {
            get { return _sobd_wh_id; }
            set
            {
                _sobd_wh_id = value;
                RaisePropertyChanged(() => SOBD_WH_ID);
            }
        }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String SOBD_WHB_ID
        {
            get { return _sobd_whb_id; }
            set
            {
                _sobd_whb_id = value;
                RaisePropertyChanged(() => SOBD_WHB_ID);
            }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? SOBD_VersionNo
        {
            get { return _sobd_versionno; }
            set
            {
                _sobd_versionno = value;
                RaisePropertyChanged(() => SOBD_VersionNo);
            }
        }
        /// <summary>
        /// 当前页面索引/要跳转的页码（翻页用）
        /// </summary>
        public Int32? PageIndex { get; set; }
        /// <summary>
        /// 页面大小（翻页用）
        /// </summary>
        public Int32? PageSize { get; set; }
        
        /// <summary>
        /// 出库单明细ID
        /// </summary>
        public String WHERE_SOBD_ID { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_SOBD_VersionNo { get; set; }
        #endregion

        #region 其他属性

        private String _tmpSobdId;
        /// <summary>
        /// 出库单明细临时ID
        /// </summary>
        public String Tmp_SOBD_ID
        {
            get { return _tmpSobdId; }
            set
            {
                _tmpSobdId = value;
                RaisePropertyChanged(() => Tmp_SOBD_ID);
            }
        }
        /// <summary>
        /// 库存ID
        /// </summary>
        public String INV_ID { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        public Decimal? INV_Qty { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String INV_SUPP_ID { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public String SUPP_Name { get; set; }

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
        
        #endregion
    }
}
