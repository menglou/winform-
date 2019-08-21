using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SkyCar.Coeus.BLL.Common
{
    public class BarcodePrinterHelper
    {
        const string szSavePath = "C:\\Argox";
        const string szSaveFile = "C:\\Argox\\PPLB_Example.Prn";
        [DllImport("Winpplb.dll")]
        private static extern int B_Bar2d_Maxi(int x, int y, int cl, int cc, int pc, string data);
        [DllImport("Winpplb.dll")]
        private static extern int B_Bar2d_PDF417(int x, int y, int w, int v, int s, int c, int px,
            int py, int r, int l, int t, int o, string data);
        [DllImport("Winpplb.dll")]
        private static extern int B_Bar2d_PDF417_N(int x, int y, int w, string para, string data);
        [DllImport("Winpplb.dll")]
        public static extern void B_ClosePrn();
        [DllImport("Winpplb.dll")]
        public static extern int B_CreatePrn(int selection, string filename);
        [DllImport("Winpplb.dll")]
        private static extern int B_Del_Form(string formname);
        [DllImport("Winpplb.dll")]
        private static extern int B_Del_Pcx(string pcxname);
        [DllImport("Winpplb.dll")]
        private static extern int B_Draw_Box(int x, int y, int thickness, int hor_dots,
            int ver_dots);
        [DllImport("Winpplb.dll")]
        private static extern int B_Draw_Line(char mode, int x, int y, int hor_dots, int ver_dots);
        [DllImport("Winpplb.dll")]
        private static extern int B_Error_Reporting(char option);
        [DllImport("Winpplb.dll")]
        private static extern string B_Get_DLL_Version(int nShowMessage);
        [DllImport("Winpplb.dll")]
        private static extern int B_Get_DLL_VersionA(int nShowMessage);
        [DllImport("Winpplb.dll")]
        private static extern int B_Get_Graphic_ColorBMP(int x, int y, string filename);
        [DllImport("Winpplb.dll")]
        private static extern int B_Get_Pcx(int x, int y, string filename);
        [DllImport("Winpplb.dll")]
        private static extern int B_Initial_Setting(int Type, string Source);
        [DllImport("Winpplb.dll")]
        private static extern int B_Load_Pcx(int x, int y, string pcxname);
        [DllImport("Winpplb.dll")]
        private static extern int B_Open_ChineseFont(string path);
        [DllImport("Winpplb.dll")]
        private static extern int B_Print_Form(int labset, int copies, string form_out, string var);
        [DllImport("Winpplb.dll")]
        private static extern int B_Print_MCopy(int labset, int copies);
        [DllImport("Winpplb.dll")]
        public static extern int B_Print_Out(int labset);
        [DllImport("Winpplb.dll")]
        public static extern int B_Prn_Barcode(int x, int y, int ori, string type, int narrow,
            int width, int height, char human, string data);
        [DllImport("Winpplb.dll")]
        private static extern void B_Prn_Configuration();
        [DllImport("Winpplb.dll")]
        private static extern int B_Prn_Text(int x, int y, int ori, int font, int hor_factor,
            int ver_factor, char mode, string data);
        [DllImport("Winpplb.dll")]
        private static extern int B_Prn_Text_Chinese(int x, int y, int fonttype, string id_name,
            string data);
        [DllImport("Winpplb.dll")]
        public static extern int B_Prn_Text_TrueType(int x, int y, int FSize, string FType,
            int Fspin, int FWeight, int FItalic, int FUnline, int FStrikeOut, string id_name,
            string data);
        [DllImport("Winpplb.dll")]
        private static extern int B_Prn_Text_TrueType_W(int x, int y, int FHeight, int FWidth,
            string FType, int Fspin, int FWeight, int FItalic, int FUnline, int FStrikeOut,
            string id_name, string data);
        [DllImport("Winpplb.dll")]
        private static extern int B_Select_Option(int option);
        [DllImport("Winpplb.dll")]
        private static extern int B_Select_Option2(int option, int p);
        [DllImport("Winpplb.dll")]
        private static extern int B_Select_Symbol(int num_bit, int symbol, int country);
        [DllImport("Winpplb.dll")]
        private static extern int B_Select_Symbol2(int num_bit, string csymbol, int country);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Backfeed(char option);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_BMPSave(int nSave, string strBMPFName);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Darkness(int darkness);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_DebugDialog(int nEnable);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Direction(char direction);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Form(string formfile);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Labgap(int lablength, int gaplength);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Labwidth(int labwidth);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Originpoint(int hor, int ver);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Prncomport(int baud, char parity, int data, int stop);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Prncomport_PC(int nBaudRate, int nByteSize, int nParity,
            int nStopBits, int nDsr, int nCts, int nXonXoff);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Speed(int speed);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_ProcessDlg(int nShow);
        [DllImport("Winpplb.dll")]
        public static extern int B_GetUSBBufferLen();
        [DllImport("Winpplb.dll")]
        public static extern int B_EnumUSB(byte[] buf);
        [DllImport("Winpplb.dll")]
        public static extern int B_CreateUSBPort(int nPort);
        [DllImport("Winpplb.dll")]
        private static extern int B_ResetPrinter();
        [DllImport("Winpplb.dll")]
        private static extern int B_GetPrinterResponse(byte[] buf, int nMax);
        [DllImport("Winpplb.dll")]
        private static extern int B_TFeedMode(int nMode);
        [DllImport("Winpplb.dll")]
        private static extern int B_TFeedTest();
        [DllImport("Winpplb.dll")]
        private static extern int B_CreatePort(int nPortType, int nPort, string filename);
        [DllImport("Winpplb.dll")]
        private static extern int B_Execute_Form(string form_out, string var);
        [DllImport("Winpplb.dll")]
        private static extern int B_Bar2d_QR(int x, int y, int model, int scl, char error,
            char dinput, int c, int d, int p, string data);
        [DllImport("Winpplb.dll")]
        private static extern int B_GetNetPrinterBufferLen();
        [DllImport("Winpplb.dll")]
        private static extern int B_EnumNetPrinter(byte[] buf);
        [DllImport("Winpplb.dll")]
        private static extern int B_CreateNetPort(int nPort);
        [DllImport("Winpplb.dll")]
        private static extern int B_Prn_Text_TrueType_Uni(int x, int y, int FSize, string FType,
            int Fspin, int FWeight, int FItalic, int FUnline, int FStrikeOut, string id_name,
            byte[] data, int format);
        [DllImport("Winpplb.dll")]
        private static extern int B_Prn_Text_TrueType_UniB(int x, int y, int FSize, string FType,
            int Fspin, int FWeight, int FItalic, int FUnline, int FStrikeOut, string id_name,
            byte[] data, int format);
        [DllImport("Winpplb.dll")]
        public static extern int B_GetUSBDeviceInfo(int nPort, byte[] pDeviceName,
            out int pDeviceNameLen, byte[] pDevicePath, out int pDevicePathLen);

        public static string PrinterBarCode(string depotName, string partsPosition,
            string partsName, string customDesc, string barCode)
        {
            // open port.
            int nLen, ret;
            ret = 0;
            byte[] pbuf = new byte[128];
            string strmsg;
            Encoding encAscII = Encoding.ASCII;
            Encoding encUnicode = Encoding.Unicode;
            // search port.
            nLen = B_GetUSBBufferLen() + 1;
            strmsg = string.Empty;
            if (nLen > 1)
            {
                byte[] buf1, buf2;
                int len1 = 128, len2 = 128;
                buf1 = new byte[len1];
                buf2 = new byte[len2];
                B_EnumUSB(pbuf);
                B_GetUSBDeviceInfo(1, buf1, out len1, buf2, out len2);
                ret = B_CreatePrn(12, encAscII.GetString(buf2, 0, len2));// open usb.
                if (0 != ret)
                {
                    strmsg += "Open USB fail!";
                }
            }

            B_Prn_Text_TrueType(10, 20, 35, "微软雅黑", 1, 800, 0, 0, 0, "A1", "仓库：" + depotName + "  仓位：" + partsPosition);
            B_Prn_Text_TrueType(10, 60, 35, "微软雅黑", 1, 800, 0, 0, 0, "A2", "物品名称：" + partsName);
            B_Prn_Text_TrueType(10, 100, 35, "微软雅黑", 1, 800, 0, 0, 0, "A3", "自定义：" + customDesc);
            B_Prn_Barcode(40, 160, 0, "1", 2, 3, 110, 'B', barCode);//have a counter
            B_Print_Out(1);
            B_ClosePrn();
            return strmsg;
        }
    }
}
