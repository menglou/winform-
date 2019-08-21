using System.Windows.Forms;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;

namespace SkyCar.Coeus.UI.Common
{
    /// <summary>
    /// 弹出消息
    /// </summary>
    public class MessageBoxs
    {
        /// <summary>
        /// 弹出消息框（指定MessageBoxButtons和MessageBoxIcon）
        /// </summary>
        /// <param name="paramBussID">业务ID[SkyCar.Coeus.BLL.Common.Trans.XX]</param>
        /// <param name="paramOwner">发出消息对象的名称</param>
        /// <param name="paramMsg">消息内容</param>
        /// <param name="paramCaption">消息标题</param>
        /// <param name="paramMsgBtn">消息按钮类型</param>
        /// <param name="paramMsgBtnIcon">消息按钮图像类型</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string paramBussID,string paramOwner, string paramMsg, string paramCaption, MessageBoxButtons paramMsgBtn, MessageBoxIcon paramMsgBtnIcon)
        {
            LogHelper.WriteBussLog(paramBussID, LoginInfoDAX.UserName, paramOwner, paramMsg, "", null);

            return MessageBox.Show(paramMsg, paramCaption, paramMsgBtn, paramMsgBtnIcon);
        }
        /// <summary>
        /// 弹出消息框
        /// </summary>
        /// <param name="paramBussID">业务ID[SkyCar.Coeus.BLL.Common.Trans.XX]</param>
        /// <param name="paramOwner">发出消息对象的名称</param>
        /// <param name="paramMsg">消息内容</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string paramBussID, string paramOwner, string paramMsg)
        {
            LogHelper.WriteBussLog(paramBussID, LoginInfoDAX.UserName, paramOwner, paramMsg, "", null);

            return MessageBox.Show(paramMsg, "Message");
        }
    }
}
