using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.BS
{
    /// <summary>
    /// 配件名称UIModel
    /// </summary>
    public class AutoPartsNameManagerUIModel : BaseUIModel
    {
        /// <summary>
        /// 配置名称ID
        /// </summary>
        public String APN_ID { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String APN_Name { get; set; }
        /// <summary>
        /// 配件别名
        /// </summary>
        public String APN_Alias { get; set; }
        /// <summary>
        /// 名称拼音简写
        /// </summary>
        public String APN_NameSpellCode { get; set; }
        /// <summary>
        /// 别名拼音简写
        /// </summary>
        public String APN_AliasSpellCode { get; set; }
        /// <summary>
        /// 配件类别ID
        /// </summary>
        public String APN_APT_ID { get; set; }
        /// <summary>
        /// 配件类别名称
        /// </summary>
        public String APT_Name { get; set; }
        /// <summary>
        /// 呆滞天数
        /// </summary>
        public Int32? APN_SlackDays { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public String APN_UOM { get; set; }
        /// <summary>
        /// 固定计量单位
        /// </summary>
        public Boolean? APN_FixUOM { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? APN_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String APN_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? APN_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String APN_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APN_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? APN_VersionNo { get; set; }

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
