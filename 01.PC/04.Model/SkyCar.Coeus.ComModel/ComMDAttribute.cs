using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.ComModel
{
    /// <summary>
    /// Model属性类
    /// </summary>
    public class ComMDAttribute
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 属性类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public object Value { get; set; }
    }
}
