using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinTabControl;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.IS;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.UIModel.IS.QCModel;
using SkyCar.Coeus.UIModel.Common.UIModel;
using SkyCar.Coeus.UIModel.IS.UIModel;

namespace SkyCar.Coeus.UI.IS
{
    /// <summary>
    /// 汽修库存和库存异动日志查询
    /// </summary>
    public partial class FrmAutoFactoryInventoryQuery : BaseFormCardListDetail<ARInventoryUIModel, ARInventoryAndTransLogQCModel, ARInventoryUIModel>
    {
        #region 全局变量

        /// <summary>
        /// 汽修库存和库存异动日志查询BLL
        /// </summary>
        private AutoFactoryInventoryQueryBLL _bll = new AutoFactoryInventoryQueryBLL();

        #region Grid数据源

        /// <summary>
        /// 库存异动日志数据源
        /// </summary>
        private List<ARInventoryTransLogUIModel> _inventoryTransLogList = new List<ARInventoryTransLogUIModel>();
        /// <summary>
        /// 库存数据源
        /// </summary>
        List<ARInventoryUIModel> _inventoryList = new List<ARInventoryUIModel>();
        #endregion

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
        /// 【汽修】库存异动类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _qxInventoryTransTypeList = new List<ComComboBoxDataSourceTC>();

        #endregion

        #region 库存翻页相关

        /// <summary>
        /// 库存当前页面索引值
        /// </summary>
        private int _inventoryPageIndex = 1;
        /// <summary>
        /// 库存总记录条数
        /// </summary>
        private int _inventoryTotalRecordCount = 0;
        /// <summary>
        /// 库存总页面数
        /// </summary>
        private int _inventoryTotalPageCount = 0;

        #endregion

        #region 库存异动翻页相关

        /// <summary>
        /// 库存异动当前页面索引值
        /// </summary>
        private int _inventoryTransLogPageIndex = 1;
        /// <summary>
        /// 库存异动总记录条数
        /// </summary>
        private int _inventoryTransLogTotalRecordCount = 0;
        /// <summary>
        /// 库存异动总页面数
        /// </summary>
        private int _inventoryTransLogTotalPageCount = 0;

        #endregion

        /// <summary>
        /// 汽修商户编码
        /// </summary>
        private string _arMerchantCode;
        /// <summary>
        /// 库存列表页面大小
        /// </summary>
        public int PageSizeOfInventoryList = 40;
        /// <summary>
        /// 库存异动列表页面大小
        /// </summary>
        public int PageSizeOfInventoryTransList = 40;
        #endregion

        #region 公共属性

        /// <summary>
        /// 库存异动翻页ToolBar
        /// </summary>
        public UltraToolbarsManager ToolBarPagingForInventoryTransLog; //ToolBarPaging
        /// <summary>
        /// 库存翻页ToolBar
        /// </summary>
        public UltraToolbarsManager ToolBarPagingForInventory;
        #endregion

        #region 系统事件

        /// <summary>
        /// FrmAutoFactoryInventoryQuery构造方法
        /// </summary>
        public FrmAutoFactoryInventoryQuery()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAutoFactoryInventoryQuery_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（动作）
            base.ToolBarActionAndNavigate = toolBarActionAndNavigate;

            #region 翻页相关

            base.ExecuteQuery = QueryAction;
            ////工具栏（动作）单击事件
            this.toolBarActionAndNavigate.ToolClick += new ToolClickEventHandler(base.toolBarActionAndNavigate_ToolClick);

            //[库存]翻页
            this.ToolBarPagingForInventory = ToolbarPagingInventory;
            //[库存]翻页单击事件
            this.ToolBarPagingForInventory.ToolClick += new ToolClickEventHandler(ToolBarPagingForInventory_ToolClick);
            //[库存]翻页[当前页]值改变事件
            this.ToolBarPagingForInventory.ToolValueChanged += new ToolEventHandler(ToolBarPagingForInventory_ToolValueChanged);

            //[库存异动日志]翻页
            this.ToolBarPagingForInventoryTransLog = ToolbarPagingInventoryTrans;
            //[库存异动日志]翻页单击事件
            this.ToolBarPagingForInventoryTransLog.ToolClick += new ToolClickEventHandler(ToolBarPagingForInventoryTransLog_ToolClick);
            //[库存异动日志]翻页[当前页]值改变事件
            this.ToolBarPagingForInventoryTransLog.ToolValueChanged += new ToolEventHandler(ToolBarPagingForInventoryTransLog_ToolValueChanged);

            #region 设置页面大小文本框
            //库存
            TextBoxTool pageSizeOfInventoryList = null;
            foreach (var loopToolControl in this.ToolBarPagingForInventory.Tools)
            {
                if (loopToolControl.Key == SysConst.EN_PAGESIZE)
                {
                    pageSizeOfInventoryList = (TextBoxTool)loopToolControl;
                }
            }
            if (pageSizeOfInventoryList != null)
            {
                pageSizeOfInventoryList.Text = PageSizeOfInventoryList.ToString();
                pageSizeOfInventoryList.AfterToolExitEditMode += PageSizeTextBoxTool_AfterToolExitEditMode;
            }
            //库存异动日志
            TextBoxTool pageSizeOfInventoryTransList = null;
            foreach (var loopToolControl in this.ToolBarPagingForInventoryTransLog.Tools)
            {
                if (loopToolControl.Key == SysConst.EN_PAGESIZE)
                {
                    pageSizeOfInventoryTransList = (TextBoxTool)loopToolControl;
                }
            }
            if (pageSizeOfInventoryTransList != null)
            {
                pageSizeOfInventoryTransList.Text = PageSizeOfInventoryTransList.ToString();
                pageSizeOfInventoryTransList.AfterToolExitEditMode += PageSizeTextBoxTool_AfterToolExitEditMode;
            }

            #endregion

            #endregion

            //初始化【库存】和【库存异动日志】Tab内控件
            InitializeListTabControls();
            //设置【库存】Tab为选中状态
            tabControlFull.Tabs[SysConst.Inventory].Selected = true;
            #endregion
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

            if (tabControlFull.Tabs[SysConst.Inventory].Selected)
            {
                PageSizeOfInventoryList = tmpPageSize;
            }
            else if (tabControlFull.Tabs[SysConst.InventoryLog].Selected)
            {
                PageSizeOfInventoryTransList = tmpPageSize;
            }

            QueryAction();
        }

        /// <summary>
        /// 选中的Tab改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlFull_SelectedTabChanging(object sender, SelectedTabChangingEventArgs e)
        {
            base.SetActionEnableBySelectedTab(e.Tab.Key);
        }

        #region 【库存】相关事件

        /// <summary>
        /// 【库存】出/入库时间终了ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtInventoryEndTime_ValueChanged(object sender, EventArgs e)
        {
            if (this.dtInventoryEndTime.Value != null &&
              this.dtInventoryEndTime.DateTime.Hour == 0 &&
              this.dtInventoryEndTime.DateTime.Minute == 0 &&
              this.dtInventoryEndTime.DateTime.Second == 0)
            {
                DateTime newDateTime = new DateTime(this.dtInventoryEndTime.DateTime.Year, this.dtInventoryEndTime.DateTime.Month, this.dtInventoryEndTime.DateTime.Day, 23, 59, 59);
                this.dtInventoryEndTime.DateTime = newDateTime;
            }
        }

        /// <summary>
        ///【库存】汽修商户改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbWhere_InvARMerchant_SelectedIndexChanged(object sender, EventArgs e)
        {
            mcbWhere_InvAROrgName.Clear();

            if (string.IsNullOrEmpty(mcbWhere_InvARMerchant.SelectedText)
                || string.IsNullOrEmpty(mcbWhere_InvARMerchant.SelectedValue))
            {
                mcbWhere_InvAROrgName.DataSource = null;
                return;
            }
            if (mcbWhere_InvARMerchant.SelectedValue != _arMerchantCode)
            {
                //获取汽修商户相关信息
                bool getARMerchantInfoResult = GetARMerchantInfo(mcbWhere_InvARMerchant.SelectedValue);
                if (!getARMerchantInfoResult)
                {
                    return;
                }
            }

            if (_autoFactoryOrgList != null)
            {
                var tempAutoFactoryOrgList =
                    _autoFactoryOrgList.Where(x => x.AutoFactoryCode == mcbWhere_InvARMerchant.SelectedValue).ToList();
                mcbWhere_InvAROrgName.DataSource = tempAutoFactoryOrgList;
            }
        }

        /// <summary>
        /// 【库存】Venus库存查询CellChange事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdInventory_CellChange(object sender, CellEventArgs e)
        {
            gdInventory.UpdateData();
        }

        #region 库存Tab页

        /// <summary>
        /// 是否区分汽修商户组织
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckIsDifferentiateOrg_CheckedValueChanged(object sender, EventArgs e)
        {
            if (ckIsDifferentiateOrg.Checked)
            {
                lblWhere_InvAROrgName.Visible = true;
                mcbWhere_InvAROrgName.Visible = true;

                //gdInventory显示汽修商户组织列
                this.gdInventory.DisplayLayout.Bands[0].Columns[SysConst.INV_Org_ShortName].Hidden = false;
            }
            else
            {
                lblWhere_InvAROrgName.Visible = false;
                mcbWhere_InvAROrgName.Visible = false;
                //gdInventory不显示汽修商户组织列
                this.gdInventory.DisplayLayout.Bands[0].Columns[SysConst.INV_Org_ShortName].Hidden = true;
            }
            _inventoryPageIndex = 1;
        }
        /// <summary>
        /// 选择配件名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAPA_Name_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            //查询配件名称
            FrmAutoPartsNameQuery frmAutoPartsNameQuery =
                new FrmAutoPartsNameQuery(SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name,
                paramSelectedValue: this.txtAPA_Name.Text)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
            DialogResult dialogResult = frmAutoPartsNameQuery.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                this.txtAPA_Name.Text = frmAutoPartsNameQuery.SelectedText;
            }
        }
        /// <summary>
        /// 选择配件品牌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAPA_Brand_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            //先选择配件名称，才能选配件品牌
            if (string.IsNullOrEmpty(txtAPA_Name.Text))
            {
                MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0006, new object[] { SystemTableColumnEnums.BS_AutoPartsArchive.Name.APA_Name, SystemActionEnum.Name.QUERY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //查询配件品牌
            FrmAutoPartsBrandQuery frmAutoPartsBrandQuery =
                new FrmAutoPartsBrandQuery(SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Brand,
                paramSelectedValue: this.txtAPA_Brand.Text,
                paramCustomeSelectionMode: CustomEnums.CustomeSelectionMode.Multiple,
                paramItemSelectedItemParentValue: this.txtAPA_Name.Text)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
            DialogResult dialogResult = frmAutoPartsBrandQuery.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                this.txtAPA_Brand.Text = frmAutoPartsBrandQuery.SelectedText;
            }
        }
        #endregion

        #region 库存Tab页Grid翻页相关方法
        #region 公共

        /// <summary>
        /// 设置翻页控件
        /// <para>窗体加载或初始化时调用</para>
        /// </summary>
        /// <param name="paramTotalRecordCount">总记录条数</param>
        public void SetBarPagingForInventory(int paramTotalRecordCount)
        {
            var funcName = "SetBarPagingForInventory";
            LogHelper.WriteBussLogStart(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
            //重新计算[总页数]，设置最新[总记录条数]
            SetTotalPageCountAndTotalRecordCountForInventory(paramTotalRecordCount);
            //设置翻页按钮状态
            SetPageButtonStatusForInventory();
            LogHelper.WriteBussLogEndOK(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void ToolBarPagingForInventory_ToolClick(object sender, ToolClickEventArgs e)
        {
            var funcName = "ToolBarPagingForInventory_ToolClick";
            LogHelper.WriteBussLogStart(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPagingForInventory != null)
            {
                switch (e.Tool.Key)
                {
                    //第一页
                    case SysConst.EN_FIRSTPAGE:
                        ((TextBoxTool)(ToolBarPagingForInventory.Tools[SysConst.EN_PAGEINDEX])).Text =
                            SysConst.NUMBER_ONE.ToString();
                        break;
                    // 前一页
                    case SysConst.EN_FORWARDPAGE:
                        ((TextBoxTool)(ToolBarPagingForInventory.Tools[SysConst.EN_PAGEINDEX])).Text = (_inventoryPageIndex - 1).ToString();
                        break;
                    // 下一页
                    case SysConst.EN_NEXTPAGE:
                        ((TextBoxTool)(ToolBarPagingForInventory.Tools[SysConst.EN_PAGEINDEX])).Text = (_inventoryPageIndex + 1).ToString();
                        break;
                    // 最后一页
                    case SysConst.EN_LASTPAGE:
                        ((TextBoxTool)(ToolBarPagingForInventory.Tools[SysConst.EN_PAGEINDEX])).Text = _inventoryTotalPageCount.ToString();
                        break;
                }
            }
            LogHelper.WriteBussLogEndOK(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
        }
        /// <summary>
        /// 翻页ToolBar的值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ToolBarPagingForInventory_ToolValueChanged(object sender, ToolEventArgs e)
        {
            var funcName = "ToolBarPagingForInventory_ToolValueChanged";
            LogHelper.WriteBussLogStart(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
            if (SysConst.EN_PAGEINDEX.Equals(e.Tool.Key))
            {
                string strValue = ((TextBoxTool)(e.Tool)).Text.Trim();
                int tmpPageIndex = 0;
                if (!int.TryParse(strValue, out tmpPageIndex) && tmpPageIndex <= 0)
                {
                    ((TextBoxTool)(e.Tool)).Text = SysConst.NUMBER_ONE.ToString();
                    ((TextBoxTool)(e.Tool)).SelectionLength = 1;
                    return;
                }
                else if (tmpPageIndex > _inventoryTotalPageCount)
                {
                    ((TextBoxTool)(e.Tool)).Text = _inventoryTotalPageCount.ToString();
                    ((TextBoxTool)(e.Tool)).SelectionLength = _inventoryTotalPageCount.ToString().Length;
                    return;
                }

                #region
                if (Convert.ToInt32(strValue) <= 0)
                {
                    ((TextBoxTool)(e.Tool)).Text = SysConst.NUMBER_ONE.ToString();
                }
                else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) >= _inventoryTotalPageCount)
                {
                    ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(_inventoryTotalPageCount).ToString();
                }
                else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) < _inventoryTotalPageCount)
                {
                    ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(strValue).ToString();
                }
                #endregion

                _inventoryPageIndex = tmpPageIndex;
                if (tmpPageIndex <= 0)
                {
                    _inventoryPageIndex = 1;
                }
                ExecuteQuery?.Invoke();
            }
            LogHelper.WriteBussLogEndOK(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
        }
        #endregion

        #region 私有

        /// <summary>
        /// 设置总页数和总记录条数
        /// </summary>
        /// <param name="paramTotalRecordCount">总记录条数</param>
        private void SetTotalPageCountAndTotalRecordCountForInventory(int paramTotalRecordCount)
        {
            var funcName = "SetTotalPageCountAndTotalRecordCountForInventory";
            LogHelper.WriteBussLogStart(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPagingForInventory != null)
            {
                if (paramTotalRecordCount > 0)
                {
                    _inventoryTotalRecordCount = paramTotalRecordCount;
                    int? remainder = _inventoryTotalRecordCount % PageSizeOfInventoryList;
                    if (remainder > 0)
                    {
                        _inventoryTotalPageCount = _inventoryTotalRecordCount / PageSizeOfInventoryList + 1;
                    }
                    else
                    {
                        _inventoryTotalPageCount = _inventoryTotalRecordCount / PageSizeOfInventoryList;
                    }
                }
                else
                {
                    _inventoryPageIndex = 1;
                    _inventoryTotalPageCount = 1;
                    _inventoryTotalRecordCount = 0;
                }
                ((TextBoxTool)(ToolBarPagingForInventory.Tools[SysConst.EN_PAGEINDEX])).Text = _inventoryPageIndex.ToString();
                ToolBarPagingForInventory.Tools[SysConst.EN_PAGECOUNT].SharedProps.Caption = _inventoryTotalPageCount.ToString();
            }
            LogHelper.WriteBussLogEndOK(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 设置翻页按钮状态
        /// </summary>
        private void SetPageButtonStatusForInventory()
        {
            var funcName = "SetPageButtonStatusForInventory";
            LogHelper.WriteBussLogStart(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPagingForInventory != null)
            {
                if (_inventoryPageIndex == 0 || _inventoryTotalRecordCount == 0)
                {
                    ToolBarPagingForInventory.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInventory.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInventory.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInventory.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;

                    return;
                }
                if (_inventoryPageIndex == 1 && _inventoryTotalRecordCount <= PageSizeOfInventoryList)
                {
                    ToolBarPagingForInventory.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInventory.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInventory.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInventory.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (_inventoryPageIndex == 1 && _inventoryTotalRecordCount > PageSizeOfInventoryList)
                {
                    ToolBarPagingForInventory.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingForInventory.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingForInventory.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInventory.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (_inventoryPageIndex != 1 && _inventoryTotalRecordCount > PageSizeOfInventoryList * _inventoryPageIndex)
                {
                    ToolBarPagingForInventory.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingForInventory.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingForInventory.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingForInventory.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
                }
                else if (_inventoryPageIndex != 1 && _inventoryTotalRecordCount <= PageSizeOfInventoryList * _inventoryPageIndex)
                {
                    ToolBarPagingForInventory.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInventory.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInventory.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingForInventory.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
                }
                else
                {
                    throw (new Exception("非预期的场合。。。"));
                }
            }
            LogHelper.WriteBussLogEndOK(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
        }
        #endregion

        #endregion

        #endregion

        #region 【库存异动日志】相关事件

        /// <summary>
        ///【库存】汽修商户改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbWhere_ItlARMerchant_SelectedIndexChanged(object sender, EventArgs e)
        {
            mcbWhere_ItlAROrgName.Clear();

            if (string.IsNullOrEmpty(mcbWhere_ItlARMerchant.SelectedText)
                || string.IsNullOrEmpty(mcbWhere_ItlARMerchant.SelectedValue))
            {
                mcbWhere_ItlAROrgName.DataSource = null;
                return;
            }
            if (mcbWhere_ItlARMerchant.SelectedValue != _arMerchantCode)
            {
                //获取汽修商户相关信息
                bool getARMerchantInfoResult = GetARMerchantInfo(mcbWhere_ItlARMerchant.SelectedValue);
                if (!getARMerchantInfoResult)
                {
                    return;
                }
            }

            if (_autoFactoryOrgList != null)
            {
                var tempAutoFactoryOrgList =
                    _autoFactoryOrgList.Where(x => x.AutoFactoryCode == mcbWhere_ItlARMerchant.SelectedValue).ToList();
                mcbWhere_ItlAROrgName.DataSource = tempAutoFactoryOrgList;
            }
        }

        #region 库存异动Tab页Grid翻页相关方法

        #region 公共
        /// <summary>
        /// 设置翻页控件
        /// <para>窗体加载或初始化时调用</para>
        /// </summary>
        /// <param name="paramTotalRecordCount">总记录条数</param>
        public void SetBarPagingForInventoryTransLog(int paramTotalRecordCount)
        {
            var funcName = "SetBarPagingForInventoryTransLog";
            LogHelper.WriteBussLogStart(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
            //重新计算[总页数]，设置最新[总记录条数]
            SetTotalPageCountAndTotalRecordCountForInventoryTransLog(paramTotalRecordCount);
            //设置翻页按钮状态
            SetPageButtonStatusForInventoryTransLog();
            LogHelper.WriteBussLogEndOK(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void ToolBarPagingForInventoryTransLog_ToolClick(object sender, ToolClickEventArgs e)
        {
            var funcName = "ToolBarPagingForInventoryTransLog_ToolClick";
            LogHelper.WriteBussLogStart(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPagingForInventoryTransLog != null)
            {
                switch (e.Tool.Key)
                {
                    //第一页
                    case SysConst.EN_FIRSTPAGE:
                        ((TextBoxTool)(ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_PAGEINDEX])).Text =
                            SysConst.NUMBER_ONE.ToString();
                        break;
                    // 前一页
                    case SysConst.EN_FORWARDPAGE:
                        ((TextBoxTool)(ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_PAGEINDEX])).Text = (_inventoryTransLogPageIndex - 1).ToString();
                        break;
                    // 下一页
                    case SysConst.EN_NEXTPAGE:
                        ((TextBoxTool)(ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_PAGEINDEX])).Text = (_inventoryTransLogPageIndex + 1).ToString();
                        break;
                    // 最后一页
                    case SysConst.EN_LASTPAGE:
                        ((TextBoxTool)(ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_PAGEINDEX])).Text = _inventoryTransLogTotalPageCount.ToString();
                        break;
                }
            }
            LogHelper.WriteBussLogEndOK(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
        }
        /// <summary>
        /// 翻页ToolBar的值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ToolBarPagingForInventoryTransLog_ToolValueChanged(object sender, ToolEventArgs e)
        {
            var funcName = "ToolBarPagingForInventoryTransLog_ToolValueChanged";
            LogHelper.WriteBussLogStart(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
            if (SysConst.EN_PAGEINDEX.Equals(e.Tool.Key))
            {
                string strValue = ((TextBoxTool)(e.Tool)).Text.Trim();
                int tmpPageIndex = 0;
                if (!int.TryParse(strValue, out tmpPageIndex) && tmpPageIndex <= 0)
                {
                    ((TextBoxTool)(e.Tool)).Text = SysConst.NUMBER_ONE.ToString();
                    ((TextBoxTool)(e.Tool)).SelectionLength = 1;
                    return;
                }
                else if (tmpPageIndex > _inventoryTransLogTotalPageCount)
                {
                    ((TextBoxTool)(e.Tool)).Text = _inventoryTransLogTotalPageCount.ToString();
                    ((TextBoxTool)(e.Tool)).SelectionLength = _inventoryTransLogTotalPageCount.ToString().Length;
                    return;
                }

                #region
                if (Convert.ToInt32(strValue) <= 0)
                {
                    ((TextBoxTool)(e.Tool)).Text = SysConst.NUMBER_ONE.ToString();
                }
                else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) >= _inventoryTransLogTotalPageCount)
                {
                    ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(_inventoryTransLogTotalPageCount).ToString();
                }
                else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) < _inventoryTransLogTotalPageCount)
                {
                    ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(strValue).ToString();
                }
                #endregion

                _inventoryTransLogPageIndex = tmpPageIndex;
                if (tmpPageIndex <= 0)
                {
                    _inventoryTransLogPageIndex = 1;
                }
                ExecuteQuery?.Invoke();
            }
            LogHelper.WriteBussLogEndOK(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
        }
        #endregion

        #region 私有
        /// <summary>
        /// 设置总页数和总记录条数
        /// </summary>
        /// <param name="paramTotalRecordCount">总记录条数</param>
        private void SetTotalPageCountAndTotalRecordCountForInventoryTransLog(int paramTotalRecordCount)
        {
            var funcName = "SetTotalPageCountAndTotalRecordCountForInventoryTransLog";
            LogHelper.WriteBussLogStart(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPagingForInventoryTransLog != null)
            {
                if (paramTotalRecordCount > 0)
                {
                    _inventoryTransLogTotalRecordCount = paramTotalRecordCount;
                    int? remainder = _inventoryTransLogTotalRecordCount % PageSizeOfInventoryTransList;
                    if (remainder > 0)
                    {
                        _inventoryTransLogTotalPageCount = _inventoryTransLogTotalRecordCount / PageSizeOfInventoryTransList + 1;
                    }
                    else
                    {
                        _inventoryTransLogTotalPageCount = _inventoryTransLogTotalRecordCount / PageSizeOfInventoryTransList;
                    }
                }
                else
                {
                    _inventoryTransLogPageIndex = 1;
                    _inventoryTransLogTotalPageCount = 1;
                    _inventoryTransLogTotalRecordCount = 0;
                }
                ((TextBoxTool)(ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_PAGEINDEX])).Text = _inventoryTransLogPageIndex.ToString();
                ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_PAGECOUNT].SharedProps.Caption = _inventoryTransLogTotalPageCount.ToString();
            }
            LogHelper.WriteBussLogEndOK(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 设置翻页按钮状态
        /// </summary>
        private void SetPageButtonStatusForInventoryTransLog()
        {
            var funcName = "SetPageButtonStatusForInventoryTransLog";
            LogHelper.WriteBussLogStart(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPagingForInventoryTransLog != null)
            {
                if (_inventoryTransLogPageIndex == 0 || _inventoryTransLogTotalRecordCount == 0)
                {
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;

                    return;
                }
                if (_inventoryTransLogPageIndex == 1 && _inventoryTransLogTotalRecordCount <= PageSizeOfInventoryTransList)
                {
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (_inventoryTransLogPageIndex == 1 && _inventoryTransLogTotalRecordCount > PageSizeOfInventoryTransList)
                {
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (_inventoryTransLogPageIndex != 1 && _inventoryTransLogTotalRecordCount > PageSizeOfInventoryTransList * _inventoryTransLogPageIndex)
                {
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
                }
                else if (_inventoryTransLogPageIndex != 1 && _inventoryTransLogTotalRecordCount <= PageSizeOfInventoryTransList * _inventoryTransLogPageIndex)
                {
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingForInventoryTransLog.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
                }
                else
                {
                    throw (new Exception("非预期的场合。。。"));
                }
            }
            LogHelper.WriteBussLogEndOK(Trans.IS, LoginInfoDAX.UserName, funcName, "", "", null);
        }
        #endregion

        #endregion

        #endregion

        #endregion

        #region 导航按钮

        /// <summary>
        /// 初始化导航按钮
        /// </summary>
        public override void InitializeNavigate()
        {
            base.InitializeNavigate();
            //转销售
            SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSALESORDER, true, true);
        }

        /// <summary>
        /// 转销售
        /// </summary>
        public override void ToSalesOrderNavigate()
        {
            //base.ToSalesOrderNavigate();

            //#region 验证

            ////验证客户名称
            //if (string.IsNullOrEmpty(mcbWhere_InvARMerchant.SelectedText)
            //    || string.IsNullOrEmpty(mcbWhere_InvARMerchant.SelectedValue)
            //    || string.IsNullOrEmpty(_arMerchantCode))
            //{
            //    MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.AUTOFACTORY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            ////验证汽修商户组织
            //if (string.IsNullOrEmpty(mcbWhere_InvAROrgName.SelectedText)
            //    || string.IsNullOrEmpty(mcbWhere_InvAROrgName.SelectedValue))
            //{
            //    MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.AUTOFACTORY_ORG }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            ////验证勾选的库存
            //List<InventoryUIModel> checkedInventoryList = _inventoryList.Where(p => p.IsChecked == true).ToList();
            //if (checkedInventoryList.Count == 0)
            //{
            //    MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { SystemTableEnums.Name.PIS_Inventory, MsgParam.TO_SALESORDER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            ////选择的库存不是mcbWhere_InvAROrgName.SelectedText组织下的
            //string[] autoFactoryOrgInfo = mcbWhere_InvAROrgName.SelectedValue.Split(';');
            //if (checkedInventoryList.Any(x => x.INV_Org_ID != autoFactoryOrgInfo[1]))
            //{
            //    MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { mcbWhere_InvAROrgName.SelectedText + MsgParam.OF + SystemTableEnums.Name.PIS_Inventory + MsgParam.TO_SALESORDER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            //#endregion

            //if (cbSO_ApprovalStatusCode.Text == ApprovalStatusEnum.Name.YSH)
            //{
            //    //初始化之前需要将之前选择平台内汽修客户的信息备份下
            //    string customerId = mcbClientName.SelectedValue;
            //    string customerName = mcbClientName.SelectedText;
            //    //将销售画面中的控件进行初始化
            //    InitializeDetailTabControls();
            //    //重新赋值
            //    mcbClientName.SelectedValue = customerId;
            //    //初始化明细
            //    _detailGridDS = new SkyCarBindingList<SalesOrderDetailUIModel, MDLSD_SalesOrderDetail>();
            //}

            //#region 设置[销售]Tab的单头

            //mcbAutoFactoryName.SelectedText = mcbWhere_InvARMerchant.SelectedText;
            //mcbAutoFactoryName.SelectedValue = mcbWhere_InvARMerchant.SelectedValue;
            //mcbClientName.SelectedValue = autoFactoryOrgInfo[0];
            //mcbClientName.SelectedText = mcbWhere_InvAROrgName.SelectedText;
            //_clientOrgID = autoFactoryOrgInfo[1];
            //_clientOrgCode = autoFactoryOrgInfo[2];
            //#endregion

            ////新增销售订单明细
            //InsertSalesOrderDetail(InsertSalesOrderDetailType.AutoRepairBusinessInventory, checkedInventoryList);
            ////选中【详情】Tab
            //tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;
        }

        #endregion

        #region 重写基类方法

        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            if (tabControlFull.Tabs[SysConst.Inventory].Selected)
            {
                #region 当前选中Tab为【库存】的场合

                //查询汽修商库存
                QueryInventory();
                #endregion
            }
            else if (tabControlFull.Tabs[SysConst.InventoryLog].Selected)
            {
                #region 当前选中Tab为【库存异动日志】的场合

                //查询汽修商库存异动日志
                QueryInventoryTransLog();
                #endregion
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public override void ClearAction()
        {
            if (tabControlFull.Tabs[SysConst.Inventory].Selected)
            {
                #region [库存]Tab

                //默认是显示商户组织
                lblWhere_InvAROrgName.Visible = true;
                mcbWhere_InvAROrgName.Visible = true;
                //汽修商户，是否分组织，汽修商户组织，配件条码，第三方编码，配件名称，配件品牌，规格型号，出入库时间（开始-终了）
                mcbWhere_InvARMerchant.Clear();
                ckIsDifferentiateOrg.Checked = true;
                mcbWhere_InvAROrgName.Clear();
                txtINV_Barcode.Clear();
                txtINV_ThirdNo.Clear();
                txtAPA_Name.Clear();
                txtAPA_Brand.Clear();
                txtAPA_Specification.Clear();
                dtInventoryBeginTime.Value = null;
                dtInventoryEndTime.Value = null;

                _inventoryList = new List<ARInventoryUIModel>();
                gdInventory.DataSource = _inventoryList;
                gdInventory.DataBind();

                #endregion
            }
            else if (tabControlFull.Tabs[SysConst.InventoryLog].Selected)
            {
                #region [库存异动日志]Tab

                mcbWhere_ItlARMerchant.Clear();
                mcbWhere_ItlAROrgName.Clear();
                cbWHERE_ITL_TransType.Items.Clear();
                cbWHERE_ITL_TransType.DisplayMember = SysConst.EN_TEXT;
                cbWHERE_ITL_TransType.ValueMember = SysConst.EN_Code;
                cbWHERE_ITL_TransType.DataSource = _qxInventoryTransTypeList;
                cbWHERE_ITL_TransType.DataBind();

                _inventoryTransLogList = new List<ARInventoryTransLogUIModel>();
                gdInventoryTransLog.DataSource = _inventoryTransLogList;
                gdInventoryTransLog.DataBind();
                #endregion
            }
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 初始化下拉框

            //汽修商户名称
            _autoFactoryList = CacheDAX.Get(CacheDAX.ConfigDataKey.ARMerchant) as List<MDLPIS_AutoFactoryCustomer>;
            if (_autoFactoryList != null)
            {
                var autoFactoryCustomerList = _autoFactoryList.Where(x => x.AFC_IsPlatform != null && x.AFC_IsPlatform.Value == true).ToList();

                mcbWhere_InvARMerchant.DisplayMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Name;
                mcbWhere_InvARMerchant.ValueMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Code;
                mcbWhere_InvARMerchant.DataSource = autoFactoryCustomerList;

                mcbWhere_ItlARMerchant.DisplayMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Name;
                mcbWhere_ItlARMerchant.ValueMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Code;
                mcbWhere_ItlARMerchant.DataSource = autoFactoryCustomerList;
            }

            //汽修商户组织
            var allCustomerList = CacheDAX.Get(CacheDAX.ConfigDataKey.Customer) as List<CustomerQueryUIModel>;
            if (allCustomerList != null)
            {
                _autoFactoryOrgList = allCustomerList.Where(x => x.CustomerType == CustomerTypeEnum.Name.PTNQXSH).ToList();
            }
            mcbWhere_InvAROrgName.DisplayMember = "CustomerName";
            mcbWhere_InvAROrgName.ValueMember = "AutoFactoryOrgInfo";
            if (!string.IsNullOrEmpty(mcbWhere_InvARMerchant.SelectedValue))
            {
                var tempAutoFactoryOrgList =
                    _autoFactoryOrgList.Where(x => x.AutoFactoryCode == mcbWhere_InvARMerchant.SelectedValue);
                mcbWhere_InvAROrgName.DataSource = tempAutoFactoryOrgList;
            }

            mcbWhere_ItlAROrgName.DisplayMember = "CustomerName";
            mcbWhere_ItlAROrgName.ValueMember = "AutoFactoryOrgInfo";
            if (!string.IsNullOrEmpty(mcbWhere_ItlARMerchant.SelectedValue))
            {
                var tempAutoFactoryOrgList =
                    _autoFactoryOrgList.Where(x => x.AutoFactoryCode == mcbWhere_ItlARMerchant.SelectedValue);
                mcbWhere_ItlAROrgName.DataSource = tempAutoFactoryOrgList;
            }

            //汽修端库存异动类型
            _qxInventoryTransTypeList = GetVenusInventoryTransType();
            cbWHERE_ITL_TransType.DisplayMember = SysConst.EN_TEXT;
            cbWHERE_ITL_TransType.ValueMember = SysConst.EN_Code;
            cbWHERE_ITL_TransType.DataSource = _qxInventoryTransTypeList;
            cbWHERE_ITL_TransType.DataBind();

            #endregion
        }

        /// <summary>
        /// 获取汽修端库存异动类型
        /// </summary>
        private List<ComComboBoxDataSourceTC> GetVenusInventoryTransType()
        {
            List<ComComboBoxDataSourceTC> resultInventoryTransType = new List<ComComboBoxDataSourceTC>
            {
                new ComComboBoxDataSourceTC
                {
                    Text = "入库",
                    Code = "RK",
                },
                new ComComboBoxDataSourceTC
                {
                    Text = "取消入库",
                    Code = "QXRK",
                },
                new ComComboBoxDataSourceTC
                {
                    Text = "领料出库",
                    Code = "LLCK",
                },
                new ComComboBoxDataSourceTC
                {
                    Text = "退货出库",
                    Code = "THCK",
                },
                new ComComboBoxDataSourceTC
                {
                    Text = "领料退库",
                    Code = "LLTK",
                },
                new ComComboBoxDataSourceTC
                {
                    Text = "盘点调整",
                    Code = "PDTZ",
                },
                new ComComboBoxDataSourceTC
                {
                    Text = "退货退库",
                    Code = "THTK",
                },
                new ComComboBoxDataSourceTC
                {
                    Text = "销售出库",
                    Code = "XSCK",
                },
                new ComComboBoxDataSourceTC
                {
                    Text = "销售退库",
                    Code = "XSTK",
                },
                new ComComboBoxDataSourceTC
                {
                    Text = "调拨入库",
                    Code = "DBRK",
                },
                new ComComboBoxDataSourceTC
                {
                    Text = "调拨出库",
                    Code = "DBCK",
                },
                new ComComboBoxDataSourceTC
                {
                    Text = "调拨退库",
                    Code = "DBTK",
                }
            };

            return resultInventoryTransType;
        }

        /// <summary>
        /// 将Grid中选中的数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetGridDataToCardCtrls()
        {
            //判断是否允许将【列表】Grid数据设置到【详情】Tab内的对应控件
            //if (!IsAllowSetGridDataToCard())
            //{
            //    return;
            //}
            //var activeRowIndex = gdGrid.ActiveRow.Index;
            ////判断Grid内[唯一标识]是否为空
            //if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SD_SalesOrder.Code.SO_ID].Value == null ||
            //    string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SD_SalesOrder.Code.SO_ID].Value.ToString()))
            //{
            //    return;
            //}
            ////将选中的Grid行对应数据Model赋值给[DetailDS]
            ////********************************************************************************
            ////**********************************【重要说明】**********************************
            ////*****此处和上面的条件判断必须用HeadDS内能唯一标识一条记录的字段作为过滤条件*****
            ////********************************************************************************
            //HeadDS = base.HeadGridDS.FirstOrDefault(x => x.SO_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SD_SalesOrder.Code.SO_ID].Value.ToString());
            //if (HeadDS == null || string.IsNullOrEmpty(HeadDS.SO_ID))
            //{
            //    return;
            //}

            ////如果[客户类型]为[平台内汽修商]
            //if (HeadDS.SO_CustomerTypeName == CustomerTypeEnum.Name.PTNQXSH
            //    && !string.IsNullOrEmpty(HeadDS.AutoFactoryCode))
            //{
            //    if (_latestAutoFactoryCode != HeadDS.AutoFactoryCode)
            //    {
            //        //获取汽修商户相关信息
            //        bool getARMerchantInfoResult = GetARMerchantInfo(HeadDS.AutoFactoryCode);
            //        if (!getARMerchantInfoResult)
            //        {
            //            return;
            //        }
            //    }

            //    _latestAutoFactoryCode = HeadDS.AutoFactoryCode;
            //}

            ////将DetailDS数据赋值给【详情】Tab内的对应控件
            //SetDetailDSToCardCtrls();

            ////选中【销售】Tab
            //tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;

            ////查询明细Grid数据并绑定
            //QueryDetail();
            ////获取出库明细列表
            //GetStockOutDetail();
            ////获取配件类别
            //GetAutoPartsTypeByName(_detailGridDS);

            ////设置详情页面控件的是否可编辑
            //SetDetailControl();
            ////设置动作按钮状态
            //SetActionEnableByStatus();
            ////设置明细单元格状态
            //SetSalesOrderDetailStyle();
        }

        private string _latestAROrgId = string.Empty;

        ///// <summary>
        ///// 是否允许将【列表】Grid数据设置到【详情】Tab内的对应控件
        ///// </summary>
        ///// <returns>true:允许；false：不允许</returns>
        //private bool IsAllowSetGridDataToCard()
        //{
        //    if (gdGrid.ActiveRow == null || gdGrid.ActiveRow.Index < 0)
        //    {
        //        InitializeDetailTabControls();
        //        return false;
        //    }
        //    if (gdGrid.DisplayLayout.Bands[0].SortedColumns.Count > 0)
        //    {
        //        foreach (UltraGridColumn loopUltraGridColumn in gdGrid.DisplayLayout.Bands[0].SortedColumns)
        //        {
        //            if (loopUltraGridColumn.IsGroupByColumn)
        //            {
        //                InitializeDetailTabControls();
        //                return false;
        //            }
        //        }
        //    }
        //    return true;
        //}

        /// <summary>
        /// 查询汽修商库存
        /// </summary>
        private void QueryInventory()
        {
            //汽修商户为必选项
            if (string.IsNullOrEmpty(mcbWhere_InvARMerchant.SelectedText)
                || string.IsNullOrEmpty(mcbWhere_InvARMerchant.SelectedValue))
            {
                MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.AUTOFACTORY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //判断汽修商的数据库链接字符串是否存在
            if (string.IsNullOrEmpty(_arMerchantCode) || !DBManager.CheckConnectin(_arMerchantCode))
            {
                MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { MsgParam.AUTOFACTORY + MsgParam.SERVER + MsgParam.CONNECTION }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ARInventoryAndTransLogQCModel argsInventory = new ARInventoryAndTransLogQCModel
            {
                PageIndex = _inventoryPageIndex,
                PageSize = PageSizeOfInventoryList,
                APA_Brand = txtAPA_Brand.Text.Trim(),
                APA_Name = txtAPA_Name.Text.Trim(),
                APA_Specification = txtAPA_Specification.Text.Trim(),
                INV_Barcode = txtINV_Barcode.Text.Trim(),
                INV_ThirdNo = txtINV_ThirdNo.Text.Trim(),
                MerchantCode = LoginInfoDAX.MCTCode,
            };
            if (!string.IsNullOrEmpty(dtInventoryBeginTime.Text.Trim()))
            {
                argsInventory.INV_CreatedTime_Start = Convert.ToDateTime(dtInventoryBeginTime.Text);
            }
            if (!string.IsNullOrEmpty(dtInventoryEndTime.Text.Trim()))
            {
                argsInventory.INV_CreatedTime_End = Convert.ToDateTime(dtInventoryEndTime.Text);
            }
            //判断是否需要显示汽修商户组织信息
            if (ckIsDifferentiateOrg.Checked)
            {
                //显示汽修商户组织
                if (!string.IsNullOrEmpty(mcbWhere_InvAROrgName.SelectedValue)
                    && !string.IsNullOrEmpty(mcbWhere_InvAROrgName.SelectedText))
                {
                    argsInventory.INV_Org_ShortName = mcbWhere_InvAROrgName.SelectedText;
                }
            }
            _inventoryList = _bll.QueryInventoryList(_arMerchantCode, argsInventory, ckIsDifferentiateOrg.Checked);
            if (_inventoryList != null)
            {
                gdInventory.DataSource = _inventoryList;
                gdInventory.DataBind();
                gdInventory.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
                gdInventory.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
                int totalRecordCount = 0;
                //设置分页控件
                if (_inventoryList.Count > 0)
                {
                    totalRecordCount = _inventoryList[0].RecordCount ?? 0;
                }
                SetBarPagingForInventory(totalRecordCount);
            }
        }

        /// <summary>
        /// 查询汽修商库存异动日志
        /// </summary>
        private void QueryInventoryTransLog()
        {
            //汽修商户为必选项
            if (string.IsNullOrEmpty(mcbWhere_ItlARMerchant.SelectedText)
                || string.IsNullOrEmpty(mcbWhere_ItlARMerchant.SelectedValue))
            {
                MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.AUTOFACTORY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //判断汽修商的数据库链接字符串是否存在
            if (string.IsNullOrEmpty(_arMerchantCode) || !DBManager.CheckConnectin(_arMerchantCode))
            {
                MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.AUTOFACTORY + MsgParam.SERVER + MsgParam.CONNECTION }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //查询汽修商户的库存异动日志
            _inventoryTransLogList = _bll.QueryInventoryTransLogList(_arMerchantCode, new ARInventoryAndTransLogQCModel
            {
                MerchantCode = LoginInfoDAX.MCTCode,
                ITL_Org_Name = mcbWhere_ItlAROrgName.SelectedText,
                ITL_TransType = cbWHERE_ITL_TransType.Text,
                PageIndex = _inventoryTransLogPageIndex,
                PageSize = PageSizeOfInventoryTransList
            });
            if (_inventoryTransLogList != null)
            {
                gdInventoryTransLog.DataSource = _inventoryTransLogList;
                gdInventoryTransLog.DataBind();
                gdInventoryTransLog.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
                int totalRecordCount = 0;
                //设置分页控件
                if (_inventoryTransLogList.Count > 0)
                {
                    totalRecordCount = _inventoryTransLogList[0].RecordCount ?? 0;
                }
                SetBarPagingForInventoryTransLog(totalRecordCount);
            }
        }

        /// <summary>
        /// 获取汽修商户相关信息
        /// </summary>
        /// <param name="paramMerchantCode">汽修商客户编码</param>
        private bool GetARMerchantInfo(string paramMerchantCode)
        {
            if (string.IsNullOrEmpty(paramMerchantCode))
            {
                //汽修商客户ID为空，汽修商户信息获取失败
                MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0016, SystemTableColumnEnums.PIS_AutoFactoryCustomer.Name.AFC_ID, MsgParam.AUTOFACTORY + MsgParam.INFORMATION + MsgParam.GET), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //从缓存中获取汽修商户信息
            List<MDLPIS_AutoFactoryCustomer> resultAutoFactoryCustomerList = CacheDAX.Get(CacheDAX.ConfigDataKey.ARMerchant) as List<MDLPIS_AutoFactoryCustomer>;
            if (resultAutoFactoryCustomerList == null)
            {
                //汽修商户信息获取失败
                MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0002, MsgParam.AUTOFACTORY + MsgParam.INFORMATION + MsgParam.GET), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, MsgParam.VALID + MsgParam.OF + MsgParam.AUTOFACTORY), MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return false;
            }

            #endregion

            return true;
        }

        #endregion
    }
}
