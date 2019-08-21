using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.SM;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.SM;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.SM.UIModel;
using SkyCar.Common.Utility;

namespace SkyCar.Coeus.UI.SM
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public partial class FrmUserManager : BaseFormCardList<UserManagerUIModel, UserManagerQCModel, MDLSM_User>
    {
        #region 局部变量

        /// <summary>
        /// 用户管理BLL
        /// </summary>
        private UserManagerBLL _bll = new UserManagerBLL();
        /// <summary>
        /// 用户组织关系数据源
        /// </summary>
        private SkyCarBindingList<UserOrgUIModel, MDLSM_UserOrg> _userOrgList = new SkyCarBindingList<UserOrgUIModel, MDLSM_UserOrg>();
        /// <summary>
        /// 是否触发选中TreeView后的AfterCheck事件
        /// </summary>
        private bool _isCanAfterCheck = true;
        /// <summary>
        /// 旧密码
        /// </summary>
        private string _oldPassword = null;
        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmUserManager构造方法
        /// </summary>
        public FrmUserManager()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmUserManager_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（动作）
            base.ToolBarActionAndNavigate = toolBarAction;
            //基类.工具栏（翻页）
            base.ToolBarPaging = toolBarPaging;
            //查询委托（基类控制翻页用）
            base.ExecuteQuery = QueryAction;
            //工具栏（动作）单击事件
            this.toolBarAction.ToolClick += new ToolClickEventHandler(base.toolBarActionAndNavigate_ToolClick);
            //工具栏（翻页）单击事件
            this.toolBarPaging.ToolClick += new ToolClickEventHandler(base.ToolBarPaging_ToolClick);
            //工具栏（翻页）[当前页]值改变事件
            this.toolBarPaging.ToolValueChanged += new ToolEventHandler(base.ToolBarPaging_ToolValueChanged);
            #region 设置页面大小文本框
            TextBoxTool pageSizeOfList = null;
            foreach (var loopToolControl in this.toolBarPaging.Tools)
            {
                if (loopToolControl.Key == SysConst.EN_PAGESIZE)
                {
                    pageSizeOfList = (TextBoxTool)loopToolControl;
                }
            }
            if (pageSizeOfList != null)
            {
                pageSizeOfList.Text = PageSize.ToString();
                pageSizeOfList.AfterToolExitEditMode += PageSizeTextBoxTool_AfterToolExitEditMode;
            }
            #endregion

            //初始化【详情】Tab内控件
            InitializeDetailTabControls();
            //初始化【列表】Tab内控件
            InitializeListTabControls();
            //设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
            //根据选中的Tab，设置动作按钮[是否可用]（在系统权限的基础上进行控制）
            base.SetActionEnableBySelectedTab(SysConst.EN_LIST);
            #endregion

            //[删除]不可用
            SetActionEnable(SystemActionEnum.Code.DELETE, false);

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            AcceptUIModelChanges();
        }

        /// <summary>
        /// 页面大小失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageSizeTextBoxTool_AfterToolExitEditMode(object sender, AfterToolExitEditModeEventArgs e)
        {
            if (!SysConst.EN_PAGESIZE.Equals(e.Tool.Key))
            {
                return;
            }
            string strValue = ((TextBoxTool)(e.Tool)).Text.Trim();
            int tmpPageSize = 0;
            if (!int.TryParse(strValue, out tmpPageSize) || tmpPageSize == 0)
            {
                ((TextBoxTool)(e.Tool)).Text = SysConst.DEFAULT_PAGESIZE.ToString();
                return;
            }
            if (tmpPageSize > SysConst.MAX_PAGESIZE)
            {
                ((TextBoxTool)(e.Tool)).Text = SysConst.MAX_PAGESIZE.ToString();
                return;
            }

            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                PageSize = tmpPageSize;
            }
            else
            {

            }

            ExecuteQuery?.Invoke();
        }

        #region 【列表】Grid相关事件

        /// <summary>
        /// 【列表】Grid的Cell的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            //将Grid数据赋值给【详情】Tab内的对应控件
            SetGridDataToCardCtrls();
        }

        /// <summary>
        /// 【列表】Grid的CellChange事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_CellChange(object sender, CellEventArgs e)
        {
            gdGrid.UpdateData();
        }

        #endregion

        #region Tab改变事件

        /// <summary>
        /// 选中的Tab改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlFull_SelectedTabChanging(object sender, SelectedTabChangingEventArgs e)
        {
            base.SetActionEnableBySelectedTab(e.Tab.Key);
        }

        /// <summary>
        /// 选中的Tab改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlFull_SelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            //[列表]页不允许删除
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                //选中【列表】Tab的场合
                //[删除]不可用
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
            }
            else
            {
                //选中【详情】Tab的场合
                //[删除]可用
                SetActionEnable(SystemActionEnum.Code.DELETE, !string.IsNullOrEmpty(txtUser_ID.Text));
            }
        }
        #endregion

        /// <summary>
        /// 选中组织节点【只有一级】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvOrg_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (!_isCanAfterCheck)
            {
                return;
            }
            TreeNode checkNode = e.Node;
            if (checkNode == null)
            {
                return;
            }
            if (tvOrg.Nodes.Contains(checkNode))
            {
                if (checkNode.Checked)
                {
                    //过滤重复的[用户组织]
                    if (!_userOrgList.Any(x => x.UO_Org_ID == checkNode.Tag?.ToString() && x.UO_User_ID == txtUser_ID.Text.Trim()))
                    {
                        //添加
                        _userOrgList.Add(new UserOrgUIModel()
                        {
                            UO_Org_ID = checkNode.Tag?.ToString(),
                            UO_User_ID = txtUser_ID.Text.Trim(),
                            UO_IsValid = true,
                            UO_CreatedBy = LoginInfoDAX.UserName,
                            UO_CreatedTime = BLLCom.GetCurStdDatetime(),
                            UO_UpdatedBy = LoginInfoDAX.UserName,
                            UO_UpdatedTime = BLLCom.GetCurStdDatetime(),
                        });
                    }
                }
                else
                {
                    //删除
                    List<UserOrgUIModel> deleteUserOrgList =
                        _userOrgList.Where(p => p.UO_Org_ID == checkNode.Tag?.ToString()
                                                && p.UO_User_ID == txtUser_ID.Text.Trim())
                            .ToList();
                    if (deleteUserOrgList.Count > 0)
                    {
                        _userOrgList.Remove(deleteUserOrgList[0]);
                    }
                }
            }
        }

        #endregion

        #region 重写基类方法

        /// <summary>
        /// 新增
        /// </summary>
        public override void NewAction()
        {
            #region 检查详情是否已保存

            SetCardCtrlsToDetailDS();
            base.NewUIModel = DetailDS;
            if (ViewHasChanged()
                || _userOrgList.InsertList.Count > 0
                || _userOrgList.UpdateList.Count > 0
                || _userOrgList.DeleteList.Count > 0)
            {
                //信息尚未保存，确定进行当前操作？
                DialogResult dialogResult = MessageBoxs.Show(Trans.SM, ToString(), MsgHelp.GetMsg(MsgCode.W_0001), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
            }
            #endregion

            //执行基类方法
            base.NewAction();
            //初始化【详情】Tab内控件
            InitializeDetailTabControls();
            //设置【详情】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;
            //设置详情是否可编辑
            SetDetailControl();

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();
        }

        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
            //设置查询条件（翻页相关属性不用设置）
            base.ConditionDS = new UserManagerQCModel()
            {
                //SqlId
                SqlId = SQLID.SM_UserManager_SQL04,
                //姓名
                WHERE_User_Name = txtWhere_User_Name.Text.Trim(),
                //工号
                WHERE_User_EMPNO = txtWhere_User_EMPNO.Text.Trim(),
                //有效
                WHERE_User_IsValid = ckWhere_User_IsValid.Checked,
            };
            if (ckWhere_User_IsValid.Checked)
            {
                //查询有效的用户时，只能查询当前组织下的用户
                //组织ID
                ConditionDS.WHERE_UO_Org_ID = LoginInfoDAX.OrgID;
            }
            //执行基类方法
            base.QueryAction();
            //3.Grid绑定数据源
            gdGrid.DataSource = base.GridDS;
            gdGrid.DataBind();
            //4.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
            //5.设置Grid自适应列宽（根据单元格内容）
            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdGrid.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 清空
        /// </summary>
        public override void ClearAction()
        {
            //初始化【列表】Tab内控件
            InitializeListTabControls();
        }

        /// <summary>
        /// 关闭画面时检查画面值是否发生变化
        /// </summary>
        /// <returns></returns>
        public override bool ViewHasChangedWhenClose()
        {
            SetCardCtrlsToDetailDS();
            base.NewUIModel = DetailDS;
            if (ViewHasChanged()
                || _userOrgList.InsertList.Count > 0
                || _userOrgList.UpdateList.Count > 0
                || _userOrgList.DeleteList.Count > 0)
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
            //1.前端检查-保存
            if (!ClientCheckForSave())
            {
                return;
            }
            //2.将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();

            #region 保存数据
            //保存数据
            bool saveUserInfoResult = _bll.SaveDetailDS(DetailDS, _userOrgList);
            if (!saveUserInfoResult)
            {
                //保存失败
                MessageBoxs.Show(Trans.SM, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.SM, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            #endregion

            //刷新列表
            RefreshList();

            //将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置用户组织信息
            SetUserOrgInfo();
            //设置详情是否可编辑
            SetDetailControl();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public override void DeleteAction()
        {
            bool isDeleteUserSuccess = false;

            //待删除的[用户]列表
            List<MDLSM_User> deleteUserList = new List<MDLSM_User>();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 详情删除
                //用户信息为空，不能删除
                if (string.IsNullOrEmpty(txtUser_ID.Text.Trim()))
                {
                    MessageBoxs.Show(Trans.SM, ToString(), MsgHelp.GetMsg(MsgCode.W_0016, new object[] { SystemTableEnums.Name.SM_User, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //被引用过的用户不能删除
                List<MDLSM_User> usedUserList = new List<MDLSM_User>();
                _bll.QueryForList<MDLSM_User>(SQLID.SM_UserManager_SQL05, new MDLSM_User()
                {
                    WHERE_User_ID = txtUser_ID.Text.Trim() + SysConst.Semicolon_DBC,
                }, usedUserList);
                if (usedUserList.Count > 0)
                {
                    //用户已被使用，不可删除！
                    MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0007, new object[] { SystemTableEnums.Name.SM_User, MsgParam.APPLY, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //确认删除
                var isDeleteUser = MessageBoxs.Show(Trans.SM, ToString(), MsgHelp.GetMsg(MsgCode.W_0012, new object[] { }),
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (isDeleteUser != DialogResult.OK)
                {
                    return;
                }
                //待删除的用户
                MDLSM_User deleteUser = new MDLSM_User
                {
                    User_ID = txtUser_ID.Text.Trim(),
                    WHERE_User_ID = txtUser_ID.Text.Trim(),
                };
                deleteUserList.Add(deleteUser);
                #endregion
            }

            #region 删除数据

            isDeleteUserSuccess = _bll.DeleteUserInfo(deleteUserList);
            if (!isDeleteUserSuccess)
            {
                //删除失败
                MessageBoxs.Show(Trans.SM, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion

            //删除成功
            MessageBoxs.Show(Trans.SM, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //3.清空【详情】画面数据
            InitializeDetailTabControls();
            //刷新列表
            RefreshList(true);

            //将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 工具生成
            //姓名
            txtUser_Name.Clear();
            //工号
            txtUser_EMPNO.Clear();
            //身份证号码
            txtUser_IDNo.Clear();
            //性别
            cbUser_Sex.Items.Clear();
            //地址
            txtUser_Address.Clear();
            //电话号码
            txtUser_PhoneNo.Clear();
            //是否允许微信认证
            ckUser_IsAllowWechatCertificate.Checked = true;
            ckUser_IsAllowWechatCertificate.CheckState = CheckState.Checked;
            //是否已微信认证
            ckUser_IsWechatCertified.Checked = false;
            ckUser_IsWechatCertified.CheckState = CheckState.Unchecked;
            //打印标题前缀
            txtUser_PrintTitlePrefix.Clear();
            //有效
            ckUser_IsValid.Checked = true;
            ckUser_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtUser_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtUser_CreatedTime.Value = BLLCom.GetCurStdDatetime();
            //修改人
            txtUser_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtUser_UpdatedTime.Value = BLLCom.GetCurStdDatetime();
            //用户ID
            txtUser_ID.Clear();
            //密码
            txtUser_Password.Clear();
            //版本号
            txtUser_VersionNo.Clear();
            //给 姓名 设置焦点
            lblUser_Name.Focus();
            #endregion

            #region 性别下拉框数据填充

            List<ComComboBoxDataSource> resultSexList = EnumDAX.GetEnumForComboBoxWithValueText(EnumKey.Sex);
            if (resultSexList != null)
            {
                cbUser_Sex.DisplayMember = SysConst.EN_TEXT;
                cbUser_Sex.ValueMember = SysConst.Value;
                cbUser_Sex.DataSource = resultSexList;
                cbUser_Sex.DataBind();
            }

            #endregion

            _userOrgList = new SkyCarBindingList<UserOrgUIModel, MDLSM_UserOrg>();
            _userOrgList.StartMonitChanges();

            //初始化树形控件
            InitializeTreeView();
        }

        /// <summary>
        /// 初始化树形控件
        /// </summary>
        private void InitializeTreeView()
        {
            List<MDLSM_Organization> resultOrganizationList = new List<MDLSM_Organization>();
            if (LoginInfoDAX.UserID == SysConst.SUPER_ADMIN)
            {
                #region SuperAdmin，组织列表为所有组织列表
                _bll.QueryForList(new MDLSM_Organization()
                {
                    WHERE_Org_IsValid = true
                }, resultOrganizationList);
                #endregion
            }
            else
            {
                #region 非SuperAdmin用户，组织列表为当前用户有权限的组织列表

                resultOrganizationList = LoginInfoDAX.OrgList;
                #endregion
            }

            //清空节点
            tvOrg.Nodes.Clear();
            if (resultOrganizationList.Count > 0)
            {
                foreach (var loopOrganization in resultOrganizationList)
                {
                    TreeNode orgNode = new TreeNode
                    {
                        Text = loopOrganization.Org_ShortName,
                        Tag = loopOrganization.Org_ID
                    };
                    tvOrg.Nodes.Add(orgNode);
                }
            }
        }

        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //姓名
            txtWhere_User_Name.Clear();
            //工号
            txtWhere_User_EMPNO.Clear();
            //有效
            ckWhere_User_IsValid.Checked = true;
            ckWhere_User_IsValid.CheckState = CheckState.Checked;
            //给 姓名 设置焦点
            lblWhere_User_Name.Focus();
            #endregion

            #region Grid初始化

            //清空Grid
            GridDS = new BindingList<UserManagerUIModel>();
            gdGrid.DataSource = base.GridDS;
            gdGrid.DataBind();

            #endregion
            #endregion
        }

        /// <summary>
        /// 将Grid数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetGridDataToCardCtrls()
        {
            //判断是否允许将【列表】Grid数据设置到【详情】Tab内的对应控件
            if (!IsAllowSetGridDataToCard())
            {
                return;
            }

            SetCardCtrlsToDetailDS();
            base.NewUIModel = DetailDS;

            var activeRowIndex = gdGrid.ActiveRow.Index;
            //判断Grid内[唯一标识]是否为空
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SM_User.Code.User_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SM_User.Code.User_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            DetailDS = base.GridDS.FirstOrDefault(x => x.User_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SM_User.Code.User_ID].Value);
            if (DetailDS == null || string.IsNullOrEmpty(DetailDS.User_ID))
            {
                return;
            }

            if (txtUser_ID.Text != DetailDS.User_ID
                || (txtUser_ID.Text == DetailDS.User_ID && txtUser_VersionNo.Text != DetailDS.User_VersionNo?.ToString()))
            {
                if (txtUser_ID.Text == DetailDS.User_ID && txtUser_VersionNo.Text != DetailDS.User_VersionNo?.ToString())
                {
                    //数据版本已过期，将加载最新详情。
                    MessageBoxs.Show(Trans.SM, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.DataHasOverdue }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else if (ViewHasChanged()
                || _userOrgList.InsertList.Count > 0
                || _userOrgList.UpdateList.Count > 0
                || _userOrgList.DeleteList.Count > 0)
                {
                    //将放弃之前的修改，是否继续？
                    DialogResult dialogResult = MessageBoxs.Show(Trans.SM, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.ConfirmGiveUpEdit }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dialogResult != DialogResult.OK)
                    {
                        //选中【详情】Tab
                        tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;
                        return;
                    }
                }
                //将DetailDS数据赋值给【详情】Tab内的对应控件
                SetDetailDSToCardCtrls();
            }

            //选中【详情】Tab
            tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;

            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置用户组织信息
            SetUserOrgInfo();
            //设置详情是否可编辑
            SetDetailControl();
        }

        /// <summary>
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //姓名
            txtUser_Name.Text = DetailDS.User_Name;
            //工号
            txtUser_EMPNO.Text = DetailDS.User_EMPNO;
            //身份证号码
            txtUser_IDNo.Text = DetailDS.User_IDNo;
            //性别
            cbUser_Sex.Text = DetailDS.User_Sex;
            //地址
            txtUser_Address.Text = DetailDS.User_Address;
            //电话号码
            txtUser_PhoneNo.Text = DetailDS.User_PhoneNo;
            //是否允许微信认证
            if (DetailDS.User_IsAllowWechatCertificate != null)
            {
                ckUser_IsAllowWechatCertificate.Checked = DetailDS.User_IsAllowWechatCertificate.Value;
            }
            //是否已微信认证
            if (DetailDS.User_IsWechatCertified != null)
            {
                ckUser_IsWechatCertified.Checked = DetailDS.User_IsWechatCertified.Value;
            }
            txtUser_PrintTitlePrefix.Text = DetailDS.User_PrintTitlePrefix;
            //有效
            if (DetailDS.User_IsValid != null)
            {
                ckUser_IsValid.Checked = DetailDS.User_IsValid.Value;
            }
            //创建人
            txtUser_CreatedBy.Text = DetailDS.User_CreatedBy;
            //创建时间
            dtUser_CreatedTime.Value = DetailDS.User_CreatedTime;
            //修改人
            txtUser_UpdatedBy.Text = DetailDS.User_UpdatedBy;
            //修改时间
            dtUser_UpdatedTime.Value = DetailDS.User_UpdatedTime;
            //用户ID
            txtUser_ID.Text = DetailDS.User_ID;
            //密码
            txtUser_Password.Text = DetailDS.User_Password;
            _oldPassword = DetailDS.User_Password;
            //版本号
            txtUser_VersionNo.Value = DetailDS.User_VersionNo?.ToString() ?? "";
        }

        /// <summary>
        /// 是否允许将【列表】Grid数据设置到【详情】Tab内的对应控件
        /// </summary>
        /// <returns>true:允许；false：不允许</returns>
        private bool IsAllowSetGridDataToCard()
        {
            if (gdGrid.ActiveRow == null || gdGrid.ActiveRow.Index < 0)
            {
                InitializeDetailTabControls();
                return false;
            }
            if (gdGrid.DisplayLayout.Bands[0].SortedColumns.Count > 0)
            {
                foreach (UltraGridColumn loopUltraGridColumn in gdGrid.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (loopUltraGridColumn.IsGroupByColumn)
                    {
                        InitializeDetailTabControls();
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 前端检查-保存
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForSave()
        {
            #region 验证
            //验证姓名
            if (string.IsNullOrEmpty(txtUser_Name.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SM, ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.SM_User.Name.User_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUser_Name.Focus();
                return false;
            }
            //验证工号
            if (string.IsNullOrEmpty(txtUser_EMPNO.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SM, ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.SM_User.Name.User_EMPNO }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUser_EMPNO.Focus();
                return false;
            }
            //工号不能为SuperAdmin
            if (txtUser_EMPNO.Text.Trim().Equals(SysConst.SUPER_ADMIN))
            {
                MessageBoxs.Show(Trans.SM, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableColumnEnums.SM_User.Name.User_EMPNO, MsgParam.BE + SysConst.SUPER_ADMIN }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUser_EMPNO.SelectAll();
                txtUser_EMPNO.Focus();
                return false;
            }
            //判断工号是否已存在
            if (_bll.JudgeEmpNoIsExist(txtUser_ID.Text, txtUser_EMPNO.Text))
            {
                MessageBoxs.Show(Trans.SM, ToString(), MsgHelp.GetMsg(MsgCode.E_0006, new object[] { SystemTableColumnEnums.SM_User.Name.User_EMPNO }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUser_EMPNO.SelectAll();
                txtUser_EMPNO.Focus();
                return false;
            }
            //密码必须大于6位
            if (txtUser_Password.Text.Trim().Length > 0 && txtUser_Password.Text.Trim().Length < 6)
            {
                MessageBoxs.Show(Trans.SM, ToString(), MsgHelp.GetMsg(MsgCode.E_0004, new object[] { SystemTableColumnEnums.SM_User.Name.User_Password, MsgParam.SIX_DIGIT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUser_Password.Focus();
                return false;
            }

            if (ckUser_IsValid.Checked == false)
            {
                //确定要恢复无效用户吗？
                var confirmRecoverUser = MessageBoxs.Show(Trans.SM, ToString(), MsgHelp.GetMsg(MsgCode.I_0003, new object[] { MsgParam.RESTORE_INVALIDUSER }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (confirmRecoverUser != DialogResult.OK)
                {
                    return false;
                }
            }

            if (_userOrgList.Count == 0)
            {
                MessageBoxs.Show(Trans.SM, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ORGNIZATION }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            #endregion

            return true;
        }

        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            DetailDS = new UserManagerUIModel
            {
                //用户ID
                User_ID = txtUser_ID.Text.Trim(),
                //姓名
                User_Name = txtUser_Name.Text.Trim(),
                //工号
                User_EMPNO = txtUser_EMPNO.Text.Trim(),
                //身份证号
                User_IDNo = txtUser_IDNo.Text.Trim(),
                //性别
                User_Sex = cbUser_Sex.Text.Trim(),
                //地址
                User_Address = txtUser_Address.Text.Trim(),
                //电话号码
                User_PhoneNo = txtUser_PhoneNo.Text.Trim(),
                //是否允许微信认证
                User_IsAllowWechatCertificate = ckUser_IsAllowWechatCertificate.Checked,
                //是否已微信认证
                User_IsWechatCertified = ckUser_IsWechatCertified.Checked,
                //打印标题前缀
                User_PrintTitlePrefix = txtUser_PrintTitlePrefix.Text.Trim(),
                //有效
                User_IsValid = ckUser_IsValid.Checked,
                //创建人
                User_CreatedBy = txtUser_CreatedBy.Text,
                //创建时间
                User_CreatedTime = (DateTime?)dtUser_CreatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //修改人
                User_UpdatedBy = txtUser_UpdatedBy.Text,
                //修改时间
                User_UpdatedTime = (DateTime?)dtUser_UpdatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //版本号
                User_VersionNo = Convert.ToInt64(txtUser_VersionNo.Text.Trim() == "" ? "1" : txtUser_VersionNo.Text.Trim()),
            };

            //密码
            string encodePassword = null;
            if (!string.IsNullOrEmpty(txtUser_Password.Text.Trim()))
            {
                if (txtUser_Password.Text.Trim() != _oldPassword)
                {
                    encodePassword = CryptoHelp.EncodeToMD5(txtUser_Password.Text.Trim());
                }
                else
                {
                    encodePassword = txtUser_Password.Text.Trim();
                }
            }
            else
            {
                encodePassword = CryptoHelp.EncodeToMD5(SysConst.USER_INITIAL_PASSWORD);
            }
            DetailDS.User_Password = encodePassword;
        }

        /// <summary>
        /// 获取最新的用户组织进行绑定
        /// </summary>
        private void SetUserOrgInfo()
        {
            #region 获取最新的用户组织进行绑定

            if (string.IsNullOrEmpty(txtUser_ID.Text.Trim()))
            {
                return;
            }
            _isCanAfterCheck = false;

            //用户ID
            string userId = txtUser_ID.Text.Trim();
            //获取用户组织信息
            List<MDLSM_UserOrg> resultUserOrgList = new List<MDLSM_UserOrg>();
            _bll.QueryForList<MDLSM_UserOrg, MDLSM_UserOrg>(new MDLSM_UserOrg()
            {
                WHERE_UO_IsValid = true,
                WHERE_UO_User_ID = userId
            }, resultUserOrgList);

            if (resultUserOrgList.Count > 0)
            {
                foreach (TreeNode loopUserOrgNode in tvOrg.Nodes)
                {
                    if (loopUserOrgNode == null)
                    {
                        continue;
                    }
                    bool isSaveUserOrg = resultUserOrgList.Any(p => p.UO_Org_ID == loopUserOrgNode.Tag?.ToString()
                                                                    && p.UO_User_ID == txtUser_ID.Text.Trim());
                    loopUserOrgNode.Checked = isSaveUserOrg;
                }
            }

            _isCanAfterCheck = true;

            //用户组织数据源
            _userOrgList = new SkyCarBindingList<UserOrgUIModel, MDLSM_UserOrg>();
            _bll.CopyModelList(resultUserOrgList, _userOrgList);
            //开始监控用户组织List变化
            _userOrgList.StartMonitChanges();

            #endregion
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <param name="paramIsDelete">是否是删除操作</param>
        private void RefreshList(bool paramIsDelete = false)
        {
            if (paramIsDelete)
            {
                if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
                {
                    var removeList = GridDS.Where(x => x.IsChecked == true).ToList();
                    foreach (var loopRemove in removeList)
                    {
                        GridDS.Remove(loopRemove);
                    }
                }
                else
                {
                    var curHead = GridDS.FirstOrDefault(x => x.User_ID == DetailDS.User_ID);
                    if (curHead != null)
                    {
                        GridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = GridDS.FirstOrDefault(x => x.User_ID == DetailDS.User_ID);
                if (curHead != null)
                {
                    _bll.CopyModel(DetailDS, curHead);
                }
                else
                {
                    GridDS.Insert(0, DetailDS);
                }
            }

            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }

        /// <summary>
        /// 设置详情页面控件是否可编辑
        /// </summary>
        private void SetDetailControl()
        {
            SetActionEnable(SystemActionEnum.Code.DELETE, !string.IsNullOrEmpty(txtUser_ID.Text));
        }
        #endregion

    }
}
