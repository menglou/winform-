using System;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.SM;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.SM;
using SkyCar.Coeus.Common.Log;

namespace SkyCar.Coeus.UI.SM
{
    /// <summary>
    /// 门店管理Test
    /// </summary>
    public partial class FrmOrganizationManagerTest : BaseFormCardList<OrganizationManagerUIModel, OrganizationManagerQCModel, MDLSM_Organization>
    {
        #region 全局变量

        /// <summary>
        /// 门店管理TestBLL
        /// </summary>
        private OrganizationManagerBLL _bll = new OrganizationManagerBLL();
        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmOrganizationManagerTest构造方法
        /// </summary>
        public FrmOrganizationManagerTest()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmOrganizationManagerTest_Load(object sender, EventArgs e)
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
        /// 【列表】Grid的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_DoubleClick(object sender, EventArgs e)
        {
            //将Grid中选中的数据赋值给【详情】Tab内的对应控件
            SetGridDataToCardCtrls();
        }
        /// <summary>
        /// 【列表】Grid的Cell的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_DoubleClickCell(object sender, Infragistics.Win.UltraWinGrid.DoubleClickCellEventArgs e)
        {
            //将Grid中选中的数据赋值给【详情】Tab内的对应控件
            SetGridDataToCardCtrls();
        }
        /// <summary>
        /// 选中的Tab改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlFull_SelectedTabChanging(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventArgs e)
        {
            base.SetActionEnableBySelectedTab(e.Tab.Key);
        }
        /// <summary>
        /// 门店编码KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_Org_Code_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //执行查询
                QueryAction();
            }
        }

        #endregion

        #region 重写基类方法

        /// <summary>
        /// 新增
        /// </summary>
        public override void NewAction()
        {
            //1.执行基类方法
            base.NewAction();
            //2.初始化【详情】Tab内控件
            InitializeDetailTabControls();
            //3.设置【详情】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;
            //4.设置控件焦点
            lblOrg_Code.Focus();
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
            //4.将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            MessageBoxs.Show(Trans.SM, this.ToString(), "保存成功！", MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
        /// <summary>
        /// 复制
        /// </summary>
        public override void CopyAction()
        {
            base.CopyAction();
            //ID
            txtOrg_ID.Clear();
            //版本号
            txtOrg_VersionNo.Clear();
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
            if (!_bll.Delete(
                new MDLSM_Organization()
                {
                    WHERE_Org_ID = txtOrg_ID.Text.Trim()
                }))
            {
                MessageBoxs.Show(Trans.SM, this.ToString(), "删除失败！");
                return;
            }
            MessageBoxs.Show(Trans.SM, this.ToString(), "删除成功！", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //3.清空【详情】画面数据
            InitializeDetailTabControls();
            //4.执行查询
            QueryAction();
        }
        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            //1.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
            //2.设置查询条件（翻页相关属性不用设置）
          
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
        /// 清空查询条件
        /// </summary>
        public override void ClearAction()
        {
            //初始化【列表】Tab内控件
            InitializeListTabControls();
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 工具生成
            //门店编码
            txtOrg_Code.Clear();
            //商户ID
            txtOrg_MCT_ID.Clear();
            //事务编号
            txtOrg_TransID.Clear();
            //版本号
            txtOrg_VersionNo.Clear();
            //修改时间
            dtOrg_UpdatedTime.Value = DateTime.Now;
            //修改人
            txtOrg_UpdatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtOrg_CreatedTime.Value = DateTime.Now;
            //创建人
            txtOrg_CreatedBy.Text = LoginInfoDAX.UserName;
            //有效
            ckOrg_IsValid.Checked = true;
            ckOrg_IsValid.CheckState = System.Windows.Forms.CheckState.Checked;
            //备注
            txtOrg_Remark.Clear();
            //主营产品
            txtOrg_MainProducts.Clear();
            //主营品牌
            txtOrg_MainBrands.Clear();
            //标注点显示内容
            txtOrg_MarkerContent.Clear();
            //标注点显示标题
            txtOrg_MarkerTitle.Clear();
            //纬度
            txtOrg_Latitude.Clear();
            //经度
            txtOrg_Longitude.Clear();
            //地址
            txtOrg_Addr.Clear();
            //区域Code
            txtOrg_Dist_Code.Clear();
            //城市Code
            txtOrg_City_Code.Clear();
            //省份Code
            txtOrg_Prov_Code.Clear();
            //移动电话
            txtOrg_PhoneNo.Clear();
            //固定电话
            txtOrg_TEL.Clear();
            //联系人
            txtOrg_Contacter.Clear();
            //组织简称
            txtOrg_ShortName.Clear();
            //组织全称
            txtOrg_FullName.Clear();
            //平台编码
            txtOrg_PlatformCode.Clear();
            //ID
            txtOrg_ID.Clear();
            //给 门店编码 设置焦点
            lblOrg_Code.Focus();
            #endregion
        }
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //门店编码
            txtWhere_Org_Code.Clear();
            //给 门店编码 设置焦点
            lblWhere_Org_Code.Focus();
            #endregion

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
            var activeRowIndex = gdGrid.ActiveRow.Index;
            //判断Grid内[唯一标识]是否为空
            if (gdGrid.Rows[activeRowIndex].Cells["Org_ID"].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells["Org_ID"].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            base.DetailDS = base.GridDS.FirstOrDefault(x => x.Org_ID == gdGrid.Rows[activeRowIndex].Cells["Org_ID"].Value);
            //将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //选中【详情】Tab
            tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;
        }
        /// <summary>
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //TODO 以下代码由工具生成，请务必检查【详情】Tab内各控件的值是否正确设定，确认无误后，请删除此行注释
            //门店编码
            txtOrg_Code.Text = base.DetailDS.Org_Code;
            //商户ID
            txtOrg_MCT_ID.Text = base.DetailDS.Org_MCT_ID;
            //事务编号
            //版本号
            txtOrg_VersionNo.Value = base.DetailDS.Org_VersionNo;
            //修改时间
            dtOrg_UpdatedTime.Value = base.DetailDS.Org_UpdatedTime;
            //修改人
            txtOrg_UpdatedBy.Text = base.DetailDS.Org_UpdatedBy;
            //创建时间
            dtOrg_CreatedTime.Value = base.DetailDS.Org_CreatedTime;
            //创建人
            txtOrg_CreatedBy.Text = base.DetailDS.Org_CreatedBy;
            //有效
            if (base.DetailDS.Org_IsValid != null)
            {
                ckOrg_IsValid.Checked = base.DetailDS.Org_IsValid.Value;
            }
            //备注
            txtOrg_Remark.Text = base.DetailDS.Org_Remark;
            //主营产品
            txtOrg_MainProducts.Text = base.DetailDS.Org_MainProducts;
            //主营品牌
            txtOrg_MainBrands.Text = base.DetailDS.Org_MainBrands;
            //标注点显示内容
            txtOrg_MarkerContent.Text = base.DetailDS.Org_MarkerContent;
            //标注点显示标题
            txtOrg_MarkerTitle.Text = base.DetailDS.Org_MarkerTitle;
            //纬度
            txtOrg_Latitude.Text = base.DetailDS.Org_Latitude;
            //经度
            txtOrg_Longitude.Text = base.DetailDS.Org_Longitude;
            //地址
            txtOrg_Addr.Text = base.DetailDS.Org_Addr;
            //区域Code
            txtOrg_Dist_Code.Text = base.DetailDS.Org_Dist_Code;
            //城市Code
            txtOrg_City_Code.Text = base.DetailDS.Org_City_Code;
            //省份Code
            txtOrg_Prov_Code.Text = base.DetailDS.Org_Prov_Code;
            //移动电话
            txtOrg_PhoneNo.Text = base.DetailDS.Org_PhoneNo;
            //固定电话
            txtOrg_TEL.Text = base.DetailDS.Org_TEL;
            //联系人
            txtOrg_Contacter.Text = base.DetailDS.Org_Contacter;
            //组织简称
            txtOrg_ShortName.Text = base.DetailDS.Org_ShortName;
            //组织全称
            txtOrg_FullName.Text = base.DetailDS.Org_FullName;
            //平台编码
            txtOrg_PlatformCode.Text = base.DetailDS.Org_PlatformCode;
            //ID
            txtOrg_ID.Text = base.DetailDS.Org_ID;
            //TODO 单选框未生成（若无此控件，请删除此行）
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
                foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn loopColumn in gdGrid.DisplayLayout.Bands[0].SortedColumns)
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
            //TODO 服务端检查（无服务端检查逻辑，请删除本行注释）
            return true;
        }
        /// <summary>
        /// 前端检查-删除
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForDelete()
        {
            //TODO 服务端检查（无服务端检查逻辑，请删除本行注释）
            return true;
        }
        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            //TODO 以下代码由工具生成，请务必检查[base.DetailDS]各属性的赋值是否正确，确认无误后，请删除此行注释

        }

        #endregion
    }
}
