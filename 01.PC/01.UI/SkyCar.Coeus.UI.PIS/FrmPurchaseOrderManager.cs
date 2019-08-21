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
using System.Threading;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.PIS;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.PIS;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;
using Infragistics.Win;
using Infragistics.Win.UltraWinTabControl;
using SkyCar.Coeus.BLL.COM;
using SkyCar.Coeus.Common.CustomControl;
using SkyCar.Coeus.UIModel.Common.APModel;
using SkyCar.Coeus.UIModel.PIS.UIModel;

namespace SkyCar.Coeus.UI.PIS
{
    /// <summary>
    /// 采购订单管理
    /// </summary>
    public partial class FrmPurchaseOrderManager : BaseFormCardListDetail<PurchaseOrderManagerUIModel, PurchaseOrderManagerQCModel, MDLPIS_PurchaseOrder>
    {
        #region 全局变量

        /// <summary>
        /// 采购订单管理BLL
        /// </summary>
        private PurchaseOrderManagerBLL _bll = new PurchaseOrderManagerBLL();

        /// <summary>
        /// 【详情】Tab内Grid绑定用的数据源
        /// </summary>
        private SkyCarBindingList<PurchaseOrderManagerDetailUIModel, MDLPIS_PurchaseOrderDetail> _detailGridDS = new SkyCarBindingList<PurchaseOrderManagerDetailUIModel, MDLPIS_PurchaseOrderDetail>();

        /// <summary>
        /// 库存图片列表
        /// </summary>
        List<AutoPartsPictureUIModel> _autoPartsPictureList = new List<AutoPartsPictureUIModel>();

        /// <summary>
        /// 库存图片控件列表
        /// </summary>
        List<SkyCarPictureExpand> _pictureExpandList = new List<SkyCarPictureExpand>();

        /// <summary>
        /// 添加采购明细Func
        /// </summary>
        private Func<MaintainAutoPartsDetailUIModel, bool> _maintainAutoPartsDetailFunc;

        /// <summary>
        /// 页面参数字典
        /// </summary>
        private Dictionary<string, object> _viewParameters;

        #region 下拉框数据源

        /// <summary>
        /// 供应商数据源
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

        /// <summary>
        /// 采购订单明细
        /// </summary>
        public List<PurchaseOrderManagerDetailUIModel> PurchaseOrderDetailList = new List<PurchaseOrderManagerDetailUIModel>();
        #endregion

        #region 系统事件

        /// <summary>
        /// FrmPurchaseOrderManager构造方法
        /// </summary>
        public FrmPurchaseOrderManager()
        {
            InitializeComponent();
            _maintainAutoPartsDetailFunc = MaintainAutoPartsDetail;
        }

        /// <summary>
        /// FrmPurchaseOrderManager构造方法
        /// </summary>
        public FrmPurchaseOrderManager(Dictionary<string, object> paramWindowParameters)
        {
            InitializeComponent();
            _viewParameters = paramWindowParameters;
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmPurchaseOrderManager_Load(object sender, EventArgs e)
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

            //给下拉框赋值
            SetToComboEditor();
            //[列表]页不允许删除
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                toolBarActionAndNavigate.Tools[SystemActionEnum.Code.SIGNIN].SharedProps.Enabled = false;
            }
            //[打印]不可用
            SetActionEnable(SystemActionEnum.Code.PRINT, false);

            #region 处理参数

            if (_viewParameters != null)
            {
                #region 采购单

                if (_viewParameters.ContainsKey(PISViewParamKey.PurchaseOrder.ToString()))
                {
                    MDLPIS_PurchaseOrder resultPurchaseOrder = _viewParameters[PISViewParamKey.PurchaseOrder.ToString()] as MDLPIS_PurchaseOrder;

                    if (resultPurchaseOrder != null)
                    {
                        //供应商
                        mcbPO_SUPP_Name.SelectedValue = resultPurchaseOrder.PO_SUPP_ID;
                        //来源类型
                        cbPO_SourceTypeName.Text = PurchaseOrderSourceTypeEnum.Name.CGYC;
                        //来源单号
                        txtPO_SourceNo.Text = resultPurchaseOrder.PO_SourceNo;
                        //订单总额
                        numPO_TotalAmount.Value = resultPurchaseOrder.PO_TotalAmount;

                    }
                    //选择【详情】Tab
                    tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;
                }
                #endregion

                #region 采购单明细

                if (_viewParameters.ContainsKey(PISViewParamKey.PurchaseOrderDetail.ToString()))
                {
                    List<MDLPIS_PurchaseOrderDetail> resultPurchaseOrderDetailList = _viewParameters[PISViewParamKey.PurchaseOrderDetail.ToString()] as List<MDLPIS_PurchaseOrderDetail>;

                    if (resultPurchaseOrderDetailList != null)
                    {
                        _detailGridDS.StartMonitChanges();
                        _bll.CopyModelList(resultPurchaseOrderDetailList, _detailGridDS);
                        gdDetail.DataSource = _detailGridDS;
                        gdDetail.DataBind();
                    }
                }
                #endregion
            }

            #endregion

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

        #region 列表Grid相关事件
        
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
        /// 【列表】Grid单元格改变事件
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
                //[列表]页不允许[删除]、[签收]、[核实]、[打印]
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                SetActionEnable(SystemActionEnum.Code.SIGNIN, false);
                SetActionEnable(SystemActionEnum.Code.VERIFY, false);
                SetActionEnable(SystemActionEnum.Code.PRINT, false);

                //[转结算]、[转出库]可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, true);
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSTOCKOUTBILL, true, false);
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
        /// 订单号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_PO_No_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 来源类型ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_PO_SourceTypeName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 来源单号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_PO_SourceNo_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 单据状态ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_PO_StatusName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 审核状态名称ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_PO_ApprovalStatusName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 有效CheckedChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckWhere_PO_IsValid_CheckedChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }

        #endregion

        #region 单头相关事件

        /// <summary>
        /// 来源类型_ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbPO_SourceTypeName_ValueChanged(object sender, EventArgs e)
        {
            if (cbPO_SourceTypeName.Text == PurchaseOrderSourceTypeEnum.Name.CGYC)
            {
                txtPO_SourceNo.Visible = true;
                lblPO_SourceNo.Visible = true;
                lblPO_SourceNoX.Visible = true;
            }
            else
            {
                txtPO_SourceNo.Visible = false;
                lblPO_SourceNo.Visible = false;
                lblPO_SourceNoX.Visible = false;
            }
        }
        #endregion

        #region 采购明细相关事件

        /// <summary>
        /// 采购明细ToolClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolbarsManagerDetail_ToolClick(object sender, ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case SysConst.EN_ADD:
                    //添加
                    AddPurchaseOrderDetail();
                    break;
                case SysConst.EN_DEL:
                    //删除
                    DeletePurchaseOrderDetail();
                    break;
                case SysConst.AllSign:
                    //全部到货
                    AllSign();
                    break;
            }
        }

        /// <summary>
        /// 明细Grid单元格改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_CellChange(object sender, CellEventArgs e)
        {
            gdDetail.UpdateData();

            if (string.IsNullOrEmpty(txtPO_ID.Text.Trim()))
            {
                return;
            }
            #region 验证输入的数据

            //验证本次签收数量是否大于零
            if (e.Cell.Column.Key == "ThisReceivedQty")
            {
                int activeRowIndex = gdDetail.ActiveRow.Index;
                //本次签收数量
                decimal receivedQtying = 0;
                //已验已收数量
                decimal receivedQty = 0;
                //订货数量
                decimal orderQty = 0;
                //未签收数量
                decimal notReceivedQty;
                //验证本次签收数量是否为空
                if (!string.IsNullOrEmpty(e.Cell.Text.Trim()))
                {
                    string id = gdDetail.DisplayLayout.Rows[activeRowIndex].Cells[
                         SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_ID].Text.Trim();
                    foreach (var loopDetail in _detailGridDS)
                    {
                        if (loopDetail.POD_ID == id)
                        {
                            receivedQtying = Convert.ToDecimal(loopDetail.ThisReceivedQty ?? 0);
                            orderQty = Convert.ToDecimal(loopDetail.POD_OrderQty ?? 0);
                            receivedQty = Convert.ToDecimal(loopDetail.POD_ReceivedQty ?? 0);
                            break;
                        }
                    }

                }
                //未签收数量
                notReceivedQty = orderQty - receivedQty;
                if (receivedQtying > notReceivedQty)
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0005, new object[]
                    { MsgParam.SIGN_COUNT,MsgParam.NOTYET + SystemTableColumnEnums.PIS_PurchaseOrderDetail.Name.POD_ReceivedQty }),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cell.Value = Convert.ToDecimal(0);
                    return;
                }
                //验证本次签收数量是否大于零
                if (receivedQtying > 0)
                {
                    gdDetail.DisplayLayout.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_StatusName].Value
                        = PurchaseOrderDetailStatusEnum.Name.YQS;
                    gdDetail.DisplayLayout.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_StatusCode].Value
                        = PurchaseOrderDetailStatusEnum.Code.YQS;
                }
                else
                {
                    gdDetail.DisplayLayout.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_StatusName].Value
                        = PurchaseOrderDetailStatusEnum.Name.YXD;
                    gdDetail.DisplayLayout.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_StatusCode].Value
                        = PurchaseOrderDetailStatusEnum.Code.YXD;
                }
            }

            #endregion

            gdDetail.UpdateData();

            #region 根据详情变化，计算总金额并设置状态下拉框

            //计算总金额
            Decimal totalAmount = 0;
            //全部签收
            Boolean isQBQS = true;
            //部分签收
            Boolean isBFQS = false;
            foreach (var loopDetail in _detailGridDS)
            {
                //已签收数量
                decimal receivedQty = loopDetail.POD_ReceivedQty ?? 0;
                //本次签收数量
                decimal receivedQtying = loopDetail.ThisReceivedQty ?? 0;
                //订货数量
                decimal orderQty = loopDetail.POD_OrderQty ?? 0;
                //订货单价
                decimal unitPrice = loopDetail.POD_UnitPrice ?? 0;
                totalAmount = totalAmount + Math.Round(unitPrice * (receivedQtying + receivedQty), 2);
                if ((receivedQtying != 0 || receivedQty != 0))
                {
                    isBFQS = true;
                }
                if (receivedQty + receivedQtying != orderQty)
                {
                    isQBQS = false;
                }
            }
            numPO_TotalAmount.Text = Convert.ToString(totalAmount);
            if (isBFQS)
            {
                cbPO_StatusName.Text = PurchaseOrderStatusEnum.Name.BFQS;
                cbPO_StatusName.Value = PurchaseOrderStatusEnum.Code.BFQS;
            }
            else
            {
                cbPO_StatusName.Text = PurchaseOrderStatusEnum.Name.YXD;
                cbPO_StatusName.Value = PurchaseOrderStatusEnum.Code.YXD;
            }
            if (isQBQS)
            {
                cbPO_StatusName.Text = PurchaseOrderStatusEnum.Name.QBQS;
                cbPO_StatusName.Value = PurchaseOrderStatusEnum.Code.QBQS;
            }

            #endregion
        }

        /// <summary>
        /// 明细ClickCellButton
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
                    cellIndex.Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_WH_ID].Value = frmWarehouseQuery.SelectedValue;
                }
                //仓库改变，清空仓位
                cellIndex.Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_WHB_ID].Value = null;
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
                    cellIndex.Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_WHB_ID].Value = frmWarehouseBinQuery.SelectedValue;
                }
            }

            //跳转配件图片
            if (e.Cell.Column.Key == "EditAutoPartsPicture")
            {
                if (cellIndex.Cells["POD_AutoPartsBarcode"].Value == null)
                {
                    //请选择采购明细
                    MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { SystemTableEnums.Name.PIS_PurchaseOrderDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //当前采购明细
                PurchaseOrderManagerDetailUIModel curDetail =
                    _detailGridDS.FirstOrDefault(x => x.POD_AutoPartsBarcode == cellIndex.Cells["POD_AutoPartsBarcode"].Value.ToString());
                if (curDetail != null)
                {
                    txtAutoPartsBarcode.Text = curDetail.POD_AutoPartsBarcode;
                }
                //加载配件图片
                LoadAutoPartsPicture(curDetail);

                //设置【配件图片】Tab为选中状态
                tabControlDetail.Tabs[SysConst.AutoPartsPicture].Selected = true;
            }

            gdDetail.UpdateData();
            //设置采购明细Grid自适应列宽（根据单元格内容）
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
            var curDetail = _detailGridDS.FirstOrDefault(x => x.POD_AutoPartsBarcode == txtAutoPartsBarcode.Text);
            if (curDetail == null)
            {
                //请选择入库单明细
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { SystemTableEnums.Name.PIS_PurchaseOrderDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                var curDetail = _detailGridDS.FirstOrDefault(x => x.POD_AutoPartsBarcode == txtAutoPartsBarcode.Text);
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
            //5.给下拉框赋值
            SetToComboEditor();

            //3.设置【详情】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;

            //6.设置详情是否可编辑以及动作按钮是否可用
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
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
            bool saveResult = _bll.SaveDetailDS(HeadDS, _detailGridDS, _autoPartsPictureList);
            if (!saveResult)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //4.开始监控List变化
            _detailGridDS.StartMonitChanges();
            //5.将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //6.设置详情是否可编辑以及动作按钮是否可用
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();

            //为图片控件设置传入的库存图片Model
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
            var argsDetail = new List<MDLPIS_PurchaseOrderDetail>();
            //将HeadDS转换为TBModel对象
            var argsHead = HeadDS.ToTBModelForSaveAndDelete<MDLPIS_PurchaseOrder>();
            //将当前DetailGridDS转换为指定类型的TBModelList
            _detailGridDS.ToTBModelListForUpdateAndDelete<MDLPIS_PurchaseOrderDetail>(argsDetail);
            //过滤明细列表中未保存的数据
            argsDetail = argsDetail.Where(x => !string.IsNullOrEmpty(x.WHERE_POD_ID)).ToList();
            //2.执行删除
            bool deleteResult = _bll.DeleteDetailDS(argsHead, argsDetail);
            if (!deleteResult)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //3.清空【详情】画面数据
            InitializeDetailTabControls();
            //刷新列表
            RefreshList(true);

            //将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
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
            base.ConditionDS = new PurchaseOrderManagerQCModel()
            {
                SqlId = SQLID.PIS_PurchaseOrderManager_SQL_01,
                //订单号
                WHERE_PO_No = txtWhere_PO_No.Text.Trim(),
                //供应商
                WHERE_PO_SUPP_Name = mcbWhere_PO_SUPP_Name.SelectedText,
                //来源类型
                WHERE_PO_SourceTypeName = cbWhere_PO_SourceTypeName.Text.Trim(),
                //来源单号
                WHERE_PO_SourceNo = txtWhere_PO_SourceNo.Text.Trim(),
                //单据状态
                WHERE_PO_StatusName = cbWhere_PO_StatusName.Text.Trim(),
                //审核状态名称
                WHERE_PO_ApprovalStatusName = cbWhere_PO_ApprovalStatusName.Text.Trim(),
                //有效
                WHERE_PO_IsValid = ckWhere_PO_IsValid.Checked,
                //组织ID
                WHERE_PO_Org_ID = LoginInfoDAX.OrgID,
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
            base.ApproveAction();
            if (!ClientCheckForApprove())
            {
                return;
            }
            //将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();

            #region 审核采购单
            bool saveApprove = _bll.ApproveDetailDS(HeadDS, _detailGridDS, _autoPartsPictureList);
            //审核失败
            if (!saveApprove)
            {
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
            //将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();
            
            //设置动作按钮状态
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();

            //为图片控件设置传入的库存图片Model
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
        /// 反审核
        /// </summary>
        public override void UnApproveAction()
        {
            base.UnApproveAction();
            if (!ClientCheckForUnApprove())
            {
                return;
            }
            //将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();

            #region 反审核采购单
            bool saveUnApprove = _bll.UnApproveDetailDS(HeadDS, _detailGridDS);
            //审核失败
            if (!saveUnApprove)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //审核成功
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
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();

            //删除所有图片控件
            _autoPartsPictureList = new List<AutoPartsPictureUIModel>();
            flowLayoutPanelPicture.Controls.Clear();
            _pictureExpandList.Clear();
        }

        /// <summary>
        /// 签收
        /// </summary>
        public override void SignInAction()
        {
            txtPO_No.Focus();
            gdDetail.UpdateData();
            if (!ClientCheckForSignIn())
            {
                return;
            }
            //将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();

            #region 签收采购单
            bool saveSignIn = _bll.SignInDetailDS(HeadDS, _detailGridDS, _autoPartsPictureList);
            //审核失败
            if (!saveSignIn)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //签收成功
            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SIGNIN }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            #endregion

            //刷新列表
            RefreshList();

            //将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //更新明细列表
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //开始监控List变化（该数据源若有其他调整，请在StartMonitChanges()方法执行前调整）
            _detailGridDS.StartMonitChanges();

            //设置动作按钮状态
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();

            //为图片控件设置传入的库存图片Model
            foreach (var loopPicture in _autoPartsPictureList)
            {
                var curPictureExpand = _pictureExpandList.FirstOrDefault(x => x.PictureKey == loopPicture.INVP_PictureName);
                if (curPictureExpand != null)
                {
                    curPictureExpand.PropertyModel = loopPicture;
                }
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        public override void PrintAction()
        {
            base.PrintAction();

            try
            {
                if (HeadDS == null || string.IsNullOrEmpty(HeadDS.PO_No))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, SystemTableEnums.Name.PIS_PurchaseOrder), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //采购订单cbPO_StatusName.Text，不能打印
                if (HeadDS.PO_StatusName != PurchaseOrderStatusEnum.Name.YWC)
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, SystemTableEnums.Name.PIS_PurchaseOrder + cbPO_StatusName.Text, SystemActionEnum.Name.PRINT), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //待打印的采购订单
                PurchaseOrderUIModelToPrint purchaseOrderToPrint = new PurchaseOrderUIModelToPrint();
                _bll.CopyModel(HeadDS, purchaseOrderToPrint);
                //待打印的额采购订单明细
                List<PurchaseOrderDetailUIModelToPrint> purchaseOrderDetailToPrintList = new List<PurchaseOrderDetailUIModelToPrint>();
                _bll.CopyModelList(_detailGridDS, purchaseOrderDetailToPrintList);
                Dictionary<string, object> argsViewParams = new Dictionary<string, object>
                {
                    //采购订单
                    {PISViewParamKey.PurchaseOrder.ToString(), purchaseOrderToPrint},
                    //采购订单明细
                    {PISViewParamKey.PurchaseOrderDetail.ToString(), purchaseOrderDetailToPrintList},
                };

                FrmViewAndPrintPurchaseOrder frmViewAndPrintPurchaseOrder = new FrmViewAndPrintPurchaseOrder(argsViewParams)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = frmViewAndPrintPurchaseOrder.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// 核实
        /// </summary>
        public override void VerifyAction()
        {
            base.VerifyAction();

            if (!ClientCheckForVerify())
            {
                return;
            }
            //将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();

            #region 签收采购单

            bool saveVerify = _bll.VerifyDetailDS(HeadDS, _detailGridDS);
            //核实失败
            if (!saveVerify)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //核实成功
            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.VERIFY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            #endregion

            //开始监控List变化
            _detailGridDS.StartMonitChanges();
            //将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置动作按钮状态
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();
        }

        #region 导航相关

        /// <summary>
        /// 转结算
        /// </summary>
        public override void ToSettlementNavigate()
        {
            #region 签收时，生成的应付单的来源单据记录入库单（入库单与应付单一一对应），所以，不在[采购订单管理]画面转结算，统一到[入库单管理]画面转结算
            return;

            base.ToSettlementNavigate();
            //选中的待付款的[采购单]列表
            List<PurchaseOrderManagerUIModel> selectedPurchaseOrderList = new List<PurchaseOrderManagerUIModel>();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 当前选中Tab为【详情】
                if (HeadDS == null || string.IsNullOrEmpty(HeadDS.PO_No) || HeadDS.PO_StatusName != PurchaseOrderStatusEnum.Name.YWC || HeadDS.APB_UnpaidAmount <= 0)
                {
                    //请选择一个已完成并且未付清的[采购单]转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[]
                        { MsgParam.ONE + PurchaseOrderStatusEnum.Name.YWC + MsgParam.AND + MsgParam.NOTYET + MsgParam.PAY_FULL +MsgParam.OF + SystemTableEnums.Name.PIS_PurchaseOrder + SystemNavigateEnum.Name.TOSETTLEMENT }),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                selectedPurchaseOrderList.Add(HeadDS);
                #endregion
            }
            else
            {
                #region 当前选中Tab为【列表】

                var checkedGrid = HeadGridDS.Where(x => x.IsChecked == true).ToList();

                if (checkedGrid.Count == 0)
                {
                    //请至少勾选一条已完成并且未付清的[采购单]信息进行转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[]
                        { PurchaseOrderStatusEnum.Name.YWC + MsgParam.AND + MsgParam.NOTYET + MsgParam.PAY_FULL +MsgParam.OF + SystemTableEnums.Name.PIS_PurchaseOrder , SystemNavigateEnum.Name.TOSETTLEMENT }),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var JYCGAndPayFullList = checkedGrid.Where(x => x.PO_StatusName != StockOutBillStatusEnum.Name.YWC || x.APB_UnpaidAmount <= 0).ToList();
                if (JYCGAndPayFullList.Count > 0)
                {
                    //请选择一个已完成并且未收清的的[采购单]转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ONE + StockOutBillStatusEnum.Name.YWC + MsgParam.AND + MsgParam.NOTYET + MsgParam.RECEIVE__FULL + MsgParam.OF + SystemTableEnums.Name.PIS_PurchaseOrder + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    foreach (var loopCheckedItem in JYCGAndPayFullList)
                    {
                        loopCheckedItem.IsChecked = false;
                    }
                    gdGrid.DataSource = HeadGridDS;
                    gdGrid.DataBind();
                    return;
                }
                var tempCannotToPaytList = checkedGrid.Where(x => x.PO_ApprovalStatusName != ApprovalStatusEnum.Name.YSH
                                                                || x.PO_StatusName != PurchaseOrderStatusEnum.Name.YWC).ToList();
                if (tempCannotToPaytList.Count > 0)
                {
                    //请选择一个已完成并且未收清的的[采购单]转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ONE + StockOutBillStatusEnum.Name.YWC + MsgParam.AND + MsgParam.NOTYET + MsgParam.RECEIVE__FULL + MsgParam.OF + SystemTableEnums.Name.PIS_PurchaseOrder + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    foreach (var loopCannotToReceipt in tempCannotToPaytList)
                    {
                        loopCannotToReceipt.IsChecked = false;
                    }
                    gdGrid.DataSource = HeadGridDS;
                    gdGrid.DataBind();
                    return;
                }

                var firstCheckedItem = checkedGrid.FirstOrDefault(x => x.PO_ApprovalStatusName == ApprovalStatusEnum.Name.YSH && x.PO_StatusName == PurchaseOrderStatusEnum.Name.YWC);
                if (firstCheckedItem == null)
                {
                    //请至少勾选一条已完成并且未付清的[采购单]信息进行转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[]
                        { PurchaseOrderStatusEnum.Name.YWC + MsgParam.AND + MsgParam.NOTYET + MsgParam.PAY_FULL +MsgParam.OF + SystemTableEnums.Name.PIS_PurchaseOrder , SystemNavigateEnum.Name.TOSETTLEMENT }),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //与第一个选择的采购订单客户ID不同 或 来源不同的选中项
                var differFirstCheckedItem = checkedGrid.Where(x => x.PO_SUPP_ID != firstCheckedItem.PO_SUPP_ID
                || x.PO_SourceTypeName != firstCheckedItem.PO_SourceTypeName).ToList();
                if (differFirstCheckedItem.Count > 0)
                {
                    //请选择相同来源类型，相同供应商的采购订单转收款
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { ApprovalStatusEnum.Name.YSH + MsgParam.OF + MsgParam.SAME_SOURCEANDSUPPLIER + SystemNavigateEnum.Name.TORECEIPTBILL }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    foreach (var loopCheckedItem in differFirstCheckedItem)
                    {
                        loopCheckedItem.IsChecked = false;
                    }
                    gdGrid.DataSource = HeadGridDS;
                    gdGrid.DataBind();
                    return;
                }

                //已付清的[采购单]
                var tempYfqAccountPayableBill = checkedGrid.Where(x => (x.APB_PaidAmount ?? 0) >= (x.APB_AccountPayableAmount ?? 0)).ToList();
                if (tempYfqAccountPayableBill.Count > 0)
                {
                    //已付金额大于应付金额，是否确认{支付}？\r\n单击【确定】{支付}单据，【取消】返回。
                    DialogResult dialogResultOK = MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0037, new object[] { MsgParam.PAID_AMOUNT, MsgParam.AMOUNTS_PAYABLE, MsgParam.PAY }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                    if (dialogResultOK != DialogResult.OK)
                    {
                        return;
                    }
                }
                _bll.CopyModelList(checkedGrid, selectedPurchaseOrderList);

                #endregion
            }
            //传入的待收款的[销售订单]列表
            PurchaseOrderDataSet.PurchaseOrderDataTable purchaseOrderDataTable = new PurchaseOrderDataSet.PurchaseOrderDataTable();

            foreach (var loopPurchaseOrderDetail in selectedPurchaseOrderList)
            {
                if (string.IsNullOrEmpty(loopPurchaseOrderDetail.PO_No))
                {
                    continue;
                }

                PurchaseOrderDataSet.PurchaseOrderRow newPurchaseOrderDetailRow = purchaseOrderDataTable.NewPurchaseOrderRow();
                newPurchaseOrderDetailRow.PO_No = loopPurchaseOrderDetail.PO_No;
                newPurchaseOrderDetailRow.PO_SourceTypeName = loopPurchaseOrderDetail.PO_SourceTypeName;
                newPurchaseOrderDetailRow.PO_Org_ID = loopPurchaseOrderDetail.PO_Org_ID;

                purchaseOrderDataTable.AddPurchaseOrderRow(newPurchaseOrderDetailRow);
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

            PurchaseOrderDataSet.PurchaseOrderToPayDataTable resultPurchaseOrderToPayList =
                new PurchaseOrderDataSet.PurchaseOrderToPayDataTable();

            try
            {
                cmd.CommandText = "P_PIS_GetPurchaseOrderListToPay";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PurchaseOrderList", SqlDbType.Structured);
                cmd.Parameters[0].Value = purchaseOrderDataTable;

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(resultPurchaseOrderToPayList);

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

            #region 转结算

            //待确认付款的采购订单列表
            List<PurchaseOrderToPayConfirmWindowModel> purchaseOrderToReceiptList = new List<PurchaseOrderToPayConfirmWindowModel>();
            foreach (var loopSelectedPurchaseOrder in resultPurchaseOrderToPayList)
            {
                PurchaseOrderToPayConfirmWindowModel purchaseOrderToReceipt = new PurchaseOrderToPayConfirmWindowModel
                {
                    BusinessID = loopSelectedPurchaseOrder.BusinessID,
                    BusinessNo = loopSelectedPurchaseOrder.BusinessNo,
                    BusinessOrgID = loopSelectedPurchaseOrder.BusinessOrgID,
                    BusinessOrgName = loopSelectedPurchaseOrder.BusinessOrgName,
                    BusinessSourceTypeName = loopSelectedPurchaseOrder.BusinessSourceTypeName,
                    BusinessSourceTypeCode = loopSelectedPurchaseOrder.BusinessSourceTypeCode,
                    BusinessSourceNo = loopSelectedPurchaseOrder.BusinessSourceNo,
                    //收款对象类型为{供应商}
                    ReceiveObjectTypeName = AmountTransObjectTypeEnum.Name.AUTOPARTSSUPPLIER,
                    ReceiveObjectTypeCode = AmountTransObjectTypeEnum.Code.AUTOPARTSSUPPLIER,
                    //收款对应为供应商
                    ReceiveObjectID = loopSelectedPurchaseOrder.PayObjectID,
                    ReceiveObjectName = loopSelectedPurchaseOrder.PayObjectName,
                    //应付总金额
                    PayableTotalAmount = loopSelectedPurchaseOrder.PayableTotalAmount,
                    //已付总金额
                    PayTotalAmount = loopSelectedPurchaseOrder.PaidTotalAmount,
                    //未付总金额
                    UnPayTotalAmount = loopSelectedPurchaseOrder.UnpaidTotalAmount,

                    //应付单相关
                    APB_SourceTypeCode = loopSelectedPurchaseOrder.APB_SourceTypeCode,
                    APB_SourceTypeName = loopSelectedPurchaseOrder.APB_SourceTypeName,
                    APB_SourceBillNo = loopSelectedPurchaseOrder.APB_SourceBillNo,
                    APB_AccountPayableAmount = loopSelectedPurchaseOrder.APB_AccountPayableAmount,
                    APB_PaidAmount = loopSelectedPurchaseOrder.APB_PaidAmount,
                    APB_UnpaidAmount = loopSelectedPurchaseOrder.APB_UnpaidAmount,
                };
                purchaseOrderToReceiptList.Add(purchaseOrderToReceipt);
            }

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //业务单确认付款
                {ComViewParamKey.BusinessPaymentConfirm.ToString(), purchaseOrderToReceiptList}
            };

            //跳转[业务单确认付款弹出窗]
            FrmPurchaseOrderToPayConfirmWindow frmBusinessPayConfirmWindow = new FrmPurchaseOrderToPayConfirmWindow(paramViewParameters)
            {
                StartPosition = FormStartPosition.CenterScreen
            };

            DialogResult dialogResult = frmBusinessPayConfirmWindow.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                QueryAction();
            }
            else
            {
                return;
            }
            #endregion

            #endregion
        }

        /// <summary>
        /// 转出库
        /// </summary>
        public override void ToStockOutBillNavigate()
        {
            base.ToStockOutBillNavigate();

            #region 转出库前检查数据

            if (string.IsNullOrEmpty(txtPO_No.Text.Trim()))
            {
                //没有获取到采购单，转出库失败
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[]
                    { SystemTableEnums.Name.PIS_PurchaseOrder, SystemNavigateEnum.Name.TOSTOCKOUTBILL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cbPO_StatusName.Text != PurchaseOrderStatusEnum.Name.YWC)
            {
                //请先核实，再转出库
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0023, new object[]
                    { SystemActionEnum.Name.VERIFY, SystemNavigateEnum.Name.TOSTOCKOUTBILL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #endregion

            #region 获取数据

            MDLPIS_PurchaseOrder purchaseOrder = new MDLPIS_PurchaseOrder
            {
                PO_No = txtPO_No.Text.Trim(),
                PO_SUPP_Name = mcbPO_SUPP_Name.SelectedText,
                PO_SUPP_ID = mcbPO_SUPP_Name.SelectedValue
            };

            #endregion

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                {PISViewParamKey.PurchaseOrder.ToString(), purchaseOrder},
            };
            //跳转到[采购退货出库管理]
            SystemFunction.ShowViewFromView(MsgParam.PURCHASERETURN_STOCKOUT_MANGER, ViewClassFullNameConst.PIS_FrmPurchaseReturnManager, true, PageDisplayMode.TabPage, paramViewParameters, null);
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
            //订单号
            txtPO_No.Clear();
            //供应商
            mcbPO_SUPP_Name.Clear();
            //来源类型
            cbPO_SourceTypeName.Items.Clear();
            //来源单号
            txtPO_SourceNo.Clear();
            //订单总额
            numPO_TotalAmount.Value = null;
            //物流费
            numPO_LogisticFee.Value = null;
            //单据状态
            cbPO_StatusName.Items.Clear();
            //审核状态
            cbPO_ApprovalStatusName.Items.Clear();
            //到货时间
            dtPO_ReceivedTime.Value = DateTime.Now;
            //有效
            ckPO_IsValid.Checked = true;
            ckPO_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtPO_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtPO_CreatedTime.Value = DateTime.Now;
            //修改人
            txtPO_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtPO_UpdatedTime.Value = DateTime.Now;
            //采购订单ID
            txtPO_ID.Clear();
            //组织ID
            txtPO_Org_ID.Clear();
            txtPO_Org_ID.Text = LoginInfoDAX.OrgID;
            //版本号
            txtPO_VersionNo.Clear();
            //给 订单号 设置焦点
            lblPO_No.Focus();
            #endregion

            #region 初始化下拉框

            //供应商
            _supplierList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsSupplier) as List<MDLPIS_Supplier>;
            mcbPO_SUPP_Name.DisplayMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_Name;
            mcbPO_SUPP_Name.ValueMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_ID;
            mcbPO_SUPP_Name.DataSource = _supplierList;

            #endregion

            //清空[明细]Grid
            _detailGridDS = new SkyCarBindingList<PurchaseOrderManagerDetailUIModel, MDLPIS_PurchaseOrderDetail>();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //1.开始监控List变化（该数据源若有其他调整，请在StartMonitChanges()方法执行前调整）
            _detailGridDS.StartMonitChanges();

            //默认【详情】中Tab选中【采购明细】
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
            //订单号
            txtWhere_PO_No.Clear();
            //供应商
            mcbWhere_PO_SUPP_Name.Clear();
            //来源类型
            cbWhere_PO_SourceTypeName.Clear();
            //来源单号
            txtWhere_PO_SourceNo.Clear();
            //单据状态
            cbWhere_PO_StatusName.Clear();
            //审核状态名称
            cbWhere_PO_ApprovalStatusName.Clear();
            //有效
            ckWhere_PO_IsValid.Checked = true;
            ckWhere_PO_IsValid.CheckState = CheckState.Checked;
            //给 订单号 设置焦点
            lblWhere_PO_No.Focus();
            #endregion

            #region 初始化下拉框

            //供应商
            mcbWhere_PO_SUPP_Name.DisplayMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_Name;
            mcbWhere_PO_SUPP_Name.ValueMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_ID;
            mcbWhere_PO_SUPP_Name.DataSource = _supplierList;

            #endregion

            #region Grid初始化

            HeadGridDS = new BindingList<PurchaseOrderManagerUIModel>();
            gdGrid.DataSource = base.HeadGridDS;
            gdGrid.DataBind();

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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_PurchaseOrder.Code.PO_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_PurchaseOrder.Code.PO_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[HeadDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            HeadDS = base.HeadGridDS.FirstOrDefault(x => x.PO_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_PurchaseOrder.Code.PO_ID].Value);

            if (HeadDS == null || string.IsNullOrEmpty(HeadDS.PO_ID))
            {
                return;
            }

            if (txtPO_ID.Text != HeadDS.PO_ID
                || (txtPO_ID.Text == HeadDS.PO_ID && txtPO_VersionNo.Text != HeadDS.PO_VersionNo?.ToString()))
            {
                if (txtPO_ID.Text == HeadDS.PO_ID && txtPO_VersionNo.Text != HeadDS.PO_VersionNo?.ToString())
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
            //设置详情是否可编辑以及动作按钮是否可用
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();
        }

        /// <summary>
        /// 将HeadDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //订单号
            txtPO_No.Text = HeadDS.PO_No;
            //供应商
            mcbPO_SUPP_Name.SelectedValue = HeadDS.PO_SUPP_ID;
            //来源类型
            cbPO_SourceTypeName.Text = HeadDS.PO_SourceTypeName ?? "";
            //来源类型编码
            cbPO_SourceTypeName.Value = HeadDS.PO_SourceTypeCode ?? "";
            //来源单号
            txtPO_SourceNo.Text = HeadDS.PO_SourceNo;
            //订单总额
            numPO_TotalAmount.Value = HeadDS.PO_TotalAmount;
            //物流费
            numPO_LogisticFee.Value = HeadDS.PO_LogisticFee;
            //单据状态
            cbPO_StatusName.Text = HeadDS.PO_StatusName ?? "";
            //单据状态编码
            cbPO_StatusName.Value = HeadDS.PO_StatusCode ?? "";
            //审核状态
            cbPO_ApprovalStatusName.Text = HeadDS.PO_ApprovalStatusName ?? "";
            //审核状态编码
            cbPO_ApprovalStatusName.Value = HeadDS.PO_ApprovalStatusCode ?? "";
            //到货时间
            dtPO_ReceivedTime.Value = HeadDS.PO_ReceivedTime;
            //有效
            if (HeadDS.PO_IsValid != null)
            {
                ckPO_IsValid.Checked = HeadDS.PO_IsValid.Value;
            }
            //创建人
            txtPO_CreatedBy.Text = HeadDS.PO_CreatedBy;
            //创建时间
            dtPO_CreatedTime.Value = HeadDS.PO_CreatedTime;
            //修改人
            txtPO_UpdatedBy.Text = HeadDS.PO_UpdatedBy;
            //修改时间
            dtPO_UpdatedTime.Value = HeadDS.PO_UpdatedTime;
            //采购订单ID
            txtPO_ID.Text = HeadDS.PO_ID;
            //组织ID
            txtPO_Org_ID.Text = HeadDS.PO_Org_ID;
            //版本号
            txtPO_VersionNo.Value = HeadDS.PO_VersionNo;
        }

        /// <summary>
        /// 查询明细Grid数据并绑定
        /// </summary>
        private void QueryDetail()
        {
            //1.设置查询条件
            var argsCondition = new PurchaseOrderManagerQCModel()
            {
                WHERE_POD_PO_ID = txtPO_ID.Text,
                SqlId = SQLID.PIS_PurchaseOrderManager_SQL_02,
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

            #region 查询明细对应所有库存图片

            var autoPartsBarcodeString = string.Empty;
            foreach (var loopDetail in _detailGridDS)
            {
                if (string.IsNullOrEmpty(loopDetail.POD_AutoPartsBarcode))
                {
                    continue;
                }
                autoPartsBarcodeString += loopDetail.POD_AutoPartsBarcode + SysConst.Semicolon_DBC;
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
        /// 将【详情】Tab内控件的值赋值给基类的HeadDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            HeadDS = new PurchaseOrderManagerUIModel()
            {
                //订单号
                PO_No = txtPO_No.Text.Trim(),
                //供应商ID
                PO_SUPP_ID = mcbPO_SUPP_Name.SelectedValue,
                //供应商
                PO_SUPP_Name = mcbPO_SUPP_Name.SelectedText,
                //来源类型
                PO_SourceTypeName = cbPO_SourceTypeName.Text,
                //来源单号
                PO_SourceNo = txtPO_SourceNo.Text.Trim(),
                //订单总额
                PO_TotalAmount = Convert.ToDecimal(numPO_TotalAmount.Value ?? 0),
                //物流费
                PO_LogisticFee = Convert.ToDecimal(numPO_LogisticFee.Value ?? 0),
                //单据状态
                PO_StatusName = cbPO_StatusName.Text,
                //审核状态
                PO_ApprovalStatusName = cbPO_ApprovalStatusName.Text,
                //到货时间
                PO_ReceivedTime = (DateTime?)dtPO_ReceivedTime.Value ?? DateTime.Now,
                //有效
                PO_IsValid = ckPO_IsValid.Checked,
                //创建人
                PO_CreatedBy = txtPO_CreatedBy.Text.Trim(),
                //创建时间
                PO_CreatedTime = (DateTime?)dtPO_CreatedTime.Value ?? DateTime.Now,
                //修改人
                PO_UpdatedBy = txtPO_UpdatedBy.Text.Trim(),
                //修改时间
                PO_UpdatedTime = (DateTime?)dtPO_UpdatedTime.Value ?? DateTime.Now,
                //采购订单ID
                PO_ID = txtPO_ID.Text.Trim(),
                //组织ID
                PO_Org_ID = txtPO_Org_ID.Text.Trim(),
                //来源类型编码
                PO_SourceTypeCode = cbPO_SourceTypeName.Value?.ToString() ?? "",
                //单据状态编码
                PO_StatusCode = cbPO_StatusName.Value?.ToString() ?? "",
                //审核状态编码
                PO_ApprovalStatusCode = cbPO_ApprovalStatusName.Value?.ToString() ?? "",
                //版本号
                PO_VersionNo = Convert.ToInt64(txtPO_VersionNo.Text.Trim() == "" ? "1" : txtPO_VersionNo.Text.Trim()),
            };
        }

        /// <summary>
        /// 给下拉框赋值
        /// </summary>
        private void SetToComboEditor()
        {
            //来源类型【手工创建，采购预测】
            List<ComComboBoxDataSourceTC> resultPurchaseOrderSourceTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.PurchaseOrderSourceType);
            cbWhere_PO_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_PO_SourceTypeName.ValueMember = SysConst.EN_Code;
            cbWhere_PO_SourceTypeName.DataSource = resultPurchaseOrderSourceTypeList;
            cbWhere_PO_SourceTypeName.DataBind();

            cbPO_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
            cbPO_SourceTypeName.ValueMember = SysConst.EN_Code;
            cbPO_SourceTypeName.DataSource = resultPurchaseOrderSourceTypeList;
            cbPO_SourceTypeName.Text = PurchaseOrderSourceTypeEnum.Name.SGCJ;
            cbPO_SourceTypeName.Value = PurchaseOrderSourceTypeEnum.Code.SGCJ;
            cbPO_SourceTypeName.DataBind();

            //单据状态【已生成，已下单，全部签收，部分签收，已完成】
            List<ComComboBoxDataSourceTC> resultPurchaseOrderStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.PurchaseOrderStatus);
            cbWhere_PO_StatusName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_PO_StatusName.ValueMember = SysConst.EN_Code;
            cbWhere_PO_StatusName.DataSource = resultPurchaseOrderStatusList;
            cbWhere_PO_StatusName.DataBind();

            cbPO_StatusName.DisplayMember = SysConst.EN_TEXT;
            cbPO_StatusName.ValueMember = SysConst.EN_Code;
            cbPO_StatusName.DataSource = resultPurchaseOrderStatusList;
            cbPO_StatusName.Text = PurchaseOrderStatusEnum.Name.YSC;
            cbPO_StatusName.Value = PurchaseOrderStatusEnum.Code.YSC;
            cbPO_StatusName.DataBind();

            //审核状态【待审核，已审核】
            List<ComComboBoxDataSourceTC> resultApprovalStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.ApprovalStatus);
            cbWhere_PO_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_PO_ApprovalStatusName.ValueMember = SysConst.EN_Code;
            cbWhere_PO_ApprovalStatusName.DataSource = resultApprovalStatusList;
            cbWhere_PO_ApprovalStatusName.DataBind();

            cbPO_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
            cbPO_ApprovalStatusName.ValueMember = SysConst.EN_Code;
            cbPO_ApprovalStatusName.DataSource = resultApprovalStatusList;
            cbPO_ApprovalStatusName.Text = ApprovalStatusEnum.Name.DSH;
            cbPO_ApprovalStatusName.Value = ApprovalStatusEnum.Code.DSH;
            cbPO_ApprovalStatusName.DataBind();
        }

        /// <summary>
        /// 设置详情是否可编辑以及动作按钮是否可用
        /// </summary>
        private void SetDetailControl()
        {
            if (cbPO_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                //[审核状态]为[已审核]的场合
                //单头
                numPO_LogisticFee.Enabled = false;
                mcbPO_SUPP_Name.Enabled = false;

                //明细不可添加、删除
                toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedPropsInternal.Enabled = false;
                toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedPropsInternal.Enabled = false;
                //是否可全部到货
                if (cbPO_StatusName.Text == PurchaseOrderStatusEnum.Name.QBQS
                    //|| cbPO_StatusName.Text == PurchaseOrderStatusEnum.Name.YSC
                    || cbPO_StatusName.Text == PurchaseOrderStatusEnum.Name.YWC)
                {
                    toolbarsManagerDetail.Tools[SysConst.AllSign].SharedPropsInternal.Enabled = false;
                }
                else
                {
                    toolbarsManagerDetail.Tools[SysConst.AllSign].SharedPropsInternal.Enabled = true;
                }

                if (cbPO_StatusName.Text == PurchaseOrderStatusEnum.Name.YWC)
                {
                    SetPurchaseOrderDetailStyle(true, true);
                }
                else
                {
                    bool allSign = true;
                    foreach (var loopDetail in _detailGridDS)
                    {
                        if (loopDetail.POD_OrderQty != loopDetail.POD_ReceivedQty)
                        {
                            allSign = false;
                        }
                    }
                    SetPurchaseOrderDetailStyle(true, allSign);
                }
            }
            else
            {
                //新增或[审核状态]为[待审核]的场合
                //单头
                numPO_LogisticFee.Enabled = true;
                //根据[是否存在明细]控制单头是否可编辑
                SetDetailByIsExistDetail();

                //明细可添加、删除，不可全部到货
                toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedPropsInternal.Enabled = true;
                toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedPropsInternal.Enabled = true;
                toolbarsManagerDetail.Tools[SysConst.AllSign].SharedPropsInternal.Enabled = false;

                SetPurchaseOrderDetailStyle(false, true);
            }
        }

        /// <summary>
        /// 设置单元格是否可以编辑，边框和背景色的颜色
        /// </summary>
        /// <param name="paramQtyAndUnitPriceIsReadOnly">设置【订货数量】【订货单价】【仓库】【仓位】【编辑图片】</param>
        /// <param name="paramThisReceivedQtyIsReadOnly">设置【本次签收个数】</param>
        private void SetPurchaseOrderDetailStyle(bool paramQtyAndUnitPriceIsReadOnly, bool paramThisReceivedQtyIsReadOnly)
        {
            #region 设置Grid数据颜色
            gdDetail.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
            foreach (var loopGridRow in gdDetail.Rows)
            {
                if (paramQtyAndUnitPriceIsReadOnly)
                {
                    #region 订货数量

                    //设置单元格只读
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_OrderQty].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_OrderQty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_OrderQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion

                    #region 订货单价

                    //设置单元格只读
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_UnitPrice].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_UnitPrice].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_UnitPrice].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion

                    #region 仓库

                    //设置单元格只读
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name].Activation = Activation.Disabled;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion

                    #region 仓位

                    //设置单元格只读
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name].Activation = Activation.Disabled;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion

                }
                else
                {
                    #region 订货数量

                    //设置单元格可编辑
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_OrderQty].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_OrderQty].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_OrderQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #region 订货单价

                    //设置单元格可编辑
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_UnitPrice].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_UnitPrice].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_UnitPrice].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #region 仓库

                    //设置单元格可编辑
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #region 仓位

                    //设置单元格可编辑
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                }
                if (paramThisReceivedQtyIsReadOnly)
                {
                    #region 本次签收次数

                    //设置单元格只读
                    gdDetail.DisplayLayout.Bands[0].Columns["ThisReceivedQty"].CellActivation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells["ThisReceivedQty"].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells["ThisReceivedQty"].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion

                }
                else
                {
                    #region 本次签收次数

                    gdDetail.DisplayLayout.Bands[0].Columns["ThisReceivedQty"].CellActivation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells["ThisReceivedQty"].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells["ThisReceivedQty"].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

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
            if (cbPO_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                //[审核状态]为[已审核]的场合，[保存]、[删除]、[审核]不可用，[打印]可用
                SetActionEnable(SystemActionEnum.Code.SAVE, false);
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                SetActionEnable(SystemActionEnum.Code.APPROVE, false);
                SetActionEnable(SystemActionEnum.Code.PRINT, true);

                if (cbPO_StatusName.Text == PurchaseOrderStatusEnum.Name.YXD)
                {
                    //[单据状态]为{已下单}的场合，[反审核]可用
                    SetActionEnable(SystemActionEnum.Code.UNAPPROVE, true);
                }
                else
                {
                    //[单据状态]不是{已下单}的场合，[反审核]不可用
                    SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);
                }
                
                if (cbPO_StatusName.Text == PurchaseOrderStatusEnum.Name.YWC)
                {
                    //[单据状态]为{已完成}的场合，[签收]、[核实]不可用
                    SetActionEnable(SystemActionEnum.Code.SIGNIN, false);
                    SetActionEnable(SystemActionEnum.Code.VERIFY, false);

                    //[转结算]、[转出库]可用
                    SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, true);
                    SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSTOCKOUTBILL, true, true);
                }
                else
                {
                    if (cbPO_StatusName.Text == PurchaseOrderStatusEnum.Name.QBQS)
                    {
                        //[单据状态]为{全部签收}的场合，[签收]不可用
                        SetActionEnable(SystemActionEnum.Code.SIGNIN, false);
                    }
                    else
                    {
                        //[单据状态]为{已下单}或{部分签收}的场合，[签收]可用
                        SetActionEnable(SystemActionEnum.Code.SIGNIN, true);
                    }

                    if (cbPO_StatusName.Text == PurchaseOrderStatusEnum.Name.YXD)
                    {
                        //[单据状态]为{已下单}的场合，[核实]不可用
                        SetActionEnable(SystemActionEnum.Code.VERIFY, false);
                    }
                    else
                    {
                        //[单据状态]为{部分签收}或{全部签收}的场合，[核实]可用
                        SetActionEnable(SystemActionEnum.Code.VERIFY, true);
                    }

                    //[转结算]、[转出库]不可用
                    SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, false);
                    SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSTOCKOUTBILL, true, false);
                }
            }
            else
            {
                //新增或[审核状态]为[待审核]的场合，[保存]、[删除]、[审核]可用，[反审核]、[打印]、[核实]不可用
                SetActionEnable(SystemActionEnum.Code.SAVE, true);
                SetActionEnable(SystemActionEnum.Code.DELETE, !string.IsNullOrEmpty(txtPO_ID.Text));
                SetActionEnable(SystemActionEnum.Code.APPROVE, !string.IsNullOrEmpty(txtPO_ID.Text));

                SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);
                SetActionEnable(SystemActionEnum.Code.PRINT, false);
                SetActionEnable(SystemActionEnum.Code.SIGNIN, false);
                SetActionEnable(SystemActionEnum.Code.VERIFY, false);

                //[转结算]、[转出库]不可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, false);
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSTOCKOUTBILL, true, false);
            }
        }

        /// <summary>
        /// 根据[是否存在明细]控制单头是否可编辑
        /// </summary>
        private void SetDetailByIsExistDetail()
        {
            if (_detailGridDS.Count == 0)
            {
                mcbPO_SUPP_Name.Enabled = true;
            }
            else
            {
                mcbPO_SUPP_Name.Enabled = false;
            }
        }

        #region 检查

        /// <summary>
        /// 前端检查-保存
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForSave()
        {
            if (cbPO_SourceTypeName.Text.Trim() != PurchaseOrderSourceTypeEnum.Name.CGYC)
            {
                //验证供应商是否被选择
                if (string.IsNullOrEmpty(mcbPO_SUPP_Name.SelectedText)
                    || string.IsNullOrEmpty(mcbPO_SUPP_Name.SelectedValue))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.SUPPLIER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            //验证来源类型
            if (string.IsNullOrEmpty(cbPO_SourceTypeName.Text))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.SOURCE_TYPE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //验证明细是否有数据
            if (_detailGridDS.Count == 0)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0014, new object[] { SystemTableEnums.Name.PIS_PurchaseOrderDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            int i = 0;
            foreach (var loopDetail in _detailGridDS)
            {
                i++;
                if (!BLLCom.IsDecimal(Convert.ToString(loopDetail.POD_UnitPrice ?? 0)))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i, SystemTableColumnEnums.PIS_PurchaseOrderDetail.Name.POD_UnitPrice, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (!BLLCom.IsDecimal(Convert.ToString(loopDetail.POD_OrderQty ?? 0)))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i, SystemTableColumnEnums.PIS_PurchaseOrderDetail.Name.POD_OrderQty, MsgParam.POSITIVE_INTEGER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 前端检查-删除
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForDelete()
        {
            if (string.IsNullOrEmpty(txtPO_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0002), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 前段检查—审核
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForApprove()
        {
            #region 前段检查—不涉及数据库
            //未保存,不能审核
            if (string.IsNullOrEmpty(txtPO_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_PurchaseOrder + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            MDLPIS_PurchaseOrder resulPurchaseOrder = new MDLPIS_PurchaseOrder();
            _bll.QueryForObject<MDLPIS_PurchaseOrder, MDLPIS_PurchaseOrder>(new MDLPIS_PurchaseOrder()
            {
                WHERE_PO_IsValid = true,
                WHERE_PO_ID = txtPO_ID.Text.Trim()
            }, resulPurchaseOrder);
            //采购单单不存在,不能审核
            if (string.IsNullOrEmpty(resulPurchaseOrder.PO_ID))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_PurchaseOrder + MsgParam.NOTEXIST, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //采购已审核,不能审核
            if (resulPurchaseOrder.PO_ApprovalStatusCode == ApprovalStatusEnum.Code.YSH)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_PurchaseOrder + ApprovalStatusEnum.Name.YSH, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //采购明细列表有变动
            //TODO 待调整：取消此验证，并且审核方法中调整
            if ((_detailGridDS.InsertList != null && _detailGridDS.InsertList.Count > 0)
                || (_detailGridDS.UpdateList != null && _detailGridDS.UpdateList.Count > 0)
                || (_detailGridDS.DeleteList != null && _detailGridDS.DeleteList.Count > 0))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0023, new object[]
                {
                    SystemActionEnum.Name.SAVE, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //确认审核操作
            DialogResult isConfirmApprove = MessageBoxs.Show(Trans.PIS, this.ToString(),
                MsgHelp.GetMsg(MsgCode.W_0014),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isConfirmApprove != DialogResult.OK)
            {
                return false;
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 前段检查—反审核
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForUnApprove()
        {
            #region 前端审核—不涉及数据库
            if (cbPO_ApprovalStatusName.Text == ApprovalStatusEnum.Name.DSH)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_PurchaseOrder + ApprovalStatusEnum.Name.DSH, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (cbPO_StatusName.Text != PurchaseOrderStatusEnum.Name.YXD)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_PurchaseOrder + cbPO_StatusName.Text, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            #endregion

            //确认审核操作
            DialogResult isConfirmApprove = MessageBoxs.Show(Trans.PIS, this.ToString(),
                MsgHelp.GetMsg(MsgCode.W_0018),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isConfirmApprove != DialogResult.OK)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 前段检查—签收
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForSignIn()
        {
            #region 前段检查-涉及数据库
            //查询采购单是否已完成
            MDLPIS_PurchaseOrder purchaseOrder = new MDLPIS_PurchaseOrder()
            {
                WHERE_PO_ID = txtPO_ID.Text.Trim(),
                WHERE_PO_VersionNo = Convert.ToInt64(txtPO_VersionNo.Text.Trim()),
            };
            _bll.QueryForObject<MDLPIS_PurchaseOrder, MDLPIS_PurchaseOrder>(purchaseOrder, purchaseOrder);
            if (purchaseOrder.PO_StatusName == PurchaseOrderStatusEnum.Name.YWC)
            {
                //采购订单的单据状态已完成，不能签收
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                     {SystemTableEnums.Name.PIS_PurchaseOrder+MsgParam.OF+ MsgParam.BILL_STATUS +PurchaseOrderStatusEnum.Name.YWC,SystemActionEnum.Name.SIGNIN}),
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            #endregion

            #region 前段检查——不涉及数据库
            //验证下拉框状态
            if (cbPO_StatusName.Text == PurchaseOrderStatusEnum.Name.YSC)
            {
                //采购订单单据状态为XXX不能签收
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_PurchaseOrder+MsgParam.OF+MsgParam.BILL_STATUS +MsgParam.BE+cbPO_StatusName.Text,SystemActionEnum.Name.SIGNIN
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            #region 验证本次签收，签收的数量
            bool isSign = false;
            foreach (var loopDetail in _detailGridDS)
            {
                if (loopDetail.ThisReceivedQty > 0)
                {
                    isSign = true;
                    break;
                }
            }
            if (!isSign)
            {
                //本次签收数量不能为空
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { "本次签收数量" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            #endregion


            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0032), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            #endregion

            return true;
        }

        /// <summary>
        /// 前端检查-核实
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForVerify()
        {
            #region 前段检查-不涉及数据库
            if (cbPO_StatusName.Text == PurchaseOrderStatusEnum.Name.YSC || cbPO_StatusName.Text == PurchaseOrderStatusEnum.Name.YWC)
            {
                //采购订单单据状态为XXX不能核实
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_PurchaseOrder+MsgParam.OF+MsgParam.BILL_STATUS +MsgParam.BE+cbPO_StatusName.Text,SystemActionEnum.Name.VERIFY
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (cbPO_StatusName.Text != PurchaseOrderStatusEnum.Name.QBQS)
            {
                //订货数量大于签收，是否确认核实？\r\n单击【确定】核实单据，【取消】返回。
                DialogResult dialogResult = MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0037, new object[]
                {
                    SystemTableColumnEnums.PIS_PurchaseOrderDetail.Name.POD_OrderQty,SystemTableColumnEnums.PIS_PurchaseOrderDetail.Name.POD_ReceivedQty,
                    SystemActionEnum.Name.VERIFY
                }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return false;
                }

            }
            #endregion

            #region 前段检查-涉及数据库
            MDLPIS_PurchaseOrder purchaseOrder = new MDLPIS_PurchaseOrder();
            _bll.QueryForObject<MDLPIS_PurchaseOrder, MDLPIS_PurchaseOrder>(new MDLPIS_PurchaseOrder()
            {
                WHERE_PO_ID = txtPO_ID.Text,
            }, purchaseOrder);
            if (cbPO_StatusName.Text != purchaseOrder.PO_StatusName)
            {
                //核实失败，失败原因：采购订单未XXX
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0018, new object[]
                {
                    SystemActionEnum.Name.VERIFY,SystemTableEnums.Name.PIS_PurchaseOrder+MsgParam.NOTYET +cbPO_StatusName.Text
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            #endregion
            return true;
        }

        #endregion

        #region 采购明细相关方法

        /// <summary>
        /// 添加采购单明细数据
        /// </summary>
        private void AddPurchaseOrderDetail()
        {

            if (cbPO_SourceTypeName.Text.Trim() != PurchaseOrderSourceTypeEnum.Name.CGYC)
            {
                if (string.IsNullOrEmpty(mcbPO_SUPP_Name.SelectedText)
                    || string.IsNullOrEmpty(mcbPO_SUPP_Name.SelectedValue))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.PIS_PurchaseOrder.Name.PO_SUPP_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            #region 显示维护配件明细窗体

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                {ComViewParamKey.SpecialSupplierID.ToString(),mcbPO_SUPP_Name.SelectedValue},
                {ComViewParamKey.SpecialSupplierName.ToString(),mcbPO_SUPP_Name.SelectedText},
                {ComViewParamKey.MaintainAutoPartsSourForm.ToString(), ComViewParamValue.MaintainAutoPartsSourForm.PIS_FrmPurchaseOrderManager},
                {ComViewParamKey.MaintainAutoPartsAction.ToString(), ComViewParamValue.MaintainAutoPartsAction.AddAutoParts},
                {ComViewParamKey.MaintainAutoPartsSourFormFunc.ToString(),_maintainAutoPartsDetailFunc},

            };
            FrmMaintainAutoPartsDetail frmMaintainAutoPartsDetail = new FrmMaintainAutoPartsDetail(paramViewParameters)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            DialogResult dialogResult = frmMaintainAutoPartsDetail.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                //根据[是否存在明细]控制单头是否可编辑
                SetDetailByIsExistDetail();
                SetPurchaseOrderDetailStyle(false, true);
                return;
            }

            numPO_TotalAmount.Text = Convert.ToString(0);

            //根据[是否存在明细]控制单头是否可编辑
            SetDetailByIsExistDetail();

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            SetPurchaseOrderDetailStyle(false, true);
            //设置采购明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
            #endregion

            if (gdDetail.Rows.Count > 0)
            {
                //计算总金额
                Decimal totalAmount = 0;
                for (int i = 0; i < gdDetail.Rows.Count; i++)
                {
                    #region 单据状态
                    string newId = Guid.NewGuid().ToString();
                    gdDetail.DisplayLayout.Rows[i].Cells["Tmp_POD_ID"].Value = newId;
                    gdDetail.DisplayLayout.Rows[i].Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_StatusName].Value = PurchaseOrderDetailStatusEnum.Name.YSC;
                    //单据状态【已生成，已下单，已签收，已完成】
                    List<ComComboBoxDataSourceTC> resultList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.PurchaseOrderDetailStatus);
                    gdDetail.DisplayLayout.ValueLists.Add("StatusList" + newId);
                    for (int j = 0; j < resultList.Count; j++)
                    {
                        gdDetail.DisplayLayout.ValueLists["StatusList" + newId].ValueListItems.Add(resultList[j].Code, resultList[j].Text);
                    }
                    gdDetail.DisplayLayout.ValueLists["StatusList" + newId].DisplayStyle = ValueListDisplayStyle.DisplayText;
                    gdDetail.DisplayLayout.Rows[i].Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_StatusName].ValueList = gdDetail.DisplayLayout.ValueLists["StatusList" + newId];
                    gdDetail.DisplayLayout.Rows[i].Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_StatusName].Value = gdDetail.DisplayLayout.ValueLists["StatusList" + newId].ValueListItems[0].DisplayText;
                    gdDetail.DisplayLayout.Rows[i].Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_StatusCode].Value = gdDetail.DisplayLayout.ValueLists["StatusList" + newId].ValueListItems[0].DataValue;

                    #endregion

                    #region 计算总价

                    if (string.IsNullOrEmpty(gdDetail.Rows[i].Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_ReceivedQty].Text))
                    {
                        continue;
                    }
                    totalAmount = totalAmount + Math.Round(
                       Convert.ToDecimal(
                           gdDetail.Rows[i].Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_UnitPrice]
                               .Text) *
                       Convert.ToDecimal(
                           gdDetail.Rows[i].Cells[SystemTableColumnEnums.PIS_PurchaseOrderDetail.Code.POD_OrderQty]
                               .Text), 2);
                    numPO_TotalAmount.Text = Convert.ToString(totalAmount);
                    #endregion
                }
            }
        }

        /// <summary>
        /// 删除采购单明细数据
        /// </summary>
        private void DeletePurchaseOrderDetail()
        {
            //待删除的采购明细列表
            gdDetail.UpdateData();
            bool isRepeat = false;
            var deletePurchaseOrderDetailList = _detailGridDS.Where(p => p.IsChecked == true).ToList();
            StringBuilder partsName = new StringBuilder();

            if (deletePurchaseOrderDetailList.Count == 0)
            {
                //选择要删除的仓位
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { SystemTableEnums.Name.PIS_PurchaseOrderDetail, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #region 检查删除的数据是否来自于采购补货
            foreach (var loopDeleteDerail in deletePurchaseOrderDetailList)
            {
                foreach (var loopPurchaseOrder in PurchaseOrderDetailList)
                {
                    if (loopDeleteDerail.POD_AutoPartsName == loopPurchaseOrder.POD_AutoPartsName)
                    {
                        partsName.Append("“" + loopDeleteDerail.POD_AutoPartsName + "”");
                        isRepeat = true;
                        continue;
                    }

                }
            }
            if (isRepeat)
            {
                DialogResult isDeleteLogisticsBill = MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0029, new object[] { partsName }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (isDeleteLogisticsBill != DialogResult.OK)
                {
                    return;
                }
            }
            #endregion

            foreach (var loopDeleteDerail in deletePurchaseOrderDetailList)
            {
                _detailGridDS.Remove(loopDeleteDerail);
            }

            //根据[是否存在明细]控制单头是否可编辑
            SetDetailByIsExistDetail();

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
        }

        /// <summary>
        /// 维护采购明细Func
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
                #region 新增采购明细的场合

                //验证条码
                if (string.IsNullOrEmpty(paramAutoPartsDetail.APA_Barcode))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0016, new object[] { MsgParam.BARCODE, MsgParam.ADD }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                //条码相同，为同一个配件
                PurchaseOrderManagerDetailUIModel samePurchaseOrderDetail = _detailGridDS.FirstOrDefault(tempPurchaseOrderManagerDetail =>
                tempPurchaseOrderManagerDetail.POD_AutoPartsBarcode == paramAutoPartsDetail.APA_Barcode);

                //明细列表中不存在相同配件
                if (samePurchaseOrderDetail == null)
                {
                    PurchaseOrderManagerDetailUIModel newPurchaseOrderManagerDetail = new PurchaseOrderManagerDetailUIModel
                    {
                        Tmp_POD_ID = Guid.NewGuid().ToString(),
                        POD_ID = null,
                        POD_PO_No = HeadDS.PO_No
                    };
                    _bll.CopyModel(paramAutoPartsDetail, newPurchaseOrderManagerDetail);

                    #region 为采购明细赋值
                    //newPurchaseOrderManagerDetail.DetailID = newPurchaseOrderManagerDetail.Tmp_POD_ID;
                    newPurchaseOrderManagerDetail.POD_AutoPartsName = paramAutoPartsDetail.APA_Name;
                    newPurchaseOrderManagerDetail.POD_AutoPartsBarcode = paramAutoPartsDetail.APA_Barcode;
                    newPurchaseOrderManagerDetail.POD_AutoPartsBrand = paramAutoPartsDetail.APA_Brand;
                    newPurchaseOrderManagerDetail.POD_OrderQty = paramAutoPartsDetail.PurchaseQuantity;
                    newPurchaseOrderManagerDetail.POD_UnitPrice = paramAutoPartsDetail.PurchaseUnitPrice;
                    newPurchaseOrderManagerDetail.POD_WH_ID = paramAutoPartsDetail.APA_WH_ID;
                    newPurchaseOrderManagerDetail.POD_WHB_ID = paramAutoPartsDetail.APA_WHB_ID;
                    newPurchaseOrderManagerDetail.POD_StatusName = PurchaseOrderDetailStatusEnum.Name.YSC;
                    newPurchaseOrderManagerDetail.POD_StatusCode = PurchaseOrderDetailStatusEnum.Code.YSC;
                    newPurchaseOrderManagerDetail.POD_ThirdCode = paramAutoPartsDetail.APA_ThirdNo;
                    newPurchaseOrderManagerDetail.POD_OEMCode = paramAutoPartsDetail.APA_OEMNo;
                    newPurchaseOrderManagerDetail.POD_AutoPartsSpec = paramAutoPartsDetail.APA_Specification;
                    newPurchaseOrderManagerDetail.POD_AutoPartsLevel = paramAutoPartsDetail.APA_Level;
                    newPurchaseOrderManagerDetail.POD_VehicleBrand = paramAutoPartsDetail.APA_VehicleBrand;
                    newPurchaseOrderManagerDetail.POD_VehicleInspire = paramAutoPartsDetail.APA_VehicleInspire;
                    newPurchaseOrderManagerDetail.POD_VehicleCapacity = paramAutoPartsDetail.APA_VehicleCapacity;
                    newPurchaseOrderManagerDetail.POD_VehicleYearModel = paramAutoPartsDetail.APA_VehicleYearModel;
                    newPurchaseOrderManagerDetail.POD_VehicleGearboxType = paramAutoPartsDetail.APA_VehicleGearboxTypeName;
                    newPurchaseOrderManagerDetail.POD_UOM = paramAutoPartsDetail.APA_UOM;
                    newPurchaseOrderManagerDetail.POD_IsValid = true;
                    newPurchaseOrderManagerDetail.POD_CreatedBy = LoginInfoDAX.UserName;
                    newPurchaseOrderManagerDetail.POD_CreatedTime = curDateTime;
                    newPurchaseOrderManagerDetail.POD_UpdatedBy = LoginInfoDAX.UserName;
                    newPurchaseOrderManagerDetail.POD_UpdatedTime = curDateTime;
                    #endregion

                    _detailGridDS.Add(newPurchaseOrderManagerDetail);
                }
                //明细列表中已存在相同配件
                else
                {
                    samePurchaseOrderDetail.POD_OrderQty = (samePurchaseOrderDetail.POD_OrderQty ?? 0) + paramAutoPartsDetail.PurchaseQuantity;
                    samePurchaseOrderDetail.POD_UnitPrice = paramAutoPartsDetail.PurchaseUnitPrice;
                }
                if (_detailGridDS.Count > 0)
                {
                    mcbPO_SUPP_Name.Enabled = false;
                }
                else
                {
                    mcbPO_SUPP_Name.Enabled = true;
                }
                #endregion
            }
            else
            {
                #region 更新采购明细的场合

                foreach (var loopPurchaseOrderManagerDetail in _detailGridDS)
                {
                    if (loopPurchaseOrderManagerDetail.POD_ID == paramAutoPartsDetail.Detail_ID ||
                        loopPurchaseOrderManagerDetail.Tmp_POD_ID == paramAutoPartsDetail.Detail_ID)
                    {
                        _bll.CopyModel(paramAutoPartsDetail, loopPurchaseOrderManagerDetail);

                        #region 为采购明细赋值
                        loopPurchaseOrderManagerDetail.POD_AutoPartsBarcode = paramAutoPartsDetail.APA_Barcode;
                        loopPurchaseOrderManagerDetail.POD_AutoPartsName = paramAutoPartsDetail.APA_Name;
                        loopPurchaseOrderManagerDetail.POD_OEMCode = paramAutoPartsDetail.APA_OEMNo;
                        loopPurchaseOrderManagerDetail.POD_ThirdCode = paramAutoPartsDetail.APA_ThirdNo;
                        loopPurchaseOrderManagerDetail.POD_AutoPartsSpec = paramAutoPartsDetail.APA_Specification;
                        loopPurchaseOrderManagerDetail.POD_UOM = paramAutoPartsDetail.APA_UOM;
                        loopPurchaseOrderManagerDetail.POD_WH_ID = paramAutoPartsDetail.APA_WH_ID;
                        loopPurchaseOrderManagerDetail.POD_WHB_ID = paramAutoPartsDetail.APA_WHB_ID;
                        loopPurchaseOrderManagerDetail.POD_OrderQty = paramAutoPartsDetail.PurchaseQuantity;
                        loopPurchaseOrderManagerDetail.POD_UnitPrice = paramAutoPartsDetail.PurchaseUnitPrice;

                        loopPurchaseOrderManagerDetail.POD_IsValid = true;
                        loopPurchaseOrderManagerDetail.POD_CreatedBy = LoginInfoDAX.UserName;
                        loopPurchaseOrderManagerDetail.POD_CreatedTime = curDateTime;
                        loopPurchaseOrderManagerDetail.POD_UpdatedBy = LoginInfoDAX.UserName;
                        loopPurchaseOrderManagerDetail.POD_UpdatedTime = curDateTime;
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
            //设置采购明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;

            return true;
        }

        /// <summary>
        /// 全部到货
        /// </summary>
        public void AllSign()
        {
            gdDetail.Refresh();
            decimal totalAmount = 0;
            foreach (var loopDetail in _detailGridDS)
            {
                loopDetail.ThisReceivedQty = Convert.ToDecimal(loopDetail.POD_OrderQty ?? 0) - Convert.ToDecimal(loopDetail.POD_ReceivedQty ?? 0);
                totalAmount = totalAmount + Convert.ToDecimal(loopDetail.POD_OrderQty ?? 0) *
                               Convert.ToDecimal(loopDetail.POD_UnitPrice ?? 0);
            }
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            gdDetail.Refresh();
            cbPO_StatusName.Text = PurchaseOrderStatusEnum.Name.QBQS;
            numPO_TotalAmount.Text = Math.Round(totalAmount, 2).ToString();
        }

        #endregion

        #region 配件图片相关方法

        private string _latestBarcode = string.Empty;
        /// <summary>
        /// 加载配件图片
        /// </summary>
        /// <param name="paramDetail">采购明细Detail</param>
        private void LoadAutoPartsPicture(PurchaseOrderManagerDetailUIModel paramDetail)
        {
            if (paramDetail == null
                || string.IsNullOrEmpty(paramDetail.POD_AutoPartsBarcode))
            {
                //请选择采购明细
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { SystemTableEnums.Name.PIS_PurchaseOrderDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (paramDetail.POD_AutoPartsBarcode == _latestBarcode)
            {
                return;
            }
            _latestBarcode = paramDetail.POD_AutoPartsBarcode;

            flowLayoutPanelPicture.Controls.Clear();
            _pictureExpandList.Clear();

            //当前明细对应的库存图片列表
            List<AutoPartsPictureUIModel> curDetailAutoPartsPictureList = new List<AutoPartsPictureUIModel>();
            curDetailAutoPartsPictureList = _autoPartsPictureList.Where(x => x.INVP_Barcode == paramDetail.POD_AutoPartsBarcode).ToList();

            if (curDetailAutoPartsPictureList.Count == 0)
            {
                //配件无库存图片时，加载一个扩展的图片控件
                //添加空图片控件
                AutoPartsPictureUIModel nullAutoPartsPicture = new AutoPartsPictureUIModel()
                {
                    INVP_Barcode = paramDetail.POD_AutoPartsBarcode,
                };
                AddNullPictureControl(nullAutoPartsPicture);
            }
            else
            {
                //配件有库存图片时，加载实际数量的扩展的图片控件以及图片
                foreach (var loopPicture in curDetailAutoPartsPictureList)
                {
                    if (string.IsNullOrEmpty(loopPicture.INVP_ID)
                        || string.IsNullOrEmpty(loopPicture.INVP_PictureName))
                    {
                        continue;
                    }

                    loopPicture.INVP_Barcode = paramDetail.POD_AutoPartsBarcode;
                    Dictionary<string, AutoPartsPictureUIModel> pictureDictionary = new Dictionary<string, AutoPartsPictureUIModel>();
                    if (!pictureDictionary.ContainsKey(loopPicture.INVP_PictureName))
                    {
                        pictureDictionary.Add(loopPicture.INVP_PictureName, loopPicture);
                    }
                    ThreadPool.QueueUserWorkItem(new WaitCallback(LoadImage), pictureDictionary);
                }
                AutoPartsPictureUIModel nullAutoPartsPicture = new AutoPartsPictureUIModel()
                {
                    INVP_Barcode = paramDetail.POD_AutoPartsBarcode,
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
        /// <param name="paramAutoPartsPicture">库存图片</param>
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
                    var curDetail = _detailGridDS.FirstOrDefault(x => x.POD_AutoPartsBarcode == curAutoPartsPicture.INVP_Barcode);
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
        /// <param name="paramAutoPartsPicture">库存图片</param>
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
            //待删除的库存图片
            AutoPartsPictureUIModel argsAutoPartsPicture = paramAutoPartsPicture as AutoPartsPictureUIModel;

            if (argsAutoPartsPicture == null
                || string.IsNullOrEmpty(argsAutoPartsPicture.INVP_PictureName))
            {
                //未保存的场合，仅清除显示图片
                //删除当前图片控件
                var deletePicture = _pictureExpandList.FirstOrDefault(x => x.PictureKey == paramPictureKey);
                if (deletePicture != null)
                {
                    flowLayoutPanelPicture.Controls.Remove(deletePicture);
                    _pictureExpandList.Remove(deletePicture);
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
        /// <param name="paramAutoPartsBarcode">当前选中明细ID</param>
        private void SelectAllPicture(string paramAutoPartsBarcode)
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
                if (curAutoPartsPicture == null || curAutoPartsPicture.INVP_Barcode != paramAutoPartsBarcode)
                {
                    continue;
                }
                loopPicture.IsChecked = ckSelectAllPicture.Checked;
            }
        }

        /// <summary>
        /// 批量上传图片
        /// </summary>
        /// <param name="paramDetail">采购明细</param>
        private void BatchUploadPicture(PurchaseOrderManagerDetailUIModel paramDetail)
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
                        INVP_Barcode = paramDetail.POD_AutoPartsBarcode,
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
                        //第一次上传的场合，新增待保存的库存图片
                        AutoPartsPictureUIModel newAutoPartsPicture = new AutoPartsPictureUIModel
                        {
                            INVP_Barcode = paramDetail.POD_AutoPartsBarcode,
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
                    INVP_Barcode = paramDetail.POD_AutoPartsBarcode,
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
        /// <param name="paramDetail">采购明细</param>
        private void BatchExportPicture(PurchaseOrderManagerDetailUIModel paramDetail)
        {
            #region 验证

            //勾选的图片列表
            var checkedPictureExpandList = _pictureExpandList.Where(x => x.IsChecked == true && x.PictureImage != null).ToList();
            List<AutoPartsPictureUIModel> curDetailAutoPartsPictureList = new List<AutoPartsPictureUIModel>();
            foreach (var loopCheckedPictureExpand in checkedPictureExpandList)
            {
                var curAutoPartsPicture = loopCheckedPictureExpand.PropertyModel as AutoPartsPictureUIModel;
                if (curAutoPartsPicture == null || curAutoPartsPicture.INVP_Barcode != paramDetail.POD_AutoPartsBarcode)
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
        /// <param name="paramDetail">采购明细</param>
        private void BatchDeletePicture(PurchaseOrderManagerDetailUIModel paramDetail)
        {
            #region 验证

            //勾选的图片列表
            var checkedPictureExpandList = _pictureExpandList.Where(x => x.IsChecked == true && x.PictureImage != null).ToList();
            List<AutoPartsPictureUIModel> curDetailAutoPartsPictureList = new List<AutoPartsPictureUIModel>();
            foreach (var loopCheckedPictureExpand in checkedPictureExpandList)
            {
                var curAutoPartsPicture = loopCheckedPictureExpand.PropertyModel as AutoPartsPictureUIModel;
                if (curAutoPartsPicture == null || curAutoPartsPicture.INVP_Barcode != paramDetail.POD_AutoPartsBarcode)
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

            //已选checkedPictureList.Count 条数据，确定删除？\r\n单击【确定】删除，【取消】返回。
            DialogResult dialogResult = MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.W_0013, new object[] { curDetailAutoPartsPictureList.Count }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            #endregion

            //待删除的库存图片列表
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
            //根据图片名称查询待删除的库存图片
            _bll.QueryForList(SQLID.COMM_SQL49, new MDLPIS_InventoryPicture()
            {
                WHERE_INVP_Barcode = paramDetail.POD_AutoPartsBarcode + SysConst.Semicolon_DBC,
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

            #region 删除数据库中库存图片

            if (deleteAutoPartsPictureList.Count > 0)
            {
                var outMsg = string.Empty;
                bool deleteDataResult = AutoPartsComFunction.DeleteAutoPartsPicture(deleteAutoPartsPictureList, ref outMsg);
                if (!deleteDataResult)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.BS, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// <param name="paramDetail">采购明细</param>
        private void SetPictureControl(PurchaseOrderManagerDetailUIModel paramDetail = null)
        {
            if (paramDetail == null
                || string.IsNullOrEmpty(paramDetail.POD_AutoPartsBarcode)
                || _isCanEditPicture == false)
            {
                flowLayoutPanelPicture.Controls.Clear();
                _pictureExpandList.Clear();

                //采购明细为空的场合 或者 无保存配件档案权限的场合，不可上传、导出、删除图片
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
                //采购明细不为空的场合，可上传、导出、删除图片

                var curDetailAutoPartsPictureList = _autoPartsPictureList.Where(x => x.INVP_Barcode == paramDetail.POD_AutoPartsBarcode).ToList();

                ckSelectAllPicture.Enabled = true;
                toolbarsAutoPartsPicture.Tools[SysConst.Upload].SharedProps.Enabled = true;
                toolbarsAutoPartsPicture.Tools[SysConst.Export].SharedProps.Enabled = true;
                toolbarsAutoPartsPicture.Tools[SysConst.EN_DEL].SharedProps.Enabled = true;

                if (toolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[SystemActionEnum.Code.SAVE].SharedPropsInternal.Enabled
                    || toolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[SystemActionEnum.Code.APPROVE].SharedPropsInternal.Enabled
                    || toolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[SystemActionEnum.Code.SIGNIN].SharedPropsInternal.Enabled)
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
                    //库存图片可保存、审核、签收的场合，可上传图片
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
                    //库存图片不可保存的场合，不可上传、删除图片
                    toolbarsAutoPartsPicture.Tools[SysConst.Upload].SharedProps.Enabled = false;
                    toolbarsAutoPartsPicture.Tools[SysConst.EN_DEL].SharedProps.Enabled = false;
                    foreach (var loopPicture in _pictureExpandList)
                    {
                        loopPicture.IsCheckedIsVisible = true;
                        loopPicture.UploadIsVisible = false;
                        loopPicture.ExportIsVisible = true;
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
        /// <param name="paramAutoPartsPicture">采购明细</param>
        private void AddNullPictureControl(AutoPartsPictureUIModel paramAutoPartsPicture)
        {
            SkyCarPictureExpand addAutoPartsPicture = new SkyCarPictureExpand
            {
                //图片名称作为操作图片的唯一标识
                PictureKey = Guid.NewGuid() + ".jpg",
                //待保存的库存图片TBModel
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
                    var curHead = HeadGridDS.FirstOrDefault(x => x.PO_ID == HeadDS.PO_ID);
                    if (curHead != null)
                    {
                        HeadGridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = HeadGridDS.FirstOrDefault(x => x.PO_ID == HeadDS.PO_ID);
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
