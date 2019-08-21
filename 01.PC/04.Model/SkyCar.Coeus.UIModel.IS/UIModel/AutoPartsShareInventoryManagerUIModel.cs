using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.IS.UIModel
{
    /// <summary>
    /// 库存共享管理UIModel
    /// </summary>
    public class AutoPartsShareInventoryManagerUIModel : BaseNotificationUIModel
    {
        #region 私有变量
        /// <summary>
        /// ID
        /// </summary>
        private String _si_id;
        /// <summary>
        /// 组织ID
        /// </summary>
        private String _si_org_id;
        /// <summary>
        /// 仓库ID
        /// </summary>
        private String _si_wh_id;
        /// <summary>
        /// 仓位ID
        /// </summary>
        private String _si_whb_id;
        /// <summary>
        /// 第三方编码
        /// </summary>
        private String _si_thirdno;
        /// <summary>
        /// 原厂编码
        /// </summary>
        private String _si_oemno;
        /// <summary>
        /// 配件条码
        /// </summary>
        private String _si_barcode;
        /// <summary>
        /// 配件批次号
        /// </summary>
        private String _si_batchno;
        /// <summary>
        /// 配件名称
        /// </summary>
        private String _si_name;
        /// <summary>
        /// 配件规格型号
        /// </summary>
        private String _si_specification;
        /// <summary>
        /// 供应商ID
        /// </summary>
        private String _si_supp_id;
        /// <summary>
        /// 数量
        /// </summary>
        private Decimal? _si_qty;

        /// <summary>
        /// 采购单价可见
        /// </summary>
        private Boolean? _si_PurchasePriceIsVisible;
        /// <summary>
        /// 采购单价
        /// </summary>
        private Decimal? _si_purchaseunitprice;
        /// <summary>
        /// 普通客户销售单价
        /// </summary>
        private Decimal? _si_priceofgeneralcustomer;
        /// <summary>
        /// 一般汽修商户销售单价
        /// </summary>
        private Decimal? _si_priceofcommonautofactory;
        /// <summary>
        /// 平台内汽修商销售单价
        /// </summary>
        private Decimal? _si_priceofplatformautofactory;
        /// <summary>
        /// 有效
        /// </summary>
        private Boolean? _si_isvalid;
        /// <summary>
        /// 创建人
        /// </summary>
        private String _si_createdby;
        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime? _si_createdtime;
        /// <summary>
        /// 修改人
        /// </summary>
        private String _si_updatedby;
        /// <summary>
        /// 修改时间
        /// </summary>
        private DateTime? _si_updatedtime;
        /// <summary>
        /// 版本号
        /// </summary>
        private Int64? _si_versionno;
        #endregion

        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String SI_ID
        {
            get { return _si_id; }
            set
            {
                _si_id = value;
                RaisePropertyChanged(() => SI_ID);
            }
        }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String SI_Org_ID
        {
            get { return _si_org_id; }
            set
            {
                _si_org_id = value;
                RaisePropertyChanged(() => SI_Org_ID);
            }
        }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String SI_WH_ID
        {
            get { return _si_wh_id; }
            set
            {
                _si_wh_id = value;
                RaisePropertyChanged(() => SI_WH_ID);
            }
        }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String SI_WHB_ID
        {
            get { return _si_whb_id; }
            set
            {
                _si_whb_id = value;
                RaisePropertyChanged(() => SI_WHB_ID);
            }
        }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String SI_ThirdNo
        {
            get { return _si_thirdno; }
            set
            {
                _si_thirdno = value;
                RaisePropertyChanged(() => SI_ThirdNo);
            }
        }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String SI_OEMNo
        {
            get { return _si_oemno; }
            set
            {
                _si_oemno = value;
                RaisePropertyChanged(() => SI_OEMNo);
            }
        }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String SI_Barcode
        {
            get { return _si_barcode; }
            set
            {
                _si_barcode = value;
                RaisePropertyChanged(() => SI_Barcode);
            }
        }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String SI_BatchNo
        {
            get { return _si_batchno; }
            set
            {
                _si_batchno = value;
                RaisePropertyChanged(() => SI_BatchNo);
            }
        }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String SI_Name
        {
            get { return _si_name; }
            set
            {
                _si_name = value;
                RaisePropertyChanged(() => SI_Name);
            }
        }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String SI_Specification
        {
            get { return _si_specification; }
            set
            {
                _si_specification = value;
                RaisePropertyChanged(() => SI_Specification);
            }
        }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String SI_SUPP_ID
        {
            get { return _si_supp_id; }
            set
            {
                _si_supp_id = value;
                RaisePropertyChanged(() => SI_SUPP_ID);
            }
        }
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? SI_Qty
        {
            get { return _si_qty; }
            set
            {
                _si_qty = value;
                RaisePropertyChanged(() => SI_Qty);
            }
        }
        /// <summary>
        /// 采购单价可见
        /// </summary>
        public Boolean? SI_PurchasePriceIsVisible
        {
            get { return _si_PurchasePriceIsVisible; }
            set
            {
                _si_PurchasePriceIsVisible = value;
                RaisePropertyChanged(() => SI_PurchasePriceIsVisible);
            }
        }
        /// <summary>
        /// 采购单价
        /// </summary>
        public Decimal? SI_PurchaseUnitPrice
        {
            get { return _si_purchaseunitprice; }
            set
            {
                _si_purchaseunitprice = value;
                RaisePropertyChanged(() => SI_PurchaseUnitPrice);
            }
        }
        /// <summary>
        /// 普通客户销售单价
        /// </summary>
        public Decimal? SI_PriceOfGeneralCustomer
        {
            get { return _si_priceofgeneralcustomer; }
            set
            {
                _si_priceofgeneralcustomer = value;
                RaisePropertyChanged(() => SI_PriceOfGeneralCustomer);
            }
        }
        /// <summary>
        /// 一般汽修商户销售单价
        /// </summary>
        public Decimal? SI_PriceOfCommonAutoFactory
        {
            get { return _si_priceofcommonautofactory; }
            set
            {
                _si_priceofcommonautofactory = value;
                RaisePropertyChanged(() => SI_PriceOfCommonAutoFactory);
            }
        }
        /// <summary>
        /// 平台内汽修商销售单价
        /// </summary>
        public Decimal? SI_PriceOfPlatformAutoFactory
        {
            get { return _si_priceofplatformautofactory; }
            set
            {
                _si_priceofplatformautofactory = value;
                RaisePropertyChanged(() => SI_PriceOfPlatformAutoFactory);
            }
        }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SI_IsValid
        {
            get { return _si_isvalid; }
            set
            {
                _si_isvalid = value;
                RaisePropertyChanged(() => SI_IsValid);
            }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SI_CreatedBy
        {
            get { return _si_createdby; }
            set
            {
                _si_createdby = value;
                RaisePropertyChanged(() => SI_CreatedBy);
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SI_CreatedTime
        {
            get { return _si_createdtime; }
            set
            {
                _si_createdtime = value;
                RaisePropertyChanged(() => SI_CreatedTime);
            }
        }
        /// <summary>
        /// 修改人
        /// </summary>
        public String SI_UpdatedBy
        {
            get { return _si_updatedby; }
            set
            {
                _si_updatedby = value;
                RaisePropertyChanged(() => SI_UpdatedBy);
            }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SI_UpdatedTime
        {
            get { return _si_updatedtime; }
            set
            {
                _si_updatedtime = value;
                RaisePropertyChanged(() => SI_UpdatedTime);
            }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? SI_VersionNo
        {
            get { return _si_versionno; }
            set
            {
                _si_versionno = value;
                RaisePropertyChanged(() => SI_VersionNo);
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
        #endregion

        private Boolean _ischecked;
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

        private string _wh_name;
        /// <summary>
        /// 仓库名称
        /// </summary>
        public String  WH_Name
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
        public String WHB_Name { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String SUPP_Name { get; set; }
        /// <summary>
        /// 临时ID
        /// </summary>
        public String RowID { get; set; }
        /// <summary>
        /// 操作类别(用于同步平台数据)
        /// </summary>
        public String OperateType { get; set; }

    }
}
