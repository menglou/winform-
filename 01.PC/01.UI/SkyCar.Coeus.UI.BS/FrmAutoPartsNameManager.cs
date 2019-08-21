using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.BS;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.BS;
using SkyCar.Coeus.Common.Log;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;

namespace SkyCar.Coeus.UI.BS
{
    /// <summary>
    /// 配件名称
    /// </summary>
    public partial class FrmAutoPartsNameManager : BaseFormCardList<AutoPartsNameManagerUIModel, AutoPartsNameManagerQCModel, MDLBS_AutoPartsName>
    {
        #region 全局变量

        /// <summary>
        /// 配件名称BLL
        /// </summary>
        private AutoPartsNameManagerBLL _bll = new AutoPartsNameManagerBLL();

        #region 下拉框数据源

        /// <summary>
        /// 配件类别数据源
        /// </summary>
        List<MDLBS_AutoPartsType> _autoPartsTypeList = new List<MDLBS_AutoPartsType>();

        #endregion

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmAutoPartsNameManager构造方法
        /// </summary>
        public FrmAutoPartsNameManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAutoPartsNameManager_Load(object sender, EventArgs e)
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
            TextBoxTool pageSizeOfListTextBox = null;
            foreach (var loopToolControl in this.toolBarPaging.Tools)
            {
                if (loopToolControl.Key == SysConst.EN_PAGESIZE)
                {
                    pageSizeOfListTextBox = (TextBoxTool)loopToolControl;
                }
            }
            if (pageSizeOfListTextBox != null)
            {
                pageSizeOfListTextBox.Text = PageSize.ToString();
                pageSizeOfListTextBox.AfterToolExitEditMode += PageSizeTextBoxTool_AfterToolExitEditMode;
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
        /// 【列表】Grid的Cell的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            //将Grid数据赋值给【详情】Tab内的对应控件
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

        /// <summary>
        /// 选中的Tab改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlFull_SelectedTabChanging(object sender, SelectedTabChangingEventArgs e)
        {
            SetActionEnableBySelectedTab(e.Tab.Key);
        }

        /// <summary>
        /// 固定计量单位改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckAPN_FixUOM_CheckedChanged(object sender, EventArgs e)
        {
            if (ckAPN_FixUOM.Checked)
            {
                lblAPN_UOM.Visible = true;
                lblAPN_UOMX.Visible = true;
                txtAPN_UOM.Visible = true;
            }
            else
            {
                lblAPN_UOM.Visible = false;
                lblAPN_UOMX.Visible = false;
                txtAPN_UOM.Visible = false;
            }
            txtAPN_UOM.Clear();
        }

        #region 详情相关事件

        /// <summary>
        /// 【详情】配件名称失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAPN_Name_Leave(object sender, EventArgs e)
        {
            txtAPN_NameSpellCode.Text = ChineseSpellCode.GetShortSpellCode(txtAPN_Name.Text.Trim());
        }

        /// <summary>
        /// 【详情】配件别名失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAPN_Alias_Leave(object sender, EventArgs e)
        {
            txtAPN_AliasSpellCode.Text = ChineseSpellCode.GetShortSpellCode(txtAPN_Alias.Text.Trim());
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
                DialogResult dialogResult = MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.W_0001), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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
            //1.前端检查
            if (!ClientCheckForSave())
            {
                return;
            }
            //2.将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();
            //3.执行保存（含服务端检查）
            if (!_bll.SaveDetailDS(DetailDS))
            {
                MessageBoxs.Show(Trans.BS, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            //1.设置查询条件（翻页相关属性不用设置）
            ConditionDS = new AutoPartsNameManagerQCModel()
            {
                SqlId = SQLID.BS_AutoPartsNameManager_SQL02,
                //配件名称
                WHERE_APN_Name = txtWhere_APN_Name.Text.Trim(),
                //配件类别
                WHERE_APN_APT_ID = mcbWhere_APT_Name.SelectedValue,
                //有效
                WHERE_APN_IsValid = ckWhere_APN_IsValid.Checked,
            };
            //2.执行基类方法
            base.QueryAction();
            //3.Grid绑定数据源
            gdGrid.DataSource = GridDS;
            gdGrid.DataBind();
            //4.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
            //5.设置Grid自适应列宽（根据单元格内容）
            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdGrid.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 删除
        /// </summary>
        public override void DeleteAction()
        {
            #region 验证删除
            //待删除的配件名称列表
            List<MDLBS_AutoPartsName> deleteAutoPartsNameList = new List<MDLBS_AutoPartsName>();
            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 详情删除
                if (string.IsNullOrEmpty(txtAPN_ID.Text))
                {
                    //配件名称信息为空，不能删除
                    MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.W_0016, new object[] { SystemTableColumnEnums.BS_AutoPartsName.Name.APN_Name, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //验证配件名称是否被引用过
                List<MDLBS_AutoPartsArchive> usedAutoPartsNameList = new List<MDLBS_AutoPartsArchive>();
                _bll.QueryForList(SQLID.BS_AutoPartsNameManager_SQL03, txtAPN_Name.Text + SysConst.Semicolon_DBC, usedAutoPartsNameList);
                if (usedAutoPartsNameList.Count > 0)
                {
                    //配件名称已经被使用，不能删除
                    MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0007, new object[] { txtAPN_Name.Text, MsgParam.APPLY, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //确认删除
                DialogResult dialogResult = MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.W_0012), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                MDLBS_AutoPartsName deleteAutoPartsName = new MDLBS_AutoPartsName()
                {
                    WHERE_APN_ID = txtAPN_ID.Text.Trim(),
                };
                deleteAutoPartsNameList.Add(deleteAutoPartsName);
                #endregion
            }
            else
            {
                #region 列表删除
                gdGrid.UpdateData();
                var checkedAutoPartsNameList = GridDS.Where(p => p.IsChecked == true).ToList();
                if (checkedAutoPartsNameList.Count == 0)
                {
                    //请勾选至少一条配件名称信息进行删除
                    MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { SystemTableColumnEnums.BS_AutoPartsName.Name.APN_Name, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string autoPartsNameStr = string.Empty;
                foreach (var loopCheckedAutoPartsName in checkedAutoPartsNameList)
                {
                    if (string.IsNullOrEmpty(loopCheckedAutoPartsName.APN_Name))
                    {
                        continue;
                    }
                    autoPartsNameStr += loopCheckedAutoPartsName.APN_Name + SysConst.Semicolon_DBC;
                }
                //验证配件名称是否被引用过
                List<MDLBS_AutoPartsArchive> usedAutoPartsNameList = new List<MDLBS_AutoPartsArchive>();
                _bll.QueryForList(SQLID.BS_AutoPartsNameManager_SQL03, autoPartsNameStr, usedAutoPartsNameList);
                if (usedAutoPartsNameList.Count > 0)
                {
                    string usedAutoPartsNameStr = string.Empty;
                    foreach (var loopName in usedAutoPartsNameList)
                    {
                        if (string.IsNullOrEmpty(loopName.APA_Name))
                        {
                            continue;
                        }
                        usedAutoPartsNameStr += loopName.APA_Name + SysConst.Comma_DBC;
                    }
                    //配件名称已经被使用，不能删除
                    MessageBoxs.Show(Trans.BS, this.ToString(),
                        MsgHelp.GetMsg(MsgCode.W_0007,
                            new object[] { usedAutoPartsNameStr, MsgParam.APPLY, SystemActionEnum.Name.DELETE }),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //已选checkedAutoPartsNameList.Count 条数据，确定删除？\r\n单击【确定】删除，【取消】返回。
                DialogResult dialogResult = MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.W_0013, new object[] { checkedAutoPartsNameList.Count }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                _bll.CopyModelList(checkedAutoPartsNameList, deleteAutoPartsNameList);
                foreach (var loopDeleteAutoPartsName in deleteAutoPartsNameList)
                {
                    loopDeleteAutoPartsName.WHERE_APN_ID = loopDeleteAutoPartsName.APN_ID;
                }
                #endregion
            }

            #endregion

            #region 删除数据
            if (deleteAutoPartsNameList.Count > 0)
            {
                var deleteAutoPartsNameResult = _bll.DeleteAutoPartsName(deleteAutoPartsNameList);
                if (!deleteAutoPartsNameResult)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.BS, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //删除成功
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            #endregion

            //3.清空【详情】画面数据
            InitializeDetailTabControls();
            //刷新列表
            RefreshList(true);

            //将最新的值Copy到初始UIModel
            SetCardCtrlsToDetailDS();
            this.AcceptUIModelChanges();
        }

        /// <summary>
        /// 导出当前页
        /// </summary>
        public override void ExportAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = SystemTableEnums.Name.BS_AutoPartsName;
            base.ExportAction(gdGrid, paramGridName);
        }

        /// <summary>
        /// 导出全部
        /// </summary>
        public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = SystemTableEnums.Name.BS_AutoPartsName;
            List<AutoPartsNameManagerUIModel> resultAllList = new List<AutoPartsNameManagerUIModel>();
            _bll.QueryForList<MDLBS_AutoPartsName, AutoPartsNameManagerUIModel>(new AutoPartsNameManagerQCModel()
            {
                PageIndex = 1,
                PageSize = null,
                //配件名称
                WHERE_APN_Name = txtWhere_APN_Name.Text.Trim(),
                //配件别名
                WHERE_APN_APT_ID = mcbWhere_APT_Name.SelectedValue,
                //有效
                WHERE_APN_IsValid = ckWhere_APN_IsValid.Checked,
            }, resultAllList);
            UltraGrid allGrid = gdGrid;
            allGrid.DataSource = resultAllList;
            allGrid.DataBind();

            base.ExportAllAction(allGrid, paramGridName);

            gdGrid.DataSource = GridDS;
            gdGrid.DataBind();
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
            //配置名称ID
            txtAPN_ID.Clear();
            //配件名称
            txtAPN_Name.Clear();
            //配件别名
            txtAPN_Alias.Clear();
            //名称拼音简写
            txtAPN_NameSpellCode.Clear();
            //别名拼音简写
            txtAPN_AliasSpellCode.Clear();
            //配件类别ID
            mcbAPT_Name.Clear();
            //呆滞天数
            txtAPN_SlackDays.Clear();
            //计量单位
            txtAPN_UOM.Clear();
            //固定计量单位
            ckAPN_FixUOM.Checked = true;
            ckAPN_FixUOM.CheckState = CheckState.Checked;
            //有效
            ckAPN_IsValid.Checked = true;
            ckAPN_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtAPN_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtAPN_CreatedTime.Value = BLLCom.GetCurStdDatetime();
            //修改人
            txtAPN_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtAPN_UpdatedTime.Value = BLLCom.GetCurStdDatetime();
            //版本号
            txtAPN_VersionNo.Clear();
            //给 配置名称 设置焦点
            lblAPN_Name.Focus();
            #endregion

            #region 初始化下拉框
            //配件类别
            _autoPartsTypeList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsType) as List<MDLBS_AutoPartsType>;
            mcbAPT_Name.DisplayMember = SystemTableColumnEnums.BS_AutoPartsType.Code.APT_Name;
            mcbAPT_Name.ValueMember = SystemTableColumnEnums.BS_AutoPartsType.Code.APT_ID;
            mcbAPT_Name.DataSource = _autoPartsTypeList;

            #endregion

            base.OrginalUIModel = null;
        }

        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //配件名称
            txtWhere_APN_Name.Clear();
            //配件类别
            mcbWhere_APT_Name.Clear();
            //有效
            ckWhere_APN_IsValid.Checked = true;
            ckWhere_APN_IsValid.CheckState = CheckState.Checked;
            //给 配件名称 设置焦点
            lblWhere_APN_Name.Focus();
            #endregion

            //清空grid数据
            GridDS = new BindingList<AutoPartsNameManagerUIModel>();
            gdGrid.DataSource = GridDS;
            gdGrid.DataBind();

            #endregion

            #region 初始化下拉框
            //配件类别
            mcbWhere_APT_Name.DisplayMember = SystemTableColumnEnums.BS_AutoPartsType.Code.APT_Name;
            mcbWhere_APT_Name.ValueMember = SystemTableColumnEnums.BS_AutoPartsType.Code.APT_ID;
            mcbWhere_APT_Name.DataSource = _autoPartsTypeList;

            #endregion
        }

        /// <summary>
        /// 将Grid数据赋值给【详情】Tab内的对应控件
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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.BS_AutoPartsName.Code.APN_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.BS_AutoPartsName.Code.APN_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            DetailDS = GridDS.FirstOrDefault(x => x.APN_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.BS_AutoPartsName.Code.APN_ID].Value);
            if (DetailDS == null || string.IsNullOrEmpty(DetailDS.APN_ID))
            {
                return;
            }

            if (txtAPN_ID.Text != DetailDS.APN_ID
                || (txtAPN_ID.Text == DetailDS.APN_ID && txtAPN_VersionNo.Text != DetailDS.APN_VersionNo?.ToString()))
            {
                if (txtAPN_ID.Text == DetailDS.APN_ID && txtAPN_VersionNo.Text != DetailDS.APN_VersionNo?.ToString())
                {
                    //数据版本已过期，将加载最新详情。
                    MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.DataHasOverdue }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else if (ViewHasChanged())
                {
                    //将放弃之前的修改，是否继续？
                    DialogResult dialogResult = MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.ConfirmGiveUpEdit }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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
            //配置名称ID
            txtAPN_ID.Text = DetailDS.APN_ID;
            //配件名称
            txtAPN_Name.Text = DetailDS.APN_Name;
            //配件别名
            txtAPN_Alias.Text = DetailDS.APN_Alias;
            //名称拼音简写
            txtAPN_NameSpellCode.Text = DetailDS.APN_NameSpellCode;
            //别名拼音简写
            txtAPN_AliasSpellCode.Text = DetailDS.APN_AliasSpellCode;
            //配件类别
            mcbAPT_Name.SelectedValue = DetailDS.APN_APT_ID;
            //呆滞天数
            txtAPN_SlackDays.Text = DetailDS.APN_SlackDays?.ToString() ?? string.Empty;
            //计量单位
            txtAPN_UOM.Text = DetailDS.APN_UOM;
            //固定计量单位
            if (DetailDS.APN_FixUOM != null)
            {
                ckAPN_FixUOM.Checked = DetailDS.APN_FixUOM.Value;
                ckAPN_FixUOM.CheckState = ckAPN_FixUOM.Checked ? CheckState.Checked : CheckState.Unchecked;
            }
            else
            {
                ckAPN_FixUOM.CheckState = CheckState.Indeterminate;
            }
            //有效
            if (DetailDS.APN_IsValid != null)
            {
                ckAPN_IsValid.Checked = DetailDS.APN_IsValid.Value;
            }
            //创建人
            txtAPN_CreatedBy.Text = DetailDS.APN_CreatedBy;
            //创建时间
            dtAPN_CreatedTime.Value = DetailDS.APN_CreatedTime;
            //修改人
            txtAPN_UpdatedBy.Text = DetailDS.APN_UpdatedBy;
            //修改时间
            dtAPN_UpdatedTime.Value = DetailDS.APN_UpdatedTime;
            //版本号
            txtAPN_VersionNo.Text = DetailDS.APN_VersionNo?.ToString() ?? string.Empty;
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
            #region 前段检查-不涉及数据表
            //检查配件名称是否为空
            if (string.IsNullOrEmpty(txtAPN_Name.Text.Trim()))
            {
                //“配件名称”不能为空
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.BS_AutoPartsName.Name.APN_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAPN_Name.Focus();
                return false;
            }
            //检查呆滞天数
            if (!string.IsNullOrEmpty(txtAPN_SlackDays.Text.Trim()))
            {
                int slackDays;
                if (int.TryParse(txtAPN_SlackDays.Text.Trim(), out slackDays) == false)
                {
                    //“呆滞天数”必须为整数
                    MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0028, new object[] { SystemTableColumnEnums.BS_AutoPartsName.Name.APN_SlackDays, MsgParam.POSITIVE_INTEGER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAPN_SlackDays.Focus();
                    return false;
                }
                else if (slackDays < 0)
                {
                    //“呆滞天数”必须为整数
                    MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0028, new object[] { SystemTableColumnEnums.BS_AutoPartsName.Name.APN_SlackDays, MsgParam.POSITIVE_INTEGER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAPN_SlackDays.Focus();
                    return false;
                }
                else if (txtAPN_SlackDays.Text.Length > 4)
                {
                    //“呆滞天数”数字长度不能超过4个
                    MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0004, new object[] { SystemTableColumnEnums.BS_AutoPartsName.Name.APN_SlackDays, MsgParam.TEN_THOUSAND }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAPN_SlackDays.Focus();
                    return false;
                }

            }
            //固定计量单位的场合，检查计量单位
            if (ckAPN_FixUOM.Checked)
            {
                if (string.IsNullOrEmpty(txtAPN_UOM.Text.Trim()))
                {
                    MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.BS_AutoPartsName.Name.APN_UOM }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAPN_UOM.Focus();
                    return false;
                }
            }

            #endregion

            #region 前端检查-涉及到数据表
            //检查配件名称是否存在
            var userdCount = _bll.QueryForObject<int>(SQLID.BS_AutoPartsNameManager_SQL01, new MDLBS_AutoPartsName
            {
                WHERE_APN_ID = txtAPN_ID.Text.Trim(),
                WHERE_APN_Name = txtAPN_Name.Text.Trim(),
            });
            if (userdCount > 0)
            {
                //配件名称已存在
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0006, new object[] { SystemTableColumnEnums.BS_AutoPartsName.Name.APN_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            #endregion
            return true;
        }

        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            DetailDS = new AutoPartsNameManagerUIModel()
            {
                //配置名称ID
                APN_ID = txtAPN_ID.Text.Trim(),
                //配件名称
                APN_Name = txtAPN_Name.Text.Trim(),
                //配件别名
                APN_Alias = txtAPN_Alias.Text.Trim(),
                //名称拼音简写
                APN_NameSpellCode = txtAPN_NameSpellCode.Text.Trim(),
                //别名拼音简写
                APN_AliasSpellCode = txtAPN_AliasSpellCode.Text.Trim(),
                //配件类别ID
                APN_APT_ID = mcbAPT_Name.SelectedValue,
                //配件类别名称
                APT_Name = mcbAPT_Name.SelectedText,
                //计量单位
                APN_UOM = txtAPN_UOM.Text.Trim(),
                //有效
                APN_IsValid = ckAPN_IsValid.Checked,
                //创建人
                APN_CreatedBy = txtAPN_CreatedBy.Text.Trim(),
                //创建时间
                APN_CreatedTime = (DateTime?)dtAPN_CreatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //修改人
                APN_UpdatedBy = LoginInfoDAX.UserName,
                //修改时间
                APN_UpdatedTime = (DateTime?)dtAPN_UpdatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //版本号
                APN_VersionNo = Convert.ToInt64(txtAPN_VersionNo.Text.Trim() == "" ? "1" : txtAPN_VersionNo.Text.Trim()),
            };

            //固定计量单位
            if (ckAPN_FixUOM.CheckState == CheckState.Indeterminate)
            {
                DetailDS.APN_FixUOM = null;
            }
            else
            {
                DetailDS.APN_FixUOM = ckAPN_FixUOM.Checked;
            }

            //呆滞天数
            if (!string.IsNullOrEmpty(txtAPN_SlackDays.Text.Trim()))
            {
                DetailDS.APN_SlackDays = Convert.ToInt32(txtAPN_SlackDays.Text.Trim());
            }
            else
            {
                DetailDS.APN_SlackDays = null;
            }
        }

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
                    var curHead = GridDS.FirstOrDefault(x => x.APN_ID == DetailDS.APN_ID);
                    if (curHead != null)
                    {
                        GridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = GridDS.FirstOrDefault(x => x.APN_ID == DetailDS.APN_ID);
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
