using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 采购订单管理明细UIModel
    /// </summary>
    public class PurchaseOrderDetailUIModelToPrint : BaseNotificationUIModel
    {
        #region 私有变量
        /// <summary>
        /// 订单号
        /// </summary>
        private String _pod_po_no;
        /// <summary>
        /// 配件条码
        /// </summary>
        private String _pod_autopartsbarcode;
        /// <summary>
        /// 第三方编码
        /// </summary>
        private String _pod_thirdcode;
        /// <summary>
        /// 原厂编码
        /// </summary>
        private String _pod_oemcode;
        /// <summary>
        /// 配件名称
        /// </summary>
        private String _pod_autopartsname;
        /// <summary>
        /// 配件品牌
        /// </summary>
        private String _pod_autopartsbrand;
        /// <summary>
        /// 规格型号
        /// </summary>
        private String _pod_autopartsspec;
        /// <summary>
        /// 配件级别
        /// </summary>
        private String _pod_autopartslevel;
        /// <summary>
        /// 计量单位
        /// </summary>
        private String _pod_uom;
        /// <summary>
        /// 汽车品牌
        /// </summary>
        private String _pod_vehiclebrand;
        /// <summary>
        /// 车系
        /// </summary>
        private String _pod_vehicleinspire;
        /// <summary>
        /// 排量
        /// </summary>
        private String _pod_vehiclecapacity;
        /// <summary>
        /// 年款
        /// </summary>
        private String _pod_vehicleyearmodel;
        /// <summary>
        /// 变速类型
        /// </summary>
        private String _pod_vehiclegearboxtype;
        /// <summary>
        /// 进货仓库ID
        /// </summary>
        private String _pod_wh_id;
        /// <summary>
        /// 进货仓库名称
        /// </summary>
        private String _wh_name;
        /// <summary>
        /// 进货仓位ID
        /// </summary>
        private String _pod_whb_id;
        /// <summary>
        /// 进货仓位名称
        /// </summary>
        private String _whb_name;
        /// <summary>
        /// 订货数量
        /// </summary>
        private Decimal? _pod_orderqty;
        /// <summary>
        /// 签收数量
        /// </summary>
        private Decimal? _pod_receivedqty;
        /// <summary>
        /// 订货单价
        /// </summary>
        private Decimal? _pod_unitprice;
        /// <summary>
        /// 单据状态
        /// </summary>
        private String _pod_statusname;
        /// <summary>
        /// 到货时间
        /// </summary>
        private DateTime? _pod_receivedtime;
        /// <summary>
        /// 有效
        /// </summary>
        private Boolean? _pod_isvalid;
        /// <summary>
        /// 创建人
        /// </summary>
        private String _pod_createdby;
        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime? _pod_createdtime;
        /// <summary>
        /// 修改人
        /// </summary>
        private String _pod_updatedby;
        /// <summary>
        /// 修改时间
        /// </summary>
        private DateTime? _pod_updatedtime;
        /// <summary>
        /// 版本号
        /// </summary>
        private Int64? _pod_versionno;
        /// <summary>
        /// 采购订单明细ID
        /// </summary>
        private String _pod_id;
        /// <summary>
        /// 采购订单ID
        /// </summary>
        private String _pod_po_id;
        /// <summary>
        /// 单据状态编码
        /// </summary>
        private String _pod_statuscode;
        #endregion

        #region 公共属性
        /// <summary>
        /// 订单号
        /// </summary>
        public String POD_PO_No
        {
            get { return _pod_po_no; }
            set
            {
                _pod_po_no = value;
                RaisePropertyChanged(() => POD_PO_No);
            }
        }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String POD_AutoPartsBarcode
        {
            get { return _pod_autopartsbarcode; }
            set
            {
                _pod_autopartsbarcode = value;
                RaisePropertyChanged(() => POD_AutoPartsBarcode);
            }
        }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String POD_ThirdCode
        {
            get { return _pod_thirdcode; }
            set
            {
                _pod_thirdcode = value;
                RaisePropertyChanged(() => POD_ThirdCode);
            }
        }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String POD_OEMCode
        {
            get { return _pod_oemcode; }
            set
            {
                _pod_oemcode = value;
                RaisePropertyChanged(() => POD_OEMCode);
            }
        }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String POD_AutoPartsName
        {
            get { return _pod_autopartsname; }
            set
            {
                _pod_autopartsname = value;
                RaisePropertyChanged(() => POD_AutoPartsName);
            }
        }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String POD_AutoPartsBrand
        {
            get { return _pod_autopartsbrand; }
            set
            {
                _pod_autopartsbrand = value;
                RaisePropertyChanged(() => POD_AutoPartsBrand);
            }
        }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String POD_AutoPartsSpec
        {
            get { return _pod_autopartsspec; }
            set
            {
                _pod_autopartsspec = value;
                RaisePropertyChanged(() => POD_AutoPartsSpec);
            }
        }
        /// <summary>
        /// 配件级别
        /// </summary>
        public String POD_AutoPartsLevel
        {
            get { return _pod_autopartslevel; }
            set
            {
                _pod_autopartslevel = value;
                RaisePropertyChanged(() => POD_AutoPartsLevel);
            }
        }
        /// <summary>
        /// 计量单位
        /// </summary>
        public String POD_UOM
        {
            get { return _pod_uom; }
            set
            {
                _pod_uom = value;
                RaisePropertyChanged(() => POD_UOM);
            }
        }
        /// <summary>
        /// 汽车品牌
        /// </summary>
        public String POD_VehicleBrand
        {
            get { return _pod_vehiclebrand; }
            set
            {
                _pod_vehiclebrand = value;
                RaisePropertyChanged(() => POD_VehicleBrand);
            }
        }
        /// <summary>
        /// 车系
        /// </summary>
        public String POD_VehicleInspire
        {
            get { return _pod_vehicleinspire; }
            set
            {
                _pod_vehicleinspire = value;
                RaisePropertyChanged(() => POD_VehicleInspire);
            }
        }
        /// <summary>
        /// 排量
        /// </summary>
        public String POD_VehicleCapacity
        {
            get { return _pod_vehiclecapacity; }
            set
            {
                _pod_vehiclecapacity = value;
                RaisePropertyChanged(() => POD_VehicleCapacity);
            }
        }
        /// <summary>
        /// 年款
        /// </summary>
        public String POD_VehicleYearModel
        {
            get { return _pod_vehicleyearmodel; }
            set
            {
                _pod_vehicleyearmodel = value;
                RaisePropertyChanged(() => POD_VehicleYearModel);
            }
        }
        /// <summary>
        /// 变速类型
        /// </summary>
        public String POD_VehicleGearboxType
        {
            get { return _pod_vehiclegearboxtype; }
            set
            {
                _pod_vehiclegearboxtype = value;
                RaisePropertyChanged(() => POD_VehicleGearboxType);
            }
        }
        /// <summary>
        /// 进货仓库ID
        /// </summary>
        public String POD_WH_ID
        {
            get { return _pod_wh_id; }
            set
            {
                _pod_wh_id = value;
                RaisePropertyChanged(() => POD_WH_ID);
            }
        }
        /// <summary>
        /// 进货仓库名称
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
        /// 进货仓位ID
        /// </summary>
        public String POD_WHB_ID
        {
            get { return _pod_whb_id; }
            set
            {
                _pod_whb_id = value;
                RaisePropertyChanged(() => POD_WHB_ID);
            }
        }
        /// <summary>
        /// 进货仓位名称
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
        /// 订货数量
        /// </summary>
        public Decimal? POD_OrderQty
        {
            get { return _pod_orderqty; }
            set
            {
                _pod_orderqty = value;
                RaisePropertyChanged(() => POD_OrderQty);
            }
        }
        /// <summary>
        /// 签收数量
        /// </summary>
        public Decimal? POD_ReceivedQty
        {
            get { return _pod_receivedqty; }
            set
            {
                _pod_receivedqty = value;
                RaisePropertyChanged(() => POD_ReceivedQty);
            }
        }
        /// <summary>
        /// 订货单价
        /// </summary>
        public Decimal? POD_UnitPrice
        {
            get { return _pod_unitprice; }
            set
            {
                _pod_unitprice = value;
                RaisePropertyChanged(() => POD_UnitPrice);
            }
        }
        /// <summary>
        /// 单据状态
        /// </summary>
        public String POD_StatusName
        {
            get { return _pod_statusname; }
            set
            {
                _pod_statusname = value;
                RaisePropertyChanged(() => POD_StatusName);
            }
        }
        /// <summary>
        /// 到货时间
        /// </summary>
        public DateTime? POD_ReceivedTime
        {
            get { return _pod_receivedtime; }
            set
            {
                _pod_receivedtime = value;
                RaisePropertyChanged(() => POD_ReceivedTime);
            }
        }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? POD_IsValid
        {
            get { return _pod_isvalid; }
            set
            {
                _pod_isvalid = value;
                RaisePropertyChanged(() => POD_IsValid);
            }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public String POD_CreatedBy
        {
            get { return _pod_createdby; }
            set
            {
                _pod_createdby = value;
                RaisePropertyChanged(() => POD_CreatedBy);
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? POD_CreatedTime
        {
            get { return _pod_createdtime; }
            set
            {
                _pod_createdtime = value;
                RaisePropertyChanged(() => POD_CreatedTime);
            }
        }
        /// <summary>
        /// 修改人
        /// </summary>
        public String POD_UpdatedBy
        {
            get { return _pod_updatedby; }
            set
            {
                _pod_updatedby = value;
                RaisePropertyChanged(() => POD_UpdatedBy);
            }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? POD_UpdatedTime
        {
            get { return _pod_updatedtime; }
            set
            {
                _pod_updatedtime = value;
                RaisePropertyChanged(() => POD_UpdatedTime);
            }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? POD_VersionNo
        {
            get { return _pod_versionno; }
            set
            {
                _pod_versionno = value;
                RaisePropertyChanged(() => POD_VersionNo);
            }
        }
        /// <summary>
        /// 采购订单明细ID
        /// </summary>
        public String POD_ID
        {
            get { return _pod_id; }
            set
            {
                _pod_id = value;
                RaisePropertyChanged(() => POD_ID);
            }
        }
        /// <summary>
        /// 采购订单ID
        /// </summary>
        public String POD_PO_ID
        {
            get { return _pod_po_id; }
            set
            {
                _pod_po_id = value;
                RaisePropertyChanged(() => POD_PO_ID);
            }
        }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String POD_StatusCode
        {
            get { return _pod_statuscode; }
            set
            {
                _pod_statuscode = value;
                RaisePropertyChanged(() => POD_StatusCode);
            }
        }
        /// <summary>
        /// 总记录条数（翻页用）
        /// </summary>
        public Int32? RecordCount { get; set; }
        /// <summary>
        /// 当前页面索引/要跳转的页码（翻页用）
        /// </summary>
        public Int32? PageIndex { get; set; }
        /// <summary>
        /// 页面大小（翻页用）
        /// </summary>
        public Int32? PageSize { get; set; }
        #endregion

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
        /// 临时ID
        /// </summary>
        public string Tmp_SID_ID { get; set; }
    }
}
