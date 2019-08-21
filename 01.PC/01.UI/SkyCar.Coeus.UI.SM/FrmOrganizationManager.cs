using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.SM;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.SM;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;

namespace SkyCar.Coeus.UI.SM
{
    /// <summary>
    /// 门店管理
    /// </summary>
    public partial class FrmOrganizationManager : BaseFormCardList<OrganizationManagerUIModel, OrganizationManagerQCModel, MDLSM_Organization>
    {
        #region 全局变量

        /// <summary>
        /// 门店管理BLL
        /// </summary>
        private OrganizationManagerBLL _bll = new OrganizationManagerBLL();
        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmOrganizationManager构造方法
        /// </summary>
        public FrmOrganizationManager()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmOrganizationManager_Load(object sender, EventArgs e)
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
        private void gdGrid_DoubleClickCell(object sender, Infragistics.Win.UltraWinGrid.DoubleClickCellEventArgs e)
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

        #region 【详情】相关事件

        /// <summary>
        /// 选择省份
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboOrg_Prov_Code_ValueChanged(object sender, EventArgs e)
        {
            QueryCityListByProv(cboOrg_Prov_Code.Value.ToString());
        }

        /// <summary>
        /// 选择城市
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboOrg_City_Code_ValueChanged(object sender, EventArgs e)
        {
            QueryDistListByCity(cboOrg_City_Code.Value.ToString());
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
                MessageBoxs.Show(Trans.SM, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
            base.ConditionDS = new OrganizationManagerQCModel()
            {
                SqlId = SQLID.SM_OrganizationManager_SQL01,
                //门店编码
                WHERE_Org_Code = txtWhere_Org_Code.Text.Trim(),
                //组织全称
                WHERE_Org_FullName = txtWhere_Org_FullName.Text.Trim(),
                //组织简称
                WHERE_Org_ShortName = txtWhere_Org_ShortName.Text.Trim(),
                //有效
                WHERE_Org_IsValid = ckWhere_Org_IsValid.Checked,
            };
            //3.执行基类方法（注意：基类使用模糊查询）
            base.QueryAction();
            //4.Grid绑定数据源
            gdGrid.DataSource = base.GridDS;
            gdGrid.DataBind();
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

        /// <summary>
        /// 同步组织
        /// </summary>
        public override void SynchronizeAction()
        {
            if (BLLCom.SynchronizeOrgInfo())
            {
                //同步组织信息成功
                MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SYNCHRONIZE + MsgParam.ORGNIZATION + MsgParam.INFORMATION }), MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                //同步组织信息失败
                MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0002, new object[] { SystemActionEnum.Name.SYNCHRONIZE + MsgParam.ORGNIZATION + MsgParam.INFORMATION }), MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
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
            //组织全称
            txtOrg_FullName.Clear();
            //组织简称
            txtOrg_ShortName.Clear();
            //联系人
            txtOrg_Contacter.Clear();
            //固定电话
            txtOrg_TEL.Clear();
            //移动电话
            txtOrg_PhoneNo.Clear();
            //省份Code
            cboOrg_Prov_Code.Clear();
            //城市Code 
            cboOrg_City_Code.Clear();
            //区域Code
            cboOrg_Dist_Code.Clear();
            //地址
            txtOrg_Addr.Clear();
            //经度
            txtOrg_Longitude.Clear();
            //纬度
            txtOrg_Latitude.Clear();
            //标注点显示标题
            txtOrg_MarkerTitle.Clear();
            //标注点显示内容
            txtOrg_MarkerContent.Clear();
            //主营品牌
            txtOrg_MainBrands.Clear();
            //主营产品
            txtOrg_MainProducts.Clear();
            //备注
            txtOrg_Remark.Clear();
            //有效
            ckOrg_IsValid.Checked = true;
            ckOrg_IsValid.CheckState = System.Windows.Forms.CheckState.Checked;
            //创建人
            txtOrg_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtOrg_CreatedTime.Value = BLLCom.GetCurStdDatetime();
            //修改人
            txtOrg_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtOrg_UpdatedTime.Value = BLLCom.GetCurStdDatetime();
            //ID
            txtOrg_ID.Clear();
            //商户ID
            txtOrg_MCT_ID.Clear();
            //平台编码
            txtOrg_PlatformCode.Clear();
            //版本号
            txtOrg_VersionNo.Clear();
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
            //组织全称
            txtWhere_Org_FullName.Clear();
            //组织简称
            txtWhere_Org_ShortName.Clear();
            //有效
            ckWhere_Org_IsValid.Checked = true;
            ckWhere_Org_IsValid.CheckState = System.Windows.Forms.CheckState.Checked;
            //给 门店编码 设置焦点
            lblWhere_Org_Code.Focus();
            #endregion

            //清空Grid
            GridDS = new BindingList<OrganizationManagerUIModel>();
            gdGrid.DataSource = base.GridDS;
            gdGrid.DataBind();

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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SM_Organization.Code.Org_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SM_Organization.Code.Org_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            base.DetailDS = base.GridDS.FirstOrDefault(x => x.Org_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SM_Organization.Code.Org_ID].Value?.ToString());
            if (DetailDS == null || string.IsNullOrEmpty(DetailDS.Org_ID))
            {
                return;
            }

            if (txtOrg_ID.Text != DetailDS.Org_ID
                || (txtOrg_ID.Text == DetailDS.Org_ID && txtOrg_VersionNo.Text != DetailDS.Org_VersionNo?.ToString()))
            {
                if (txtOrg_ID.Text == DetailDS.Org_ID && txtOrg_VersionNo.Text != DetailDS.Org_VersionNo?.ToString())
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
            //门店编码
            txtOrg_Code.Text = base.DetailDS.Org_Code ?? "";
            //组织全称
            txtOrg_FullName.Text = base.DetailDS.Org_FullName ?? "";
            //组织简称
            txtOrg_ShortName.Text = base.DetailDS.Org_ShortName ?? "";
            //联系人
            txtOrg_Contacter.Text = base.DetailDS.Org_Contacter ?? "";
            //固定电话
            txtOrg_TEL.Text = base.DetailDS.Org_TEL ?? "";
            //移动电话
            txtOrg_PhoneNo.Text = base.DetailDS.Org_PhoneNo ?? "";
            //省份Code
            cboOrg_Prov_Code.Value = base.DetailDS.Org_Prov_Code ?? "";
            //城市Code
            cboOrg_City_Code.Value = base.DetailDS.Org_City_Code ?? "";
            //区域Code
            cboOrg_Dist_Code.Value = base.DetailDS.Org_Dist_Code ?? "";
            //地址
            txtOrg_Addr.Text = base.DetailDS.Org_Addr ?? "";
            //经度
            txtOrg_Longitude.Text = base.DetailDS.Org_Longitude ?? "";
            //纬度
            txtOrg_Latitude.Text = base.DetailDS.Org_Latitude ?? "";
            //标注点显示标题
            txtOrg_MarkerTitle.Text = base.DetailDS.Org_MarkerTitle ?? "";
            //标注点显示内容
            txtOrg_MarkerContent.Text = base.DetailDS.Org_MarkerContent ?? "";
            //主营品牌
            txtOrg_MainBrands.Text = base.DetailDS.Org_MainBrands ?? "";
            //主营产品
            txtOrg_MainProducts.Text = base.DetailDS.Org_MainProducts ?? "";
            //备注
            txtOrg_Remark.Text = base.DetailDS.Org_Remark ?? "";
            //有效
            if (base.DetailDS.Org_IsValid != null)
            {
                ckOrg_IsValid.Checked = base.DetailDS.Org_IsValid.Value;
            }
            //创建人
            txtOrg_CreatedBy.Text = base.DetailDS.Org_CreatedBy ?? "";
            //创建时间
            dtOrg_CreatedTime.Value = base.DetailDS.Org_CreatedTime == null ? "" : base.DetailDS.Org_CreatedTime.ToString();
            //修改人
            txtOrg_UpdatedBy.Text = base.DetailDS.Org_UpdatedBy ?? "";
            //修改时间
            dtOrg_UpdatedTime.Value = base.DetailDS.Org_UpdatedTime == null ? "" : base.DetailDS.Org_UpdatedTime.ToString();
            //ID
            txtOrg_ID.Text = base.DetailDS.Org_ID ?? "";
            //商户ID
            txtOrg_MCT_ID.Text = base.DetailDS.Org_MCT_ID ?? "";
            //平台编码
            txtOrg_PlatformCode.Text = base.DetailDS.Org_PlatformCode ?? "";
            //版本号
            txtOrg_VersionNo.Text = base.DetailDS.Org_VersionNo == null ? "" : base.DetailDS.Org_VersionNo.ToString();
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
                foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn loopUltraGridColumn in gdGrid.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (loopUltraGridColumn.IsGroupByColumn)
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
            //组织编码不能为空
            if (string.IsNullOrEmpty(txtOrg_Code.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.SM_Organization.Name.Org_Code }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtOrg_Code.Focus();
                return false;
            }
            //组织全称不能为空
            if (string.IsNullOrEmpty(txtOrg_FullName.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.SM_Organization.Name.Org_FullName }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtOrg_FullName.Focus();
                return false;
            }
            //组织简称不能为空
            if (string.IsNullOrEmpty(txtOrg_ShortName.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.SM_Organization.Name.Org_ShortName }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtOrg_ShortName.Focus();
                return false;
            }

            //判断ID，编码是否存在
            int resultCount = _bll.QueryForObject<int>(SQLID.SM_OrganizationManager_SQL02, new MDLSM_Organization()
            {
                WHERE_Org_Code = txtOrg_Code.Text.Trim(),
                WHERE_Org_ID = txtOrg_ID.Text.Trim()
            });
            if (resultCount > 0)
            {
                MessageBoxs.Show(Trans.SM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0006, new object[] { SystemTableColumnEnums.SM_Organization.Name.Org_Code }), MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                txtOrg_Code.Focus();
                txtOrg_Code.SelectAll();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            base.DetailDS = new OrganizationManagerUIModel()
            {
                //门店编码
                Org_Code = txtOrg_Code.Text.Trim(),
                //组织全称
                Org_FullName = txtOrg_FullName.Text.Trim(),
                //组织简称
                Org_ShortName = txtOrg_ShortName.Text.Trim(),
                //联系人
                Org_Contacter = txtOrg_Contacter.Text.Trim(),
                //固定电话
                Org_TEL = txtOrg_TEL.Text.Trim(),
                //移动电话
                Org_PhoneNo = txtOrg_PhoneNo.Text.Trim(),
                //省份Code
                Org_Prov_Code = cboOrg_Prov_Code.Value?.ToString(),
                //城市Code
                Org_City_Code = cboOrg_City_Code.Value?.ToString(),
                //区域Code
                Org_Dist_Code = cboOrg_Dist_Code.Value?.ToString(),
                //地址
                Org_Addr = txtOrg_Addr.Text.Trim(),
                //经度
                Org_Longitude = txtOrg_Longitude.Text.Trim(),
                //纬度
                Org_Latitude = txtOrg_Latitude.Text.Trim(),
                //标注点显示标题
                Org_MarkerTitle = txtOrg_MarkerTitle.Text.Trim(),
                //标注点显示内容
                Org_MarkerContent = txtOrg_MarkerContent.Text.Trim(),
                //主营品牌
                Org_MainBrands = txtOrg_MainBrands.Text.Trim(),
                //主营产品
                Org_MainProducts = txtOrg_MainProducts.Text.Trim(),
                //备注
                Org_Remark = txtOrg_Remark.Text.Trim(),
                //有效
                Org_IsValid = ckOrg_IsValid.Checked,
                //创建人
                Org_CreatedBy = txtOrg_CreatedBy.Text.Trim(),
                //创建时间
                Org_CreatedTime = (DateTime?)dtOrg_CreatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //修改人
                Org_UpdatedBy = txtOrg_UpdatedBy.Text.Trim(),
                //修改时间
                Org_UpdatedTime = (DateTime?)dtOrg_UpdatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //ID
                Org_ID = txtOrg_ID.Text.Trim(),
                //商户ID
                Org_MCT_ID = txtOrg_MCT_ID.Text.Trim(),
                //平台编码
                Org_PlatformCode = txtOrg_PlatformCode.Text.Trim(),
                //版本号
                Org_VersionNo = Convert.ToInt64(txtOrg_VersionNo.Text.Trim() == "" ? "1" : txtOrg_VersionNo.Text.Trim()),

                //省
                Prov_Name = cboOrg_Prov_Code.Text,
                //市
                City_Name = cboOrg_City_Code.Text,
                //区
                Dist_Name = cboOrg_Dist_Code.Text,
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
            cboOrg_Prov_Code.DisplayMember = SysConst.EN_TEXT;
            cboOrg_Prov_Code.ValueMember = SysConst.Value;
            cboOrg_Prov_Code.DataSource = resultProvList;
            cboOrg_Prov_Code.DataBind();
            cboOrg_Prov_Code.SelectedIndex = 0;
            cboOrg_Prov_Code.DropDown();
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
            cboOrg_City_Code.DisplayMember = SysConst.EN_TEXT;
            cboOrg_City_Code.ValueMember = SysConst.Value;
            cboOrg_City_Code.DataSource = resultCityList;
            cboOrg_City_Code.DataBind();
            cboOrg_City_Code.SelectedIndex = 0;
            cboOrg_City_Code.DropDown();
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
            cboOrg_Dist_Code.DisplayMember = SysConst.EN_TEXT;
            cboOrg_Dist_Code.ValueMember = SysConst.Value;
            cboOrg_Dist_Code.DataSource = resultDistList;
            cboOrg_Dist_Code.DataBind();
            cboOrg_Dist_Code.SelectedIndex = 0;
            cboOrg_Dist_Code.DropDown();
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        private void RefreshList()
        {
            var curHead = GridDS.FirstOrDefault(x => x.Org_ID == DetailDS.Org_ID);
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
