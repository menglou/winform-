using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.BS;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.BS;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;

namespace SkyCar.Coeus.UI.BS
{
    /// <summary>
    /// 配件类别
    /// </summary>
    public partial class FrmAutoPartsTypeManager : BaseFormCardList<AutoPartsTypeManagerUIModel, AutoPartsTypeManagerQCModel, MDLBS_AutoPartsType>
    {
        #region 全局变量
        /// <summary>
        /// 配件类别BLL
        /// </summary>
        private AutoPartsTypeManagerBLL _bll = new AutoPartsTypeManagerBLL();

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
        /// FrmAutoPartsTypeManager构造方法
        /// </summary>
        public FrmAutoPartsTypeManager()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAutoPartsTypeManager_Load(object sender, EventArgs e)
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
            gdGrid.UpdateData();
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
        
        /// <summary>
        /// 类别名称失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAPT_Name_Leave(object sender, EventArgs e)
        {
            // 产生类别顺序
            if (string.IsNullOrEmpty(txtAPT_Index.Text.Trim()) ||
                string.IsNullOrEmpty(txtAPT_ID.Text))
            {
                //如果时新增需要加1
                int addIndex = string.IsNullOrEmpty(txtAPT_ID.Text) ? 1 : 0;

                int curTypeCount = 0;
                if (!string.IsNullOrEmpty(mcbAPT_ParentName.SelectedText))
                {
                    curTypeCount = _bll.QueryForCount<MDLBS_AutoPartsType>(new MDLBS_AutoPartsType
                    {
                        WHERE_APT_ParentID = mcbAPT_ParentName.SelectedValue,
                        WHERE_APT_IsValid = true
                    });
                }
                else
                {
                    curTypeCount = _bll.QueryForObject<int>(SQLID.BS_AutoPartsTypeManager_SQL03, 0);
                }
                txtAPT_Index.Text = (curTypeCount + addIndex).ToString();
            }
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
            //1.前端检查-保存
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
            MessageBoxs.Show(Trans.SM, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //4.将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();
        }
        /// <summary>
        /// 复制
        /// </summary>
        public override void CopyAction()
        {
            base.CopyAction();
            //ID
            txtAPT_ID.Clear();
            //APT_Name
            txtAPT_Name.Clear();
            //版本号
            txtAPT_VersionNo.Clear();
        }
        /// <summary>
        /// 删除
        /// </summary>
        public override void DeleteAction()
        {
            gdGrid.UpdateData();

            #region 准备数据

            //待删除的配件类别列表
            List<MDLBS_AutoPartsType> deleteAutoPartsTypeList = new List<MDLBS_AutoPartsType>();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 详情删除
                if (string.IsNullOrEmpty(txtAPT_ID.Text))
                {
                    //配件类别信息为空，不能删除
                    MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.W_0016, new object[] { SystemTableColumnEnums.BS_AutoPartsType.Name.APT_Name, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //验证配件类别是否被引用过
                List<MDLBS_AutoPartsType> usedAutoPartsTypeList = new List<MDLBS_AutoPartsType>();
                _bll.QueryForList(SQLID.BS_AutoPartsTypeManager_SQL04, txtAPT_ID.Text + SysConst.Semicolon_DBC, usedAutoPartsTypeList);
                if (usedAutoPartsTypeList.Count > 0)
                {
                    //配件类别已经被使用，不能删除
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0007, new object[] { txtAPT_Name.Text, MsgParam.APPLY, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //确认删除
                DialogResult dialogResult = MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.W_0012), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                MDLBS_AutoPartsType deleteAutoPartsType = new MDLBS_AutoPartsType()
                {
                    WHERE_APT_ID = txtAPT_ID.Text.Trim()
                };
                deleteAutoPartsTypeList.Add(deleteAutoPartsType);
                #endregion
            }
            else
            {
                #region 列表删除

                gdGrid.UpdateData();
                var checkedAutoPartsTypeList = base.GridDS.Where(p => p.IsChecked == true).ToList();
                if (checkedAutoPartsTypeList.Count == 0)
                {
                    //请勾选至少一条配件类别信息进行删除
                    MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { SystemTableColumnEnums.BS_AutoPartsType.Name.APT_Name, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string autoPartsTypeIdStr = string.Empty;
                foreach (var loopCheckedAutoPartsName in checkedAutoPartsTypeList)
                {
                    if (string.IsNullOrEmpty(loopCheckedAutoPartsName.APT_ID))
                    {
                        continue;
                    }
                    autoPartsTypeIdStr += loopCheckedAutoPartsName.APT_ID + SysConst.Semicolon_DBC;
                }
                //验证配件类别是否被引用过
                List<MDLBS_AutoPartsType> usedAutoPartsTypeList = new List<MDLBS_AutoPartsType>();
                _bll.QueryForList(SQLID.BS_AutoPartsTypeManager_SQL04, autoPartsTypeIdStr, usedAutoPartsTypeList);
                if (usedAutoPartsTypeList.Count > 0)
                {
                    string usedAutoPartsNameStr = string.Empty;
                    foreach (var loopName in usedAutoPartsTypeList)
                    {
                        if (string.IsNullOrEmpty(loopName.APT_Name))
                        {
                            continue;
                        }
                        usedAutoPartsNameStr += loopName.APT_Name + SysConst.Comma_DBC;
                    }
                    //配件类别已经被使用，不能删除
                    MessageBoxs.Show(Trans.PIS, this.ToString(),
                        MsgHelp.GetMsg(MsgCode.W_0007,
                            new object[] {usedAutoPartsNameStr, MsgParam.APPLY, SystemActionEnum.Name.DELETE}),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //已选checkedAutoPartsTypeList.Count 条数据，确定删除？\r\n单击【确定】删除，【取消】返回。
                DialogResult dialogResult = MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.W_0013, new object[] { checkedAutoPartsTypeList.Count }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                _bll.CopyModelList(checkedAutoPartsTypeList, deleteAutoPartsTypeList);
                foreach (var loopSelectedItem in deleteAutoPartsTypeList)
                {
                    loopSelectedItem.WHERE_APT_ID = loopSelectedItem.APT_ID;
                }
                #endregion
            }

            #endregion

            #region 删除数据
            if (deleteAutoPartsTypeList.Count > 0)
            {
                var deleteAutoPartsArchiveResult = _bll.DeleteAutoPartsType(deleteAutoPartsTypeList);
                if (!deleteAutoPartsArchiveResult)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.BS, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            //1.设置查询条件（翻页相关属性不用设置）
            base.ConditionDS = new AutoPartsTypeManagerQCModel()
            {
                SqlId = SQLID.BS_AutoPartsTypeManager_SQL01,
                //配件类别名称
                WHERE_APT_Name = txtWhere_APT_Name.Text.Trim(),
                //父类名称
                WHERE_APT_ParentName = mcbWhere_APT_ParentName.SelectedText,
                //有效
                WHERE_APT_IsValid = ckWhere_APT_IsValid.Checked,
            };
            //2.执行基类方法（注意：基类使用模糊查询）
            base.QueryAction();
            //3.Grid绑定数据源
            gdGrid.DataSource = base.GridDS;
            gdGrid.DataBind();
            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdGrid.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
            //4.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
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
            #region 初始化【详情】Tab内控件
            //配件类别名称
            txtAPT_Name.Clear();
            //父类名称
            mcbAPT_ParentName.Clear();
            //顺序
            txtAPT_Index.Clear();
            //有效
            ckAPT_IsValid.Checked = true;
            ckAPT_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtAPT_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtAPT_CreatedTime.Value = BLLCom.GetCurStdDatetime();
            //修改人
            txtAPT_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtAPT_UpdatedTime.Value = BLLCom.GetCurStdDatetime();
            //版本号
            txtAPT_VersionNo.Clear();
            //配件类别ID
            txtAPT_ID.Clear();
            //给 配件类别名称 设置焦点
            lblAPT_Name.Focus();
            #endregion

            #region 初始化下拉框
            //配件类别
            _autoPartsTypeList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsType) as List<MDLBS_AutoPartsType>;

            //父级
            mcbAPT_ParentName.DisplayMember = SystemTableColumnEnums.BS_AutoPartsType.Code.APT_Name;
            mcbAPT_ParentName.ValueMember = SystemTableColumnEnums.BS_AutoPartsType.Code.APT_ID;
            mcbAPT_ParentName.DataSource = _autoPartsTypeList;

            #endregion
        }
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //配件类别名称
            txtWhere_APT_Name.Clear();
            //父类名称
            mcbWhere_APT_ParentName.Clear();
            //有效
            ckWhere_APT_IsValid.Checked = true;
            ckWhere_APT_IsValid.CheckState = CheckState.Checked;
            //给 配件类别名称 设置焦点
            lblWhere_APT_Name.Focus();
            #endregion

            //清空grid数据
            GridDS = new BindingList<AutoPartsTypeManagerUIModel>();
            gdGrid.DataSource = base.GridDS;
            gdGrid.DataBind();
            base.ClearAction();

            #endregion

            #region 初始化下拉框

            //父级类别名称
            mcbWhere_APT_ParentName.DisplayMember = SystemTableColumnEnums.BS_AutoPartsType.Code.APT_Name;
            mcbWhere_APT_ParentName.ValueMember = SystemTableColumnEnums.BS_AutoPartsType.Code.APT_ID;
            mcbWhere_APT_ParentName.DataSource = _autoPartsTypeList;

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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.BS_AutoPartsType.Code.APT_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.BS_AutoPartsType.Code.APT_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            DetailDS = base.GridDS.FirstOrDefault(x => x.APT_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.BS_AutoPartsType.Code.APT_ID].Value);
            if (DetailDS == null || string.IsNullOrEmpty(DetailDS.APT_ID))
            {
                return;
            }

            if (txtAPT_ID.Text != DetailDS.APT_ID
                || (txtAPT_ID.Text == DetailDS.APT_ID && txtAPT_VersionNo.Text != DetailDS.APT_VersionNo?.ToString()))
            {
                if (txtAPT_ID.Text == DetailDS.APT_ID && txtAPT_VersionNo.Text != DetailDS.APT_VersionNo?.ToString())
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
            //配件类别名称
            txtAPT_Name.Text = DetailDS.APT_Name;
            //父类名称
            mcbAPT_ParentName.SelectedValue = DetailDS.APT_ParentID;
            //顺序
            txtAPT_Index.Text = DetailDS.APT_Index?.ToString();
            //有效
            if (DetailDS.APT_IsValid != null)
            {
                ckAPT_IsValid.Checked = DetailDS.APT_IsValid.Value;
            }
            //创建人
            txtAPT_CreatedBy.Text = DetailDS.APT_CreatedBy;
            //创建时间
            dtAPT_CreatedTime.Value = DetailDS.APT_CreatedTime == null ? "" : DetailDS.APT_CreatedTime.ToString();
            //修改人
            txtAPT_UpdatedBy.Text = DetailDS.APT_UpdatedBy;
            //修改时间
            dtAPT_UpdatedTime.Value = DetailDS.APT_UpdatedTime?.ToString();
            //版本号
            txtAPT_VersionNo.Text = DetailDS.APT_VersionNo?.ToString();
            //配件类别ID
            txtAPT_ID.Text = DetailDS.APT_ID;
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
                foreach (UltraGridColumn loopColumns in gdGrid.DisplayLayout.Bands[0].SortedColumns)
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
            //判断【配件类别名称】是否为空
            if (string.IsNullOrEmpty(txtAPT_Name.Text))
            {
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.BS_AutoPartsType.Name.APT_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAPT_Name.Focus();
                return false;
            }
            //判断【顺序】是否超过超过9个
            if (txtAPT_Index.Text.Length > 9)
            {
                //“顺序”字数不能超过9个
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0029, new object[] { SystemTableColumnEnums.BS_AutoPartsType.Name.APT_Index, MsgParam.NINE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAPT_Index.Focus();
                return false;
            }

            //检查配件类别是否存在
            var userdCount = _bll.QueryForObject<int>(SQLID.BS_AutoPartsTypeManager_SQL02, new MDLBS_AutoPartsType
            {
                WHERE_APT_ID = txtAPT_ID.Text.Trim(),
                WHERE_APT_Name = txtAPT_Name.Text.Trim(),
            });
            if (userdCount > 0)
            {
                //配件名称已存在
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0006, new object[] { SystemTableColumnEnums.BS_AutoPartsName.Name.APN_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information );
                return false;
            }
            return true;
        }
        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            DetailDS = new AutoPartsTypeManagerUIModel()
            {
                //配件类别名称
                APT_Name = txtAPT_Name.Text.Trim(),
                //父类名称
                APT_ParentName = mcbAPT_ParentName.SelectedText,
                //父类ID
                APT_ParentID = mcbAPT_ParentName.SelectedValue,
                //顺序
                APT_Index = Convert.ToInt32(txtAPT_Index.Text.Trim() == "" ? "1" : txtAPT_Index.Text.Trim()),
                //有效
                APT_IsValid = ckAPT_IsValid.Checked,
                //创建人
                APT_CreatedBy = txtAPT_CreatedBy.Text.Trim(),
                //创建时间
                APT_CreatedTime = (DateTime?)dtAPT_CreatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //修改人
                APT_UpdatedBy = LoginInfoDAX.UserName,
                //修改时间
                APT_UpdatedTime = (DateTime?)dtAPT_UpdatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //版本号
                APT_VersionNo = Convert.ToInt64(txtAPT_VersionNo.Text.Trim() == "" ? "1" : txtAPT_VersionNo.Text.Trim()),
                //配件类别ID
                APT_ID = txtAPT_ID.Text.Trim(),
            };
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
                    var curHead = GridDS.FirstOrDefault(x => x.APT_ID == DetailDS.APT_ID);
                    if (curHead != null)
                    {
                        GridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = GridDS.FirstOrDefault(x => x.APT_ID == DetailDS.APT_ID);
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
