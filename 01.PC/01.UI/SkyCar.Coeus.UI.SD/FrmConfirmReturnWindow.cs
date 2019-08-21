using SkyCar.Coeus.Common.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.SD.APModel;
using SkyCar.Framework.WindowUI;

namespace SkyCar.Coeus.UI.SD
{
    /// <summary>
    /// 确认退货弹出窗
    /// </summary>
    public partial class FrmConfirmReturnWindow : BaseForm
    {
        #region 全局变量
        /// <summary>
        /// 确认退货BLL
        /// </summary>
        private BLLBase _bll = new BLLBase(Trans.COM);

        /// <summary>
        /// 页面参数字典
        /// </summary>
        private Dictionary<string, object> _viewParameters = new Dictionary<string, object>();

        /// <summary>
        /// 确认退货的来源类型
        /// </summary>
        private SDViewParamValue.ConfirmReturnSourType _confirmReturnSourType;

        /// <summary>
        /// 销售退货明细列表
        /// </summary>
        List<SalesReturnDetailUIModel> SalesOrderReturnDetailList = new List<SalesReturnDetailUIModel>();

        /// <summary>
        /// 退货入库明细列表
        /// </summary>
        List<ReturnStockInDetailUIModel> ReturnStockInDetailList = new List<ReturnStockInDetailUIModel>();

        #endregion

        #region 公共属性


        #endregion

        #region 系统事件

        /// <summary>
        /// 确认退货
        /// </summary>
        public FrmConfirmReturnWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 确认退货
        /// </summary>
        /// <param name="paramWindowParameters"></param>
        public FrmConfirmReturnWindow(Dictionary<string, object> paramWindowParameters)
        {
            _viewParameters = paramWindowParameters;

            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmConfirmReturnWindow_Load(object sender, EventArgs e)
        {
            #region 处理参数

            if (_viewParameters == null)
            {
                return;
            }

            #region 来源方式（核实销售订单/审核退货的销售订单）

            if (_viewParameters.ContainsKey(SDViewParamKey.ConfirmReturnSourType.ToString()))
            {
                _confirmReturnSourType = (SDViewParamValue.ConfirmReturnSourType)_viewParameters[SDViewParamKey.ConfirmReturnSourType.ToString()];

                if (_confirmReturnSourType == SDViewParamValue.ConfirmReturnSourType.VerifySalesOrder)
                {
                    //来源方式为{核实销售订单}的场合，[退货明细]列表中显示[销售数量]、[签收数量]、[拒收数量]、[丢失数量]
                    gdSalesOrderReturnDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Hidden = false;
                    gdSalesOrderReturnDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Hidden = false;
                    gdSalesOrderReturnDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Hidden = false;
                    gdSalesOrderReturnDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Hidden = false;
                    gdSalesOrderReturnDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TotalAmount].Header.Caption = "销售总金额";
                }
                else if (_confirmReturnSourType == SDViewParamValue.ConfirmReturnSourType.ApproveReturnSalesOrder)
                {
                    //来源方式为{核实销售订单}的场合，[退货明细]列表中仅显示[销售数量]，并且Caption为{退货数量}
                    gdSalesOrderReturnDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Hidden = true;
                    gdSalesOrderReturnDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Hidden = true;
                    gdSalesOrderReturnDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Hidden = true;

                    gdSalesOrderReturnDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Header.Caption = "退货数量";
                    gdSalesOrderReturnDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TotalAmount].Header.Caption = "退货总金额";
                }
            }
            #endregion

            #region 销售退货明细

            if (_viewParameters.ContainsKey(SDViewParamKey.SalesOrderReturnDetail.ToString()))
            {
                var tempList = _viewParameters[SDViewParamKey.SalesOrderReturnDetail.ToString()] as List<SalesReturnDetailUIModel>;
                if (tempList != null)
                {
                    SalesOrderReturnDetailList = tempList;

                    gdSalesOrderReturnDetail.DataSource = SalesOrderReturnDetailList;
                    gdSalesOrderReturnDetail.DataBind();
                    SetSalesOrderReturnDetailStyle();
                }
            }
            #endregion

            #region 退货入库明细

            if (_viewParameters.ContainsKey(SDViewParamKey.ReturnStockInDetail.ToString()))
            {
                var tempList = _viewParameters[SDViewParamKey.ReturnStockInDetail.ToString()] as List<ReturnStockInDetailUIModel>;
                if (tempList != null)
                {
                    ReturnStockInDetailList = tempList;

                    gdReturnStockInDetail.DataSource = ReturnStockInDetailList;
                    gdReturnStockInDetail.DataBind();
                    SetReturnStockInDetailStyle();
                }
            }
            #endregion

            #endregion
        }

        #region 退货入库明细相关事件

        /// <summary>
        /// gdReturnStockInDetail单元格的值变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdReturnStockInDetail_CellChange(object sender, CellEventArgs e)
        {
            var cellIndex = gdReturnStockInDetail.Rows[e.Cell.Row.Index];

            #region Cell为[入库数量]

            if (e.Cell.Column.Key == SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Qty)
            {
                //验证入库数量
                if (e.Cell.Column.Key == SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Qty)
                {
                    gdReturnStockInDetail.UpdateData();
                    if (cellIndex.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Value != null)
                    {
                        if (Convert.ToDecimal(e.Cell.Value??0) > Convert.ToDecimal(cellIndex.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Value??0))
                        {
                            //最大入库数量为该条码配件最大退货数量
                            e.Cell.Value = cellIndex.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Value;
                        }
                    }
                }

                if (BLLCom.IsDecimal(cellIndex.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_UnitCostPrice].Text) &&
                    BLLCom.IsDecimal(cellIndex.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Qty].Text))
                {
                    //计算入库金额
                    cellIndex.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Amount].Value = Math.Round(Convert.ToDecimal(cellIndex.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_UnitCostPrice].Text) * Convert.ToDecimal(cellIndex.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Qty].Text), 2);
                }
            }
            #endregion

            gdReturnStockInDetail.UpdateData();
            //设置入库单明细Grid自适应列宽（根据单元格内容）
            gdReturnStockInDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }
        /// <summary>
        /// gdReturnStockInDetail单击单元格事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdReturnStockInDetail_ClickCellButton(object sender, CellEventArgs e)
        {
            var cellIndex = gdReturnStockInDetail.Rows[e.Cell.Row.Index];

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

            gdReturnStockInDetail.UpdateData();
            //设置入库单明细Grid自适应列宽（根据单元格内容）
            gdReturnStockInDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }
        #endregion

        #region 按钮事件

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            #region 验证

            for (int i = 0; i < gdReturnStockInDetail.Rows.Count; i++)
            {
                //验证入库数量
                if (!BLLCom.IsDecimal(gdReturnStockInDetail.Rows[i].Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Qty].Text.Trim()))
                {
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, SystemTableColumnEnums.PIS_StockInDetail.Name.SID_Qty, MsgParam.POSITIVE_INTEGER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //计算入库金额：入库金额 = 入库数量 * 入库单价
                gdReturnStockInDetail.Rows[i].Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Amount].Value =
                    Convert.ToDecimal(gdReturnStockInDetail.Rows[i].Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Qty].Value) *
                    Convert.ToDecimal(gdReturnStockInDetail.Rows[i].Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_UnitCostPrice].Value);
            }
            //[退货明细]列表中配件的拒收数量要与[退货入库明细]列表中对应配件的入库总数量一致
            foreach (var loopSalesOrderReturnDetail in SalesOrderReturnDetailList)
            {
                var tempStockInDetailList = ReturnStockInDetailList.Where(x => x.SID_Barcode == loopSalesOrderReturnDetail.SOD_Barcode).ToList();
                //退货入库明细中该配件的入库总数量
                decimal barcodeStockInTotalQty = 0;
                foreach (var loopStockInDetail in tempStockInDetailList)
                {
                    barcodeStockInTotalQty += (loopStockInDetail.SID_Qty ?? 0);
                }
                //退货明细中该配件的退货总数量
                decimal barcodeReturnTotalQty = 0;
                if (_confirmReturnSourType == SDViewParamValue.ConfirmReturnSourType.VerifySalesOrder)
                {
                    barcodeReturnTotalQty = loopSalesOrderReturnDetail.SOD_RejectQty ?? 0;
                }
                else if (_confirmReturnSourType == SDViewParamValue.ConfirmReturnSourType.ApproveReturnSalesOrder)
                {
                    barcodeReturnTotalQty = loopSalesOrderReturnDetail.SOD_Qty ?? 0;
                }
                if (barcodeStockInTotalQty < barcodeReturnTotalQty)
                {
                    //配件：loopSalesOrderReturnDetail.SOD_Name入库数量的总和小于退货数量，请调整入库数量！
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[]
                    {"配件："+loopSalesOrderReturnDetail.SOD_Name+"入库数量的总和小于退货数量，请调整入库数量！"}), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (barcodeStockInTotalQty > barcodeReturnTotalQty)
                {
                    //配件：loopSalesOrderReturnDetail.SOD_Name入库数量的总和大于退货数量，请调整入库数量！
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[]
                    {"配件："+loopSalesOrderReturnDetail.SOD_Name+"入库数量的总和大于退货数量，请调整入库数量！"}), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            #endregion

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #endregion

        #region 自定义方法

        /// <summary>
        /// 设置单元格是否可以编辑
        /// </summary>
        private void SetPayConfirmWindowStyle()
        {
            #region 设置Grid数据颜色
           

            #endregion
        }

        /// <summary>
        /// 设置单元格是否可以编辑
        /// </summary>
        private void SetSalesOrderReturnDetailStyle()
        {
            #region 设置Grid数据颜色
           
            gdSalesOrderReturnDetail.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
           
            #endregion
        }

        /// <summary>
        /// 设置单元格是否可以编辑
        /// </summary>
        private void SetReturnStockInDetailStyle()
        {
            #region 设置Grid数据颜色
            
            gdReturnStockInDetail.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
            foreach (var loopGridRow in gdReturnStockInDetail.Rows)
            {
                //设置单元格 
                loopGridRow.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Qty].Activation = Activation.AllowEdit;
                //设置单元格背景色
                loopGridRow.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Qty].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                //设置单元格边框颜色
                loopGridRow.Cells[SystemTableColumnEnums.PIS_StockInDetail.Code.SID_Qty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;
            }

            #endregion
        }

        #endregion

    }
}
