using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
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
using SkyCar.Coeus.UIModel.Common.APModel;
using SkyCar.Coeus.UIModel.Common.UIModel;

namespace SkyCar.Coeus.UI.PIS
{
    /// <summary>
    /// 普通客户管理
    /// </summary>
    public partial class FrmGeneralCustomerManager : BaseFormCardList<GeneralCustomerManagerUIModel, GeneralCustomerManagerQCModel, MDLPIS_GeneralCustomer>
    {
        #region 全局变量

        /// <summary>
        /// 普通客户管理BLL
        /// </summary>
        private GeneralCustomerManagerBLL _bll = new GeneralCustomerManagerBLL();

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

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmGeneralCustomerManager构造方法
        /// </summary>
        public FrmGeneralCustomerManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmGeneralCustomerManager_Load(object sender, EventArgs e)
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

                if (string.IsNullOrEmpty(txtGC_ID.Text))
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
        /// 姓名KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_GC_Name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //执行查询
                //QueryAction();
            }
        }
        /// <summary>
        /// 手机号码KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_GC_PhoneNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //执行查询
                //QueryAction();
            }
        }
        /// <summary>
        /// 有效CheckedChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckWhere_GC_IsValid_CheckedChanged(object sender, EventArgs e)
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
        private void txtGC_DeliveryByName_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(cbGC_DeliveryTypeName.Text))
            {
                //请先选择物流人员类型，再选择物流人员
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0034, new object[] { MsgParam.DELIVERYBY_TYPE, MsgParam.DELIVERYBY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cbGC_DeliveryTypeName.Text == DeliveryTypeEnum.Name.YG)
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
                    txtGC_DeliveryByName.Text = userQuery.SelectedGridList[0].User_Name;
                    //物流人员ID
                    txtGC_DeliveryByID.Text = userQuery.SelectedGridList[0].User_ID;
                    //物流人员手机号
                    txtGC_DeliveryByPhoneNo.Text = userQuery.SelectedGridList[0].User_PhoneNo;
                }

                #endregion
            }
            else
            {
                #region 物流人员为{第三方个人}、{第三方公司}的场合

                //根据物流订单.[物流人员类型]查询相应物流订单中物流人员信息
                List<ComComboBoxDataSourceTC> paramDeliveryTypeList = new List<ComComboBoxDataSourceTC>();
                paramDeliveryTypeList = _deliveryTypeList.Where(x => x.Text == cbGC_DeliveryTypeName.Text).ToList();

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
                    txtGC_DeliveryByName.Text = logisticsBillQuery.SelectedGridList[0].LB_DeliveryBy;
                    //物流人员手机号
                    txtGC_DeliveryByPhoneNo.Text = logisticsBillQuery.SelectedGridList[0].LB_PhoneNo;
                }

                #endregion
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
            bool saveResult = _bll.SaveDetailDS(DetailDS);
            if (!saveResult)
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
        }

        /// <summary>
        /// 删除
        /// </summary>
        public override void DeleteAction()
        {
            //2.执行删除
            #region 准备数据

            //待删除的[普通客户]列表
            List<MDLPIS_GeneralCustomer> deleteGeneralCustomerList = new List<MDLPIS_GeneralCustomer>();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 详情删除
                if (string.IsNullOrEmpty(txtGC_ID.Text))
                {
                    //普通客户信息为空，不能删除
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0016, new object[] { SystemTableEnums.Name.PIS_GeneralCustomer, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                #region 验证普通客户是否已被使用

                if (!string.IsNullOrEmpty(txtGC_ID.Text.Trim()))
                {
                    //验证普通客户是否已被使用
                    StringBuilder customerIDs = new StringBuilder();
                    customerIDs.Append(SysConst.Semicolon_DBC + txtGC_ID.Text + SysConst.Semicolon_DBC);
                    //查询普通客户是否被引用过
                    List<MDLPIS_GeneralCustomer> generalCustomerList = new List<MDLPIS_GeneralCustomer>();
                    _bll.QueryForList(SQLID.PIS_GeneralCustomerManager_SQL02, new MDLPIS_GeneralCustomer
                    {
                        WHERE_GC_ID = customerIDs.ToString()
                    }, generalCustomerList);
                    if (generalCustomerList.Count > 0)
                    {
                        MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0007, new object[] { txtGC_Name.Text.Trim(), MsgParam.APPLY, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                #endregion

                //确认删除操作
                DialogResult dialogResult = MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0012), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                //待删除的普通客户
                MDLPIS_GeneralCustomer deleteGeneralCustomer = new MDLPIS_GeneralCustomer
                {
                    WHERE_GC_ID = txtGC_ID.Text.Trim()
                };
                deleteGeneralCustomerList.Add(deleteGeneralCustomer);
                #endregion
            }
            else
            {
                #region 列表删除
                gdGrid.UpdateData();
                //勾选的普通客户列表
                List<GeneralCustomerManagerUIModel> checkedGeneralCustomerList = GridDS.Where(x => x.IsChecked == true).ToList();
                if (checkedGeneralCustomerList.Count == 0)
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { SystemTableEnums.Name.PIS_GeneralCustomer, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                #region 验证普通客户是否已被使用

                //验证普通客户是否已被使用
                StringBuilder customerIDs = new StringBuilder();
                customerIDs.Append(SysConst.Semicolon_DBC);
                foreach (var loopSelectedItem in checkedGeneralCustomerList)
                {
                    customerIDs.Append(loopSelectedItem.GC_ID + SysConst.Semicolon_DBC);
                }
                //查询普通客户是否被引用过
                List<MDLPIS_GeneralCustomer> generalCustomerList = new List<MDLPIS_GeneralCustomer>();
                _bll.QueryForList(SQLID.PIS_GeneralCustomerManager_SQL02, new MDLPIS_GeneralCustomer
                {
                    WHERE_GC_ID = customerIDs.ToString()
                }, generalCustomerList);
                if (generalCustomerList.Count > 0)
                {
                    StringBuilder customerName = new StringBuilder();
                    int i = 0;
                    foreach (var loopWarehouse in generalCustomerList)
                    {
                        i++;
                        if (i == 1)
                        {
                            customerName.Append(loopWarehouse.GC_Name);
                        }
                        else
                        {
                            customerName.Append(SysConst.Comma_DBC + loopWarehouse.GC_Name);
                        }
                    }
                    //普通客户已经被使用，不能删除
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0007, new object[] { customerName, MsgParam.APPLY, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                #endregion
                
                foreach (var loopCheckedGeneralCustomer in checkedGeneralCustomerList)
                {
                    if (string.IsNullOrEmpty(loopCheckedGeneralCustomer.GC_ID))
                    {
                        continue;
                    }

                    //待删除的普通客户
                    MDLPIS_GeneralCustomer deleteGeneralCustomer = new MDLPIS_GeneralCustomer
                    {
                        WHERE_GC_ID = loopCheckedGeneralCustomer.GC_ID
                    };
                    deleteGeneralCustomerList.Add(deleteGeneralCustomer);
                }
                #endregion
            }

            #endregion

            #region 删除数据
            if (deleteGeneralCustomerList.Count > 0)
            {
                var deleteGeneralCustomerResult = _bll.DeleteGeneralCustomer(deleteGeneralCustomerList);
                if (!deleteGeneralCustomerResult)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //删除成功
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
            base.ConditionDS = new GeneralCustomerManagerQCModel()
            {
                //姓名
                WHERE_GC_Name = txtWhere_GC_Name.Text.Trim(),
                //手机号码
                WHERE_GC_PhoneNo = txtWhere_GC_PhoneNo.Text.Trim(),
                //有效
                WHERE_GC_IsValid = ckWhere_GC_IsValid.Checked,
                //组织ID
                WHERE_GC_Org_ID = LoginInfoDAX.OrgID,
            };
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
                //验证该客户是否已开户
                List<WalletInfoUIModel> resultWalletList = BLLCom.GetWalletListByOwnerInfo(CustomerTypeEnum.Name.PTKH, DetailDS.GC_ID);
                if (resultWalletList.Count > 0)
                {
                    //该客户已开户
                    MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "客户：" + DetailDS.GC_Name + "已开户！" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                customerToCreateAccount.CustomerID = DetailDS.GC_ID;
                customerToCreateAccount.CustomerName = DetailDS.GC_Name;
                customerToCreateAccount.CustomerType = CustomerTypeEnum.Name.PTKH;
            }
            else
            {
                //选中【列表】Tab的场合
                gdGrid.UpdateData();

                var selectedCustomerList = GridDS.Where(x => x.IsChecked == true).ToList();
                if (selectedCustomerList.Count == 0 || selectedCustomerList.Count > 1)
                {
                    //请选择一个普通客户进行开户
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0006, new object[] { MsgParam.ONE + SystemTableEnums.Name.PIS_GeneralCustomer, SystemNavigateEnum.Name.CREATEACCOUNT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                //验证该客户是否已开户
                List<WalletInfoUIModel> resultWalletList = BLLCom.GetWalletListByOwnerInfo(CustomerTypeEnum.Name.PTKH, selectedCustomerList[0].GC_ID);
                if (resultWalletList.Count > 0)
                {
                    //该客户已开户
                    MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "客户：" + selectedCustomerList[0].GC_Name + "已开户！" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    selectedCustomerList[0].IsChecked = false;
                    gdGrid.DataSource = GridDS;
                    gdGrid.DataBind();
                    return;
                }
                customerToCreateAccount.CustomerID = selectedCustomerList[0].GC_ID;
                customerToCreateAccount.CustomerName = selectedCustomerList[0].GC_Name;
                customerToCreateAccount.CustomerType = CustomerTypeEnum.Name.PTKH;
            }

            if (string.IsNullOrEmpty(customerToCreateAccount.CustomerID)
                || string.IsNullOrEmpty(customerToCreateAccount.CustomerName))
            {
                //没有获取到客户，开户失败
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.PIS_GeneralCustomer, SystemNavigateEnum.Name.CREATEACCOUNT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtGC_Org_ID.Clear();
            //姓名
            txtGC_Name.Clear();
            //手机号码
            txtGC_PhoneNo.Clear();
            //地址
            txtGC_Address.Clear();
            //信用额度
            numGC_CreditAmount.Value = null;
            //默认支付类型名称
            cbGC_PaymentTypeName.Clear();
            //默认开票类型名称
            cbGC_BillingTypeName.Value = null;
            //默认物流人员类型名称
            cbGC_DeliveryTypeName.Value = null;
            //默认物流人员ID
            txtGC_DeliveryByID.Clear();
            //默认物流人员名称
            txtGC_DeliveryByName.Clear();
            //默认物流人员手机号
            txtGC_DeliveryByPhoneNo.Clear();
            //是否终止销售
            ckGC_IsEndSales.Checked = true;
            ckGC_IsEndSales.CheckState = CheckState.Unchecked;
            //配件价格类别
            mcbGC_AutoPartsPriceType.Clear();
            //备注
            txtGC_Remark.Clear();
            //有效
            ckGC_IsValid.Checked = true;
            ckGC_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtGC_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtGC_CreatedTime.Value = DateTime.Now;
            //修改人
            txtGC_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtGC_UpdatedTime.Value = DateTime.Now;
            //普通客户ID
            txtGC_ID.Clear();
            //版本号
            txtGC_VersionNo.Clear();
            //给 姓名 设置焦点
            lblGC_Name.Focus();
            #endregion

            #region 初始化下拉框
            //默认支付类型
            _paymentTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.TradeType);
            cbGC_PaymentTypeName.DisplayMember = SysConst.EN_TEXT;
            cbGC_PaymentTypeName.ValueMember = SysConst.EN_Code;
            cbGC_PaymentTypeName.DataSource = _paymentTypeList;
            cbGC_PaymentTypeName.DataBind();
            //默认支付类型为{钱包}
            cbGC_PaymentTypeName.Text = TradeTypeEnum.Name.CASH;
            cbGC_PaymentTypeName.Value = TradeTypeEnum.Code.CASH;

            //默认开票类型
            _billingTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.BillingType);
            cbGC_BillingTypeName.DisplayMember = SysConst.EN_TEXT;
            cbGC_BillingTypeName.ValueMember = SysConst.EN_Code;
            cbGC_BillingTypeName.DataSource = _billingTypeList;
            cbGC_BillingTypeName.DataBind();
            //默认开票类型为{普通票}
            cbGC_BillingTypeName.Text = BillingTypeEnum.Name.GENERAL;
            cbGC_BillingTypeName.Value = BillingTypeEnum.Code.GENERAL;

            //默认物流人员类型
            _deliveryTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.DeliveryType);
            cbGC_DeliveryTypeName.DisplayMember = SysConst.EN_TEXT;
            cbGC_DeliveryTypeName.ValueMember = SysConst.EN_Code;
            cbGC_DeliveryTypeName.DataSource = _deliveryTypeList;
            cbGC_DeliveryTypeName.DataBind();
            //默认物流人员类型为{员工}
            cbGC_DeliveryTypeName.Text = DeliveryTypeEnum.Name.YG;
            cbGC_DeliveryTypeName.Value = DeliveryTypeEnum.Code.YG;

            //配件价格类别（从码表获取）
            _autoPartsPriceTypeList = CodeTableHelp.GetEnumForComboBoxWithValueText(CodeType.AutoPartsPriceType);
            mcbGC_AutoPartsPriceType.DisplayMember = SysConst.EN_TEXT;
            mcbGC_AutoPartsPriceType.ValueMember = SysConst.EN_TEXT;
            mcbGC_AutoPartsPriceType.DataSource = _autoPartsPriceTypeList;

            #endregion

            //默认组织为当前登录组织
            txtGC_Org_ID.Text = LoginInfoDAX.OrgID;
            //默认信用额度为0
            numGC_CreditAmount.Text = Convert.ToDecimal(0).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //姓名
            txtWhere_GC_Name.Clear();
            //手机号码
            txtWhere_GC_PhoneNo.Clear();
            //有效
            ckWhere_GC_IsValid.Checked = true;
            ckWhere_GC_IsValid.CheckState = CheckState.Checked;
            //给 姓名 设置焦点
            lblWhere_GC_Name.Focus();
            #endregion

            //清空Grid
            GridDS = new BindingList<GeneralCustomerManagerUIModel>();
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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_GeneralCustomer.Code.GC_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_GeneralCustomer.Code.GC_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            DetailDS = base.GridDS.FirstOrDefault(x => x.GC_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_GeneralCustomer.Code.GC_ID].Value);
            if (DetailDS == null || string.IsNullOrEmpty(DetailDS.GC_ID))
            {
                return;
            }

            if (txtGC_ID.Text != DetailDS.GC_ID
                || (txtGC_ID.Text == DetailDS.GC_ID && txtGC_VersionNo.Text != DetailDS.GC_VersionNo?.ToString()))
            {
                if (txtGC_ID.Text == DetailDS.GC_ID && txtGC_VersionNo.Text != DetailDS.GC_VersionNo?.ToString())
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
        }

        /// <summary>
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //组织ID
            txtGC_Org_ID.Text = DetailDS.GC_Org_ID;
            //姓名
            txtGC_Name.Text = DetailDS.GC_Name;
            //手机号码
            txtGC_PhoneNo.Text = DetailDS.GC_PhoneNo;
            //地址
            txtGC_Address.Text = DetailDS.GC_Address;
            //信用额度
            numGC_CreditAmount.Text = DetailDS.GC_CreditAmount?.ToString();
            //默认支付类型名称
            cbGC_PaymentTypeName.Text = DetailDS.GC_PaymentTypeName;
            //默认支付类型编码
            cbGC_PaymentTypeName.Value = DetailDS.GC_PaymentTypeCode;
            //默认开票类型名称
            cbGC_BillingTypeName.Text = DetailDS.GC_BillingTypeName;
            //默认开票类型名称
            cbGC_BillingTypeName.Value = DetailDS.GC_BillingTypeCode;
            //默认物流人员类型名称
            cbGC_DeliveryTypeName.Text = DetailDS.GC_DeliveryTypeName;
            //默认物流人员类型名称
            cbGC_DeliveryTypeName.Value = DetailDS.GC_DeliveryTypeCode;
            //默认物流人员ID
            txtGC_DeliveryByID.Text = DetailDS.GC_DeliveryByID;
            //默认物流人员名称
            txtGC_DeliveryByName.Text = DetailDS.GC_DeliveryByName;
            //默认物流人员手机号
            txtGC_DeliveryByPhoneNo.Text = DetailDS.GC_DeliveryByPhoneNo;
            //是否终止销售
            if (DetailDS.GC_IsEndSales != null)
            {
                ckGC_IsEndSales.Checked = DetailDS.GC_IsEndSales.Value;
            }
            //配件价格类别
            mcbGC_AutoPartsPriceType.SelectedValue = DetailDS.GC_AutoPartsPriceType;
            //备注
            txtGC_Remark.Text = DetailDS.GC_Remark;
            //有效
            if (DetailDS.GC_IsValid != null)
            {
                ckGC_IsValid.Checked = DetailDS.GC_IsValid.Value;
            }
            //创建人
            txtGC_CreatedBy.Text = DetailDS.GC_CreatedBy;
            //创建时间
            dtGC_CreatedTime.Value = DetailDS.GC_CreatedTime;
            //修改人
            txtGC_UpdatedBy.Text = DetailDS.GC_UpdatedBy;
            //修改时间
            dtGC_UpdatedTime.Value = DetailDS.GC_UpdatedTime;
            //普通客户ID
            txtGC_ID.Text = DetailDS.GC_ID;
            //版本号
            txtGC_VersionNo.Value = DetailDS.GC_VersionNo;
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
            if (!string.IsNullOrEmpty(txtGC_ID.Text.Trim()))
            {
                //验证普通客户是否已被使用
                StringBuilder customerIDs = new StringBuilder();
                customerIDs.Append(SysConst.Semicolon_DBC + txtGC_ID.Text + SysConst.Semicolon_DBC);
                //查询普通客户是否被引用过
                List<MDLPIS_GeneralCustomer> generalCustomerList = new List<MDLPIS_GeneralCustomer>();
                _bll.QueryForList(SQLID.PIS_GeneralCustomerManager_SQL02, new MDLPIS_GeneralCustomer
                {
                    WHERE_GC_ID = customerIDs.ToString()
                }, generalCustomerList);
                if (generalCustomerList.Count > 0)
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0007, new object[] { SystemTableEnums.Name.PIS_GeneralCustomer, MsgParam.APPLY, SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

            }
            //验证姓名
            if (string.IsNullOrEmpty(txtGC_Name.Text))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.PIS_GeneralCustomer.Name.GC_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGC_Name.Focus();
                return false;
            }
            //验证手机号码
            if (string.IsNullOrEmpty(txtGC_PhoneNo.Text))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.PIS_GeneralCustomer.Name.GC_PhoneNo }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGC_PhoneNo.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            DetailDS = new GeneralCustomerManagerUIModel()
            {
                //组织ID
                GC_Org_ID = txtGC_Org_ID.Text,
                //姓名
                GC_Name = txtGC_Name.Text.Trim(),
                //手机号码
                GC_PhoneNo = txtGC_PhoneNo.Text.Trim(),
                //地址
                GC_Address = txtGC_Address.Text.Trim(),
                //信用额度
                GC_CreditAmount = Convert.ToDecimal(numGC_CreditAmount.Value ?? 0),
                //默认支付类型名称
                GC_PaymentTypeName = cbGC_PaymentTypeName.Text,
                //默认支付类型编码
                GC_PaymentTypeCode = cbGC_PaymentTypeName.Value?.ToString(),
                //默认开票类型名称
                GC_BillingTypeName = cbGC_BillingTypeName.Text,
                //默认开票类型编码
                GC_BillingTypeCode = cbGC_BillingTypeName.Value?.ToString(),
                //默认物流人员类型名称
                GC_DeliveryTypeName = cbGC_DeliveryTypeName.Text,
                //默认物流人员类型编码
                GC_DeliveryTypeCode = cbGC_DeliveryTypeName.Value?.ToString(),
                //默认物流人员ID
                GC_DeliveryByID = txtGC_DeliveryByID.Text,
                //默认物流人员名称
                GC_DeliveryByName = txtGC_DeliveryByName.Text,
                //默认物流人员手机号
                GC_DeliveryByPhoneNo = txtGC_DeliveryByPhoneNo.Text,
                //是否终止销售
                GC_IsEndSales = ckGC_IsEndSales.Checked,
                //配件价格类别
                GC_AutoPartsPriceType = mcbGC_AutoPartsPriceType.SelectedValue,
                //备注
                GC_Remark = txtGC_Remark.Text.Trim(),
                //有效
                GC_IsValid = ckGC_IsValid.Checked,
                //创建人
                GC_CreatedBy = txtGC_CreatedBy.Text.Trim(),
                //创建时间
                GC_CreatedTime = (DateTime?)dtGC_CreatedTime.Value ?? DateTime.Now,
                //修改人
                GC_UpdatedBy = txtGC_UpdatedBy.Text.Trim(),
                //修改时间
                GC_UpdatedTime = (DateTime?)dtGC_UpdatedTime.Value ?? DateTime.Now,
                //普通客户ID
                GC_ID = txtGC_ID.Text.Trim(),
                //版本号
                GC_VersionNo = Convert.ToInt64(txtGC_VersionNo.Text.Trim() == "" ? "1" : txtGC_VersionNo.Text.Trim()),
            };
        }

        /// <summary>
        /// 设置详情页面控件以及导航按钮是否可编辑
        /// </summary>
        private void SetDetailControl()
        {
            if (string.IsNullOrEmpty(txtGC_ID.Text))
            {
                //未保存的场合，[开户]按钮不可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.CREATEACCOUNT, true, false);
            }
            else
            {
                //已保存的场合，[开户]按钮可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.CREATEACCOUNT, true, true);
            }
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
                    var curHead = GridDS.FirstOrDefault(x => x.GC_ID == DetailDS.GC_ID);
                    if (curHead != null)
                    {
                        GridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = GridDS.FirstOrDefault(x => x.GC_ID == DetailDS.GC_ID);
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
