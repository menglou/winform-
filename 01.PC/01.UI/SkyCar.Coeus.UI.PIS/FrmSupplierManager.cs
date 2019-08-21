using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.PIS;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.PIS;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.Common.Enums;
using System.Text;
using Infragistics.Win.UltraWinTabControl;

namespace SkyCar.Coeus.UI.PIS
{
    /// <summary>
    /// 供应商管理
    /// </summary>
    public partial class FrmSupplierManager : BaseFormCardList<SupplierManagerUIModel, SupplierManagerQCModel, MDLPIS_Supplier>
    {
        #region 全局变量

        /// <summary>
        /// 供应商管理BLL
        /// </summary>
        private SupplierManagerBLL _bll = new SupplierManagerBLL();
        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmSupplierManager构造方法
        /// </summary>
        public FrmSupplierManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSupplierManager_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（动作，导航）
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
            //根据选中的Tab，设置动作按钮[是否可用]（在系统权限的基础上进行控制）
            base.SetActionEnableBySelectedTab(SysConst.EN_LIST);
            #endregion

            //加载中国的省份
            InitializeProvList();

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
        /// 编码KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_SUPP_Code_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 名称KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_SUPP_Name_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 简称KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_SUPP_ShortName_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 联系人KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_SUPP_Contacter_KeyDown(object sender, KeyEventArgs e)
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
        private void ckWhere_SUPP_IsValid_CheckedChanged(object sender, EventArgs e)
        {
            ////执行查询
            //QueryAction();
        }
        #endregion

        #region 【详情】相关事件

        /// <summary>
        /// [省]下拉框发生改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cbSUPP_Prov_Code_ValueChanged(object sender, EventArgs e)
        {
            QueryCityListByProv(cbSUPP_Prov_Code.Value.ToString());
        }
        /// <summary>
        /// [市]下拉框发生改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSUPP_City_Code_ValueChanged(object sender, EventArgs e)
        {
            QueryDistListByCity(cbSUPP_City_Code.Value.ToString());
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
            if (!_bll.SaveDetailDS(DetailDS))
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //4.将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();
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

            #region 准备数据

            //待删除的入库单列表
            List<MDLPIS_Supplier> deleteSupplierList = new List<MDLPIS_Supplier>();
            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 详情删除

                MDLPIS_Supplier deleteSupplier = new MDLPIS_Supplier()
                {
                    WHERE_SUPP_ID = txtSUPP_ID.Text.Trim()
                };
                deleteSupplierList.Add(deleteSupplier);

                #endregion
            }
            else
            {
                #region 列表删除

                gdGrid.UpdateData();
                var deleteSupplierUIModelList = base.GridDS.Where(p => p.IsChecked == true).ToList();
                _bll.CopyModelList<SupplierManagerUIModel, MDLPIS_Supplier>(deleteSupplierUIModelList, deleteSupplierList);
                foreach (var loopSelectedItem in deleteSupplierList)
                {
                    if (loopSelectedItem.SUPP_ID == null)
                    {
                        continue;
                    }
                    loopSelectedItem.WHERE_SUPP_ID = loopSelectedItem.SUPP_ID;
                }
                #endregion
            }

            #endregion

            #region 删除数据
            if (deleteSupplierList.Count > 0)
            {
                var deleteSupplierResult = _bll.DeleteSupplier(deleteSupplierList);
                if (!deleteSupplierResult)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //删除成功
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            #endregion

            //4.清空【详情】画面数据
            InitializeDetailTabControls();
            //刷新列表
            RefreshList(true);

            //将DetailDS数据赋值给【详情】Tab内的对应控件
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
            base.ConditionDS = new SupplierManagerQCModel()
            {
                SqlId = SQLID.PIS_SupplierManager_SQL01,
                //编码
                WHERE_SUPP_Code = txtWhere_SUPP_Code.Text.Trim(),
                //名称
                WHERE_SUPP_Name = txtWhere_SUPP_Name.Text.Trim(),
                //简称
                WHERE_SUPP_ShortName = txtWhere_SUPP_ShortName.Text.Trim(),
                //联系人
                WHERE_SUPP_Contacter = txtWhere_SUPP_Contacter.Text.Trim(),
                //有效
                WHERE_SUPP_IsValid = ckWhere_SUPP_IsValid.Checked,
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
        /// 清空
        /// </summary>
        public override void ClearAction()
        {
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
            //名称
            txtSUPP_Name.Clear();
            //简称
            txtSUPP_ShortName.Clear();
            //编码
            txtSUPP_Code.Clear();
            //联系人
            txtSUPP_Contacter.Clear();
            //固定号码
            txtSUPP_Tel.Clear();
            //电话号码
            txtSUPP_Phone.Clear();
            //QQ号码
            txtSUPP_QQ.Clear();
            //地区
            txtSUPP_Territory.Clear();
            //省
            cbSUPP_Prov_Code.Clear();
            //市
            //cbSUPP_City_Code.Clear();
            //区
            cbSUPP_Dist_Code.Clear();
            //地址
            txtSUPP_Address.Clear();
            //评估等级
            txtSUPP_EvaluateLevel.Clear();
            //最近评估日
            dtSUPP_LastEvaluateDate.Value = DateTime.Now;
            //开户行
            txtSUPP_BankName.Clear();
            //开户名
            txtSUPP_BankAccountName.Clear();
            //账号
            txtSUPP_BankAccountNo.Clear();
            //主营配件
            txtSUPP_MainAutoParts.Clear();
            //备注
            txtSUPP_Remark.Clear();
            //有效
            ckSUPP_IsValid.Checked = true;
            ckSUPP_IsValid.CheckState = System.Windows.Forms.CheckState.Checked;
            //创建人
            txtSUPP_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtSUPP_CreatedTime.Value = DateTime.Now;
            //修改人
            txtSUPP_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtSUPP_UpdatedTime.Value = DateTime.Now;
            //ID
            txtSUPP_ID.Clear();
            //版本号
            txtSUPP_VersionNo.Clear();
            //给 名称 设置焦点
            lblSUPP_Name.Focus();
            #endregion
        }
        
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //编码
            txtWhere_SUPP_Code.Clear();
            //名称
            txtWhere_SUPP_Name.Clear();
            //简称
            txtWhere_SUPP_ShortName.Clear();
            //联系人
            txtWhere_SUPP_Contacter.Clear();
            //有效
            ckWhere_SUPP_IsValid.Checked = true;
            ckWhere_SUPP_IsValid.CheckState = System.Windows.Forms.CheckState.Checked;
            //给 编码 设置焦点
            lblWhere_SUPP_Code.Focus();
            #endregion

            //清空grid数据
            GridDS = new BindingList<SupplierManagerUIModel>();
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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_Supplier.Code.SUPP_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_Supplier.Code.SUPP_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            DetailDS = base.GridDS.FirstOrDefault(x => x.SUPP_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_Supplier.Code.SUPP_ID].Value);
            if (DetailDS == null || string.IsNullOrEmpty(DetailDS.SUPP_ID))
            {
                return;
            }

            if (txtSUPP_ID.Text != DetailDS.SUPP_ID
                || (txtSUPP_ID.Text == DetailDS.SUPP_ID && txtSUPP_VersionNo.Text != DetailDS.SUPP_VersionNo?.ToString()))
            {
                if (txtSUPP_ID.Text == DetailDS.SUPP_ID && txtSUPP_VersionNo.Text != DetailDS.SUPP_VersionNo?.ToString())
                {
                    //数据版本已过期，将加载最新详情。
                    MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.DataHasOverdue }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else if (ViewHasChanged())
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
        }
        
        /// <summary>
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //名称
            txtSUPP_Name.Text = DetailDS.SUPP_Name ;
            //简称
            txtSUPP_ShortName.Text = DetailDS.SUPP_ShortName ;
            //编码
            txtSUPP_Code.Text = DetailDS.SUPP_Code ;
            //联系人
            txtSUPP_Contacter.Text = DetailDS.SUPP_Contacter ;
            //固定号码
            txtSUPP_Tel.Text = DetailDS.SUPP_Tel ;
            //电话号码
            txtSUPP_Phone.Text = DetailDS.SUPP_Phone ;
            //QQ号码
            txtSUPP_QQ.Text = DetailDS.SUPP_QQ ;
            //地区
            txtSUPP_Territory.Text = DetailDS.SUPP_Territory ;
            //省
            cbSUPP_Prov_Code.Value = DetailDS.SUPP_Prov_Code ;
            //市
            cbSUPP_City_Code.Value = DetailDS.SUPP_City_Code ;
            //区
            cbSUPP_Dist_Code.Value = DetailDS.SUPP_Dist_Code ;
            //地址
            txtSUPP_Address.Text = DetailDS.SUPP_Address ;
            //评估等级
            txtSUPP_EvaluateLevel.Text = DetailDS.SUPP_EvaluateLevel ;
            //最近评估日
            dtSUPP_LastEvaluateDate.Value = DetailDS.SUPP_LastEvaluateDate == null ? "" : DetailDS.SUPP_LastEvaluateDate.ToString();
            //开户行
            txtSUPP_BankName.Text = DetailDS.SUPP_BankName ;
            //开户名
            txtSUPP_BankAccountName.Text = DetailDS.SUPP_BankAccountName ;
            //账号
            txtSUPP_BankAccountNo.Text = DetailDS.SUPP_BankAccountNo ;
            //主营配件
            txtSUPP_MainAutoParts.Text = DetailDS.SUPP_MainAutoParts ;
            //备注
            txtSUPP_Remark.Text = DetailDS.SUPP_Remark;
            //有效
            if (DetailDS.SUPP_IsValid != null)
            {
                ckSUPP_IsValid.Checked = DetailDS.SUPP_IsValid.Value;
            }
            //创建人
            txtSUPP_CreatedBy.Text = DetailDS.SUPP_CreatedBy ;
            //创建时间
            dtSUPP_CreatedTime.Value = DetailDS.SUPP_CreatedTime == null ? "" : DetailDS.SUPP_CreatedTime.ToString();
            //修改人
            txtSUPP_UpdatedBy.Text = DetailDS.SUPP_UpdatedBy ;
            //修改时间
            dtSUPP_UpdatedTime.Value = DetailDS.SUPP_UpdatedTime == null ? "" : DetailDS.SUPP_UpdatedTime.ToString();
            //ID
            txtSUPP_ID.Text = DetailDS.SUPP_ID ;
            //版本号
            txtSUPP_VersionNo.Value = DetailDS.SUPP_VersionNo == null ? "" : DetailDS.SUPP_VersionNo.ToString();

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
            #region 前端检查-不涉及数据库
            //验证名称是否为空
            if (string.IsNullOrEmpty(txtSUPP_Name.Text.Trim()))
            {
                //名称不能为空
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[]
                    { SystemTableColumnEnums.PIS_Supplier.Name.SUPP_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSUPP_Name.Focus();
                return false;
            }
            //验证编码是否为空
            if (string.IsNullOrEmpty(txtSUPP_Code.Text.Trim()))
            {
                //编码不能为空
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[]
                    { SystemTableColumnEnums.PIS_Supplier.Name.SUPP_Code }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSUPP_Code.Focus();
                return false;
            }
            #endregion

            #region 前端检查-涉及数据库
            //1.前端检查名称是否存在
            var arguSupplier = new MDLPIS_Supplier
            {
                //设置查询条件
                WHERE_SUPP_Name = txtSUPP_Name.Text.Trim(),
            };
            //2.执行查询（检查名称在数据表中是否已存在
            if (_bll.QueryForCount<MDLPIS_Supplier>(arguSupplier) != 0)
            {
                //判断供应商ID为空，做添加前检查
                if (string.IsNullOrEmpty(txtSUPP_ID.Text.Trim()))
                {
                    //名称已存在
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0006, new object[] { SystemTableColumnEnums.PIS_Supplier.Name.SUPP_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //如果供应商ID不为空，做修改前检查
                arguSupplier.WHERE_SUPP_ID = txtSUPP_ID.Text.Trim();
                if (_bll.QueryForCount<MDLBS_AutoPartsName>(arguSupplier) == 0)
                {
                    //名称已存在
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0006, new object[] { SystemTableColumnEnums.PIS_Supplier.Name.SUPP_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(txtSUPP_Code.Text.Trim()))
            {
                //1.前端检查编码是否存在
                arguSupplier = new MDLPIS_Supplier
                {
                    //设置查询条件
                    WHERE_SUPP_Code = txtSUPP_Code.Text.Trim(),
                };
                //2.执行查询（检查编码在数据表中是否已存在
                if (_bll.QueryForCount<MDLPIS_Supplier>(arguSupplier) != 0)
                {
                    //判断供应商ID为空，做添加前检查
                    if (string.IsNullOrEmpty(txtSUPP_ID.Text.Trim()))
                    {
                        //编码已存在
                        MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0006, new object[] { SystemTableColumnEnums.PIS_Supplier.Name.SUPP_Code }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                    //如果供应商ID不为空，做修改前检查
                    arguSupplier.WHERE_SUPP_ID = txtSUPP_ID.Text.Trim();
                    if (_bll.QueryForCount<MDLBS_AutoPartsName>(arguSupplier) == 0)
                    {
                        //编码已存在
                        MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0006, new object[] { SystemTableColumnEnums.PIS_Supplier.Name.SUPP_Code }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }

            #endregion
            return true;
        }
        
        /// <summary>
        /// 前端检查-删除
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForDelete()
        {
            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 详情前端检查-删除
                if (string.IsNullOrEmpty(txtSUPP_ID.Text))
                {
                    //请选择要删除的供应商
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0006, new object[] { MsgParam.SUPPLIER, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                #region 供应商检查

                var resultSupplier = _bll.QueryForObject(SQLID.PIS_SupplierManager_SQL02, new MDLPIS_Supplier
                {
                    WHERE_SUPP_ID = txtSUPP_ID.Text + SysConst.Semicolon_DBC
                });
                if (resultSupplier != null)
                {
                    //供应商已经被使用，不能删除
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0007, new object[] { MsgParam.SUPPLIER, MsgParam.APPLY, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                #endregion

                //确认删除
                DialogResult dialogResult = MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0012), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return false;
                }
                #endregion
            }
            else
            {
                #region 列表删除

                #region 前端检查——不涉及数据库
                var checkedSupplierList = base.GridDS.Where(p => p.IsChecked == true).ToList();
                if (checkedSupplierList.Count == 0)
                {
                    //请勾选至少一条供应商信息进行删除！
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { MsgParam.SUPPLIER, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                #endregion

                #region 前端检查-涉及数据库
                StringBuilder deleteSupplier = new StringBuilder();
                deleteSupplier.Append(SysConst.Semicolon_DBC);
                foreach (var loopSelectedItem in checkedSupplierList)
                {
                    deleteSupplier.Append(loopSelectedItem.SUPP_ID + SysConst.Semicolon_DBC);
                }
                List<MDLPIS_Supplier> returnSupplierList = new List<MDLPIS_Supplier>();
                _bll.QueryForList(SQLID.PIS_SupplierManager_SQL02, new MDLPIS_Supplier
                {
                    WHERE_SUPP_ID = deleteSupplier.ToString()
                }, returnSupplierList);

                if (returnSupplierList.Count > 0)
                {
                    StringBuilder supplierName = new StringBuilder();
                    int i = 0;
                    foreach (var loopSupplier in returnSupplierList)
                    {
                        i++;
                        if (i == 1)
                        {
                            supplierName.Append(loopSupplier.SUPP_Name);
                        }
                        else
                        {
                            supplierName.Append(SysConst.Comma_DBC + loopSupplier.SUPP_Name);
                        }
                    }

                    //仓位已经被使用，不能删除
                    MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0007, new object[] { supplierName, MsgParam.APPLY, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                #endregion

                //已选checkedSupplierList.Count条数据，确定删除？\r\n单击【确定】删除，【取消】返回。
                DialogResult dialogResult = MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0013, new object[] { checkedSupplierList.Count }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return false;
                }
                #endregion
            }
            return true;
        }
        
        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            DetailDS = new SupplierManagerUIModel()
            {
                //名称
                SUPP_Name = txtSUPP_Name.Text.Trim(),
                //简称
                SUPP_ShortName = txtSUPP_ShortName.Text.Trim(),
                //编码
                SUPP_Code = txtSUPP_Code.Text.Trim(),
                //联系人
                SUPP_Contacter = txtSUPP_Contacter.Text.Trim(),
                //固定号码
                SUPP_Tel = txtSUPP_Tel.Text.Trim(),
                //电话号码
                SUPP_Phone = txtSUPP_Phone.Text.Trim(),
                //QQ号码
                SUPP_QQ = txtSUPP_QQ.Text.Trim(),
                //地区
                SUPP_Territory = txtSUPP_Territory.Text.Trim(),
                //省
                SUPP_Prov_Code = cbSUPP_Prov_Code.Value == null ? null : cbSUPP_Prov_Code.Value.ToString(),
                //市
                SUPP_City_Code = cbSUPP_City_Code.Value == null ? null : cbSUPP_City_Code.Value.ToString(),
                //区
                SUPP_Dist_Code = cbSUPP_Dist_Code.Value == null ? null : cbSUPP_Dist_Code.Value.ToString(),
                //地址
                SUPP_Address = txtSUPP_Address.Text.Trim(),
                //评估等级
                SUPP_EvaluateLevel = txtSUPP_EvaluateLevel.Text.Trim(),
                //最近评估日
                SUPP_LastEvaluateDate = (DateTime?)dtSUPP_LastEvaluateDate.Value ?? DateTime.Now,
                //开户行
                SUPP_BankName = txtSUPP_BankName.Text.Trim(),
                //开户名
                SUPP_BankAccountName = txtSUPP_BankAccountName.Text.Trim(),
                //账号
                SUPP_BankAccountNo = txtSUPP_BankAccountNo.Text.Trim(),
                //主营配件
                SUPP_MainAutoParts = txtSUPP_MainAutoParts.Text.Trim(),
                //备注
                SUPP_Remark = txtSUPP_Remark.Text.Trim(),
                //有效
                SUPP_IsValid = ckSUPP_IsValid.Checked,
                //创建人
                SUPP_CreatedBy = txtSUPP_CreatedBy.Text.Trim(),
                //创建时间
                SUPP_CreatedTime = (DateTime?)dtSUPP_CreatedTime.Value ?? DateTime.Now,
                //修改人
                SUPP_UpdatedBy = txtSUPP_UpdatedBy.Text.Trim(),
                //修改时间
                SUPP_UpdatedTime = (DateTime?)dtSUPP_UpdatedTime.Value ?? DateTime.Now,
                //ID
                SUPP_ID = txtSUPP_ID.Text.Trim(),
                //版本号
                SUPP_VersionNo = Convert.ToInt64(txtSUPP_VersionNo.Text.Trim() == "" ? "1" : txtSUPP_VersionNo.Text.Trim()),
            };
        }

        /// <summary>
        /// 初始化【详情】省份数据源
        /// </summary>
        private void InitializeProvList()
        {
            List<EnumCodeTextModel> resultProvList = BLLCom.QueryProvList(string.Empty);
            if (resultProvList == null)
            {
                resultProvList = new List<EnumCodeTextModel>();
            }
            resultProvList.Insert(0, new EnumCodeTextModel { Code = string.Empty, Text = SysConst.CHS_COMBTEXTBIND });
            cbSUPP_Prov_Code.DisplayMember = SysConst.EN_TEXT;
            cbSUPP_Prov_Code.ValueMember = SysConst.Value;
            cbSUPP_Prov_Code.DataSource = resultProvList;
            cbSUPP_Prov_Code.DataBind();
            cbSUPP_Prov_Code.SelectedIndex = 0;
            cbSUPP_Prov_Code.DropDown();
        }
        
        /// <summary>
        /// 根据省份筛选城市
        /// </summary>
        /// <param name="paramProvCode"></param>
        private void QueryCityListByProv(string paramProvCode)
        {
            List<EnumCodeTextModel> resultCityList = BLLCom.QueryCityList(paramProvCode);
            if (resultCityList == null)
            {
                resultCityList = new List<EnumCodeTextModel>();
            }
            resultCityList.Insert(0, new EnumCodeTextModel { Code = string.Empty, Text = SysConst.CHS_COMBTEXTBIND });
            cbSUPP_City_Code.DisplayMember = SysConst.EN_TEXT;
            cbSUPP_City_Code.ValueMember = SysConst.Value;
            cbSUPP_City_Code.DataSource = resultCityList;
            cbSUPP_City_Code.DataBind();
            cbSUPP_City_Code.SelectedIndex = 0;
            cbSUPP_City_Code.DropDown();
        }

        /// <summary>
        /// 根据城市筛选区域
        /// </summary>
        /// <param name="paramCityCode"></param>
        private void QueryDistListByCity(string paramCityCode)
        {
            List<EnumCodeTextModel> resultDistList = BLLCom.QueryDistList(paramCityCode);
            if (resultDistList == null)
            {
                resultDistList = new List<EnumCodeTextModel>();
            }
            resultDistList.Insert(0, new EnumCodeTextModel { Code = string.Empty, Text = SysConst.CHS_COMBTEXTBIND });
            cbSUPP_Dist_Code.DisplayMember = SysConst.EN_TEXT;
            cbSUPP_Dist_Code.ValueMember = SysConst.Value;
            cbSUPP_Dist_Code.DataSource = resultDistList;
            cbSUPP_Dist_Code.DataBind();
            cbSUPP_Dist_Code.SelectedIndex = 0;
            cbSUPP_Dist_Code.DropDown();
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
                    var curHead = GridDS.FirstOrDefault(x => x.SUPP_ID == DetailDS.SUPP_ID);
                    if (curHead != null)
                    {
                        GridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = GridDS.FirstOrDefault(x => x.SUPP_ID == DetailDS.SUPP_ID);
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
