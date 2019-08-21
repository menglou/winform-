using SkyCar.Framework.WindowUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.UIModel.SD;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using Microsoft.Reporting.WinForms;
using SkyCar.Coeus.BLL.Common;

namespace SkyCar.Coeus.UI.SD
{
    /// <summary>
    /// 打印物流单
    /// </summary>
    public partial class FrmViewAndPrintLogisticsBill : BaseForm
    {
        #region 全局变量

        /// <summary>
        /// 打印纸张大小
        /// </summary>
        private BillPaperSize _paperSize;
        /// <summary>
        /// 物流单数据
        /// </summary>
        LogisticsBillUIModelToPrint _logisticsBill = new LogisticsBillUIModelToPrint();
        /// <summary>
        /// 物流单明细数据
        /// </summary>
        List<LogisticsBillDetailUIModelToPrint> _logisticsBillDetailList = new List<LogisticsBillDetailUIModelToPrint>();

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
        public FrmViewAndPrintLogisticsBill()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        public FrmViewAndPrintLogisticsBill(Dictionary<string, object> paramViewParameters)
        {
            InitializeComponent();

            _viewParameters = paramViewParameters;
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmViewAndPrintLogisticsBill_Load(object sender, EventArgs e)
        {
            if (_viewParameters != null)
            {
                #region 物流单

                if (_viewParameters.ContainsKey(SDViewParamKey.LogisticsBill.ToString()))
                {
                    _logisticsBill = _viewParameters[SDViewParamKey.LogisticsBill.ToString()] as LogisticsBillUIModelToPrint;
                }
                #endregion

                #region 物流单明细

                if (_viewParameters.ContainsKey(SDViewParamKey.LogisticsBillDetail.ToString()))
                {
                    _logisticsBillDetailList = _viewParameters[SDViewParamKey.LogisticsBillDetail.ToString()] as List<LogisticsBillDetailUIModelToPrint>;
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
            LoadLogisticsBill(_paperSize);
        }
        /// <summary>
        /// A4尺寸
        /// </summary>
        private void ExecutePrintA4Size()
        {
            _paperSize = BillPaperSize.A4Size;
            LoadLogisticsBill(_paperSize);
        }
        /// <summary>
        /// A5尺寸
        /// </summary>
        private void ExecutePrintA5Size()
        {
            _paperSize = BillPaperSize.A5Size;
            LoadLogisticsBill(_paperSize);
        }
        /// <summary>
        /// A4尺寸充满
        /// </summary>
        private void ExecutePrintA4SizeFull()
        {
            _paperSize = BillPaperSize.A4SizeFull;
            LoadLogisticsBill(_paperSize);
        }
        /// <summary>
        /// 加载页面
        /// </summary>
        /// <param name="paramBillPaperSize"></param>
        private void LoadLogisticsBill(BillPaperSize paramBillPaperSize)
        {
            reportViewer1.Reset();
            string rdlcFileName = string.Empty;
            switch (paramBillPaperSize)
            {
                case BillPaperSize.CustomerSize:
                    rdlcFileName = "SD_ViewAndPrintLogisticsBill2228_" + @".rdlc";
                    break;
                case BillPaperSize.A4Size:
                    rdlcFileName = "SD_ViewAndPrintLogisticsBill_" + @".rdlc";
                    break;
                case BillPaperSize.A5Size:
                    rdlcFileName = "SD_ViewAndPrintLogisticsBillA5_" + @".rdlc";
                    break;
                case BillPaperSize.A4SizeFull:
                    rdlcFileName = "SD_ViewAndPrintLogisticsBillA4Full_" + @".rdlc";
                    break;
            }
            string reportFilePath = Application.StartupPath + @"\RdlcFiles\" + rdlcFileName;

            if (!File.Exists(reportFilePath))
            {
                switch (paramBillPaperSize)
                {
                    case BillPaperSize.CustomerSize:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\SD_ViewAndPrintLogisticsBill2228.rdlc";
                        break;
                    case BillPaperSize.A4Size:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\SD_ViewAndPrintLogisticsBill.rdlc";
                        break;
                    case BillPaperSize.A5Size:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\SD_ViewAndPrintLogisticsBillA5.rdlc";
                        break;
                    case BillPaperSize.A4SizeFull:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\SD_ViewAndPrintLogisticsBillA4Full.rdlc";
                        break;
                }
            }
            if (File.Exists(reportFilePath))
            {
                reportViewer1.LocalReport.ReportPath = reportFilePath;
                reportViewer1.LocalReport.DataSources.Clear();
                List<LogisticsBillUIModelToPrint> billList = new List<LogisticsBillUIModelToPrint> { _logisticsBill };
                ReportDataSource rpd1 = new ReportDataSource(SDViewParamKey.LogisticsBill.ToString(), billList as object);
                reportViewer1.LocalReport.DataSources.Add(rpd1);

                ReportDataSource rpd2 = new ReportDataSource(SDViewParamKey.LogisticsBillDetail.ToString(), _logisticsBillDetailList as object);
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
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0010, new object[]
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
