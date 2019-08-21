using System;
using System.Windows.Forms;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Common;
using SkyCar.Common;
using SkyCar.Common.Utility;
using SkyCar.Framework.WindowUI;

namespace SkyCar.Coeus.Ult.Entrance
{
    public partial class FrmModifyPassword : BaseForm
    {
        #region 变量
        /// <summary>
        /// 登陆窗体
        /// </summary>
        readonly FrmMainView _supplierMain = new FrmMainView();
        /// <summary>
        /// 登陆窗体
        /// </summary>
        FrmLogin _flg = new FrmLogin();
        /// <summary>
        /// 定义用户业务逻辑的对象
        /// </summary>
        LoginBLL _bll = new LoginBLL();
        #endregion

        public FrmModifyPassword(FrmMainView paramSupplierMain)
        {
            _supplierMain = paramSupplierMain;
            InitializeComponent();
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtOriginalPassword.Text.Trim() == string.Empty)
                {
                    MessageBoxs.Show(Trans.COM, this.ToString(), "原密码不可为空", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!LoginInfoDAX.UserPassword.Equals(CryptoHelp.EncodeToMD5(txtOriginalPassword.Text.Trim())))
                {
                    MessageBoxs.Show(Trans.COM, this.ToString(), "原密码错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtNewPassword.Text.Trim() == string.Empty)
                {
                    MessageBoxs.Show(Trans.COM, this.ToString(), "新密码不可为空", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtNewPassword.Text.Trim() != txtSurePassword.Text.Trim())
                {
                    MessageBoxs.Show(Trans.COM, this.ToString(), "新密码和确认密码不一致", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DialogResult dr = MessageBoxs.Show(Trans.COM, this.ToString(), "您将要修改密码！\r\n单击【确定】继续，【取消】返回。", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr != DialogResult.OK)
                {
                    return;
                }
                MDLSM_User userQuery = new MDLSM_User();
                userQuery.WHERE_User_ID = LoginInfoDAX.UserID;
                MDLSM_User userResult = new MDLSM_User();
                _bll.QueryForObject<MDLSM_User, MDLSM_User>(userQuery, userResult);
                userResult.WHERE_User_ID = LoginInfoDAX.UserID;
                userResult.WHERE_User_VersionNo = userResult.User_VersionNo;
                userResult.User_Password = CryptoHelp.EncodeToMD5(txtNewPassword.Text.Trim());
                userResult.User_VersionNo += 1;
                bool boolTmp = _bll.Save<MDLSM_User>(userResult);
                if (!boolTmp)
                {
                    MessageBoxs.Show(Trans.COM, this.ToString(), "保存失败！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    MessageBoxs.Show(Trans.COM, this.ToString(), "保存成功！", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                    _supplierMain.DialogResult = DialogResult.Yes;
                    _supplierMain.Close();
                    _flg.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBoxs.Show(Trans.COM, this.ToString(), ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.WriteErrorLog(Trans.COM, System.Reflection.MethodBase.GetCurrentMethod().ToString() + SysConst.ENTER + ex.StackTrace, ex.Message, null, ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
