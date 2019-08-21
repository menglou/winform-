using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.FM
{
    /// <summary>
    /// 应收管理明细UIModel
    /// </summary>
    public class AccountReceivableBillDetailUIModelToPrint : BaseNotificationUIModel
    {
        #region 私有变量
        /// <summary>
        /// 选择
        /// </summary>
        private bool _ischecked;
        /// <summary>
        /// 是否负向明细
        /// </summary>
        private Boolean? _arbd_isminusdetail;
        /// <summary>
        /// 应收金额
        /// </summary>
        private Decimal? _arbd_accountreceivableamount;
        /// <summary>
        /// 已收金额
        /// </summary>
        private Decimal? _arbd_receivedamount;
        /// <summary>
        /// 未收金额
        /// </summary>
        private Decimal? _arbd_unreceiveamount;
        /// <summary>
        /// 业务状态
        /// </summary>
        private String _arbd_businessstatusname;
        /// <summary>
        /// 审核状态
        /// </summary>
        private String _arbd_approvalstatusname;
        /// <summary>
        /// 备注
        /// </summary>
        private String _arbd_remark;
        /// <summary>
        /// 有效
        /// </summary>
        private Boolean? _arbd_isvalid;
        /// <summary>
        /// 创建人
        /// </summary>
        private String _arbd_createdby;
        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime? _arbd_createdtime;
        /// <summary>
        /// 修改人
        /// </summary>
        private String _arbd_updatedby;
        /// <summary>
        /// 修改时间
        /// </summary>
        private DateTime? _arbd_updatedtime;
        /// <summary>
        /// 应收单明细ID
        /// </summary>
        private String _arbd_id;
        /// <summary>
        /// 应收单ID
        /// </summary>
        private String _arbd_arb_id;
        /// <summary>
        /// 来源单据号
        /// </summary>
        private String _arbd_srcbillno;
        /// <summary>
        /// 来源单据明细ID
        /// </summary>
        private String _arbd_srcbilldetailid;
        /// <summary>
        /// 组织ID
        /// </summary>
        private String _arbd_org_id;
        /// <summary>
        /// 组织名称
        /// </summary>
        private String _arbd_org_name;
        /// <summary>
        /// 业务状态编码
        /// </summary>
        private String _arbd_businessstatuscode;
        /// <summary>
        /// 审核状态编码
        /// </summary>
        private String _arbd_approvalstatuscode;
        /// <summary>
        /// 版本号
        /// </summary>
        private Int64? _arbd_versionno;
        /// <summary>
        /// 事务编号
        /// </summary>
        private String _arbd_transid;
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
        /// 是否负向明细
        /// </summary>
        public Boolean? ARBD_IsMinusDetail
        {
            get { return _arbd_isminusdetail; }
            set
            {
                _arbd_isminusdetail = value;
                RaisePropertyChanged(() => ARBD_IsMinusDetail);
            }
        }
        /// <summary>
        /// 应收金额
        /// </summary>
        public Decimal? ARBD_AccountReceivableAmount
        {
            get { return _arbd_accountreceivableamount; }
            set
            {
                _arbd_accountreceivableamount = value;
                RaisePropertyChanged(() => ARBD_AccountReceivableAmount);
            }
        }
        /// <summary>
        /// 已收金额
        /// </summary>
        public Decimal? ARBD_ReceivedAmount
        {
            get { return _arbd_receivedamount; }
            set
            {
                _arbd_receivedamount = value;
                RaisePropertyChanged(() => ARBD_ReceivedAmount);
            }
        }
        /// <summary>
        /// 未收金额
        /// </summary>
        public Decimal? ARBD_UnReceiveAmount
        {
            get { return _arbd_unreceiveamount; }
            set
            {
                _arbd_unreceiveamount = value;
                RaisePropertyChanged(() => ARBD_UnReceiveAmount);
            }
        }
        /// <summary>
        /// 业务状态
        /// </summary>
        public String ARBD_BusinessStatusName
        {
            get { return _arbd_businessstatusname; }
            set
            {
                _arbd_businessstatusname = value;
                RaisePropertyChanged(() => ARBD_BusinessStatusName);
            }
        }
        /// <summary>
        /// 审核状态
        /// </summary>
        public String ARBD_ApprovalStatusName
        {
            get { return _arbd_approvalstatusname; }
            set
            {
                _arbd_approvalstatusname = value;
                RaisePropertyChanged(() => ARBD_ApprovalStatusName);
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public String ARBD_Remark
        {
            get { return _arbd_remark; }
            set
            {
                _arbd_remark = value;
                RaisePropertyChanged(() => ARBD_Remark);
            }
        }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? ARBD_IsValid
        {
            get { return _arbd_isvalid; }
            set
            {
                _arbd_isvalid = value;
                RaisePropertyChanged(() => ARBD_IsValid);
            }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public String ARBD_CreatedBy
        {
            get { return _arbd_createdby; }
            set
            {
                _arbd_createdby = value;
                RaisePropertyChanged(() => ARBD_CreatedBy);
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? ARBD_CreatedTime
        {
            get { return _arbd_createdtime; }
            set
            {
                _arbd_createdtime = value;
                RaisePropertyChanged(() => ARBD_CreatedTime);
            }
        }
        /// <summary>
        /// 修改人
        /// </summary>
        public String ARBD_UpdatedBy
        {
            get { return _arbd_updatedby; }
            set
            {
                _arbd_updatedby = value;
                RaisePropertyChanged(() => ARBD_UpdatedBy);
            }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ARBD_UpdatedTime
        {
            get { return _arbd_updatedtime; }
            set
            {
                _arbd_updatedtime = value;
                RaisePropertyChanged(() => ARBD_UpdatedTime);
            }
        }
        /// <summary>
        /// 应收单明细ID
        /// </summary>
        public String ARBD_ID
        {
            get { return _arbd_id; }
            set
            {
                _arbd_id = value;
                RaisePropertyChanged(() => ARBD_ID);
            }
        }
        /// <summary>
        /// 应收单ID
        /// </summary>
        public String ARBD_ARB_ID
        {
            get { return _arbd_arb_id; }
            set
            {
                _arbd_arb_id = value;
                RaisePropertyChanged(() => ARBD_ARB_ID);
            }
        }
        /// <summary>
        /// 来源单据号
        /// </summary>
        public String ARBD_SrcBillNo
        {
            get { return _arbd_srcbillno; }
            set
            {
                _arbd_srcbillno = value;
                RaisePropertyChanged(() => ARBD_SrcBillNo);
            }
        }
        /// <summary>
        /// 来源单据明细ID
        /// </summary>
        public String ARBD_SrcBillDetailID
        {
            get { return _arbd_srcbilldetailid; }
            set
            {
                _arbd_srcbilldetailid = value;
                RaisePropertyChanged(() => ARBD_SrcBillDetailID);
            }
        }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String ARBD_Org_ID
        {
            get { return _arbd_org_id; }
            set
            {
                _arbd_org_id = value;
                RaisePropertyChanged(() => ARBD_Org_ID);
            }
        }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String ARBD_Org_Name
        {
            get { return _arbd_org_name; }
            set
            {
                _arbd_org_name = value;
                RaisePropertyChanged(() => ARBD_Org_Name);
            }
        }
        /// <summary>
        /// 业务状态编码
        /// </summary>
        public String ARBD_BusinessStatusCode
        {
            get { return _arbd_businessstatuscode; }
            set
            {
                _arbd_businessstatuscode = value;
                RaisePropertyChanged(() => ARBD_BusinessStatusCode);
            }
        }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String ARBD_ApprovalStatusCode
        {
            get { return _arbd_approvalstatuscode; }
            set
            {
                _arbd_approvalstatuscode = value;
                RaisePropertyChanged(() => ARBD_ApprovalStatusCode);
            }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? ARBD_VersionNo
        {
            get { return _arbd_versionno; }
            set
            {
                _arbd_versionno = value;
                RaisePropertyChanged(() => ARBD_VersionNo);
            }
        }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String ARBD_TransID
        {
            get { return _arbd_transid; }
            set
            {
                _arbd_transid = value;
                RaisePropertyChanged(() => ARBD_TransID);
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

    }
}
