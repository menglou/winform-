using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.BS
{
    /// <summary>
    /// 码表管理UIModel
    /// </summary>
    public class CodeTableManagerUIModel : BaseUIModel
    {
        /// <summary>
        /// 类型
        /// </summary>
        public String CT_Type { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public String CT_Name { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public String CT_Value { get; set; }
        /// <summary>
        /// 参数描述
        /// </summary>
        public String CT_Desc { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? CT_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String CT_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CT_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String CT_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? CT_UpdatedTime { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public String CT_ID { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? CT_VersionNo { get; set; }

        #region 其他属性

        /// <summary>
        /// 枚举显示名称
        /// </summary>
        public String Enum_DisplayName { get; set; }

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
        #endregion
    }
}
