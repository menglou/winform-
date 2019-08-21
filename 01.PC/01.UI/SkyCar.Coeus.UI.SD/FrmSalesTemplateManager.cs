using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.SD;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.Common.UIModel;
using SkyCar.Coeus.UIModel.SD;
using SkyCar.Coeus.UIModel.SD.UIModel;

namespace SkyCar.Coeus.UI.SD
{
    /// <summary>
    /// 主动销售模板管理
    /// </summary>
    public partial class FrmSalesTemplateManager : BaseFormCardListDetail<SalesTemplateManagerUIModel, SalesTemplateManagerQCModel, MDLSD_SalesTemplate>
    {
        #region 全局变量

        /// <summary>
        /// 主动销售模板管理BLL
        /// </summary>
        private SalesTemplateManagerBLL _bll = new SalesTemplateManagerBLL();

        /// <summary>
        /// 销售模板明细数据源
        /// </summary>
        private SkyCarBindingList<SalesTemplateDetailUIModel, MDLSD_SalesTemplateDetail> _detailGridDS =
            new SkyCarBindingList<SalesTemplateDetailUIModel, MDLSD_SalesTemplateDetail>();

        /// <summary>
        /// 销售模板下发数据源
        /// </summary>
        private SkyCarBindingList<DistributePathUIModel, MDLSD_DistributePath> _distributePathList =
            new SkyCarBindingList<DistributePathUIModel, MDLSD_DistributePath>();

        /// <summary>
        /// 是否触发选中TreeView后的AfterCheck事件
        /// </summary>
        private bool _isCanAfterCheck = true;

        /// <summary>
        /// 汽修商户编码
        /// </summary>
        private string _arMerchantCode;

        #region 下拉框数据源

        /// <summary>
        /// 汽修商户
        /// </summary>
        List<MDLPIS_AutoFactoryCustomer> _autoFactoryList = new List<MDLPIS_AutoFactoryCustomer>();

        /// <summary>
        /// 汽修商户组织
        /// </summary>
        List<CustomerQueryUIModel> _autoFactoryOrgList = new List<CustomerQueryUIModel>();

        /// <summary>
        /// 审核状态列表
        /// </summary>
        List<ComComboBoxDataSourceTC> _approveStatusList = new List<ComComboBoxDataSourceTC>();

        #endregion
        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmSalesTemplateManager构造方法
        /// </summary>
        public FrmSalesTemplateManager()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSalesTemplateManager_Load(object sender, EventArgs e)
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

            //[列表]页不允许删除
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
            }

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
                //[列表]页不允许删除
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
            }
            else
            {
                //设置动作按钮状态
                SetActionEnableByStatus();
            }
        }
        #endregion

        #region 查询条件相关事件

        /// <summary>
        /// 销售模板名称KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_SasT_Name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //执行查询
                QueryAction();
            }
        }

        /// <summary>
        /// 审核状态ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_SasT_ApprovalStatusCode_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }

        /// <summary>
        /// 有效CheckedChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckWhere_SasT_IsValid_CheckedChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }

        /// <summary>
        /// 【列表】汽修商户改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbWhereSasT_AutoFactoryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(mcbWhereSasT_AutoFactoryName.SelectedText)
                || string.IsNullOrEmpty(mcbWhereSasT_AutoFactoryName.SelectedValue))
            {
                mcbWhere_SasT_CustomerName.DataSource = _autoFactoryOrgList;
            }
            else
            {
                var tempAutoFactoryOrgList =
                    _autoFactoryOrgList.Where(x => x.AutoFactoryCode == mcbWhereSasT_AutoFactoryName.SelectedValue).ToList();
                mcbWhere_SasT_CustomerName.DataSource = tempAutoFactoryOrgList;
            }
        }
        #endregion

        #region 单头相关事件

        private string _latestAutoFactoryCode = string.Empty;
        /// <summary>
        /// 【详情】汽修商户改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbSasT_AutoFactoryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSasT_CustomerID.Clear();
            mcbSasT_CustomerName.Clear();
            txtAROrgID.Clear();
            txtSasT_AutoFactoryOrgCode.Clear();

            if (string.IsNullOrEmpty(mcbSasT_AutoFactoryName.SelectedText)
                || string.IsNullOrEmpty(mcbSasT_AutoFactoryName.SelectedValue))
            {
                mcbSasT_CustomerName.DataSource = null;
                return;
            }

            #region 获取汽修商户信息

            if (mcbSasT_AutoFactoryName.SelectedValue != _latestAutoFactoryCode)
            {
                //获取汽修商户相关信息
                bool getARMerchantInfoResult = GetARMerchantInfo(mcbSasT_AutoFactoryName.SelectedValue);
                if (!getARMerchantInfoResult)
                {
                    return;
                }
            }
            _latestAutoFactoryCode = mcbSasT_AutoFactoryName.SelectedValue;

            #endregion

            var tempAutoFactoryOrgList =
                _autoFactoryOrgList.Where(x => x.AutoFactoryCode == mcbSasT_AutoFactoryName.SelectedValue).ToList();
            mcbSasT_CustomerName.DataSource = tempAutoFactoryOrgList;
        }

        /// <summary>
        /// 【详情】汽修商组织改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbSasT_CustomerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSasT_CustomerID.Clear();
            txtAROrgID.Clear();
            txtSasT_AutoFactoryOrgCode.Clear();

            if (string.IsNullOrEmpty(mcbSasT_CustomerName.SelectedText)
                || string.IsNullOrEmpty(mcbSasT_CustomerName.SelectedValue))
            {
                return;
            }

            string[] autoFactoryOrgList = mcbSasT_CustomerName.SelectedValue.Split(';');
            //汽修商客户ID
            txtSasT_CustomerID.Text = autoFactoryOrgList[0];
            //汽修商户组织ID
            txtAROrgID.Text = autoFactoryOrgList[1];
            //汽修商户组织编码
            txtSasT_AutoFactoryOrgCode.Text = autoFactoryOrgList[2];
        }

        /// <summary>
        /// 【详情】Tab内Grid动作按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolBarDetailManager_ToolClick(object sender, ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                //添加
                case SysConst.EN_ADD:
                    AddSalesTmpDetail();
                    break;
                //删除
                case SysConst.EN_DEL:
                    DeleteSalesTmpDetail();
                    break;
            }
        }
        #endregion

        #region 明细相关事件

        /// <summary>
        /// 单元格失去焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_CellChange(object sender, CellEventArgs e)
        {
            #region Cell为[价格是否含税]

            if (e.Cell.Column.Key == SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_PriceIsIncludeTax)
            {
                //勾选含税的场合，税率可编辑
                if (e.Cell.Text == SysConst.True)
                {
                    gdDetail.Rows[e.Cell.Row.Index].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_TaxRate].Activation = Activation.AllowEdit;
                }
                else
                {
                    //不勾选含税的场合，税率不可编辑
                    gdDetail.Rows[e.Cell.Row.Index].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_TaxRate].Value = Convert.ToDecimal(0);
                    gdDetail.Rows[e.Cell.Row.Index].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_TaxRate].Activation = Activation.ActivateOnly;
                    gdDetail.Rows[e.Cell.Row.Index].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_TotalTax].Value = Convert.ToDecimal(0);
                }
            }

            #endregion

            #region Cell为[单价/数量/税率]

            if (e.Cell.Column.Key == SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_UnitPrice
                || e.Cell.Column.Key == SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_Qty
                || e.Cell.Column.Key == SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_TaxRate)
            {
                gdDetail.UpdateData();
                if (BLLCom.IsDecimal(gdDetail.Rows[e.Cell.Row.Index].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_UnitPrice].Value?.ToString())
                    && BLLCom.IsDecimal(gdDetail.Rows[e.Cell.Row.Index].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_Qty].Value?.ToString()))
                {
                    //计算总金额 总金额 = 单价 * 数量 
                    gdDetail.Rows[e.Cell.Row.Index].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_TotalAmount].Value = Math.Round(Convert.ToDecimal(gdDetail.Rows[e.Cell.Row.Index].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_UnitPrice].Value?.ToString()) * Convert.ToDecimal(gdDetail.Rows[e.Cell.Row.Index].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_Qty].Value?.ToString()), 2);

                    if (gdDetail.Rows[e.Cell.Row.Index].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_PriceIsIncludeTax].Text == SysConst.True
                        || BLLCom.IsDecimal(gdDetail.Rows[e.Cell.Row.Index].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_TaxRate].Value?.ToString()))
                    {
                        //勾选[价格是否含税]的场合， 计算税额： 税额 = 单价 * 数量 * 税率 
                        gdDetail.Rows[e.Cell.Row.Index].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_TotalTax].Value = Math.Round(
                            Convert.ToDecimal(gdDetail.Rows[e.Cell.Row.Index].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_UnitPrice].Value?.ToString()) *
                            Convert.ToDecimal(gdDetail.Rows[e.Cell.Row.Index].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_Qty].Value?.ToString()) *
                            Convert.ToDecimal(gdDetail.Rows[e.Cell.Row.Index].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_TaxRate].Value?.ToString()), 2);
                    }
                }
            }

            #endregion

            gdDetail.UpdateData();
        }
        #endregion

        #region 下发路径相关事件

        /// <summary>
        /// 选中组织节点【只有一级】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvMenu_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (!_isCanAfterCheck)
            {
                return;
            }
            TreeNode checkNode = e.Node;
            if (tvMenu.Nodes.Contains(checkNode))
            {
                if (checkNode.Checked)
                {
                    //添加
                    _distributePathList.Add(new DistributePathUIModel()
                    {
                        DP_Org_ID_From = LoginInfoDAX.OrgID,
                        DP_Org_ID_To = checkNode.Tag.ToString(),
                        DP_SendPerson = LoginInfoDAX.UserID,
                        DP_SendDataID = txtSasT_ID.Text.Trim(),
                        DP_SendDataTypeCode = SendDataTypeEnum.Code.XSMB,
                        DP_SendDataTypeName = SendDataTypeEnum.Name.XSMB,
                        DP_IsValid = true,
                        DP_CreatedBy = LoginInfoDAX.UserName,
                        DP_UpdatedBy = LoginInfoDAX.UserName,
                        DP_VersionNo = 1
                    });
                }
                else
                {
                    //删除
                    #region 验证

                    //目的组织下的[销售模板].[审核状态]为[已审核]，则不能取消下发
                    //查询目的组织下的[销售模板]
                    var resultSalesTemplate = _bll.QueryForObject<MDLSD_SalesTemplate>(SQLID.SD_SalesTemplate_SQL03, new SalesTemplateManagerQCModel
                    {
                        WHERE_SasT_Name = txtSasT_Name.Text.Trim(),
                        WHERE_DP_SendDataID = txtSasT_ID.Text.Trim(),
                        WHERE_DP_Org_ID_From = LoginInfoDAX.OrgID,
                        WHERE_DP_Org_ID_To = checkNode.Tag.ToString()
                    });
                    if (resultSalesTemplate != null
                        && !string.IsNullOrEmpty(resultSalesTemplate.SasT_ID)
                        && resultSalesTemplate.SasT_ApprovalStatusName == ApprovalStatusEnum.Name.YSH)
                    {
                        //目的组织中对应的销售模板已审核，不能取消下发
                        MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0023, new object[] { MsgParam.PURPOSEORGNIZATION + MsgParam.IN, SystemTableEnums.Name.SD_SalesTemplate, SystemActionEnum.Name.CANCELDELIVER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    #endregion

                    List<DistributePathUIModel> deleteDistributePathList = _distributePathList.Where(p => p.DP_Org_ID_To == checkNode.Tag.ToString() && p.DP_Org_ID_From == LoginInfoDAX.OrgID).ToList();
                    if (deleteDistributePathList.Count > 0)
                    {
                        _distributePathList.Remove(deleteDistributePathList[0]);
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
            base.NewUIModel = HeadDS;
            if (ViewHasChanged()
                || _detailGridDS.InsertList.Count > 0
                || _detailGridDS.UpdateList.Count > 0
                || _detailGridDS.DeleteList.Count > 0)
            {
                //信息尚未保存，确定进行当前操作？
                DialogResult dialogResult = MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.W_0001), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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

            //设置动作按钮状态
            SetActionEnableByStatus();
            //控制详情页面是否可编辑
            SetDetailControl();
            //设置单元格是否可以编辑
            SetSalesOrderDetailStyle();
            
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
            if (!_bll.SaveDetailDs(base.HeadDS, _detailGridDS, _distributePathList))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //保存成功
            MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //4.开始监控明细List变化
            _detailGridDS.StartMonitChanges();
            //开始监控下发路径数据源
            _distributePathList.StartMonitChanges();
            //4.将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置动作按钮状态
            SetActionEnableByStatus();
            //控制详情页面是否可编辑
            SetDetailControl();
            //设置单元格是否可以编辑
            SetSalesOrderDetailStyle();
        }

        /// <summary>
        /// 复制
        /// </summary>
        public override void CopyAction()
        {
            #region 检查详情是否已保存

            SetCardCtrlsToDetailDS();
            base.NewUIModel = HeadDS;
            if (ViewHasChanged()
                || _detailGridDS.InsertList.Count > 0
                || _detailGridDS.UpdateList.Count > 0
                || _detailGridDS.DeleteList.Count > 0)
            {
                //信息尚未保存，确定进行当前操作？
                DialogResult dialogResult = MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.W_0001), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
            }
            #endregion

            base.CopyAction();
            //ID
            txtSasT_ID.Clear();
            //销售模板名称
            txtSasT_Name.Clear();
            //审核状态
            cbSasT_ApprovalStatusCode.Text = ApprovalStatusEnum.Name.DSH;
            cbSasT_ApprovalStatusCode.Value = ApprovalStatusEnum.Code.DSH;
            //版本号
            txtSasT_VersionNo.Clear();

            SkyCarBindingList<SalesTemplateDetailUIModel, MDLSD_SalesTemplateDetail> detailGridDS =
            new SkyCarBindingList<SalesTemplateDetailUIModel, MDLSD_SalesTemplateDetail>();
            _bll.CopyModelList(_detailGridDS, detailGridDS);
            _detailGridDS = new SkyCarBindingList<SalesTemplateDetailUIModel, MDLSD_SalesTemplateDetail>();
            //开始监控明细List变化
            _detailGridDS.StartMonitChanges();
            _bll.CopyModelList(detailGridDS, _detailGridDS);
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            
            //设置动作按钮状态
            SetActionEnableByStatus();
            //控制详情页面是否可编辑
            SetDetailControl();
            //设置单元格是否可以编辑
            SetSalesOrderDetailStyle();

            //将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public override void DeleteAction()
        {
            //1.前端检查-删除
            if (!ClientCheckForDelete())
            {
                return;
            }
            var argsDetail = new List<MDLSD_SalesTemplateDetail>();
            var argsDistributePath = new List<MDLSD_DistributePath>();

            //将HeadDS转换为TBModel对象
            var argsHead = base.HeadDS.ToTBModelForSaveAndDelete<MDLSD_SalesTemplate>();
            //将当前DetailGridDS转换为指定类型的TBModelList
            _detailGridDS.ToTBModelListForUpdateAndDelete<MDLSD_SalesTemplateDetail>(argsDetail);
            //过滤明细列表中未保存的数据
            argsDetail = argsDetail.Where(x => !string.IsNullOrEmpty(x.WHERE_SasTD_ID)).ToList();
            //将当前下发路径转换为指定类型的TBModelList
            _distributePathList.ToTBModelListForUpdateAndDelete<MDLSD_DistributePath>(argsDistributePath);
            //过滤下发路径列表中未保存的数据
            argsDistributePath = argsDistributePath.Where(x => !string.IsNullOrEmpty(x.WHERE_DP_ID)).ToList();

            //2.执行删除
            var deleteSalesTmpResult = _bll.DeleteSalesTemplateInfo(argsHead, argsDetail, argsDistributePath);
            if (!deleteSalesTmpResult)
            {
                //删除失败
                MessageBoxs.Show(Trans.SD, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //删除成功
            MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //3.清空【详情】画面数据
            InitializeDetailTabControls();
            //刷新列表
            RefreshList(true);

            //将DetailDS数据赋值给【详情】Tab内的对应控件
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
            base.ConditionDS = new SalesTemplateManagerQCModel()
            {
                //SqlId
                SqlId = SQLID.SD_SalesTemplate_SQL02,
                //销售模板名称
                WHERE_SasT_Name = txtWhere_SasT_Name.Text.Trim(),
                //汽修商户名称
                WHERE_SasT_AutoFactoryName = mcbWhereSasT_AutoFactoryName.SelectedText,
                //汽修商户组织
                WHERE_SasT_CustomerName = mcbWhere_SasT_CustomerName.SelectedText,
                //审核状态名称
                WHERE_SasT_ApprovalStatusName = cbWhere_SasT_ApprovalStatusCode.Text.Trim(),
                //有效
                WHERE_SasT_IsValid = ckWhere_SasT_IsValid.Checked,
                //组织ID
                WHERE_SasT_Org_ID = LoginInfoDAX.OrgID,
            };
            //3.执行基类方法（注意：基类使用模糊查询）
            base.QueryAction();
            //4.Grid绑定数据源
            gdGrid.DataSource = base.HeadGridDS;
            gdGrid.DataBind();
            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            //5.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
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
            base.NewUIModel = HeadDS;
            if (ViewHasChanged()
                || _detailGridDS.InsertList.Count > 0
                || _detailGridDS.UpdateList.Count > 0
                || _detailGridDS.DeleteList.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 审核
        /// </summary>
        public override void ApproveAction()
        {
            #region 验证

            //销售模板未保存
            if (string.IsNullOrEmpty(txtSasT_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.SD_SalesTemplate + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //不能审核其他组织创建的销售模板
            if (txtSasT_Org_ID.Text != LoginInfoDAX.OrgID)
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0015, new object[]
                {
                    SystemActionEnum.Name.APPROVE, SystemTableEnums.Name.SD_SalesTemplate
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MDLSD_SalesTemplate resulSalesTemplate = new MDLSD_SalesTemplate();
            _bll.QueryForObject<MDLSD_SalesTemplate, MDLSD_SalesTemplate>(new MDLSD_SalesTemplate()
            {
                WHERE_SasT_IsValid = true,
                WHERE_SasT_ID = txtSasT_ID.Text.Trim()
            }, resulSalesTemplate);
            //销售模板不存在
            if (string.IsNullOrEmpty(resulSalesTemplate.SasT_ID))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.SD_SalesTemplate + MsgParam.NOTEXIST, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #region 判断该汽修商户该组织是否存在[已审核]的销售模板

            int approvalCount = _bll.QueryForObject<int>(SQLID.SD_SalesTemplate_SQL05, new SalesTemplateManagerQCModel
            {
                WHERE_SasT_Org_ID = txtSasT_Org_ID.Text,
                WHERE_SasT_CustomerID = resulSalesTemplate.SasT_CustomerID,
                WHERE_SasT_ApprovalStatusName = ApprovalStatusEnum.Name.YSH,
                WHERE_AROrgID = txtAROrgID.Text,
            });
            if (approvalCount > 0)
            {
                //txtSasT_CustomerName.Text在txtOrg_ShortName.Text中已存在[已审核]的销售模板，不能再审核
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { (!string.IsNullOrEmpty(mcbSasT_CustomerName.SelectedText) ? mcbSasT_CustomerName.SelectedText : mcbSasT_AutoFactoryName.SelectedText) + MsgParam.AT + LoginInfoDAX.OrgShortName + MsgParam.IN + MsgParam.ALREADYEXIST + ApprovalStatusEnum.Name.YSH + MsgParam.OF + SystemTableEnums.Name.SD_SalesTemplate, SystemActionEnum.Name.APPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #endregion

            //确认审核操作
            DialogResult isApprove = MessageBoxs.Show(Trans.SD, this.ToString(),
                MsgHelp.GetMsg(MsgCode.W_0014),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isApprove != DialogResult.OK)
            {
                return;
            }
            #endregion

            SetCardCtrlsToDetailDS();
            bool saveApprove = _bll.ApproveDetailDS(HeadDS, _detailGridDS);
            if (!saveApprove)
            {
                //审核失败
                MessageBoxs.Show(Trans.SD, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //审核成功
            MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[]
            {
                SystemActionEnum.Name.APPROVE
            }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //开始监控List变化
            _detailGridDS.StartMonitChanges();
            //将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置动作按钮状态
            SetActionEnableByStatus();
            //控制详情页面是否可编辑
            SetDetailControl();
            //设置单元格是否可以编辑
            SetSalesOrderDetailStyle();
        }

        /// <summary>
        /// 反审核
        /// </summary>
        public override void UnApproveAction()
        {
            //销售模板未保存，不能反审核
            if (string.IsNullOrEmpty(txtSasT_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.SD_SalesTemplate + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //不能反审核其他组织创建的销售模板
            if (txtSasT_Org_ID.Text != LoginInfoDAX.OrgID)
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0015, new object[]
                {
                    SystemActionEnum.Name.UNAPPROVE, SystemTableEnums.Name.SD_SalesTemplate
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MDLSD_SalesTemplate resulSalesTemplate = new MDLSD_SalesTemplate();
            _bll.QueryForObject<MDLSD_SalesTemplate, MDLSD_SalesTemplate>(new MDLSD_SalesTemplate()
            {
                WHERE_SasT_IsValid = true,
                WHERE_SasT_ID = txtSasT_ID.Text.Trim()
            }, resulSalesTemplate);
            //销售模板不存在
            if (string.IsNullOrEmpty(resulSalesTemplate.SasT_ID))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.SD_SalesTemplate + MsgParam.NOTEXIST, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //确认反审核操作
            DialogResult isApprove = MessageBoxs.Show(Trans.SD, this.ToString(),
                MsgHelp.GetMsg(MsgCode.W_0018),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isApprove != DialogResult.OK)
            {
                return;
            }

            SetCardCtrlsToDetailDS();
            bool saveUnApprove = _bll.UnApproveDetailDS(HeadDS);
            if (!saveUnApprove)
            {
                //反审核失败
                MessageBoxs.Show(Trans.SD, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //反审核成功
            MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.UNAPPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //开始监控List变化
            _detailGridDS.StartMonitChanges();
            //将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置动作按钮状态
            SetActionEnableByStatus();
            //控制详情页面是否可编辑
            SetDetailControl();
            //设置单元格是否可以编辑
            SetSalesOrderDetailStyle();
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 工具生成
            //销售模板名称
            txtSasT_Name.Clear();
            //汽修商户
            mcbSasT_AutoFactoryName.Clear();
            //汽修商户组织
            mcbSasT_CustomerName.Clear();
            //备注
            txtSasT_Remark.Clear();
            //有效
            ckSasT_IsValid.Checked = true;
            ckSasT_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtSasT_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtSasT_CreatedTime.Value = BLLCom.GetCurStdDatetime();
            //修改人
            txtSasT_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtSasT_UpdatedTime.Value = BLLCom.GetCurStdDatetime();
            //销售模板ID
            txtSasT_ID.Clear();
            //组织ID
            txtSasT_Org_ID.Clear();
            //汽修商户ID
            txtSasT_CustomerID.Clear();
            //审核状态编码
            cbSasT_ApprovalStatusCode.Items.Clear();
            //版本号
            txtSasT_VersionNo.Clear();
            //汽修商户组织ID
            txtAROrgID.Clear();
            //汽修商组织Code
            txtSasT_AutoFactoryOrgCode.Clear();
            //给 销售模板名称 设置焦点
            lblSasT_Name.Focus();

            //初始化[销售模板明细]列表
            _detailGridDS = new SkyCarBindingList<SalesTemplateDetailUIModel, MDLSD_SalesTemplateDetail>();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //开始监控List变化（该数据源若有其他调整，请在StartMonitChanges()方法执行前调整）
            _detailGridDS.StartMonitChanges();
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);

            #endregion

            #region 初始化下拉框

            //汽修商户
            _autoFactoryList = CacheDAX.Get(CacheDAX.ConfigDataKey.ARMerchant) as List<MDLPIS_AutoFactoryCustomer>;
            if (_autoFactoryList != null)
            {
                var autoFactoryCustomerList = _autoFactoryList.Where(x => x.AFC_IsPlatform == true).ToList();
                mcbSasT_AutoFactoryName.DisplayMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Name;
                mcbSasT_AutoFactoryName.ValueMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Code;
                mcbSasT_AutoFactoryName.DataSource = autoFactoryCustomerList;
            }

            //汽修商户组织
            var allCustomerList = CacheDAX.Get(CacheDAX.ConfigDataKey.Customer) as List<CustomerQueryUIModel>;
            if (allCustomerList != null)
            {
                _autoFactoryOrgList = allCustomerList.Where(x => x.CustomerType == CustomerTypeEnum.Name.PTNQXSH).ToList();

                mcbSasT_CustomerName.DisplayMember = "CustomerName";
                mcbSasT_CustomerName.ValueMember = "AutoFactoryOrgInfo";
            }

            //审核状态
            _approveStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.ApprovalStatus);
            cbSasT_ApprovalStatusCode.DisplayMember = SysConst.EN_TEXT;
            cbSasT_ApprovalStatusCode.ValueMember = SysConst.EN_Code;
            cbSasT_ApprovalStatusCode.DataSource = _approveStatusList;
            cbSasT_ApprovalStatusCode.DataBind();

            #endregion

            #region 相关默认设置

            //默认[销售模板名称]和[客户名称]可编辑
            txtSasT_Name.Enabled = true;
            mcbSasT_AutoFactoryName.Enabled = true;

            //默认显示待审核
            cbSasT_ApprovalStatusCode.Text = ApprovalStatusEnum.Name.DSH;
            cbSasT_ApprovalStatusCode.Value = ApprovalStatusEnum.Code.DSH;

            //默认组织为当前组织
            txtSasT_Org_ID.Text = LoginInfoDAX.OrgID;

            #endregion

            //初始化属性控件
            InitializeTreeView();
        }

        /// <summary>
        /// 初始化树形控件
        /// </summary>
        private void InitializeTreeView()
        {
            //初始化下发路径数据源
            _distributePathList = new SkyCarBindingList<DistributePathUIModel, MDLSD_DistributePath>();
            //监视下发路径数据源
            _distributePathList.StartMonitChanges();

            List<MDLSM_Organization> resultOrganizationList = new List<MDLSM_Organization>();
            _bll.QueryForList<MDLSM_Organization>(new MDLSM_Organization()
            {
                WHERE_Org_IsValid = true
            }, resultOrganizationList);

            List<MDLSM_Organization> tempOrganizationList =
                resultOrganizationList.Where(x => x.Org_ID != LoginInfoDAX.OrgID).ToList();

            //清空节点
            tvMenu.Nodes.Clear();
            if (tempOrganizationList.Count > 0)
            {
                foreach (var loopOrganization in tempOrganizationList)
                {
                    TreeNode orgNode = new TreeNode();
                    orgNode.Text = loopOrganization.Org_ShortName;
                    orgNode.Tag = loopOrganization.Org_ID;
                    tvMenu.Nodes.Add(orgNode);
                }
            }
            SetDistributePathInfo();
        }

        /// <summary>
        /// 获取最新的下发路径进行绑定
        /// </summary>
        private void SetDistributePathInfo()
        {
            #region 获取最新的下发路径进行绑定

            if (string.IsNullOrEmpty(txtSasT_ID.Text.Trim()))
            {
                return;
            }
            //获取下发路径信息
            List<MDLSD_DistributePath> resultDistributePathList = new List<MDLSD_DistributePath>();
            _bll.QueryForList<MDLSD_DistributePath, MDLSD_DistributePath>(new MDLSD_DistributePath()
            {
                WHERE_DP_IsValid = true,
                WHERE_DP_SendDataID = txtSasT_ID.Text.Trim(),
                WHERE_DP_Org_ID_From = LoginInfoDAX.OrgID
            }, resultDistributePathList);
            if (resultDistributePathList.Count > 0)
            {
                _isCanAfterCheck = false;
                foreach (TreeNode loopDistributePath in tvMenu.Nodes)
                {
                    bool isSaveUserOrg = resultDistributePathList.Any(p => p.DP_Org_ID_To == loopDistributePath.Tag.ToString()
                                                                    && p.DP_SendDataID == txtSasT_ID.Text.Trim());
                    loopDistributePath.Checked = isSaveUserOrg;
                }
                _isCanAfterCheck = true;

                //暂停下发路径数据源的监控
                _distributePathList.SuspendMonitChanges();
                _bll.CopyModelList(resultDistributePathList, _distributePathList);
                //继续监控下发路径数据源
                _distributePathList.ContinueMonitChanges();
            }
            #endregion
        }

        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 查询条件初始化
            //销售模板名称
            txtWhere_SasT_Name.Clear();
            //汽修商户
            mcbWhereSasT_AutoFactoryName.Clear();
            //汽修商户组织
            mcbWhere_SasT_CustomerName.Clear();
            //有效
            ckWhere_SasT_IsValid.Checked = true;
            ckWhere_SasT_IsValid.CheckState = CheckState.Checked;
            //给 销售模板名称 设置焦点
            lblWhere_SasT_Name.Focus();

            #endregion

            #region Grid初始化

            //清空Grid
            HeadGridDS = new BindingList<SalesTemplateManagerUIModel>();
            gdGrid.DataSource = HeadGridDS;
            gdGrid.DataBind();

            #endregion

            #region 初始化下拉框
            //汽修商户
            if (_autoFactoryList != null)
            {
                var autoFactoryCustomerList = _autoFactoryList.Where(x => x.AFC_IsPlatform == true).ToList();
                mcbWhereSasT_AutoFactoryName.DisplayMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Name;
                mcbWhereSasT_AutoFactoryName.ValueMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Code;
                mcbWhereSasT_AutoFactoryName.DataSource = autoFactoryCustomerList;
            }

            //汽修商户组织
            mcbWhere_SasT_CustomerName.DisplayMember = "CustomerName";
            mcbWhere_SasT_CustomerName.ValueMember = "AutoFactoryOrgInfo";
            mcbWhere_SasT_CustomerName.DataSource = _autoFactoryOrgList;

            //审核状态
            cbWhere_SasT_ApprovalStatusCode.DisplayMember = SysConst.EN_TEXT;
            cbWhere_SasT_ApprovalStatusCode.ValueMember = SysConst.EN_Code;
            cbWhere_SasT_ApprovalStatusCode.DataSource = _approveStatusList;
            cbWhere_SasT_ApprovalStatusCode.DataBind();

            #endregion
        }

        private string _lastDetailAutoFactoryCode = string.Empty;
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
            base.NewUIModel = HeadDS;

            var activeRowIndex = gdGrid.ActiveRow.Index;
            //判断Grid内[唯一标识]是否为空
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SD_SalesTemplate.Code.SasT_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SD_SalesTemplate.Code.SasT_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用HeadGridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            HeadDS = HeadGridDS.FirstOrDefault(x => x.SasT_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SD_SalesTemplate.Code.SasT_ID].Value.ToString());
            if (HeadDS == null || string.IsNullOrEmpty(HeadDS.SasT_ID))
            {
                return;
            }

            if (txtSasT_ID.Text != HeadDS.SasT_ID
                || (txtSasT_ID.Text == HeadDS.SasT_ID && txtSasT_VersionNo.Text != HeadDS.SasT_VersionNo?.ToString()))
            {
                if (txtSasT_ID.Text == HeadDS.SasT_ID && txtSasT_VersionNo.Text != HeadDS.SasT_VersionNo?.ToString())
                {
                    //数据版本已过期，将加载最新详情。
                    MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.DataHasOverdue }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else if (ViewHasChanged()
                || _detailGridDS.InsertList.Count > 0
                || _detailGridDS.UpdateList.Count > 0
                || _detailGridDS.DeleteList.Count > 0)
                {
                    //将放弃之前的修改，是否继续？
                    DialogResult dialogResult = MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.ConfirmGiveUpEdit }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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

            if (!string.IsNullOrEmpty(HeadDS.SasT_AutoFactoryCode))
            {
                if (_lastDetailAutoFactoryCode != HeadDS.SasT_AutoFactoryCode)
                {
                    //获取汽修商户相关信息
                    bool getARMerchantInfoResult = GetARMerchantInfo(HeadDS.SasT_AutoFactoryCode);
                    if (!getARMerchantInfoResult)
                    {
                        return;
                    }
                }

                _lastDetailAutoFactoryCode = HeadDS.SasT_AutoFactoryCode;
            }

            //查询明细Grid数据并绑定
            QueryDetail();

            //设置动作按钮状态
            SetActionEnableByStatus();
            //控制详情页面是否可编辑
            SetDetailControl();
            //下发记录
            InitializeTreeView();
            //设置单元格是否可以编辑
            SetSalesOrderDetailStyle();
        }

        /// <summary>
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //销售模板名称
            txtSasT_Name.Text = HeadDS.SasT_Name;
            //汽修商户
            mcbSasT_AutoFactoryName.SelectedText = HeadDS.SasT_AutoFactoryName;
            //汽修商组织
            mcbSasT_CustomerName.SelectedValue = HeadDS.AutoFactoryOrgInfo;
            //汽修商组织编码
            txtSasT_AutoFactoryOrgCode.Text = HeadDS.SasT_AutoFactoryOrgCode;
            //审核状态编码
            cbSasT_ApprovalStatusCode.Value = HeadDS.SasT_ApprovalStatusCode;
            //审核状态名称
            cbSasT_ApprovalStatusCode.Text = HeadDS.SasT_ApprovalStatusName;
            //备注
            txtSasT_Remark.Text = HeadDS.SasT_Remark;
            //有效
            if (HeadDS.SasT_IsValid != null)
            {
                ckSasT_IsValid.Checked = HeadDS.SasT_IsValid.Value;
            }
            //创建人
            txtSasT_CreatedBy.Text = HeadDS.SasT_CreatedBy;
            //创建时间
            dtSasT_CreatedTime.Value = HeadDS.SasT_CreatedTime;
            //修改人
            txtSasT_UpdatedBy.Text = HeadDS.SasT_UpdatedBy;
            //修改时间
            dtSasT_UpdatedTime.Value = HeadDS.SasT_UpdatedTime;
            //销售模板ID
            txtSasT_ID.Text = HeadDS.SasT_ID;
            //组织ID
            txtSasT_Org_ID.Text = HeadDS.SasT_Org_ID;
            //版本号
            txtSasT_VersionNo.Text = HeadDS.SasT_VersionNo.ToString();

            //汽修商户组织ID
            txtAROrgID.Text = HeadDS.AROrgID;
        }

        /// <summary>
        /// 查询明细Grid数据并绑定
        /// </summary>
        private void QueryDetail()
        {
            //销售模板明细列表
            _bll.QueryForList(new MDLSD_SalesTemplateDetail
            {
                WHERE_SasTD_SasT_ID = txtSasT_ID.Text.Trim(),
                WHERE_SasTD_IsValid = true,
            }, _detailGridDS);

            //3.开始监控List变化（该数据源若有其他调整，请在StartMonitChanges()方法执行前调整）
            _detailGridDS.StartMonitChanges();

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();

            //设置销售订单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
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
            //不能保存其他组织创建的销售模板
            if (txtSasT_Org_ID.Text != LoginInfoDAX.OrgID)
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0015, new object[]
                {
                    SystemActionEnum.Name.SAVE, SystemTableEnums.Name.SD_SalesTemplate
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //验证[销售模板名称]
            if (string.IsNullOrEmpty(txtSasT_Name.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, SystemTableColumnEnums.SD_SalesTemplate.Name.SasT_Name), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSasT_Name.Focus();
                return false;
            }

            //验证[汽修商户]
            if (string.IsNullOrEmpty(mcbSasT_AutoFactoryName.SelectedText)
                || string.IsNullOrEmpty(mcbSasT_AutoFactoryName.SelectedValue))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.AUTOFACTORY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                mcbSasT_AutoFactoryName.Focus();
                return false;
            }

            //验证[汽修商户组织]
            if (string.IsNullOrEmpty(txtSasT_CustomerID.Text.Trim())
                || string.IsNullOrEmpty(mcbSasT_CustomerName.SelectedText)
                || string.IsNullOrEmpty(txtAROrgID.Text)
                || string.IsNullOrEmpty(txtSasT_AutoFactoryOrgCode.Text))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.AUTOFACTORY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                mcbSasT_CustomerName.Focus();
                return false;
            }

            #region 验证销售模板明细的信息

            if (gdDetail.Rows.Count == 0)
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0014, new object[] { SystemTableEnums.Name.SD_SalesTemplateDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            for (int i = 0; i < gdDetail.Rows.Count; i++)
            {
                //验证数量
                if (gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_Qty].Value == null
                    || gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_Qty].Value.ToString() == "0")
                {
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, SystemTableColumnEnums.SD_SalesTemplateDetail.Name.SasTD_Qty, MsgParam.POSITIVE_INTEGER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (!BLLCom.IsDecimal(gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_Qty].Value.ToString()))
                {
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, SystemTableColumnEnums.SD_SalesTemplateDetail.Name.SasTD_Qty, MsgParam.POSITIVE_INTEGER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //验证单价
                if (gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_UnitPrice].Value == null)
                {
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, SystemTableColumnEnums.SD_SalesTemplateDetail.Name.SasTD_UnitPrice, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (!BLLCom.IsDecimal(gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_UnitPrice].Value?.ToString()))
                {
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, SystemTableColumnEnums.SD_SalesTemplateDetail.Name.SasTD_UnitPrice, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //如果价格含税
                if (gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_PriceIsIncludeTax].Text == SysConst.True)
                {
                    //验证税率
                    if (!BLLCom.IsDecimal(gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_TaxRate].Value?.ToString()))
                    {
                        MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, SystemTableColumnEnums.SD_SalesTemplateDetail.Name.SasTD_TaxRate, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }

            #endregion

            return true;
        }
       
        /// <summary>
        /// 前端检查-删除
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForDelete()
        {
            //确认删除操作
            DialogResult dialogResult = MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0012), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.OK)
            {
                return false;
            }
            return true;
        }
      
        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            HeadDS = new SalesTemplateManagerUIModel()
            {
                //销售模板名称
                SasT_Name = txtSasT_Name.Text.Trim(),
                //汽修商户名称
                SasT_AutoFactoryName = mcbSasT_AutoFactoryName.SelectedText,
                //汽修商户编码
                SasT_AutoFactoryCode = mcbSasT_AutoFactoryName.SelectedValue,
                //汽修商户名称
                SasT_CustomerName = mcbSasT_CustomerName.SelectedText,
                AutoFactoryOrgInfo = mcbSasT_CustomerName.SelectedValue,
                //汽修商户组织编码
                SasT_AutoFactoryOrgCode = txtSasT_AutoFactoryOrgCode.Text.Trim(),
                //审核状态编码
                SasT_ApprovalStatusCode = cbSasT_ApprovalStatusCode.Value?.ToString() ?? "",
                //审核状态名称
                SasT_ApprovalStatusName = cbSasT_ApprovalStatusCode.Text.Trim(),
                //备注
                SasT_Remark = txtSasT_Remark.Text.Trim(),
                //有效
                SasT_IsValid = ckSasT_IsValid.Checked,
                //创建人
                SasT_CreatedBy = txtSasT_CreatedBy.Text.Trim(),
                //创建时间
                SasT_CreatedTime = (DateTime?)dtSasT_CreatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //修改人
                SasT_UpdatedBy = txtSasT_UpdatedBy.Text.Trim(),
                //修改时间
                SasT_UpdatedTime = (DateTime?)dtSasT_UpdatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //销售模板ID
                SasT_ID = txtSasT_ID.Text.Trim(),
                //组织ID
                SasT_Org_ID = txtSasT_Org_ID.Text.Trim(),
                //汽修商户ID
                SasT_CustomerID = txtSasT_CustomerID.Text.Trim(),
                //版本号
                SasT_VersionNo = Convert.ToInt64(txtSasT_VersionNo.Text.Trim() == "" ? "1" : txtSasT_VersionNo.Text.Trim()),

                //汽修商户组织ID
                AROrgID = txtAROrgID.Text,
            };
        }

        /// <summary>
        /// 设置动作按钮状态
        /// </summary>
        private void SetActionEnableByStatus()
        {
            if (cbSasT_ApprovalStatusCode.Text == ApprovalStatusEnum.Name.YSH)
            {
                #region [审核状态]为[已审核]的场合

                //[保存]、[删除]、[审核]、[复制]不可用
                SetActionEnable(SystemActionEnum.Code.SAVE, false);
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                SetActionEnable(SystemActionEnum.Code.APPROVE, false);
                SetActionEnable(SystemActionEnum.Code.COPY, false);
                SetActionEnable(SystemActionEnum.Code.UNAPPROVE, true);

                #endregion
            }
            else
            {
                #region 新增或[审核状态]为[待审核]的场合

                //[反审核]不可用
                SetActionEnable(SystemActionEnum.Code.SAVE, true);
                SetActionEnable(SystemActionEnum.Code.DELETE, !string.IsNullOrEmpty(txtSasT_ID.Text));
                SetActionEnable(SystemActionEnum.Code.APPROVE, !string.IsNullOrEmpty(txtSasT_ID.Text));
                SetActionEnable(SystemActionEnum.Code.COPY, !string.IsNullOrEmpty(txtSasT_ID.Text));
                SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);

                #endregion
            }
        }

        /// <summary>
        /// 设置详情页面控件是否可编辑
        /// </summary>
        private void SetDetailControl()
        {
            if (cbSasT_ApprovalStatusCode.Text == ApprovalStatusEnum.Name.YSH)
            {
                #region 销售模板.[审核状态]为[已审核] 的场合，详情不可编辑

                //单头不可编辑
                txtSasT_Name.Enabled = false;
                mcbSasT_AutoFactoryName.Enabled = false;
                mcbSasT_CustomerName.Enabled = false;
                txtSasT_Remark.Enabled = false;

                //明细列表不可添加、删除、更新
                toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedProps.Enabled = false;
                toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = false;
                
                //不可下发
                tvMenu.Enabled = false;

                #endregion
            }
            else
            {
                #region 销售模板未保存 或 销售模板.[审核状态]为[待审核]的场合，详情可编辑

                //单头可编辑
                txtSasT_Name.Enabled = true;
                txtSasT_Remark.Enabled = true;

                //根据[是否存在明细]控制单头是否可编辑
                SetDetailByIsExistDetail();

                //明细列表可添加、删除、更新
                toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedProps.Enabled = true;
                toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = true;

                //只可下发[待审核]的[销售模板]
                tvMenu.Enabled = true;

                #endregion
            }
        }

        /// <summary>
        /// 根据[是否存在明细]控制单头是否可编辑
        /// </summary>
        private void SetDetailByIsExistDetail()
        {
            if (_detailGridDS.Count == 0)
            {
                mcbSasT_AutoFactoryName.Enabled = true;
                mcbSasT_CustomerName.Enabled = true;
            }
            else
            {
                mcbSasT_AutoFactoryName.Enabled = false;
                mcbSasT_CustomerName.Enabled = false;
            }
        }

        /// <summary>
        /// 新增销售模板明细【配件条码和配件名称要具有唯一性】
        /// </summary>
        private void AddSalesTmpDetail()
        {
            //验证平台内的商户
            if (string.IsNullOrEmpty(_arMerchantCode))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.AUTOFACTORY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //验证客户名称
            if (string.IsNullOrEmpty(mcbSasT_AutoFactoryName.SelectedText)
                || string.IsNullOrEmpty(txtAROrgID.Text)
                || string.IsNullOrEmpty(txtSasT_AutoFactoryOrgCode.Text))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { MsgParam.AUTOFACTORY_ORG }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //查询配件档案
            FrmAutoPartsArchiveQuery frmAutoPartsArchiveQuery = new FrmAutoPartsArchiveQuery(CustomEnums.CustomeSelectionMode.Multiple)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            DialogResult dialogResult = frmAutoPartsArchiveQuery.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            //选择的配件列表
            List<MDLBS_AutoPartsArchive> selectedAutoPartsArchiveList = frmAutoPartsArchiveQuery.SelectedGridList;
            if (selectedAutoPartsArchiveList == null || selectedAutoPartsArchiveList.Count == 0)
            {
                return;
            }
            List<SalesTemplateDetailUIModel> selectSalesTemplateDetailList = new List<SalesTemplateDetailUIModel>();
            foreach (var loopAutoPartsArchive in selectedAutoPartsArchiveList)
            {
                //条码相同为同一个配件（已保存的明细不与其他配件合并）
                SalesTemplateDetailUIModel sameSalesTemplateDetail = _detailGridDS.FirstOrDefault(x => x.SasTD_Barcode == loopAutoPartsArchive.APA_Barcode && string.IsNullOrEmpty(x.SasTD_ID));
                if (sameSalesTemplateDetail == null)
                {
                    SalesTemplateDetailUIModel tempSalesTemplateDetail = new SalesTemplateDetailUIModel
                    {
                        SasTD_Barcode = loopAutoPartsArchive.APA_Barcode,
                        SasTD_Name = loopAutoPartsArchive.APA_Name,
                        SasTD_Specification = loopAutoPartsArchive.APA_Specification,
                        SasTD_UOM = loopAutoPartsArchive.APA_UOM,
                        //默认数量为1
                        SasTD_Qty = 1,
                    };
                    selectSalesTemplateDetailList.Add(tempSalesTemplateDetail);
                }
                else
                {
                    sameSalesTemplateDetail.SasTD_Qty = sameSalesTemplateDetail.SasTD_Qty + 1;
                    sameSalesTemplateDetail.SasTD_TotalAmount = Math.Round((sameSalesTemplateDetail.SasTD_Qty ?? 0) *
                                                           (sameSalesTemplateDetail.SasTD_UnitPrice ?? 0), 2);
                }
            }

            //新增的数量
            int insertCount = 0;
            //判断新增的配件信息已经在销售模板明细中存在
            foreach (var loopSalesTemplateDetail in selectSalesTemplateDetailList)
            {
                if (loopSalesTemplateDetail.SasTD_UnitPrice == null)
                {
                    //获取单价
                    loopSalesTemplateDetail.SasTD_UnitPrice = BLLCom.GetAutoPartUnitPrice(loopSalesTemplateDetail.SasTD_Barcode, txtSasT_CustomerID.Text.Trim());
                }

                if (loopSalesTemplateDetail.SasTD_TotalAmount == null)
                {
                    //计算金额
                    loopSalesTemplateDetail.SasTD_TotalAmount = Math.Round((loopSalesTemplateDetail.SasTD_UnitPrice ?? 0) *
                                                           (loopSalesTemplateDetail.SasTD_Qty ?? 0), 2);
                }
                loopSalesTemplateDetail.SasTD_AutoFactoryOrgID = txtAROrgID.Text;
                loopSalesTemplateDetail.SasTD_AutoFactoryOrgName = mcbSasT_CustomerName.SelectedText;
                loopSalesTemplateDetail.SasTD_IsValid = true;
                loopSalesTemplateDetail.SasTD_CreatedBy = LoginInfoDAX.UserName;
                loopSalesTemplateDetail.SasTD_CreatedTime = BLLCom.GetCurStdDatetime();
                loopSalesTemplateDetail.SasTD_UpdatedBy = LoginInfoDAX.UserName;
                loopSalesTemplateDetail.SasTD_UpdatedTime = BLLCom.GetCurStdDatetime();

                SalesTemplateDetailUIModel salesTemplateDetail = new SalesTemplateDetailUIModel();
                _bll.CopyModel(loopSalesTemplateDetail, salesTemplateDetail);
                _detailGridDS.Add(salesTemplateDetail);

                gdDetail.DataSource = _detailGridDS;
                gdDetail.DataBind();

                insertCount++;
            }

            //根据[是否存在明细]控制单头是否可编辑
            SetDetailByIsExistDetail();

            //不存在新增的明细时
            if (insertCount == 0)
            {
                return;
            }
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
            //设置单元格是否可以编辑
            SetSalesOrderDetailStyle();
            //对新增的一个和多个进行单元格的特殊处理
            for (int i = insertCount; i > 0; i--)
            {
                gdDetail.Rows[gdDetail.Rows.Count - i].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_PriceIsIncludeTax].Value = SysConst.False;
                gdDetail.Rows[gdDetail.Rows.Count - i].Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_TaxRate].Activation = Activation.ActivateOnly;
            }
        }

        /// <summary>
        /// 删除销售模板明细
        /// </summary>
        private void DeleteSalesTmpDetail()
        {
            //验证销售模板审核状态：销售模板已审核，不能删除明细
            if (cbSasT_ApprovalStatusCode.Value != null && cbSasT_ApprovalStatusCode.Value.ToString() == ApprovalStatusEnum.Code.YSH)
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableEnums.Name.SD_SalesTemplate + ApprovalStatusEnum.Name.YSH, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gdDetail.UpdateData();
            //待删除的销售模板明细
            List<SalesTemplateDetailUIModel> resultSalesTemplateDetailList = _detailGridDS.Where(p => p.IsChecked == true).ToList();
            if (resultSalesTemplateDetailList.Count == 0)
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { SystemTableEnums.Name.SD_SalesTemplateDetail, SystemActionEnum.Name.DELETE }),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            for (int i = 0; i < resultSalesTemplateDetailList.Count; i++)
            {
                //配件名称
                string autoPartName = resultSalesTemplateDetailList[i].SasTD_Name;
                //配件条码
                string autoPartBarcode = resultSalesTemplateDetailList[i].SasTD_Barcode;
                List<SalesTemplateDetailUIModel> deleteSalesTemplateDetailList = _detailGridDS.Where(p => p.SasTD_Name == autoPartName && p.SasTD_Barcode == autoPartBarcode)
                    .ToList();
                if (deleteSalesTemplateDetailList.Count > 0)
                {
                    foreach (var loopSalesTemplateDetail in deleteSalesTemplateDetailList)
                    {
                        _detailGridDS.Remove(loopSalesTemplateDetail);
                    }
                }
            }
            //根据[是否存在明细]控制单头是否可编辑
            SetDetailByIsExistDetail();

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }

        /// <summary>
        /// 获取汽修商户相关信息
        /// </summary>
        /// <param name="paramMerchantCode">汽修商客户ID</param>
        private bool GetARMerchantInfo(string paramMerchantCode)
        {
            #region 验证

            if (string.IsNullOrEmpty(paramMerchantCode))
            {
                //汽修商客户ID为空，汽修商户信息获取失败
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0016, SystemTableColumnEnums.PIS_AutoFactoryCustomer.Name.AFC_Code, MsgParam.AUTOFACTORY + MsgParam.INFORMATION + MsgParam.GET), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //从缓存中获取汽修商户信息
            List<MDLPIS_AutoFactoryCustomer> resultAutoFactoryCustomerList = CacheDAX.Get(CacheDAX.ConfigDataKey.ARMerchant) as List<MDLPIS_AutoFactoryCustomer>;
            if (resultAutoFactoryCustomerList == null)
            {
                //汽修商户信息获取失败
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0002, MsgParam.AUTOFACTORY + MsgParam.INFORMATION + MsgParam.GET), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //获取该汽修商Code
            string tempAutoFactoryCode = string.Empty;
            List<MDLPIS_AutoFactoryCustomer> tempAutoFactoryCustomerList = resultAutoFactoryCustomerList.Where(p => p.AFC_Code == paramMerchantCode).ToList();
            if (tempAutoFactoryCustomerList.Count == 1)
            {
                tempAutoFactoryCode = tempAutoFactoryCustomerList[0].AFC_Code;
            }

            #region 获取商户数据库配置Key

            _arMerchantCode = BLLCom.GetMerchantDbConfigKey(tempAutoFactoryCode);

            if (string.IsNullOrEmpty(_arMerchantCode))
            {
                //请选择有效的汽修商户
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, MsgParam.VALID + MsgParam.OF + MsgParam.AUTOFACTORY), MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return false;
            }

            #endregion

            #endregion

            return true;
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
                    var removeList = HeadGridDS.Where(x => x.IsChecked == true).ToList();
                    foreach (var loopRemove in removeList)
                    {
                        HeadGridDS.Remove(loopRemove);
                    }
                }
                else
                {
                    var curHead = HeadGridDS.FirstOrDefault(x => x.SasT_ID == HeadDS.SasT_ID);
                    if (curHead != null)
                    {
                        HeadGridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = HeadGridDS.FirstOrDefault(x => x.SasT_ID == HeadDS.SasT_ID);
                if (curHead != null)
                {
                    _bll.CopyModel(HeadDS, curHead);
                }
                else
                {
                    HeadGridDS.Insert(0, HeadDS);
                }
            }

            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }
       
        /// <summary>
        /// 设置单元格是否可以编辑
        /// </summary>
        private void SetSalesOrderDetailStyle()
        {
            #region 设置Grid数据颜色
            gdDetail.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;

            foreach (var loopGridRow in gdDetail.Rows)
            {
                if (cbSasT_ApprovalStatusCode.Text == ApprovalStatusEnum.Name.YSH)
                {
                    #region 明细中[数量]、[单价]、[价格是否含税]、[税率]、[汽修商组织]和[备注]列不可编辑

                    #region 数量

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_Qty].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_Qty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_Qty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion

                    #region 单价

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_UnitPrice].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_UnitPrice].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_UnitPrice].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion

                    #region 价格是否含税

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_PriceIsIncludeTax].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_PriceIsIncludeTax].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_PriceIsIncludeTax].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion

                    #region 税率

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_TaxRate].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_TaxRate].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_TaxRate].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion

                    #region 汽修商组织ID

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_AutoFactoryOrgID].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_AutoFactoryOrgID].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_AutoFactoryOrgID].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion

                    #region 备注

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_Remark].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_Remark].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_Remark].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion

                    #endregion
                }
                else
                {
                    #region 明细中[数量]、[单价]、[价格是否含税]、[税率]、[汽修商组织]和[备注]列可编辑

                    #region 数量

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_Qty].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_Qty].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_Qty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #region 单价

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_UnitPrice].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_UnitPrice].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_UnitPrice].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #region 价格是否含税

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_PriceIsIncludeTax].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_PriceIsIncludeTax].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_PriceIsIncludeTax].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #region 税率

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_TaxRate].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_TaxRate].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_TaxRate].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #region 汽修商组织ID

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_AutoFactoryOrgID].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_AutoFactoryOrgID].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_AutoFactoryOrgID].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #region 备注

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_Remark].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_Remark].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesTemplateDetail.Code.SasTD_Remark].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #endregion
                }

            }
            #endregion
        }

        #endregion

    }
}
