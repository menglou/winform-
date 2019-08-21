using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.Common.Enums
{
    public class CustomEnums
    {
        #region 公用
        /// <summary>
        /// 选择模式
        /// </summary>
        public enum CustomeSelectionMode
        {
            /// <summary>
            /// 单选
            /// </summary>
            Single,
            /// <summary>
            /// 多选
            /// </summary>
            Multiple
        }

        /// <summary>
        /// 列边框颜色
        /// </summary>
        public class ColumnBorderColor
        {
            /// <summary>
            /// 可编辑的颜色
            /// </summary>
            public static Color EditableColor = Color.DodgerBlue;
            /// <summary>
            /// 不可编辑的颜色
            /// </summary>
            public static Color NoEditableColor = Color.LightGray;
        }

        /// <summary>
        /// 单元格背景色
        /// </summary>
        public class CellBackColor
        {
            /// <summary>
            /// 可编辑的颜色
            /// </summary>
            public static Color EditableColor = Color.White;
            /// <summary>
            /// 不可编辑的颜色
            /// </summary>
            public static Color NoEditableColor = Color.LightGray;
        }
        #endregion
    }
}
