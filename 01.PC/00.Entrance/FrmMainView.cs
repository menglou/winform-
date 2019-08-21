using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Windows.Forms;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.SM;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Framework.WindowUI;

namespace SkyCar.Coeus.Ult.Entrance
{
    public partial class FrmMainView : BaseForm
    {
        [DllImport("kernel32")]
        private static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);

        #region 变量
        /// <summary>
        /// 窗体是否激活
        /// </summary>
        private bool _isActivated = false;
        /// <summary>
        /// 登陆窗体
        /// </summary>
        FrmLogin _flg = new FrmLogin();
        /// <summary>
        /// 业务逻辑对象【固定】
        /// </summary>
        MenuBLL _bll = new MenuBLL();
        /// <summary>
        /// 菜单
        /// </summary>
        BindingList<MenuGroupActionUIModel> _menuList = new BindingList<MenuGroupActionUIModel>();
        /// <summary>
        /// 菜单组
        /// </summary>
        BindingList<MenuGroupActionUIModel> _menuGroupList = new BindingList<MenuGroupActionUIModel>();
        /// <summary>
        /// 菜单明细
        /// </summary>
        BindingList<MenuGroupActionUIModel> _menuDetailList = new BindingList<MenuGroupActionUIModel>();
        /// <summary>
        /// 当前已打开的Form
        /// </summary>
        public Dictionary<string, Form> _dicWorkForm = new Dictionary<string, Form>();
        /// <summary>
        /// 同一窗体是否允许多开
        /// </summary>
        bool isAllowMuiltRun = true;
        #endregion
        public FrmMainView()
        {
            InitializeComponent();

        }

        private void SCAMain_Load(object sender, EventArgs e)
        {
            //主窗体
            BLLBase.MainFrom = this;
            //当前已打开的Form
            BLLBase.DicWorkForm = _dicWorkForm;

            //初始化菜单
            CreateMenu();

            //初始化状态栏
            InitializeStatusBar();

            Infragistics.Shared.ResourceCustomizer rc = Infragistics.Win.UltraWinToolbars.Resources.Customizer; 
            rc.SetCustomizedString("MinimizeRibbon", "最小化菜单");
            rc.SetCustomizedString("QuickAccessToolbarBelowRibbon", "显示快速访问工具栏在下方");
            rc.SetCustomizedString("QuickAccessToolbarAboveRibbon", "显示快速访问工具栏在上方");

            #region 获取系统参数（同一窗体是否允许多开）
            isAllowMuiltRun = false;
            //List<MDLSM_Parameter> parameterList = CacheDAX.Get(CacheDAX.CacheKey.S1001) as List<MDLSM_Parameter>;

            //if (parameterList==null || parameterList.Count <= 0)
            //{
            //    MessageBoxs.Show(Trans.Com, this.ToString(), "系统参数：同一窗体是否允许多开 初始化失败（未获取到相关系统配置）！\r\n自动设置默认值：禁止同一窗体多开！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    isAllowMuiltRun = false;
            //}
            //else
            //{
            //    if (!bool.TryParse(parameterList[0].Para_Value1, out isAllowMuiltRun))
            //    {
            //        MessageBoxs.Show(Trans.Com, this.ToString(), "系统参数：同一窗体是否允许多开 初始化失败（系统配置数据类型错误）！\r\n自动设置默认值：禁止同一窗体多开！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        isAllowMuiltRun = false;
            //    }
            //}
            #endregion

            foreach (Control ctl in this.Controls)
            {
                try
                {
                    // Attempt to cast the control to type MdiClient.
                    MdiClient mdiClient = (MdiClient)ctl;

                    // Set the BackColor of the MdiClient control.
                    mdiClient.BackColor = this.BackColor;
                }
                catch (InvalidCastException ex)
                {
                    // Catch and ignore the error if casting failed.
                }
            }
            this.statusBar.Panels["lblCopyRight"].Text = SystemConfigInfo.CopyRightDesc;

            //ShowForm("SkyCar.Coeus.Ult.Entrance.FrmWelcome");
            this.Invalidate(true);
        }

        /// <summary>
        /// 初始化菜单
        /// </summary>
        private void CreateMenu()
        {
            try
            {
                #region 根据登录用户获取对应系统权限

                var argsMenuGroupActionQCModel = new MenuGroupActionQCModel();
                if (LoginInfoDAX.UserEMPNO != SysConst.SUPER_ADMIN)
                {
                    argsMenuGroupActionQCModel.WHERE_Org_ID = LoginInfoDAX.OrgID;
                    argsMenuGroupActionQCModel.WHERE_User_ID = LoginInfoDAX.UserID;
                }
                var menuGroupActionResultList = new List<MenuGroupActionUIModel>();
                //查询用户菜单和动作权限
                _bll.QueryForList(SQLID.SM_Menu_SQL01, argsMenuGroupActionQCModel, menuGroupActionResultList);
                if (menuGroupActionResultList.Count == 0) return;
                //将用户菜单动作权限保存到缓存；
                CacheDAX.Add(CacheDAX.ConfigDataKey.SystemUserAuthority, menuGroupActionResultList, true);
                foreach (var loopMga in menuGroupActionResultList)
                {
                    if (_menuList.FirstOrDefault(x => x.Menu_ID == loopMga.Menu_ID) == null)
                    {
                        _menuList.Add(loopMga);
                    }
                    if (_menuGroupList.FirstOrDefault(x => x.Menu_ID == loopMga.Menu_ID && x.MenuG_ID == loopMga.MenuG_ID) == null)
                    {
                        _menuGroupList.Add(loopMga);
                    }
                    if (_menuDetailList.FirstOrDefault(x => x.MenuD_ID == loopMga.MenuD_ID) == null)
                    {
                        _menuDetailList.Add(loopMga);
                    }
                }

                #endregion

                #region 加菜单

                if (_menuList.Count == 0)
                {
                    return;
                }
                //将当前用户菜单信息保存到缓存
                CacheDAX.Add(CacheDAX.CacheKey.U1001, _menuDetailList, true);
                foreach (var loopMenu in _menuList)
                {
                    //添加菜单
                    var ribbonTabMenuName = new RibbonTab(loopMenu.Menu_Code)
                    {
                        //菜单标题
                        Caption = loopMenu.Menu_Name,
                        Key = loopMenu.Menu_Code,
                    };
                    ribbonTabMenuName.Settings.Appearance.FontData.SizeInPoints = 10.5f;

                    //获取当前菜单下的菜单组
                    var menuGroupListTemp = _menuGroupList.Where(p => p.Menu_ID == loopMenu.Menu_ID).OrderBy(p => p.MenuG_Index).ToList();
                    if (menuGroupListTemp.Count == 0)
                    {
                        return;
                    }

                    foreach (var loopMenuGroup in menuGroupListTemp)
                    {
                        //添加菜单组
                        var menuGroup = new RibbonGroup(loopMenuGroup.MenuG_Code)
                        {
                            //菜单组标题
                            Caption = loopMenuGroup.MenuG_Name
                        };

                        menuGroup.Settings.Appearance.FontData.SizeInPoints = 9;
                        //获取当前菜单下的菜单组明细
                        #region 当前菜单下的菜单组明细
                        var menuDetail = _menuDetailList.Where(p => p.Menu_ID == loopMenu.Menu_ID && p.MenuG_ID == loopMenuGroup.MenuG_ID).OrderBy(p => p.MenuD_Index).ToList();
                        if (menuDetail.Count == 0)
                        {
                            return;
                        }
                        foreach (var loopMenuDetail in menuDetail)
                        {
                            //添加菜单组明细
                            var buttonToolMenuDetail = new ButtonTool(loopMenuDetail.MenuD_Code);
                            var buttonToolMenuDetailImage = new ButtonTool(loopMenuDetail.MenuD_Code);
                            //设置图片                                                       
                            var appearanceMenuDetail = new Infragistics.Win.Appearance();
                            if (imgForm.Images.Keys.IndexOf(loopMenuDetail.MenuD_ImgListKey) >= 0)
                            {
                                appearanceMenuDetail.Image = imgForm.Images[imgForm.Images.Keys.IndexOf(loopMenuDetail.MenuD_ImgListKey)];
                            }
                            //ImageList.ImageCollection
                            //ImageList
                            buttonToolMenuDetailImage.SharedPropsInternal.AppearancesLarge.Appearance = appearanceMenuDetail;
                            //菜单组明细标题 
                            buttonToolMenuDetailImage.SharedPropsInternal.Caption = loopMenuDetail.MenuD_Name;
                            //设置Key
                            buttonToolMenuDetailImage.Key = loopMenuDetail.MenuD_ClassFullName;
                            buttonToolMenuDetailImage.InstanceProps.PreferredSizeOnRibbon = RibbonToolSize.Large;
                            //菜单组明细标题   
                            buttonToolMenuDetail.InstanceProps.PreferredSizeOnRibbon = RibbonToolSize.Large;
                            toolbars.Tools.AddRange(new ToolBase[] { buttonToolMenuDetail });
                            menuGroup.Tools.AddRange(new ToolBase[] { buttonToolMenuDetailImage });

                            //Click事件
                            buttonToolMenuDetail.ToolClick += toolbars_ToolClick;

                        }

                        ribbonTabMenuName.Groups.AddRange(new RibbonGroup[] { menuGroup });
                        #endregion
                    }
                    toolbars.Ribbon.NonInheritedRibbonTabs.AddRange(new RibbonTab[] { ribbonTabMenuName });
                }

                #endregion
            }
            catch (Exception ex)
            {
                MessageBoxs.Show(Trans.COM, this.ToString(), ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        /// <summary>
        /// 初始化状态栏
        /// </summary>
        private void InitializeStatusBar()
        {
            statusBar.Panels["Version"].Text = SysConst.VersionNo;
            statusBar.Panels["OrgName"].Text = LoginInfoDAX.OrgShortName;
            statusBar.Panels["UserName"].Text = LoginInfoDAX.UserName;
  
        }

        private void toolbars_ToolClick(object sender, ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "Exit":
                    Application.Exit();
                    break;
                case "logoff":
                    this.DialogResult = DialogResult.Yes;
                    this.Close();
                    if (_isCloseCurLogin)
                    {
                        _flg.Show();
                    }
                    _isCloseCurLogin = true;
                    break;
                case "UpdatePW":
                    var frmupdatepw = new FrmModifyPassword(this);
                    frmupdatepw.Show();
                    break;
                default:
                    ShowForm(e.Tool.Key);
                    break;
            }

        }
        private void ShowForm(string paramKey)
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                }

                #region 限制打开界面数
                int maxViewCount = 15;
                if (_dicWorkForm.Count == maxViewCount)
                {
                    MessageBoxs.Show(Trans.COM, this.ToString(), "为了保证用户体验，只能打开 " + maxViewCount + " 个界面，请关闭不常用的界面后再打开新界面。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                #endregion

                if (!isAllowMuiltRun)
                {
                    if (!_dicWorkForm.ContainsKey(paramKey))
                    {
                        ObjectHandle t = Activator.CreateInstance(paramKey.Substring(0, paramKey.IndexOf(".Frm", StringComparison.Ordinal)), paramKey);
                        var form = (Form)t.Unwrap();
                        _dicWorkForm.Add(paramKey, form);

                        form.MdiParent = this;
                        form.Show();
                    }
                    else
                    {
                        _dicWorkForm[paramKey].Focus();
                    }
                }
                else
                {
                    ObjectHandle t = Activator.CreateInstance(paramKey.Substring(0, paramKey.IndexOf(".Frm", StringComparison.Ordinal)), paramKey);
                    var form = (Form)t.Unwrap();

                    _dicWorkForm.Add(Guid.NewGuid().ToString(), form);
                    form.MdiParent = this;
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBoxs.Show(Trans.COM, this.ToString(), ex.Message, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                return;
            }
        }

        private void SCAMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != System.Windows.Forms.DialogResult.Yes)
            {
                DialogResult dr = MessageBoxs.Show(Trans.COM, this.ToString(), "确定退出系统？", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (DialogResult.OK != dr)
                {
                    e.Cancel = true;
                }
                else
                {
                    Application.ExitThread();
                }
            }
        }

        private void tabManager_InitializeContextMenu(object sender, Infragistics.Win.UltraWinTabbedMdi.MdiTabContextMenuEventArgs e)
        {
            if (e.ContextMenuType == Infragistics.Win.UltraWinTabbedMdi.MdiTabContextMenu.Default)
            {
                e.ContextMenu.MenuItems.Clear();
                var closeMenuItem = new Infragistics.Win.IGControls.IGMenuItem("关闭")
                {
                    Tag = e.Tab
                };
                closeMenuItem.Click += new EventHandler(OnCustomMenuItemClose);

                var closeAllMenuItem = new Infragistics.Win.IGControls.IGMenuItem("关闭所有窗体") { Tag = e.Tab };
                closeAllMenuItem.Click += new EventHandler(OnCustomMenuItemClose);

                var closeAllExpOneMenuItem = new Infragistics.Win.IGControls.IGMenuItem("除此之外全部关闭") { Tag = e.Tab };
                closeAllExpOneMenuItem.Click += new EventHandler(OnCustomMenuItemClose);

                e.ContextMenu.MenuItems.Add(closeMenuItem);
                e.ContextMenu.MenuItems.Add(closeAllMenuItem);
                e.ContextMenu.MenuItems.Add(closeAllExpOneMenuItem);
            }
        }
        private void OnCustomMenuItemClose(object sender, EventArgs e)
        {
            Infragistics.Win.UltraWinTabbedMdi.MdiTab tab = null;

            Infragistics.Win.IGControls.IGMenuItem mi = sender as Infragistics.Win.IGControls.IGMenuItem;
            switch (mi.Index)
            {
                case 0:
                    tab = mi.Tag as Infragistics.Win.UltraWinTabbedMdi.MdiTab;
                    tab.Close();
                    break;
                case 1:
                    do { tabManager.TabGroups[0].Tabs[0].Close(); } while (tabManager.TabGroups.Count != 0);
                    break;
                case 2:
                    tab = mi.Tag as Infragistics.Win.UltraWinTabbedMdi.MdiTab;
                    do
                    {
                        if (tabManager.TabGroups[0].Tabs[0].Key != tab.Key)
                        {
                            tabManager.TabGroups[0].Tabs[0].Close();
                        }
                        else if (tabManager.TabGroups[0].Tabs.Count > 1)
                        {
                            tabManager.TabGroups[0].Tabs[1].Close();
                        }

                    } while (tabManager.TabGroups[0].Tabs.Count != 1);
                    break;
            }

        }

        private void tabManager_InitializeTab(object sender, Infragistics.Win.UltraWinTabbedMdi.MdiTabEventArgs e)
        {
            e.Tab.Key = Guid.NewGuid().ToString();
            //e.Tab.Settings.CloseButtonVisibility = TabCloseButtonVisibility.WhenSelectedOrHotTracked;
        }

        private bool _isCloseCurLogin = true;
        /// <summary>
        /// 关闭画面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabManager_TabClosing(object sender, Infragistics.Win.UltraWinTabbedMdi.CancelableMdiTabEventArgs e)
        {
            if (_dicWorkForm.ContainsKey(e.Tab.Form.GetType().FullName))
            {
                dynamic curForm = _dicWorkForm[e.Tab.Form.GetType().FullName];
                if (curForm != null)
                {
                    if (curForm.ViewHasChangedWhenClose())
                    {
                        //信息尚未保存，确定进行当前操作？
                        DialogResult dialogResult = MessageBoxs.Show(Trans.COM, ToString(), MsgHelp.GetMsg(MsgCode.W_0001), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dialogResult != DialogResult.OK)
                        {
                            e.Cancel = true;
                            _isCloseCurLogin = false;
                            return;
                        }
                    }
                }

                _dicWorkForm.Remove(e.Tab.Form.GetType().FullName);
            }
        }

        private void FrmMainView_Activated(object sender, EventArgs e)
        {
            if (!_isActivated)
            {
                _isActivated = true;
                this.Invalidate(true);
            }
        }

        private void FrmMainView_Deactivate(object sender, EventArgs e)
        {
            if (_isActivated)
            {
                _isActivated = false;
                this.Invalidate(true);
            }
        }

        private void FrmMainView_SizeChanged(object sender, EventArgs e)
        {
            SystemConfigInfo.MainWindowWidth = this.Width;
            SystemConfigInfo.MainWindowHeight = this.Height;
        }
    }
}
