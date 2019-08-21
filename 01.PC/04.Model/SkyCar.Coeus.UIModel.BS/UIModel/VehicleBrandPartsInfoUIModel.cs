using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.BS.UIModel
{
    /// <summary>
    /// 车辆原厂品牌关联信息UIModel
    /// </summary>
    public class VehicleBrandPartsInfoUIModel : BaseNotificationUIModel
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

        private String _vbpiId;
        /// <summary>
        /// ID
        /// </summary>
        public String VBPI_ID
        {
            get { return _vbpiId; }
            set
            {
                _vbpiId = value;
                RaisePropertyChanged(() => VBPI_ID);
            }
        }

        private String _vbpiVcVin;
        /// <summary>
        /// 车架号
        /// </summary>
        public String VBPI_VC_VIN
        {
            get { return _vbpiVcVin; }
            set
            {
                _vbpiVcVin = value;
                RaisePropertyChanged(() => VBPI_VC_VIN);
            }
        }

        private String _vbpiVopiOemNo;
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String VBPI_VOPI_OEMNo
        {
            get { return _vbpiVopiOemNo; }
            set
            {
                _vbpiVopiOemNo = value;
                RaisePropertyChanged(() => VBPI_VOPI_OEMNo);
            }
        }

        private String _vbpiThirdNo;
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String VBPI_ThirdNo
        {
            get { return _vbpiThirdNo; }
            set
            {
                _vbpiThirdNo = value;
                RaisePropertyChanged(() => VBPI_ThirdNo);
            }
        }

        private String _vbpiAutoPartsBrand;
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String VBPI_AutoPartsBrand
        {
            get { return _vbpiAutoPartsBrand; }
            set
            {
                _vbpiAutoPartsBrand = value;
                RaisePropertyChanged(() => VBPI_AutoPartsBrand);
            }
        }

        private String _vbpiRemark;
        /// <summary>
        /// 备注
        /// </summary>
        public String VBPI_Remark
        {
            get { return _vbpiRemark; }
            set
            {
                _vbpiRemark = value;
                RaisePropertyChanged(() => VBPI_Remark);
            }
        }

        private Boolean? _vbpiIsValid;
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? VBPI_IsValid
        {
            get { return _vbpiIsValid; }
            set
            {
                _vbpiIsValid = value;
                RaisePropertyChanged(() => VBPI_IsValid);
            }
        }

        private String _vbpiCreatedBy;
        /// <summary>
        /// 创建人
        /// </summary>
        public String VBPI_CreatedBy
        {
            get { return _vbpiCreatedBy; }
            set
            {
                _vbpiCreatedBy = value;
                RaisePropertyChanged(() => VBPI_CreatedBy);
            }
        }

        private DateTime? _vbpiCreatedTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? VBPI_CreatedTime
        {
            get { return _vbpiCreatedTime; }
            set
            {
                _vbpiCreatedTime = value;
                RaisePropertyChanged(() => VBPI_CreatedTime);
            }
        }

        private String _vbpiUpdatedBy;
        /// <summary>
        /// 修改人
        /// </summary>
        public String VBPI_UpdatedBy
        {
            get { return _vbpiUpdatedBy; }
            set
            {
                _vbpiUpdatedBy = value;
                RaisePropertyChanged(() => VBPI_UpdatedBy);
            }
        }

        private DateTime? _vbpiUpdatedTime;
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? VBPI_UpdatedTime
        {
            get { return _vbpiUpdatedTime; }
            set
            {
                _vbpiUpdatedTime = value;
                RaisePropertyChanged(() => VBPI_UpdatedTime);
            }
        }

        private Int64? _vbpiVersionNo;
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? VBPI_VersionNo
        {
            get { return _vbpiVersionNo; }
            set
            {
                _vbpiVersionNo = value;
                RaisePropertyChanged(() => VBPI_VersionNo);
            }
        }
        #endregion

        /// <summary>
        /// ID
        /// </summary>
        public String WHERE_VBPI_ID { get; set; }

        /// <summary>
        /// 操作类别
        /// </summary>
        public String VBPI_OperateType { get; set; }
    }
}
