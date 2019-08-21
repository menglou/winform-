using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.SM;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.SM;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.Common.Enums;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;

namespace SkyCar.Coeus.UI.SM
{
    /// <summary>
    /// 已授权汽修商户授权查询
    /// </summary>
    public partial class FrmMerchantAuthorityQuery : BaseFormCardList<MerchantAuthorityQueryUIModel, MerchantAuthorityQueryQCModel, MDLSM_AROrgSupMerchantAuthority>
    {
        #region 全局变量

        /// <summary>
        /// 已授权汽修商户授权查询BLL
        /// </summary>
        private MerchantAuthorityQueryBLL _bll = new MerchantAuthorityQueryBLL();
        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmMerchantAuthorityQuery构造方法
        /// </summary>
        public FrmMerchantAuthorityQuery()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMerchantAuthorityQuery_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（动作）
            base.ToolBarActionAndNavigate = toolBarAction;
            //基类.工具栏（翻页）
            base.ToolBarPaging = toolBarPaging;
            //查询委托（基类控制翻页用）
            base.ExecuteQuery = QueryAction;
            //工具栏（动作）单击事件
            this.toolBarAction.ToolClick += new ToolClickEventHandler(base.toolBarActionAndNavigate_ToolClick);
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
            //设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
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

        /// <summary>
        /// 【列表】Grid的Row的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
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
        }
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
                MessageBoxs.Show(Trans.SM, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功！
            MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //4.将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();
        }

        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            //1.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
            //2.设置查询条件（翻页相关属性不用设置）
            base.ConditionDS = new MerchantAuthorityQueryQCModel()
            {
                //汽修商户名称
                WHERE_ASAH_ARMerchant_Name = txtWhere_ASAH_ARMerchant_Name.Text.Trim(),
                //汽修组织名称
                WHERE_ASAH_AROrg_Name = txtWhere_ASAH_AROrg_Name.Text.Trim(),
                //有效
                WHERE_ASAH_IsValid = ckWhere_ASAH_IsValid.Checked,
            };
            //3.执行基类方法（注意：基类使用模糊查询）
            base.QueryAction();

            //4.Grid绑定数据源
            gdGrid.DataSource = base.GridDS;
            gdGrid.DataBind();
            //5.设置Grid自适应列宽（根据单元格内容）
            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }

        /// <summary>
        /// 同步
        /// </summary>
        public override void SynchronizeAction()
        {
            base.SynchronizeAction();
            //同步组织信息
            if (MerchantAuthorityQueryBLL.SynchronizeSupMerchantAuthorityInfo())
            {
                //同步汽修商户授权信息成功
                MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.SYNCHRONIZE_SUCCESS }), MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                //同步汽修商户授权信息失败
                MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.SYNCHRONIZE_FAIL }), MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 清空
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
            //汽修商户编码
            txtASAH_ARMerchant_Code.Clear();
            //汽修商户名称
            txtASAH_ARMerchant_Name.Clear();
            //汽修组织编码
            txtASAH_AROrg_Code.Clear();
            //汽修组织名称
            txtASAH_AROrg_Name.Clear();
            //汽修组织联系人
            txtASAH_AROrg_Contacter.Clear();
            //汽修组织联系方式
            txtASAH_AROrg_Phone.Clear();
            //备注
            txtASAH_Remark.Clear();
            //有效
            ckASAH_IsValid.Checked = true;
            ckASAH_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtASAH_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtASAH_CreatedTime.Value = BLLCom.GetCurStdDatetime();
            //修改人
            txtASAH_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtASAH_UpdatedTime.Value = BLLCom.GetCurStdDatetime();
            //版本号
            txtASAH_VersionNo.Clear();
            //ID
            txtASAH_ID.Clear();
            //给 汽修商户编码 设置焦点
            lblASAH_ARMerchant_Code.Focus();
            #endregion
        }

        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //汽修商户名称
            txtWhere_ASAH_ARMerchant_Name.Clear();
            //汽修组织名称
            txtWhere_ASAH_AROrg_Name.Clear();
            //有效
            ckWhere_ASAH_IsValid.Checked = true;
            ckWhere_ASAH_IsValid.CheckState = CheckState.Checked;
            //给 汽修商户名称 设置焦点
            lblWhere_ASAH_ARMerchant_Name.Focus();
            #endregion

            //清空grid数据
            GridDS = new BindingList<MerchantAuthorityQueryUIModel>();
            gdGrid.DataSource = base.GridDS;
            gdGrid.DataBind();
            base.ClearAction();

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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SM_AROrgSupMerchantAuthority.Code.ASAH_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SM_AROrgSupMerchantAuthority.Code.ASAH_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            base.DetailDS = base.GridDS.FirstOrDefault(x => x.ASAH_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SM_AROrgSupMerchantAuthority.Code.ASAH_ID].Value);
            if (DetailDS == null || string.IsNullOrEmpty(DetailDS.ASAH_ID))
            {
                return;
            }

            if (txtASAH_ID.Text != DetailDS.ASAH_ID
                || (txtASAH_ID.Text == DetailDS.ASAH_ID && txtASAH_VersionNo.Text != DetailDS.ASAH_VersionNo?.ToString()))
            {
                if (txtASAH_ID.Text == DetailDS.ASAH_ID && txtASAH_VersionNo.Text != DetailDS.ASAH_VersionNo?.ToString())
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
        }
       
        /// <summary>
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //汽修商户编码
            txtASAH_ARMerchant_Code.Text = base.DetailDS.ASAH_ARMerchant_Code ?? "";
            //汽修商户名称
            txtASAH_ARMerchant_Name.Text = base.DetailDS.ASAH_ARMerchant_Name ?? "";
            //汽修组织编码
            txtASAH_AROrg_Code.Text = base.DetailDS.ASAH_AROrg_Code ?? "";
            //汽修组织名称
            txtASAH_AROrg_Name.Text = base.DetailDS.ASAH_AROrg_Name ?? "";
            //汽修组织联系人
            txtASAH_AROrg_Contacter.Text = base.DetailDS.ASAH_AROrg_Contacter ?? "";
            //汽修组织联系方式
            txtASAH_AROrg_Phone.Text = base.DetailDS.ASAH_AROrg_Phone ?? "";
            //备注
            txtASAH_Remark.Text = base.DetailDS.ASAH_Remark ?? "";
            //有效
            if (base.DetailDS.ASAH_IsValid != null)
            {
                ckASAH_IsValid.Checked = base.DetailDS.ASAH_IsValid.Value;
            }
            //创建人
            txtASAH_CreatedBy.Text = base.DetailDS.ASAH_CreatedBy ?? "";
            //创建时间
            dtASAH_CreatedTime.Value = base.DetailDS.ASAH_CreatedTime == null ? "" : base.DetailDS.ASAH_CreatedTime.ToString();
            //修改人
            txtASAH_UpdatedBy.Text = base.DetailDS.ASAH_UpdatedBy ?? "";
            //修改时间
            dtASAH_UpdatedTime.Value = base.DetailDS.ASAH_UpdatedTime == null ? "" : base.DetailDS.ASAH_UpdatedTime.ToString();
            //版本号
            txtASAH_VersionNo.Text = base.DetailDS.ASAH_VersionNo == null ? "" : base.DetailDS.ASAH_VersionNo.ToString();
            //ID
            txtASAH_ID.Text = base.DetailDS.ASAH_ID ?? "";

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
                foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn loopColumns in gdGrid.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (loopColumns.IsGroupByColumn)
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
            //判断显示的是否为【列表】Tab
            if (string.IsNullOrEmpty(txtASAH_ARMerchant_Name.Text))
            {
                //汽修商户名称不能为空
                MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.SM_AROrgSupMerchantAuthority.Name.ASAH_ARMerchant_Name }), MessageBoxButtons.OK,
                   MessageBoxIcon.Information);
                txtASAH_ARMerchant_Name.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            base.DetailDS = new MerchantAuthorityQueryUIModel()
            {
                //汽修商户编码
                ASAH_ARMerchant_Code = txtASAH_ARMerchant_Code.Text.Trim(),
                //汽修商户名称
                ASAH_ARMerchant_Name = txtASAH_ARMerchant_Name.Text.Trim(),
                //汽修组织编码
                ASAH_AROrg_Code = txtASAH_AROrg_Code.Text.Trim(),
                //汽修组织名称
                ASAH_AROrg_Name = txtASAH_AROrg_Name.Text.Trim(),
                //汽修组织联系人
                ASAH_AROrg_Contacter = txtASAH_AROrg_Contacter.Text.Trim(),
                //汽修组织联系方式
                ASAH_AROrg_Phone = txtASAH_AROrg_Phone.Text.Trim(),
                //备注
                ASAH_Remark = txtASAH_Remark.Text.Trim(),
                //是否有效
                ASAH_IsValid = ckASAH_IsValid.Checked,
                //创建人
                ASAH_CreatedBy = txtASAH_CreatedBy.Text.Trim(),
                //创建时间
                ASAH_CreatedTime = (DateTime?)dtASAH_CreatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //修改人
                ASAH_UpdatedBy = LoginInfoDAX.UserName,
                //修改时间
                ASAH_UpdatedTime = DateTime.Now,
                //版本号
                ASAH_VersionNo = Convert.ToInt64(txtASAH_VersionNo.Text.Trim() == "" ? "1" : txtASAH_VersionNo.Text.Trim()),
                //ID
                ASAH_ID = txtASAH_ID.Text.Trim(),
            };
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        private void RefreshList()
        {
            var curHead = GridDS.FirstOrDefault(x => x.ASAH_ID == DetailDS.ASAH_ID);
            if (curHead != null)
            {
                _bll.CopyModel(DetailDS, curHead);
            }
            else
            {
                GridDS.Insert(0, DetailDS);
            }

            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }
        #endregion

    }
}
