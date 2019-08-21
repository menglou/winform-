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
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;

namespace SkyCar.Coeus.UI.BS
{
    /// <summary>
    /// 码表管理
    /// </summary>
    public partial class FrmCodeTableManager : BaseFormCardList<CodeTableManagerUIModel, CodeTableManagerQCModel, MDLSM_CodeTable>
    {
        #region 全局变量

        /// <summary>
        /// 码表管理BLL
        /// </summary>
        private CodeTableManagerBLL _bll = new CodeTableManagerBLL();
        /// <summary>
        /// 参数类型数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _codeTypeDS = new List<ComComboBoxDataSourceTC>();

        /// <summary>
        /// 界面属性值发生变化时不予检查的属性列表
        /// </summary>
        List<string> _skipPropertyList = new List<string>();

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmCodeTableManager构造方法
        /// </summary>
        public FrmCodeTableManager()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCodeTableManager_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（动作）
            base.ToolBarActionAndNavigate = toolBarActionAndNavigate;
            //基类.工具栏（翻页）
            base.ToolBarPaging = toolBarPaging;
            //查询委托（基类控制翻页用）
            base.ExecuteQuery = QueryAction;
            //工具栏（动作）单击事件
            this.toolBarActionAndNavigate.ToolClick += new ToolClickEventHandler(base.toolBarActionAndNavigate_ToolClick);
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

            #region 界面发生变化时不予检查的属性值

            _skipPropertyList.Add("Enum_DisplayName");
            #endregion
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
            if (ViewHasChanged(_skipPropertyList))
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

            //将最新的值Copy到初始UIModel
            SetCardCtrlsToDetailDS();
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
            bool saveResult = _bll.SaveDetailDS(DetailDS);
            if (!saveResult)
            {
                MessageBoxs.Show(Trans.BS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            txtCT_ID.Clear();
            //版本号
            txtCT_VersionNo.Clear();
        }
        /// <summary>
        /// 删除
        /// </summary>
        public override void DeleteAction()
        {
            //2.执行删除
            #region 准备数据

            //待删除的码表列表
            List<MDLSM_CodeTable> deleteCodeTableList = new List<MDLSM_CodeTable>();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 详情删除
                if (string.IsNullOrEmpty(txtCT_ID.Text))
                {
                    //码表信息为空，不能删除
                    MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0016, new object[] { SystemTableEnums.Name.SM_CodeTable, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //查询[码表]是否被引用过
                var usedCount = 0;
                _bll.QueryForObject<Int32>(SQLID.BS_CodeTableManager_SQL03, new MDLSM_CodeTable
                {
                    WHERE_CT_Type = cbCT_Type.Text.Trim(),
                    WHERE_CT_Name = txtCT_Name.Text.Trim()
                });
                if (usedCount > 0)
                {
                    MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0003, new object[] { SystemTableEnums.Name.SM_CodeTable }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //确认删除操作
                DialogResult dialogResult = MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0012), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                //待删除的码表
                MDLSM_CodeTable deleteCodeTable = new MDLSM_CodeTable
                {
                    WHERE_CT_ID = txtCT_ID.Text.Trim()
                };
                deleteCodeTableList.Add(deleteCodeTable);
                #endregion
            }
            else
            {
                #region 列表删除
                gdGrid.UpdateData();
                //勾选的码表列表
                List<CodeTableManagerUIModel> checkedCodeTableList = GridDS.Where(x => x.IsChecked == true).ToList();
                if (checkedCodeTableList.Count == 0)
                {
                    MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { SystemTableEnums.Name.SM_CodeTable, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                #region 查询码表是否被引用过
                //勾选的[码表]列表中已被引用的[码表]列表
                List<CodeTableManagerUIModel> checkedUsedList = new List<CodeTableManagerUIModel>();

                foreach (var loopCodeTable in checkedCodeTableList)
                {
                    var resultUsedCount = _bll.QueryForObject<Int32>(SQLID.BS_CodeTableManager_SQL03, new MDLSM_CodeTable
                    {
                        WHERE_CT_Type = loopCodeTable.CT_Type,
                        WHERE_CT_Name = loopCodeTable.CT_Name
                    });
                    if (resultUsedCount > 0)
                    {
                        loopCodeTable.IsChecked = false;
                        checkedUsedList.Add(loopCodeTable);
                    }
                }

                if (checkedUsedList.Count > 0)
                {
                    //勾选的列表中包含已被引用的[码表]，是否忽略
                    DialogResult continueDelete = MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0026, new object[] { }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (continueDelete != DialogResult.OK)
                    {
                        foreach (var loopCheckedUsed in checkedUsedList)
                        {
                            loopCheckedUsed.IsChecked = false;
                        }
                        gdGrid.DataSource = GridDS;
                        gdGrid.DataBind();
                        return;
                    }
                    checkedCodeTableList.RemoveAll(x => x.IsChecked == false);

                    gdGrid.DataSource = GridDS;
                    gdGrid.DataBind();
                }
                else
                {
                    //勾选的列表中不包含已被引用的[码表]
                    //确认删除操作
                    DialogResult confirmDeleteResult = MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0013, new object[] { checkedCodeTableList.Count }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (confirmDeleteResult != DialogResult.OK)
                    {
                        return;
                    }
                }
                #endregion

                foreach (var loopCheckedCodeTable in checkedCodeTableList)
                {
                    if (string.IsNullOrEmpty(loopCheckedCodeTable.CT_ID))
                    {
                        continue;
                    }

                    //待删除的码表
                    MDLSM_CodeTable deleteCodeTable = new MDLSM_CodeTable
                    {
                        WHERE_CT_ID = loopCheckedCodeTable.CT_ID
                    };
                    deleteCodeTableList.Add(deleteCodeTable);
                }
                #endregion
            }

            #endregion

            #region 删除数据
            if (deleteCodeTableList.Count > 0)
            {
                var deleteCodeTableResult = _bll.DeleteCodeTable(deleteCodeTableList);
                if (!deleteCodeTableResult)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.BS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //删除成功
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            //1.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
            //2.设置查询条件（翻页相关属性不用设置）
            base.ConditionDS = new CodeTableManagerQCModel()
            {
                //类型
                WHERE_CT_Type = cbWhere_CT_Type.Value?.ToString() ?? "",
                //参数名称
                WHERE_CT_Name = txtWhere_CT_Name.Text.Trim(),
                //有效
                WHERE_CT_IsValid = ckWhere_CT_IsValid.Checked,
                //SQLID
                SqlId = SQLID.BS_CodeTableManager_SQL01
            };
            //3.执行基类方法（注意：基类使用模糊查询）
            base.QueryAction();
            //4.Grid绑定数据源
            gdGrid.DataSource = base.GridDS;
            gdGrid.DataBind();
            //5.设置Grid自适应列宽（根据单元格内容）
            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdGrid.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
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
            if (ViewHasChanged(_skipPropertyList))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 导出当前页
        /// </summary>
        public override void ExportAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = SystemTableEnums.Name.SM_CodeTable;
            base.ExportAction(gdGrid, paramGridName);
        }
        /// <summary>
        /// 导出全部
        /// </summary>
        public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = SystemTableEnums.Name.SM_CodeTable;
            List<CodeTableManagerUIModel> resultAllList = new List<CodeTableManagerUIModel>();
            _bll.QueryForList(SQLID.BS_CodeTableManager_SQL01, new CodeTableManagerQCModel()
            {
                PageIndex = 1,
                PageSize = null,
                //类型
                WHERE_CT_Type = cbWhere_CT_Type.Value?.ToString() ?? "",
                //参数名称
                WHERE_CT_Name = txtWhere_CT_Name.Text.Trim(),
                //有效
                WHERE_CT_IsValid = ckWhere_CT_IsValid.Checked,
            }, resultAllList);
            UltraGrid allGrid = gdGrid;
            allGrid.DataSource = resultAllList;
            allGrid.DataBind();

            base.ExportAllAction(allGrid, paramGridName);

            gdGrid.DataSource = GridDS;
            gdGrid.DataBind();
        }
        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 工具生成
            //类型
            cbCT_Type.Items.Clear();
            //参数名称
            txtCT_Name.Clear();
            //参数值
            txtCT_Value.Clear();
            //参数描述
            txtCT_Desc.Clear();
            //有效
            ckCT_IsValid.Checked = true;
            ckCT_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtCT_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtCT_CreatedTime.Value = BLLCom.GetCurStdDatetime();
            //修改人
            txtCT_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtCT_UpdatedTime.Value = BLLCom.GetCurStdDatetime();
            //ID
            txtCT_ID.Clear();
            //版本号
            txtCT_VersionNo.Clear();
            //给 类型 设置焦点
            lblCT_Type.Focus();
            #endregion

            #region 初始化参数类型
            _codeTypeDS = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.CodeType);
            cbCT_Type.DisplayMember = SysConst.EN_TEXT;
            cbCT_Type.ValueMember = SysConst.EN_Code;
            cbCT_Type.DataSource = _codeTypeDS;
            cbCT_Type.DataBind();
            #endregion
        }
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //类型
            cbWhere_CT_Type.Items.Clear();
            //参数值
            txtWhere_CT_Name.Clear();
            //有效
            ckWhere_CT_IsValid.Checked = true;
            ckWhere_CT_IsValid.CheckState = System.Windows.Forms.CheckState.Checked;
            //给 类型 设置焦点
            lblWhere_CT_Type.Focus();
            #endregion

            //清空Grid
            GridDS = new BindingList<CodeTableManagerUIModel>();
            gdGrid.DataSource = base.GridDS;
            gdGrid.DataBind();

            base.ClearAction();

            #endregion

            #region 初始化参数类型
            cbWhere_CT_Type.DisplayMember = SysConst.EN_TEXT;
            cbWhere_CT_Type.ValueMember = SysConst.EN_Code;
            cbWhere_CT_Type.DataSource = _codeTypeDS;
            cbWhere_CT_Type.DataBind();
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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SM_CodeTable.Code.CT_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SM_CodeTable.Code.CT_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            DetailDS = base.GridDS.FirstOrDefault(x => x.CT_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SM_CodeTable.Code.CT_ID].Value);
            if (DetailDS == null || string.IsNullOrEmpty(DetailDS.CT_ID))
            {
                return;
            }

            if (txtCT_ID.Text != DetailDS.CT_ID
                || (txtCT_ID.Text == DetailDS.CT_ID && txtCT_VersionNo.Text != DetailDS.CT_VersionNo?.ToString()))
            {
                if (txtCT_ID.Text == DetailDS.CT_ID && txtCT_VersionNo.Text != DetailDS.CT_VersionNo?.ToString())
                {
                    //数据版本已过期，将加载最新详情。
                    MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.DataHasOverdue }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else if (ViewHasChanged(_skipPropertyList))
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
            //类型
            cbCT_Type.Text = DetailDS.CT_Type ?? string.Empty;
            //参数名称
            txtCT_Name.Text = DetailDS.CT_Name;
            //参数值
            txtCT_Value.Text = DetailDS.CT_Value;
            //参数描述
            txtCT_Desc.Text = DetailDS.CT_Desc;
            //有效
            if (DetailDS.CT_IsValid != null)
            {
                ckCT_IsValid.Checked = DetailDS.CT_IsValid.Value;
            }
            //创建人
            txtCT_CreatedBy.Text = DetailDS.CT_CreatedBy;
            //创建时间
            dtCT_CreatedTime.Value = DetailDS.CT_CreatedTime;
            //修改人
            txtCT_UpdatedBy.Text = DetailDS.CT_UpdatedBy;
            //修改时间
            dtCT_UpdatedTime.Value = DetailDS.CT_UpdatedTime;
            //ID
            txtCT_ID.Text = DetailDS.CT_ID;
            //版本号
            txtCT_VersionNo.Text = DetailDS.CT_VersionNo?.ToString() ?? string.Empty;
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
            //验证参数类型
            if (string.IsNullOrEmpty(cbCT_Type.Text.Trim()))
            {
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.SM_CodeTable.Name.CT_Type }), MessageBoxButtons.OK, MessageBoxIcon.Information );
                cbCT_Type.Focus();
                return false;
            }

            //验证参数名称
            if (string.IsNullOrEmpty(txtCT_Name.Text.Trim()))
            {
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.SM_CodeTable.Name.CT_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information );
                txtCT_Name.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            DetailDS = new CodeTableManagerUIModel()
            {
                //类型
                CT_Type = cbCT_Type.Value?.ToString() ?? "",
                //参数名称
                CT_Name = txtCT_Name.Text.Trim(),
                //参数值
                CT_Value = txtCT_Value.Text.Trim(),
                //参数描述
                CT_Desc = txtCT_Desc.Text.Trim(),
                //有效
                CT_IsValid = ckCT_IsValid.Checked,
                //创建人
                CT_CreatedBy = txtCT_CreatedBy.Text.Trim(),
                //创建时间
                CT_CreatedTime = (DateTime?)dtCT_CreatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //修改人
                CT_UpdatedBy = txtCT_UpdatedBy.Text.Trim(),
                //修改时间
                CT_UpdatedTime = (DateTime?)dtCT_UpdatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //ID
                CT_ID = txtCT_ID.Text.Trim(),
                //版本号
                CT_VersionNo = Convert.ToInt64(txtCT_VersionNo.Text.Trim() == "" ? "1" : txtCT_VersionNo.Text.Trim()),
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
                    var curHead = GridDS.FirstOrDefault(x => x.CT_ID == DetailDS.CT_ID);
                    if (curHead != null)
                    {
                        GridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = GridDS.FirstOrDefault(x => x.CT_ID == DetailDS.CT_ID);
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
