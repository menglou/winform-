using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SkyCar.Coeus.UpdateClient
{
    /// <summary>
    /// 自动升级Coeus
    /// </summary>
    public static class AutoUpdateCoeus
    {
        public static bool IsFinishUpdate = false;
        public static bool HaveNewVersion = false;
        public static bool WillUpdate = false;
        public static bool DownloadSuccess = true;
        public static bool IsServer = false;
        public static string UrlOfUpgrade = string.Empty;
        public static string UserNameOfUpgrade = string.Empty;
        public static string PasswordOfUpgrade = string.Empty;
        public static string LatestSoftVersionNo = string.Empty;
        public static string TempUpgradeDirectory = "tempUpdateDir";
        public static string EntranceFileName = "SKYCAR.COEUS.ULT.ENTRANCE";
        public static string LocalExeFileName = "SkyCar.Coeus.Ult.Entrance.exe";
        public static string LocalConfigFileName = "SkyCar.Coeus.Ult.Entrance.exe.config";

        public static ProgramDir ProDir;
        public static List<string> SelfFiles = new List<string>
        {
            "updlog.txt",
            "ftplib.dll",
            "kernel32.dll",
            "SkyCar.Coeus.AutoUpdate.dll",
            "SkyCar.Coeus.UpdateClient.exe",
            "SkyCar.Coeus.UpdateClient.exe.config",
            "SkyCar.Coeus.Entrance.Ult.exe.config",
            "softConfig.ini",
            "ErrorLog",
            "ComponentFactory.Krypton.Toolkit.dll",
            "Infragistics4.Shared.v15.1.dll",
            "Infragistics4.Win.Misc.v15.1.dll",
            "Infragistics4.Win.UltraWinEditors.v15.1.dll",
            "Infragistics4.Win.v15.1.dll",
            "Microsoft.CSharp.dll",
            "Newtonsoft.JsonForUpgrade.dll",
            "PresentationFramework.dll",
            "System.dll",
            "System.Configuration.dll",
            "System.Core.dll",
            "System.Data.dll",
            "System.Data.DataSetExtensions.dll",
            "System.Drawing.dll",
            "System.Runtime.Serialization.Formatters.Soap.dll",
            "System.Xml.dll",
            "System.Xml.Linq.dll",
            "UIAutomationProvider.dll",
            "UIAutomationTypes.dll",
            "WindowsBase.dll",
            "SkyCar.Coeus.UpdateClient.pdb"
        };
        public static readonly string LogFileName = "updlog.txt";

        private static string _callbackEXE;
        public static string CallBackEXE
        {
            get { return _callbackEXE; }
            set { _callbackEXE = value; }
        }

        public class ProgramFile
        {
            public string Name { get; set; }
            public string Hash { get; set; }
        }

        public class ProgramDir
        {
            public string Name { get; set; }

            private List<ProgramFile> _files;
            public List<ProgramFile> Files { get { return _files; } }

            private List<ProgramDir> _dirs;
            public List<ProgramDir> Dirs { get { return _dirs; } }

            public ProgramDir()
            {
                _files = new List<ProgramFile>();
                _dirs = new List<ProgramDir>();
            }
        }

        /// <summary>
        /// 延时一段时间后关闭
        /// </summary>
        /// <param name="delay"></param>
        public static void CloseWindowDelay(int delay)
        {
            Thread th = new Thread(delegate ()
            {
                Thread.Sleep(delay);
                IsFinishUpdate = true;
                Environment.Exit(0);
            });
            th.Start();
        }

        public static string GetFileHash(string filePath)
        {
            if (!File.Exists(filePath))
                return string.Empty;

            string md5;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                byte[] buffer;
                using (HashAlgorithm hash = HashAlgorithm.Create())
                {
                    buffer = hash.ComputeHash(fs);
                }
                md5 = Convert.ToBase64String(buffer);
            }
            return md5;
        }


        public static void WriteLogFileAndEmail(Exception ex, string paramAttachContent = "")
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
                //exceptionContent = ex.Message.Equals(SQLError.ErrorValue.UpdatedError) ? ex.Message : string.Format("Application UnhandledException:{0};\n\r堆栈信息:{1}", ex.Message, ex.StackTrace);
            }

            if (!Directory.Exists(Application.StartupPath + @"\ErrorLog"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\ErrorLog");
            }
            string errLogFile = Application.StartupPath + @"\ErrorLog\升级异常" + DateTime.Now.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString().ToUpper() + ".txt";
            using (StreamWriter sw = new StreamWriter(errLogFile, true))
            {
                sw.WriteLine(exceptionContent);
                sw.WriteLine("---------------------------------------------------------");
                sw.Close();
            }

            SendMail("skycardev@skycarcn.com", "Coeus升级异常反馈:", "升级发现了异常，详情见附件," + paramAttachContent, errLogFile);
        }

        public static void WriteLogFileAndEmail(UnhandledExceptionEventArgs e, string paramAttachContent = "")
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
            string errLogFile = Application.StartupPath + @"\ErrorLog\升级异常" + DateTime.Now.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString().ToUpper() + ".txt";
            using (StreamWriter sw = new StreamWriter(errLogFile, true))
            {
                sw.WriteLine(exceptionContent);
                sw.WriteLine("---------------------------------------------------------");
                sw.Close();
            }

            SendMail("skycardev@skycarcn.com", "Coeus升级异常反馈:", "升级发现了异常，详情见附件," + paramAttachContent, errLogFile);
        }

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
            string errLogFile = Application.StartupPath + @"\ErrorLog\Exception" + DateTime.Now.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString().ToUpper() + ".txt";

            using (StreamWriter sw = new StreamWriter(errLogFile, true))
            {
                sw.WriteLine(exceptionContent);
                sw.WriteLine("---------------------------------------------------------");
                sw.Close();
            }
        }

        public static void SendMail(string paramRecipientEmailAdress, string paramEmailSubject, string paramEmailBody, string paramAttachFilePath)
        {
            string addressOfSender = "skycardev@skycarcn.com";
            string pwdOfSender = "skycardev";
            if (addressOfSender.Length > 0)
            {
                MailAddress mailAdress = new MailAddress(addressOfSender, addressOfSender.Substring(0, addressOfSender.IndexOf("@", StringComparison.Ordinal)));
                MailMessage mailMessage = new MailMessage
                {
                    Subject = paramEmailSubject.Replace((char)13, (char)0).Replace((char)10, (char)0),
                    From = mailAdress
                };

                //设置邮件收件人  
                string[] recpEmailList = (paramRecipientEmailAdress + ";").Split(Convert.ToChar(";"));
                foreach (string name in recpEmailList)
                {
                    if (name != string.Empty)
                    {
                        string address;
                        string displayName;
                        if (name.IndexOf('<') > 0)
                        {
                            displayName = name.Substring(0, name.IndexOf('<'));
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }
                        else
                        {
                            displayName = string.Empty;
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }
                        mailMessage.To.Add(new MailAddress(address, displayName));
                    }
                }

                //设置邮件的内容
                mailMessage.Body = paramEmailBody;
                //设置邮件的格式
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.IsBodyHtml = true;
                //设置邮件的发送级别
                mailMessage.Priority = MailPriority.Normal;

                //设置邮件的附件，将在客户端选择的附件先上传到服务器保存一个，然后加入到mail中  
                if (!string.IsNullOrEmpty(paramAttachFilePath))
                {
                    mailMessage.Attachments.Add(new Attachment(paramAttachFilePath));
                }
                mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                SmtpClient client = new SmtpClient
                {
                    Host = "smtp.ym.163.com",
                    Port = 25,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(addressOfSender, pwdOfSender),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
                client.Send(mailMessage);
            }
            else
            {
                MessageBox.Show("参数表未配置云车研发中心邮箱(0000)");
                return;
            }
        }

    }
}
