using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UI.PIS
{
    /// <summary>
    /// 盘点管理
    /// </summary>
    public partial class FrmStocktakingTaskManager : BaseFormCardListDetail<StocktakingTaskManagerUIModel, StocktakingTaskManagerQCModel, MDLPIS_StocktakingTask>
    {
        #region 全局变量

        /// <summary>
        /// 盘点管理BLL
        /// </summary>
        private StocktakingTaskManagerBLL _bll = new StocktakingTaskManagerBLL();

        #region 单头相关数据源

        /// <summary>
        /// 仓库数据源
        /// </summary>
        List<MDLPIS_Warehouse> _warehouseDs = new List<MDLPIS_Warehouse>();
        /// <summary>
        /// 仓位数据源
        /// </summary>
        List<MDLPIS_WarehouseBin> _warehouseBinDs = new List<MDLPIS_WarehouseBin>();

        /// <summary>
        /// 盘点结果数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _stocktakingBillCheckResultDs = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 盘点单状态数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _stocktakingBillStatusDs = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 审核状态数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _approvalStatusDs = new List<ComComboBoxDataSourceTC>();

        #endregion

        #region 明细相关数据源

        /// <summary>
        /// 【详情】Tab内Grid绑定用的数据源
        /// </summary>
        private SkyCarBindingList<StocktakingTaskManagerDetailUIModel, MDLPIS_StocktakingTaskDetail> _detailGridDS = new SkyCarBindingList<StocktakingTaskManagerDetailUIModel, MDLPIS_StocktakingTaskDetail>();

        #endregion

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmStocktakingTaskManager构造方法
        /// </summary>
        public FrmStocktakingTaskManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmStocktakingTaskManager_Load(object sender, EventArgs e)
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

            //明细Toolbar的[加载零库存]是否可编辑
            toolbarsManagerDetail.Tools[SysConst.LoadZeroInventory].SharedProps.Enabled = toolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[SystemActionEnum.Code.STOCKCOUNTZEROINVENTORY].SharedPropsInternal.Enabled;
            //按钮[确认损益]是否可编辑
            btnConfirmProfitAndLoss.Enabled = toolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[SystemActionEnum.Code.CONFIRMPROFITANDLOSS].SharedPropsInternal.Enabled;
            //按钮[取消确认损益]是否可编辑
            btnCancelConfirmProfitAndLoss.Enabled = toolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[SystemActionEnum.Code.CANCELCONFIRMPROFITANDLOSS].SharedPropsInternal.Enabled;
            //按钮[校正库存]是否可编辑
            btnCorrectInventoryByProfitAndLoss.Enabled = toolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[SystemActionEnum.Code.INVENTORYRECTIFY].SharedPropsInternal.Enabled;

            //隐藏动作按钮栏的[审核]、[反审核]、[损益分析]、[盘点零库存]、[校正库存]、[确认库存损益]、[取消确认库存损益]
            SetActionVisiableAndEnable(SystemActionEnum.Code.APPROVE, false, false);
            SetActionVisiableAndEnable(SystemActionEnum.Code.UNAPPROVE, false, false);
            SetActionVisiableAndEnable(SystemActionEnum.Code.ANALYSE, false, false);
            SetActionVisiableAndEnable(SystemActionEnum.Code.STOCKCOUNTZEROINVENTORY, false, false);
            SetActionVisiableAndEnable(SystemActionEnum.Code.INVENTORYRECTIFY, false, false);
            SetActionVisiableAndEnable(SystemActionEnum.Code.CONFIRMPROFITANDLOSS, false, false);
            SetActionVisiableAndEnable(SystemActionEnum.Code.CANCELCONFIRMPROFITANDLOSS, false, false);

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
        /// 盘点单号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_ST_No_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 开始时间ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_ST_StartTime_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 结束时间ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_ST_EndTime_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 有效CheckedChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckWhere_ST_IsValid_CheckedChanged(object sender, EventArgs e)
        {
            ////执行查询
            //QueryAction();
        }
        /// <summary>
        /// 仓库SelectedIndexChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbWhere_ST_WH_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(mcbWhere_ST_WH_ID.SelectedValue))
            {
                return;
            }
            if (_warehouseBinDs == null)
            {
                return;
            }
            //加载选中仓库下仓位List
            var tempWarehouseBinDs = _warehouseBinDs.Where(x => x.WHB_WH_ID == mcbWhere_ST_WH_ID.SelectedValue.ToString()).ToList();
            mcbWhere_ST_WHB_ID.DataSource = tempWarehouseBinDs;
        }

        /// <summary>
        /// 查询配件名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_STD_Name_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            //查询配件名称
            FrmAutoPartsNameQuery frmAutoPartsNameQuery =
                new FrmAutoPartsNameQuery(SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name,
                paramSelectedValue: txtWhere_STD_Name.Text)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
            DialogResult dialogResult = frmAutoPartsNameQuery.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                txtWhere_STD_Name.Text = frmAutoPartsNameQuery.SelectedText;
            }
        }
        /// <summary>
        /// 列表查询条件dtWhere_ST_StartTime_End_ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_ST_StartTime_End_ValueChanged(object sender, EventArgs e)
        {
            if (this.dtWhere_ST_StartTime_End.Value != null &&
              this.dtWhere_ST_StartTime_End.DateTime.Hour == 0 &&
              this.dtWhere_ST_StartTime_End.DateTime.Minute == 0 &&
              this.dtWhere_ST_StartTime_End.DateTime.Second == 0)
            {
                DateTime newDateTime = new DateTime(this.dtWhere_ST_StartTime_End.DateTime.Year, this.dtWhere_ST_StartTime_End.DateTime.Month, this.dtWhere_ST_StartTime_End.DateTime.Day, 23, 59, 59);
                this.dtWhere_ST_StartTime_End.DateTime = newDateTime;
            }
        }
        /// <summary>
        /// 列表查询条件dtWhere_ST_EndTime_End_ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_ST_EndTime_End_ValueChanged(object sender, EventArgs e)
        {
            if (this.dtWhere_ST_EndTime_End.Value != null &&
              this.dtWhere_ST_EndTime_End.DateTime.Hour == 0 &&
              this.dtWhere_ST_EndTime_End.DateTime.Minute == 0 &&
              this.dtWhere_ST_EndTime_End.DateTime.Second == 0)
            {
                DateTime newDateTime = new DateTime(this.dtWhere_ST_EndTime_End.DateTime.Year, this.dtWhere_ST_EndTime_End.DateTime.Month, this.dtWhere_ST_EndTime_End.DateTime.Day, 23, 59, 59);
                this.dtWhere_ST_EndTime_End.DateTime = newDateTime;
            }
        }
        #endregion

        #region 单头相关事件

        /// <summary>
        /// 【详情】仓库_SelectedIndexChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbST_WH_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(mcbST_WH_ID.SelectedValue))
            {
                return;
            }
            if (_warehouseBinDs == null)
            {
                return;
            }
            //加载选中仓库下仓位List
            var tempWarehouseBinDs = _warehouseBinDs.Where(x => x.WHB_WH_ID == mcbST_WH_ID.SelectedValue.ToString()).ToList();
            mcbST_WHB_ID.DataSource = tempWarehouseBinDs;
        }
        /// <summary>
        /// 显示成本CheckedChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckST_IsShowCost_CheckedChanged(object sender, EventArgs e)
        {
            if (ckST_IsShowCost.Checked == true)
            {
                //显示成本的场合，单头显示[应有金额]、[实际金额]、[金额损失率]，盘点明细列表显示[采购单价]、[应有金额]、[实际金额]、[金额损失率]列
                lblST_DueAmount.Visible = true;
                txtST_DueAmount.Visible = true;
                lblST_ActualAmount.Visible = true;
                txtST_ActualAmount.Visible = true;
                lblST_AmountLossRatio.Visible = true;
                txtST_AmountLossRatio.Visible = true;

                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_Inventory.Code.INV_PurchaseUnitPrice].Hidden = false;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_StocktakingTaskDetail.Code.STD_DueAmount].Hidden = false;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_StocktakingTaskDetail.Code.STD_ActualAmount].Hidden = false;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_StocktakingTaskDetail.Code.STD_AmountLossRatio].Hidden = false;
            }
            else
            {
                //不显示成本的场合，单头不显示[应有金额]、[实际金额]、[金额损失率]，盘点明细列表不显示[采购单价]、[应有金额]、[实际金额]、[金额损失率]列
                lblST_DueAmount.Visible = false;
                txtST_DueAmount.Visible = false;
                lblST_ActualAmount.Visible = false;
                txtST_ActualAmount.Visible = false;
                lblST_AmountLossRatio.Visible = false;
                txtST_AmountLossRatio.Visible = false;

                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_Inventory.Code.INV_PurchaseUnitPrice].Hidden = true;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_StocktakingTaskDetail.Code.STD_DueAmount].Hidden = true;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_StocktakingTaskDetail.Code.STD_ActualAmount].Hidden = true;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_StocktakingTaskDetail.Code.STD_AmountLossRatio].Hidden = true;
            }
        }
        #endregion

        #region 明细toolBar单击事件

        /// <summary>
        /// 查询/清空/导出/加载零库存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolbarsManagerDetail_ToolClick(object sender, ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                //查询盘点明细
                case SystemActionEnum.Code.QUERY:
                    SearchDetail();
                    break;

                //清空盘点明细
                case SystemActionEnum.Code.CLEAR:
                    ClearDetail();
                    break;

                //导出盘点明细
                case SystemActionEnum.Code.EXPORT:
                    ExportDetail();
                    break;

                //加载零库存
                case SysConst.LoadZeroInventory:
                    LoadZeroInventory();
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region 明细列表相关事件

        /// <summary>
        /// 盘点任务明细单元格值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_CellChange(object sender, CellEventArgs e)
        {
            gdDetail.UpdateData();
            var detailRowIndex = gdDetail.Rows[e.Cell.Row.Index];

            if (e.Cell.Column.Key == SystemTableColumnEnums.PIS_StocktakingTaskDetail.Code.STD_ActualQty)
            {

                foreach (var loopDetail in _detailGridDS)
                {
                    if (loopDetail.RowID == detailRowIndex.Cells["RowID"].Text)
                    {
                        //计算差异数量：差异数量 = 应有量 - 实际量
                        loopDetail.STD_AdjustQty = (loopDetail.STD_DueQty ?? 0) - (loopDetail.STD_ActualQty ?? 0);

                        loopDetail.STD_SnapshotQty = (loopDetail.STD_ActualQty ?? 0) - (loopDetail.STD_DueQty ?? 0);

                        //计算实际金额：实际金额 = 实际量 * [库存].采购单价
                        loopDetail.STD_ActualAmount = Math.Round((loopDetail.STD_ActualQty ?? 0) * Convert.ToDecimal(loopDetail.INV_PurchaseUnitPrice ?? 0), 2);

                        //计算金额损失率：金额损失率 = （应有金额 - 实际金额） / 应有金额
                        decimal amountLossRatio = 0;
                        if ((loopDetail.STD_DueAmount ?? 0) == 0)
                        {
                            amountLossRatio = 0;
                        }
                        else
                        {
                            amountLossRatio = Math.Round((Convert.ToDecimal(loopDetail.STD_DueAmount ?? 0) - Convert.ToDecimal(loopDetail.STD_ActualAmount ?? 0)) / Convert.ToDecimal(loopDetail.STD_DueAmount ?? 0), 4);
                        }
                        loopDetail.STD_AmountLossRatio = amountLossRatio;
                        break;
                    }
                }

            }
            gdDetail.UpdateData();
            ////设置盘点单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }
        #endregion

        #region 明细相关命令

        /// <summary>
        /// 上传数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUploadStocktaskingData_Click(object sender, EventArgs e)
        {
            #region 验证

            //当前盘点状态为[已经确认]、[已经取消]或[校正完成]，不能上传数据
            if (cbST_StatusName.Text == StocktakingBillStatusEnum.Name.YJQR ||
                cbST_StatusName.Text == StocktakingBillStatusEnum.Name.YJQX ||
                cbST_StatusName.Text == StocktakingBillStatusEnum.Name.JZWC)
            {
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { MsgParam.CURRENT + MsgParam.STOCKTAKING_STATUS + MsgParam.BE + cbST_StatusName.Text, MsgParam.UPLOAD_DATA }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #endregion

            //上传数据
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = @"txt文档|*.txt",//设置保存文件类型
                    AddExtension = true//设置添加扩展名
                };
                openFileDialog.ShowDialog();
                string openFilePath = openFileDialog.FileName;
                if (string.IsNullOrEmpty(openFilePath))
                {
                    return;
                }

                foreach (var loopStockTakingDetail in _detailGridDS)
                {
                    loopStockTakingDetail.STD_ActualQty = 0;
                }
                FileStream fs = new FileStream(openFilePath, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                string strBarcode = sr.ReadLine();
                while (strBarcode != null)
                {
                    if (strBarcode.Length > 0)
                    {
                        var autoPartsBarcode = strBarcode;
                        var tempStockTakingDetailList = _detailGridDS.Where(x => x.STD_Barcode + x.STD_BatchNo == autoPartsBarcode);
                        foreach (var loopStockTakingItem in tempStockTakingDetailList)
                        {
                            loopStockTakingItem.STD_ActualQty += 1;
                            break;
                        }
                    }
                    strBarcode = sr.ReadLine();
                }
            }
            catch (Exception ex)
            {
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0018, new object[] { MsgParam.UPLOAD_DATA, ex.Message }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //计算明细中各项数据（差异数量、实际金额、金额损失率）
            CalculateStockingTaskDetailData();

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置单元格样式
            SetStocktakingTaskStyle();
        }

        /// <summary>
        /// 生成损益表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerateProfitAndLoss_Click(object sender, EventArgs e)
        {
            #region 1.验证

            //盘点任务未保存，不能生成损益表
            if (string.IsNullOrEmpty(txtST_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_StocktakingTask + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, MsgParam.GENERATE_PROFITANDLOSS
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MDLPIS_StocktakingTask resultStocktakingTask = new MDLPIS_StocktakingTask();
            _bll.QueryForObject<MDLPIS_StocktakingTask, MDLPIS_StocktakingTask>(new MDLPIS_StocktakingTask()
            {
                WHERE_ST_ID = txtST_ID.Text.Trim(),
                WHERE_ST_IsValid = true
            }, resultStocktakingTask);
            //盘点任务不存在，不能生成损益表
            if (string.IsNullOrEmpty(resultStocktakingTask.ST_ID)
                || string.IsNullOrEmpty(resultStocktakingTask.ST_No))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_StocktakingTask + MsgParam.NOTEXIST, MsgParam.GENERATE_PROFITANDLOSS
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = 0; i < gdDetail.Rows.Count; i++)
            {
                //验证实际数量格式
                string actualQtyContent = gdDetail.Rows[i].Cells[SystemTableColumnEnums.PIS_StocktakingTaskDetail.Code.STD_ActualQty].Text.Trim();
                if (string.IsNullOrEmpty(actualQtyContent) || !BLLCom.IsDecimal(actualQtyContent))
                {
                    MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, SystemTableColumnEnums.PIS_StocktakingTaskDetail.Name.STD_ActualQty, MsgParam.POSITIVE_INTEGER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            CalculateStockingTaskData();
            #endregion

            //2.准备数据

            //将【详情】Tab内控件的值赋值给基类的HeadDS
            SetCardCtrlsToDetailDS();

            var tempStocktakingDetailList = _detailGridDS.Where(x => x.STD_ActualQty == null).ToList();
            foreach (var loopStocktakingDetail in tempStocktakingDetailList)
            {
                loopStocktakingDetail.STD_ActualQty = 0;
                loopStocktakingDetail.STD_ActualAmount = 0;
            }

            //3.执行保存（含服务端检查）
            //更新[盘点状态]为[等待确认]
            bool saveDetailResult = _bll.SaveDetailDS(base.HeadDS, _detailGridDS, StocktakingBillStatusEnum.Name.DDQR, StocktakingBillStatusEnum.Code.DDQR);
            if (!saveDetailResult)
            {
                //生成损益表失败
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.GENERATE_PROFITANDLOSS }), MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            //设置详情是否可编辑
            SetDetailControl();

            //设置动作按钮状态
            SetActionEnableByStatus();

            //生成损益表成功
            MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { MsgParam.GENERATE_PROFITANDLOSS }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置盘点明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
            //设置单元格样式
            SetStocktakingTaskStyle();
        }

        /// <summary>
        /// 导出损益表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportProfitAndLoss_Click(object sender, EventArgs e)
        {
            if (gdDetail == null)
            {
                return;
            }

            //盘点单明细列表中[差异数量]不为零的项
            var tempStocktakingTaskGridDs = _detailGridDS.Where(x => (x.STD_AdjustQty ?? 0) != 0).ToList();
            gdDetail.DataSource = tempStocktakingTaskGridDs;
            gdDetail.DataBind();

            //导出库存盘库损益表
            base.ExportAction(gdDetail, SysConst.InventoryStockProfitAndLoss);

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
        }

        /// <summary>
        /// 确认损益
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirmProfitAndLoss_Click(object sender, EventArgs e)
        {
            #region 1.验证

            //验证当前用户是否有[确认库存损益]的权限
            if (toolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[SystemActionEnum.Code.CONFIRMPROFITANDLOSS].SharedPropsInternal.Enabled == false)
            {
                //当前用户没有确认库存损益的权限，您可以联系管理员申请授权。
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.W_0028, new object[] { SystemActionEnum.Name.CONFIRMPROFITANDLOSS }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //盘点任务未保存，不能确认库存损益
            if (string.IsNullOrEmpty(txtST_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_StocktakingTask + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, MsgParam.CONFIRM_PROFITANDLOSS
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MDLPIS_StocktakingTask resultStocktakingTask = new MDLPIS_StocktakingTask();
            _bll.QueryForObject<MDLPIS_StocktakingTask, MDLPIS_StocktakingTask>(new MDLPIS_StocktakingTask()
            {
                WHERE_ST_ID = txtST_ID.Text.Trim(),
                WHERE_ST_IsValid = true
            }, resultStocktakingTask);
            //盘点任务不存在，不能确认库存损益
            if (string.IsNullOrEmpty(resultStocktakingTask.ST_ID)
                || string.IsNullOrEmpty(resultStocktakingTask.ST_No))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_StocktakingTask + MsgParam.NOTEXIST, MsgParam.CONFIRM_PROFITANDLOSS
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //[盘点状态]为[等待确认]的盘点任务才可以确认损益
            if (cbST_StatusName.Text != StocktakingBillStatusEnum.Name.DDQR)
            {
                //当前盘点状态为cbST_StatusName.Text(正在盘库/已经确认/校正完成/已经取消)，不能确认库存损益
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { MsgParam.CURRENT + MsgParam.STOCKTAKING_STATUS + MsgParam.BE + cbST_StatusName.Text, MsgParam.CONFIRM_PROFITANDLOSS }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion

            //2.准备数据

            //生成需要更新的库存列表
            List<MDLPIS_Inventory> updateInventoryList = GetToUpdateInventoryList();
            if (updateInventoryList.Count > 0)
            {
                bool existsInvalidTaskDetail = CheckIfExistsInvalidTaskDetail(updateInventoryList);
                if (existsInvalidTaskDetail)
                {
                    MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "发现 盘点时已丢失，但是正常出库 的配件，\n" + "请重新核实数量" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            //将【详情】Tab内控件的值赋值给基类的HeadDS
            SetCardCtrlsToDetailDS();

            //3.执行保存（含服务端检查）
            //更新[盘点状态]为[已经确认]
            bool saveDetailResult = _bll.SaveDetailDS(base.HeadDS, _detailGridDS, StocktakingBillStatusEnum.Name.YJQR, StocktakingBillStatusEnum.Code.YJQR);
            if (!saveDetailResult)
            {
                //确认库存损益失败
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.CONFIRM_PROFITANDLOSS }), MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            //设置详情是否可编辑
            SetDetailControl();

            //设置动作按钮状态
            SetActionEnableByStatus();

            //确认库存损益成功
            MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { MsgParam.CONFIRM_PROFITANDLOSS }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置盘点明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
            //设置单元格样式
            SetStocktakingTaskStyle();
        }

        /// <summary>
        /// 矫正库存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCorrectInventoryByProfitAndLoss_Click(object sender, EventArgs e)
        {
            #region 1.验证

            //验证当前用户是否具备校正库存的权限
            if (toolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[SystemActionEnum.Code.INVENTORYRECTIFY].SharedPropsInternal.Enabled == false)
            {
                //当前用户没有校正库存的权限，您可以联系管理员申请授权。
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.W_0028, new object[] { MsgParam.ADJUST_INVENTORY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //盘点任务未保存，不能校正库存
            if (string.IsNullOrEmpty(txtST_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_StocktakingTask + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, MsgParam.ADJUST_INVENTORY
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MDLPIS_StocktakingTask resultStocktakingTask = new MDLPIS_StocktakingTask();
            _bll.QueryForObject<MDLPIS_StocktakingTask, MDLPIS_StocktakingTask>(new MDLPIS_StocktakingTask()
            {
                WHERE_ST_ID = txtST_ID.Text.Trim(),
                WHERE_ST_IsValid = true
            }, resultStocktakingTask);
            //盘点任务不存在，不能校正库存
            if (string.IsNullOrEmpty(resultStocktakingTask.ST_ID)
                || string.IsNullOrEmpty(resultStocktakingTask.ST_No))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_StocktakingTask + MsgParam.NOTEXIST, MsgParam.ADJUST_INVENTORY
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //[盘点状态]为[已经确认]的盘点任务才可以校正库存
            if (cbST_StatusName.Text != StocktakingBillStatusEnum.Name.YJQR)
            {
                //当前盘点状态为cbST_StatusName.Text(正在盘库/等待确认/校正完成/已经取消)，不能校正库存
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { MsgParam.CURRENT + MsgParam.STOCKTAKING_STATUS + MsgParam.BE + cbST_StatusName.Text, MsgParam.ADJUST_INVENTORY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //验证仓库是否存在
            MDLPIS_Warehouse resultWarehouse = new MDLPIS_Warehouse();
            _bll.QueryForObject<MDLPIS_Warehouse, MDLPIS_Warehouse>(new MDLPIS_Warehouse
            {
                WHERE_WH_ID = mcbST_WH_ID.SelectedValue?.ToString(),
                WHERE_WH_IsValid = true
            }, resultWarehouse);
            if (string.IsNullOrEmpty(resultWarehouse.WH_ID) || string.IsNullOrEmpty(resultWarehouse.WH_Name))
            {
                //仓库不存在，不能校正库存
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { MsgParam.WAREHOUSE + MsgParam.NOTEXIST, MsgParam.ADJUST_INVENTORY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion

            //2.准备数据

            //生成需要更新的库存列表
            List<MDLPIS_Inventory> updateInventoryList = GetToUpdateInventoryList();
            if (updateInventoryList.Count > 0)
            {
                bool existsInvalidTaskDetail = CheckIfExistsInvalidTaskDetail(updateInventoryList);
                if (existsInvalidTaskDetail)
                {
                    MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "发现 盘点时已丢失，但是正常出库 的配件，\n" + "请 取消确认损益后，重新核实数量" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            //确认损益表无误并校正库存吗？
            DialogResult isAdjustInventory = MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0000, new object[] { "确认损益表无误并校正库存吗？" }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isAdjustInventory != DialogResult.OK)
            {
                return;
            }

            //将【详情】Tab内控件的值赋值给基类的HeadDS
            SetCardCtrlsToDetailDS();

            //3.执行保存（根据损益表校正库存）
            bool adjustInventoryResult = _bll.CorrectInventoryByProfitAndLoss(base.HeadDS, _detailGridDS, updateInventoryList);
            if (!adjustInventoryResult)
            {
                //校正库存失败
                MessageBoxs.Show(Trans.PIS, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            //设置详情是否可编辑
            SetDetailControl();

            //设置动作按钮状态
            SetActionEnableByStatus();

            //校正库存成功
            MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { MsgParam.ADJUST_INVENTORY }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置盘点明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
            //设置单元格样式
            SetStocktakingTaskStyle();
        }

        /// <summary>
        /// 取消确认损益
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelConfirmProfitAndLoss_Click(object sender, EventArgs e)
        {
            #region 1.验证

            //当前用户是否有[取消确认损益]的权限
            if (toolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[SystemActionEnum.Code.CANCELCONFIRMPROFITANDLOSS].SharedPropsInternal.Enabled == false)
            {
                //当前用户没有取消确认库存损益的权限，您可以联系管理员申请授权。
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.W_0028, new object[] { SystemActionEnum.Name.CANCELCONFIRMPROFITANDLOSS }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //盘点任务未保存，不能取消确认损益
            if (string.IsNullOrEmpty(txtST_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_StocktakingTask + MsgParam.NOTYET + SystemActionEnum.Name.SAVE,MsgParam.CANCLE_CONFIRM_PROFITANDLOSS
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MDLPIS_StocktakingTask resultStocktakingTask = new MDLPIS_StocktakingTask();
            _bll.QueryForObject<MDLPIS_StocktakingTask, MDLPIS_StocktakingTask>(new MDLPIS_StocktakingTask()
            {
                WHERE_ST_ID = txtST_ID.Text.Trim(),
                WHERE_ST_IsValid = true
            }, resultStocktakingTask);
            //盘点任务不存在，不能取消确认损益
            if (string.IsNullOrEmpty(resultStocktakingTask.ST_ID)
                || string.IsNullOrEmpty(resultStocktakingTask.ST_No))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_StocktakingTask + MsgParam.NOTEXIST, MsgParam.CANCLE_CONFIRM_PROFITANDLOSS
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //[盘点状态]为[已经确认]的盘点任务才可以取消确认损益
            if (cbST_StatusName.Text != StocktakingBillStatusEnum.Name.YJQR)
            {
                //当前盘点状态为cbST_StatusName.Text(正在盘库/等待确认/校正完成/已经取消)，不能取消确认损益
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { MsgParam.CURRENT + MsgParam.STOCKTAKING_STATUS + MsgParam.BE + cbST_StatusName.Text, MsgParam.CANCLE_CONFIRM_PROFITANDLOSS }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //确定要取消吗？
            DialogResult isCancleConfirm = MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0003, new object[] { MsgParam.CANCLE }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isCancleConfirm != DialogResult.OK)
            {
                return;
            }
            #endregion

            //2.准备数据

            //将【详情】Tab内控件的值赋值给基类的HeadDS
            SetCardCtrlsToDetailDS();

            //3.执行保存（含服务端检查）
            //更新[盘点状态]为[等待确认]
            bool saveDetailResult = _bll.SaveDetailDS(base.HeadDS, _detailGridDS, StocktakingBillStatusEnum.Name.DDQR, StocktakingBillStatusEnum.Code.DDQR);
            if (!saveDetailResult)
            {
                //取消确认库存损益失败
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.CANCELCONFIRMPROFITANDLOSS }), MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            //设置详情是否可编辑
            SetDetailControl();

            //设置动作按钮状态
            SetActionEnableByStatus();

            //取消确认库存损益成功
            MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.CANCELCONFIRMPROFITANDLOSS }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置盘点明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
            //设置单元格样式
            SetStocktakingTaskStyle();
        }

        /// <summary>
        /// 取消盘点任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelStocktasking_Click(object sender, EventArgs e)
        {
            #region 验证

            //盘点任务未保存，不能取消
            if (string.IsNullOrEmpty(txtST_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_StocktakingTask + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, MsgParam.CANCLE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MDLPIS_StocktakingTask resultStocktakingTask = new MDLPIS_StocktakingTask();
            _bll.QueryForObject<MDLPIS_StocktakingTask, MDLPIS_StocktakingTask>(new MDLPIS_StocktakingTask()
            {
                WHERE_ST_ID = txtST_ID.Text.Trim(),
                WHERE_ST_IsValid = true
            }, resultStocktakingTask);
            //盘点任务不存在，不能取消
            if (string.IsNullOrEmpty(resultStocktakingTask.ST_ID)
                || string.IsNullOrEmpty(resultStocktakingTask.ST_No))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_StocktakingTask + MsgParam.NOTEXIST, MsgParam.CANCLE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //[盘点状态]为[校正完成/已经取消]的盘点任务不可以取消
            if (cbST_StatusName.Text == StocktakingBillStatusEnum.Name.JZWC
                || cbST_StatusName.Text == StocktakingBillStatusEnum.Name.YJQX)
            {
                //当前盘点状态为cbST_StatusName.Text(校正完成/已经取消)，不能取消盘点任务
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { MsgParam.CURRENT + MsgParam.STOCKTAKING_STATUS + MsgParam.BE + cbST_StatusName.Text, MsgParam.CANCLE_CONFIRM_PROFITANDLOSS }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //确定要取消盘点任务吗？
            DialogResult isCancleStockingtask = MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0003, new object[] { MsgParam.CANCLE_STOCKTAKINGTASK }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isCancleStockingtask != DialogResult.OK)
            {
                return;
            }
            #endregion

            //2.准备数据

            //将【详情】Tab内控件的值赋值给基类的HeadDS
            SetCardCtrlsToDetailDS();

            //3.执行保存（含服务端检查）
            //更新[盘点状态]为[已经取消]
            bool saveDetailResult = _bll.SaveDetailDS(base.HeadDS, _detailGridDS, StocktakingBillStatusEnum.Name.YJQX, StocktakingBillStatusEnum.Code.YJQX);
            if (!saveDetailResult)
            {
                //取消盘点任务失败
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.CANCLE_STOCKTAKINGTASK }), MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            //设置详情是否可编辑
            SetDetailControl();

            //设置动作按钮状态
            SetActionEnableByStatus();

            //取消盘点任务成功
            MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { MsgParam.CANCLE_STOCKTAKINGTASK }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置盘点明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
            //设置单元格样式
            SetStocktakingTaskStyle();
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

            //设置详情是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();
            //设置单元格样式
            SetStocktakingTaskStyle();

            //将最新的值Copy到初始UIModel
            SetCardCtrlsToDetailDS();
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
            //计算单头各项数据
            CalculateStockingTaskData();

            //2.将【详情】Tab内控件的值赋值给基类的HeadDS
            SetCardCtrlsToDetailDS();
            //3.执行保存（含服务端检查）
            bool saveDetailResult = _bll.SaveDetailDS(base.HeadDS, _detailGridDS, HeadDS.ST_StatusName, HeadDS.ST_StatusCode);
            if (!saveDetailResult)
            {
                //保存失败
                MessageBoxs.Show(Trans.PIS, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //4.开始监控List变化
            _detailGridDS.StartMonitChanges();
            //5.将HeadDS数据赋值给【详情】Tab内的对应控件
            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置详情是否可编辑
            SetDetailControl();

            //设置动作按钮状态
            SetActionEnableByStatus();

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置盘点单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
            //设置单元格样式
            SetStocktakingTaskStyle();
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
            var argsDetail = new List<MDLPIS_StocktakingTaskDetail>();
            //将HeadDS转换为TBModel对象
            var argsHead = base.HeadDS.ToTBModelForSaveAndDelete<MDLPIS_StocktakingTask>();
            //将当前DetailGridDS转换为指定类型的TBModelList
            _detailGridDS.ToTBModelListForUpdateAndDelete<MDLPIS_StocktakingTaskDetail>(argsDetail);
            //过滤明细列表中未保存的数据
            argsDetail = argsDetail.Where(x => !string.IsNullOrEmpty(x.WHERE_STD_ID)).ToList();
            //2.执行删除
            bool deleteDetailResult = _bll.UnityDelete<MDLPIS_StocktakingTask, MDLPIS_StocktakingTaskDetail>(argsHead,
                argsDetail);
            if (!deleteDetailResult)
            {
                //删除失败
                MessageBoxs.Show(Trans.PIS, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //删除成功
            MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //3.清空【详情】画面数据
            InitializeDetailTabControls();
            //刷新列表
            RefreshList(true);

            //将最新的值Copy到初始UIModel
            SetCardCtrlsToDetailDS();
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
            base.ConditionDS = new StocktakingTaskManagerQCModel()
            {
                //查询用SqlId
                SqlId = SQLID.PIS_StocktakingTaskManager_SQL_01,
                //盘点单号
                WHERE_ST_No = txtWhere_ST_No.Text.Trim(),
                //仓库
                WHERE_ST_WH_ID = mcbWhere_ST_WH_ID.SelectedValue?.ToString(),
                //仓位
                WHERE_ST_WHB_ID = mcbWhere_ST_WHB_ID.SelectedValue?.ToString(),
                //盘点结果名称
                WHERE_ST_CheckResultName = cbWhere_ST_CheckResultName.Text.Trim(),
                //盘点单状态名称
                WHERE_ST_StatusName = cbWhere_ST_StatusName.Text.Trim(),
                //有效
                WHERE_ST_IsValid = ckWhere_ST_IsValid.Checked,
                //组织
                WHERE_ST_Org_ID = LoginInfoDAX.OrgID,
            };

            //开始时间（开始）
            if (dtWhere_ST_StartTime_Start.Value != null)
            {
                ConditionDS._StartTimeStart = dtWhere_ST_StartTime_Start.DateTime;
            }
            //开始时间（终了）
            if (dtWhere_ST_StartTime_End.Value != null)
            {
                ConditionDS._StartTimeEnd = dtWhere_ST_StartTime_End.DateTime;
            }
            //结束时间（开始）
            if (dtWhere_ST_EndTime_Start.Value != null)
            {
                ConditionDS._EndTimeStart = dtWhere_ST_EndTime_Start.DateTime;
            }
            //结束时间（终了）
            if (dtWhere_ST_EndTime_End.Value != null)
            {
                ConditionDS._EndTimeEnd = dtWhere_ST_EndTime_End.DateTime;
            }

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
        /// 启动盘点
        /// </summary>
        public override void StartStockTaskAction()
        {
            #region 验证

            //验证仓库
            if (string.IsNullOrEmpty(mcbST_WH_ID.SelectedValue))
            {
                //请选择仓库进行启动盘点
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.W_0006, new object[] { MsgParam.WAREHOUSE, SystemActionEnum.Name.STARTSTOCKTASK }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //验证当前仓库是否存在未完成的盘点任务（同一仓库，不同仓位可以一起盘库）
            var incompleteCount = _bll.QueryForObject<int>(SQLID.PIS_StocktakingTaskManager_SQL_05, new MDLPIS_StocktakingTask
            {
                WHERE_ST_WH_ID = mcbST_WH_ID.SelectedValue.ToString(),
                WHERE_ST_WHB_ID = mcbST_WHB_ID.SelectedValue?.ToString(),
            });
            if (incompleteCount > 0)
            {
                //cbST_WH_ID.Text已存在未完成的盘点任务，不能启动盘点
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { mcbST_WH_ID.SelectedText + MsgParam.ALREADYEXIST + MsgParam.INCOMPLETE + MsgParam.OF + SystemTableEnums.Name.PIS_StocktakingTask, SystemActionEnum.Name.STARTSTOCKTASK }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #endregion

            //开始时间为当前时间
            dtST_StartTime.Value = BLLCom.GetCurStdDatetime();
            //更新[盘点状态]为[正在盘库]
            cbST_StatusName.Text = StocktakingBillStatusEnum.Name.ZZPK;
            cbST_StatusName.Value = StocktakingBillStatusEnum.Code.ZZPK;
            txtST_StatusCode.Text = StocktakingBillStatusEnum.Code.ZZPK;

            //查询当前仓库下的库存信息
            SearchDetail();

            //设置详情是否可编辑
            SetDetailControl();

            //设置动作按钮状态
            SetActionEnableByStatus();
        }
        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 工具生成
            //盘点单号
            txtST_No.Clear();
            //仓库
            mcbST_WH_ID.Clear();
            //仓位
            mcbST_WHB_ID.Clear();
            //盘点次数
            txtST_CheckAmount.Clear();
            //开始时间
            dtST_StartTime.Value = null;
            //结束时间
            dtST_EndTime.Value = null;
            //显示成本
            ckST_IsShowCost.Checked = true;
            ckST_IsShowCost.CheckState = CheckState.Checked;
            //应有库存量
            txtST_DueQty.Clear();
            //实际库存量
            txtST_ActualQty.Clear();
            //数量损失率
            txtST_QtyLossRatio.Clear();
            //应有库存金额
            txtST_DueAmount.Clear();
            //实际库存金额
            txtST_ActualAmount.Clear();
            //金额损失率
            txtST_AmountLossRatio.Clear();
            //盘点结果
            cbST_CheckResultName.Items.Clear();
            //盘点单状态
            cbST_StatusName.Items.Clear();
            //审核状态
            cbST_ApprovalStatusName.Items.Clear();
            //有效
            ckST_IsValid.Checked = true;
            ckST_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtST_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtST_CreatedTime.Value = DateTime.Now;
            //修改人
            txtST_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtST_UpdatedTime.Value = DateTime.Now;
            //盘点任务ID
            txtST_ID.Clear();
            //组织ID
            txtST_Org_ID.Clear();
            //盘点结果编码
            txtST_CheckResultCode.Clear();
            //盘点单状态编码
            txtST_StatusCode.Clear();
            //审核状态编码
            txtST_ApprovalStatusCode.Clear();
            //版本号
            txtST_VersionNo.Clear();
            //组织简称
            txtOrg_ShortName.Clear();
            //给 盘点单号 设置焦点
            lblST_No.Focus();
            //清空[明细]Grid
            _detailGridDS = new SkyCarBindingList<StocktakingTaskManagerDetailUIModel, MDLPIS_StocktakingTaskDetail>();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();

            //开始监控List变化（该数据源若有其他调整，请在StartMonitChanges()方法执行前调整）
            _detailGridDS.StartMonitChanges();
            #endregion

            #region 初始化下拉框

            #region 仓库List

            //获取当前组织下的仓库列表
            if (_warehouseDs.Count == 0)
            {
                _warehouseDs = BLLCom.GetWarehouseList(LoginInfoDAX.OrgID);
            }
            mcbST_WH_ID.DisplayMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name;
            mcbST_WH_ID.ValueMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_ID;
            mcbST_WH_ID.DataSource = _warehouseDs;

            #endregion

            #region 仓位List

            //获取当前组织下的仓位列表
            if (_warehouseBinDs.Count == 0)
            {
                _warehouseBinDs = BLLCom.GetWarehouseBinList(LoginInfoDAX.OrgID);
            }
            mcbST_WHB_ID.DisplayMember = SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name;
            mcbST_WHB_ID.ValueMember = SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_ID;
            mcbST_WHB_ID.DataSource = _warehouseBinDs;

            #endregion

            #region 盘点结果

            //盘点结果
            _stocktakingBillCheckResultDs = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.StocktakingBillCheckResult);
            cbST_CheckResultName.DisplayMember = SysConst.EN_TEXT;
            cbST_CheckResultName.ValueMember = SysConst.EN_Code;
            cbST_CheckResultName.DataSource = _stocktakingBillCheckResultDs;
            cbST_CheckResultName.DataBind();

            #endregion

            #region 盘点单状态

            //盘点单状态
            _stocktakingBillStatusDs = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.StocktakingBillStatus);
            cbST_StatusName.DisplayMember = SysConst.EN_TEXT;
            cbST_StatusName.ValueMember = SysConst.EN_Code;
            cbST_StatusName.DataSource = _stocktakingBillStatusDs;
            cbST_StatusName.DataBind();
            #endregion

            #region 审核状态

            //审核状态
            _approvalStatusDs = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.ApprovalStatus);
            cbST_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
            cbST_ApprovalStatusName.ValueMember = SysConst.EN_Code;
            cbST_ApprovalStatusName.DataSource = _approvalStatusDs;
            cbST_ApprovalStatusName.DataBind();
            //默认审核状态为[待审核]
            cbST_ApprovalStatusName.Text = ApprovalStatusEnum.Name.DSH;
            txtST_ApprovalStatusCode.Text = ApprovalStatusEnum.Code.DSH;
            #endregion

            #endregion

            //默认组织为当前组织
            txtST_Org_ID.Text = LoginInfoDAX.OrgID;

            #region 现在是控制单元格是否可以编辑，直接控制列

            //明细中 [实际量]列 不可编辑
            //gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_StocktakingTaskDetail.Code.STD_ActualQty].CellActivation = Activation.ActivateOnly;

            #endregion
        }

        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //盘点单号
            txtWhere_ST_No.Clear();
            //仓库
            mcbWhere_ST_WH_ID.Clear();
            //仓位
            mcbWhere_ST_WHB_ID.Clear();
            //开始时间-开始
            dtWhere_ST_StartTime_Start.Value = null;
            //开始时间-终了
            dtWhere_ST_StartTime_End.Value = null;
            //结束时间-开始
            dtWhere_ST_EndTime_Start.Value = null;
            //结束时间-终了
            dtWhere_ST_EndTime_End.Value = null;
            //盘点结果名称
            cbWhere_ST_CheckResultName.Items.Clear();
            //盘点单状态名称
            cbWhere_ST_StatusName.Items.Clear();
            //有效
            ckWhere_ST_IsValid.Checked = true;
            ckWhere_ST_IsValid.CheckState = CheckState.Checked;
            //给 盘点单号 设置焦点
            lblWhere_ST_No.Focus();
            #endregion

            #region Grid初始化

            //清空Grid
            HeadGridDS = new BindingList<StocktakingTaskManagerUIModel>();
            gdGrid.DataSource = HeadGridDS;
            gdGrid.DataBind();

            #endregion

            #endregion

            #region 初始化下拉框

            #region 仓库List

            //仓库
            mcbWhere_ST_WH_ID.DisplayMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name;
            mcbWhere_ST_WH_ID.ValueMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_ID;
            mcbWhere_ST_WH_ID.DataSource = _warehouseDs;


            #endregion

            #region 仓位List

            //仓位
            mcbWhere_ST_WHB_ID.DisplayMember = SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name;
            mcbWhere_ST_WHB_ID.ValueMember = SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_ID;
            mcbWhere_ST_WHB_ID.DataSource = _warehouseBinDs;

            #endregion

            #region 盘点结果

            //盘点结果
            _stocktakingBillCheckResultDs = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.StocktakingBillCheckResult);
            cbWhere_ST_CheckResultName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_ST_CheckResultName.ValueMember = SysConst.EN_Code;
            cbWhere_ST_CheckResultName.DataSource = _stocktakingBillCheckResultDs;
            cbWhere_ST_CheckResultName.DataBind();

            #endregion

            #region 盘点单状态

            //盘点单状态
            cbWhere_ST_StatusName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_ST_StatusName.ValueMember = SysConst.EN_Code;
            cbWhere_ST_StatusName.DataSource = _stocktakingBillStatusDs;
            cbWhere_ST_StatusName.DataBind();
            #endregion

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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_StocktakingTask.Code.ST_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_StocktakingTask.Code.ST_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[HeadDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            base.HeadDS = base.HeadGridDS.FirstOrDefault(x => x.ST_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_StocktakingTask.Code.ST_ID].Value);
            if (HeadDS == null || string.IsNullOrEmpty(HeadDS.ST_ID))
            {
                return;
            }

            if (txtST_ID.Text != HeadDS.ST_ID
                || (txtST_ID.Text == HeadDS.ST_ID && txtST_VersionNo.Text != HeadDS.ST_VersionNo?.ToString()))
            {
                if (txtST_ID.Text == HeadDS.ST_ID && txtST_VersionNo.Text != HeadDS.ST_VersionNo?.ToString())
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

            //设置详情是否可编辑
            SetDetailControl();

            //设置动作按钮状态
            SetActionEnableByStatus();

            //查询明细Grid数据并绑定
            QueryDetail();
        }

        /// <summary>
        /// 将HeadDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //盘点单号
            txtST_No.Text = base.HeadDS.ST_No;
            //仓库
            mcbST_WH_ID.SelectedValue = base.HeadDS.ST_WH_ID;
            //仓位
            mcbST_WHB_ID.SelectedValue = base.HeadDS.ST_WHB_ID;
            //盘点次数
            txtST_CheckAmount.Value = base.HeadDS.ST_CheckAmount;
            //开始时间
            dtST_StartTime.Value = base.HeadDS.ST_StartTime;
            //结束时间
            dtST_EndTime.Value = base.HeadDS.ST_EndTime;
            //显示成本
            if (base.HeadDS.ST_IsShowCost != null)
            {
                ckST_IsShowCost.Checked = base.HeadDS.ST_IsShowCost.Value;
            }
            //应有库存量
            txtST_DueQty.Value = base.HeadDS.ST_DueQty;
            //实际库存量
            txtST_ActualQty.Value = base.HeadDS.ST_ActualQty;
            //数量损失率
            txtST_QtyLossRatio.Value = base.HeadDS.ST_QtyLossRatio;
            //应有库存金额
            txtST_DueAmount.Value = base.HeadDS.ST_DueAmount;
            //实际库存金额
            txtST_ActualAmount.Value = base.HeadDS.ST_ActualAmount;
            //金额损失率
            txtST_AmountLossRatio.Value = base.HeadDS.ST_AmountLossRatio;
            //盘点结果
            cbST_CheckResultName.Text = base.HeadDS.ST_CheckResultName ?? string.Empty;
            //盘点单状态
            cbST_StatusName.Text = base.HeadDS.ST_StatusName ?? string.Empty;
            //审核状态
            cbST_ApprovalStatusName.Text = base.HeadDS.ST_ApprovalStatusName ?? string.Empty;
            //有效
            if (base.HeadDS.ST_IsValid != null)
            {
                ckST_IsValid.Checked = base.HeadDS.ST_IsValid.Value;
            }
            //创建人
            txtST_CreatedBy.Text = base.HeadDS.ST_CreatedBy;
            //创建时间
            dtST_CreatedTime.Value = base.HeadDS.ST_CreatedTime;
            //修改人
            txtST_UpdatedBy.Text = base.HeadDS.ST_UpdatedBy;
            //修改时间
            dtST_UpdatedTime.Value = base.HeadDS.ST_UpdatedTime;
            //盘点任务ID
            txtST_ID.Text = base.HeadDS.ST_ID;
            //组织ID
            txtST_Org_ID.Text = base.HeadDS.ST_Org_ID;
            //盘点结果编码
            txtST_CheckResultCode.Text = base.HeadDS.ST_CheckResultCode;
            //盘点单状态编码
            txtST_StatusCode.Text = base.HeadDS.ST_StatusCode;
            //审核状态编码
            txtST_ApprovalStatusCode.Text = base.HeadDS.ST_ApprovalStatusCode;
            //版本号
            txtST_VersionNo.Value = base.HeadDS.ST_VersionNo;
            //组织简称
            txtOrg_ShortName.Text = base.HeadDS.Org_ShortName;
        }
        
        /// <summary>
        /// 查询明细Grid数据并绑定
        /// </summary>
        private void QueryDetail()
        {
            //1.设置查询条件
            var argsCondition = new StocktakingTaskManagerQCModel()
            {
                //查询用SqlId
                SqlId = SQLID.PIS_StocktakingTaskManager_SQL_02,
                //盘点任务ID
                WHERE_STD_TB_ID = txtST_ID.Text.Trim(),
                //盘点单号
                WHERE_STD_TB_No = txtST_No.Text.Trim(),
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
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
            //设置单元格样式
            SetStocktakingTaskStyle();
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
            //验证仓库
            if (string.IsNullOrEmpty(mcbST_WH_ID.SelectedValue) || string.IsNullOrEmpty(mcbST_WH_ID.SelectedText))
            {
                //请选择仓库
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.WAREHOUSE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            #region 验证盘点单明细

            if (gdDetail.Rows.Count == 0)
            {
                //盘库任务不存在库存，不能保存
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableEnums.Name.PIS_StocktakingTask + MsgParam.NOTEXIST + SystemTableEnums.Name.PIS_Inventory, SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
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
            DialogResult dialogResult = MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0012), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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
            base.HeadDS = new StocktakingTaskManagerUIModel()
            {
                //盘点单号
                ST_No = txtST_No.Text.Trim(),
                //仓库ID
                ST_WH_ID = mcbST_WH_ID.SelectedValue?.ToString(),
                //仓位ID
                ST_WHB_ID = mcbST_WHB_ID.SelectedValue?.ToString(),
                //盘点次数
                ST_CheckAmount = Convert.ToInt32(txtST_CheckAmount.Text.Trim() == "" ? "1" : txtST_CheckAmount.Text.Trim()),
                //开始时间
                ST_StartTime = (DateTime?)dtST_StartTime.Value,
                //结束时间
                ST_EndTime = (DateTime?)dtST_EndTime.Value,
                //显示成本
                ST_IsShowCost = ckST_IsShowCost.Checked,
                //应有库存量
                ST_DueQty = Convert.ToDecimal(txtST_DueQty.Text.Trim() == "" ? "1" : txtST_DueQty.Text.Trim()),
                //实际库存量
                ST_ActualQty = Convert.ToDecimal(txtST_ActualQty.Text.Trim() == "" ? "1" : txtST_ActualQty.Text.Trim()),
                //数量损失率
                ST_QtyLossRatio = Convert.ToDecimal(txtST_QtyLossRatio.Text.Trim() == "" ? "1" : txtST_QtyLossRatio.Text.Trim()),
                //应有库存金额
                ST_DueAmount = Convert.ToDecimal(txtST_DueAmount.Text.Trim() == "" ? "1" : txtST_DueAmount.Text.Trim()),
                //实际库存金额
                ST_ActualAmount = Convert.ToDecimal(txtST_ActualAmount.Text.Trim() == "" ? "1" : txtST_ActualAmount.Text.Trim()),
                //金额损失率
                ST_AmountLossRatio = Convert.ToDecimal(txtST_AmountLossRatio.Text.Trim() == "" ? "1" : txtST_AmountLossRatio.Text.Trim()),
                //盘点结果名称
                ST_CheckResultName = cbST_CheckResultName.Text.Trim(),
                //盘点单状态名称
                ST_StatusName = cbST_StatusName.Text.Trim(),
                //审核状态名称
                ST_ApprovalStatusName = cbST_ApprovalStatusName.Text.Trim(),
                //有效
                ST_IsValid = ckST_IsValid.Checked,
                //创建人
                ST_CreatedBy = txtST_CreatedBy.Text.Trim(),
                //创建时间
                ST_CreatedTime = (DateTime?)dtST_CreatedTime.Value ?? DateTime.Now,
                //修改人
                ST_UpdatedBy = txtST_UpdatedBy.Text.Trim(),
                //修改时间
                ST_UpdatedTime = (DateTime?)dtST_UpdatedTime.Value ?? DateTime.Now,
                //盘点任务ID
                ST_ID = txtST_ID.Text.Trim(),
                //组织ID
                ST_Org_ID = txtST_Org_ID.Text.Trim(),
                //盘点结果编码
                ST_CheckResultCode = txtST_CheckResultCode.Text.Trim(),
                //盘点单状态编码
                ST_StatusCode = txtST_StatusCode.Text.Trim(),
                //审核状态编码
                ST_ApprovalStatusCode = txtST_ApprovalStatusCode.Text.Trim(),
                //版本号
                ST_VersionNo = Convert.ToInt64(txtST_VersionNo.Text.Trim() == "" ? "1" : txtST_VersionNo.Text.Trim()),
                //组织简称
                Org_ShortName = txtOrg_ShortName.Text.Trim(),
            };
        }

        #region 明细相关

        /// <summary>
        /// 查询明细
        /// </summary>
        private void SearchDetail()
        {
            //查询当前仓库的库存
            _bll.QueryForList(SQLID.PIS_StocktakingTaskManager_SQL_03, new StocktakingTaskManagerQCModel()
            {
                //组织ID
                WHERE_ST_Org_ID = txtST_Org_ID.Text.Trim(),
                //仓库ID
                WHERE_ST_WH_ID = mcbST_WH_ID.SelectedValue?.ToString(),
                //仓位ID
                WHERE_ST_WHB_ID = mcbST_WHB_ID.SelectedValue?.ToString(),
                //配件名称
                WHERE_STD_Name = txtWhere_STD_Name.Text.Trim(),
                //配件条形码
                WHERE_STD_Barcode = txtWhere_STD_Barcode.Text.Trim(),
                STD_CreatedBy = LoginInfoDAX.UserName,
                STD_UpdatedBy = LoginInfoDAX.UserName
            }, _detailGridDS);

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();

            //设置盘点单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
            //设置单元格样式
            SetStocktakingTaskStyle();
        }

        /// <summary>
        /// 清空明细
        /// </summary>
        private void ClearDetail()
        {
            //清空明细的查询条件
            txtWhere_STD_Name.Clear();
            txtWhere_STD_Barcode.Clear();

            //清空明细列表
            _detailGridDS = new SkyCarBindingList<StocktakingTaskManagerDetailUIModel, MDLPIS_StocktakingTaskDetail>();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
        }

        /// <summary>
        /// 导出明细
        /// </summary>
        private void ExportDetail()
        {
            base.ExportAction(gdDetail, SystemTableEnums.Name.PIS_StocktakingTaskDetail);
        }

        /// <summary>
        /// 加载零库存
        /// </summary>
        private void LoadZeroInventory()
        {
            #region 验证

            //验证当前用户有无[盘点零库存]权限
            if (toolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[SystemActionEnum.Code.STOCKCOUNTZEROINVENTORY].SharedPropsInternal.Enabled == false)
            {
                //当前用户没有盘点零库存权限\n 您可以\n 导出零库存并将盘盈的配件入库 \n 或 \n 联系管理员申请授权。
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.W_0027, new object[] { SystemActionEnum.Name.STOCKCOUNTZEROINVENTORY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //当前盘点状态为[校正完成] 或 [已经取消]，不能加载零库存
            if (cbST_StatusName.Text == StocktakingBillStatusEnum.Name.JZWC
                || cbST_StatusName.Text == StocktakingBillStatusEnum.Name.YJQX)
            {
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { MsgParam.CURRENT + MsgParam.STOCKTAKING_STATUS + MsgParam.BE + cbST_StatusName.Text, MsgParam.LOAD_ZEROINVENTORY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #endregion

            //将【详情】Tab内控件的值赋值给基类的HeadDS
            SetCardCtrlsToDetailDS();

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                [PISViewParamKey.StocktakingBill.ToString()] = HeadDS,
                [PISViewParamKey.StocktakingDetailList.ToString()] = null
            };
            //查询零库存
            FrmSelectAutoPartsZeroInventoryWindow frmSelectAutoPartsZeroInventoryWindow = new FrmSelectAutoPartsZeroInventoryWindow(paramViewParameters, CustomEnums.CustomeSelectionMode.Multiple)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            DialogResult dialogResult = frmSelectAutoPartsZeroInventoryWindow.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            List<StocktakingTaskManagerDetailUIModel> selectedZeroInventoryList = new List<StocktakingTaskManagerDetailUIModel>();
            selectedZeroInventoryList = frmSelectAutoPartsZeroInventoryWindow.SelectedGridList;

            //添加零库存
            foreach (var loopSelectedZeroInventory in selectedZeroInventoryList)
            {
                if (_detailGridDS.Count == 0
                    || !_detailGridDS.All(x => x.STD_Barcode == loopSelectedZeroInventory.STD_Barcode && x.STD_BatchNo == loopSelectedZeroInventory.STD_BatchNo))
                {
                    loopSelectedZeroInventory.IsChecked = false;
                    loopSelectedZeroInventory.RowID = Guid.NewGuid().ToString();
                    _detailGridDS.Add(loopSelectedZeroInventory);
                }
            }

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置盘点单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
            //设置单元格样式
            SetStocktakingTaskStyle();
        }

        #endregion

        /// <summary>
        /// 设置详情页面控件的是否可编辑
        /// </summary>
        private void SetDetailControl()
        {
            if (string.IsNullOrEmpty(cbST_StatusName.Text))
            {
                //盘点单状态为空的场合，[仓库]、[仓位]可编辑
                mcbST_WH_ID.Enabled = true;
                mcbST_WHB_ID.Enabled = true;

                //不显示明细
                gbDetail.Visible = false;
            }
            else
            {
                //盘点单状态不为空的场合，[仓库]、[仓位]不可编辑
                mcbST_WH_ID.Enabled = false;
                mcbST_WHB_ID.Enabled = false;

                //显示明细
                gbDetail.Visible = true;

                #region 设置明细列表中[实际量]是否可编辑

                if (cbST_StatusName.Text == StocktakingBillStatusEnum.Name.ZZPK
                    || cbST_StatusName.Text == StocktakingBillStatusEnum.Name.DDQR)
                {

                    ckST_IsShowCost.Enabled = true;
                }
                else
                {

                    ckST_IsShowCost.Enabled = false;
                }

                #endregion

                #region 设置明细列表中[调整量]是否可编辑

                //if (cbST_StatusName.Text == StocktakingBillStatusEnum.Name.YJQR)
                //{
                //    //盘点单状态为 [已经确认] 的场合，明细列表中[调整量]可编辑
                //    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_StocktakingTaskDetail.Code.STD_SnapshotQty].CellActivation = Activation.AllowEdit;
                //}
                //else
                //{
                //    //盘点单状态为 [正在盘库]、[等待确认]、[校正完成] 或 [已经取消]  的场合，明细列表中[调整量]不可编辑
                //    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_StocktakingTaskDetail.Code.STD_SnapshotQty].CellActivation = Activation.ActivateOnly;
                //}
                #endregion
            }

            #region 设置警告Label是否显示

            if (cbST_StatusName.Text == StocktakingBillStatusEnum.Name.ZZPK)
            {
                //盘点单状态为 [正在盘库] 的场合，显示警告
                lblWarningText.Visible = true;
            }
            else
            {
                //其他场合不显示警告
                lblWarningText.Visible = false;
            }

            #endregion
        }

        /// <summary>
        /// 设置动作按钮状态
        /// </summary>
        private void SetActionEnableByStatus()
        {
            if (string.IsNullOrEmpty(cbST_StatusName.Text))
            {
                //盘点单状态为空的场合，[保存]、[删除]不可用，[启动盘点]可用
                SetActionEnable(SystemActionEnum.Code.SAVE, false);
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                SetActionEnable(SystemActionEnum.Code.STARTSTOCKTASK, true);
            }
            else if (cbST_StatusName.Text == StocktakingBillStatusEnum.Name.ZZPK)
            {
                //盘点单状态为 [正在盘库]的场合，[保存]可用，[删除]、[启动盘点]不可用
                SetActionEnable(SystemActionEnum.Code.SAVE, true);
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                SetActionEnable(SystemActionEnum.Code.STARTSTOCKTASK, false);
            }
            else if (cbST_StatusName.Text == StocktakingBillStatusEnum.Name.DDQR
                || cbST_StatusName.Text == StocktakingBillStatusEnum.Name.YJQR)
            {
                //盘点单状态为 [等待确认] 或 [已经确认] 的场合，[保存]、[删除]可用，[启动盘点]不可用
                SetActionEnable(SystemActionEnum.Code.SAVE, true);
                SetActionEnable(SystemActionEnum.Code.DELETE, !string.IsNullOrEmpty(txtST_ID.Text));
                SetActionEnable(SystemActionEnum.Code.STARTSTOCKTASK, false);
            }
            else if (cbST_StatusName.Text == StocktakingBillStatusEnum.Name.JZWC)
            {
                //盘点单状态为 [校正完成] 的场合，[保存]、[删除]、[启动盘点]不可用
                SetActionEnable(SystemActionEnum.Code.SAVE, false);
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                SetActionEnable(SystemActionEnum.Code.STARTSTOCKTASK, false);
            }
            else if (cbST_StatusName.Text == StocktakingBillStatusEnum.Name.YJQX)
            {
                //盘点单状态为 [已经取消] 的场合，[删除]可用，[保存]、[启动盘点]不可用
                SetActionEnable(SystemActionEnum.Code.SAVE, false);
                SetActionEnable(SystemActionEnum.Code.DELETE, !string.IsNullOrEmpty(txtST_ID.Text));
                SetActionEnable(SystemActionEnum.Code.STARTSTOCKTASK, false);
            }
        }
        
        /// <summary>
        /// 设置单元格样式
        /// </summary>
        private void SetStocktakingTaskStyle()
        {
            #region 设置Grid数据颜色

            gdDetail.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
            foreach (var loopGridRow in gdDetail.Rows)
            {
                #region 设置明细列表中[实际量]是否可编辑

                if (cbST_StatusName.Text == StocktakingBillStatusEnum.Name.ZZPK
                    || cbST_StatusName.Text == StocktakingBillStatusEnum.Name.DDQR)
                {
                    //盘点单状态为 [正在盘库] 或 [等待确认] 的场合，明细列表中[实际量]可编辑
                    #region 实际量

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StocktakingTaskDetail.Code.STD_ActualQty].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StocktakingTaskDetail.Code.STD_ActualQty].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StocktakingTaskDetail.Code.STD_ActualQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                }
                else
                {
                    //盘点单状态为 [已经确认]、[校正完成] 或 [已经取消] 的场合，明细列表中[实际量]不可编辑
                    #region 实际量

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StocktakingTaskDetail.Code.STD_ActualQty].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StocktakingTaskDetail.Code.STD_ActualQty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StocktakingTaskDetail.Code.STD_ActualQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion
                }

                #endregion
            }

            #endregion
        }
        
        /// <summary>
        /// 计算单头各项数据
        /// </summary>
        private void CalculateStockingTaskData()
        {
            if (gdDetail.Rows.Count == 0)
            {
                return;
            }

            //[应有库存量]
            decimal dueQty = 0;
            //[实际库存量]
            decimal actualQty = 0;
            //[应有库存金额]
            decimal dueAmount = 0;
            //[实际库存金额]
            decimal actualAmount = 0;
            //[数量损失率]
            decimal qtyLossRatio = 0;
            //[金额损失率]
            decimal amountLossRatio = 0;

            foreach (var loopDetail in _detailGridDS)
            {
                dueQty = dueQty + (loopDetail.STD_DueQty ?? 0);
                dueAmount = dueAmount + (loopDetail.STD_DueAmount ?? 0);
                actualQty = actualQty + (loopDetail.STD_ActualQty ?? 0);
                actualAmount = actualAmount + (loopDetail.STD_ActualAmount ?? 0);
            }

            //计算[数量损失率]：数量损失率 = （应有库存量 - 实际库存量） / 应有库存量
            qtyLossRatio = dueQty > 0 ? Math.Round((dueQty - actualQty) / dueQty, 4) : 0;
            //计算[金额损失率]：金额损失率 = （应有库存金额 - 实际库存金额） / 应有库存金额
            amountLossRatio = dueAmount > 0 ? Math.Round((dueAmount - actualAmount) / dueAmount, 4) : 0;

            txtST_DueQty.Value = dueQty;
            txtST_ActualQty.Value = actualQty;
            txtST_DueAmount.Value = dueAmount;
            txtST_ActualAmount.Value = actualAmount;
            txtST_QtyLossRatio.Value = qtyLossRatio;
            txtST_AmountLossRatio.Value = amountLossRatio;
        }

        /// <summary>
        /// 计算明细中各项数据
        /// </summary>
        private void CalculateStockingTaskDetailData()
        {
            if (_detailGridDS == null)
            {
                return;
            }

            foreach (var loopDetail in _detailGridDS)
            {
                //计算差异数量：差异数量 = 应有量 - 实际量
                loopDetail.STD_AdjustQty = (loopDetail.STD_DueQty ?? 0) - (loopDetail.STD_ActualQty ?? 0);

                //计算实际金额：实际金额 = 实际量 * [库存].采购单价
                loopDetail.STD_ActualAmount = Math.Round((loopDetail.STD_ActualQty ?? 0) * (loopDetail.INV_PurchaseUnitPrice ?? 0), 2);

                //计算金额损失率：金额损失率 = （应有金额 - 实际金额） / 应有金额
                loopDetail.STD_AmountLossRatio = loopDetail.STD_DueAmount > 0 ? Math.Round(((loopDetail.STD_DueAmount ?? 0) - (loopDetail.STD_ActualAmount ?? 0)) / (loopDetail.STD_DueAmount ?? 0), 4) : 0;
            }
        }

        /// <summary>
        /// 获取需要更新的库存
        /// </summary>
        /// <returns></returns>
        private List<MDLPIS_Inventory> GetToUpdateInventoryList()
        {
            //待更新的[库存]列表
            List<MDLPIS_Inventory> updateInventoryList = new List<MDLPIS_Inventory>();
            foreach (var loopStockTaskDetail in _detailGridDS)
            {
                //[应有量]等于[实际量]的配件不进行校正
                if (loopStockTaskDetail.STD_DueQty == loopStockTaskDetail.STD_ActualQty)
                {
                    continue;
                }
                //库存不存在的配件，直接校正失败
                if (string.IsNullOrEmpty(loopStockTaskDetail.INV_ID))
                {
                    //配件：(条形码：loopStockTaskDetail.STD_Barcode，批次号：loopStockTaskDetail.STD_BatchNo)不存在，请检查库存，校正失败！
                    MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.W_0010, new object[] {
                        $"{MsgParam.AUTOPARTS}({MsgParam.BARCODE}:{loopStockTaskDetail.STD_Barcode},{MsgParam.BATCHNO}：{loopStockTaskDetail.STD_BatchNo})", MsgParam.INVENTORYRECTIFY_FAILED }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return new List<MDLPIS_Inventory>();
                }
                MDLPIS_Inventory relatedInventory = new MDLPIS_Inventory();
                //将库存必要信息Copy到relatedInventory
                _bll.CopyModel(loopStockTaskDetail, relatedInventory);

                relatedInventory.INV_Qty = (relatedInventory.INV_Qty ?? 0) + ((loopStockTaskDetail.STD_ActualQty ?? 0) - (loopStockTaskDetail.STD_DueQty ?? 0));
                relatedInventory.WHERE_INV_ID = relatedInventory.INV_ID;
                relatedInventory.WHERE_INV_VersionNo = relatedInventory.INV_VersionNo;

                updateInventoryList.Add(relatedInventory);
            }
            return updateInventoryList;
        }

        /// <summary>
        /// 检查是否存在异常的盘库明细
        /// </summary>
        /// <returns></returns>
        private bool CheckIfExistsInvalidTaskDetail(List<MDLPIS_Inventory> paramTaskDetailList)
        {
            //异常的[库存]列表
            List<MDLPIS_Inventory> invalidInventoryList = new List<MDLPIS_Inventory>();
            foreach (var loopInventory in paramTaskDetailList)
            {
                if (loopInventory.INV_Qty >= 0)
                {
                    continue;
                }
                invalidInventoryList.Add(loopInventory);
            }
            if (invalidInventoryList.Count > 0)
            {
                //StocktakingTaskDetailQuery.STD_RetrieveOnStocktaking = true;
                //ExecuteDetailSearch();
                return true;
            }
            return false;
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
                    var curHead = HeadGridDS.FirstOrDefault(x => x.ST_ID == HeadDS.ST_ID);
                    if (curHead != null)
                    {
                        HeadGridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = HeadGridDS.FirstOrDefault(x => x.ST_ID == HeadDS.ST_ID);
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
