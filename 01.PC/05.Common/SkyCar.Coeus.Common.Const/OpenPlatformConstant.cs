using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SkyCar.Coeus.Common.Const
{
    public static class SupplierConst
    {
        public static string SoftName = "Supplier";
        public static string AppStartPath = Application.StartupPath.ToString();
        public static string CurrentVersionStr = "1.13";

        //写入ini文件
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        //读取ini文件
        [DllImport("kernel32")]
        public static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder refVal, int size, string filePath);


        public static class FilePathOfINIFile
        {
            public static string softConfigFilePath = AppStartPath + "\\softConfig.ini";
            public static string softActiveFilePath = AppStartPath + "\\softActive.ini";
        }

        /// <summary>
        /// ini文件的Selection部分
        /// </summary>
        public static class SelectionName
        {
            public static string ActiveInfo = "ActiveInfo";
            public static string SystemInfo = "SystemInfo";
            public static string BranchInfo = "BranchInfo";
            public static string StartOption = "StartOption";
            public static string LoginInfo = "LoginInfo";
            public static string LoginForm = "LoginForm";
            public static string UserForm = "UserForm";
            public static string RemindInfoForm = "RemindInfoForm";
            public static string RemindMusic = "RemindMusic";
            public static string BusinessConfig = "BusinessConfig";
            public static string DeviceInfo = "DeviceInfo";
            public static string IDCardRecogConfig = "IDCardRecogConfig";
            public static string PlateRecogConfig = "PlateRecogConfig";
        }

        public static class KeyName
        {
            public static string SoftName = "SoftName";
            public static string SoftVersion = "SoftVersion";
            public static string ActiveCode = "ActiveCode";
        }
    }
}
