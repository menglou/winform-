using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using System.Data.SqlClient;
using SkyCar.Coeus.UIModel.PIS.UIModel;

namespace SkyCar.Coeus.UI.PIS
{
    /// <summary>
    /// 采购退货出库管理
    /// </summary>
    public partial class FrmPurchaseReturnManager : BaseFormCardListDetail<PurchaseReturnManagerUIModel, PurchaseReturnManagerQCModel, MDLPIS_StockOutBill>
    {
        #region 全局变量

        /// <summary>
        /// 采购退货出库管理BLL
        /// </summary>
        private PurchaseReturnManagerBLL _bll = new PurchaseReturnManagerBLL();
        /// <summary>
        /// 【详情】Tab内Grid绑定用的数据源
        /// </summary>
        private SkyCarBindingList<StockOutBillManagerDetailUIModel, MDLPIS_StockOutBillDetail> _detailGridDS = new SkyCarBindingList<StockOutBillManagerDetailUIModel, MDLPIS_StockOutBillDetail>();
        /// <summary>
        /// 页面参数字典
        /// </summary>
        private Dictionary<string, object> _viewParameters;

        #region 下拉框数据源

        /// <summary>
        /// 出库单来源类型数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _stockOutBillSourceTypeDs = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 出库单单据状态数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _stockOutBillStatusDs = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 审核状态数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _approvalStatusDs = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 供应商数据源
        /// </summary>
        List<MDLPIS_Supplier> _supplierList = new List<MDLPIS_Supplier>();

        #endregion

        /// <summary>
        /// 界面属性值发生变化时不予检查的属性列表
        /// </summary>
        List<string> _skipPropertyList = new List<string>();

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmPurchaseReturnManager构造方法
        /// </summary>
        public FrmPurchaseReturnManager()
        {
            InitializeComponent();
        }
        /// <summary>
        /// FrmPurchaseReturnManager构造方法
        /// </summary>
        /// <param name="paramWindowParameters"></param>
        public FrmPurchaseReturnManager(Dictionary<string, object> paramWindowParameters)
        {
            InitializeComponent();
            _viewParameters = paramWindowParameters;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmPurchaseReturnManager_Load(object sender, EventArgs e)
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

            #region 处理参数

            if (_viewParameters != null)
            {
                #region 接收采购单数据

                if (_viewParameters.ContainsKey(PISViewParamKey.PurchaseOrder.ToString()))
                {
                    MDLPIS_PurchaseOrder resultPurchaseOrder = _viewParameters[PISViewParamKey.PurchaseOrder.ToString()] as MDLPIS_PurchaseOrder;
                    //出库单.[来源类型]为{退货出库}
                    cbSOB_SourceTypeName.Text = StockOutBillSourceTypeEnum.Name.THCK;
                    cbSOB_SourceTypeName.Value = StockOutBillSourceTypeEnum.Code.THCK;
                    if (resultPurchaseOrder != null)
                    {
                        txtSOB_SourceNo.Text = resultPurchaseOrder.PO_No;
                        mcbSOB_SUPP_Name.SelectedValue = resultPurchaseOrder.PO_SUPP_ID;
                        mcbSOB_SUPP_Name.SelectedText = resultPurchaseOrder.PO_SUPP_Name;
                    }
                    txtSOB_SourceNo.Enabled = false;
                    //选择【详情】Tab
                    tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;
                    //根据入库单单号获取出库明细
                    SetStockOutDetailGrid();
                }

                #endregion
            }

            #endregion

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
        private void txtWhere_SOB_No_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    //QueryAction();
            //}
        }
        /// <summary>
        /// 来源单号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_SOB_SourceNo_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    //QueryAction();
            //}
        }
        /// <summary>
        /// 单据状态ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_SOB_StatusName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 审核状态ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_SOB_ApprovalStatusName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }

        /// <summary>
        /// 【列表】查询来源单号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_SOB_SourceNo_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            #region 查询入库单

            //仅查询[来源类型]为{手工入库}或{采购入库}，[审核状态]为{已审核}的入库单
            var paramApprovetatusList = _approvalStatusDs.Where(x => x.Text == ApprovalStatusEnum.Name.YSH).ToList();

            List<ComComboBoxDataSourceTC> stockInSourceTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.StockInBillSourceType);
            var paramSourceTypeList = stockInSourceTypeList.Where(x => x.Text == StockInBillSourceTypeEnum.Name.SGCJ || x.Text == StockInBillSourceTypeEnum.Name.CGRK).ToList();

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //审核状态
                {ComViewParamKey.ApprovalStatus.ToString(), paramApprovetatusList},
                //入库单来源类型
                {ComViewParamKey.SourceType.ToString(), paramSourceTypeList},
            };
            FrmPurchaseOrderQuery frmPurchaseOrderQuery = new FrmPurchaseOrderQuery(paramViewParameters)
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
                txtWhere_SOB_SourceNo.Text = frmPurchaseOrderQuery.SelectedGridList[0].PO_No;
            }
            #endregion
        }

        #endregion

        #region 单头相关事件

        /// <summary>
        /// 【详情】查询来源单号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSOB_SourceNo_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(cbSOB_SourceTypeName.Text))
            {
                //请先选择来源类型，再选择来源单号
                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0034, new object[] { MsgParam.SOURCE_TYPE, SystemTableColumnEnums.PIS_StockOutBill.Name.SOB_SourceNo }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cbSOB_SourceTypeName.Text == StockOutBillSourceTypeEnum.Name.THCK)
            {
                #region 退货出库

                //仅查询[来源类型]为{手工入库}或{采购入库}，[审核状态]为{已审核}的入库单
                var paramApprovetatusList = _approvalStatusDs.Where(x => x.Text == ApprovalStatusEnum.Name.YSH).ToList();

                List<ComComboBoxDataSourceTC> stockInSourceTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.StockInBillSourceType);
                var paramSourceTypeList = stockInSourceTypeList.Where(x => x.Text == StockInBillSourceTypeEnum.Name.SGCJ || x.Text == StockInBillSourceTypeEnum.Name.CGRK).ToList();

                Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
                {
                    //审核状态
                    {ComViewParamKey.ApprovalStatus.ToString(), paramApprovetatusList},
                    //入库单来源类型
                    {ComViewParamKey.SourceType.ToString(), paramSourceTypeList},
                };
                FrmStockInBillQuery frmStockInBillQuery = new FrmStockInBillQuery(paramViewParameters)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = frmStockInBillQuery.ShowDialog();

                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                if (frmStockInBillQuery.SelectedGridList.Count == 1)
                {
                    //来源单号
                    txtSOB_SourceNo.Text = frmStockInBillQuery.SelectedGridList[0].SIB_No;
                    //供应商ID
                    mcbSOB_SUPP_Name.SelectedValue = frmStockInBillQuery.SelectedGridList[0].SID_SUPP_ID;
                    //供应商名称
                    mcbSOB_SUPP_Name.SelectedText = frmStockInBillQuery.SelectedGridList[0].SUPP_Name;
                }

                #region 根据[来源单号]加载[出库单明细]列表

                if (string.IsNullOrEmpty(txtSOB_SourceNo.Text))
                {
                    //请选择来源单号
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, SystemTableColumnEnums.PIS_StockOutBill.Name.SOB_SourceNo), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //根据入库单单号获取出库明细
                SetStockOutDetailGrid();

                //根据[是否存在明细]控制单头是否可编辑
                SetDetailByIsExistDetail();
                #endregion

                #endregion
            }
        }
        #endregion

        #region 明细toolBar事件

        /// <summary>
        /// 添加/删除 出库单明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolbarsManagerDetail_ToolClick(object sender, ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case SysConst.EN_DEL:
                    //删除
                    DeleteStockOutDetail();
                    break;
            }
        }
        #endregion

        #region 明细列表相关事件

        /// <summary>
        /// 出库单明细单元格值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_CellChange(object sender, CellEventArgs e)
        {
            gdDetail.UpdateData();
            //当前行
            var curActiveRow = gdDetail.Rows[e.Cell.Row.Index];

            //Cell为进货单价或出库数量 
            if (e.Cell.Column.Key == SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_UnitCostPrice
                || e.Cell.Column.Key == SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_Qty)
            {
                gdDetail.UpdateData();
                //计算总金额：出库金额 = 出库单价 * 出库数量
                if (BLLCom.IsDecimal(curActiveRow.Cells[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_UnitCostPrice].Value?.ToString())
                    && BLLCom.IsDecimal(curActiveRow.Cells[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_Qty].Value?.ToString()))
                {
                    curActiveRow.Cells[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_Amount].Value = Math.Round(
                        Convert.ToDecimal(curActiveRow.Cells[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_UnitCostPrice].Value?.ToString()) *
                        Convert.ToDecimal(curActiveRow.Cells[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_Qty].Value?.ToString()), 2);
                }
            }
            //设置出库单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
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

            //控制详情是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();
            //设置单元格样式
            SetWarehouseStyle();

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
            bool saveResult = _bll.SaveDetailDS(HeadDS, _detailGridDS);
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

            //4.开始监控List变化
            _detailGridDS.StartMonitChanges();
            //5.将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //控制详情是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置出库单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
            //设置单元格样式
            SetWarehouseStyle();
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
            var argsDetail = new List<MDLPIS_StockOutBillDetail>();
            //将HeadDS转换为TBModel对象
            var argsHead = HeadDS.ToTBModelForSaveAndDelete<MDLPIS_StockOutBill>();
            //将当前DetailGridDS转换为指定类型的TBModelList
            _detailGridDS.ToTBModelListForUpdateAndDelete<MDLPIS_StockOutBillDetail>(argsDetail);
            //过滤明细列表中未保存的数据
            argsDetail = argsDetail.Where(x => !string.IsNullOrEmpty(x.WHERE_SOBD_ID)).ToList();
            //2.执行删除
            bool deleteResult = _bll.UnityDelete<MDLPIS_StockOutBill, MDLPIS_StockOutBillDetail>(argsHead, argsDetail);
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

            //将HeadDS数据赋值给【详情】Tab内的对应控件
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
            base.ConditionDS = new PurchaseReturnManagerQCModel()
            {
                //查询用SqlId 
                SqlId = SQLID.PIS_PurchaseReturnManager_SQL_01,
                //单号
                WHERE_SOB_No = txtWhere_SOB_No.Text.Trim(),
                //来源单号
                WHERE_SOB_SourceNo = txtWhere_SOB_SourceNo.Text.Trim(),
                //供应商名称
                WHERE_SOB_SUPP_Name = mcbWhere_SOB_SUPP_Name.SelectedText,
                //单据状态
                WHERE_SOB_StatusName = cbWhere_SOB_StatusName.Text.Trim(),
                //审核状态
                WHERE_SOB_ApprovalStatusName = cbWhere_SOB_ApprovalStatusName.Text.Trim(),
                //组织
                WHERE_SOB_Org_ID = LoginInfoDAX.OrgID,
            };
            //3.执行基类方法（注意：基类使用模糊查询）
            base.QueryAction();
            //4.Grid绑定数据源
            gdGrid.DataSource = base.HeadGridDS;
            gdGrid.DataBind();
            //5.设置Grid自适应列宽（根据单元格内容）
            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdGrid.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
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

            #region 验证

            //出库单未保存,不能审核
            if (string.IsNullOrEmpty(txtSOB_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_StockOutBill + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MDLPIS_StockOutBill resultStockOutBill = new MDLPIS_StockOutBill();
            _bll.QueryForObject<MDLPIS_StockOutBill, MDLPIS_StockOutBill>(new MDLPIS_StockOutBill()
            {
                WHERE_SOB_IsValid = true,
                WHERE_SOB_ID = txtSOB_ID.Text.Trim()
            }, resultStockOutBill);
            //出库单不存在,不能审核
            if (string.IsNullOrEmpty(resultStockOutBill.SOB_ID))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_StockOutBill + MsgParam.NOTEXIST, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            #endregion

            //确认审核操作
            DialogResult isConfirmApprove = MessageBoxs.Show(Trans.PIS, this.ToString(),
                MsgHelp.GetMsg(MsgCode.W_0014),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isConfirmApprove != DialogResult.OK)
            {
                return;
            }

            SetCardCtrlsToDetailDS();

            bool saveApprove = _bll.ApproveDetailDS(HeadDS, _detailGridDS);
            //审核失败
            if (!saveApprove)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //审核成功
            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.APPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //开始监控List变化
            _detailGridDS.StartMonitChanges();
            //将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //控制详情是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();
            //设置单元格样式
            SetWarehouseStyle();
        }

        /// <summary>
        /// 打印
        /// </summary>
        public override void PrintAction()
        {
            try
            {
                if (HeadDS == null || string.IsNullOrEmpty(HeadDS.SOB_No))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, SystemTableEnums.Name.PIS_StockOutBill), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //出库单待审核，不能打印
                if (HeadDS.SOB_ApprovalStatusName != ApprovalStatusEnum.Name.YSH)
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, SystemTableEnums.Name.PIS_StockOutBill + cbSOB_ApprovalStatusName.Text, SystemActionEnum.Name.PRINT), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //待打印的出库单
                StockOutBillUIModelToPrint stockOutBillToPrint = new StockOutBillUIModelToPrint();
                _bll.CopyModel(HeadDS, stockOutBillToPrint);
                //待打印的出库单明细
                List<StockOutBillDetailUIModelToPrint> stockOutBillDetailToPrintList = new List<StockOutBillDetailUIModelToPrint>();
                _bll.CopyModelList(_detailGridDS, stockOutBillDetailToPrintList);
                Dictionary<string, object> argsViewParams = new Dictionary<string, object>
                {
                    //出库单
                    {PISViewParamKey.StockOutBill.ToString(), stockOutBillToPrint},
                    //出库单明细
                    {PISViewParamKey.StockOutBillDetail.ToString(), stockOutBillDetailToPrintList},
                };

                FrmViewAndPrintReturnStockOutBill frmViewAndPrintReturnStockOutBill = new FrmViewAndPrintReturnStockOutBill(argsViewParams)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = frmViewAndPrintReturnStockOutBill.ShowDialog();
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
        /// 转结算
        /// </summary>
        public override void ToSettlementNavigate()
        {
            base.ToSettlementNavigate();

            base.ToReceiptBillNavigate();
            gdGrid.UpdateData();
            //选中的待收款的[出库单]列表
            List<PurchaseReturnManagerUIModel> stockOutBillManagerList = new List<PurchaseReturnManagerUIModel>();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 当前选中Tab为【详情】

                if (HeadDS == null || string.IsNullOrEmpty(HeadDS.SOB_No) || HeadDS.SOB_StatusName != StockOutBillStatusEnum.Name.YWC || HeadDS.APB_UnPaidAmount >= 0)
                {
                    //请选择一个已完成并且未收清的的出库单转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ONE + StockOutBillStatusEnum.Name.YWC + MsgParam.AND + MsgParam.NOTYET + MsgParam.RECEIVE__FULL + MsgParam.OF + SystemTableEnums.Name.PIS_StockOutBill + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                stockOutBillManagerList.Add(HeadDS);

                #endregion
            }
            else
            {
                #region 当前选中Tab为【列表】

                var checkedGrid = HeadGridDS.Where(x => x.IsChecked == true).ToList();

                if (checkedGrid.Count == 0)
                {
                    //请勾选至少一条已完成并且未收清的的出库单转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { StockOutBillStatusEnum.Name.YWC + MsgParam.AND + MsgParam.NOTYET + MsgParam.RECEIVE__FULL + MsgParam.OF + SystemTableEnums.Name.PIS_StockOutBill, SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var JYCGAndPayFullList = checkedGrid.Where(x => x.SOB_StatusName != StockOutBillStatusEnum.Name.YWC || x.APB_UnPaidAmount >= 0).ToList();
                if (JYCGAndPayFullList.Count > 0)
                {
                    //请选择一个已完成并且未收清的的出库单转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ONE + StockOutBillStatusEnum.Name.YWC + MsgParam.AND + MsgParam.NOTYET + MsgParam.RECEIVE__FULL + MsgParam.OF + SystemTableEnums.Name.PIS_StockOutBill + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    foreach (var loopCheckedItem in JYCGAndPayFullList)
                    {
                        loopCheckedItem.IsChecked = false;
                    }
                    gdGrid.DataSource = HeadGridDS;
                    gdGrid.DataBind();
                    return;
                }
                var firstCheckedItem = checkedGrid.FirstOrDefault(x => x.SOB_ApprovalStatusName == ApprovalStatusEnum.Name.YSH);
                if (firstCheckedItem == null)
                {
                    //请选择一个已完成并且未收清的的出库单转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ONE + StockOutBillStatusEnum.Name.YWC + MsgParam.AND + MsgParam.NOTYET + MsgParam.RECEIVE__FULL + MsgParam.OF + SystemTableEnums.Name.PIS_StockOutBill + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //选择的待审核的出库单
                var differFirstCheckedItem = checkedGrid.Where(x => x.SOB_ApprovalStatusName != ApprovalStatusEnum.Name.YSH).ToList();
                if (differFirstCheckedItem.Count > 0)
                {
                    //请选择已审核的出库单转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { ApprovalStatusEnum.Name.YSH + MsgParam.OF + SystemTableEnums.Name.PIS_StockOutBill + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Error);

                    foreach (var loopCheckedItem in differFirstCheckedItem)
                    {
                        loopCheckedItem.IsChecked = false;
                    }
                    gdGrid.DataSource = HeadGridDS;
                    gdGrid.DataBind();
                    return;
                }

                _bll.CopyModelList(checkedGrid, stockOutBillManagerList);

                #endregion
            }

            #region 访问数据库，获取应收单数据

            //传入的待收款的[销售订单]列表
            StockOutDataSet.StockOutDataTable stockOutDataTable = new StockOutDataSet.StockOutDataTable();

            foreach (var loopStockOutDetail in stockOutBillManagerList)
            {
                if (string.IsNullOrEmpty(loopStockOutDetail.SOB_No))
                {
                    continue;
                }

                StockOutDataSet.StockOutRow newStockOutDetailRow = stockOutDataTable.NewStockOutRow();
                newStockOutDetailRow.SOB_Org_ID = loopStockOutDetail.SOB_Org_ID;
                newStockOutDetailRow.SOB_No = loopStockOutDetail.SOB_No;
                newStockOutDetailRow.SOB_SourceTypeName = loopStockOutDetail.SOB_SourceTypeName;

                stockOutDataTable.AddStockOutRow(newStockOutDetailRow);
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

            StockOutDataSet.StockOutToReceiptDataTable resultStockOutToReceiptList =
                new StockOutDataSet.StockOutToReceiptDataTable();

            try
            {
                cmd.CommandText = "P_PIS_GetStockOutListToReceipt";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@StockOutList", SqlDbType.Structured);
                cmd.Parameters[0].Value = stockOutDataTable;

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(resultStockOutToReceiptList);

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

            //待确认收款的出库单信息列表
            List<BusinessCollectionConfirmUIModel> stockOutBillToReceiptList = new List<BusinessCollectionConfirmUIModel>();
            foreach (var loopSalesOrderToReceipt in resultStockOutToReceiptList)
            {
                BusinessCollectionConfirmUIModel salesOrderToReceipt = new BusinessCollectionConfirmUIModel
                {
                    IsBusinessSourceAccountPayableBill = true,
                    BusinessID = loopSalesOrderToReceipt.BusinessID,
                    BusinessNo = loopSalesOrderToReceipt.BusinessNo,
                    BusinessOrgID = loopSalesOrderToReceipt.BusinessOrgID,
                    BusinessOrgName = loopSalesOrderToReceipt.BusinessOrgName,
                    BusinessSourceTypeName = loopSalesOrderToReceipt.BusinessSourceTypeName,
                    BusinessSourceTypeCode = loopSalesOrderToReceipt.BusinessSourceTypeCode,
                    BusinessSourceNo = loopSalesOrderToReceipt.BusinessSourceNo,
                    PayObjectTypeName = AmountTransObjectTypeEnum.Name.AUTOPARTSSUPPLIER,
                    PayObjectTypeCode = AmountTransObjectTypeEnum.Code.AUTOPARTSSUPPLIER,
                    PayObjectID = loopSalesOrderToReceipt.PayObjectID,
                    PayObjectName = loopSalesOrderToReceipt.PayObjectName,
                    ReceivableTotalAmount = -loopSalesOrderToReceipt.ReceivableTotalAmount,
                    ReceiveTotalAmount = -loopSalesOrderToReceipt.ReceiveTotalAmount,
                    UnReceiveTotalAmount = -loopSalesOrderToReceipt.UnReceiveTotalAmount,

                    //应付单相关
                    ARB_ID = loopSalesOrderToReceipt.APB_ID,
                    ARB_No = loopSalesOrderToReceipt.APB_No,
                    ARB_BillDirectCode = BillDirectionEnum.Code.MINUS,
                    ARB_BillDirectName = BillDirectionEnum.Name.MINUS,
                    ARB_SourceTypeCode = loopSalesOrderToReceipt.APB_SourceTypeCode,
                    ARB_SourceTypeName = loopSalesOrderToReceipt.APB_SourceTypeName,
                    ARB_SrcBillNo = loopSalesOrderToReceipt.APB_SourceBillNo,
                    ARB_Org_ID = loopSalesOrderToReceipt.APB_Org_ID,
                    ARB_Org_Name = loopSalesOrderToReceipt.APB_Org_Name,
                    ARB_AccountReceivableAmount = loopSalesOrderToReceipt.APB_AccountPayableAmount,
                    ARB_ReceivedAmount = loopSalesOrderToReceipt.APB_PaidAmount,
                    ARB_UnReceiveAmount = loopSalesOrderToReceipt.APB_UnpaidAmount,
                    ARB_BusinessStatusCode = loopSalesOrderToReceipt.APB_BusinessStatusCode,
                    ARB_BusinessStatusName = loopSalesOrderToReceipt.APB_BusinessStatusName,
                    ARB_ApprovalStatusCode = loopSalesOrderToReceipt.APB_ApprovalStatusCode,
                    ARB_ApprovalStatusName = loopSalesOrderToReceipt.APB_ApprovalStatusName,
                    ARB_CreatedBy = loopSalesOrderToReceipt.APB_CreatedBy,
                    ARB_CreatedTime = loopSalesOrderToReceipt.APB_CreatedTime,
                    ARB_VersionNo = loopSalesOrderToReceipt.APB_VersionNo,
                };
                stockOutBillToReceiptList.Add(salesOrderToReceipt);
            }

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //业务单确认收款
                {ComViewParamKey.BusinessCollectionConfirm.ToString(), stockOutBillToReceiptList}
            };

            //跳转[业务单确认收款弹出窗]
            FrmBusinessCollectionConfirmWindow frmBusinessCollectionConfirmWindow = new FrmBusinessCollectionConfirmWindow(paramViewParameters)
            {
                StartPosition = FormStartPosition.CenterScreen
            };

            DialogResult dialogResult = frmBusinessCollectionConfirmWindow.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                QueryAction();
                return;
            }
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 工具生成
            //单据编号
            txtSOB_No.Clear();
            //组织
            txtOrg_ShortName.Clear();
            //来源类型
            cbSOB_SourceTypeName.Items.Clear();
            //来源单号
            txtSOB_SourceNo.Clear();
            //供应商名称
            mcbSOB_SUPP_Name.Clear();
            //单据状态
            cbSOB_StatusName.Items.Clear();
            //审核状态
            cbSOB_ApprovalStatusName.Items.Clear();
            //备注
            txtSOB_Remark.Clear();
            //有效
            ckSOB_IsValid.Checked = true;
            ckSOB_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtSOB_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtSOB_CreatedTime.Value = DateTime.Now;
            //修改人
            txtSOB_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtSOB_UpdatedTime.Value = DateTime.Now;
            //出库单ID
            txtSOB_ID.Clear();
            //组织ID
            txtSOB_Org_ID.Clear();
            //版本号
            txtSOB_VersionNo.Clear();
            //给 单据编号 设置焦点
            lblSOB_No.Focus();
            //清空[明细]Grid
            _detailGridDS = new SkyCarBindingList<StockOutBillManagerDetailUIModel, MDLPIS_StockOutBillDetail>();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();

            //3.开始监控List变化（该数据源若有其他调整，请在StartMonitChanges()方法执行前调整）
            _detailGridDS.StartMonitChanges();

            #endregion

            #region 初始化下拉框
            //来源类型
            _stockOutBillSourceTypeDs = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.StockOutBillSourceType);
            var tempStockOutBillSourceTypeDs = _stockOutBillSourceTypeDs.Where(x => x.Text == StockOutBillSourceTypeEnum.Name.THCK).ToList();
            //只可新增来源类型为[退货出库]的出库单
            cbSOB_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
            cbSOB_SourceTypeName.ValueMember = SysConst.EN_Code;
            cbSOB_SourceTypeName.DataSource = tempStockOutBillSourceTypeDs;
            cbSOB_SourceTypeName.DataBind();
            //默认来源类型为[退货出库]
            cbSOB_SourceTypeName.Text = StockOutBillSourceTypeEnum.Name.THCK;
            cbSOB_SourceTypeName.Value = StockOutBillSourceTypeEnum.Code.THCK;

            //单据状态
            _stockOutBillStatusDs = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.StockOutBillStatus);
            cbSOB_StatusName.DisplayMember = SysConst.EN_TEXT;
            cbSOB_StatusName.ValueMember = SysConst.EN_Code;
            cbSOB_StatusName.DataSource = _stockOutBillStatusDs;
            cbSOB_StatusName.DataBind();
            //默认单据状态为[已完成]
            cbSOB_StatusName.Text = StockOutBillStatusEnum.Name.YWC;
            cbSOB_StatusName.Value = StockOutBillStatusEnum.Code.YWC;

            //审核状态
            _approvalStatusDs = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.ApprovalStatus);
            cbSOB_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
            cbSOB_ApprovalStatusName.ValueMember = SysConst.EN_Code;
            cbSOB_ApprovalStatusName.DataSource = _approvalStatusDs;
            cbSOB_ApprovalStatusName.DataBind();
            //默认审核状态为[待审核]
            cbSOB_ApprovalStatusName.Text = ApprovalStatusEnum.Name.DSH;
            cbSOB_ApprovalStatusName.Value = ApprovalStatusEnum.Code.DSH;

            //供应商
            _supplierList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsSupplier) as List<MDLPIS_Supplier>;
            mcbSOB_SUPP_Name.DisplayMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_Name;
            mcbSOB_SUPP_Name.ValueMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_ID;
            mcbSOB_SUPP_Name.DataSource = _supplierList;
            #endregion

            //初始化明细Grid
            _detailGridDS = new SkyCarBindingList<StockOutBillManagerDetailUIModel, MDLPIS_StockOutBillDetail>();
            _detailGridDS.StartMonitChanges();

            //默认组织为当前组织
            txtOrg_ShortName.Text = LoginInfoDAX.OrgShortName;
            txtSOB_Org_ID.Text = LoginInfoDAX.OrgID;
        }
        
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //单号
            txtWhere_SOB_No.Clear();
            //来源单号
            txtWhere_SOB_SourceNo.Clear();
            //供应商
            mcbWhere_SOB_SUPP_Name.Clear();
            //单据状态
            cbWhere_SOB_StatusName.Items.Clear();
            //审核状态
            cbWhere_SOB_ApprovalStatusName.Items.Clear();
            //给 单号 设置焦点
            lblWhere_SOB_No.Focus();

            #endregion 初始化下拉框

            //供应商
            mcbWhere_SOB_SUPP_Name.DisplayMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_Name;
            mcbWhere_SOB_SUPP_Name.ValueMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_ID;
            mcbWhere_SOB_SUPP_Name.DataSource = _supplierList;

            #region 初始化下拉框



            #endregion

            #region Grid初始化

            //清空Grid
            HeadGridDS = new BindingList<PurchaseReturnManagerUIModel>();
            gdGrid.DataSource = HeadGridDS;
            gdGrid.DataBind();

            #endregion

            #endregion

            #region 初始化下拉框

            //单据状态
            cbWhere_SOB_StatusName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_SOB_StatusName.ValueMember = SysConst.EN_Code;
            cbWhere_SOB_StatusName.DataSource = _stockOutBillStatusDs;
            cbWhere_SOB_StatusName.DataBind();

            //审核状态
            cbWhere_SOB_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_SOB_ApprovalStatusName.ValueMember = SysConst.EN_Code;
            cbWhere_SOB_ApprovalStatusName.DataSource = _approvalStatusDs;
            cbWhere_SOB_ApprovalStatusName.DataBind();
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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_StockOutBill.Code.SOB_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_StockOutBill.Code.SOB_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[HeadDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            HeadDS = base.HeadGridDS.FirstOrDefault(x => x.SOB_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_StockOutBill.Code.SOB_ID].Value);
            if (HeadDS == null || string.IsNullOrEmpty(HeadDS.SOB_ID))
            {
                return;
            }

            //设置[来源类型]数据源
            cbSOB_SourceTypeName.DataSource = _stockOutBillSourceTypeDs;
            cbSOB_SourceTypeName.DataBind();

            if (txtSOB_ID.Text != HeadDS.SOB_ID
                || (txtSOB_ID.Text == HeadDS.SOB_ID && txtSOB_VersionNo.Text != HeadDS.SOB_VersionNo?.ToString()))
            {
                if (txtSOB_ID.Text == HeadDS.SOB_ID && txtSOB_VersionNo.Text != HeadDS.SOB_VersionNo?.ToString())
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

            //控制详情是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();
            //设置单元格样式
            SetWarehouseStyle();
        }
        
        /// <summary>
        /// 将HeadDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //单据编号
            txtSOB_No.Text = HeadDS.SOB_No;
            //组织
            txtOrg_ShortName.Text = HeadDS.Org_ShortName;
            //来源类型名称
            cbSOB_SourceTypeName.Text = HeadDS.SOB_SourceTypeName;
            //来源类型编码
            cbSOB_SourceTypeName.Value = HeadDS.SOB_SourceTypeCode;
            //来源单号
            txtSOB_SourceNo.Text = HeadDS.SOB_SourceNo;
            //供应商ID
            mcbSOB_SUPP_Name.SelectedValue = HeadDS.SOB_SUPP_ID;
            //供应商名称
            mcbSOB_SUPP_Name.SelectedText = HeadDS.SOB_SUPP_Name;
            //单据状态名称
            cbSOB_StatusName.Text = HeadDS.SOB_StatusName;
            //单据状态编码
            cbSOB_StatusName.Value = HeadDS.SOB_StatusCode;
            //审核状态名称
            cbSOB_ApprovalStatusName.Text = HeadDS.SOB_ApprovalStatusName;
            //审核状态编码
            cbSOB_ApprovalStatusName.Value = HeadDS.SOB_ApprovalStatusCode;
            //备注
            txtSOB_Remark.Text = HeadDS.SOB_Remark;
            //有效
            if (HeadDS.SOB_IsValid != null)
            {
                ckSOB_IsValid.Checked = HeadDS.SOB_IsValid.Value;
            }
            //创建人
            txtSOB_CreatedBy.Text = HeadDS.SOB_CreatedBy;
            //创建时间
            dtSOB_CreatedTime.Value = HeadDS.SOB_CreatedTime;
            //修改人
            txtSOB_UpdatedBy.Text = HeadDS.SOB_UpdatedBy;
            //修改时间
            dtSOB_UpdatedTime.Value = HeadDS.SOB_UpdatedTime;
            //出库单ID
            txtSOB_ID.Text = HeadDS.SOB_ID;
            //组织ID
            txtSOB_Org_ID.Text = HeadDS.SOB_Org_ID;
            //版本号
            txtSOB_VersionNo.Value = HeadDS.SOB_VersionNo;
        }
        
        /// <summary>
        /// 查询明细Grid数据并绑定
        /// </summary>
        private void QueryDetail()
        {
            //1.设置查询条件
            var argsCondition = new PurchaseReturnManagerQCModel()
            {
                //查询用SqlId 
                SqlId = SQLID.PIS_PurchaseReturnManager_SQL_02,
                //出库单ID
                WHERE_SOBD_SOB_ID = txtSOB_ID.Text.Trim(),
                //出库单号
                WHERE_SOBD_SOB_No = txtSOB_No.Text.Trim(),
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
            if (!string.IsNullOrEmpty(txtSOB_Org_ID.Text.Trim()))
            {
                if (txtSOB_Org_ID.Text.Trim() != LoginInfoDAX.OrgID)
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0015, new object[] { SystemActionEnum.Name.SAVE, SystemTableEnums.Name.PIS_StockOutBill }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            //验证来源类型
            if (string.IsNullOrEmpty(cbSOB_SourceTypeName.Text))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.SOURCE_TYPE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //验证来源单号
            if (string.IsNullOrEmpty(txtSOB_SourceNo.Text.Trim()))
            {
                //请选择供应商
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.SUPPLIER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            #region 验证出库单明细

            if (gdDetail.Rows.Count == 0)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0014, new object[] { SystemTableEnums.Name.PIS_StockOutBillDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            int i = 0;
            foreach (var loopDetail in _detailGridDS)
            {
                i++;
                //验证出库数量不能为空
                if (string.IsNullOrEmpty(Convert.ToString(loopDetail.SOBD_Qty ?? 0)))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i, SystemTableColumnEnums.PIS_StockOutBillDetail.Name.SOBD_Qty, MsgParam.POSITIVE_INTEGER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //验证出库数量格式
                if (!BLLCom.IsDecimal(Convert.ToString(loopDetail.SOBD_Qty ?? 0)))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i, SystemTableColumnEnums.PIS_StockOutBillDetail.Name.SOBD_Qty, MsgParam.POSITIVE_INTEGER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //验证进货单价不能为空
                if (string.IsNullOrEmpty(Convert.ToString(loopDetail.SOBD_UnitCostPrice ?? 0)))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i, SystemTableColumnEnums.PIS_StockOutBillDetail.Name.SOBD_UnitCostPrice, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //验证进货单价格式
                if (!BLLCom.IsDecimal(Convert.ToString(loopDetail.SOBD_UnitCostPrice)))
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i, SystemTableColumnEnums.PIS_StockOutBillDetail.Name.SOBD_UnitCostPrice, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (!string.IsNullOrEmpty(txtSOB_Org_ID.Text.Trim()))
            {
                if (LoginInfoDAX.OrgID != txtSOB_Org_ID.Text.Trim())
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
            HeadDS = new PurchaseReturnManagerUIModel()
            {
                //单据编号
                SOB_No = txtSOB_No.Text.Trim(),
                //组织
                Org_ShortName = txtOrg_ShortName.Text.Trim(),
                //来源类型
                SOB_SourceTypeName = cbSOB_SourceTypeName.Text.Trim(),
                //来源类型编码
                SOB_SourceTypeCode = cbSOB_SourceTypeName.Value?.ToString() ?? "",
                //来源单号
                SOB_SourceNo = txtSOB_SourceNo.Text.Trim(),
                //供应商ID
                SOB_SUPP_ID = mcbSOB_SUPP_Name.SelectedValue,
                //供应商名称
                SOB_SUPP_Name = mcbSOB_SUPP_Name.SelectedText,
                //单据状态
                SOB_StatusName = cbSOB_StatusName.Text.Trim(),
                //单据状态编码
                SOB_StatusCode = cbSOB_StatusName.Value?.ToString() ?? "",
                //审核状态
                SOB_ApprovalStatusName = cbSOB_ApprovalStatusName.Text.Trim(),
                //审核状态编码
                SOB_ApprovalStatusCode = cbSOB_ApprovalStatusName.Value?.ToString() ?? "",
                //备注
                SOB_Remark = txtSOB_Remark.Text.Trim(),
                //有效
                SOB_IsValid = ckSOB_IsValid.Checked,
                //创建人
                SOB_CreatedBy = txtSOB_CreatedBy.Text.Trim(),
                //创建时间
                SOB_CreatedTime = (DateTime?)dtSOB_CreatedTime.Value ?? DateTime.Now,
                //修改人
                SOB_UpdatedBy = txtSOB_UpdatedBy.Text.Trim(),
                //修改时间
                SOB_UpdatedTime = (DateTime?)dtSOB_UpdatedTime.Value ?? DateTime.Now,
                //出库单ID
                SOB_ID = txtSOB_ID.Text.Trim(),
                //组织ID
                SOB_Org_ID = txtSOB_Org_ID.Text.Trim(),
                //版本号
                SOB_VersionNo = Convert.ToInt64(txtSOB_VersionNo.Text.Trim() == "" ? "1" : txtSOB_VersionNo.Text.Trim()),
            };
        }

        #region 明细相关

        /// <summary>
        /// 删除出库明细
        /// </summary>
        private void DeleteStockOutDetail()
        {
            #region 验证

            //验证出库单的审核状态，[审核状态]为[已审核]的出库单不能删除明细
            if (cbSOB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableEnums.Name.PIS_StockOutBillDetail + ApprovalStatusEnum.Name.YSH, SystemActionEnum.Name.DELETE + SystemTableEnums.Name.PIS_StockOutBillDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            gdDetail.UpdateData();
            //待删除的出库单明细列表
            var deleteStockOutBillDetailList = _detailGridDS.Where(p => p.IsChecked == true).ToList();
            if (deleteStockOutBillDetailList.Count == 0)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { SystemTableEnums.Name.PIS_StockOutBillDetail, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion

            //移除勾选项
            foreach (var loopStockOutDetail in deleteStockOutBillDetailList)
            {
                _detailGridDS.Remove(loopStockOutDetail);
            }

            //根据[是否存在明细]控制单头是否可编辑
            SetDetailByIsExistDetail();

            if (_detailGridDS.Count == 0)
            {
                //清空来源单号和供应商
                txtSOB_SourceNo.Clear();
                mcbSOB_SUPP_Name.Clear();

                mcbSOB_SUPP_Name.DisplayMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_Name;
                mcbSOB_SUPP_Name.ValueMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_ID;
                mcbSOB_SUPP_Name.DataSource = _supplierList;
            }

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置出库单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        #endregion

        /// <summary>
        /// 设置详情页面控件是否可编辑
        /// </summary>
        private void SetDetailControl()
        {
            if (cbSOB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                #region 出库单.[审核状态]为{已审核}的场合

                //单头
                txtSOB_SourceNo.Enabled = false;
                txtSOB_Remark.Enabled = false;

                //明细列表不可删除
                toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = false;

                #endregion
            }
            else
            {
                #region 出库单未保存 或 出库单.[审核状态]为{待审核}的场合

                //根据[是否存在明细]控制单头是否可编辑
                SetDetailByIsExistDetail();
                txtSOB_Remark.Enabled = true;

                //明细列表可删除
                toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = true;

                #endregion
            }
        }

        /// <summary>
        /// 设置动作按钮状态
        /// </summary>
        private void SetActionEnableByStatus()
        {
            if (cbSOB_SourceTypeName.Text == StockOutBillSourceTypeEnum.Name.XSCK)
            {
                //[来源类型]为[销售出库]的场合，出库单不能[保存]、[删除]、[审核]、[转结算]
                SetActionEnable(SystemActionEnum.Code.SAVE, false);
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                SetActionEnable(SystemActionEnum.Code.APPROVE, false);
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, false);
            }
            else
            {
                if (cbSOB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
                {
                    //[审核状态]为[已审核]的场合，[保存]、[删除]、[审核]、[转结算]不可用
                    SetActionEnable(SystemActionEnum.Code.SAVE, false);
                    SetActionEnable(SystemActionEnum.Code.DELETE, false);
                    SetActionEnable(SystemActionEnum.Code.APPROVE, false);
                    SetActionEnable(SystemActionEnum.Code.PRINT, true);
                    SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, true);
                }
                else
                {
                    //新增或[审核状态]为[待审核]的场合，[打印]、[转结算]不可用
                    SetActionEnable(SystemActionEnum.Code.SAVE, true);
                    SetActionEnable(SystemActionEnum.Code.DELETE, !string.IsNullOrEmpty(txtSOB_ID.Text));
                    SetActionEnable(SystemActionEnum.Code.APPROVE, !string.IsNullOrEmpty(txtSOB_ID.Text));
                    SetActionEnable(SystemActionEnum.Code.PRINT, false);
                    SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, false);
                }
            }
        }

        /// <summary>
        /// 设置单元格样式
        /// </summary>
        private void SetWarehouseStyle()
        {
            #region 设置Grid数据颜色
            gdDetail.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
            foreach (var loopGridRow in gdDetail.Rows)
            {
                if (cbSOB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
                {
                    #region 出库数量

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_Qty].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_Qty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_Qty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion
                }
                else
                {
                    #region 出库数量

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_Qty].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_Qty].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_Qty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion
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
                txtSOB_SourceNo.Enabled = true;
            }
            else
            {
                txtSOB_SourceNo.Enabled = false;
            }
        }

        /// <summary>
        /// 根据入库单单号获取出库明细
        /// </summary>
        private void SetStockOutDetailGrid()
        {
            //根据入库单单号获取出库明细
            List<StockOutBillManagerDetailUIModel> resultStockOutDetailList = new List<StockOutBillManagerDetailUIModel>();
            _bll.QueryForList(SQLID.PIS_StockOutBillManager_SQL_03, new MDLPIS_StockOutBill
            {
                WHERE_SOB_SourceNo = txtSOB_SourceNo.Text,
                WHERE_SOB_UpdatedBy = LoginInfoDAX.UserName,
            }, resultStockOutDetailList);

            foreach (var loopStockOutDetail in resultStockOutDetailList)
            {
                StockOutBillManagerDetailUIModel addStockOutBillDetail = new StockOutBillManagerDetailUIModel()
                {
                    IsChecked = loopStockOutDetail.IsChecked,
                    Tmp_SOBD_ID = loopStockOutDetail.Tmp_SOBD_ID,
                    SOBD_ID = loopStockOutDetail.SOBD_ID,
                    SOBD_SOB_ID = loopStockOutDetail.SOBD_SOB_ID,
                    SOBD_SourceDetailID = loopStockOutDetail.SOBD_SourceDetailID,
                    SOBD_Name = loopStockOutDetail.SOBD_Name,
                    SOBD_OEMNo = loopStockOutDetail.SOBD_OEMNo,
                    SOBD_ThirdNo = loopStockOutDetail.SOBD_ThirdNo,
                    SOBD_Barcode = loopStockOutDetail.SOBD_Barcode,
                    SOBD_BatchNo = loopStockOutDetail.SOBD_BatchNo,
                    SOBD_UnitSalePrice = loopStockOutDetail.SOBD_UnitSalePrice,
                    SOBD_UnitCostPrice = loopStockOutDetail.SOBD_UnitCostPrice,
                    //退货数量：默认原出库数量
                    SOBD_Qty = loopStockOutDetail.SOBD_Qty,
                    SOBD_Amount = loopStockOutDetail.SOBD_Amount,
                    SOBD_Specification = loopStockOutDetail.SOBD_Specification,
                    SOBD_UOM = loopStockOutDetail.SOBD_UOM,
                    SOBD_WH_ID = loopStockOutDetail.SOBD_WH_ID,
                    SOBD_WHB_ID = loopStockOutDetail.SOBD_WHB_ID,

                    SOBD_IsValid = true,
                    SOBD_CreatedBy = LoginInfoDAX.UserName,
                    SOBD_CreatedTime = BLLCom.GetCurStdDatetime(),
                    SOBD_UpdatedBy = LoginInfoDAX.UserName,
                    SOBD_UpdatedTime = BLLCom.GetCurStdDatetime(),
                    SOBD_VersionNo = loopStockOutDetail.SOBD_VersionNo,

                    INV_ID = loopStockOutDetail.INV_ID,
                    INV_Qty = loopStockOutDetail.INV_Qty,
                    INV_SUPP_ID = loopStockOutDetail.INV_SUPP_ID,

                    SUPP_Name = loopStockOutDetail.SUPP_Name,
                    WH_Name = loopStockOutDetail.WH_Name,
                    WHB_Name = loopStockOutDetail.WHB_Name,

                    APA_Level = loopStockOutDetail.APA_Level,
                    APA_Brand = loopStockOutDetail.APA_Brand,
                    APA_VehicleBrand = loopStockOutDetail.APA_VehicleBrand,
                    APA_VehicleInspire = loopStockOutDetail.APA_VehicleInspire,
                    APA_VehicleCapacity = loopStockOutDetail.APA_VehicleCapacity,
                    APA_VehicleGearboxTypeCode = loopStockOutDetail.APA_VehicleGearboxTypeCode,
                    APA_VehicleGearboxTypeName = loopStockOutDetail.APA_VehicleGearboxTypeName,
                    APA_VehicleYearModel = loopStockOutDetail.APA_VehicleYearModel,
                };
                _detailGridDS.Add(addStockOutBillDetail);
            }

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置单元格样式
            SetWarehouseStyle();
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
                    var curHead = HeadGridDS.FirstOrDefault(x => x.SOB_ID == HeadDS.SOB_ID);
                    if (curHead != null)
                    {
                        HeadGridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = HeadGridDS.FirstOrDefault(x => x.SOB_ID == HeadDS.SOB_ID);
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
