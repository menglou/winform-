using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace SkyCar.Coeus.UpdateClient
{
    /// <summary>
    /// 自动升级的入口点
    /// </summary>
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Process[] processList = Process.GetProcesses();
            string currentDirectory = Process.GetCurrentProcess().MainModule.FileName.Replace(Process.GetCurrentProcess().MainModule.ModuleName, string.Empty);
            foreach (Process process in processList)
            {
                if (process.ProcessName.ToUpper().Equals(AutoUpdateCoeus.EntranceFileName)
                    && process.MainModule.FileName.Replace(process.MainModule.ModuleName, string.Empty).Equals(currentDirectory))
                {
                    process.Kill();
                    process.Close();
                }
            }

            if (args.Length > 0)
            {
                AutoUpdateCoeus.LatestSoftVersionNo = args[0];
            }
            //处理未捕获的异常   
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常   
            Application.ThreadException += Application_ThreadException;
            //处理非UI线程异常   
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmAutoUpdate());
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var ex = e.Exception as Exception;
            if (ex != null)
            {
                MessageBox.Show("发现异常：" + ex.Message);
            }
            AutoUpdateCoeus.WriteLogFileAndEmail(e);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                MessageBox.Show("发现异常：" + ex.Message);
            }
            AutoUpdateCoeus.WriteLogFileAndEmail(e);
        }
    }
}
