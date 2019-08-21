using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting;
using System.Windows.Forms;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.SM;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Common;

namespace SkyCar.Coeus.Ult.Entrance
{
    public partial class SupplierMain : Form
    {
        #region 变量
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
        BindingList<MDLSM_Menu> _menuList = new BindingList<MDLSM_Menu>();
        /// <summary>
        /// 菜单组
        /// </summary>
        BindingList<MDLSM_MenuGroup> _menuGroupList = new BindingList<MDLSM_MenuGroup>();
        /// <summary>
        /// 菜单明细
        /// </summary>
        BindingList<MDLSM_MenuDetail> _menuDetailList = new BindingList<MDLSM_MenuDetail>();
        /// <summary>
        /// 当前已打开的Form
        /// </summary>
        Dictionary<string, Form> _dicWorkForm = new Dictionary<string, Form>();
        /// <summary>
        /// 同一窗体是否允许多开
        /// </summary>
        bool isAllowMuiltRun = true;
        #endregion
        public SupplierMain()
        {
            InitializeComponent();

        }

        private void SCAMain_Load(object sender, EventArgs e)
        {
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
            //    MessageBoxs.Show(Trans.Com, this.ToString(), "系统参数：同一窗体是否允许多开 初始化失败（未获取到相关系统配置）！\r\n自动设置默认值：禁止同一窗体多开！", "消息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    isAllowMuiltRun = false;
            //}
            //else
            //{
            //    if (!bool.TryParse(parameterList[0].Para_Value1, out isAllowMuiltRun))
            //    {
            //        MessageBoxs.Show(Trans.Com, this.ToString(), "系统参数：同一窗体是否允许多开 初始化失败（系统配置数据类型错误）！\r\n自动设置默认值：禁止同一窗体多开！", "消息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        isAllowMuiltRun = false;
            //    }
            //}
            #endregion

        }

        /// <summary>
        /// 初始化菜单
        /// </summary>
        private void CreateMenu()
        {
            #region 查询
            if (LoginInfoDAX.UserEMPNO == SysConst.SUPER_ADMIN)
            {
                //获取系统菜单
                var argsMenuQuery = new MDLSM_Menu { WHERE_Menu_IsValid = true };
                _bll.QuerryForList<MDLSM_Menu>(argsMenuQuery, _menuList);
                //获取系统菜单分组
                var argsMenuGroupQuery = new MDLSM_MenuGroup { WHERE_MenuG_IsValid = true };
                _bll.QuerryForList<MDLSM_MenuGroup>(argsMenuGroupQuery, _menuGroupList);
                //获取系统菜单明细
                var argsMenuDetailQuery = new MDLSM_MenuDetail { WHERE_MenuD_IsValid = true };
                _bll.QuerryForList<MDLSM_MenuDetail>(argsMenuDetailQuery, _menuDetailList);
            }
            else
            {
                var argsLoginModelQuery = new ComLoginModel { User_EMPNO = LoginInfoDAX.UserEMPNO };
                //获取菜单
                _bll.QuerryForList<MDLSM_Menu>(SQLID.Menu_SQL03, argsLoginModelQuery, _menuList);
                //获取菜单组
                _bll.QuerryForList<MDLSM_MenuGroup>(SQLID.Menu_SQL02, argsLoginModelQuery, _menuGroupList);
                //获取菜单明细
                _bll.QuerryForList<MDLSM_MenuDetail>(SQLID.Menu_SQL01, argsLoginModelQuery, _menuDetailList);
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
                var RibbonTabMenuName = new RibbonTab(loopMenu.Menu_Code)
                {
                    //菜单标题
                    Caption = loopMenu.Menu_Name,
                    Key = loopMenu.Menu_Code
                };

                //获取当前菜单下的菜单组
                var menuGroupListTemp = _menuGroupList.Where(p => p.MenuG_Menu_ID == loopMenu.Menu_ID).OrderBy(p => p.MenuG_Index).ToList();
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

                    //获取当前菜单下的菜单组明细
                    #region 当前菜单下的菜单组明细
                    var menuDetail = _menuDetailList.Where(p => p.MenuD_Menu_ID == loopMenu.Menu_ID && p.MenuD_MenuG_ID == loopMenuGroup.MenuG_ID).OrderBy(p => p.MenuD_Index).ToList();
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

                    RibbonTabMenuName.Groups.AddRange(new RibbonGroup[] { menuGroup });
                    #endregion
                }
                toolbars.Ribbon.NonInheritedRibbonTabs.AddRange(new RibbonTab[] { RibbonTabMenuName });
            }

            #endregion
        }
        /// <summary>
        /// 初始化状态栏
        /// </summary>
        private void InitializeStatusBar()
        {
            statusBar.Panels["Version"].Text = SysConst.VersionNo;
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
                    this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                    this.Close();
                    _flg.Show();
                    break;
                case "UpdatePW":
                    var frmupdatepw = new FrmUpdatePW(this);
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
                MessageBoxs.Show(Trans.COM, this.ToString(), ex.Message, "发现异常", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                return;
            }
        }

        private void SCAMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != System.Windows.Forms.DialogResult.Yes)
            {
                DialogResult dr = MessageBoxs.Show(Trans.COM, this.ToString(), "您即将退出本系统，是否继续关闭？", "消息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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
        }

        private void tabManager_TabClosing(object sender, Infragistics.Win.UltraWinTabbedMdi.CancelableMdiTabEventArgs e)
        {
            _dicWorkForm.Remove(e.Tab.Form.GetType().FullName);
        }

    }
}
