using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 库存查询明细UIModel
    /// </summary>
    public class InventoryTransLogUIModel : BaseNotificationUIModel
    {
        #region 私有变量
        /// <summary>
        /// 异动类型
        /// </summary>
        private String _itl_transtype;
        /// <summary>
        /// 组织ID
        /// </summary>
        private String _itl_org_id;
        /// <summary>
        /// 组织名称
        /// </summary>
        private String _itl_org_fullname;
        /// <summary>
        /// 仓库ID
        /// </summary>
        private String _itl_wh_id;
        /// <summary>
        /// 仓库名称
        /// </summary>
        private String _itl_wh_name;
        /// <summary>
        /// 仓位ID
        /// </summary>
        private String _itl_whb_id;
        /// <summary>
        /// 仓位名称
        /// </summary>
        private String _itl_whb_name;
        /// <summary>
        /// 业务单号
        /// </summary>
        private String _itl_businessno;
        /// <summary>
        /// 配件条码
        /// </summary>
        private String _itl_barcode;
        /// <summary>
        /// 配件批次号
        /// </summary>
        private String _itl_batchno;
        /// <summary>
        /// 配件名称
        /// </summary>
        private String _itl_name;
        /// <summary>
        /// 配件规格型号
        /// </summary>
        private String _itl_specification;
        /// <summary>
        /// 单位成本
        /// </summary>
        private Decimal? _itl_unitcostprice;
        /// <summary>
        /// 单位销价
        /// </summary>
        private Decimal? _itl_unitsaleprice;
        /// <summary>
        /// 异动数量
        /// </summary>
        private Decimal? _itl_qty;
        /// <summary>
        /// 异动后库存数量
        /// </summary>
        private Decimal? _itl_aftertransqty;

        /// <summary>
        /// 出发地
        /// </summary>
        private String _itl_source;

        /// <summary>
        /// 目的地
        /// </summary>
        public String _itl_destination;
        /// <summary>
        /// 有效
        /// </summary>
        private Boolean? _itl_isvalid;
        /// <summary>
        /// 创建人
        /// </summary>
        private String _itl_createdby;
        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime? _itl_createdtime;
        /// <summary>
        /// 修改人
        /// </summary>
        private String _itl_updatedby;
        /// <summary>
        /// 修改时间
        /// </summary>
        private DateTime? _itl_updatedtime;
        /// <summary>
        /// 版本号
        /// </summary>
        private Int64? _itl_versionno;
        #endregion

        #region 公共属性
        /// <summary>
        /// 异动类型
        /// </summary>
        public String ITL_TransType
        {
            get { return _itl_transtype; }
            set
            {
                _itl_transtype = value;
                RaisePropertyChanged(() => ITL_TransType);
            }
        }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String ITL_Org_ID
        {
            get { return _itl_org_id; }
            set
            {
                _itl_org_id = value;
                RaisePropertyChanged(() => ITL_Org_ID);
            }
        }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String ITL_Org_FullName
        {
            get { return _itl_org_fullname; }
            set
            {
                _itl_org_fullname = value;
                RaisePropertyChanged(() => ITL_Org_FullName);
            }
        }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String ITL_WH_ID
        {
            get { return _itl_wh_id; }
            set
            {
                _itl_wh_id = value;
                RaisePropertyChanged(() => ITL_WH_ID);
            }
        }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public String ITL_WH_Name
        {
            get { return _itl_wh_name; }
            set
            {
                _itl_wh_name = value;
                RaisePropertyChanged(() => ITL_WH_Name);
            }
        }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String ITL_WHB_ID
        {
            get { return _itl_whb_id; }
            set
            {
                _itl_whb_id = value;
                RaisePropertyChanged(() => ITL_WHB_ID);
            }
        }
        /// <summary>
        /// 仓位名称
        /// </summary>
        public String ITL_WHB_Name
        {
            get { return _itl_whb_name; }
            set
            {
                _itl_whb_name = value;
                RaisePropertyChanged(() => ITL_WHB_Name);
            }
        }
        /// <summary>
        /// 业务单号
        /// </summary>
        public String ITL_BusinessNo
        {
            get { return _itl_businessno; }
            set
            {
                _itl_businessno = value;
                RaisePropertyChanged(() => ITL_BusinessNo);
            }
        }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String ITL_Barcode
        {
            get { return _itl_barcode; }
            set
            {
                _itl_barcode = value;
                RaisePropertyChanged(() => ITL_Barcode);
            }
        }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String ITL_BatchNo
        {
            get { return _itl_batchno; }
            set
            {
                _itl_batchno = value;
                RaisePropertyChanged(() => ITL_BatchNo);
            }
        }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String ITL_Name
        {
            get { return _itl_name; }
            set
            {
                _itl_name = value;
                RaisePropertyChanged(() => ITL_Name);
            }
        }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String ITL_Specification
        {
            get { return _itl_specification; }
            set
            {
                _itl_specification = value;
                RaisePropertyChanged(() => ITL_Specification);
            }
        }
        /// <summary>
        /// 单位成本
        /// </summary>
        public Decimal? ITL_UnitCostPrice
        {
            get { return _itl_unitcostprice; }
            set
            {
                _itl_unitcostprice = value;
                RaisePropertyChanged(() => ITL_UnitCostPrice);
            }
        }
        /// <summary>
        /// 单位销价
        /// </summary>
        public Decimal? ITL_UnitSalePrice
        {
            get { return _itl_unitsaleprice; }
            set
            {
                _itl_unitsaleprice = value;
                RaisePropertyChanged(() => ITL_UnitSalePrice);
            }
        }
        /// <summary>
        /// 异动数量
        /// </summary>
        public Decimal? ITL_Qty
        {
            get { return _itl_qty; }
            set
            {
                _itl_qty = value;
                RaisePropertyChanged(() => ITL_Qty);
            }
        }
        /// <summary>
        /// 异动后库存数量
        /// </summary>
        public Decimal? ITL_AfterTransQty
        {
            get { return _itl_aftertransqty; }
            set
            {
                _itl_aftertransqty = value;
                RaisePropertyChanged(() => ITL_AfterTransQty);
            }
        }
        /// <summary>
        /// 出发地
        /// </summary>
        public String ITL_Source
        {
            get { return _itl_source; }
            set
            {
                _itl_source = value;
                RaisePropertyChanged(() => ITL_Source);
            }
        }
        /// <summary>
        /// 目的地
        /// </summary>
        public String ITL_Destination
        {
            get { return _itl_destination; }
            set
            {
                _itl_destination = value;
                RaisePropertyChanged(() => ITL_Destination);
            }
        }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? ITL_IsValid
        {
            get { return _itl_isvalid; }
            set
            {
                _itl_isvalid = value;
                RaisePropertyChanged(() => ITL_IsValid);
            }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public String ITL_CreatedBy
        {
            get { return _itl_createdby; }
            set
            {
                _itl_createdby = value;
                RaisePropertyChanged(() => ITL_CreatedBy);
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? ITL_CreatedTime
        {
            get { return _itl_createdtime; }
            set
            {
                _itl_createdtime = value;
                RaisePropertyChanged(() => ITL_CreatedTime);
            }
        }
        /// <summary>
        /// 修改人
        /// </summary>
        public String ITL_UpdatedBy
        {
            get { return _itl_updatedby; }
            set
            {
                _itl_updatedby = value;
                RaisePropertyChanged(() => ITL_UpdatedBy);
            }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ITL_UpdatedTime
        {
            get { return _itl_updatedtime; }
            set
            {
                _itl_updatedtime = value;
                RaisePropertyChanged(() => ITL_UpdatedTime);
            }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? ITL_VersionNo
        {
            get { return _itl_versionno; }
            set
            {
                _itl_versionno = value;
                RaisePropertyChanged(() => ITL_VersionNo);
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
