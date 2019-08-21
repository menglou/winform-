using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.BS
{
    /// <summary>
    /// 品牌车系UIModel
    /// </summary>
    public class VehicleBrandInspireSummaManagerUIModel : BaseUIModel
    {
        /// <summary>
        /// 品牌
        /// </summary>
        public String VBIS_Brand { get; set; }
        /// <summary>
        /// 品牌拼音首字母
        /// </summary>
        public String VBIS_BrandSpellCode { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String VBIS_Inspire { get; set; }
        /// <summary>
        /// 车系拼音首字母
        /// </summary>
        public String VBIS_InspireSpellCode { get; set; }
        /// <summary>
        /// 车辆类型
        /// </summary>
        public String VBIS_Model { get; set; }
        /// <summary>
        /// 车型描述
        /// </summary>
        public String VBIS_ModelDesc { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? VBIS_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String VBIS_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? VBIS_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String VBIS_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? VBIS_UpdatedTime { get; set; }
        /// <summary>
        /// 品牌车系ID
        /// </summary>
        public String VBIS_ID { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? VBIS_VersionNo { get; set; }

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
    }
}
