using SkyCar.Framework.WindowUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.UIModel.RPT;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using Microsoft.Reporting.WinForms;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.UIModel.RPT.UIModel;

namespace SkyCar.Coeus.UI.RPT
{
    /// <summary>
    /// 打印出库明细表
    /// </summary>
    public partial class FrmViewAndPrintStockOutDetailReport : BaseForm
    {
        #region 全局变量

        /// <summary>
        /// 打印纸张大小
        /// </summary>
        private SettleBillPaperSize _paperSize;
        /// <summary>
        /// 出库明细数据
        /// </summary>
        List<AutoPartsStockOutDetailReportUIModel> _stockOutDetailList = new List<AutoPartsStockOutDetailReportUIModel>();

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
        public FrmViewAndPrintStockOutDetailReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        public FrmViewAndPrintStockOutDetailReport(Dictionary<string, object> paramViewParameters)
        {
            InitializeComponent();

            _viewParameters = paramViewParameters;
        }
        /// <summary>
        /// 加载界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmViewAndPrintStockOutDetailReport_Load(object sender, EventArgs e)
        {
            if (_viewParameters != null)
            {
                #region 出库明细

                if (_viewParameters.ContainsKey(ComViewParamKey.APModel.ToString()))
                {
                    _stockOutDetailList = _viewParameters[ComViewParamKey.APModel.ToString()] as List<AutoPartsStockOutDetailReportUIModel>;
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
            _paperSize = SettleBillPaperSize.CustomerSize;
            LoadStockOutDetailReport(_paperSize);
        }
        /// <summary>
        /// A4尺寸
        /// </summary>
        private void ExecutePrintA4Size()
        {
            _paperSize = SettleBillPaperSize.A4Size;
            LoadStockOutDetailReport(_paperSize);
        }
        /// <summary>
        /// A5尺寸
        /// </summary>
        private void ExecutePrintA5Size()
        {
            _paperSize = SettleBillPaperSize.A5Size;
            LoadStockOutDetailReport(_paperSize);
        }
        /// <summary>
        /// A4尺寸充满
        /// </summary>
        private void ExecutePrintA4SizeFull()
        {
            _paperSize = SettleBillPaperSize.A4SizeFull;
            LoadStockOutDetailReport(_paperSize);
        }
        /// <summary>
        /// 加载页面
        /// </summary>
        /// <param name="paramBillPaperSize"></param>
        private void LoadStockOutDetailReport(SettleBillPaperSize paramBillPaperSize)
        {
            reportViewer1.Reset();
            string rdlcFileName = string.Empty;
            switch (paramBillPaperSize)
            {
                case SettleBillPaperSize.CustomerSize:
                    rdlcFileName = "RPT_ViewAndPrintStockOutDetailReport2228_" + @".rdlc";
                    break;
                case SettleBillPaperSize.A4Size:
                    rdlcFileName = "RPT_ViewAndPrintStockOutDetailReport_" + @".rdlc";
                    break;
                case SettleBillPaperSize.A5Size:
                    rdlcFileName = "RPT_ViewAndPrintStockOutDetailReportA5_" + @".rdlc";
                    break;
                case SettleBillPaperSize.A4SizeFull:
                    rdlcFileName = "RPT_ViewAndPrintStockOutDetailReportA4Full_" + @".rdlc";
                    break;
            }
            string reportFilePath = Application.StartupPath + @"\RdlcFiles\" + rdlcFileName;

            if (!File.Exists(reportFilePath))
            {
                switch (paramBillPaperSize)
                {
                    case SettleBillPaperSize.CustomerSize:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\RPT_ViewAndPrintStockOutDetailReport2228.rdlc";
                        break;
                    case SettleBillPaperSize.A4Size:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\RPT_ViewAndPrintStockOutDetailReport.rdlc";
                        break;
                    case SettleBillPaperSize.A5Size:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\RPT_ViewAndPrintStockOutDetailReportA5.rdlc";
                        break;
                    case SettleBillPaperSize.A4SizeFull:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\RPT_ViewAndPrintStockOutDetailReportA4Full.rdlc";
                        break;
                }
            }
            if (File.Exists(reportFilePath))
            {
                reportViewer1.LocalReport.ReportPath = reportFilePath;
                reportViewer1.LocalReport.DataSources.Clear();

                ReportDataSource rpd1 = new ReportDataSource("StockOutDetail", _stockOutDetailList as object);
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
        private enum SettleBillPaperSize
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
