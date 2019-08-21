using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.PIS;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.PIS;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.Common.UIModel;

namespace SkyCar.Coeus.UI.PIS
{
    /// <summary>
    /// 汽修商管理
    /// </summary>
    public partial class FrmAutoFactoryCustomerManager : BaseFormCardList<AutoFactoryCustomerManagerUIModel, AutoFactoryCustomerManagerQCModel, MDLPIS_AutoFactoryCustomer>
    {
        #region 全局变量

        /// <summary>
        /// 汽修商管理BLL
        /// </summary>
        private AutoFactoryCustomerManagerBLL _bll = new AutoFactoryCustomerManagerBLL();

        #region 下拉框数据源

        /// <summary>
        /// 默认支付类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _paymentTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 默认开票类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _billingTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 默认物流人员类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _deliveryTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 配件价格类别
        /// </summary>
        ObservableCollection<CodeTableValueTextModel> _autoPartsPriceTypeList = new ObservableCollection<CodeTableValueTextModel>();
        #endregion

        /// <summary>
        /// 是否触发选中TreeView后的AfterCheck事件
        /// </summary>
        private bool _isCanAfterCheck = true;

        /// <summary>
        /// 汽配汽修组织授权数据源
        /// </summary>
        private SkyCarBindingList<AROrgSupOrgAuthorityUIModel, MDLSM_AROrgSupOrgAuthority> _arOrgSupOrgAuthorityList = new SkyCarBindingList<AROrgSupOrgAuthorityUIModel, MDLSM_AROrgSupOrgAuthority>();

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmAutoFactoryCustomerManager构造方法
        /// </summary>
        public FrmAutoFactoryCustomerManager()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAutoFactoryCustomerManager_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（动作，导航）
            base.ToolBarActionAndNavigate = toolBarActionAndNavigate;
            //基类.工具栏（翻页）
            base.ToolBarPaging = toolBarPaging;
            //查询委托（基类控制翻页用）
            base.ExecuteQuery = QueryAction;
            //工具栏（动作）单击事件
            this.toolBarActionAndNavigate.ToolClick += new ToolClickEventHandler(base.toolBarActionAndNavigate_ToolClick);
            //工具栏（翻页）单击事件
            this.toolBarPaging.ToolClick += new ToolClickEventHandler(base.ToolBarPaging_ToolClick);
            //工具栏（翻页）[当前页]值改变事件
            this.toolBarPaging.ToolValueChanged += new ToolEventHandler(base.ToolBarPaging_ToolValueChanged);
         
            #region 设置页面大小文本框
            TextBoxTool pageSizeOfListTextBox = null;
            foreach (var loopToolControl in this.toolBarPaging.Tools)
            {
                if (loopToolControl.Key == SysConst.EN_PAGESIZE)
                {
                    pageSizeOfListTextBox = (TextBoxTool)loopToolControl;
                }
            }
            if (pageSizeOfListTextBox != null)
            {
                pageSizeOfListTextBox.Text = PageSize.ToString();
                pageSizeOfListTextBox.AfterToolExitEditMode += PageSizeTextBoxTool_AfterToolExitEditMode;
            }
            #endregion

            //初始化【详情】Tab内控件
            InitializeDetailTabControls();
            //初始化【列表】Tab内控件
            InitializeListTabControls();
            //根据选中的Tab，设置动作按钮[是否可用]（在系统权限的基础上进行控制）
            base.SetActionEnableBySelectedTab(SysConst.EN_LIST);
            #endregion

            //导航按钮[开户]可用
            SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.CREATEACCOUNT, true, true);

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
            //将Grid中选中的数据赋值给【详情】Tab内的对应控件
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
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                #region 选中【列表】Tab的场合

                //[开户]按钮可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.CREATEACCOUNT, true, true);

                #endregion
            }
            else
            {
                #region 选中【详情】Tab的场合

                //平台商户的客户不能删除
                if (ckAFC_IsPlatform.Checked)
                {
                    SetActionEnable(SystemActionEnum.Code.DELETE, false);
                }
                else
                {
                    SetActionEnable(SystemActionEnum.Code.DELETE, true);
                }

                if (string.IsNullOrEmpty(txtAFC_ID.Text)
                    || string.IsNullOrEmpty(txtAFC_Name.Text))
                {
                    //未保存的场合，[开户]按钮不可用
                    SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.CREATEACCOUNT, true, false);
                }
                else
                {
                    //已保存的场合，[开户]按钮可用
                    SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.CREATEACCOUNT, true, true);
                }
                #endregion
            }
        }

        #endregion

        #region 查询条件相关事件

        /// <summary>
        /// 汽修商编码KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_AFC_Code_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 汽修商名称KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_AFC_Name_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 手机号码KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_AFC_PhoneNo_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 是否平台商户CheckedChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckWhere_AFC_IsPlatform_CheckedChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        #endregion

        #region 详情相关事件

        /// <summary>
        /// 【详情】查询物流人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAFC_DeliveryByName_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(cbAFC_DeliveryTypeName.Text))
            {
                //请先选择物流人员类型，再选择物流人员
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0034, new object[] { MsgParam.DELIVERYBY_TYPE, MsgParam.DELIVERYBY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cbAFC_DeliveryTypeName.Text == DeliveryTypeEnum.Name.YG)
            {
                #region 物流人员为{员工}的场合

                //查询当前组织下的用户
                FrmUserQuery userQuery = new FrmUserQuery(paramItemSelectedItemParentValue: LoginInfoDAX.OrgID, paramItemSelectedItemParentText: LoginInfoDAX.OrgShortName)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = userQuery.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                if (userQuery.SelectedGridList != null && userQuery.SelectedGridList.Count == 1)
                {
                    //物流人员
                    txtAFC_DeliveryByName.Text = userQuery.SelectedGridList[0].User_Name;
                    //物流人员ID
                    txtAFC_DeliveryByID.Text = userQuery.SelectedGridList[0].User_ID;
                    //物流人员手机号
                    txtAFC_DeliveryByPhoneNo.Text = userQuery.SelectedGridList[0].User_PhoneNo;
                }

                #endregion
            }
            else
            {
                #region 物流人员为{第三方个人}、{第三方公司}的场合

                //根据物流订单.[物流人员类型]查询相应物流订单中物流人员信息
                List<ComComboBoxDataSourceTC> paramDeliveryTypeList = new List<ComComboBoxDataSourceTC>();
                paramDeliveryTypeList = _deliveryTypeList.Where(x => x.Text == cbAFC_DeliveryTypeName.Text).ToList();

                Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
                {
                    {SDViewParamKey.DeliveryType.ToString(), paramDeliveryTypeList}
                };

                FrmLogisticsBillQuery logisticsBillQuery = new FrmLogisticsBillQuery(paramViewParameters)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = logisticsBillQuery.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                if (logisticsBillQuery.SelectedGridList != null && logisticsBillQuery.SelectedGridList.Count == 1)
                {
                    //物流人员
                    txtAFC_DeliveryByName.Text = logisticsBillQuery.SelectedGridList[0].LB_DeliveryBy;
                    //物流人员手机号
                    txtAFC_DeliveryByPhoneNo.Text = logisticsBillQuery.SelectedGridList[0].LB_PhoneNo;
                }

                #endregion
            }
        }

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
                    if (!_arOrgSupOrgAuthorityList.Any(x => x.ASOAH_SupOrg_ID == checkNode.Tag?.ToString()
                    && x.ASOAH_AFC_ID == txtAFC_ID.Text.Trim()
                    && x.ASOAH_ARMerchant_Code == txtAFC_Code.Text.Trim()
                    && x.ASOAH_AROrg_Code == txtAFC_AROrg_Code.Text.Trim()))
                    {
                        //添加
                        _arOrgSupOrgAuthorityList.Add(new AROrgSupOrgAuthorityUIModel()
                        {
                            ASOAH_SupOrg_ID = checkNode.Tag?.ToString(),
                            ASOAH_AFC_ID = txtAFC_ID.Text.Trim(),
                            ASOAH_ARMerchant_Code = txtAFC_Code.Text.Trim(),
                            ASOAH_ARMerchant_Name = txtAFC_Name.Text.Trim(),
                            ASOAH_AROrg_Code = txtAFC_AROrg_Code.Text.Trim(),
                            ASOAH_AROrg_Name = txtAFC_AROrg_Name.Text.Trim(),
                            ASOAH_IsValid = true,
                            ASOAH_CreatedBy = LoginInfoDAX.UserName,
                            ASOAH_CreatedTime = BLLCom.GetCurStdDatetime(),
                            ASOAH_UpdatedBy = LoginInfoDAX.UserName,
                            ASOAH_UpdatedTime = BLLCom.GetCurStdDatetime(),
                        });
                    }
                }
                else
                {
                    //删除
                    AROrgSupOrgAuthorityUIModel deleteAROrgSupOrgAuthorityList =
                        _arOrgSupOrgAuthorityList.FirstOrDefault(x => x.ASOAH_SupOrg_ID == checkNode.Tag?.ToString()
                                                                      && x.ASOAH_AFC_ID == txtAFC_ID.Text.Trim()
                                                                      && x.ASOAH_ARMerchant_Code == txtAFC_Code.Text.Trim()
                                                                      && x.ASOAH_AROrg_Code == txtAFC_AROrg_Code.Text.Trim());
                    if (deleteAROrgSupOrgAuthorityList != null)
                    {
                        _arOrgSupOrgAuthorityList.Remove(deleteAROrgSupOrgAuthorityList);
                    }
                }
            }
        }

        #endregion

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
            if (ViewHasChanged())
            {
                //信息尚未保存，确定进行当前操作？
                DialogResult dialogResult = MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.W_0001), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
            }
            #endregion

            //1.执行基类方法
            base.NewAction();
            //2.初始化【详情】Tab内控件
            InitializeDetailTabControls();
            //3.设置【详情】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;

            //设置详情页面控件以及导航按钮是否可编辑
            SetDetailControl();

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();
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

            //3.执行保存（含服务端检查）
            bool saveAutoFactoryCustomerResult = _bll.SaveDetailDS(DetailDS, _arOrgSupOrgAuthorityList);
            if (!saveAutoFactoryCustomerResult)
            {
                //保存失败
                MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();
            
            //4.将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置详情页面控件以及导航按钮是否可编辑
            SetDetailControl();
            //设置汽配汽修组织信息
            SetUserOrgInfo();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public override void DeleteAction()
        {
            //2.执行删除
            #region 准备数据

            //待删除的[汽修商]列表
            List<MDLPIS_AutoFactoryCustomer> deleteAutoFactoryCustomerList = new List<MDLPIS_AutoFactoryCustomer>();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 详情删除
                if (string.IsNullOrEmpty(txtAFC_ID.Text))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0016, new object[] { SystemTableEnums.Name.PIS_AutoFactoryCustomer, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //查询汽修商是否被引用过
                var usedCount = _bll.QueryForObject<Int32>(SQLID.PIS_AutoFactoryCustomerManager_SQL02, new MDLPIS_AutoFactoryCustomer()
                {
                    WHERE_AFC_ID = txtAFC_ID.Text.Trim(),
                    WHERE_AFC_Code = txtAFC_Code.Text.Trim()
                });
                if (usedCount > 0)
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0007, new object[] { SystemTableEnums.Name.PIS_AutoFactoryCustomer, MsgParam.APPLY, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //确认删除操作
                DialogResult dialogResult = MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0012), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                //待删除的汽修商
                MDLPIS_AutoFactoryCustomer deleteAutoFactoryCustomer = new MDLPIS_AutoFactoryCustomer
                {
                    WHERE_AFC_ID = txtAFC_ID.Text.Trim()
                };
                deleteAutoFactoryCustomerList.Add(deleteAutoFactoryCustomer);
                #endregion
            }
            else
            {
                #region 列表删除
                gdGrid.UpdateData();
                //勾选的汽修商列表
                List<AutoFactoryCustomerManagerUIModel> checkedAutoFactoryCustomerList = GridDS.Where(x => x.IsChecked == true).ToList();
                if (checkedAutoFactoryCustomerList.Count == 0)
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { SystemTableEnums.Name.PIS_AutoFactoryCustomer, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //勾选的列表中的平台商户
                var platformCustomerList = checkedAutoFactoryCustomerList.Where(x => x.AFC_IsPlatform == true).ToList();
                if (platformCustomerList.Count > 0)
                {
                    //平台内汽修商客户，不能删除
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { MsgParam.PLATFORM + SystemTableEnums.Name.PIS_AutoFactoryCustomer, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    foreach (var loopPlatformCustomer in platformCustomerList)
                    {
                        loopPlatformCustomer.IsChecked = false;
                    }
                    gdGrid.DataSource = GridDS;
                    gdGrid.DataBind();
                    return;
                }

                #region 查询汽修商是否被引用过
                //勾选的[汽配商]列表中已被引用的[汽配商]列表
                List<AutoFactoryCustomerManagerUIModel> checkedUsedList = new List<AutoFactoryCustomerManagerUIModel>();

                foreach (var loopAutoFactoryCustomer in checkedAutoFactoryCustomerList)
                {
                    var resultUsedCount = _bll.QueryForObject<Int32>(SQLID.PIS_AutoFactoryCustomerManager_SQL02, new MDLPIS_AutoFactoryCustomer()
                    {
                        WHERE_AFC_ID = loopAutoFactoryCustomer.AFC_ID,
                        WHERE_AFC_Code = loopAutoFactoryCustomer.AFC_Code
                    });
                    if (resultUsedCount > 0)
                    {
                        loopAutoFactoryCustomer.IsChecked = false;
                        checkedUsedList.Add(loopAutoFactoryCustomer);
                    }
                }

                if (checkedUsedList.Count > 0)
                {
                    //勾选的列表中包含已被引用的[汽配商]，是否忽略
                    DialogResult continueDelete = MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0026, new object[] { }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (continueDelete != DialogResult.OK)
                    {
                        foreach (var loopCheckedUsed in checkedUsedList)
                        {
                            loopCheckedUsed.IsChecked = false;
                        }
                        gdGrid.DataSource = GridDS;
                        gdGrid.DataBind();
                        return;
                    }
                    checkedAutoFactoryCustomerList.RemoveAll(x => x.IsChecked == false);

                    gdGrid.DataSource = GridDS;
                    gdGrid.DataBind();
                }
                else
                {
                    //勾选的列表中不包含已被引用的[汽配商]
                    //确认删除操作
                    DialogResult confirmDeleteResult = MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0013, new object[] { checkedAutoFactoryCustomerList.Count }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (confirmDeleteResult != DialogResult.OK)
                    {
                        return;
                    }
                }
                #endregion

                foreach (var loopCheckedAutoFactoryCustomer in checkedAutoFactoryCustomerList)
                {
                    if (string.IsNullOrEmpty(loopCheckedAutoFactoryCustomer.AFC_ID))
                    {
                        continue;
                    }

                    //待删除的汽修商
                    MDLPIS_AutoFactoryCustomer deleteAutoFactoryCustomer = new MDLPIS_AutoFactoryCustomer
                    {
                        WHERE_AFC_ID = loopCheckedAutoFactoryCustomer.AFC_ID
                    };
                    deleteAutoFactoryCustomerList.Add(deleteAutoFactoryCustomer);
                }
                #endregion
            }

            #endregion

            #region 删除数据
            if (deleteAutoFactoryCustomerList.Count > 0)
            {
                var deleteAutoFactoryCustomerResult = _bll.DeleteAutoFactoryCustomer(deleteAutoFactoryCustomerList);
                if (!deleteAutoFactoryCustomerResult)
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            #endregion

            //3.清空【详情】画面数据
            InitializeDetailTabControls();
            //刷新列表
            RefreshList(true);

            //4.将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();
        }

        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            //1.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
            //2.设置查询条件（翻页相关属性不用设置）
            base.ConditionDS = new AutoFactoryCustomerManagerQCModel()
            {
                //SqlId
                SqlId = SQLID.PIS_AutoFactoryCustomerManager_SQL03,
                //汽修商编码
                WHERE_AFC_Code = txtWhere_AFC_Code.Text.Trim(),
                //汽修商名称
                WHERE_AFC_Name = txtWhere_AFC_Name.Text.Trim(),
                //手机号码
                WHERE_AFC_PhoneNo = txtWhere_AFC_PhoneNo.Text.Trim(),
                //汽修商户组织
                WHERE_AFC_AROrg_Name = txtWhere_AFC_AROrg_Name.Text.Trim(),
                //汽修商户组织联系人
                WHERE_AFC_AROrg_Contacter = txtWhere_AFC_AROrg_Contacter.Text.Trim(),
                //汽修商户组织联系方式
                WHERE_AFC_AROrg_Phone = txtWhere_AFC_AROrg_Phone.Text.Trim(),
                //组织ID
                WHERE_AFC_Org_ID = LoginInfoDAX.UserID == SysConst.SUPER_ADMIN ? null : LoginInfoDAX.OrgID,
            };
            if (ckWhere_AFC_IsPlatform.CheckState == CheckState.Checked)
            {
                //是否平台商户
                base.ConditionDS.WHERE_AFC_IsPlatform = true;
            }
            else if (ckWhere_AFC_IsPlatform.CheckState == CheckState.Unchecked)
            {
                //是否平台商户
                base.ConditionDS.WHERE_AFC_IsPlatform = false;
            }
            else
            {
                //是否平台商户
                base.ConditionDS.WHERE_AFC_IsPlatform = null;
            }
            if (ckWhere_AFC_IsEndSales.CheckState == CheckState.Checked)
            {
                //是否终止销售
                base.ConditionDS.WHERE_AFC_IsEndSales = true;
            }
            else if (ckWhere_AFC_IsEndSales.CheckState == CheckState.Unchecked)
            {
                //是否终止销售
                base.ConditionDS.WHERE_AFC_IsEndSales = false;
            }
            else
            {
                //是否终止销售
                base.ConditionDS.WHERE_AFC_IsEndSales = null;
            }
            //3.执行基类方法（注意：基类使用模糊查询）
            base.QueryAction();
            //4.Grid绑定数据源
            gdGrid.DataSource = base.GridDS;
            gdGrid.DataBind();
            //5.设置Grid自适应列宽（根据单元格内容）
            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdGrid.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
            //6.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;

        }
        
        /// <summary>
        /// 清空
        /// </summary>
        public override void ClearAction()
        {
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
            if (ViewHasChanged())
            {
                return true;
            }
            return false;
        }

        #region 导航按钮

        /// <summary>
        /// 初始化导航按钮
        /// </summary>
        public override void InitializeNavigate()
        {
            base.InitializeNavigate();
            SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.DEPOSITMONEY, true, false);
        }

        /// <summary>
        /// 开户
        /// </summary>
        public override void WalletCreateAccount()
        {
            //待开户的客户
            CustomerQueryUIModel customerToCreateAccount = new CustomerQueryUIModel();
            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                //选中【详情】Tab的场合
                customerToCreateAccount.CustomerID = DetailDS.AFC_ID;
                customerToCreateAccount.CustomerName = DetailDS.AFC_IsPlatform == true ? DetailDS.AFC_AROrg_Name : DetailDS.AFC_Name;
                customerToCreateAccount.AutoFactoryCode = DetailDS.AFC_Code;
                customerToCreateAccount.AutoFactoryName = DetailDS.AFC_Name;
                customerToCreateAccount.AutoFactoryOrgCode = DetailDS.AFC_AROrg_Code;
                customerToCreateAccount.CustomerType = DetailDS.AFC_IsPlatform == true ? CustomerTypeEnum.Name.PTNQXSH : CustomerTypeEnum.Name.YBQXSH;
            }
            else
            {
                //选中【列表】Tab的场合
                gdGrid.UpdateData();

                var selectedCustomerList = GridDS.Where(x => x.IsChecked == true).ToList();
                if (selectedCustomerList.Count == 0 || selectedCustomerList.Count > 1)
                {
                    //请选择一个普通客户进行开户
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0006, new object[] { MsgParam.ONE + SystemTableEnums.Name.PIS_AutoFactoryCustomer, SystemNavigateEnum.Name.CREATEACCOUNT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    for (int i = 0; i < selectedCustomerList.Count; i++)
                    {
                        if (i != 0)
                        {
                            selectedCustomerList[i].IsChecked = false;
                        }
                        gdGrid.DataSource = GridDS;
                        gdGrid.DataBind();
                    }
                    return;
                }
                customerToCreateAccount.CustomerID = selectedCustomerList[0].AFC_ID;
                customerToCreateAccount.CustomerName = selectedCustomerList[0].AFC_IsPlatform == true ? selectedCustomerList[0].AFC_AROrg_Name : selectedCustomerList[0].AFC_Name;
                customerToCreateAccount.AutoFactoryCode = selectedCustomerList[0].AFC_Code;
                customerToCreateAccount.AutoFactoryName = selectedCustomerList[0].AFC_Name;
                customerToCreateAccount.AutoFactoryOrgCode = selectedCustomerList[0].AFC_AROrg_Code;
                customerToCreateAccount.CustomerType = selectedCustomerList[0].AFC_IsPlatform == true ? CustomerTypeEnum.Name.PTNQXSH : CustomerTypeEnum.Name.YBQXSH;
            }

            if (string.IsNullOrEmpty(customerToCreateAccount.CustomerID)
                || string.IsNullOrEmpty(customerToCreateAccount.CustomerName))
            {
                //没有获取到客户，开户失败
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.PIS_AutoFactoryCustomer, SystemNavigateEnum.Name.CREATEACCOUNT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //客户信息
                {ComViewParamKey.CustomerInfo.ToString(), customerToCreateAccount},
            };

            //跳转到[钱包开户]
            SystemFunction.ShowViewFromView(MsgParam.WALLET_CREATEACCOUNT, ViewClassFullNameConst.RIA_FrmWalletCreateAccount, true, PageDisplayMode.TabPage, paramViewParameters, null);
        }
        #endregion

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 工具生成
            //组织ID
            txtAFC_Org_ID.Clear();
            //汽修商编码
            txtAFC_Code.Clear();
            //汽修商名称
            txtAFC_Name.Clear();
            //汽修商联系人
            txtAFC_Contacter.Clear();
            //汽修商联系方式
            txtAFC_PhoneNo.Clear();
            //汽修商地址
            txtAFC_Address.Clear();
            //汽修商组织编码
            txtAFC_AROrg_Code.Clear();
            //汽修商组织名称
            txtAFC_AROrg_Name.Clear();
            //汽修商组织联系人
            txtAFC_AROrg_Contacter.Clear();
            //汽修商组织联系方式
            txtAFC_AROrg_Phone.Clear();
            //汽修商组织地址
            txtAFC_AROrg_Address.Clear();
            //信用额度
            numAFC_CreditAmount.Value = null;
            //默认支付类型名称
            cbAFC_PaymentTypeName.Clear();
            //默认开票类型名称
            cbAFC_BillingTypeName.Value = null;
            //默认物流人员类型名称
            cbAFC_DeliveryTypeName.Value = null;
            //默认物流人员ID
            txtAFC_DeliveryByID.Clear();
            //默认物流人员名称
            txtAFC_DeliveryByName.Clear();
            //默认物流人员手机号
            txtAFC_DeliveryByPhoneNo.Clear();
            //是否平台商户
            ckAFC_IsPlatform.Checked = true;
            ckAFC_IsPlatform.CheckState = CheckState.Unchecked;
            //是否终止销售
            ckAFC_IsEndSales.Checked = true;
            ckAFC_IsEndSales.CheckState = CheckState.Unchecked;
            //配件价格类别
            mcbAFC_AutoPartsPriceType.Clear();
            //备注
            txtAFC_Remark.Clear();
            //有效
            ckAFC_IsValid.Checked = true;
            ckAFC_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtAFC_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtAFC_CreatedTime.Value = DateTime.Now;
            //修改人
            txtAFC_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtAFC_UpdatedTime.Value = DateTime.Now;
            //版本号
            txtAFC_VersionNo.Clear();
            //汽修商客户ID
            txtAFC_ID.Clear();
            //给 汽修商编码 设置焦点
            lblAFC_Code.Focus();
            #endregion

            #region 初始化下拉框
            //默认支付类型
            _paymentTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.TradeType);
            cbAFC_PaymentTypeName.DisplayMember = SysConst.EN_TEXT;
            cbAFC_PaymentTypeName.ValueMember = SysConst.EN_Code;
            cbAFC_PaymentTypeName.DataSource = _paymentTypeList;
            cbAFC_PaymentTypeName.DataBind();
            //默认支付类型为{钱包}
            cbAFC_PaymentTypeName.Text = TradeTypeEnum.Name.WALLET;
            cbAFC_PaymentTypeName.Value = TradeTypeEnum.Code.WALLET;

            //默认开票类型
            _billingTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.BillingType);
            cbAFC_BillingTypeName.DisplayMember = SysConst.EN_TEXT;
            cbAFC_BillingTypeName.ValueMember = SysConst.EN_Code;
            cbAFC_BillingTypeName.DataSource = _billingTypeList;
            cbAFC_BillingTypeName.DataBind();
            //默认开票类型为{普通票}
            cbAFC_BillingTypeName.Text = BillingTypeEnum.Name.GENERAL;
            cbAFC_BillingTypeName.Value = BillingTypeEnum.Code.GENERAL;

            //默认物流人员类型
            _deliveryTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.DeliveryType);
            cbAFC_DeliveryTypeName.DisplayMember = SysConst.EN_TEXT;
            cbAFC_DeliveryTypeName.ValueMember = SysConst.EN_Code;
            cbAFC_DeliveryTypeName.DataSource = _deliveryTypeList;
            cbAFC_DeliveryTypeName.DataBind();
            //默认物流人员类型为{员工}
            cbAFC_DeliveryTypeName.Text = DeliveryTypeEnum.Name.YG;
            cbAFC_DeliveryTypeName.Value = DeliveryTypeEnum.Code.YG;

            //配件价格类别（从码表获取）
            _autoPartsPriceTypeList = CodeTableHelp.GetEnumForComboBoxWithValueText(CodeType.AutoPartsPriceType);
            mcbAFC_AutoPartsPriceType.DisplayMember = SysConst.EN_TEXT;
            mcbAFC_AutoPartsPriceType.ValueMember = SysConst.EN_TEXT;
            mcbAFC_AutoPartsPriceType.DataSource = _autoPartsPriceTypeList;

            #endregion

            //默认组织为当前登录组织
            txtAFC_Org_ID.Text = LoginInfoDAX.OrgID;
            //默认信用额度为0
            numAFC_CreditAmount.Value = Convert.ToDecimal(0);
        }

        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //汽修商编码
            txtWhere_AFC_Code.Clear();
            //汽修商名称
            txtWhere_AFC_Name.Clear();
            //汽修商手机号码
            txtWhere_AFC_PhoneNo.Clear();
            //汽修商组织名称
            txtWhere_AFC_AROrg_Name.Clear();
            //汽修商组织联系人
            txtWhere_AFC_AROrg_Contacter.Clear();
            //汽修商组织联系方式
            txtWhere_AFC_AROrg_Phone.Clear();
            //是否平台商户
            ckWhere_AFC_IsPlatform.Checked = false;
            ckWhere_AFC_IsPlatform.CheckState = CheckState.Indeterminate;
            //终止销售
            ckWhere_AFC_IsEndSales.Checked = false;
            ckWhere_AFC_IsEndSales.CheckState = CheckState.Unchecked;
            //给 汽修商编码 设置焦点
            lblWhere_AFC_Code.Focus();
            #endregion

            //清空Grid
            GridDS = new BindingList<AutoFactoryCustomerManagerUIModel>();
            gdGrid.DataSource = base.GridDS;
            gdGrid.DataBind();

            base.ClearAction();

            #endregion
        }
        
        /// <summary>
        /// 将Grid中选中的数据赋值给【详情】Tab内的对应控件
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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            DetailDS = base.GridDS.FirstOrDefault(x => x.AFC_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_ID].Value);
            if (DetailDS == null || string.IsNullOrEmpty(DetailDS.AFC_ID))
            {
                return;
            }

            if (txtAFC_ID.Text != DetailDS.AFC_ID
                || (txtAFC_ID.Text == DetailDS.AFC_ID && txtAFC_VersionNo.Text != DetailDS.AFC_VersionNo?.ToString()))
            {
                if (txtAFC_ID.Text == DetailDS.AFC_ID && txtAFC_VersionNo.Text != DetailDS.AFC_VersionNo?.ToString())
                {
                    //数据版本已过期，将加载最新详情。
                    MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.DataHasOverdue }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else if (ViewHasChanged())
                {
                    //将放弃之前的修改，是否继续？
                    DialogResult dialogResult = MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.ConfirmGiveUpEdit }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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

            //设置详情页面控件以及导航按钮是否可编辑
            SetDetailControl();
            //设置汽配汽修组织信息
            SetUserOrgInfo();
        }
        
        /// <summary>
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //组织ID
            txtAFC_Org_ID.Text = DetailDS.AFC_Org_ID;
            //汽修商编码
            txtAFC_Code.Text = DetailDS.AFC_Code;
            //汽修商名称
            txtAFC_Name.Text = DetailDS.AFC_Name;
            //汽修商联系人
            txtAFC_Contacter.Text = DetailDS.AFC_Contacter;
            //汽修商联系方式
            txtAFC_PhoneNo.Text = DetailDS.AFC_PhoneNo;
            //汽修商地址
            txtAFC_Address.Text = DetailDS.AFC_Address;
            //汽修商组织编码
            txtAFC_AROrg_Code.Text = DetailDS.AFC_AROrg_Code;
            //汽修商组织名称
            txtAFC_AROrg_Name.Text = DetailDS.AFC_AROrg_Name;
            //汽修商组织联系人
            txtAFC_AROrg_Contacter.Text = DetailDS.AFC_AROrg_Contacter;
            //汽修商组织联系方式
            txtAFC_AROrg_Phone.Text = DetailDS.AFC_AROrg_Phone;
            //汽修商组织地址
            txtAFC_AROrg_Address.Text = DetailDS.AFC_AROrg_Address;
            //信用额度
            numAFC_CreditAmount.Value = DetailDS.AFC_CreditAmount ?? 0;
            //默认支付类型名称
            cbAFC_PaymentTypeName.Text = DetailDS.AFC_PaymentTypeName;
            //默认支付类型编码
            cbAFC_PaymentTypeName.Value = DetailDS.AFC_PaymentTypeCode;
            //默认开票类型名称
            cbAFC_BillingTypeName.Text = DetailDS.AFC_BillingTypeName;
            //默认开票类型名称
            cbAFC_BillingTypeName.Value = DetailDS.AFC_BillingTypeCode;
            //默认物流人员类型名称
            cbAFC_DeliveryTypeName.Text = DetailDS.AFC_DeliveryTypeName;
            //默认物流人员类型名称
            cbAFC_DeliveryTypeName.Value = DetailDS.AFC_DeliveryTypeCode;
            //默认物流人员ID
            txtAFC_DeliveryByID.Text = DetailDS.AFC_DeliveryByID;
            //默认物流人员名称
            txtAFC_DeliveryByName.Text = DetailDS.AFC_DeliveryByName;
            //默认物流人员手机号
            txtAFC_DeliveryByPhoneNo.Text = DetailDS.AFC_DeliveryByPhoneNo;
            //是否平台商户
            if (DetailDS.AFC_IsPlatform != null)
            {
                ckAFC_IsPlatform.Checked = DetailDS.AFC_IsPlatform.Value;
            }
            //是否终止销售
            if (DetailDS.AFC_IsEndSales != null)
            {
                ckAFC_IsEndSales.Checked = DetailDS.AFC_IsEndSales.Value;
            }
            //配件价格类别
            mcbAFC_AutoPartsPriceType.SelectedValue = DetailDS.AFC_AutoPartsPriceType;
            //备注
            txtAFC_Remark.Text = DetailDS.AFC_Remark;
            //有效
            if (DetailDS.AFC_IsValid != null)
            {
                ckAFC_IsValid.Checked = DetailDS.AFC_IsValid.Value;
            }
            //创建人
            txtAFC_CreatedBy.Text = DetailDS.AFC_CreatedBy;
            //创建时间
            dtAFC_CreatedTime.Value = DetailDS.AFC_CreatedTime;
            //修改人
            txtAFC_UpdatedBy.Text = DetailDS.AFC_UpdatedBy;
            //修改时间
            dtAFC_UpdatedTime.Value = DetailDS.AFC_UpdatedTime;
            //版本号
            txtAFC_VersionNo.Value = DetailDS.AFC_VersionNo;
            //汽修商客户ID
            txtAFC_ID.Text = DetailDS.AFC_ID;
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
                foreach (UltraGridColumn loopColumn in gdGrid.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (loopColumn.IsGroupByColumn)
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
            if (ckAFC_IsPlatform.Checked == true)
            {
                //平台汽修商户的场合，验证汽修商组织联系人、联系方式

                //验证汽修商组织联系人
                if (string.IsNullOrEmpty(txtAFC_AROrg_Contacter.Text))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(),
                        MsgHelp.GetMsg(MsgCode.E_0001,
                            new object[] { SystemTableColumnEnums.PIS_AutoFactoryCustomer.Name.AFC_AROrg_Contacter }),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAFC_AROrg_Contacter.Focus();
                    return false;
                }
                //验证汽修商组织手机号码
                if (string.IsNullOrEmpty(txtAFC_AROrg_Phone.Text))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(),
                        MsgHelp.GetMsg(MsgCode.E_0001,
                            new object[] { SystemTableColumnEnums.PIS_AutoFactoryCustomer.Name.AFC_AROrg_Phone }),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAFC_AROrg_Phone.Focus();
                    return false;
                }

                //验证授权组织
                if (_arOrgSupOrgAuthorityList.Count == 0)
                {
                    MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ORGNIZATION }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

            }
            else
            {
                //一般汽修商户的场合，验证汽修商联系人、联系方式

                //验证汽修商编码
                if (string.IsNullOrEmpty(txtAFC_Code.Text))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.PIS_AutoFactoryCustomer.Name.AFC_Code }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAFC_Code.Focus();
                    return false;
                }
                //验证汽修商名称
                if (string.IsNullOrEmpty(txtAFC_Name.Text))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.PIS_AutoFactoryCustomer.Name.AFC_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAFC_Name.Focus();
                    return false;
                }
                //验证汽修商联系人
                if (string.IsNullOrEmpty(txtAFC_Contacter.Text))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.PIS_AutoFactoryCustomer.Name.AFC_Contacter }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAFC_Contacter.Focus();
                    return false;
                }
                //验证汽修商手机号码
                if (string.IsNullOrEmpty(txtAFC_PhoneNo.Text))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.PIS_AutoFactoryCustomer.Name.AFC_PhoneNo }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAFC_PhoneNo.Focus();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            DetailDS = new AutoFactoryCustomerManagerUIModel()
            {
                //组织ID
                AFC_Org_ID = ckAFC_IsPlatform.Checked ? null : txtAFC_Org_ID.Text,
                //汽修商编码
                AFC_Code = txtAFC_Code.Text.Trim(),
                //汽修商名称
                AFC_Name = txtAFC_Name.Text.Trim(),
                //汽修联系人
                AFC_Contacter = txtAFC_Contacter.Text.Trim(),
                //汽修商联系方式
                AFC_PhoneNo = txtAFC_PhoneNo.Text.Trim(),
                //汽修商地址
                AFC_Address = txtAFC_Address.Text.Trim(),
                //汽修商组织编码
                AFC_AROrg_Code = txtAFC_AROrg_Code.Text.Trim(),
                //汽修商组织名称
                AFC_AROrg_Name = txtAFC_AROrg_Name.Text.Trim(),
                //汽修商组织联系人
                AFC_AROrg_Contacter = txtAFC_AROrg_Contacter.Text.Trim(),
                //汽修商组织联系方式
                AFC_AROrg_Phone = txtAFC_AROrg_Phone.Text.Trim(),
                //汽修商组织地址
                AFC_AROrg_Address = txtAFC_AROrg_Address.Text.Trim(),
                //信用额度
                AFC_CreditAmount = Convert.ToDecimal(numAFC_CreditAmount.Value ?? 0),
                //默认支付类型名称
                AFC_PaymentTypeName = cbAFC_PaymentTypeName.Text,
                //默认支付类型编码
                AFC_PaymentTypeCode = cbAFC_PaymentTypeName.Value?.ToString(),
                //默认开票类型名称
                AFC_BillingTypeName = cbAFC_BillingTypeName.Text,
                //默认开票类型编码
                AFC_BillingTypeCode = cbAFC_BillingTypeName.Value?.ToString(),
                //默认物流人员类型名称
                AFC_DeliveryTypeName = cbAFC_DeliveryTypeName.Text,
                //默认物流人员类型编码
                AFC_DeliveryTypeCode = cbAFC_DeliveryTypeName.Value?.ToString(),
                //默认物流人员ID
                AFC_DeliveryByID = txtAFC_DeliveryByID.Text,
                //默认物流人员名称
                AFC_DeliveryByName = txtAFC_DeliveryByName.Text,
                //默认物流人员手机号
                AFC_DeliveryByPhoneNo = txtAFC_DeliveryByPhoneNo.Text,
                //是否平台商户
                AFC_IsPlatform = ckAFC_IsPlatform.Checked,
                //是否终止销售
                AFC_IsEndSales = ckAFC_IsEndSales.Checked,
                //配件价格类别
                AFC_AutoPartsPriceType = mcbAFC_AutoPartsPriceType.SelectedValue,
                //备注
                AFC_Remark = txtAFC_Remark.Text.Trim(),
                //有效
                AFC_IsValid = ckAFC_IsValid.Checked,
                //创建人
                AFC_CreatedBy = txtAFC_CreatedBy.Text.Trim(),
                //创建时间
                AFC_CreatedTime = (DateTime?)dtAFC_CreatedTime.Value ?? DateTime.Now,
                //修改人
                AFC_UpdatedBy = txtAFC_UpdatedBy.Text.Trim(),
                //修改时间
                AFC_UpdatedTime = (DateTime?)dtAFC_UpdatedTime.Value ?? DateTime.Now,
                //版本号
                AFC_VersionNo = Convert.ToInt64(txtAFC_VersionNo.Text.Trim() == "" ? "1" : txtAFC_VersionNo.Text.Trim()),
                //汽修商客户ID
                AFC_ID = txtAFC_ID.Text.Trim(),
            };
        }

        /// <summary>
        /// 设置详情页面控件以及导航按钮是否可编辑
        /// </summary>
        private void SetDetailControl()
        {
            if (string.IsNullOrEmpty(txtAFC_ID.Text)
                || string.IsNullOrEmpty(txtAFC_Name.Text))
            {
                //未保存的场合，[开户]按钮不可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.CREATEACCOUNT, true, false);
            }
            else
            {
                //已保存的场合，[开户]按钮可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.CREATEACCOUNT, true, true);
            }

            if (ckAFC_IsPlatform.Checked)
            {
                #region 平台内汽修商的场合，显示汽修商组织信息，显示组织授权

                txtAFC_Code.Enabled = false;
                txtAFC_Name.Enabled = false;

                lblRedAFC_Contacter.Visible = false;
                lblRedAFC_PhoneNo.Visible = false;

                panelAFC_AROrg_Name.Visible = true;
                txtAFC_AROrg_Name.Visible = true;
                panelAFC_AROrg_Contacter.Visible = true;
                txtAFC_AROrg_Contacter.Visible = true;
                panelAFC_AROrg_Phone.Visible = true;
                txtAFC_AROrg_Phone.Visible = true;
                lblAFC_AROrg_Address.Visible = true;
                txtAFC_AROrg_Address.Visible = true;

                //显示组织授权
                gbOrgAuthority.Visible = true;

                //平台商户的客户不能删除
                SetActionEnable(SystemActionEnum.Code.DELETE, false);

                //初始化树形控件
                InitializeTreeView();
                #endregion
            }
            else
            {
                #region 一般汽修商的场合，隐藏汽修商组织信息，隐藏组织授权

                txtAFC_Code.Enabled = true;
                txtAFC_Name.Enabled = true;

                lblRedAFC_Contacter.Visible = true;
                lblRedAFC_PhoneNo.Visible = true;

                panelAFC_AROrg_Name.Visible = false;
                txtAFC_AROrg_Name.Visible = false;
                panelAFC_AROrg_Contacter.Visible = false;
                txtAFC_AROrg_Contacter.Visible = false;
                panelAFC_AROrg_Phone.Visible = false;
                txtAFC_AROrg_Phone.Visible = false;
                lblAFC_AROrg_Address.Visible = false;
                txtAFC_AROrg_Address.Visible = false;

                //隐藏组织授权
                gbOrgAuthority.Visible = false;

                //一般汽修商客户能删除
                SetActionEnable(SystemActionEnum.Code.DELETE, true);

                #endregion
            }
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
        /// 获取最新的汽配汽修组织授权进行绑定
        /// </summary>
        private void SetUserOrgInfo()
        {
            #region 获取最新的汽配汽修组织授权进行绑定

            if (string.IsNullOrEmpty(txtAFC_ID.Text.Trim()))
            {
                return;
            }
            _isCanAfterCheck = false;

            //获取汽配汽修组织授权信息
            List<MDLSM_AROrgSupOrgAuthority> resultAROrgSupOrgAuthorityList = new List<MDLSM_AROrgSupOrgAuthority>();
            _bll.QueryForList<MDLSM_AROrgSupOrgAuthority, MDLSM_AROrgSupOrgAuthority>(new MDLSM_AROrgSupOrgAuthority()
            {
                WHERE_ASOAH_IsValid = true,
                //汽修商客户ID
                WHERE_ASOAH_AFC_ID = txtAFC_ID.Text.Trim(),
                //汽修商户编码
                WHERE_ASOAH_ARMerchant_Code = txtAFC_Code.Text.Trim(),
                //汽修商户组织编码
                WHERE_ASOAH_AROrg_Code = txtAFC_AROrg_Code.Text.Trim()
            }, resultAROrgSupOrgAuthorityList);

            if (resultAROrgSupOrgAuthorityList.Count > 0)
            {
                foreach (TreeNode loopAROrgSupOrgAuthorityNode in tvOrg.Nodes)
                {
                    if (loopAROrgSupOrgAuthorityNode == null)
                    {
                        continue;
                    }
                    bool isSaveAROrgSupOrgAuthority =
                        resultAROrgSupOrgAuthorityList.Any(
                            p => p.ASOAH_SupOrg_ID == loopAROrgSupOrgAuthorityNode.Tag?.ToString()
                                 && p.ASOAH_AFC_ID == txtAFC_ID.Text.Trim()
                                 && p.ASOAH_ARMerchant_Code == txtAFC_Code.Text.Trim()
                                 && p.ASOAH_AROrg_Code == txtAFC_AROrg_Code.Text.Trim());
                    loopAROrgSupOrgAuthorityNode.Checked = isSaveAROrgSupOrgAuthority;
                }
            }

            _isCanAfterCheck = true;

            //汽配汽修组织授权数据源
            _arOrgSupOrgAuthorityList = new SkyCarBindingList<AROrgSupOrgAuthorityUIModel, MDLSM_AROrgSupOrgAuthority>();
            _bll.CopyModelList(resultAROrgSupOrgAuthorityList, _arOrgSupOrgAuthorityList);
            //开始监控汽配汽修组织授权List变化
            _arOrgSupOrgAuthorityList.StartMonitChanges();

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
                    var curHead = GridDS.FirstOrDefault(x => x.AFC_ID == DetailDS.AFC_ID);
                    if (curHead != null)
                    {
                        GridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = GridDS.FirstOrDefault(x => x.AFC_ID == DetailDS.AFC_ID);
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
        
        #endregion

    }
}
