using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.BS.UIModel
{
    /// <summary>
    /// 车型配件匹配管理UIModel
    /// </summary>
    public class VehicleModelMatchAutoPartsManagerUIModel : BaseUIModel
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String VC_ID { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        public String VC_VIN { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public String VC_PlateNumber { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public String VC_Brand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String VC_Inspire { get; set; }
        /// <summary>
        /// 车型描述
        /// </summary>
        public String VC_BrandDesc { get; set; }
        /// <summary>
        /// 排量
        /// </summary>
        public String VC_Capacity { get; set; }
        /// <summary>
        /// 发动机型号
        /// </summary>
        public String VC_EngineType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String VC_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? VC_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String VC_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? VC_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String VC_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? VC_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? VC_VersionNo { get; set; }
        #endregion

        #region 其他属性
        
        private bool _isChecked = false;
        /// <summary>
        /// 选择
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    PropertyValueChanged = true;
                }
                _isChecked = value;
            }
        }

        /// <summary>
        /// 操作类别
        /// </summary>
        public String VC_OperateType { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public String WHERE_VC_ID { get; set; }
        #endregion
    }
}
