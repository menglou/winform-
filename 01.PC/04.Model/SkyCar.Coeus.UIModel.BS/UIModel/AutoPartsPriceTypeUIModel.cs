using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.BS
{
    /// <summary>
    /// 配件价格类别UIModel
    /// </summary>
    public class AutoPartsPriceTypeUIModel : BaseNotificationUIModel
    {
        #region 配件档案

        private String _apptId;
        /// <summary>
        /// 配件价格类别ID
        /// </summary>
        public String APPT_ID
        {
            get { return _apptId; }
            set
            {
                _apptId = value;
                RaisePropertyChanged(() => APPT_ID);
            }
        }
        private String _apptOrgId;
        /// <summary>
        /// 组织ID
        /// </summary>
        public String APPT_Org_ID
        {
            get { return _apptOrgId; }
            set
            {
                _apptOrgId = value;
                RaisePropertyChanged(() => APPT_Org_ID);
            }
        }
        private String _apptName;
        /// <summary>
        /// 配件价格类别名称
        /// </summary>
        public String APPT_Name
        {
            get { return _apptName; }
            set
            {
                _apptName = value;
                RaisePropertyChanged(() => APPT_Name);
            }
        }
        private String _apptBarcode;
        /// <summary>
        /// 条形码
        /// </summary>
        public String APPT_Barcode
        {
            get { return _apptBarcode; }
            set
            {
                _apptBarcode = value;
                RaisePropertyChanged(() => APPT_Barcode);
            }
        }
        private Decimal? _apptPrice;
        /// <summary>
        /// 价格
        /// </summary>
        public Decimal? APPT_Price
        {
            get { return _apptPrice; }
            set
            {
                _apptPrice = value;
                RaisePropertyChanged(() => APPT_Price);
            }
        }
        private Boolean? _apptIsValid;
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? APPT_IsValid
        {
            get { return _apptIsValid; }
            set
            {
                _apptIsValid = value;
                RaisePropertyChanged(() => APPT_IsValid);
            }
        }
        private String _apptCreatedBy;
        /// <summary>
        /// 创建人
        /// </summary>
        public String APPT_CreatedBy
        {
            get { return _apptCreatedBy; }
            set
            {
                _apptCreatedBy = value;
                RaisePropertyChanged(() => APPT_CreatedBy);
            }
        }
        private DateTime? _apptCreatedTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? APPT_CreatedTime
        {
            get { return _apptCreatedTime; }
            set
            {
                _apptCreatedTime = value;
                RaisePropertyChanged(() => APPT_CreatedTime);
            }
        }
        private String _apptUpdatedBy;
        /// <summary>
        /// 修改人
        /// </summary>
        public String APPT_UpdatedBy
        {
            get { return _apptUpdatedBy; }
            set
            {
                _apptUpdatedBy = value;
                RaisePropertyChanged(() => APPT_UpdatedBy);
            }
        }
        private DateTime? _APPT_UpdatedTime;
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APPT_UpdatedTime
        {
            get { return _APPT_UpdatedTime; }
            set
            {
                _APPT_UpdatedTime = value;
                RaisePropertyChanged(() => APPT_UpdatedTime);
            }
        }
        private Int64? _apptVersionNo;
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? APPT_VersionNo
        {
            get { return _apptVersionNo; }
            set
            {
                _apptVersionNo = value;
                RaisePropertyChanged(() => APPT_VersionNo);
            }
        }
        #endregion

        #region 其他

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
        private String _tmpId;
        /// <summary>
        /// 临时ID（用于下拉框）
        /// </summary>
        public String Tmp_ID
        {
            get { return _tmpId; }
            set
            {
                _tmpId = value;
                RaisePropertyChanged(() => Tmp_ID);
            }
        }
        
        /// <summary>
        /// 操作类别
        /// </summary>
        public String APPT_OperateType { get; set; }
        #endregion
    }
}
