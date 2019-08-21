using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.DAL;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinTabControl;
using SkyCar.Coeus.BLL.IS;
using SkyCar.Coeus.UIModel.IS.QCModel;
using SkyCar.Coeus.UIModel.IS.UIModel;

namespace SkyCar.Coeus.UI.IS
{
    /// <summary>
    /// 汽配库存共享管理
    /// </summary>
    public partial class FrmAutoPartsShareInventoryManager : BaseFormCardList<AutoPartsShareInventoryManagerUIModel, AutoPartsShareInventoryManagerQCModel, MDLPIS_ShareInventory>
    {
        #region 全局变量

        /// <summary>
        /// 汽配库存共享管理BLL
        /// </summary>
        private AutoPartsShareInventoryManagerBLL _bll = new AutoPartsShareInventoryManagerBLL();
        /// <summary>
        /// Tab内Grid绑定用的数据源
        /// </summary>
        private SkyCarBindingList<AutoPartsShareInventoryManagerUIModel, MDLPIS_ShareInventory> _detailGridDS = new SkyCarBindingList<AutoPartsShareInventoryManagerUIModel, MDLPIS_ShareInventory>();

        /// <summary>
        /// 界面属性值发生变化时不予检查的属性列表
        /// </summary>
        List<string> _skipPropertyList = new List<string>();

        #region 下拉框数据源

        /// <summary>
        /// 配件名称
        /// </summary>
        List<MDLBS_AutoPartsName> _autoPartsNameList = new List<MDLBS_AutoPartsName>();
        #endregion

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmAutoPartsShareInventoryManager构造方法
        /// </summary>
        public FrmAutoPartsShareInventoryManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAutoPartsShareInventoryManager_Load(object sender, EventArgs e)
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
            TextBoxTool pageSizeOfListTextBox = null;
            foreach (var loopToolControl in this.toolBarManagerListTabPaging.Tools)
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
            //设置保存可用
            SetActionEnable(SystemActionEnum.Code.SAVE, true);

            #endregion

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            AcceptUIModelChanges();
            _skipPropertyList.Add("OperateType");
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
            //设置保存可用
            SetActionEnable(SystemActionEnum.Code.SAVE, true);
        }
        #endregion

        #region 【列表】Grid相关事件

        /// <summary>
        /// 【列表】Grid单元格改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_CellChange(object sender, CellEventArgs e)
        {
            gdGrid.UpdateData();
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
        #endregion

        #region 【详情】相关事件

        /// <summary>
        /// 【详情】选择配件名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSI_Name_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            //查询配件档案
            FrmAutoPartsArchiveQuery frmAutoPartsArchiveQuery = new FrmAutoPartsArchiveQuery()
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            DialogResult dialogResult = frmAutoPartsArchiveQuery.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            var selectedAutoPartsArchiveList = frmAutoPartsArchiveQuery.SelectedGridList;
            foreach (var loopAutoPartsArchive in selectedAutoPartsArchiveList)
            {
                txtSI_Org_ID.Text = LoginInfoDAX.OrgID;
                txtSI_ThirdNo.Text = loopAutoPartsArchive.APA_ThirdNo;
                txtSI_OEMNo.Text = loopAutoPartsArchive.APA_OEMNo;
                txtSI_Barcode.Text = loopAutoPartsArchive.APA_Barcode;
                txtSI_Name.Text = loopAutoPartsArchive.APA_Name;
                txtSI_Specification.Text = loopAutoPartsArchive.APA_Specification;
                ckSI_PurchasePriceIsVisible.Checked = false;
                ckSI_IsValid.Checked = true;
            }
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
            if (ViewHasChanged(_skipPropertyList)
                || _detailGridDS.InsertList.Count > 0
                || _detailGridDS.UpdateList.Count > 0
                || _detailGridDS.DeleteList.Count > 0)
            {
                //信息尚未保存，确定进行当前操作？
                DialogResult dialogResult = MessageBoxs.Show(Trans.IS, ToString(), MsgHelp.GetMsg(MsgCode.W_0001), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
            }
            #endregion

            //执行基类方法
            base.NewAction();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 当前选中【详情】Tab的场合，初始化【详情】

                //初始化【详情】Tab内控件
                InitializeDetailTabControls();

                //将最新的值Copy到初始UIModel
                SetCardCtrlsToDetailDS();
                this.AcceptUIModelChanges();
                #endregion
            }
            else
            {
                #region 当前选中【列表】Tab的场合，查询配件档案

                FrmAutoPartsArchiveQuery frmAutoPartsArchiveQuery = new FrmAutoPartsArchiveQuery(CustomEnums.CustomeSelectionMode.Multiple)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = frmAutoPartsArchiveQuery.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                List<MDLBS_AutoPartsArchive> selectedAutoPartsArchiveList = new List<MDLBS_AutoPartsArchive>();
                selectedAutoPartsArchiveList = frmAutoPartsArchiveQuery.SelectedGridList;
                //选择的数据中重复的数量
                int repeatCount = 0;
                foreach (var loopAutoPartsArchive in selectedAutoPartsArchiveList)
                {
                    var isRepeat = false;
                    foreach (var loopGrid in _detailGridDS)
                    {
                        if (loopGrid.SI_Barcode == loopAutoPartsArchive.APA_Barcode)
                        {
                            isRepeat = true;
                            repeatCount += 1;
                            continue;
                        }
                    }
                    if (!isRepeat)
                    {
                        AutoPartsShareInventoryManagerUIModel shareInventory = new AutoPartsShareInventoryManagerUIModel
                        {
                            RowID = Guid.NewGuid().ToString(),
                            SI_Org_ID = LoginInfoDAX.OrgID,
                            SI_ThirdNo = loopAutoPartsArchive.APA_ThirdNo,
                            SI_OEMNo = loopAutoPartsArchive.APA_OEMNo,
                            SI_Barcode = loopAutoPartsArchive.APA_Barcode,
                            SI_Name = loopAutoPartsArchive.APA_Name,
                            SI_Specification = loopAutoPartsArchive.APA_Specification,
                            SI_PurchasePriceIsVisible = false,
                            SI_IsValid = true
                        };
                        shareInventory.SI_CreatedBy = shareInventory.SI_UpdatedBy = LoginInfoDAX.UserName;
                        shareInventory.SI_CreatedTime = shareInventory.SI_UpdatedTime = BLLCom.GetCurStdDatetime();
                        _detailGridDS.Insert(0, shareInventory);
                    }
                }
                //4.Grid绑定数据源
                gdGrid.DataSource = _detailGridDS;
                gdGrid.DataBind();
                //5.设置Grid自适应列宽（根据单元格内容）
                gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);

                if (repeatCount > 0)
                {
                    MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { "已过滤" + repeatCount + "条重复的数据！" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public override void SaveAction()
        {
            gdGrid.UpdateData();
            var isExist = false;
            //1.前端检查-保存
            if (!ClientCheckForSave())
            {
                return;
            }
            //2.将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();
            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                foreach (var loopInsertInfo in _detailGridDS)
                {
                    if (loopInsertInfo.RowID == DetailDS.RowID)
                    {
                        loopInsertInfo.SI_Qty = DetailDS.SI_Qty;
                        loopInsertInfo.SI_PurchaseUnitPrice = DetailDS.SI_PurchaseUnitPrice;
                        loopInsertInfo.SI_PriceOfCommonAutoFactory = DetailDS.SI_PriceOfCommonAutoFactory;
                        loopInsertInfo.SI_PriceOfGeneralCustomer = DetailDS.SI_PriceOfGeneralCustomer;
                        loopInsertInfo.SI_PriceOfPlatformAutoFactory = DetailDS.SI_PriceOfPlatformAutoFactory;
                        isExist = true;
                        break;
                    }
                }
                if (!isExist)
                {
                    _detailGridDS.InsertList.Add(DetailDS);
                }
            }
            //3.执行保存（含服务端检查）
            if (!_bll.SaveDetailDS(_detailGridDS))
            {
                MessageBoxs.Show(Trans.IS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //开始监控List变化
            _detailGridDS.StartMonitChanges();
            //将DetailDS数据赋值给【详情】Tab内的对应控件
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
            //2.执行删除
            List<AutoPartsShareInventoryManagerUIModel> deleteShareInventoryList = new List<AutoPartsShareInventoryManagerUIModel>();
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                deleteShareInventoryList = _detailGridDS.Where(p => p.IsChecked == true).ToList();
                deleteShareInventoryList.ForEach(x => x.OperateType = "Delete");
            }
            else
            {
                gdGrid.UpdateData();
                foreach (var loopShareInventory in _detailGridDS)
                {
                    if (loopShareInventory.RowID == txtRowID.Text.Trim())
                    {
                        loopShareInventory.OperateType = "Delete";
                        deleteShareInventoryList.Add(loopShareInventory);
                        break;
                    }
                }
            }
            bool deleteResult = _bll.DeleteDetailDS(deleteShareInventoryList);
            if (!deleteResult)
            {
                //删除失败
                MessageBoxs.Show(Trans.IS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //删除成功
            MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            InitializeDetailTabControls();
            //刷新列表
            RefreshList(true);

            SetCardCtrlsToDetailDS();
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
            base.ConditionDS = new AutoPartsShareInventoryManagerQCModel()
            {
                //SqlId
                SqlId = SQLID.IS_AutoPartsShareInventoryManager_SQL01,
                //配件名称
                WHERE_SI_Name = mcbWhere_SI_Name.SelectedText,
                //第三方编码
                WHERE_SI_ThirdNo = txtWhere_SI_ThirdNo.Text.Trim(),
                //原厂编码
                WHERE_SI_OEMNo = txtWhere_SI_OEMNo.Text.Trim(),
                //配件条码
                WHERE_SI_Barcode = txtWhere_SI_Barcode.Text.Trim(),
                //组织ID
                WHERE_SI_Org_ID = LoginInfoDAX.OrgID,
            };
            //3.执行基类方法（注意：基类使用模糊查询）
            base.QueryAction();
            _bll.CopyModelList(base.GridDS, _detailGridDS);

            //4.Grid绑定数据源
            gdGrid.DataSource = _detailGridDS;
            gdGrid.DataBind();
            //5.设置Grid自适应列宽（根据单元格内容）
            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            //6.开始监控List变化
            _detailGridDS.StartMonitChanges();
            //7.设置【列表】Tab为选中状态
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

        /// <summary>
        /// 关闭画面时检查画面值是否发生变化
        /// </summary>
        /// <returns></returns>
        public override bool ViewHasChangedWhenClose()
        {
            SetCardCtrlsToDetailDS();
            base.NewUIModel = DetailDS;
            if (ViewHasChanged(_skipPropertyList)
                || _detailGridDS.InsertList.Count > 0
                || _detailGridDS.UpdateList.Count > 0
                || _detailGridDS.DeleteList.Count > 0)
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
            //仓库ID
            txtSI_WH_ID.Clear();
            //仓库名称
            txtWH_Name.Clear();
            //仓位ID
            txtSI_WHB_ID.Clear();
            //仓位名称
            txtWHB_Name.Clear();
            //第三方编码
            txtSI_ThirdNo.Clear();
            //原厂编码
            txtSI_OEMNo.Clear();
            //配件条码
            txtSI_Barcode.Clear();
            //配件批次号
            txtSI_BatchNo.Clear();
            //配件名称
            txtSI_Name.Clear();
            //配件规格型号
            txtSI_Specification.Clear();
            //供应商ID
            txtSI_SUPP_ID.Clear();
            //供应商名称
            txtSUPP_Name.Clear();
            //数量
            numSI_Qty.Value = null;
            //采购单价可见
            ckSI_PurchasePriceIsVisible.Checked = false;
            ckSI_PurchasePriceIsVisible.CheckState = CheckState.Unchecked;
            //采购单价
            numSI_PurchaseUnitPrice.Value = null;
            //普通客户销售单价
            numSI_PriceOfGeneralCustomer.Value = null;
            //一般汽修商户销售单价
            numSI_PriceOfCommonAutoFactory.Value = null;
            //平台内汽修商销售单价
            numSI_PriceOfPlatformAutoFactory.Value = null;
            //有效
            ckSI_IsValid.Checked = true;
            ckSI_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtSI_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtSI_CreatedTime.Value = DateTime.Now;
            //修改人
            txtSI_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtSI_UpdatedTime.Value = DateTime.Now;
            //ID
            txtSI_ID.Clear();
            //组织ID
            txtSI_Org_ID.Clear();
            //版本号
            txtSI_VersionNo.Clear();
            txtRowID.Clear();
            //给 仓库ID 设置焦点
            lblWH_Name.Focus();
            #endregion
        }

        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 查询条件初始化
            //第三方编码
            txtWhere_SI_ThirdNo.Clear();
            //原厂编码
            txtWhere_SI_OEMNo.Clear();
            //配件条码
            txtWhere_SI_Barcode.Clear();
            //配件名称
            mcbWhere_SI_Name.Clear();
            #endregion

            #region 初始化下拉框

            //配件名称
            _autoPartsNameList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsName) as List<MDLBS_AutoPartsName>;
            mcbWhere_SI_Name.DisplayMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbWhere_SI_Name.ValueMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_ID;
            mcbWhere_SI_Name.DataSource = _autoPartsNameList;

            #endregion

            #region grid初始化
            _detailGridDS = new SkyCarBindingList<AutoPartsShareInventoryManagerUIModel, MDLPIS_ShareInventory>();
            gdGrid.DataSource = _detailGridDS;
            gdGrid.DataBind();
            _detailGridDS.StartMonitChanges();
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
            if (gdGrid.Rows[activeRowIndex].Cells["RowID"].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells["RowID"].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            DetailDS = _detailGridDS.FirstOrDefault(x => x.RowID == gdGrid.Rows[activeRowIndex].Cells["RowID"].Value);
            if (DetailDS == null || string.IsNullOrEmpty(DetailDS.RowID))
            {
                return;
            }
            if (txtSI_ID.Text != DetailDS.SI_ID
                || (txtSI_ID.Text == DetailDS.SI_ID && txtSI_VersionNo.Text != DetailDS.SI_VersionNo?.ToString()))
            {
                if (txtSI_ID.Text == DetailDS.SI_ID && txtSI_VersionNo.Text != DetailDS.SI_VersionNo?.ToString())
                {
                    //数据版本已过期，将加载最新详情。
                    MessageBoxs.Show(Trans.IS, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.DataHasOverdue }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else if (ViewHasChanged(_skipPropertyList)
                || _detailGridDS.InsertList.Count > 0
                || _detailGridDS.UpdateList.Count > 0
                || _detailGridDS.DeleteList.Count > 0)
                {
                    //将放弃之前的修改，是否继续？
                    DialogResult dialogResult = MessageBoxs.Show(Trans.IS, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.ConfirmGiveUpEdit }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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
            //临时ID
            txtRowID.Text = DetailDS.RowID;
            //仓库ID
            txtSI_WH_ID.Text = DetailDS.SI_WH_ID;
            //仓库名称
            txtWH_Name.Text = DetailDS.WH_Name;
            //仓位ID
            txtSI_WHB_ID.Text = DetailDS.SI_WHB_ID;
            //仓位名称
            txtWHB_Name.Text = DetailDS.WHB_Name;
            //第三方编码
            txtSI_ThirdNo.Text = DetailDS.SI_ThirdNo;
            //原厂编码
            txtSI_OEMNo.Text = DetailDS.SI_OEMNo;
            //配件条码
            txtSI_Barcode.Text = DetailDS.SI_Barcode;
            //配件批次号
            txtSI_BatchNo.Text = DetailDS.SI_BatchNo;
            //配件名称
            txtSI_Name.Text = DetailDS.SI_Name;
            //配件规格型号
            txtSI_Specification.Text = DetailDS.SI_Specification;
            //供应商ID
            txtSI_SUPP_ID.Text = DetailDS.SI_SUPP_ID;
            //供应商名称
            txtSUPP_Name.Text = DetailDS.SUPP_Name;
            //数量
            numSI_Qty.Value = DetailDS.SI_Qty;
            //采购单价可见
            if (DetailDS.SI_PurchasePriceIsVisible != null)
            {
                ckSI_PurchasePriceIsVisible.Checked = DetailDS.SI_PurchasePriceIsVisible.Value;
            }
            //采购单价
            numSI_PurchaseUnitPrice.Value = DetailDS.SI_PurchaseUnitPrice;
            //普通客户销售单价
            numSI_PriceOfGeneralCustomer.Value = DetailDS.SI_PriceOfGeneralCustomer;
            //一般汽修商户销售单价
            numSI_PriceOfCommonAutoFactory.Value = DetailDS.SI_PriceOfCommonAutoFactory;
            //平台内汽修商销售单价
            numSI_PriceOfPlatformAutoFactory.Value = DetailDS.SI_PriceOfPlatformAutoFactory;
            //有效
            if (DetailDS.SI_IsValid != null)
            {
                ckSI_IsValid.Checked = DetailDS.SI_IsValid.Value;
            }
            //创建人
            txtSI_CreatedBy.Text = DetailDS.SI_CreatedBy;
            //创建时间
            dtSI_CreatedTime.Value = DetailDS.SI_CreatedTime;
            //修改人
            txtSI_UpdatedBy.Text = DetailDS.SI_UpdatedBy;
            //修改时间
            dtSI_UpdatedTime.Value = DetailDS.SI_UpdatedTime;
            //ID
            txtSI_ID.Text = DetailDS.SI_ID;
            //组织ID
            txtSI_Org_ID.Text = DetailDS.SI_Org_ID;
            //版本号
            txtSI_VersionNo.Value = DetailDS.SI_VersionNo;
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
            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                //验证配件名称是否为空
                if (string.IsNullOrEmpty(txtSI_Name.Text.Trim()))
                {
                    //配件名称不能为空
                    MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[]
                    {SystemTableColumnEnums.PIS_ShareInventory.Name.SI_Name}), MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    txtSI_Name.Focus();
                    return false;
                }
            }
            else
            {
                if (_detailGridDS.Count == 0)
                {
                    //请至少添加一条共享库存信息
                    MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0014, new object[]
                        { SystemTableEnums.Name.PIS_ShareInventory }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 前端检查-删除
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForDelete()
        {
            //【详情】Tab删除
            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 前端检查-不涉及数据库
                if (string.IsNullOrEmpty(txtSI_ID.Text.Trim()))
                {
                    //共享库存信息为空，不能删除
                    MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0016, new object[] { SystemTableEnums.Name.PIS_ShareInventory, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                #endregion

                //确认删除操作
                DialogResult dialogResult = MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0012),
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return false;
                }
            }
            else
            {
                gdGrid.UpdateData();
                var deleteShareInventoryList = _detailGridDS.Where(p => p.IsChecked == true).ToList();
                if (deleteShareInventoryList.Count == 0)
                {
                    //请勾选至少一条共享库存信息进行删除！
                    MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017,
                            new object[] { SystemTableEnums.Name.PIS_ShareInventory, SystemActionEnum.Name.DELETE }),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //确认删除操作
                DialogResult dialogResult = MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0013,
                    new object[] { deleteShareInventoryList.Count }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            DetailDS = new AutoPartsShareInventoryManagerUIModel()
            {
                //仓库ID
                SI_WH_ID = txtSI_WH_ID.Text.Trim(),
                //仓库名称
                WH_Name = txtWH_Name.Text.Trim(),
                //仓位ID
                SI_WHB_ID = txtSI_WHB_ID.Text.Trim(),
                //仓位名称
                WHB_Name = txtWHB_Name.Text.Trim(),
                //第三方编码
                SI_ThirdNo = txtSI_ThirdNo.Text.Trim(),
                //原厂编码
                SI_OEMNo = txtSI_OEMNo.Text.Trim(),
                //配件条码
                SI_Barcode = txtSI_Barcode.Text.Trim(),
                //配件批次号
                SI_BatchNo = txtSI_BatchNo.Text.Trim(),
                //配件名称
                SI_Name = txtSI_Name.Text.Trim(),
                //配件规格型号
                SI_Specification = txtSI_Specification.Text.Trim(),
                //供应商ID
                SI_SUPP_ID = txtSI_SUPP_ID.Text.Trim(),
                //供应商名称
                SUPP_Name = txtSUPP_Name.Text.Trim(),
                //数量
                SI_Qty = Convert.ToDecimal(numSI_Qty.Value ?? 0),
                //采购单价可见
                SI_PurchasePriceIsVisible = ckSI_PurchasePriceIsVisible.Checked,
                //采购单价
                SI_PurchaseUnitPrice = Convert.ToDecimal(numSI_PurchaseUnitPrice.Value ?? 0),
                //普通客户销售单价
                SI_PriceOfGeneralCustomer = Convert.ToDecimal(numSI_PriceOfGeneralCustomer.Value ?? 0),
                //一般汽修商户销售单价
                SI_PriceOfCommonAutoFactory = Convert.ToDecimal(numSI_PriceOfCommonAutoFactory.Value ?? 0),
                //平台内汽修商销售单价
                SI_PriceOfPlatformAutoFactory = Convert.ToDecimal(numSI_PriceOfPlatformAutoFactory.Value ?? 0),
                //有效
                SI_IsValid = ckSI_IsValid.Checked,
                //创建人
                SI_CreatedBy = txtSI_CreatedBy.Text.Trim(),
                //创建时间
                SI_CreatedTime = (DateTime?)dtSI_CreatedTime.Value ?? DateTime.Now,
                //修改人
                SI_UpdatedBy = txtSI_UpdatedBy.Text.Trim(),
                //修改时间
                SI_UpdatedTime = (DateTime?)dtSI_UpdatedTime.Value ?? DateTime.Now,
                //ID
                SI_ID = txtSI_ID.Text.Trim(),
                //组织ID
                SI_Org_ID = txtSI_Org_ID.Text.Trim(),
                //版本号
                SI_VersionNo = Convert.ToInt64(txtSI_VersionNo.Text.Trim() == "" ? "1" : txtSI_VersionNo.Text.Trim()),
                RowID = txtRowID.Text.Trim(),
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
                    var removeList = _detailGridDS.Where(x => x.IsChecked == true).ToList();
                    foreach (var loopRemove in removeList)
                    {
                        _detailGridDS.Remove(loopRemove);
                    }
                }
                else
                {
                    var curHead = GridDS.FirstOrDefault(x => x.SI_ID == DetailDS.SI_ID);
                    if (curHead != null)
                    {
                        _detailGridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
                {
                    var curHead = _detailGridDS.FirstOrDefault(x => x.SI_ID == DetailDS.SI_ID);
                    if (curHead != null)
                    {
                        _bll.CopyModel(DetailDS, curHead);
                    }
                    else
                    {
                        _detailGridDS.Insert(0, DetailDS);
                    }
                }
            }

            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }
        #endregion

    }
}
