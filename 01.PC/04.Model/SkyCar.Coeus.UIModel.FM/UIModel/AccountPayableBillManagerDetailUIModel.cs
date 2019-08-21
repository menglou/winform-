using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.FM
{
    /// <summary>
    /// 应付管理明细UIModel
    /// </summary>
    public class AccountPayableBillManagerDetailUIModel : BaseNotificationUIModel
    {
        #region 私有变量
        /// <summary>
        /// 选择
        /// </summary>
        private Boolean _ischecked;
        /// <summary>
        /// 是否负向明细
        /// </summary>
        private Boolean? _apbd_isminusdetail;
        /// <summary>
        /// 来源单号
        /// </summary>
        private String _apbd_sourcebillno;
        /// <summary>
        /// 组织名称
        /// </summary>
        private String _apbd_org_name;
        /// <summary>
        /// 应付金额
        /// </summary>
        private Decimal? _apbd_accountpayableamount;
        /// <summary>
        /// 已付金额
        /// </summary>
        private Decimal? _apbd_paidamount;
        /// <summary>
        /// 未付金额
        /// </summary>
        private Decimal? _apbd_unpaidamount;
        /// <summary>
        /// 业务状态
        /// </summary>
        private String _apbd_businessstatusname;
        /// <summary>
        /// 审核状态
        /// </summary>
        private String _apbd_approvalstatusname;
        /// <summary>
        /// 备注
        /// </summary>
        private String _apbd_remark;
        /// <summary>
        /// 有效
        /// </summary>
        private Boolean? _apbd_isvalid;
        /// <summary>
        /// 创建人
        /// </summary>
        private String _apbd_createdby;
        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime? _apbd_createdtime;
        /// <summary>
        /// 修改人
        /// </summary>
        private String _apbd_updatedby;
        /// <summary>
        /// 修改时间
        /// </summary>
        private DateTime? _apbd_updatedtime;
        /// <summary>
        /// 应付单明细ID
        /// </summary>
        private String _apbd_id;
        /// <summary>
        /// 应付单ID
        /// </summary>
        private String _apbd_apb_id;
        /// <summary>
        /// 来源单据明细ID
        /// </summary>
        private String _apbd_sourcebilldetailid;
        /// <summary>
        /// 组织ID
        /// </summary>
        private String _apbd_org_id;
        /// <summary>
        /// 业务状态编码
        /// </summary>
        private String _apbd_businessstatuscode;
        /// <summary>
        /// 审核状态编码
        /// </summary>
        private String _apbd_approvalstatuscode;
        /// <summary>
        /// 版本号
        /// </summary>
        private Int64? _apbd_versionno;
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
        /// 是否负向明细
        /// </summary>
        public Boolean? APBD_IsMinusDetail
        {
            get { return _apbd_isminusdetail; }
            set
            {
                _apbd_isminusdetail = value;
                RaisePropertyChanged(() => APBD_IsMinusDetail);
            }
        }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String APBD_SourceBillNo
        {
            get { return _apbd_sourcebillno; }
            set
            {
                _apbd_sourcebillno = value;
                RaisePropertyChanged(() => APBD_SourceBillNo);
            }
        }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String APBD_Org_Name
        {
            get { return _apbd_org_name; }
            set
            {
                _apbd_org_name = value;
                RaisePropertyChanged(() => APBD_Org_Name);
            }
        }
        /// <summary>
        /// 应付金额
        /// </summary>
        public Decimal? APBD_AccountPayableAmount
        {
            get { return _apbd_accountpayableamount; }
            set
            {
                _apbd_accountpayableamount = value;
                RaisePropertyChanged(() => APBD_AccountPayableAmount);
            }
        }
        /// <summary>
        /// 已付金额
        /// </summary>
        public Decimal? APBD_PaidAmount
        {
            get { return _apbd_paidamount; }
            set
            {
                _apbd_paidamount = value;
                RaisePropertyChanged(() => APBD_PaidAmount);
            }
        }
        /// <summary>
        /// 未付金额
        /// </summary>
        public Decimal? APBD_UnpaidAmount
        {
            get { return _apbd_unpaidamount; }
            set
            {
                _apbd_unpaidamount = value;
                RaisePropertyChanged(() => APBD_UnpaidAmount);
            }
        }
        /// <summary>
        /// 业务状态
        /// </summary>
        public String APBD_BusinessStatusName
        {
            get { return _apbd_businessstatusname; }
            set
            {
                _apbd_businessstatusname = value;
                RaisePropertyChanged(() => APBD_BusinessStatusName);
            }
        }
        /// <summary>
        /// 审核状态
        /// </summary>
        public String APBD_ApprovalStatusName
        {
            get { return _apbd_approvalstatusname; }
            set
            {
                _apbd_approvalstatusname = value;
                RaisePropertyChanged(() => APBD_ApprovalStatusName);
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public String APBD_Remark
        {
            get { return _apbd_remark; }
            set
            {
                _apbd_remark = value;
                RaisePropertyChanged(() => APBD_Remark);
            }
        }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? APBD_IsValid
        {
            get { return _apbd_isvalid; }
            set
            {
                _apbd_isvalid = value;
                RaisePropertyChanged(() => APBD_IsValid);
            }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public String APBD_CreatedBy
        {
            get { return _apbd_createdby; }
            set
            {
                _apbd_createdby = value;
                RaisePropertyChanged(() => APBD_CreatedBy);
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? APBD_CreatedTime
        {
            get { return _apbd_createdtime; }
            set
            {
                _apbd_createdtime = value;
                RaisePropertyChanged(() => APBD_CreatedTime);
            }
        }
        /// <summary>
        /// 修改人
        /// </summary>
        public String APBD_UpdatedBy
        {
            get { return _apbd_updatedby; }
            set
            {
                _apbd_updatedby = value;
                RaisePropertyChanged(() => APBD_UpdatedBy);
            }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APBD_UpdatedTime
        {
            get { return _apbd_updatedtime; }
            set
            {
                _apbd_updatedtime = value;
                RaisePropertyChanged(() => APBD_UpdatedTime);
            }
        }
        /// <summary>
        /// 应付单明细ID
        /// </summary>
        public String APBD_ID
        {
            get { return _apbd_id; }
            set
            {
                _apbd_id = value;
                RaisePropertyChanged(() => APBD_ID);
            }
        }
        /// <summary>
        /// 应付单ID
        /// </summary>
        public String APBD_APB_ID
        {
            get { return _apbd_apb_id; }
            set
            {
                _apbd_apb_id = value;
                RaisePropertyChanged(() => APBD_APB_ID);
            }
        }
        /// <summary>
        /// 来源单据明细ID
        /// </summary>
        public String APBD_SourceBillDetailID
        {
            get { return _apbd_sourcebilldetailid; }
            set
            {
                _apbd_sourcebilldetailid = value;
                RaisePropertyChanged(() => APBD_SourceBillDetailID);
            }
        }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String APBD_Org_ID
        {
            get { return _apbd_org_id; }
            set
            {
                _apbd_org_id = value;
                RaisePropertyChanged(() => APBD_Org_ID);
            }
        }
        /// <summary>
        /// 业务状态编码
        /// </summary>
        public String APBD_BusinessStatusCode
        {
            get { return _apbd_businessstatuscode; }
            set
            {
                _apbd_businessstatuscode = value;
                RaisePropertyChanged(() => APBD_BusinessStatusCode);
            }
        }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String APBD_ApprovalStatusCode
        {
            get { return _apbd_approvalstatuscode; }
            set
            {
                _apbd_approvalstatuscode = value;
                RaisePropertyChanged(() => APBD_ApprovalStatusCode);
            }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? APBD_VersionNo
        {
            get { return _apbd_versionno; }
            set
            {
                _apbd_versionno = value;
                RaisePropertyChanged(() => APBD_VersionNo);
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
