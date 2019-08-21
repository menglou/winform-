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
using SkyCar.Coeus.BLL.PIS;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.PIS;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UI.PIS
{
    /// <summary>
    /// 仓库管理
    /// </summary>
    public partial class FrmWarehouseManager : BaseFormCardListDetail<WarehouseManagerUIModel, WarehouseManagerQCModel, MDLPIS_Warehouse>
    {
        #region 全局变量

        /// <summary>
        /// 仓库管理BLL
        /// </summary>
        private WarehouseManagerBLL _bll = new WarehouseManagerBLL();
        /// <summary>
        /// 仓位数据源
        /// </summary>
        private SkyCarBindingList<WarehouseBinManagerUIModel, MDLPIS_WarehouseBin> _warehouseBinList = new SkyCarBindingList<WarehouseBinManagerUIModel, MDLPIS_WarehouseBin>();
        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmWarehouseManager构造方法
        /// </summary>
        public FrmWarehouseManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmWarehouseManager_Load(object sender, EventArgs e)
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
            //根据选中的Tab，设置动作按钮[是否可用]（在系统权限的基础上进行控制）
            base.SetActionEnableBySelectedTab(SysConst.EN_LIST);
            #endregion

            //设置单元格样式
            SetWarehouseStyle();

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

        #region 【列表】Grid相关方法
        
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
        #endregion

        #region 查询条件相关方法

        /// <summary>
        /// 仓库名称KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_WH_Name_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 仓库编号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_WH_No_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 仓库地址KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_WH_Address_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 有效CheckedChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckWhere_WH_IsValid_CheckedChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        #endregion

        #region 仓库相关事件

        /// <summary>
        /// 有效CheckedChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckWH_IsValid_CheckedChanged(object sender, EventArgs e)
        {
            if (!ckWH_IsValid.Checked)
            {
                StringBuilder warehouseIDs = new StringBuilder();
                warehouseIDs.Append(SysConst.Semicolon_DBC + txtWH_ID.Text + SysConst.Semicolon_DBC);
                List<MDLPIS_Warehouse> resultWarehouseList = new List<MDLPIS_Warehouse>();
                _bll.QueryForList(SQLID.PIS_WarehouseManager_SQL01, new MDLPIS_Warehouse
                {
                    WHERE_WH_ID = warehouseIDs.ToString()
                }, resultWarehouseList);

                if (resultWarehouseList.Count > 0)
                {
                    //仓库已经被使用，不能为无效
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0007, new object[]
                    { txtWH_Name.Text, MsgParam.APPLY, MsgParam.BE+SystemTableColumnEnums.PIS_Warehouse.Name.WH_IsValid }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ckWH_IsValid.Checked = true;
                }
            }
        }
        #endregion

        #region 仓位相关事件

        /// <summary>
        /// 仓位toolBar点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolBarWarehouseBinManager_ToolClick(object sender, ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                //添加仓位
                case SysConst.EN_ADD:
                    AddWarehouseBin();
                    break;

                //删除仓位
                case SysConst.EN_DEL:
                    DeleteWarehouseBin();
                    break;
            }
        }

        /// <summary>
        /// 仓位Grid双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGridWarehouseBin_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            UpdateWarehouseBin();
        }

        /// <summary>
        /// 【列表】Grid的CellChange事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGridWarehouseBin_CellChange(object sender, CellEventArgs e)
        {
            gdGridWarehouseBin.UpdateData();
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
            base.NewUIModel = HeadDS;
            if (ViewHasChanged()
                || _warehouseBinList.InsertList.Count > 0
                || _warehouseBinList.UpdateList.Count > 0
                || _warehouseBinList.DeleteList.Count > 0)
            {
                //信息尚未保存，确定进行当前操作？
                DialogResult dialogResult = MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.W_0001), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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

            //设置详情是否可编辑
            SetDetailControl();

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
            gdGridWarehouseBin.UpdateData();

            //3.执行保存（含服务端检查）
            bool saveResult = _bll.SaveDetailDS(HeadDS, _warehouseBinList);
            if (!saveResult)
            {
                //保存失败
                MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //设置详情是否可编辑
            SetDetailControl();

            _warehouseBinList.StartMonitChanges();
            //4.将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();
        }

        /// <summary>
        /// 删除仓库
        /// </summary>
        public override void DeleteAction()
        {
            #region 准备数据

            //待删除的仓库列表
            List<MDLPIS_Warehouse> deleteWarehouseList = new List<MDLPIS_Warehouse>();
            //待删除的仓位列表
            List<MDLPIS_WarehouseBin> deleteWarehouseBinList = new List<MDLPIS_WarehouseBin>();
            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 详情删除

                #region 验证

                if (string.IsNullOrEmpty(txtWH_ID.Text.Trim()))
                {
                    //选择要删除的数据
                    MessageBoxs.Show(Trans.PIS, this.ToString(),
                        MsgHelp.GetMsg(MsgCode.W_0006,
                            new object[] { SystemTableEnums.Name.PIS_Warehouse, SystemActionEnum.Name.DELETE }),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //验证仓库是否已被使用
                StringBuilder warehouseIDs = new StringBuilder();
                warehouseIDs.Append(SysConst.Semicolon_DBC + txtWH_ID.Text + SysConst.Semicolon_DBC);
                //已被使用的仓库
                List<MDLPIS_Warehouse> usedWarehouseList = new List<MDLPIS_Warehouse>();
                _bll.QueryForList(SQLID.PIS_WarehouseManager_SQL01, new MDLPIS_Warehouse
                {
                    WHERE_WH_ID = warehouseIDs.ToString()
                }, usedWarehouseList);
                if (usedWarehouseList.Count > 0)
                {
                    //仓库已经被使用，不能删除
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0007, new object[]
                    { txtWH_Name.Text, MsgParam.APPLY, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //确认删除操作
                DialogResult dialogResult = MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0012), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                #endregion

                MDLPIS_Warehouse deleteWarehouse = new MDLPIS_Warehouse()
                {
                    WHERE_WH_ID = txtWH_ID.Text.Trim()
                };
                deleteWarehouseList.Add(deleteWarehouse);
                foreach (var loopWarehouseBin in _warehouseBinList)
                {
                    MDLPIS_WarehouseBin deleteWarehouseBin = new MDLPIS_WarehouseBin()
                    {
                        WHERE_WHB_ID = loopWarehouseBin.WHB_ID,
                    };
                    deleteWarehouseBinList.Add(deleteWarehouseBin);
                }
                #endregion
            }
            else
            {
                #region 列表删除

                #region 验证

                gdGrid.UpdateData();
                var checkedWarehouseList = HeadGridDS.Where(p => p.IsChecked == true).ToList();

                if (checkedWarehouseList.Count == 0)
                {
                    //请至少勾选一条仓库信息进行删除
                    MessageBoxs.Show(Trans.PIS, this.ToString(),
                        MsgHelp.GetMsg(MsgCode.W_0017,
                            new object[] { SystemTableEnums.Name.PIS_Warehouse, SystemActionEnum.Name.DELETE }),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //验证仓库是否已被使用
                StringBuilder warehouseIDs = new StringBuilder();
                warehouseIDs.Append(SysConst.Semicolon_DBC);
                foreach (var loopSelectedItem in checkedWarehouseList)
                {
                    warehouseIDs.Append(loopSelectedItem.WH_ID + SysConst.Semicolon_DBC);
                }
                //已被使用的仓库
                List<MDLPIS_Warehouse> usedWarehouseList = new List<MDLPIS_Warehouse>();
                _bll.QueryForList(SQLID.PIS_WarehouseManager_SQL01, new MDLPIS_Warehouse
                {
                    WHERE_WH_ID = warehouseIDs.ToString()
                }, usedWarehouseList);
                if (usedWarehouseList.Count > 0)
                {
                    StringBuilder warehouseName = new StringBuilder();
                    int i = 0;
                    foreach (var loopWarehouse in usedWarehouseList)
                    {
                        i++;
                        if (i == 1)
                        {
                            warehouseName.Append(loopWarehouse.WH_Name);
                        }
                        else
                        {
                            warehouseName.Append(SysConst.Comma_DBC + loopWarehouse.WH_Name);
                        }
                    }
                    //仓库已经被使用，不能删除
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0007, new object[] { warehouseName, MsgParam.APPLY, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //确认删除
                DialogResult dialogResult = MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0013, new object[] { checkedWarehouseList.Count }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                #endregion

                _bll.CopyModelList(checkedWarehouseList, deleteWarehouseList);
                string warehouseIdStr = string.Empty;
                foreach (var loopSelectedItem in deleteWarehouseList)
                {
                    if (string.IsNullOrEmpty(loopSelectedItem.WH_ID))
                    {
                        continue;
                    }
                    loopSelectedItem.WHERE_WH_ID = loopSelectedItem.WH_ID;
                    warehouseIdStr += loopSelectedItem.WH_ID + SysConst.Semicolon_DBC;
                }
                //根据仓库ID查询仓位列表
                _bll.QueryForList(SQLID.PIS_WarehouseManager_SQL03, warehouseIdStr, deleteWarehouseBinList);
                #endregion
            }

            #endregion

            #region 删除数据
            if (deleteWarehouseList.Count > 0)
            {
                var deleteWarehouseResult = _bll.DeleteWarehouseAndBin(deleteWarehouseList, deleteWarehouseBinList);
                if (!deleteWarehouseResult)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //删除成功
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            #endregion

            //4.清空【详情】画面数据
            InitializeDetailTabControls();
            //刷新列表
            RefreshList(true);

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

            //2.执行基类方法（注意：基类使用模糊查询）
            base.ConditionDS = new WarehouseManagerQCModel()
            {
                SqlId = SQLID.PIS_WarehouseManager_SQL02,
                //仓库名称
                WHERE_WH_Name = txtWhere_WH_Name.Text.Trim(),
                //仓库编号
                WHERE_WH_No = txtWhere_WH_No.Text.Trim(),
                //仓库地址
                WHERE_WH_Address = txtWhere_WH_Address.Text.Trim(),
                //组织ID
                WHERE_WH_Org_ID = LoginInfoDAX.OrgID,
                //有效
                WHERE_WH_IsValid = ckWhere_WH_IsValid.Checked,
            };
            base.QueryAction();
            //3.Grid绑定数据源
            gdGrid.DataSource = HeadGridDS;
            gdGrid.DataBind();
            //3.设置Grid自适应列宽（根据单元格内容）
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
            base.NewUIModel = HeadDS;
            if (ViewHasChanged()
                || _warehouseBinList.InsertList.Count > 0
                || _warehouseBinList.UpdateList.Count > 0
                || _warehouseBinList.DeleteList.Count > 0)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region 自定义方法

        #region 仓库管理
        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            //仓库名称
            txtWH_Name.Clear();
            //仓库编号
            txtWH_No.Clear();
            //组织ID
            txtOrgName.Clear();
            txtOrgName.Text = LoginInfoDAX.OrgShortName;
            //仓库地址
            txtWH_Address.Clear();
            //仓库描述
            txtWH_Description.Clear();
            //有效
            ckWH_IsValid.Checked = true;
            ckWH_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtWH_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtWH_CreatedTime.Value = BLLCom.GetCurStdDatetime();
            //修改人
            txtWH_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtWH_UpdatedTime.Value = BLLCom.GetCurStdDatetime();
            //仓库ID
            txtWH_ID.Clear();
            //版本号
            txtWH_VersionNo.Clear();
            //给 仓库名称 设置焦点
            lblWH_Name.Focus();

            #region 仓位管理
            _warehouseBinList = new SkyCarBindingList<WarehouseBinManagerUIModel, MDLPIS_WarehouseBin>();
            gdGridWarehouseBin.DataSource = _warehouseBinList;
            gdGridWarehouseBin.DataBind();
            _warehouseBinList.StartMonitChanges();

            #endregion
        }
        
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //仓库名称
            txtWhere_WH_Name.Clear();
            //仓库编号
            txtWhere_WH_No.Clear();
            //仓库地址
            txtWhere_WH_Address.Clear();
            //有效
            ckWhere_WH_IsValid.Checked = true;
            ckWhere_WH_IsValid.CheckState = CheckState.Checked;
            //给 仓库名称 设置焦点
            lblWhere_WH_Name.Focus();
            #endregion

            //清空grid数据
            HeadGridDS = new BindingList<WarehouseManagerUIModel>();
            gdGrid.DataSource = HeadGridDS;
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
            base.NewUIModel = HeadDS;

            var activeRowIndex = gdGrid.ActiveRow.Index;
            //判断Grid内[唯一标识]是否为空
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_Warehouse.Code.WH_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_Warehouse.Code.WH_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            HeadDS = HeadGridDS.FirstOrDefault(x => x.WH_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_Warehouse.Code.WH_ID].Value);
            if (HeadDS == null || string.IsNullOrEmpty(HeadDS.WH_ID))
            {
                return;
            }

            if (txtWH_ID.Text != HeadDS.WH_ID
                || (txtWH_ID.Text == HeadDS.WH_ID && txtWH_VersionNo.Text != HeadDS.WH_VersionNo?.ToString()))
            {
                if (txtWH_ID.Text == HeadDS.WH_ID && txtWH_VersionNo.Text != HeadDS.WH_VersionNo?.ToString())
                {
                    //数据版本已过期，将加载最新详情。
                    MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.DataHasOverdue }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else if (ViewHasChanged()
                || _warehouseBinList.InsertList.Count > 0
                || _warehouseBinList.UpdateList.Count > 0
                || _warehouseBinList.DeleteList.Count > 0)
                {
                    //将放弃之前的修改，是否继续？
                    DialogResult dialogResult = MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.ConfirmGiveUpEdit }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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

            //设置详情是否可编辑
            SetDetailControl();
            //查询仓位
            QueryWarehouseBin();
        }
      
        /// <summary>
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //仓库名称
            txtWH_Name.Text = HeadDS.WH_Name;
            //仓库编号
            txtWH_No.Text = HeadDS.WH_No;
            //组织ID
            txtOrgName.Text = HeadDS.OrgName;
            //仓库地址
            txtWH_Address.Text = HeadDS.WH_Address;
            //仓库描述
            txtWH_Description.Text = HeadDS.WH_Description;
            //有效
            if (HeadDS.WH_IsValid != null)
            {
                ckWH_IsValid.Checked = HeadDS.WH_IsValid.Value;
            }
            //创建人
            txtWH_CreatedBy.Text = HeadDS.WH_CreatedBy;
            //创建时间
            dtWH_CreatedTime.Value = HeadDS.WH_CreatedTime?.ToString() ?? "";
            //修改人
            txtWH_UpdatedBy.Text = HeadDS.WH_UpdatedBy;
            //修改时间
            dtWH_UpdatedTime.Value = HeadDS.WH_UpdatedTime?.ToString() ?? "";
            //仓库ID
            txtWH_ID.Text = HeadDS.WH_ID;
            //版本号
            txtWH_VersionNo.Text = HeadDS.WH_VersionNo?.ToString() ?? "";

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
            #region 前段检查—不涉及数据库

            //判断仓库名称为空
            if (string.IsNullOrEmpty(txtWH_Name.Text))
            {
                //仓库名称不能为空txtWH_Address
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[]
                    { SystemTableColumnEnums.PIS_Warehouse.Name.WH_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtWH_Name.Focus();
                return false;
            }
            //判断仓库编号为空
            if (string.IsNullOrEmpty(txtWH_No.Text))
            {
                //仓库编号不能为空
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[]
                    { SystemTableColumnEnums.PIS_Warehouse.Name.WH_No }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtWH_No.Focus();
                return false;
            }
            //判断仓库地址为空
            if (string.IsNullOrEmpty(txtWH_Address.Text))
            {
                //仓库地址不能为空
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[]
                    { SystemTableColumnEnums.PIS_Warehouse.Name.WH_Address }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtWH_Address.Focus();
                return false;
            }
            
            #endregion

            #region 前端检查-涉及到数据表

            #region 检查【仓库】是否已存在

            //检查仓库名称是否存在
            var userdCount = _bll.QueryForObject<int>(SQLID.PIS_WarehouseManager_SQL04, new MDLPIS_Warehouse
            {
                WHERE_WH_ID = txtWH_ID.Text.Trim(),
                WHERE_WH_Name = txtWH_Name.Text.Trim(),
                WHERE_WH_Org_ID = LoginInfoDAX.OrgID,

            });
            if (userdCount > 0)
            {
                //仓库名称已存在
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0006, new object[] { SystemTableColumnEnums.PIS_Warehouse.Name.WH_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            #endregion

            #region 检查【仓库编码】是否已存在

            //检查仓库名称是否存在
            var userdWHNoCount = _bll.QueryForObject<int>(SQLID.PIS_WarehouseManager_SQL05, new MDLPIS_Warehouse
            {
                WHERE_WH_ID = txtWH_ID.Text.Trim(),
                WHERE_WH_No = txtWH_No.Text.Trim(),
                WHERE_WH_Org_ID = LoginInfoDAX.OrgID,
            });
            if (userdWHNoCount > 0)
            {
                //仓库名称已存在
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0006, new object[] { SystemTableColumnEnums.PIS_Warehouse.Name.WH_No }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            #endregion

            #region 检查【仓位】是否已使用
            StringBuilder deleteWarehouseBillIDs = new StringBuilder();
            deleteWarehouseBillIDs.Append(SysConst.Semicolon_DBC);
            foreach (var loopSelectedItem in _warehouseBinList.DeleteList)
            {
                deleteWarehouseBillIDs.Append(loopSelectedItem.WHB_ID + SysConst.Semicolon_DBC);
            }
            List<MDLPIS_WarehouseBin> returnWarehouseBinList = new List<MDLPIS_WarehouseBin>();
            _bll.QueryForList<MDLPIS_WarehouseBin>(SQLID.PIS_WarehouseBinManager_SQL01, new MDLPIS_WarehouseBin
            { WHERE_WHB_ID = deleteWarehouseBillIDs.ToString() }, returnWarehouseBinList);

            if (returnWarehouseBinList.Count > 0)
            {
                StringBuilder warehouseBinName = new StringBuilder();
                int i = 0;
                foreach (var loopWarehouseBin in returnWarehouseBinList)
                {
                    i++;
                    if (i == 1)
                    {
                        warehouseBinName.Append(loopWarehouseBin.WHB_Name);
                    }
                    else
                    {
                        warehouseBinName.Append(SysConst.Comma_DBC + loopWarehouseBin);
                    }
                }

                //仓位已经被使用，不能删除
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0007, new object[] { warehouseBinName, MsgParam.APPLY, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            #endregion

            #endregion

            return true;
        }
      
        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            HeadDS = new WarehouseManagerUIModel()
            {
                //仓库名称
                WH_Name = txtWH_Name.Text.Trim(),
                //仓库编号
                WH_No = txtWH_No.Text.Trim(),
                //组织ID
                WH_Org_ID = LoginInfoDAX.OrgID,
                //组织名称
                OrgName = txtOrgName.Text.Trim(),
                //仓库地址
                WH_Address = txtWH_Address.Text.Trim(),
                //仓库描述
                WH_Description = txtWH_Description.Text.Trim(),
                //有效
                WH_IsValid = ckWH_IsValid.Checked,
                //创建人
                WH_CreatedBy = txtWH_CreatedBy.Text.Trim(),
                //创建时间
                WH_CreatedTime = (DateTime?)dtWH_CreatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //修改人
                WH_UpdatedBy = LoginInfoDAX.UserName,
                //修改时间
                WH_UpdatedTime = DateTime.Now,
                //仓库ID
                WH_ID = txtWH_ID.Text.Trim(),
                //版本号
                WH_VersionNo = Convert.ToInt64(txtWH_VersionNo.Text.Trim() == "" ? "1" : txtWH_VersionNo.Text.Trim()),
            };
        }
        #endregion

        #region 仓位管理
        /// <summary>
        /// 将Grid中值赋值给属性，留页面传参用
        /// </summary>
        private void UpdateWarehouseBin()
        {
            //初始化窗体对象
            FrmWarehouseBinManager warehouseBinManager = new FrmWarehouseBinManager();
            //获取双击的行
            int activeRowIndex = gdGridWarehouseBin.ActiveRow.Index;
            //取得双击行的值
            warehouseBinManager.warehouseBinManagerUIModel.WHB_ID = gdGridWarehouseBin.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_ID].Text.Trim();
            warehouseBinManager.warehouseBinManagerUIModel.WHB_WH_ID = gdGridWarehouseBin.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_WH_ID].Text.Trim();
            warehouseBinManager.warehouseBinManagerUIModel.WHB_Name = gdGridWarehouseBin.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name].Text.Trim();
            warehouseBinManager.warehouseBinManagerUIModel.WHB_Description = gdGridWarehouseBin.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Description].Text.Trim();
            warehouseBinManager.warehouseBinManagerUIModel.WHB_CreatedBy = gdGridWarehouseBin.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_CreatedBy].Text.Trim();
            warehouseBinManager.warehouseBinManagerUIModel.WHB_UpdatedBy = gdGridWarehouseBin.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_UpdatedBy].Text.Trim();
            warehouseBinManager.warehouseBinManagerUIModel.WHB_VersionNo = Convert.ToInt64(gdGridWarehouseBin.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_VersionNo].Text.Trim());
            warehouseBinManager.warehouseBinManagerUIModel.WHB_IsValid = Convert.ToBoolean(gdGridWarehouseBin.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_IsValid].Text.Trim());
            warehouseBinManager.warehouseBinManagerUIModel.WHB_CreatedTime = Convert.ToDateTime(gdGridWarehouseBin.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_CreatedTime].Text.Trim());
            warehouseBinManager.warehouseBinManagerUIModel.WHB_UpdatedTime = Convert.ToDateTime(gdGridWarehouseBin.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_UpdatedTime].Text.Trim());
            warehouseBinManager.warehouseBinManagerUIModel.Tmp_SID_ID = gdGridWarehouseBin.Rows[activeRowIndex].Cells["Tmp_SID_ID"].Text.Trim();
            //显示窗体
            warehouseBinManager.ShowDialog();
            //判断修改的仓位名称是否重复
            foreach (var loopWarehouseBin in _warehouseBinList)
            {
                if (loopWarehouseBin.WHB_Name == warehouseBinManager.warehouseBinManagerUIModel.WHB_Name && loopWarehouseBin.Tmp_SID_ID != warehouseBinManager.warehouseBinManagerUIModel.Tmp_SID_ID)
                {
                    //仓位名称已存在
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0006, new object[] { SystemTableColumnEnums.PIS_WarehouseBin.Name.WHB_Name }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            //将修改过的信息赋值给对应的_warehouseBinList
            foreach (var loopWarehouseBin in _warehouseBinList)
            {
                if (loopWarehouseBin.Tmp_SID_ID == warehouseBinManager.warehouseBinManagerUIModel.Tmp_SID_ID)
                {
                    loopWarehouseBin.WHB_ID = warehouseBinManager.warehouseBinManagerUIModel.WHB_ID;
                    loopWarehouseBin.WHB_WH_ID = warehouseBinManager.warehouseBinManagerUIModel.WHB_WH_ID;
                    loopWarehouseBin.WHB_Name = warehouseBinManager.warehouseBinManagerUIModel.WHB_Name;
                    loopWarehouseBin.WHB_Description = warehouseBinManager.warehouseBinManagerUIModel.WHB_Description;
                    loopWarehouseBin.WHB_VersionNo = warehouseBinManager.warehouseBinManagerUIModel.WHB_VersionNo;
                    loopWarehouseBin.WHB_IsValid = warehouseBinManager.warehouseBinManagerUIModel.WHB_IsValid;
                    loopWarehouseBin.WHB_UpdatedBy = LoginInfoDAX.UserName;
                    loopWarehouseBin.WHB_UpdatedTime = DateTime.Now;
                }
            }
            gdGridWarehouseBin.DataSource = _warehouseBinList;
            gdGridWarehouseBin.DataBind();

            gdGridWarehouseBin.UpdateData();
        }

        /// <summary>
        /// 查询仓位
        /// </summary>
        private void QueryWarehouseBin()
        {
            //将查询出来的结果赋值grid
            _bll.QueryForList<MDLPIS_WarehouseBin, WarehouseBinManagerUIModel>(new MDLPIS_WarehouseBin
            {
                WHERE_WHB_WH_ID = txtWH_ID.Text.Trim(),
                WHERE_WHB_IsValid = true
            }, _warehouseBinList);
            //开始监控仓位List变化
            _warehouseBinList.StartMonitChanges();
            foreach (var loopwarehouseBin in _warehouseBinList)
            {
                loopwarehouseBin.Tmp_SID_ID = Guid.NewGuid().ToString();
            }
            gdGridWarehouseBin.DataSource = _warehouseBinList;
            gdGridWarehouseBin.DataBind();
            //自适应
            gdGridWarehouseBin.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdGridWarehouseBin.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 添加仓位
        /// </summary>
        private void AddWarehouseBin()
        {
            FrmWarehouseBinManager warehouseBinManager = new FrmWarehouseBinManager();

            DialogResult dialogResult = warehouseBinManager.ShowDialog();
            warehouseBinManager.warehouseBinManagerUIModel.WHB_WH_ID = txtWH_ID.Text.Trim();
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            if (string.IsNullOrEmpty(warehouseBinManager.warehouseBinManagerUIModel.WHB_Name))
            {
                //仓位名称不能为空
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.PIS_WarehouseBin.Name.WHB_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //接受仓位窗体传来的值
            WarehouseBinManagerUIModel warehouseBin = new WarehouseBinManagerUIModel()
            {
                WHB_Name = warehouseBinManager.warehouseBinManagerUIModel.WHB_Name,
                WHB_Description = warehouseBinManager.warehouseBinManagerUIModel.WHB_Description,
                WHB_IsValid = warehouseBinManager.warehouseBinManagerUIModel.WHB_IsValid,
                WHB_VersionNo = warehouseBinManager.warehouseBinManagerUIModel.WHB_VersionNo,
                WHB_CreatedBy = warehouseBinManager.warehouseBinManagerUIModel.WHB_CreatedBy,
                WHB_CreatedTime = warehouseBinManager.warehouseBinManagerUIModel.WHB_CreatedTime,
                WHB_UpdatedBy = LoginInfoDAX.UserName,
                WHB_UpdatedTime = DateTime.Now,
                Tmp_SID_ID = warehouseBinManager.warehouseBinManagerUIModel.Tmp_SID_ID
            };
            //判断传来仓位名称是否重复
            foreach (var loopwarehouseBin in _warehouseBinList)
            {
                if (loopwarehouseBin.WHB_Name == warehouseBin.WHB_Name)
                {
                    //仓位名称已存在
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0006, new object[] { SystemTableColumnEnums.PIS_WarehouseBin.Name.WHB_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            _warehouseBinList.Insert(0, warehouseBin);

            if (dialogResult == DialogResult.OK)
            {
                warehouseBinManager.Close();
                gdGridWarehouseBin.DataSource = _warehouseBinList;
                gdGridWarehouseBin.DataBind();
                //设置Grid自适应列宽（根据单元格内容）
                gdGridWarehouseBin.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
                //设置单元格样式
                SetWarehouseStyle();
            }
        }

        /// <summary>
        /// 删除仓位
        /// </summary>
        private void DeleteWarehouseBin()
        {
            #region 从列表中删除
            gdGridWarehouseBin.UpdateData();
            //待删除的仓库明细列表
            var deleteWarehouseBinList = _warehouseBinList.Where(p => p.IsChecked == true).ToList();
            if (deleteWarehouseBinList.Count == 0)
            {
                //选择要删除的仓位
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { SystemTableEnums.Name.PIS_WarehouseBin, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //移除勾选项
            foreach (var loopSalesOrderDetail in deleteWarehouseBinList)
            {
                _warehouseBinList.Remove(loopSalesOrderDetail);
            }
            gdGridWarehouseBin.DataSource = _warehouseBinList;
            gdGridWarehouseBin.DataBind();
            #endregion
        }

        #endregion

        /// <summary>
        /// 设置详情是否可编辑
        /// </summary>
        private void SetDetailControl()
        {
            //已保存的仓库名称不可编辑
            if (!string.IsNullOrEmpty(txtWH_ID.Text))
            {
                txtWH_Name.Enabled = false;
            }
            else
            {
                txtWH_Name.Enabled = true;
            }
        }

        /// <summary>
        /// 设置单元格样式
        /// </summary>
        private void SetWarehouseStyle()
        {
            #region 设置Grid数据颜色
            gdGridWarehouseBin.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
            foreach (var loopGridRow in gdGridWarehouseBin.Rows)
            {
                #region 仓位名称

                //设置单元格 
                loopGridRow.Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name].Activation = Activation.AllowEdit;
                //设置单元格背景色
                loopGridRow.Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                //设置单元格边框颜色
                loopGridRow.Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                #endregion

                #region 仓位描述

                //设置单元格 
                loopGridRow.Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Description].Activation = Activation.AllowEdit;
                //设置单元格背景色
                loopGridRow.Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Description].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                //设置单元格边框颜色
                loopGridRow.Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Description].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                #endregion

                #region 有效

                //设置单元格 
                loopGridRow.Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_IsValid].Activation = Activation.AllowEdit;
                //设置单元格背景色
                loopGridRow.Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_IsValid].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                //设置单元格边框颜色
                loopGridRow.Cells[SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_IsValid].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                #endregion
            }

            #endregion
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
                    var removeList = HeadGridDS.Where(x => x.IsChecked == true).ToList();
                    foreach (var loopRemove in removeList)
                    {
                        HeadGridDS.Remove(loopRemove);
                    }
                }
                else
                {
                    var curHead = HeadGridDS.FirstOrDefault(x => x.WH_ID == HeadDS.WH_ID);
                    if (curHead != null)
                    {
                        HeadGridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = HeadGridDS.FirstOrDefault(x => x.WH_ID == HeadDS.WH_ID);
                if (curHead != null)
                {
                    _bll.CopyModel(HeadDS, curHead);
                }
                else
                {
                    HeadGridDS.Insert(0, HeadDS);
                }
            }

            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }

        #endregion

    }
}
