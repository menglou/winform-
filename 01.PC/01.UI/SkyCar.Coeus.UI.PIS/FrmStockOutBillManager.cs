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
    /// 出库管理
    /// </summary>
    public partial class FrmStockOutBillManager : BaseFormCardListDetail<StockOutBillManagerUIModel, StockOutBillManagerQCModel, MDLPIS_StockOutBill>
    {
        #region 全局变量

        /// <summary>
        /// 出库管理BLL
        /// </summary>
        private StockOutBillManagerBLL _bll = new StockOutBillManagerBLL();
        /// <summary>
        /// 【详情】Tab内Grid绑定用的数据源
        /// </summary>
        private SkyCarBindingList<StockOutBillManagerDetailUIModel, MDLPIS_StockOutBillDetail> _detailGridDS = new SkyCarBindingList<StockOutBillManagerDetailUIModel, MDLPIS_StockOutBillDetail>();
        /// <summary>
        /// 添加出库明细Func
        /// </summary>
        private Func<List<PickPartsQueryUIModel>, bool> _pickPartsDetailFunc;
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

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmStockOutBillManager构造方法
        /// </summary>
        public FrmStockOutBillManager()
        {
            InitializeComponent();
        }
        /// <summary>
        /// FrmStockOutBillManager构造方法
        /// </summary>
        /// <param name="paramWindowParameters"></param>
        public FrmStockOutBillManager(Dictionary<string, object> paramWindowParameters)
        {
            InitializeComponent();
            _viewParameters = paramWindowParameters;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmStockOutBillManager_Load(object sender, EventArgs e)
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

            #region 暂时只能查询出库单，如果有后续直接出库的需求，再考虑保存、审核等功能

            //隐藏动作[新增]、[保存]、[删除]、[审核]、[反审核]
            SetActionVisiableAndEnable(SystemActionEnum.Code.NEW, false, false);
            SetActionVisiableAndEnable(SystemActionEnum.Code.SAVE, false, false);
            SetActionVisiableAndEnable(SystemActionEnum.Code.DELETE, false, false);
            SetActionVisiableAndEnable(SystemActionEnum.Code.APPROVE, false, false);
            SetActionVisiableAndEnable(SystemActionEnum.Code.UNAPPROVE, false, false);
            //隐藏明细按钮[添加]、[删除
            toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedProps.Visible = false;
            toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Visible = false;
            //隐藏导航按钮[转结算]
            SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, false, false);
            //明细Grid不可编辑（设计界面完成）

            #endregion
            
            //设置单元格样式
            SetStockOutBillStyle();

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
                //[转结算]可用
                //SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, true);
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
        /// 来源类型名称ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_SOB_SourceTypeName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
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
        /// 有效CheckedChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckWhere_SOB_IsValid_CheckedChanged(object sender, EventArgs e)
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
            if (string.IsNullOrEmpty(cbWhere_SOB_SourceTypeName.Text))
            {
                //请先选择来源类型，再选择来源单号
                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0034, new object[] { MsgParam.SOURCE_TYPE, SystemTableColumnEnums.PIS_StockOutBill.Name.SOB_SourceNo }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #region 销售出库

            //仅查询[审核状态]为[已审核]的出库单
            var paramApprovetatusList = _approvalStatusDs.Where(x => x.Text == ApprovalStatusEnum.Name.YSH).ToList();
            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
                {
                    {ComViewParamKey.ApprovalStatus.ToString(), paramApprovetatusList},
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
                txtWhere_SOB_SourceNo.Text = frmSalesOrderQuery.SelectedGridList[0].SO_No;
            }
            #endregion
        }

        #endregion

        #region 单头相关事件

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
                case SysConst.EN_ADD:
                    //添加
                    AddStockOutDetail();
                    break;

                case SysConst.EN_DEL:
                    //删除
                    DeleteStockOutDetail();
                    break;
            }
        }
        #endregion

        #region 明细列表相关事件

        /// <summary>
        /// 出库单明细gdDetail的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_DoubleClick(object sender, EventArgs e)
        {
            UpdateStockOutDetail();
        }

        /// <summary>
        /// 出库单明细gdDetail的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            UpdateStockOutDetail();
        }

        /// <summary>
        /// 出库单明细单击单元格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_ClickCellButton(object sender, CellEventArgs e)
        {
            //更新明细
            if (e.Cell.Column.Key == SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_Name)
            {
                UpdateStockOutDetail();
            }

            gdDetail.UpdateData();
            //设置出库单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

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
                //string id = cellIndex.Cells["Tmp_SOBD_ID"].Text;
                //foreach (var loopDetail in _detailGridDS)
                //{
                //    if (loopDetail.Tmp_SOBD_ID == id)
                //    {
                //        loopDetail.SOBD_Amount = (loopDetail.SOBD_UnitCostPrice ?? 0) * (loopDetail.SOBD_Qty ?? 0);
                //    }
                //}
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

            //控制详情是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();

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
            base.ConditionDS = new StockOutBillManagerQCModel()
            {
                //查询用SqlId 
                SqlId = SQLID.PIS_StockOutBillManager_SQL_01,
                //单号
                WHERE_SOB_No = txtWhere_SOB_No.Text.Trim(),
                //来源类型名称
                WHERE_SOB_SourceTypeName = cbWhere_SOB_SourceTypeName.Text.Trim(),
                //来源单号
                WHERE_SOB_SourceNo = txtWhere_SOB_SourceNo.Text.Trim(),
                //单据状态
                WHERE_SOB_StatusName = cbWhere_SOB_StatusName.Text.Trim(),
                //审核状态
                WHERE_SOB_ApprovalStatusName = cbWhere_SOB_ApprovalStatusName.Text.Trim(),
                //有效
                WHERE_SOB_IsValid = ckWhere_SOB_IsValid.Checked,
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
        }

        /// <summary>
        /// 反审核
        /// </summary>
        public override void UnApproveAction()
        {
            #region 验证

            //出库单未保存,不能反审核
            if (string.IsNullOrEmpty(txtSOB_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_StockOutBill + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MDLPIS_StockOutBill resultStockOutBill = new MDLPIS_StockOutBill();
            _bll.QueryForObject<MDLPIS_StockOutBill, MDLPIS_StockOutBill>(new MDLPIS_StockOutBill()
            {
                WHERE_SOB_IsValid = true,
                WHERE_SOB_ID = txtSOB_ID.Text.Trim()
            }, resultStockOutBill);
            //出库单不存在,不能反审核
            if (string.IsNullOrEmpty(resultStockOutBill.SOB_ID))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.PIS_StockOutBill + MsgParam.NOTEXIST, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //出库单存在[已审核]的[应收单]，不能反审核
            MDLFM_AccountReceivableBill resultAccountPayableBill = _bll.GetAccountReceivableBillByNo(resultStockOutBill.SOB_No);
            if (resultAccountPayableBill != null && resultAccountPayableBill.ARB_ApprovalStatusName == ApprovalStatusEnum.Name.YSH)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0023, new object[] { SystemTableEnums.Name.PIS_StockOutBill, SystemTableEnums.Name.FM_AccountPayableBill, SystemActionEnum.Name.UNAPPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //出库单存在[审核状态]为[已审核] 或者 [单据状态]不是[已生成]的[物流单]，不能反审核
            MDLSD_LogisticsBill resultLogisticsBill = new MDLSD_LogisticsBill();
            _bll.QueryForObject<MDLSD_LogisticsBill, MDLSD_LogisticsBill>(new MDLSD_LogisticsBill()
            {
                WHERE_LB_IsValid = true,
                WHERE_LB_SourceNo = txtSOB_No.Text.Trim(),
            }, resultLogisticsBill);
            if (!string.IsNullOrEmpty(resultLogisticsBill.LB_ID))
            {
                //出库单对应的[物流单].[审核状态]为[已审核]，不能反审核
                if (resultLogisticsBill.LB_ApprovalStatusName == ApprovalStatusEnum.Name.YSH)
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0023, new object[] { SystemTableEnums.Name.SD_SalesOrder, SystemTableEnums.Name.SD_LogisticsBill, SystemActionEnum.Name.UNAPPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //出库单对应的[物流单].[单据状态]不是[已生成]，不能反审核
                if (resultLogisticsBill.LB_StatusName != LogisticsBillStatusEnum.Name.YSC)
                {
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableEnums.Name.SD_SalesOrder + MsgParam.CORRESPONDENCE + MsgParam.OF + SystemTableEnums.Name.SD_LogisticsBill + resultLogisticsBill.LB_StatusName, SystemActionEnum.Name.UNAPPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
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

            //控制详情是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();
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

                FrmViewAndPrintStockOutBill frmViewAndPrintStockOutBill = new FrmViewAndPrintStockOutBill(argsViewParams)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = frmViewAndPrintStockOutBill.ShowDialog();
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
            List<StockOutBillManagerUIModel> stockOutBillManagerList = new List<StockOutBillManagerUIModel>();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 当前选中Tab为【详情】

                if (HeadDS == null || string.IsNullOrEmpty(HeadDS.SOB_No))
                {
                    //请选择来源类型为手工创建并且已审核的出库单转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.SGCJ_YSH + SystemTableEnums.Name.PIS_StockOutBill + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    //请选择来源类型为手工创建并且已审核的出库单转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.SGCJ_YSH + SystemTableEnums.Name.PIS_StockOutBill + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var firstCheckedItem = checkedGrid.FirstOrDefault(x => x.SOB_ApprovalStatusName == ApprovalStatusEnum.Name.YSH && x.SOB_SourceTypeName == StockOutBillSourceTypeEnum.Name.SGCJ);
                if (firstCheckedItem == null)
                {
                    //请选择来源类型为手工创建并且已审核的出库单转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.SGCJ_YSH + SystemTableEnums.Name.PIS_StockOutBill + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //选择不为手工创建的出库单
                var differFirstCheckedItem = checkedGrid.Where(x => x.SOB_SourceTypeName != StockOutBillSourceTypeEnum.Name.SGCJ || x.SOB_ApprovalStatusName != ApprovalStatusEnum.Name.YSH).ToList();
                if (differFirstCheckedItem.Count > 0)
                {
                    //请选择手工创建并且已审核的出库单转结算
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.SGCJ_YSH + SystemTableEnums.Name.PIS_StockOutBill + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Error);

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

            //传入的待收款的[出库单]列表
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
                    ReceivableTotalAmount = loopSalesOrderToReceipt.ReceivableTotalAmount,
                    ReceiveTotalAmount = loopSalesOrderToReceipt.ReceiveTotalAmount,
                    UnReceiveTotalAmount = loopSalesOrderToReceipt.UnReceiveTotalAmount,

                    //应付单相关
                    ARB_ID = loopSalesOrderToReceipt.APB_ID,
                    ARB_No = loopSalesOrderToReceipt.APB_No,
                    ARB_BillDirectCode = BillDirectionEnum.Code.MINUS,
                    ARB_BillDirectName = BillDirectionEnum.Name.MINUS,
                    ARB_SourceTypeCode = loopSalesOrderToReceipt.BusinessSourceTypeCode,
                    ARB_SourceTypeName = loopSalesOrderToReceipt.BusinessSourceTypeName,
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
            var tempStockOutBillSourceTypeDs = _stockOutBillSourceTypeDs.Where(x => x.Text == StockOutBillSourceTypeEnum.Name.SGCJ).ToList();
            //只可新增来源类型为[手工创建]的出库单
            cbSOB_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
            cbSOB_SourceTypeName.ValueMember = SysConst.EN_Code;
            cbSOB_SourceTypeName.DataSource = tempStockOutBillSourceTypeDs;
            cbSOB_SourceTypeName.DataBind();
            //默认来源类型为[手工创建]
            cbSOB_SourceTypeName.Text = StockOutBillSourceTypeEnum.Name.SGCJ;
            cbSOB_SourceTypeName.Value = StockOutBillSourceTypeEnum.Code.SGCJ;
            cbSOB_SourceTypeName.Enabled = false;

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
            //来源类型名称
            cbWhere_SOB_SourceTypeName.Items.Clear();
            //来源单号
            txtWhere_SOB_SourceNo.Clear();
            //单据状态
            cbWhere_SOB_StatusName.Items.Clear();
            //审核状态
            cbWhere_SOB_ApprovalStatusName.Items.Clear();
            //有效
            ckWhere_SOB_IsValid.Checked = true;
            ckWhere_SOB_IsValid.CheckState = CheckState.Checked;
            //给 单号 设置焦点
            lblWhere_SOB_No.Focus();

            #endregion 初始化下拉框

            #region 初始化下拉框



            #endregion

            #region Grid初始化

            //清空Grid
            HeadGridDS = new BindingList<StockOutBillManagerUIModel>();
            gdGrid.DataSource = HeadGridDS;
            gdGrid.DataBind();

            #endregion

            #endregion

            #region 初始化下拉框
            //来源类型【仅查询[销售出库]的出库单】
            var tempStockOutBillSourceTypeDs = _stockOutBillSourceTypeDs.Where(x => x.Text == StockOutBillSourceTypeEnum.Name.XSCK).ToList();
            cbWhere_SOB_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_SOB_SourceTypeName.ValueMember = SysConst.EN_Code;
            cbWhere_SOB_SourceTypeName.DataSource = tempStockOutBillSourceTypeDs;
            cbWhere_SOB_SourceTypeName.DataBind();

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

            //控制详情是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();
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
            //供应商
            mcbSOB_SUPP_Name.SelectedValue = HeadDS.SOB_SUPP_ID;
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
            var argsCondition = new StockOutBillManagerQCModel()
            {
                //查询用SqlId 
                SqlId = SQLID.PIS_StockOutBillManager_SQL_02,
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

            //[手工创建]的场合，验证供应商
            if (string.IsNullOrEmpty(cbSOB_SourceTypeName.Text.Trim())
                && (string.IsNullOrEmpty(mcbSOB_SUPP_Name.SelectedValue)
                || string.IsNullOrEmpty(mcbSOB_SUPP_Name.SelectedText)))
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
            HeadDS = new StockOutBillManagerUIModel()
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
        /// 处理领料明细Func
        /// </summary>
        /// <param name="paramPickPartsDetailList"></param>
        /// <returns></returns>
        private bool HandlePickPartsDetail(List<PickPartsQueryUIModel> paramPickPartsDetailList)
        {
            //将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();

            DateTime curDateTime = BLLCom.GetCurStdDatetime();

            #region 从[领料查询]中添加出库明细

            foreach (var loopPickPartsDetail in paramPickPartsDetailList)
            {
                if (string.IsNullOrEmpty(loopPickPartsDetail.INV_Name))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(loopPickPartsDetail.INV_ID))
                {
                    //当前组织不存在该配件
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.PIS_Inventory, MsgParam.ADD }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (loopPickPartsDetail.INV_Qty == 0)
                {
                    //库存为零，领料失败
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.PIS_Inventory, MsgParam.ADD }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                //条码+批次相同，为同一个配件
                StockOutBillManagerDetailUIModel sameStockOutDetail = _detailGridDS.FirstOrDefault(tempStockOutDetail =>
                    tempStockOutDetail.SOBD_Barcode == loopPickPartsDetail.INV_Barcode && tempStockOutDetail.SOBD_BatchNo == loopPickPartsDetail.INV_BatchNo);

                //明细列表中不存在相同配件
                if (sameStockOutDetail == null)
                {
                    #region 为出库明细赋值
                    StockOutBillManagerDetailUIModel newAutoPartsStockOutDetail = new StockOutBillManagerDetailUIModel
                    {
                        IsChecked = false,
                        INV_ID = loopPickPartsDetail.INV_ID,
                        INV_Qty = loopPickPartsDetail.INV_Qty,
                        INV_SUPP_ID = loopPickPartsDetail.INV_SUPP_ID,
                        SUPP_Name = loopPickPartsDetail.SUPP_Name,
                        Tmp_SOBD_ID = Guid.NewGuid().ToString(),

                        SOBD_ID = null,
                        SOBD_SOB_ID = HeadDS.SOB_ID,
                        SOBD_SOB_No = HeadDS.SOB_No,
                        SOBD_Name = loopPickPartsDetail.INV_Name,
                        SOBD_ThirdNo = loopPickPartsDetail.INV_ThirdNo,
                        SOBD_OEMNo = loopPickPartsDetail.INV_OEMNo,
                        SOBD_Barcode = loopPickPartsDetail.INV_Barcode,
                        SOBD_BatchNo = loopPickPartsDetail.INV_BatchNo,
                        SOBD_Specification = loopPickPartsDetail.INV_Specification,
                        SOBD_UOM = loopPickPartsDetail.APA_UOM,
                        SOBD_UnitCostPrice = loopPickPartsDetail.INV_PurchaseUnitPrice,
                        SOBD_Qty = loopPickPartsDetail.INV_Qty > 1 ? 1 : loopPickPartsDetail.INV_Qty,
                        SOBD_WH_ID = loopPickPartsDetail.INV_WH_ID,
                        WH_Name = loopPickPartsDetail.WH_Name,
                        SOBD_WHB_ID = loopPickPartsDetail.INV_WHB_ID,
                        WHB_Name = loopPickPartsDetail.WHB_Name,

                        APA_Brand = loopPickPartsDetail.APA_Brand,
                        APA_Level = loopPickPartsDetail.APA_Level,
                        APA_VehicleBrand = loopPickPartsDetail.APA_VehicleBrand,
                        APA_VehicleInspire = loopPickPartsDetail.APA_VehicleInspire,
                        APA_VehicleCapacity = loopPickPartsDetail.APA_VehicleCapacity,
                        APA_VehicleGearboxTypeCode = loopPickPartsDetail.APA_VehicleGearboxTypeCode,
                        APA_VehicleGearboxTypeName = loopPickPartsDetail.APA_VehicleGearboxTypeName,
                        APA_VehicleYearModel = loopPickPartsDetail.APA_VehicleYearModel,

                        SOBD_IsValid = true,
                        SOBD_CreatedBy = LoginInfoDAX.UserName,
                        SOBD_CreatedTime = curDateTime,
                        SOBD_UpdatedBy = LoginInfoDAX.UserName,
                        SOBD_UpdatedTime = curDateTime
                    };
                    //计算金额： 出库金额 = 出库成本 * 数量
                    newAutoPartsStockOutDetail.SOBD_Amount = Math.Round((newAutoPartsStockOutDetail.SOBD_UnitCostPrice ?? 0) * (newAutoPartsStockOutDetail.SOBD_Qty ?? 0), 2);
                    #endregion
                    _detailGridDS.Add(newAutoPartsStockOutDetail);
                }
                //明细列表中已存在相同配件
                else
                {
                    decimal? tempQty = (sameStockOutDetail.SOBD_Qty ?? 0) + (loopPickPartsDetail.INV_Qty > 1 ? 1 : loopPickPartsDetail.INV_Qty);
                    sameStockOutDetail.SOBD_Qty = tempQty > loopPickPartsDetail.INV_Qty ? loopPickPartsDetail.INV_Qty : tempQty;
                    sameStockOutDetail.SOBD_UnitCostPrice = loopPickPartsDetail.INV_PurchaseUnitPrice;
                    sameStockOutDetail.SOBD_Amount = Math.Round((sameStockOutDetail.SOBD_Qty ?? 0) * (sameStockOutDetail.SOBD_UnitCostPrice ?? 0), 2);
                }
            }

            #endregion

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置出库单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;

            return true;
        }

        /// <summary>
        /// 添加出库明细
        /// </summary>
        private void AddStockOutDetail()
        {
            #region 验证
            //来源单号为空时，供应商必选
            if (string.IsNullOrEmpty(txtSOB_SourceNo.Text.Trim())
                && (string.IsNullOrEmpty(mcbSOB_SUPP_Name.SelectedText)
                || string.IsNullOrEmpty(mcbSOB_SUPP_Name.SelectedValue)))
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

            #region 从[领料查询]中添加[出库单明细]

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //供应商ID
                {ComViewParamKey.SpecialSupplierID.ToString(),mcbSOB_SUPP_Name.SelectedValue},
                //供应商名称
                {ComViewParamKey.SpecialSupplierName.ToString(),mcbSOB_SUPP_Name.SelectedText},
                //配件领料Func
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

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置出库单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 更新出库明细
        /// </summary>
        private void UpdateStockOutDetail()
        {
            #region 验证

            if (!IsAllowUpdateDetailGrid())
            {
                return;
            }

            //验证出库单的审核状态，[已审核]的出库单不能更新明细
            if (cbSOB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableEnums.Name.PIS_StockOutBill + ApprovalStatusEnum.Name.YSH, MsgParam.UPDATE + SystemTableEnums.Name.PIS_StockOutBillDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //[来源类型]为[销售出库]的出库单不能更新明细
            if (cbSOB_SourceTypeName.Text == StockOutBillSourceTypeEnum.Name.XSCK)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableEnums.Name.PIS_StockOutBill + MsgParam.OF + MsgParam.SOURCE_TYPE + MsgParam.BE + StockOutBillSourceTypeEnum.Name.XSCK, MsgParam.UPDATE + SystemTableEnums.Name.PIS_StockOutBillDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var activeRowIndex = gdDetail.ActiveRow.Index;
            //判断出库单明细Grid内[唯一标识]是否为空
            if ((gdDetail.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_ID].Value == null || string.IsNullOrEmpty(gdDetail.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_ID].Value.ToString()))
                &&
                (gdDetail.Rows[activeRowIndex].Cells["Tmp_SOBD_ID"].Value == null ||
                string.IsNullOrEmpty(gdDetail.Rows[activeRowIndex].Cells["Tmp_SOBD_ID"].Value.ToString()))
                )
            {
                return;
            }
            //待更新明细的ID
            string tempDetailId = string.Empty;
            if (gdDetail.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_ID] != null
                && gdDetail.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_ID].Value != null)
            {
                tempDetailId = gdDetail.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_ID].Value.ToString();
            }
            else if (gdDetail.Rows[activeRowIndex].Cells["Tmp_SOBD_ID"] != null
                && gdDetail.Rows[activeRowIndex].Cells["Tmp_SOBD_ID"].Value != null)
            {
                tempDetailId = gdDetail.Rows[activeRowIndex].Cells["Tmp_SOBD_ID"].Value.ToString();
            }
            //待更新的出库单明细
            StockOutBillManagerDetailUIModel stockOutBillToUpdateDetail = _detailGridDS.FirstOrDefault(x => (!string.IsNullOrEmpty(x.SOBD_ID) && x.SOBD_ID == tempDetailId) || (!string.IsNullOrEmpty(x.Tmp_SOBD_ID) && x.Tmp_SOBD_ID == tempDetailId));

            #endregion

            #region 从[领料查询]中添加[出库单明细]

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //供应商ID
                {ComViewParamKey.SpecialSupplierID.ToString(),mcbSOB_SUPP_Name.SelectedValue},
                //供应商名称
                {ComViewParamKey.SpecialSupplierName.ToString(),mcbSOB_SUPP_Name.SelectedText},
                //配件领料Func
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

            //移除当前行
            _detailGridDS.Remove(stockOutBillToUpdateDetail);

            //根据[是否存在明细]控制单头是否可编辑
            SetDetailByIsExistDetail();

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置出库单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

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
            if (cbSOB_SourceTypeName.Text == StockOutBillSourceTypeEnum.Name.XSCK)
            {
                #region 出库单.[来源类型]为{销售出库}的场合

                //显示[来源单号]
                lblSOB_SourceNo.Visible = true;
                txtSOB_SourceNo.Visible = true;

                //只能查看不能编辑详情
                //单头
                txtSOB_SourceNo.Enabled = false;
                txtSOB_Remark.Enabled = false;

                //明细列表不可添加、删除、更新
                toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedProps.Enabled = false;
                toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = false;
                //明细列表中[选择]、[配件名称]、[进货单价]和[出库数量]列不可编辑
                gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].CellActivation = Activation.ActivateOnly;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_Name]
                    .CellActivation = Activation.ActivateOnly;
                gdDetail.DisplayLayout.Bands[0].Columns[
                    SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_UnitCostPrice].CellActivation =
                    Activation.ActivateOnly;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_Qty]
                    .CellActivation = Activation.ActivateOnly;

                #endregion
            }
            else
            {
                #region 出库单.[来源类型]为{手工创建}的场合

                //隐藏[来源单号]
                lblSOB_SourceNo.Visible = false;
                txtSOB_SourceNo.Visible = false;

                if (cbSOB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
                {
                    #region 出库单.[审核状态]为{已审核}的场合

                    //单头
                    txtSOB_SourceNo.Enabled = false;
                    txtSOB_Remark.Enabled = false;

                    //明细列表不可添加、删除、更新
                    toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedProps.Enabled = false;
                    toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = false;
                    //明细列表中[选择]、[配件名称]、[进货单价]和[出库数量]列不可编辑
                    gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].CellActivation = Activation.ActivateOnly;
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_Name
                        ].CellActivation = Activation.ActivateOnly;
                    gdDetail.DisplayLayout.Bands[0].Columns[
                        SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_UnitCostPrice].CellActivation =
                        Activation.ActivateOnly;
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_Qty]
                        .CellActivation = Activation.ActivateOnly;

                    #endregion
                }
                else
                {
                    #region 出库单未保存 或出库单.[审核状态]为{待审核}的场合

                    //根据[是否存在明细]控制单头是否可编辑
                    SetDetailByIsExistDetail();
                    txtSOB_Remark.Enabled = true;

                    //明细列表可删除、更新
                    toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedProps.Enabled = true;
                    toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = true;

                    //明细列表中[选择]、[进货单价]和[出库数量]列可编辑
                    gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].CellActivation = Activation.AllowEdit;
                    gdDetail.DisplayLayout.Bands[0].Columns[
                        SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_UnitCostPrice].CellActivation =
                        Activation.AllowEdit;
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.PIS_StockOutBillDetail.Code.SOBD_Qty]
                        .CellActivation = Activation.AllowEdit;

                    #endregion
                }

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
                //[来源类型]为[销售出库]的场合，出库单不能[保存]、[删除]、[审核]、[反审核]、[转结算]
                SetActionEnable(SystemActionEnum.Code.SAVE, false);
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                SetActionEnable(SystemActionEnum.Code.APPROVE, false);
                SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);
                //SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, false);
            }
            else
            {
                if (cbSOB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
                {
                    //[审核状态]为[已审核]的场合，[保存]、[删除]、[审核]、[转结算]不可用
                    SetActionEnable(SystemActionEnum.Code.SAVE, false);
                    SetActionEnable(SystemActionEnum.Code.DELETE, false);
                    SetActionEnable(SystemActionEnum.Code.APPROVE, false);
                    SetActionEnable(SystemActionEnum.Code.UNAPPROVE, true);
                    SetActionEnable(SystemActionEnum.Code.PRINT, true);
                    //SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, true);
                }
                else
                {
                    //新增或[审核状态]为[待审核]的场合，[反审核]、[打印]、[转结算]不可用
                    SetActionEnable(SystemActionEnum.Code.SAVE, true);
                    SetActionEnable(SystemActionEnum.Code.DELETE, !string.IsNullOrEmpty(txtSOB_ID.Text));
                    SetActionEnable(SystemActionEnum.Code.APPROVE, !string.IsNullOrEmpty(txtSOB_ID.Text));
                    SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);
                    SetActionEnable(SystemActionEnum.Code.PRINT, false);
                    //SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, false);
                }
            }
        }

        /// <summary>
        /// 设置单元格样式
        /// </summary>
        private void SetStockOutBillStyle()
        {
            #region 设置Grid数据颜色
            gdDetail.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;

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
                mcbSOB_SUPP_Name.Enabled = true;
            }
            else
            {
                txtSOB_SourceNo.Enabled = false;
                mcbSOB_SUPP_Name.Enabled = false;
            }
        }

        /// <summary>
        /// 是否允许更新[出库单明细]列表的数据
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
