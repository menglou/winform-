namespace SkyCar.Coeus.Common.Const
{
    /// <summary>
    /// 格式常量
    /// </summary>
    public class FormatConst
    {
        #region 日期

        /// <summary>
        /// 日期格式01
        /// <para>yyyy/MM/dd</para>
        /// </summary>
        public static string DATE_FORMAT_01 = "yyyy/MM/dd";
        /// <summary>
        /// 日期格式02
        /// <para>yy/MM/dd</para>
        /// </summary>
        public static string DATE_FORMAT_02 = "yy/MM/dd";
        /// <summary>
        /// 日期时间格式01
        /// <para>yyyy/MM/dd HH:mm:ss</para>
        /// </summary>
        public static string DATE_TIME_FORMAT_01 = "yyyy/MM/dd HH:mm:ss";
        /// <summary>
        /// 日期时间格式02
        /// <para>yy/MM/dd HH:mm:ss</para>
        /// </summary>
        public const string DATE_TIME_FORMAT_02 = "yy/MM/dd HH:mm:ss";
        /// <summary>
        /// 日期时间格式03
        /// <para>yyyyMMddHHmmss</para>
        /// </summary>
        public static string DATE_TIME_FORMAT_03 = "yyyyMMddHHmmss";

        #endregion

        /// <summary>
        /// 金额格式
        /// </summary>
        public static string MONEY_FORMAT_01 = "#,##0.00";

        #region 文件类型筛选器
        /// <summary>
        /// 文件类型筛选器(*.xlsx,*,xls)
        /// <para>Excel 工作簿|*.xlsx|Excel 97-2003 工作簿|*.xls</para>
        /// </summary>
        public static string FILE_TYPE_FILTER_EXCEL_01 = "Excel 工作簿|*.xlsx|Excel 97-2003 工作簿|*.xls";
        /// <summary>
        /// 文件类型筛选器(*.xls)
        /// <para>Excel 97-2003 工作簿|*.xls</para>
        /// </summary>
        public static string FILE_TYPE_FILTER_EXCEL_02 = "Excel 97-2003 工作簿|*.xls";
        /// <summary>
        /// 文件类型筛选器(*.xls,*.*)
        /// <para>Excel文件(*.xls)|*.xls|All Files|*.*</para>
        /// </summary>
        public const string FILE_TYPE_FILTER_EXCEL_03 = "Excel文件(*.xls)|*.xls|All Files|*.*";
        /// <summary>
        /// 文件类型筛选器(*.xlsx,*,xls,*.*)
        /// <para>Excel 工作簿|*.xlsx|Excel 97-2003 工作簿|*.xls|All Files|*.*</para>
        /// </summary>
        public static string FILE_TYPE_FILTER_EXCEL_04 = "Excel 工作簿|*.xlsx|Excel 97-2003 工作簿|*.xls|All Files|*.*";
        #endregion
    }
}
