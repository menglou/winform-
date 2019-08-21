using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.BS.UIModel
{
    /// <summary>
    /// 原厂件信息UIModel
    /// </summary>
    public class VehicleOemPartsInfoUIModel : BaseNotificationUIModel
    {
        #region 公共属性

        private bool _isChecked;
        /// <summary>
        /// 是否选中
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

        private String _vopiId;
        /// <summary>
        /// ID
        /// </summary>
        public String VOPI_ID
        {
            get { return _vopiId; }
            set
            {
                _vopiId = value;
                RaisePropertyChanged(() => VOPI_ID);
            }
        }

        private String _vopiVcVin;
        /// <summary>
        /// 车架号
        /// </summary>
        public String VOPI_VC_VIN
        {
            get { return _vopiVcVin; }
            set
            {
                _vopiVcVin = value;
                RaisePropertyChanged(() => VOPI_VC_VIN);
            }
        }

        private String _vopiOemNo;
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String VOPI_OEMNo
        {
            get { return _vopiOemNo; }
            set
            {
                _vopiOemNo = value;
                RaisePropertyChanged(() => VOPI_OEMNo);
            }
        }

        private String _vopiAutoPartsName;
        /// <summary>
        /// 配件名称
        /// </summary>
        public String VOPI_AutoPartsName
        {
            get { return _vopiAutoPartsName; }
            set
            {
                _vopiAutoPartsName = value;
                RaisePropertyChanged(() => VOPI_AutoPartsName);
            }
        }

        private String _vopiRemark;
        /// <summary>
        /// 备注
        /// </summary>
        public String VOPI_Remark
        {
            get { return _vopiRemark; }
            set
            {
                _vopiRemark = value;
                RaisePropertyChanged(() => VOPI_Remark);
            }
        }

        private Boolean? _vopiIsValid;
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? VOPI_IsValid
        {
            get { return _vopiIsValid; }
            set
            {
                _vopiIsValid = value;
                RaisePropertyChanged(() => VOPI_IsValid);
            }
        }

        private String _vopiCreatedBy;
        /// <summary>
        /// 创建人
        /// </summary>
        public String VOPI_CreatedBy
        {
            get { return _vopiCreatedBy; }
            set
            {
                _vopiCreatedBy = value;
                RaisePropertyChanged(() => VOPI_CreatedBy);
            }
        }

        private DateTime? _vopiCreatedTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? VOPI_CreatedTime
        {
            get { return _vopiCreatedTime; }
            set
            {
                _vopiCreatedTime = value;
                RaisePropertyChanged(() => VOPI_CreatedTime);
            }
        }

        private String _vopiUpdatedBy;
        /// <summary>
        /// 修改人
        /// </summary>
        public String VOPI_UpdatedBy
        {
            get { return _vopiUpdatedBy; }
            set
            {
                _vopiUpdatedBy = value;
                RaisePropertyChanged(() => VOPI_UpdatedBy);
            }
        }

        private DateTime? _vopiUpdatedTime;
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? VOPI_UpdatedTime
        {
            get { return _vopiUpdatedTime; }
            set
            {
                _vopiUpdatedTime = value;
                RaisePropertyChanged(() => VOPI_UpdatedTime);
            }
        }

        private Int64? _vopiVersionNo;
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? VOPI_VersionNo
        {
            get { return _vopiVersionNo; }
            set
            {
                _vopiVersionNo = value;
                RaisePropertyChanged(() => VOPI_VersionNo);
            }
        }
        #endregion

        /// <summary>
        /// ID
        /// </summary>
        public String WHERE_VOPI_ID { get; set; }

        /// <summary>
        /// 操作类别
        /// </summary>
        public String VOPI_OperateType { get; set; }
    }
}
