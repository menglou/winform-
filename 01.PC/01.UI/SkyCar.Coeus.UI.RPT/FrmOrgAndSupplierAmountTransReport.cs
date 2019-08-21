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
    /// 组织与供应商资金往来统计报表
    /// </summary>
    public partial class FrmOrgAndSupplierAmountTransReport : BaseFormCardListDetail<OrgAndSupplierAmountTransTotalReportUIModel, OrgAndSupplierAmountTransReportQCModel, OrgAndSupplierAmountTransTotalReportUIModel>
    {
        #region 全局变量

        /// <summary>
        /// 组织与供应商资金往来统计报表BLL
        /// </summary>
        private OrgAndSupplierAmountTransReportBLL _bll = new OrgAndSupplierAmountTransReportBLL();

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
        /// 组织与供应商资金往来统计汇总数据源
        /// </summary>
        private List<OrgAndSupplierAmountTransTotalReportUIModel> _orgAndSupplierAmountTransTotalReportDs = new List<OrgAndSupplierAmountTransTotalReportUIModel>();
        /// <summary>
        /// 组织与供应商资金往来统计明细数据源
        /// </summary>
        private List<OrgAndSupplierAmountTransDetailReportUIModel> _orgAndSupplierAmountTransDetailReportDs = new List<OrgAndSupplierAmountTransDetailReportUIModel>();

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
        /// FrmOrgAndSupplierAmountTransReport构造方法
        /// </summary>
        public FrmOrgAndSupplierAmountTransReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmOrgAndSupplierAmountTransReport_Load(object sender, EventArgs e)
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
        private void OrgAndSupplierAmountTransReportGrid_DoubleClick(object sender, EventArgs e)
        {
            //将Grid中选中的数据赋值给【详情】Tab内的对应控件
            SetGridDataToCardCtrls();
        }
        /// <summary>
        /// 【汇总】Grid的Cell的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrgAndSupplierAmountTransReportGrid_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            //将Grid中选中的数据赋值给【详情】Tab内的对应控件
            SetGridDataToCardCtrls();
        }
        /// <summary>
        /// 【汇总】Grid的CellChange事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrgAndSupplierAmountTransReportGrid_CellChange(object sender, CellEventArgs e)
        {
            OrgAndSupplierAmountTransReportGrid.UpdateData();
        }
        #endregion

        #region 【明细】Grid相关事件

        #endregion

        #region 【查询条件】相关事件
        /// <summary>
        /// 出/入库时间终了ValueChanged事件
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
            //1.设置查询条件（翻页相关属性不用设置）
            base.ConditionDS = new OrgAndSupplierAmountTransReportQCModel()
            {
                //客户ID
                SupplierID = this.mcbWhere_Supplier_Name.SelectedValue,
            };
            //组织ID
            if (string.IsNullOrEmpty(mcbWhere_Org.SelectedValue))
            {
                var tmpOrgList = _orgList.Select(p => p.Org_ID).ToList();

                if (tmpOrgList.Count > 0)
                {
                    ConditionDS.OrgIdList = string.Join(SysConst.Semicolon_DBC, tmpOrgList);
                }
            }
            else
            {
                ConditionDS.OrgIdList = mcbWhere_Org.SelectedValue;
            }

            //出/入库时间-开始
            if (this.dtWhere_CreatedTimeStart.Value != null)
            {
                ConditionDS.StartTime = this.dtWhere_CreatedTimeStart.DateTime;
            }
            else
            {
                ConditionDS.StartTime = null;
            }
            //出/入库时间-终了
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
                QueryOrgAndSupplierAmountTransTotalReport();
            }
            else
            {
                QueryOrgAndSupplierAmountTransDetailReport();
            }
        }

        #region 查询汇总
        /// <summary>
        /// 查询组织与供应商资金往来统计汇总
        /// </summary>
        private void QueryOrgAndSupplierAmountTransTotalReport()
        {
            try
            {
                OrgAndSupplierAmountTransReportQCModel argsModel = ConditionDS;
                argsModel.PageSize = PageSizeOfTotal;
                argsModel.PageIndex = PageIndexOfTotal;
                //SqlId
                argsModel.SqlId = SQLID.RPT_OrgAndSupplierAmountTransReport_SQL01;

                _orgAndSupplierAmountTransTotalReportDs = new List<OrgAndSupplierAmountTransTotalReportUIModel>();
                _bll.QueryForList(argsModel.SqlId, argsModel, _orgAndSupplierAmountTransTotalReportDs);

                decimal totalAccountPayableAmountOfCurPage = 0;
                decimal totalPaidAmountOfCurPage = 0;
                decimal totalUnpaidAmountOfCurPage = 0;
                decimal totalAccountPayableAmountOfAllPage = 0;
                decimal totalPaidAmountOfAllPage = 0;
                decimal totalUnpaidAmountOfAllPage = 0;

                if (_orgAndSupplierAmountTransTotalReportDs.Count > 0)
                {
                    OrgAndSupplierAmountTransTotalReportUIModel subObject = _orgAndSupplierAmountTransTotalReportDs[0];
                    TotalRecordCountOfTotal = subObject.RecordCount ?? 0;
                    totalAccountPayableAmountOfAllPage = (subObject.TotalAccountPayableAmount ?? 0);
                    totalPaidAmountOfAllPage = (subObject.TotalPaidAmount ?? 0);
                    totalUnpaidAmountOfAllPage = (subObject.TotalUnpaidAmount ?? 0);
                }
                else
                {
                    TotalRecordCountOfTotal = 0;
                }

                foreach (var loopSotckInTotalItem in _orgAndSupplierAmountTransTotalReportDs)
                {
                    totalAccountPayableAmountOfCurPage += (loopSotckInTotalItem.APB_AccountPayableAmount ?? 0);
                    totalPaidAmountOfCurPage += (loopSotckInTotalItem.APB_PaidAmount ?? 0);
                    totalUnpaidAmountOfCurPage += (loopSotckInTotalItem.APB_UnpaidAmount ?? 0);
                }
                OrgAndSupplierAmountTransTotalReportUIModel curPageTotal = new OrgAndSupplierAmountTransTotalReportUIModel
                {
                    SupplierName = _sumCurPageDesc,
                    APB_AccountPayableAmount = Math.Round(totalAccountPayableAmountOfCurPage, 2),
                    APB_PaidAmount = Math.Round(totalPaidAmountOfCurPage, 2),
                    APB_UnpaidAmount = Math.Round(totalUnpaidAmountOfCurPage, 2)
                };
                _orgAndSupplierAmountTransTotalReportDs.Add(curPageTotal);

                OrgAndSupplierAmountTransTotalReportUIModel allPageTotal = new OrgAndSupplierAmountTransTotalReportUIModel
                {
                    SupplierName = _sumAllPageDesc,
                    APB_AccountPayableAmount = Math.Round(totalAccountPayableAmountOfAllPage, 2),
                    APB_PaidAmount = Math.Round(totalPaidAmountOfAllPage, 2),
                    APB_UnpaidAmount = Math.Round(totalUnpaidAmountOfAllPage, 2)
                };
                _orgAndSupplierAmountTransTotalReportDs.Add(allPageTotal);

                //重新计算[总页数]，设置最新[allPageTotal]
                SetTotalPageCountAndTotalRecordCountOfTotal(TotalRecordCountOfTotal);
                //设置翻页按钮状态
                SetPageButtonStatusOfTotal();

                OrgAndSupplierAmountTransReportGrid.DataSource = _orgAndSupplierAmountTransTotalReportDs;
                OrgAndSupplierAmountTransReportGrid.DataBind();
                OrgAndSupplierAmountTransReportGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
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
        /// 查询组织与供应商资金往来统计明细
        /// </summary>
        /// <param name="paramTotalReport">汇总行</param>
        private void QueryOrgAndSupplierAmountTransDetailReport(OrgAndSupplierAmountTransTotalReportUIModel paramTotalReport = null)
        {
            try
            {
                OrgAndSupplierAmountTransReportQCModel argsModel = ConditionDS;
                argsModel.PageSize = PageSizeOfDetail;
                argsModel.PageIndex = PageIndexOfDetail;
                argsModel.SqlId = SQLID.RPT_OrgAndSupplierAmountTransReport_SQL02;
                if (paramTotalReport != null)
                {
                    argsModel.OrgIdList = paramTotalReport.OrgID;
                    argsModel.SupplierID = paramTotalReport.SupplierID;
                    argsModel.BusinessType = paramTotalReport.BusinessType;
                }
                else
                {
                    argsModel.BusinessType = null;
                }

                _orgAndSupplierAmountTransDetailReportDs = new List<OrgAndSupplierAmountTransDetailReportUIModel>();
                _bll.QueryForList(argsModel.SqlId, argsModel, _orgAndSupplierAmountTransDetailReportDs);

                decimal totalAutoPartsQtyOfCurPage = 0;
                decimal totalAutoPartsAmountOfCurPage = 0;
                decimal totalAutoPartsQtyOfAllPage = 0;
                decimal totalAutoPartsAmountOfAllPage = 0;

                if (_orgAndSupplierAmountTransDetailReportDs.Count > 0)
                {
                    OrgAndSupplierAmountTransDetailReportUIModel subObject = _orgAndSupplierAmountTransDetailReportDs[0];
                    TotalRecordCountOfDetail = (subObject.RecordCount ?? 0);
                    totalAutoPartsQtyOfAllPage = (subObject.TotalAutoPartsQty ?? 0);
                    totalAutoPartsAmountOfAllPage = (subObject.TotalAutoPartsAmount ?? 0);
                }
                else
                {
                    TotalRecordCountOfDetail = 0;
                }

                foreach (var loopDetail in _orgAndSupplierAmountTransDetailReportDs)
                {
                    totalAutoPartsQtyOfCurPage += (loopDetail.AutoPartsQty ?? 0);
                    totalAutoPartsAmountOfCurPage += (loopDetail.AutoPartsAmount ?? 0);
                }
                OrgAndSupplierAmountTransDetailReportUIModel curPageTotal = new OrgAndSupplierAmountTransDetailReportUIModel
                {
                    APA_Specification = _sumCurPageDesc,
                    AutoPartsQty = Math.Round(totalAutoPartsQtyOfCurPage, 0),
                    AutoPartsAmount = Math.Round(totalAutoPartsAmountOfCurPage, 2)
                };
                _orgAndSupplierAmountTransDetailReportDs.Add(curPageTotal);

                OrgAndSupplierAmountTransDetailReportUIModel allPageTotal = new OrgAndSupplierAmountTransDetailReportUIModel
                {
                    APA_Specification = _sumAllPageDesc,
                    AutoPartsQty = Math.Round(totalAutoPartsQtyOfAllPage, 0),
                    AutoPartsAmount = Math.Round(totalAutoPartsAmountOfAllPage, 2)
                };
                _orgAndSupplierAmountTransDetailReportDs.Add(allPageTotal);

                //重新计算[总页数]，设置最新[总记录条数]
                SetTotalPageCountAndTotalRecordCountOfDetail(TotalRecordCountOfDetail);
                //设置翻页按钮状态
                SetPageButtonStatusOfDetail();

                OrgAndSupplierAmountTransDetailGrid.DataSource = _orgAndSupplierAmountTransDetailReportDs;
                OrgAndSupplierAmountTransDetailGrid.DataBind();
                OrgAndSupplierAmountTransDetailGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
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
            mcbWhere_Org.Clear();
            //供应商
            mcbWhere_Supplier_Name.Clear();
            //出/入库时间开始
            dtWhere_CreatedTimeStart.Value = null;
            //出/入库时间结束
            dtWhere_CreatedTimeEnd.Value = null;
            //给 销售组织 设置焦点
            mcbWhere_Org.Focus();
            #endregion

            #region Grid初始化
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                _orgAndSupplierAmountTransTotalReportDs = new List<OrgAndSupplierAmountTransTotalReportUIModel>();
                OrgAndSupplierAmountTransReportGrid.DataSource = _orgAndSupplierAmountTransTotalReportDs;
                OrgAndSupplierAmountTransReportGrid.DataBind();
            }
            else
            {
                _orgAndSupplierAmountTransDetailReportDs = new List<OrgAndSupplierAmountTransDetailReportUIModel>();
                OrgAndSupplierAmountTransDetailGrid.DataSource = _orgAndSupplierAmountTransDetailReportDs;
                OrgAndSupplierAmountTransDetailGrid.DataBind();
            }
            #endregion
        }

        /// <summary>
        /// 导出当前页
        /// </summary>
        public override void ExportAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = tabControlFull.Tabs[SysConst.EN_LIST].Selected ? "组织与供应商资金往来统计汇总" : "组织与供应商资金往来统计明细";
            base.ExportAction(tabControlFull.Tabs[SysConst.EN_LIST].Selected ? OrgAndSupplierAmountTransReportGrid : OrgAndSupplierAmountTransDetailGrid, paramGridName);
        }

        /// <summary>
        /// 导出全部
        /// </summary>
        public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = tabControlFull.Tabs[SysConst.EN_LIST].Selected ? "组织与供应商资金往来统计汇总" : "组织与供应商资金往来统计明细";
            OrgAndSupplierAmountTransReportQCModel tempConditionDS = new OrgAndSupplierAmountTransReportQCModel()
            {
                //SqlId
                SqlId = tabControlFull.Tabs[SysConst.EN_LIST].Selected ? SQLID.RPT_OrgAndSupplierAmountTransReport_SQL01 : SQLID.RPT_OrgAndSupplierAmountTransReport_SQL02,
                //客户ID
                SupplierID = this.mcbWhere_Supplier_Name.SelectedValue,
                PageIndex = 1,
                PageSize = null
            };
            //组织ID
            if (string.IsNullOrEmpty(mcbWhere_Org.SelectedValue))
            {
                var tmpOrgList = _orgList.Select(p => p.Org_ID).ToList();

                if (tmpOrgList.Count > 0)
                {
                    ConditionDS.OrgIdList = string.Join(SysConst.Semicolon_DBC, tmpOrgList);
                }
            }
            else
            {
                ConditionDS.OrgIdList = mcbWhere_Org.SelectedValue;
            }

            if (this.dtWhere_CreatedTimeStart.Value != null)
            {
                //出/入库时间-开始
                tempConditionDS.StartTime = this.dtWhere_CreatedTimeStart.DateTime;
            }
            if (this.dtWhere_CreatedTimeEnd.Value != null)
            {
                //出/入库时间-终了
                tempConditionDS.EndTime = this.dtWhere_CreatedTimeEnd.DateTime;
            }

            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                List<OrgAndSupplierAmountTransTotalReportUIModel> resultAllList = new List<OrgAndSupplierAmountTransTotalReportUIModel>();
                _bll.QueryForList(SQLID.RPT_OrgAndSupplierAmountTransReport_SQL01, tempConditionDS, resultAllList);

                decimal totalAccountPayableAmountOfCurPage = 0;
                decimal totalPaidAmountOfCurPage = 0;
                decimal totalAccountPayableAmountOfAllPage = 0;
                decimal totalPaidAmountOfAllPage = 0;

                if (resultAllList.Count > 0)
                {
                    OrgAndSupplierAmountTransTotalReportUIModel subObject = resultAllList[0];
                    totalAccountPayableAmountOfAllPage = (subObject.TotalAccountPayableAmount ?? 0);
                    totalPaidAmountOfAllPage = (subObject.TotalPaidAmount ?? 0);
                }

                foreach (var loopSotckInTotalItem in resultAllList)
                {
                    totalAccountPayableAmountOfCurPage += (loopSotckInTotalItem.APB_AccountPayableAmount ?? 0);
                    totalPaidAmountOfCurPage += (loopSotckInTotalItem.APB_PaidAmount ?? 0);
                }
                OrgAndSupplierAmountTransTotalReportUIModel curPageTotal = new OrgAndSupplierAmountTransTotalReportUIModel
                {
                    SupplierName = _sumCurPageDesc,
                    APB_AccountPayableAmount = Math.Round(totalAccountPayableAmountOfCurPage, 2),
                    APB_PaidAmount = Math.Round(totalPaidAmountOfCurPage, 2)
                };
                resultAllList.Add(curPageTotal);

                OrgAndSupplierAmountTransTotalReportUIModel allPageTotal = new OrgAndSupplierAmountTransTotalReportUIModel
                {
                    SupplierName = _sumAllPageDesc,
                    APB_AccountPayableAmount = Math.Round(totalAccountPayableAmountOfAllPage, 2),
                    APB_PaidAmount = Math.Round(totalPaidAmountOfAllPage, 2)
                };
                resultAllList.Add(allPageTotal);

                UltraGrid allGrid = OrgAndSupplierAmountTransReportGrid;
                allGrid.DataSource = resultAllList;
                allGrid.DataBind();

                base.ExportAllAction(allGrid, paramGridName);

                OrgAndSupplierAmountTransReportGrid.DataSource = _orgAndSupplierAmountTransTotalReportDs;
                OrgAndSupplierAmountTransReportGrid.DataBind();
            }
            else
            {
                List<OrgAndSupplierAmountTransDetailReportUIModel> resultAllList = new List<OrgAndSupplierAmountTransDetailReportUIModel>();
                _bll.QueryForList(SQLID.RPT_OrgAndSupplierAmountTransReport_SQL02, tempConditionDS, resultAllList);

                decimal totalAutoPartsQtyOfCurPage = 0;
                decimal totalAutoPartsAmountOfCurPage = 0;
                decimal totalAutoPartsQtyOfAllPage = 0;
                decimal totalAutoPartsAmountOfAllPage = 0;

                if (resultAllList.Count > 0)
                {
                    OrgAndSupplierAmountTransDetailReportUIModel subObject = resultAllList[0];
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
                OrgAndSupplierAmountTransDetailReportUIModel curPageTotal = new OrgAndSupplierAmountTransDetailReportUIModel
                {
                    APA_Specification = _sumCurPageDesc,
                    AutoPartsQty = Math.Round(totalAutoPartsQtyOfCurPage, 0),
                    AutoPartsAmount = Math.Round(totalAutoPartsAmountOfCurPage, 2)
                };
                resultAllList.Add(curPageTotal);

                OrgAndSupplierAmountTransDetailReportUIModel allPageTotal = new OrgAndSupplierAmountTransDetailReportUIModel
                {
                    APA_Specification = _sumAllPageDesc,
                    AutoPartsQty = Math.Round(totalAutoPartsQtyOfAllPage, 0),
                    AutoPartsAmount = Math.Round(totalAutoPartsAmountOfAllPage, 2)
                };
                resultAllList.Add(allPageTotal);

                UltraGrid allGrid = OrgAndSupplierAmountTransDetailGrid;
                allGrid.DataSource = resultAllList;
                allGrid.DataBind();

                base.ExportAllAction(allGrid, paramGridName);

                OrgAndSupplierAmountTransDetailGrid.DataSource = _orgAndSupplierAmountTransDetailReportDs;
                OrgAndSupplierAmountTransDetailGrid.DataBind();
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
            mcbWhere_Org.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
            mcbWhere_Org.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
            mcbWhere_Org.DataSource = _orgList;

            //客户
            var tempClientList = BLLCom.GetAllCustomerList(true, LoginInfoDAX.OrgID);
            if (tempClientList != null)
            {
                _clientList = tempClientList.Where(x => x.ClientType == "供应商").ToList();
            }
            mcbWhere_Supplier_Name.ExtraDisplayMember = "ClientType";
            mcbWhere_Supplier_Name.DisplayMember = "ClientName";
            mcbWhere_Supplier_Name.ValueMember = "ClientID";
            mcbWhere_Supplier_Name.DataSource = _clientList;

            #endregion

            #region 查询条件初始化
            //销售组织
            mcbWhere_Org.Clear();
            //供应商
            mcbWhere_Supplier_Name.Clear();
            //出/入库时间开始
            dtWhere_CreatedTimeStart.Value = null;
            //出/入库时间结束
            dtWhere_CreatedTimeEnd.Value = null;
            //给 销售组织 设置焦点
            mcbWhere_Org.Focus();
            #endregion

            #region Grid初始化
            _orgAndSupplierAmountTransTotalReportDs = new List<OrgAndSupplierAmountTransTotalReportUIModel>();
            OrgAndSupplierAmountTransReportGrid.DataSource = _orgAndSupplierAmountTransTotalReportDs;
            OrgAndSupplierAmountTransReportGrid.DataBind();

            _orgAndSupplierAmountTransDetailReportDs = new List<OrgAndSupplierAmountTransDetailReportUIModel>();
            OrgAndSupplierAmountTransDetailGrid.DataSource = _orgAndSupplierAmountTransDetailReportDs;
            OrgAndSupplierAmountTransDetailGrid.DataBind();
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
            var activeRowIndex = OrgAndSupplierAmountTransReportGrid.ActiveRow.Index;
            //当前选中汇总行
            var curActiveTotalRow = _orgAndSupplierAmountTransTotalReportDs.FirstOrDefault(
                x =>
                    x.OrgID == OrgAndSupplierAmountTransReportGrid.Rows[activeRowIndex].Cells["OrgID"].Value?.ToString() &&
                    x.SupplierID == OrgAndSupplierAmountTransReportGrid.Rows[activeRowIndex].Cells["SupplierID"].Value?.ToString() &&
                    x.BusinessType == OrgAndSupplierAmountTransReportGrid.Rows[activeRowIndex].Cells["BusinessType"].Value?.ToString());
            if (curActiveTotalRow != null
                && !string.IsNullOrEmpty(curActiveTotalRow.SupplierID))
            {
                //查询汇总行对应的明细
                QueryOrgAndSupplierAmountTransDetailReport(curActiveTotalRow);
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
            if (OrgAndSupplierAmountTransReportGrid.ActiveRow == null || OrgAndSupplierAmountTransReportGrid.ActiveRow.Index < 0)
            {
                return false;
            }
            if (OrgAndSupplierAmountTransReportGrid.DisplayLayout.Bands[0].SortedColumns.Count > 0)
            {
                foreach (UltraGridColumn loopColumn in OrgAndSupplierAmountTransReportGrid.DisplayLayout.Bands[0].SortedColumns)
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
