using SkyCar.Framework.WindowUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.UIModel.FM;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using Microsoft.Reporting.WinForms;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Const;

namespace SkyCar.Coeus.UI.FM
{
    public partial class FrmViewAndPrintReceiptBill : BaseForm
    {
        #region 全局变量

        /// <summary>
        /// 打印纸张大小
        /// </summary>
        private SettleBillPaperSize _paperSize;
        /// <summary>
        /// 收款单数据
        /// </summary>
        ReceiptBillUIModelToPrint _receiptBill = new ReceiptBillUIModelToPrint();
        /// <summary>
        /// 收款单明细数据
        /// </summary>
        List<ReceiptBillDetailUIModelToPtint> _receiptBillDetailList = new List<ReceiptBillDetailUIModelToPtint>();
        #endregion

        #region 公共属性

        /// <summary>
        /// 界面参数
        /// </summary>
        private Dictionary<string, object> _viewParameters;

        #endregion

        #region 系统方法

        /// <summary>
        /// 构造方法
        /// </summary>
        public FrmViewAndPrintReceiptBill()
        {
            InitializeComponent();

        }
        /// <summary>
        /// 构造方法
        /// </summary>
        public FrmViewAndPrintReceiptBill(Dictionary<string, object> paramViewParameters)
        {
            InitializeComponent();

            _viewParameters = paramViewParameters;
        }
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmViewAndPrintReceiptBill_Load(object sender, EventArgs e)
        {
            if (_viewParameters != null)
            {
                #region 收款单

                if (_viewParameters.ContainsKey(FMViewParamKey.ReceiptBill.ToString()))
                {
                    _receiptBill = _viewParameters[FMViewParamKey.ReceiptBill.ToString()] as ReceiptBillUIModelToPrint;
                }
                #endregion

                #region 收款单明细

                if (_viewParameters.ContainsKey(FMViewParamKey.ReceiptBillDetail.ToString()))
                {
                    _receiptBillDetailList = _viewParameters[FMViewParamKey.ReceiptBillDetail.ToString()] as List<ReceiptBillDetailUIModelToPtint>;
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
                    rdlcFileName = "FM_ViewAndPrintReceiptBill2228_" + @".rdlc";
                    break;
                case SettleBillPaperSize.A4Size:
                    rdlcFileName = "FM_ViewAndPrintReceiptBill_" + @".rdlc";
                    break;
                case SettleBillPaperSize.A5Size:
                    rdlcFileName = "FM_ViewAndPrintReceiptBillA5_" + @".rdlc";
                    break;
                case SettleBillPaperSize.A4SizeFull:
                    rdlcFileName = "FM_ViewAndPrintReceiptBillA4Full_" + @".rdlc";
                    break;
            }
            string reportFilePath = Application.StartupPath + @"\RdlcFiles\" + rdlcFileName;

            if (!File.Exists(reportFilePath))
            {
                switch (paramBillPaperSize)
                {
                    case SettleBillPaperSize.CustomerSize:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\FM_ViewAndPrintReceiptBill2228.rdlc";
                        break;
                    case SettleBillPaperSize.A4Size:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\FM_ViewAndPrintReceiptBill.rdlc";
                        break;
                    case SettleBillPaperSize.A5Size:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\FM_ViewAndPrintReceiptBillA5.rdlc";
                        break;
                    case SettleBillPaperSize.A4SizeFull:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\FM_ViewAndPrintReceiptBillA4Full.rdlc";
                        break;
                }
            }
            if (File.Exists(reportFilePath))
            {
                reportViewer1.LocalReport.ReportPath = reportFilePath;
                reportViewer1.LocalReport.DataSources.Clear();
                List<ReceiptBillUIModelToPrint> receiptBillList = new List<ReceiptBillUIModelToPrint> { _receiptBill };
                ReportDataSource rpd1 = new ReportDataSource(FMViewParamKey.ReceiptBill.ToString(), receiptBillList as object);
                reportViewer1.LocalReport.DataSources.Add(rpd1);
                ReportDataSource rpd2 = new ReportDataSource(FMViewParamKey.ReceiptBillDetail.ToString(), _receiptBillDetailList as object);
                reportViewer1.LocalReport.DataSources.Add(rpd2);
                ReportParameter rp1 = new ReportParameter("Org_Name", LoginInfoDAX.User_PrintTitlePrefix);

                reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1 });

                reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;
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
