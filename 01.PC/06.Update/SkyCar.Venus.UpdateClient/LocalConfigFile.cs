using System.Diagnostics;
using System.IO;

namespace SkyCar.Coeus.UpdateClient
{
    /// <summary>
    /// 本地配置文件
    /// </summary>
    public static class LocalConfigFile
    {
        /// <summary>
        /// ini文件的Selection部分
        /// </summary>
        public static class SelectionName
        {
            /// <summary>
            /// 版本号信息
            /// </summary>
            public static string SoftInfo = "SoftInfo";
        }

        /// <summary>
        /// ini文件的Key部分
        /// </summary>
        public static class KeyName
        {
            /// <summary>
            /// 该产品当前的版本号
            /// </summary>
            public static string SoftVersionNo = "SoftVersionNo";
        }

        public static class ConfigFilePath
        {
            /// <summary>
            /// 产品的版本号
            /// </summary>
            public static string SoftVersionNo = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\LocalConfig\\SysConData\\SoftConfig.ini";
        }
    }
}
