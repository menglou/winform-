using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SD
{
    /// <summary>
    /// 物流单明细管理UIModel
    /// </summary>
    public class LogisticsBillDetailUIModelToPrint : BaseNotificationUIModel
    {
        #region 私有变量
        /// <summary>
        /// 物流订单明细ID
        /// </summary>
        private String _lbd_id;
        /// <summary>
        /// 物流订单ID
        /// </summary>
        private String _lbd_lb_id;
        /// <summary>
        /// 物流单号
        /// </summary>
        private String _lbd_lb_no;
        /// <summary>
        /// 条码
        /// </summary>
        private String _lbd_barcode;
        /// <summary>
        /// 配件批次号（原库存批次）
        /// </summary>
        private String _lbd_batchno;
        /// <summary>
        /// 配件批次号（汽修厂用）
        /// </summary>
        private String _lbd_batchnonew;
        /// <summary>
        /// 名称
        /// </summary>
        private String _lbd_name;
        /// <summary>
        /// 规格型号
        /// </summary>
        private String _lbd_specification;
        /// <summary>
        /// 单位
        /// </summary>
        private String _lbd_uom;
        /// <summary>
        /// 配送数量
        /// </summary>
        private Decimal? _lbd_deliveryqty;
        /// <summary>
        /// 签收数量
        /// </summary>
        private Decimal? _lbd_signqty;
        /// <summary>
        /// 拒收数量
        /// </summary>
        private Decimal? _lbd_rejectqty;
        /// <summary>
        /// 丢失数量
        /// </summary>
        private Decimal? _lbd_loseqty;
        /// <summary>
        /// 赔偿金
        /// </summary>
        private Decimal? _lbd_indemnification;
        /// <summary>
        /// 应收金额
        /// </summary>
        private Decimal? _lbd_accountreceivableamount;
        /// <summary>
        /// 状态编码
        /// </summary>
        private String _lbd_statuscode;
        /// <summary>
        /// 状态名称
        /// </summary>
        private String _lbd_statusname;
        /// <summary>
        /// 备注
        /// </summary>
        private String _lbd_remark;
        /// <summary>
        /// 有效
        /// </summary>
        private Boolean? _lbd_isvalid;
        /// <summary>
        /// 创建人
        /// </summary>
        private String _lbd_createdby;
        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime? _lbd_createdtime;
        /// <summary>
        /// 修改人
        /// </summary>
        private String _lbd_updatedby;
        /// <summary>
        /// 修改时间
        /// </summary>
        private DateTime? _lbd_updatedtime;
        /// <summary>
        /// 版本号
        /// </summary>
        private Int64? _lbd_versionno;
        #endregion

        #region 公共属性
        /// <summary>
        /// 物流订单明细ID
        /// </summary>
        public String LBD_ID
        {
            get { return _lbd_id; }
            set
            {
                _lbd_id = value;
                RaisePropertyChanged(() => LBD_ID);
            }
        }
        /// <summary>
        /// 物流订单ID
        /// </summary>
        public String LBD_LB_ID
        {
            get { return _lbd_lb_id; }
            set
            {
                _lbd_lb_id = value;
                RaisePropertyChanged(() => LBD_LB_ID);
            }
        }
        /// <summary>
        /// 物流单号
        /// </summary>
        public String LBD_LB_No
        {
            get { return _lbd_lb_no; }
            set
            {
                _lbd_lb_no = value;
                RaisePropertyChanged(() => LBD_LB_No);
            }
        }
        /// <summary>
        /// 条码
        /// </summary>
        public String LBD_Barcode
        {
            get { return _lbd_barcode; }
            set
            {
                _lbd_barcode = value;
                RaisePropertyChanged(() => LBD_Barcode);
            }
        }
        /// <summary>
        /// 配件批次号（原库存批次）
        /// </summary>
        public String LBD_BatchNo
        {
            get { return _lbd_batchno; }
            set
            {
                _lbd_batchno = value;
                RaisePropertyChanged(() => LBD_BatchNo);
            }
        }
        /// <summary>
        /// 配件批次号（汽修厂用）
        /// </summary>
        public String LBD_BatchNoNew
        {
            get { return _lbd_batchnonew; }
            set
            {
                _lbd_batchnonew = value;
                RaisePropertyChanged(() => LBD_BatchNoNew);
            }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public String LBD_Name
        {
            get { return _lbd_name; }
            set
            {
                _lbd_name = value;
                RaisePropertyChanged(() => LBD_Name);
            }
        }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String LBD_Specification
        {
            get { return _lbd_specification; }
            set
            {
                _lbd_specification = value;
                RaisePropertyChanged(() => LBD_Specification);
            }
        }
        /// <summary>
        /// 单位
        /// </summary>
        public String LBD_UOM
        {
            get { return _lbd_uom; }
            set
            {
                _lbd_uom = value;
                RaisePropertyChanged(() => LBD_UOM);
            }
        }
        /// <summary>
        /// 配送数量
        /// </summary>
        public Decimal? LBD_DeliveryQty
        {
            get { return _lbd_deliveryqty; }
            set
            {
                _lbd_deliveryqty = value;
                RaisePropertyChanged(() => LBD_DeliveryQty);
            }
        }
        /// <summary>
        /// 签收数量
        /// </summary>
        public Decimal? LBD_SignQty
        {
            get { return _lbd_signqty; }
            set
            {
                _lbd_signqty = value;
                RaisePropertyChanged(() => LBD_SignQty);
            }
        }
        /// <summary>
        /// 拒收数量
        /// </summary>
        public Decimal? LBD_RejectQty
        {
            get { return _lbd_rejectqty; }
            set
            {
                _lbd_rejectqty = value;
                RaisePropertyChanged(() => LBD_RejectQty);
            }
        }
        /// <summary>
        /// 丢失数量
        /// </summary>
        public Decimal? LBD_LoseQty
        {
            get { return _lbd_loseqty; }
            set
            {
                _lbd_loseqty = value;
                RaisePropertyChanged(() => LBD_LoseQty);
            }
        }
        /// <summary>
        /// 赔偿金
        /// </summary>
        public Decimal? LBD_Indemnification
        {
            get { return _lbd_indemnification; }
            set
            {
                _lbd_indemnification = value;
                RaisePropertyChanged(() => LBD_Indemnification);
            }
        }
        /// <summary>
        /// 应收金额
        /// </summary>
        public Decimal? LBD_AccountReceivableAmount
        {
            get { return _lbd_accountreceivableamount; }
            set
            {
                _lbd_accountreceivableamount = value;
                RaisePropertyChanged(() => LBD_AccountReceivableAmount);
            }
        }
        /// <summary>
        /// 状态编码
        /// </summary>
        public String LBD_StatusCode
        {
            get { return _lbd_statuscode; }
            set
            {
                _lbd_statuscode = value;
                RaisePropertyChanged(() => LBD_StatusCode);
            }
        }
        /// <summary>
        /// 状态名称
        /// </summary>
        public String LBD_StatusName
        {
            get { return _lbd_statusname; }
            set
            {




                _lbd_statusname = value;
                RaisePropertyChanged(() => LBD_StatusName);
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public String LBD_Remark
        {
            get { return _lbd_remark; }
            set
            {
                _lbd_remark = value;
                RaisePropertyChanged(() => LBD_Remark);
            }
        }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? LBD_IsValid
        {
            get { return _lbd_isvalid; }
            set
            {
                _lbd_isvalid = value;
                RaisePropertyChanged(() => LBD_IsValid);
            }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public String LBD_CreatedBy
        {
            get { return _lbd_createdby; }
            set
            {
                _lbd_createdby = value;
                RaisePropertyChanged(() => LBD_CreatedBy);
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? LBD_CreatedTime
        {
            get { return _lbd_createdtime; }
            set
            {
                _lbd_createdtime = value;
                RaisePropertyChanged(() => LBD_CreatedTime);
            }
        }
        /// <summary>
        /// 修改人
        /// </summary>
        public String LBD_UpdatedBy
        {
            get { return _lbd_updatedby; }
            set
            {
                _lbd_updatedby = value;
                RaisePropertyChanged(() => LBD_UpdatedBy);
            }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? LBD_UpdatedTime
        {
            get { return _lbd_updatedtime; }
            set
            {
                _lbd_updatedtime = value;
                RaisePropertyChanged(() => LBD_UpdatedTime);
            }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? LBD_VersionNo
        {
            get { return _lbd_versionno; }
            set
            {
                _lbd_versionno = value;
                RaisePropertyChanged(() => LBD_VersionNo);
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

        #region 私有的
        /// <summary>
        /// 单价
        /// </summary>
        public string LBD_SOD_UnitPrice { get; set; }
        /// <summary>
        /// 临时ID
        /// </summary>
        public string Tmp_SID_ID { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public string IsChecked { get; set; }
        #endregion

    }


}

