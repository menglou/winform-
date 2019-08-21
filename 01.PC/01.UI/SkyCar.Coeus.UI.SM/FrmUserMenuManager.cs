using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.SM;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.SM;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.UIModel.SM.UIModel;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UI.SM
{
    /// <summary>
    /// 用户菜单管理（组织）
    /// </summary>
    public partial class FrmUserMenuManager : BaseFormCardList<UserMenuManagerUIModel, UserMenuManagerQCModel, MDLSM_UserMenuAuthority>
    {
        #region 全局变量

        /// <summary>
        /// 用户菜单管理（组织）BLL
        /// </summary>
        private UserMenuManagerBLL _bll = new UserMenuManagerBLL();

        /// <summary>
        /// 用户菜单明细授权数据源
        /// </summary>
        private SkyCarBindingList<UserMenuAuthorityUIModel, MDLSM_UserMenuAuthority> _userMenuAuthoritiyList =
            new SkyCarBindingList<UserMenuAuthorityUIModel, MDLSM_UserMenuAuthority>();
        /// <summary>
        /// 用户菜单明细动作授权数据源
        /// </summary>
        private SkyCarBindingList<UserActionAuthorityUIModel, MDLSM_UserActionAuthority> _userActionAuthoritiyList =
            new SkyCarBindingList<UserActionAuthorityUIModel, MDLSM_UserActionAuthority>();

        /// <summary>
        /// 用户作业授权数据源
        /// </summary>
        private SkyCarBindingList<UserJobAuthorityUIModel, MDLSM_UserJobAuthority> _userJobAuthoritiyList =
            new SkyCarBindingList<UserJobAuthorityUIModel, MDLSM_UserJobAuthority>();

        /// <summary>
        /// 是否触发选中菜单动作TreeView后的AfterCheck事件
        /// </summary>
        private bool _isCanAfterCheckOfMenu = true;

        /// <summary>
        /// 是否触发选中作业TreeView后的AfterCheck事件
        /// </summary>
        private bool _isCanAfterCheckOfJob = true;

        #region 下拉框数据源

        /// <summary>
        /// 用户名数据源
        /// </summary>
        List<MDLSM_User> _userList = new List<MDLSM_User>();

        #endregion

        #endregion

        #region 公共属性
        #endregion

        #region 系统事件

        /// <summary>
        /// FrmUserMenuManager构造方法
        /// </summary>
        public FrmUserMenuManager()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmUserMenuManager_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（动作）
            base.ToolBarActionAndNavigate = toolBarAction;
            //查询委托（基类控制翻页用）
            base.ExecuteQuery = QueryAction;
            //工具栏（动作）单击事件
            this.toolBarAction.ToolClick += new ToolClickEventHandler(base.toolBarActionAndNavigate_ToolClick);
           
            //初始化【详情】Tab内控件
            InitializeDetailTabControls();
            #endregion
        }

        /// <summary>
        /// 【菜单动作授权】选中TreeView节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvUserMenuAction_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (!_isCanAfterCheckOfMenu)
            {
                return;
            }
            //考虑三种情况 第一：点击菜单  第二：点击菜单明细 第三：点击菜单明细动作
            TreeNode checkNode = e.Node;
            if (checkNode == null)
            {
                return;
            }
            if (checkNode.Level == 0)
            {
                #region 点击菜单的场合

                //该[菜单]下所有[菜单明细]的勾选状态与该[菜单]一致
                foreach (TreeNode loopMenuDetailTreeNode in checkNode.Nodes)
                {
                    loopMenuDetailTreeNode.Checked = checkNode.Checked;
                }

                #endregion
            }
            else if (checkNode.Level == 1)
            {
                #region 点击菜单明细的场合【新增/删除用户菜单明细授权】

                if (checkNode.Checked)
                {
                    #region 新增用户菜单明细授权

                    //过滤重复的[用户菜单明细授权]
                    if (!_userMenuAuthoritiyList.Any(x => x.UMA_User_ID == mcbUserName.SelectedValue
                    && x.UMA_Org_ID == LoginInfoDAX.OrgID
                    && x.UMA_MenuD_ID == checkNode.Tag.ToString()))
                    {
                        _userMenuAuthoritiyList.Add(new UserMenuAuthorityUIModel()
                        {
                            UMA_IsValid = true,
                            UMA_User_ID = mcbUserName.SelectedValue,
                            UMA_Org_ID = LoginInfoDAX.OrgID,
                            UMA_MenuD_ID = checkNode.Tag.ToString(),
                            UMA_CreatedBy = LoginInfoDAX.UserName,
                            UMA_UpdatedBy = LoginInfoDAX.UserName
                        });
                    }

                    #endregion
                }
                else
                {
                    #region 删除用户菜单明细授权

                    List<UserMenuAuthorityUIModel> deleteUserMenuAuthorityList =
                        _userMenuAuthoritiyList.Where(p => p.UMA_Org_ID == LoginInfoDAX.OrgID
                                                           && p.UMA_User_ID == mcbUserName.SelectedValue
                                                           && p.UMA_MenuD_ID == checkNode.Tag.ToString()).ToList();
                    if (deleteUserMenuAuthorityList.Count > 0)
                    {
                        _userMenuAuthoritiyList.Remove(deleteUserMenuAuthorityList[0]);
                    }

                    #endregion
                }

                //该[菜单明细]下所有[菜单明细动作]的勾选状态与该[菜单明细]一致
                foreach (TreeNode loopMenuActionTreeNode in checkNode.Nodes)
                {
                    loopMenuActionTreeNode.Checked = checkNode.Checked;
                }

                #endregion
            }
            else if (checkNode.Level == 2)
            {
                #region 点击菜单明细动作的场合【新增/删除用户菜单明细动作授权】

                if (checkNode.Checked)
                {
                    #region 新增用户菜单明细动作授权

                    //过滤重复的[用户菜单明细动作授权]
                    if (!_userActionAuthoritiyList.Any(x => x.UAA_User_ID == mcbUserName.SelectedValue
                    && x.UAA_Org_ID == LoginInfoDAX.OrgID
                    && x.UAA_MenuD_ID == checkNode.Parent.Tag.ToString()
                    && x.UAA_Action_ID == checkNode.Tag.ToString()))
                    {
                        _userActionAuthoritiyList.Add(new UserActionAuthorityUIModel()
                        {
                            UAA_IsValid = true,
                            UAA_User_ID = mcbUserName.SelectedValue,
                            UAA_Org_ID = LoginInfoDAX.OrgID,
                            UAA_MenuD_ID = checkNode.Parent.Tag.ToString(),
                            UAA_Action_ID = checkNode.Tag.ToString(),
                            UAA_CreatedBy = LoginInfoDAX.UserName,
                            UAA_UpdatedBy = LoginInfoDAX.UserName
                        });
                    }

                    #endregion
                }
                else
                {
                    #region 删除用户菜单明细动作授权

                    List<UserActionAuthorityUIModel> deleteUserActionAuthorityList =
                        _userActionAuthoritiyList.Where(p => p.UAA_Org_ID == LoginInfoDAX.OrgID
                                                             && p.UAA_User_ID == mcbUserName.SelectedValue
                                                             && p.UAA_MenuD_ID == checkNode.Parent.Tag.ToString()
                                                             && p.UAA_Action_ID == checkNode.Tag.ToString()).ToList();
                    if (deleteUserActionAuthorityList.Count > 0)
                    {
                        _userActionAuthoritiyList.Remove(deleteUserActionAuthorityList[0]);
                    }

                    #endregion
                }

                #endregion
            }
        }

        /// <summary>
        /// 【订阅作业结果】选中TreeView节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvUserJob_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (!_isCanAfterCheckOfJob)
            {
                return;
            }
            TreeNode checkNode = e.Node;
            if (checkNode == null)
            {
                return;
            }
            //if (checkNode.Level == 0)
            {
                if (checkNode.Checked)
                {
                    #region 新增用户作业授权

                    //过滤重复的[用户作业授权]
                    if (!_userJobAuthoritiyList.Any(x => x.UJA_User_ID == mcbUserName.SelectedValue
                    && x.BJ_ID == checkNode.Tag.ToString()))
                    {
                        _userJobAuthoritiyList.Add(new UserJobAuthorityUIModel()
                        {
                            UJA_IsValid = true,
                            UJA_User_ID = mcbUserName.SelectedValue,
                            UJA_BJ_ID = checkNode.Tag.ToString(),
                            UJA_CreatedBy = LoginInfoDAX.UserName,
                            UJA_UpdatedBy = LoginInfoDAX.UserName
                        });
                    }

                    #endregion
                }
                else
                {
                    #region 删除用户作业授权

                    List<UserJobAuthorityUIModel> deleteUserJobList =
                        _userJobAuthoritiyList.Where(
                            p => p.UJA_User_ID == mcbUserName.SelectedValue && p.BJ_ID == checkNode.Tag.ToString())
                            .ToList();
                    if (deleteUserJobList.Count > 0)
                    {
                        _userJobAuthoritiyList.Remove(deleteUserJobList[0]);
                    }

                    #endregion
                }
            }
        }

        #endregion

        #region 重写基类方法

        /// <summary>
        /// 清空
        /// </summary>
        public override void ClearAction()
        {
            //2.初始化【详情】Tab内控件
            InitializeDetailTabControls();
        }

        /// <summary>
        /// 关闭画面时检查画面值是否发生变化
        /// </summary>
        /// <returns></returns>
        public override bool ViewHasChangedWhenClose()
        {
            if (_userMenuAuthoritiyList.InsertList.Count > 0
                || _userMenuAuthoritiyList.UpdateList.Count > 0
                || _userMenuAuthoritiyList.DeleteList.Count > 0
                || _userActionAuthoritiyList.InsertList.Count > 0
                || _userActionAuthoritiyList.UpdateList.Count > 0
                || _userActionAuthoritiyList.DeleteList.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public override void SaveAction()
        {
            //1.前端检查
            if (!ClientCheck())
            {
                return;
            }

            //保存数据
            bool saveResult = _bll.SaveUserMenu(mcbUserName.SelectedValue, _userMenuAuthoritiyList, _userActionAuthoritiyList, _userJobAuthoritiyList);
            if (!saveResult)
            {
                //保存失败
                MessageBoxs.Show(Trans.SM, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            _userMenuAuthoritiyList.StartMonitChanges();
            _userActionAuthoritiyList.StartMonitChanges();
            _userJobAuthoritiyList.StartMonitChanges();

            //获取最新的用户菜单明细授权和用户菜单明细动作授权进行绑定
            SetUserMenuAndActionInfo();
            //获取最新的用户作业授权进行绑定
            SetUserJobInfo();
        }

        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            //请选择用户姓名
            if (string.IsNullOrEmpty(mcbUserName.SelectedText)
                || string.IsNullOrEmpty(mcbUserName.SelectedValue))
            {
                MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { SystemTableColumnEnums.SM_User.Name.User_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //获取最新的用户菜单明细授权和用户菜单明细动作授权进行绑定
            SetUserMenuAndActionInfo();
            //获取最新的用户作业授权进行绑定
            SetUserJobInfo();
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 初始化下拉框

            //用户名
            _userList = CacheDAX.Get(CacheDAX.ConfigDataKey.SystemUser) as List<MDLSM_User>;
            mcbUserName.DisplayMember = SystemTableColumnEnums.SM_User.Code.User_Name;
            mcbUserName.ValueMember = SystemTableColumnEnums.SM_User.Code.User_ID;
            mcbUserName.DataSource = _userList;

            #endregion

            //初始化用户菜单明细List
            _userMenuAuthoritiyList = new SkyCarBindingList<UserMenuAuthorityUIModel, MDLSM_UserMenuAuthority>();
            //开始监控用户菜单明细List变化
            _userMenuAuthoritiyList.StartMonitChanges();

            //初始化用户菜单明细动作List
            _userActionAuthoritiyList = new SkyCarBindingList<UserActionAuthorityUIModel, MDLSM_UserActionAuthority>();
            //开始监控用户菜单明细动作List变化
            _userActionAuthoritiyList.StartMonitChanges();

            //初始化用户作业授权List
            _userJobAuthoritiyList = new SkyCarBindingList<UserJobAuthorityUIModel, MDLSM_UserJobAuthority>();
            //开始监控用户作业授权List变化
            _userJobAuthoritiyList.StartMonitChanges();

            //初始化树形控件
            InitializeMenuActionTreeView();
            InitializeUserJobTreeView();
        }

        /// <summary>
        /// 前端检查
        /// </summary>
        /// <returns></returns>
        private bool ClientCheck()
        {
            //验证姓名
            if (string.IsNullOrEmpty(mcbUserName.SelectedText)
                || string.IsNullOrEmpty(mcbUserName.SelectedValue))
            {
                MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.SM_User.Name.User_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 初始化菜单动作树形控件
        /// </summary>
        private void InitializeMenuActionTreeView()
        {
            #region 加载用户菜单动作TreeView数据

            //初始化时，只显示当前用户拥有权限的菜单和动作（SuperAdmin拥有所有权限的菜单和动作）

            //当前登录用户拥有权限的菜单List
            List<MDLSM_Menu> resultLoginUserMenuList = new List<MDLSM_Menu>();
            //当前登录用户的菜单授权List
            List<UserMenuAuthorityUIModel> resultLoginUserMenuAuthoritiyList = new List<UserMenuAuthorityUIModel>();
            //当前登录用户的菜单动作授权List
            List<UserActionAuthorityUIModel> resultLoginUserActionAuthoritiyList = new List<UserActionAuthorityUIModel>();

            //查询当前登录用户在当前组织下所有授权的菜单
            _bll.QueryForList(SQLID.SM_UserMenuManager_SQL04, new MDLSM_UserMenuAuthority()
            {
                WHERE_UMA_User_ID = LoginInfoDAX.UserID,
                WHERE_UMA_Org_ID = LoginInfoDAX.OrgID
            }, resultLoginUserMenuList);

            //查询当前登录用户在当前组织下所有授权的菜单明细
            _bll.QueryForList(SQLID.SM_UserMenuManager_SQL01, new MDLSM_UserMenuAuthority()
            {
                WHERE_UMA_User_ID = LoginInfoDAX.UserID,
                WHERE_UMA_Org_ID = LoginInfoDAX.OrgID
            }, resultLoginUserMenuAuthoritiyList);

            //查询当前登录用户在当前组织下所有授权的菜单动作
            _bll.QueryForList(SQLID.SM_UserMenuManager_SQL03, new MDLSM_UserActionAuthority()
            {
                WHERE_UAA_User_ID = LoginInfoDAX.UserID,
                WHERE_UAA_Org_ID = LoginInfoDAX.OrgID
            }, resultLoginUserActionAuthoritiyList);

            //清空节点
            tvUserMenuAction.Nodes.Clear();
            if (resultLoginUserMenuList.Count > 0)
            {
                foreach (var loopMenu in resultLoginUserMenuList)
                {
                    //菜单节点
                    TreeNode menuNode = new TreeNode
                    {
                        Text = loopMenu.Menu_Name,
                        Tag = loopMenu.Menu_ID
                    };
                    tvUserMenuAction.Nodes.Add(menuNode);

                    //获取该菜单下所有菜单明细
                    List<UserMenuAuthorityUIModel> tempMenuDetailList = resultLoginUserMenuAuthoritiyList.Where(p => p.Menu_ID == loopMenu.Menu_ID).ToList();
                    if (tempMenuDetailList.Count > 0)
                    {
                        foreach (var loopMenuDetail in tempMenuDetailList)
                        {
                            //菜单明细节点
                            TreeNode menuDetailNode = new TreeNode
                            {
                                Text = loopMenuDetail.MenuD_Name,
                                Tag = loopMenuDetail.UMA_MenuD_ID
                            };
                            menuNode.Nodes.Add(menuDetailNode);

                            //获取该菜单明细下所有菜单明细动作
                            List<UserActionAuthorityUIModel> tempMenuDetailActionList = resultLoginUserActionAuthoritiyList.Where(p => p.UAA_MenuD_ID == loopMenuDetail.UMA_MenuD_ID).ToList();
                            if (tempMenuDetailActionList.Count > 0)
                            {
                                foreach (var loopMenuDetailAction in tempMenuDetailActionList)
                                {
                                    //菜单明细动作节点
                                    TreeNode menuDetailActionNode = new TreeNode
                                    {
                                        Text = loopMenuDetailAction.Act_Name,
                                        Tag = loopMenuDetailAction.UAA_Action_ID
                                    };
                                    menuDetailNode.Nodes.Add(menuDetailActionNode);
                                }
                            }
                        }
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// 初始化用户作业授权树形控件
        /// </summary>
        private void InitializeUserJobTreeView()
        {
            #region 加载用户作业TreeView数据

            //初始化时，只显示当前用户拥有权限的作业（SuperAdmin拥有所有权限的作业）

            //查询当前登录用户所有授权的作业
            List<UserJobAuthorityUIModel> resultLoginUserJobList = new List<UserJobAuthorityUIModel>();

            _bll.QueryForList(SQLID.SM_UserMenuManager_SQL05, new MDLSM_UserJobAuthority()
            {
                WHERE_UJA_User_ID = LoginInfoDAX.UserID,
            }, resultLoginUserJobList);

            //清空节点
            tvUserJob.Nodes.Clear();
            if (resultLoginUserJobList.Count > 0)
            {
                foreach (var loopJob in resultLoginUserJobList)
                {
                    //菜单节点
                    TreeNode jobNode = new TreeNode
                    {
                        Text = loopJob.BJ_Name,
                        Tag = loopJob.BJ_ID
                    };
                    tvUserJob.Nodes.Add(jobNode);
                }
            }

            #endregion
        }

        /// <summary>
        /// 获取最新的用户菜单明细授权和用户菜单明细动作授权进行绑定
        /// </summary>
        private void SetUserMenuAndActionInfo()
        {
            #region 获取最新的用户菜单明细授权和用户菜单明细动作授权进行绑定

            if (string.IsNullOrEmpty(mcbUserName.SelectedValue))
            {
                return;
            }
            _isCanAfterCheckOfMenu = false;

            //用户ID
            string userId = mcbUserName.SelectedValue;
            //用户拥有权限的菜单List
            List<MDLSM_Menu> resultUserMenuList = new List<MDLSM_Menu>();
            //用户的菜单授权List
            List<UserMenuAuthorityUIModel> resultUserMenuAuthoritiyList = new List<UserMenuAuthorityUIModel>();
            //用户的菜单动作授权List
            List<UserActionAuthorityUIModel> resultUserActionAuthoritiyList = new List<UserActionAuthorityUIModel>();

            //查询用户在当前组织下所有授权的菜单
            _bll.QueryForList(SQLID.SM_UserMenuManager_SQL04, new MDLSM_UserMenuAuthority()
            {
                WHERE_UMA_User_ID = userId,
                WHERE_UMA_Org_ID = LoginInfoDAX.OrgID
            }, resultUserMenuList);

            //查询用户在当前组织下所有授权的菜单明细
            _bll.QueryForList(SQLID.SM_UserMenuManager_SQL01, new MDLSM_UserMenuAuthority()
            {
                WHERE_UMA_User_ID = userId,
                WHERE_UMA_Org_ID = LoginInfoDAX.OrgID
            }, resultUserMenuAuthoritiyList);

            //查询用户在当前组织下所有授权的菜单动作
            _bll.QueryForList(SQLID.SM_UserMenuManager_SQL03, new MDLSM_UserActionAuthority()
            {
                WHERE_UAA_User_ID = userId,
                WHERE_UAA_Org_ID = LoginInfoDAX.OrgID
            }, resultUserActionAuthoritiyList);

            foreach (TreeNode loopMenuNode in tvUserMenuAction.Nodes)
            {
                if (loopMenuNode == null)
                {
                    continue;
                }

                //勾选的菜单明细节点的数量
                int menuDetailNodeCount = 0;
                foreach (TreeNode loopMenuDetailNode in loopMenuNode.Nodes)
                {
                    if (loopMenuDetailNode == null || loopMenuDetailNode.Tag == null)
                    {
                        continue;
                    }

                    if (resultUserMenuAuthoritiyList.Any(p => p.UMA_MenuD_ID == loopMenuDetailNode.Tag.ToString()))
                    {
                        loopMenuDetailNode.Checked = true;
                        menuDetailNodeCount++;

                        //勾选的菜单动作的节点数
                        int menuDetailActionCount = 0;
                        foreach (TreeNode loopActionNode in loopMenuDetailNode.Nodes)
                        {
                            if (loopActionNode == null || loopActionNode.Tag == null)
                            {
                                continue;
                            }

                            if (resultUserActionAuthoritiyList.Any(p => p.UAA_MenuD_ID == loopMenuDetailNode.Tag.ToString() && p.UAA_Action_ID == loopActionNode.Tag.ToString()))
                            {
                                loopActionNode.Checked = true;
                                menuDetailActionCount++;
                            }
                            else
                            {
                                loopActionNode.Checked = false;
                            }
                        }
                        //如果菜单明细下的菜单动作节点都勾选，则该菜单明细勾选
                        if (menuDetailActionCount == loopMenuDetailNode.Nodes.Count)
                        {
                            loopMenuDetailNode.Checked = true;
                        }
                        else
                        {
                            loopMenuDetailNode.Checked = false;
                        }
                    }
                    else
                    {
                        loopMenuDetailNode.Checked = false;
                        foreach (TreeNode loopActionNode in loopMenuDetailNode.Nodes)
                        {
                            if (resultUserActionAuthoritiyList.Any(p => p.UAA_MenuD_ID == loopMenuDetailNode.Tag.ToString() && p.UAA_Action_ID == loopActionNode.Tag.ToString()))
                            {
                                loopActionNode.Checked = true;
                            }
                            else
                            {
                                loopActionNode.Checked = false;
                            }
                        }
                    }
                }
                //如果菜单下的菜单明细节点都勾选，则该菜单勾选
                if (menuDetailNodeCount == loopMenuNode.Nodes.Count)
                {
                    loopMenuNode.Checked = true;
                }
                else
                {
                    loopMenuNode.Checked = false;
                }
            }

            _isCanAfterCheckOfMenu = true;

            //用户菜单明细授权数据源
            _userMenuAuthoritiyList = new SkyCarBindingList<UserMenuAuthorityUIModel, MDLSM_UserMenuAuthority>();
            _bll.CopyModelList(resultUserMenuAuthoritiyList, _userMenuAuthoritiyList);
            //开始监控用户菜单明细授权List变化
            _userMenuAuthoritiyList.StartMonitChanges();

            //用户菜单明细动作授权数据源
            _userActionAuthoritiyList = new SkyCarBindingList<UserActionAuthorityUIModel, MDLSM_UserActionAuthority>();
            _bll.CopyModelList(resultUserActionAuthoritiyList, _userActionAuthoritiyList);
            //开始监控用户菜单明细动作List变化
            _userActionAuthoritiyList.StartMonitChanges();

            #endregion
        }

        /// <summary>
        /// 获取最新的用户作业授权进行绑定
        /// </summary>
        private void SetUserJobInfo()
        {
            #region 获取最新的用户作业授权进行绑定

            if (string.IsNullOrEmpty(mcbUserName.SelectedValue))
            {
                return;
            }
            _isCanAfterCheckOfJob = false;

            //用户ID
            string userId = mcbUserName.SelectedValue;
            //用户拥有权限的作业List
            List<UserJobAuthorityUIModel> resultUserJobList = new List<UserJobAuthorityUIModel>();

            _bll.QueryForList(SQLID.SM_UserMenuManager_SQL05, new MDLSM_UserJobAuthority()
            {
                WHERE_UJA_User_ID = userId,
            }, resultUserJobList);
            foreach (TreeNode loopJobNode in tvUserJob.Nodes)
            {
                if (loopJobNode == null)
                {
                    continue;
                }

                if (resultUserJobList.Any(p => p.BJ_ID == loopJobNode.Tag.ToString()))
                {
                    loopJobNode.Checked = true;
                }
                else
                {
                    loopJobNode.Checked = false;
                }
            }

            _isCanAfterCheckOfJob = true;

            //用户作业授权数据源
            _userJobAuthoritiyList = new SkyCarBindingList<UserJobAuthorityUIModel, MDLSM_UserJobAuthority>();
            _bll.CopyModelList(resultUserJobList, _userJobAuthoritiyList);
            //开始监控用户作业授权List变化
            _userJobAuthoritiyList.StartMonitChanges();

            #endregion
        }

        #endregion

    }
}
