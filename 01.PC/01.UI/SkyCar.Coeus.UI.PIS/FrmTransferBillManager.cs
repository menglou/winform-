using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Infragistics.Win;
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
using ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle;
using ColumnStyle = Infragistics.Win.UltraWinGrid.ColumnStyle;

namespace SkyCar.Coeus.UI.PIS
{
    /// <summary>
    /// 调拨管理
    /// </summary>
    public partial class FrmTransferBillManager : BaseFormCardListDetail<TransferBillManagerUIModel, TransferBillManagerQCModel, MDLPIS_TransferBill>
    {
        #region 全局变量

        /// <summary>
        /// 调拨管理BLL
        /// </summary>
        private TransferBillManagerBLL _bll = new TransferBillManagerBLL();

        #region 下拉框数据源

        /// <summary>
        /// 调入组织数据源
        /// </summary>
        List<MDLSM_Organization> _transferOutOrgDs = new List<MDLSM_Organization>();
        /// <summary>
        /// 调入组织数据源
        /// </summary>
        List<MDLSM_Organization> _transferInOrgDs = new List<MDLSM_Organization>();
        /// <summary>
        /// 调拨单单据类型数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _transferBillTypeDs = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 调拨单调拨类型数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _transferTypeDs = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 调拨单单据状态数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _transferBillStatusDs = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 审核状态数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _approvalStatusDs = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 批量设置调入仓库
        /// </summary>
        List<MDLPIS_Warehouse> _batchWarehouseList = new List<MDLPIS_Warehouse>();

        #endregion

        #region 明细相关数据源

        /// <summary>
        /// 【详情】Tab内Grid绑定用的数据源
        /// </summary>
        private SkyCarBindingList<TransferBillManagerDetailUIModel, MDLPIS_TransferBillDetail> _detailGridDS = new SkyCarBindingList<TransferBillManagerDetailUIModel, MDLPIS_TransferBillDetail>();
        /// <summary>
        /// 调入仓库数据源
        /// </summary>
        List<MDLPIS_Warehouse> _transferInWarehouseDs = new List<MDLPIS_Warehouse>();
        /// <summary>
        /// 调入仓位数据源
        /// </summary>
        List<MDLPIS_WarehouseBin> _transferInWarehouseBinDs = new List<MDLPIS_WarehouseBin>();

        #endregion

        /// <summary>
        /// 添加调拨明细Func
        /// </summary>
        private Func<List<PickPartsQueryUIModel>, bool> _pickPartsDetailFunc;

        /// <summary>
        /// 调拨明细列表数量
        /// </summary>
        private int _gridListCount = 0;
        /// <summary>
        /// 详情页Grid当前选中行
        /// </summary>
        private int _currentRowIndex = -1;

        /// <summary>
        /// 上次选择的调入仓库ID
        /// </summary>
        private string _latestAutoFactoryWarehouseID = null;
        /// <summary>
        /// 上次选择的调入仓位ID
        /// </summary>
        private string _latestAutoFactoryWarehouseBinID = null;
        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmTransferBillManager构造方法
        /// </summary>
        public FrmTransferBillManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmTransferBillManager_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（动作，导航）
            base.ToolBarActionAndNavigate = toolBarActionAndNavigate;
            //基类.工具栏（翻页）
            base.ToolBarPaging = toolBarManagerListTabPaging;
            //查询委托（基类控制翻页用）
            base.ExecuteQuery = QueryAction;
            //工具栏（动作）单击事件
            this.toolBarActionAndNavigate.ToolClick += new ToolClickEventHandler(base.toolBarActionAndNavigate_ToolClick);
            //工具栏（翻页）单击事件
            this.toolBarManagerListTabPaging.ToolClick += new ToolClickEventHandler(base.ToolBarPaging_ToolClick);
            //工具栏（翻页）[当前页]值改变事件
            this.toolBarManagerListTabPaging.ToolValueChanged += new ToolEventHandler(base.ToolBarPaging_ToolValueChanged);
            #region 设置页面大小文本框
            TextBoxTool pageSizeOfListTextBox = null;
            foreach (var loopToolControl in this.toolBarManagerListTabPaging.Tools)
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

            //[列表]页不允许删除
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
            }
            //[打印]不可用
            SetActionEnable(SystemActionEnum.Code.PRINT, false);

            _pickPartsDetailFunc = HandlePickPartsDetail;

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
            decimal totalAmount = 0;
            decimal totalQty = 0;

            foreach (var loopGridRow in gdGrid.Rows)
            {
                if (string.IsNullOrEmpty(loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBill.Code.TB_No].Value?.ToString())
                    || loopGridRow.Cells["IsChecked"].Value == null
                    || (bool)loopGridRow.Cells["IsChecked"].Value == false)
                {
                    continue;
                }
                totalAmount = totalAmount + Convert.ToDecimal(loopGridRow.Cells["TotalAmount"].Value?.ToString());
                totalQty = totalQty + Convert.ToDecimal(loopGridRow.Cells["TotalQty"].Value?.ToString());
            }

            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtAmount"])).Text = Convert.ToString(totalAmount);
            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtQty"])).Text = Convert.ToString(totalQty);
        }
        /// <summary>
        /// 【列表】Grid的AfterHeaderCheckStateChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            decimal totalAmount = 0;
            decimal totalQty = 0;

            foreach (var loopGridRow in gdGrid.Rows)
            {
                if (string.IsNullOrEmpty(loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBill.Code.TB_No].Value?.ToString())
                    || loopGridRow.Cells["IsChecked"].Value == null
                    || (bool)loopGridRow.Cells["IsChecked"].Value == false)
                {
                    continue;
                }
                totalAmount = totalAmount + Convert.ToDecimal(loopGridRow.Cells["TotalAmount"].Value?.ToString());
                totalQty = totalQty + Convert.ToDecimal(loopGridRow.Cells["TotalQty"].Value?.ToString());
            }

            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtAmount"])).Text = Convert.ToString(totalAmount);
            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtQty"])).Text = Convert.ToString(totalQty);
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
        /// 单号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_TB_No_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 单据类型名称ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_TB_TypeName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 【列表】调拨类型名称ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_TB_TransferTypeName_ValueChanged(object sender, EventArgs e)
        {
            if (cbWhere_TB_TransferTypeName.Value == null
                || string.IsNullOrEmpty(cbWhere_TB_TransferTypeName.Text))
            {
                return;
            }
            if (cbWhere_TB_TransferTypeName.Text == TransferTypeEnum.Name.CKZC
                || cbWhere_TB_TransferTypeName.Text == TransferTypeEnum.Name.KWZC)
            {
                #region 调拨类型为[仓库转储]或[库位转储]的场合

                //调入组织与调入组织一致
                cbWhere_TB_TransferInOrgName.Text = cbTB_TransferOutOrgName.Text;
                cbWhere_TB_TransferInOrgName.Value = cbTB_TransferOutOrgName.Value;
                cbWhere_TB_TransferInOrgName.Enabled = false;

                #endregion
            }
            else if (cbWhere_TB_TransferTypeName.Text == TransferTypeEnum.Name.ZZJDB)
            {
                #region 调拨类型为[组织间调拨]的场合

                cbWhere_TB_TransferInOrgName.Enabled = true;
                #endregion
            }
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 调入组织名称ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_TB_TransferOutOrgName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 调入组织名称ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_TB_TransferInOrgName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 单据状态名称ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_TB_StatusName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 审核状态名称ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_TB_ApprovalStatusName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 有效CheckedChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckWhere_TB_IsValid_CheckedChanged(object sender, EventArgs e)
        {
            //执行查询
            QueryAction();
        }
        #endregion

        #region 单头相关事件

        /// <summary>
        /// 【详情】调拨类型ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbTB_TransferTypeName_ValueChanged(object sender, EventArgs e)
        {
            if (cbTB_TransferTypeName.Value == null
                || string.IsNullOrEmpty(cbTB_TransferTypeName.Text))
            {
                return;
            }

            //清空明细Grid
            _detailGridDS = new SkyCarBindingList<TransferBillManagerDetailUIModel, MDLPIS_TransferBillDetail>();
            _detailGridDS.StartMonitChanges();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();

            if (cbTB_TransferTypeName.Text == TransferTypeEnum.Name.ZZJDB)
            {
                #region 调拨类型为[组织间调拨]的场合

                //调入组织数据源不包括调出组织
                var tempTransferInOrgDs = _transferInOrgDs.Where(x => x.Org_ID != cbTB_TransferOutOrgName.Value?.ToString()).ToList();
                cbTB_TransferInOrgName.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
                cbTB_TransferInOrgName.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
                cbTB_TransferInOrgName.DataSource = tempTransferInOrgDs;
                cbTB_TransferInOrgName.DataBind();

                //调入组织可编辑
                cbTB_TransferInOrgName.Enabled = true;

                //[调入仓库]可编辑
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].CellActivation = Activation.AllowEdit;
                //设置[调入仓库]Style为DropDownList
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Style = ColumnStyle.DropDownList;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].ButtonDisplayStyle = ButtonDisplayStyle.Always;

                //[调入仓位]列隐藏
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Hidden = true;

                if (_batchWarehouseList != null)
                {
                    //批量设置调入仓库数据源为调入组织下的所有仓库
                    var tempWarehouseList = _batchWarehouseList.Where(x => x.WH_Org_ID == cbTB_TransferInOrgName.Value?.ToString()).ToList();
                    mcbBatchWH_Name.DataSource = tempWarehouseList;
                }
                //批量设置内容显示
                mcbBatchWH_Name.Clear();
                lblBatch.Visible = true;
                lblBatchWH_Name.Visible = true;
                mcbBatchWH_Name.Visible = true;
                btnBatchSet.Visible = true;

                #endregion
            }
            else
            {
                #region 调拨类型为[仓库转储]或[库位转储]的场合

                //调入组织数据源为调出组织
                var tempTransferInOrgDs = _transferInOrgDs.Where(x => x.Org_ID == cbTB_TransferOutOrgName.Value?.ToString()).ToList();
                cbTB_TransferInOrgName.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
                cbTB_TransferInOrgName.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
                cbTB_TransferInOrgName.DataSource = tempTransferInOrgDs;
                cbTB_TransferInOrgName.DataBind();

                //调入组织与调出组织一致
                cbTB_TransferInOrgName.Text = cbTB_TransferOutOrgName.Text;
                cbTB_TransferInOrgName.Value = cbTB_TransferOutOrgName.Value;
                txtTB_TransferInOrgId.Text = txtTB_TransferInOrgId.Text;

                //调入组织不可编辑
                cbTB_TransferInOrgName.Enabled = false;

                if (cbTB_TransferTypeName.Text == TransferTypeEnum.Name.KWZC)
                {
                    #region 调拨类型为[库位转储]的场合

                    //[调入仓库]不可编辑
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].CellActivation = Activation.ActivateOnly;
                    //设置[调入仓库]Style为Defalut
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Style = ColumnStyle.Default;
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].ButtonDisplayStyle = ButtonDisplayStyle.OnMouseEnter;

                    //[调入仓位]列显示
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Hidden = false;

                    //批量设置内容隐藏
                    mcbBatchWH_Name.Clear();
                    lblBatch.Visible = false;
                    lblBatchWH_Name.Visible = false;
                    mcbBatchWH_Name.Visible = false;
                    btnBatchSet.Visible = false;
                    #endregion
                }
                else
                {
                    #region 调拨类型为[仓库转储]的场合

                    //[调入仓库]可编辑
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].CellActivation = Activation.AllowEdit;
                    //设置[调入仓库]Style为DropDown
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Style = ColumnStyle.DropDownList;
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].ButtonDisplayStyle = ButtonDisplayStyle.Always;

                    //[调入仓位]列隐藏
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Hidden = true;

                    if (_batchWarehouseList != null)
                    {
                        //批量设置调入仓库数据源为调出组织下的所有仓库
                        var tempWarehouseList = _batchWarehouseList.Where(x => x.WH_Org_ID == cbTB_TransferOutOrgName.Value?.ToString()).ToList();
                        mcbBatchWH_Name.DataSource = tempWarehouseList;
                    }
                    //批量设置内容显示
                    mcbBatchWH_Name.Clear();
                    lblBatch.Visible = true;
                    lblBatchWH_Name.Visible = true;
                    mcbBatchWH_Name.Visible = true;
                    btnBatchSet.Visible = true;

                    #endregion
                }
                #endregion
            }
        }

        /// <summary>
        /// 【详情】调入组织ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbTB_TransferInOrgName_ValueChanged(object sender, EventArgs e)
        {
            if (cbTB_TransferInOrgName.Value == null
                || string.IsNullOrEmpty(cbTB_TransferInOrgName.Text))
            {
                return;
            }
            txtTB_TransferInOrgId.Text = cbTB_TransferInOrgName.Value.ToString();

            //加载选择的调入组织下的所有调入仓库
            _transferInWarehouseDs = BLLCom.GetWarehouseList(cbTB_TransferInOrgName.Value.ToString());
            _transferInWarehouseBinDs = BLLCom.GetWarehouseBinList(cbTB_TransferInOrgName.Value.ToString());

            mcbBatchWH_Name.Clear();
            if (_batchWarehouseList != null)
            {
                //批量设置调入仓库数据源为调入组织下的所有仓库
                var tempWarehouseList = _batchWarehouseList.Where(x => x.WH_Org_ID == cbTB_TransferInOrgName.Value?.ToString()).ToList();
                mcbBatchWH_Name.DataSource = tempWarehouseList;
            }
        }
        #endregion

        #region 明细toolBar单击事件

        /// <summary>
        /// 添加/删除 调拨单明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolbarsManagerDetail_ToolClick(object sender, ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                //添加
                case SysConst.EN_ADD:
                    AddTransferDetail();
                    break;

                //删除
                case SysConst.EN_DEL:
                    DeleteTransferDetail();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 明细列表相关事件

        /// <summary>
        /// 调拨单明细gdDetail的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_DoubleClick(object sender, EventArgs e)
        {
            UpdateTransferDetail();
        }

        /// <summary>
        /// 调拨单明细gdDetail的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            UpdateTransferDetail();
        }

        /// <summary>
        /// 调拨单明细单击单元格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_ClickCellButton(object sender, CellEventArgs e)
        {
            //更新明细
            if (e.Cell.Column.Key == SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_Name)
            {
                UpdateTransferDetail();
            }

            gdDetail.UpdateData();
            //设置调拨单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 调拨单明细单元格值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_CellChange(object sender, CellEventArgs e)
        {
            gdDetail.UpdateData();
            var curActiveRow = gdDetail.Rows[e.Cell.Row.Index];

            if (e.Cell.Column.Key == SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_DestUnitPrice
                || e.Cell.Column.Key == SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_Qty)
            {
                string id = curActiveRow.Cells["Tmp_TBD_ID"].Text;
                foreach (var loopDetail in _detailGridDS)
                {
                    if (loopDetail.Tmp_TBD_ID == id)
                    {
                        //明细金额
                        loopDetail.DetailDestAmount = Math.Round((loopDetail.TBD_DestUnitPrice ?? 0) * (loopDetail.TBD_Qty ?? 0), 2);
                        break;
                    }
                }

                #region 计算单头总数量和总金额

                //明细中总数量之和
                decimal tempTotalDetailQty = 0;
                //明细中总金额之和
                decimal tempTotalDetailAmount = 0;

                foreach (var loopDeatil in _detailGridDS)
                {
                    tempTotalDetailQty += (loopDeatil.TBD_Qty ?? 0);
                    tempTotalDetailAmount += Math.Round((loopDeatil.TBD_Qty ?? 0) * (loopDeatil.TBD_DestUnitPrice ?? 0), 2);
                }
                //单头总数量
                txtTotalQty.Text = tempTotalDetailQty.ToString();
                //单头总金额
                txtTotalAmount.Text = tempTotalDetailAmount.ToString();
                #endregion
            }
            //设置调拨单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 调拨单明细单元格下拉框选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_CellListSelect(object sender, CellEventArgs e)
        {
            gdDetail.UpdateData();
            if (gdDetail.Rows.Count > 0)
            {
                //当前选中行行号
                _currentRowIndex = gdDetail.ActiveRow.Index;
                if (_currentRowIndex != -1)
                {
                    if (gdDetail.ActiveCell.Column.Key == SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId)
                    {
                        #region Cell为[调入仓库]

                        //调入仓库ID
                        var transInWarehouseId = gdDetail.Rows[_currentRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value == null
                            ? string.Empty
                            : gdDetail.Rows[_currentRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value.ToString();
                        gdDetail.Rows[_currentRowIndex].Cells["TransInWhName"].Value = gdDetail.Rows[_currentRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Text;
                        //调入仓库选择后，设定仓位的值列表和默认选中值
                        SetCellWarehouseBinAfterByWarehouse(transInWarehouseId, _currentRowIndex);

                        //最新选择的调入仓库ID
                        _latestAutoFactoryWarehouseID = transInWarehouseId;

                        //#region 调入仓位List

                        //_gridListCount += 1;
                        //this.gdDetail.DisplayLayout.ValueLists.Add("List" + _gridListCount);
                        //var transInWarehouseBinList = _transferInWarehouseBinDs.Where(p => p.WHB_WH_ID == transInWarehouseId).ToList();
                        //if (transInWarehouseBinList.Count > 0)
                        //{
                        //    for (int j = 0; j < transInWarehouseBinList.Count; j++)
                        //    {
                        //        gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].ValueListItems.Add(
                        //            transInWarehouseBinList[j].WHB_ID, transInWarehouseBinList[j].WHB_Name);
                        //    }
                        //    gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].DisplayStyle = ValueListDisplayStyle.DisplayText;

                        //    //调入仓位List
                        //    gdDetail.DisplayLayout.Rows[_currentRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].ValueList = gdDetail.DisplayLayout.ValueLists["List" + _gridListCount];

                        //    ////调入仓位ID
                        //    //gdDetail.Rows[_currentRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Value = gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].ValueListItems[0].DataValue;
                        //}
                        //else
                        //{
                        //    gdDetail.DisplayLayout.Rows[_currentRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].ValueList = null;
                        //    gdDetail.Rows[_currentRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Value = null;
                        //}

                        //#endregion

                        #endregion
                    }
                    else if (gdDetail.ActiveCell.Column.Key == SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId)
                    {
                        #region Cell为[调入仓位]

                        //调入仓位ID
                        var transInWarehouseBinId = gdDetail.Rows[_currentRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Value == null
                            ? string.Empty
                            : gdDetail.Rows[_currentRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Value.ToString();

                        //最新选择的调入仓位ID
                        _latestAutoFactoryWarehouseBinID = transInWarehouseBinId;

                        #endregion
                    }
                }
                _currentRowIndex = gdDetail.ActiveRow.Index;
            }
            gdDetail.UpdateData();
            //设置调拨单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 批量设置调入仓库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBatchSet_Click(object sender, EventArgs e)
        {
            foreach (var loopDetail in _detailGridDS)
            {
                loopDetail.TBD_TransInWhId = mcbBatchWH_Name.SelectedValue;
                loopDetail.TransInWhName = mcbBatchWH_Name.SelectedText;
            }
            gdDetail.Refresh();
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

            //控制动作按钮状态
            SetActionEnableByStatus();
            //设置详情是否可编辑
            SetDetailControl();

            //将最新的值Copy到初始UIModel
            SetCardCtrlsToDetailDS();
            this.AcceptUIModelChanges();
        }

        /// <summary>
        /// 保存
        /// </summary>
        public override void SaveAction()
        {
            gdDetail.UpdateData();
            //1.前端检查-保存
            if (!ClientCheckForSave())
            {
                return;
            }

            //2.将【详情】Tab内控件的值赋值给基类的HeadDS
            SetCardCtrlsToDetailDS();

            //3.执行保存（含服务端检查）
            if (!_bll.SaveDetailDS(HeadDS, _detailGridDS))
            {
                //保存失败
                MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //刷新列表
            RefreshList();

            //4.开始监控List变化
            _detailGridDS.StartMonitChanges();
            //5.将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置动作按钮状态
            SetActionEnableByStatus();

            //设置详情是否可编辑
            SetDetailControl();

            //保存成功
            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置调拨单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
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
                DialogResult dialogResult = MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.W_0001), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
            }
            #endregion

            #region 前段检查

            if (txtTB_TransferOutOrgId.Text != LoginInfoDAX.OrgID)
            {
                //调出组织名称为***不能复制
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableColumnEnums.PIS_TransferBill.Name.TB_TransferOutOrgName + MsgParam.BE + cbTB_TransferOutOrgName.Text, SystemActionEnum.Name.COPY }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            #endregion

            base.CopyAction();
            //ID
            txtTB_ID.Clear();
            //版本号
            txtTB_VersionNo.Clear();
            //单号
            txtTB_No.Clear();
            //单据状态
            cbTB_StatusName.Text = TransfeStatusEnum.Name.YSC;
            //审核状态
            cbTB_ApprovalStatusName.Text = ApprovalStatusEnum.Name.DSH;

            SkyCarBindingList<TransferBillManagerDetailUIModel, MDLPIS_TransferBillDetail> detailGridDS = new SkyCarBindingList<TransferBillManagerDetailUIModel, MDLPIS_TransferBillDetail>();
            _bll.CopyModelList(_detailGridDS, detailGridDS);
            _detailGridDS = new SkyCarBindingList<TransferBillManagerDetailUIModel, MDLPIS_TransferBillDetail>();
            _detailGridDS.StartMonitChanges();
            _bll.CopyModelList(detailGridDS, _detailGridDS);
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            for (int i = 0; i < gdDetail.DisplayLayout.Rows.Count; i++)
            {
                var detailRow = gdDetail.DisplayLayout.Rows[i];

                //调入仓库ID为空
                if (detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value == null)
                {
                    continue;
                }

                #region 加载调入仓库

                _gridListCount += 1;
                gdDetail.DisplayLayout.ValueLists.Add("List" + _gridListCount);
                //调入组织下的仓库列表
                if (_transferInWarehouseDs.Count > 0)
                {
                    //该明细调入仓库在仓库列表的位置
                    int? detailWareHouseIndex = null;
                    for (int j = 0; j < _transferInWarehouseDs.Count; j++)
                    {
                        gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].ValueListItems.Add(_transferInWarehouseDs[j].WH_ID, _transferInWarehouseDs[j].WH_Name);

                        if (_transferInWarehouseDs[j].WH_ID == detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value.ToString())
                        {
                            detailWareHouseIndex = j;
                        }
                    }
                    gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].DisplayStyle = ValueListDisplayStyle.DisplayText;
                    detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].ValueList = gdDetail.DisplayLayout.ValueLists["List" + _gridListCount];

                    if (detailWareHouseIndex != null)
                    {
                        detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value = gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].ValueListItems[(int)detailWareHouseIndex].DataValue;
                    }
                }
                else
                {
                    detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].ValueList = null;
                    detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value = null;
                }
                #endregion

                #region 加载调入仓位

                //调入仓位ID为空
                if (detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Value == null)
                {
                    continue;
                }

                //调入仓库ID
                var transInWarehouseId = detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value == null
                    ? string.Empty
                    : detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value.ToString();

                _gridListCount += 1;
                gdDetail.DisplayLayout.ValueLists.Add("List" + _gridListCount);

                //该明细调入仓库下的仓位列表
                var transInWarehouseBinList = _transferInWarehouseBinDs.Where(p => p.WHB_WH_ID == transInWarehouseId).ToList();
                if (transInWarehouseBinList.Count > 0)
                {
                    //该明细调入仓位在仓位列表的位置
                    int? detailWarehouseBinIndex = null;
                    for (int j = 0; j < transInWarehouseBinList.Count; j++)
                    {
                        gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].ValueListItems.Add(transInWarehouseBinList[j].WHB_ID, transInWarehouseBinList[j].WHB_Name);
                        if (transInWarehouseBinList[j].WHB_ID == detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Value.ToString())
                        {
                            detailWarehouseBinIndex = j;
                        }
                    }
                    gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].DisplayStyle = ValueListDisplayStyle.DisplayText;
                    detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].ValueList = gdDetail.DisplayLayout.ValueLists["List" + _gridListCount];

                    if (detailWarehouseBinIndex != null)
                    {
                        detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Value = gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].ValueListItems[(int)detailWarehouseBinIndex].DataValue;
                    }
                }
                else
                {
                    detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].ValueList = null;
                    detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Value = null;
                }

                #endregion
            }
            //设置动作按钮状态
            SetActionEnableByStatus();
            //设置详情是否可编辑
            SetDetailControl();

            //5.将HeadDS数据赋值给【详情】Tab内的对应控件
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
            var argsDetail = new List<MDLPIS_TransferBillDetail>();
            //将HeadDS转换为TBModel对象
            var argsHead = HeadDS.ToTBModelForSaveAndDelete<MDLPIS_TransferBill>();

            //从数据库查询最新的明细
            _bll.QueryForList<MDLPIS_TransferBillDetail, MDLPIS_TransferBillDetail>(new MDLPIS_TransferBillDetail { WHERE_TBD_TB_No = argsHead.TB_No }, argsDetail);

            //Todo:两步调拨需要考虑明细的签收数量
            foreach (var loopTransferDetail in argsDetail)
            {
                loopTransferDetail.WHERE_TBD_ID = loopTransferDetail.TBD_ID;
                loopTransferDetail.WHERE_TBD_VersionNo = loopTransferDetail.TBD_VersionNo;
                loopTransferDetail.TBD_VersionNo += 1;
            }
            if (argsDetail.Count > 0)
            {
                //2.执行删除
                bool deleteResult = _bll.UnityDelete<MDLPIS_TransferBill, MDLPIS_TransferBillDetail>(argsHead, argsDetail);
                if (!deleteResult)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                //2.执行删除
                bool deleteResult = _bll.Delete<MDLPIS_TransferBill>(argsHead);
                if (!deleteResult)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //删除成功
            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //3.清空【详情】画面数据
            InitializeDetailTabControls();
            //刷新列表
            RefreshList(true);

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();
        }

        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            //2.设置查询条件（翻页相关属性不用设置）
            base.ConditionDS = new TransferBillManagerQCModel()
            {
                //查询用SqlId 
                SqlId = SQLID.PIS_TransferBillManager_SQL_01,
                //单号
                WHERE_TB_No = txtWhere_TB_No.Text.Trim(),
                //单据类型名称
                WHERE_TB_TypeName = cbWhere_TB_TypeName.Text,
                //调拨类型名称
                WHERE_TB_TransferTypeName = cbWhere_TB_TransferTypeName.Text,
                //调出组织名称
                WHERE_TB_TransferOutOrgName = cbWhere_TB_TransferOutOrgName.Text,
                //调入组织名称
                WHERE_TB_TransferInOrgName = cbWhere_TB_TransferInOrgName.Text,
                //单据状态名称
                WHERE_TB_StatusName = cbWhere_TB_StatusName.Text,
                //审核状态名称
                WHERE_TB_ApprovalStatusName = cbWhere_TB_ApprovalStatusName.Text,
                //有效
                WHERE_TB_IsValid = ckWhere_TB_IsValid.Checked,
            };
            //3.执行基类方法（注意：基类使用模糊查询）
            base.QueryAction();
            //4.Grid绑定数据源
            gdGrid.DataSource = base.HeadGridDS;
            gdGrid.DataBind();
            //5.设置Grid自适应列宽（根据单元格内容）
            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            //6.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
        }

        /// <summary>
        /// 清空查询条件
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
            gdDetail.UpdateData();

            #region 验证数据
            //调拨单未保存，不能审核
            if (string.IsNullOrEmpty(txtTB_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_TransferBill + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MDLPIS_TransferBill resultTransferBill = new MDLPIS_TransferBill();
            _bll.QueryForObject<MDLPIS_TransferBill, MDLPIS_TransferBill>(new MDLPIS_TransferBill()
            {
                WHERE_TB_ID = txtTB_ID.Text.Trim(),
                WHERE_TB_IsValid = true
            }, resultTransferBill);
            //调拨单不存在，不能审核
            if (string.IsNullOrEmpty(resultTransferBill.TB_ID))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_TransferBill + MsgParam.NOTEXIST, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //确认审核操作
            DialogResult isApprove = MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0014), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isApprove != DialogResult.OK)
            {
                return;
            }
            #endregion

            base.ApproveAction();

            //将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();

            #region 审核调拨单

            bool saveApproveResult = _bll.ApproveDetailDS(HeadDS, _detailGridDS);
            if (!saveApproveResult)
            {
                //审核失败
                MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //审核成功
            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.APPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            #endregion

            //刷新列表
            RefreshList();

            //开始监控List变化
            _detailGridDS.StartMonitChanges();
            //将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置动作按钮状态
            SetActionEnableByStatus();
            //设置详情是否可编辑
            SetDetailControl();
        }

        /// <summary>
        /// 反审核
        /// </summary>
        public override void UnApproveAction()
        {
            #region 验证

            //调拨单未保存,不能反审核
            if (string.IsNullOrEmpty(txtTB_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_TransferBill + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MDLPIS_TransferBill resultTransferBill = new MDLPIS_TransferBill();
            _bll.QueryForObject<MDLPIS_TransferBill, MDLPIS_TransferBill>(new MDLPIS_TransferBill()
            {
                WHERE_TB_IsValid = true,
                WHERE_TB_ID = txtTB_ID.Text.Trim()
            }, resultTransferBill);
            //调拨单不存在,不能反审核
            if (string.IsNullOrEmpty(resultTransferBill.TB_ID))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_TransferBill + MsgParam.NOTEXIST, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion

            //确认反审核操作
            DialogResult isConfirmUnApprove = MessageBoxs.Show(Trans.PIS, this.ToString(),
                MsgHelp.GetMsg(MsgCode.W_0018),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isConfirmUnApprove != DialogResult.OK)
            {
                return;
            }

            SetCardCtrlsToDetailDS();

            bool saveUnApprove = _bll.UnApproveDetailDS(HeadDS, _detailGridDS);
            //反审核失败
            if (!saveUnApprove)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //反审核成功
            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.UNAPPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            //设置详情是否可编辑
            SetDetailControl();
        }

        /// <summary>
        /// 导出当前页
        /// </summary>
        /// <param name="paramGrid"></param>
        /// <param name="paramGridName"></param>
        public override void ExportAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = SystemTableEnums.Name.PIS_TransferBill;
            base.ExportAction(gdGrid, paramGridName);
        }

        /// <summary>
        /// 导出所有
        /// </summary>
        /// <param name="paramGrid"></param>
        /// <param name="paramGridName"></param>
        public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = SystemTableEnums.Name.PIS_TransferBill;
            List<TransferBillManagerUIModel> resultAllList = new List<TransferBillManagerUIModel>();
            TransferBillManagerQCModel ConditionDS = new TransferBillManagerQCModel()
            {
                PageIndex = 1,
                PageSize = null,
                //单号
                WHERE_TB_No = txtWhere_TB_No.Text.Trim(),
                //单据类型名称
                WHERE_TB_TypeName = cbWhere_TB_TypeName.Text,
                //调拨类型名称
                WHERE_TB_TransferTypeName = cbWhere_TB_TransferTypeName.Text,
                //调出组织名称
                WHERE_TB_TransferOutOrgName = cbWhere_TB_TransferOutOrgName.Text,
                //调入组织名称
                WHERE_TB_TransferInOrgName = cbWhere_TB_TransferInOrgName.Text,
                //单据状态名称
                WHERE_TB_StatusName = cbWhere_TB_StatusName.Text,
                //审核状态名称
                WHERE_TB_ApprovalStatusName = cbWhere_TB_ApprovalStatusName.Text,
                //有效
                WHERE_TB_IsValid = ckWhere_TB_IsValid.Checked,
            };
            _bll.QueryForList<TransferBillManagerUIModel>(SQLID.PIS_TransferBillManager_SQL_01, ConditionDS, resultAllList);

            UltraGrid allGrid = gdGrid;
            allGrid.DataSource = resultAllList;
            allGrid.DataBind();
            base.ExportAllAction(allGrid, paramGridName);

            gdGrid.DataSource = base.HeadGridDS;
            gdGrid.DataBind();

        }

        /// <summary>
        /// 打印
        /// </summary>
        public override void PrintAction()
        {
            try
            {
                if (HeadDS == null || string.IsNullOrWhiteSpace(HeadDS.TB_No))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, SystemTableEnums.Name.PIS_TransferBill), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //调拨单待审核，不能打印
                if (cbTB_ApprovalStatusName.Text != ApprovalStatusEnum.Name.YSH)
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, SystemTableEnums.Name.PIS_TransferBill + cbTB_ApprovalStatusName.Text, SystemActionEnum.Name.PRINT), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //待打印的调拨单
                TransferBillUIModelToPrint transferBillToPrint = new TransferBillUIModelToPrint();
                _bll.CopyModel(HeadDS, transferBillToPrint);
                //待打印的调拨单明细
                List<TransferBillDetailUIModelToPrint> transferBillDetailToPrintList = new List<TransferBillDetailUIModelToPrint>();
                _bll.CopyModelList(_detailGridDS, transferBillDetailToPrintList);

                foreach (var loopDetail in transferBillDetailToPrintList)
                {
                    loopDetail.TBD_Qty = Math.Round((loopDetail.TBD_Qty ?? 0), 0);
                }
                transferBillToPrint.TotalQty = Math.Round(transferBillToPrint.TotalQty, 0);

                Dictionary<string, object> argsViewParams = new Dictionary<string, object>
                {
                    //调拨单
                    {PISViewParamKey.TransferBill.ToString(), transferBillToPrint},
                    //调拨单明细
                    {PISViewParamKey.TransferBillDetail.ToString(), transferBillDetailToPrintList},
                };

                FrmViewAndPrintTransferBill frmViewAndPrintTransferBill = new FrmViewAndPrintTransferBill(argsViewParams)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = frmViewAndPrintTransferBill.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), ex.Message.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// 签收
        /// </summary>
        public override void SignInAction()
        {
            //TODO 签收调拨单
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 转物流
        /// </summary>
        public override void ToLogisticsBillNavigate()
        {
            base.ToLogisticsBillNavigate();

            #region 验证及准备数据

            //要跳转到物流的调拨订单
            TransferBillManagerUIModel curTransferBillToLogistics = new TransferBillManagerUIModel();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                //选中【调拨】Tab的场合
                _bll.CopyModel(HeadDS, curTransferBillToLogistics);
            }
            else if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                //选中【调拨列表】Tab的场合
                var selectedSalesOrderList = HeadGridDS.Where(x => x.IsChecked == true).ToList();
                if (selectedSalesOrderList.Count == 1)
                {
                    if (selectedSalesOrderList[0].TB_TransferTypeName != TransferTypeEnum.Name.ZZJDB)
                    {
                        //请选择组织间的调拨订单转物流
                        MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { TransferTypeEnum.Name.ZZJDB + MsgParam.OF + SystemTableEnums.Name.PIS_TransferBill + SystemNavigateEnum.Name.TOLOGISTICSBILL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        selectedSalesOrderList[0].IsChecked = false;
                        gdGrid.DataSource = HeadGridDS;
                        gdGrid.DataBind();
                        return;
                    }
                    if (selectedSalesOrderList[0].TB_ApprovalStatusName != ApprovalStatusEnum.Name.YSH
                    || selectedSalesOrderList[0].TB_StatusName != TransfeStatusEnum.Name.DFH)
                    {
                        //请选择待发货的调拨订单转物流
                        MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { TransfeStatusEnum.Name.DFH + MsgParam.OF + SystemTableEnums.Name.PIS_TransferBill + SystemNavigateEnum.Name.TOLOGISTICSBILL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        selectedSalesOrderList[0].IsChecked = false;
                        gdGrid.DataSource = HeadGridDS;
                        gdGrid.DataBind();
                        return;
                    }
                    curTransferBillToLogistics = selectedSalesOrderList[0];
                }
                else
                {
                    var tempCannotToLogistic =
                        selectedSalesOrderList.Where(x => x.TB_StatusName != TransfeStatusEnum.Name.YWC).ToList();
                    //请选择一个待发货的调拨订单转物流
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ONE + SalesOrderStatusEnum.Name.DFH + MsgParam.OF + SystemTableEnums.Name.PIS_TransferBill + SystemNavigateEnum.Name.TOLOGISTICSBILL }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    foreach (var loopCannotToLogistic in tempCannotToLogistic)
                    {
                        loopCannotToLogistic.IsChecked = false;
                        gdGrid.DataSource = HeadGridDS;
                        gdGrid.DataBind();
                        return;
                    }
                    return;
                }

                ////查询调拨订单明细
                //_bll.QueryForList(SQLID.SD_SalesOrder_SQL01, new SalesOrderManagerQCModel
                //{
                //    WHERE_SOD_SO_ID = curTransferBillToLogistics.SO_ID
                //}, _detailGridDS);
            }

            if (string.IsNullOrEmpty(curTransferBillToLogistics.TB_ID)
                || string.IsNullOrEmpty(curTransferBillToLogistics.TB_No))
            {
                //没有获取到调拨订单，转物流失败
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.PIS_TransferBill, SystemNavigateEnum.Name.TOLOGISTICSBILL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //查询该调拨订单是否已存在物流单
            var logisticsBillCount = _bll.QueryForCount<int>(new MDLSD_LogisticsBill
            {
                WHERE_LB_SourceNo = curTransferBillToLogistics.TB_No,
                WHERE_LB_IsValid = true
            });
            if (logisticsBillCount > 0)
            {
                //curTransferBillToLogistics.SO_No对应的物流订单已存在，不能重复添加
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0006, new object[] { curTransferBillToLogistics.TB_No + MsgParam.CORRESPONDENCE + MsgParam.OF + SystemTableEnums.Name.SD_LogisticsBill }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion

            //传入的物流单
            MDLSD_LogisticsBill argsLogisticsBill = new MDLSD_LogisticsBill();
            //传入的物流单明细
            List<MDLSD_LogisticsBillDetail> argsLogisticsBillDetailList = new List<MDLSD_LogisticsBillDetail>();

            #region 准备[物流单]数据

            argsLogisticsBill.LB_Org_ID = LoginInfoDAX.OrgID;
            argsLogisticsBill.LB_Org_Name = LoginInfoDAX.OrgShortName;
            argsLogisticsBill.LB_SourceTypeName = DeliveryBillSourceTypeEnum.Name.ZZDB;
            argsLogisticsBill.LB_SourceTypeCode = DeliveryBillSourceTypeEnum.Code.ZZDB;
            argsLogisticsBill.LB_SourceNo = curTransferBillToLogistics.TB_No;
            //默认物流单状态为{已生成}
            argsLogisticsBill.LB_StatusName = LogisticsBillStatusEnum.Name.YSC;
            argsLogisticsBill.LB_StatusCode = LogisticsBillStatusEnum.Code.YSC;
            //默认审核状态为{待审核}
            argsLogisticsBill.LB_ApprovalStatusName = ApprovalStatusEnum.Name.DSH;
            argsLogisticsBill.LB_ApprovalStatusCode = ApprovalStatusEnum.Code.DSH;
            //默认物流费付款状态为{未支付}
            argsLogisticsBill.LB_PayStautsName = PaymentStatusEnum.Name.WZF;
            argsLogisticsBill.LB_PayStautsCode = PaymentStatusEnum.Code.WZF;
            //默认物流人员接单时间为当前时间
            argsLogisticsBill.LB_AcceptTime = BLLCom.GetCurStdDatetime();
            argsLogisticsBill.LB_IsValid = true;
            argsLogisticsBill.LB_CreatedBy = LoginInfoDAX.UserName;
            argsLogisticsBill.LB_UpdatedBy = LoginInfoDAX.UserName;
            argsLogisticsBill.LB_CreatedTime = BLLCom.GetCurStdDatetime();
            argsLogisticsBill.LB_UpdatedTime = BLLCom.GetCurStdDatetime();
            argsLogisticsBill.LB_VersionNo = 1;
            //收件人
            argsLogisticsBill.LB_Receiver = curTransferBillToLogistics.TB_TransferInOrgName;
            //获取客户信息
            List<MDLSM_Organization> resultOrganizationList = new List<MDLSM_Organization>();
            _bll.QueryForList(new MDLSM_Organization()
            {
                WHERE_Org_ID = curTransferBillToLogistics.TB_TransferInOrgId,
            }, resultOrganizationList);
            if (resultOrganizationList.Count == 1)
            {
                //收件人手机号
                argsLogisticsBill.LB_ReceiverPhoneNo = resultOrganizationList[0].Org_PhoneNo;
                //收件人地址
                argsLogisticsBill.LB_ReceiverAddress = resultOrganizationList[0].Org_Addr;
            }
            #endregion

            #region 准备[物流单明细]数据

            decimal amount = 0;
            foreach (var loopSalesOrderDetail in _detailGridDS)
            {
                if (string.IsNullOrEmpty(loopSalesOrderDetail.TBD_ID))
                {
                    continue;
                }
                MDLSD_LogisticsBillDetail addLogisticsBillDetail = new MDLSD_LogisticsBillDetail
                {
                    LBD_Barcode = loopSalesOrderDetail.TBD_Barcode,
                    LBD_BatchNoNew = loopSalesOrderDetail.TBD_TransInBatchNo,
                    LBD_Name = loopSalesOrderDetail.TBD_Name,
                    LBD_Specification = loopSalesOrderDetail.TBD_Specification,
                    LBD_UOM = loopSalesOrderDetail.TBD_UOM,
                    LBD_DeliveryQty = loopSalesOrderDetail.TBD_Qty,
                    LBD_AccountReceivableAmount = (loopSalesOrderDetail.TBD_Qty ?? 0) * (loopSalesOrderDetail.TBD_DestUnitPrice ?? 0),
                    //默认物流明细为{已生成}
                    LBD_StatusName = LogisticsBillDetailStatusEnum.Name.YSC,
                    LBD_StatusCode = LogisticsBillDetailStatusEnum.Code.YSC,
                    LBD_IsValid = true,
                    LBD_CreatedBy = LoginInfoDAX.UserName,
                    LBD_UpdatedBy = LoginInfoDAX.UserName,
                    LBD_CreatedTime = BLLCom.GetCurStdDatetime(),
                    LBD_UpdatedTime = BLLCom.GetCurStdDatetime(),
                    LBD_VersionNo = 1
                };
                amount = amount + (addLogisticsBillDetail.LBD_AccountReceivableAmount ?? 0);
                argsLogisticsBillDetailList.Add(addLogisticsBillDetail);
            }
            argsLogisticsBill.LB_AccountReceivableAmount = amount;
            #endregion

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //物流单
                {SDViewParamKey.LogisticsBill.ToString(), argsLogisticsBill},
                //物流单明细
                {SDViewParamKey.LogisticsBillDetail.ToString(), argsLogisticsBillDetailList},
            };

            //跳转到[物流单管理]
            SystemFunction.ShowViewFromView(MsgParam.LOGISTICSBILL_MANAGER, ViewClassFullNameConst.SD_FrmLogisticsBillManager, true, PageDisplayMode.TabPage, paramViewParameters, null);
        }

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 工具生成
            //单号
            txtTB_No.Clear();
            //单据类型
            cbTB_TypeName.Items.Clear();
            //调拨类型
            cbTB_TransferTypeName.Items.Clear();
            //调入组织
            cbTB_TransferOutOrgName.Items.Clear();
            //调入组织
            cbTB_TransferInOrgName.Items.Clear();
            //单据状态
            cbTB_StatusName.Items.Clear();
            //审核状态
            cbTB_ApprovalStatusName.Items.Clear();
            //备注
            txtTB_Remark.Clear();
            //有效
            ckTB_IsValid.Checked = true;
            ckTB_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtTB_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtTB_CreatedTime.Value = DateTime.Now;
            //修改人
            txtTB_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtTB_UpdatedTime.Value = DateTime.Now;
            //调拨单ID
            txtTB_ID.Clear();
            //调入组织ID
            txtTB_TransferOutOrgId.Clear();
            //调入组织ID
            txtTB_TransferInOrgId.Clear();
            //版本号
            txtTB_VersionNo.Clear();
            //总数量
            txtTotalQty.Clear();
            //总金额
            txtTotalAmount.Clear();
            //给 单号 设置焦点
            lblTB_No.Focus();
            //清空[明细]Grid
            _detailGridDS = new SkyCarBindingList<TransferBillManagerDetailUIModel, MDLPIS_TransferBillDetail>();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();

            //开始监控List变化（该数据源若有其他调整，请在StartMonitChanges()方法执行前调整）
            _detailGridDS.StartMonitChanges();
            #endregion

            #region 初始化下拉框

            #region 调入组织

            //调入组织
            _transferInOrgDs = LoginInfoDAX.OrgList;
            cbTB_TransferInOrgName.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
            cbTB_TransferInOrgName.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
            cbTB_TransferInOrgName.DataSource = _transferInOrgDs;
            cbTB_TransferInOrgName.DataBind();
            #endregion

            #region 调出组织

            //调出组织
            _transferOutOrgDs = _transferInOrgDs.Where(x => x.Org_ID == LoginInfoDAX.OrgID).ToList();
            cbTB_TransferOutOrgName.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
            cbTB_TransferOutOrgName.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
            cbTB_TransferOutOrgName.DataSource = _transferOutOrgDs;
            cbTB_TransferOutOrgName.DataBind();
            //默认为[当前组织]
            cbTB_TransferOutOrgName.Text = LoginInfoDAX.OrgShortName;
            txtTB_TransferOutOrgId.Text = LoginInfoDAX.OrgID;
            #endregion

            #region 单据类型

            //单据类型
            _transferBillTypeDs = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.TransferBillType);
            var tempTransferBillTypeList = _transferBillTypeDs.Where(x => x.Text == TransferBillTypeEnum.Name.YBS).ToList();
            cbTB_TypeName.DisplayMember = SysConst.EN_TEXT;
            cbTB_TypeName.ValueMember = SysConst.EN_Code;
            cbTB_TypeName.DataSource = tempTransferBillTypeList;
            cbTB_TypeName.DataBind();
            //默认单据类型为[一步式调拨]
            cbTB_TypeName.Text = TransferBillTypeEnum.Name.YBS;
            cbTB_TypeName.Value = TransferBillTypeEnum.Code.YBS;
            //暂定只能[一步式调拨]
            cbTB_TypeName.Enabled = false;
            #endregion

            #region 调拨类型

            //调拨类型
            _transferTypeDs = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.TransferType);
            cbTB_TransferTypeName.DisplayMember = SysConst.EN_TEXT;
            cbTB_TransferTypeName.ValueMember = SysConst.EN_Code;
            cbTB_TransferTypeName.DataSource = _transferTypeDs;
            cbTB_TransferTypeName.DataBind();
            //默认调拨类型为[组织间调拨]
            cbTB_TransferTypeName.Text = TransferTypeEnum.Name.ZZJDB;
            cbTB_TransferTypeName.Value = TransferTypeEnum.Code.ZZJDB;
            #endregion

            #region 单据状态

            //单据状态
            _transferBillStatusDs = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.TransfeStatus);
            cbTB_StatusName.DisplayMember = SysConst.EN_TEXT;
            cbTB_StatusName.ValueMember = SysConst.EN_Code;
            cbTB_StatusName.DataSource = _transferBillStatusDs;
            cbTB_StatusName.DataBind();
            //默认单据状态为[已生成]
            cbTB_StatusName.Text = TransfeStatusEnum.Name.YSC;
            cbTB_StatusName.Value = TransfeStatusEnum.Code.YSC;
            #endregion

            #region 审核状态

            //审核状态
            _approvalStatusDs = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.ApprovalStatus);
            cbTB_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
            cbTB_ApprovalStatusName.ValueMember = SysConst.EN_Code;
            cbTB_ApprovalStatusName.DataSource = _approvalStatusDs;
            cbTB_ApprovalStatusName.DataBind();
            //默认审核状态为[待审核]
            cbTB_ApprovalStatusName.Text = ApprovalStatusEnum.Name.DSH;
            cbTB_ApprovalStatusName.Value = ApprovalStatusEnum.Code.DSH;
            #endregion

            #region 批量设置调入仓库

            //调入仓库
            _batchWarehouseList = CacheDAX.Get(CacheDAX.ConfigDataKey.Warehouse) as List<MDLPIS_Warehouse>;
            mcbBatchWH_Name.DisplayMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name;
            mcbBatchWH_Name.ValueMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_ID;
            if (_batchWarehouseList != null)
            {
                if (cbTB_TransferTypeName.Text == TransferTypeEnum.Name.ZZJDB)
                {
                    //[组织间调拨]的场合，批量设置调入仓库数据源为调入组织下的所有仓库
                    var tempWarehouseList = _batchWarehouseList.Where(x => x.WH_Org_ID == cbTB_TransferInOrgName.Value?.ToString()).ToList();
                    mcbBatchWH_Name.DataSource = tempWarehouseList;
                }
                else if (cbTB_TransferTypeName.Text == TransferTypeEnum.Name.CKZC)
                {
                    //[仓库转储]的场合，批量设置调入仓库数据源为调出组织下的所有仓库
                    var tempWarehouseList = _batchWarehouseList.Where(x => x.WH_Org_ID == cbTB_TransferOutOrgName.Value?.ToString()).ToList();
                    mcbBatchWH_Name.DataSource = tempWarehouseList;
                }
            }

            #endregion

            #endregion
        }
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 查询条件初始化
            //单号
            txtWhere_TB_No.Clear();
            //单据类型名称
            cbWhere_TB_TypeName.Items.Clear();
            //调拨类型名称
            cbWhere_TB_TransferTypeName.Items.Clear();
            //调入组织名称
            cbWhere_TB_TransferOutOrgName.Items.Clear();
            //调入组织名称
            cbWhere_TB_TransferInOrgName.Items.Clear();
            //单据状态名称
            cbWhere_TB_StatusName.Items.Clear();
            //审核状态名称
            cbWhere_TB_ApprovalStatusName.Items.Clear();
            //有效
            ckWhere_TB_IsValid.Checked = true;
            ckWhere_TB_IsValid.CheckState = CheckState.Checked;
            //给 单号 设置焦点
            lblWhere_TB_No.Focus();
            #endregion

            #region Grid初始化

            //清空Grid
            HeadGridDS = new BindingList<TransferBillManagerUIModel>();
            gdGrid.DataSource = HeadGridDS;
            gdGrid.DataBind();

            #endregion

            #region 初始化下拉框

            //调出组织
            var contidionTransferOutOrgDs = LoginInfoDAX.OrgList;
            cbWhere_TB_TransferOutOrgName.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
            cbWhere_TB_TransferOutOrgName.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
            cbWhere_TB_TransferOutOrgName.DataSource = contidionTransferOutOrgDs;
            cbWhere_TB_TransferOutOrgName.DataBind();

            //调入组织
            cbWhere_TB_TransferInOrgName.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
            cbWhere_TB_TransferInOrgName.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
            cbWhere_TB_TransferInOrgName.DataSource = _transferInOrgDs;
            cbWhere_TB_TransferInOrgName.DataBind();

            //单据类型
            cbWhere_TB_TypeName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_TB_TypeName.ValueMember = SysConst.EN_Code;
            cbWhere_TB_TypeName.DataSource = _transferBillTypeDs;
            cbWhere_TB_TypeName.DataBind();

            //调拨类型
            cbWhere_TB_TransferTypeName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_TB_TransferTypeName.ValueMember = SysConst.EN_Code;
            cbWhere_TB_TransferTypeName.DataSource = _transferTypeDs;
            cbWhere_TB_TransferTypeName.DataBind();

            //单据状态
            cbWhere_TB_StatusName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_TB_StatusName.ValueMember = SysConst.EN_Code;
            cbWhere_TB_StatusName.DataSource = _transferBillStatusDs;
            cbWhere_TB_StatusName.DataBind();

            //审核状态
            cbWhere_TB_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_TB_ApprovalStatusName.ValueMember = SysConst.EN_Code;
            cbWhere_TB_ApprovalStatusName.DataSource = _approvalStatusDs;
            cbWhere_TB_ApprovalStatusName.DataBind();
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
            base.NewUIModel = HeadDS;

            var activeRowIndex = gdGrid.ActiveRow.Index;
            //判断Grid内[唯一标识]是否为空
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBill.Code.TB_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBill.Code.TB_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[HeadDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            HeadDS = base.HeadGridDS.FirstOrDefault(x => x.TB_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBill.Code.TB_ID].Value);

            if (HeadDS == null || string.IsNullOrEmpty(HeadDS.TB_ID))
            {
                return;
            }

            //设置调出组织和调入组织数据源
            _transferOutOrgDs = LoginInfoDAX.OrgList;
            cbTB_TransferOutOrgName.DataSource = _transferOutOrgDs;
            cbTB_TransferOutOrgName.DataBind();
            _transferInOrgDs = LoginInfoDAX.OrgList;
            cbTB_TransferInOrgName.DataSource = _transferInOrgDs;
            cbTB_TransferInOrgName.DataBind();

            if (txtTB_ID.Text != HeadDS.TB_ID
                || (txtTB_ID.Text == HeadDS.TB_ID && txtTB_VersionNo.Text != HeadDS.TB_VersionNo?.ToString()))
            {
                if (txtTB_ID.Text == HeadDS.TB_ID && txtTB_VersionNo.Text != HeadDS.TB_VersionNo?.ToString())
                {
                    //数据版本已过期，将加载最新详情。
                    MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.DataHasOverdue }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else if (ViewHasChanged()
                || _detailGridDS.InsertList.Count > 0
                || _detailGridDS.UpdateList.Count > 0
                || _detailGridDS.DeleteList.Count > 0)
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

            //查询明细Grid数据并绑定
            QueryDetail();

            //设置动作按钮状态
            SetActionEnableByStatus();
            //设置详情是否可编辑
            SetDetailControl();
        }
        
        /// <summary>
        /// 将HeadDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //单号
            txtTB_No.Text = HeadDS.TB_No;
            //单据类型
            cbTB_TypeName.Text = HeadDS.TB_TypeName ?? "";
            //单据类型编码
            cbTB_TypeName.Value = HeadDS.TB_TypeCode;
            //调拨类型
            cbTB_TransferTypeName.Text = HeadDS.TB_TransferTypeName ?? "";
            //调拨类型编码
            cbTB_TransferTypeName.Value = HeadDS.TB_TransferTypeCode;
            //调入组织
            cbTB_TransferOutOrgName.Text = HeadDS.TB_TransferOutOrgName ?? "";
            //调入组织
            cbTB_TransferInOrgName.Text = HeadDS.TB_TransferInOrgName ?? "";
            //单据状态
            cbTB_StatusName.Text = HeadDS.TB_StatusName ?? "";
            //单据状态编码
            cbTB_StatusName.Value = HeadDS.TB_StatusCode;
            //审核状态
            cbTB_ApprovalStatusName.Text = HeadDS.TB_ApprovalStatusName ?? "";
            //审核状态编码
            cbTB_ApprovalStatusName.Value = HeadDS.TB_ApprovalStatusCode;
            //备注
            txtTB_Remark.Text = HeadDS.TB_Remark;
            //有效
            if (HeadDS.TB_IsValid != null)
            {
                ckTB_IsValid.Checked = HeadDS.TB_IsValid.Value;
            }
            //创建人
            txtTB_CreatedBy.Text = HeadDS.TB_CreatedBy;
            //创建时间
            dtTB_CreatedTime.Value = HeadDS.TB_CreatedTime;
            //修改人
            txtTB_UpdatedBy.Text = HeadDS.TB_UpdatedBy;
            //修改时间
            dtTB_UpdatedTime.Value = HeadDS.TB_UpdatedTime;
            //调拨单ID
            txtTB_ID.Text = HeadDS.TB_ID;
            //调入组织ID
            txtTB_TransferOutOrgId.Text = HeadDS.TB_TransferOutOrgId;
            //调入组织ID
            txtTB_TransferInOrgId.Text = HeadDS.TB_TransferInOrgId;
            //版本号
            txtTB_VersionNo.Value = HeadDS.TB_VersionNo;
            //总数量
            txtTotalQty.Text = HeadDS.TotalQty.ToString();
            //总金额
            txtTotalAmount.Text = HeadDS.TotalAmount.ToString();
        }
        
        /// <summary>
        /// 查询明细Grid数据并绑定
        /// </summary>
        private void QueryDetail()
        {
            //1.设置查询条件
            var argsCondition = new TransferBillManagerQCModel()
            {
                //查询用SqlId
                SqlId = SQLID.PIS_TransferBillManager_SQL_02,
                //调拨单ID
                WHERE_TBD_TB_ID = txtTB_ID.Text.Trim(),
                //调拨单号
                WHERE_TBD_TB_No = txtTB_No.Text.Trim(),
            };
            //2.执行查询方法
            _bll.QueryForList(argsCondition.SqlId, argsCondition, _detailGridDS);
            //3.开始监控List变化（该数据源若有其他调整，请在StartMonitChanges()方法执行前调整）
            _detailGridDS.StartMonitChanges();
            //4.Grid绑定数据源
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //5.设置Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);

            #region 加载调入仓库、调入仓位

            for (int i = 0; i < gdDetail.DisplayLayout.Rows.Count; i++)
            {
                var detailRow = gdDetail.DisplayLayout.Rows[i];

                //调入仓库ID为空
                if (detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value == null)
                {
                    continue;
                }

                #region 加载调入仓库

                _gridListCount += 1;
                gdDetail.DisplayLayout.ValueLists.Add("List" + _gridListCount);
                //调入组织下的仓库列表
                if (_transferInWarehouseDs.Count > 0)
                {
                    //该明细调入仓库在仓库列表的位置
                    int? detailWareHouseIndex = null;
                    for (int j = 0; j < _transferInWarehouseDs.Count; j++)
                    {
                        gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].ValueListItems.Add(_transferInWarehouseDs[j].WH_ID, _transferInWarehouseDs[j].WH_Name);

                        if (_transferInWarehouseDs[j].WH_ID == detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value.ToString())
                        {
                            detailWareHouseIndex = j;
                        }
                    }
                    gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].DisplayStyle = ValueListDisplayStyle.DisplayText;
                    detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].ValueList = gdDetail.DisplayLayout.ValueLists["List" + _gridListCount];

                    if (detailWareHouseIndex != null)
                    {
                        detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value = gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].ValueListItems[(int)detailWareHouseIndex].DataValue;
                    }
                }
                else
                {
                    detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].ValueList = null;
                    detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value = null;
                }
                #endregion

                #region 加载调入仓位

                //调入仓位ID为空
                if (detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Value == null)
                {
                    continue;
                }

                //调入仓库ID
                var transInWarehouseId = detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value == null
                    ? string.Empty
                    : detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value.ToString();

                _gridListCount += 1;
                gdDetail.DisplayLayout.ValueLists.Add("List" + _gridListCount);

                //该明细调入仓库下的仓位列表
                var transInWarehouseBinList = _transferInWarehouseBinDs.Where(p => p.WHB_WH_ID == transInWarehouseId).ToList();
                if (transInWarehouseBinList.Count > 0)
                {
                    //该明细调入仓位在仓位列表的位置
                    int? detailWarehouseBinIndex = null;
                    for (int j = 0; j < transInWarehouseBinList.Count; j++)
                    {
                        gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].ValueListItems.Add(transInWarehouseBinList[j].WHB_ID, transInWarehouseBinList[j].WHB_Name);
                        if (transInWarehouseBinList[j].WHB_ID == detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Value.ToString())
                        {
                            detailWarehouseBinIndex = j;
                        }
                    }
                    gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].DisplayStyle = ValueListDisplayStyle.DisplayText;
                    detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].ValueList = gdDetail.DisplayLayout.ValueLists["List" + _gridListCount];

                    if (detailWarehouseBinIndex != null)
                    {
                        detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Value = gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].ValueListItems[(int)detailWarehouseBinIndex].DataValue;
                    }
                }
                else
                {
                    detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].ValueList = null;
                    detailRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Value = null;
                }

                #endregion
            }

            #endregion
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
            //验证单据类型
            if (string.IsNullOrEmpty(cbTB_TypeName.Text))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.BILL_TYPE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //验证调拨类型
            if (string.IsNullOrEmpty(cbTB_TransferTypeName.Text))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.TRANSFER_TYPE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //验证调出组织
            if (!string.IsNullOrEmpty(txtTB_TransferOutOrgId.Text.Trim()))
            {
                if (txtTB_TransferOutOrgId.Text.Trim() != LoginInfoDAX.OrgID)
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0015, new object[] { SystemActionEnum.Name.SAVE, SystemTableEnums.Name.PIS_TransferBill }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            //验证调入组织
            if (string.IsNullOrEmpty(cbTB_TransferInOrgName.Text))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.TRANSFERIN_ORG }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            #region 验证调拨单明细

            if (gdDetail.Rows.Count == 0)
            {
                //请至少添加一条调拨单明细
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0014, new object[] { SystemTableEnums.Name.PIS_TransferBillDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            int i = 0;
            foreach (var loopDetail in _detailGridDS)
            {
                i++;
                //验证调拨数量不能为空
                if (string.IsNullOrEmpty(Convert.ToString(loopDetail.TBD_Qty ?? 0)))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i, SystemTableColumnEnums.PIS_TransferBillDetail.Name.TBD_Qty, MsgParam.POSITIVE_INTEGER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //验证调拨数量格式
                if (!BLLCom.IsDecimal(Convert.ToString(loopDetail.TBD_Qty ?? 0)))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i, SystemTableColumnEnums.PIS_TransferBillDetail.Name.TBD_Qty, MsgParam.POSITIVE_INTEGER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //验证入库单价不能为空
                if (string.IsNullOrEmpty(Convert.ToString(loopDetail.TBD_DestUnitPrice ?? 0)))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i, SystemTableColumnEnums.PIS_TransferBillDetail.Name.TBD_DestUnitPrice, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //验证入库单价格式
                if (!BLLCom.IsDecimal(Convert.ToString(loopDetail.TBD_DestUnitPrice ?? 0)))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i, SystemTableColumnEnums.PIS_TransferBillDetail.Name.TBD_DestUnitPrice, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (cbTB_TransferTypeName.Text == TransferTypeEnum.Name.ZZJDB
                    || cbTB_TransferTypeName.Text == TransferTypeEnum.Name.CKZC)
                {
                    //[调拨类型]为[组织间调拨]或[仓库转储]的场合，[调入仓库]不能为空
                    if (string.IsNullOrEmpty(loopDetail.TBD_TransInWhId))
                    {
                        MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0027, new object[] { i, MsgParam.TRANSFERIN_WAREHOUSE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
                if (cbTB_TransferTypeName.Text == TransferTypeEnum.Name.KWZC)
                {
                    //[调拨类型]为[库位转储]的场合

                    //[调入仓库]与[调出仓库]要一致
                    if (loopDetail.TBD_TransInWhId != loopDetail.TBD_TransOutWhId)
                    {
                        //调拨类型为库位转储，第i+1行，调入仓库要与调出仓库一致！
                        MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0031, new object[] { i }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                    //[调入仓位]不能为空
                    if (string.IsNullOrEmpty(loopDetail.TBD_TransInBinId))
                    {
                        MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0027, new object[] { i, MsgParam.TRANSFERIN_WAREHOUSEBIN }), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            //验证调出组织
            if (!string.IsNullOrEmpty(txtTB_TransferOutOrgId.Text.Trim()))
            {
                if (txtTB_TransferOutOrgId.Text.Trim() != LoginInfoDAX.OrgID)
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0015, new object[] { SystemActionEnum.Name.DELETE, SystemTableEnums.Name.PIS_StockOutBill }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            //确认删除操作
            DialogResult dialogResult = MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0012), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.OK)
            {
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的HeadDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            HeadDS = new TransferBillManagerUIModel()
            {
                //单号
                TB_No = txtTB_No.Text.Trim(),
                //单据类型
                TB_TypeName = cbTB_TypeName.Text,
                //单据类型编码
                TB_TypeCode = cbTB_TypeName.Value == null ? "" : cbTB_TypeName.Value.ToString(),
                //调拨类型
                TB_TransferTypeName = cbTB_TransferTypeName.Text,
                //调拨类型编码
                TB_TransferTypeCode = cbTB_TransferTypeName.Value == null ? "" : cbTB_TransferTypeName.Value.ToString(),
                //调入组织
                TB_TransferOutOrgName = cbTB_TransferOutOrgName.Text,
                //调入组织
                TB_TransferInOrgName = cbTB_TransferInOrgName.Text,
                //单据状态
                TB_StatusName = cbTB_StatusName.Text,
                //单据状态编码
                TB_StatusCode = cbTB_StatusName.Value == null ? "" : cbTB_StatusName.Value.ToString(),
                //审核状态
                TB_ApprovalStatusName = cbTB_ApprovalStatusName.Text,
                //审核状态编码
                TB_ApprovalStatusCode = cbTB_ApprovalStatusName.Value == null ? "" : cbTB_ApprovalStatusName.Value.ToString(),
                //备注
                TB_Remark = txtTB_Remark.Text,
                //有效
                TB_IsValid = ckTB_IsValid.Checked,
                //创建人
                TB_CreatedBy = txtTB_CreatedBy.Text.Trim(),
                //创建时间
                TB_CreatedTime = (DateTime?)dtTB_CreatedTime.Value ?? DateTime.Now,
                //修改人
                TB_UpdatedBy = txtTB_UpdatedBy.Text.Trim(),
                //修改时间
                TB_UpdatedTime = (DateTime?)dtTB_UpdatedTime.Value ?? DateTime.Now,
                //调拨单ID
                TB_ID = txtTB_ID.Text.Trim(),
                //调入组织ID
                TB_TransferOutOrgId = txtTB_TransferOutOrgId.Text.Trim(),
                //调入组织ID
                TB_TransferInOrgId = txtTB_TransferInOrgId.Text.Trim(),
                //版本号
                TB_VersionNo = Convert.ToInt64(txtTB_VersionNo.Text.Trim() == "" ? "1" : txtTB_VersionNo.Text.Trim()),
                //总数量
                TotalQty = Convert.ToDecimal(txtTotalQty.Text.Trim() == "" ? "0" : txtTotalQty.Text.Trim()),
                //总金额
                TotalAmount = Convert.ToDecimal(txtTotalAmount.Text.Trim() == "" ? "0" : txtTotalAmount.Text.Trim()),
            };
        }

        #region 明细相关

        /// <summary>
        /// 处理领料明细Func
        /// </summary>
        /// <param name="paramPickPartsDetailList"></param>
        /// <returns></returns>
        private bool HandlePickPartsDetail(List<PickPartsQueryUIModel> paramPickPartsDetailList)
        {
            //将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();

            DateTime curDateTime = BLLCom.GetCurStdDatetime();

            #region 从[领料查询]中添加调拨明细

            foreach (var loopPickPartsDetail in paramPickPartsDetailList)
            {
                if (string.IsNullOrEmpty(loopPickPartsDetail.INV_Name))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(loopPickPartsDetail.INV_ID))
                {
                    //没有获取到paramPickPartsDetail.INV_Name的库存，添加失败
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[] { loopPickPartsDetail.INV_Name + MsgParam.OF + SystemTableEnums.Name.PIS_Inventory, MsgParam.ADD }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (cbTB_TransferTypeName.Text == TransferTypeEnum.Name.KWZC)
                {
                    if (string.IsNullOrEmpty(loopPickPartsDetail.INV_WH_ID))
                    {
                        //调拨单的[调拨类型]为[库位转储]，不能添加仓库为空的配件
                        MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { MsgParam.TRANSFER_TYPE + MsgParam.BE + TransferTypeEnum.Name.KWZC, MsgParam.ADD + MsgParam.WAREHOUSE + MsgParam.BE + SysConst.CHS_COMBTEXTBIND + MsgParam.OF + MsgParam.AUTOPARTS }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
                if (loopPickPartsDetail.INV_Qty == 0)
                {
                    //库存为零，领料失败
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.PIS_Inventory, MsgParam.ADD }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                //条码+批次相同，为同一个配件
                TransferBillManagerDetailUIModel sameTransferDetail = _detailGridDS.FirstOrDefault(x => x.TBD_Barcode == loopPickPartsDetail.INV_Barcode && x.TBD_TransOutBatchNo == loopPickPartsDetail.INV_BatchNo);

                if (sameTransferDetail == null)
                {
                    //明细列表中不存在相同配件

                    #region 为调拨明细赋值
                    TransferBillManagerDetailUIModel newAutoPartsTransferDetail = new TransferBillManagerDetailUIModel
                    {
                        IsChecked = false,
                        INV_ID = loopPickPartsDetail.INV_ID,
                        INV_Qty = loopPickPartsDetail.INV_Qty,

                        Tmp_TBD_ID = Guid.NewGuid().ToString(),
                        TBD_ID = null,
                        TBD_TB_ID = HeadDS.TB_ID,
                        TBD_TB_No = HeadDS.TB_No,
                        TBD_Name = loopPickPartsDetail.INV_Name,
                        TBD_ThirdNo = loopPickPartsDetail.INV_ThirdNo,
                        TBD_OEMNo = loopPickPartsDetail.INV_OEMNo,
                        TBD_Barcode = loopPickPartsDetail.INV_Barcode,
                        TBD_TransOutBatchNo = loopPickPartsDetail.INV_BatchNo,
                        TBD_Specification = loopPickPartsDetail.INV_Specification,
                        TBD_UOM = loopPickPartsDetail.APA_UOM,
                        TBD_SourUnitPrice = loopPickPartsDetail.INV_PurchaseUnitPrice,
                        TBD_DestUnitPrice = loopPickPartsDetail.INV_PurchaseUnitPrice,
                        TBD_Qty = loopPickPartsDetail.INV_Qty > 1 ? 1 : loopPickPartsDetail.INV_Qty,
                        TBD_SUPP_ID = loopPickPartsDetail.INV_SUPP_ID,
                        //调出仓库
                        TBD_TransOutWhId = loopPickPartsDetail.INV_WH_ID,
                        TransOutWhName = loopPickPartsDetail.WH_Name,
                        //调出仓位
                        TBD_TransOutBinId = loopPickPartsDetail.INV_WHB_ID,
                        TransOutWhbName = loopPickPartsDetail.WHB_Name,

                        APA_Brand = loopPickPartsDetail.APA_Brand,
                        APA_Level = loopPickPartsDetail.APA_Level,
                        APA_VehicleBrand = loopPickPartsDetail.APA_VehicleBrand,
                        APA_VehicleInspire = loopPickPartsDetail.APA_VehicleInspire,
                        APA_VehicleCapacity = loopPickPartsDetail.APA_VehicleCapacity,
                        APA_VehicleGearboxTypeCode = loopPickPartsDetail.APA_VehicleGearboxTypeCode,
                        APA_VehicleGearboxTypeName = loopPickPartsDetail.APA_VehicleGearboxTypeName,
                        APA_VehicleYearModel = loopPickPartsDetail.APA_VehicleYearModel,

                        //TBD_IsSettled = false,
                        TBD_IsValid = true,
                        TBD_CreatedBy = LoginInfoDAX.UserName,
                        TBD_CreatedTime = curDateTime,
                        TBD_UpdatedBy = LoginInfoDAX.UserName,
                        TBD_UpdatedTime = curDateTime
                    };
                    //明细金额
                    newAutoPartsTransferDetail.DetailDestAmount =
                        Math.Round(
                            (newAutoPartsTransferDetail.TBD_Qty ?? 0) *
                            (newAutoPartsTransferDetail.TBD_DestUnitPrice ?? 0), 2);
                    #endregion

                    if (cbTB_TransferTypeName.Text == TransferTypeEnum.Name.KWZC)
                    {
                        //[调拨类型]为[库位转储]的场合，[调入仓库]与[调出仓库]一致，且不可编辑
                        newAutoPartsTransferDetail.TBD_TransInWhId = newAutoPartsTransferDetail.TBD_TransOutWhId;
                        gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].CellActivation = Activation.ActivateOnly;
                        //设置[调入仓库]Style为Defalut
                        gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Style = ColumnStyle.Default;
                        gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].ButtonDisplayStyle = ButtonDisplayStyle.OnMouseEnter;

                        _latestAutoFactoryWarehouseID = newAutoPartsTransferDetail.TBD_TransInWhId;
                    }

                    _detailGridDS.Add(newAutoPartsTransferDetail);

                    //设定指定单元格的值列表
                    SetCellValueListForNew(gdDetail.Rows.Count == 0 ? 0 : gdDetail.Rows.Count - 1, _latestAutoFactoryWarehouseID, _latestAutoFactoryWarehouseBinID);
                }
                else
                {
                    //明细列表中已存在相同配件
                    decimal? tempQty = (sameTransferDetail.TBD_Qty ?? 0) + (loopPickPartsDetail.INV_Qty > 1 ? 1 : loopPickPartsDetail.INV_Qty);
                    sameTransferDetail.TBD_Qty = tempQty > loopPickPartsDetail.INV_Qty ? loopPickPartsDetail.INV_Qty : tempQty;
                    sameTransferDetail.TBD_SourUnitPrice = loopPickPartsDetail.INV_PurchaseUnitPrice;
                    sameTransferDetail.TBD_DestUnitPrice = loopPickPartsDetail.INV_PurchaseUnitPrice;
                }
            }

            #endregion

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置调拨单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;

            return true;
        }

        /// <summary>
        /// 添加调拨明细
        /// </summary>
        private void AddTransferDetail()
        {
            #region 验证

            //验证调入组织
            if (cbTB_TransferOutOrgName.Value == null
                || string.IsNullOrEmpty(cbTB_TransferOutOrgName.Text))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0016, new object[] { MsgParam.TRANSFEROUT_ORG, MsgParam.ADD }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //验证调入组织
            if (cbTB_TransferInOrgName.Value == null
                || string.IsNullOrEmpty(cbTB_TransferInOrgName.Text))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0023, new object[] { MsgParam.SELECT + MsgParam.TRANSFERIN_ORG, MsgParam.ADD }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //提醒明细数量
            if (gdDetail.Rows.Count >= 25 && gdDetail.Rows.Count % 25 == 0)
            {
                DialogResult isAddContinueResult = MessageBoxs.Show(Trans.PIS, this.ToString(),
                    MsgHelp.GetMsg(MsgCode.W_0020, new object[] { gdDetail.Rows.Count }), MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question);
                if (isAddContinueResult != DialogResult.OK)
                {
                    return;
                }
            }
            #endregion

            #region 从[领料查询]中添加[调拨单明细]

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                {ComViewParamKey.PickAutoPartsFunc.ToString(),_pickPartsDetailFunc},
            };

            FrmPickPartsQuery frmPickPartsQuery = new FrmPickPartsQuery(paramViewParameters, CustomEnums.CustomeSelectionMode.Multiple)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            DialogResult dialogResult = frmPickPartsQuery.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            #endregion

            //根据[是否存在明细]控制单头是否可编辑
            SetDetailByIsExistDetail();

            #region 计算单头总数量和总金额

            //明细总数量
            decimal detailQty = 0;
            //明细总金额
            decimal detailAmount = 0;

            foreach (var loopDetail in _detailGridDS)
            {
                detailQty += (loopDetail.TBD_Qty ?? 0);
                detailAmount += Math.Round((loopDetail.TBD_Qty ?? 0) * (loopDetail.TBD_DestUnitPrice ?? 0), 2);
            }
            //单头总数量
            txtTotalQty.Text = detailQty.ToString();
            //单头总金额
            txtTotalAmount.Text = detailAmount.ToString();
            #endregion

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置调拨单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
            SetDetailControl();
        }

        /// <summary>
        /// 更新调拨明细
        /// </summary>
        private void UpdateTransferDetail()
        {
            #region 验证

            if (!IsAllowUpdateDetailGrid())
            {
                return;
            }

            //验证调拨单的审核状态，[已审核]的调拨单不能更新明细
            if (cbTB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableEnums.Name.PIS_TransferBill + ApprovalStatusEnum.Name.YSH, MsgParam.UPDATE + SystemTableEnums.Name.PIS_TransferBillDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var activeRowIndex = gdDetail.ActiveRow.Index;
            //判断调拨单明细Grid内[唯一标识]是否为空
            if ((gdDetail.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_ID].Value == null || string.IsNullOrEmpty(gdDetail.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_ID].Value.ToString()))
                &&
                (gdDetail.Rows[activeRowIndex].Cells["Tmp_TBD_ID"].Value == null ||
                string.IsNullOrEmpty(gdDetail.Rows[activeRowIndex].Cells["Tmp_TBD_ID"].Value.ToString())))
            {
                return;
            }
            //待更新明细的ID
            string tempDetailId = string.Empty;
            if (gdDetail.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_ID] != null
                && gdDetail.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_ID].Value != null)
            {
                tempDetailId = gdDetail.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_ID].Value.ToString();
            }
            else if (gdDetail.Rows[activeRowIndex].Cells["Tmp_TBD_ID"] != null
                && gdDetail.Rows[activeRowIndex].Cells["Tmp_TBD_ID"].Value != null)
            {
                tempDetailId = gdDetail.Rows[activeRowIndex].Cells["Tmp_TBD_ID"].Value.ToString();
            }
            //待更新的调拨单明细
            TransferBillManagerDetailUIModel transferBillToUpdateDetail = _detailGridDS.FirstOrDefault(x => (!string.IsNullOrEmpty(x.TBD_ID) && x.TBD_ID == tempDetailId) || (!string.IsNullOrEmpty(x.Tmp_TBD_ID) && x.Tmp_TBD_ID == tempDetailId));

            #endregion

            #region 从[领料查询]中添加[调拨单明细]

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                {ComViewParamKey.PickAutoPartsFunc.ToString(),_pickPartsDetailFunc},
            };

            FrmPickPartsQuery frmPickPartsQuery = new FrmPickPartsQuery(paramViewParameters, CustomEnums.CustomeSelectionMode.Multiple)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            DialogResult dialogResult = frmPickPartsQuery.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            //移除当前行
            _detailGridDS.Remove(transferBillToUpdateDetail);
            #endregion

            //根据[是否存在明细]设置详情是否可编辑
            SetDetailByIsExistDetail();

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置调拨单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 删除入库明细
        /// </summary>
        private void DeleteTransferDetail()
        {
            #region 验证

            gdDetail.UpdateData();
            //待删除的调拨单明细列表
            var deleteTransferBillDetailList = _detailGridDS.Where(p => p.IsChecked == true).ToList();
            if (deleteTransferBillDetailList.Count == 0)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { SystemTableEnums.Name.PIS_TransferBillDetail, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion

            //待删除明细总数量
            decimal deleteDetailQty = 0;
            //待删除的明细总金额
            decimal deleteDetailAmount = 0;

            //移除勾选项
            foreach (var loopTransferDetail in deleteTransferBillDetailList)
            {
                deleteDetailQty += (loopTransferDetail.TBD_Qty ?? 0);
                deleteDetailAmount += Math.Round((loopTransferDetail.TBD_Qty ?? 0) * (loopTransferDetail.TBD_DestUnitPrice ?? 0), 2);
                _detailGridDS.Remove(loopTransferDetail);
            }

            //根据[是否存在明细]设置详情是否可编辑
            SetDetailByIsExistDetail();

            #region 计算单头总数量和总金额

            //原单头总数量
            decimal tempTotalQty = Convert.ToDecimal(txtTotalQty.Text.Trim() == "" ? "0" : txtTotalQty.Text.Trim());
            //原单头总金额
            decimal tempTotalAmount = Convert.ToDecimal(txtTotalAmount.Text.Trim() == "" ? "0" : txtTotalAmount.Text.Trim());

            //单头总数量
            txtTotalQty.Text = (tempTotalQty - deleteDetailQty).ToString();
            //单头总金额
            txtTotalAmount.Text = (tempTotalAmount - deleteDetailAmount).ToString();
            #endregion

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置调拨单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 是否允许更新[调拨单明细]列表的数据
        /// </summary>
        /// <returns>true:允许；false：不允许</returns>
        private bool IsAllowUpdateDetailGrid()
        {
            if (gdDetail.ActiveRow == null || gdDetail.ActiveRow.Index < 0)
            {
                return false;
            }
            if (gdDetail.DisplayLayout.Bands[0].SortedColumns.Count > 0)
            {
                foreach (UltraGridColumn loopColumn in gdDetail.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (loopColumn.IsGroupByColumn)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 设置详情页面控件的是否可编辑
        /// </summary>
        private void SetDetailControl()
        {
            if (cbTB_TransferOutOrgName.Text == LoginInfoDAX.OrgShortName
                && (string.IsNullOrEmpty(txtTB_ID.Text.Trim())
                || cbTB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.DSH))
            {
                #region [调出组织]是当前组织 并且 调拨单未保存 或 [审核状态]为[待审核] 的场合，详情可编辑

                //根据[是否存在明细]设置详情是否可编辑
                SetDetailByIsExistDetail();

                //明细列表可添加、删除、更新
                toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedProps.Enabled = true;
                toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = true;

                //[配件名称]列可单击
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_Name].Style = ColumnStyle.EditButton;

                SetTransferBillDetailStyle(false, false);

                ////明细列表中[选择]、[调入仓位]、[入库单价]和[调出数量]列可编辑
                //gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].CellActivation = Activation.AllowEdit;
                //gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].CellActivation = Activation.AllowEdit;
                //gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_DestUnitPrice].CellActivation = Activation.AllowEdit;
                //gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_Qty].CellActivation = Activation.AllowEdit;

                if (cbTB_TransferTypeName.Text == TransferTypeEnum.Name.KWZC)
                {

                    SetTransferBillDetailStyle(false, true);

                    ////[调拨类型]为[库位存储]的场合，[调入仓库]不可编辑
                    //gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].CellActivation = Activation.ActivateOnly;
                    ////[调入仓位]列显示
                    //gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Hidden = false;
                }
                else
                {
                    SetTransferBillDetailStyle(false, false);

                    ////[调拨类型]为[仓库存储]或[组织间调拨]的场合，[调入仓库]可编辑
                    //gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].CellActivation = Activation.AllowEdit;
                    ////[调入仓位]列隐藏
                    //gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Hidden = true;
                }
                #endregion
            }
            else
            {
                #region [调出组织]不是当前组织 或 [审核状态]为[已审核] 的场合，详情不可编辑

                //调拨类型
                cbTB_TransferTypeName.Enabled = false;
                //调入组织
                cbTB_TransferInOrgName.Enabled = false;

                //明细列表不可添加、删除、更新
                toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedProps.Enabled = false;
                toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = false;

                //明细列表中[配件名称]列不可单击
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_Name].Style = ColumnStyle.Default;


                SetTransferBillDetailStyle(true, true);
                ////明细列表中[选择]、[调入仓库]、[调入仓位]、[入库单价]和[调出数量]不列可编辑
                //gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].CellActivation = Activation.ActivateOnly;
                //gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].CellActivation = Activation.ActivateOnly;
                //gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].CellActivation = Activation.ActivateOnly;
                //gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_DestUnitPrice].CellActivation = Activation.ActivateOnly;
                //gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_Qty].CellActivation = Activation.ActivateOnly;

                #endregion
            }

            #region 设置[调入仓库]Style

            if (gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].CellActivation == Activation.AllowEdit)
            {
                //[调入仓库]可编辑的场合，设置ColumnStyle为DropDown
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Style = ColumnStyle.DropDownList;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].ButtonDisplayStyle = ButtonDisplayStyle.Always;
            }
            else
            {
                //[调入仓库]不可编辑的场合，设置ColumnStyle为Default
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Style = ColumnStyle.Default;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].ButtonDisplayStyle = ButtonDisplayStyle.OnMouseEnter;
            }

            #endregion

            #region 设置[调入仓位]Style

            if (gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].CellActivation == Activation.AllowEdit)
            {
                //[调入仓位]可编辑的场合，设置ColumnStyle为DropDown
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Style = ColumnStyle.DropDownList;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].ButtonDisplayStyle = ButtonDisplayStyle.Always;
            }
            else
            {
                //[调入仓位]不可编辑的场合，设置ColumnStyle为Default
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Style = ColumnStyle.Default;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].ButtonDisplayStyle = ButtonDisplayStyle.OnMouseEnter;
            }

            #endregion
        }
        /// <summary>
        /// 设置单元格是否可以编辑，边框和背景色的颜色
        /// </summary>
        /// <param name="paramQtyAndUnitPriceIsReadOnly">设置【调入仓位】【入库价格】【数量】【调入仓位】</param>
        /// <param name="paramThisReceivedQtyIsReadOnly">设置【调入仓库】</param>
        private void SetTransferBillDetailStyle(bool paramQtyAndUnitPriceIsReadOnly, bool paramThisReceivedQtyIsReadOnly)
        {
            #region 设置Grid数据颜色
            gdDetail.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
            foreach (var loopGridRow in gdDetail.Rows)
            {
                if (paramQtyAndUnitPriceIsReadOnly)
                {
                    #region 调入仓位

                    //设置单元格只读
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion

                    #region 入库价格

                    //设置单元格只读
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_DestUnitPrice].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_DestUnitPrice].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_DestUnitPrice].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion

                    #region 数量

                    //设置单元格只读
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_Qty].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_Qty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_Qty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion

                    #region 调入仓库

                    //设置单元格可编辑
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion
                }
                else
                {
                    #region 调入仓位

                    //设置单元格可编辑
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #region 入库价格

                    //设置单元格可编辑
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_DestUnitPrice].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_DestUnitPrice].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_DestUnitPrice].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #region 数量

                    //设置单元格可编辑
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_Qty].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_Qty].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_Qty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #region 调入仓库

                    if (paramThisReceivedQtyIsReadOnly)
                    {
                        #region 调入仓库

                        //设置单元格只读
                        loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Activation = Activation.ActivateOnly;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                        loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Hidden = false;

                        #endregion
                    }
                    else
                    {
                        #region 调入仓库

                        //设置单元格只读
                        loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Activation = Activation.AllowEdit;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                        loopGridRow.Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Hidden = true;

                        #endregion

                    }

                    #endregion
                }

            }
            #endregion
        }

        /// <summary>
        /// 设置动作按钮状态
        /// </summary>
        private void SetActionEnableByStatus()
        {
            if (cbTB_TransferOutOrgName.Text != LoginInfoDAX.OrgShortName)
            {
                #region [调出组织]不是当前组织的场合，[保存]、[删除]、[审核]、[反审核]不可用

                SetActionEnable(SystemActionEnum.Code.SAVE, false);
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                SetActionEnable(SystemActionEnum.Code.APPROVE, false);
                SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);
                SetActionEnable(SystemActionEnum.Code.COPY, false);
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOLOGISTICSBILL, true, false);
                #endregion
            }
            else
            {
                #region [调出组织]不是当前组织的场合，根据审核状态区分
                if (cbTB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
                {
                    //[审核状态]为[已审核]的场合，[保存]、[删除]、[审核]不可用
                    SetActionEnable(SystemActionEnum.Code.SAVE, false);
                    SetActionEnable(SystemActionEnum.Code.DELETE, false);
                    SetActionEnable(SystemActionEnum.Code.APPROVE, false);
                    SetActionEnable(SystemActionEnum.Code.UNAPPROVE, true);
                    SetActionEnable(SystemActionEnum.Code.PRINT, true);
                    SetActionEnable(SystemActionEnum.Code.COPY, true);
                    if (cbTB_StatusName.Text == TransfeStatusEnum.Name.DFH)
                    {
                        SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOLOGISTICSBILL, true, cbTB_TransferTypeName.Text == TransferTypeEnum.Name.ZZJDB);
                    }
                    else
                    {
                        SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOLOGISTICSBILL, true, false);
                    }
                }
                else
                {
                    //新增或[审核状态]为[待审核]的场合，[反审核]、[打印]不可用
                    SetActionEnable(SystemActionEnum.Code.SAVE, true);
                    SetActionEnable(SystemActionEnum.Code.DELETE, !string.IsNullOrEmpty(txtTB_ID.Text));
                    SetActionEnable(SystemActionEnum.Code.APPROVE, !string.IsNullOrEmpty(txtTB_ID.Text));
                    SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);
                    SetActionEnable(SystemActionEnum.Code.PRINT, false);
                    SetActionEnable(SystemActionEnum.Code.COPY, false);
                    SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOLOGISTICSBILL, true, false);
                }
                #endregion
            }
        }

        /// <summary>
        /// 设定指定单元格的值列表
        /// </summary>
        /// <param name="paramRowIndex">行索引</param>
        /// <param name="paramAutoFactoryWarehouseId">调入仓库ID</param>
        /// <param name="paranAutoFactoryWarehouseBinId">调入仓位ID</param>
        private void SetCellValueListForNew(int paramRowIndex, string paramAutoFactoryWarehouseId, string paranAutoFactoryWarehouseBinId)
        {
            #region 设置[调入仓库]的ValueList和默认值

            _gridListCount += 1;
            this.gdDetail.DisplayLayout.ValueLists.Add("List" + _gridListCount);
            //调入仓库List
            List<MDLPIS_Warehouse> tempInWarehouseList = _transferInWarehouseDs;

            if (cbTB_TransferTypeName.Text == TransferTypeEnum.Name.CKZC)
            {
                //[调拨类型]为[仓库转储]的场合，[调入仓库]与[调出仓库]不同
                tempInWarehouseList = _transferInWarehouseDs.Where(
                        x => x.WH_ID != gdDetail.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransOutWhId].Value?.ToString())
                        .ToList();
            }

            if (tempInWarehouseList.Count > 0)
            {
                for (int j = 0; j < tempInWarehouseList.Count; j++)
                {
                    this.gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].ValueListItems.Add(
                        tempInWarehouseList[j].WH_ID, tempInWarehouseList[j].WH_Name);
                }
                this.gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].DisplayStyle = ValueListDisplayStyle.DisplayText;
                //调入仓库ValueList
                gdDetail.DisplayLayout.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].ValueList = gdDetail.DisplayLayout.ValueLists["List" + _gridListCount];

                if (!string.IsNullOrEmpty(paramAutoFactoryWarehouseId))
                {
                    //最后一次选择的调入仓库不为空，默认该仓库
                    var tempWarehouse = tempInWarehouseList.FirstOrDefault(x => x.WH_ID == paramAutoFactoryWarehouseId);
                    if (tempWarehouse != null)
                    {
                        gdDetail.DisplayLayout.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value = tempWarehouse.WH_ID;
                    }
                }
                //if (gdDetail.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value == null)
                //{
                //    //默认调入仓库
                //    gdDetail.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value = gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].ValueListItems[0].DataValue;
                //}
            }
            else
            {
                gdDetail.DisplayLayout.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].ValueList = null;
                gdDetail.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value = null;
            }

            #endregion

            #region 设置[调入仓位]的ValueList和默认值

            if (gdDetail.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value != null)
            {
                _gridListCount += 1;
                this.gdDetail.DisplayLayout.ValueLists.Add("List" + _gridListCount);

                //调入仓位List
                List<MDLPIS_WarehouseBin> tempInWarehouseBinList = _transferInWarehouseBinDs;

                if (cbTB_TransferTypeName.Text == TransferTypeEnum.Name.CKZC)
                {
                    //[调拨类型]为[库位转储]的场合，[调入仓位]与[调出仓位]不同
                    tempInWarehouseBinList = _transferInWarehouseBinDs.Where(
                            x => x.WHB_ID != gdDetail.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransOutBinId].Value?.ToString())
                            .ToList();
                }
                //调入仓库下的调入仓位List
                var transInWarehouseBinList = tempInWarehouseBinList.Where(p => p.WHB_WH_ID == gdDetail.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInWhId].Value.ToString()).ToList();
                if (transInWarehouseBinList.Count > 0)
                {
                    for (int j = 0; j < transInWarehouseBinList.Count; j++)
                    {
                        this.gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].ValueListItems.Add(transInWarehouseBinList[j].WHB_ID, transInWarehouseBinList[j].WHB_Name);
                    }
                    this.gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].DisplayStyle = ValueListDisplayStyle.DisplayText;
                    //调入仓位List
                    gdDetail.DisplayLayout.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].ValueList = gdDetail.DisplayLayout.ValueLists["List" + _gridListCount];

                    if (!string.IsNullOrEmpty(paranAutoFactoryWarehouseBinId))
                    {
                        //最后一次选择的调入仓位不为空，默认该仓位
                        var tempWarehouseBin = transInWarehouseBinList.FirstOrDefault(x => x.WHB_ID == paranAutoFactoryWarehouseBinId);
                        if (tempWarehouseBin != null)
                        {
                            gdDetail.DisplayLayout.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Value = tempWarehouseBin.WHB_ID;
                        }
                    }
                    //if (gdDetail.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Value == null)
                    //{
                    //    //默认调入仓位
                    //    gdDetail.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Value = this.gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].ValueListItems[0].DataValue;
                    //}
                }
                else
                {
                    gdDetail.DisplayLayout.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].ValueList = null;
                    gdDetail.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Value = null;
                }
            }
            #endregion

            gdDetail.UpdateData();
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 仓库选择后，设定仓位的值列表和默认选中值
        /// </summary>
        /// <param name="paramWarehouseId">仓库ID</param>
        /// <param name="paramRowIndex">行索引</param>
        private void SetCellWarehouseBinAfterByWarehouse(string paramWarehouseId, int paramRowIndex)
        {
            #region 调入仓位

            _gridListCount += 1;
            this.gdDetail.DisplayLayout.ValueLists.Add("List" + _gridListCount);
            //判断是否存在调入仓位信息
            if (_transferInWarehouseBinDs.Count == 0)
            {
                //获取调入仓位信息
                _transferInWarehouseBinDs = BLLCom.GetWarehouseBinList(cbTB_TransferInOrgName.Value?.ToString(), paramWarehouseId);
            }

            //获取指定调入仓库下的调入仓位，并过滤重复数据
            var transInWarehouseBinList = _transferInWarehouseBinDs.Where(p => p.WHB_WH_ID == paramWarehouseId).Select(p => new { p.WHB_ID, p.WHB_Name }).Distinct().ToList();

            if (transInWarehouseBinList.Count > 0)
            {
                for (int j = 0; j < transInWarehouseBinList.Count; j++)
                {
                    this.gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].ValueListItems.Add(transInWarehouseBinList[j].WHB_ID, transInWarehouseBinList[j].WHB_Name);
                }
                this.gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].DisplayStyle = ValueListDisplayStyle.DisplayText;
                gdDetail.DisplayLayout.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].ValueList = gdDetail.DisplayLayout.ValueLists["List" + _gridListCount];
                //gdDetail.DisplayLayout.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Value =
                //        gdDetail.DisplayLayout.ValueLists["List" + _gridListCount].ValueListItems[0].DisplayText;
            }
            else
            {
                gdDetail.DisplayLayout.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].ValueList = null;
                gdDetail.Rows[paramRowIndex].Cells[SystemTableColumnEnums.PIS_TransferBillDetail.Code.TBD_TransInBinId].Value = null;
            }

            #endregion

            gdDetail.UpdateData();
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }
        #endregion

        /// <summary>
        /// 根据[是否存在明细]设置详情是否可编辑
        /// </summary>
        private void SetDetailByIsExistDetail()
        {
            if (_detailGridDS.Count == 0)
            {
                //不存在明细的场合，[调拨类型]、[调入组织]可编辑
                cbTB_TransferTypeName.Enabled = true;
                cbTB_TransferInOrgName.Enabled = true;
            }
            else
            {
                //存在明细的场合，[调拨类型]、[调入组织]不可编辑
                cbTB_TransferTypeName.Enabled = false;
                cbTB_TransferInOrgName.Enabled = false;
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
                    var removeList = HeadGridDS.Where(x => x.IsChecked == true).ToList();
                    foreach (var loopRemove in removeList)
                    {
                        HeadGridDS.Remove(loopRemove);
                    }
                }
                else
                {
                    var curHead = HeadGridDS.FirstOrDefault(x => x.TB_ID == HeadDS.TB_ID);
                    if (curHead != null)
                    {
                        HeadGridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = HeadGridDS.FirstOrDefault(x => x.TB_ID == HeadDS.TB_ID);
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
        #endregion

    }
}
