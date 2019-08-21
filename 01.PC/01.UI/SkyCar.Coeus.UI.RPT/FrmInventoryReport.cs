using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.RPT;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.RPT.QCModel;
using SkyCar.Coeus.UIModel.RPT.UIModel;

namespace SkyCar.Coeus.UI.RPT
{
    /// <summary>
    /// 库存统计报表
    /// </summary>
    public partial class FrmInventoryReport : BaseFormCardListDetail<InventoryTotalReportUIModel, InventoryReportQCModel, InventoryTotalReportUIModel>
    {
        #region 全局变量

        /// <summary>
        /// 库存统计报表BLL
        /// </summary>
        private InventoryReportBLL _bll = new InventoryReportBLL();

        /// <summary>
        /// 当前页合计
        /// </summary>
        string _sumCurPageDesc = "当前页合计：";

        /// <summary>
        /// 合计
        /// </summary>
        string _sumAllPageDesc = "合计：";

        #region 下拉框数据源

        /// <summary>
        /// 组织数据源
        /// </summary>
        List<MDLSM_Organization> _orgList = new List<MDLSM_Organization>();
        /// <summary>
        /// 仓库数据源
        /// </summary>
        List<MDLPIS_Warehouse> _warehouseList = new List<MDLPIS_Warehouse>();
        /// <summary>
        /// 配件名称数据源
        /// </summary>
        List<MDLBS_AutoPartsName> _autoPartsNameList = new List<MDLBS_AutoPartsName>();

        #endregion

        #region Grid数据源
        /// <summary>
        /// 根据配件名称统计库存汇总数据源
        /// </summary>
        private List<InventoryTotalReportUIModel> _inventoryByNameTotalReportDs = new List<InventoryTotalReportUIModel>();
        /// <summary>
        /// 根据配件名称和品牌统计库存汇总数据源
        /// </summary>
        private List<InventoryTotalReportUIModel> _inventoryByNameAndBrandTotalReportDs = new List<InventoryTotalReportUIModel>();
        /// <summary>
        /// 库存统计明细数据源
        /// </summary>
        private List<InventoryDetailReportUIModel> _inventoryDetailReportDs = new List<InventoryDetailReportUIModel>();

        #endregion

        #region 根据配件名称统计汇总分页
        private UltraToolbarsManager _toolBarPagingByNameOfTotal = new UltraToolbarsManager();
        /// <summary>
        /// 翻页ToolBar
        /// </summary>
        public new UltraToolbarsManager ToolBarPagingByNameOfTotal
        {
            get { return _toolBarPagingByNameOfTotal; }
            set
            {
                _toolBarPagingByNameOfTotal = value;
                //初始化翻页
                SetBarPaging(TotalRecordCountByNameOfTotal);
            }
        }
        /// <summary>
        /// 当前页面索引值
        /// </summary>
        public new int PageIndexByNameOfTotal = 1;
        /// <summary>
        /// 总记录条数
        /// </summary>
        public new int TotalRecordCountByNameOfTotal = 0;
        /// <summary>
        /// 页面大小
        /// </summary>
        public new int PageSizeByNameOfTotal = 40;
        /// <summary>
        /// 总页面数
        /// </summary>
        public new int TotalPageCountByNameOfTotal = 0;
        #endregion

        #region 根据配件名称和品牌统计汇总分页
        private UltraToolbarsManager _toolBarPagingByNameAndBrandOfTotal = new UltraToolbarsManager();
        /// <summary>
        /// 翻页ToolBar
        /// </summary>
        public new UltraToolbarsManager ToolBarPagingByNameAndBrandOfTotal
        {
            get { return _toolBarPagingByNameAndBrandOfTotal; }
            set
            {
                _toolBarPagingByNameAndBrandOfTotal = value;
                //初始化翻页
                SetBarPaging(TotalRecordCountByNameAndBrandOfTotal);
            }
        }
        /// <summary>
        /// 当前页面索引值
        /// </summary>
        public new int PageIndexByNameAndBrandOfTotal = 1;
        /// <summary>
        /// 总记录条数
        /// </summary>
        public new int TotalRecordCountByNameAndBrandOfTotal = 0;
        /// <summary>
        /// 页面大小
        /// </summary>
        public new int PageSizeByNameAndBrandOfTotal = 40;
        /// <summary>
        /// 总页面数
        /// </summary>
        public new int TotalPageCountByNameAndBrandOfTotal = 0;
        #endregion

        #region 明细分页
        private UltraToolbarsManager _toolBarPagingOfDetail = new UltraToolbarsManager();
        /// <summary>
        /// 翻页ToolBar
        /// </summary>
        public new UltraToolbarsManager ToolBarPagingOfDetail
        {
            get { return _toolBarPagingOfDetail; }
            set
            {
                _toolBarPagingOfDetail = value;
                //初始化翻页
                SetBarPaging(TotalRecordCountOfDetail);
            }
        }
        /// <summary>
        /// 当前页面索引值
        /// </summary>
        public new int PageIndexOfDetail = 1;
        /// <summary>
        /// 总记录条数
        /// </summary>
        public new int TotalRecordCountOfDetail = 0;
        /// <summary>
        /// 页面大小
        /// </summary>
        public new int PageSizeOfDetail = 40;
        /// <summary>
        /// 总页面数
        /// </summary>
        public new int TotalPageCountOfDetail = 0;
        #endregion

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmSalesOrderManager构造方法
        /// </summary>
        public FrmInventoryReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSalesOrderManager_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（动作）
            base.ToolBarActionAndNavigate = toolBarActionAndNavigate;
            //工具栏（翻页）
            base.ToolBarPaging = this.toolBarPagingTotalByName;
            ToolBarPagingByNameOfTotal = this.toolBarPagingTotalByName;
            ToolBarPagingByNameAndBrandOfTotal = this.toolBarPagingTotalByNameAndBrand;
            ToolBarPagingOfDetail = this.toolBarPagingDetail;
            //查询委托（基类控制翻页用）
            base.ExecuteQuery = QueryAction;
            //工具栏（动作）单击事件
            this.toolBarActionAndNavigate.ToolClick += new ToolClickEventHandler(base.toolBarActionAndNavigate_ToolClick);
            //工具栏（翻页）单击事件
            this.toolBarPagingTotalByName.ToolClick += new ToolClickEventHandler(ToolBarPagingByName_ToolClick);
            this.toolBarPagingTotalByNameAndBrand.ToolClick += new ToolClickEventHandler(ToolBarPagingByNameAndBrand_ToolClick);
            this.toolBarPagingDetail.ToolClick += new ToolClickEventHandler(ToolBarPagingOfDetail_ToolClick);
            //工具栏（翻页）[当前页]值改变事件
            this.toolBarPagingTotalByName.ToolValueChanged += new ToolEventHandler(ToolBarPagingByName_ToolValueChanged);
            this.toolBarPagingTotalByNameAndBrand.ToolValueChanged += new ToolEventHandler(ToolBarPagingByNameAndBrand_ToolValueChanged);
            this.toolBarPagingDetail.ToolValueChanged += new ToolEventHandler(ToolBarPagingOfDetail_ToolValueChanged);

            #region 设置页面大小文本框
            //根据配件名称统计库存
            TextBoxTool pageSizeByNameOfTotalTextBox = null;
            foreach (var loopToolControl in this.toolBarPagingTotalByName.Tools)
            {
                if (loopToolControl.Key == SysConst.EN_PAGESIZE)
                {
                    pageSizeByNameOfTotalTextBox = (TextBoxTool)loopToolControl;
                }
            }
            if (pageSizeByNameOfTotalTextBox != null)
            {
                pageSizeByNameOfTotalTextBox.Text = PageSizeByNameOfTotal.ToString();
                pageSizeByNameOfTotalTextBox.AfterToolExitEditMode += PageSizeTextBoxToolByName_AfterToolExitEditMode;
            }

            //根据配件名称和品牌统计库存
            TextBoxTool pageSizeByNameAndBrandOfTotalTextBox = null;
            foreach (var loopToolControl in this.toolBarPagingTotalByNameAndBrand.Tools)
            {
                if (loopToolControl.Key == SysConst.EN_PAGESIZE)
                {
                    pageSizeByNameAndBrandOfTotalTextBox = (TextBoxTool)loopToolControl;
                }
            }
            if (pageSizeByNameAndBrandOfTotalTextBox != null)
            {
                pageSizeByNameAndBrandOfTotalTextBox.Text = PageSizeByNameAndBrandOfTotal.ToString();
                pageSizeByNameAndBrandOfTotalTextBox.AfterToolExitEditMode += PageSizeTextBoxToolByNameAndBrand_AfterToolExitEditMode;
            }

            //库存明细
            TextBoxTool pageSizeOfDetailTextBox = null;
            foreach (var loopToolControl in this.toolBarPagingDetail.Tools)
            {
                if (loopToolControl.Key == SysConst.EN_PAGESIZE)
                {
                    pageSizeOfDetailTextBox = (TextBoxTool)loopToolControl;
                }
            }
            if (pageSizeOfDetailTextBox != null)
            {
                pageSizeOfDetailTextBox.Text = PageSizeOfDetail.ToString();
                pageSizeOfDetailTextBox.AfterToolExitEditMode += PageSizeTextBoxToolOfDetail_AfterToolExitEditMode;
            }
            #endregion

            //设置总页数
            SetTotalPageCountAndTotalRecordCountByNameOfTotal(0);
            SetTotalPageCountAndTotalRecordCountByNameAndBrandOfTotal(0);
            SetTotalPageCountAndTotalRecordCountOfDetail(0);

            //初始化【列表】Tab内控件
            InitializeListTabControls();

            //设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
            //根据选中的Tab，设置动作按钮[是否可用]（在系统权限的基础上进行控制）
            var exportEnable = MenuGroupActionList.FirstOrDefault(x => x.Act_Key == SystemActionEnum.Code.EXPORT) !=
                    null ? true : false;
            SetActionEnable(SystemActionEnum.Code.EXPORT, exportEnable);
            SetActionEnable(SystemActionEnum.Code.QUERY, true);
            #endregion
        }

        #region [根据配件名称统计库存]相关事件

        /// <summary>
        /// 【汇总】[根据配件名称统计库存]Grid的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InventoryByNameReportGrid_DoubleClick(object sender, EventArgs e)
        {
            //将Grid中选中的数据赋值给【详情】Tab内的对应控件
            SetByNameGridDataToCardCtrls();
        }
        /// <summary>
        /// 【汇总】[根据配件名称统计库存]Grid的Cell的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InventoryByNameReportGrid_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            //将Grid中选中的数据赋值给【详情】Tab内的对应控件
            SetByNameGridDataToCardCtrls();
        }
        /// <summary>
        /// 【汇总】[根据配件名称统计库存]Grid的CellChange事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InventoryByNameReportGrid_CellChange(object sender, CellEventArgs e)
        {
            InventoryByNameReportGrid.UpdateData();
        }
        #endregion

        #region [根据配件名称和品牌统计库存]相关事件

        /// <summary>
        /// 【汇总】[根据配件名称和品牌统计库存]Grid的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InventoryByNameAndBrandReportGrid_DoubleClick(object sender, EventArgs e)
        {
            //将Grid中选中的数据赋值给【详情】Tab内的对应控件
            SetByNameAndBrandGridDataToCardCtrls();
        }

        /// <summary>
        /// 【汇总】[根据配件名称和品牌统计库存]Grid的Cell的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InventoryByNameAndBrandReportGrid_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            //将Grid中选中的数据赋值给【详情】Tab内的对应控件
            SetByNameAndBrandGridDataToCardCtrls();
        }
        /// <summary>
        /// 【汇总】[根据配件名称和品牌统计库存]Grid的CellChange事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InventoryByNameAndBrandReportGrid_CellChange(object sender, CellEventArgs e)
        {
            InventoryByNameAndBrandReportGrid.UpdateData();
        }
        #endregion

        #region 查询条件相关事件

        /// <summary>
        /// 入库时间终了ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_SalesTimeEnd_ValueChanged(object sender, EventArgs e)
        {
            if (this.dtWhere_StockInTimeEnd.Value != null &&
                this.dtWhere_StockInTimeEnd.DateTime.Hour == 0 &&
                this.dtWhere_StockInTimeEnd.DateTime.Minute == 0 &&
                this.dtWhere_StockInTimeEnd.DateTime.Second == 0)
            {
                DateTime newDateTime = new DateTime(this.dtWhere_StockInTimeEnd.DateTime.Year, this.dtWhere_StockInTimeEnd.DateTime.Month, this.dtWhere_StockInTimeEnd.DateTime.Day, 23, 59, 59);
                this.dtWhere_StockInTimeEnd.DateTime = newDateTime;
            }
        }

        /// <summary>
        /// 库存组织名称改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbWhere_InventoryOrgID_SelectedIndexChanged(object sender, EventArgs e)
        {
            mcbWhere_WarehouseName.Clear();
            if (!string.IsNullOrEmpty(mcbWhere_InventoryOrgID.SelectedText))
            {
                var warehouseList =
               _warehouseList.Where(x => x.WH_Org_ID == mcbWhere_InventoryOrgID.SelectedValue).ToList();
                mcbWhere_WarehouseName.DisplayMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name;
                mcbWhere_WarehouseName.ValueMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_ID;
                mcbWhere_WarehouseName.DataSource = warehouseList;
            }
            else
            {
                mcbWhere_WarehouseName.DisplayMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name;
                mcbWhere_WarehouseName.ValueMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_ID;
                mcbWhere_WarehouseName.DataSource = _warehouseList;
            }
        }
        #endregion

        #endregion

        #region 重写基类方法

        #region 动作按钮
        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            //1.设置查询条件（翻页相关属性不用设置）
            base.ConditionDS = new InventoryReportQCModel()
            {
                //组织ID
                OrgID = mcbWhere_InventoryOrgID.SelectedValue,
                //仓库ID
                WarehouseID = mcbWhere_WarehouseName.SelectedValue,
            };
            //入库时间-开始
            if (this.dtWhere_StockInTimeStart.Value != null)
            {
                ConditionDS.StockInTimeStart = this.dtWhere_StockInTimeStart.DateTime;
            }
            else
            {
                ConditionDS.StockInTimeStart = null;
            }
            //入库时间-终了
            if (this.dtWhere_StockInTimeEnd.Value != null)
            {
                ConditionDS.StockInTimeEnd = this.dtWhere_StockInTimeEnd.DateTime;
            }
            else
            {
                ConditionDS.StockInTimeEnd = null;
            }

            //2.查询并绑定数据源
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                //根据配件名称查询库存汇总
                QueryInventoryByNameTotalReport();
                //根据配件名称和品牌查询汇总
                QueryInventoryByNameAndBrandTotalReport();
            }
            else
            {
                //查询库存明细
                QueryInventoryDetailReport();
            }
        }

        #region 根据配件名称查询汇总
        /// <summary>
        /// 根据配件名称查询库存汇总统计
        /// </summary>
        private void QueryInventoryByNameTotalReport()
        {
            try
            {
                InventoryReportQCModel argsModel = ConditionDS;
                argsModel.PageSize = PageSizeByNameOfTotal;
                argsModel.PageIndex = PageIndexByNameOfTotal;
                argsModel.SqlId = SQLID.RPT_InventoryReport_SQL01;

                _inventoryByNameTotalReportDs = new List<InventoryTotalReportUIModel>();
                _bll.QueryForList(argsModel.SqlId, argsModel, _inventoryByNameTotalReportDs);

                decimal totalInventoryAmountOfCurPage = 0;
                decimal totalInventoryQtyOfCurPage = 0;
                decimal totalInventoryAmountOfAllPage = 0;
                decimal totalInventoryQtyOfAllPage = 0;

                if (_inventoryByNameTotalReportDs.Count > 0)
                {
                    InventoryTotalReportUIModel subObject = _inventoryByNameTotalReportDs[0];
                    TotalRecordCountByNameOfTotal = subObject.RecordCount ?? 0;
                    totalInventoryAmountOfAllPage = (subObject.TotalInventoryAmount ?? 0);
                    totalInventoryQtyOfAllPage = (subObject.TotalInventoryQty ?? 0);
                }
                else
                {
                    TotalRecordCountByNameOfTotal = 0;
                }

                foreach (var loopSotckInTotalItem in _inventoryByNameTotalReportDs)
                {
                    totalInventoryAmountOfCurPage += (loopSotckInTotalItem.InventoryAmount ?? 0);
                    totalInventoryQtyOfCurPage += (loopSotckInTotalItem.InventoryQty ?? 0);
                }
                InventoryTotalReportUIModel curPageTotal = new InventoryTotalReportUIModel
                {
                    AutoPartsName = _sumCurPageDesc,
                    InventoryAmount = Math.Round(totalInventoryAmountOfCurPage, 2),
                    InventoryQty = Math.Round(totalInventoryQtyOfCurPage, 0)
                };
                _inventoryByNameTotalReportDs.Add(curPageTotal);

                InventoryTotalReportUIModel allPageTotal = new InventoryTotalReportUIModel
                {
                    AutoPartsName = _sumAllPageDesc,
                    InventoryAmount = Math.Round(totalInventoryAmountOfAllPage, 2),
                    InventoryQty = Math.Round(totalInventoryQtyOfAllPage, 0)
                };
                _inventoryByNameTotalReportDs.Add(allPageTotal);

                //重新计算[总页数]，设置最新[allPageTotal]
                SetTotalPageCountAndTotalRecordCountByNameOfTotal(TotalRecordCountByNameOfTotal);
                //设置翻页按钮状态
                SetPageButtonStatusByNameOfTotal();

                InventoryByNameReportGrid.DataSource = _inventoryByNameTotalReportDs;
                InventoryByNameReportGrid.DataBind();
                InventoryByNameReportGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置总页数和总记录条数
        /// </summary>
        /// <param name="paramTotalRecordCount">总记录条数</param>
        private void SetTotalPageCountAndTotalRecordCountByNameOfTotal(int paramTotalRecordCount)
        {
            if (ToolBarPagingByNameOfTotal != null)
            {
                if (paramTotalRecordCount > 0)
                {
                    TotalRecordCountByNameOfTotal = paramTotalRecordCount;
                    int? remainder = TotalRecordCountByNameOfTotal % PageSizeByNameOfTotal;
                    if (remainder > 0)
                    {
                        TotalPageCountByNameOfTotal = TotalRecordCountByNameOfTotal / PageSizeByNameOfTotal + 1;
                    }
                    else
                    {
                        TotalPageCountByNameOfTotal = TotalRecordCountByNameOfTotal / PageSizeByNameOfTotal;
                    }
                }
                else
                {
                    PageIndexByNameOfTotal = 1;
                    TotalPageCountByNameOfTotal = 1;
                    TotalRecordCountByNameOfTotal = 0;
                }
                ((TextBoxTool)(ToolBarPagingByNameOfTotal.Tools[SysConst.EN_PAGEINDEX])).Text = PageIndexByNameOfTotal.ToString();
                ToolBarPagingByNameOfTotal.Tools[SysConst.EN_PAGECOUNT].SharedProps.Caption = TotalPageCountByNameOfTotal.ToString();
            }
        }

        /// <summary>
        /// 设置翻页按钮状态
        /// </summary>
        private void SetPageButtonStatusByNameOfTotal()
        {
            if (ToolBarPagingByNameOfTotal != null)
            {
                if (PageIndexByNameOfTotal == 0 || TotalRecordCountByNameOfTotal == 0)
                {
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;

                    return;
                }
                if (PageIndexByNameOfTotal == 1 && TotalRecordCountByNameOfTotal <= PageSizeByNameOfTotal)
                {
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (PageIndexByNameOfTotal == 1 && TotalRecordCountByNameOfTotal > PageSizeByNameOfTotal)
                {
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (PageIndexByNameOfTotal != 1 && TotalRecordCountByNameOfTotal > PageSizeByNameOfTotal * PageIndexByNameOfTotal)
                {
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
                }
                else if (PageIndexByNameOfTotal != 1 && TotalRecordCountByNameOfTotal <= PageSizeByNameOfTotal * PageIndexByNameOfTotal)
                {
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingByNameOfTotal.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
                }
                else
                {
                    throw (new Exception("非预期的场合。。。"));
                }
            }
        }

        #endregion

        #region 根据配件名称和品牌查询汇总
        /// <summary>
        /// 根据配件名称查询库存汇总统计
        /// </summary>
        private void QueryInventoryByNameAndBrandTotalReport()
        {
            try
            {
                InventoryReportQCModel argsModel = ConditionDS;
                argsModel.PageSize = PageSizeByNameAndBrandOfTotal;
                argsModel.PageIndex = PageIndexByNameAndBrandOfTotal;
                argsModel.SqlId = SQLID.RPT_InventoryReport_SQL02;

                _inventoryByNameAndBrandTotalReportDs = new List<InventoryTotalReportUIModel>();
                _bll.QueryForList(argsModel.SqlId, argsModel, _inventoryByNameAndBrandTotalReportDs);

                decimal totalInventoryAmountOfCurPage = 0;
                decimal totalInventoryQtyOfCurPage = 0;
                decimal totalInventoryAmountOfAllPage = 0;
                decimal totalInventoryQtyOfAllPage = 0;

                if (_inventoryByNameAndBrandTotalReportDs.Count > 0)
                {
                    InventoryTotalReportUIModel subObject = _inventoryByNameAndBrandTotalReportDs[0];
                    TotalRecordCountByNameAndBrandOfTotal = subObject.RecordCount ?? 0;
                    totalInventoryAmountOfAllPage = (subObject.TotalInventoryAmount ?? 0);
                    totalInventoryQtyOfAllPage = (subObject.TotalInventoryQty ?? 0);
                }
                else
                {
                    TotalRecordCountByNameAndBrandOfTotal = 0;
                }

                foreach (var loopSotckInTotalItem in _inventoryByNameAndBrandTotalReportDs)
                {
                    totalInventoryAmountOfCurPage += (loopSotckInTotalItem.InventoryAmount ?? 0);
                    totalInventoryQtyOfCurPage += (loopSotckInTotalItem.InventoryQty ?? 0);
                }
                InventoryTotalReportUIModel curPageTotal = new InventoryTotalReportUIModel
                {
                    AutoPartsBrand = _sumCurPageDesc,
                    InventoryAmount = Math.Round(totalInventoryAmountOfCurPage, 2),
                    InventoryQty = Math.Round(totalInventoryQtyOfCurPage, 0)
                };
                _inventoryByNameAndBrandTotalReportDs.Add(curPageTotal);

                InventoryTotalReportUIModel allPageTotal = new InventoryTotalReportUIModel
                {
                    AutoPartsBrand = _sumAllPageDesc,
                    InventoryAmount = Math.Round(totalInventoryAmountOfAllPage, 2),
                    InventoryQty = Math.Round(totalInventoryQtyOfAllPage, 0)
                };
                _inventoryByNameAndBrandTotalReportDs.Add(allPageTotal);

                //重新计算[总页数]，设置最新[allPageTotal]
                SetTotalPageCountAndTotalRecordCountByNameAndBrandOfTotal(TotalRecordCountByNameAndBrandOfTotal);
                //设置翻页按钮状态
                SetPageButtonStatusAndBrandOfTotal();

                InventoryByNameAndBrandReportGrid.DataSource = _inventoryByNameAndBrandTotalReportDs;
                InventoryByNameAndBrandReportGrid.DataBind();
                InventoryByNameAndBrandReportGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置总页数和总记录条数
        /// </summary>
        /// <param name="paramTotalRecordCount">总记录条数</param>
        private void SetTotalPageCountAndTotalRecordCountByNameAndBrandOfTotal(int paramTotalRecordCount)
        {
            if (ToolBarPagingByNameAndBrandOfTotal != null)
            {
                if (paramTotalRecordCount > 0)
                {
                    TotalRecordCountByNameAndBrandOfTotal = paramTotalRecordCount;
                    int? remainder = TotalRecordCountByNameAndBrandOfTotal % PageSizeByNameAndBrandOfTotal;
                    if (remainder > 0)
                    {
                        TotalPageCountByNameAndBrandOfTotal = TotalRecordCountByNameAndBrandOfTotal / PageSizeByNameAndBrandOfTotal + 1;
                    }
                    else
                    {
                        TotalPageCountByNameAndBrandOfTotal = TotalRecordCountByNameAndBrandOfTotal / PageSizeByNameAndBrandOfTotal;
                    }
                }
                else
                {
                    PageIndexByNameAndBrandOfTotal = 1;
                    TotalPageCountByNameAndBrandOfTotal = 1;
                    TotalRecordCountByNameAndBrandOfTotal = 0;
                }
                ((TextBoxTool)(ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_PAGEINDEX])).Text = PageIndexByNameAndBrandOfTotal.ToString();
                ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_PAGECOUNT].SharedProps.Caption = TotalPageCountByNameAndBrandOfTotal.ToString();
            }
        }

        /// <summary>
        /// 设置翻页按钮状态
        /// </summary>
        private void SetPageButtonStatusAndBrandOfTotal()
        {
            if (ToolBarPagingByNameAndBrandOfTotal != null)
            {
                if (PageIndexByNameAndBrandOfTotal == 0 || TotalRecordCountByNameAndBrandOfTotal == 0)
                {
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;

                    return;
                }
                if (PageIndexByNameAndBrandOfTotal == 1 && TotalRecordCountByNameAndBrandOfTotal <= PageSizeByNameAndBrandOfTotal)
                {
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (PageIndexByNameAndBrandOfTotal == 1 && TotalRecordCountByNameAndBrandOfTotal > PageSizeByNameAndBrandOfTotal)
                {
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (PageIndexByNameAndBrandOfTotal != 1 && TotalRecordCountByNameAndBrandOfTotal > PageSizeByNameAndBrandOfTotal * PageIndexByNameAndBrandOfTotal)
                {
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
                }
                else if (PageIndexByNameAndBrandOfTotal != 1 && TotalRecordCountByNameAndBrandOfTotal <= PageSizeByNameAndBrandOfTotal * PageIndexByNameAndBrandOfTotal)
                {
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
                }
                else
                {
                    throw (new Exception("非预期的场合。。。"));
                }
            }
        }

        #endregion

        #region 查询明细

        /// <summary>
        /// 查询库存明细统计
        /// </summary>
        /// <param name="paramTotalReport">汇总行</param>
        private void QueryInventoryDetailReport(InventoryTotalReportUIModel paramTotalReport = null)
        {
            try
            {
                InventoryReportQCModel argsModel = ConditionDS;
                argsModel.PageSize = PageSizeOfDetail;
                argsModel.PageIndex = PageIndexOfDetail;
                argsModel.SqlId = SQLID.RPT_InventoryReport_SQL03;
                if (paramTotalReport != null)
                {
                    argsModel.OrgID = paramTotalReport.OrgID;
                    argsModel.WarehouseID = paramTotalReport.WarehouseID;
                    argsModel.AutoPartsName = paramTotalReport.AutoPartsName;
                    argsModel.AutoPartsBrand = paramTotalReport.AutoPartsBrand;
                }
                else
                {
                    argsModel.AutoPartsName = null;
                    argsModel.AutoPartsBrand = null;
                }

                _inventoryDetailReportDs = new List<InventoryDetailReportUIModel>();
                _bll.QueryForList(argsModel.SqlId, argsModel, _inventoryDetailReportDs);

                decimal totalInventoryQtyOfCurPage = 0;
                decimal totalInventoryAmountOfCurPage = 0;
                decimal totalInventoryQtyOfAllPage = 0;
                decimal totalInventoryAmountOfAllPage = 0;

                if (_inventoryDetailReportDs.Count > 0)
                {
                    InventoryDetailReportUIModel subObject = _inventoryDetailReportDs[0];
                    TotalRecordCountOfDetail = (subObject.RecordCount ?? 0);
                    totalInventoryQtyOfAllPage = (subObject.TotalInventoryQty ?? 0);
                    totalInventoryAmountOfAllPage = (subObject.TotalInventoryAmount ?? 0);
                }
                else
                {
                    TotalRecordCountOfDetail = 0;
                }

                foreach (var loopDetail in _inventoryDetailReportDs)
                {
                    totalInventoryQtyOfCurPage += (loopDetail.INV_Qty ?? 0);
                    totalInventoryAmountOfCurPage += (loopDetail.InventoryAmount ?? 0);
                }
                InventoryDetailReportUIModel curPageTotal = new InventoryDetailReportUIModel
                {
                    WHB_Name = _sumCurPageDesc,
                    INV_Qty = Math.Round(totalInventoryQtyOfCurPage, 0),
                    InventoryAmount = Math.Round(totalInventoryAmountOfCurPage, 2)
                };
                _inventoryDetailReportDs.Add(curPageTotal);

                InventoryDetailReportUIModel allPageTotal = new InventoryDetailReportUIModel
                {
                    WHB_Name = _sumAllPageDesc,
                    INV_Qty = Math.Round(totalInventoryQtyOfAllPage, 0),
                    InventoryAmount = Math.Round(totalInventoryAmountOfAllPage, 2)
                };
                _inventoryDetailReportDs.Add(allPageTotal);

                //重新计算[总页数]，设置最新[总记录条数]
                SetTotalPageCountAndTotalRecordCountOfDetail(TotalRecordCountOfDetail);
                //设置翻页按钮状态
                SetPageButtonStatusOfDetail();

                InventoryDetailGrid.DataSource = _inventoryDetailReportDs;
                InventoryDetailGrid.DataBind();
                InventoryDetailGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置总页数和总记录条数
        /// </summary>
        /// <param name="paramTotalRecordCount">总记录条数</param>
        private void SetTotalPageCountAndTotalRecordCountOfDetail(int paramTotalRecordCount)
        {
            if (ToolBarPagingOfDetail != null)
            {
                if (paramTotalRecordCount > 0)
                {
                    TotalRecordCountOfDetail = paramTotalRecordCount;
                    int? remainder = TotalRecordCountOfDetail % PageSizeOfDetail;
                    if (remainder > 0)
                    {
                        TotalPageCountOfDetail = TotalRecordCountOfDetail / PageSizeOfDetail + 1;
                    }
                    else
                    {
                        TotalPageCountOfDetail = TotalRecordCountOfDetail / PageSizeOfDetail;
                    }
                }
                else
                {
                    PageIndexOfDetail = 1;
                    TotalPageCountOfDetail = 1;
                    TotalRecordCountOfDetail = 0;
                }
                ((TextBoxTool)(ToolBarPagingOfDetail.Tools[SysConst.EN_PAGEINDEX])).Text = PageIndexOfDetail.ToString();
                ToolBarPagingOfDetail.Tools[SysConst.EN_PAGECOUNT].SharedProps.Caption = TotalPageCountOfDetail.ToString();
            }
        }

        /// <summary>
        /// 设置翻页按钮状态
        /// </summary>
        private void SetPageButtonStatusOfDetail()
        {
            if (ToolBarPagingOfDetail != null)
            {
                if (PageIndexOfDetail == 0 || TotalRecordCountOfDetail == 0)
                {
                    ToolBarPagingOfDetail.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingOfDetail.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingOfDetail.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingOfDetail.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;

                    return;
                }
                if (PageIndexOfDetail == 1 && TotalRecordCountOfDetail <= PageSizeOfDetail)
                {
                    ToolBarPagingOfDetail.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingOfDetail.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingOfDetail.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingOfDetail.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (PageIndexOfDetail == 1 && TotalRecordCountOfDetail > PageSizeOfDetail)
                {
                    ToolBarPagingOfDetail.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingOfDetail.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingOfDetail.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingOfDetail.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (PageIndexOfDetail != 1 && TotalRecordCountOfDetail > PageSizeOfDetail * PageIndexOfDetail)
                {
                    ToolBarPagingOfDetail.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingOfDetail.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingOfDetail.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingOfDetail.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
                }
                else if (PageIndexOfDetail != 1 && TotalRecordCountOfDetail <= PageSizeOfDetail * PageIndexOfDetail)
                {
                    ToolBarPagingOfDetail.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingOfDetail.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingOfDetail.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingOfDetail.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
                }
                else
                {
                    throw (new Exception("非预期的场合。。。"));
                }
            }
        }

        #endregion

        #region 分页

        #region 根据配件名称统计库存

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public new void ToolBarPagingByName_ToolClick(object sender, ToolClickEventArgs e)
        {
            var funcName = "ToolBarPagingByName_ToolClick";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPagingByNameOfTotal != null)
            {
                switch (e.Tool.Key)
                {
                    //第一页
                    case SysConst.EN_FIRSTPAGE:
                        ((TextBoxTool)(ToolBarPagingByNameOfTotal.Tools[SysConst.EN_PAGEINDEX])).Text =
                            SysConst.NUMBER_ONE.ToString();
                        break;
                    // 前一页
                    case SysConst.EN_FORWARDPAGE:
                        ((TextBoxTool)(ToolBarPagingByNameOfTotal.Tools[SysConst.EN_PAGEINDEX])).Text = (PageIndexByNameOfTotal - 1).ToString();
                        break;
                    // 下一页
                    case SysConst.EN_NEXTPAGE:
                        ((TextBoxTool)(ToolBarPagingByNameOfTotal.Tools[SysConst.EN_PAGEINDEX])).Text = (PageIndexByNameOfTotal + 1).ToString();
                        break;
                    // 最后一页
                    case SysConst.EN_LASTPAGE:
                        ((TextBoxTool)(ToolBarPagingByNameOfTotal.Tools[SysConst.EN_PAGEINDEX])).Text = TotalPageCountByNameOfTotal.ToString();
                        break;
                }
            }

            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 翻页ToolBar的值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public new void ToolBarPagingByName_ToolValueChanged(object sender, ToolEventArgs e)
        {
            var funcName = "ToolBarPagingByName_ToolValueChanged";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
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
                else if (tmpPageIndex > TotalPageCountByNameOfTotal)
                {
                    ((TextBoxTool)(e.Tool)).Text = TotalPageCountByNameOfTotal.ToString();
                    ((TextBoxTool)(e.Tool)).SelectionLength = TotalPageCountByNameOfTotal.ToString().Length;
                    return;
                }

                #region 当前页码设置
                if (Convert.ToInt32(strValue) <= 0)
                {
                    ((TextBoxTool)(e.Tool)).Text = SysConst.NUMBER_ONE.ToString();
                }
                else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) >= TotalPageCountByNameOfTotal)
                {
                    ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(TotalPageCountByNameOfTotal).ToString();
                }
                else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) < TotalPageCountByNameOfTotal)
                {
                    ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(strValue).ToString();
                }
                #endregion

                PageIndexByNameOfTotal = tmpPageIndex;
                if (tmpPageIndex <= 0)
                {
                    PageIndexByNameOfTotal = 1;
                }
                ExecuteQuery?.Invoke();
            }
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 页面大小失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageSizeTextBoxToolByName_AfterToolExitEditMode(object sender, AfterToolExitEditModeEventArgs e)
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
            PageSizeByNameOfTotal = tmpPageSize;

            ExecuteQuery?.Invoke();
        }

        #endregion

        #region 根据配件名称和品牌统计库存

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public new void ToolBarPagingByNameAndBrand_ToolClick(object sender, ToolClickEventArgs e)
        {
            var funcName = "ToolBarPagingByNameAndBrand_ToolClick";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPagingByNameAndBrandOfTotal != null)
            {
                switch (e.Tool.Key)
                {
                    //第一页
                    case SysConst.EN_FIRSTPAGE:
                        ((TextBoxTool)(ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_PAGEINDEX])).Text =
                            SysConst.NUMBER_ONE.ToString();
                        break;
                    // 前一页
                    case SysConst.EN_FORWARDPAGE:
                        ((TextBoxTool)(ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_PAGEINDEX])).Text = (PageIndexByNameAndBrandOfTotal - 1).ToString();
                        break;
                    // 下一页
                    case SysConst.EN_NEXTPAGE:
                        ((TextBoxTool)(ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_PAGEINDEX])).Text = (PageIndexByNameAndBrandOfTotal + 1).ToString();
                        break;
                    // 最后一页
                    case SysConst.EN_LASTPAGE:
                        ((TextBoxTool)(ToolBarPagingByNameAndBrandOfTotal.Tools[SysConst.EN_PAGEINDEX])).Text = TotalPageCountByNameAndBrandOfTotal.ToString();
                        break;
                }
            }

            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 翻页ToolBar的值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public new void ToolBarPagingByNameAndBrand_ToolValueChanged(object sender, ToolEventArgs e)
        {
            var funcName = "ToolBarPagingByNameAndBrand_ToolValueChanged";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
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
                else if (tmpPageIndex > TotalPageCountByNameAndBrandOfTotal)
                {
                    ((TextBoxTool)(e.Tool)).Text = TotalPageCountByNameAndBrandOfTotal.ToString();
                    ((TextBoxTool)(e.Tool)).SelectionLength = TotalPageCountByNameAndBrandOfTotal.ToString().Length;
                    return;
                }

                #region 当前页码设置
                if (Convert.ToInt32(strValue) <= 0)
                {
                    ((TextBoxTool)(e.Tool)).Text = SysConst.NUMBER_ONE.ToString();
                }
                else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) >= TotalPageCountByNameAndBrandOfTotal)
                {
                    ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(TotalPageCountByNameAndBrandOfTotal).ToString();
                }
                else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) < TotalPageCountByNameAndBrandOfTotal)
                {
                    ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(strValue).ToString();
                }
                #endregion

                PageIndexByNameAndBrandOfTotal = tmpPageIndex;
                if (tmpPageIndex <= 0)
                {
                    PageIndexByNameAndBrandOfTotal = 1;
                }
                ExecuteQuery?.Invoke();
            }
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 页面大小失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageSizeTextBoxToolByNameAndBrand_AfterToolExitEditMode(object sender, AfterToolExitEditModeEventArgs e)
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
            PageSizeByNameAndBrandOfTotal = tmpPageSize;

            ExecuteQuery?.Invoke();
        }

        #endregion

        #region 查询库存明细

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public new void ToolBarPagingOfDetail_ToolClick(object sender, ToolClickEventArgs e)
        {
            var funcName = "ToolBarPagingOfDetail_ToolClick";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPagingOfDetail != null)
            {
                switch (e.Tool.Key)
                {
                    //第一页
                    case SysConst.EN_FIRSTPAGE:
                        ((TextBoxTool)(ToolBarPagingOfDetail.Tools[SysConst.EN_PAGEINDEX])).Text =
                            SysConst.NUMBER_ONE.ToString();
                        break;
                    // 前一页
                    case SysConst.EN_FORWARDPAGE:
                        ((TextBoxTool)(ToolBarPagingOfDetail.Tools[SysConst.EN_PAGEINDEX])).Text = (PageIndexOfDetail - 1).ToString();
                        break;
                    // 下一页
                    case SysConst.EN_NEXTPAGE:
                        ((TextBoxTool)(ToolBarPagingOfDetail.Tools[SysConst.EN_PAGEINDEX])).Text = (PageIndexOfDetail + 1).ToString();
                        break;
                    // 最后一页
                    case SysConst.EN_LASTPAGE:
                        ((TextBoxTool)(ToolBarPagingOfDetail.Tools[SysConst.EN_PAGEINDEX])).Text = TotalPageCountOfDetail.ToString();
                        break;
                }
            }
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 翻页ToolBar的值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public new void ToolBarPagingOfDetail_ToolValueChanged(object sender, ToolEventArgs e)
        {
            var funcName = "ToolBarPagingOfDetail_ToolValueChanged";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
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
                else if (tmpPageIndex > TotalPageCountOfDetail)
                {
                    ((TextBoxTool)(e.Tool)).Text = TotalPageCountOfDetail.ToString();
                    ((TextBoxTool)(e.Tool)).SelectionLength = TotalPageCountOfDetail.ToString().Length;
                    return;
                }

                #region 当前页码设置
                if (Convert.ToInt32(strValue) <= 0)
                {
                    ((TextBoxTool)(e.Tool)).Text = SysConst.NUMBER_ONE.ToString();
                }
                else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) >= TotalPageCountOfDetail)
                {
                    ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(TotalPageCountOfDetail).ToString();
                }
                else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) < TotalPageCountOfDetail)
                {
                    ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(strValue).ToString();
                }
                #endregion

                PageIndexOfDetail = tmpPageIndex;
                if (tmpPageIndex <= 0)
                {
                    PageIndexOfDetail = 1;
                }
                ExecuteQuery?.Invoke();
            }
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 页面大小失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageSizeTextBoxToolOfDetail_AfterToolExitEditMode(object sender, AfterToolExitEditModeEventArgs e)
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
            PageSizeOfDetail = tmpPageSize;
            ExecuteQuery?.Invoke();
        }

        #endregion

        #endregion

        /// <summary>
        /// 清空
        /// </summary>
        public override void ClearAction()
        {
            #region 查询条件初始化
            //库存组织
            mcbWhere_InventoryOrgID.Clear();
            //仓库
            mcbWhere_WarehouseName.Clear();
            //入库时间开始
            dtWhere_StockInTimeStart.Value = null;
            //入库时间结束
            dtWhere_StockInTimeEnd.Value = null;
            //给 库存组织 设置焦点
            mcbWhere_InventoryOrgID.Focus();
            #endregion

            #region Grid初始化
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                //根据配件名称统计库存汇总
                _inventoryByNameTotalReportDs = new List<InventoryTotalReportUIModel>();
                InventoryByNameReportGrid.DataSource = _inventoryByNameTotalReportDs;
                InventoryByNameReportGrid.DataBind();

                //根据配件名称和品牌统计库存汇总
                _inventoryByNameAndBrandTotalReportDs = new List<InventoryTotalReportUIModel>();
                InventoryByNameAndBrandReportGrid.DataSource = _inventoryByNameAndBrandTotalReportDs;
                InventoryByNameAndBrandReportGrid.DataBind();
            }
            else
            {
                //库存明细
                _inventoryDetailReportDs = new List<InventoryDetailReportUIModel>();
                InventoryDetailGrid.DataSource = _inventoryDetailReportDs;
                InventoryDetailGrid.DataBind();
            }
            #endregion
        }

        /// <summary>
        /// 导出当前页
        /// </summary>
        public override void ExportAction(UltraGrid paramGrid, string paramGridName = "")
        {
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                string defaultFilePath = string.Empty;

                paramGridName = "根据配件名称统计库存汇总";
                ExportCurPage(InventoryByNameReportGrid, ref defaultFilePath, paramGridName);

                paramGridName = "根据配件名称和品牌统计库存汇总";
                ExportCurPage(InventoryByNameAndBrandReportGrid, ref defaultFilePath, paramGridName);
            }
            else
            {
                paramGridName = "库存明细";
                base.ExportAction(InventoryDetailGrid, paramGridName);
            }
        }

        /// <summary>
        /// 导出当前页
        /// </summary>
        /// <param name="paramGrid"></param>
        /// <param name="paramDefaultFilePath"></param>
        /// <param name="paramGridName"></param>
        private void ExportCurPage(UltraGrid paramGrid, ref string paramDefaultFilePath, string paramGridName = "")
        {
            if (paramGrid == null)
            {
                return;
            }
            if (paramGridName == null)
            {
                return;
            }
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    FileName = $"Export{paramGridName + DateTime.Now.ToString(FormatConst.DATE_TIME_FORMAT_03)}",
                    Filter = FormatConst.FILE_TYPE_FILTER_EXCEL_03,
                };
                if (!string.IsNullOrEmpty(paramDefaultFilePath))
                {
                    saveFileDialog.InitialDirectory = paramDefaultFilePath;
                    saveFileDialog.FileName = saveFileDialog.FileName + ".xls";
                }
                else
                {
                    saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                }

                //获取保存文件的上一级目录
                Directory.SetCurrentDirectory(Directory.GetParent(saveFileDialog.FileName).FullName);
                paramDefaultFilePath = Directory.GetCurrentDirectory();

                var gdGridExcelExporter = new UltraGridExcelExporter();
                gdGridExcelExporter.Export(paramGrid).Save(saveFileDialog.FileName);

                MessageBox.Show("导出" + paramGridName + "成功！", SysConst.CHS_MSG_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出" + paramGridName + "失败！", SysConst.CHS_MSG_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 导出全部
        /// </summary>
        public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            InventoryReportQCModel tempConditionDS = new InventoryReportQCModel()
            {
                //SqlId
                SqlId = tabControlFull.Tabs[SysConst.EN_LIST].Selected ? SQLID.RPT_InventoryReport_SQL01 : SQLID.RPT_InventoryReport_SQL03,
                //组织ID
                OrgID = mcbWhere_InventoryOrgID.SelectedValue,
                PageIndex = 1,
                PageSize = null
            };
            if (this.dtWhere_StockInTimeStart.Value != null)
            {
                //入库时间-开始
                tempConditionDS.StockInTimeStart = this.dtWhere_StockInTimeStart.DateTime;
            }
            if (this.dtWhere_StockInTimeEnd.Value != null)
            {
                //入库时间-终了
                tempConditionDS.StockInTimeEnd = this.dtWhere_StockInTimeEnd.DateTime;
            }

            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                #region 选中[汇总]Tab的场合

                string defaultFilePath = string.Empty;

                #region 导出全部[根据配件名称查询库存Grid] 
                List<InventoryTotalReportUIModel> resultAllByNameList = new List<InventoryTotalReportUIModel>();
                _bll.QueryForList(SQLID.RPT_InventoryReport_SQL01, tempConditionDS, resultAllByNameList);

                decimal totalInventoryAmountOfCurPageByName = 0;
                decimal totalInventoryQtyOfCurPageByName = 0;
                decimal totalInventoryAmountOfAllPageByName = 0;
                decimal totalInventoryQtyOfAllPageByName = 0;

                if (resultAllByNameList.Count > 0)
                {
                    InventoryTotalReportUIModel subObject = resultAllByNameList[0];
                    totalInventoryAmountOfAllPageByName = (subObject.TotalInventoryAmount ?? 0);
                    totalInventoryQtyOfAllPageByName = (subObject.TotalInventoryQty ?? 0);
                }

                foreach (var loopSotckInTotalItem in resultAllByNameList)
                {
                    totalInventoryAmountOfCurPageByName += (loopSotckInTotalItem.InventoryAmount ?? 0);
                    totalInventoryQtyOfCurPageByName += (loopSotckInTotalItem.InventoryQty ?? 0);
                }

                InventoryTotalReportUIModel curPageTotalByName = new InventoryTotalReportUIModel
                {
                    WarehouseName = _sumCurPageDesc,
                    InventoryAmount = Math.Round(totalInventoryAmountOfCurPageByName, 2),
                    InventoryQty = Math.Round(totalInventoryQtyOfCurPageByName, 2)
                };
                resultAllByNameList.Add(curPageTotalByName);

                InventoryTotalReportUIModel allPageTotalByName = new InventoryTotalReportUIModel
                {
                    WarehouseName = _sumAllPageDesc,
                    InventoryAmount = Math.Round(totalInventoryAmountOfAllPageByName, 2),
                    InventoryQty = Math.Round(totalInventoryQtyOfAllPageByName, 2)
                };
                resultAllByNameList.Add(allPageTotalByName);

                UltraGrid allGridByName = InventoryByNameReportGrid;
                allGridByName.DataSource = resultAllByNameList;
                allGridByName.DataBind();

                paramGridName = "根据配件名称统计库存汇总";
                ExportAllPage(InventoryByNameReportGrid, ref defaultFilePath, paramGridName);

                InventoryByNameReportGrid.DataSource = _inventoryByNameTotalReportDs;
                InventoryByNameReportGrid.DataBind();

                #endregion

                #region 导出全部[根据配件名称和品牌查询库存Grid]

                List<InventoryTotalReportUIModel> resultAllByNameAndBrandList = new List<InventoryTotalReportUIModel>();
                _bll.QueryForList(SQLID.RPT_InventoryReport_SQL02, tempConditionDS, resultAllByNameAndBrandList);

                decimal totalInventoryAmountOfCurPageByNameAndBrand = 0;
                decimal totalInventoryQtyOfCurPageByNameAndBrand = 0;
                decimal totalInventoryAmountOfAllPageByNameAndBrand = 0;
                decimal totalInventoryQtyOfAllPageByNameAndBrand = 0;

                if (resultAllByNameAndBrandList.Count > 0)
                {
                    InventoryTotalReportUIModel subObject = resultAllByNameAndBrandList[0];
                    TotalRecordCountByNameAndBrandOfTotal = subObject.RecordCount ?? 0;
                    totalInventoryAmountOfAllPageByNameAndBrand = (subObject.TotalInventoryAmount ?? 0);
                    totalInventoryQtyOfAllPageByNameAndBrand = (subObject.TotalInventoryQty ?? 0);
                }
                else
                {
                    TotalRecordCountByNameAndBrandOfTotal = 0;
                }

                foreach (var loopSotckInTotalItem in resultAllByNameAndBrandList)
                {
                    totalInventoryAmountOfCurPageByNameAndBrand += (loopSotckInTotalItem.InventoryAmount ?? 0);
                    totalInventoryQtyOfCurPageByNameAndBrand += (loopSotckInTotalItem.InventoryQty ?? 0);
                }
                InventoryTotalReportUIModel curPageTotal = new InventoryTotalReportUIModel
                {
                    AutoPartsBrand = _sumCurPageDesc,
                    InventoryAmount = Math.Round(totalInventoryAmountOfCurPageByNameAndBrand, 2),
                    InventoryQty = Math.Round(totalInventoryQtyOfCurPageByNameAndBrand, 0)
                };
                resultAllByNameAndBrandList.Add(curPageTotal);

                InventoryTotalReportUIModel allPageTotal = new InventoryTotalReportUIModel
                {
                    AutoPartsBrand = _sumAllPageDesc,
                    InventoryAmount = Math.Round(totalInventoryAmountOfAllPageByNameAndBrand, 2),
                    InventoryQty = Math.Round(totalInventoryQtyOfAllPageByNameAndBrand, 0)
                };
                resultAllByNameAndBrandList.Add(allPageTotal);

                UltraGrid allGridByNameAndBrand = InventoryByNameAndBrandReportGrid;
                allGridByNameAndBrand.DataSource = resultAllByNameList;
                allGridByNameAndBrand.DataBind();

                paramGridName = "根据配件名称和品牌统计库存汇总";
                ExportAllPage(InventoryByNameAndBrandReportGrid, ref defaultFilePath, paramGridName);

                InventoryByNameAndBrandReportGrid.DataSource = _inventoryByNameAndBrandTotalReportDs;
                InventoryByNameAndBrandReportGrid.DataBind();

                #endregion

                #endregion
            }
            else
            {
                #region 选中[明细]Tab的场合

                #region 导出全部[库存明细Grid]

                List<InventoryDetailReportUIModel> resultAllList = new List<InventoryDetailReportUIModel>();
                _bll.QueryForList(SQLID.RPT_InventoryReport_SQL02, tempConditionDS, resultAllList);

                decimal totalSalesPerforamnceQtyOfCurPage = 0;
                decimal totalSalesPerforamnceAmountOfCurPage = 0;
                decimal totalSalesPerforamnceQtyOfAllPage = 0;
                decimal totalSalesPerforamnceAmountOfAllPage = 0;

                if (resultAllList.Count > 0)
                {
                    InventoryDetailReportUIModel subObject = resultAllList[0];
                    TotalRecordCountOfDetail = (subObject.RecordCount ?? 0);
                    totalSalesPerforamnceQtyOfAllPage = (subObject.TotalInventoryQty ?? 0);
                    totalSalesPerforamnceAmountOfAllPage = (subObject.TotalInventoryAmount ?? 0);
                }
                else
                {
                    TotalRecordCountOfDetail = 0;
                }

                foreach (var loopDetail in resultAllList)
                {
                    totalSalesPerforamnceQtyOfCurPage += (loopDetail.INV_Qty ?? 0);
                    totalSalesPerforamnceAmountOfCurPage += (loopDetail.InventoryAmount ?? 0);
                }
                InventoryDetailReportUIModel curPageTotal = new InventoryDetailReportUIModel
                {
                    WHB_Name = _sumCurPageDesc,
                    INV_Qty = Math.Round(totalSalesPerforamnceQtyOfCurPage, 0),
                    InventoryAmount = Math.Round(totalSalesPerforamnceAmountOfCurPage, 2)
                };
                resultAllList.Add(curPageTotal);

                InventoryDetailReportUIModel allPageTotal = new InventoryDetailReportUIModel
                {
                    WHB_Name = _sumAllPageDesc,
                    INV_Qty = Math.Round(totalSalesPerforamnceQtyOfAllPage, 0),
                    InventoryAmount = Math.Round(totalSalesPerforamnceAmountOfAllPage, 2)
                };
                resultAllList.Add(allPageTotal);

                UltraGrid allGrid = InventoryDetailGrid;
                allGrid.DataSource = resultAllList;
                allGrid.DataBind();

                paramGridName = "库存统计明细";
                base.ExportAllAction(allGrid, paramGridName);

                InventoryDetailGrid.DataSource = _inventoryDetailReportDs;
                InventoryDetailGrid.DataBind();
                #endregion

                #endregion
            }
        }

        /// <summary>
        /// 导出全部
        /// </summary>
        /// <param name="paramGrid"></param>
        /// <param name="paramDefaultFilePath"></param>
        /// <param name="paramGridName"></param>
        private void ExportAllPage(UltraGrid paramGrid, ref string paramDefaultFilePath, string paramGridName = "")
        {
            if (paramGrid == null)
            {
                return;
            }
            if (paramGridName == null)
            {
                return;
            }
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    FileName = $"Export{paramGridName + DateTime.Now.ToString(FormatConst.DATE_TIME_FORMAT_03)}_All",
                    Filter = FormatConst.FILE_TYPE_FILTER_EXCEL_03,
                };
                if (!string.IsNullOrEmpty(paramDefaultFilePath))
                {
                    saveFileDialog.InitialDirectory = paramDefaultFilePath;
                    saveFileDialog.FileName = saveFileDialog.FileName + ".xls";
                }
                else
                {
                    saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                }

                //获取保存文件的上一级目录
                Directory.SetCurrentDirectory(Directory.GetParent(saveFileDialog.FileName).FullName);
                paramDefaultFilePath = Directory.GetCurrentDirectory();

                var gdGridExcelExporter = new UltraGridExcelExporter();
                gdGridExcelExporter.Export(paramGrid).Save(saveFileDialog.FileName);

                MessageBox.Show("导出" + paramGridName + "成功！", SysConst.CHS_MSG_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出" + paramGridName + "失败！", SysConst.CHS_MSG_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 初始化下拉框

            //组织
            _orgList = LoginInfoDAX.OrgList;
            mcbWhere_InventoryOrgID.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
            mcbWhere_InventoryOrgID.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
            mcbWhere_InventoryOrgID.DataSource = _orgList;

            //仓库
            _warehouseList = CacheDAX.Get(CacheDAX.ConfigDataKey.Warehouse) as List<MDLPIS_Warehouse>;
            mcbWhere_WarehouseName.DisplayMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name;
            mcbWhere_WarehouseName.ValueMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_ID;
            mcbWhere_WarehouseName.DataSource = _warehouseList;

            #endregion

            #region 查询条件初始化
            //库存组织
            mcbWhere_InventoryOrgID.Clear();
            //仓库名称
            mcbWhere_WarehouseName.Clear();
            //入库时间开始
            dtWhere_StockInTimeStart.Value = null;
            //入库时间结束
            dtWhere_StockInTimeEnd.Value = null;
            //给 库存组织 设置焦点
            mcbWhere_InventoryOrgID.Focus();
            #endregion

            #region Grid初始化
            _inventoryByNameTotalReportDs = new List<InventoryTotalReportUIModel>();
            InventoryByNameReportGrid.DataSource = _inventoryByNameTotalReportDs;
            InventoryByNameReportGrid.DataBind();

            _inventoryByNameAndBrandTotalReportDs = new List<InventoryTotalReportUIModel>();
            InventoryByNameAndBrandReportGrid.DataSource = _inventoryByNameAndBrandTotalReportDs;
            InventoryByNameAndBrandReportGrid.DataBind();

            _inventoryDetailReportDs = new List<InventoryDetailReportUIModel>();
            InventoryDetailGrid.DataSource = _inventoryDetailReportDs;
            InventoryDetailGrid.DataBind();
            #endregion
        }

        #region [根据配件名称统计库存]相关方法

        /// <summary>
        /// 将Grid中选中的数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetByNameGridDataToCardCtrls()
        {
            //判断是否允许将【列表】Grid数据设置到【详情】Tab内的对应控件
            if (!IsAllowSetByNameGridDataToCard())
            {
                return;
            }
            var activeRowIndex = InventoryByNameReportGrid.ActiveRow.Index;
            //当前选中汇总行
            var curActiveTotalRow = _inventoryByNameTotalReportDs.FirstOrDefault(
                    x =>
                        x.OrgID == InventoryByNameReportGrid.Rows[activeRowIndex].Cells["OrgID"].Value?.ToString() &&
                        x.WarehouseID == InventoryByNameReportGrid.Rows[activeRowIndex].Cells["WarehouseID"].Value?.ToString() &&
                        x.AutoPartsName == InventoryByNameReportGrid.Rows[activeRowIndex].Cells["AutoPartsName"].Value?.ToString());
            if (curActiveTotalRow != null
                && !string.IsNullOrEmpty(curActiveTotalRow.OrgID))
            {
                //查询汇总行对应的明细
                QueryInventoryDetailReport(curActiveTotalRow);
                //选中【明细】Tab
                tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;
            }
        }

        /// <summary>
        /// 是否允许将【列表】Grid数据设置到【详情】Tab内的对应控件
        /// </summary>
        /// <returns>true:允许；false：不允许</returns>
        private bool IsAllowSetByNameGridDataToCard()
        {
            if (InventoryByNameReportGrid.ActiveRow == null || InventoryByNameReportGrid.ActiveRow.Index < 0)
            {
                return false;
            }
            if (InventoryByNameReportGrid.DisplayLayout.Bands[0].SortedColumns.Count > 0)
            {
                foreach (UltraGridColumn loopColumn in InventoryByNameReportGrid.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (loopColumn.IsGroupByColumn)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion

        #region [根据配件名称和品牌统计库存]相关方法

        /// <summary>
        /// 将Grid中选中的数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetByNameAndBrandGridDataToCardCtrls()
        {
            //判断是否允许将【列表】Grid数据设置到【详情】Tab内的对应控件
            if (!IsAllowSetByNameAndBrandGridDataToCard())
            {
                return;
            }
            var activeRowIndex = InventoryByNameAndBrandReportGrid.ActiveRow.Index;
            //当前选中汇总行
            var curActiveTotalRow = _inventoryByNameTotalReportDs.FirstOrDefault(
                    x =>
                        x.OrgID == InventoryByNameAndBrandReportGrid.Rows[activeRowIndex].Cells["OrgID"].Value?.ToString() &&
                        x.WarehouseID == InventoryByNameAndBrandReportGrid.Rows[activeRowIndex].Cells["WarehouseID"].Value?.ToString() &&
                        x.AutoPartsName == InventoryByNameReportGrid.Rows[activeRowIndex].Cells["AutoPartsName"].Value?.ToString() &&
                        x.AutoPartsBrand == InventoryByNameReportGrid.Rows[activeRowIndex].Cells["AutoPartsBrand"].Value?.ToString());
            if (curActiveTotalRow != null
                && !string.IsNullOrEmpty(curActiveTotalRow.OrgID))
            {
                //查询汇总行对应的明细
                QueryInventoryDetailReport(curActiveTotalRow);
                //选中【明细】Tab
                tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;
            }
        }

        /// <summary>
        /// 是否允许将【列表】Grid数据设置到【详情】Tab内的对应控件
        /// </summary>
        /// <returns>true:允许；false：不允许</returns>
        private bool IsAllowSetByNameAndBrandGridDataToCard()
        {
            if (InventoryByNameAndBrandReportGrid.ActiveRow == null || InventoryByNameAndBrandReportGrid.ActiveRow.Index < 0)
            {
                return false;
            }
            if (InventoryByNameAndBrandReportGrid.DisplayLayout.Bands[0].SortedColumns.Count > 0)
            {
                foreach (UltraGridColumn loopColumn in InventoryByNameAndBrandReportGrid.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (loopColumn.IsGroupByColumn)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion

        #endregion

    }
}
