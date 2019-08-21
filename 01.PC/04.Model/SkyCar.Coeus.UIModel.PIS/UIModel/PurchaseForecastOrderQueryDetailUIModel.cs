using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 采购补货建议查询明细UIModel
    /// </summary>
    public class PurchaseForecastOrderQueryDetailUIModel : BaseNotificationUIModel
    {
        #region 私有变量
        /// <summary>
        /// 订单号
        /// </summary>
        private String _pfod_pfo_no;
        /// <summary>
        /// 配件条码
        /// </summary>
        private String _pfod_autopartsbarcode;
        /// <summary>
        /// 第三方编码
        /// </summary>
        private String _pfod_thirdcode;
        /// <summary>
        /// 原厂编码
        /// </summary>
        private String _pfod_oemcode;
        /// <summary>
        /// 配件名称
        /// </summary>
        private String _pfod_autopartsname;
        /// <summary>
        /// 配件品牌
        /// </summary>
        private String _pfod_autopartsbrand;
        /// <summary>
        /// 规格型号
        /// </summary>
        private String _pfod_autopartsspec;
        /// <summary>
        /// 配件级别
        /// </summary>
        private String _pfod_autopartslevel;
        /// <summary>
        /// 计量单位
        /// </summary>
        private String _pfod_uom;
        /// <summary>
        /// 汽车品牌
        /// </summary>
        private String _pfod_vehiclebrand;
        /// <summary>
        /// 车系
        /// </summary>
        private String _pfod_vehicleinspire;
        /// <summary>
        /// 排量
        /// </summary>
        private String _pfod_vehiclecapacity;
        /// <summary>
        /// 年款
        /// </summary>
        private String _pfod_vehicleyearmodel;
        /// <summary>
        /// 变速类型
        /// </summary>
        private String _pfod_vehiclegearboxtype;
        /// <summary>
        /// 数量
        /// </summary>
        private Decimal? _pfod_qty;
        /// <summary>
        /// 最后一次采购单价
        /// </summary>
        private Decimal? _pfod_lastunitprice;
        /// <summary>
        /// 有效
        /// </summary>
        private Boolean? _pfod_isvalid;
        /// <summary>
        /// 创建人
        /// </summary>
        private String _pfod_createdby;
        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime? _pfod_createdtime;
        /// <summary>
        /// 修改人
        /// </summary>
        private String _pfod_updatedby;
        /// <summary>
        /// 修改时间
        /// </summary>
        private DateTime? _pfod_updatedtime;
        /// <summary>
        /// 版本号
        /// </summary>
        private Int64? _pfod_versionno;
        /// <summary>
        /// 采购预测订单明细ID
        /// </summary>
        private String _pfod_id;
        /// <summary>
        /// 采购预测订单ID
        /// </summary>
        private String _pfod_pfo_id;
        #endregion

        #region 公共属性
        /// <summary>
        /// 订单号
        /// </summary>
        public String PFOD_PFO_No
        {
            get { return _pfod_pfo_no; }
            set
            {
                _pfod_pfo_no = value;
                RaisePropertyChanged(() => PFOD_PFO_No);
            }
        }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String PFOD_AutoPartsBarcode
        {
            get { return _pfod_autopartsbarcode; }
            set
            {
                _pfod_autopartsbarcode = value;
                RaisePropertyChanged(() => PFOD_AutoPartsBarcode);
            }
        }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String PFOD_ThirdCode
        {
            get { return _pfod_thirdcode; }
            set
            {
                _pfod_thirdcode = value;
                RaisePropertyChanged(() => PFOD_ThirdCode);
            }
        }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String PFOD_OEMCode
        {
            get { return _pfod_oemcode; }
            set
            {
                _pfod_oemcode = value;
                RaisePropertyChanged(() => PFOD_OEMCode);
            }
        }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String PFOD_AutoPartsName
        {
            get { return _pfod_autopartsname; }
            set
            {
                _pfod_autopartsname = value;
                RaisePropertyChanged(() => PFOD_AutoPartsName);
            }
        }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String PFOD_AutoPartsBrand
        {
            get { return _pfod_autopartsbrand; }
            set
            {
                _pfod_autopartsbrand = value;
                RaisePropertyChanged(() => PFOD_AutoPartsBrand);
            }
        }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String PFOD_AutoPartsSpec
        {
            get { return _pfod_autopartsspec; }
            set
            {
                _pfod_autopartsspec = value;
                RaisePropertyChanged(() => PFOD_AutoPartsSpec);
            }
        }
        /// <summary>
        /// 配件级别
        /// </summary>
        public String PFOD_AutoPartsLevel
        {
            get { return _pfod_autopartslevel; }
            set
            {
                _pfod_autopartslevel = value;
                RaisePropertyChanged(() => PFOD_AutoPartsLevel);
            }
        }
        /// <summary>
        /// 计量单位
        /// </summary>
        public String PFOD_UOM
        {
            get { return _pfod_uom; }
            set
            {
                _pfod_uom = value;
                RaisePropertyChanged(() => PFOD_UOM);
            }
        }
        /// <summary>
        /// 汽车品牌
        /// </summary>
        public String PFOD_VehicleBrand
        {
            get { return _pfod_vehiclebrand; }
            set
            {
                _pfod_vehiclebrand = value;
                RaisePropertyChanged(() => PFOD_VehicleBrand);
            }
        }
        /// <summary>
        /// 车系
        /// </summary>
        public String PFOD_VehicleInspire
        {
            get { return _pfod_vehicleinspire; }
            set
            {
                _pfod_vehicleinspire = value;
                RaisePropertyChanged(() => PFOD_VehicleInspire);
            }
        }
        /// <summary>
        /// 排量
        /// </summary>
        public String PFOD_VehicleCapacity
        {
            get { return _pfod_vehiclecapacity; }
            set
            {
                _pfod_vehiclecapacity = value;
                RaisePropertyChanged(() => PFOD_VehicleCapacity);
            }
        }
        /// <summary>
        /// 年款
        /// </summary>
        public String PFOD_VehicleYearModel
        {
            get { return _pfod_vehicleyearmodel; }
            set
            {
                _pfod_vehicleyearmodel = value;
                RaisePropertyChanged(() => PFOD_VehicleYearModel);
            }
        }
        /// <summary>
        /// 变速类型
        /// </summary>
        public String PFOD_VehicleGearboxType
        {
            get { return _pfod_vehiclegearboxtype; }
            set
            {
                _pfod_vehiclegearboxtype = value;
                RaisePropertyChanged(() => PFOD_VehicleGearboxType);
            }
        }
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? PFOD_Qty
        {
            get { return _pfod_qty; }
            set
            {
                _pfod_qty = value;
                RaisePropertyChanged(() => PFOD_Qty);
            }
        }
        /// <summary>
        /// 最后一次采购单价
        /// </summary>
        public Decimal? PFOD_LastUnitPrice
        {
            get { return _pfod_lastunitprice; }
            set
            {
                _pfod_lastunitprice = value;
                RaisePropertyChanged(() => PFOD_LastUnitPrice);
            }
        }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? PFOD_IsValid
        {
            get { return _pfod_isvalid; }
            set
            {
                _pfod_isvalid = value;
                RaisePropertyChanged(() => PFOD_IsValid);
            }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public String PFOD_CreatedBy
        {
            get { return _pfod_createdby; }
            set
            {
                _pfod_createdby = value;
                RaisePropertyChanged(() => PFOD_CreatedBy);
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? PFOD_CreatedTime
        {
            get { return _pfod_createdtime; }
            set
            {
                _pfod_createdtime = value;
                RaisePropertyChanged(() => PFOD_CreatedTime);
            }
        }
        /// <summary>
        /// 修改人
        /// </summary>
        public String PFOD_UpdatedBy
        {
            get { return _pfod_updatedby; }
            set
            {
                _pfod_updatedby = value;
                RaisePropertyChanged(() => PFOD_UpdatedBy);
            }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? PFOD_UpdatedTime
        {
            get { return _pfod_updatedtime; }
            set
            {
                _pfod_updatedtime = value;
                RaisePropertyChanged(() => PFOD_UpdatedTime);
            }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? PFOD_VersionNo
        {
            get { return _pfod_versionno; }
            set
            {
                _pfod_versionno = value;
                RaisePropertyChanged(() => PFOD_VersionNo);
            }
        }
        /// <summary>
        /// 采购预测订单明细ID
        /// </summary>
        public String PFOD_ID
        {
            get { return _pfod_id; }
            set
            {
                _pfod_id = value;
                RaisePropertyChanged(() => PFOD_ID);
            }
        }
        /// <summary>
        /// 采购预测订单ID
        /// </summary>
        public String PFOD_PFO_ID
        {
            get { return _pfod_pfo_id; }
            set
            {
                _pfod_pfo_id = value;
                RaisePropertyChanged(() => PFOD_PFO_ID);
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


    }
}
