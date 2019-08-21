using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.RPT;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.Common.UIModel;
using SkyCar.Coeus.UIModel.RPT.QCModel;
using SkyCar.Coeus.UIModel.RPT.UIModel;

namespace SkyCar.Coeus.UI.RPT
{
    /// <summary>
    /// 配件入库统计报表
    /// </summary>
    public partial class FrmAutoPartsStockInReport : BaseFormCardListDetail<AutoPartsStockInTotalReportUIModel, AutoPartsStockInReportQCModel, AutoPartsStockInTotalReportUIModel>
    {
        #region 全局变量

        /// <summary>
        /// 入库统计报表BLL
        /// </summary>
        private AutoPartsStockInReportBLL _bll = new AutoPartsStockInReportBLL();

        /// <summary>
        /// 当前页合计
        /// </summary>
        string _sumCurPageDesc = "当前页合计：";

        /// <summary>
        /// 合计
        /// </summary>
        string _sumAllPageDesc = "合计：";

        #region Grid数据源
        /// <summary>
        /// 组织数据源
        /// </summary>
        List<MDLSM_Organization> _orgList = new List<MDLSM_Organization>();
        /// <summary>
        /// 客户数据源
        /// </summary>
        List<ComClientUIModel> _clientList = new List<ComClientUIModel>();
        /// <summary>
        /// 库存异动类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _inventoryTransList = new List<ComComboBoxDataSourceTC>();

        /// <summary>
        /// 入库统计汇总数据源
        /// </summary>
        private List<AutoPartsStockInTotalReportUIModel> _stockInTotalReportDS = new List<AutoPartsStockInTotalReportUIModel>();
        /// <summary>
        /// 入库统计明细数据源
        /// </summary>
        private List<AutoPartsStockInDetailReportUIModel> _stockInDetailReportDS = new List<AutoPartsStockInDetailReportUIModel>();

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

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmAutoPartsStockInReport构造方法
        /// </summary>
        public FrmAutoPartsStockInReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAutoPartsStockInReport_Load(object sender, EventArgs e)
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
            StockInReportGrid.UpdateData();
        }

        /// <summary>
        /// 入库时间终了ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_StockIn_TimeEnd_ValueChanged(object sender, EventArgs e)
        {
            if (this.dtWhere_StockIn_TimeEnd.Value != null &&
                this.dtWhere_StockIn_TimeEnd.DateTime.Hour == 0 &&
                this.dtWhere_StockIn_TimeEnd.DateTime.Minute == 0 &&
                this.dtWhere_StockIn_TimeEnd.DateTime.Second == 0)
            {
                DateTime newDateTime = new DateTime(this.dtWhere_StockIn_TimeEnd.DateTime.Year, this.dtWhere_StockIn_TimeEnd.DateTime.Month, this.dtWhere_StockIn_TimeEnd.DateTime.Day, 23, 59, 59);
                this.dtWhere_StockIn_TimeEnd.DateTime = newDateTime;
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
            base.ConditionDS = new AutoPartsStockInReportQCModel()
            {
                //客户ID
                CustomerID = this.mcbWhere_Client_Name.SelectedValue,
                //库存异动类型
                InventoryTransType = cbWhere_InventoryTransType.Text,
            };

            //组织ID
            if (string.IsNullOrEmpty(mcbWhere_StockIn_Org.SelectedValue))
            {
                var tmpOrgList = _orgList.Select(p => p.Org_ID).ToList();

                if (tmpOrgList.Count > 0)
                {
                    ConditionDS.OrgIdList = string.Join(SysConst.Semicolon_DBC, tmpOrgList);
                }
            }
            else
            {
                ConditionDS.OrgIdList = mcbWhere_StockIn_Org.SelectedValue;
            }

            //入库时间-开始
            if (this.dtWhere_StockIn_TimeStart.Value != null)
            {
                ConditionDS.StartTime = this.dtWhere_StockIn_TimeStart.DateTime;
            }
            else
            {
                ConditionDS.StartTime = null;
            }
            //入库时间-终了
            if (this.dtWhere_StockIn_TimeEnd.Value != null)
            {
                ConditionDS.EndTime = this.dtWhere_StockIn_TimeEnd.DateTime;
            }
            else
            {
                ConditionDS.EndTime = null;
            }

            //2.查询并绑定数据源
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                QueryStockInTotalReport();
            }
            else
            {
                QueryStockInDetailReport();
            }
        }

        #region 查询汇总
        /// <summary>
        /// 查询配件入库统计汇总
        /// </summary>
        private void QueryStockInTotalReport()
        {
            try
            {
                AutoPartsStockInReportQCModel argsModel = ConditionDS;
                argsModel.PageSize = PageSizeOfTotal;
                argsModel.PageIndex = PageIndexOfTotal;
                argsModel.SqlId = SQLID.RPT_StockInReport_SQL01;

                _stockInTotalReportDS = new List<AutoPartsStockInTotalReportUIModel>();
                _bll.QueryForList(argsModel.SqlId, argsModel, _stockInTotalReportDS);

                decimal totalAccountPayableAmountOfCurPage = 0;
                decimal totalPaidAmountOfCurPage = 0;
                decimal totalAccountPayableAmountOfAllPage = 0;
                decimal totalPaidAmountOfAllPage = 0;

                if (_stockInTotalReportDS.Count > 0)
                {
                    AutoPartsStockInTotalReportUIModel subObject = _stockInTotalReportDS[0];
                    TotalRecordCountOfTotal = subObject.RecordCount ?? 0;
                    totalAccountPayableAmountOfAllPage = (subObject.TotalAccountPayableAmount ?? 0);
                    totalPaidAmountOfAllPage = (subObject.TotalPaidAmount ?? 0);
                }
                else
                {
                    TotalRecordCountOfTotal = 0;
                }

                foreach (var loopSotckInTotalItem in _stockInTotalReportDS)
                {
                    totalAccountPayableAmountOfCurPage += (loopSotckInTotalItem.APB_AccountPayableAmount ?? 0);
                    totalPaidAmountOfCurPage += (loopSotckInTotalItem.APB_PaidAmount ?? 0);
                }
                AutoPartsStockInTotalReportUIModel curPageTotal = new AutoPartsStockInTotalReportUIModel
                {
                    CustomerName = _sumCurPageDesc,
                    APB_AccountPayableAmount = Math.Round(totalAccountPayableAmountOfCurPage, 2),
                    APB_PaidAmount = Math.Round(totalPaidAmountOfCurPage, 2)
                };
                _stockInTotalReportDS.Add(curPageTotal);

                AutoPartsStockInTotalReportUIModel allPageTotal = new AutoPartsStockInTotalReportUIModel
                {
                    CustomerName = _sumAllPageDesc,
                    APB_AccountPayableAmount = Math.Round(totalAccountPayableAmountOfAllPage, 2),
                    APB_PaidAmount = Math.Round(totalPaidAmountOfAllPage, 2)
                };
                _stockInTotalReportDS.Add(allPageTotal);

                //重新计算[总页数]，设置最新[allPageTotal]
                SetTotalPageCountAndTotalRecordCountOfTotal(TotalRecordCountOfTotal);
                //设置翻页按钮状态
                SetPageButtonStatusOfTotal();

                StockInReportGrid.DataSource = _stockInTotalReportDS;
                StockInReportGrid.DataBind();
                StockInReportGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
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
        /// 查询配件入库统计明细
        /// </summary>
        /// <param name="paramTotalReport">汇总行</param>
        private void QueryStockInDetailReport(AutoPartsStockInTotalReportUIModel paramTotalReport = null)
        {
            try
            {
                AutoPartsStockInReportQCModel argsModel = ConditionDS;
                argsModel.PageSize = PageSizeOfDetail;
                argsModel.PageIndex = PageIndexOfDetail;
                argsModel.SqlId = SQLID.RPT_StockInReport_SQL02;
                if (paramTotalReport != null)
                {
                    argsModel.OrgIdList = paramTotalReport.OrgID;
                    argsModel.CustomerID = paramTotalReport.CustomerID;
                    argsModel.InventoryTransType = paramTotalReport.InventoryTransType;
                }

                _stockInDetailReportDS = new List<AutoPartsStockInDetailReportUIModel>();
                _bll.QueryForList(argsModel.SqlId, argsModel, _stockInDetailReportDS);

                decimal totalStockInQtyOfCurPage = 0;
                decimal totalStockInAmountOfCurPage = 0;
                decimal totalStockInQtyOfAllPage = 0;
                decimal totalStockInAmountOfAllPage = 0;

                if (_stockInDetailReportDS.Count > 0)
                {
                    AutoPartsStockInDetailReportUIModel subObject = _stockInDetailReportDS[0];
                    TotalRecordCountOfDetail = (subObject.RecordCount ?? 0);
                    totalStockInQtyOfAllPage = (subObject.TotalStockInQty ?? 0);
                    totalStockInAmountOfAllPage = (subObject.TotalStockInAmount ?? 0);
                }
                else
                {
                    TotalRecordCountOfDetail = 0;
                }

                foreach (var loopSotckInDetailItem in _stockInDetailReportDS)
                {
                    totalStockInQtyOfCurPage += (loopSotckInDetailItem.SID_Qty ?? 0);
                    totalStockInAmountOfCurPage += (loopSotckInDetailItem.SID_Amount ?? 0);
                }
                AutoPartsStockInDetailReportUIModel curPageTotal = new AutoPartsStockInDetailReportUIModel
                {
                    APA_Specification = _sumCurPageDesc,
                    SID_Qty = Math.Round(totalStockInQtyOfCurPage, 0),
                    SID_Amount = Math.Round(totalStockInAmountOfCurPage, 2)
                };
                _stockInDetailReportDS.Add(curPageTotal);

                AutoPartsStockInDetailReportUIModel allPageTotal = new AutoPartsStockInDetailReportUIModel
                {
                    APA_Specification = _sumAllPageDesc,
                    SID_Qty = Math.Round(totalStockInQtyOfAllPage, 0),
                    SID_Amount = Math.Round(totalStockInAmountOfAllPage, 2)
                };
                _stockInDetailReportDS.Add(allPageTotal);

                //重新计算[总页数]，设置最新[总记录条数]
                SetTotalPageCountAndTotalRecordCountOfDetail(TotalRecordCountOfDetail);
                //设置翻页按钮状态
                SetPageButtonStatusOfDetail();

                StockInDetailGrid.DataSource = _stockInDetailReportDS;
                StockInDetailGrid.DataBind();
                StockInDetailGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
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
                else if (SysConst.EN_PAGESIZE.Equals(e.Tool.Key))
                {
                    //string strValue = ((TextBoxTool)(e.Tool)).Text.Trim();
                    //int tmpPageSize = 0;
                    //if (!int.TryParse(strValue, out tmpPageSize) || tmpPageSize == 0)
                    //{
                    //    ((TextBoxTool)(e.Tool)).Text = SysConst.DEFAULT_PAGESIZE.ToString();
                    //    return;
                    //}
                    //if (tmpPageSize > SysConst.MAX_PAGESIZE)
                    //{
                    //    ((TextBoxTool)(e.Tool)).Text = SysConst.MAX_PAGESIZE.ToString();
                    //    return;
                    //}
                    //PageSizeOfTotal = tmpPageSize;

                    //ExecuteQuery?.Invoke();
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
                else if (SysConst.EN_PAGESIZE.Equals(e.Tool.Key))
                {
                    //string strValue = ((TextBoxTool)(e.Tool)).Text.Trim();
                    //int tmpPageSize = 0;
                    //if (!int.TryParse(strValue, out tmpPageSize) || tmpPageSize == 0)
                    //{
                    //    ((TextBoxTool)(e.Tool)).Text = SysConst.DEFAULT_PAGESIZE.ToString();
                    //    return;
                    //}
                    //if (tmpPageSize > SysConst.MAX_PAGESIZE)
                    //{
                    //    ((TextBoxTool)(e.Tool)).Text = SysConst.MAX_PAGESIZE.ToString();
                    //    return;
                    //}
                    //PageSizeOfDetail = tmpPageSize;

                    //ExecuteQuery?.Invoke();
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
            //入库组织
            mcbWhere_StockIn_Org.Clear();
            //供应商
            mcbWhere_Client_Name.Clear();
            //异动类型
            cbWhere_InventoryTransType.Clear();
            //入库时间开始
            dtWhere_StockIn_TimeStart.Value = null;
            //入库时间结束
            dtWhere_StockIn_TimeEnd.Value = null;
            //给 入库组织 设置焦点
            mcbWhere_StockIn_Org.Focus();
            #endregion

            #region Grid初始化
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                _stockInTotalReportDS = new List<AutoPartsStockInTotalReportUIModel>();
                StockInReportGrid.DataSource = _stockInTotalReportDS;
                StockInReportGrid.DataBind();
            }
            else
            {
                _stockInDetailReportDS = new List<AutoPartsStockInDetailReportUIModel>();
                StockInDetailGrid.DataSource = _stockInDetailReportDS;
                StockInDetailGrid.DataBind();
            }
            #endregion
        }

        /// <summary>
        /// 导出当前页
        /// </summary>
        public override void ExportAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = tabControlFull.Tabs[SysConst.EN_LIST].Selected ? "入库汇总" : "入库明细";
            base.ExportAction(tabControlFull.Tabs[SysConst.EN_LIST].Selected ? StockInReportGrid : StockInDetailGrid, paramGridName);
        }

        /// <summary>
        /// 导出全部
        /// </summary>
        public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = tabControlFull.Tabs[SysConst.EN_LIST].Selected ? "入库汇总" : "入库明细";
            AutoPartsStockInReportQCModel tempConditionDS = new AutoPartsStockInReportQCModel()
            {
                //SqlId
                SqlId = tabControlFull.Tabs[SysConst.EN_LIST].Selected ? SQLID.RPT_StockInReport_SQL01 : SQLID.RPT_StockInReport_SQL02,
                //客户ID
                CustomerID = this.mcbWhere_Client_Name.SelectedValue,
                PageIndex = 1,
                PageSize = null
            };
            //组织ID
            if (string.IsNullOrEmpty(mcbWhere_StockIn_Org.SelectedValue))
            {
                var tmpOrgList = _orgList.Select(p => p.Org_ID).ToList();

                if (tmpOrgList.Count > 0)
                {
                    ConditionDS.OrgIdList = string.Join(SysConst.Semicolon_DBC, tmpOrgList);
                }
            }
            else
            {
                ConditionDS.OrgIdList = mcbWhere_StockIn_Org.SelectedValue;
            }

            if (this.dtWhere_StockIn_TimeStart.Value != null)
            {
                //入库时间-开始
                tempConditionDS.StartTime = this.dtWhere_StockIn_TimeStart.DateTime;
            }
            if (this.dtWhere_StockIn_TimeEnd.Value != null)
            {
                //入库时间-终了
                tempConditionDS.EndTime = this.dtWhere_StockIn_TimeEnd.DateTime;
            }

            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                List<AutoPartsStockInTotalReportUIModel> resultAllList = new List<AutoPartsStockInTotalReportUIModel>();
                _bll.QueryForList(SQLID.RPT_StockInReport_SQL01, tempConditionDS, resultAllList);

                decimal totalAccountPayableAmountOfCurPage = 0;
                decimal totalPaidAmountOfCurPage = 0;
                decimal totalAccountPayableAmountOfAllPage = 0;
                decimal totalPaidAmountOfAllPage = 0;

                if (resultAllList.Count > 0)
                {
                    AutoPartsStockInTotalReportUIModel subObject = resultAllList[0];
                    totalAccountPayableAmountOfAllPage = (subObject.TotalAccountPayableAmount ?? 0);
                    totalPaidAmountOfAllPage = (subObject.TotalPaidAmount ?? 0);
                }

                foreach (var loopSotckInTotalItem in resultAllList)
                {
                    totalAccountPayableAmountOfCurPage += (loopSotckInTotalItem.APB_AccountPayableAmount ?? 0);
                    totalPaidAmountOfCurPage += (loopSotckInTotalItem.APB_PaidAmount ?? 0);
                }

                AutoPartsStockInTotalReportUIModel curPageTotal = new AutoPartsStockInTotalReportUIModel
                {
                    CustomerName = _sumCurPageDesc,
                    APB_AccountPayableAmount = Math.Round(totalAccountPayableAmountOfCurPage, 2),
                    APB_PaidAmount = Math.Round(totalPaidAmountOfCurPage, 2)
                };
                resultAllList.Add(curPageTotal);

                AutoPartsStockInTotalReportUIModel allPageTotal = new AutoPartsStockInTotalReportUIModel
                {
                    CustomerName = _sumAllPageDesc,
                    APB_AccountPayableAmount = Math.Round(totalAccountPayableAmountOfAllPage, 2),
                    APB_PaidAmount = Math.Round(totalPaidAmountOfAllPage, 2)
                };
                resultAllList.Add(allPageTotal);

                UltraGrid allGrid = StockInReportGrid;
                allGrid.DataSource = resultAllList;
                allGrid.DataBind();

                base.ExportAllAction(allGrid, paramGridName);

                StockInReportGrid.DataSource = _stockInTotalReportDS;
                StockInReportGrid.DataBind();
            }
            else
            {
                List<AutoPartsStockInDetailReportUIModel> resultAllList = new List<AutoPartsStockInDetailReportUIModel>();
                _bll.QueryForList(SQLID.RPT_StockInReport_SQL02, tempConditionDS, resultAllList);

                decimal totalStockInQtyOfCurPage = 0;
                decimal totalStockInAmountOfCurPage = 0;
                decimal totalStockInQtyOfAllPage = 0;
                decimal totalStockInAmountOfAllPage = 0;

                if (resultAllList.Count > 0)
                {
                    AutoPartsStockInDetailReportUIModel subObject = resultAllList[0];
                    TotalRecordCountOfDetail = (subObject.RecordCount ?? 0);
                    totalStockInQtyOfAllPage = (subObject.TotalStockInQty ?? 0);
                    totalStockInAmountOfAllPage = (subObject.TotalStockInAmount ?? 0);
                }
                else
                {
                    TotalRecordCountOfDetail = 0;
                }

                foreach (var loopSotckInDetailItem in resultAllList)
                {
                    totalStockInQtyOfCurPage += (loopSotckInDetailItem.SID_Qty ?? 0);
                    totalStockInAmountOfCurPage += (loopSotckInDetailItem.SID_Amount ?? 0);
                }
                AutoPartsStockInDetailReportUIModel curPageTotal = new AutoPartsStockInDetailReportUIModel
                {
                    APA_Specification = _sumCurPageDesc,
                    SID_Qty = Math.Round(totalStockInQtyOfCurPage, 0),
                    SID_Amount = Math.Round(totalStockInAmountOfCurPage, 2)
                };
                resultAllList.Add(curPageTotal);

                AutoPartsStockInDetailReportUIModel allPageTotal = new AutoPartsStockInDetailReportUIModel
                {
                    APA_Specification = _sumAllPageDesc,
                    SID_Qty = Math.Round(totalStockInQtyOfAllPage, 0),
                    SID_Amount = Math.Round(totalStockInAmountOfAllPage, 2)
                };
                resultAllList.Add(allPageTotal);

                UltraGrid allGrid = StockInDetailGrid;
                allGrid.DataSource = resultAllList;
                allGrid.DataBind();

                base.ExportAllAction(allGrid, paramGridName);

                StockInDetailGrid.DataSource = _stockInDetailReportDS;
                StockInDetailGrid.DataBind();
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
                    if (_stockInTotalReportDS == null || _stockInTotalReportDS.Count == 0)
                    {
                        MessageBoxs.Show(Trans.SD, this.ToString(), "请查询要打印的数据。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    List<AutoPartsStockInTotalReportUIModel> stockInTotalReportToPrint = new List<AutoPartsStockInTotalReportUIModel>();
                    foreach (var loopStockInTotal in _stockInTotalReportDS)
                    {
                        if (loopStockInTotal.CustomerName == _sumAllPageDesc || loopStockInTotal.CustomerName == _sumCurPageDesc)
                        {
                            continue;
                        }
                        stockInTotalReportToPrint.Add(loopStockInTotal);
                    }

                    Dictionary<string, object> argsViewParams = new Dictionary<string, object>
                    {
                        {ComViewParamKey.APModel.ToString(), stockInTotalReportToPrint},
                    };

                    FrmViewAndPrintStockInTotalReport frmViewAndPrintStockInTotalReport = new FrmViewAndPrintStockInTotalReport(argsViewParams)
                    {
                        StartPosition = FormStartPosition.CenterScreen
                    };
                    DialogResult dialogResult = frmViewAndPrintStockInTotalReport.ShowDialog();
                    if (dialogResult != DialogResult.OK)
                    {
                        return;
                    }
                }
                else
                {
                    if (_stockInDetailReportDS == null || _stockInDetailReportDS.Count == 0)
                    {
                        MessageBoxs.Show(Trans.SD, this.ToString(), "请查询要打印的数据。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    List<AutoPartsStockInDetailReportUIModel> stockInDetailReportToPrint = new List<AutoPartsStockInDetailReportUIModel>();
                    foreach (var loopStockInDetail in _stockInDetailReportDS)
                    {
                        if (loopStockInDetail.APA_Specification == _sumAllPageDesc || loopStockInDetail.APA_Specification == _sumCurPageDesc)
                        {
                            continue;
                        }
                        stockInDetailReportToPrint.Add(loopStockInDetail);
                    }

                    Dictionary<string, object> argsViewParams = new Dictionary<string, object>
                    {
                        {ComViewParamKey.APModel.ToString(), stockInDetailReportToPrint},
                    };

                    FrmViewAndPrintStockInDetailReport frmViewAndPrintStockInDetailReport = new FrmViewAndPrintStockInDetailReport(argsViewParams)
                    {
                        StartPosition = FormStartPosition.CenterScreen
                    };
                    DialogResult dialogResult = frmViewAndPrintStockInDetailReport.ShowDialog();
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

        #region 导航按钮

        /// <summary>
        /// 初始化导航按钮
        /// </summary>
        public override void InitializeNavigate()
        {
            base.InitializeNavigate();
            SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.ONLINEPAY, true, true);
            SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOLOGISTICSBILL, true, true);
            SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, true);
        }

        #endregion

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 查询条件初始化
            //入库组织
            mcbWhere_StockIn_Org.Clear();
            //供应商
            mcbWhere_Client_Name.Clear();
            //异动类型
            cbWhere_InventoryTransType.Clear();
            //入库时间开始
            dtWhere_StockIn_TimeStart.Value = null;
            //入库时间结束
            dtWhere_StockIn_TimeEnd.Value = null;
            //给 入库组织 设置焦点
            mcbWhere_StockIn_Org.Focus();
            #endregion

            #region 初始化下拉框

            //组织下拉框
            _orgList = LoginInfoDAX.OrgList;
            mcbWhere_StockIn_Org.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
            mcbWhere_StockIn_Org.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
            mcbWhere_StockIn_Org.DataSource = _orgList;

            //客户
            _clientList = BLLCom.GetAllCustomerList(true, LoginInfoDAX.OrgID);
            mcbWhere_Client_Name.ExtraDisplayMember = "ClientType";
            mcbWhere_Client_Name.DisplayMember = "ClientName";
            mcbWhere_Client_Name.ValueMember = "ClientID";
            mcbWhere_Client_Name.DataSource = _clientList;

            //库存异动类型（直接入库，采购入库，调拨入库，销售退货）
            var tempInventoryTransList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.InventoryTransType);
            if (tempInventoryTransList != null)
            {
                _inventoryTransList =
                    tempInventoryTransList.Where(
                        x =>
                            x.Text == InventoryTransTypeEnum.Name.ZJRK || x.Text == InventoryTransTypeEnum.Name.CGRK ||
                            x.Text == InventoryTransTypeEnum.Name.DBRK ||
                            x.Text == InventoryTransTypeEnum.Name.XSTH).ToList();
            }
            cbWhere_InventoryTransType.DisplayMember = SysConst.EN_TEXT;
            cbWhere_InventoryTransType.ValueMember = SysConst.EN_Code;
            cbWhere_InventoryTransType.DataSource = _inventoryTransList;
            cbWhere_InventoryTransType.DataBind();

            #endregion

            #region Grid初始化
            _stockInTotalReportDS = new List<AutoPartsStockInTotalReportUIModel>();
            StockInReportGrid.DataSource = _stockInTotalReportDS;
            StockInReportGrid.DataBind();

            _stockInDetailReportDS = new List<AutoPartsStockInDetailReportUIModel>();
            StockInDetailGrid.DataSource = _stockInDetailReportDS;
            StockInDetailGrid.DataBind();
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
            var activeRowIndex = StockInReportGrid.ActiveRow.Index;
            //当前选中汇总行
            var curActiveTotalRow = _stockInTotalReportDS.FirstOrDefault(
                x =>
                    x.OrgID == StockInReportGrid.Rows[activeRowIndex].Cells["OrgID"].Value?.ToString() &&
                    x.CustomerID == StockInReportGrid.Rows[activeRowIndex].Cells["CustomerID"].Value?.ToString());
            if (curActiveTotalRow != null
                && !string.IsNullOrEmpty(curActiveTotalRow.CustomerID))
            {
                //查询汇总行对应的明细
                QueryStockInDetailReport(curActiveTotalRow);
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
            if (StockInReportGrid.ActiveRow == null || StockInReportGrid.ActiveRow.Index < 0)
            {
                return false;
            }
            if (StockInReportGrid.DisplayLayout.Bands[0].SortedColumns.Count > 0)
            {
                foreach (UltraGridColumn loopColumn in StockInReportGrid.DisplayLayout.Bands[0].SortedColumns)
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
