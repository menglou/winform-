using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 汽配汽修组织授权UIModel
    /// </summary>
    public class AROrgSupOrgAuthorityUIModel : BaseNotificationUIModel
    {
        #region 公共属性

        private String _asoahId;
        /// <summary>
        /// ID
        /// </summary>
        public String ASOAH_ID
        {
            get { return _asoahId; }
            set
            {
                _asoahId = value;
                RaisePropertyChanged(() => ASOAH_ID);
            }
        }
        private String _asoahSupOrgId;
        /// <summary>
        /// 汽配组织ID
        /// </summary>
        public String ASOAH_SupOrg_ID
        {
            get { return _asoahSupOrgId; }
            set
            {
                _asoahSupOrgId = value;
                RaisePropertyChanged(() => ASOAH_SupOrg_ID);
            }
        }
        private String _asoahAfcId;
        /// <summary>
        /// 汽修商客户ID
        /// </summary>
        public String ASOAH_AFC_ID
        {
            get { return _asoahAfcId; }
            set
            {
                _asoahAfcId = value;
                RaisePropertyChanged(() => ASOAH_AFC_ID);
            }
        }
        private String _asoahArMerchantCode;
        /// <summary>
        /// 汽修商户编码
        /// </summary>
        public String ASOAH_ARMerchant_Code
        {
            get { return _asoahArMerchantCode; }
            set
            {
                _asoahArMerchantCode = value;
                RaisePropertyChanged(() => ASOAH_ARMerchant_Code);
            }
        }
        private String _asoahArMerchantName;
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String ASOAH_ARMerchant_Name
        {
            get { return _asoahArMerchantName; }
            set
            {
                _asoahArMerchantName = value;
                RaisePropertyChanged(() => ASOAH_ARMerchant_Name);
            }
        }
        private String _asoahArOrgCode;
        /// <summary>
        /// 汽修组织编码
        /// </summary>
        public String ASOAH_AROrg_Code
        {
            get { return _asoahArOrgCode; }
            set
            {
                _asoahArOrgCode = value;
                RaisePropertyChanged(() => ASOAH_AROrg_Code);
            }
        }
        private String _asoahArOrgName;
        /// <summary>
        /// 汽修组织名称
        /// </summary>
        public String ASOAH_AROrg_Name
        {
            get { return _asoahArOrgName; }
            set
            {
                _asoahArOrgName = value;
                RaisePropertyChanged(() => ASOAH_AROrg_Name);
            }
        }
        private String _asoahRemark;
        /// <summary>
        /// 备注
        /// </summary>
        public String ASOAH_Remark
        {
            get { return _asoahRemark; }
            set
            {
                _asoahRemark = value;
                RaisePropertyChanged(() => ASOAH_Remark);
            }
        }
        private Boolean? _asoahIsValid;
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? ASOAH_IsValid
        {
            get { return _asoahIsValid; }
            set
            {
                _asoahIsValid = value;
                RaisePropertyChanged(() => ASOAH_IsValid);
            }
        }
        private String _asoahCreatedBy;
        /// <summary>
        /// 创建人
        /// </summary>
        public String ASOAH_CreatedBy
        {
            get { return _asoahCreatedBy; }
            set
            {
                _asoahCreatedBy = value;
                RaisePropertyChanged(() => ASOAH_CreatedBy);
            }
        }
        private DateTime? _asoahCreatedTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? ASOAH_CreatedTime
        {
            get { return _asoahCreatedTime; }
            set
            {
                _asoahCreatedTime = value;
                RaisePropertyChanged(() => ASOAH_CreatedTime);
            }
        }
        private String _asoahUpdatedBy;
        /// <summary>
        /// 修改人
        /// </summary>
        public String ASOAH_UpdatedBy
        {
            get { return _asoahUpdatedBy; }
            set
            {
                _asoahUpdatedBy = value;
                RaisePropertyChanged(() => ASOAH_UpdatedBy);
            }
        }
        private DateTime? _asoahUpdatedTime;
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ASOAH_UpdatedTime
        {
            get { return _asoahUpdatedTime; }
            set
            {
                _asoahUpdatedTime = value;
                RaisePropertyChanged(() => ASOAH_UpdatedTime);
            }
        }
        private Int64? _asoahVersionNo;
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? ASOAH_VersionNo
        {
            get { return _asoahVersionNo; }
            set
            {
                _asoahVersionNo = value;
                RaisePropertyChanged(() => ASOAH_VersionNo);
            }
        }
        #endregion
    }
}
