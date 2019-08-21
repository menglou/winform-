using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SkyCar.Coeus.TBModel;

namespace SkyCar.Coeus.Common.Message
{
    /// <summary>
    /// 系统消息类
    /// </summary>
    public class MsgHelp
    {
        #region 变量定义
        /// <summary>
        /// 消息字典
        /// </summary>
        private static Dictionary<string, string> dicMsg = new Dictionary<string, string>();
        #endregion

        #region 自定义方法
        /// <summary>
        /// 初始化系统消息
        /// </summary>
        /// <param name="paramMsgList"></param>
        public static void InitializeMsg(IList<MDLSM_Message> paramMsgList)
        {
            dicMsg = new Dictionary<string, string>();
            foreach (MDLSM_Message md in paramMsgList)
            {
                dicMsg.Add(md.Msg_Code, md.Msg_Content);
            }
        }
        /// <summary>
        /// 通过消息编码，获取消息
        /// </summary>
        /// <param name="paramMsgCode">消息编码（MsgCode.XXX）</param>
        /// <returns></returns>
        public static string GetMsg(MsgCode paramMsgCode)
        {
            return dicMsg[paramMsgCode.ToString()];
        }
        /// <summary>
        /// 通过消息编码和参数，获取消息
        /// </summary>
        /// <param name="paramMsgCode">消息编码（MsgCode.XXX）</param>
        /// <param name="paramMsgParam">消息参数（MsgParam.XXX）</param>
        /// <returns></returns>
        public static string GetMsg(MsgCode paramMsgCode, string paramMsgParam)
        {
            return String.Format(dicMsg[paramMsgCode.ToString()], paramMsgParam);
        }

        /// <summary>
        /// 通过消息编码和参数，获取消息
        /// </summary>
        /// <param name="paramMsgCode">消息编码（MsgCode.XXX）</param>
        /// <param name="paramMsgParam1">消息参数1（MsgParam.XXX）</param>
        /// <param name="paramMsgParam2">消息参数1（MsgParam.XXX）</param>
        /// <returns></returns>
        public static string GetMsg(MsgCode paramMsgCode, string paramMsgParam1,string paramMsgParam2)
        {
            return String.Format(dicMsg[paramMsgCode.ToString()], paramMsgParam1, paramMsgParam2);
        }
        /// <summary>
        /// 通过消息编码和参数，获取消息
        /// </summary>
        /// <param name="paramMsgCode">消息编码（MsgCode.XXX）</param>
        /// <param name="paramMsgParam">消息参数数组（MsgParam.XXX）</param>
        /// <returns></returns>
        public static string GetMsg(MsgCode paramMsgCode, object[] paramMsgParam)
        {
            return String.Format(dicMsg[paramMsgCode.ToString()], paramMsgParam);
        }
        #endregion
    }
}
