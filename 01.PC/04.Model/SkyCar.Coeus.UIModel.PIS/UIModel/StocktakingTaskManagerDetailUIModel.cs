using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 盘点管理明细UIModel
    /// </summary>
    public class StocktakingTaskManagerDetailUIModel : BaseNotificationUIModel
    {
        #region 私有变量
        /// <summary>
        /// 选择
        /// </summary>
        private Boolean _ischecked;
        /// <summary>
        /// 盘点任务明细ID
        /// </summary>
        private String _std_id;
        /// <summary>
        /// 盘点任务ID
        /// </summary>
        private String _std_tb_id;
        /// <summary>
        /// 盘点单号
        /// </summary>
        private String _std_tb_no;
        /// <summary>
        /// 仓库ID
        /// </summary>
        private String _std_wh_id;
        /// <summary>
        /// 仓位ID
        /// </summary>
        private String _std_whb_id;
        /// <summary>
        /// 应有量
        /// </summary>
        private Decimal? _std_dueqty;
        /// <summary>
        /// 实际量
        /// </summary>
        private Decimal? _std_actualqty;
        /// <summary>
        /// 差异数量
        /// </summary>
        private Decimal? _std_adjustqty;
        /// <summary>
        /// 允差数量
        /// </summary>
        private Decimal? _std_apprdiffqty;
        /// <summary>
        /// 数量允差比
        /// </summary>
        private Decimal? _std_apprdiffqtyrate;
        /// <summary>
        /// 调整数量
        /// </summary>
        private Decimal? _std_snapshotqty;
        /// <summary>
        /// 应有金额
        /// </summary>
        private Decimal? _std_dueamount;
        /// <summary>
        /// 实际金额
        /// </summary>
        private Decimal? _std_actualamount;
        /// <summary>
        /// 金额损失率
        /// </summary>
        private Decimal? _std_amountlossratio;
        /// <summary>
        /// 配件条码
        /// </summary>
        private String _std_barcode;
        /// <summary>
        /// 配件批次号
        /// </summary>
        private String _std_batchno;
        /// <summary>
        /// 第三方编码
        /// </summary>
        private String _std_thirdno;
        /// <summary>
        /// 原厂编码
        /// </summary>
        private String _std_oemno;
        /// <summary>
        /// 配件名称
        /// </summary>
        private String _std_name;
        /// <summary>
        /// 配件规格型号
        /// </summary>
        private String _std_specification;
        /// <summary>
        /// 单位
        /// </summary>
        private String _std_uom;
        /// <summary>
        /// 有效
        /// </summary>
        private Boolean? _std_isvalid;
        /// <summary>
        /// 创建人
        /// </summary>
        private String _std_createdby;
        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime? _std_createdtime;
        /// <summary>
        /// 修改人
        /// </summary>
        private String _std_updatedby;
        /// <summary>
        /// 修改时间
        /// </summary>
        private DateTime? _std_updatedtime;
        /// <summary>
        /// 版本号
        /// </summary>
        private Int64? _std_versionno;
        /// <summary>
        /// 配件品牌
        /// </summary>
        private String _apa_brand;
        /// <summary>
        /// 配件级别
        /// </summary>
        private String _apa_level;
        /// <summary>
        /// 汽车品牌
        /// </summary>
        private String _apa_vehiclebrand;
        /// <summary>
        /// 车系
        /// </summary>
        private String _apa_vehicleinspire;
        /// <summary>
        /// 排量
        /// </summary>
        private String _apa_vehiclecapacity;
        /// <summary>
        /// 年款
        /// </summary>
        private String _apa_vehicleyearmodel;
        /// <summary>
        /// 变速类型编码
        /// </summary>
        private String _apa_vehiclegearboxtypecode;
        /// <summary>
        /// 变速类型名称
        /// </summary>
        private String _apa_vehiclegearboxtypename;
        /// <summary>
        /// 数量
        /// </summary>
        private Decimal? _inv_qty;
        /// <summary>
        /// 采购单价
        /// </summary>
        private Decimal? _inv_purchaseunitprice;
        /// <summary>
        /// 仓库名称
        /// </summary>
        private String _wh_name;
        /// <summary>
        /// 仓位名称
        /// </summary>
        private String _whb_name;
        #endregion

        #region 公共属性
        /// <summary>
        /// 选择
        /// </summary>
        public Boolean IsChecked
        {
            get { return _ischecked; }
            set
            {
                _ischecked = value;
                RaisePropertyChanged(() => IsChecked);
            }
        }
        /// <summary>
        /// 盘点任务明细ID
        /// </summary>
        public String STD_ID
        {
            get { return _std_id; }
            set
            {
                _std_id = value;
                RaisePropertyChanged(() => STD_ID);
            }
        }
        /// <summary>
        /// 盘点任务ID
        /// </summary>
        public String STD_TB_ID
        {
            get { return _std_tb_id; }
            set
            {
                _std_tb_id = value;
                RaisePropertyChanged(() => STD_TB_ID);
            }
        }
        /// <summary>
        /// 盘点单号
        /// </summary>
        public String STD_TB_No
        {
            get { return _std_tb_no; }
            set
            {
                _std_tb_no = value;
                RaisePropertyChanged(() => STD_TB_No);
            }
        }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String STD_WH_ID
        {
            get { return _std_wh_id; }
            set
            {
                _std_wh_id = value;
                RaisePropertyChanged(() => STD_WH_ID);
            }
        }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String STD_WHB_ID
        {
            get { return _std_whb_id; }
            set
            {
                _std_whb_id = value;
                RaisePropertyChanged(() => STD_WHB_ID);
            }
        }
        /// <summary>
        /// 应有量
        /// </summary>
        public Decimal? STD_DueQty
        {
            get { return _std_dueqty; }
            set
            {
                _std_dueqty = value;
                RaisePropertyChanged(() => STD_DueQty);
            }
        }
        /// <summary>
        /// 实际量
        /// </summary>
        public Decimal? STD_ActualQty
        {
            get { return _std_actualqty; }
            set
            {
                _std_actualqty = value;
                RaisePropertyChanged(() => STD_ActualQty);
            }
        }
        /// <summary>
        /// 差异数量
        /// </summary>
        public Decimal? STD_AdjustQty
        {
            get { return _std_adjustqty; }
            set
            {
                _std_adjustqty = value;
                RaisePropertyChanged(() => STD_AdjustQty);
            }
        }
        /// <summary>
        /// 允差数量
        /// </summary>
        public Decimal? STD_ApprDiffQty
        {
            get { return _std_apprdiffqty; }
            set
            {
                _std_apprdiffqty = value;
                RaisePropertyChanged(() => STD_ApprDiffQty);
            }
        }
        /// <summary>
        /// 数量允差比
        /// </summary>
        public Decimal? STD_ApprDiffQtyRate
        {
            get { return _std_apprdiffqtyrate; }
            set
            {
                _std_apprdiffqtyrate = value;
                RaisePropertyChanged(() => STD_ApprDiffQtyRate);
            }
        }
        /// <summary>
        /// 调整数量
        /// </summary>
        public Decimal? STD_SnapshotQty
        {
            get { return _std_snapshotqty; }
            set
            {
                _std_snapshotqty = value;
                RaisePropertyChanged(() => STD_SnapshotQty);
            }
        }
        /// <summary>
        /// 应有金额
        /// </summary>
        public Decimal? STD_DueAmount
        {
            get { return _std_dueamount; }
            set
            {
                _std_dueamount = value;
                RaisePropertyChanged(() => STD_DueAmount);
            }
        }
        /// <summary>
        /// 实际金额
        /// </summary>
        public Decimal? STD_ActualAmount
        {
            get { return _std_actualamount; }
            set
            {
                _std_actualamount = value;
                RaisePropertyChanged(() => STD_ActualAmount);
            }
        }
        /// <summary>
        /// 金额损失率
        /// </summary>
        public Decimal? STD_AmountLossRatio
        {
            get { return _std_amountlossratio; }
            set
            {
                _std_amountlossratio = value;
                RaisePropertyChanged(() => STD_AmountLossRatio);
            }
        }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String STD_Barcode
        {
            get { return _std_barcode; }
            set
            {
                _std_barcode = value;
                RaisePropertyChanged(() => STD_Barcode);
            }
        }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String STD_BatchNo
        {
            get { return _std_batchno; }
            set
            {
                _std_batchno = value;
                RaisePropertyChanged(() => STD_BatchNo);
            }
        }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String STD_ThirdNo
        {
            get { return _std_thirdno; }
            set
            {
                _std_thirdno = value;
                RaisePropertyChanged(() => STD_ThirdNo);
            }
        }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String STD_OEMNo
        {
            get { return _std_oemno; }
            set
            {
                _std_oemno = value;
                RaisePropertyChanged(() => STD_OEMNo);
            }
        }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String STD_Name
        {
            get { return _std_name; }
            set
            {
                _std_name = value;
                RaisePropertyChanged(() => STD_Name);
            }
        }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String STD_Specification
        {
            get { return _std_specification; }
            set
            {
                _std_specification = value;
                RaisePropertyChanged(() => STD_Specification);
            }
        }
        /// <summary>
        /// 单位
        /// </summary>
        public String STD_UOM
        {
            get { return _std_uom; }
            set
            {
                _std_uom = value;
                RaisePropertyChanged(() => STD_UOM);
            }
        }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? STD_IsValid
        {
            get { return _std_isvalid; }
            set
            {
                _std_isvalid = value;
                RaisePropertyChanged(() => STD_IsValid);
            }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public String STD_CreatedBy
        {
            get { return _std_createdby; }
            set
            {
                _std_createdby = value;
                RaisePropertyChanged(() => STD_CreatedBy);
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? STD_CreatedTime
        {
            get { return _std_createdtime; }
            set
            {
                _std_createdtime = value;
                RaisePropertyChanged(() => STD_CreatedTime);
            }
        }
        /// <summary>
        /// 修改人
        /// </summary>
        public String STD_UpdatedBy
        {
            get { return _std_updatedby; }
            set
            {
                _std_updatedby = value;
                RaisePropertyChanged(() => STD_UpdatedBy);
            }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? STD_UpdatedTime
        {
            get { return _std_updatedtime; }
            set
            {
                _std_updatedtime = value;
                RaisePropertyChanged(() => STD_UpdatedTime);
            }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? STD_VersionNo
        {
            get { return _std_versionno; }
            set
            {
                _std_versionno = value;
                RaisePropertyChanged(() => STD_VersionNo);
            }
        }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String APA_Brand
        {
            get { return _apa_brand; }
            set
            {
                _apa_brand = value;
                RaisePropertyChanged(() => APA_Brand);
            }
        }
        /// <summary>
        /// 配件级别
        /// </summary>
        public String APA_Level
        {
            get { return _apa_level; }
            set
            {
                _apa_level = value;
                RaisePropertyChanged(() => APA_Level);
            }
        }
        /// <summary>
        /// 汽车品牌
        /// </summary>
        public String APA_VehicleBrand
        {
            get { return _apa_vehiclebrand; }
            set
            {
                _apa_vehiclebrand = value;
                RaisePropertyChanged(() => APA_VehicleBrand);
            }
        }
        /// <summary>
        /// 车系
        /// </summary>
        public String APA_VehicleInspire
        {
            get { return _apa_vehicleinspire; }
            set
            {
                _apa_vehicleinspire = value;
                RaisePropertyChanged(() => APA_VehicleInspire);
            }
        }
        /// <summary>
        /// 排量
        /// </summary>
        public String APA_VehicleCapacity
        {
            get { return _apa_vehiclecapacity; }
            set
            {
                _apa_vehiclecapacity = value;
                RaisePropertyChanged(() => APA_VehicleCapacity);
            }
        }
        /// <summary>
        /// 年款
        /// </summary>
        public String APA_VehicleYearModel
        {
            get { return _apa_vehicleyearmodel; }
            set
            {
                _apa_vehicleyearmodel = value;
                RaisePropertyChanged(() => APA_VehicleYearModel);
            }
        }
        /// <summary>
        /// 变速类型编码
        /// </summary>
        public String APA_VehicleGearboxTypeCode
        {
            get { return _apa_vehiclegearboxtypecode; }
            set
            {
                _apa_vehiclegearboxtypecode = value;
                RaisePropertyChanged(() => APA_VehicleGearboxTypeCode);
            }
        }
        /// <summary>
        /// 变速类型名称
        /// </summary>
        public String APA_VehicleGearboxTypeName
        {
            get { return _apa_vehiclegearboxtypename; }
            set
            {
                _apa_vehiclegearboxtypename = value;
                RaisePropertyChanged(() => APA_VehicleGearboxTypeName);
            }
        }
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? INV_Qty
        {
            get { return _inv_qty; }
            set
            {
                _inv_qty = value;
                RaisePropertyChanged(() => INV_Qty);
            }
        }
        /// <summary>
        /// 采购单价
        /// </summary>
        public Decimal? INV_PurchaseUnitPrice
        {
            get { return _inv_purchaseunitprice; }
            set
            {
                _inv_purchaseunitprice = value;
                RaisePropertyChanged(() => INV_PurchaseUnitPrice);
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

        #endregion

        /// <summary>
        /// 行标识
        /// </summary>
        public string RowID { get; set; }
        /// <summary>
        /// 库存ID
        /// </summary>
        public String INV_ID { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? INV_VersionNo { get; set; }
    }
}
