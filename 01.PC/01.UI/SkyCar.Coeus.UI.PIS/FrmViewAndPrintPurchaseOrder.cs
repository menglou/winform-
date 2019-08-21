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
    /// 打印采购订单
    /// </summary>
    public partial class FrmViewAndPrintPurchaseOrder : BaseForm
    {
        #region 全局变量

        /// <summary>
        /// 打印纸张大小
        /// </summary>
        private SettleBillPaperSize _paperSize;
        /// <summary>
        /// 采购订单数据
        /// </summary>
        PurchaseOrderUIModelToPrint _purchaseOrder = new PurchaseOrderUIModelToPrint();
        /// <summary>
        /// 采购订单明细数据
        /// </summary>
        List<PurchaseOrderDetailUIModelToPrint> _purchaseOrderDetailList = new List<PurchaseOrderDetailUIModelToPrint>();

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
        public FrmViewAndPrintPurchaseOrder()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        public FrmViewAndPrintPurchaseOrder(Dictionary<string, object> paramViewParameters)
        {
            InitializeComponent();

            _viewParameters = paramViewParameters;
        }
        /// <summary>
        /// 页面下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmViewAndPrintPurchaseOrser_Load(object sender, EventArgs e)
        {
            if (_viewParameters != null)
            {
                #region 采购订单

                if (_viewParameters.ContainsKey(PISViewParamKey.PurchaseOrder.ToString()))
                {
                    _purchaseOrder = _viewParameters[PISViewParamKey.PurchaseOrder.ToString()] as PurchaseOrderUIModelToPrint;
                }
                #endregion

                #region 采购订单明细

                if (_viewParameters.ContainsKey(PISViewParamKey.PurchaseOrderDetail.ToString()))
                {
                    _purchaseOrderDetailList =
                        _viewParameters[PISViewParamKey.PurchaseOrderDetail.ToString()] as List<PurchaseOrderDetailUIModelToPrint>;
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
            LoadPurchaseOrder(_paperSize);
        }
        /// <summary>
        /// A4尺寸
        /// </summary>
        private void ExecutePrintA4Size()
        {
            _paperSize = SettleBillPaperSize.A4Size;
            LoadPurchaseOrder(_paperSize);
        }
        /// <summary>
        /// A5尺寸
        /// </summary>
        private void ExecutePrintA5Size()
        {
            _paperSize = SettleBillPaperSize.A5Size;
            LoadPurchaseOrder(_paperSize);
        }
        /// <summary>
        /// A4尺寸充满
        /// </summary>
        private void ExecutePrintA4SizeFull()
        {
            _paperSize = SettleBillPaperSize.A4SizeFull;
            LoadPurchaseOrder(_paperSize);
        }
        /// <summary>
        /// 加载页面
        /// </summary>
        /// <param name="paramBillPaperSize"></param>
        private void LoadPurchaseOrder(SettleBillPaperSize paramBillPaperSize)
        {
            reportViewer1.Reset();
            string rdlcFileName = string.Empty;
            switch (paramBillPaperSize)
            {
                case SettleBillPaperSize.CustomerSize:
                    rdlcFileName = "PIS_ViewAndPrintPurchaseOrder2228_" + @".rdlc";
                    break;
                case SettleBillPaperSize.A4Size:
                    rdlcFileName = "PIS_ViewAndPrintPurchaseOrder_" + @".rdlc";
                    break;
                case SettleBillPaperSize.A5Size:
                    rdlcFileName = "PIS_ViewAndPrintPurchaseOrderA5_" + @".rdlc";
                    break;
                case SettleBillPaperSize.A4SizeFull:
                    rdlcFileName = "PIS_ViewAndPrintPurchaseOrderA4Full_" + @".rdlc";
                    break;
            }
            string reportFilePath = Application.StartupPath + @"\RdlcFiles\" + rdlcFileName;

            if (!File.Exists(reportFilePath))
            {
                switch (paramBillPaperSize)
                {
                    case SettleBillPaperSize.CustomerSize:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\PIS_ViewAndPrintPurchaseOrder2228.rdlc";
                        break;
                    case SettleBillPaperSize.A4Size:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\PIS_ViewAndPrintPurchaseOrder.rdlc";
                        break;
                    case SettleBillPaperSize.A5Size:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\PIS_ViewAndPrintPurchaseOrderA5.rdlc";
                        break;
                    case SettleBillPaperSize.A4SizeFull:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\PIS_ViewAndPrintPurchaseOrderA4Full.rdlc";
                        break;
                }
            }
            if (File.Exists(reportFilePath))
            {
                reportViewer1.LocalReport.ReportPath = reportFilePath;
                reportViewer1.LocalReport.DataSources.Clear();
                List<PurchaseOrderUIModelToPrint> billList = new List<PurchaseOrderUIModelToPrint> { _purchaseOrder };
                ReportDataSource rpd1 = new ReportDataSource(PISViewParamKey.PurchaseOrder.ToString(), billList as object);
                reportViewer1.LocalReport.DataSources.Add(rpd1);

                ReportDataSource rpd2 = new ReportDataSource(PISViewParamKey.PurchaseOrderDetail.ToString(), _purchaseOrderDetailList as object);
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
