using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.BS.UIModel
{
    /// <summary>
    /// 车辆品牌件信息UIModel
    /// </summary>
    public class VehicleThirdPartsInfoUIModel : BaseNotificationUIModel
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

        private String _vtpiId;
        /// <summary>
        /// ID
        /// </summary>
        public String VTPI_ID
        {
            get { return _vtpiId; }
            set
            {
                _vtpiId = value;
                RaisePropertyChanged(() => VTPI_ID);
            }
        }

        private String _vtpiVcVin;
        /// <summary>
        /// 车架号
        /// </summary>
        public String VTPI_VC_VIN
        {
            get { return _vtpiVcVin; }
            set
            {
                _vtpiVcVin = value;
                RaisePropertyChanged(() => VTPI_VC_VIN);
            }
        }
        
        private String _vtpiThirdNo;
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String VTPI_ThirdNo
        {
            get { return _vtpiThirdNo; }
            set
            {
                _vtpiThirdNo = value;
                RaisePropertyChanged(() => VTPI_ThirdNo);
            }
        }

        private String _vtpiAutoPartsName;
        /// <summary>
        /// 配件名称
        /// </summary>
        public String VTPI_AutoPartsName
        {
            get { return _vtpiAutoPartsName; }
            set
            {
                _vtpiAutoPartsName = value;
                RaisePropertyChanged(() => VTPI_AutoPartsName);
            }
        }

        private String _vtpiAutoPartsBrand;
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String VTPI_AutoPartsBrand
        {
            get { return _vtpiAutoPartsBrand; }
            set
            {
                _vtpiAutoPartsBrand = value;
                RaisePropertyChanged(() => VTPI_AutoPartsBrand);
            }
        }

        private String _vtpiRemark;
        /// <summary>
        /// 备注
        /// </summary>
        public String VTPI_Remark
        {
            get { return _vtpiRemark; }
            set
            {
                _vtpiRemark = value;
                RaisePropertyChanged(() => VTPI_Remark);
            }
        }

        private Boolean? _vtpiIsValid;
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? VTPI_IsValid
        {
            get { return _vtpiIsValid; }
            set
            {
                _vtpiIsValid = value;
                RaisePropertyChanged(() => VTPI_IsValid);
            }
        }

        private String _vtpiCreatedBy;
        /// <summary>
        /// 创建人
        /// </summary>
        public String VTPI_CreatedBy
        {
            get { return _vtpiCreatedBy; }
            set
            {
                _vtpiCreatedBy = value;
                RaisePropertyChanged(() => VTPI_CreatedBy);
            }
        }

        private DateTime? _vtpiCreatedTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? VTPI_CreatedTime
        {
            get { return _vtpiCreatedTime; }
            set
            {
                _vtpiCreatedTime = value;
                RaisePropertyChanged(() => VTPI_CreatedTime);
            }
        }

        private String _vtpiUpdatedBy;
        /// <summary>
        /// 修改人
        /// </summary>
        public String VTPI_UpdatedBy
        {
            get { return _vtpiUpdatedBy; }
            set
            {
                _vtpiUpdatedBy = value;
                RaisePropertyChanged(() => VTPI_UpdatedBy);
            }
        }

        private DateTime? _vtpiUpdatedTime;
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? VTPI_UpdatedTime
        {
            get { return _vtpiUpdatedTime; }
            set
            {
                _vtpiUpdatedTime = value;
                RaisePropertyChanged(() => VTPI_UpdatedTime);
            }
        }

        private Int64? _vtpiVersionNo;
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? VTPI_VersionNo
        {
            get { return _vtpiVersionNo; }
            set
            {
                _vtpiVersionNo = value;
                RaisePropertyChanged(() => VTPI_VersionNo);
            }
        }
        #endregion

        /// <summary>
        /// ID
        /// </summary>
        public String WHERE_VTPI_ID { get; set; }

        /// <summary>
        /// 操作类别
        /// </summary>
        public String VTPI_OperateType { get; set; }
    }
}
