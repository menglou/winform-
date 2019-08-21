using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.RPT;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UIModel.RPT.QCModel;
using SkyCar.Coeus.UIModel.RPT.UIModel;

namespace SkyCar.Coeus.UI.RPT
{
    /// <summary>
    /// 经营情况统计报表
    /// </summary>
    public partial class FrmManagementSituationReport : BaseFormCardListDetail<ManagementSituationTotalReportUIModel, ManagementSituationReportQCModel, ManagementSituationTotalReportUIModel>
    {
        #region 全局变量

        /// <summary>
        /// 经营情况统计报表BLL
        /// </summary>
        private ManagementSituationReportBLL _bll = new ManagementSituationReportBLL();

        /// <summary>
        /// 合计
        /// </summary>
        string _sumCurPageDesc = "合计：";

        #region 下拉框数据源

        /// <summary>
        /// 组织数据源
        /// </summary>
        List<MDLSM_Organization> _orgList = new List<MDLSM_Organization>();

        #endregion

        #region Grid数据源
        /// <summary>
        /// 经营情况统计汇总数据源
        /// </summary>
        private List<ManagementSituationTotalReportUIModel> _managementSituationTotalReportDs = new List<ManagementSituationTotalReportUIModel>();

        /// <summary>
        /// 【销售统计】根据时间统计销售数据源
        /// </summary>
        private List<ManagementSituationDetailReportUIModel> _statisticsSalesByTimeReportDs = new List<ManagementSituationDetailReportUIModel>();

        /// <summary>
        /// 【销售统计】根据产品统计销售数据源
        /// </summary>
        private List<ManagementSituationDetailReportUIModel> _statisticsSalesByProductReportDs = new List<ManagementSituationDetailReportUIModel>();

        /// <summary>
        /// 【销售统计】根据客户统计销售数据源
        /// </summary>
        private List<ManagementSituationDetailReportUIModel> _statisticsSalesByCustomerReportDs = new List<ManagementSituationDetailReportUIModel>();

        #endregion

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmManagementSituationReport构造方法
        /// </summary>
        public FrmManagementSituationReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmManagementSituationReport_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（动作）
            base.ToolBarActionAndNavigate = toolBarActionAndNavigate;
            //查询委托（基类控制翻页用）
            base.ExecuteQuery = QueryAction;
            //工具栏（动作）单击事件
            this.toolBarActionAndNavigate.ToolClick += new ToolClickEventHandler(base.toolBarActionAndNavigate_ToolClick);

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

        /// <summary>
        /// 统计时间终了ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_StatisticsTimeEnd_ValueChanged(object sender, EventArgs e)
        {
            if (this.dtWhere_StatisticsTimeEnd.Value != null &&
                this.dtWhere_StatisticsTimeEnd.DateTime.Hour == 0 &&
                this.dtWhere_StatisticsTimeEnd.DateTime.Minute == 0 &&
                this.dtWhere_StatisticsTimeEnd.DateTime.Second == 0)
            {
                DateTime newDateTime = new DateTime(this.dtWhere_StatisticsTimeEnd.DateTime.Year, this.dtWhere_StatisticsTimeEnd.DateTime.Month, this.dtWhere_StatisticsTimeEnd.DateTime.Day, 23, 59, 59);
                this.dtWhere_StatisticsTimeEnd.DateTime = newDateTime;
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
            base.ConditionDS = new ManagementSituationReportQCModel();

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

            //统计时间-开始
            if (this.dtWhere_StatisticsTimeStart.Value != null)
            {
                ConditionDS.StartTime = this.dtWhere_StatisticsTimeStart.DateTime;
            }
            else
            {
                ConditionDS.StartTime = null;
            }
            //统计时间-终了
            if (this.dtWhere_StatisticsTimeEnd.Value != null)
            {
                ConditionDS.EndTime = this.dtWhere_StatisticsTimeEnd.DateTime;
            }
            else
            {
                ConditionDS.EndTime = null;
            }

            //2.查询并绑定数据源
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                //查询经营情况统计汇总
                QueryManagementSituationTotalReport();
            }
            else
            {
                //根据时间统计销售
                QueryStatisticsSalesByTimeReport();
                //根据产品统计销售
                QueryStatisticsSalesByProductReport();
                //根据客户统计销售
                QueryStatisticsSalesByCustomerReport();
            }
        }

        #region 查询汇总
        /// <summary>
        /// 查询经营情况统计汇总
        /// </summary>
        private void QueryManagementSituationTotalReport()
        {
            try
            {
                ManagementSituationReportQCModel argsModel = ConditionDS;

                argsModel.SqlId = SQLID.RPT_ManagementSituationReport_SQL01;

                _managementSituationTotalReportDs = new List<ManagementSituationTotalReportUIModel>();
                _bll.QueryForList(argsModel.SqlId, argsModel, _managementSituationTotalReportDs);

                ManagementSituationReportGrid.DataSource = _managementSituationTotalReportDs;
                ManagementSituationReportGrid.DataBind();
                ManagementSituationReportGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 查询销售统计

        /// <summary>
        /// 根据时间统计销售
        /// </summary>
        private void QueryStatisticsSalesByTimeReport()
        {
            try
            {
                ManagementSituationReportQCModel argsModel = ConditionDS;

                if (rbByMonth.Checked)
                {
                    //按月统计
                    argsModel.StatisticsMode = "Month";
                }
                else if (rbByQuarter.Checked)
                {
                    //按季度统计
                    argsModel.StatisticsMode = "Quarter";
                }
                else if (rbByYear.Checked)
                {
                    //按年统计
                    argsModel.StatisticsMode = "Year";
                }

                //是否包含退货
                argsModel.IsContainReturn = ckByTimeIsContainReturn.Checked;

                argsModel.SqlId = SQLID.RPT_ManagementSituationReport_SQL02;
                _statisticsSalesByTimeReportDs = new List<ManagementSituationDetailReportUIModel>();
                _bll.QueryForList(argsModel.SqlId, argsModel, _statisticsSalesByTimeReportDs);

                decimal totalSalesQtyOfCurPage = 0;
                decimal totalSalesAmountOfCurPage = 0;
                decimal totalGrossProfitAmountOfCurPage = 0;

                foreach (var loopDetail in _statisticsSalesByTimeReportDs)
                {
                    totalSalesQtyOfCurPage += (loopDetail.SalesQty ?? 0);
                    totalSalesAmountOfCurPage += (loopDetail.SalesAmount ?? 0);
                    totalGrossProfitAmountOfCurPage += (loopDetail.GrossProfitAmount ?? 0);
                }
                ManagementSituationDetailReportUIModel curPageTotal = new ManagementSituationDetailReportUIModel
                {
                    StatisticsType = _sumCurPageDesc,
                    SalesQty = Math.Round(totalSalesQtyOfCurPage, 0),
                    SalesAmount = Math.Round(totalSalesAmountOfCurPage, 2),
                    GrossProfitAmount = Math.Round(totalGrossProfitAmountOfCurPage, 2)
                };
                _statisticsSalesByTimeReportDs.Add(curPageTotal);

                TJSalesByTimeGrid.DataSource = _statisticsSalesByTimeReportDs;
                TJSalesByTimeGrid.DataBind();
                TJSalesByTimeGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据产品统计销售
        /// </summary>
        private void QueryStatisticsSalesByProductReport()
        {
            try
            {
                ManagementSituationReportQCModel argsModel = ConditionDS;

                if (rbSellTopTenProduct.Checked)
                {
                    //统计畅销前十产品
                    argsModel.StatisticsMode = "BySellQty";
                }
                else if (rbProfitTopTenProduct.Checked)
                {
                    //统计毛利润前十产品
                    argsModel.StatisticsMode = "ByProfit";
                }

                //是否包含退货
                argsModel.IsContainReturn = ckByProductIsContainReturn.Checked;

                argsModel.SqlId = SQLID.RPT_ManagementSituationReport_SQL03;
                _statisticsSalesByProductReportDs = new List<ManagementSituationDetailReportUIModel>();
                _bll.QueryForList(argsModel.SqlId, argsModel, _statisticsSalesByProductReportDs);

                decimal totalSalesQtyOfCurPage = 0;
                decimal totalSalesAmountOfCurPage = 0;
                decimal totalGrossProfitAmountOfCurPage = 0;

                foreach (var loopDetail in _statisticsSalesByProductReportDs)
                {
                    totalSalesQtyOfCurPage += (loopDetail.SalesQty ?? 0);
                    totalSalesAmountOfCurPage += (loopDetail.SalesAmount ?? 0);
                    totalGrossProfitAmountOfCurPage += (loopDetail.GrossProfitAmount ?? 0);
                }
                ManagementSituationDetailReportUIModel curPageTotal = new ManagementSituationDetailReportUIModel
                {
                    StatisticsType = _sumCurPageDesc,
                    SalesQty = Math.Round(totalSalesQtyOfCurPage, 0),
                    SalesAmount = Math.Round(totalSalesAmountOfCurPage, 2),
                    GrossProfitAmount = Math.Round(totalGrossProfitAmountOfCurPage, 2)
                };
                _statisticsSalesByProductReportDs.Add(curPageTotal);

                TJSalesByProductGrid.DataSource = _statisticsSalesByProductReportDs;
                TJSalesByProductGrid.DataBind();
                TJSalesByProductGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据客户统计销售
        /// </summary>
        private void QueryStatisticsSalesByCustomerReport()
        {
            try
            {
                ManagementSituationReportQCModel argsModel = ConditionDS;

                if (rbSalesAmountTopTenCustomer.Checked)
                {
                    //统计销售额前十客户
                    argsModel.StatisticsMode = "BySalesAmount";
                }
                else if (rbProfitTopTenCustomer.Checked)
                {
                    //统计毛利润前十客户
                    argsModel.StatisticsMode = "ByProfit";
                }
                argsModel.SqlId = SQLID.RPT_ManagementSituationReport_SQL04;

                //是否包含退货
                argsModel.IsContainReturn = ckByCustomerIsContainReturn.Checked;

                _statisticsSalesByCustomerReportDs = new List<ManagementSituationDetailReportUIModel>();
                _bll.QueryForList(argsModel.SqlId, argsModel, _statisticsSalesByCustomerReportDs);

                decimal totalSalesQtyOfCurPage = 0;
                decimal totalSalesAmountOfCurPage = 0;
                decimal totalGrossProfitAmountOfCurPage = 0;

                foreach (var loopDetail in _statisticsSalesByCustomerReportDs)
                {
                    totalSalesQtyOfCurPage += (loopDetail.SalesQty ?? 0);
                    totalSalesAmountOfCurPage += (loopDetail.SalesAmount ?? 0);
                    totalGrossProfitAmountOfCurPage += (loopDetail.GrossProfitAmount ?? 0);
                }
                ManagementSituationDetailReportUIModel curPageTotal = new ManagementSituationDetailReportUIModel
                {
                    StatisticsType = _sumCurPageDesc,
                    SalesQty = Math.Round(totalSalesQtyOfCurPage, 0),
                    SalesAmount = Math.Round(totalSalesAmountOfCurPage, 2),
                    GrossProfitAmount = Math.Round(totalGrossProfitAmountOfCurPage, 2)
                };
                _statisticsSalesByCustomerReportDs.Add(curPageTotal);

                TJSalesByCustomerGrid.DataSource = _statisticsSalesByCustomerReportDs;
                TJSalesByCustomerGrid.DataBind();
                TJSalesByCustomerGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 清空
        /// </summary>
        public override void ClearAction()
        {
            #region 查询条件初始化
            //组织
            mcbWhere_Org.Clear();
            //统计时间开始
            dtWhere_StatisticsTimeStart.Value = null;
            //统计时间结束
            dtWhere_StatisticsTimeEnd.Value = null;
            //给 组织 设置焦点
            mcbWhere_Org.Focus();

            //设置默认值
            ckByTimeIsContainReturn.Checked = false;
            rbByMonth.Checked = true;
            ckByProductIsContainReturn.Checked = false;
            rbSellTopTenProduct.Checked = true;
            ckByCustomerIsContainReturn.Checked = false;
            rbSalesAmountTopTenCustomer.Checked = true;
            #endregion

            #region Grid初始化
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                //经营情况统计汇总数据源
                _managementSituationTotalReportDs = new List<ManagementSituationTotalReportUIModel>();
                ManagementSituationReportGrid.DataSource = _managementSituationTotalReportDs;
                ManagementSituationReportGrid.DataBind();
            }
            else
            {
                //【销售统计】根据时间统计销售数据源
                _statisticsSalesByTimeReportDs = new List<ManagementSituationDetailReportUIModel>();
                TJSalesByTimeGrid.DataSource = _statisticsSalesByTimeReportDs;
                TJSalesByTimeGrid.DataBind();

                //【销售统计】根据产品统计销售数据源
                _statisticsSalesByProductReportDs = new List<ManagementSituationDetailReportUIModel>();
                TJSalesByProductGrid.DataSource = _statisticsSalesByProductReportDs;
                TJSalesByProductGrid.DataBind();

                //【销售统计】根据客户统计销售数据源
                _statisticsSalesByCustomerReportDs = new List<ManagementSituationDetailReportUIModel>();
                TJSalesByCustomerGrid.DataSource = _statisticsSalesByCustomerReportDs;
                TJSalesByCustomerGrid.DataBind();
            }
            #endregion
        }

        /// <summary>
        /// 导出当前页
        /// </summary>
        public override void ExportAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = tabControlFull.Tabs[SysConst.EN_LIST].Selected ? "经营情况统计汇总" : "销售业绩明细";
            base.ExportAction(tabControlFull.Tabs[SysConst.EN_LIST].Selected ? ManagementSituationReportGrid : TJSalesByTimeGrid, paramGridName);

            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                paramGridName = "经营情况统计汇总";
                base.ExportAction(ManagementSituationReportGrid, paramGridName);
            }
            else
            {
                string defaultFilePath = string.Empty;

                paramGridName = "经营情况统计_根据时间统计销售";
                ExportCurPage(TJSalesByTimeGrid, ref defaultFilePath, paramGridName);

                paramGridName = "经营情况统计_根据产品统计销售";
                ExportCurPage(TJSalesByProductGrid, ref defaultFilePath, paramGridName);

                paramGridName = "经营情况统计_根据客户统计销售";
                ExportCurPage(TJSalesByCustomerGrid, ref defaultFilePath, paramGridName);
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

        ///// <summary>
        ///// 导出全部
        ///// </summary>
        //public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        //{
        //    paramGridName = tabControlFull.Tabs[SysConst.EN_LIST].Selected ? "经营情况统计" : "销售业绩明细";
        //    ManagementSituationReportQCModel tempConditionDS = new ManagementSituationReportQCModel()
        //    {
        //        //SqlId
        //        SqlId = tabControlFull.Tabs[SysConst.EN_LIST].Selected ? SQLID.RPT_ManagementSituationReport_SQL01 : SQLID.RPT_ManagementSituationReport_SQL02,
        //        //组织ID
        //        OrgID = mcbWhere_Org.SelectedValue,
        //        PageIndex = 1,
        //        PageSize = null
        //    };
        //    if (this.dtWhere_StatisticsTimeStart.Value != null)
        //    {
        //        //统计时间-开始
        //        tempConditionDS.StartTime = this.dtWhere_StatisticsTimeStart.DateTime;
        //    }
        //    if (this.dtWhere_StatisticsTimeEnd.Value != null)
        //    {
        //        //统计时间-终了
        //        tempConditionDS.EndTime = this.dtWhere_StatisticsTimeEnd.DateTime;
        //    }

        //    if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
        //    {
        //        List<ManagementSituationTotalReportUIModel> resultAllList = new List<ManagementSituationTotalReportUIModel>();
        //        _bll.QueryForList(SQLID.RPT_ManagementSituationReport_SQL01, tempConditionDS, resultAllList);

        //        decimal totalAccountPayableAmountOfCurPage = 0;
        //        decimal totalPaidAmountOfCurPage = 0;
        //        decimal totalAccountPayableAmountOfAllPage = 0;
        //        decimal totalPaidAmountOfAllPage = 0;

        //        if (resultAllList.Count > 0)
        //        {
        //            ManagementSituationTotalReportUIModel subObject = resultAllList[0];
        //            //totalAccountPayableAmountOfAllPage = (subObject.TotalSalesAmount ?? 0);
        //            //totalPaidAmountOfAllPage = (subObject.TotalSalesReturnAmount ?? 0);
        //        }

        //        foreach (var loopSotckInTotalItem in resultAllList)
        //        {
        //            totalAccountPayableAmountOfCurPage += (loopSotckInTotalItem.StatisticsAmount ?? 0);
        //            //totalPaidAmountOfCurPage += (loopSotckInTotalItem.SalesReturnAmount ?? 0);
        //        }

        //        ManagementSituationTotalReportUIModel curPageTotal = new ManagementSituationTotalReportUIModel
        //        {
        //            StatisticsType = _sumCurPageDesc,
        //            StatisticsAmount = Math.Round(totalAccountPayableAmountOfCurPage, 2),
        //            //SalesReturnAmount = Math.Round(totalPaidAmountOfCurPage, 2)
        //        };
        //        resultAllList.Add(curPageTotal);

        //        ManagementSituationTotalReportUIModel allPageTotal = new ManagementSituationTotalReportUIModel
        //        {
        //            StatisticsType = _sumAllPageDesc,
        //            StatisticsAmount = Math.Round(totalAccountPayableAmountOfAllPage, 2),
        //            //SalesReturnAmount = Math.Round(totalPaidAmountOfAllPage, 2)
        //        };
        //        resultAllList.Add(allPageTotal);

        //        UltraGrid allGrid = ManagementSituationReportGrid;
        //        allGrid.DataSource = resultAllList;
        //        allGrid.DataBind();

        //        base.ExportAllAction(allGrid, paramGridName);

        //        ManagementSituationReportGrid.DataSource = _managementSituationTotalReportDs;
        //        ManagementSituationReportGrid.DataBind();
        //    }
        //    else
        //    {
        //        List<ManagementSituationDetailReportUIModel> resultAllList = new List<ManagementSituationDetailReportUIModel>();
        //        _bll.QueryForList(SQLID.RPT_ManagementSituationReport_SQL02, tempConditionDS, resultAllList);

        //        decimal totalSalesPerforamnceQtyOfCurPage = 0;
        //        decimal totalSalesPerforamnceAmountOfCurPage = 0;

        //        //foreach (var loopDetail in resultAllList)
        //        //{
        //        //    totalSalesPerforamnceQtyOfCurPage += (loopDetail.DetailSalesQty ?? 0);
        //        //    totalSalesPerforamnceAmountOfCurPage += (loopDetail.DetailSalesAmount ?? 0);
        //        //}
        //        //ManagementSituationDetailReportUIModel curPageTotal = new ManagementSituationDetailReportUIModel
        //        //{
        //        //    SO_CustomerName = _sumCurPageDesc,
        //        //    DetailSalesQty = Math.Round(totalSalesPerforamnceQtyOfCurPage, 0),
        //        //    DetailSalesAmount = Math.Round(totalSalesPerforamnceAmountOfCurPage, 2)
        //        //};
        //        //resultAllList.Add(curPageTotal);

        //        //ManagementSituationDetailReportUIModel allPageTotal = new ManagementSituationDetailReportUIModel
        //        //{
        //        //    SO_CustomerName = _sumAllPageDesc,
        //        //    DetailSalesQty = Math.Round(totalSalesPerforamnceQtyOfAllPage, 0),
        //        //    DetailSalesAmount = Math.Round(totalSalesPerforamnceAmountOfAllPage, 2)
        //        //};
        //        //resultAllList.Add(allPageTotal);

        //        UltraGrid allGrid = TJSalesByTimeGrid;
        //        allGrid.DataSource = resultAllList;
        //        allGrid.DataBind();

        //        base.ExportAllAction(allGrid, paramGridName);

        //        TJSalesByTimeGrid.DataSource = _statisticsSalesByTimeReportDs;
        //        TJSalesByTimeGrid.DataBind();
        //    }
        //}

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
            mcbWhere_Org.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
            mcbWhere_Org.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
            mcbWhere_Org.DataSource = _orgList;

            #endregion

            #region 查询条件初始化
            //组织
            mcbWhere_Org.Clear();
            //统计时间开始
            dtWhere_StatisticsTimeStart.Value = null;
            //统计时间结束
            dtWhere_StatisticsTimeEnd.Value = null;
            //给 组织 设置焦点
            mcbWhere_Org.Focus();

            //设置默认值
            ckByTimeIsContainReturn.Checked = false;
            rbByMonth.Checked = true;
            ckByProductIsContainReturn.Checked = false;
            rbSellTopTenProduct.Checked = true;
            ckByCustomerIsContainReturn.Checked = false;
            rbSalesAmountTopTenCustomer.Checked = true;
            #endregion

            #region Grid初始化

            //经营情况统计汇总数据源
            _managementSituationTotalReportDs = new List<ManagementSituationTotalReportUIModel>();
            ManagementSituationReportGrid.DataSource = _managementSituationTotalReportDs;
            ManagementSituationReportGrid.DataBind();

            //【销售统计】根据时间统计销售数据源
            _statisticsSalesByTimeReportDs = new List<ManagementSituationDetailReportUIModel>();
            TJSalesByTimeGrid.DataSource = _statisticsSalesByTimeReportDs;
            TJSalesByTimeGrid.DataBind();

            //【销售统计】根据产品统计销售数据源
            _statisticsSalesByProductReportDs = new List<ManagementSituationDetailReportUIModel>();
            TJSalesByProductGrid.DataSource = _statisticsSalesByProductReportDs;
            TJSalesByProductGrid.DataBind();

            //【销售统计】根据客户统计销售数据源
            _statisticsSalesByCustomerReportDs = new List<ManagementSituationDetailReportUIModel>();
            TJSalesByCustomerGrid.DataSource = _statisticsSalesByCustomerReportDs;
            TJSalesByCustomerGrid.DataBind();
            #endregion
        }

        #endregion
    }
}
