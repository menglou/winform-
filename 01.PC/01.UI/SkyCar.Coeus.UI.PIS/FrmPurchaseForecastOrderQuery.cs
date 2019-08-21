using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.PIS;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.PIS;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.Common.Message;

namespace SkyCar.Coeus.UI.PIS
{
    /// <summary>
    /// 采购补货建议查询
    /// </summary>
    public partial class FrmPurchaseForecastOrderQuery : BaseFormCardListDetail<PurchaseForecastOrderQueryUIModel, PurchaseForecastOrderQueryQCModel, MDLPIS_PurchaseForecastOrder>
    {
        #region 全局变量

        /// <summary>
        /// 采购补货建议查询BLL
        /// </summary>
        private PurchaseForecastOrderQueryBLL _bll = new PurchaseForecastOrderQueryBLL();
        /// <summary>
        /// 【详情】Tab内Grid绑定用的数据源
        /// </summary>
        private SkyCarBindingList<PurchaseForecastOrderQueryDetailUIModel, MDLPIS_PurchaseForecastOrderDetail> _detailGridDS = new SkyCarBindingList<PurchaseForecastOrderQueryDetailUIModel, MDLPIS_PurchaseForecastOrderDetail>();
        /// <summary>
        /// 列表选择模式，默认单选，多选时可以选择多项。
        /// </summary>
        private CustomEnums.CustomeSelectionMode ListSelectionMode { get; set; }

        #region 下拉框数据源
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
        /// FrmPurchaseForecastOrderQuery构造方法
        /// </summary>
        public FrmPurchaseForecastOrderQuery()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmPurchaseForecastOrderQuery_Load(object sender, EventArgs e)
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
            //设置删除按钮是否可用
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
            }
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
        /// 选中的Tab改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlFull_SelectedTabChanging(object sender, SelectedTabChangingEventArgs e)
        {
            base.SetActionEnableBySelectedTab(e.Tab.Key);
            //设置删除按钮是否可用
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                SetActionEnable(SystemActionEnum.Code.DELETE, !string.IsNullOrEmpty(txtPFO_ID.Text));
            }
            else
            {
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
            }
        }

        #region 查询条件相关事件

        /// <summary>
        /// 单号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_PFO_No_KeyDown(object sender, KeyEventArgs e)
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
        private void cbWhere_PFO_SourceTypeName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 单据状态名称ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_PFO_StatusName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 有效CheckedChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckWhere_PFO_IsValid_CheckedChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        #endregion

        #region 【列表】Grid相关事件

        /// <summary>
        /// 单元格改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_CellChange(object sender, CellEventArgs e)
        {
            gdGrid.UpdateData();
        }
        #endregion
        #endregion

        #region 重写基类方法

        /// <summary>
        /// 新增
        /// </summary>
        public override void NewAction()
        {
            //1.执行基类方法
            base.NewAction();
            //2.初始化【详情】Tab内控件
            InitializeDetailTabControls();
            //3.设置【详情】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;
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
            //2.将【详情】Tab内控件的值赋值给基类的HeadDS
            SetCardCtrlsToDetailDS();
            //3.执行保存（含服务端检查）
            if (!_bll.SaveDetailDS(base.HeadDS, _detailGridDS))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg);
                return;
            }
            //4.开始监控List变化
            _detailGridDS.StartMonitChanges();
            //5.将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// 复制
        /// </summary>
        public override void CopyAction()
        {
            base.CopyAction();
            //ID
            txtPFO_ID.Clear();
            //版本号
            txtPFO_VersionNo.Clear();
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
            var argsDetail = new List<MDLPIS_PurchaseForecastOrderDetail>();
            //将HeadDS转换为TBModel对象
            var argsHead = base.HeadDS.ToTBModelForSaveAndDelete<MDLPIS_PurchaseForecastOrder>();
            //将当前DetailGridDS转换为指定类型的TBModelList
            _detailGridDS.ToTBModelListForUpdateAndDelete<MDLPIS_PurchaseForecastOrderDetail>(argsDetail);
            //过滤明细列表中未保存的数据
            argsDetail = argsDetail.Where(x => !string.IsNullOrEmpty(x.WHERE_PFOD_ID)).ToList();
            //2.执行删除
            if (!_bll.UnityDelete<MDLPIS_PurchaseForecastOrder, MDLPIS_PurchaseForecastOrderDetail>(argsHead, argsDetail))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            //3.清空【详情】画面数据
            InitializeDetailTabControls();
            //4.执行查询
            QueryAction();
        }
        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            //1.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
            //2.设置查询条件（翻页相关属性不用设置）
            base.ConditionDS = new PurchaseForecastOrderQueryQCModel()
            {
                SqlId = SQLID.PIS_PurchaseForecastOrderQuery_SQL_01,
                //单号
                WHERE_PFO_No = txtWhere_PFO_No.Text.Trim(),
                //供应商名称
                WHERE_PFO_SUPP_Name = mcbWhere_PFO_SUPP_Name.SelectedText,
                //来源类型名称
                WHERE_PFO_SourceTypeName = cbWhere_PFO_SourceTypeName.Text,
                //单据状态名称
                WHERE_PFO_StatusName = cbWhere_PFO_StatusName.Text,
                //有效
                WHERE_PFO_IsValid = ckWhere_PFO_IsValid.Checked,
                //组织ID
                WHERE_PFO_Org_ID = LoginInfoDAX.OrgID
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
        /// 转采购
        /// </summary>
        public override void ToPurchaseOrderNavigate()
        {
            base.ToPurchaseOrderNavigate();
            //获取采购预测数据
            PurchaseForecastOrderQueryUIModel curPurchaseForecastOrderToPurchaseOrder = new PurchaseForecastOrderQueryUIModel();

            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                var purchaseForecastOrderList = HeadGridDS.Where(x => x.IsChecked == true).ToList();
                if (purchaseForecastOrderList.Count == 1)
                {
                    curPurchaseForecastOrderToPurchaseOrder = purchaseForecastOrderList[0];
                }
            }
            else if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                _bll.CopyModel(HeadDS, curPurchaseForecastOrderToPurchaseOrder);
            }

            if (string.IsNullOrEmpty(curPurchaseForecastOrderToPurchaseOrder.PFO_ID)
                || string.IsNullOrEmpty(curPurchaseForecastOrderToPurchaseOrder.PFO_No))
            {
                //没有获取到采购预测，转采购失败
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.PIS_PurchaseForecastOrder, SystemNavigateEnum.Name.TOPURCHASEORDER }), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //传入采购单数据
            MDLPIS_PurchaseOrder argsPurchaseOrder = new MDLPIS_PurchaseOrder();
            //传入采购单明细数据
            List<MDLPIS_PurchaseOrderDetail> argsMdlpisPurchaseOrderDetailList = new List<MDLPIS_PurchaseOrderDetail>();

            #region 准备[采购单]数据

            argsPurchaseOrder.PO_Org_ID = LoginInfoDAX.OrgID;
            argsPurchaseOrder.PO_SUPP_ID = curPurchaseForecastOrderToPurchaseOrder.PFO_SUPP_ID;
            argsPurchaseOrder.PO_SUPP_Name = curPurchaseForecastOrderToPurchaseOrder.PFO_SUPP_Name;
            argsPurchaseOrder.PO_SourceTypeCode = PurchaseOrderSourceTypeEnum.Code.CGYC;
            argsPurchaseOrder.PO_SourceTypeName = PurchaseOrderSourceTypeEnum.Name.CGYC;
            argsPurchaseOrder.PO_SourceNo = curPurchaseForecastOrderToPurchaseOrder.PFO_No;
            argsPurchaseOrder.PO_TotalAmount = curPurchaseForecastOrderToPurchaseOrder.PFO_TotalAmount;

            #endregion

            #region 准备[采购单明细]数据
            List<PurchaseForecastOrderQueryDetailUIModel> purchaseForecastOrderQueryDetailList = new List<PurchaseForecastOrderQueryDetailUIModel>();
            MDLPIS_PurchaseForecastOrderDetail purchaseForecastOrder = new MDLPIS_PurchaseForecastOrderDetail()
            {
                WHERE_PFOD_PFO_ID = curPurchaseForecastOrderToPurchaseOrder.PFO_ID,
            };
            _bll.QueryForList<MDLPIS_PurchaseForecastOrderDetail, PurchaseForecastOrderQueryDetailUIModel>(purchaseForecastOrder, purchaseForecastOrderQueryDetailList);
            foreach (var loopPurchaseForecastOrder in purchaseForecastOrderQueryDetailList)
            {
                MDLPIS_PurchaseOrderDetail purchaseOrderDetail = new MDLPIS_PurchaseOrderDetail();
                purchaseOrderDetail.POD_AutoPartsBarcode = loopPurchaseForecastOrder.PFOD_AutoPartsBarcode;
                purchaseOrderDetail.POD_ThirdCode = loopPurchaseForecastOrder.PFOD_ThirdCode;
                purchaseOrderDetail.POD_OEMCode = loopPurchaseForecastOrder.PFOD_OEMCode;
                purchaseOrderDetail.POD_AutoPartsName = loopPurchaseForecastOrder.PFOD_AutoPartsName;
                purchaseOrderDetail.POD_AutoPartsBrand = loopPurchaseForecastOrder.PFOD_AutoPartsBrand;
                purchaseOrderDetail.POD_AutoPartsSpec = loopPurchaseForecastOrder.PFOD_AutoPartsSpec;
                purchaseOrderDetail.POD_AutoPartsLevel = loopPurchaseForecastOrder.PFOD_AutoPartsLevel;
                purchaseOrderDetail.POD_UOM = loopPurchaseForecastOrder.PFOD_UOM;
                purchaseOrderDetail.POD_VehicleBrand = loopPurchaseForecastOrder.PFOD_VehicleBrand;
                purchaseOrderDetail.POD_VehicleInspire = loopPurchaseForecastOrder.PFOD_VehicleInspire;
                purchaseOrderDetail.POD_VehicleCapacity = loopPurchaseForecastOrder.PFOD_VehicleCapacity;
                purchaseOrderDetail.POD_VehicleYearModel = loopPurchaseForecastOrder.PFOD_VehicleYearModel;
                purchaseOrderDetail.POD_VehicleGearboxType = loopPurchaseForecastOrder.PFOD_VehicleGearboxType;
                purchaseOrderDetail.POD_OrderQty = loopPurchaseForecastOrder.PFOD_Qty;
                purchaseOrderDetail.POD_UnitPrice = loopPurchaseForecastOrder.PFOD_LastUnitPrice;
                argsMdlpisPurchaseOrderDetailList.Add(purchaseOrderDetail);
            }
            #endregion

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                {PISViewParamKey.PurchaseOrder.ToString(), argsPurchaseOrder},
                {PISViewParamKey.PurchaseOrderDetail.ToString(), argsMdlpisPurchaseOrderDetailList},
            };

            //跳转到[采购单管理]
            SystemFunction.ShowViewFromView(MsgParam.PURCHASEORDER_MANAGER, ViewClassFullNameConst.PIS_FrmPurchaseOrderManager, true, PageDisplayMode.TabPage, paramViewParameters, null);

        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {

            //单号
            txtPFO_No.Clear();
            //供应商名称
            txtPFO_SUPP_Name.Clear();
            //来源类型
            cbPFO_SourceTypeName.Items.Clear();
            //订单总额
            txtPFO_TotalAmount.Clear();
            //单据状态
            cbPFO_StatusName.Items.Clear();
            //有效
            ckPFO_IsValid.Checked = true;
            ckPFO_IsValid.CheckState = System.Windows.Forms.CheckState.Checked;
            //创建人
            txtPFO_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtPFO_CreatedTime.Value = DateTime.Now;
            //修改人
            txtPFO_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtPFO_UpdatedTime.Value = DateTime.Now;
            //采购预测订单ID
            txtPFO_ID.Clear();
            //组织ID
            txtPFO_Org_ID.Clear();
            txtPFO_Org_ID.Text = LoginInfoDAX.OrgID;
            //供应商ID
            txtPFO_SUPP_ID.Clear();
            //来源类型编码
            txtPFO_SourceTypeCode.Clear();
            //单据状态编码
            txtPFO_StatusCode.Clear();
            //版本号
            txtPFO_VersionNo.Clear();
            //给 单号 设置焦点
            lblPFO_No.Focus();
            //清空[明细]Grid
            _detailGridDS = new SkyCarBindingList<PurchaseForecastOrderQueryDetailUIModel, MDLPIS_PurchaseForecastOrderDetail>();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();

        }
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //单号
            txtWhere_PFO_No.Clear();
            //供应商名称
            mcbWhere_PFO_SUPP_Name.Clear();
            //来源类型名称
            cbWhere_PFO_SourceTypeName.Items.Clear();
            //单据状态名称
            cbWhere_PFO_StatusName.Items.Clear();
            //有效
            ckWhere_PFO_IsValid.Checked = true;
            ckWhere_PFO_IsValid.CheckState = CheckState.Checked;
            //给 单号 设置焦点
            lblWhere_PFO_No.Focus();
            #endregion

            #region 初始化下拉框

            //供应商
            _supplierList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsSupplier) as List<MDLPIS_Supplier>;
            mcbWhere_PFO_SUPP_Name.DisplayMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_Name;
            mcbWhere_PFO_SUPP_Name.ValueMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_ID;
            mcbWhere_PFO_SUPP_Name.DataSource = _supplierList;

            #endregion

            SetToComboEditor();

            #region Grid初始化

            base.HeadGridDS = new BindingList<PurchaseForecastOrderQueryUIModel>();
            gdGrid.DataSource = base.HeadGridDS;
            gdGrid.DataBind();

            #endregion

            #endregion

            //给下拉框复制

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
            var activeRowIndex = gdGrid.ActiveRow.Index;
            //判断Grid内[唯一标识]是否为空
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_PurchaseForecastOrder.Code.PFO_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_PurchaseForecastOrder.Code.PFO_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[HeadDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            base.HeadDS = base.HeadGridDS.FirstOrDefault(x => x.PFO_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_PurchaseForecastOrder.Code.PFO_ID].Value);
            //将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //查询明细Grid数据并绑定
            QueryDetail();
            //选中【详情】Tab
            tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;
        }
        /// <summary>
        /// 将HeadDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {

            //单号
            txtPFO_No.Text = base.HeadDS.PFO_No;
            //供应商名称
            txtPFO_SUPP_Name.Text = base.HeadDS.PFO_SUPP_Name;
            //来源类型
            cbPFO_SourceTypeName.Text = base.HeadDS.PFO_SourceTypeName;
            //订单总额
            txtPFO_TotalAmount.Value = base.HeadDS.PFO_TotalAmount;
            //单据状态
            cbPFO_StatusName.Text = base.HeadDS.PFO_StatusName;
            //有效
            if (base.HeadDS.PFO_IsValid != null)
            {
                ckPFO_IsValid.Checked = base.HeadDS.PFO_IsValid.Value;
            }
            //创建人
            txtPFO_CreatedBy.Text = base.HeadDS.PFO_CreatedBy;
            //创建时间
            dtPFO_CreatedTime.Value = base.HeadDS.PFO_CreatedTime;
            //修改人
            txtPFO_UpdatedBy.Text = base.HeadDS.PFO_UpdatedBy;
            //修改时间
            dtPFO_UpdatedTime.Value = base.HeadDS.PFO_UpdatedTime;
            //采购预测订单ID
            txtPFO_ID.Text = base.HeadDS.PFO_ID;
            //组织ID
            txtPFO_Org_ID.Text = base.HeadDS.PFO_Org_ID;
            //供应商ID
            txtPFO_SUPP_ID.Text = base.HeadDS.PFO_SUPP_ID;
            //来源类型编码
            txtPFO_SourceTypeCode.Text = base.HeadDS.PFO_SourceTypeCode;
            //单据状态编码
            txtPFO_StatusCode.Text = base.HeadDS.PFO_StatusCode;
            //版本号
            txtPFO_VersionNo.Value = base.HeadDS.PFO_VersionNo;

        }
        /// <summary>
        /// 查询明细Grid数据并绑定
        /// </summary>
        private void QueryDetail()
        {
            //1.设置查询条件
            var argsCondition = new PurchaseForecastOrderQueryQCModel()
            {
                //查询用SqlId 
                SqlId = SQLID.PIS_PurchaseForecastOrderQuery_SQL_02,
                //采购预测订单ID
                WHERE_PFOD_PFO_ID = txtPFO_ID.Text.Trim(),
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
            return true;
        }
        /// <summary>
        /// 前端检查-删除
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForDelete()
        {
            if (cbPFO_StatusName.Text == PurchaseForecastOrderStatusEnum.Name.YZCG)
            {
                MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                { PurchaseForecastOrderStatusEnum.Name.YZCG, SystemActionEnum.Name.DELETE }),
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的HeadDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {

            base.HeadDS = new PurchaseForecastOrderQueryUIModel()
            {
                //单号
                PFO_No = txtPFO_No.Text.Trim(),
                //供应商名称
                PFO_SUPP_Name = txtPFO_SUPP_Name.Text.Trim(),
                //来源类型
                PFO_SourceTypeName = cbPFO_SourceTypeName.Value?.ToString() ?? "",
                //订单总额
                PFO_TotalAmount = Convert.ToDecimal(txtPFO_TotalAmount.Text.Trim() == "" ? "1" : txtPFO_TotalAmount.Text.Trim()),
                //单据状态
                PFO_StatusName = cbPFO_StatusName.Value?.ToString() ?? "",
                //有效
                PFO_IsValid = ckPFO_IsValid.Checked,
                //创建人
                PFO_CreatedBy = txtPFO_CreatedBy.Text.Trim(),
                //创建时间
                PFO_CreatedTime = (DateTime?)dtPFO_CreatedTime.Value ?? DateTime.Now,
                //修改人
                PFO_UpdatedBy = txtPFO_UpdatedBy.Text.Trim(),
                //修改时间
                PFO_UpdatedTime = (DateTime?)dtPFO_UpdatedTime.Value ?? DateTime.Now,
                //采购预测订单ID
                PFO_ID = txtPFO_ID.Text.Trim(),
                //组织ID
                PFO_Org_ID = txtPFO_Org_ID.Text.Trim(),
                //供应商ID
                PFO_SUPP_ID = txtPFO_SUPP_ID.Text.Trim(),
                //来源类型编码
                PFO_SourceTypeCode = txtPFO_SourceTypeCode.Text.Trim(),
                //单据状态编码
                PFO_StatusCode = txtPFO_StatusCode.Text.Trim(),
                //版本号
                PFO_VersionNo = Convert.ToInt64(txtPFO_VersionNo.Text.Trim() == "" ? "1" : txtPFO_VersionNo.Text.Trim()),
            };
        }

        /// <summary>
        /// 给下拉框赋值
        /// </summary>
        private void SetToComboEditor()
        {
            //采购预测订单来源类型【安全库存备货】
            List<ComComboBoxDataSourceTC> resultPurchaseForecastOrderSourceTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.PurchaseForecastOrderSourceType);
            cbWhere_PFO_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_PFO_SourceTypeName.ValueMember = SysConst.EN_Code;
            cbWhere_PFO_SourceTypeName.DataSource = resultPurchaseForecastOrderSourceTypeList;

            cbWhere_PFO_SourceTypeName.DataBind();

            cbPFO_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
            cbPFO_SourceTypeName.ValueMember = SysConst.EN_Code;
            cbPFO_SourceTypeName.DataSource = resultPurchaseForecastOrderSourceTypeList;
            cbPFO_SourceTypeName.Text = PurchaseForecastOrderSourceTypeEnum.Name.AQKCBH;
            cbPFO_SourceTypeName.Value = PurchaseForecastOrderSourceTypeEnum.Code.AQKCBH;
            cbPFO_SourceTypeName.DataBind();

            List<ComComboBoxDataSourceTC> resultPurchaseForecastOrderStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.PurchaseForecastOrderStatus);
            cbWhere_PFO_StatusName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_PFO_StatusName.ValueMember = SysConst.EN_Code;
            cbWhere_PFO_StatusName.DataSource = resultPurchaseForecastOrderStatusList;
            cbWhere_PFO_StatusName.DataBind();

            cbPFO_StatusName.DisplayMember = SysConst.EN_TEXT;
            cbPFO_StatusName.ValueMember = SysConst.EN_Code;
            cbPFO_StatusName.DataSource = resultPurchaseForecastOrderStatusList;
            cbPFO_StatusName.Text = PurchaseForecastOrderStatusEnum.Name.YSC;
            cbPFO_StatusName.Value = PurchaseForecastOrderStatusEnum.Code.YSC;
            cbPFO_StatusName.DataBind();

        }
        #endregion
    }
}
