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
    /// 组织与客户资金往来统计报表
    /// </summary>
    public partial class FrmOrgAndCustomerAmountTransReport : BaseFormCardListDetail<OrgAndCustomerAmountTransTotalReportUIModel, OrgAndCustomerAmountTransReportQCModel, OrgAndCustomerAmountTransTotalReportUIModel>
    {
        #region 全局变量

        /// <summary>
        /// 组织与客户资金往来统计报表BLL
        /// </summary>
        private OrgAndCustomerAmountTransReportBLL _bll = new OrgAndCustomerAmountTransReportBLL();

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
        /// 组织与客户资金往来统计汇总数据源
        /// </summary>
        private List<OrgAndCustomerAmountTransTotalReportUIModel> _orgAndCustomerAmountTransTotalReportDS = new List<OrgAndCustomerAmountTransTotalReportUIModel>();
        /// <summary>
        /// 组织与客户资金往来统计明细数据源
        /// </summary>
        private List<OrgAndCustomerAmountTransDetailReportUIModel> _orgAndCustomerAmountTransDetailReportDS = new List<OrgAndCustomerAmountTransDetailReportUIModel>();

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
        /// FrmOrgAndCustomerAmountTransReport构造方法
        /// </summary>
        public FrmOrgAndCustomerAmountTransReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmOrgAndCustomerAmountTransReport_Load(object sender, EventArgs e)
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
        private void OrgAndCustomerAmountTransReportGrid_DoubleClick(object sender, EventArgs e)
        {
            //将Grid中选中的数据赋值给【详情】Tab内的对应控件
            SetGridDataToCardCtrls();
        }
        /// <summary>
        /// 【汇总】Grid的Cell的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrgAndCustomerAmountTransReportGrid_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            //将Grid中选中的数据赋值给【详情】Tab内的对应控件
            SetGridDataToCardCtrls();
        }
        /// <summary>
        /// 【汇总】Grid的CellChange事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrgAndCustomerAmountTransReportGrid_CellChange(object sender, CellEventArgs e)
        {
            OrgAndCustomerAmountTransReportGrid.UpdateData();
        }
        #endregion

        #region 【明细】Grid相关事件

        #endregion

        #region 【查询条件】相关事件
        /// <summary>
        /// 销售时间终了ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_Sales_TimeEnd_ValueChanged(object sender, EventArgs e)
        {
            if (this.dtWhere_Sales_TimeEnd.Value != null &&
                this.dtWhere_Sales_TimeEnd.DateTime.Hour == 0 &&
                this.dtWhere_Sales_TimeEnd.DateTime.Minute == 0 &&
                this.dtWhere_Sales_TimeEnd.DateTime.Second == 0)
            {
                DateTime newDateTime = new DateTime(this.dtWhere_Sales_TimeEnd.DateTime.Year, this.dtWhere_Sales_TimeEnd.DateTime.Month, this.dtWhere_Sales_TimeEnd.DateTime.Day, 23, 59, 59);
                this.dtWhere_Sales_TimeEnd.DateTime = newDateTime;
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
            base.ConditionDS = new OrgAndCustomerAmountTransReportQCModel()
            {
                //客户ID
                CustomerID = this.mcbWhere_Client_Name.SelectedValue,
            };
            //组织ID
            if (string.IsNullOrEmpty(mcbWhere_Sales_Org.SelectedValue))
            {
                var tmpOrgList = _orgList.Select(p => p.Org_ID).ToList();

                if (tmpOrgList.Count > 0)
                {
                    ConditionDS.OrgIdList = string.Join(SysConst.Semicolon_DBC, tmpOrgList);
                }
            }
            else
            {
                ConditionDS.OrgIdList = mcbWhere_Sales_Org.SelectedValue;
            }

            //销售时间-开始
            if (this.dtWhere_Sales_TimeStart.Value != null)
            {
                ConditionDS.StartTime = this.dtWhere_Sales_TimeStart.DateTime;
            }
            else
            {
                ConditionDS.StartTime = null;
            }
            //销售时间-终了
            if (this.dtWhere_Sales_TimeEnd.Value != null)
            {
                ConditionDS.EndTime = this.dtWhere_Sales_TimeEnd.DateTime;
            }
            else
            {
                ConditionDS.EndTime = null;
            }

            //2.查询并绑定数据源
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                QueryOrgAndCustomerAmountTransTotalReport();
            }
            else
            {
                QueryOrgAndCustomerAmountTransDetailReport();
            }
        }

        #region 查询汇总
        /// <summary>
        /// 查询组织与客户资金往来统计汇总
        /// </summary>
        private void QueryOrgAndCustomerAmountTransTotalReport()
        {
            try
            {
                OrgAndCustomerAmountTransReportQCModel argsModel = ConditionDS;
                argsModel.PageSize = PageSizeOfTotal;
                argsModel.PageIndex = PageIndexOfTotal;
                //SqlId
                argsModel.SqlId = SQLID.RPT_OrgAndCustomerAmountTransReport_SQL01;

                _orgAndCustomerAmountTransTotalReportDS = new List<OrgAndCustomerAmountTransTotalReportUIModel>();
                _bll.QueryForList(argsModel.SqlId, argsModel, _orgAndCustomerAmountTransTotalReportDS);

                decimal totalAccountPayableAmountOfCurPage = 0;
                decimal totalPaidAmountOfCurPage = 0;
                decimal totalAccountPayableAmountOfAllPage = 0;
                decimal totalPaidAmountOfAllPage = 0;

                if (_orgAndCustomerAmountTransTotalReportDS.Count > 0)
                {
                    OrgAndCustomerAmountTransTotalReportUIModel subObject = _orgAndCustomerAmountTransTotalReportDS[0];
                    TotalRecordCountOfTotal = subObject.RecordCount ?? 0;
                    totalAccountPayableAmountOfAllPage = (subObject.TotalAccountReceivableAmount ?? 0);
                    totalPaidAmountOfAllPage = (subObject.TotalReceivedAmount ?? 0);
                }
                else
                {
                    TotalRecordCountOfTotal = 0;
                }

                foreach (var loopSotckInTotalItem in _orgAndCustomerAmountTransTotalReportDS)
                {
                    totalAccountPayableAmountOfCurPage += (loopSotckInTotalItem.ARB_AccountReceivableAmount ?? 0);
                    totalPaidAmountOfCurPage += (loopSotckInTotalItem.ARB_ReceivedAmount ?? 0);
                }
                OrgAndCustomerAmountTransTotalReportUIModel curPageTotal = new OrgAndCustomerAmountTransTotalReportUIModel
                {
                    CustomerName = _sumCurPageDesc,
                    ARB_AccountReceivableAmount = Math.Round(totalAccountPayableAmountOfCurPage, 2),
                    ARB_ReceivedAmount = Math.Round(totalPaidAmountOfCurPage, 2)
                };
                _orgAndCustomerAmountTransTotalReportDS.Add(curPageTotal);

                OrgAndCustomerAmountTransTotalReportUIModel allPageTotal = new OrgAndCustomerAmountTransTotalReportUIModel
                {
                    CustomerName = _sumAllPageDesc,
                    ARB_AccountReceivableAmount = Math.Round(totalAccountPayableAmountOfAllPage, 2),
                    ARB_ReceivedAmount = Math.Round(totalPaidAmountOfAllPage, 2)
                };
                _orgAndCustomerAmountTransTotalReportDS.Add(allPageTotal);

                //重新计算[总页数]，设置最新[allPageTotal]
                SetTotalPageCountAndTotalRecordCountOfTotal(TotalRecordCountOfTotal);
                //设置翻页按钮状态
                SetPageButtonStatusOfTotal();

                OrgAndCustomerAmountTransReportGrid.DataSource = _orgAndCustomerAmountTransTotalReportDS;
                OrgAndCustomerAmountTransReportGrid.DataBind();
                OrgAndCustomerAmountTransReportGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
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
        /// 查询组织与客户资金往来统计明细
        /// </summary>
        /// <param name="paramTotalReport">汇总行</param>
        private void QueryOrgAndCustomerAmountTransDetailReport(OrgAndCustomerAmountTransTotalReportUIModel paramTotalReport = null)
        {
            try
            {
                OrgAndCustomerAmountTransReportQCModel argsModel = ConditionDS;
                argsModel.PageSize = PageSizeOfDetail;
                argsModel.PageIndex = PageIndexOfDetail;
                argsModel.SqlId = SQLID.RPT_OrgAndCustomerAmountTransReport_SQL02;
                if (paramTotalReport != null)
                {
                    argsModel.OrgIdList = paramTotalReport.OrgID;
                    argsModel.CustomerID = paramTotalReport.CustomerID;
                    argsModel.BusinessType = paramTotalReport.BusinessType;
                }
                else
                {
                    argsModel.BusinessType = null;
                }

                _orgAndCustomerAmountTransDetailReportDS = new List<OrgAndCustomerAmountTransDetailReportUIModel>();
                _bll.QueryForList(argsModel.SqlId, argsModel, _orgAndCustomerAmountTransDetailReportDS);

                decimal totalAutoPartsQtyOfCurPage = 0;
                decimal totalAutoPartsAmountOfCurPage = 0;
                decimal totalAutoPartsQtyOfAllPage = 0;
                decimal totalAutoPartsAmountOfAllPage = 0;

                if (_orgAndCustomerAmountTransDetailReportDS.Count > 0)
                {
                    OrgAndCustomerAmountTransDetailReportUIModel subObject = _orgAndCustomerAmountTransDetailReportDS[0];
                    TotalRecordCountOfDetail = (subObject.RecordCount ?? 0);
                    totalAutoPartsQtyOfAllPage = (subObject.TotalAutoPartsQty ?? 0);
                    totalAutoPartsAmountOfAllPage = (subObject.TotalAutoPartsAmount ?? 0);
                }
                else
                {
                    TotalRecordCountOfDetail = 0;
                }

                foreach (var loopDetail in _orgAndCustomerAmountTransDetailReportDS)
                {
                    totalAutoPartsQtyOfCurPage += (loopDetail.AutoPartsQty ?? 0);
                    totalAutoPartsAmountOfCurPage += (loopDetail.AutoPartsAmount ?? 0);
                }
                OrgAndCustomerAmountTransDetailReportUIModel curPageTotal = new OrgAndCustomerAmountTransDetailReportUIModel
                {
                    APA_Specification = _sumCurPageDesc,
                    AutoPartsQty = Math.Round(totalAutoPartsQtyOfCurPage, 0),
                    AutoPartsAmount = Math.Round(totalAutoPartsAmountOfCurPage, 2)
                };
                _orgAndCustomerAmountTransDetailReportDS.Add(curPageTotal);

                OrgAndCustomerAmountTransDetailReportUIModel allPageTotal = new OrgAndCustomerAmountTransDetailReportUIModel
                {
                    APA_Specification = _sumAllPageDesc,
                    AutoPartsQty = Math.Round(totalAutoPartsQtyOfAllPage, 0),
                    AutoPartsAmount = Math.Round(totalAutoPartsAmountOfAllPage, 2)
                };
                _orgAndCustomerAmountTransDetailReportDS.Add(allPageTotal);

                //重新计算[总页数]，设置最新[总记录条数]
                SetTotalPageCountAndTotalRecordCountOfDetail(TotalRecordCountOfDetail);
                //设置翻页按钮状态
                SetPageButtonStatusOfDetail();

                OrgAndCustomerAmountTransDetailGrid.DataSource = _orgAndCustomerAmountTransDetailReportDS;
                OrgAndCustomerAmountTransDetailGrid.DataBind();
                OrgAndCustomerAmountTransDetailGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
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
            mcbWhere_Sales_Org.Clear();
            //供应商
            mcbWhere_Client_Name.Clear();
            //销售时间开始
            dtWhere_Sales_TimeStart.Value = null;
            //销售时间结束
            dtWhere_Sales_TimeEnd.Value = null;
            //给 销售组织 设置焦点
            mcbWhere_Sales_Org.Focus();
            #endregion

            #region Grid初始化
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                _orgAndCustomerAmountTransTotalReportDS = new List<OrgAndCustomerAmountTransTotalReportUIModel>();
                OrgAndCustomerAmountTransReportGrid.DataSource = _orgAndCustomerAmountTransTotalReportDS;
                OrgAndCustomerAmountTransReportGrid.DataBind();
            }
            else
            {
                _orgAndCustomerAmountTransDetailReportDS = new List<OrgAndCustomerAmountTransDetailReportUIModel>();
                OrgAndCustomerAmountTransDetailGrid.DataSource = _orgAndCustomerAmountTransDetailReportDS;
                OrgAndCustomerAmountTransDetailGrid.DataBind();
            }
            #endregion
        }

        /// <summary>
        /// 导出当前页
        /// </summary>
        public override void ExportAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = tabControlFull.Tabs[SysConst.EN_LIST].Selected ? "组织与客户资金往来统计汇总" : "组织与客户资金往来统计明细";
            base.ExportAction(tabControlFull.Tabs[SysConst.EN_LIST].Selected ? OrgAndCustomerAmountTransReportGrid : OrgAndCustomerAmountTransDetailGrid, paramGridName);
        }

        /// <summary>
        /// 导出全部
        /// </summary>
        public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = tabControlFull.Tabs[SysConst.EN_LIST].Selected ? "组织与客户资金往来统计汇总" : "组织与客户资金往来统计明细";
            OrgAndCustomerAmountTransReportQCModel tempConditionDS = new OrgAndCustomerAmountTransReportQCModel()
            {
                //SqlId
                SqlId = tabControlFull.Tabs[SysConst.EN_LIST].Selected ? SQLID.RPT_OrgAndCustomerAmountTransReport_SQL01 : SQLID.RPT_OrgAndCustomerAmountTransReport_SQL02,
                //客户ID
                CustomerID = this.mcbWhere_Client_Name.SelectedValue,
                PageIndex = 1,
                PageSize = null
            };
            //组织ID
            if (string.IsNullOrEmpty(mcbWhere_Sales_Org.SelectedValue))
            {
                var tmpOrgList = _orgList.Select(p => p.Org_ID).ToList();

                if (tmpOrgList.Count > 0)
                {
                    ConditionDS.OrgIdList = string.Join(SysConst.Semicolon_DBC, tmpOrgList);
                }
            }
            else
            {
                ConditionDS.OrgIdList = mcbWhere_Sales_Org.SelectedValue;
            }
            if (this.dtWhere_Sales_TimeStart.Value != null)
            {
                //销售时间-开始
                tempConditionDS.StartTime = this.dtWhere_Sales_TimeStart.DateTime;
            }
            if (this.dtWhere_Sales_TimeEnd.Value != null)
            {
                //销售时间-终了
                tempConditionDS.EndTime = this.dtWhere_Sales_TimeEnd.DateTime;
            }

            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                List<OrgAndCustomerAmountTransTotalReportUIModel> resultAllList = new List<OrgAndCustomerAmountTransTotalReportUIModel>();
                _bll.QueryForList(SQLID.RPT_OrgAndCustomerAmountTransReport_SQL01, tempConditionDS, resultAllList);

                decimal totalAccountPayableAmountOfCurPage = 0;
                decimal totalPaidAmountOfCurPage = 0;
                decimal totalAccountPayableAmountOfAllPage = 0;
                decimal totalPaidAmountOfAllPage = 0;

                if (resultAllList.Count > 0)
                {
                    OrgAndCustomerAmountTransTotalReportUIModel subObject = resultAllList[0];
                    totalAccountPayableAmountOfAllPage = (subObject.TotalAccountReceivableAmount ?? 0);
                    totalPaidAmountOfAllPage = (subObject.TotalReceivedAmount ?? 0);
                }

                foreach (var loopSotckInTotalItem in resultAllList)
                {
                    totalAccountPayableAmountOfCurPage += (loopSotckInTotalItem.ARB_AccountReceivableAmount ?? 0);
                    totalPaidAmountOfCurPage += (loopSotckInTotalItem.ARB_ReceivedAmount ?? 0);
                }
                OrgAndCustomerAmountTransTotalReportUIModel curPageTotal = new OrgAndCustomerAmountTransTotalReportUIModel
                {
                    CustomerName = _sumCurPageDesc,
                    ARB_AccountReceivableAmount = Math.Round(totalAccountPayableAmountOfCurPage, 2),
                    ARB_ReceivedAmount = Math.Round(totalPaidAmountOfCurPage, 2)
                };
                resultAllList.Add(curPageTotal);

                OrgAndCustomerAmountTransTotalReportUIModel allPageTotal = new OrgAndCustomerAmountTransTotalReportUIModel
                {
                    CustomerName = _sumAllPageDesc,
                    ARB_AccountReceivableAmount = Math.Round(totalAccountPayableAmountOfAllPage, 2),
                    ARB_ReceivedAmount = Math.Round(totalPaidAmountOfAllPage, 2)
                };
                resultAllList.Add(allPageTotal);

                UltraGrid allGrid = OrgAndCustomerAmountTransReportGrid;
                allGrid.DataSource = resultAllList;
                allGrid.DataBind();

                base.ExportAllAction(allGrid, paramGridName);

                OrgAndCustomerAmountTransReportGrid.DataSource = _orgAndCustomerAmountTransTotalReportDS;
                OrgAndCustomerAmountTransReportGrid.DataBind();
            }
            else
            {
                List<OrgAndCustomerAmountTransDetailReportUIModel> resultAllList = new List<OrgAndCustomerAmountTransDetailReportUIModel>();
                _bll.QueryForList(SQLID.RPT_OrgAndCustomerAmountTransReport_SQL02, tempConditionDS, resultAllList);

                decimal totalAutoPartsQtyOfCurPage = 0;
                decimal totalAutoPartsAmountOfCurPage = 0;
                decimal totalAutoPartsQtyOfAllPage = 0;
                decimal totalAutoPartsAmountOfAllPage = 0;

                if (resultAllList.Count > 0)
                {
                    OrgAndCustomerAmountTransDetailReportUIModel subObject = resultAllList[0];
                    TotalRecordCountOfDetail = (subObject.RecordCount ?? 0);
                    totalAutoPartsQtyOfAllPage = (subObject.TotalAutoPartsQty ?? 0);
                    totalAutoPartsAmountOfAllPage = (subObject.TotalAutoPartsAmount ?? 0);
                }
                else
                {
                    TotalRecordCountOfDetail = 0;
                }

                foreach (var loopSotckInDetailItem in resultAllList)
                {
                    totalAutoPartsQtyOfCurPage += (loopSotckInDetailItem.AutoPartsQty ?? 0);
                    totalAutoPartsAmountOfCurPage += (loopSotckInDetailItem.AutoPartsAmount ?? 0);
                }
                OrgAndCustomerAmountTransDetailReportUIModel curPageTotal = new OrgAndCustomerAmountTransDetailReportUIModel
                {
                    APA_Specification = _sumCurPageDesc,
                    AutoPartsQty = Math.Round(totalAutoPartsQtyOfCurPage, 0),
                    AutoPartsAmount = Math.Round(totalAutoPartsAmountOfCurPage, 2)
                };
                resultAllList.Add(curPageTotal);

                OrgAndCustomerAmountTransDetailReportUIModel allPageTotal = new OrgAndCustomerAmountTransDetailReportUIModel
                {
                    APA_Specification = _sumAllPageDesc,
                    AutoPartsQty = Math.Round(totalAutoPartsQtyOfAllPage, 0),
                    AutoPartsAmount = Math.Round(totalAutoPartsAmountOfAllPage, 2)
                };
                resultAllList.Add(allPageTotal);

                UltraGrid allGrid = OrgAndCustomerAmountTransDetailGrid;
                allGrid.DataSource = resultAllList;
                allGrid.DataBind();

                base.ExportAllAction(allGrid, paramGridName);

                OrgAndCustomerAmountTransDetailGrid.DataSource = _orgAndCustomerAmountTransDetailReportDS;
                OrgAndCustomerAmountTransDetailGrid.DataBind();
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
            mcbWhere_Sales_Org.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
            mcbWhere_Sales_Org.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
            mcbWhere_Sales_Org.DataSource = _orgList;

            //客户
            var tempClientList = BLLCom.GetAllCustomerList(false, LoginInfoDAX.OrgID);
            if (tempClientList != null)
            {
                _clientList = tempClientList.Where(x => x.ClientType != "供应商").ToList();
            }
            mcbWhere_Client_Name.ExtraDisplayMember = "ClientType";
            mcbWhere_Client_Name.DisplayMember = "ClientName";
            mcbWhere_Client_Name.ValueMember = "ClientID";
            mcbWhere_Client_Name.DataSource = _clientList;

            #endregion

            #region 查询条件初始化
            //销售组织
            mcbWhere_Sales_Org.Clear();
            //供应商
            mcbWhere_Client_Name.Clear();
            //销售时间开始
            dtWhere_Sales_TimeStart.Value = null;
            //销售时间结束
            dtWhere_Sales_TimeEnd.Value = null;
            //给 销售组织 设置焦点
            mcbWhere_Sales_Org.Focus();
            #endregion

            #region Grid初始化
            _orgAndCustomerAmountTransTotalReportDS = new List<OrgAndCustomerAmountTransTotalReportUIModel>();
            OrgAndCustomerAmountTransReportGrid.DataSource = _orgAndCustomerAmountTransTotalReportDS;
            OrgAndCustomerAmountTransReportGrid.DataBind();

            _orgAndCustomerAmountTransDetailReportDS = new List<OrgAndCustomerAmountTransDetailReportUIModel>();
            OrgAndCustomerAmountTransDetailGrid.DataSource = _orgAndCustomerAmountTransDetailReportDS;
            OrgAndCustomerAmountTransDetailGrid.DataBind();
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
            var activeRowIndex = OrgAndCustomerAmountTransReportGrid.ActiveRow.Index;
            //当前选中汇总行
            var curActiveTotalRow = _orgAndCustomerAmountTransTotalReportDS.FirstOrDefault(
                x =>
                    x.OrgID == OrgAndCustomerAmountTransReportGrid.Rows[activeRowIndex].Cells["OrgID"].Value?.ToString() &&
                    x.CustomerID == OrgAndCustomerAmountTransReportGrid.Rows[activeRowIndex].Cells["CustomerID"].Value?.ToString() &&
                    x.BusinessType == OrgAndCustomerAmountTransReportGrid.Rows[activeRowIndex].Cells["BusinessType"].Value?.ToString());
            if (curActiveTotalRow != null
                && !string.IsNullOrEmpty(curActiveTotalRow.CustomerID))
            {
                //查询汇总行对应的明细
                QueryOrgAndCustomerAmountTransDetailReport(curActiveTotalRow);
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
            if (OrgAndCustomerAmountTransReportGrid.ActiveRow == null || OrgAndCustomerAmountTransReportGrid.ActiveRow.Index < 0)
            {
                return false;
            }
            if (OrgAndCustomerAmountTransReportGrid.DisplayLayout.Bands[0].SortedColumns.Count > 0)
            {
                foreach (UltraGridColumn loopColumn in OrgAndCustomerAmountTransReportGrid.DisplayLayout.Bands[0].SortedColumns)
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
