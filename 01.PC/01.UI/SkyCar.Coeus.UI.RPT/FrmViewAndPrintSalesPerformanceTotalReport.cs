using SkyCar.Framework.WindowUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using Microsoft.Reporting.WinForms;
using SkyCar.Coeus.UIModel.RPT.UIModel;

namespace SkyCar.Coeus.UI.RPT
{
    /// <summary>
    /// 打印销售业绩统计汇总表
    /// </summary>
    public partial class FrmViewAndPrintSalesPerformanceTotalReport : BaseForm
    {
        #region 全局变量

        /// <summary>
        /// 打印纸张大小
        /// </summary>
        private SalesPerformanceTotalPaperSize _totalPaperSize;
        /// <summary>
        /// 销售业绩统计汇总数据
        /// </summary>
        List<SalesPerformanceTotalReportUIModel> _salesPerformanceTotalList = new List<SalesPerformanceTotalReportUIModel>();

        /// <summary>
        /// 界面参数
        /// </summary>
        private Dictionary<string, object> _viewParameters;

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// 构造方法
        /// </summary>
        public FrmViewAndPrintSalesPerformanceTotalReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        public FrmViewAndPrintSalesPerformanceTotalReport(Dictionary<string, object> paramViewParameters)
        {
            InitializeComponent();

            _viewParameters = paramViewParameters;
        }
        /// <summary>
        /// 加载界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmViewAndPrintSalesPerformanceTotalReport_Load(object sender, EventArgs e)
        {
            if (_viewParameters != null)
            {
                #region 销售业绩统计汇总

                if (_viewParameters.ContainsKey(ComViewParamKey.APModel.ToString()))
                {
                    _salesPerformanceTotalList = _viewParameters[ComViewParamKey.APModel.ToString()] as List<SalesPerformanceTotalReportUIModel>;
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
            _totalPaperSize = SalesPerformanceTotalPaperSize.CustomerSize;
            LoadSalesPerformanceTotalReport(_totalPaperSize);
        }
        /// <summary>
        /// A4尺寸
        /// </summary>
        private void ExecutePrintA4Size()
        {
            _totalPaperSize = SalesPerformanceTotalPaperSize.A4Size;
            LoadSalesPerformanceTotalReport(_totalPaperSize);
        }
        /// <summary>
        /// A5尺寸
        /// </summary>
        private void ExecutePrintA5Size()
        {
            _totalPaperSize = SalesPerformanceTotalPaperSize.A5Size;
            LoadSalesPerformanceTotalReport(_totalPaperSize);
        }
        /// <summary>
        /// A4尺寸充满
        /// </summary>
        private void ExecutePrintA4SizeFull()
        {
            _totalPaperSize = SalesPerformanceTotalPaperSize.A4SizeFull;
            LoadSalesPerformanceTotalReport(_totalPaperSize);
        }
        /// <summary>
        /// 加载页面
        /// </summary>
        /// <param name="paramBillTotalPaperSize"></param>
        private void LoadSalesPerformanceTotalReport(SalesPerformanceTotalPaperSize paramBillTotalPaperSize)
        {
            reportViewer1.Reset();
            string rdlcFileName = string.Empty;
            switch (paramBillTotalPaperSize)
            {
                case SalesPerformanceTotalPaperSize.CustomerSize:
                    rdlcFileName = "RPT_ViewAndPrintSalesPerformanceTotalReport2228_" + @".rdlc";
                    break;
                case SalesPerformanceTotalPaperSize.A4Size:
                    rdlcFileName = "RPT_ViewAndPrintSalesPerformanceTotalReport_" + @".rdlc";
                    break;
                case SalesPerformanceTotalPaperSize.A5Size:
                    rdlcFileName = "RPT_ViewAndPrintSalesPerformanceTotalReportA5_" + @".rdlc";
                    break;
                case SalesPerformanceTotalPaperSize.A4SizeFull:
                    rdlcFileName = "RPT_ViewAndPrintSalesPerformanceTotalReportA4Full_" + @".rdlc";
                    break;
            }
            string reportFilePath = Application.StartupPath + @"\RdlcFiles\" + rdlcFileName;

            if (!File.Exists(reportFilePath))
            {
                switch (paramBillTotalPaperSize)
                {
                    case SalesPerformanceTotalPaperSize.CustomerSize:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\RPT_ViewAndPrintSalesPerformanceTotalReport2228.rdlc";
                        break;
                    case SalesPerformanceTotalPaperSize.A4Size:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\RPT_ViewAndPrintSalesPerformanceTotalReport.rdlc";
                        break;
                    case SalesPerformanceTotalPaperSize.A5Size:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\RPT_ViewAndPrintSalesPerformanceTotalReportA5.rdlc";
                        break;
                    case SalesPerformanceTotalPaperSize.A4SizeFull:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\RPT_ViewAndPrintSalesPerformanceTotalReportA4Full.rdlc";
                        break;
                }
            }
            if (File.Exists(reportFilePath))
            {
                reportViewer1.LocalReport.ReportPath = reportFilePath;
                reportViewer1.LocalReport.DataSources.Clear();

                ReportDataSource rpd1 = new ReportDataSource("SalesPerformanceTotal", _salesPerformanceTotalList as object);
                reportViewer1.LocalReport.DataSources.Add(rpd1);

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
        private enum SalesPerformanceTotalPaperSize
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
