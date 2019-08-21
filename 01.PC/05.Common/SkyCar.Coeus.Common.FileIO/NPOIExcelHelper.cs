using System;
using System.Data;
using System.IO;
using NPOI.SS.UserModel;

namespace SkyCar.Coeus.Common.FileIO
{
    public class CellModel
    {
        /// <summary>
        /// 在XamGrid中的顺序
        /// </summary>
        public int SourColIndex;
        /// <summary>
        /// 表头文本
        /// </summary>
        public string HeaderText;
        /// <summary>
        /// 行位置，从0开始
        /// </summary>
        public int RowIndex { get; set; }
        /// <summary>
        /// 列位置，从0开始
        /// </summary>
        public int ColumnIndex { get; set; }

        /// <summary>
        /// 跨行数，默认为1
        /// </summary>
        private int _rowSpan = 1;
        /// <summary>
        /// 跨行数，默认为1
        /// </summary>
        public int RowSpan
        {
            get { return _rowSpan; }
            set { _rowSpan = value; }
        }

        /// <summary>
        /// 跨列数，默认为1
        /// </summary>
        private int _columnSpan = 1;
        /// <summary>
        /// 跨列数，默认为1
        /// </summary>
        public int ColumnSpan
        {
            get { return _columnSpan; }
            set { _columnSpan = value; }
        }

        /// <summary>
        /// 行高
        /// </summary>
        public int RowHeight { get; set; }
        /// <summary>
        /// 列宽
        /// </summary>
        public int ColumnWidth { get; set; }

    }
    public class NPOIExcelHelper
    {
        /// <summary>
        /// 将excel文件内容读取到DataTable数据表中
        /// </summary>
        /// <param name="fileName">文件完整路径名</param>
        /// <param name="sheetName">指定读取excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名：true=是，false=否</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable ReadExcelToDataTable(string fileName, string sheetName = null, bool isFirstRowColumn = true)
        {
            //定义要返回的datatable对象
            DataTable data = new DataTable();
            //excel工作表
            NPOI.SS.UserModel.ISheet sheet = null;
            //数据开始行(排除标题行)
            int startRow = 0;
            try
            {
                if (!File.Exists(fileName))
                {
                    return null;
                }
                //根据指定路径读取文件
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                //根据文件流创建excel数据结构
                NPOI.SS.UserModel.IWorkbook workbook = NPOI.SS.UserModel.WorkbookFactory.Create(fs);
                //IWorkbook workbook = new HSSFWorkbook(fs);
                //如果有指定工作表名称
                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheet = workbook.GetSheet(sheetName);
                    //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    if (sheet == null)
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    //如果没有指定的sheetName，则尝试获取第一个sheet
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    NPOI.SS.UserModel.IRow firstRow = sheet.GetRow(0);
                    //一行最后一个cell的编号 即总的列数
                    int cellCount = firstRow.LastCellNum;
                    //如果第一行是标题列名
                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            NPOI.SS.UserModel.ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }
                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        NPOI.SS.UserModel.IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                            {
                                DateTime? dateCellValue = null;
                                if (row.GetCell(j).CellType == CellType.Numeric)
                                {
                                    try
                                    {
                                        dateCellValue = row.GetCell(j).DateCellValue;
                                    }
                                    catch
                                    {
                                        dateCellValue = null;
                                    }
                                }
                                if (dateCellValue == null)
                                {
                                    dataRow[j] = row.GetCell(j).ToString();
                                }
                                else
                                {
                                    dataRow[j] = dateCellValue.ToString();
                                }
                            }

                        }
                        data.Rows.Add(dataRow);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Excel文件是否包含指定表格
        /// </summary>
        /// <param name="paramExcelFileName">Excel文件名称</param>
        /// <param name="paramExcelSheetName">Excel表格名称</param>
        /// <returns></returns>
        public static bool ExcelExistsSheet(string paramExcelFileName, string paramExcelSheetName)
        {
            if (!File.Exists(paramExcelFileName))
            {
                return false;
            }
            if (string.IsNullOrEmpty(paramExcelSheetName))
            {
                return false;
            }
            //根据指定路径读取文件
            FileStream fs = new FileStream(paramExcelFileName, FileMode.Open, FileAccess.Read);
            //根据文件流创建excel数据结构
            IWorkbook workbook = WorkbookFactory.Create(fs);
            var sheet = workbook.GetSheet(paramExcelSheetName);
            
            if (sheet == null)
            {
                return false;
            }

            return true;
        }
    }
}
