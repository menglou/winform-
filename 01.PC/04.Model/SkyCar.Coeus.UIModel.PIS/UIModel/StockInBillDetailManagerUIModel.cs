using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 入库管理明细UIModel
    /// </summary>
    public class StockInBillDetailManagerUIModel : BaseNotificationUIModel
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

        private String _sidId;
        /// <summary>
        /// 入库单明细ID
        /// </summary>
        public String SID_ID
        {
            get { return _sidId; }
            set
            {
                _sidId = value;
                RaisePropertyChanged(() => SID_ID);
            }
        }
        private String _sidSibId;
        /// <summary>
        /// 入库单ID
        /// </summary>
        public String SID_SIB_ID
        {
            get { return _sidSibId; }
            set
            {
                _sidSibId = value;
                RaisePropertyChanged(() => SID_SIB_ID);
            }
        }
        private String _sidSibNo;
        /// <summary>
        /// 入库单号
        /// </summary>
        public String SID_SIB_No
        {
            get { return _sidSibNo; }
            set
            {
                _sidSibNo = value;
                RaisePropertyChanged(() => SID_SIB_No);
            }
        }
        private String _sidSourceDetailId;
        /// <summary>
        /// 来源单明细ID
        /// </summary>
        public String SID_SourceDetailID
        {
            get { return _sidSourceDetailId; }
            set
            {
                _sidSourceDetailId = value;
                RaisePropertyChanged(() => SID_SourceDetailID);
            }
        }
        private String _sidBarcode;
        /// <summary>
        /// 配件条码
        /// </summary>
        public String SID_Barcode
        {
            get { return _sidBarcode; }
            set
            {
                _sidBarcode = value;
                RaisePropertyChanged(() => SID_Barcode);
            }
        }
        private String _sidBatchNo;
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String SID_BatchNo
        {
            get { return _sidBatchNo; }
            set
            {
                _sidBatchNo = value;
                RaisePropertyChanged(() => SID_BatchNo);
            }
        }
        private String _sidThirdNo;
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String SID_ThirdNo
        {
            get { return _sidThirdNo; }
            set
            {
                _sidThirdNo = value;
                RaisePropertyChanged(() => SID_ThirdNo);
            }
        }
        private String _sidOemNo;
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String SID_OEMNo
        {
            get { return _sidOemNo; }
            set
            {
                _sidOemNo = value;
                RaisePropertyChanged(() => SID_OEMNo);
            }
        }
        private String _sidName;
        /// <summary>
        /// 配件名称
        /// </summary>
        public String SID_Name
        {
            get { return _sidName; }
            set
            {
                _sidName = value;
                RaisePropertyChanged(() => SID_Name);
            }
        }
        private String _sidSpecification;
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String SID_Specification
        {
            get { return _sidSpecification; }
            set
            {
                _sidSpecification = value;
                RaisePropertyChanged(() => SID_Specification);
            }
        }
        private String _sidSuppId;
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String SID_SUPP_ID
        {
            get { return _sidSuppId; }
            set
            {
                _sidSuppId = value;
                RaisePropertyChanged(() => SID_SUPP_ID);
            }
        }
        private String _sidWhId;
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String SID_WH_ID
        {
            get { return _sidWhId; }
            set
            {
                _sidWhId = value;
                RaisePropertyChanged(() => SID_WH_ID);
            }
        }

        private String _sidWhbId;
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String SID_WHB_ID
        {
            get { return _sidWhbId; }
            set
            {
                _sidWhbId = value;
                RaisePropertyChanged(() => SID_WHB_ID);
            }
        }

        private Decimal? _sidQty;
        /// <summary>
        /// 入库数量
        /// </summary>
        public Decimal? SID_Qty
        {
            get { return _sidQty; }
            set
            {
                _sidQty = value;
                RaisePropertyChanged(() => SID_Qty);
            }
        }
        private String _sidUom;
        /// <summary>
        /// 单位
        /// </summary>
        public String SID_UOM
        {
            get { return _sidUom; }
            set
            {
                _sidUom = value;
                RaisePropertyChanged(() => SID_UOM);
            }
        }

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

        private Decimal? _sidAmount;
        /// <summary>
        /// 入库金额
        /// </summary>
        public Decimal? SID_Amount
        {
            get { return _sidAmount; }
            set
            {
                _sidAmount = value;
                RaisePropertyChanged(() => SID_Amount);
            }
        }
        private Boolean? _sidIsSettled;
        /// <summary>
        /// 已结算标志
        /// </summary>
        public Boolean? SID_IsSettled
        {
            get { return _sidIsSettled; }
            set
            {
                _sidIsSettled = value;
                RaisePropertyChanged(() => SID_IsSettled);
            }
        }
        private Boolean? _sidIsValid;
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SID_IsValid
        {
            get { return _sidIsValid; }
            set
            {
                _sidIsValid = value;
                RaisePropertyChanged(() => SID_IsValid);
            }
        }
        private String _sidCreatedBy;
        /// <summary>
        /// 创建人
        /// </summary>
        public String SID_CreatedBy
        {
            get { return _sidCreatedBy; }
            set
            {
                _sidCreatedBy = value;
                RaisePropertyChanged(() => SID_CreatedBy);
            }
        }
        private DateTime? _SID_CreatedTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SID_CreatedTime
        {
            get { return _SID_CreatedTime; }
            set
            {
                _SID_CreatedTime = value;
                RaisePropertyChanged(() => SID_CreatedTime);
            }
        }
        private String _sidUpdatedBy;
        /// <summary>
        /// 修改人
        /// </summary>
        public String SID_UpdatedBy
        {
            get { return _sidUpdatedBy; }
            set
            {
                _sidUpdatedBy = value;
                RaisePropertyChanged(() => SID_UpdatedBy);
            }
        }
        private DateTime? _SID_UpdatedTime;
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SID_UpdatedTime
        {
            get { return _SID_UpdatedTime; }
            set
            {
                _SID_UpdatedTime = value;
                RaisePropertyChanged(() => SID_UpdatedTime);
            }
        }
        private Int64? _sidVersionNo;
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? SID_VersionNo
        {
            get { return _sidVersionNo; }
            set
            {
                _sidVersionNo = value;
                RaisePropertyChanged(() => SID_VersionNo);
            }
        }
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

        private Boolean _isChecked = false;
        /// <summary>
        /// 选择
        /// </summary>
        public Boolean IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                RaisePropertyChanged(() => IsChecked);
            }
        }

        private int? _printcount;
        /// <summary>
        /// 打印次数
        /// </summary>
        public int? PrintCount
        {
            get { return _printcount; }
            set
            {
                _printcount = value;
                RaisePropertyChanged(() => PrintCount);
            }
        }

        /// <summary>
        /// 编辑库存图片（仅做转配件图片用）
        /// </summary>
        public String EditAutoPartsPicture { get; set; }

        #endregion
    }
}
