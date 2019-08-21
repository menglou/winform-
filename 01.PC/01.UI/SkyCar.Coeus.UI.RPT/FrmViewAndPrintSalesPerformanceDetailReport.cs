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
    /// 打印销售业绩统计明细表
    /// </summary>
    public partial class FrmViewAndPrintSalesPerformanceDetailReport : BaseForm
    {
        #region 全局变量

        /// <summary>
        /// 打印纸张大小
        /// </summary>
        private SalesPerformanceDetailPaperSize _paperSize;
        /// <summary>
        /// 销售业绩统计明细数据
        /// </summary>
        List<SalesPerformanceDetailReportUIModel> _salesPerformanceDetailList = new List<SalesPerformanceDetailReportUIModel>();

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
        public FrmViewAndPrintSalesPerformanceDetailReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        public FrmViewAndPrintSalesPerformanceDetailReport(Dictionary<string, object> paramViewParameters)
        {
            InitializeComponent();

            _viewParameters = paramViewParameters;
        }
        /// <summary>
        /// 加载界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmViewAndPrintSalesPerformanceDetailReport_Load(object sender, EventArgs e)
        {
            if (_viewParameters != null)
            {
                #region 销售业绩统计明细

                if (_viewParameters.ContainsKey(ComViewParamKey.APModel.ToString()))
                {
                    _salesPerformanceDetailList = _viewParameters[ComViewParamKey.APModel.ToString()] as List<SalesPerformanceDetailReportUIModel>;
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
            _paperSize = SalesPerformanceDetailPaperSize.CustomerSize;
            LoadSalesPerformanceDetailReport(_paperSize);
        }
        /// <summary>
        /// A4尺寸
        /// </summary>
        private void ExecutePrintA4Size()
        {
            _paperSize = SalesPerformanceDetailPaperSize.A4Size;
            LoadSalesPerformanceDetailReport(_paperSize);
        }
        /// <summary>
        /// A5尺寸
        /// </summary>
        private void ExecutePrintA5Size()
        {
            _paperSize = SalesPerformanceDetailPaperSize.A5Size;
            LoadSalesPerformanceDetailReport(_paperSize);
        }
        /// <summary>
        /// A4尺寸充满
        /// </summary>
        private void ExecutePrintA4SizeFull()
        {
            _paperSize = SalesPerformanceDetailPaperSize.A4SizeFull;
            LoadSalesPerformanceDetailReport(_paperSize);
        }
        /// <summary>
        /// 加载页面
        /// </summary>
        /// <param name="paramBillPaperSize"></param>
        private void LoadSalesPerformanceDetailReport(SalesPerformanceDetailPaperSize paramBillPaperSize)
        {
            reportViewer1.Reset();
            string rdlcFileName = string.Empty;
            switch (paramBillPaperSize)
            {
                case SalesPerformanceDetailPaperSize.CustomerSize:
                    rdlcFileName = "RPT_ViewAndPrintSalesPerformanceDetailReport2228_" + @".rdlc";
                    break;
                case SalesPerformanceDetailPaperSize.A4Size:
                    rdlcFileName = "RPT_ViewAndPrintSalesPerformanceDetailReport_" + @".rdlc";
                    break;
                case SalesPerformanceDetailPaperSize.A5Size:
                    rdlcFileName = "RPT_ViewAndPrintSalesPerformanceDetailReportA5_" + @".rdlc";
                    break;
                case SalesPerformanceDetailPaperSize.A4SizeFull:
                    rdlcFileName = "RPT_ViewAndPrintSalesPerformanceDetailReportA4Full_" + @".rdlc";
                    break;
            }
            string reportFilePath = Application.StartupPath + @"\RdlcFiles\" + rdlcFileName;

            if (!File.Exists(reportFilePath))
            {
                switch (paramBillPaperSize)
                {
                    case SalesPerformanceDetailPaperSize.CustomerSize:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\RPT_ViewAndPrintSalesPerformanceDetailReport2228.rdlc";
                        break;
                    case SalesPerformanceDetailPaperSize.A4Size:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\RPT_ViewAndPrintSalesPerformanceDetailReport.rdlc";
                        break;
                    case SalesPerformanceDetailPaperSize.A5Size:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\RPT_ViewAndPrintSalesPerformanceDetailReportA5.rdlc";
                        break;
                    case SalesPerformanceDetailPaperSize.A4SizeFull:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\RPT_ViewAndPrintSalesPerformanceDetailReportA4Full.rdlc";
                        break;
                }
            }
            if (File.Exists(reportFilePath))
            {
                reportViewer1.LocalReport.ReportPath = reportFilePath;
                reportViewer1.LocalReport.DataSources.Clear();

                ReportDataSource rpd1 = new ReportDataSource("SalesPerformanceDetail", _salesPerformanceDetailList as object);
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
        private enum SalesPerformanceDetailPaperSize
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
