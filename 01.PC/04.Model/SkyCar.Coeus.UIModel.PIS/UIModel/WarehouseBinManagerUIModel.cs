using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 仓位管理UIModel
    /// </summary>
    public class WarehouseBinManagerUIModel : BaseNotificationUIModel
    {
        private string _whb_Name;
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
        private String _whb_Description;
        /// <summary>
        /// 仓位描述
        /// </summary>
        public String WHB_Description
        {
            get { return _whb_Description; }
            set
            {
                _whb_Description = value;
                RaisePropertyChanged(() => WHB_Description);
            }
        }
        private Boolean? _whb_IsValid;
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHB_IsValid
        {
            get { return _whb_IsValid; }
            set
            {
                _whb_IsValid = value;
                RaisePropertyChanged(() => WHB_IsValid);
            }
        }
        private String _whbCreatedBy;
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHB_CreatedBy
        {
            get { return _whbCreatedBy; }
            set
            {
                _whbCreatedBy = value;
                RaisePropertyChanged(() => WHB_CreatedBy);
            }
        }
        private DateTime? _whbCreatedTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHB_CreatedTime
        {
            get { return _whbCreatedTime; }
            set
            {
                _whbCreatedTime = value;
                RaisePropertyChanged(() => WHB_CreatedTime);
            }
        }
        private String _whbUpdatedBy;
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHB_UpdatedBy
        {
            get { return _whbUpdatedBy; }
            set
            {
                _whbUpdatedBy = value;
                RaisePropertyChanged(() => WHB_UpdatedBy);
            }
        }
        private Int64? _whbVersionNo;
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHB_VersionNo
        {
            get { return _whbVersionNo; }
            set
            {
                _whbVersionNo = value;
                RaisePropertyChanged(() => WHB_VersionNo);
            }
        }
        private String _whbId;
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String WHB_ID
        {
            get { return _whbId; }
            set
            {
                _whbId = value;
                RaisePropertyChanged(() => WHB_ID);
            }
        }
        private String _whbWhId;
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WHB_WH_ID
        {
            get { return _whbWhId; }
            set
            {
                _whbWhId = value;
                RaisePropertyChanged(() => WHB_WH_ID);
            }
        }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String WHERE_WHB_ID { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHB_UpdatedTime { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public String Tmp_SID_ID { get; set; }

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
    }
}
