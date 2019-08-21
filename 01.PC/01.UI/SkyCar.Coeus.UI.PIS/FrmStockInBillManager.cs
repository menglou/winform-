using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.PIS;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.PIS;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.Common.Enums;
using System.Text.RegularExpressions;
using System.Threading;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinTabControl;
using SkyCar.Coeus.BLL.COM;
using SkyCar.Coeus.Common.CustomControl;
using SkyCar.Coeus.UIModel.PIS.UIModel;
using ColumnStyle = Infragistics.Win.UltraWinGrid.ColumnStyle;
using SkyCar.Coeus.UIModel.Common.APModel;

namespace SkyCar.Coeus.UI.PIS
{
    /// <summary>
    /// 入库管理
    /// </summary>
    public partial class FrmStockInBillManager : BaseFormCardListDetail<StockInBillManagerUIModel, StockInBillManagerQCModel, MDLPIS_StockInBill>
    {
        #region 全局变量

        /// <summary>
        /// 入库管理BLL
        /// </summary>
        private StockInBillManagerBLL _bll = new StockInBillManagerBLL();

        /// <summary>
        /// 【详情】入库明细数据源
        /// </summary>
        private SkyCarBindingList<StockInBillDetailManagerUIModel, MDLPIS_StockInDetail> _detailGridDS = new SkyCarBindingList<StockInBillDetailManagerUIModel, MDLPIS_StockInDetail>();

        /// <summary>
        /// 【详情】配件图片列表
        /// </summary>
        List<AutoPartsPictureUIModel> _autoPartsPictureList = new List<AutoPartsPictureUIModel>();

        /// <summary>
        /// 【详情】配件图片控件列表
        /// </summary>
        List<SkyCarPictureExpand> _pictureExpandList = new List<SkyCarPictureExpand>();

        /// <summary>
        /// 添加入库明细Func
        /// </summary>
        private Func<MaintainAutoPartsDetailUIModel, bool> _maintainAutoPartsDetailFunc;

        #region 下拉框数据源

        /// <summary>
        /// 入库单来源类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _stockInBillSourceTypeDs = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 入库单单据状态
        /// </summary>
        List<ComComboBoxDataSourceTC> _stockInBillStatusDs = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 审核状态
        /// </summary>
        List<ComComboBoxDataSourceTC> _approvalStatusDs = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 供应商
        /// </summary>
        List<MDLPIS_Supplier> _supplierList = new List<MDLPIS_Supplier>();
        #endregion

        /// <summary>
        /// 是否可编辑配件图片（有保存配件档案权限可编辑，否则不可编辑）
        /// </summary>
        private bool _isCanEditPicture;

        /// <summary>
        /// 界面属性值发生变化时不予检查的属性列表
        /// </summary>
        List<string> _skipPropertyList = new List<string>();

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmStockInBillManager构造方法
        /// </summary>
        public FrmStockInBillManager()
        {
            InitializeComponent();

            _maintainAutoPartsDetailFunc = MaintainAutoPartsDetail;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmStockInBillManager_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（动作）
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

            //[列表]页不允许删除
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
            }
            //[打印]不可用
            SetActionEnable(SystemActionEnum.Code.PRINT, false);
            //[转结算]可用
            SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, true);
            //[打印条码]不可用
            SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.PRINTBARCODE, true, false);

            //可保存[配件档案]的场合，[保存]可用，否则不可用
            _isCanEditPicture = BLLCom.HasAuthorityInOrg(LoginInfoDAX.OrgID, SystemActionEnum.Code.SAVE,
                MenuActionConst.MenuID.MenuD_3035);
            SetActionEnable(SystemActionEnum.Code.SAVE, _isCanEditPicture);

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            AcceptUIModelChanges();

            #region 界面发生变化时不予检查的属性值

            _skipPropertyList.Add("APB_AccountPayableAmount");
            _skipPropertyList.Add("APB_PaidAmount");
            _skipPropertyList.Add("APB_UnpaidAmount");
            _skipPropertyList.Add("Org_ShortName");
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
            decimal paidAmount = 0;
            decimal unpaidAmount = 0;

            foreach (var loopGridRow in gdGrid.Rows)
            {
                if (string.IsNullOrEmpty(loopGridRow.Cells[SystemTableColumnEnums.PIS_StockInBill.Code.SIB_No].Value?.ToString())
                    || loopGridRow.Cells["IsChecked"].Value == null
                    || (bool)loopGridRow.Cells["IsChecked"].Value == false)
                {
                    continue;
                }
                totalAmount = totalAmount + Convert.ToDecimal(loopGridRow.Cells["TotalAmount"].Value?.ToString());
                paidAmount = paidAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountPayableBill.Code.APB_PaidAmount].Value?.ToString());
                unpaidAmount = unpaidAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountPayableBill.Code.APB_UnpaidAmount].Value?.ToString());
            }

            ((TextBoxTool)(toolBarPaging.Tools["txtTotalAmount"])).Text = Convert.ToString(totalAmount);
            ((TextBoxTool)(toolBarPaging.Tools["txtPaidAmount"])).Text = Convert.ToString(paidAmount);
            ((TextBoxTool)(toolBarPaging.Tools["txtUnpaidAmount"])).Text = Convert.ToString(unpaidAmount);
        }

        /// <summary>
        /// 【列表】Grid的AfterHeaderCheckStateChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            gdGrid.UpdateData();
            decimal totalAmount = 0;
            decimal paidAmount = 0;
            decimal unpaidAmount = 0;

            foreach (var loopGridRow in gdGrid.Rows)
            {
                if (string.IsNullOrEmpty(loopGridRow.Cells[SystemTableColumnEnums.PIS_StockInBill.Code.SIB_No].Value?.ToString())
                    || loopGridRow.Cells["IsChecked"].Value == null
                    || (bool)loopGridRow.Cells["IsChecked"].Value == false)
                {
                    continue;
                }
                totalAmount = totalAmount + Convert.ToDecimal(loopGridRow.Cells["TotalAmount"].Value?.ToString());
                paidAmount = paidAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountPayableBill.Code.APB_PaidAmount].Value?.ToString());
                unpaidAmount = unpaidAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountPayableBill.Code.APB_UnpaidAmount].Value?.ToString());
            }

            ((TextBoxTool)(toolBarPaging.Tools["txtTotalAmount"])).Text = Convert.ToString(totalAmount);
            ((TextBoxTool)(toolBarPaging.Tools["txtPaidAmount"])).Text = Convert.ToString(paidAmount);
            ((TextBoxTool)(toolBarPaging.Tools["txtUnpaidAmount"])).Text = Convert.ToString(unpaidAmount);
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
                //[列表]页不允许[删除]、[打印条码]
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.PRINTBARCODE, true, false);
                //[转结算]可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, true);
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
        private void txtWhere_SIB_No_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }

        /// <summary>
        /// 来源类型名称ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_SIB_SourceTypeName_ValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbWhere_SIB_SourceTypeName.Text)
                )
            {
                return;
            }
            txtWhere_SIB_SourceNo.Text = string.Empty;

            //来源类型为[手工创建]时，[来源单号]为空
            if (cbWhere_SIB_SourceTypeName.Text == StockInBillSourceTypeEnum.Name.SGCJ)
            {
                txtWhere_SIB_SourceNo.ReadOnly = true;
                txtWhere_SIB_SourceNo.Enabled = false;
            }
            else
            {
                txtWhere_SIB_SourceNo.ReadOnly = false;
                txtWhere_SIB_SourceNo.Enabled = true;
            }
        }

        /// <summary>
        /// 来源单号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_SIB_SourceNo_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 单据状态名称ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_SIB_StatusName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 审核状态名称ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_SIB_ApprovalStatusName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 有效CheckedChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckWhere_SIB_IsValid_CheckedChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }

        /// <summary>
        /// 查询来源单号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_SIB_SourceNo_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(cbWhere_SIB_SourceTypeName.Text))
            {
                //请先选择来源类型，再选择来源单号
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0034, new object[] { MsgParam.SOURCE_TYPE, SystemTableColumnEnums.PIS_StockInBill.Name.SIB_SourceNo }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cbWhere_SIB_SourceTypeName.Text == StockInBillSourceTypeEnum.Name.SGCJ)
            {
                return;
            }

            #region 采购入库

            if (cbWhere_SIB_SourceTypeName.Text == StockInBillSourceTypeEnum.Name.CGRK)
            {
                //查询采购订单
                FrmPurchaseOrderQuery frmPurchaseOrderQuery = new FrmPurchaseOrderQuery
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = frmPurchaseOrderQuery.ShowDialog();

                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                if (frmPurchaseOrderQuery.SelectedGridList.Count == 1)
                {
                    txtWhere_SIB_SourceNo.Text = frmPurchaseOrderQuery.SelectedGridList[0].PO_No;
                }
            }
            #endregion

            #region 销售退货

            if (cbWhere_SIB_SourceTypeName.Text == StockInBillSourceTypeEnum.Name.SSTH)
            {
                //仅查询[审核状态]为[已审核]的销售订单
                var paramApprovetatusList = _approvalStatusDs.Where(x => x.Text == ApprovalStatusEnum.Name.YSH).ToList();

                Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
                {
                    {ComViewParamKey.ApprovalStatus.ToString(), paramApprovetatusList}
                };

                FrmSalesOrderQuery frmSalesOrderQuery = new FrmSalesOrderQuery(paramViewParameters)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = frmSalesOrderQuery.ShowDialog();

                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                if (frmSalesOrderQuery.SelectedGridList.Count == 1)
                {
                    txtWhere_SIB_SourceNo.Text = frmSalesOrderQuery.SelectedGridList[0].SO_No;
                }
            }
            #endregion
        }
        /// <summary>
        /// 列表查询条件dtWhere_PB_DateEnd_ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_PB_DateEnd_ValueChanged(object sender, EventArgs e)
        {
            if (this.dtWhere_PB_DateEnd.Value != null &&
             this.dtWhere_PB_DateEnd.DateTime.Hour == 0 &&
             this.dtWhere_PB_DateEnd.DateTime.Minute == 0 &&
             this.dtWhere_PB_DateEnd.DateTime.Second == 0)
            {
                DateTime newDateTime = new DateTime(this.dtWhere_PB_DateEnd.DateTime.Year, this.dtWhere_PB_DateEnd.DateTime.Month, this.dtWhere_PB_DateEnd.DateTime.Day, 23, 59, 59);
                this.dtWhere_PB_DateEnd.DateTime = newDateTime;
            }
        }
        #endregion

        #region 单头相关事件

        /// <summary>
        /// 【详情】来源类型值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSIB_SourceTypeName_ValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbSIB_SourceTypeName.Text))
            {
                return;
            }

            txtSIB_SourceNo.Text = string.Empty;

            _detailGridDS = new SkyCarBindingList<StockInBillDetailManagerUIModel, MDLPIS_StockInDetail>();
            _detailGridDS.StartMonitChanges();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();

            if (cbSIB_SourceTypeName.Text == StockInBillSourceTypeEnum.Name.SGCJ)
            {
                //来源类型为[手工创建]的场合，[来源单号]不显示
                lblSIB_SourceNo.Visible = false;
                txtSIB_SourceNo.Visible = false;
            }
            else
            {
                //来源类型为[采购入库] 或 [销售退货]的场合，[来源单号]显示
                lblSIB_SourceNo.Visible = true;
                txtSIB_SourceNo.Visible = true;
            }
        }

        #endregion

        #region 入库单明细相关事件

        /// <summary>
        /// 添加/删除 入库单明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolbarsManagerDetail_ToolClick(object sender, ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case SysConst.EN_ADD:
                    //添加
                    AddStockInDetail();
                    break;

                case SysConst.EN_DEL:
                    //删除
                    DeleteStockInDetail();
                    break;
            }
        }

        /// <summary>
        /// 入库单明细gdStockInDetail的Cell的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            UpdateStockInDetail();
        }
        /// <summary>
        /// 入库单明细gdStockInDetail的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_DoubleClick(object sender, EventArgs e)
        {
            UpdateStockInDetail();
        }

        /// <summary>
        /// 入库单明细单击单元格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_ClickCellButton(object sender, CellEventArgs e)
        {
            var cellIndex = gdDetail.Rows[e.Cell.Row.Index];

            //选择仓库
            if (e.Cell.Column.Key == SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name)
            {
                FrmWarehouseQuery frmWarehouseQuery = new FrmWarehouseQuery(SystemTableColumnEnums.PIS_Warehouse.Code.WH_ID,
                    SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name,
                paramItemSelectedItemParentValue: LoginInfoDAX.OrgID,
                paramItemSelectedItemParentText: LoginInfoDAX.OrgShortName);
                DialogResult dialogResult = frmWarehouseQuery.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    e.Cell.Value = frmWarehouseQuery.SelectedText;
                    cellIndex.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_WH_ID].Value = frmWarehouseQuery.SelectedValue;
                }
                //仓库改变，清空仓位
                cellIndex.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_WHB_ID].Value = null;
                cellIndex.Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name].Value = null;
            }

            //选择仓位
            if (e.Cell.Column.Key == SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name)
            {
                //请先选择仓库，再选择仓位
                if (string.IsNullOrEmpty(cellIndex.Cells[SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name].Text))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0034, new object[] { MsgParam.WAREHOUSE, MsgParam.WAREHOUSE_BIN }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                FrmWarehouseBinQuery frmWarehouseBinQuery = new FrmWarehouseBinQuery(SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_ID,
                    SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name,
                    paramCustomeSelectionMode: CustomEnums.CustomeSelectionMode.Single,
                    paramItemSelectedItemParentValue: cellIndex.Cells[SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name].Text);
                DialogResult dialogResult = frmWarehouseBinQuery.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    e.Cell.Value = frmWarehouseBinQuery.SelectedText;
                    cellIndex.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_WHB_ID].Value = frmWarehouseBinQuery.SelectedValue;
                }
            }

            //跳转配件图片
            if (e.Cell.Column.Key == "EditAutoPartsPicture")
            {
                if (cellIndex.Cells["SID_Barcode"].Value == null)
                {
                    //请选择入库单明细
                    MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { SystemTableEnums.Name.PIS_StockInDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //当前入库单明细
                StockInBillDetailManagerUIModel curDetail =
                    _detailGridDS.FirstOrDefault(x => x.SID_Barcode == cellIndex.Cells["SID_Barcode"].Value.ToString());
                if (curDetail != null)
                {
                    txtAutoPartsBarcode.Text = curDetail.SID_Barcode;
                }
                //加载配件图片
                LoadAutoPartsPicture(curDetail);

                //设置【配件图片】Tab为选中状态
                tabControlDetail.Tabs[SysConst.AutoPartsPicture].Selected = true;
            }

            gdDetail.UpdateData();
            //设置入库单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 入库单明细单元格值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_CellChange(object sender, CellEventArgs e)
        {
            gdDetail.UpdateData();
            var cellIndex = gdDetail.Rows[e.Cell.Row.Index];

            //输入的单价和输入的数量计算总金额 计算公式： 单价 * 数量 = 总金额
            if (e.Cell.Column.Key == SystemTableColumnEnums.PIS_StockInDetail.Code.SID_UnitCostPrice
                || e.Cell.Column.Key == SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Qty)
            {
                //总金额
                decimal amount = 0;
                string id = cellIndex.Cells["Tmp_SID_ID"].Text;
                foreach (var loopDetail in _detailGridDS)
                {
                    if (loopDetail.Tmp_SID_ID == id)
                    {
                        loopDetail.SID_Amount = Math.Round(
                            (loopDetail.SID_UnitCostPrice ?? 0) * (loopDetail.SID_Qty ?? 0), 2);
                    }

                }
                foreach (var loopDetail in _detailGridDS)
                {
                    amount = amount + Convert.ToDecimal(loopDetail.SID_Amount ?? 0);
                }
                txtTotalAmount.Text = Convert.ToString(amount);
            }
            //验证打印次数
            if (e.Cell.Column.Key == "PrintCount")
            {
                if (!BLLCom.IsDecimal(e.Cell.Text))
                {
                    e.Cell.Value = Regex.Replace(e.Cell.Text, @"[^\d]*", "") == string.Empty ? Convert.ToDecimal(0) : e.Cell.Value;
                }
                if (!BLLCom.Validation(RegularFormula.POSITIVE_INTEGER, e.Cell.Text))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0002, new object[] { MsgParam.POSITIVE_INTEGER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cell.Value = 0;
                }
            }
            //设置入库单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }
        #endregion

        #region 配件图片相关事件

        /// <summary>
        /// 配件图片ToolClick事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolbarsAutoPartsPicture_ToolClick(object sender, ToolClickEventArgs e)
        {
            //当前入库单明细
            var curDetail = _detailGridDS.FirstOrDefault(x => x.SID_Barcode == txtAutoPartsBarcode.Text);
            if (curDetail == null)
            {
                //请选择入库单明细
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { SystemTableEnums.Name.PIS_StockInDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            switch (e.Tool.Key)
            {
                case SysConst.Upload:
                    //批量上传图片
                    BatchUploadPicture(curDetail);
                    break;
                case SysConst.Export:
                    //批量导出图片
                    BatchExportPicture(curDetail);
                    break;
                case SysConst.EN_DEL:
                    //批量删除图片
                    BatchDeletePicture(curDetail);
                    break;
            }
        }

        /// <summary>
        /// 全选Checked改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckSelectAllPicture_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAutoPartsBarcode.Text))
            {
                SelectAllPicture(txtAutoPartsBarcode.Text);
            }
        }

        /// <summary>
        /// 【详情】选中的Tab改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlDetail_SelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            if (tabControlDetail.Tabs[SysConst.AutoPartsPicture].Selected)
            {
                //选中Tab为【配件图片】的场合
                var curDetail = _detailGridDS.FirstOrDefault(x => x.SID_Barcode == txtAutoPartsBarcode.Text);
                SetPictureControl(curDetail);
            }
            else
            {
                txtAutoPartsBarcode.Clear();
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
            if (ViewHasChanged(_skipPropertyList)
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

            //设置动作按钮状态
            SetActionEnableByStatus();
            //控制详情是否可编辑
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

            //2.将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();

            //3.执行保存（含服务端检查）
            if (!_bll.SaveDetailDS(HeadDS, _detailGridDS, _autoPartsPictureList))
            {
                //保存失败
                MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //4.开始监控List变化
            _detailGridDS.StartMonitChanges();
            //5.将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置动作按钮状态
            SetActionEnableByStatus();
            //控制详情是否可编辑
            SetDetailControl();

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置入库单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;

            //为图片控件设置传入的配件图片Model
            foreach (var loopPicture in _autoPartsPictureList)
            {
                //清空图片的来源类型
                loopPicture.SourceFilePath = null;

                var curPictureExpand = _pictureExpandList.FirstOrDefault(x => x.PictureKey == loopPicture.INVP_PictureName);
                if (curPictureExpand != null)
                {
                    curPictureExpand.PropertyModel = loopPicture;
                }
            }
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
            var argsDetail = new List<MDLPIS_StockInDetail>();
            //将HeadDS转换为TBModel对象
            var argsHead = HeadDS.ToTBModelForSaveAndDelete<MDLPIS_StockInBill>();
            //将当前DetailGridDS转换为指定类型的TBModelList
            _detailGridDS.ToTBModelListForUpdateAndDelete<MDLPIS_StockInDetail>(argsDetail);
            //过滤明细列表中未保存的数据
            argsDetail = argsDetail.Where(x => !string.IsNullOrEmpty(x.WHERE_SID_ID)).ToList();
            //2.执行删除
            bool deleteResult = _bll.DeleteDetailDS(argsHead, argsDetail);
            if (!deleteResult)
            {
                //删除失败
                MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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

            //删除所有图片控件
            _autoPartsPictureList = new List<AutoPartsPictureUIModel>();
            flowLayoutPanelPicture.Controls.Clear();
            _pictureExpandList.Clear();
        }

        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            //1.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
            //2.设置查询条件（翻页相关属性不用设置）
            base.ConditionDS = new StockInBillManagerQCModel()
            {
                //组织
                WHERE_SIB_Org_ID = LoginInfoDAX.OrgID,
                //单号
                WHERE_SIB_No = txtWhere_SIB_No.Text.Trim(),
                //来源类型名称
                WHERE_SIB_SourceTypeName = cbWhere_SIB_SourceTypeName.Text.Trim(),
                //来源单号
                WHERE_SIB_SourceNo = txtWhere_SIB_SourceNo.Text.Trim(),
                //单据状态名称
                WHERE_SIB_StatusName = cbWhere_SIB_StatusName.Text.Trim(),
                //审核状态名称
                WHERE_SIB_ApprovalStatusName = cbWhere_SIB_ApprovalStatusName.Text.Trim(),
                //供应商ID
                WHERE_SIB_SUPP_ID = mcbWhere_SUPP_Name.SelectedValue,
                //有效
                WHERE_SIB_IsValid = ckWhere_SIB_IsValid.Checked,
                //SqlId
                SqlId = SQLID.PIS_StockInBillManager_SQL01
            };
            if (dtWhere_PB_DateStart.Value != null)
            {
                //收款时间-开始
                ConditionDS._DateStart = dtWhere_PB_DateStart.DateTime;
            }
            if (dtWhere_PB_DateEnd.Value != null)
            {
                //收款时间-终了
                ConditionDS._DateEnd = dtWhere_PB_DateEnd.DateTime;
            }
            //3.执行基类方法（注意：基类使用模糊查询）
            base.QueryAction();
            //4.Grid绑定数据源
            gdGrid.DataSource = base.HeadGridDS;
            gdGrid.DataBind();
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
            base.NewUIModel = HeadDS;
            if (ViewHasChanged(_skipPropertyList)
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
            //入库单未保存，不能审核
            if (string.IsNullOrEmpty(txtSIB_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_StockInBill + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MDLPIS_StockInBill resulStockInBill = new MDLPIS_StockInBill();
            _bll.QueryForObject<MDLPIS_StockInBill, MDLPIS_StockInBill>(new MDLPIS_StockInBill()
            {
                WHERE_SIB_ID = txtSIB_ID.Text.Trim(),
                WHERE_SIB_IsValid = true
            }, resulStockInBill);
            //入库单不存在，不能审核
            if (string.IsNullOrEmpty(resulStockInBill.SIB_ID))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_StockInBill + MsgParam.NOTEXIST, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //确认审核操作
            DialogResult isApprove = MessageBoxs.Show(Trans.PIS, this.ToString(),
                MsgHelp.GetMsg(MsgCode.W_0014),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isApprove != DialogResult.OK)
            {
                return;
            }
            #endregion

            base.ApproveAction();

            //将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();

            #region 审核入库单

            bool saveApproveResult = _bll.ApproveDetailDS(HeadDS, _detailGridDS, _autoPartsPictureList);
            if (!saveApproveResult)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.APPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            #endregion

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
            //控制详情是否可编辑
            SetDetailControl();

            //为图片控件设置传入的配件图片Model
            foreach (var loopPicture in _autoPartsPictureList)
            {
                var curPictureExpand = _pictureExpandList.FirstOrDefault(x => x.PictureKey == loopPicture.INVP_PictureName);
                if (curPictureExpand != null)
                {
                    curPictureExpand.PropertyModel = loopPicture;
                }
            }

            foreach (var loopDetail in _detailGridDS)
            {
                SetPictureControl(loopDetail);
            }

            //审核后不能更新数量和单价，可以修改打印数量
            SetStockInDetailStyle(true, false);
        }

        /// <summary>
        /// 反审核
        /// </summary>
        public override void UnApproveAction()
        {
            #region 验证数据

            //入库单未保存，不能反审核
            if (string.IsNullOrEmpty(txtSIB_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_StockInBill + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MDLPIS_StockInBill resultStockInBill = new MDLPIS_StockInBill();
            _bll.QueryForObject<MDLPIS_StockInBill, MDLPIS_StockInBill>(new MDLPIS_StockInBill()
            {
                WHERE_SIB_ID = txtSIB_ID.Text.Trim(),
                WHERE_SIB_IsValid = true
            }, resultStockInBill);
            //入库单不存在，不能反审核
            if (string.IsNullOrEmpty(resultStockInBill.SIB_ID)
                || string.IsNullOrEmpty(resultStockInBill.SIB_No))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_StockInBill + MsgParam.NOTEXIST, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //确认反审核操作
            DialogResult isUnApprove = MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0018),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isUnApprove != DialogResult.OK)
            {
                return;
            }
            #endregion

            base.UnApproveAction();

            //将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            #region 反审核入库单

            bool saveUnApproveResult = _bll.UnApproveDetailDS(HeadDS, _detailGridDS);
            if (!saveUnApproveResult)
            {
                //反审核失败
                MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //反审核成功
            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.UNAPPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            #endregion

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
            //控制详情是否可编辑
            SetDetailControl();

            //删除所有图片控件
            _autoPartsPictureList = new List<AutoPartsPictureUIModel>();
            flowLayoutPanelPicture.Controls.Clear();
            _pictureExpandList.Clear();
        }

        /// <summary>
        /// 打印
        /// </summary>
        public override void PrintAction()
        {
            base.PrintAction();
            try
            {
                if (HeadDS == null || string.IsNullOrEmpty(HeadDS.SIB_No))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, SystemTableEnums.Name.PIS_StockInBill), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (HeadDS.SIB_ApprovalStatusName != ApprovalStatusEnum.Name.YSH)
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, SystemTableEnums.Name.PIS_StockInBill + cbSIB_ApprovalStatusName.Text, SystemActionEnum.Name.PRINT), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //待打印的入库单
                StockInBillUIModelToPrint stockInBillToPrint = new StockInBillUIModelToPrint();
                _bll.CopyModel(HeadDS, stockInBillToPrint);
                //待打印的入库单明细
                List<StockInBillDetailUIModelToPrint> stockInBillDetailToPrintList = new List<StockInBillDetailUIModelToPrint>();
                _bll.CopyModelList(_detailGridDS, stockInBillDetailToPrintList);
                foreach (var loopDetail in stockInBillDetailToPrintList)
                {
                    if (string.IsNullOrEmpty(loopDetail.SID_Barcode))
                    {
                        continue;
                    }
                    loopDetail.SID_Qty = Math.Round((loopDetail.SID_Qty ?? 0), 0);
                    stockInBillToPrint.TotalStockInQty += (loopDetail.SID_Qty ?? 0);
                }
                Dictionary<string, object> argsViewParams = new Dictionary<string, object>
                {
                    //入库单
                    {PISViewParamKey.StockInBill.ToString(), stockInBillToPrint},
                    //入库单明细
                    {PISViewParamKey.StockInBillDetail.ToString(), stockInBillDetailToPrintList},
                };

                FrmViewAndPrintStockInBill frmViewAndPrintStockInBill = new FrmViewAndPrintStockInBill(argsViewParams)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = frmViewAndPrintStockInBill.ShowDialog();
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
        /// 打印条码
        /// </summary>
        public override void PrintBarCodeNavigate()
        {
            base.PrintBarCodeNavigate();
            try
            {
                SkyCarBindingList<StockInBillDetailManagerUIModel, MDLPIS_StockInDetail> tempPrintBarcodeList = new SkyCarBindingList<StockInBillDetailManagerUIModel, MDLPIS_StockInDetail>();

                if (HeadGridDS != null)
                {

                    foreach (var loopInventory in _detailGridDS)
                    {
                        if (loopInventory.IsChecked && !string.IsNullOrEmpty(loopInventory.SID_Barcode))
                        {
                            tempPrintBarcodeList.Add(loopInventory);
                        }
                    }
                }
                if (tempPrintBarcodeList.Count == 0)
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0034), MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                #region 验证打印次数和入库数量一致

                int i = 0;
                StringBuilder rowCount = new StringBuilder();
                foreach (var loopPrintBarcode in tempPrintBarcodeList)
                {
                    if (loopPrintBarcode.PrintCount == null)
                    {
                        continue;
                    }
                    i++;
                    if (loopPrintBarcode.PrintCount != loopPrintBarcode.SID_Qty)
                    {
                        rowCount.Append(i + SysConst.Semicolon_DBC);
                    }
                }
                if (rowCount.Length > 0)
                {
                    rowCount.Remove(rowCount.Length - 1, 1);
                    DialogResult returnResult = MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0007, new object[]
                        {rowCount, MsgParam.PRINT_COUNT, SystemTableColumnEnums.PIS_StockInDetail.Name.SID_Qty,MsgParam.PRINTBARCODE}), MessageBoxButtons.OKCancel,
                       MessageBoxIcon.Information);
                    if (returnResult != DialogResult.OK)
                    {
                        return;
                    }

                }

                #endregion

                StringBuilder remindContentSB = new StringBuilder();
                StringBuilder printName = new StringBuilder();
                foreach (var loopPrintItem in tempPrintBarcodeList)
                {
                    if (loopPrintItem.PrintCount == null)
                    {
                        continue;
                    }
                    if (loopPrintItem.PrintCount == 0)
                    {
                        printName.AppendLine(loopPrintItem.SID_Name);
                    }
                    if (loopPrintItem.PrintCount > 50)
                    {
                        remindContentSB.AppendLine(loopPrintItem.SID_Name + "：" + loopPrintItem.PrintCount +
                                                   MsgParam.NUMBER_TIMES);
                    }

                }
                #region 

                #endregion 提醒打印次数不能为零

                if (printName.ToString().Length > 0)
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { printName + MsgParam.PRINT_COUNT, MsgParam.BE + MsgParam.ZERO }), MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                #region 提醒打印次数超过50的条码

                if (remindContentSB.ToString().Length > 0)
                {
                    string remindContent = MsgHelp.GetMsg(MsgCode.I_0005) + remindContentSB;
                    remindContent += MsgHelp.GetMsg(MsgCode.I_0006);
                    DialogResult confirmPrintBarcode = MessageBoxs.Show(Trans.PIS, this.ToString(), remindContent,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirmPrintBarcode == DialogResult.No)
                    {
                        return;
                    }
                }

                #endregion

                #region 检查打印机连接

                byte[] pbuf = new byte[128];
                Encoding encAscIi = Encoding.ASCII;
                var nLen = BarcodePrinterHelper.B_GetUSBBufferLen() + 1;
                //if (nLen > 1)
                //{
                int len1 = 128, len2 = 128;
                var buf1 = new byte[len1];
                var buf2 = new byte[len2];
                BarcodePrinterHelper.B_EnumUSB(pbuf);
                BarcodePrinterHelper.B_GetUSBDeviceInfo(1, buf1, out len1, buf2, out len2);
                var ret = BarcodePrinterHelper.B_CreatePrn(12, encAscIi.GetString(buf2, 0, len2));
                if (0 != ret)
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0035), MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                //}

                #endregion

                foreach (var loopPrintItem in tempPrintBarcodeList)
                {
                    if (loopPrintItem.PrintCount == null)
                    {
                        continue;
                    }

                    string inspire = loopPrintItem.APA_VehicleInspire;
                    if (!string.IsNullOrEmpty(loopPrintItem.APA_VehicleInspire) &&
                        loopPrintItem.APA_VehicleInspire.Trim(';').Length > 16)
                    {
                        inspire =
                            loopPrintItem.APA_VehicleInspire.Substring(0,
                                Math.Min(18, loopPrintItem.APA_VehicleInspire.Length)) + "...";
                    }
                    string spec = loopPrintItem.SID_Specification;
                    if (!string.IsNullOrEmpty(loopPrintItem.SID_Specification) &&
                        loopPrintItem.SID_Specification.Trim(';').Length > 16)
                    {
                        spec =
                            loopPrintItem.SID_Specification.Substring(0,
                                Math.Min(18, loopPrintItem.SID_Specification.Length)) + "...";
                    }
                    if (loopPrintItem.PrintCount > 0)
                    {
                        BarcodePrinterHelper.B_Prn_Text_TrueType(10, 10, 35, "微软雅黑", 1, 800, 0, 0, 0, "A2",
                        loopPrintItem.SID_Name ?? string.Empty);
                        BarcodePrinterHelper.B_Prn_Text_TrueType(10, 50, 35, "微软雅黑", 1, 800, 0, 0, 0, "A3",
                            inspire ?? string.Empty);
                        BarcodePrinterHelper.B_Prn_Text_TrueType(10, 90, 35, "微软雅黑", 1, 800, 0, 0, 0, "A4",
                            spec ?? string.Empty);
                        BarcodePrinterHelper.B_Prn_Barcode(30, 130, 0, "1", 2, 8, 110, 'B',
                            (loopPrintItem.SID_Barcode ?? string.Empty) + (loopPrintItem.SID_Barcode ?? string.Empty));
                        BarcodePrinterHelper.B_Print_Out((loopPrintItem.PrintCount ?? 0));
                    }
                }
                BarcodePrinterHelper.B_ClosePrn();
            }
            catch (Exception ex)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(),
                    MsgHelp.GetMsg(MsgCode.I_0002, new object[] { SystemActionEnum.Name.PRINT }) + ex.Message,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
       
        /// <summary>
        /// 转结算
        /// </summary>
        public override void ToSettlementNavigate()
        {
            base.ToSettlementNavigate();
            //选中的待付款的[入库单]列表
            List<StockInBillManagerUIModel> selectedStockInBillList = new List<StockInBillManagerUIModel>();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 当前选中Tab为【详情】
                if (HeadDS == null || string.IsNullOrEmpty(HeadDS.SIB_No))
                {
                    //请选择一个手工创建或采购入库并且已审核的[入库单]转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[]
                    {
                        MsgParam.ONE + StockInBillSourceTypeEnum.Name.SGCJ + MsgParam.OR +StockInBillSourceTypeEnum.Name.CGRK+ MsgParam.AND + ApprovalStatusEnum.Name.YSH +
                        MsgParam.OF + SystemTableEnums.Name.PIS_StockInBill + SystemNavigateEnum.Name.TOSETTLEMENT
                    }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (HeadDS.SIB_ApprovalStatusName != ApprovalStatusEnum.Name.YSH
                    || (HeadDS.SIB_SourceTypeName != StockInBillSourceTypeEnum.Name.SGCJ
                    && HeadDS.SIB_SourceTypeName != StockInBillSourceTypeEnum.Name.CGRK))
                {
                    //请选择一个手工创建或采购入库并且已审核的[入库单]转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[]
                    {
                        MsgParam.ONE + StockInBillSourceTypeEnum.Name.SGCJ+ MsgParam.OR +StockInBillSourceTypeEnum.Name.CGRK + MsgParam.AND + ApprovalStatusEnum.Name.YSH +
                        MsgParam.OF + SystemTableEnums.Name.PIS_StockInBill + SystemNavigateEnum.Name.TOSETTLEMENT
                    }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                selectedStockInBillList.Add(HeadDS);
                #endregion
            }
            else
            {
                #region 当前选中Tab为【列表】

                //勾选的入库单
                var checkedGrid = HeadGridDS.Where(x => x.IsChecked == true).ToList();

                if (checkedGrid.Count == 0)
                {
                    //请至少勾选一条已审核的[入库单]信息进行转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[]
                    {
                        ApprovalStatusEnum.Name.YSH + MsgParam.OF + SystemTableEnums.Name.PIS_StockInBill,
                        SystemNavigateEnum.Name.TOSETTLEMENT
                    }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var tempCannotToSettleList =
                    checkedGrid.Where(
                        x =>
                            x.SIB_ApprovalStatusName != ApprovalStatusEnum.Name.YSH ||
                            (x.SIB_SourceTypeName != StockInBillSourceTypeEnum.Name.SGCJ &&
                             x.SIB_SourceTypeName != StockInBillSourceTypeEnum.Name.CGRK)).ToList();
                if (tempCannotToSettleList.Count > 0)
                {
                    //请选择手工创建或采购入库并且已审核的[入库单]转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[]
                    {
                        StockInBillSourceTypeEnum.Name.SGCJ + MsgParam.OR +StockInBillSourceTypeEnum.Name.CGRK+ MsgParam.AND + ApprovalStatusEnum.Name.YSH + MsgParam.OF +
                        SystemTableEnums.Name.PIS_StockInBill + SystemNavigateEnum.Name.TOSETTLEMENT
                    }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    foreach (var loopCannotToReceipt in tempCannotToSettleList)
                    {
                        loopCannotToReceipt.IsChecked = false;
                    }
                    gdGrid.DataSource = HeadGridDS;
                    gdGrid.DataBind();
                    return;
                }
                var firstCheckedItem = checkedGrid.FirstOrDefault(x => x.SIB_ApprovalStatusName == ApprovalStatusEnum.Name.YSH && (x.SIB_SourceTypeName == StockInBillSourceTypeEnum.Name.SGCJ || x.SIB_SourceTypeName == StockInBillSourceTypeEnum.Name.CGRK));
                if (firstCheckedItem == null)
                {
                    //请选择手工创建或采购入库并且已审核的[入库单]转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[]
                    {
                        StockInBillSourceTypeEnum.Name.SGCJ + MsgParam.OR +StockInBillSourceTypeEnum.Name.CGRK+ MsgParam.AND + ApprovalStatusEnum.Name.YSH + MsgParam.OF +
                        SystemTableEnums.Name.PIS_StockInBill + SystemNavigateEnum.Name.TOSETTLEMENT
                    }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return;
                }
                var differFirstCheckedItem = checkedGrid.Where(x => x.SUPP_ID != firstCheckedItem.SUPP_ID
                || x.SUPP_Name != firstCheckedItem.SUPP_Name).ToList();
                if (differFirstCheckedItem.Count > 0)
                {
                    //请选择同一供应商的入库单转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.SAME_SUPPLIER + SystemTableEnums.Name.PIS_StockInBill + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    foreach (var loopCheckedItem in differFirstCheckedItem)
                    {
                        loopCheckedItem.IsChecked = false;
                    }
                    gdGrid.DataSource = HeadGridDS;
                    gdGrid.DataBind();
                    return;
                }
                //已付清的[入库单]
                var tempYfqAccountPayableBill = checkedGrid.Where(x => x.APB_AccountPayableAmount < x.APB_PaidAmount).ToList();
                if (tempYfqAccountPayableBill.Count > 0)
                {
                    //已付金额大于应付金额，是否确认{支付}？\r\n单击【确定】{支付}单据，【取消】返回。
                    DialogResult isConfirmPay = MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0037, new object[] { MsgParam.PAID_AMOUNT, MsgParam.AMOUNTS_PAYABLE, MsgParam.PAY }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (isConfirmPay != DialogResult.OK)
                    {
                        return;
                    }
                }

                _bll.CopyModelList(checkedGrid, selectedStockInBillList);

                #endregion
            }

            #region 访问数据库，获取应收单数据

            //传入的待付款的[入库单]列表
            StockInToPayDataSet.StockInDataTable stockInDataTable = new StockInToPayDataSet.StockInDataTable();

            foreach (var loopStockInBill in selectedStockInBillList)
            {
                if (string.IsNullOrEmpty(loopStockInBill.SIB_No))
                {
                    continue;
                }

                StockInToPayDataSet.StockInRow newStockInRow = stockInDataTable.NewStockInRow();
                newStockInRow.SIB_No = loopStockInBill.SIB_No;
                newStockInRow.SIB_SourceTypeName = loopStockInBill.SIB_SourceTypeName;
                newStockInRow.SIB_Org_ID = loopStockInBill.SIB_Org_ID;

                stockInDataTable.AddStockInRow(newStockInRow);
            }
            //创建SqlConnection数据库连接对象
            SqlConnection sqlCon = new SqlConnection
            {
                ConnectionString = DBManager.GetConnectionString(DBCONFIG.Coeus)
            };
            //打开数据库连接
            sqlCon.Open();
            //创建并初始化SqlCommand对象
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlCon;

            StockInToPayDataSet.StockInToPayDataTable resultStockInToPayList =
                new StockInToPayDataSet.StockInToPayDataTable();

            try
            {
                cmd.CommandText = "P_PIS_GetStockInListToPay";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@StockInList", SqlDbType.Structured);
                cmd.Parameters[0].Value = stockInDataTable;

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(resultStockInToPayList);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return;
            }
            finally
            {
                sqlCon.Close();
            }

            #endregion

            //待确认付款的入库单列表
            List<BusinessPayConfirmWindowModel> businessPayConfirmList = new List<BusinessPayConfirmWindowModel>();
            foreach (var loopStockInToPay in resultStockInToPayList)
            {
                BusinessPayConfirmWindowModel stockInBusinessReceipt = new BusinessPayConfirmWindowModel
                {
                    IsBusinessSourceAccountPayableBill = true,
                    BusinessID = loopStockInToPay.BusinessID,
                    BusinessNo = loopStockInToPay.BusinessNo,
                    BusinessOrgID = loopStockInToPay.BusinessOrgID,
                    BusinessOrgName = loopStockInToPay.BusinessOrgName,
                    BusinessSourceTypeName = loopStockInToPay.BusinessSourceTypeName,
                    BusinessSourceTypeCode = loopStockInToPay.BusinessSourceTypeCode,
                    ReceiveObjectTypeCode = loopStockInToPay.ReceiveObjectTypeCode,
                    ReceiveObjectTypeName = loopStockInToPay.ReceiveObjectTypeName,
                    ReceiveObjectID = loopStockInToPay.RecObjectID,
                    ReceiveObjectName = loopStockInToPay.RecObjectName,
                    PayableTotalAmount = loopStockInToPay.PayableTotalAmount,
                    PayTotalAmount = loopStockInToPay.PaidTotalAmount,
                    UnPayTotalAmount = loopStockInToPay.UnpaidTotalAmount,

                    //入库单相关
                    APB_ID = loopStockInToPay.APB_ID,
                    APB_No = loopStockInToPay.APB_No,
                    APB_BillDirectCode = BillDirectionEnum.Code.PLUS,
                    APB_BillDirectName = BillDirectionEnum.Name.PLUS,
                    APB_SourceTypeCode = loopStockInToPay.APB_SourceTypeCode,
                    APB_SourceTypeName = loopStockInToPay.APB_SourceTypeName,
                    APB_SourceBillNo = loopStockInToPay.APB_SourceBillNo,
                    APB_Org_ID = loopStockInToPay.APB_Org_ID,
                    APB_Org_Name = loopStockInToPay.APB_Org_Name,
                    APB_AccountPayableAmount = loopStockInToPay.APB_AccountPayableAmount,
                    APB_PaidAmount = loopStockInToPay.APB_PaidAmount,
                    APB_UnpaidAmount = loopStockInToPay.APB_UnpaidAmount,
                    APB_BusinessStatusCode = loopStockInToPay.APB_BusinessStatusCode,
                    APB_BusinessStatusName = loopStockInToPay.APB_BusinessStatusName,
                    APB_ApprovalStatusCode = loopStockInToPay.APB_ApprovalStatusCode,
                    APB_ApprovalStatusName = loopStockInToPay.APB_ApprovalStatusName,
                    APB_CreatedBy = loopStockInToPay.APB_CreatedBy,
                    APB_CreatedTime = loopStockInToPay.APB_CreatedTime,
                    APB_VersionNo = loopStockInToPay.APB_VersionNo,
                };
                businessPayConfirmList.Add(stockInBusinessReceipt);
            }

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //业务单确认付款
                {ComViewParamKey.BusinessPaymentConfirm.ToString(), businessPayConfirmList}
            };

            //跳转[业务单确认付款弹出窗]
            FrmBusinessPayConfirmWindow frmBusinessPayConfirmWindow = new FrmBusinessPayConfirmWindow(paramViewParameters)
            {
                StartPosition = FormStartPosition.CenterScreen
            };

            DialogResult dialogResult = frmBusinessPayConfirmWindow.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                QueryAction();
            }
        }
        
        /// <summary>
        /// 导出当前页
        /// </summary>
        /// <param name="paramGrid"></param>
        /// <param name="paramGridName"></param>
        public override void ExportAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = SystemTableEnums.Name.PIS_StockInBill;
            base.ExportAction(gdGrid, paramGridName);
        }
        
        /// <summary>
        /// 导出所有
        /// </summary>
        /// <param name="paramGrid"></param>
        /// <param name="paramGridName"></param>
        public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = SystemTableEnums.Name.PIS_StockInBill;
            List<StockInBillManagerUIModel> resultAllList = new List<StockInBillManagerUIModel>();
            StockInBillManagerQCModel ConditionDS = new StockInBillManagerQCModel()
            {
                PageIndex = 1,
                PageSize = null,
                //组织
                WHERE_SIB_Org_ID = LoginInfoDAX.OrgID,
                //单号
                WHERE_SIB_No = txtWhere_SIB_No.Text.Trim(),
                //来源类型名称
                WHERE_SIB_SourceTypeName = cbWhere_SIB_SourceTypeName.Text.Trim(),
                //来源单号
                WHERE_SIB_SourceNo = txtWhere_SIB_SourceNo.Text.Trim(),
                //单据状态名称
                WHERE_SIB_StatusName = cbWhere_SIB_StatusName.Text.Trim(),
                //审核状态名称
                WHERE_SIB_ApprovalStatusName = cbWhere_SIB_ApprovalStatusName.Text.Trim(),
                //供应商ID
                WHERE_SIB_SUPP_ID = mcbWhere_SUPP_Name.SelectedValue,
                //有效
                WHERE_SIB_IsValid = ckWhere_SIB_IsValid.Checked,
            };
            if (dtWhere_PB_DateStart.Value != null)
            {
                //收款时间-开始
                ConditionDS._DateStart = dtWhere_PB_DateStart.DateTime;
            }
            if (dtWhere_PB_DateEnd.Value != null)
            {
                //收款时间-终了
                ConditionDS._DateEnd = dtWhere_PB_DateEnd.DateTime;
            }
            _bll.QueryForList<StockInBillManagerUIModel>(SQLID.PIS_StockInBillManager_SQL01, ConditionDS, resultAllList);

            UltraGrid allGrid = gdGrid;
            allGrid.DataSource = resultAllList;
            allGrid.DataBind();
            base.ExportAllAction(allGrid, paramGridName);

            gdGrid.DataSource = base.HeadGridDS;
            gdGrid.DataBind();

        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 工具生成
            //单号
            txtSIB_No.Clear();
            //来源单号
            txtSIB_SourceNo.Clear();
            //供应商名称
            mcbSUPP_Name.Clear();
            //有效
            ckSIB_IsValid.Checked = true;
            ckSIB_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtSIB_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtSIB_CreatedTime.Value = BLLCom.GetCurStdDatetime();
            //修改人
            txtSIB_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtSIB_UpdatedTime.Value = BLLCom.GetCurStdDatetime();
            //入库单ID
            txtSIB_ID.Clear();
            //组织ID
            txtSIB_Org_ID.Clear();
            //版本号
            txtSIB_VersionNo.Clear();
            //合计金额
            txtTotalAmount.Clear();
            //备注
            txtSIB_Remark.Clear();
            //给 单号 设置焦点
            lblSIB_No.Focus();

            //清空[明细]Grid
            _detailGridDS = new SkyCarBindingList<StockInBillDetailManagerUIModel, MDLPIS_StockInDetail>();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();

            //3.开始监控List变化（该数据源若有其他调整，请在StartMonitChanges()方法执行前调整）
            _detailGridDS.StartMonitChanges();

            #endregion

            //初始化组织
            txtSIB_Org_ID.Text = LoginInfoDAX.OrgID;

            #region 初始化下拉框
            //来源类型
            _stockInBillSourceTypeDs = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.StockInBillSourceType);
            var tempSourceTypeDs =
                _stockInBillSourceTypeDs.Where(x => x.Text == StockInBillSourceTypeEnum.Name.SGCJ).ToList();
            cbSIB_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
            cbSIB_SourceTypeName.ValueMember = SysConst.EN_Code;
            cbSIB_SourceTypeName.DataSource = tempSourceTypeDs;
            cbSIB_SourceTypeName.DataBind();
            //默认来源类型为[手工创建]
            cbSIB_SourceTypeName.Text = StockInBillSourceTypeEnum.Name.SGCJ;
            cbSIB_SourceTypeName.Value = StockInBillSourceTypeEnum.Code.SGCJ;

            //单据状态
            _stockInBillStatusDs = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.StockInBillStatus);
            cbSIB_StatusName.DisplayMember = SysConst.EN_TEXT;
            cbSIB_StatusName.ValueMember = SysConst.EN_Code;
            cbSIB_StatusName.DataSource = _stockInBillStatusDs;
            cbSIB_StatusName.DataBind();
            //默认单据状态为[已生成]
            cbSIB_StatusName.Text = StockInBillStatusEnum.Name.YSC;
            cbSIB_StatusName.Value = StockInBillStatusEnum.Code.YSC;

            //审核状态
            _approvalStatusDs = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.ApprovalStatus);
            cbSIB_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
            cbSIB_ApprovalStatusName.ValueMember = SysConst.EN_Code;
            cbSIB_ApprovalStatusName.DataSource = _approvalStatusDs;
            cbSIB_ApprovalStatusName.DataBind();
            //默认审核状态为[待审核]
            cbSIB_ApprovalStatusName.Text = ApprovalStatusEnum.Name.DSH;
            cbSIB_ApprovalStatusName.Value = ApprovalStatusEnum.Code.DSH;

            //供应商
            _supplierList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsSupplier) as List<MDLPIS_Supplier>;
            mcbSUPP_Name.DisplayMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_Name;
            mcbSUPP_Name.ValueMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_ID;
            mcbSUPP_Name.DataSource = _supplierList;

            #endregion

            //默认【详情】中Tab选中【入库明细】
            tabControlDetail.Tabs[SysConst.DetailList].Selected = true;

            //清空所有图片控件
            _autoPartsPictureList = new List<AutoPartsPictureUIModel>();
            flowLayoutPanelPicture.Controls.Clear();
            _pictureExpandList.Clear();
        }
        
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //单号
            txtWhere_SIB_No.Clear();
            //来源类型
            cbWhere_SIB_SourceTypeName.Value = null;
            //来源单号
            txtWhere_SIB_SourceNo.Clear();
            //单据状态
            cbWhere_SIB_StatusName.Value = null;
            //审核状态
            cbWhere_SIB_ApprovalStatusName.Value = null;
            //供应商
            mcbWhere_SUPP_Name.Clear();
            //有效
            ckWhere_SIB_IsValid.Checked = true;
            ckWhere_SIB_IsValid.CheckState = CheckState.Checked;
            dtWhere_PB_DateStart.Value = null;
            dtWhere_PB_DateEnd.Value = null;

            //给 单号 设置焦点
            lblWhere_SIB_No.Focus();
            #endregion

            #region Grid初始化

            //清空Grid
            HeadGridDS = new BindingList<StockInBillManagerUIModel>();
            gdGrid.DataSource = HeadGridDS;
            gdGrid.DataBind();

            #endregion

            #endregion

            #region 初始化下拉框
            //来源类型
            cbWhere_SIB_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_SIB_SourceTypeName.ValueMember = SysConst.EN_Code;
            cbWhere_SIB_SourceTypeName.DataSource = _stockInBillSourceTypeDs;
            cbWhere_SIB_SourceTypeName.DataBind();

            //供应商
            mcbWhere_SUPP_Name.DisplayMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_Name;
            mcbWhere_SUPP_Name.ValueMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_ID;
            mcbWhere_SUPP_Name.DataSource = _supplierList;
            //单据状态
            cbWhere_SIB_StatusName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_SIB_StatusName.ValueMember = SysConst.EN_Code;
            cbWhere_SIB_StatusName.DataSource = _stockInBillStatusDs;
            cbWhere_SIB_StatusName.DataBind();

            //审核状态
            cbWhere_SIB_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_SIB_ApprovalStatusName.ValueMember = SysConst.EN_Code;
            cbWhere_SIB_ApprovalStatusName.DataSource = _approvalStatusDs;
            cbWhere_SIB_ApprovalStatusName.DataBind();

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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_StockInBill.Code.SIB_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_StockInBill.Code.SIB_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            HeadDS = HeadGridDS.FirstOrDefault(x => x.SIB_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_StockInBill.Code.SIB_ID].Value);
            if (HeadDS == null || string.IsNullOrEmpty(HeadDS.SIB_ID))
            {
                return;
            }

            //设置来源类型数据源
            cbSIB_SourceTypeName.DataSource = _stockInBillSourceTypeDs;
            cbSIB_SourceTypeName.DataBind();
            
            if (txtSIB_ID.Text != HeadDS.SIB_ID
                || (txtSIB_ID.Text == HeadDS.SIB_ID && txtSIB_VersionNo.Text != HeadDS.SIB_VersionNo?.ToString()))
            {
                if (txtSIB_ID.Text == HeadDS.SIB_ID && txtSIB_VersionNo.Text != HeadDS.SIB_VersionNo?.ToString())
                {
                    //数据版本已过期，将加载最新详情。
                    MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.DataHasOverdue }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else if (ViewHasChanged(_skipPropertyList)
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
            //控制详情是否可编辑
            SetDetailControl();
        }
      
        /// <summary>
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //单号q
            txtSIB_No.Text = HeadDS.SIB_No;
            //来源类型
            cbSIB_SourceTypeName.Text = HeadDS.SIB_SourceTypeName ?? string.Empty;
            //来源类型编码
            cbSIB_SourceTypeName.Value = HeadDS.SIB_SourceTypeCode;
            //来源单号
            txtSIB_SourceNo.Text = HeadDS.SIB_SourceNo;
            //供应商
            mcbSUPP_Name.SelectedValue = HeadDS.SUPP_ID;
            //单据状态
            cbSIB_StatusName.Text = HeadDS.SIB_StatusName ?? string.Empty;
            //单据状态编码
            cbSIB_StatusName.Value = HeadDS.SIB_StatusCode;
            //审核状态
            cbSIB_ApprovalStatusName.Text = HeadDS.SIB_ApprovalStatusName ?? string.Empty;
            //审核状态编码
            cbSIB_ApprovalStatusName.Value = HeadDS.SIB_ApprovalStatusCode;
            //有效
            if (HeadDS.SIB_IsValid != null)
            {
                ckSIB_IsValid.Checked = HeadDS.SIB_IsValid.Value;
            }
            //创建人
            txtSIB_CreatedBy.Text = HeadDS.SIB_CreatedBy;
            //创建时间
            dtSIB_CreatedTime.Value = HeadDS.SIB_CreatedTime;
            //修改人
            txtSIB_UpdatedBy.Text = HeadDS.SIB_UpdatedBy;
            //修改时间
            dtSIB_UpdatedTime.Value = HeadDS.SIB_UpdatedTime;
            //入库单ID
            txtSIB_ID.Text = HeadDS.SIB_ID;
            //组织ID
            txtSIB_Org_ID.Text = HeadDS.SIB_Org_ID;
            //版本号
            txtSIB_VersionNo.Text = HeadDS.SIB_VersionNo?.ToString() ?? string.Empty;
            //合计金额
            txtTotalAmount.Text = Convert.ToString(HeadDS.TotalAmount ?? 0);
            //备注
            txtSIB_Remark.Text = HeadDS.SIB_Remark;
        }
        
        /// <summary>
        /// 查询明细Grid数据并绑定
        /// </summary>
        private void QueryDetail()
        {
            //1.设置查询条件
            var argsCondition = new StockInBillManagerQCModel()
            {
                //查询用SqlId 
                SqlId = SQLID.PIS_StockInBillManager_SQL02,
                //入库单ID
                WHERE_SID_SIB_ID = txtSIB_ID.Text.Trim(),
                //入库单NO
                WHERE_SID_SIB_No = txtSIB_No.Text.Trim()
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

            #region 查询明细对应所有配件图片

            var autoPartsBarcodeString = string.Empty;
            foreach (var loopDetail in _detailGridDS)
            {
                if (string.IsNullOrEmpty(loopDetail.SID_Barcode))
                {
                    continue;
                }
                autoPartsBarcodeString += loopDetail.SID_Barcode + SysConst.Semicolon_DBC;
            }
            List<MDLPIS_InventoryPicture> resultAutoPartsPictureList = new List<MDLPIS_InventoryPicture>();
            _bll.QueryForList(SQLID.COMM_SQL49, new MDLPIS_InventoryPicture
            {
                WHERE_INVP_Barcode = autoPartsBarcodeString,
            }, resultAutoPartsPictureList);
            _bll.CopyModelList(resultAutoPartsPictureList, _autoPartsPictureList);

            flowLayoutPanelPicture.Controls.Clear();
            _pictureExpandList.Clear();

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
            //验证组织
            if (!string.IsNullOrEmpty(txtSIB_Org_ID.Text.Trim()))
            {
                if (LoginInfoDAX.OrgID != txtSIB_Org_ID.Text.Trim())
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0015, new object[] { SystemActionEnum.Name.SAVE, SystemTableEnums.Name.PIS_StockInBill }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            //验证来源类型
            if (string.IsNullOrEmpty(cbSIB_SourceTypeName.Text))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.SOURCE_TYPE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            #region 验证入库单明细

            if (gdDetail.Rows.Count == 0)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0014, new object[] { SystemTableEnums.Name.PIS_StockInDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            int i = 0;
            foreach (var loopDetail in _detailGridDS)
            {
                i++;
                //验证入库数量
                if (!BLLCom.IsDecimal(Convert.ToString(loopDetail.SID_Qty ?? 0)))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i, SystemTableColumnEnums.PIS_StockInDetail.Name.SID_Qty, MsgParam.POSITIVE_INTEGER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //验证入库单价
                if (!BLLCom.IsDecimal(Convert.ToString(loopDetail.SID_UnitCostPrice ?? 0)))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i, SystemTableColumnEnums.PIS_StockInDetail.Name.SID_UnitCostPrice, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //验证供应商
                if (string.IsNullOrEmpty(loopDetail.SID_SUPP_ID) || string.IsNullOrEmpty(loopDetail.SUPP_Name))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0027, new object[] { i, MsgParam.SUPPLIER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                //验证仓库
                if (string.IsNullOrEmpty(loopDetail.SID_WH_ID) || string.IsNullOrEmpty(loopDetail.WH_Name))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0027, new object[] { i, MsgParam.WAREHOUSE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
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
            //验证组织
            if (!string.IsNullOrEmpty(txtSIB_Org_ID.Text.Trim()))
            {
                if (LoginInfoDAX.OrgID != txtSIB_Org_ID.Text.Trim())
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0015, new object[] { SystemActionEnum.Name.DELETE, SystemTableEnums.Name.PIS_StockInBill }), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            HeadDS = new StockInBillManagerUIModel()
            {
                //单号
                SIB_No = txtSIB_No.Text.Trim(),
                //来源类型
                SIB_SourceTypeName = cbSIB_SourceTypeName.Text.Trim(),
                //来源单号
                SIB_SourceNo = txtSIB_SourceNo.Text.Trim(),
                //供应商ID
                SUPP_ID = mcbSUPP_Name.SelectedValue,
                //供应商名称
                SUPP_Name = mcbSUPP_Name.SelectedText,
                //单据状态
                SIB_StatusName = cbSIB_StatusName.Text.Trim(),
                //审核状态
                SIB_ApprovalStatusName = cbSIB_ApprovalStatusName.Text.Trim(),
                //有效
                SIB_IsValid = ckSIB_IsValid.Checked,
                //创建人
                SIB_CreatedBy = txtSIB_CreatedBy.Text.Trim(),
                //创建时间
                SIB_CreatedTime = (DateTime?)dtSIB_CreatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //修改人
                SIB_UpdatedBy = txtSIB_UpdatedBy.Text.Trim(),
                //修改时间
                SIB_UpdatedTime = (DateTime?)dtSIB_UpdatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //入库单ID
                SIB_ID = txtSIB_ID.Text.Trim(),
                //组织ID
                SIB_Org_ID = txtSIB_Org_ID.Text.Trim(),
                //来源类型编码
                SIB_SourceTypeCode = cbSIB_SourceTypeName.Value?.ToString(),
                //单据状态编码
                SIB_StatusCode = cbSIB_StatusName.Value?.ToString() ?? "",
                //审核状态编码
                SIB_ApprovalStatusCode = cbSIB_ApprovalStatusName.Value?.ToString(),
                //版本号
                SIB_VersionNo = Convert.ToInt64(txtSIB_VersionNo.Text.Trim() == "" ? "1" : txtSIB_VersionNo.Text.Trim()),
                //合计金额
                TotalAmount = Convert.ToDecimal(txtTotalAmount.Text.Trim() == "" ? "0" : txtTotalAmount.Text.Trim()),
                //备注
                SIB_Remark = txtSIB_Remark.Text,
            };
        }

        #region 入库明细相关方法

        /// <summary>
        /// 维护入库明细Func
        /// </summary>
        /// <param name="paramAutoPartsDetail"></param>
        /// <returns></returns>
        private bool MaintainAutoPartsDetail(MaintainAutoPartsDetailUIModel paramAutoPartsDetail)
        {
            //将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();

            DateTime curDateTime = BLLCom.GetCurStdDatetime();

            if (string.IsNullOrEmpty(paramAutoPartsDetail.Detail_ID))
            {
                #region 新增入库明细的场合

                //验证条码
                if (string.IsNullOrEmpty(paramAutoPartsDetail.APA_Barcode))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0016, new object[] { MsgParam.BARCODE, MsgParam.ADD }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                //条码相同，为同一个配件
                StockInBillDetailManagerUIModel sameStockInDetail = _detailGridDS.FirstOrDefault(tempStockInDetail =>
                tempStockInDetail.SID_Barcode == paramAutoPartsDetail.APA_Barcode);

                //明细列表中不存在相同配件
                if (sameStockInDetail == null)
                {
                    StockInBillDetailManagerUIModel newStockInBillDetail = new StockInBillDetailManagerUIModel
                    {
                        Tmp_SID_ID = Guid.NewGuid().ToString(),
                        SID_ID = null,
                        SID_SIB_No = HeadDS.SIB_No
                    };
                    _bll.CopyModel(paramAutoPartsDetail, newStockInBillDetail);

                    #region 为入库明细赋值

                    newStockInBillDetail.SID_Barcode = paramAutoPartsDetail.APA_Barcode;
                    newStockInBillDetail.SID_Name = paramAutoPartsDetail.APA_Name;
                    newStockInBillDetail.SID_OEMNo = paramAutoPartsDetail.APA_OEMNo;
                    newStockInBillDetail.SID_ThirdNo = paramAutoPartsDetail.APA_ThirdNo;
                    //保存到数据库时取批次号newStockInBillDetail.SID_AutoPartsBatchNo
                    newStockInBillDetail.SID_Specification = paramAutoPartsDetail.APA_Specification;
                    newStockInBillDetail.SID_UOM = paramAutoPartsDetail.APA_UOM;
                    newStockInBillDetail.SID_SUPP_ID = paramAutoPartsDetail.APA_SUPP_ID;
                    newStockInBillDetail.SID_WH_ID = paramAutoPartsDetail.APA_WH_ID;
                    newStockInBillDetail.SID_WHB_ID = paramAutoPartsDetail.APA_WHB_ID;
                    newStockInBillDetail.SID_Qty = paramAutoPartsDetail.PurchaseQuantity;
                    newStockInBillDetail.SID_UnitCostPrice = paramAutoPartsDetail.PurchaseUnitPrice;
                    newStockInBillDetail.SID_Amount = paramAutoPartsDetail.StockInAmount;
                    newStockInBillDetail.SID_IsSettled = false;

                    newStockInBillDetail.SID_IsValid = true;
                    newStockInBillDetail.SID_CreatedBy = LoginInfoDAX.UserName;
                    newStockInBillDetail.SID_CreatedTime = curDateTime;
                    newStockInBillDetail.SID_UpdatedBy = LoginInfoDAX.UserName;
                    newStockInBillDetail.SID_UpdatedTime = curDateTime;
                    #endregion

                    _detailGridDS.Add(newStockInBillDetail);
                }
                //明细列表中已存在相同配件
                else
                {
                    sameStockInDetail.SID_Qty = (sameStockInDetail.SID_Qty ?? 0) + paramAutoPartsDetail.PurchaseQuantity;
                    sameStockInDetail.SID_UnitCostPrice = paramAutoPartsDetail.PurchaseUnitPrice;
                    sameStockInDetail.SID_Amount = (sameStockInDetail.SID_Amount ?? 0) + paramAutoPartsDetail.StockInAmount;
                }
                #endregion
            }
            else
            {
                #region 更新入库明细的场合

                foreach (var loopStockInDetail in _detailGridDS)
                {
                    if (loopStockInDetail.SID_ID == paramAutoPartsDetail.Detail_ID ||
                        loopStockInDetail.Tmp_SID_ID == paramAutoPartsDetail.Detail_ID)
                    {
                        _bll.CopyModel(paramAutoPartsDetail, loopStockInDetail);

                        #region 为入库明细赋值
                        loopStockInDetail.SID_Barcode = paramAutoPartsDetail.APA_Barcode;
                        loopStockInDetail.SID_Name = paramAutoPartsDetail.APA_Name;
                        loopStockInDetail.SID_OEMNo = paramAutoPartsDetail.APA_OEMNo;
                        loopStockInDetail.SID_ThirdNo = paramAutoPartsDetail.APA_ThirdNo;
                        //保存到数据库时取批次号newStockInBillDetail.SID_AutoPartsBatchNo
                        loopStockInDetail.SID_Specification = paramAutoPartsDetail.APA_Specification;
                        loopStockInDetail.SID_UOM = paramAutoPartsDetail.APA_UOM;
                        loopStockInDetail.SID_SUPP_ID = paramAutoPartsDetail.APA_SUPP_ID;
                        loopStockInDetail.SID_WH_ID = paramAutoPartsDetail.APA_WH_ID;
                        loopStockInDetail.SID_WHB_ID = paramAutoPartsDetail.APA_WHB_ID;
                        loopStockInDetail.SID_Qty = paramAutoPartsDetail.PurchaseQuantity;
                        loopStockInDetail.SID_UnitCostPrice = paramAutoPartsDetail.PurchaseUnitPrice;
                        loopStockInDetail.SID_Amount = paramAutoPartsDetail.StockInAmount;
                        loopStockInDetail.SID_IsSettled = false;

                        loopStockInDetail.SID_IsValid = true;
                        loopStockInDetail.SID_CreatedBy = LoginInfoDAX.UserName;
                        loopStockInDetail.SID_CreatedTime = curDateTime;
                        loopStockInDetail.SID_UpdatedBy = LoginInfoDAX.UserName;
                        loopStockInDetail.SID_UpdatedTime = curDateTime;
                        #endregion
                        break;
                    }
                }
                //验证条码
                if (string.IsNullOrEmpty(paramAutoPartsDetail.APA_Barcode))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0016, new object[] { MsgParam.BARCODE, MsgParam.UPDATE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                #endregion
            }

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置入库单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;

            return true;
        }

        /// <summary>
        /// 添加入库明细
        /// </summary>
        private void AddStockInDetail()
        {
            #region 验证

            //验证入库单的[审核状态]，[审核状态]为[已审核]的入库单不能添加明细
            if (cbSIB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableEnums.Name.PIS_StockInBill + ApprovalStatusEnum.Name.YSH, MsgParam.ADD + SystemTableEnums.Name.PIS_StockInDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //手工创建的场合，供应商必选
            if (string.IsNullOrEmpty(mcbSUPP_Name.SelectedValue)
                || string.IsNullOrEmpty(mcbSUPP_Name.SelectedText))
            {
                //请选择供应商
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.SUPPLIER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //提醒明细数量
            if (gdDetail.Rows.Count >= 25 && gdDetail.Rows.Count % 25 == 0)
            {
                DialogResult isAddContinueResult = MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0020, new object[] { gdDetail.Rows.Count }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (isAddContinueResult != DialogResult.OK)
                {
                    return;
                }
            }
            #endregion

            #region 从[维护配件明细]中添加[入库单明细]
            //默认供应商
            string defaultSupplierID = null;
            //默认仓库
            string defaultWarehouseID = null;
            //默认仓位
            string defaultWarehouseBinID = null;
            defaultSupplierID = mcbSUPP_Name.SelectedValue;
            foreach (var loopStockInDetail in _detailGridDS)
            {
                defaultWarehouseID = loopStockInDetail.SID_WH_ID;
                defaultWarehouseBinID = loopStockInDetail.SID_WHB_ID;
                break;
            }
            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //维护配件明细的源界面
                {ComViewParamKey.MaintainAutoPartsSourForm.ToString(), ComViewParamValue.MaintainAutoPartsSourForm.PIS_FrmStockInBillManager},
                //维护配件明细的动作
                { ComViewParamKey.MaintainAutoPartsAction.ToString(), ComViewParamValue.MaintainAutoPartsAction.AddAutoParts},
                //维护配件明细的源界面Func
                {ComViewParamKey.MaintainAutoPartsSourFormFunc.ToString(),_maintainAutoPartsDetailFunc},
                //维护配件明细的默认供应商
                {ComViewParamKey.MaintainAutoPartsDefaultSupplierID.ToString(),defaultSupplierID},
                //维护配件明细的默认仓库
                { ComViewParamKey.MaintainAutoPartsDefaultWarehouseID.ToString(),defaultWarehouseID},
                //维护配件明细的默认仓库
                {ComViewParamKey.MaintainAutoPartsDefaultWarehouseBinID.ToString(),defaultWarehouseBinID},
                //指定供应商ID
                {ComViewParamKey.SpecialSupplierID.ToString(),mcbSUPP_Name.SelectedValue},
                //指定供应商名称
                {ComViewParamKey.SpecialSupplierName.ToString(),mcbSUPP_Name.SelectedText},
            };

            FrmMaintainAutoPartsDetail frmMaintainAutoPartsDetail = new FrmMaintainAutoPartsDetail(paramViewParameters)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            DialogResult dialogResult = frmMaintainAutoPartsDetail.ShowDialog();
            //根据[是否存在明细]控制单头是否可编辑
            SetDetailByIsExistDetail();

            decimal amount = 0;
            foreach (var loopDetail in _detailGridDS)
            {
                amount = amount + (loopDetail.SID_Amount ?? 0);
            }
            //string amountStr = Math.Round(amount, 2).ToString();
            //txtSID_Amount.Text = amountStr;
            txtTotalAmount.Text = Convert.ToString(amount);

            #endregion
            if (dialogResult != DialogResult.OK)
            {
                SetStockInDetailStyle(false, true);
                return;
            }
            else
            {
                gdDetail.DataSource = _detailGridDS;
                gdDetail.DataBind();
                //设置入库单明细Grid自适应列宽（根据单元格内容）
                gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
                gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
                SetStockInDetailStyle(false, true);
            }
        }

        /// <summary>
        /// 更新入库明细
        /// </summary>
        private void UpdateStockInDetail()
        {
            #region 验证

            if (!IsAllowUpdateDetailGrid())
            {
                return;
            }

            //验证入库单的审核状态，[已审核]的入库单不能更新明细
            if (cbSIB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableEnums.Name.PIS_StockInBill + ApprovalStatusEnum.Name.YSH, MsgParam.UPDATE + SystemTableEnums.Name.PIS_StockInDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var activeRowIndex = gdDetail.ActiveRow.Index;
            //判断入库单明细Grid内[唯一标识]是否为空
            if ((gdDetail.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_ID].Value == null || string.IsNullOrEmpty(gdDetail.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_ID].Value.ToString()))
                &&
                (gdDetail.Rows[activeRowIndex].Cells["Tmp_SID_ID"].Value == null ||
                string.IsNullOrEmpty(gdDetail.Rows[activeRowIndex].Cells["Tmp_SID_ID"].Value.ToString()))
                )
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0006, new object[] { SystemTableEnums.Name.PIS_StockInDetail, MsgParam.UPDATE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion

            #region 从[维护配件明细]中添加[入库单明细]
            //默认供应商
            string defaultSupplierID = null;
            //默认仓库
            string defaultWarehouseID = null;
            //默认仓位
            string defaultWarehouseBinID = null;
            foreach (var loopStockInDetail in _detailGridDS)
            {
                defaultSupplierID = loopStockInDetail.SID_SUPP_ID;
                defaultWarehouseID = loopStockInDetail.SID_WH_ID;
                defaultWarehouseBinID = loopStockInDetail.SID_WHB_ID;
                break;
            }
            //待更新明细的ID
            string tempDetailId = string.Empty;
            if (gdDetail.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_ID] != null
                && gdDetail.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_ID].Value != null)
            {
                tempDetailId = gdDetail.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_ID].Value.ToString();
            }
            else if (gdDetail.Rows[activeRowIndex].Cells["Tmp_SID_ID"] != null
                && gdDetail.Rows[activeRowIndex].Cells["Tmp_SID_ID"].Value != null)
            {
                tempDetailId = gdDetail.Rows[activeRowIndex].Cells["Tmp_SID_ID"].Value.ToString();
            }
            //待更新的入库单明细
            StockInBillDetailManagerUIModel stockInBillToUpdateDetail = _detailGridDS.FirstOrDefault(x => (!string.IsNullOrEmpty(x.SID_ID) && x.SID_ID == tempDetailId) || (!string.IsNullOrEmpty(x.Tmp_SID_ID) && x.Tmp_SID_ID == tempDetailId));
            if (stockInBillToUpdateDetail == null)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0016, new object[] { SystemTableEnums.Name.PIS_StockInDetail, MsgParam.UPDATE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MaintainAutoPartsDetailUIModel updateAutoPartsDetail = new MaintainAutoPartsDetailUIModel();
            _bll.CopyModel(stockInBillToUpdateDetail, updateAutoPartsDetail);
            updateAutoPartsDetail.Detail_ID = tempDetailId;
            updateAutoPartsDetail.APA_Barcode = stockInBillToUpdateDetail.SID_Barcode;
            updateAutoPartsDetail.APA_Name = stockInBillToUpdateDetail.SID_Name;
            updateAutoPartsDetail.APA_OEMNo = stockInBillToUpdateDetail.SID_OEMNo;
            updateAutoPartsDetail.APA_ThirdNo = stockInBillToUpdateDetail.SID_ThirdNo;
            updateAutoPartsDetail.APA_Specification = stockInBillToUpdateDetail.SID_Specification;
            updateAutoPartsDetail.APA_SUPP_ID = stockInBillToUpdateDetail.SID_SUPP_ID;
            updateAutoPartsDetail.APA_WH_ID = stockInBillToUpdateDetail.SID_WH_ID;
            updateAutoPartsDetail.APA_WHB_ID = stockInBillToUpdateDetail.SID_WHB_ID;
            updateAutoPartsDetail.APA_UOM = stockInBillToUpdateDetail.SID_UOM;
            updateAutoPartsDetail.PurchaseQuantity = stockInBillToUpdateDetail.SID_Qty;
            updateAutoPartsDetail.PurchaseUnitPrice = stockInBillToUpdateDetail.SID_UnitCostPrice;
            updateAutoPartsDetail.StockInAmount = stockInBillToUpdateDetail.SID_Amount;

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                {ComViewParamKey.MaintainAutoPartsSourForm.ToString(), ComViewParamValue.MaintainAutoPartsSourForm.PIS_FrmStockInBillManager},
                {ComViewParamKey.MaintainAutoPartsAction.ToString(), ComViewParamValue.MaintainAutoPartsAction.UpdateAutoParts},
                {ComViewParamKey.MaintainAutoPartsSourFormFunc.ToString(),_maintainAutoPartsDetailFunc},
                {ComViewParamKey.MaintainAutoPartsDefaultSupplierID.ToString(),defaultSupplierID},
                {ComViewParamKey.MaintainAutoPartsDefaultWarehouseID.ToString(),defaultWarehouseID},
                {ComViewParamKey.MaintainAutoPartsDefaultWarehouseBinID.ToString(),defaultWarehouseBinID},
                {ComViewParamKey.DestModel.ToString(),updateAutoPartsDetail},
                //指定供应商ID
                {ComViewParamKey.SpecialSupplierID.ToString(),mcbSUPP_Name.SelectedValue},
                //指定供应商名称
                {ComViewParamKey.SpecialSupplierName.ToString(),mcbSUPP_Name.SelectedText},
            };

            FrmMaintainAutoPartsDetail frmMaintainAutoPartsDetail = new FrmMaintainAutoPartsDetail(paramViewParameters)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            DialogResult dialogResult = frmMaintainAutoPartsDetail.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            #endregion

            //根据[是否存在明细]控制单头是否可编辑
            SetDetailByIsExistDetail();

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置入库单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 删除入库明细
        /// </summary>
        private void DeleteStockInDetail()
        {
            #region 验证

            //验证入库单的审核状态，[审核状态]为[已审核]的入库单不能删除明细
            if (cbSIB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableEnums.Name.PIS_StockInBill + ApprovalStatusEnum.Name.YSH, SystemActionEnum.Name.DELETE + SystemTableEnums.Name.PIS_StockInDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            gdDetail.UpdateData();
            //待删除的入库单明细列表
            var deleteStockInBillDetailList = _detailGridDS.Where(p => p.IsChecked == true).ToList();
            if (deleteStockInBillDetailList.Count == 0)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { SystemTableEnums.Name.PIS_StockInDetail, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion

            //移除勾选项
            foreach (var loopStockInDetail in deleteStockInBillDetailList)
            {
                _detailGridDS.Remove(loopStockInDetail);
            }

            //根据[是否存在明细]控制单头是否可编辑
            SetDetailByIsExistDetail();

            decimal amount = 0;
            foreach (var loopDetail in _detailGridDS)
            {
                loopDetail.SID_Amount = (loopDetail.SID_UnitCostPrice ?? 0) * (loopDetail.SID_Qty ?? 0);
                amount = amount + (loopDetail.SID_Amount ?? 0);
            }
            txtTotalAmount.Text = Convert.ToString(amount);
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置入库单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        #endregion

        /// <summary>
        /// 设置动作按钮状态
        /// </summary>
        private void SetActionEnableByStatus()
        {
            if (cbSIB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                //[审核状态]为[已审核]的场合，[保存]、[删除]、[审核]不可用
                SetActionEnable(SystemActionEnum.Code.SAVE, false);
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                SetActionEnable(SystemActionEnum.Code.APPROVE, false);
                SetActionEnable(SystemActionEnum.Code.UNAPPROVE, true);
                SetActionEnable(SystemActionEnum.Code.PRINT, true);

                //[打印条码]、[转结算]可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.PRINTBARCODE, true, true);

                if (cbSIB_SourceTypeName.Text == StockInBillSourceTypeEnum.Name.SGCJ
                    || cbSIB_SourceTypeName.Text == StockInBillSourceTypeEnum.Name.CGRK)
                {
                    //[来源类型]为{手工创建}或{采购入库}的场合，[转结算]可用
                    SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, true);
                }
                else
                {
                    //[来源类型]为{销售退货}的场合，[转结算]不可用
                    SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, false);
                }
            }
            else
            {
                //新增或[审核状态]为[待审核]的场合，[反审核]、[打印]不可用
                SetActionEnable(SystemActionEnum.Code.SAVE, true);
                SetActionEnable(SystemActionEnum.Code.DELETE, !string.IsNullOrEmpty(txtSIB_ID.Text));
                SetActionEnable(SystemActionEnum.Code.APPROVE, !string.IsNullOrEmpty(txtSIB_ID.Text));
                SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);
                SetActionEnable(SystemActionEnum.Code.PRINT, false);

                //[打印条码]、[转结算]不可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.PRINTBARCODE, true, false);
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, false);
            }
            if (cbSIB_SourceTypeName.Text != StockInBillSourceTypeEnum.Name.SGCJ)
            {
                SetActionEnable(SystemActionEnum.Code.APPROVE, false);
                SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);
            }
        }

        /// <summary>
        /// 设置详情页面控件是否可编辑
        /// </summary>
        private void SetDetailControl()
        {
            //数量和单价只读
            bool qtyAndUnitPriceIsReadOnly = false;

            if (cbSIB_SourceTypeName.Text != StockInBillSourceTypeEnum.Name.SGCJ
                || cbSIB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                #region 入库单.[来源类型]不是{手工创建} 或 入库单.[审核状态]为[已审核] 的场合，详情不可编辑

                //单头
                txtSIB_SourceNo.Enabled = false;
                mcbSUPP_Name.Enabled = false;
                txtSIB_Remark.Enabled = false;

                //明细列表不可添加、删除、更新
                toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedProps.Enabled = false;
                toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = false;
                //明细列表中[入库单价]、[入库数量]、[供应商]、[仓库]、[仓位]列不可编辑

                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_UnitCostPrice].CellActivation = Activation.ActivateOnly;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Qty].CellActivation = Activation.ActivateOnly;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name].CellActivation = Activation.ActivateOnly;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name].Style = ColumnStyle.Default;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name].CellActivation = Activation.ActivateOnly;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name].Style = ColumnStyle.Default;

                #endregion

                qtyAndUnitPriceIsReadOnly = true;
            }
            else
            {
                #region 入库单未保存 或 入库单.[审核状态]为[待审核]的场合，详情可编辑

                //根据[是否存在明细]控制单头是否可编辑
                SetDetailByIsExistDetail();
                txtSIB_Remark.Enabled = true;

                //明细列表可添加、删除、更新
                toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedProps.Enabled = true;
                toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = true;
                //明细列表中[入库单价]、[入库数量]、[供应商]、[仓库]、[仓位]列可编辑
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_UnitCostPrice].CellActivation = Activation.AllowEdit;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Qty].CellActivation = Activation.AllowEdit;

                //仓库
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name].CellActivation = Activation.ActivateOnly;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name].Style = ColumnStyle.EditButton;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name]
                    .CellButtonAppearance.Image = Properties.Resources.Search;
                //仓位
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name].CellActivation = Activation.ActivateOnly;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name].Style = ColumnStyle.EditButton;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name]
                    .CellButtonAppearance.Image = Properties.Resources.Search;

                #endregion
            }

            if (cbSIB_SourceTypeName.Text == StockInBillSourceTypeEnum.Name.SGCJ)
            {
                //来源类型为[手工创建]的场合，[来源单号]不显示
                lblSIB_SourceNo.Visible = false;
                txtSIB_SourceNo.Visible = false;
            }
            else
            {
                //来源类型为[采购入库] 或 [销售退货]的场合，[来源单号]显示
                lblSIB_SourceNo.Visible = true;
                txtSIB_SourceNo.Visible = true;
            }

            //打印数量只读
            bool printCountIsReadOnly = false;
            if (cbSIB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                gdDetail.DisplayLayout.Bands[0].Columns["PrintCount"].CellActivation = Activation.AllowEdit;
            }
            else
            {
                gdDetail.DisplayLayout.Bands[0].Columns["PrintCount"].CellActivation = Activation.ActivateOnly;

                printCountIsReadOnly = true;
            }


            SetStockInDetailStyle(qtyAndUnitPriceIsReadOnly, printCountIsReadOnly);
        }
        
        /// <summary>
        /// 设置入库明细Grid单元格样式
        /// </summary>
        /// <param name="paramQtyAndUnitPriceIsReadOnly"></param>
        /// <param name="paramPrintCountIsReadOnly"></param>
        private void SetStockInDetailStyle(bool paramQtyAndUnitPriceIsReadOnly, bool paramPrintCountIsReadOnly)
        {
            #region 设置Grid数据颜色
            gdDetail.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
            foreach (var loopGridRow in gdDetail.Rows)
            {
                if (paramQtyAndUnitPriceIsReadOnly)
                {
                    //设置单元格只读
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Qty].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Qty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Qty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    //设置单元格只读
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_UnitCostPrice].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_UnitCostPrice].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_UnitCostPrice].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;
                }
                else
                {
                    //设置单元格可编辑
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Qty].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Qty].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Qty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    //设置单元格可编辑
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_UnitCostPrice].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_UnitCostPrice].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_UnitCostPrice].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;
                }
                if (paramPrintCountIsReadOnly)
                {
                    //设置单元格只读
                    loopGridRow.Cells["PrintCount"].Activation = Activation.ActivateOnly;
                    //设置单元格边框颜色
                    loopGridRow.Cells["PrintCount"].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;
                }
                else
                {
                    //设置单元格可编辑
                    loopGridRow.Cells["PrintCount"].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells["PrintCount"].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells["PrintCount"].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;
                }

            }
            #endregion
        }

        /// <summary>
        /// 根据[是否存在明细]控制单头是否可编辑
        /// </summary>
        private void SetDetailByIsExistDetail()
        {
            if (_detailGridDS.Count == 0)
            {
                txtSIB_SourceNo.Enabled = true;
                mcbSUPP_Name.Enabled = true;
            }
            else
            {
                txtSIB_SourceNo.Enabled = false;
                mcbSUPP_Name.Enabled = false;
            }
        }

        /// <summary>
        /// 是否允许更新[入库单明细]列表的数据
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
        
        #region 配件图片相关方法

        private string _latestBarcode = string.Empty;
        /// <summary>
        /// 加载配件图片
        /// </summary>
        /// <param name="paramDetail">入库明细Detail</param>
        private void LoadAutoPartsPicture(StockInBillDetailManagerUIModel paramDetail)
        {
            if (paramDetail == null
                || string.IsNullOrEmpty(paramDetail.SID_Barcode))
            {
                //请选择入库单明细
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { SystemTableEnums.Name.PIS_StockInDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (paramDetail.SID_Barcode == _latestBarcode)
            {
                return;
            }
            _latestBarcode = paramDetail.SID_Barcode;

            flowLayoutPanelPicture.Controls.Clear();
            _pictureExpandList.Clear();

            //当前明细对应的配件图片列表
            List<AutoPartsPictureUIModel> curDetailAutoPartsPictureList = new List<AutoPartsPictureUIModel>();
            curDetailAutoPartsPictureList = _autoPartsPictureList.Where(x => x.INVP_Barcode == paramDetail.SID_Barcode).ToList();

            if (curDetailAutoPartsPictureList.Count == 0)
            {
                //配件无图片时，加载一个扩展的图片控件
                //添加空图片控件
                AutoPartsPictureUIModel nullAutoPartsPicture = new AutoPartsPictureUIModel()
                {
                    INVP_Barcode = paramDetail.SID_Barcode,
                };
                AddNullPictureControl(nullAutoPartsPicture);
            }
            else
            {
                //配件有图片时，加载实际数量的扩展的图片控件以及图片
                foreach (var loopPicture in curDetailAutoPartsPictureList)
                {
                    if (string.IsNullOrEmpty(loopPicture.INVP_ID)
                        || string.IsNullOrEmpty(loopPicture.INVP_PictureName))
                    {
                        continue;
                    }

                    loopPicture.INVP_Barcode = paramDetail.SID_Barcode;
                    Dictionary<string, AutoPartsPictureUIModel> pictureDictionary = new Dictionary<string, AutoPartsPictureUIModel>();
                    if (!pictureDictionary.ContainsKey(loopPicture.INVP_PictureName))
                    {
                        pictureDictionary.Add(loopPicture.INVP_PictureName, loopPicture);
                    }
                    ThreadPool.QueueUserWorkItem(new WaitCallback(LoadImage), pictureDictionary);
                }
                AutoPartsPictureUIModel nullAutoPartsPicture = new AutoPartsPictureUIModel()
                {
                    INVP_Barcode = paramDetail.SID_Barcode,
                };
                //添加空图片控件
                AddNullPictureControl(nullAutoPartsPicture);
            }
        }

        /// <summary>
        /// 异步加载配件图片
        /// </summary>
        /// <param name="paramPictureDic">图片名称和Image Dic</param>
        private void LoadImage(object paramPictureDic)
        {
            Dictionary<string, AutoPartsPictureUIModel> pictureDictionary = (Dictionary<string, AutoPartsPictureUIModel>)paramPictureDic;
            if (paramPictureDic == null)
            {
                return;
            }
            string curPictureName = pictureDictionary.FirstOrDefault(x => !string.IsNullOrEmpty(x.Key)).Key;

            if (!string.IsNullOrEmpty(curPictureName))
            {
                object tempImage = null;
                //通过文件名获取Image
                tempImage = BLLCom.GetBitmapImageByFileName(curPictureName);
                if (tempImage != null)
                {
                    this.Invoke((Action)(() =>
                    {
                        SkyCarPictureExpand autoPartsPicture = new SkyCarPictureExpand
                        {
                            //图片名称作为操作图片的唯一标识
                            PictureKey = curPictureName,
                            //图片Image
                            PictureImage = tempImage,
                            //待保存的配件图片TBModel
                            PropertyModel = pictureDictionary[curPictureName],
                            //上传图片
                            ExecuteUpload = UploadPicture,
                            //导出图片
                            ExecuteExport = ExportPicture,
                            //删除图片
                            ExecuteDelete = DeletePicture
                        };
                        if (autoPartsPicture.PictureImage != null)
                        {
                            flowLayoutPanelPicture.Controls.Add(autoPartsPicture);
                            _pictureExpandList.Add(autoPartsPicture);
                        }
                    }));
                }
            }
        }

        #region 单独操作图片

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="paramPictureKey">图片唯一标识</param>
        /// <param name="paramAutoPartsPicture">配件图片</param>
        /// <returns></returns>
        private object UploadPicture(string paramPictureKey, object paramAutoPartsPicture = null)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                if (fileDialog.ShowDialog() != DialogResult.OK)
                {
                    return null;
                }

                Image curImage =
                    Image.FromStream(WebRequest.Create(fileDialog.FileName).GetResponse().GetResponseStream());
                //临时保存图片
                string tempFileName = Application.StartupPath + @"\" + paramPictureKey;
                curImage.Save(tempFileName, ImageFormat.Jpeg);

                AutoPartsPictureUIModel curAutoPartsPicture = paramAutoPartsPicture as AutoPartsPictureUIModel;
                if (_autoPartsPictureList.Count == 0
                    ||
                    _autoPartsPictureList.Any(
                        x => x.INVP_PictureName != paramPictureKey && x.SourceFilePath != tempFileName))
                {
                    AutoPartsPictureUIModel newAutoPartsPicture = new AutoPartsPictureUIModel
                    {
                        INVP_PictureName = paramPictureKey,
                        SourceFilePath = tempFileName,
                    };
                    if (curAutoPartsPicture != null)
                    {
                        newAutoPartsPicture.INVP_Barcode = curAutoPartsPicture.INVP_Barcode;
                    }
                    _autoPartsPictureList.Add(newAutoPartsPicture);
                }
                else
                {
                    var curUploadPicture =
                        _autoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == paramPictureKey);
                    if (curUploadPicture != null)
                    {
                        curUploadPicture.SourceFilePath = tempFileName;
                    }
                }

                var nullImagePicture = _pictureExpandList.Where(x => x.PictureImage == null).ToList();
                if (nullImagePicture.Count == 1)
                {
                    //添加空图片控件
                    AddNullPictureControl(curAutoPartsPicture);
                }

                //设置图片是否可见、可编辑
                if (curAutoPartsPicture != null)
                {
                    var tempPicture = _pictureExpandList.FirstOrDefault(x => x.PictureKey == paramPictureKey);
                    if (tempPicture != null)
                    {
                        tempPicture.PictureImage = curImage;
                    }
                    var curDetail = _detailGridDS.FirstOrDefault(x => x.SID_Barcode == curAutoPartsPicture.INVP_Barcode);
                    if (curDetail != null)
                    {
                        SetPictureControl(curDetail);
                    }
                }

                return curImage;
            }
            catch (Exception ex)
            {
                //上传图片失败
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0018, new object[] { MsgParam.UPLOAD_IMAGE, ex.Message }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// 导出图片
        /// </summary>
        /// <param name="paramPictureKey">图片唯一标识</param>
        /// <returns></returns>
        private bool ExportPicture(string paramPictureKey)
        {
            //是否需要从服务器下载文件
            bool isDownFromWeb = false;
            //本地临时路径
            string tempLocalPicturePath = string.Empty;
            if (_autoPartsPictureList == null || _autoPartsPictureList.Count == 0)
            {
                isDownFromWeb = true;
            }
            else
            {
                var curPicture = _autoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == paramPictureKey);
                if (curPicture != null)
                {
                    tempLocalPicturePath = curPicture.SourceFilePath;
                }
                if (string.IsNullOrEmpty(tempLocalPicturePath))
                {
                    isDownFromWeb = true;
                }
                else
                {
                    isDownFromWeb = false;
                }
            }
            if (isDownFromWeb == false)
            {
                SaveFileDialog saveImageDialog = new SaveFileDialog
                {
                    Title = "图片保存",
                    //文件类型
                    Filter = @"jpeg|*.jpg|bmp|*.bmp|gif|*.gif",
                    //默认文件名
                    FileName = paramPictureKey,
                    //保存对话框是否记忆上次打开的目录
                    RestoreDirectory = true,
                };
                if (saveImageDialog.ShowDialog() != DialogResult.OK)
                {
                    return true;
                }
                string destFileName = saveImageDialog.FileName;
                if (string.IsNullOrEmpty(destFileName))
                {
                    return true;
                }

                //从本地Copy到选择的路径
                File.Copy(tempLocalPicturePath, saveImageDialog.FileName, true);
                //导出成功
                MessageBoxs.Show(Trans.PIS, ToString(),
                    MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.EXPORT }), MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                //消息
                var outMsg = string.Empty;
                bool exportResult = BLLCom.ExportFileByFileName(paramPictureKey, ref outMsg);
                if (exportResult == false)
                {
                    //导出失败
                    MessageBoxs.Show(Trans.PIS, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (string.IsNullOrEmpty(outMsg))
                {
                    return true;
                }
                //导出成功
                MessageBoxs.Show(Trans.PIS, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return true;
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="paramPictureKey"></param>
        /// <param name="paramAutoPartsPicture">配件图片</param>
        /// <returns></returns>
        private bool DeletePicture(string paramPictureKey, object paramAutoPartsPicture = null)
        {
            if (paramAutoPartsPicture == null)
            {
                //未保存的场合，仅清除显示图片
                //删除当前图片控件
                var deleteImage = _pictureExpandList.FirstOrDefault(x => x.PictureKey == paramPictureKey);
                if (deleteImage != null)
                {
                    flowLayoutPanelPicture.Controls.Remove(deleteImage);
                    _pictureExpandList.Remove(deleteImage);
                }
                //移除配件图片列表中数据
                var deletePicture = _autoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == paramPictureKey);
                if (deletePicture != null)
                {
                    _autoPartsPictureList.Remove(deletePicture);
                }
                return true;
            }
            //待删除的配件图片
            AutoPartsPictureUIModel argsAutoPartsPicture = paramAutoPartsPicture as AutoPartsPictureUIModel;

            if (argsAutoPartsPicture == null
                || string.IsNullOrEmpty(argsAutoPartsPicture.INVP_PictureName))
            {
                //未保存的场合，仅清除显示图片
                //删除当前图片控件
                var deleteImage = _pictureExpandList.FirstOrDefault(x => x.PictureKey == paramPictureKey);
                if (deleteImage != null)
                {
                    flowLayoutPanelPicture.Controls.Remove(deleteImage);
                    _pictureExpandList.Remove(deleteImage);
                }
                return true;
            }

            //错误消息
            var outMsg = string.Empty;
            //删除本地和文件服务器中的图片
            bool deleteResult = BLLCom.DeleteFileByFileName(paramPictureKey, ref outMsg);
            if (deleteResult == false)
            {
                //删除失败
                MessageBoxs.Show(Trans.PIS, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            #region 删除数据库中图片
            MDLPIS_InventoryPicture deleteAutoPartsPicture = new MDLPIS_InventoryPicture();
            if (!string.IsNullOrEmpty(argsAutoPartsPicture.INVP_ID))
            {
                deleteAutoPartsPicture.WHERE_INVP_ID = argsAutoPartsPicture.INVP_ID;
                deleteAutoPartsPicture.WHERE_INVP_VersionNo = argsAutoPartsPicture.INVP_VersionNo;
                try
                {
                    bool deleteInvPicture = _bll.Delete<MDLPIS_InventoryPicture>(deleteAutoPartsPicture);
                    if (!deleteInvPicture)
                    {
                        //删除失败
                        MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    //删除失败，失败原因：ex.Message
                    MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.DELETE, ex.Message }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            #endregion

            //删除成功
            MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //删除当前图片控件
            var removeImage = _pictureExpandList.FirstOrDefault(x => x.PictureKey == paramPictureKey);
            if (removeImage != null)
            {
                flowLayoutPanelPicture.Controls.Remove(removeImage);
                _pictureExpandList.Remove(removeImage);
            }
            //移除配件图片列表中数据
            var removePicture = _autoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == paramPictureKey);
            if (removePicture != null)
            {
                _autoPartsPictureList.Remove(removePicture);
            }

            return true;
        }

        #endregion

        #region 批量操作图片

        /// <summary>
        /// 全选图片
        /// </summary>
        /// <param name="paramDetailId">当前选中明细ID</param>
        private void SelectAllPicture(string paramDetailId)
        {
            if (_pictureExpandList.Count == 0)
            {
                return;
            }
            foreach (var loopPicture in _pictureExpandList)
            {
                if (loopPicture.IsCheckedIsVisible == false
                    || loopPicture.IsCheckedIsEnabled == false)
                {
                    continue;
                }
                var curAutoPartsPicture = loopPicture.PropertyModel as AutoPartsPictureUIModel;
                if (curAutoPartsPicture == null || curAutoPartsPicture.INVP_Barcode != paramDetailId)
                {
                    continue;
                }
                loopPicture.IsChecked = ckSelectAllPicture.Checked;
            }
        }

        /// <summary>
        /// 批量上传图片
        /// </summary>
        /// <param name="paramDetail">入库单明细</param>
        private void BatchUploadPicture(StockInBillDetailManagerUIModel paramDetail)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Multiselect = true;
                if (fileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                foreach (var loopFileName in fileDialog.FileNames)
                {
                    Image curImage = Image.FromStream(WebRequest.Create(loopFileName).GetResponse().GetResponseStream());

                    AutoPartsPictureUIModel curAutoPartsPicture = new AutoPartsPictureUIModel()
                    {
                        INVP_Barcode = paramDetail.SID_Barcode,
                    };
                    SkyCarPictureExpand autoPartsPicture = new SkyCarPictureExpand
                    {
                        //图片名称作为操作图片的唯一标识
                        PictureKey = Guid.NewGuid() + ".jpg",
                        //当前图片Model
                        PropertyModel = curAutoPartsPicture,
                        //上传图片
                        ExecuteUpload = UploadPicture,
                        //导出图片
                        ExecuteExport = ExportPicture,
                        //删除图片
                        ExecuteDelete = DeletePicture,
                        //图片Image
                        PictureImage = curImage
                    };
                    var removePicture = _pictureExpandList.FirstOrDefault(x => x.PictureImage == null);
                    if (removePicture != null)
                    {
                        flowLayoutPanelPicture.Controls.Remove(removePicture);
                        _pictureExpandList.Remove(removePicture);
                    }

                    flowLayoutPanelPicture.Controls.Add(autoPartsPicture);
                    _pictureExpandList.Add(autoPartsPicture);

                    //临时保存图片
                    string tempFileName = Application.StartupPath + @"\" + autoPartsPicture.PictureKey;
                    curImage.Save(tempFileName, ImageFormat.Jpeg);

                    if ((_autoPartsPictureList.Count == 0
                        || _autoPartsPictureList.Any(x => x.INVP_PictureName != autoPartsPicture.PictureKey && x.SourceFilePath != tempFileName)))
                    {
                        //第一次上传的场合，新增待保存的配件图片
                        AutoPartsPictureUIModel newAutoPartsPicture = new AutoPartsPictureUIModel
                        {
                            INVP_Barcode = paramDetail.SID_Barcode,
                            INVP_PictureName = autoPartsPicture.PictureKey,
                            SourceFilePath = tempFileName,
                        };
                        _autoPartsPictureList.Add(newAutoPartsPicture);
                    }
                    else
                    {
                        //更新上传的场合，更新图片文件源
                        var curUploadPicture = _autoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == autoPartsPicture.PictureKey);
                        if (curUploadPicture != null)
                        {
                            curUploadPicture.SourceFilePath = tempFileName;
                        }
                    }
                }

                //添加空图片控件
                AutoPartsPictureUIModel nullAutoPartsPicture = new AutoPartsPictureUIModel()
                {
                    INVP_Barcode = paramDetail.SID_Barcode,
                };
                AddNullPictureControl(nullAutoPartsPicture);

                //设置图片是否可见、可编辑
                SetPictureControl(paramDetail);
            }
            catch (Exception ex)
            {
                //上传图片失败
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0018, new object[] { MsgParam.UPLOAD_IMAGE, ex.Message }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// 批量导出时的默认路径
        /// </summary>
        private string _defaultFilePath = string.Empty;
        /// <summary>
        /// 批量导出图片
        /// </summary>
        /// <param name="paramDetail">入库单明细</param>
        private void BatchExportPicture(StockInBillDetailManagerUIModel paramDetail)
        {
            #region 验证

            //勾选的图片列表
            var checkedPictureExpandList = _pictureExpandList.Where(x => x.IsChecked == true && x.PictureImage != null).ToList();
            List<AutoPartsPictureUIModel> curDetailAutoPartsPictureList = new List<AutoPartsPictureUIModel>();
            foreach (var loopCheckedPictureExpand in checkedPictureExpandList)
            {
                var curAutoPartsPicture = loopCheckedPictureExpand.PropertyModel as AutoPartsPictureUIModel;
                if (curAutoPartsPicture == null || curAutoPartsPicture.INVP_Barcode != paramDetail.SID_Barcode)
                {
                    continue;
                }
                curDetailAutoPartsPictureList.Add(curAutoPartsPicture);
            }
            if (curDetailAutoPartsPictureList.Count == 0)
            {
                //请选择图片
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.IMAGE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #endregion

            List<string> fileNameList = new List<string>();
            //导出图片
            foreach (var loopPicture in curDetailAutoPartsPictureList)
            {
                if (string.IsNullOrEmpty(loopPicture.INVP_PictureName))
                {
                    continue;
                }
                fileNameList.Add(loopPicture.INVP_PictureName);
            }

            //错误消息
            var outMsg = string.Empty;
            bool exportResult = BLLCom.ExportFileListByFileNameList(fileNameList, ref _defaultFilePath, ref outMsg);
            if (exportResult == false)
            {
                //导出失败
                MessageBoxs.Show(Trans.PIS, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //导出成功
            MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.EXPORT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 批量删除图片
        /// </summary>
        /// <param name="paramDetail">入库单明细</param>
        private void BatchDeletePicture(StockInBillDetailManagerUIModel paramDetail)
        {
            #region 验证

            //勾选的图片列表
            var checkedPictureExpandList = _pictureExpandList.Where(x => x.IsChecked == true && x.PictureImage != null).ToList();
            List<AutoPartsPictureUIModel> curDetailAutoPartsPictureList = new List<AutoPartsPictureUIModel>();
            foreach (var loopCheckedPictureExpand in checkedPictureExpandList)
            {
                var curAutoPartsPicture = loopCheckedPictureExpand.PropertyModel as AutoPartsPictureUIModel;
                if (curAutoPartsPicture == null || curAutoPartsPicture.INVP_Barcode != paramDetail.SID_Barcode)
                {
                    continue;
                }
                curDetailAutoPartsPictureList.Add(curAutoPartsPicture);
            }
            if (curDetailAutoPartsPictureList.Count == 0)
            {
                //请选择图片
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.IMAGE }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //已选checkedPictureList.Count 条数据，确定删除？\r\n单击【确定】删除，【取消】返回。
            DialogResult dialogResult = MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.W_0013, new object[] { curDetailAutoPartsPictureList.Count }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            #endregion

            //待删除的配件图片列表
            List<MDLPIS_InventoryPicture> deleteAutoPartsPictureList = new List<MDLPIS_InventoryPicture>();

            string pictureNameString = string.Empty;
            foreach (var loopPicture in curDetailAutoPartsPictureList)
            {
                if (string.IsNullOrEmpty(loopPicture.INVP_PictureName))
                {
                    continue;
                }
                pictureNameString += loopPicture.INVP_PictureName + SysConst.Semicolon_DBC;
            }
            //根据图片名称查询待删除的配件图片
            _bll.QueryForList(SQLID.COMM_SQL49, new MDLPIS_InventoryPicture
            {
                WHERE_INVP_Barcode = paramDetail.SID_Barcode + SysConst.Semicolon_DBC,
                WHERE_INVP_PictureName = pictureNameString
            }, deleteAutoPartsPictureList);
            foreach (var loopPictureExpand in checkedPictureExpandList)
            {
                loopPictureExpand.PictureImage = null;

                var curAutoPartsPicture = deleteAutoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == loopPictureExpand.PictureKey);
                if (curAutoPartsPicture == null)
                {
                    //未保存到数据库的图片，仅清除
                    continue;
                }

                #region 删除本地和文件服务器中的图片

                //错误消息
                var outMsg = string.Empty;
                //删除本地和文件服务器中的图片
                bool deleteResult = BLLCom.DeleteFileByFileName(curAutoPartsPicture.INVP_PictureName, ref outMsg);
                if (deleteResult == false)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.PIS, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion
            }

            #region 删除数据库中配件图片

            if (deleteAutoPartsPictureList.Count > 0)
            {
                var outMsg = string.Empty;
                bool deleteDataResult = AutoPartsComFunction.DeleteAutoPartsPicture(deleteAutoPartsPictureList, ref outMsg);
                if (!deleteDataResult)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.PIS, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            //删除成功
            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            #endregion

            foreach (var loopPicture in deleteAutoPartsPictureList)
            {
                //删除当前图片控件
                var removeImage = _pictureExpandList.FirstOrDefault(x => x.PictureKey == loopPicture.INVP_PictureName);
                if (removeImage != null)
                {
                    flowLayoutPanelPicture.Controls.Remove(removeImage);
                    _pictureExpandList.Remove(removeImage);
                }
                //移除配件图片列表中数据
                var removePicture = _autoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == loopPicture.INVP_PictureName);
                if (removePicture != null)
                {
                    _autoPartsPictureList.Remove(removePicture);
                }
            }
        }

        #endregion

        /// <summary>
        /// 设置图片是否可见、可编辑
        /// </summary>
        /// <param name="paramDetail">入库单明细</param>
        private void SetPictureControl(StockInBillDetailManagerUIModel paramDetail = null)
        {
            if (paramDetail == null
                || string.IsNullOrEmpty(paramDetail.SID_Barcode)
                || _isCanEditPicture == false)
            {
                flowLayoutPanelPicture.Controls.Clear();
                _pictureExpandList.Clear();

                //入库单明细为空的场合 或者 无保存配件档案权限的场合，不可上传、导出、删除图片
                ckSelectAllPicture.Enabled = false;
                toolbarsAutoPartsPicture.Tools[SysConst.Upload].SharedProps.Enabled = false;
                toolbarsAutoPartsPicture.Tools[SysConst.Export].SharedProps.Enabled = false;
                toolbarsAutoPartsPicture.Tools[SysConst.EN_DEL].SharedProps.Enabled = false;
                foreach (var loopPicture in _pictureExpandList)
                {
                    loopPicture.IsCheckedIsVisible = false;
                    loopPicture.UploadIsVisible = false;
                    loopPicture.ExportIsVisible = false;
                    loopPicture.DeleteIsVisible = false;
                }
            }
            else
            {
                //入库单明细不为空的场合，可上传、导出、删除图片

                var curDetailAutoPartsPictureList = _autoPartsPictureList.Where(x => x.INVP_Barcode == paramDetail.SID_Barcode).ToList();

                ckSelectAllPicture.Enabled = true;
                toolbarsAutoPartsPicture.Tools[SysConst.Upload].SharedProps.Enabled = true;
                toolbarsAutoPartsPicture.Tools[SysConst.Export].SharedProps.Enabled = true;
                toolbarsAutoPartsPicture.Tools[SysConst.EN_DEL].SharedProps.Enabled = true;

                if (toolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[SystemActionEnum.Code.SAVE].SharedPropsInternal.Enabled
                    || toolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[SystemActionEnum.Code.APPROVE].SharedPropsInternal.Enabled)
                {
                    if (curDetailAutoPartsPictureList.Count == 0)
                    {
                        //无图片的场合，不可导出、删除图片
                        ckSelectAllPicture.Enabled = false;
                        toolbarsAutoPartsPicture.Tools[SysConst.Export].SharedProps.Enabled = false;
                        toolbarsAutoPartsPicture.Tools[SysConst.EN_DEL].SharedProps.Enabled = false;
                    }
                    else
                    {
                        //有图片的场合，可导出、删除图片
                        ckSelectAllPicture.Enabled = true;
                        toolbarsAutoPartsPicture.Tools[SysConst.Export].SharedProps.Enabled = true;
                        toolbarsAutoPartsPicture.Tools[SysConst.EN_DEL].SharedProps.Enabled = true;
                    }
                    //配件图片可保存的场合，可上传图片
                    toolbarsAutoPartsPicture.Tools[SysConst.Upload].SharedProps.Enabled = true;
                    foreach (var loopPicture in _pictureExpandList)
                    {
                        loopPicture.IsCheckedIsVisible = true;
                        loopPicture.UploadIsVisible = true;
                        loopPicture.ExportIsVisible = true;
                        loopPicture.DeleteIsVisible = true;

                        loopPicture.IsCheckedIsEnabled = loopPicture.PictureImage != null;
                        loopPicture.UploadIsEnabled = true;
                        loopPicture.DeleteIsEnabled = loopPicture.PictureImage != null;
                        loopPicture.ExportIsEnabled = loopPicture.PictureImage != null;
                    }
                }
                else
                {
                    //配件图片不可保存的场合，不可上传、删除图片
                    toolbarsAutoPartsPicture.Tools[SysConst.Upload].SharedProps.Enabled = false;
                    toolbarsAutoPartsPicture.Tools[SysConst.EN_DEL].SharedProps.Enabled = false;
                    foreach (var loopPicture in _pictureExpandList)
                    {
                        loopPicture.IsCheckedIsVisible = loopPicture.PictureImage != null;
                        loopPicture.UploadIsVisible = false;
                        loopPicture.ExportIsVisible = loopPicture.PictureImage != null;
                        loopPicture.DeleteIsVisible = false;
                    }
                    if (curDetailAutoPartsPictureList.Count == 0)
                    {
                        //无图片的场合，不可导出、删除图片
                        ckSelectAllPicture.Enabled = false;
                        toolbarsAutoPartsPicture.Tools[SysConst.Export].SharedProps.Enabled = false;
                    }
                    else
                    {
                        //有图片的场合，可导出、删除图片
                        ckSelectAllPicture.Enabled = true;
                        toolbarsAutoPartsPicture.Tools[SysConst.Export].SharedProps.Enabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// 添加空图片控件
        /// </summary>
        /// <param name="paramAutoPartsPicture">入库单明细</param>
        private void AddNullPictureControl(AutoPartsPictureUIModel paramAutoPartsPicture)
        {
            SkyCarPictureExpand addAutoPartsPicture = new SkyCarPictureExpand
            {
                //图片名称作为操作图片的唯一标识
                PictureKey = Guid.NewGuid() + ".jpg",
                //待保存的配件图片TBModel
                PropertyModel = paramAutoPartsPicture,
                //上传图片
                ExecuteUpload = UploadPicture,
                //导出图片
                ExecuteExport = ExportPicture,
                //删除图片
                ExecuteDelete = DeletePicture,
            };

            flowLayoutPanelPicture.Controls.Add(addAutoPartsPicture);
            _pictureExpandList.Add(addAutoPartsPicture);
        }

        #endregion

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
                    var curHead = HeadGridDS.FirstOrDefault(x => x.SIB_ID == HeadDS.SIB_ID);
                    if (curHead != null)
                    {
                        HeadGridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = HeadGridDS.FirstOrDefault(x => x.SIB_ID == HeadDS.SIB_ID);
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
