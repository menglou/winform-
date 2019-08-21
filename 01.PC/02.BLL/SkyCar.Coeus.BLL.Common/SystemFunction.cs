using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.BLL.Common
{
    /// <summary>
    /// 系统方法
    /// </summary>
    public static class SystemFunction
    {
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string settingIniPath);
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string settingIniPath);

        /// <summary>
        /// 基本业务BLL
        /// </summary>
        private static BLLBase _bllBase = new BLLBase(Trans.COM);

        /// <summary>
        /// 从界面中导航到其他界面
        /// </summary>
        /// <param name="paramTabTitle">页面标题</param>
        /// <param name="paramClassFullName">类全名</param>
        /// <param name="paramCanDuplicateShow">是否可以重复打开</param>
        /// <param name="paramPageDisplayMode">页面展现方式</param>
        /// <param name="paramViewPrarmeters">页面间传递参数</param>
        /// <param name="paramResultPrarmeter">返回结果参数</param>
        public static void ShowViewFromView(string paramTabTitle, string paramClassFullName, bool paramCanDuplicateShow, PageDisplayMode paramPageDisplayMode, Dictionary<string, object> paramViewPrarmeters, object paramResultPrarmeter)
        {
            try
            {
                //验证用户是否有菜单权限
                if (paramPageDisplayMode == PageDisplayMode.TabPage 
                    && !HasMenuAuthorityInCurrentOrg(paramClassFullName))
                {
                    //当前用户没有对应菜单的权限，您可以联系管理员授权。
                    _bllBase.ResultMsg = MsgHelp.GetMsg(MsgCode.W_0028, new object[] { MsgParam.CORRESPONDENCE + SystemTableEnums.Name.SM_Menu });
                    return;
                }

                //如果传入空参数，创建参数加入页面呈现方式
                if (paramViewPrarmeters == null)
                {
                    paramViewPrarmeters = new Dictionary<string, object>
                    {
                        { ComViewParamKey.PageDisplayMode.ToString(), paramPageDisplayMode}
                    };
                }
                else if (!paramViewPrarmeters.ContainsKey(ComViewParamKey.PageDisplayMode.ToString()))
                {
                    paramViewPrarmeters.Add(ComViewParamKey.PageDisplayMode.ToString(), paramPageDisplayMode);
                }

                //验证类全名
                if (paramClassFullName.LastIndexOf(SysConst.DOT, StringComparison.Ordinal) < 0)
                {
                    //类全名: paramClassFullName 异常，打开界面失败
                    Exception ex = new Exception(MsgHelp.GetMsg(MsgCode.W_0030, new object[] { SystemTableColumnEnums.SM_MenuDetail.Name.MenuD_ClassFullName + paramClassFullName + MsgParam.EXCEPTION }));
                    _bllBase.ResultMsg = MsgHelp.GetMsg(MsgCode.E_0000, new object[] { ex.Message });
                    return;
                }
                string assemplyName = paramClassFullName.Substring(0,
                    paramClassFullName.LastIndexOf(SysConst.DOT, StringComparison.Ordinal));
                //加载对应的程序集
                Assembly assembly = Assembly.Load(assemplyName);
                if (assembly == null)
                {
                    //程序集assemplyName不存在，打开界面失败
                    Exception ex = new Exception(MsgHelp.GetMsg(MsgCode.W_0030, new object[] { MsgParam.ASSEMBLY + assemplyName + MsgParam.NOTEXIST }));
                    _bllBase.ResultMsg = MsgHelp.GetMsg(MsgCode.E_0000, new object[] { ex.Message });
                    return;
                }
                //获取类型信息
                Type classType = assembly.GetType(paramClassFullName);
                if (classType == null)
                {
                    //类全名:paramClassFullName 不存在，打开界面失败
                    Exception ex = new Exception(MsgHelp.GetMsg(MsgCode.W_0030, new object[] { SystemTableColumnEnums.SM_MenuDetail.Name.MenuD_ClassFullName + paramClassFullName + MsgParam.NOTEXIST }));
                    _bllBase.ResultMsg = MsgHelp.GetMsg(MsgCode.E_0000, new object[] { ex.Message });
                    return;
                }
                //构造方法的参数
                object[] constuctParms = null;
                constuctParms = new object[] { paramViewPrarmeters };

                if (!paramCanDuplicateShow)
                {
                    //菜单不允许重复打开
                    if (!BLLBase.DicWorkForm.ContainsKey(paramClassFullName))
                    {
                        dynamic t = Activator.CreateInstance(classType, constuctParms);
                        var form = (Form)t.Unwrap();

                        BLLBase.DicWorkForm.Add(paramClassFullName, form);

                        form.Text = paramTabTitle;
                        if (paramPageDisplayMode == PageDisplayMode.TabPage)
                        {
                            form.MdiParent = BLLBase.MainFrom;
                        }
                        form.Show();
                    }
                    else
                    {
                        BLLBase.DicWorkForm[paramClassFullName].Focus();
                    }
                }
                else
                {
                    //菜单允许重复打开
                    dynamic t = Activator.CreateInstance(classType, constuctParms);
                    var form = (Form)t;

                    BLLBase.DicWorkForm.Add(Guid.NewGuid().ToString(), form);

                    form.Text = paramTabTitle;
                    if (paramPageDisplayMode == PageDisplayMode.TabPage)
                    {
                        form.MdiParent = BLLBase.MainFrom;
                    }
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                _bllBase.ResultMsg = MsgHelp.GetMsg(MsgCode.E_0000, new object[] { ex.Message });
                return;
            }
        }

        /// <summary>
        /// 用户在当前组织下是否有菜单权限
        /// </summary>
        /// <param name="paramMenuFullName">菜单全名</param>
        /// <returns></returns>
        private static bool HasMenuAuthorityInCurrentOrg(string paramMenuFullName)
        {
            if (string.IsNullOrEmpty(paramMenuFullName))
            {
                return false;
            }
            //SuperAdmin 拥有所有权限
            if (LoginInfoDAX.UserID == SysConst.SUPER_ADMIN)
            {
                return true;
            }

            var authorityResult = (int)_bllBase.QueryForObject(SQLID.COMM_SQL35, new AuthorityVerifyQCModel
            {
                OrgId = LoginInfoDAX.OrgID,
                UserId = LoginInfoDAX.UserID,
                MenuFullName = paramMenuFullName
            });
            if (authorityResult == 0)
            {
                return false;
            }
            return true;
        }

    }

    /// <summary>
    /// 界面呈现方式
    /// </summary>
    public enum PageDisplayMode
    {
        /// <summary>
        /// 标签页方式
        /// </summary>
        TabPage,
        /// <summary>
        /// 对话框方式
        /// </summary>
        DialogWindow
    }
}
