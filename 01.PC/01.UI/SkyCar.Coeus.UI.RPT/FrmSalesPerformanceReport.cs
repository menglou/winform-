using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
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
    /// 销售业绩统计报表
    /// </summary>
    public partial class FrmSalesPerformanceReport : BaseFormCardListDetail<SalesPerformanceTotalReportUIModel, SalesPerformanceReportQCModel, SalesPerformanceTotalReportUIModel>
    {
        #region 全局变量

        /// <summary>
        /// 销售业绩统计报表BLL
        /// </summary>
        private SalesPerformanceReportBLL _bll = new SalesPerformanceReportBLL();

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
        /// 业务员数据源
        /// </summary>
        List<MDLSM_User> _salesByList = new List<MDLSM_User>();

        #endregion

        #region Grid数据源
        /// <summary>
        /// 销售业绩统计汇总数据源
        /// </summary>
        private List<SalesPerformanceTotalReportUIModel> _salesPerformanceTotalReportDS = new List<SalesPerformanceTotalReportUIModel>();
        /// <summary>
        /// 销售业绩统计明细数据源
        /// </summary>
        private List<SalesPerformanceDetailReportUIModel> _salesPerformanceDetailReportDS = new List<SalesPerformanceDetailReportUIModel>();

        #endregion

        #region 汇总分页
        private UltraToolbarsManager _toolBarPagingOfTotal = new UltraToolbarsManager();
        /// <summary>
        /// 翻页ToolBar
        /// </summary>
        public new UltraToolbarsManager ToolBarPagingOfTotal
        {
            get { return _toolBarPagingOfTotal; }
            set
            {
                _toolBarPagingOfTotal = value;
                //初始化翻页
                SetBarPaging(TotalRecordCountOfTotal);
            }
        }
        /// <summary>
        /// 当前页面索引值
        /// </summary>
        public new int PageIndexOfTotal = 1;
        /// <summary>
        /// 总记录条数
        /// </summary>
        public new int TotalRecordCountOfTotal = 0;
        /// <summary>
        /// 页面大小
        /// </summary>
        public new int PageSizeOfTotal = 40;
        /// <summary>
        /// 总页面数
        /// </summary>
        public new int TotalPageCountOfTotal = 0;
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
        public FrmSalesPerformanceReport()
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
            base.ToolBarPaging = this.toolBarPagingTotal;
            ToolBarPagingOfTotal = this.toolBarPagingTotal;
            ToolBarPagingOfDetail = this.toolBarPagingDetail;
            //查询委托（基类控制翻页用）
            base.ExecuteQuery = QueryAction;
            //工具栏（动作）单击事件
            this.toolBarActionAndNavigate.ToolClick += new ToolClickEventHandler(base.toolBarActionAndNavigate_ToolClick);
            //工具栏（翻页）单击事件
            this.toolBarPagingTotal.ToolClick += new ToolClickEventHandler(ToolBarPaging_ToolClick);
            this.toolBarPagingDetail.ToolClick += new ToolClickEventHandler(ToolBarPaging_ToolClick);
            //工具栏（翻页）[当前页]值改变事件
            this.toolBarPagingTotal.ToolValueChanged += new ToolEventHandler(ToolBarPaging_ToolValueChanged);
            this.toolBarPagingDetail.ToolValueChanged += new ToolEventHandler(ToolBarPaging_ToolValueChanged);

            #region 设置页面大小文本框
            TextBoxTool pageSizeOfTotalTextBox = null;
            foreach (var loopToolControl in this.toolBarPagingTotal.Tools)
            {
                if (loopToolControl.Key == SysConst.EN_PAGESIZE)
                {
                    pageSizeOfTotalTextBox = (TextBoxTool)loopToolControl;
                }
            }
            if (pageSizeOfTotalTextBox != null)
            {
                pageSizeOfTotalTextBox.Text = PageSizeOfTotal.ToString();
                pageSizeOfTotalTextBox.AfterToolExitEditMode += PageSizeTextBoxTool_AfterToolExitEditMode;
            }

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
                pageSizeOfDetailTextBox.AfterToolExitEditMode += PageSizeTextBoxTool_AfterToolExitEditMode;
            }
            #endregion

            //设置总页数
            SetTotalPageCountAndTotalRecordCountOfTotal(0);
            SetTotalPageCountAndTotalRecordCountOfDetail(0);
            //this.toolBarPagingTotal.Tools[SysConst.EN_PAGECOUNT].SharedProps.Caption = TotalPageCountOfTotal.ToString();
            //this.toolBarPagingDetail.Tools[SysConst.EN_PAGECOUNT].SharedProps.Caption = TotalPageCountOfDetail.ToString();

            //初始化【列表】Tab内控件
            InitializeListTabControls();

            //设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
            //根据选中的Tab，设置动作按钮[是否可用]（在系统权限的基础上进行控制）
            var exportEnable = MenuGroupActionList.FirstOrDefault(x => x.Act_Key == SystemActionEnum.Code.EXPORT) !=
                    null ? true : false;
            SetActionEnable(SystemActionEnum.Code.EXPORT, exportEnable);
            SetActionEnable(SystemActionEnum.Code.PRINT, true);
            SetActionEnable(SystemActionEnum.Code.QUERY, true);
            #endregion
        }

        /// <summary>
        /// 【列表】Grid的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_DoubleClick(object sender, EventArgs e)
        {
            //将Grid中选中的数据赋值给【详情】Tab内的对应控件
            SetGridDataToCardCtrls();
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
        /// 【列表】Grid的CellChange事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_CellChange(object sender, CellEventArgs e)
        {
            SalesPerforamnceReportGrid.UpdateData();
        }

        /// <summary>
        /// 销售时间终了ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_SalesTimeEnd_ValueChanged(object sender, EventArgs e)
        {
            if (this.dtWhere_SalesTimeEnd.Value != null &&
                this.dtWhere_SalesTimeEnd.DateTime.Hour == 0 &&
                this.dtWhere_SalesTimeEnd.DateTime.Minute == 0 &&
                this.dtWhere_SalesTimeEnd.DateTime.Second == 0)
            {
                DateTime newDateTime = new DateTime(this.dtWhere_SalesTimeEnd.DateTime.Year, this.dtWhere_SalesTimeEnd.DateTime.Month, this.dtWhere_SalesTimeEnd.DateTime.Day, 23, 59, 59);
                this.dtWhere_SalesTimeEnd.DateTime = newDateTime;
            }
        }

        #endregion

        #region 重写基类方法

        #region 动作按钮
        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            //1.设置查询条件（翻页相关属性不用设置）
            base.ConditionDS = new SalesPerformanceReportQCModel()
            {
                //业务员ID
                SalesByID = mcbWhere_SalesByName.SelectedValue,
            };

            //组织ID
            if (string.IsNullOrEmpty(mcbWhere_SalesOrg.SelectedValue))
            {
                var tmpOrgList = _orgList.Select(p => p.Org_ID).ToList();

                if (tmpOrgList.Count > 0)
                {
                    ConditionDS.OrgIdList = string.Join(SysConst.Semicolon_DBC, tmpOrgList);
                }
            }
            else
            {
                ConditionDS.OrgIdList = mcbWhere_SalesOrg.SelectedValue;
            }

            //销售时间-开始
            if (this.dtWhere_SalesTimeStart.Value != null)
            {
                ConditionDS.StartTime = this.dtWhere_SalesTimeStart.DateTime;
            }
            else
            {
                ConditionDS.StartTime = null;
            }
            //销售时间-终了
            if (this.dtWhere_SalesTimeEnd.Value != null)
            {
                ConditionDS.EndTime = this.dtWhere_SalesTimeEnd.DateTime;
            }
            else
            {
                ConditionDS.EndTime = null;
            }

            //2.查询并绑定数据源
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                //查询销售业绩汇总
                QuerySalesPerformanceTotalReport();
            }
            else
            {
                //查询销售业绩明细
                QuerySalesPerformanceDetailReport();
            }
        }

        #region 查询汇总
        /// <summary>
        /// 查询销售业绩汇总统计
        /// </summary>
        private void QuerySalesPerformanceTotalReport()
        {
            try
            {
                SalesPerformanceReportQCModel argsModel = ConditionDS;
                argsModel.PageSize = PageSizeOfTotal;
                argsModel.PageIndex = PageIndexOfTotal;
                //SqlId
                argsModel.SqlId = SQLID.RPT_SalesPerformanceReport_SQL01;

                _salesPerformanceTotalReportDS = new List<SalesPerformanceTotalReportUIModel>();
                _bll.QueryForList(argsModel.SqlId, argsModel, _salesPerformanceTotalReportDS);

                decimal totalSalesAmountOfCurPage = 0;
                decimal totalCostAmountOfCurPage = 0;
                decimal totalGrossProfitAmountOfCurPage = 0;
                decimal totalSalesReturnAmountOfCurPage = 0;
                decimal totalSalesQtyOfCurPage = 0;
                decimal totalReturnQtyOfCurPage = 0;

                decimal totalSalesAmountOfAllPage = 0;
                decimal totalCostAmountOfAllPage = 0;
                decimal totalGrossProfitAmountOfAllPage = 0;
                decimal totalSalesReturnAmountOfAllPage = 0;
                decimal totalSalesQtyOfAllPage = 0;
                decimal totalReturnQtyOfAllPage = 0;

                if (_salesPerformanceTotalReportDS.Count > 0)
                {
                    SalesPerformanceTotalReportUIModel subObject = _salesPerformanceTotalReportDS[0];
                    TotalRecordCountOfTotal = subObject.RecordCount ?? 0;
                    totalSalesAmountOfAllPage = (subObject.TotalSalesAmount ?? 0);
                    totalCostAmountOfAllPage = (subObject.TotalCostAmount ?? 0);
                    totalGrossProfitAmountOfAllPage = (subObject.TotalGrossProfitAmount ?? 0);
                    totalSalesReturnAmountOfAllPage = (subObject.TotalSalesReturnAmount ?? 0);

                    totalSalesQtyOfAllPage = (subObject.TotalSalesQty ?? 0);
                    totalReturnQtyOfAllPage = (subObject.TotalSalesReturnQty ?? 0);
                }
                else
                {
                    TotalRecordCountOfTotal = 0;
                }

                foreach (var loopSotckInTotalItem in _salesPerformanceTotalReportDS)
                {
                    totalSalesAmountOfCurPage += (loopSotckInTotalItem.SalesAmount ?? 0);
                    totalCostAmountOfCurPage += (loopSotckInTotalItem.CostAmount ?? 0);
                    totalGrossProfitAmountOfCurPage += (loopSotckInTotalItem.GrossProfitAmount ?? 0);
                    totalSalesReturnAmountOfCurPage += (loopSotckInTotalItem.SalesReturnAmount ?? 0);

                    totalSalesQtyOfCurPage += (loopSotckInTotalItem.SalesQty ?? 0);
                    totalReturnQtyOfCurPage += (loopSotckInTotalItem.SalesReturnQty ?? 0);
                }
                SalesPerformanceTotalReportUIModel curPageTotal = new SalesPerformanceTotalReportUIModel
                {
                    SalesByName = _sumCurPageDesc,
                    SalesAmount = Math.Round(totalSalesAmountOfCurPage, 2),
                    CostAmount = Math.Round(totalCostAmountOfCurPage, 2),
                    GrossProfitAmount = Math.Round(totalGrossProfitAmountOfCurPage, 2),
                    SalesReturnAmount = Math.Round(totalSalesReturnAmountOfCurPage, 2),

                    SalesQty = Math.Round(totalSalesQtyOfCurPage, 2),
                    SalesReturnQty = Math.Round(totalReturnQtyOfCurPage, 2)
                };
                _salesPerformanceTotalReportDS.Add(curPageTotal);

                SalesPerformanceTotalReportUIModel allPageTotal = new SalesPerformanceTotalReportUIModel
                {
                    SalesByName = _sumAllPageDesc,
                    SalesAmount = Math.Round(totalSalesAmountOfAllPage, 2),
                    CostAmount = Math.Round(totalCostAmountOfAllPage, 2),
                    GrossProfitAmount = Math.Round(totalGrossProfitAmountOfAllPage, 2),
                    SalesReturnAmount = Math.Round(totalSalesReturnAmountOfAllPage, 2),

                    SalesQty = Math.Round(totalSalesQtyOfAllPage, 0),
                    SalesReturnQty = Math.Round(totalReturnQtyOfAllPage, 0)
                };
                _salesPerformanceTotalReportDS.Add(allPageTotal);

                //重新计算[总页数]，设置最新[allPageTotal]
                SetTotalPageCountAndTotalRecordCountOfTotal(TotalRecordCountOfTotal);
                //设置翻页按钮状态
                SetPageButtonStatusOfTotal();

                SalesPerforamnceReportGrid.DataSource = _salesPerformanceTotalReportDS;
                SalesPerforamnceReportGrid.DataBind();
                SalesPerforamnceReportGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
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
        private void SetTotalPageCountAndTotalRecordCountOfTotal(int paramTotalRecordCount)
        {
            if (ToolBarPagingOfTotal != null)
            {
                if (paramTotalRecordCount > 0)
                {
                    TotalRecordCountOfTotal = paramTotalRecordCount;
                    int? remainder = TotalRecordCountOfTotal % PageSizeOfTotal;
                    if (remainder > 0)
                    {
                        TotalPageCountOfTotal = TotalRecordCountOfTotal / PageSizeOfTotal + 1;
                    }
                    else
                    {
                        TotalPageCountOfTotal = TotalRecordCountOfTotal / PageSizeOfTotal;
                    }
                }
                else
                {
                    PageIndexOfTotal = 1;
                    TotalPageCountOfTotal = 1;
                    TotalRecordCountOfTotal = 0;
                }
                ((TextBoxTool)(ToolBarPagingOfTotal.Tools[SysConst.EN_PAGEINDEX])).Text = PageIndexOfTotal.ToString();
                ToolBarPagingOfTotal.Tools[SysConst.EN_PAGECOUNT].SharedProps.Caption = TotalPageCountOfTotal.ToString();
            }
        }

        /// <summary>
        /// 设置翻页按钮状态
        /// </summary>
        private void SetPageButtonStatusOfTotal()
        {
            if (ToolBarPagingOfTotal != null)
            {
                if (PageIndexOfTotal == 0 || TotalRecordCountOfTotal == 0)
                {
                    ToolBarPagingOfTotal.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingOfTotal.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingOfTotal.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingOfTotal.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;

                    return;
                }
                if (PageIndexOfTotal == 1 && TotalRecordCountOfTotal <= PageSizeOfTotal)
                {
                    ToolBarPagingOfTotal.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingOfTotal.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingOfTotal.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingOfTotal.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (PageIndexOfTotal == 1 && TotalRecordCountOfTotal > PageSizeOfTotal)
                {
                    ToolBarPagingOfTotal.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingOfTotal.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingOfTotal.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingOfTotal.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (PageIndexOfTotal != 1 && TotalRecordCountOfTotal > PageSizeOfTotal * PageIndexOfTotal)
                {
                    ToolBarPagingOfTotal.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingOfTotal.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingOfTotal.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingOfTotal.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
                }
                else if (PageIndexOfTotal != 1 && TotalRecordCountOfTotal <= PageSizeOfTotal * PageIndexOfTotal)
                {
                    ToolBarPagingOfTotal.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingOfTotal.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingOfTotal.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingOfTotal.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
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
        /// 查询销售业绩明细统计
        /// </summary>
        /// <param name="paramTotalReport">汇总行</param>
        private void QuerySalesPerformanceDetailReport(SalesPerformanceTotalReportUIModel paramTotalReport = null)
        {
            try
            {
                SalesPerformanceReportQCModel argsModel = ConditionDS;
                argsModel.PageSize = PageSizeOfDetail;
                argsModel.PageIndex = PageIndexOfDetail;
                argsModel.SqlId = SQLID.RPT_SalesPerformanceReport_SQL02;
                if (paramTotalReport != null)
                {
                    argsModel.OrgIdList = paramTotalReport.OrgID;
                    argsModel.SalesByID = paramTotalReport.SalesByID;
                }

                _salesPerformanceDetailReportDS = new List<SalesPerformanceDetailReportUIModel>();
                _bll.QueryForList(argsModel.SqlId, argsModel, _salesPerformanceDetailReportDS);

                decimal totalSalesPerformanceQtyOfCurPage = 0;
                decimal totalSalesPerformanceAmountOfCurPage = 0;
                decimal totalSalesPerformanceCostAmountOfCurPage = 0;
                decimal totalSalesPerformanceGrossProfitAmountOfCurPage = 0;

                decimal totalSalesPerformanceQtyOfAllPage = 0;
                decimal totalSalesPerformanceAmountOfAllPage = 0;
                decimal totalSalesPerformanceCostAmountOfAllPage = 0;
                decimal totalSalesPerformanceGrossProfitAmountOfAllPage = 0;

                if (_salesPerformanceDetailReportDS.Count > 0)
                {
                    SalesPerformanceDetailReportUIModel subObject = _salesPerformanceDetailReportDS[0];
                    TotalRecordCountOfDetail = (subObject.RecordCount ?? 0);
                    totalSalesPerformanceQtyOfAllPage = (subObject.TotalSalesQty ?? 0);
                    totalSalesPerformanceAmountOfAllPage = (subObject.TotalSalesAmount ?? 0);
                    totalSalesPerformanceCostAmountOfAllPage = (subObject.TotalCostAmount ?? 0);
                    totalSalesPerformanceGrossProfitAmountOfAllPage = (subObject.TotalGrossProfitAmount ?? 0);
                }
                else
                {
                    TotalRecordCountOfDetail = 0;
                }

                foreach (var loopDetail in _salesPerformanceDetailReportDS)
                {
                    totalSalesPerformanceQtyOfCurPage += (loopDetail.SalesTotalQty ?? 0);
                    totalSalesPerformanceAmountOfCurPage += (loopDetail.SalesTotalAmount ?? 0);
                    totalSalesPerformanceCostAmountOfCurPage += (loopDetail.CostTotalAmount ?? 0);
                    totalSalesPerformanceGrossProfitAmountOfCurPage += (loopDetail.GrossProfitTotalAmount ?? 0);
                }
                SalesPerformanceDetailReportUIModel curPageTotal = new SalesPerformanceDetailReportUIModel
                {
                    SO_CustomerName = _sumCurPageDesc,
                    SalesTotalQty = Math.Round(totalSalesPerformanceQtyOfCurPage, 0),
                    SalesTotalAmount = Math.Round(totalSalesPerformanceAmountOfCurPage, 2),
                    CostTotalAmount = Math.Round(totalSalesPerformanceCostAmountOfCurPage, 2),
                    GrossProfitTotalAmount = Math.Round(totalSalesPerformanceGrossProfitAmountOfCurPage, 2)
                };
                _salesPerformanceDetailReportDS.Add(curPageTotal);

                SalesPerformanceDetailReportUIModel allPageTotal = new SalesPerformanceDetailReportUIModel
                {
                    SO_CustomerName = _sumAllPageDesc,
                    SalesTotalQty = Math.Round(totalSalesPerformanceQtyOfAllPage, 0),
                    SalesTotalAmount = Math.Round(totalSalesPerformanceAmountOfAllPage, 2),
                    CostTotalAmount = Math.Round(totalSalesPerformanceCostAmountOfAllPage, 2),
                    GrossProfitTotalAmount = Math.Round(totalSalesPerformanceGrossProfitAmountOfAllPage, 2)
                };
                _salesPerformanceDetailReportDS.Add(allPageTotal);

                //重新计算[总页数]，设置最新[总记录条数]
                SetTotalPageCountAndTotalRecordCountOfDetail(TotalRecordCountOfDetail);
                //设置翻页按钮状态
                SetPageButtonStatusOfDetail();

                SalesPerformanceDetailGrid.DataSource = _salesPerformanceDetailReportDS;
                SalesPerformanceDetailGrid.DataBind();
                SalesPerformanceDetailGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
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
        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public new void ToolBarPaging_ToolClick(object sender, ToolClickEventArgs e)
        {
            var funcName = "ToolBarPaging_ToolClick";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                if (ToolBarPagingOfTotal != null)
                {
                    switch (e.Tool.Key)
                    {
                        //第一页
                        case SysConst.EN_FIRSTPAGE:
                            ((TextBoxTool)(ToolBarPagingOfTotal.Tools[SysConst.EN_PAGEINDEX])).Text =
                                SysConst.NUMBER_ONE.ToString();
                            break;
                        // 前一页
                        case SysConst.EN_FORWARDPAGE:
                            ((TextBoxTool)(ToolBarPagingOfTotal.Tools[SysConst.EN_PAGEINDEX])).Text = (PageIndexOfTotal - 1).ToString();
                            break;
                        // 下一页
                        case SysConst.EN_NEXTPAGE:
                            ((TextBoxTool)(ToolBarPagingOfTotal.Tools[SysConst.EN_PAGEINDEX])).Text = (PageIndexOfTotal + 1).ToString();
                            break;
                        // 最后一页
                        case SysConst.EN_LASTPAGE:
                            ((TextBoxTool)(ToolBarPagingOfTotal.Tools[SysConst.EN_PAGEINDEX])).Text = TotalPageCountOfTotal.ToString();
                            break;
                    }
                }
            }
            else
            {
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
            }

            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 翻页ToolBar的值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public new void ToolBarPaging_ToolValueChanged(object sender, ToolEventArgs e)
        {
            var funcName = "ToolBarPaging_ToolValueChanged";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
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
                    else if (tmpPageIndex > TotalPageCountOfTotal)
                    {
                        ((TextBoxTool)(e.Tool)).Text = TotalPageCountOfTotal.ToString();
                        ((TextBoxTool)(e.Tool)).SelectionLength = TotalPageCountOfTotal.ToString().Length;
                        return;
                    }

                    #region 当前页码设置
                    if (Convert.ToInt32(strValue) <= 0)
                    {
                        ((TextBoxTool)(e.Tool)).Text = SysConst.NUMBER_ONE.ToString();
                    }
                    else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) >= TotalPageCountOfTotal)
                    {
                        ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(TotalPageCountOfTotal).ToString();
                    }
                    else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) < TotalPageCountOfTotal)
                    {
                        ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(strValue).ToString();
                    }
                    #endregion

                    PageIndexOfTotal = tmpPageIndex;
                    if (tmpPageIndex <= 0)
                    {
                        PageIndexOfTotal = 1;
                    }
                    ExecuteQuery?.Invoke();
                }
            }
            else
            {
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
            }
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
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
                PageSizeOfTotal = tmpPageSize;
            }
            else
            {
                PageSizeOfDetail = tmpPageSize;
            }

            ExecuteQuery?.Invoke();
        }

        #endregion

        /// <summary>
        /// 清空
        /// </summary>
        public override void ClearAction()
        {
            #region 查询条件初始化
            //销售组织
            mcbWhere_SalesOrg.Clear();
            //业务员
            mcbWhere_SalesByName.Clear();
            //销售时间开始
            dtWhere_SalesTimeStart.Value = null;
            //销售时间结束
            dtWhere_SalesTimeEnd.Value = null;
            //给 销售组织 设置焦点
            mcbWhere_SalesOrg.Focus();
            #endregion

            #region Grid初始化
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                _salesPerformanceTotalReportDS = new List<SalesPerformanceTotalReportUIModel>();
                SalesPerforamnceReportGrid.DataSource = _salesPerformanceTotalReportDS;
                SalesPerforamnceReportGrid.DataBind();
            }
            else
            {
                _salesPerformanceDetailReportDS = new List<SalesPerformanceDetailReportUIModel>();
                SalesPerformanceDetailGrid.DataSource = _salesPerformanceDetailReportDS;
                SalesPerformanceDetailGrid.DataBind();
            }
            #endregion
        }

        /// <summary>
        /// 导出当前页
        /// </summary>
        public override void ExportAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = tabControlFull.Tabs[SysConst.EN_LIST].Selected ? "销售业绩汇总" : "销售业绩明细";
            base.ExportAction(tabControlFull.Tabs[SysConst.EN_LIST].Selected ? SalesPerforamnceReportGrid : SalesPerformanceDetailGrid, paramGridName);
        }

        /// <summary>
        /// 导出全部
        /// </summary>
        public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = tabControlFull.Tabs[SysConst.EN_LIST].Selected ? "销售业绩汇总" : "销售业绩明细";
            SalesPerformanceReportQCModel tempConditionDS = new SalesPerformanceReportQCModel()
            {
                //SqlId
                SqlId = tabControlFull.Tabs[SysConst.EN_LIST].Selected ? SQLID.RPT_SalesPerformanceReport_SQL01 : SQLID.RPT_SalesPerformanceReport_SQL02,
                //业务员ID
                SalesByID = mcbWhere_SalesByName.SelectedValue,
                PageIndex = 1,
                PageSize = null
            };

            //组织ID
            if (string.IsNullOrEmpty(mcbWhere_SalesOrg.SelectedValue))
            {
                var tmpOrgList = _orgList.Select(p => p.Org_ID).ToList();

                if (tmpOrgList.Count > 0)
                {
                    ConditionDS.OrgIdList = string.Join(SysConst.Semicolon_DBC, tmpOrgList);
                }
            }
            else
            {
                ConditionDS.OrgIdList = mcbWhere_SalesOrg.SelectedValue;
            }

            if (this.dtWhere_SalesTimeStart.Value != null)
            {
                //销售时间-开始
                tempConditionDS.StartTime = this.dtWhere_SalesTimeStart.DateTime;
            }
            if (this.dtWhere_SalesTimeEnd.Value != null)
            {
                //销售时间-终了
                tempConditionDS.EndTime = this.dtWhere_SalesTimeEnd.DateTime;
            }

            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                List<SalesPerformanceTotalReportUIModel> resultAllList = new List<SalesPerformanceTotalReportUIModel>();
                _bll.QueryForList(SQLID.RPT_SalesPerformanceReport_SQL01, tempConditionDS, resultAllList);

                decimal totalAccountPayableAmountOfCurPage = 0;
                decimal totalPaidAmountOfCurPage = 0;
                decimal totalAccountPayableAmountOfAllPage = 0;
                decimal totalPaidAmountOfAllPage = 0;

                if (resultAllList.Count > 0)
                {
                    SalesPerformanceTotalReportUIModel subObject = resultAllList[0];
                    totalAccountPayableAmountOfAllPage = (subObject.TotalSalesAmount ?? 0);
                    totalPaidAmountOfAllPage = (subObject.TotalSalesReturnAmount ?? 0);
                }

                foreach (var loopSotckInTotalItem in resultAllList)
                {
                    totalAccountPayableAmountOfCurPage += (loopSotckInTotalItem.SalesAmount ?? 0);
                    totalPaidAmountOfCurPage += (loopSotckInTotalItem.SalesReturnAmount ?? 0);
                }

                SalesPerformanceTotalReportUIModel curPageTotal = new SalesPerformanceTotalReportUIModel
                {
                    SalesByName = _sumCurPageDesc,
                    SalesAmount = Math.Round(totalAccountPayableAmountOfCurPage, 2),
                    SalesReturnAmount = Math.Round(totalPaidAmountOfCurPage, 2)
                };
                resultAllList.Add(curPageTotal);

                SalesPerformanceTotalReportUIModel allPageTotal = new SalesPerformanceTotalReportUIModel
                {
                    SalesByName = _sumAllPageDesc,
                    SalesAmount = Math.Round(totalAccountPayableAmountOfAllPage, 2),
                    SalesReturnAmount = Math.Round(totalPaidAmountOfAllPage, 2)
                };
                resultAllList.Add(allPageTotal);

                UltraGrid allGrid = SalesPerforamnceReportGrid;
                allGrid.DataSource = resultAllList;
                allGrid.DataBind();

                base.ExportAllAction(allGrid, paramGridName);

                SalesPerforamnceReportGrid.DataSource = _salesPerformanceTotalReportDS;
                SalesPerforamnceReportGrid.DataBind();
            }
            else
            {
                List<SalesPerformanceDetailReportUIModel> resultAllList = new List<SalesPerformanceDetailReportUIModel>();
                _bll.QueryForList(SQLID.RPT_SalesPerformanceReport_SQL02, tempConditionDS, resultAllList);

                decimal totalSalesPerforamnceQtyOfCurPage = 0;
                decimal totalSalesPerforamnceAmountOfCurPage = 0;
                decimal totalSalesPerforamnceQtyOfAllPage = 0;
                decimal totalSalesPerforamnceAmountOfAllPage = 0;

                if (resultAllList.Count > 0)
                {
                    SalesPerformanceDetailReportUIModel subObject = resultAllList[0];
                    TotalRecordCountOfDetail = (subObject.RecordCount ?? 0);
                    totalSalesPerforamnceQtyOfAllPage = (subObject.TotalSalesQty ?? 0);
                    totalSalesPerforamnceAmountOfAllPage = (subObject.TotalSalesAmount ?? 0);
                }
                else
                {
                    TotalRecordCountOfDetail = 0;
                }

                foreach (var loopDetail in resultAllList)
                {
                    totalSalesPerforamnceQtyOfCurPage += (loopDetail.SalesTotalQty ?? 0);
                    totalSalesPerforamnceAmountOfCurPage += (loopDetail.SalesTotalAmount ?? 0);
                }
                SalesPerformanceDetailReportUIModel curPageTotal = new SalesPerformanceDetailReportUIModel
                {
                    SO_CustomerName = _sumCurPageDesc,
                    SalesTotalQty = Math.Round(totalSalesPerforamnceQtyOfCurPage, 0),
                    SalesTotalAmount = Math.Round(totalSalesPerforamnceAmountOfCurPage, 2)
                };
                resultAllList.Add(curPageTotal);

                SalesPerformanceDetailReportUIModel allPageTotal = new SalesPerformanceDetailReportUIModel
                {
                    SO_CustomerName = _sumAllPageDesc,
                    SalesTotalQty = Math.Round(totalSalesPerforamnceQtyOfAllPage, 0),
                    SalesTotalAmount = Math.Round(totalSalesPerforamnceAmountOfAllPage, 2)
                };
                resultAllList.Add(allPageTotal);

                UltraGrid allGrid = SalesPerformanceDetailGrid;
                allGrid.DataSource = resultAllList;
                allGrid.DataBind();

                base.ExportAllAction(allGrid, paramGridName);

                SalesPerformanceDetailGrid.DataSource = _salesPerformanceDetailReportDS;
                SalesPerformanceDetailGrid.DataBind();
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        public override void PrintAction()
        {
            try
            {
                if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
                {
                    if (_salesPerformanceTotalReportDS == null || _salesPerformanceTotalReportDS.Count == 0)
                    {
                        MessageBoxs.Show(Trans.RPT, this.ToString(), "请查询要打印的数据。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    //待打印的销售业绩汇总
                    List<SalesPerformanceTotalReportUIModel> salesPerformanceTotalReportToPrint = new List<SalesPerformanceTotalReportUIModel>();
                    foreach (var loopTotal in _salesPerformanceTotalReportDS)
                    {
                        if (loopTotal.SalesByName == _sumAllPageDesc || loopTotal.SalesByName == _sumCurPageDesc)
                        {
                            continue;
                        }
                        salesPerformanceTotalReportToPrint.Add(loopTotal);
                    }

                    Dictionary<string, object> argsViewParams = new Dictionary<string, object>
                    {
                        {ComViewParamKey.APModel.ToString(), salesPerformanceTotalReportToPrint},
                    };

                    FrmViewAndPrintSalesPerformanceTotalReport frmViewAndPrintSalesPerformanceTotalReport = new FrmViewAndPrintSalesPerformanceTotalReport(argsViewParams)
                    {
                        StartPosition = FormStartPosition.CenterScreen
                    };
                    DialogResult dialogResult = frmViewAndPrintSalesPerformanceTotalReport.ShowDialog();
                    if (dialogResult != DialogResult.OK)
                    {
                        return;
                    }
                }
                else
                {
                    if (_salesPerformanceDetailReportDS == null || _salesPerformanceDetailReportDS.Count == 0)
                    {
                        MessageBoxs.Show(Trans.RPT, this.ToString(), "请查询要打印的数据。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    //待打印的销售业绩明细
                    List<SalesPerformanceDetailReportUIModel> salesPerformanceDetailReportToPrint = new List<SalesPerformanceDetailReportUIModel>();
                    foreach (var loopDetail in _salesPerformanceDetailReportDS)
                    {
                        if (loopDetail.SO_CustomerName == _sumAllPageDesc || loopDetail.SO_CustomerName == _sumCurPageDesc)
                        {
                            continue;
                        }
                        salesPerformanceDetailReportToPrint.Add(loopDetail);
                    }

                    Dictionary<string, object> argsViewParams = new Dictionary<string, object>
                    {
                        {ComViewParamKey.APModel.ToString(), salesPerformanceDetailReportToPrint},
                    };

                    FrmViewAndPrintSalesPerformanceDetailReport frmViewAndPrintSalesPerformanceDetailReport = new FrmViewAndPrintSalesPerformanceDetailReport(argsViewParams)
                    {
                        StartPosition = FormStartPosition.CenterScreen
                    };
                    DialogResult dialogResult = frmViewAndPrintSalesPerformanceDetailReport.ShowDialog();
                    if (dialogResult != DialogResult.OK)
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxs.Show(Trans.RPT, this.ToString(), ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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
            mcbWhere_SalesOrg.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
            mcbWhere_SalesOrg.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
            mcbWhere_SalesOrg.DataSource = _orgList;

            //业务员
            _salesByList = CacheDAX.Get(CacheDAX.ConfigDataKey.SystemUser) as List<MDLSM_User>;
            mcbWhere_SalesByName.DisplayMember = SystemTableColumnEnums.SM_User.Code.User_Name;
            mcbWhere_SalesByName.ValueMember = SystemTableColumnEnums.SM_User.Code.User_ID;
            mcbWhere_SalesByName.DataSource = _salesByList;
            #endregion

            #region 查询条件初始化
            //销售组织
            mcbWhere_SalesOrg.Clear();
            //业务员
            mcbWhere_SalesByName.Clear();
            //销售时间开始
            dtWhere_SalesTimeStart.Value = null;
            //销售时间结束
            dtWhere_SalesTimeEnd.Value = null;
            //给 销售组织 设置焦点
            mcbWhere_SalesOrg.Focus();
            #endregion

            #region Grid初始化
            _salesPerformanceTotalReportDS = new List<SalesPerformanceTotalReportUIModel>();
            SalesPerforamnceReportGrid.DataSource = _salesPerformanceTotalReportDS;
            SalesPerforamnceReportGrid.DataBind();

            _salesPerformanceDetailReportDS = new List<SalesPerformanceDetailReportUIModel>();
            SalesPerformanceDetailGrid.DataSource = _salesPerformanceDetailReportDS;
            SalesPerformanceDetailGrid.DataBind();
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
            var activeRowIndex = SalesPerforamnceReportGrid.ActiveRow.Index;
            //当前选中汇总行
            var curActiveTotalRow = _salesPerformanceTotalReportDS.FirstOrDefault(
                    x =>
                        x.OrgID == SalesPerforamnceReportGrid.Rows[activeRowIndex].Cells["OrgID"].Value?.ToString() &&
                        x.SalesByID == SalesPerforamnceReportGrid.Rows[activeRowIndex].Cells["SalesByID"].Value?.ToString());
            if (curActiveTotalRow != null
                && !string.IsNullOrEmpty(curActiveTotalRow.OrgID))
            {
                //查询汇总行对应的明细
                QuerySalesPerformanceDetailReport(curActiveTotalRow);
                //选中【明细】Tab
                tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;
            }
        }

        /// <summary>
        /// 是否允许将【列表】Grid数据设置到【详情】Tab内的对应控件
        /// </summary>
        /// <returns>true:允许；false：不允许</returns>
        private bool IsAllowSetGridDataToCard()
        {
            if (SalesPerforamnceReportGrid.ActiveRow == null || SalesPerforamnceReportGrid.ActiveRow.Index < 0)
            {
                return false;
            }
            if (SalesPerforamnceReportGrid.DisplayLayout.Bands[0].SortedColumns.Count > 0)
            {
                foreach (UltraGridColumn loopColumn in SalesPerforamnceReportGrid.DisplayLayout.Bands[0].SortedColumns)
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
    }
}
