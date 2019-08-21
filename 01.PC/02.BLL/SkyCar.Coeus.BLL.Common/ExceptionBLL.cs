using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.ComModel;
using SkyCar.Common.Utility;

namespace SkyCar.Coeus.BLL.Common
{
    /// <summary>
    /// 异常公共类
    /// </summary>
    public class ExceptionBLL
    {
        /// <summary>
        /// 忽略的异常列表
        /// </summary>
        public static List<string> IgnoreExceptionsMessage = new List<string>
        {

        };

        public static void WriteLogFileAndEmail(ThreadExceptionEventArgs e)
        {
            var ex = e.Exception;
            string exceptionContent = null;
            if (ex != null)
            {
                if (ex.Message.Contains("信息已过期"))
                {
                    return;
                }
                StackTrace st = new StackTrace(ex, true);
                exceptionContent = string.Format("错误的信息：{0}\n出错的方法名:{1}\n出错的类名:{2}\n出错行号:{3}\n文件名:{4}\n错误的堆栈{5}",
                    ex.Message, ex.TargetSite.Name, ex.GetType().Name, st.GetFrame(0).GetFileLineNumber(), st.GetFrame(0).GetFileName(), ex.StackTrace);

                //exceptionContent = ex.Message.Equals(SQLError.ErrorValue.UpdatedError) ? ex.Message : string.Format("异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n", ex.GetType().Name, ex.Message, ex.StackTrace);
            }
            if (!Directory.Exists(Application.StartupPath + @"\ErrorLog"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\ErrorLog");
            }
            string errLogFile = Application.StartupPath + @"\ErrorLog\Exception" + BLLCom.GetCurStdDatetime().ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString().ToUpper() + ".txt";

            using (StreamWriter sw = new StreamWriter(errLogFile, true))
            {
                sw.WriteLine(exceptionContent);
                sw.WriteLine("---------------------------------------------------------");
                sw.Close();
            }
            string messageTitle = (ex != null ? ex.Message : string.Empty);
            bool ignoreMessage = IgnoreExceptionsMessage.Any(ignoreMsg => messageTitle.Contains(ignoreMsg));
            if (!ignoreMessage && !string.IsNullOrEmpty(SystemConfigInfo.SkyCarDevEmailAdress))
            {
                string emailTitle = SysConst.ProductCode + "(" + SysConst.VersionNo + ")" + "在" + LoginInfoDAX.MCTCode + "_" + LoginInfoDAX.OrgShortName + "_" + "出现异常:" + (ex != null ? ex.Message : string.Empty) + ",物理内存:" + LocalSystemHelper.GetPhisicalMemorySize() + "G,可用内存:" + LocalSystemHelper.AvailableMemory + "G";
                if (!SameContentFileExists(Application.StartupPath + @"\ErrorLog", errLogFile, true))
                {
                    BLLCom.SendMail(SystemConfigInfo.SkyCarDevEmailAdress, string.Empty, "异常反馈:" + emailTitle, LoginInfoDAX.UserID + "在" + LoginInfoDAX.OrgShortName + "发现了异常，详情见附件。", errLogFile);
                }
            }
        }

        public static void WriteLogFileAndEmail(UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            string exceptionContent = null;
            if (ex != null)
            {
                if (ex.Message.Contains("信息已过期"))
                {
                    return;
                }
                StackTrace st = new StackTrace(ex, true);
                exceptionContent = string.Format("错误的信息：{0}\n出错的方法名:{1}\n出错的类名:{2}\n出错行号:{3}\n文件名:{4}\n错误的堆栈{5}",
                   ex.Message, ex.TargetSite.Name, ex.GetType().Name, st.GetFrame(0).GetFileLineNumber(), st.GetFrame(0).GetFileName(), ex.StackTrace);
                //exceptionContent = ex.Message.Equals(SQLError.ErrorValue.UpdatedError) ? ex.Message : string.Format("Application UnhandledException:{0};\n\r堆栈信息:{1}", ex.Message, ex.StackTrace);
            }

            if (!Directory.Exists(Application.StartupPath + @"\ErrorLog"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\ErrorLog");
            }
            string errLogFile = Application.StartupPath + @"\ErrorLog\Exception" + BLLCom.GetCurStdDatetime().ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString().ToUpper() + ".txt";
            using (StreamWriter sw = new StreamWriter(errLogFile, true))
            {
                sw.WriteLine(exceptionContent);
                sw.WriteLine("---------------------------------------------------------");
                sw.Close();
            }
            string messageTitle = (ex != null ? ex.Message : string.Empty);
            bool ignoreMessage = IgnoreExceptionsMessage.Any(ignoreMsg => messageTitle.Contains(ignoreMsg));
            if (!ignoreMessage && !string.IsNullOrEmpty(SystemConfigInfo.SkyCarDevEmailAdress))
            {
                string emailTitle = LoginInfoDAX.SPCode + "(" + SysConst.VersionNo + ")" + "在" + LoginInfoDAX.MCTCode + "_" + LoginInfoDAX.OrgShortName + "_" + "出现异常:" + (ex != null ? ex.Message : string.Empty) + ",物理内存:" + LocalSystemHelper.GetPhisicalMemorySize() + "G,可用内存:" + LocalSystemHelper.AvailableMemory + "G";
                if (!SameContentFileExists(Application.StartupPath + @"\ErrorLog", errLogFile, true))
                {
                    BLLCom.SendMail(SystemConfigInfo.SkyCarDevEmailAdress, string.Empty, "异常反馈:" + emailTitle, LoginInfoDAX.UserID + "在" + LoginInfoDAX.OrgShortName + "发现了异常，详情见附件。", errLogFile);
                }
            }
        }

        public static void WriteLogFileAndEmail(Exception ex)
        {
            string exceptionContent = null;
            if (ex != null)
            {
                if (ex.Message.Contains("信息已过期"))
                {
                    return;
                }
                StackTrace st = new StackTrace(ex, true);
                exceptionContent = string.Format("错误的信息：{0}\n出错的方法名:{1}\n出错的类名:{2}\n出错行号:{3}\n文件名:{4}\n错误的堆栈{5}",
                   ex.Message, ex.TargetSite.Name, ex.GetType().Name, st.GetFrame(0).GetFileLineNumber(), st.GetFrame(0).GetFileName(), ex.StackTrace);
                //exceptionContent = ex.Message.Equals(SQLError.ErrorValue.UpdatedError) ? ex.Message : string.Format("编码人员发现的异常信息:{0};\n\r堆栈信息:{1};编码人员的备注:{2}", ex.Message, ex.StackTrace, paramDevlepersRemark);
            }
            if (!Directory.Exists(Application.StartupPath + @"\ErrorLog"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\ErrorLog");
            }
            string errLogFile = Application.StartupPath + @"\ErrorLog\Exception" + DateTime.Now.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString().ToUpper() + ".txt";
            using (StreamWriter sw = new StreamWriter(errLogFile, true))
            {
                sw.WriteLine(exceptionContent);
                sw.WriteLine("---------------------------------------------------------");
                sw.Close();
            }
            string messageTitle = (ex != null ? ex.Message : string.Empty);
            bool ignoreMessage = IgnoreExceptionsMessage.Any(ignoreMsg => messageTitle.Contains(ignoreMsg));
            if (!ignoreMessage && !string.IsNullOrEmpty(SystemConfigInfo.SkyCarDevEmailAdress))
            {
                string emailTitle = LoginInfoDAX.SPCode + "(" + SysConst.VersionNo + ")" + "在" + LoginInfoDAX.MCTCode + "_" + LoginInfoDAX.OrgShortName + "_" + "出现异常:" + (ex != null ? ex.Message : string.Empty) + ",物理内存:" + LocalSystemHelper.GetPhisicalMemorySize() + "G,可用内存:" + LocalSystemHelper.AvailableMemory + "G";
                if (!SameContentFileExists(Application.StartupPath + @"\ErrorLog", errLogFile, true))
                {
                    BLLCom.SendMail(SystemConfigInfo.SkyCarDevEmailAdress, string.Empty, "异常反馈:" + emailTitle, LoginInfoDAX.UserID + "在" + LoginInfoDAX.OrgShortName + "发现了异常，详情见附件。", errLogFile);
                }
            }
        }

        public static bool SameContentFileExists(string folderFullName, string fileFullName, bool sameDate)
        {
            bool sameContentFileExists = false;
            DirectoryInfo destFolder = new DirectoryInfo(folderFullName);
            FileInfo sourFile = new FileInfo(fileFullName);
            //遍历文件
            foreach (FileInfo destFile in destFolder.GetFiles())
            {
                if (!destFile.Name.Equals(sourFile.Name)
                    && GetFileHash(destFile.FullName).Equals(GetFileHash(sourFile.FullName)))
                {
                    if (!sameDate)
                    {
                        sameContentFileExists = true;
                        break;
                    }
                    else
                    {
                        if (destFile.CreationTime.Date.Equals(sourFile.CreationTime.Date))
                        {
                            sameContentFileExists = true;
                            break;
                        }
                    }
                }
            }
            return sameContentFileExists;
        }

        public static string GetFileHash(string filePath)
        {
            if (!File.Exists(filePath))
                return string.Empty;

            string md5;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer;
                using (HashAlgorithm hash = HashAlgorithm.Create())
                {
                    buffer = hash.ComputeHash(fs);
                    hash.Clear();
                }
                md5 = Convert.ToBase64String(buffer);
                fs.Close();
            }
            return md5;
        }
    }
}
