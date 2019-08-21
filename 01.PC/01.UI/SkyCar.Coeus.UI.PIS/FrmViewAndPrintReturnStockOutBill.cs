using SkyCar.Framework.WindowUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.UIModel.PIS;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using Microsoft.Reporting.WinForms;
using SkyCar.Coeus.BLL.Common;

namespace SkyCar.Coeus.UI.PIS
{
    /// <summary>
    /// 打印退货出库单
    /// </summary>
    public partial class FrmViewAndPrintReturnStockOutBill : BaseForm
    {
        #region 全局变量

        /// <summary>
        /// 打印纸张大小
        /// </summary>
        private BillPaperSize _paperSize;
        /// <summary>
        /// 出库单数据
        /// </summary>
        StockOutBillUIModelToPrint _stockOutBill = new StockOutBillUIModelToPrint();
        /// <summary>
        /// 出库单明细数据
        /// </summary>
        List<StockOutBillDetailUIModelToPrint> _stockOutBillDetailList = new List<StockOutBillDetailUIModelToPrint>();

        /// <summary>
        /// 界面参数
        /// </summary>
        private Dictionary<string, object> _viewParameters;

        #endregion

        #region 系统事件

        /// <summary>
        /// 构造方法
        /// </summary>
        public FrmViewAndPrintReturnStockOutBill()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        public FrmViewAndPrintReturnStockOutBill(Dictionary<string, object> paramViewParameters)
        {
            InitializeComponent();

            _viewParameters = paramViewParameters;
        }

        /// <summary>
        /// 页面下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmViewAndPrintReturnStockOutBill_Load(object sender, EventArgs e)
        {
            if (_viewParameters != null)
            {
                #region 出库单

                if (_viewParameters.ContainsKey(PISViewParamKey.StockOutBill.ToString()))
                {
                    _stockOutBill = _viewParameters[PISViewParamKey.StockOutBill.ToString()] as StockOutBillUIModelToPrint;
                }
                #endregion

                #region 出库单明细

                if (_viewParameters.ContainsKey(PISViewParamKey.StockOutBillDetail.ToString()))
                {
                    _stockOutBillDetailList = _viewParameters[PISViewParamKey.StockOutBillDetail.ToString()] as List<StockOutBillDetailUIModelToPrint>;
                }
                #endregion
            }
            else
            {
                MessageBox.Show(@"请传入参数ViewParameters");
                return;
            }
            ExecutePrintA4Size();
            
        }

        #endregion

        #region 自定义方法
        /// <summary>
        /// 客户尺寸
        /// </summary>
        private void ExecutePrintCustomerSize()
        {
            _paperSize = BillPaperSize.CustomerSize;
            LoadReturnStockOutBill(_paperSize);
        }
        /// <summary>
        /// A4尺寸
        /// </summary>
        private void ExecutePrintA4Size()
        {
            _paperSize = BillPaperSize.A4Size;
            LoadReturnStockOutBill(_paperSize);
        }
        /// <summary>
        /// A5尺寸
        /// </summary>
        private void ExecutePrintA5Size()
        {
            _paperSize = BillPaperSize.A5Size;
            LoadReturnStockOutBill(_paperSize);
        }
        /// <summary>
        /// A4尺寸充满
        /// </summary>
        private void ExecutePrintA4SizeFull()
        {
            _paperSize = BillPaperSize.A4SizeFull;
            LoadReturnStockOutBill(_paperSize);
        }
        /// <summary>
        /// 加载页面
        /// </summary>
        /// <param name="paramBillPaperSize"></param>
        private void LoadReturnStockOutBill(BillPaperSize paramBillPaperSize)
        {
            reportViewer1.Reset();
            string rdlcFileName = string.Empty;
            switch (paramBillPaperSize)
            {
                case BillPaperSize.CustomerSize:
                    rdlcFileName = "PIS_ViewAndPrintReturnStockOutBill2228_" + @".rdlc";
                    break;
                case BillPaperSize.A4Size:
                    rdlcFileName = "PIS_ViewAndPrintReturnStockOutBill_" + @".rdlc";
                    break;
                case BillPaperSize.A5Size:
                    rdlcFileName = "PIS_ViewAndPrintReturnStockOutBillA5_" + @".rdlc";
                    break;
                case BillPaperSize.A4SizeFull:
                    rdlcFileName = "PIS_ViewAndPrintReturnStockOutBillA4Full_" + @".rdlc";
                    break;
            }
            string reportFilePath = Application.StartupPath + @"\RdlcFiles\" + rdlcFileName;

            if (!File.Exists(reportFilePath))
            {
                switch (paramBillPaperSize)
                {
                    case BillPaperSize.CustomerSize:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\PIS_ViewAndPrintReturnStockOutBill2228.rdlc";
                        break;
                    case BillPaperSize.A4Size:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\PIS_ViewAndPrintReturnStockOutBill.rdlc";
                        break;
                    case BillPaperSize.A5Size:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\PIS_ViewAndPrintReturnStockOutBillA5.rdlc";
                        break;
                    case BillPaperSize.A4SizeFull:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\PIS_ViewAndPrintReturnStockOutBillA4Full.rdlc";
                        break;
                }
            }
            if (File.Exists(reportFilePath))
            {
                reportViewer1.LocalReport.ReportPath = reportFilePath;
                reportViewer1.LocalReport.DataSources.Clear();
                List<StockOutBillUIModelToPrint> billList = new List<StockOutBillUIModelToPrint> { _stockOutBill };
                ReportDataSource rpd1 = new ReportDataSource(PISViewParamKey.StockOutBill.ToString(), billList as object);
                reportViewer1.LocalReport.DataSources.Add(rpd1);
                ReportDataSource rpd2 = new ReportDataSource(PISViewParamKey.StockOutBillDetail.ToString(), _stockOutBillDetailList as object);
                reportViewer1.LocalReport.DataSources.Add(rpd2);
                ReportParameter rp1 = new ReportParameter("Org_Name", LoginInfoDAX.User_PrintTitlePrefix);
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1 });
                
                //设置打印布局模式,显示物理页面大小
                reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                //缩放模式为百分比,以100%方式显示
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;
                reportViewer1.RefreshReport();
                reportViewer1.Refresh();
            }
            else
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0010, new object[]
                             { SystemActionEnum.Name.PRINT}), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        #endregion

        #region 枚举

        /// <summary>
        /// 打印纸张大小
        /// </summary>
        private enum BillPaperSize
        {
            /// <summary>
            /// 22cm*28cm打印
            /// </summary>
            CustomerSize,
            /// <summary>
            /// A4纸打印
            /// </summary>
            A4Size,
            /// <summary>
            /// A5纸打印
            /// </summary>
            A5Size,
            /// <summary>
            /// A4纸打印完整版
            /// </summary>
            A4SizeFull
        }

        #endregion
    }
}
