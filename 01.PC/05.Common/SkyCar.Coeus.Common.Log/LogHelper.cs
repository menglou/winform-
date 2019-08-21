using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using log4net;

namespace SkyCar.Coeus.Common.Log
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class LogHelper
    {
        #region 日志定义

        //COM
        private static ILog comLog;
        //COM ERROR
        private static ILog comErrorLog;
        //BS
        private static ILog bsLog;
        //BS ERROR
        private static ILog bsErrorLog;
        //SM
        private static ILog smLog;
        //SM ERROR
        private static ILog smErrorLog;
        //FM
        private static ILog fmLog;
        //FM ERROR
        private static ILog fmErrorLog;
        //PIS
        private static ILog pisLog;
        //PIS ERROR
        private static ILog pisErrorLog;
        //WC
        private static ILog wcLog;
        //WC ERROR
        private static ILog wcErrorLog;
        //SD
        private static ILog sdLog;
        //SD ERROR
        private static ILog sdErrorLog;
        //RPT
        private static ILog rptLog;
        //RPT ERROR
        private static ILog rptErrorLog;
        #endregion

        #region 公共（不要修改）

        #region 业务日志 public static void WriteBussLog(String BussID, string loginUserID, String JobName, String message, String FormName, string[] args)
        /// <summary>
        /// 业务日志
        /// </summary>
        /// <param name="paramBussID">业务ID(Trans.[XX])</param>
        /// <param name="paramLoginUserID">登陆用户ID/名称</param>
        /// <param name="paramJobName">方法名</param>
        /// <param name="paramMessage">消息内容</param>
        /// <param name="paramFormName">画面名</param>
        /// <param name="paramArgs">消息参数</param>
        public static void WriteBussLog(String paramBussID, string paramLoginUserID, String paramJobName, String paramMessage, String paramFormName, string[] paramArgs)
        {
            //日志内容的取得
            string logMessage = getLogMessage(paramLoginUserID, paramFormName, paramJobName, paramMessage, paramArgs, null, null);
            //业务日志取得
            ILog logger = getBusiLogger(paramBussID);

            if (logger.IsInfoEnabled)
            {
                logger.Info(logMessage);
            }

            if (logger.IsDebugEnabled)
            {
                logger.Debug(logMessage);
            }

        }
        /// <summary>
        /// 业务日志-开始
        /// </summary>
        /// <param name="paramBussID">业务ID(Trans.[XX])</param>
        /// <param name="paramLoginUserID">登陆用户ID/名称</param>
        /// <param name="paramJobName">方法名</param>
        /// <param name="paramMessage">消息内容</param>
        /// <param name="paramFormName">画面名</param>
        /// <param name="paramArgs">消息参数</param>
        public static void WriteBussLogStart(String paramBussID, string paramLoginUserID, String paramJobName, String paramMessage, String paramFormName, string[] paramArgs)
        {
            //日志内容的取得
            string logMessage = getLogMessage(paramLoginUserID, paramFormName, paramJobName + " 【START】", paramMessage, paramArgs, null, null);
            //业务日志取得
            ILog logger = getBusiLogger(paramBussID);

            if (logger.IsInfoEnabled)
            {
                logger.Info(logMessage);
            }

            if (logger.IsDebugEnabled)
            {
                logger.Debug(logMessage);
            }

        }
        /// <summary>
        /// 业务日志-结束（无错误）
        /// </summary>
        /// <param name="paramBussID">业务ID(Trans.[XX])</param>
        /// <param name="paramLoginUserID">登陆用户ID/名称</param>
        /// <param name="paramJobName">方法名</param>
        /// <param name="paramMessage">消息内容</param>
        /// <param name="paramFormName">画面名</param>
        /// <param name="paramArgs">消息参数</param>
        public static void WriteBussLogEndOK(String paramBussID, string paramLoginUserID, String paramJobName, String paramMessage, String paramFormName, string[] paramArgs)
        {
            //日志内容的取得
            string logMessage = getLogMessage(paramLoginUserID, paramFormName, paramJobName + " 【END】-OK", paramMessage, paramArgs, null, null);
            //业务日志取得
            ILog logger = getBusiLogger(paramBussID);

            if (logger.IsInfoEnabled)
            {
                logger.Info(logMessage);
            }

            if (logger.IsDebugEnabled)
            {
                logger.Debug(logMessage);
            }

        }
        /// <summary>
        /// 业务日志-结束（有错误）
        /// </summary>
        /// <param name="paramBussID">业务ID(Trans.[XX])</param>
        /// <param name="paramLoginUserID">登陆用户ID/名称</param>
        /// <param name="paramJobName">方法名</param>
        /// <param name="paramMessage">消息内容</param>
        /// <param name="paramFormName">画面名</param>
        /// <param name="paramArgs">消息参数</param>
        public static void WriteBussLogEndNG(String paramBussID, string paramLoginUserID, String paramJobName, String paramMessage, String paramFormName, string[] paramArgs)
        {
            //日志内容的取得
            string logMessage = getLogMessage(paramLoginUserID, paramFormName, paramJobName + " 【END】-NG", paramMessage, paramArgs, null, null);
            //业务日志取得
            ILog logger = getBusiLogger(paramBussID);

            if (logger.IsInfoEnabled)
            {
                logger.Info(logMessage);
            }

            if (logger.IsDebugEnabled)
            {
                logger.Debug(logMessage);
            }

        }
        #endregion

        #region 性能日志 WriteQualityLog(String BussID, String message, String sqlID)

        ///// <summary>
        ///// 性能日志
        ///// </summary>
        ///// <param name="BussID">业务ID</param>
        ///// <param name="JobName">处理名</param>
        ///// <param name="message">消息</param>
        ///// <param name="sqlID">SQLID</param>
        //public static void WriteQualityLog(String BussID, String JobName, String message, String sqlID)
        //{
        //    string logMessage = getLogMessage(null, null,JobName, message, null, sqlID, null);
        //    ILog logger = getQualityLogger(BussID);

        //    if (logger.IsInfoEnabled)
        //    {
        //        logger.Info(logMessage);
        //    }

        //    if (logger.IsDebugEnabled)
        //    {
        //        logger.Debug(logMessage);
        //    }
        //}
        #endregion

        #region 错误日志 WriteErrorLog(String BussID, String JobName, String message, String[] args, Exception ex)
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="paramBussID">业务ID(Trans.[XX])</param>
        /// <param name="paramJobName">方法名</param>
        /// <param name="paramMessage">消息内容</param>
        /// <param name="paramArgs">消息参数</param>
        /// <param name="paramEX">异常对象</param>
        public static void WriteErrorLog(String paramBussID, String paramJobName, String paramMessage, String[] paramArgs, Exception paramEX)
        {
            string logMessage = getLogMessage(null, null, paramJobName, paramMessage, paramArgs, null, paramEX);
            ILog logger = getErrorLogger(paramBussID);
            if (logger.IsErrorEnabled)
            {
                logger.Error(logMessage);
            }
            WriteBussLog(paramBussID, "", paramJobName, paramEX.Message + "\r\n" + paramEX.StackTrace, "", null);
        }
        #endregion

        #region 日志内容的取得  private static String getLogMessage(string loginID, string FormName, String JobName, String message, String[] args, String sqlID, Exception ex)
        /// <summary>
        /// GetLogMessage
        /// </summary>
        /// <param name="loginID">登陆用户ID</param>
        /// <param name="FormName">窗体名</param>
        /// <param name="JobName">处理名</param>
        /// <param name="message">消息内容</param>
        /// <param name="args">消息参数</param>
        /// <param name="sqlID">SQLID</param>
        /// <param name="ex">异常</param>
        /// <returns>日志内容</returns>
        private static String getLogMessage(string loginID, string FormName, String JobName, String message, String[] args, String sqlID, Exception ex)
        {
            StringBuilder logInfo = new StringBuilder();

            //登陆用户ID
            if (loginID != null)
            {
                logInfo.Append(loginID + LogConstants.LOG_SPACE);
            }

            //窗体名
            if ((FormName != null) && (!FormName.Equals(string.Empty)))
            {
                logInfo.Append(FormName + LogConstants.LOG_SPACE);
            }
            //处理名
            if (JobName != null)
            {
                logInfo.Append(JobName + LogConstants.LOG_SPACE);
            }
            //消息内容
            logInfo.Append(getAppMessage(message, args) + LogConstants.LOG_SPACE);
            //SQLID
            if (sqlID != null)
            {
                logInfo.Append(sqlID);
            }
            //异常信息
            if (ex != null)
            {
                logInfo.Append(LogConstants.LOG_SPACE);
                logInfo.Append(ex.ToString());
            }

            return logInfo.ToString();
        }
        #endregion

        #endregion

        #region 消息内容取得(不要修改）
        /// <summary>
        ///消息内容取得
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="args">参数</param>
        /// <returns>消息内容</returns>
        private static string getAppMessage(string message, string[] args)
        {
            if (args != null)
            {
                string resultMessage = string.Format(message, args);

                return resultMessage;
            }
            else
            {
                return message;
            }
        }
        #endregion

        #region 业务日志取得 getTransLogger(string BussID)
        /// <summary>
        /// 业务日志取得
        /// </summary>
        /// <param name="BussID">业务ID(Trans.[XX])</param>
        /// <returns>业务日志</returns>
        private static ILog getBusiLogger(string BussID)
        {
            if (BussID.Contains(Trans.COM))
            {
                InitLogProcess(null);
                return comLog;
            }
            if (BussID.Contains(Trans.BS))
            {
                InitLogProcess(null);
                return bsLog;
            }
            if (BussID.Contains(Trans.SM))
            {
                InitLogProcess(null);
                return smLog;
            }
            if (BussID.Contains(Trans.FM))
            {
                InitLogProcess(null);
                return fmLog;
            }
            if (BussID.Contains(Trans.PIS))
            {
                InitLogProcess(null);
                return pisLog;
            }
            if (BussID.Contains(Trans.WC))
            {
                InitLogProcess(null);
                return wcLog;
            }
            if (BussID.Contains(Trans.SD))
            {
                InitLogProcess(null);
                return sdLog;
            }
            if (BussID.Contains(Trans.RPT))
            {
                InitLogProcess(null);
                return rptLog;
            }

            return LogManager.GetLogger("Busi_Log");
        }
        #endregion

        #region 错误日志取得 getErrorLogger
        /// <summary>
        /// 错误日志取得
        /// </summary>
        /// <param name="BussID">业务ID(Trans.[XX])</param>
        /// <returns>错误日志</returns>
        private static ILog getErrorLogger(string BussID)
        {
            if (BussID.Contains(Trans.COM))
            {
                InitLogProcess(null);
                return comErrorLog;
            }
            if (BussID.Contains(Trans.BS))
            {
                InitLogProcess(null);
                return bsErrorLog;
            }
            if (BussID.Contains(Trans.SM))
            {
                InitLogProcess(null);
                return smErrorLog;
            }
            if (BussID.Contains(Trans.FM))

            {
                InitLogProcess(null);
                return fmErrorLog;
            }
            if (BussID.Contains(Trans.PIS))
            {
                InitLogProcess(null);
                return pisErrorLog;
            }
            if (BussID.Contains(Trans.WC))
            {
                InitLogProcess(null);
                return wcErrorLog;
            }
            if (BussID.Contains(Trans.SD))
            {
                InitLogProcess(null);
                return sdErrorLog;
            }
            if (BussID.Contains(Trans.RPT))
            {
                InitLogProcess(null);
                return rptErrorLog;
            }

            return LogManager.GetLogger("Error_Logger");
        }
        #endregion

        #region 日志初始化 void InitWebLogProcess(FileInfo cfile)
        /// <summary>
        /// 日志初始化
        /// </summary>
        /// <param name="cfile">FileInfo</param>
        public static void InitLogProcess(FileInfo cfile)
        {
            if (cfile != null)
            {
                log4net.Config.XmlConfigurator.Configure(cfile);
            }
            else
            {
                log4net.Config.XmlConfigurator.Configure();
            }
            comLog = LogManager.GetLogger(LogConstants.COM_LOG);
            comErrorLog = LogManager.GetLogger(LogConstants.COM_ERROR_LOG);
            bsLog = LogManager.GetLogger(LogConstants.BS_LOG);
            bsErrorLog = LogManager.GetLogger(LogConstants.BS_ERROR_LOG);
            smLog = LogManager.GetLogger(LogConstants.SM_LOG);
            smErrorLog = LogManager.GetLogger(LogConstants.SM_ERROR_LOG);
            fmLog = LogManager.GetLogger(LogConstants.FM_LOG);
            fmErrorLog = LogManager.GetLogger(LogConstants.FM_ERROR_LOG);
            pisLog = LogManager.GetLogger(LogConstants.PIS_LOG);
            pisErrorLog = LogManager.GetLogger(LogConstants.PIS_ERROR_LOG);
            wcLog = LogManager.GetLogger(LogConstants.WC_LOG);
            wcErrorLog = LogManager.GetLogger(LogConstants.WC_ERROR_LOG);
            sdLog = LogManager.GetLogger(LogConstants.SD_LOG);
            sdErrorLog = LogManager.GetLogger(LogConstants.SD_ERROR_LOG);
            rptLog = LogManager.GetLogger(LogConstants.RPT_LOG);
            rptErrorLog = LogManager.GetLogger(LogConstants.RPT_ERROR_LOG);
        }
        #endregion
    }
}
