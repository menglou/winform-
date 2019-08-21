using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.BS
{
    /// <summary>
    /// 配件类别UIModel
    /// </summary>
    public class AutoPartsTypeManagerUIModel : BaseUIModel
    {
        /// <summary>
        /// 配件类别名称
        /// </summary>
        public String APT_Name { get; set; }
        /// <summary>
        /// 父级类别ID
        /// </summary>
        public String APT_ParentID { get; set; }
        /// <summary>
        /// 父级类别名称
        /// </summary>
        public String APT_ParentName { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? APT_Index { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? APT_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String APT_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? APT_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String APT_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APT_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? APT_VersionNo { get; set; }
        /// <summary>
        /// 配件类别ID
        /// </summary>
        public String APT_ID { get; set; }

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
