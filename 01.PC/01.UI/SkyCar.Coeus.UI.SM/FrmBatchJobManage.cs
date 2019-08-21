using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.SM;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.SM;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.Server.Com;

namespace SkyCar.Coeus.UI.SM
{
    /// <summary>
    /// 作业管理
    /// </summary>
    public partial class FrmBatchJobManage : BaseFormCardList<BatchJobManageUIModel, BatchJobManageQCModel, MDLCSM_BatchJob>
    {
        #region 全局变量

        /// <summary>
        /// 作业管理BLL
        /// </summary>
        private BatchJobManageBLL _bll = new BatchJobManageBLL();

        #region 下拉框数据源

        /// <summary>
        /// 作业方式数据源
        /// </summary>
        List<ServerEnum> _batchJobTypeDs = new List<ServerEnum>();
        /// <summary>
        /// 消息推送方式数据源
        /// </summary>
        List<ServerEnum> _sendMsgModeDs = new List<ServerEnum>();
        /// <summary>
        /// 业务类别数据源
        /// </summary>
        List<ServerEnum> _businessTypeDs = new List<ServerEnum>();
        /// <summary>
        /// 执行类型数据源
        /// </summary>
        List<ServerEnum> _executionTypeDs = new List<ServerEnum>();

        #endregion

        #region 提示消息StringBuilder

        /// <summary>
        /// 执行类型对应的提示消息
        /// </summary>
        private StringBuilder executionTypeTipMessage = new StringBuilder();

        /// <summary>
        /// 执行时间部分的提示消息
        /// </summary>
        private StringBuilder executeDateTipMessage = new StringBuilder();

        /// <summary>
        /// 计划部分对象的提示消息
        /// </summary>
        private StringBuilder cronMessage = new StringBuilder();

        /// <summary>
        /// cron表达式
        /// </summary>
        private StringBuilder cronExpression = new StringBuilder();

        #endregion

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmBatchJobManage构造方法
        /// </summary>
        public FrmBatchJobManage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmBatchJobManage_Load(object sender, EventArgs e)
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
            TextBoxTool pageSizeOfList = null;
            foreach (var loopToolControl in this.toolBarManagerListTabPaging.Tools)
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
        #endregion

        #region 查询条件相关事件

        /// <summary>
        /// 作业编码KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_BJ_Code_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 作业名称KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_BJ_Name_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 作业方式ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_BJ_Pattern_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 消息类别ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_BJ_PushMode_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 业务类别ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_BJ_BusinessType_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 执行类型ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_BJ_ExecutionType_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 执行时间ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_BJ_ExecuteTime_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        #endregion

        #region 下拉框ValueChanged事件

        /// <summary>
        /// 执行类型ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbBJ_ExecutionType_ValueChanged(object sender, EventArgs e)
        {
            if (cbBJ_ExecutionType.Value == null)
            {
                return;
            }

            //清空提示消息
            executionTypeTipMessage.Clear();
            executeDateTipMessage.Clear();
            cronExpression.Clear();
            cronMessage.Clear();

            if (cbBJ_ExecutionType.Text == ExecutionTypeEnum.Name.CFZX)
            {
                #region 执行类型为[重复执行]的场合

                //[执行时间]不显示，[计划时间设置]显示
                lblBJ_ExecuteTime.Visible = false;
                dtBJ_ExecuteTime.Visible = false;
                gbSetPlanTime.Visible = true;
                gbSetPlanTime.Expanded = true;

                dtBJ_ExecuteTime.Value = null;
                GenerateCron();
                #endregion
            }
            else
            {
                #region 执行类型为[执行一次]的场合

                //[执行时间]显示，[计划时间设置]不显示
                lblBJ_ExecuteTime.Visible = true;
                dtBJ_ExecuteTime.Visible = true;
                gbSetPlanTime.Visible = false;
                gbSetPlanTime.Expanded = false;

                executionTypeTipMessage.Append("在");
                DateTime tempDateTime = DateTime.Now.AddMinutes(3);
                dtBJ_ExecuteTime.Value = tempDateTime;

                #endregion
            }
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 执行时间ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtBJ_ExecuteTime_ValueChanged(object sender, EventArgs e)
        {
            executeDateTipMessage.Clear();
            if (dtBJ_ExecuteTime.Value != null)
            {
                executeDateTipMessage.Append(dtBJ_ExecuteTime.Value);
            }
        }
        #endregion

        #region 计划时间设置——单选按钮CheckedChanged事件

        /// <summary>
        /// 月区间CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbMonthInternal_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMonthInternal.Checked)
            {
                //月区间被选择，月区间开始部分 和 月区间结束部分 可编辑
                numMonthInternalStart.Enabled = true;
                numMonthInternalEnd.Enabled = true;
                //月间隔开始日 和 月间隔月数 不可编辑
                numMonthCycleFrom.Enabled = false;
                numMonthCycleMonths.Enabled = false;
            }
            else
            {
                //月区间不被选择，月区间开始部分 和 月区间结束部分 不可编辑
                numMonthInternalStart.Enabled = false;
                numMonthInternalEnd.Enabled = false;
                //月间隔开始日 和 月间隔月数 可编辑
                numMonthCycleFrom.Enabled = true;
                numMonthCycleMonths.Enabled = true;
            }

            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 月间隔CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbMonthCycle_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMonthCycle.Checked)
            {
                //月间隔被选择，月间隔开始日 和 月间隔月数 可编辑
                numMonthCycleFrom.Enabled = true;
                numMonthCycleMonths.Enabled = true;
                //月区间开始部分 和 月区间结束部分 不可编辑
                numMonthInternalStart.Enabled = false;
                numMonthInternalEnd.Enabled = false;
            }
            else
            {
                //月间隔不被选择，月间隔开始日 和 月间隔月数 不可编辑
                numMonthCycleFrom.Enabled = false;
                numMonthCycleMonths.Enabled = false;
                //月区间开始部分 和 月区间结束部分 可编辑
                numMonthInternalStart.Enabled = true;
                numMonthInternalEnd.Enabled = true;
            }

            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 天区间CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbDayInternal_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDayInternal.Checked)
            {
                //天区间被选择，天区间开始部分 和 天区间结束部分 可编辑
                numDayInternalStart.Enabled = true;
                numDayInternalEnd.Enabled = true;
                //天间隔开始日 和 天间隔天数 不可编辑
                numDayCycleFrom.Enabled = false;
                numDayCycleDays.Enabled = false;
            }
            else
            {
                //天区间不被选择，天区间开始部分 和 天区间结束部分 不可编辑
                numDayInternalStart.Enabled = false;
                numDayInternalEnd.Enabled = false;
                //天间隔开始日 和 天间隔天数 可编辑
                numDayCycleFrom.Enabled = true;
                numDayCycleDays.Enabled = true;
            }

            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 天间隔CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbDayCycle_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDayCycle.Checked)
            {
                //天间隔被选择，天间隔开始日 和 天间隔天数 可编辑
                numDayCycleFrom.Enabled = true;
                numDayCycleDays.Enabled = true;
                //天区间开始部分 和 天区间结束部分 不可编辑
                numDayInternalStart.Enabled = false;
                numDayInternalEnd.Enabled = false;
            }
            else
            {
                //天间隔不被选择，天间隔开始日 和 天间隔天数 不可编辑
                numDayCycleFrom.Enabled = false;
                numDayCycleDays.Enabled = false;
                //天区间开始部分 和 天区间结束部分 可编辑
                numDayInternalStart.Enabled = true;
                numDayInternalEnd.Enabled = true;
            }

            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 小时区间CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbHourInternal_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHourInternal.Checked)
            {
                //小时区间被选择，小时区间开始部分 和 小时区间结束部分 可编辑
                numHourInternalStart.Enabled = true;
                numHourInternalEnd.Enabled = true;
                //小时间隔开始日 和 小时间隔小时数 不可编辑
                numHourCycleFrom.Enabled = false;
                numHourCycleHours.Enabled = false;
            }
            else
            {
                //小时区间不被选择，小时区间开始部分 和 小时区间结束部分 不可编辑
                numHourInternalStart.Enabled = false;
                numHourInternalEnd.Enabled = false;
                //小时间隔开始日 和 小时间隔小时数 可编辑
                numHourCycleFrom.Enabled = true;
                numHourCycleHours.Enabled = true;
            }

            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 小时间隔CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbHourCycle_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHourCycle.Checked)
            {
                //小时间隔被选择，小时间隔开始日 和 小时间隔小时数 可编辑
                numHourCycleFrom.Enabled = true;
                numHourCycleHours.Enabled = true;
                //小时区间开始部分 和 小时区间结束部分 不可编辑
                numHourInternalStart.Enabled = false;
                numHourInternalEnd.Enabled = false;
            }
            else
            {
                //小时间隔不被选择，小时间隔开始日 和 小时间隔小时数 不可编辑
                numHourCycleFrom.Enabled = false;
                numHourCycleHours.Enabled = false;
                //小时区间开始部分 和 小时区间结束部分 可编辑
                numHourInternalStart.Enabled = true;
                numHourInternalEnd.Enabled = true;
            }

            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 分钟区间CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbMinuteInternal_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMinuteInternal.Checked)
            {
                //分钟区间被选择，分钟区间开始部分 和 分钟区间结束部分 可编辑
                numMinuteInternalStart.Enabled = true;
                numMinuteInternalEnd.Enabled = true;
                //分钟间隔开始日 和 分钟间隔分钟数 不可编辑
                numMinuteCycleFrom.Enabled = false;
                numMinuteCycleMinutes.Enabled = false;
            }
            else
            {
                //分钟区间不被选择，分钟区间开始部分 和 分钟区间结束部分 不可编辑
                numMinuteInternalStart.Enabled = false;
                numMinuteInternalEnd.Enabled = false;
                //分钟间隔开始日 和 分钟间隔分钟数 可编辑
                numMinuteCycleFrom.Enabled = true;
                numMinuteCycleMinutes.Enabled = true;
            }

            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 分钟间隔CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbMinuteCycle_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMinuteCycle.Checked)
            {
                //分钟间隔被选择，分钟间隔开始日 和 分钟间隔分钟数 可编辑
                numMinuteCycleFrom.Enabled = true;
                numMinuteCycleMinutes.Enabled = true;
                //分钟区间开始部分 和 分钟区间结束部分 不可编辑
                numMinuteInternalStart.Enabled = false;
                numMinuteInternalEnd.Enabled = false;
            }
            else
            {
                //分钟间隔不被选择，分钟间隔开始日 和 分钟间隔分钟数 不可编辑
                numMinuteCycleFrom.Enabled = false;
                numMinuteCycleMinutes.Enabled = false;
                //分钟区间开始部分 和 分钟区间结束部分 可编辑
                numMinuteInternalStart.Enabled = true;
                numMinuteInternalEnd.Enabled = true;
            }

            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }
        #endregion

        #region 计划时间设置——时间ValueChanged事件

        /// <summary>
        /// 月区间开始部分ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numMonthInternalStart_ValueChanged(object sender, EventArgs e)
        {
            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 月区间结束部分ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numMonthInternalEnd_ValueChanged(object sender, EventArgs e)
        {
            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 月间隔开始日ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numMonthCycleFrom_ValueChanged(object sender, EventArgs e)
        {
            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 月间隔月数ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numMonthCycleMonths_ValueChanged(object sender, EventArgs e)
        {
            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 天区间开始部分ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numDayInternalStart_ValueChanged(object sender, EventArgs e)
        {
            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 天区间结束部分ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numDayInternalEnd_ValueChanged(object sender, EventArgs e)
        {
            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 天间隔开始日ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numDayCycleFrom_ValueChanged(object sender, EventArgs e)
        {
            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 天间隔天数ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numDayCycleDays_ValueChanged(object sender, EventArgs e)
        {
            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 小时区间开始ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numHourInternalStart_ValueChanged(object sender, EventArgs e)
        {
            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 小时区间结束ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numHourInternalEnd_ValueChanged(object sender, EventArgs e)
        {
            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 小时间隔开始小时ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numHourCycleFrom_ValueChanged(object sender, EventArgs e)
        {
            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 小时间隔小时数ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numHourCycleHours_ValueChanged(object sender, EventArgs e)
        {
            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 分钟区间开始部分ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numMinuteInternalStart_ValueChanged(object sender, EventArgs e)
        {
            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 分钟区间结束部分ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numMinuteInternalEnd_ValueChanged(object sender, EventArgs e)
        {
            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 分钟间隔开始分钟ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numMinuteCycleFrom_ValueChanged(object sender, EventArgs e)
        {
            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
        }

        /// <summary>
        /// 分钟间隔分钟数ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numMinuteCycleMinutes_ValueChanged(object sender, EventArgs e)
        {
            //生成计划cron格式串
            GenerateCron();
            //生成计划说明
            GenerateMessage();
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
            base.NewUIModel = DetailDS;
            if (ViewHasChanged())
            {
                //信息尚未保存，确定进行当前操作？
                DialogResult dialogResult = MessageBoxs.Show(Trans.SM, ToString(), MsgHelp.GetMsg(MsgCode.W_0001), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();
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
            //2.将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();
            //3.执行保存（含服务端检查）
            if (!_bll.SaveDetailDS(base.DetailDS))
            {
                //保存失败
                MessageBoxs.Show(Trans.SM, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //4.将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //TODO 保存成功后，通知服务器端更新作业
            //DataSyncForUpdate(base.DetailDS.BJ_ID);
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

            //2.执行删除
            #region 准备数据

            //待删除的系统作业列表
            List<MDLCSM_BatchJob> deleteBatchJobList = new List<MDLCSM_BatchJob>();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 详情删除
                if (string.IsNullOrEmpty(txtBJ_ID.Text))
                {
                    MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0016, new object[] { SystemTableEnums.Name.CSM_BatchJob, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //确认删除操作
                DialogResult dialogResult = MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0012), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                //待删除的系统作业
                MDLCSM_BatchJob deleteBatchJob = new MDLCSM_BatchJob
                {
                    WHERE_BJ_ID = txtBJ_ID.Text.Trim()
                };
                deleteBatchJobList.Add(deleteBatchJob);
                #endregion
            }
            else
            {
                #region 列表删除
                gdGrid.UpdateData();
                //勾选的系统作业列表
                List<BatchJobManageUIModel> checkedBatchJobList = GridDS.Where(x => x.IsChecked == true).ToList();
                if (checkedBatchJobList.Count == 0)
                {
                    MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { SystemTableEnums.Name.CSM_BatchJob, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //确认删除操作
                DialogResult confirmDeleteResult = MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0013, new object[] { checkedBatchJobList.Count }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (confirmDeleteResult != DialogResult.OK)
                {
                    return;
                }

                foreach (var loopCheckedBatchJob in checkedBatchJobList)
                {
                    if (string.IsNullOrEmpty(loopCheckedBatchJob.BJ_ID))
                    {
                        continue;
                    }

                    //待删除的系统作业
                    MDLCSM_BatchJob deleteBatchJob = new MDLCSM_BatchJob
                    {
                        WHERE_BJ_ID = loopCheckedBatchJob.BJ_ID
                    };
                    deleteBatchJobList.Add(deleteBatchJob);
                }
                #endregion
            }

            #endregion

            #region 删除数据
            if (deleteBatchJobList.Count > 0)
            {
                var deleteBatchJobResult = _bll.DeleteBatchJob(deleteBatchJobList);
                if (!deleteBatchJobResult)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.SM, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //删除成功
                MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            #endregion

            //3.清空【详情】画面数据
            InitializeDetailTabControls();
            //刷新列表
            RefreshList(true);

            //4.将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //TODO 删除成功后，通知服务器端删除作业
            //DataSyncForDelete(GridDSSelected);
        }

        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            //1.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
            //2.设置查询条件（翻页相关属性不用设置）
            base.ConditionDS = new BatchJobManageQCModel()
            {
                //作业编码
                WHERE_BJ_Code = txtWhere_BJ_Code.Text.Trim(),
                //作业名称
                WHERE_BJ_Name = txtWhere_BJ_Name.Text.Trim(),
                //作业方式
                WHERE_BJ_Pattern = cbWhere_BJ_Pattern.Text,
                //消息类别
                WHERE_BJ_PushMode = cbWhere_BJ_PushMode.Text,
                //业务类别
                WHERE_BJ_BusinessType = cbWhere_BJ_BusinessType.Text,
                //执行类型
                WHERE_BJ_ExecutionType = cbWhere_BJ_ExecutionType.Text,
            };
            //3.执行基类方法（注意：基类使用模糊查询）
            base.QueryAction();
            //4.Grid绑定数据源
            gdGrid.DataSource = base.GridDS;
            gdGrid.DataBind();
            //5.设置Grid自适应列宽（根据单元格内容）
            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            //6.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
        }

        /// <summary>
        /// 导出当前页
        /// </summary>
        public override void ExportAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = SystemTableEnums.Name.CSM_BatchJob;
            base.ExportAction(gdGrid, paramGridName);
        }

        /// <summary>
        /// 导出全部
        /// </summary>
        public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = SystemTableEnums.Name.CSM_BatchJob;
            List<BatchJobManageUIModel> resultAllList = new List<BatchJobManageUIModel>();
            _bll.QueryForList<MDLCSM_BatchJob, BatchJobManageUIModel>(new BatchJobManageQCModel()
            {
                PageIndex = 1,
                PageSize = null,
                //作业编码
                WHERE_BJ_Code = txtWhere_BJ_Code.Text.Trim(),
                //作业名称
                WHERE_BJ_Name = txtWhere_BJ_Name.Text.Trim(),
                //作业方式
                WHERE_BJ_Pattern = cbWhere_BJ_Pattern.Text,
                //消息类别
                WHERE_BJ_PushMode = cbWhere_BJ_PushMode.Text,
                //业务类别
                WHERE_BJ_BusinessType = cbWhere_BJ_BusinessType.Text,
                //执行类型
                WHERE_BJ_ExecutionType = cbWhere_BJ_ExecutionType.Text,
            }, resultAllList);
            UltraGrid allGrid = gdGrid;
            allGrid.DataSource = resultAllList;
            allGrid.DataBind();

            base.ExportAllAction(allGrid, paramGridName);

            gdGrid.DataSource = GridDS;
            gdGrid.DataBind();
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
            base.NewUIModel = DetailDS;
            if (ViewHasChanged())
            {
                return true;
            }
            return false;
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 工具生成
            //作业编码
            txtBJ_Code.Clear();
            //作业名称
            txtBJ_Name.Clear();
            //作业方式
            cbBJ_Pattern.Items.Clear();
            //消息类别
            cbBJ_PushMode.Items.Clear();
            //业务类别
            cbBJ_BusinessType.Items.Clear();
            //执行类型
            cbBJ_ExecutionType.Items.Clear();
            //执行时间
            dtBJ_ExecuteTime.Value = null;
            //计划生效时间
            dtBJ_PlanStartDate.Value = null;
            //计划失效时间
            dtBJ_PlanEndDate.Value = null;
            //计划说明
            txtBJ_Remark.Clear();
            //有效
            ckBJ_IsValid.Checked = true;
            ckBJ_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtBJ_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtBJ_CreatedTime.Value = DateTime.Now;
            //修改人
            txtBJ_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtBJ_UpdatedTime.Value = DateTime.Now;
            //ID
            txtBJ_ID.Clear();
            //组织ID
            txtBJ_Org_ID.Clear();
            //组织名称
            txtBJ_Org_Name.Clear();
            //作业分组
            txtBJ_GroupName.Clear();
            //类全名
            txtBJ_FullClassName.Clear();
            //执行间隔
            cbBJ_ExecutionInterval.Items.Clear();
            //执行间隔值
            txtBJ_ExecutionIntervalValue.Clear();
            //日执行类型
            cbBJ_DayExecutionType.Items.Clear();
            //日一次执行时间
            txtBJ_DayExecutionTime.Clear();
            //日执行间隔
            cbBJ_DayExecutionFrequency.Items.Clear();
            //日执行间隔值
            txtBJ_DayExecutionIntervalValue.Clear();
            //日执行间隔的开始时间
            txtBJ_DayExecutionStartTime.Clear();
            //日执行间隔的结束时间
            txtBJ_DayExecutionEndTime.Clear();
            //Cron表达式
            txtBJ_CronExpression.Clear();
            //版本号
            txtBJ_VersionNo.Clear();
            //给 作业编码 设置焦点
            lblBJ_Code.Focus();
            #endregion

            #region 初始化下拉框
            //作业方式
            _batchJobTypeDs = CoeusJobConst.GetBatchJobPattern();
            cbBJ_Pattern.DisplayMember = SysConst.EN_TEXT;
            cbBJ_Pattern.ValueMember = SysConst.EN_Code;
            cbBJ_Pattern.DataSource = _batchJobTypeDs;
            cbBJ_Pattern.DataBind();
            //默认作业方式为[调度执行]
            cbBJ_Pattern.Text = BatchJobTypeEnum.Name.DDZX;
            cbBJ_Pattern.Value = BatchJobTypeEnum.Code.DDZX;

            //消息推送方式
            _sendMsgModeDs = CoeusJobConst.GetSendMsgMode();
            cbBJ_PushMode.DisplayMember = SysConst.EN_TEXT;
            cbBJ_PushMode.ValueMember = SysConst.EN_Code;
            cbBJ_PushMode.DataSource = _sendMsgModeDs;
            cbBJ_PushMode.DataBind();
            //默认消息推送方式为[电脑]
            cbBJ_PushMode.Text = SendMsgModeEnum.Name.PC;
            cbBJ_PushMode.Value = SendMsgModeEnum.Code.PC;

            //业务类别
            _businessTypeDs = CoeusJobConst.GetBatchBusinessType();
            cbBJ_BusinessType.DisplayMember = SysConst.EN_TEXT;
            cbBJ_BusinessType.ValueMember = SysConst.EN_Code;
            cbBJ_BusinessType.DataSource = _businessTypeDs;
            cbBJ_BusinessType.DataBind();

            //执行类型
            _executionTypeDs = CoeusJobConst.GetExecutionType();
            cbBJ_ExecutionType.DisplayMember = SysConst.EN_TEXT;
            cbBJ_ExecutionType.ValueMember = SysConst.EN_Code;
            cbBJ_ExecutionType.DataSource = _executionTypeDs;
            cbBJ_ExecutionType.DataBind();
            //默认执行类型为[重复执行]
            cbBJ_ExecutionType.Text = ExecutionTypeEnum.Name.CFZX;
            cbBJ_ExecutionType.Value = ExecutionTypeEnum.Code.CFZX;

            #endregion

            #region 初始化[计划时间设置]

            #region 月

            //默认月区间开始部分、月区间结束部分为1，都不可编辑
            numMonthInternalStart.Value = 1;
            numMonthInternalEnd.Value = 1;
            numMonthInternalStart.Enabled = false;
            numMonthInternalEnd.Enabled = false;
            //默认月间隔开始日、月间隔月数为1，都不可编辑
            numMonthCycleFrom.Value = 1;
            numMonthCycleMonths.Value = 1;
            numMonthCycleFrom.Enabled = false;
            numMonthCycleMonths.Enabled = false;

            #endregion

            #region 天

            //默认天区间勾选，天间隔不勾选
            rbDayInternal.Checked = true;
            rbDayCycle.Checked = false;

            //默认天区间开始部分、天区间结束部分为1，都可编辑
            numDayInternalStart.Value = 1;
            numDayInternalEnd.Value = 1;
            numDayInternalStart.Enabled = true;
            numDayInternalEnd.Enabled = true;
            //默认天间隔开始日、天间隔天数为1，都不可编辑
            numDayCycleFrom.Value = 1;
            numDayCycleDays.Value = 1;
            numDayCycleFrom.Enabled = false;
            numDayCycleDays.Enabled = false;

            #endregion

            #region 小时

            //默认时区间勾选，时间隔不勾选
            rbHourInternal.Checked = true;
            rbHourCycle.Checked = false;

            //默认小时区间开始小时、小时区间结束为0，都可编辑
            numHourInternalStart.Value = 0;
            numHourInternalEnd.Value = 0;
            numHourInternalStart.Enabled = true;
            numHourInternalEnd.Enabled = true;
            //默认小时间隔开始小时为0、小时间隔小时数为1，都不可编辑
            numHourCycleFrom.Value = 0;
            numHourCycleHours.Value = 1;
            numHourCycleFrom.Enabled = false;
            numHourCycleHours.Enabled = false;

            #endregion

            #region 分

            //默认分区间勾选，分间隔不勾选
            rbMinuteInternal.Checked = true;
            rbMinuteCycle.Checked = false;

            //默认分钟区间开始部分、分钟区间结束部分为0，都可编辑
            numMinuteInternalStart.Value = 0;
            numMinuteInternalEnd.Value = 0;
            numMinuteInternalStart.Enabled = true;
            numMinuteInternalEnd.Enabled = true;
            //默认分钟间隔开始分钟为0、分钟间隔分钟数为1，都不可编辑
            numMinuteCycleFrom.Value = 0;
            numMinuteCycleMinutes.Value = 1;
            numMinuteCycleFrom.Enabled = false;
            numMinuteCycleMinutes.Enabled = false;

            #endregion

            #endregion

            #region 初始化组织

            //默认组织为当前组织
            txtBJ_Org_ID.Text = LoginInfoDAX.OrgID;
            txtBJ_Org_Name.Text = LoginInfoDAX.OrgShortName;
            #endregion
        }
        
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //作业编码
            txtWhere_BJ_Code.Clear();
            //作业名称
            txtWhere_BJ_Name.Clear();
            //作业方式
            cbWhere_BJ_Pattern.Items.Clear();
            //消息类别
            cbWhere_BJ_PushMode.Items.Clear();
            //业务类别
            cbWhere_BJ_BusinessType.Items.Clear();
            //执行类型
            cbWhere_BJ_ExecutionType.Items.Clear();
            //执行时间-开始
            dtWhere_BJ_ExecuteTimeStart.Value = null;
            //执行时间-终了
            dtWhere_BJ_ExecuteTimeEnd.Value = DateTime.Now;
            //给 作业编码 设置焦点
            lblWhere_BJ_Code.Focus();
            #endregion

            #region Grid初始化

            //清空Grid
            GridDS = new BindingList<BatchJobManageUIModel>();
            gdGrid.DataSource = GridDS;
            gdGrid.DataBind();

            #endregion

            #endregion

            #region 初始化下拉框
            //作业方式
            cbWhere_BJ_Pattern.DisplayMember = SysConst.EN_TEXT;
            cbWhere_BJ_Pattern.ValueMember = SysConst.EN_Code;
            cbWhere_BJ_Pattern.DataSource = _batchJobTypeDs;
            cbWhere_BJ_Pattern.DataBind();

            //消息推送方式
            cbWhere_BJ_PushMode.DisplayMember = SysConst.EN_TEXT;
            cbWhere_BJ_PushMode.ValueMember = SysConst.EN_Code;
            cbWhere_BJ_PushMode.DataSource = _sendMsgModeDs;
            cbWhere_BJ_PushMode.DataBind();

            //业务类别
            cbWhere_BJ_BusinessType.DisplayMember = SysConst.EN_TEXT;
            cbWhere_BJ_BusinessType.ValueMember = SysConst.EN_Code;
            cbWhere_BJ_BusinessType.DataSource = _businessTypeDs;
            cbWhere_BJ_BusinessType.DataBind();

            //执行类型
            cbWhere_BJ_ExecutionType.DisplayMember = SysConst.EN_TEXT;
            cbWhere_BJ_ExecutionType.ValueMember = SysConst.EN_Code;
            cbWhere_BJ_ExecutionType.DataSource = _executionTypeDs;
            cbWhere_BJ_ExecutionType.DataBind();

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
            base.NewUIModel = DetailDS;

            var activeRowIndex = gdGrid.ActiveRow.Index;
            //判断Grid内[唯一标识]是否为空
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.CSM_BatchJob.Code.BJ_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.CSM_BatchJob.Code.BJ_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            base.DetailDS = base.GridDS.FirstOrDefault(x => x.BJ_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.CSM_BatchJob.Code.BJ_ID].Value);
            if (DetailDS == null || string.IsNullOrEmpty(DetailDS.BJ_ID))
            {
                return;
            }

            if (txtBJ_ID.Text != DetailDS.BJ_ID
                || (txtBJ_ID.Text == DetailDS.BJ_ID && txtBJ_VersionNo.Text != DetailDS.BJ_VersionNo?.ToString()))
            {
                if (txtBJ_ID.Text == DetailDS.BJ_ID && txtBJ_VersionNo.Text != DetailDS.BJ_VersionNo?.ToString())
                {
                    //数据版本已过期，将加载最新详情。
                    MessageBoxs.Show(Trans.SM, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.DataHasOverdue }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else if (ViewHasChanged())
                {
                    //将放弃之前的修改，是否继续？
                    DialogResult dialogResult = MessageBoxs.Show(Trans.SM, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.ConfirmGiveUpEdit }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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

            //控制详情
            SetDetailControl();
        }

        /// <summary>
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //作业编码
            txtBJ_Code.Text = base.DetailDS.BJ_Code;
            //作业名称
            txtBJ_Name.Text = base.DetailDS.BJ_Name;
            //作业方式
            cbBJ_Pattern.Text = base.DetailDS.BJ_Pattern;
            //消息类别
            cbBJ_PushMode.Text = base.DetailDS.BJ_PushMode;
            //业务类别
            cbBJ_BusinessType.Text = base.DetailDS.BJ_BusinessType;
            //执行类型
            cbBJ_ExecutionType.Text = base.DetailDS.BJ_ExecutionType;
            //执行时间
            dtBJ_ExecuteTime.Value = base.DetailDS.BJ_ExecuteTime;
            //计划生效时间
            dtBJ_PlanStartDate.Value = base.DetailDS.BJ_PlanStartDate;
            //计划失效时间
            dtBJ_PlanEndDate.Value = base.DetailDS.BJ_PlanEndDate;
            //计划说明
            txtBJ_Remark.Text = base.DetailDS.BJ_Remark;
            //有效
            if (base.DetailDS.BJ_IsValid != null)
            {
                ckBJ_IsValid.Checked = base.DetailDS.BJ_IsValid.Value;
            }
            //创建人
            txtBJ_CreatedBy.Text = base.DetailDS.BJ_CreatedBy;
            //创建时间
            dtBJ_CreatedTime.Value = base.DetailDS.BJ_CreatedTime;
            //修改人
            txtBJ_UpdatedBy.Text = base.DetailDS.BJ_UpdatedBy;
            //修改时间
            dtBJ_UpdatedTime.Value = base.DetailDS.BJ_UpdatedTime;
            //ID
            txtBJ_ID.Text = base.DetailDS.BJ_ID;
            //组织ID
            txtBJ_Org_ID.Text = base.DetailDS.BJ_Org_ID;
            //组织名称
            txtBJ_Org_Name.Text = base.DetailDS.BJ_Org_Name;
            //作业分组
            txtBJ_GroupName.Text = base.DetailDS.BJ_GroupName;
            //类全名
            txtBJ_FullClassName.Text = base.DetailDS.BJ_FullClassName;
            //执行间隔
            cbBJ_ExecutionInterval.Text = base.DetailDS.BJ_ExecutionInterval;
            //执行间隔值
            txtBJ_ExecutionIntervalValue.Value = base.DetailDS.BJ_ExecutionIntervalValue;
            //日执行类型
            cbBJ_DayExecutionType.Text = base.DetailDS.BJ_DayExecutionType;
            //日一次执行时间
            txtBJ_DayExecutionTime.Text = base.DetailDS.BJ_DayExecutionTime;
            //日执行间隔
            cbBJ_DayExecutionFrequency.Text = base.DetailDS.BJ_DayExecutionFrequency;
            //日执行间隔值
            txtBJ_DayExecutionIntervalValue.Value = base.DetailDS.BJ_DayExecutionIntervalValue;
            //日执行间隔的开始时间
            txtBJ_DayExecutionStartTime.Text = base.DetailDS.BJ_DayExecutionStartTime;
            //日执行间隔的结束时间
            txtBJ_DayExecutionEndTime.Text = base.DetailDS.BJ_DayExecutionEndTime;
            //Cron表达式
            txtBJ_CronExpression.Text = base.DetailDS.BJ_CronExpression;
            //版本号
            txtBJ_VersionNo.Value = base.DetailDS.BJ_VersionNo;
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
            //验证作业名称
            if (string.IsNullOrEmpty(txtBJ_Name.Text))
            {
                //作业名称不能为空
                MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.CSM_BatchJob.Name.BJ_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBJ_Name.Focus();
                return false;
            }
            //验证作业方式
            if (string.IsNullOrEmpty(cbBJ_Pattern.Text))
            {
                //请选择作业名称
                MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.EXECUTE_PATTERN }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbBJ_Pattern.Focus();
                return false;
            }
            //验证消息推送方式
            if (string.IsNullOrEmpty(cbBJ_PushMode.Text))
            {
                //请选择消息推送方式
                MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.PUSH_MODE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbBJ_PushMode.Focus();
                return false;
            }
            //验证业务类别
            if (string.IsNullOrEmpty(cbBJ_BusinessType.Text))
            {
                //请选择业务类别
                MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.BUSINESS_TYPE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbBJ_BusinessType.Focus();
                return false;
            }
            //验证执行类型
            if (string.IsNullOrEmpty(cbBJ_ExecutionType.Text))
            {
                //请选择执行类型
                MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.EXECUTE_TYPE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbBJ_ExecutionType.Focus();
                return false;
            }

            return true;
        }
   
        /// <summary>
        /// 前端检查-删除
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForDelete()
        {
            return true;
        }
    
        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            base.DetailDS = new BatchJobManageUIModel()
            {
                //作业编码
                BJ_Code = txtBJ_Code.Text.Trim(),
                //作业名称
                BJ_Name = txtBJ_Name.Text.Trim(),
                //作业方式
                BJ_Pattern = cbBJ_Pattern.Text,
                //消息类别
                BJ_PushMode = cbBJ_PushMode.Text,
                //业务类别 
                BJ_BusinessType = cbBJ_BusinessType.Text,
                //执行类型
                BJ_ExecutionType = cbBJ_ExecutionType.Text,
                //执行时间
                BJ_ExecuteTime = (DateTime?)dtBJ_ExecuteTime.Value,
                //计划生效时间
                BJ_PlanStartDate = (DateTime?)dtBJ_PlanStartDate.Value,
                //计划失效时间
                BJ_PlanEndDate = (DateTime?)dtBJ_PlanEndDate.Value,
                //计划说明
                BJ_Remark = txtBJ_Remark.Text.Trim(),
                //有效
                BJ_IsValid = ckBJ_IsValid.Checked,
                //创建人
                BJ_CreatedBy = txtBJ_CreatedBy.Text.Trim(),
                //创建时间
                BJ_CreatedTime = (DateTime?)dtBJ_CreatedTime.Value ?? DateTime.Now,
                //修改人
                BJ_UpdatedBy = txtBJ_UpdatedBy.Text.Trim(),
                //修改时间
                BJ_UpdatedTime = (DateTime?)dtBJ_UpdatedTime.Value ?? DateTime.Now,
                //ID
                BJ_ID = txtBJ_ID.Text.Trim(),
                //组织ID
                BJ_Org_ID = txtBJ_Org_ID.Text.Trim(),
                //组织名称
                BJ_Org_Name = txtBJ_Org_Name.Text.Trim(),
                //作业分组
                BJ_GroupName = txtBJ_GroupName.Text.Trim(),
                //类全名
                BJ_FullClassName = txtBJ_FullClassName.Text.Trim(),
                //执行间隔
                BJ_ExecutionInterval = cbBJ_ExecutionInterval.Text,
                //执行间隔值
                BJ_ExecutionIntervalValue = Convert.ToInt32(txtBJ_ExecutionIntervalValue.Text.Trim() == "" ? "1" : txtBJ_ExecutionIntervalValue.Text.Trim()),
                //日执行类型
                BJ_DayExecutionType = cbBJ_DayExecutionType.Text,
                //日一次执行时间
                BJ_DayExecutionTime = txtBJ_DayExecutionTime.Text.Trim(),
                //日执行间隔
                BJ_DayExecutionFrequency = cbBJ_DayExecutionFrequency.Text,
                //日执行间隔值
                BJ_DayExecutionIntervalValue = Convert.ToInt32(txtBJ_DayExecutionIntervalValue.Text.Trim() == "" ? "1" : txtBJ_DayExecutionIntervalValue.Text.Trim()),
                //日执行间隔的开始时间
                BJ_DayExecutionStartTime = txtBJ_DayExecutionStartTime.Text.Trim(),
                //日执行间隔的结束时间
                BJ_DayExecutionEndTime = txtBJ_DayExecutionEndTime.Text.Trim(),
                //Cron表达式
                BJ_CronExpression = txtBJ_CronExpression.Text.Trim(),
                //版本号
                BJ_VersionNo = Convert.ToInt64(txtBJ_VersionNo.Text.Trim() == "" ? "1" : txtBJ_VersionNo.Text.Trim()),
            };
        }

        /// <summary>
        /// 控制详情
        /// </summary>
        private void SetDetailControl()
        {
            if (cbBJ_ExecutionType.Text == ExecutionTypeEnum.Name.CFZX)
            {
                #region 执行类型为[重复执行]的场合

                //[执行时间]不显示，[计划时间设置]显示
                lblBJ_ExecuteTime.Visible = false;
                dtBJ_ExecuteTime.Visible = false;
                gbSetPlanTime.Visible = true;
                gbSetPlanTime.Expanded = true;

                //解析Cron表达式
                ParaseCronExpression(txtBJ_CronExpression.Text);
                #endregion
            }
            else
            {
                #region 执行类型为[执行一次]的场合

                //[执行时间]显示，[计划时间设置]不显示
                lblBJ_ExecuteTime.Visible = true;
                dtBJ_ExecuteTime.Visible = true;
                gbSetPlanTime.Visible = false;
                gbSetPlanTime.Expanded = false;

                #endregion
            }
        }

        /// <summary>
        /// 生成计划cron格式串
        /// </summary>
        private void GenerateCron()
        {
            StringBuilder tempCron = new StringBuilder();
            cronExpression.Clear();
            tempCron.Append("? ");
            cronMessage.Clear();

            if (rbMonthInternal.Checked || rbMonthCycle.Checked)
            {
                if (rbMonthInternal.Checked)
                {
                    if (numMonthInternalEnd.Value == numMonthInternalStart.Value)
                    {
                        cronMessage.AppendFormat("{0}月", numMonthInternalStart.Value);
                        tempCron.AppendFormat("{0} ", numMonthInternalStart.Value);
                    }
                    else
                    {
                        cronMessage.AppendFormat("{0}月到{1}月，", numMonthInternalStart.Value, numMonthInternalEnd.Value);
                        tempCron.AppendFormat("{0}-{1} ", numMonthInternalStart.Value, numMonthInternalEnd.Value);
                    }
                }
                else if (rbMonthCycle.Checked)
                {
                    cronMessage.AppendFormat("{0}月开始每{1}月，", numMonthCycleFrom.Value, numMonthCycleMonths.Value);
                    tempCron.AppendFormat("{0}/{1} ", numMonthCycleFrom.Value, numMonthCycleMonths.Value);
                }
            }
            else
            {
                cronMessage.Append("每月");
                tempCron.Append("* ");
            }

            if (rbDayInternal.Checked || rbDayCycle.Checked)
            {
                if (rbDayInternal.Checked)
                {
                    if (numDayInternalEnd.Value == numDayInternalStart.Value)
                    {
                        cronMessage.AppendFormat("{0}日", numDayInternalStart.Value);
                        tempCron.AppendFormat("{0} ", numDayInternalStart.Value);
                    }
                    else
                    {
                        cronMessage.AppendFormat("{0}日到{1}日，", numDayInternalStart.Value, numDayInternalEnd.Value);
                        tempCron.AppendFormat("{0}-{1} ", numDayInternalStart.Value, numDayInternalEnd.Value);
                    }
                }
                else if (rbDayCycle.Checked)
                {
                    cronMessage.AppendFormat("{0}日开始每{1}天，", numDayCycleFrom.Value, numDayCycleDays.Value);
                    tempCron.AppendFormat("{0}/{1} ", numDayCycleFrom.Value, numDayCycleDays.Value);
                }
            }
            else
            {
                cronMessage.Append("每天");
                tempCron.Append("* ");
            }

            if (rbHourCycle.Checked || rbHourInternal.Checked)
            {
                if (rbHourInternal.Checked)
                {
                    if (numHourInternalEnd.Value == numHourInternalStart.Value)
                    {
                        cronMessage.AppendFormat("{0}点", numHourInternalStart.Value);
                        tempCron.AppendFormat("{0} ", numHourInternalStart.Value);
                    }
                    else
                    {
                        cronMessage.AppendFormat("{0}点到{1}点，", numHourInternalStart.Value, numHourInternalEnd.Value);
                        tempCron.AppendFormat("{0}-{1} ", numHourInternalStart.Value, numHourInternalEnd.Value);
                    }
                }
                else if (rbHourCycle.Checked)
                {
                    cronMessage.AppendFormat("{0}点开始每{1}个小时，", numHourCycleFrom.Value, numHourCycleHours.Value);
                    tempCron.AppendFormat("{0}/{1} ", numHourCycleFrom.Value, numHourCycleHours.Value);
                }
            }
            else
            {
                cronMessage.Append("每小时");
                tempCron.Append("* ");
            }

            if (rbMinuteCycle.Checked || rbMinuteInternal.Checked)
            {
                if (rbMinuteInternal.Checked)
                {
                    if (numMinuteInternalEnd.Value == numMinuteInternalStart.Value)
                    {
                        cronMessage.AppendFormat("{0}分", numMinuteInternalStart.Value);
                        tempCron.AppendFormat("{0} ", numMinuteInternalStart.Value);
                    }
                    else
                    {
                        cronMessage.AppendFormat("{0}分到{1}分", numMinuteInternalStart.Value, numMinuteInternalEnd.Value);
                        tempCron.AppendFormat("{0}-{1} ", numMinuteInternalStart.Value, numMinuteInternalEnd.Value);
                    }
                }
                else if (rbMinuteCycle.Checked)
                {
                    cronMessage.AppendFormat("{0}分开始每{1}分钟，", numMinuteCycleFrom.Value, numMinuteCycleMinutes.Value);
                    tempCron.AppendFormat("{0}/{1} ", numMinuteCycleFrom.Value, numMinuteCycleMinutes.Value);
                }
            }
            else
            {
                cronMessage.Append("每分钟");
                tempCron.Append("* ");
            }

            tempCron.Append("0");
            var arrayCron = tempCron.ToString().Split(' ');

            for (int i = arrayCron.Length - 1; i >= 0; i--)
            {
                cronExpression.Append(arrayCron[i] + ' ');
            }
            //base.DetailDS.BJ_CronExpression = cronExpression.ToString().TrimEnd(' ');
            txtBJ_CronExpression.Text = cronExpression.ToString().TrimEnd(' ');
        }

        /// <summary>
        /// 生成计划说明
        /// </summary>
        private void GenerateMessage()
        {
            if (!string.IsNullOrEmpty(cbBJ_Pattern.Text)
                && !string.IsNullOrEmpty(cbBJ_BusinessType.Text)
                && !string.IsNullOrEmpty(cbBJ_ExecutionType.Text))
            {
                //业务类别：
                //执行类型 + 执行时间 + 计划部分对象 + 作业方式
                txtBJ_Remark.Text = cbBJ_BusinessType.Text + ":\n" + executionTypeTipMessage + executeDateTipMessage + cronMessage + cbBJ_Pattern.Text;
            }
        }

        #region Cron表达式解析

        /// <summary>
        /// 解析Cron表达式
        /// </summary>
        /// <param name="paranCronString">Cron字符串</param>
        private void ParaseCronExpression(string paranCronString)
        {
            if (!string.IsNullOrEmpty(paranCronString))
            {
                var regs = paranCronString.Split(' ');
                if (regs.Length == 6)
                {
                    ParaseMinuteOfCronExpression(regs[1]);
                    ParaseHourOfCronExpression(regs[2]);
                    ParaseDayOfCronExpression(regs[3]);
                    ParaseMonthOfCronExpression(regs[4]);
                }
            }
        }
        /// <summary>
        /// 解析月Cron表达式
        /// </summary>
        /// <param name="paramMonthString">月字符串</param>
        private void ParaseMonthOfCronExpression(string paramMonthString)
        {
            if (!string.IsNullOrEmpty(paramMonthString))
            {
                if ("*" == paramMonthString)
                {
                    //rbDayCycle.Checked = rbDayInternal.Checked  = false;
                    rbMonthInternal.Checked = true;
                    rbMonthCycle.Checked = false;
                    numMonthInternalStart.Value = 1;
                    numMonthInternalEnd.Value = 12;
                    numMonthCycleFrom.Value = 1;
                    numMonthCycleMonths.Value = 1;

                }
                else if (paramMonthString.Split('-').Length > 1)
                {
                    var arr = paramMonthString.Split('-');
                    rbMonthInternal.Checked = true;
                    rbMonthCycle.Checked = false;
                    numMonthInternalStart.Value = int.Parse(arr[0]);
                    numMonthInternalEnd.Value = int.Parse(arr[1]);
                    numMonthCycleFrom.Value = 1;
                    numMonthCycleFrom.Value = 1;
                }
                else if (paramMonthString.Split('/').Length > 1)
                {
                    var arr = paramMonthString.Split('/');
                    rbMonthInternal.Checked = false;
                    rbMonthCycle.Checked = true;
                    numMonthCycleFrom.Value = int.Parse(arr[0]);
                    numMonthCycleFrom.Value = int.Parse(arr[1]);
                    numMonthInternalStart.Value = 1;
                    numMonthInternalEnd.Value = 1;
                }
                else
                {
                    rbMonthInternal.Checked = true;
                    rbMonthCycle.Checked = false;
                    numMonthInternalStart.Value = numMonthInternalEnd.Value = int.Parse(paramMonthString);
                    numMonthCycleFrom.Value = 1;
                    numMonthCycleFrom.Value = 1;
                }
            }
        }
        /// <summary>
        /// 解析天Cron表达式
        /// </summary>
        /// <param name="paramDayString"></param>
        private void ParaseDayOfCronExpression(string paramDayString)
        {
            if (!string.IsNullOrEmpty(paramDayString))
            {
                if ("*" == paramDayString)
                {
                    //rbDayCycle.Checked = rbDayInternal.Checked  = false;
                    rbDayInternal.Checked = true;
                    rbDayCycle.Checked = false;
                    numDayInternalStart.Value = 1;
                    numDayInternalEnd.Value = 31;
                    numDayCycleFrom.Value = 1;
                    numDayCycleDays.Value = 1;

                }
                else if (paramDayString.Split('-').Length > 1)
                {
                    var arr = paramDayString.Split('-');
                    rbDayInternal.Checked = true;
                    rbDayCycle.Checked = false;
                    numDayInternalStart.Value = int.Parse(arr[0]);
                    numDayInternalStart.Value = int.Parse(arr[1]);
                    numDayCycleFrom.Value = 1;
                    numDayCycleDays.Value = 1;
                }
                else if (paramDayString.Split('/').Length > 1)
                {
                    var arr = paramDayString.Split('/');
                    rbDayInternal.Checked = false;
                    rbDayCycle.Checked = true;
                    numDayCycleFrom.Value = int.Parse(arr[0]);
                    numDayCycleDays.Value = int.Parse(arr[1]);
                    numDayInternalStart.Value = 1;
                    numDayInternalStart.Value = 1;
                }
                else
                {
                    rbDayInternal.Checked = true;
                    rbDayCycle.Checked = false;
                    numDayInternalStart.Value = numDayInternalStart.Value = int.Parse(paramDayString);
                    numDayCycleFrom.Value = 1;
                    numDayCycleDays.Value = 1;
                }
            }
        }
        /// <summary>
        /// 解析小时Cron表达式
        /// </summary>
        /// <param name="paramHourString"></param>
        private void ParaseHourOfCronExpression(string paramHourString)
        {
            if (!string.IsNullOrEmpty(paramHourString))
            {
                if ("*" == paramHourString)
                {
                    //rbHourCycle.Checked = rbHourInternal.Checked = false;
                    rbHourInternal.Checked = true;
                    rbHourCycle.Checked = false;
                    numHourInternalStart.Value = 0;
                    numHourInternalEnd.Value = 23;
                    numHourCycleFrom.Value = 0;
                    numHourCycleHours.Value = 1;
                }
                else if (paramHourString.Split('-').Length > 1)
                {
                    var arr = paramHourString.Split('-');
                    rbHourInternal.Checked = true;
                    rbHourCycle.Checked = false;
                    numHourInternalStart.Value = int.Parse(arr[0]);
                    numHourInternalEnd.Value = int.Parse(arr[1]);
                    numHourCycleFrom.Value = 0;
                    numHourCycleHours.Value = 1;
                }
                else if (paramHourString.Split('/').Length > 1)
                {
                    var arr = paramHourString.Split('/');
                    rbHourInternal.Checked = false;
                    rbHourCycle.Checked = true;
                    numHourCycleFrom.Value = int.Parse(arr[0]);
                    numHourCycleHours.Value = int.Parse(arr[1]);
                    numHourInternalStart.Value = 0;
                    numHourInternalEnd.Value = 0;
                }
                else
                {
                    rbHourInternal.Checked = true;
                    rbHourCycle.Checked = false;
                    numHourInternalStart.Value = numHourInternalEnd.Value = int.Parse(paramHourString);
                    numHourCycleFrom.Value = 0;
                    numHourCycleHours.Value = 1;
                }
            }
        }
        /// <summary>
        /// 解析分钟Cron表达式
        /// </summary>
        /// <param name="paramMinuteString"></param>
        private void ParaseMinuteOfCronExpression(string paramMinuteString)
        {
            if (!string.IsNullOrEmpty(paramMinuteString))
            {
                if ("*" == paramMinuteString)
                {
                    //rbMinuteCycle.Checked = rbMinuteInternal.Checked = false;
                    rbMinuteInternal.Checked = true;
                    rbMinuteCycle.Checked = false;
                    numMinuteInternalStart.Value = 0;
                    numMinuteInternalEnd.Value = 59;
                    numMinuteCycleFrom.Value = 0;
                    numMinuteCycleMinutes.Value = 1;
                }
                else if (paramMinuteString.Split('-').Length > 1)
                {
                    var arr = paramMinuteString.Split('-');
                    rbMinuteInternal.Checked = true;
                    rbMinuteCycle.Checked = false;
                    numMinuteInternalStart.Value = int.Parse(arr[0]);
                    numMinuteInternalEnd.Value = int.Parse(arr[1]);
                    numMinuteCycleFrom.Value = 0;
                    numMinuteCycleMinutes.Value = 1;
                }
                else if (paramMinuteString.Split('/').Length > 1)
                {
                    var arr = paramMinuteString.Split('/');
                    rbMinuteInternal.Checked = false;
                    rbMinuteCycle.Checked = true;
                    numMinuteCycleFrom.Value = int.Parse(arr[0]);
                    numMinuteCycleMinutes.Value = int.Parse(arr[1]);
                    numMinuteInternalStart.Value = 0;
                    numMinuteInternalEnd.Value = 0;
                }
                else
                {
                    rbMinuteInternal.Checked = true;
                    rbMinuteCycle.Checked = false;
                    numMinuteInternalStart.Value = numMinuteInternalEnd.Value = int.Parse(paramMinuteString);
                    numMinuteCycleFrom.Value = 0;
                    numMinuteCycleMinutes.Value = 1;
                }
            }
        }
        #endregion

        #region 数据变动向服务端同步

        //TODO
        //private void DataSyncForUpdate(string bj_id)
        //{
        //    Thread th = new Thread(() =>
        //    {
        //        int count = 1;
        //        while (count <= 3)
        //        {
        //            try
        //            {
        //                WCFClientComObj.DataSyncClient.JobUpdate(bj_id, SystemConfigInfo.OrgPlatformCode, SysConst.ProductCode);
        //                return;
        //            }
        //            catch (Exception)
        //            {
        //                count++;
        //                Thread.Sleep(500);
        //                continue;
        //            }
        //        }

        //    });
        //    th.Start();
        //}

        //private void DataSyncForDelete(ObservableCollection<ManageBatchJobUIModel> batchJobs)
        //{
        //    Thread th = new Thread(() =>
        //    {
        //        List<WCFClient.VenusServiceReference.MDLCSM_BatchJob> batchJobsForDelete = new List<WCFClient.VenusServiceReference.MDLCSM_BatchJob>();
        //        foreach (var batchJob in batchJobs)
        //        {
        //            batchJobsForDelete.Add(new WCFClient.VenusServiceReference.MDLCSM_BatchJob()
        //            {
        //                BJ_BusinessType = batchJob.BJ_BusinessType,
        //                BJ_Pattern = batchJob.BJ_Pattern,
        //                BJ_Name = batchJob.BJ_Name,
        //                BJ_ID = batchJob.BJ_ID,
        //                BJ_Code = batchJob.BJ_Code,
        //                BJ_IsValid = batchJob.BJ_IsValid
        //            });
        //        }


        //        int count = 1;
        //        while (count <= 3)
        //        {
        //            try
        //            {
        //                WCFClientComObj.DataSyncClient.JobDelete(batchJobsForDelete.ToArray(), SystemConfigInfo.OrgPlatformCode, SysConst.ProductCode);
        //                return;
        //            }
        //            catch (Exception)
        //            {
        //                count++;
        //                Thread.Sleep(500);
        //                continue;
        //            }
        //        }


        //    });
        //    th.Start();
        //}

        #endregion

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
                    var removeList = GridDS.Where(x => x.IsChecked == true).ToList();
                    foreach (var loopRemove in removeList)
                    {
                        GridDS.Remove(loopRemove);
                    }
                }
                else
                {
                    var curHead = GridDS.FirstOrDefault(x => x.BJ_ID == DetailDS.BJ_ID);
                    if (curHead != null)
                    {
                        GridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = GridDS.FirstOrDefault(x => x.BJ_ID == DetailDS.BJ_ID);
                if (curHead != null)
                {
                    _bll.CopyModel(DetailDS, curHead);
                }
                else
                {
                    GridDS.Insert(0, DetailDS);
                }
            }

            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }

        #endregion

    }
}
