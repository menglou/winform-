﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SkyCar.Coeus.BLL;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UI.SM;

namespace SkyCar.Coeus.Ult.Entrance
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //处理未捕获的异常   
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常   
            Application.ThreadException += Application_ThreadException;
            //处理非UI线程异常   
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            #region 禁止客户端多开

            Process[] processes = Process.GetProcessesByName("SkyCar.Coeus");
            if (processes.Length > 1)
            {
                DialogResult dr = MessageBox.Show("该系统已经运行！\r\n是否确定强制关闭已经运行的进程，并重新启动新进程", "消息",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (DialogResult.OK == dr)
                {
                    processes[0].Kill();
                }
                else
                {
                    return;
                }
            }

            #endregion 禁止客户端多开End

            #region 启动更新程序

            try
            {
                #region 关闭自动更新程序

                var updateProcesses = Process.GetProcessesByName("SkyCar.AutoUpdate");
                foreach (var p in updateProcesses)
                {
                    p.Kill();
                }

                #endregion 关闭自动更新程序End

                var fileInfo = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"Update\NewSkyCar.AutoUpdate.exe");
                if (fileInfo.Exists)
                {
                    fileInfo.CopyTo(fileInfo.FullName.Replace("NewSkyCar.AutoUpdate.exe", "SkyCar.AutoUpdate.exe"), true);
                    fileInfo.Delete();
                }
                //检查UpdateList.xml是否存在
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "UpdateList.xml"))
                {
                    var autoUpdateFile = @"Update\SkyCar.AutoUpdate.exe";
                    if (File.Exists(autoUpdateFile))
                    {
                        Process.Start(autoUpdateFile);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxs.Show(Trans.SM, "Main", ex.Message +
                    "自动更新模块启动错误！\r\n建议做如下检查：\r\n1.确认UpdateList.xml在启动目录中是否存在。", "消息", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            #endregion

            #region 激活

            if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[AppSettingKey.CONNECTION_STRING]))
            {
                //链接字符串为空表示产品未激活
                try
                {
                    Application.Run(new FrmActivateSoft());
                    return;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteErrorLog(Trans.COM, "Main", ex.Message + SysConst.ENTER + ex.StackTrace, null, ex);
                    return;
                }
            }

            #endregion

            //初始化系统
            BLLCom.InitializeSystem();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmLogin());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string exSource = e.Exception != null ? e.Exception.Source : string.Empty;
            string exMessage = e.Exception != null ? e.Exception.Message : string.Empty;
            MessageBoxs.Show(Trans.COM, exSource, exMessage, "发现异常", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ExceptionBLL.WriteLogFileAndEmail(e);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            string exSource = string.Empty;
            string exMessage = string.Empty;
            if (ex != null)
            {
                exSource = ex.Source;
                exMessage = ex.Message;
            }
            MessageBoxs.Show(Trans.COM, exSource, exMessage, "发现异常", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ExceptionBLL.WriteLogFileAndEmail(e);
        }
    }
}
