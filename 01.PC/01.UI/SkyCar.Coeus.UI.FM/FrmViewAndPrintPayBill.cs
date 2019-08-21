using SkyCar.Framework.WindowUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.UIModel.FM;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using Microsoft.Reporting.WinForms;
using SkyCar.Coeus.BLL.Common;

namespace SkyCar.Coeus.UI.FM
{
    public partial class FrmViewAndPrintPayBill : BaseForm
    {
        #region 全局变量

        /// <summary>
        /// 打印纸张大小
        /// </summary>
        private SettleBillPaperSize _paperSize;
        /// <summary>
        /// 付款单数据
        /// </summary>
        PayBillUIModelToPrint _payBill = new PayBillUIModelToPrint();
        /// <summary>
        /// 付款单明细数据
        /// </summary>
        List<PayBillDetailUIModelToPrint> _payBillDetailList = new List<PayBillDetailUIModelToPrint>();
        #endregion

        #region 公共属性

        /// <summary>
        /// 界面参数
        /// </summary>
        private Dictionary<string, object> _viewParameters;

        #endregion

        #region 系统事件

        /// <summary>
        /// 构造方法
        /// </summary>
        public FrmViewAndPrintPayBill()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        public FrmViewAndPrintPayBill(Dictionary<string, object> paramViewParameters)
        {
            InitializeComponent();

            _viewParameters = paramViewParameters;
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmViewAndPrintPayBill_Load(object sender, EventArgs e)
        {
            if (_viewParameters != null)
            {
                #region 付款单

                if (_viewParameters.ContainsKey(FMViewParamKey.PayBill.ToString()))
                {
                    _payBill = _viewParameters[FMViewParamKey.PayBill.ToString()] as PayBillUIModelToPrint;
                }
                #endregion

                #region 付款单明细

                if (_viewParameters.ContainsKey(FMViewParamKey.PayBillDetail.ToString()))
                {
                    _payBillDetailList = _viewParameters[FMViewParamKey.PayBillDetail.ToString()] as List<PayBillDetailUIModelToPrint>;
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
            LoadSettleBill(_paperSize);
        }
        /// <summary>
        /// A4尺寸
        /// </summary>
        private void ExecutePrintA4Size()
        {
            _paperSize = SettleBillPaperSize.A4Size;
            LoadSettleBill(_paperSize);
        }
        /// <summary>
        /// A5尺寸
        /// </summary>
        private void ExecutePrintA5Size()
        {
            _paperSize = SettleBillPaperSize.A5Size;
            LoadSettleBill(_paperSize);
        }
        /// <summary>
        /// A4尺寸充满
        /// </summary>
        private void ExecutePrintA4SizeFull()
        {
            _paperSize = SettleBillPaperSize.A4SizeFull;
            LoadSettleBill(_paperSize);
        }
        /// <summary>
        /// 加载页面
        /// </summary>
        /// <param name="paramBillPaperSize"></param>
        private void LoadSettleBill(SettleBillPaperSize paramBillPaperSize)
        {
            reportViewer1.Reset();
            string rdlcFileName = string.Empty;
            switch (paramBillPaperSize)
            {
                case SettleBillPaperSize.CustomerSize:
                    rdlcFileName = "FM_ViewAndPrintPayBill2228_" + @".rdlc";
                    break;
                case SettleBillPaperSize.A4Size:
                    rdlcFileName = "FM_ViewAndPrintPayBill_" + @".rdlc";
                    break;
                case SettleBillPaperSize.A5Size:
                    rdlcFileName = "FM_ViewAndPrintPayBillA5_" + @".rdlc";
                    break;
                case SettleBillPaperSize.A4SizeFull:
                    rdlcFileName = "FM_ViewAndPrintPayBillA4Full_" + @".rdlc";
                    break;
            }
            string reportFilePath = Application.StartupPath + @"\RdlcFiles\" + rdlcFileName;

            if (!File.Exists(reportFilePath))
            {
                switch (paramBillPaperSize)
                {
                    case SettleBillPaperSize.CustomerSize:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\FM_ViewAndPrintPayBill2228.rdlc";
                        break;
                    case SettleBillPaperSize.A4Size:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\FM_ViewAndPrintPayBill.rdlc";
                        break;
                    case SettleBillPaperSize.A5Size:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\FM_ViewAndPrintPayBillA5.rdlc";
                        break;
                    case SettleBillPaperSize.A4SizeFull:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\FM_ViewAndPrintPayBillA4Full.rdlc";
                        break;
                }
            }
            if (File.Exists(reportFilePath))
            {
                reportViewer1.LocalReport.ReportPath = reportFilePath;
                reportViewer1.LocalReport.DataSources.Clear();
                List<PayBillUIModelToPrint> payBillList = new List<PayBillUIModelToPrint> { _payBill };
                ReportDataSource rpd1 = new ReportDataSource(FMViewParamKey.PayBill.ToString(), payBillList as object);
                reportViewer1.LocalReport.DataSources.Add(rpd1);
                ReportDataSource rpd2 = new ReportDataSource(FMViewParamKey.PayBillDetail.ToString(), _payBillDetailList as object);
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
