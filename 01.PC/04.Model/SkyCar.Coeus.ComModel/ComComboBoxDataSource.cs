using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.ComModel
{
    /// <summary>
    /// ComboBoxDataSource类型(Text,Value)
    /// </summary>
    public class ComComboBoxDataSource
    {
        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// 显示内容
        /// </summary>
        public string Text { get; set; }
    }
    /// <summary>
    /// ComboBoxDataSource类型(Text,Code)
    /// </summary>
    public class ComComboBoxDataSourceTC
    {
        /// <summary>
        /// Code值
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 显示内容
        /// </summary>
        public string Text { get; set; }
    }
}
