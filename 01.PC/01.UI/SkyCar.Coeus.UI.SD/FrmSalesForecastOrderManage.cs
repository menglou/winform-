using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.SD;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.SD;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.UIModel.Common.UIModel;
using SkyCar.Coeus.UIModel.SD.UIModel;

namespace SkyCar.Coeus.UI.SD
{
    /// <summary>
    /// 销售补货建议查询
    /// </summary>
    public partial class FrmSalesForecastOrderManage : BaseFormCardListDetail<SalesForecastOrderManageUIModel, SalesForecastOrderManageQCModel, MDLSD_SalesForecastOrder>
    {
        #region 全局变量

        /// <summary>
        /// 销售补货建议查询BLL
        /// </summary>
        private SalesForecastOrderManageBLL _bll = new SalesForecastOrderManageBLL();
        /// <summary>
        /// 销售预测订单明细数据源
        /// </summary>
        private List<SalesForecastOrderDetailUIModel> _detailGridDS = new List<SalesForecastOrderDetailUIModel>();

        #region 下拉框数据源

        /// <summary>
        /// 汽修商户
        /// </summary>
        List<MDLPIS_AutoFactoryCustomer> _autoFactoryList = new List<MDLPIS_AutoFactoryCustomer>();

        /// <summary>
        /// 汽修商户组织
        /// </summary>
        List<CustomerQueryUIModel> _autoFactoryOrgList = new List<CustomerQueryUIModel>();

        #endregion

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmSalesForecastOrderManage构造方法
        /// </summary>
        public FrmSalesForecastOrderManage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSalesForecastOrderManage_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（动作，导航）
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
            //根据选中的Tab，设置动作按钮[是否可用]（在系统权限的基础上进行控制）
            base.SetActionEnableBySelectedTab(SysConst.EN_LIST);
            #endregion

            //[列表]页不允许删除
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
            //[列表]页不允许删除
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
            }
            else
            {
                SetActionEnable(SystemActionEnum.Code.DELETE, true);
            }
        }
        #endregion

        #region 查询条件相关事件

        /// <summary>
        /// 单据编号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_SFO_No_KeyDown(object sender, KeyEventArgs e)
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
        private void cbWhere_SFO_SourceTypeName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }

        /// <summary>
        /// 单据状态ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_SFO_StatusName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 创建时间开始ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_SFO_CreatedTime_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }

        /// <summary>
        /// 创建时间终了ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_CreatedTimeEnd_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            if (this.dtWhere_CreatedTimeEnd.Value != null &&
              this.dtWhere_CreatedTimeEnd.DateTime.Hour == 0 &&
              this.dtWhere_CreatedTimeEnd.DateTime.Minute == 0 &&
              this.dtWhere_CreatedTimeEnd.DateTime.Second == 0)
            {
                DateTime newDateTime = new DateTime(this.dtWhere_CreatedTimeEnd.DateTime.Year, this.dtWhere_CreatedTimeEnd.DateTime.Month, this.dtWhere_CreatedTimeEnd.DateTime.Day, 23, 59, 59);
                this.dtWhere_CreatedTimeEnd.DateTime = newDateTime;
            }
        }
        /// <summary>
        /// 汽修商户改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbWhere_SFO_AutoFactoryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(mcbWhere_SFO_AutoFactoryName.SelectedText)
                || string.IsNullOrEmpty(mcbWhere_SFO_AutoFactoryName.SelectedValue))
            {
                mcbWhere_SFO_CustomerName.DataSource = _autoFactoryOrgList;
            }
            else
            {
                var tempAutoFactoryOrgList =
                    _autoFactoryOrgList.Where(x => x.AutoFactoryCode == mcbWhere_SFO_AutoFactoryName.SelectedValue).ToList();
                mcbWhere_SFO_CustomerName.DataSource = tempAutoFactoryOrgList;
            }
        }
        #endregion

        #endregion

        #region 重写基类方法

        /// <summary>
        /// 删除
        /// </summary>
        public override void DeleteAction()
        {
            #region 验证

            //销售预测订单为空，不能删除
            if (string.IsNullOrEmpty(txtSFO_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.W_0016, new object[] { SystemTableEnums.Name.SD_SalesForecastOrder, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //单据已转销售，不能删除
            if (txtSFO_StatusName.Text == SalesForecastOrderStatusEnum.Name.YZXS)
            {
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableEnums.Name.SD_SalesForecastOrder + SalesForecastOrderStatusEnum.Name.YZXS, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            #endregion
            //确认删除操作
            var isDelete = MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.W_0012, new object[] { }),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isDelete != DialogResult.OK)
            {
                return;
            }
            bool isDeleteSuccess = _bll.DeleteSalesForecastOrder(txtSFO_ID.Text.Trim());
            if (!isDeleteSuccess)
            {
                //删除失败
                MessageBoxs.Show(Trans.SD, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //删除成功
            MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            //3.清空【详情】画面数据
            InitializeDetailTabControls();
            //4.执行查询，以更新Grid数据源
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
            base.ConditionDS = new SalesForecastOrderManageQCModel()
            {
                //单据编号
                WHERE_SFO_No = txtWhere_SFO_No.Text.Trim(),
                //汽修商户名称
                WHERE_SFO_AutoFactoryName = mcbWhere_SFO_AutoFactoryName.SelectedText,
                //客户名称
                WHERE_SFO_CustomerName = mcbWhere_SFO_CustomerName.SelectedText,
                //来源类型
                WHERE_SFO_SourceTypeName = cbWhere_SFO_SourceTypeName.Text.Trim(),
                //单据状态
                WHERE_SFO_StatusName = cbWhere_SFO_StatusName.Text.Trim(),
                //组织
                WHERE_SFO_Org_ID = LoginInfoDAX.OrgID,
            };
            if (dtWhere_CreatedTimeStart.Value != null)
            {
                //创建时间-开始
                ConditionDS._CreatedTimeStart = dtWhere_CreatedTimeStart.DateTime;
            }
            if (dtWhere_CreatedTimeEnd.Value != null)
            {
                //创建时间-终了
                ConditionDS._CreatedTimeEnd = dtWhere_CreatedTimeEnd.DateTime;
            }
            //3.执行基类方法（注意：基类使用模糊查询）
            base.QueryAction();
            //4.Grid绑定数据源
            gdGrid.DataSource = HeadGridDS;
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

        #endregion

        #region 导航按钮

        /// <summary>
        /// 转销售
        /// </summary>
        public override void ToSalesOrderNavigate()
        {
            base.ToSalesOrderNavigate();

            #region 验证及准备数据

            //要跳转到销售的销售预测订单
            SalesForecastOrderManageUIModel curForecastOrderToSalesOrder = new SalesForecastOrderManageUIModel();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 选中[详情]Tab的场合

                //选中【详情】Tab的场合
                _bll.CopyModel(HeadDS, curForecastOrderToSalesOrder);
                #endregion
            }
            else
            {
                #region 选中[列表]Tab的场合

                var selectedForecastOrderList = HeadGridDS.Where(x => x.IsChecked == true).ToList();
                if (selectedForecastOrderList.Count == 1)
                {
                    if (selectedForecastOrderList[0].SFO_StatusName != SalesForecastOrderStatusEnum.Name.YSC)
                    {
                        //请选择已生成的销售预测订单转销售
                        MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { SalesForecastOrderStatusEnum.Name.YSC + MsgParam.OF + SystemTableEnums.Name.SD_SalesForecastOrder + SystemNavigateEnum.Name.TOSALESORDER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        selectedForecastOrderList[0].IsChecked = false;
                        gdGrid.DataSource = HeadGridDS;
                        gdGrid.DataBind();
                        return;
                    }
                    curForecastOrderToSalesOrder = selectedForecastOrderList[0];
                }
                else
                {
                    var tempCannotToSalesOrder = selectedForecastOrderList.Where(x => x.SFO_StatusName != SalesForecastOrderStatusEnum.Name.YSC).ToList();
                    //请选择一个已生成的销售预测订单转销售
                    MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ONE + SalesForecastOrderStatusEnum.Name.YSC + MsgParam.OF + SystemTableEnums.Name.SD_SalesForecastOrder + SystemNavigateEnum.Name.TOSALESORDER }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    foreach (var loopCannotToLogistic in tempCannotToSalesOrder)
                    {
                        loopCannotToLogistic.IsChecked = false;
                        gdGrid.DataSource = HeadGridDS;
                        gdGrid.DataBind();
                        return;
                    }
                    return;
                }

                ////查询销售预测订单明细
                //_bll.QueryForList<MDLSD_SalesForecastOrderDetail, SalesForecastOrderDetailUIModel>(new MDLSD_SalesForecastOrderDetail()
                //{
                //    WHERE_SFOD_IsValid = true,
                //    WHERE_SFOD_ST_ID = curForecastOrderToSalesOrder.SFO_ID
                //}, _detailGridDS);
                #endregion
            }

            if (string.IsNullOrEmpty(curForecastOrderToSalesOrder.SFO_ID)
                || string.IsNullOrEmpty(curForecastOrderToSalesOrder.SFO_No))
            {
                //没有获取到销售预测订单，转销售失败
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.SD_SalesForecastOrder, SystemNavigateEnum.Name.TOSALESORDER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #endregion

            //传入的销售订单
            MDLSD_SalesForecastOrder argsSalesOrder = new MDLSD_SalesForecastOrder();
            _bll.CopyModel(curForecastOrderToSalesOrder, argsSalesOrder);
            //#region 准备[销售订单]数据

            //argsSalesOrder.SO_Org_ID = LoginInfoDAX.OrgID;
            ////[来源类型]为{销售预测}
            //argsSalesOrder.SO_SourceTypeName = SalesOrderSourceTypeEnum.Name.XSYC;
            //argsSalesOrder.SO_SourceTypeCode = SalesOrderSourceTypeEnum.Code.XSYC;
            //argsSalesOrder.SO_SourceNo = curForecastOrderToSalesOrder.SFO_No;
            ////[客户类型]为{平台内汽修商}
            //argsSalesOrder.SO_CustomerTypeCode = CustomerTypeEnum.Code.PTNQXSH;
            //argsSalesOrder.SO_CustomerTypeName = CustomerTypeEnum.Name.PTNQXSH;
            //argsSalesOrder.SO_CustomerID = curForecastOrderToSalesOrder.SFO_CustomerID;
            //argsSalesOrder.SO_CustomerName = curForecastOrderToSalesOrder.SFO_CustomerName;
            ////[单据状态]为{已生成}
            //argsSalesOrder.SO_StatusName = SalesOrderStatusEnum.Name.YSC;
            //argsSalesOrder.SO_StatusCode = SalesOrderStatusEnum.Code.YSC;
            ////[审核状态]为{待审核}
            //argsSalesOrder.SO_ApprovalStatusName = ApprovalStatusEnum.Name.DSH;
            //argsSalesOrder.SO_ApprovalStatusCode = ApprovalStatusEnum.Code.DSH;
            //argsSalesOrder.SO_IsValid = true;
            //argsSalesOrder.SO_CreatedBy = LoginInfoDAX.UserName;
            //argsSalesOrder.SO_UpdatedBy = LoginInfoDAX.UserName;
            //argsSalesOrder.SO_CreatedTime = BLLCom.GetCurStdDatetime();
            //argsSalesOrder.SO_UpdatedTime = BLLCom.GetCurStdDatetime();
            //argsSalesOrder.SO_VersionNo = 1;
            //#endregion

            //#region 准备[销售订单明细]数据

            //decimal totalAmount = 0;
            //foreach (var loopDetail in _detailGridDS)
            //{
            //    if (string.IsNullOrEmpty(loopDetail.SFOD_ID))
            //    {
            //        continue;
            //    }
            //    MDLSD_SalesOrderDetail addSalesOrderDetail = new MDLSD_SalesOrderDetail
            //    {
            //        SOD_PriceIsIncludeTax = loopDetail.SFOD_PriceIsIncludeTax,
            //        SOD_TaxRate = loopDetail.SFOD_TaxRate,
            //        SOD_TotalTax = loopDetail.SFOD_TotalTax,

            //        SOD_Qty = loopDetail.SFOD_Qty,
            //        SOD_UnitPrice = loopDetail.SFOD_UnitPrice,
            //        SOD_TotalAmount = loopDetail.SFOD_TotalAmount,

            //        SOD_Barcode = loopDetail.SFOD_Barcode,
            //        SOD_Name = loopDetail.SFOD_Name,
            //        SOD_Specification = loopDetail.SFOD_Specification,
            //        SOD_UOM = loopDetail.SFOD_UOM,

            //        SOD_StockInOrgID = loopDetail.SFOD_AutoFactoryOrgID,
            //        SOD_StockInOrgName = loopDetail.SFOD_AutoFactoryOrgName,

            //        SOD_StatusName = argsSalesOrder.SO_StatusName,
            //        SOD_StatusCode = argsSalesOrder.SO_StatusCode,
            //        SOD_IsValid = true,
            //        SOD_CreatedBy = LoginInfoDAX.UserName,
            //        SOD_UpdatedBy = LoginInfoDAX.UserName,
            //        SOD_CreatedTime = BLLCom.GetCurStdDatetime(),
            //        SOD_UpdatedTime = BLLCom.GetCurStdDatetime(),
            //        SOD_VersionNo = 1
            //    };
            //    totalAmount += addSalesOrderDetail.SOD_TotalAmount ?? 0;
            //    argsSalesOrderDetailList.Add(addSalesOrderDetail);
            //}
            //argsSalesOrder.SO_TotalAmount = totalAmount;
            //#endregion

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //销售预测订单
                {SDViewParamKey.SalesForecastOrder.ToString(), argsSalesOrder},
                //销售订单明细
                //{SDViewParamKey.SalesOrderDetail.ToString(), argsSalesOrderDetailList},
            };

            //跳转到[主动销售管理]
            SystemFunction.ShowViewFromView(MsgParam.PROACTIVE_SALES_MANAGER, ViewClassFullNameConst.SD_FrmProactiveSalesManager, true, PageDisplayMode.TabPage, paramViewParameters, null);
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
            txtSFO_No.Clear();
            //客户名称
            txtSFO_CustomerName.Clear();
            //备注
            txtSFO_Remark.Clear();
            //有效
            ckSFO_IsValid.Checked = true;
            ckSFO_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtSFO_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtSFO_CreatedTime.Value = DateTime.Now;
            //修改人
            txtSFO_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtSFO_UpdatedTime.Value = DateTime.Now;
            //来源类型名称
            txtSFO_SourceTypeName.Clear();
            //单据状态名称
            txtSFO_StatusName.Clear();
            //销售预测订单ID
            txtSFO_ID.Clear();
            //组织ID
            txtSFO_Org_ID.Clear();
            //客户ID
            txtSFO_CustomerID.Clear();
            //版本号
            txtSFO_VersionNo.Clear();
            //给 单据编号 设置焦点
            lblSFO_No.Focus();

            //清空订单明细列表
            _detailGridDS = new List<SalesForecastOrderDetailUIModel>();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            #endregion
        }
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //单据编号
            txtWhere_SFO_No.Clear();
            //汽修商户组织名称
            mcbWhere_SFO_AutoFactoryName.Clear();
            //客户名称
            mcbWhere_SFO_CustomerName.Clear();
            //来源类型
            cbWhere_SFO_SourceTypeName.Clear();
            //单据状态
            cbWhere_SFO_StatusName.Clear();
            //创建时间-开始
            dtWhere_CreatedTimeStart.Value = null;
            //创建时间-终了
            dtWhere_CreatedTimeEnd.Value = null;
            //给 单据编号 设置焦点
            lblWhere_SFO_No.Focus();

            #endregion

            #region Grid初始化

            //清空Grid
            HeadGridDS = new BindingList<SalesForecastOrderManageUIModel>();
            gdGrid.DataSource = HeadGridDS;
            gdGrid.DataBind();

            #endregion

            #endregion

            #region 初始化下拉框

            //汽修商户
            _autoFactoryList = CacheDAX.Get(CacheDAX.ConfigDataKey.ARMerchant) as List<MDLPIS_AutoFactoryCustomer>;
            if (_autoFactoryList != null)
            {
                var tempAutoFactoryList = _autoFactoryList.Where(x => x.AFC_IsPlatform != null && x.AFC_IsPlatform.Value == true).ToList();
                mcbWhere_SFO_AutoFactoryName.DisplayMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Name;
                mcbWhere_SFO_AutoFactoryName.ValueMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Code;
                mcbWhere_SFO_AutoFactoryName.DataSource = tempAutoFactoryList;
            }

            //汽修商户组织
            var allCustomerList = CacheDAX.Get(CacheDAX.ConfigDataKey.Customer) as List<CustomerQueryUIModel>;
            if (allCustomerList != null)
            {
                _autoFactoryOrgList = allCustomerList.Where(x => x.CustomerType == CustomerTypeEnum.Name.PTNQXSH).ToList();

                mcbWhere_SFO_CustomerName.DisplayMember = "CustomerName";
                mcbWhere_SFO_CustomerName.ValueMember = "AutoFactoryOrgInfo";
                mcbWhere_SFO_CustomerName.DataSource = _autoFactoryOrgList;
            }

            //来源类型
            List<ComComboBoxDataSourceTC> resultSalesForecastOrderSourceTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.SalesForecastOrderSourceType);
            cbWhere_SFO_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_SFO_SourceTypeName.ValueMember = SysConst.EN_Code;
            cbWhere_SFO_SourceTypeName.DataSource = resultSalesForecastOrderSourceTypeList;
            cbWhere_SFO_SourceTypeName.DataBind();

            //单据状态
            List<ComComboBoxDataSourceTC> resultStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.SalesForecastOrderStatus);
            cbWhere_SFO_StatusName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_SFO_StatusName.ValueMember = SysConst.EN_Code;
            cbWhere_SFO_StatusName.DataSource = resultStatusList;
            cbWhere_SFO_StatusName.DataBind();

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
            var activeRowIndex = gdGrid.ActiveRow.Index;
            //判断Grid内[唯一标识]是否为空
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SD_SalesForecastOrder.Code.SFO_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SD_SalesForecastOrder.Code.SFO_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用HeadGridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            HeadDS = HeadGridDS.FirstOrDefault(x => x.SFO_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SD_SalesForecastOrder.Code.SFO_ID].Value?.ToString());
            if (HeadDS == null || string.IsNullOrEmpty(HeadDS.SFO_ID))
            {
                return;
            }
            //将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();

            //选中【详情】Tab
            tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;

            //查询明细Grid数据并绑定
            QueryDetail();

            //设置详情
            SetDetailControl();
        }

        /// <summary>
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //单据编号
            txtSFO_No.Text = HeadDS.SFO_No;
            //汽修商户
            txtSFO_AutoFactoryName.Text = HeadDS.SFO_AutoFactoryName;
            //客户名称
            txtSFO_CustomerName.Text = HeadDS.SFO_CustomerName;
            //备注
            txtSFO_Remark.Text = HeadDS.SFO_Remark;
            //有效
            if (HeadDS.SFO_IsValid != null)
            {
                ckSFO_IsValid.Checked = HeadDS.SFO_IsValid.Value;
            }
            //创建人
            txtSFO_CreatedBy.Text = HeadDS.SFO_CreatedBy;
            //创建时间
            dtSFO_CreatedTime.Value = HeadDS.SFO_CreatedTime;
            //修改人
            txtSFO_UpdatedBy.Text = HeadDS.SFO_UpdatedBy;
            //修改时间
            dtSFO_UpdatedTime.Value = HeadDS.SFO_UpdatedTime;
            //来源类型名称
            txtSFO_SourceTypeName.Text = HeadDS.SFO_SourceTypeName;
            //来源类型编码
            txtSFO_SourceTypeCode.Text = HeadDS.SFO_SourceTypeCode;
            //单据状态名称
            txtSFO_StatusName.Text = HeadDS.SFO_StatusName;
            //单据状态编码
            txtSFO_StatusCode.Text = HeadDS.SFO_StatusCode;
            //销售预测订单ID
            txtSFO_ID.Text = HeadDS.SFO_ID;
            //组织ID
            txtSFO_Org_ID.Text = HeadDS.SFO_Org_ID;
            //客户ID
            txtSFO_CustomerID.Text = HeadDS.SFO_CustomerID;
            //版本号
            txtSFO_VersionNo.Value = HeadDS.SFO_VersionNo;
        }

        /// <summary>
        /// 查询明细Grid数据并绑定
        /// </summary>
        private void QueryDetail()
        {
            //加载销售预测订单明细
            if (string.IsNullOrEmpty(HeadDS.SFO_ID))
            {
                return;
            }
            _bll.QueryForList<MDLSD_SalesForecastOrderDetail, SalesForecastOrderDetailUIModel>(new MDLSD_SalesForecastOrderDetail()
            {
                WHERE_SFOD_IsValid = true,
                WHERE_SFOD_ST_ID = HeadDS.SFO_ID
            }, _detailGridDS);
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置销售补货建议明细Grid自适应列宽（根据单元格内容）
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
        /// 设置详情页面控件的是否可编辑
        /// </summary>
        private void SetDetailControl()
        {
            if (txtSFO_StatusName.Text == SalesForecastOrderStatusEnum.Name.YSC)
            {
                #region 销售预测订单.[单据状态]为{已生成}的场合，[转销售]可用

                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSALESORDER, true, true);
                #endregion
            }
            else
            {
                #region 销售预测订单.[单据状态]不是{已生成}的场合，[转销售]不可用

                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSALESORDER, true, false);
                #endregion
            }
        }
        #endregion

    }
}
