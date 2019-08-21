using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 调拨管理明细UIModel
    /// </summary>
    public class TransferBillManagerDetailUIModel : BaseNotificationUIModel
    {
        #region 私有变量
        /// <summary>
        /// 选择
        /// </summary>
        private bool _ischecked;
        /// <summary>
        /// 配件名称
        /// </summary>
        private String _tbd_name;
        /// <summary>
        /// 单位
        /// </summary>
        private String _tbd_uom;
        /// <summary>
        /// 配件条码
        /// </summary>
        private String _tbd_barcode;
        /// <summary>
        /// 调出配件批次号
        /// </summary>
        private String _tbd_transoutbatchno;
        /// <summary>
        /// 调入配件批次号
        /// </summary>
        private String _tbd_transinbatchno;
        /// <summary>
        /// 数量
        /// </summary>
        private Decimal? _tbd_qty;
        /// <summary>
        /// 源库存单价
        /// </summary>
        private Decimal? _tbd_sourunitprice;
        /// <summary>
        /// 入库单价
        /// </summary>
        private Decimal? _tbd_destunitprice;
        /// <summary>
        /// 原厂编码
        /// </summary>
        private String _tbd_oemno;
        /// <summary>
        /// 第三方编码
        /// </summary>
        private String _tbd_thirdno;
        /// <summary>
        /// 配件品牌
        /// </summary>
        private String _apa_brand;
        /// <summary>
        /// 配件规格型号
        /// </summary>
        private String _tbd_specification;
        /// <summary>
        /// 已结算标志
        /// </summary>
        private String _tbd_issettled;
        /// <summary>
        /// 有效
        /// </summary>
        private Boolean? _tbd_isvalid;
        /// <summary>
        /// 创建人
        /// </summary>
        private String _tbd_createdby;
        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime? _tbd_createdtime;
        /// <summary>
        /// 修改人
        /// </summary>
        private String _tbd_updatedby;
        /// <summary>
        /// 修改时间
        /// </summary>
        private DateTime? _tbd_updatedtime;
        /// <summary>
        /// 名称
        /// </summary>
        private String _supp_name;
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
        /// 变速类型名称
        /// </summary>
        private String _apa_vehiclegearboxtypename;
        /// <summary>
        /// 变速类型编码
        /// </summary>
        private String _apa_vehiclegearboxtypecode;
        /// <summary>
        /// 调拨单明细ID
        /// </summary>
        private String _tbd_id;
        /// <summary>
        /// 调拨单ID
        /// </summary>
        private String _tbd_tb_id;
        /// <summary>
        /// 调拨单号
        /// </summary>
        private String _tbd_tb_no;
        /// <summary>
        /// 调出仓库ID
        /// </summary>
        private String _tbd_transoutwhid;
        /// <summary>
        /// 调出仓位ID
        /// </summary>
        private String _tbd_transoutbinid;
        /// <summary>
        /// 调入仓库ID
        /// </summary>
        private String _tbd_transinwhid;
        /// <summary>
        /// 调入仓位ID
        /// </summary>
        private String _tbd_transinbinid;
        /// <summary>
        /// 供应商ID
        /// </summary>
        private String _tbd_supp_id;
        /// <summary>
        /// 版本号
        /// </summary>
        private Int64? _tbd_versionno;
        /// <summary>
        /// 事务编号
        /// </summary>
        private String _tbd_transid;
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
        public String TBD_Name
        {
            get { return _tbd_name; }
            set
            {
                _tbd_name = value;
                RaisePropertyChanged(() => TBD_Name);
            }
        }
        /// <summary>
        /// 单位
        /// </summary>
        public String TBD_UOM
        {
            get { return _tbd_uom; }
            set
            {
                _tbd_uom = value;
                RaisePropertyChanged(() => TBD_UOM);
            }
        }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String TBD_Barcode
        {
            get { return _tbd_barcode; }
            set
            {
                _tbd_barcode = value;
                RaisePropertyChanged(() => TBD_Barcode);
            }
        }
        /// <summary>
        /// 调出配件批次号
        /// </summary>
        public String TBD_TransOutBatchNo
        {
            get { return _tbd_transoutbatchno; }
            set
            {
                _tbd_transoutbatchno = value;
                RaisePropertyChanged(() => TBD_TransOutBatchNo);
            }
        }
        /// <summary>
        /// 调入配件批次号
        /// </summary>
        public String TBD_TransInBatchNo
        {
            get { return _tbd_transinbatchno; }
            set
            {
                _tbd_transinbatchno = value;
                RaisePropertyChanged(() => TBD_TransInBatchNo);
            }
        }
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? TBD_Qty
        {
            get { return _tbd_qty; }
            set
            {
                _tbd_qty = value;
                RaisePropertyChanged(() => TBD_Qty);
            }
        }
        /// <summary>
        /// 源库存单价
        /// </summary>
        public Decimal? TBD_SourUnitPrice
        {
            get { return _tbd_sourunitprice; }
            set
            {
                _tbd_sourunitprice = value;
                RaisePropertyChanged(() => TBD_SourUnitPrice);
            }
        }
        /// <summary>
        /// 入库单价
        /// </summary>
        public Decimal? TBD_DestUnitPrice
        {
            get { return _tbd_destunitprice; }
            set
            {
                _tbd_destunitprice = value;
                RaisePropertyChanged(() => TBD_DestUnitPrice);
            }
        }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String TBD_OEMNo
        {
            get { return _tbd_oemno; }
            set
            {
                _tbd_oemno = value;
                RaisePropertyChanged(() => TBD_OEMNo);
            }
        }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String TBD_ThirdNo
        {
            get { return _tbd_thirdno; }
            set
            {
                _tbd_thirdno = value;
                RaisePropertyChanged(() => TBD_ThirdNo);
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
        /// 配件规格型号
        /// </summary>
        public String TBD_Specification
        {
            get { return _tbd_specification; }
            set
            {
                _tbd_specification = value;
                RaisePropertyChanged(() => TBD_Specification);
            }
        }
        /// <summary>
        /// 已结算标志
        /// </summary>
        public String TBD_IsSettled
        {
            get { return _tbd_issettled; }
            set
            {
                _tbd_issettled = value;
                RaisePropertyChanged(() => TBD_IsSettled);
            }
        }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? TBD_IsValid
        {
            get { return _tbd_isvalid; }
            set
            {
                _tbd_isvalid = value;
                RaisePropertyChanged(() => TBD_IsValid);
            }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public String TBD_CreatedBy
        {
            get { return _tbd_createdby; }
            set
            {
                _tbd_createdby = value;
                RaisePropertyChanged(() => TBD_CreatedBy);
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? TBD_CreatedTime
        {
            get { return _tbd_createdtime; }
            set
            {
                _tbd_createdtime = value;
                RaisePropertyChanged(() => TBD_CreatedTime);
            }
        }
        /// <summary>
        /// 修改人
        /// </summary>
        public String TBD_UpdatedBy
        {
            get { return _tbd_updatedby; }
            set
            {
                _tbd_updatedby = value;
                RaisePropertyChanged(() => TBD_UpdatedBy);
            }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? TBD_UpdatedTime
        {
            get { return _tbd_updatedtime; }
            set
            {
                _tbd_updatedtime = value;
                RaisePropertyChanged(() => TBD_UpdatedTime);
            }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public String SUPP_Name
        {
            get { return _supp_name; }
            set
            {
                _supp_name = value;
                RaisePropertyChanged(() => SUPP_Name);
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
        /// 调拨单明细ID
        /// </summary>
        public String TBD_ID
        {
            get { return _tbd_id; }
            set
            {
                _tbd_id = value;
                RaisePropertyChanged(() => TBD_ID);
            }
        }
        /// <summary>
        /// 调拨单ID
        /// </summary>
        public String TBD_TB_ID
        {
            get { return _tbd_tb_id; }
            set
            {
                _tbd_tb_id = value;
                RaisePropertyChanged(() => TBD_TB_ID);
            }
        }
        /// <summary>
        /// 调拨单号
        /// </summary>
        public String TBD_TB_No
        {
            get { return _tbd_tb_no; }
            set
            {
                _tbd_tb_no = value;
                RaisePropertyChanged(() => TBD_TB_No);
            }
        }
        /// <summary>
        /// 调出仓库ID
        /// </summary>
        public String TBD_TransOutWhId
        {
            get { return _tbd_transoutwhid; }
            set
            {
                _tbd_transoutwhid = value;
                RaisePropertyChanged(() => TBD_TransOutWhId);
            }
        }
        /// <summary>
        /// 调出仓位ID
        /// </summary>
        public String TBD_TransOutBinId
        {
            get { return _tbd_transoutbinid; }
            set
            {
                _tbd_transoutbinid = value;
                RaisePropertyChanged(() => TBD_TransOutBinId);
            }
        }
        /// <summary>
        /// 调入仓库ID
        /// </summary>
        public String TBD_TransInWhId
        {
            get { return _tbd_transinwhid; }
            set
            {
                _tbd_transinwhid = value;
                RaisePropertyChanged(() => TBD_TransInWhId);
            }
        }
        /// <summary>
        /// 调入仓位ID
        /// </summary>
        public String TBD_TransInBinId
        {
            get { return _tbd_transinbinid; }
            set
            {
                _tbd_transinbinid = value;
                RaisePropertyChanged(() => TBD_TransInBinId);
            }
        }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String TBD_SUPP_ID
        {
            get { return _tbd_supp_id; }
            set
            {
                _tbd_supp_id = value;
                RaisePropertyChanged(() => TBD_SUPP_ID);
            }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? TBD_VersionNo
        {
            get { return _tbd_versionno; }
            set
            {
                _tbd_versionno = value;
                RaisePropertyChanged(() => TBD_VersionNo);
            }
        }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String TBD_TransID
        {
            get { return _tbd_transid; }
            set
            {
                _tbd_transid = value;
                RaisePropertyChanged(() => TBD_TransID);
            }
        }
        #endregion

        #region 其他属性

        /// <summary>
        /// 金额
        /// </summary>
        public decimal DetailDestAmount { get; set; }

        private String _tmpTbdId;
        /// <summary>
        /// 调拨单明细临时ID
        /// </summary>
        public String Tmp_TBD_ID
        {
            get { return _tmpTbdId; }
            set
            {
                _tmpTbdId = value;
                RaisePropertyChanged(() => Tmp_TBD_ID);
            }
        }
        private String _transOutwhname;
        /// <summary>
        /// 调出仓库名称
        /// </summary>
        public String TransOutWhName
        {
            get { return _transOutwhname; }
            set
            {
                _transOutwhname = value;
                RaisePropertyChanged(() => TransOutWhName);
            }
        }
        private String _transOutWhbName;
        /// <summary>
        /// 调出仓位名称
        /// </summary>
        public String TransOutWhbName
        {
            get { return _transOutWhbName; }
            set
            {
                _transOutWhbName = value;
                RaisePropertyChanged(() => TransOutWhbName);
            }
        }
        private String _transInwhname;
        /// <summary>
        /// 调入仓库名称
        /// </summary>
        public String TransInWhName
        {
            get { return _transInwhname; }
            set
            {
                _transInwhname = value;
                RaisePropertyChanged(() => TransInWhName);
            }
        }
        private String _transInWhbName;
        /// <summary>
        /// 调入仓位名称
        /// </summary>
        public String TransInWhbName
        {
            get { return _transInWhbName; }
            set
            {
                _transInWhbName = value;
                RaisePropertyChanged(() => TransInWhbName);
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
        /// 调拨单明细ID
        /// </summary>
        public String WHERE_TBD_ID { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_TBD_VersionNo { get; set; }
        #endregion
    }
}
