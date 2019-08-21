using System;
using System.Collections.Generic;
using System.Linq;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.RPT;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UIModel.Common.UIModel;
using SkyCar.Coeus.UIModel.RPT.QCModel;
using SkyCar.Coeus.UIModel.RPT.UIModel;

namespace SkyCar.Coeus.UI.RPT
{
    /// <summary>
    /// 组织与组织资金往来统计报表
    /// </summary>
    public partial class FrmOrgAndOrgAmountTransReport : BaseFormCardListDetail<OrgAndOrgAmountTransTotalReportUIModel, OrgAndOrgAmountTransReportQCModel, OrgAndOrgAmountTransTotalReportUIModel>
    {
        #region 全局变量

        /// <summary>
        /// 组织与组织资金往来统计报表BLL
        /// </summary>
        private OrgAndOrgAmountTransReportBLL _bll = new OrgAndOrgAmountTransReportBLL();

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
        /// 组织与组织资金往来统计汇总数据源
        /// </summary>
        private List<OrgAndOrgAmountTransTotalReportUIModel> _orgAndOrgAmountTransTotalReportDs = new List<OrgAndOrgAmountTransTotalReportUIModel>();
        /// <summary>
        /// 组织与组织资金往来统计明细数据源
        /// </summary>
        private List<OrgAndOrgAmountTransDetailReportUIModel> _orgAndOrgAmountTransDetailReportDs = new List<OrgAndOrgAmountTransDetailReportUIModel>();

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
        /// FrmOrgAndOrgAmountTransReport构造方法
        /// </summary>
        public FrmOrgAndOrgAmountTransReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmOrgAndOrgAmountTransReport_Load(object sender, EventArgs e)
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
            //设置总页数
            SetTotalPageCountAndTotalRecordCountOfTotal(0);
            SetTotalPageCountAndTotalRecordCountOfDetail(0);

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

        #region 【汇总】Grid相关事件

        /// <summary>
        /// 【汇总】Grid的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrgAndOrgAmountTransReportGrid_DoubleClick(object sender, EventArgs e)
        {
            //将Grid中选中的数据赋值给【详情】Tab内的对应控件
            SetGridDataToCardCtrls();
        }
        /// <summary>
        /// 【汇总】Grid的Cell的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrgAndOrgAmountTransReportGrid_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            //将Grid中选中的数据赋值给【详情】Tab内的对应控件
            SetGridDataToCardCtrls();
        }
        /// <summary>
        /// 【汇总】Grid的CellChange事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrgAndOrgAmountTransReportGrid_CellChange(object sender, CellEventArgs e)
        {
            OrgAndOrgAmountTransReportGrid.UpdateData();
        }
        #endregion

        #region 【明细】Grid相关事件

        #endregion

        #region 【查询条件】相关事件
        /// <summary>
        /// 时间终了ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_CreatedTimeEnd_ValueChanged(object sender, EventArgs e)
        {
            if (this.dtWhere_CreatedTimeEnd.Value != null &&
                this.dtWhere_CreatedTimeEnd.DateTime.Hour == 0 &&
                this.dtWhere_CreatedTimeEnd.DateTime.Minute == 0 &&
                this.dtWhere_CreatedTimeEnd.DateTime.Second == 0)
            {
                DateTime newDateTime = new DateTime(this.dtWhere_CreatedTimeEnd.DateTime.Year, this.dtWhere_CreatedTimeEnd.DateTime.Month, this.dtWhere_CreatedTimeEnd.DateTime.Day, 23, 59, 59);
                this.dtWhere_CreatedTimeEnd.DateTime = newDateTime;
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
            //源组织ID
            if (string.IsNullOrEmpty(mcbWhere_SourOrg.SelectedValue))
            {
                var tmpOrgList = _orgList.Select(p => p.Org_ID).ToList();

                if (tmpOrgList.Count > 0)
                {
                    ConditionDS.SourOrgIdList = string.Join(SysConst.Semicolon_DBC, tmpOrgList);
                }
            }
            else
            {
                ConditionDS.SourOrgIdList = mcbWhere_SourOrg.SelectedValue;
            }
            //目的组织ID
            if (string.IsNullOrEmpty(mcbWhere_DestOrg.SelectedValue))
            {
                var tmpOrgList = _orgList.Select(p => p.Org_ID).ToList();

                if (tmpOrgList.Count > 0)
                {
                    ConditionDS.DestOrgIdList = string.Join(SysConst.Semicolon_DBC, tmpOrgList);
                }
            }
            else
            {
                ConditionDS.DestOrgIdList = mcbWhere_DestOrg.SelectedValue;
            }

            //时间-开始
            if (this.dtWhere_CreatedTimeStart.Value != null)
            {
                ConditionDS.StartTime = this.dtWhere_CreatedTimeStart.DateTime;
            }
            else
            {
                ConditionDS.StartTime = null;
            }
            //时间-终了
            if (this.dtWhere_CreatedTimeEnd.Value != null)
            {
                ConditionDS.EndTime = this.dtWhere_CreatedTimeEnd.DateTime;
            }
            else
            {
                ConditionDS.EndTime = null;
            }

            //2.查询并绑定数据源
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                QueryOrgAndOrgAmountTransTotalReport();
            }
            else
            {
                QueryOrgAndOrgAmountTransDetailReport();
            }
        }

        #region 查询汇总
        /// <summary>
        /// 查询组织与组织资金往来统计汇总
        /// </summary>
        private void QueryOrgAndOrgAmountTransTotalReport()
        {
            try
            {
                OrgAndOrgAmountTransReportQCModel argsModel = ConditionDS;
                argsModel.PageSize = PageSizeOfTotal;
                argsModel.PageIndex = PageIndexOfTotal;
                //SqlId
                argsModel.SqlId = SQLID.RPT_OrgAndOrgAmountTransReport_SQL01;

                _orgAndOrgAmountTransTotalReportDs = new List<OrgAndOrgAmountTransTotalReportUIModel>();
                _bll.QueryForList(argsModel.SqlId, argsModel, _orgAndOrgAmountTransTotalReportDs);

                //当前页合计
                decimal totalAccountReceivableAmountOfCurPage = 0;
                decimal totalReceivedAmountOfCurPage = 0;
                decimal totalAccountPayableAmountOfCurPage = 0;
                decimal totalPaidAmountOfCurPage = 0;
                //合计
                decimal totalAccountReceivableAmountOfAllPage = 0;
                decimal totalReceivedAmountOfAllPage = 0;
                decimal totalAccountPayableAmountOfAllPage = 0;
                decimal totalPaidAmountOfAllPage = 0;

                foreach (var loopTotal in _orgAndOrgAmountTransTotalReportDs)
                {
                    totalAccountReceivableAmountOfCurPage += (loopTotal.ARB_AccountReceivableAmount ?? 0);
                    totalReceivedAmountOfCurPage += (loopTotal.ARB_ReceivedAmount ?? 0);
                    totalAccountPayableAmountOfCurPage += (loopTotal.APB_AccountPayableAmount ?? 0);
                    totalPaidAmountOfCurPage += (loopTotal.APB_PaidAmount ?? 0);
                }
                OrgAndOrgAmountTransTotalReportUIModel curPageTotal = new OrgAndOrgAmountTransTotalReportUIModel
                {
                    DestOrgName = _sumCurPageDesc,
                    ARB_AccountReceivableAmount = Math.Round(totalAccountReceivableAmountOfCurPage, 2),
                    ARB_ReceivedAmount = Math.Round(totalReceivedAmountOfCurPage, 2),
                    APB_AccountPayableAmount = Math.Round(totalAccountPayableAmountOfCurPage, 2),
                    APB_PaidAmount = Math.Round(totalPaidAmountOfCurPage, 2),
                };
                _orgAndOrgAmountTransTotalReportDs.Add(curPageTotal);

                if (_orgAndOrgAmountTransTotalReportDs.Count > 0)
                {
                    OrgAndOrgAmountTransTotalReportUIModel subObject = _orgAndOrgAmountTransTotalReportDs[0];
                    TotalRecordCountOfTotal = subObject.RecordCount ?? 0;
                    totalAccountReceivableAmountOfAllPage = (subObject.TotalAccountReceivableAmount ?? 0);
                    totalReceivedAmountOfAllPage = (subObject.TotalReceivedAmount ?? 0);
                    totalAccountPayableAmountOfAllPage = (subObject.TotalAccountPayableAmount ?? 0);
                    totalPaidAmountOfAllPage = (subObject.TotalPaidAmount ?? 0);
                }
                else
                {
                    TotalRecordCountOfTotal = 0;
                }
                OrgAndOrgAmountTransTotalReportUIModel allPageTotal = new OrgAndOrgAmountTransTotalReportUIModel
                {
                    DestOrgName = _sumAllPageDesc,
                    ARB_AccountReceivableAmount = Math.Round(totalAccountReceivableAmountOfAllPage, 2),
                    ARB_ReceivedAmount = Math.Round(totalReceivedAmountOfAllPage, 2),
                    APB_AccountPayableAmount = Math.Round(totalAccountPayableAmountOfAllPage, 2),
                    APB_PaidAmount = Math.Round(totalPaidAmountOfAllPage, 2),
                };
                _orgAndOrgAmountTransTotalReportDs.Add(allPageTotal);

                //重新计算[总页数]，设置最新[allPageTotal]
                SetTotalPageCountAndTotalRecordCountOfTotal(TotalRecordCountOfTotal);
                //设置翻页按钮状态
                SetPageButtonStatusOfTotal();

                OrgAndOrgAmountTransReportGrid.DataSource = _orgAndOrgAmountTransTotalReportDs;
                OrgAndOrgAmountTransReportGrid.DataBind();
                OrgAndOrgAmountTransReportGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
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
        /// 查询组织与组织资金往来统计明细
        /// </summary>
        /// <param name="paramTotalReport">汇总行</param>
        private void QueryOrgAndOrgAmountTransDetailReport(OrgAndOrgAmountTransTotalReportUIModel paramTotalReport = null)
        {
            try
            {
                OrgAndOrgAmountTransReportQCModel argsModel = ConditionDS;
                argsModel.PageSize = PageSizeOfDetail;
                argsModel.PageIndex = PageIndexOfDetail;
                argsModel.SqlId = SQLID.RPT_OrgAndOrgAmountTransReport_SQL02;
                if (paramTotalReport != null)
                {
                    argsModel.SourOrgId = paramTotalReport.SourOrgID;
                    argsModel.DestOrgId = paramTotalReport.DestOrgID;
                }
                else
                {
                    argsModel.SourOrgId = null;
                    argsModel.DestOrgId = null;
                }

                _orgAndOrgAmountTransDetailReportDs = new List<OrgAndOrgAmountTransDetailReportUIModel>();
                _bll.QueryForList(argsModel.SqlId, argsModel, _orgAndOrgAmountTransDetailReportDs);

                //当前页合计
                decimal totalQtyOfCurPage = 0;
                decimal totalAmountOfCurPage = 0;
                //合计
                decimal totalQtyOfAllPage = 0;
                decimal totalAmountOfAllPage = 0;

                foreach (var loopDetail in _orgAndOrgAmountTransDetailReportDs)
                {
                    totalQtyOfCurPage += (loopDetail.TBD_Qty ?? 0);
                    totalAmountOfCurPage += (loopDetail.TransferAmount ?? 0);
                }
                OrgAndOrgAmountTransDetailReportUIModel curPageTotal = new OrgAndOrgAmountTransDetailReportUIModel
                {
                    TBD_Specification = _sumCurPageDesc,
                    TBD_Qty = Math.Round(totalQtyOfCurPage, 2),
                    TransferAmount = Math.Round(totalAmountOfCurPage, 2),
                };
                _orgAndOrgAmountTransDetailReportDs.Add(curPageTotal);

                if (_orgAndOrgAmountTransDetailReportDs.Count > 0)
                {
                    OrgAndOrgAmountTransDetailReportUIModel subObject = _orgAndOrgAmountTransDetailReportDs[0];
                    TotalRecordCountOfDetail = (subObject.RecordCount ?? 0);
                    totalQtyOfAllPage = (subObject.TotalQty ?? 0);
                    totalAmountOfAllPage = (subObject.TotalAmount ?? 0);
                }
                else
                {
                    TotalRecordCountOfDetail = 0;
                }

                OrgAndOrgAmountTransDetailReportUIModel allPageTotal = new OrgAndOrgAmountTransDetailReportUIModel
                {
                    TBD_Specification = _sumAllPageDesc,
                    TBD_Qty = Math.Round(totalQtyOfAllPage, 2),
                    TransferAmount = Math.Round(totalAmountOfAllPage, 2),
                };
                _orgAndOrgAmountTransDetailReportDs.Add(allPageTotal);

                //重新计算[总页数]，设置最新[总记录条数]
                SetTotalPageCountAndTotalRecordCountOfDetail(TotalRecordCountOfDetail);
                //设置翻页按钮状态
                SetPageButtonStatusOfDetail();

                OrgAndOrgAmountTransDetailGrid.DataSource = _orgAndOrgAmountTransDetailReportDs;
                OrgAndOrgAmountTransDetailGrid.DataBind();
                OrgAndOrgAmountTransDetailGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
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
            //源组织
            mcbWhere_SourOrg.Clear();
            //目的组织
            mcbWhere_DestOrg.Clear();
            //时间开始
            dtWhere_CreatedTimeStart.Value = null;
            //时间结束
            dtWhere_CreatedTimeEnd.Value = null;
            //给 源组织 设置焦点
            mcbWhere_SourOrg.Focus();
            #endregion

            #region Grid初始化
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                _orgAndOrgAmountTransTotalReportDs = new List<OrgAndOrgAmountTransTotalReportUIModel>();
                OrgAndOrgAmountTransReportGrid.DataSource = _orgAndOrgAmountTransTotalReportDs;
                OrgAndOrgAmountTransReportGrid.DataBind();
            }
            else
            {
                _orgAndOrgAmountTransDetailReportDs = new List<OrgAndOrgAmountTransDetailReportUIModel>();
                OrgAndOrgAmountTransDetailGrid.DataSource = _orgAndOrgAmountTransDetailReportDs;
                OrgAndOrgAmountTransDetailGrid.DataBind();
            }
            #endregion
        }

        /// <summary>
        /// 导出当前页
        /// </summary>
        public override void ExportAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = tabControlFull.Tabs[SysConst.EN_LIST].Selected ? "组织与组织资金往来统计汇总" : "组织与组织资金往来统计明细";
            base.ExportAction(tabControlFull.Tabs[SysConst.EN_LIST].Selected ? OrgAndOrgAmountTransReportGrid : OrgAndOrgAmountTransDetailGrid, paramGridName);
        }

        /// <summary>
        /// 导出全部
        /// </summary>
        public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = tabControlFull.Tabs[SysConst.EN_LIST].Selected ? "组织与组织资金往来统计汇总" : "组织与组织资金往来统计明细";
            OrgAndOrgAmountTransReportQCModel tempConditionDS = new OrgAndOrgAmountTransReportQCModel()
            {
                //SqlId
                SqlId = tabControlFull.Tabs[SysConst.EN_LIST].Selected ? SQLID.RPT_OrgAndOrgAmountTransReport_SQL01 : SQLID.RPT_OrgAndOrgAmountTransReport_SQL02,
                PageIndex = 1,
                PageSize = null
            };
            //源组织ID
            if (string.IsNullOrEmpty(mcbWhere_SourOrg.SelectedValue))
            {
                var tmpOrgList = _orgList.Select(p => p.Org_ID).ToList();

                if (tmpOrgList.Count > 0)
                {
                    tempConditionDS.SourOrgIdList = string.Join(SysConst.Semicolon_DBC, tmpOrgList);
                }
            }
            else
            {
                tempConditionDS.SourOrgIdList = mcbWhere_SourOrg.SelectedValue;
            }
            //目的组织ID
            if (string.IsNullOrEmpty(mcbWhere_DestOrg.SelectedValue))
            {
                var tmpOrgList = _orgList.Select(p => p.Org_ID).ToList();

                if (tmpOrgList.Count > 0)
                {
                    tempConditionDS.DestOrgIdList = string.Join(SysConst.Semicolon_DBC, tmpOrgList);
                }
            }
            else
            {
                tempConditionDS.DestOrgIdList = mcbWhere_DestOrg.SelectedValue;
            }

            if (this.dtWhere_CreatedTimeStart.Value != null)
            {
                //时间-开始
                tempConditionDS.StartTime = this.dtWhere_CreatedTimeStart.DateTime;
            }
            if (this.dtWhere_CreatedTimeEnd.Value != null)
            {
                //时间-终了
                tempConditionDS.EndTime = this.dtWhere_CreatedTimeEnd.DateTime;
            }

            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                List<OrgAndOrgAmountTransTotalReportUIModel> resultAllList = new List<OrgAndOrgAmountTransTotalReportUIModel>();
                _bll.QueryForList(SQLID.RPT_OrgAndOrgAmountTransReport_SQL01, tempConditionDS, resultAllList);

                //当前页合计
                decimal totalAccountReceivableAmountOfCurPage = 0;
                decimal totalReceivedAmountOfCurPage = 0;
                decimal totalAccountPayableAmountOfCurPage = 0;
                decimal totalPaidAmountOfCurPage = 0;
                //合计
                decimal totalAccountReceivableAmountOfAllPage = 0;
                decimal totalReceivedAmountOfAllPage = 0;
                decimal totalAccountPayableAmountOfAllPage = 0;
                decimal totalPaidAmountOfAllPage = 0;

                foreach (var loopItem in resultAllList)
                {
                    totalAccountReceivableAmountOfCurPage += (loopItem.ARB_AccountReceivableAmount ?? 0);
                    totalReceivedAmountOfCurPage += (loopItem.ARB_ReceivedAmount ?? 0);
                    totalAccountPayableAmountOfCurPage += (loopItem.APB_AccountPayableAmount ?? 0);
                    totalPaidAmountOfCurPage += (loopItem.APB_PaidAmount ?? 0);
                }
                OrgAndOrgAmountTransTotalReportUIModel curPageTotal = new OrgAndOrgAmountTransTotalReportUIModel
                {
                    DestOrgName = _sumCurPageDesc,
                    ARB_AccountReceivableAmount = Math.Round(totalAccountReceivableAmountOfCurPage, 2),
                    ARB_ReceivedAmount = Math.Round(totalReceivedAmountOfCurPage, 2),
                    APB_AccountPayableAmount = Math.Round(totalAccountPayableAmountOfCurPage, 2),
                    APB_PaidAmount = Math.Round(totalPaidAmountOfCurPage, 2)
                };
                resultAllList.Add(curPageTotal);

                if (resultAllList.Count > 0)
                {
                    OrgAndOrgAmountTransTotalReportUIModel subObject = _orgAndOrgAmountTransTotalReportDs[0];
                    TotalRecordCountOfTotal = subObject.RecordCount ?? 0;
                    totalAccountReceivableAmountOfAllPage = (subObject.TotalAccountReceivableAmount ?? 0);
                    totalReceivedAmountOfAllPage = (subObject.TotalReceivedAmount ?? 0);
                    totalAccountPayableAmountOfAllPage = (subObject.TotalAccountPayableAmount ?? 0);
                    totalPaidAmountOfAllPage = (subObject.TotalPaidAmount ?? 0);
                }

                OrgAndOrgAmountTransTotalReportUIModel allPageTotal = new OrgAndOrgAmountTransTotalReportUIModel
                {
                    DestOrgName = _sumAllPageDesc,
                    ARB_AccountReceivableAmount = Math.Round(totalAccountReceivableAmountOfAllPage, 2),
                    ARB_ReceivedAmount = Math.Round(totalReceivedAmountOfAllPage, 2),
                    APB_AccountPayableAmount = Math.Round(totalAccountPayableAmountOfAllPage, 2),
                    APB_PaidAmount = Math.Round(totalPaidAmountOfAllPage, 2)
                };
                resultAllList.Add(allPageTotal);

                UltraGrid allGrid = OrgAndOrgAmountTransReportGrid;
                allGrid.DataSource = resultAllList;
                allGrid.DataBind();

                base.ExportAllAction(allGrid, paramGridName);

                OrgAndOrgAmountTransReportGrid.DataSource = _orgAndOrgAmountTransTotalReportDs;
                OrgAndOrgAmountTransReportGrid.DataBind();
            }
            else
            {
                List<OrgAndOrgAmountTransDetailReportUIModel> resultAllList = new List<OrgAndOrgAmountTransDetailReportUIModel>();
                _bll.QueryForList(SQLID.RPT_OrgAndOrgAmountTransReport_SQL02, tempConditionDS, resultAllList);

                //当前页合计
                decimal totalQtyOfCurPage = 0;
                decimal totalAmountOfCurPage = 0;
                //合计
                decimal totalQtyOfAllPage = 0;
                decimal totalAmountOfAllPage = 0;

                foreach (var loopDetail in resultAllList)
                {
                    totalQtyOfCurPage += (loopDetail.TBD_Qty ?? 0);
                    totalAmountOfCurPage += (loopDetail.TransferAmount ?? 0);
                }
                OrgAndOrgAmountTransDetailReportUIModel curPageTotal = new OrgAndOrgAmountTransDetailReportUIModel
                {
                    TBD_Specification = _sumCurPageDesc,
                    TBD_Qty = Math.Round(totalQtyOfCurPage, 2),
                    TransferAmount = Math.Round(totalAmountOfCurPage, 2),
                };
                resultAllList.Add(curPageTotal);

                if (resultAllList.Count > 0)
                {
                    OrgAndOrgAmountTransDetailReportUIModel subObject = resultAllList[0];
                    TotalRecordCountOfDetail = (subObject.RecordCount ?? 0);
                    totalQtyOfAllPage = (subObject.TotalQty ?? 0);
                    totalAmountOfAllPage = (subObject.TotalAmount ?? 0);
                }
                else
                {
                    TotalRecordCountOfDetail = 0;
                }

                OrgAndOrgAmountTransDetailReportUIModel allPageTotal = new OrgAndOrgAmountTransDetailReportUIModel
                {
                    TBD_Specification = _sumAllPageDesc,
                    TBD_Qty = Math.Round(totalQtyOfAllPage, 2),
                    TransferAmount = Math.Round(totalAmountOfAllPage, 2),
                };
                resultAllList.Add(allPageTotal);

                UltraGrid allGrid = OrgAndOrgAmountTransDetailGrid;
                allGrid.DataSource = resultAllList;
                allGrid.DataBind();

                base.ExportAllAction(allGrid, paramGridName);

                OrgAndOrgAmountTransDetailGrid.DataSource = _orgAndOrgAmountTransDetailReportDs;
                OrgAndOrgAmountTransDetailGrid.DataBind();
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

            //组织下拉框
            _orgList = LoginInfoDAX.OrgList;
            //源组织
            mcbWhere_SourOrg.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
            mcbWhere_SourOrg.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
            mcbWhere_SourOrg.DataSource = _orgList;
            //目的组织
            mcbWhere_DestOrg.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
            mcbWhere_DestOrg.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
            mcbWhere_DestOrg.DataSource = _orgList;

            #endregion

            #region 查询条件初始化
            //源组织
            mcbWhere_SourOrg.Clear();
            //目的组织
            mcbWhere_DestOrg.Clear();
            //时间开始
            dtWhere_CreatedTimeStart.Value = null;
            //时间结束
            dtWhere_CreatedTimeEnd.Value = null;
            //给 源组织 设置焦点
            mcbWhere_SourOrg.Focus();
            #endregion

            #region Grid初始化
            _orgAndOrgAmountTransTotalReportDs = new List<OrgAndOrgAmountTransTotalReportUIModel>();
            OrgAndOrgAmountTransReportGrid.DataSource = _orgAndOrgAmountTransTotalReportDs;
            OrgAndOrgAmountTransReportGrid.DataBind();

            _orgAndOrgAmountTransDetailReportDs = new List<OrgAndOrgAmountTransDetailReportUIModel>();
            OrgAndOrgAmountTransDetailGrid.DataSource = _orgAndOrgAmountTransDetailReportDs;
            OrgAndOrgAmountTransDetailGrid.DataBind();
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
            var activeRowIndex = OrgAndOrgAmountTransReportGrid.ActiveRow.Index;
            //当前选中汇总行
            var curActiveTotalRow = _orgAndOrgAmountTransTotalReportDs.FirstOrDefault(
                x =>
                    x.SourOrgID == OrgAndOrgAmountTransReportGrid.Rows[activeRowIndex].Cells["SourOrgID"].Value?.ToString() &&
                    x.DestOrgID == OrgAndOrgAmountTransReportGrid.Rows[activeRowIndex].Cells["DestOrgID"].Value?.ToString());
            if (curActiveTotalRow != null
                && !string.IsNullOrEmpty(curActiveTotalRow.SourOrgID)
                && !string.IsNullOrEmpty(curActiveTotalRow.DestOrgID))
            {
                //查询汇总行对应的明细
                QueryOrgAndOrgAmountTransDetailReport(curActiveTotalRow);
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
            if (OrgAndOrgAmountTransReportGrid.ActiveRow == null || OrgAndOrgAmountTransReportGrid.ActiveRow.Index < 0)
            {
                return false;
            }
            if (OrgAndOrgAmountTransReportGrid.DisplayLayout.Bands[0].SortedColumns.Count > 0)
            {
                foreach (UltraGridColumn loopColumn in OrgAndOrgAmountTransReportGrid.DisplayLayout.Bands[0].SortedColumns)
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
