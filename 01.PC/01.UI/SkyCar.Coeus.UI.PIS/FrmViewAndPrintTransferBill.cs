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
using SkyCar.Coeus.BLL.PIS;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.TBModel;

namespace SkyCar.Coeus.UI.PIS
{
    /// <summary>
    /// 打印调拨单
    /// </summary>
    public partial class FrmViewAndPrintTransferBill : BaseForm
    {
        #region 全局变量

        /// <summary>
        /// 调拨管理BLL
        /// </summary>
        private TransferBillManagerBLL _bll = new TransferBillManagerBLL();

        /// <summary>
        /// 打印纸张大小
        /// </summary>
        private BillPaperSize _paperSize;
        /// <summary>
        /// 调拨管理数据
        /// </summary>
        TransferBillUIModelToPrint _transferBill = new TransferBillUIModelToPrint();
        /// <summary>
        /// 调拨管理明细数据
        /// </summary>
        List<TransferBillDetailUIModelToPrint> _transferBillDetailList = new List<TransferBillDetailUIModelToPrint>();

        /// <summary>
        /// 界面参数
        /// </summary>
        private Dictionary<string, object> _viewParameters;

        #endregion

        #region 系统事件

        /// <summary>
        /// 构造方法
        /// </summary>
        public FrmViewAndPrintTransferBill()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        public FrmViewAndPrintTransferBill(Dictionary<string, object> paramViewParameters)
        {
            InitializeComponent();

            _viewParameters = paramViewParameters;
        }

        /// <summary>
        /// 页面下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmViewAndPrintTransferBill_Load(object sender, EventArgs e)
        {
            if (_viewParameters != null)
            {
                #region 调拨单

                if (_viewParameters.ContainsKey(PISViewParamKey.TransferBill.ToString()))
                {
                    _transferBill = _viewParameters[PISViewParamKey.TransferBill.ToString()] as TransferBillUIModelToPrint;
                }
                #endregion

                #region 调拨单明细

                if (_viewParameters.ContainsKey(PISViewParamKey.TransferBillDetail.ToString()))
                {
                    _transferBillDetailList = _viewParameters[PISViewParamKey.TransferBillDetail.ToString()] as List<TransferBillDetailUIModelToPrint>;
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
            LoadTransferBill(_paperSize);
        }
        /// <summary>
        /// A4尺寸
        /// </summary>
        private void ExecutePrintA4Size()
        {
            _paperSize = BillPaperSize.A4Size;
            LoadTransferBill(_paperSize);
        }
        /// <summary>
        /// A5尺寸
        /// </summary>
        private void ExecutePrintA5Size()
        {
            _paperSize = BillPaperSize.A5Size;
            LoadTransferBill(_paperSize);
        }
        /// <summary>
        /// A4尺寸充满
        /// </summary>
        private void ExecutePrintA4SizeFull()
        {
            _paperSize = BillPaperSize.A4SizeFull;
            LoadTransferBill(_paperSize);
        }
        /// <summary>
        /// 加载页面
        /// </summary>
        /// <param name="paramBillPaperSize"></param>
        private void LoadTransferBill(BillPaperSize paramBillPaperSize)
        {
            reportViewer1.Reset();
            string rdlcFileName = string.Empty;
            switch (paramBillPaperSize)
            {
                case BillPaperSize.CustomerSize:
                    rdlcFileName = "PIS_ViewAndPrintTransferBill2228_" + @".rdlc";
                    break;
                case BillPaperSize.A4Size:
                    rdlcFileName = "PIS_ViewAndPrintTransferBill_" + @".rdlc";
                    break;
                case BillPaperSize.A5Size:
                    rdlcFileName = "PIS_ViewAndPrintTransferBillA5_" + @".rdlc";
                    break;
                case BillPaperSize.A4SizeFull:
                    rdlcFileName = "PIS_ViewAndPrintTransferBillA4Full_" + @".rdlc";
                    break;
            }
            string reportFilePath = Application.StartupPath + @"\RdlcFiles\" + rdlcFileName;

            if (!File.Exists(reportFilePath))
            {
                switch (paramBillPaperSize)
                {
                    case BillPaperSize.CustomerSize:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\PIS_ViewAndPrintTransferBill2228.rdlc";
                        break;
                    case BillPaperSize.A4Size:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\PIS_ViewAndPrintTransferBill.rdlc";
                        break;
                    case BillPaperSize.A5Size:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\PIS_ViewAndPrintTransferBillA5.rdlc";
                        break;
                    case BillPaperSize.A4SizeFull:
                        reportFilePath = Application.StartupPath + @"\RdlcFiles\PIS_ViewAndPrintTransferBillA4Full.rdlc";
                        break;
                }
            }
            if (File.Exists(reportFilePath))
            {
                reportViewer1.LocalReport.ReportPath = reportFilePath;
                reportViewer1.LocalReport.DataSources.Clear();
                List<TransferBillUIModelToPrint> billList = new List<TransferBillUIModelToPrint> { _transferBill };
                ReportDataSource rpd1 = new ReportDataSource(PISViewParamKey.TransferBill.ToString(), billList as object);
                reportViewer1.LocalReport.DataSources.Add(rpd1);
                ReportDataSource rpd2 = new ReportDataSource(PISViewParamKey.TransferBillDetail.ToString(), _transferBillDetailList as object);
                reportViewer1.LocalReport.DataSources.Add(rpd2);

                //查询调入组织信息
                MDLSM_Organization transferInOrganization = new MDLSM_Organization();
                _bll.QueryForObject<MDLSM_Organization, MDLSM_Organization>(new MDLSM_Organization
                {
                    WHERE_Org_ID = _transferBill.TB_TransferInOrgId,
                }, transferInOrganization);

                //调入方信息
                ReportParameter rp1 = new ReportParameter("TransferInOrgName", string.IsNullOrEmpty(_transferBill.TB_TransferInOrgName) ? SysConst.SPACE : _transferBill.TB_TransferInOrgName);
                ReportParameter rp2 = new ReportParameter("TransferInOrgPhone", string.IsNullOrEmpty(transferInOrganization.Org_TEL) ? SysConst.SPACE : transferInOrganization.Org_TEL);
                ReportParameter rp3 = new ReportParameter("TransferBillNo", _transferBill.TB_No);

                //调出方信息
                //查询调出组织信息
                MDLSM_Organization transferOutOrganization = new MDLSM_Organization();
                if (_transferBill.TB_TransferInOrgId == _transferBill.TB_TransferOutOrgId)
                {
                    transferOutOrganization = transferInOrganization;
                }
                else
                {
                    _bll.QueryForObject<MDLSM_Organization, MDLSM_Organization>(new MDLSM_Organization
                    {
                        WHERE_Org_ID = _transferBill.TB_TransferOutOrgId,
                    }, transferOutOrganization);
                }
                ReportParameter rp4 = new ReportParameter("TransferOutOrgName", string.IsNullOrEmpty(_transferBill.TB_TransferOutOrgName) ? SysConst.SPACE : _transferBill.TB_TransferOutOrgName);
                ReportParameter rp5 = new ReportParameter("TransferOutOrgAddress", string.IsNullOrEmpty(transferOutOrganization.Org_Addr) ? SysConst.SPACE : transferOutOrganization.Org_Addr);
                ReportParameter rp6 = new ReportParameter("TransferOutOrgTel", string.IsNullOrEmpty(transferOutOrganization.Org_TEL) ? SysConst.SPACE : transferOutOrganization.Org_TEL);
                ReportParameter rp7 = new ReportParameter("TransferOutOrgMainProducts", string.IsNullOrEmpty(transferOutOrganization.Org_MainProducts) ? SysConst.SPACE : transferOutOrganization.Org_MainProducts);
                ReportParameter rp8 = new ReportParameter("CreatedBy", string.IsNullOrEmpty(_transferBill.TB_CreatedBy) ? SysConst.SPACE : _transferBill.TB_CreatedBy);
                ReportParameter rp9 = new ReportParameter("TotalAmount", _transferBill.TotalAmount.ToString());
                ReportParameter rp10 = new ReportParameter("TotalAmountString", ConvertMoneyNum(_transferBill.TotalAmount));
                ReportParameter rp11 = new ReportParameter("TotalQty", _transferBill.TotalQty.ToString());

                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4, rp5, rp6, rp7, rp8, rp9, rp10, rp11 });

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

        #region 大小写转换相关方法

        /// <summary>
        /// 大小写转换方法
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static string ConvertMoneyNum(decimal amount)
        {
            if (amount.Equals(0))
            {
                return "零元整";
            }
            else
            {
                string str = amount.ToString();
                int index = str.IndexOf(".00");
                if (index > 0)
                {
                    str = str.Remove(index);
                }
                if (!IsPositveDecimal(str))

                    return "金额未输入或输入不规范!";

                if (Double.Parse(str) > 999999999999.99)

                    return "数字太大，无法换算，请输入一万亿元以下的金额";

                char[] ch = new char[1];

                ch[0] = '.'; //小数点 

                string[] splitstr = null; //定义按小数点分割后的字符串数组 

                splitstr = !string.IsNullOrEmpty(str) ? str.Split(ch[0]) : null;//按小数点分割字符串 

                if (splitstr != null && splitstr.Length == 1) //只有整数部分 
                {

                    return ConvertData(str) + "元整";
                }
                else //有小数部分 
                {

                    string rstr = null;

                    if (splitstr != null)
                    {
                        rstr = ConvertData(splitstr[0]) + "元";//转换整数部分 

                        rstr += ConvertXiaoShu(splitstr[1]);//转换小数部分 
                    }

                    return rstr;
                }
            }

        }

        /// 判断是否是正数字字符串 
        /// 判断字符串 
        /// 如果是数字，返回true，否则返回false 
        public static bool IsPositveDecimal(string str)
        {

            Decimal d;

            try
            {

                d = Decimal.Parse(str);

            }

            catch (Exception)
            {

                return false;

            }

            if (d > 0)

                return true;

            else

                return false;

        }

        /// 转换数字（小数部分） 
        /// 需要转换的小数部分数字字符串 
        /// 转换成中文大写后的字符串 
        public static string ConvertXiaoShu(string str)
        {

            int strlen = str.Length;

            string rstr;

            if (strlen == 1)
            {

                rstr = ConvertChinese(str) + "角";

                return rstr;

            }

            else
            {

                string tmpstr = str.Substring(0, 1);

                rstr = ConvertChinese(tmpstr) + "角";

                tmpstr = str.Substring(1, 1);

                rstr += ConvertChinese(tmpstr) + "分";

                rstr = rstr.Replace("零分", string.Empty);

                rstr = rstr.Replace("零角", string.Empty);

                return rstr;

            }

        }

        /// 转换数字（整数） 
        /// 需要转换的整数数字字符串 
        /// 转换成中文大写后的字符串 
        public static string ConvertData(string str)
        {

            string tmpstr = string.Empty;

            string rstr = string.Empty;

            int strlen = str.Length;

            if (strlen <= 4)//数字长度小于四位 
            {

                rstr = ConvertDigit(str);

            }

            else
            {

                if (strlen <= 8)//数字长度大于四位，小于八位 
                {

                    tmpstr = str.Substring(strlen - 4, 4);//先截取最后四位数字 

                    rstr = ConvertDigit(tmpstr);//转换最后四位数字 

                    tmpstr = str.Substring(0, strlen - 4);//截取其余数字 

                    //将两次转换的数字加上萬后相连接 

                    rstr = String.Concat(ConvertDigit(tmpstr) + "萬", rstr);

                    rstr = rstr.Replace("零零", "零");

                }

                else

                    if (strlen <= 12)//数字长度大于八位，小于十二位 
                {

                    tmpstr = str.Substring(strlen - 4, 4);//先截取最后四位数字 

                    rstr = ConvertDigit(tmpstr);//转换最后四位数字 

                    tmpstr = str.Substring(strlen - 8, 4);//再截取四位数字 

                    rstr = String.Concat(ConvertDigit(tmpstr) + "萬", rstr);

                    tmpstr = str.Substring(0, strlen - 8);

                    rstr = String.Concat(ConvertDigit(tmpstr) + "億", rstr);

                    rstr = rstr.Replace("零億", "億");

                    rstr = rstr.Replace("零萬", "零");

                    rstr = rstr.Replace("零零", "零");

                    rstr = rstr.Replace("零零", "零");

                }

            }

            strlen = rstr.Length;

            if (strlen >= 2)
            {

                switch (rstr.Substring(strlen - 2, 2))
                {

                    case "佰零": rstr = rstr.Substring(0, strlen - 2) + "佰"; break;

                    case "仟零": rstr = rstr.Substring(0, strlen - 2) + "仟"; break;

                    case "萬零": rstr = rstr.Substring(0, strlen - 2) + "萬"; break;

                    case "億零": rstr = rstr.Substring(0, strlen - 2) + "億"; break;

                }

            }

            return rstr;

        }

        /// 转换数字 
        /// 转换的字符串（四位以内） 
        public static string ConvertDigit(string str)
        {

            int strlen = str.Length;

            string rstr = string.Empty;

            switch (strlen)
            {

                case 1: rstr = ConvertChinese(str); break;

                case 2: rstr = Convert2Digit(str); break;

                case 3: rstr = Convert3Digit(str); break;

                case 4: rstr = Convert4Digit(str); break;

            }

            rstr = rstr.Replace("拾零", "拾");

            strlen = rstr.Length;

            return rstr;

        }

        ///转换四位数字 
        public static string Convert4Digit(string str)
        {

            string str1 = str.Substring(0, 1);

            string str2 = str.Substring(1, 1);

            string str3 = str.Substring(2, 1);

            string str4 = str.Substring(3, 1);

            string rstring = string.Empty;

            rstring += ConvertChinese(str1) + "仟";

            rstring += ConvertChinese(str2) + "佰";

            rstring += ConvertChinese(str3) + "拾";

            rstring += ConvertChinese(str4);

            rstring = rstring.Replace("零仟", "零");

            rstring = rstring.Replace("零佰", "零");

            rstring = rstring.Replace("零拾", "零");

            rstring = rstring.Replace("零零", "零");

            rstring = rstring.Replace("零零", "零");

            rstring = rstring.Replace("零零", "零");

            return rstring;

        }

        /// 转换三位数字 
        public static string Convert3Digit(string str)
        {

            string str1 = str.Substring(0, 1);

            string str2 = str.Substring(1, 1);

            string str3 = str.Substring(2, 1);

            string rstring = string.Empty;

            rstring += ConvertChinese(str1) + "佰";

            rstring += ConvertChinese(str2) + "拾";

            rstring += ConvertChinese(str3);

            rstring = rstring.Replace("零佰", "零");

            rstring = rstring.Replace("零拾", "零");

            rstring = rstring.Replace("零零", "零");

            rstring = rstring.Replace("零零", "零");

            return rstring;

        }

        /// 转换二位数字 
        public static string Convert2Digit(string str)
        {

            string str1 = str.Substring(0, 1);

            string str2 = str.Substring(1, 1);

            string rstring = string.Empty;

            rstring += ConvertChinese(str1) + "拾";

            rstring += ConvertChinese(str2);

            rstring = rstring.Replace("零拾", "零");

            rstring = rstring.Replace("零零", "零");

            return rstring;

        }

        /// 将一位数字转换成中文大写数字 
        public static string ConvertChinese(string str)
        {

            //"零壹贰叁肆伍陆柒捌玖拾佰仟萬億元整角分" 

            string cstr = string.Empty;

            switch (str)
            {

                case "0": cstr = "零"; break;

                case "1": cstr = "壹"; break;

                case "2": cstr = "贰"; break;

                case "3": cstr = "叁"; break;

                case "4": cstr = "肆"; break;

                case "5": cstr = "伍"; break;

                case "6": cstr = "陆"; break;

                case "7": cstr = "柒"; break;

                case "8": cstr = "捌"; break;

                case "9": cstr = "玖"; break;

            }

            return (cstr);

        }

        #endregion


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
