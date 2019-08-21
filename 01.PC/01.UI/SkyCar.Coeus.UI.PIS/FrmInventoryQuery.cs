using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.PIS;
using SkyCar.Coeus.BLL.PIS;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.Common.Enums;
using System.Text.RegularExpressions;
using System.Threading;
using Infragistics.Win.UltraWinTabControl;
using SkyCar.Coeus.Common.CustomControl;
using SkyCar.Coeus.BLL.COM;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.UIModel.Common.APModel;

namespace SkyCar.Coeus.UI.PIS
{
    /// <summary>
    /// 库存查询
    /// </summary>
    public partial class FrmInventoryQuery :
        BaseFormCardListDetail<InventoryQueryUIModel, InventoryQueryQCModel, MDLPIS_Inventory>
    {
        #region 全局变量

        /// <summary>
        /// 库存查询BLL
        /// </summary>
        InventoryQueryBLL _bll = new InventoryQueryBLL();

        /// <summary>
        /// 【列表】配件图片控件列表
        /// </summary>
        List<SkyCarPictureExpand> _pictureExpandList = new List<SkyCarPictureExpand>();

        /// <summary>
        /// 【列表】当前条形码对应的配件图片
        /// </summary>
        List<AutoPartsPictureUIModel> _curAutoPartsPictureList = new List<AutoPartsPictureUIModel>();

        /// <summary>
        /// 【详情】库存明细数据源
        /// </summary>
        List<InventoryQueryDetailUIModel> _detailGridDS = new List<InventoryQueryDetailUIModel>();

        /// <summary>
        /// 【详情】当前选中的库存明细
        /// </summary>
        InventoryQueryDetailUIModel _activeInventoryDetail = new InventoryQueryDetailUIModel();

        /// <summary>
        /// 【详情】库存异动日志数据源
        /// </summary>
        List<InventoryTransLogUIModel> _inventoryTransLogList = new List<InventoryTransLogUIModel>();

        #region 下拉框数据源

        /// <summary>
        /// 组织
        /// </summary>
        List<MDLSM_Organization> _orgList = new List<MDLSM_Organization>();
        /// <summary>
        /// 仓库
        /// </summary>
        List<MDLPIS_Warehouse> _warehouseList = new List<MDLPIS_Warehouse>();
        /// <summary>
        /// 仓位
        /// </summary>
        List<MDLPIS_WarehouseBin> _warehouseBinList = new List<MDLPIS_WarehouseBin>();
        /// <summary>
        /// 配件名称
        /// </summary>
        List<MDLBS_AutoPartsName> _autoPartsNameList = new List<MDLBS_AutoPartsName>();
        /// <summary>
        /// 供应商数据源
        /// </summary>
        List<MDLPIS_Supplier> _supplierList = new List<MDLPIS_Supplier>();

        /// <summary>
        /// 异动类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _inventoryTransList = new List<ComComboBoxDataSourceTC>();

        #endregion

        #region 库存明细翻页相关

        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSizeOfDetail = 40;

        /// <summary>
        /// 库存明细当前页面索引值
        /// </summary>
        private int _inventoryDetailPageIndex = 1;
        /// <summary>
        /// 库存明细总记录条数
        /// </summary>
        private int _inventoryDetailTotalRecordCount = 0;
        /// <summary>
        /// 库存明细总页面数
        /// </summary>
        private int _inventoryDetailTotalPageCount = 0;

        #endregion

        /// <summary>
        /// 是否可编辑配件图片（有保存配件档案权限可编辑，否则不可编辑）
        /// </summary>
        private bool _isCanEditPicture;

        /// <summary>
        /// 当前选中的库存汇总（仅用于配件图片）
        /// </summary>
        private InventoryQueryUIModel _curActiveInventoryTotal;
        #endregion

        #region 公共属性

        /// <summary>
        /// 库存明细翻页ToolBar
        /// </summary>
        public UltraToolbarsManager ToolBarPagingForInvDetail;

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmInventoryQuery构造方法
        /// </summary>
        public FrmInventoryQuery()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmInventoryQuery_Load(object sender, EventArgs e)
        {
            //基类.工具栏（动作，导航）
            base.ToolBarActionAndNavigate = toolBarActionAndNavigate;
            //查询委托（基类控制翻页用）
            base.ExecuteQuery = QueryAction;
            //工具栏（动作）单击事件
            this.toolBarActionAndNavigate.ToolClick += new ToolClickEventHandler(base.toolBarActionAndNavigate_ToolClick);
            //基类.工具栏（翻页）
            base.ToolBarPaging = toolBarManagerListTabPaging;
            //工具栏（翻页）单击事件
            this.toolBarManagerListTabPaging.ToolClick += new ToolClickEventHandler(base.ToolBarPaging_ToolClick);
            //工具栏（翻页）[当前页]值改变事件
            this.toolBarManagerListTabPaging.ToolValueChanged += new ToolEventHandler(base.ToolBarPaging_ToolValueChanged);
            //[库存明细]翻页
            this.ToolBarPagingForInvDetail = ToolbarPagingDetail;
            //[库存明细]翻页单击事件
            this.ToolBarPagingForInvDetail.ToolClick += new ToolClickEventHandler(ToolBarPagingForInvDetail_ToolClick);
            //[库存明细]翻页[当前页]值改变事件
            this.ToolBarPagingForInvDetail.ToolValueChanged += new ToolEventHandler(ToolBarPagingForInvDetail_ToolValueChanged);

            #region 设置页面大小文本框
            TextBoxTool pageSizeOfTotalTextBox = null;
            foreach (var loopToolControl in this.toolBarManagerListTabPaging.Tools)
            {
                if (loopToolControl.Key == SysConst.EN_PAGESIZE)
                {
                    pageSizeOfTotalTextBox = (TextBoxTool)loopToolControl;
                }
            }
            if (pageSizeOfTotalTextBox != null)
            {
                pageSizeOfTotalTextBox.Text = PageSize.ToString();
                pageSizeOfTotalTextBox.AfterToolExitEditMode += PageSizeTextBoxTool_AfterToolExitEditMode;
            }

            TextBoxTool pageSizeOfDetailTextBox = null;
            foreach (var loopToolControl in this.ToolBarPagingForInvDetail.Tools)
            {
                if (loopToolControl.Key == SysConst.EN_PAGESIZE)
                {
                    pageSizeOfDetailTextBox = (TextBoxTool)loopToolControl;
                }
            }
            if (pageSizeOfDetailTextBox != null)
            {
                pageSizeOfDetailTextBox.Text = PageSizeOfDetail.ToString();
                pageSizeOfDetailTextBox.AfterToolExitEditMode += PageSizeTextBoxTool_AfterToolExitEditMode;
            }
            #endregion

            //初始化【列表】Tab内控件
            InitializeListTabControls();
            //初始化【明细】Tab内控件
            InitializeDetailTabControls();
            //初始化【异动】Tab内控件
            InitializeTransLogTabControls();
            //根据选中的Tab，设置动作按钮[是否可用]（在系统权限的基础上进行控制）
            base.SetActionEnableBySelectedTab(SysConst.EN_LIST);

            //选中库存汇总的场合，隐藏[打印条码]
            SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.PRINTBARCODE, false, false);
            //可保存[配件档案]的场合，[保存]可用，否则不可用
            _isCanEditPicture = BLLCom.HasAuthorityInOrg(LoginInfoDAX.OrgID, SystemActionEnum.Code.SAVE,
                MenuActionConst.MenuID.MenuD_3035);
            SetActionEnable(SystemActionEnum.Code.SAVE, _isCanEditPicture);

            //gbInventoryTransLog.Expanded = false;
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
                PageSizeOfDetail = tmpPageSize;
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
            //验证打印次数
            if (e.Cell.Column.Key == "PrintCount")
            {
                if (!BLLCom.IsDecimal(e.Cell.Text))
                {
                    e.Cell.Value = Regex.Replace(e.Cell.Text, @"[^\d]*", "") == string.Empty ? Convert.ToDecimal(0) : e.Cell.Value;
                }
                if (!BLLCom.Validation(RegularFormula.POSITIVE_INTEGER, e.Cell.Text))
                {
                    MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0002, new object[] { MsgParam.POSITIVE_INTEGER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cell.Value = 0;
                }
            }
            gdGrid.UpdateData();
        }

        /// <summary>
        /// 【列表】Grid单击行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_Click(object sender, EventArgs e)
        {
            var curActiveRow = gdGrid.ActiveRow;

            //当前库存汇总
            _curActiveInventoryTotal = HeadGridDS.FirstOrDefault(x => x.INV_Barcode == curActiveRow.Cells[SystemTableColumnEnums.PIS_Inventory.Code.INV_Barcode].Value.ToString());

            //加载配件图片
            LoadAutoPartsPicture(_curActiveInventoryTotal);

            //展开【配件图片】GroupBox
            gbPicture.Expanded = true;
        }

        /// <summary>
        /// 【列表】Grid单击单元格事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_ClickCell(object sender, ClickCellEventArgs e)
        {
            var curActiveRow = gdGrid.Rows[e.Cell.Row.Index];

            //当前库存汇总
            _curActiveInventoryTotal = HeadGridDS.FirstOrDefault(x => x.INV_Barcode == curActiveRow.Cells[SystemTableColumnEnums.PIS_Inventory.Code.INV_Barcode].Value.ToString());

            //加载配件图片
            LoadAutoPartsPicture(_curActiveInventoryTotal);

            //展开【配件图片】GroupBox
            gbPicture.Expanded = true;
        }
        #endregion

        #region Tab页切换事件

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
        /// 选中的Tab改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlFull_SelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                //选中库存汇总的场合，隐藏[打印条码]，[保存]可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.PRINTBARCODE, false, false);
                SetActionEnable(SystemActionEnum.Code.SAVE, _isCanEditPicture);
                //设置图片是否可见、可编辑
                SetPictureControl(_curActiveInventoryTotal);
            }
            else
            {
                //选中库存明细，[打印条码]可用，[保存]不可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.PRINTBARCODE, true, true);
                SetActionEnable(SystemActionEnum.Code.SAVE, false);
            }
            //查询
            SetActionEnable(SystemActionEnum.Code.QUERY, true);
            //清空
            SetActionEnable(SystemActionEnum.Code.CLEAR, true);
            //导出
            SetActionEnable(SystemActionEnum.Code.EXPORT, true);
        }

        #endregion

        #region 查询条件相关事件

        /// <summary>
        /// 组织名称SelectedIndexChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbWhere_Org_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(mcbWhere_Org_Name.SelectedValue))
            {
                return;
            }
            mcbWhere_INV_WH_Name.Clear();
            mcbWhere_INV_WHB_Name.Clear();
            mcbWhere_INV_WHB_Name.DataSource = null;

            #region 初始化下拉框

            var allWarehouseList = CacheDAX.Get(CacheDAX.ConfigDataKey.Warehouse) as List<MDLPIS_Warehouse>;
            mcbWhere_INV_WH_Name.DisplayMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name;
            mcbWhere_INV_WH_Name.ValueMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_ID;
            if (allWarehouseList != null)
            {
                _warehouseList = allWarehouseList.Where(x => x.WH_Org_ID == mcbWhere_Org_Name.SelectedValue).ToList();
                mcbWhere_INV_WH_Name.DataSource = _warehouseList;
            }

            #endregion
        }

        /// <summary>
        /// 仓库SelectedIndexChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbWhere_INV_WH_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            mcbWhere_INV_WHB_Name.Clear();

            #region 初始化下拉框

            //仓位
            _warehouseBinList = CacheDAX.Get(CacheDAX.ConfigDataKey.WarehouseBin) as List<MDLPIS_WarehouseBin>;
            mcbWhere_INV_WHB_Name.DisplayMember = SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name;
            mcbWhere_INV_WHB_Name.ValueMember = SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_ID;
            if (_warehouseBinList != null && !string.IsNullOrEmpty(mcbWhere_INV_WH_Name.SelectedValue))
            {
                var curWarehouseBinList = _warehouseBinList.Where(x => x.WHB_WH_ID == mcbWhere_INV_WH_Name.SelectedValue).ToList();
                mcbWhere_INV_WHB_Name.DataSource = curWarehouseBinList;
            }

            #endregion
        }

        /// <summary>
        /// 第三方编码KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_INV_ThirdNo_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }

        /// <summary>
        /// 原厂编码KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_INV_OEMNo_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }

        /// <summary>
        /// 配件条码KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_INV_Barcode_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }

        /// <summary>
        /// 组织SelectedIndexChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbDetailWhere_Org_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            mcbDetailWhere_INV_WH_Name.Clear();
            mcbDetailWhere_INV_WHB_Name.Clear();
            mcbDetailWhere_INV_WHB_Name.DataSource = null;

            #region 初始化下拉框

            var allWarehouseList = CacheDAX.Get(CacheDAX.ConfigDataKey.Warehouse) as List<MDLPIS_Warehouse>;
            mcbDetailWhere_INV_WH_Name.DisplayMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name;
            mcbDetailWhere_INV_WH_Name.ValueMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_ID;
            if (allWarehouseList != null)
            {
                _warehouseList = allWarehouseList.Where(x => x.WH_Org_ID == mcbDetailWhere_Org_Name.SelectedValue).ToList();
                mcbDetailWhere_INV_WH_Name.DataSource = _warehouseList;
            }

            #endregion
        }

        /// <summary>
        /// 仓库SelectedIndexChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbDetailWhere_INV_WH_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            mcbDetailWhere_INV_WHB_Name.Clear();

            #region 初始化下拉框

            _warehouseBinList = CacheDAX.Get(CacheDAX.ConfigDataKey.WarehouseBin) as List<MDLPIS_WarehouseBin>;
            mcbDetailWhere_INV_WHB_Name.DisplayMember = SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name;
            mcbDetailWhere_INV_WHB_Name.ValueMember = SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_ID;
            if (_warehouseBinList != null && !string.IsNullOrEmpty(mcbDetailWhere_INV_WH_Name.SelectedValue))
            {
                var curWarehouseBinList = _warehouseBinList.Where(x => x.WHB_WH_ID == mcbDetailWhere_INV_WH_Name.SelectedValue).ToList();
                mcbDetailWhere_INV_WHB_Name.DataSource = curWarehouseBinList;
            }

            #endregion
        }

        #endregion

        #region 库存明细相关事件

        /// <summary>
        /// 明细grid_DoubleClick事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetailGrid_DoubleClick(object sender, EventArgs e)
        {
            if (gdDetailGrid.ActiveRow == null)
            {
                return;
            }
            var activeRowIndex = gdDetailGrid.ActiveRow.Index;
            //判断Grid内[唯一标识]是否为空
            if (gdDetailGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_Inventory.Code.INV_ID].Value == null ||
                string.IsNullOrEmpty(gdDetailGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_Inventory.Code.INV_ID].Value.ToString()))
            {
                return;
            }
            _activeInventoryDetail = _detailGridDS.FirstOrDefault(x => x.INV_ID == gdDetailGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_Inventory.Code.INV_ID].Value.ToString());
            if (_activeInventoryDetail != null || (!string.IsNullOrEmpty(_activeInventoryDetail.INV_ID)))
            {
                tabControlFull.Tabs["TransLog"].Selected = true;
                mcbWhere_ITL_Org_Name.SelectedValue = _activeInventoryDetail.INV_Org_ID;
                mcbWhere_ITL_Name.SelectedValue = _activeInventoryDetail.INV_Name;
                mcWhere_ITL_WH_Name.SelectedValue = _activeInventoryDetail.INV_WH_ID;
                txtWhere_ITL_BarcodeAndBatchNo.Text = _activeInventoryDetail.INV_Barcode +
                                                      _activeInventoryDetail.INV_BatchNo;
                txtWhere_ITL_Specification.Text = _activeInventoryDetail.INV_Specification;

            }
            else
            {
                return;
            }
            //查询明细Grid数据并绑定
            QueryInventoryTransLog();

        }
        /// <summary>
        /// 明细grid_DoubleClickCell事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetailGrid_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            if (gdDetailGrid.ActiveRow == null)
            {
                return;
            }
            var activeRowIndex = gdDetailGrid.ActiveRow.Index;
            //判断Grid内[唯一标识]是否为空
            if (gdDetailGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_Inventory.Code.INV_ID].Value == null ||
                string.IsNullOrEmpty(gdDetailGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_Inventory.Code.INV_ID].Value.ToString()))
            {
                return;
            }
            _activeInventoryDetail = _detailGridDS.FirstOrDefault(x => x.INV_ID == gdDetailGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_Inventory.Code.INV_ID].Value.ToString());
            if (_activeInventoryDetail != null || (!string.IsNullOrEmpty(_activeInventoryDetail.INV_ID)))
            {
                tabControlFull.Tabs["TransLog"].Selected = true;
                mcbWhere_ITL_Org_Name.SelectedValue = _activeInventoryDetail.INV_Org_ID;
                mcbWhere_ITL_Name.SelectedValue = _activeInventoryDetail.INV_Name;
                mcWhere_ITL_WH_Name.SelectedValue = _activeInventoryDetail.INV_WH_ID;
                txtWhere_ITL_BarcodeAndBatchNo.Text = _activeInventoryDetail.INV_Barcode +
                                                      _activeInventoryDetail.INV_BatchNo;
                txtWhere_ITL_Specification.Text = _activeInventoryDetail.INV_Specification;

            }
            else
            {
                return;
            }
            //查询明细Grid数据并绑定
            QueryInventoryTransLog();

        }
        #region 库存明细toolbar翻页事件

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ToolBarPagingForInvDetail_ToolClick(object sender, ToolClickEventArgs e)
        {
            var funcName = "ToolBarPagingForDetail_ToolClick";
            LogHelper.WriteBussLogStart(Trans.PIS, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPagingForInvDetail != null)
            {
                switch (e.Tool.Key)
                {
                    //第一页
                    case SysConst.EN_FIRSTPAGE:
                        ((TextBoxTool)(ToolBarPagingForInvDetail.Tools[SysConst.EN_PAGEINDEX])).Text =
                            SysConst.NUMBER_ONE.ToString();
                        break;
                    // 前一页
                    case SysConst.EN_FORWARDPAGE:
                        ((TextBoxTool)(ToolBarPagingForInvDetail.Tools[SysConst.EN_PAGEINDEX])).Text = (_inventoryDetailPageIndex - 1).ToString();
                        break;
                    // 下一页
                    case SysConst.EN_NEXTPAGE:
                        ((TextBoxTool)(ToolBarPagingForInvDetail.Tools[SysConst.EN_PAGEINDEX])).Text = (_inventoryDetailPageIndex + 1).ToString();
                        break;
                    // 最后一页
                    case SysConst.EN_LASTPAGE:
                        ((TextBoxTool)(ToolBarPagingForInvDetail.Tools[SysConst.EN_PAGEINDEX])).Text = _inventoryDetailTotalPageCount.ToString();
                        break;
                }
            }
            LogHelper.WriteBussLogEndOK(Trans.PIS, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 翻页ToolBar的值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ToolBarPagingForInvDetail_ToolValueChanged(object sender, ToolEventArgs e)
        {
            var funcName = "ToolBarPagingForDetail_ToolValueChanged";
            LogHelper.WriteBussLogStart(Trans.PIS, LoginInfoDAX.UserName, funcName, "", "", null);
            if (SysConst.EN_PAGEINDEX.Equals(e.Tool.Key))
            {
                string strValue = ((TextBoxTool)(e.Tool)).Text.Trim();
                int tmpPageIndex = 0;
                if (!int.TryParse(strValue, out tmpPageIndex) && tmpPageIndex <= 0)
                {
                    ((TextBoxTool)(e.Tool)).Text = SysConst.NUMBER_ONE.ToString();
                    ((TextBoxTool)(e.Tool)).SelectionLength = 1;
                    return;
                }
                else if (tmpPageIndex > _inventoryDetailTotalPageCount)
                {
                    ((TextBoxTool)(e.Tool)).Text = _inventoryDetailTotalPageCount.ToString();
                    ((TextBoxTool)(e.Tool)).SelectionLength = _inventoryDetailTotalPageCount.ToString().Length;
                    return;
                }

                #region
                if (Convert.ToInt32(strValue) <= 0)
                {
                    ((TextBoxTool)(e.Tool)).Text = SysConst.NUMBER_ONE.ToString();
                }
                else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) >= _inventoryDetailTotalPageCount)
                {
                    ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(_inventoryDetailTotalPageCount).ToString();
                }
                else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) < _inventoryDetailTotalPageCount)
                {
                    ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(strValue).ToString();
                }
                #endregion

                _inventoryDetailPageIndex = tmpPageIndex;
                if (tmpPageIndex <= 0)
                {
                    _inventoryDetailPageIndex = 1;
                }
                ExecuteQuery?.Invoke();
            }
            LogHelper.WriteBussLogEndOK(Trans.PIS, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        #endregion

        #endregion

        #region 配件图片相关事件

        /// <summary>
        /// 配件图片ToolClick事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolbarsAutoPartsPicture_ToolClick(object sender, ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case SysConst.Upload:
                    //批量上传图片
                    BatchUploadPicture();
                    break;
                case SysConst.Export:
                    //批量导出图片
                    BatchExportPicture();
                    break;
                case SysConst.EN_DEL:
                    //批量删除图片
                    BatchDeletePicture();
                    break;
            }
        }

        /// <summary>
        /// 全选Checked改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckSelectAllPicture_CheckedChanged(object sender, EventArgs e)
        {
            SelectAllPicture();
        }
        /// <summary>
        /// 异动【组织】_SelectedIndexChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbWhere_ITL_Org_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            mcWhere_ITL_WH_Name.Clear();
            mcWhere_ITL_WH_Name.Clear();
            mcWhere_ITL_WH_Name.DataSource = null;

            #region 初始化下拉框

            var allWarehouseList = CacheDAX.Get(CacheDAX.ConfigDataKey.Warehouse) as List<MDLPIS_Warehouse>;
            mcWhere_ITL_WH_Name.DisplayMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name;
            mcWhere_ITL_WH_Name.ValueMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_ID;
            if (allWarehouseList != null)
            {
                _warehouseList = allWarehouseList.Where(x => x.WH_Org_ID == mcbDetailWhere_Org_Name.SelectedValue).ToList();
                mcWhere_ITL_WH_Name.DataSource = _warehouseList;
            }

            #endregion
        }
        #endregion

        #endregion

        #region 重写基类方法

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
            //3.执行保存（含服务端检查）
            bool saveResult = _bll.SaveAutoPartsPicture(_curAutoPartsPictureList);
            if (!saveResult)
            {
                MessageBoxs.Show(Trans.PIS, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //为图片控件设置传入的配件图片Model
            foreach (var loopPicture in _curAutoPartsPictureList)
            {
                //清空图片的来源类型
                loopPicture.SourceFilePath = null;

                var curPictureExpand = _pictureExpandList.FirstOrDefault(x => x.PictureKey == loopPicture.INVP_PictureName);
                if (curPictureExpand != null)
                {
                    curPictureExpand.PropertyModel = loopPicture;
                }
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                base.ConditionDS = new InventoryQueryQCModel()
                {
                    PageSize = PageSize,
                    PageIndex = PageIndex,
                    SqlId = SQLID.PIS_InventoryQuery_SQL_03,
                    //配件编码
                    WHERE_AutoPartsCode = txtWhere_AutoPartsCode.Text.Trim(),
                    //其他描述
                    WHERE_OtherDesc = txtWhere_OtherDesc.Text.Trim(),
                    //配件条码+批次号
                    WHERE_BarcodeAndBatchNo = txtWhere_BarcodeAndBatchNo.Text.Trim(),
                    //配件名称
                    WHERE_INV_Name = mcbWhere_INV_Name.SelectedText,
                    //供应商名称
                    WHERE_INV_SUPP_Name = mcbWhere_SUPP_Name.SelectedText,
                    //仓库ID
                    WHERE_INV_WH_ID = mcbWhere_INV_WH_Name.SelectedValue,
                    //仓位ID
                    WHERE_INV_WHB_ID = mcbWhere_INV_WHB_Name.SelectedValue,
                    //组织ID
                    WHERE_INV_Org_ID = string.IsNullOrEmpty(mcbWhere_Org_Name.SelectedValue) ? LoginInfoDAX.OrgID : mcbWhere_Org_Name.SelectedValue,
                };
                base.QueryAction();
                gdGrid.DataSource = HeadGridDS;
                gdGrid.DataBind();
                //设置Grid自适应列宽（根据单元格内容）
                gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            }
            else if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                //查询库存明细列表
                QueryInventoryDetail();
            }
            else
            {
                //查询库存异动日志列表
                QueryInventoryTransLog();
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public override void ClearAction()
        {
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                //初始化【列表】Tab内控件
                InitializeListTabControls();
            }
            else if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                //初始化【明细】Tab内控件
                InitializeDetailTabControls();
            }
            else
            {
                //初始化【异动】Tab内控件
                InitializeTransLogTabControls();
            }
        }

        /// <summary>
        /// 打印条码
        /// </summary>
        public override void PrintBarCodeNavigate()
        {
            base.PrintBarCodeNavigate();
            PrintBarcode();
        }

        /// <summary>
        /// 导出当前页
        /// </summary>
        /// <param name="paramGrid"></param>
        /// <param name="paramGridName"></param>
        public override void ExportAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = SystemTableEnums.Name.PIS_Inventory;
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                base.ExportAction(gdGrid, paramGridName);
            }
            else
            {
                base.ExportAction(gdDetailGrid, paramGridName);
            }
        }

        /// <summary>
        /// 导出全部
        /// </summary>
        /// <param name="paramGrid"></param>
        /// <param name="paramGridName"></param>
        public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = SystemTableEnums.Name.PIS_Inventory;
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                List<InventoryQueryUIModel> resultAllList = new List<InventoryQueryUIModel>();
                _bll.QueryForList<InventoryQueryUIModel>(SQLID.PIS_InventoryQuery_SQL_03, new InventoryQueryQCModel()
                {
                    PageIndex = 1,
                    PageSize = null,
                    //配件编码
                    WHERE_AutoPartsCode = txtWhere_AutoPartsCode.Text.Trim(),
                    //其他描述
                    WHERE_OtherDesc = txtWhere_OtherDesc.Text.Trim(),
                    //配件条码+批次号
                    WHERE_BarcodeAndBatchNo = txtWhere_BarcodeAndBatchNo.Text.Trim(),
                    //配件名称
                    WHERE_INV_Name = mcbWhere_INV_Name.SelectedText,
                    //供应商名称
                    WHERE_INV_SUPP_Name = mcbWhere_SUPP_Name.SelectedText,
                    //仓库ID
                    WHERE_INV_WH_ID = mcbWhere_INV_WH_Name.SelectedValue,
                    //仓位ID
                    WHERE_INV_WHB_ID = mcbWhere_INV_WHB_Name.SelectedValue,
                    //组织ID
                    WHERE_INV_Org_ID = string.IsNullOrEmpty(mcbWhere_Org_Name.SelectedValue) ? LoginInfoDAX.OrgID : mcbWhere_Org_Name.SelectedValue,
                }, resultAllList);

                UltraGrid allGrid = gdGrid;
                allGrid.DataSource = resultAllList;
                allGrid.DataBind();
                base.ExportAllAction(allGrid, paramGridName);
                gdGrid.DataSource = HeadGridDS;
                gdGrid.DataBind();
            }
            else
            {
                if (dtINV_CreatedTimeStart.Value != null)
                {
                    //创建时间-开始
                    ConditionDS._CreatedTimeStart = dtINV_CreatedTimeStart.DateTime;
                }
                if (dtINV_CreatedTimeEnd.Value != null)
                {
                    //创建时间-终了
                    ConditionDS._CreatedTimeEnd = dtINV_CreatedTimeEnd.DateTime;
                }
                List<InventoryQueryDetailUIModel> resultAllList = new List<InventoryQueryDetailUIModel>();
                _bll.QueryForList<InventoryQueryDetailUIModel>(SQLID.PIS_InventoryQuery_SQL_01, new InventoryQueryQCModel()
                {
                    PageSize = PageSize,
                    PageIndex = PageIndex,
                    //配件编码
                    WHERE_AutoPartsCode = txtDetailWhere_AutoPartsCode.Text.Trim(),
                    //其他描述
                    WHERE_OtherDesc = txtDetailWhere_OtherDesc.Text.Trim(),
                    //配件条码+批次号
                    WHERE_BarcodeAndBatchNo = txtDetailWhere_BarcodeAndBatchNo.Text.Trim(),
                    //配件名称
                    WHERE_INV_Name = mcbDetailWhere_INV_Name.SelectedText,
                    //供应商名称
                    WHERE_INV_SUPP_Name = mcbDetailWhere_SUPP_Name.SelectedText,
                    //仓库名称
                    WHERE_INV_WH_Name = mcbDetailWhere_INV_WH_Name.SelectedText,
                    //仓位名称
                    WHERE_INV_WHB_Name = mcbDetailWhere_INV_WHB_Name.SelectedText,
                    //零库存
                    WHERE_INV_IsZero = ckIsZero.Checked,
                    //组织ID
                    WHERE_INV_Org_ID = string.IsNullOrEmpty(mcbDetailWhere_Org_Name.SelectedValue) ? LoginInfoDAX.OrgID : mcbDetailWhere_Org_Name.SelectedValue,
                }, resultAllList);

                UltraGrid allGrid = gdDetailGrid;
                allGrid.DataSource = resultAllList;
                allGrid.DataBind();
                base.ExportAllAction(allGrid, paramGridName);
                gdDetailGrid.DataSource = _detailGridDS;
                gdDetailGrid.DataBind();
            }
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 查询条件初始化

            //仓库
            mcbWhere_INV_WH_Name.Clear();
            //仓位
            mcbWhere_INV_WHB_Name.Clear();
            //第三方编码
            txtWhere_AutoPartsCode.Clear();
            //原厂编码
            txtWhere_OtherDesc.Clear();
            //配件条码
            txtWhere_BarcodeAndBatchNo.Clear();
            //配件名称
            mcbWhere_INV_Name.Clear();
            //供应商名称
            mcbWhere_SUPP_Name.Clear();
            //组织名称
            mcbWhere_Org_Name.Clear();
            //给 第三方编码 设置焦点
            lblWhere_AutoPartsCode.Focus();

            #endregion

            #region Grid初始化

            HeadGridDS = new BindingList<InventoryQueryUIModel>();
            gdGrid.DataSource = HeadGridDS;
            gdGrid.DataBind();

            #endregion

            #region 初始化下拉框

            //组织下拉框
            _orgList = LoginInfoDAX.OrgList;
            mcbWhere_Org_Name.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
            mcbWhere_Org_Name.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
            mcbWhere_Org_Name.DataSource = _orgList;

            //配件名称下拉框
            _autoPartsNameList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsName) as List<MDLBS_AutoPartsName>;
            mcbWhere_INV_Name.DisplayMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbWhere_INV_Name.ValueMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbWhere_INV_Name.DataSource = _autoPartsNameList;

            //供应商
            _supplierList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsSupplier) as List<MDLPIS_Supplier>;
            mcbWhere_SUPP_Name.DisplayMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_Name;
            mcbWhere_SUPP_Name.ValueMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_ID;
            mcbWhere_SUPP_Name.DataSource = _supplierList;

            #endregion

            //默认组织为当前组织
            mcbWhere_Org_Name.SelectedValue = LoginInfoDAX.OrgID;

            //初始化图片
            _latestBarcode = string.Empty;
            _curAutoPartsPictureList = new List<AutoPartsPictureUIModel>();
            flowLayoutPanelPicture.Controls.Clear();
            _pictureExpandList.Clear();
        }

        /// <summary>
        /// 初始化【明细】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 查询条件初始化

            //仓库
            mcbDetailWhere_INV_WH_Name.Clear();
            //仓位
            mcbDetailWhere_INV_WHB_Name.Clear();
            //配件编码
            txtDetailWhere_AutoPartsCode.Clear();
            //其他描述
            txtDetailWhere_OtherDesc.Clear();
            //配件条码
            txtDetailWhere_BarcodeAndBatchNo.Clear();
            //配件名称
            mcbDetailWhere_INV_Name.Clear();
            //供应商名称
            mcbDetailWhere_SUPP_Name.Clear();
            //组织名称
            mcbDetailWhere_Org_Name.Clear();
            //0库存
            ckIsZero.Checked = false;
            //创建时间-开始
            dtINV_CreatedTimeStart.Value = null;
            //创建时间-终了
            dtINV_CreatedTimeEnd.Value = null;
            //给 第三方编码 设置焦点
            txtDetailWhere_AutoPartsCode.Focus();

            #endregion

            #region Grid初始化

            //库存明细
            _detailGridDS = new List<InventoryQueryDetailUIModel>();
            gdDetailGrid.DataSource = _detailGridDS;
            gdDetailGrid.DataBind();

            //库存异动
            _inventoryTransLogList = new List<InventoryTransLogUIModel>();
            gdTransLog.DataSource = _inventoryTransLogList;
            gdTransLog.DataBind();
            #endregion

            #region 初始化下拉框

            //组织下拉框
            _orgList = LoginInfoDAX.OrgList;
            mcbDetailWhere_Org_Name.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
            mcbDetailWhere_Org_Name.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
            mcbDetailWhere_Org_Name.DataSource = _orgList;

            //配件名称下拉框
            mcbDetailWhere_INV_Name.DisplayMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbDetailWhere_INV_Name.ValueMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbDetailWhere_INV_Name.DataSource = _autoPartsNameList;

            //供应商
            mcbDetailWhere_SUPP_Name.DisplayMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_Name;
            mcbDetailWhere_SUPP_Name.ValueMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_ID;
            mcbDetailWhere_SUPP_Name.DataSource = _supplierList;

            #endregion

            //默认组织为当前组织
            mcbDetailWhere_Org_Name.SelectedValue = LoginInfoDAX.OrgID;
        }

        /// <summary>
        /// 初始化【异动】Tab内控件
        /// </summary>
        private void InitializeTransLogTabControls()
        {
            #region 查询条件初始化

            mcbWhere_ITL_Org_Name.Clear();
            txtWhere_ITL_BusinessNo.Clear();
            mcbWhere_ITL_Name.Clear();
            cbWhere_ITL_TransType.Clear();
            mcWhere_ITL_WH_Name.Clear();
            txtWhere_ITL_BarcodeAndBatchNo.Clear();
            txtWhere_ITL_Specification.Clear();
            dtITL_CreatedTimeStart.Value = null;
            dtITL_CreatedTimeEnd.Value = null;
            #endregion

            #region Grid初始化

            //库存异动
            _inventoryTransLogList = new List<InventoryTransLogUIModel>();
            gdTransLog.DataSource = _inventoryTransLogList;
            gdTransLog.DataBind();
            #endregion

            #region 初始化下拉框

            //组织下拉框
            _orgList = LoginInfoDAX.OrgList;
            mcbWhere_ITL_Org_Name.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
            mcbWhere_ITL_Org_Name.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
            mcbWhere_ITL_Org_Name.DataSource = _orgList;

            //配件名称下拉框
            mcbWhere_ITL_Name.DisplayMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbWhere_ITL_Name.ValueMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbWhere_ITL_Name.DataSource = _autoPartsNameList;

            var allWarehouseList = CacheDAX.Get(CacheDAX.ConfigDataKey.Warehouse) as List<MDLPIS_Warehouse>;
            mcWhere_ITL_WH_Name.DisplayMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name;
            mcWhere_ITL_WH_Name.ValueMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_ID;
            if (allWarehouseList != null)
            {
                _warehouseList = allWarehouseList.Where(x => x.WH_Org_ID == mcbWhere_ITL_Org_Name.SelectedValue).ToList();
                mcWhere_ITL_WH_Name.DataSource = _warehouseList;
            }

            _inventoryTransList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.InventoryTransType);
            cbWhere_ITL_TransType.DisplayMember = SysConst.EN_TEXT;
            cbWhere_ITL_TransType.ValueMember = SysConst.EN_Code;
            cbWhere_ITL_TransType.DataSource = _inventoryTransList;
            cbWhere_ITL_TransType.DataBind();

            #endregion

            //默认组织为当前组织
            mcbWhere_ITL_Org_Name.SelectedValue = LoginInfoDAX.OrgID;
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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_Inventory.Code.INV_Barcode].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_Inventory.Code.INV_Barcode].Value.ToString())
                || gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_Inventory.Code.INV_Org_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_Inventory.Code.INV_Org_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[HeadDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            HeadDS = HeadGridDS.FirstOrDefault(x => x.INV_Barcode == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_Inventory.Code.INV_Barcode].Value && x.INV_Org_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.PIS_Inventory.Code.INV_Org_ID].Value);
            if (HeadDS == null || string.IsNullOrEmpty(HeadDS.INV_Barcode) || string.IsNullOrEmpty(HeadDS.INV_Org_ID))
            {
                return;
            }
            #region 给详情页面控件赋值
            
            mcbDetailWhere_Org_Name.SelectedValue = HeadDS.INV_Org_ID;
            mcbDetailWhere_INV_WH_Name.SelectedValue = HeadDS.INV_WH_ID;
            mcbDetailWhere_INV_Name.SelectedValue = HeadDS.INV_Name;
            txtDetailWhere_BarcodeAndBatchNo.Text = HeadDS.INV_Barcode;
            ckIsZero.Checked = false;

            #endregion

            //查询库存明细列表
            QueryInventoryDetail();

            //选中【详情】Tab
            tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;
        }

        /// <summary>
        /// 查询库存明细列表
        /// </summary>
        private void QueryInventoryDetail()
        {
            var argsInventoryQuery = new InventoryQueryQCModel
            {
                PageSize = PageSizeOfDetail,
                PageIndex = _inventoryDetailPageIndex,
                //配件编码
                WHERE_AutoPartsCode = txtDetailWhere_AutoPartsCode.Text.Trim(),
                //其他描述
                WHERE_OtherDesc = txtDetailWhere_OtherDesc.Text.Trim(),
                //配件条码+批次号
                WHERE_BarcodeAndBatchNo = txtDetailWhere_BarcodeAndBatchNo.Text.Trim(),
                //配件名称
                WHERE_INV_Name = mcbDetailWhere_INV_Name.SelectedText,
                //供应商名称
                WHERE_INV_SUPP_Name = mcbDetailWhere_SUPP_Name.SelectedText,
                //仓库名称
                WHERE_INV_WH_Name = mcbDetailWhere_INV_WH_Name.SelectedText,
                //仓位名称
                WHERE_INV_WHB_Name = mcbDetailWhere_INV_WHB_Name.SelectedText,
                //零库存
                WHERE_INV_IsZero = ckIsZero.Checked,
                //组织ID
                WHERE_INV_Org_ID = string.IsNullOrEmpty(mcbDetailWhere_Org_Name.SelectedValue) ? LoginInfoDAX.OrgID : mcbDetailWhere_Org_Name.SelectedValue,
            };
            if (dtINV_CreatedTimeStart.Value != null)
            {
                //创建时间-开始
                argsInventoryQuery._CreatedTimeStart = dtINV_CreatedTimeStart.DateTime;
            }
            if (dtINV_CreatedTimeEnd.Value != null)
            {
                //创建时间-终了
                argsInventoryQuery._CreatedTimeEnd = dtINV_CreatedTimeEnd.DateTime;
            }
            _bll.QueryForList(SQLID.PIS_InventoryQuery_SQL_01, argsInventoryQuery, _detailGridDS);
            if (_detailGridDS != null)
            {
                gdDetailGrid.DataSource = _detailGridDS;
                gdDetailGrid.DataBind();
                //设置Grid自适应列宽（根据单元格内容）
                gdDetailGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
                int totalRecordCount = 0;
                //设置分页控件
                if (_detailGridDS.Count > 0)
                {
                    totalRecordCount = _detailGridDS[0].RecordCount ?? 0;
                }
                SetBarPagingForInventoryDetail(totalRecordCount);
            }
        }

        /// <summary>
        /// 查询库存异动日志列表
        /// </summary>
        private void QueryInventoryTransLog()
        {
            //1.设置查询条件
            var argsCondition = new InventoryTransLogQCModel()
            {
                SqlId = SQLID.PIS_InventoryQuery_SQL_02,
                WHERE_ITL_Org_ID = mcbWhere_ITL_Org_Name.SelectedValue,
                WHERE_ITL_BusinessNo = txtWhere_ITL_BusinessNo.Text,
                WHERE_ITL_Name = mcbWhere_ITL_Name.SelectedText,
                WHERE_ITL_TransType = cbWhere_ITL_TransType.Text,
                WHERE_ITL_WH_ID = mcWhere_ITL_WH_Name.SelectedValue,
                WHERE_ITL_BatchNo = txtWhere_ITL_BarcodeAndBatchNo.Text,
                WHERE_ITL_Specification = txtWhere_ITL_Specification.Text,
                //WHERE_ITL_Barcode = _activeInventoryDetail.INV_Barcode,
                //WHERE_ITL_BatchNo = _activeInventoryDetail.INV_BatchNo,
                //WHERE_ITL_WH_ID = _activeInventoryDetail.INV_WH_ID,
            };
            if (dtITL_CreatedTimeStart.Value != null)
            {
                //创建时间-开始
                argsCondition._CreatedTimeStart = dtITL_CreatedTimeStart.DateTime;
            }
            if (dtITL_CreatedTimeEnd.Value != null)
            {
                //创建时间-终了
                argsCondition._CreatedTimeEnd = dtITL_CreatedTimeEnd.DateTime;
            }
            //2.执行查询方法
            _bll.QueryForList(argsCondition.SqlId, argsCondition, _inventoryTransLogList);
            //4.Grid绑定数据源
            gdTransLog.DataSource = _inventoryTransLogList;
            gdTransLog.DataBind();
            //5.设置Grid自适应列宽（根据单元格内容）
            gdTransLog.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }

        /// <summary>
        /// 是否允许将【列表】Grid数据设置到【详情】Tab内的对应控件
        /// </summary>
        /// <returns>true:允许；false：不允许</returns>
        private bool IsAllowSetGridDataToCard()
        {
            if (gdGrid.ActiveRow == null || gdGrid.ActiveRow.Index < 0)
            {
                return false;
            }
            if (gdGrid.DisplayLayout.Bands[0].SortedColumns.Count > 0)
            {
                foreach (UltraGridColumn loopColumn in gdGrid.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (loopColumn.IsGroupByColumn)
                    {
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
            if (_curAutoPartsPictureList.Count == 0)
            {
                //请上传至少一张配件图片
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0032, new object[] { MsgParam.ATLEAST_APIECE + MsgParam.INVENTORY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 打印条码
        /// </summary>
        public void PrintBarcode()
        {
            try
            {
                List<InventoryQueryDetailUIModel> tempPrintBarcodeList = new List<InventoryQueryDetailUIModel>();
                if (HeadGridDS != null)
                {
                    foreach (var loopInventory in _detailGridDS)
                    {
                        if (loopInventory.IsChecked
                            && !string.IsNullOrEmpty(loopInventory.INV_BatchNo)
                            && loopInventory.PrintCount > 0)
                        {
                            tempPrintBarcodeList.Add(loopInventory);
                        }
                    }
                }
                if (tempPrintBarcodeList.Count == 0)
                {
                    MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0034), MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                #region 提醒打印次数超过50的条码

                StringBuilder remindContentSB = new StringBuilder();
                foreach (var loopPrintItem in tempPrintBarcodeList)
                {
                    if (loopPrintItem.PrintCount > 50)
                    {
                        remindContentSB.AppendLine(loopPrintItem.INV_Name + "：" + loopPrintItem.PrintCount +
                                                   MsgParam.NUMBER_TIMES);
                    }
                }
                if (remindContentSB.ToString().Length > 0)
                {
                    string remindContent = MsgHelp.GetMsg(MsgCode.I_0005) + remindContentSB;
                    remindContent += MsgHelp.GetMsg(MsgCode.I_0006);
                    DialogResult confirmPrintBarcode = MessageBoxs.Show(Trans.PIS, ToString(), remindContent,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirmPrintBarcode == DialogResult.No)
                    {
                        return;
                    }
                }

                #endregion

                #region 检查打印机连接

                byte[] pbuf = new byte[128];
                Encoding encAscIi = Encoding.ASCII;
                var nLen = BarcodePrinterHelper.B_GetUSBBufferLen() + 1;
                //if (nLen > 1)
                //{
                int len1 = 128, len2 = 128;
                var buf1 = new byte[len1];
                var buf2 = new byte[len2];
                BarcodePrinterHelper.B_EnumUSB(pbuf);
                BarcodePrinterHelper.B_GetUSBDeviceInfo(1, buf1, out len1, buf2, out len2);
                var ret = BarcodePrinterHelper.B_CreatePrn(12, encAscIi.GetString(buf2, 0, len2));
                if (0 != ret)
                {
                    MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0035), MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                //}

                #endregion

                foreach (var loopPrintItem in tempPrintBarcodeList)
                {
                    string inspire = loopPrintItem.APA_VehicleInspire;
                    if (!string.IsNullOrEmpty(loopPrintItem.APA_VehicleInspire) &&
                        loopPrintItem.APA_VehicleInspire.Trim(';').Length > 16)
                    {
                        inspire = loopPrintItem.APA_VehicleInspire.Substring(0, Math.Min(18, loopPrintItem.APA_VehicleInspire.Length)) + "...";
                    }
                    string spec = loopPrintItem.INV_Specification;
                    if (!string.IsNullOrEmpty(loopPrintItem.INV_Specification) &&
                        loopPrintItem.INV_Specification.Trim(';').Length > 16)
                    {
                        spec = loopPrintItem.INV_Specification.Substring(0, Math.Min(18, loopPrintItem.INV_Specification.Length)) + "...";
                    }
                    if (loopPrintItem.PrintCount > 0)
                    {
                        BarcodePrinterHelper.B_Prn_Text_TrueType(10, 10, 35, "微软雅黑", 1, 800, 0, 0, 0, "A2",
                        loopPrintItem.INV_Name ?? string.Empty);
                        BarcodePrinterHelper.B_Prn_Text_TrueType(10, 50, 35, "微软雅黑", 1, 800, 0, 0, 0, "A3",
                            inspire ?? string.Empty);
                        BarcodePrinterHelper.B_Prn_Text_TrueType(10, 90, 35, "微软雅黑", 1, 800, 0, 0, 0, "A4",
                            spec ?? string.Empty);
                        BarcodePrinterHelper.B_Prn_Barcode(30, 130, 0, "1", 2, 8, 110, 'B',
                            (loopPrintItem.INV_Barcode ?? string.Empty) + (loopPrintItem.INV_Barcode ?? string.Empty));
                        BarcodePrinterHelper.B_Print_Out(loopPrintItem.PrintCount);
                    }
                }
                BarcodePrinterHelper.B_ClosePrn();
            }
            catch (Exception ex)
            {
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0002, new object[] { SystemActionEnum.Name.PRINT }) + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        #region 配件图片相关方法

        private string _latestBarcode = string.Empty;
        /// <summary>
        /// 加载配件图片
        /// </summary>
        private void LoadAutoPartsPicture(InventoryQueryUIModel paramInventoryTotal)
        {
            if (paramInventoryTotal == null
                || string.IsNullOrEmpty(paramInventoryTotal.INV_Barcode)
                || paramInventoryTotal.INV_Barcode == _latestBarcode)
            {
                return;
            }
            _latestBarcode = paramInventoryTotal.INV_Barcode;

            //查询当前条形码对应配件图片
            List<MDLPIS_InventoryPicture> resultAutoPartsPictureList = new List<MDLPIS_InventoryPicture>();
            _bll.QueryForList(SQLID.COMM_SQL49, new MDLPIS_InventoryPicture
            {
                WHERE_INVP_Barcode = paramInventoryTotal.INV_Barcode + SysConst.Semicolon_DBC,
            }, resultAutoPartsPictureList);
            _bll.CopyModelList(resultAutoPartsPictureList, _curAutoPartsPictureList);

            flowLayoutPanelPicture.Controls.Clear();
            _pictureExpandList.Clear();

            if (_curAutoPartsPictureList.Count == 0)
            {
                //配件无图片时，加载一个扩展的图片控件
                //添加空图片控件
                AddNullPictureControl();
            }
            else
            {
                //配件有图片时，加载实际数量的扩展的图片控件以及图片
                foreach (var loopPicture in _curAutoPartsPictureList)
                {
                    if (string.IsNullOrEmpty(loopPicture.INVP_ID)
                        || string.IsNullOrEmpty(loopPicture.INVP_PictureName))
                    {
                        continue;
                    }

                    loopPicture.INVP_Barcode = paramInventoryTotal.INV_Barcode;
                    Dictionary<string, AutoPartsPictureUIModel> pictureDictionary = new Dictionary<string, AutoPartsPictureUIModel>();
                    if (!pictureDictionary.ContainsKey(loopPicture.INVP_PictureName))
                    {
                        pictureDictionary.Add(loopPicture.INVP_PictureName, loopPicture);
                    }
                    ThreadPool.QueueUserWorkItem(new WaitCallback(LoadImage), pictureDictionary);
                }
                //添加空图片控件
                AddNullPictureControl();
            }

            //设置图片是否可见、可编辑
            SetPictureControl(paramInventoryTotal);
        }

        /// <summary>
        /// 异步加载配件图片
        /// </summary>
        /// <param name="paramPictureDic">图片名称和Image Dic</param>
        private void LoadImage(object paramPictureDic)
        {
            Dictionary<string, AutoPartsPictureUIModel> pictureDictionary = (Dictionary<string, AutoPartsPictureUIModel>)paramPictureDic;
            if (paramPictureDic == null)
            {
                return;
            }
            string curPictureName = pictureDictionary.FirstOrDefault(x => !string.IsNullOrEmpty(x.Key)).Key;

            if (!string.IsNullOrEmpty(curPictureName))
            {
                object tempImage = null;
                //通过文件名获取Image
                tempImage = BLLCom.GetBitmapImageByFileName(curPictureName);
                if (tempImage != null)
                {
                    this.Invoke((Action)(() =>
                    {
                        SkyCarPictureExpand autoPartsPicture = new SkyCarPictureExpand
                        {
                            //图片名称作为操作图片的唯一标识
                            PictureKey = curPictureName,
                            //图片Image
                            PictureImage = tempImage,
                            //待保存的配件图片TBModel
                            PropertyModel = pictureDictionary[curPictureName],
                            //上传图片
                            ExecuteUpload = UploadPicture,
                            //导出图片
                            ExecuteExport = ExportPicture,
                            //删除图片
                            ExecuteDelete = DeletePicture
                        };
                        if (autoPartsPicture.PictureImage != null)
                        {
                            flowLayoutPanelPicture.Controls.Add(autoPartsPicture);
                            _pictureExpandList.Add(autoPartsPicture);
                        }
                    }));
                }
            }
        }

        #region 单独操作图片

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="paramPictureKey">图片唯一标识</param>
        /// <param name="paramAutoPartsPicture">配件图片</param>
        /// <returns></returns>
        private object UploadPicture(string paramPictureKey, object paramAutoPartsPicture = null)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                if (fileDialog.ShowDialog() != DialogResult.OK)
                {
                    return null;
                }

                Image curImage = Image.FromStream(WebRequest.Create(fileDialog.FileName).GetResponse().GetResponseStream());
                //临时保存图片
                string tempFileName = Application.StartupPath + @"\" + paramPictureKey;
                curImage.Save(tempFileName, ImageFormat.Jpeg);

                AutoPartsPictureUIModel curAutoPartsPicture = paramAutoPartsPicture as AutoPartsPictureUIModel;
                if (curAutoPartsPicture == null
                    && (_curAutoPartsPictureList.Count == 0
                        ||
                        _curAutoPartsPictureList.Any(x => x.INVP_PictureName != paramPictureKey && x.SourceFilePath != tempFileName)))
                {
                    AutoPartsPictureUIModel newAutoPartsPicture = new AutoPartsPictureUIModel
                    {
                        INVP_PictureName = paramPictureKey,
                        SourceFilePath = tempFileName,
                        INVP_Barcode = _curActiveInventoryTotal.INV_Barcode,
                    };

                    _curAutoPartsPictureList.Add(newAutoPartsPicture);
                }
                else
                {
                    var curUploadPicture = _curAutoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == paramPictureKey);
                    if (curUploadPicture != null)
                    {
                        curUploadPicture.SourceFilePath = tempFileName;
                    }
                }

                var nullImagePicture = _pictureExpandList.Where(x => x.PictureImage == null).ToList();
                if (nullImagePicture.Count == 1)
                {
                    //添加空图片控件
                    AddNullPictureControl();
                }

                //设置图片是否可见、可编辑
                SetPictureControl(_curActiveInventoryTotal);

                return curImage;
            }
            catch (Exception ex)
            {
                //上传图片失败
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0018, new object[] { MsgParam.UPLOAD_IMAGE, ex.Message }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// 导出图片
        /// </summary>
        /// <param name="paramPictureKey">图片唯一标识</param>
        /// <returns></returns>
        private bool ExportPicture(string paramPictureKey)
        {
            //是否需要从服务器下载文件
            bool isDownFromWeb = false;
            //本地临时路径
            string tempLocalPicturePath = string.Empty;
            if (_curAutoPartsPictureList == null || _curAutoPartsPictureList.Count == 0)
            {
                isDownFromWeb = true;
            }
            else
            {
                var curPicture = _curAutoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == paramPictureKey);
                if (curPicture != null)
                {
                    tempLocalPicturePath = curPicture.SourceFilePath;
                }
                if (string.IsNullOrEmpty(tempLocalPicturePath))
                {
                    isDownFromWeb = true;
                }
                else
                {
                    isDownFromWeb = false;
                }
            }
            if (isDownFromWeb == false)
            {
                SaveFileDialog saveImageDialog = new SaveFileDialog
                {
                    Title = "图片保存",
                    //文件类型
                    Filter = @"jpeg|*.jpg|bmp|*.bmp|gif|*.gif",
                    //默认文件名
                    FileName = paramPictureKey,
                    //保存对话框是否记忆上次打开的目录
                    RestoreDirectory = true,
                };
                if (saveImageDialog.ShowDialog() != DialogResult.OK)
                {
                    return true;
                }
                string destFileName = saveImageDialog.FileName;
                if (string.IsNullOrEmpty(destFileName))
                {
                    return true;
                }

                //从本地Copy到选择的路径
                File.Copy(tempLocalPicturePath, saveImageDialog.FileName, true);
                //导出成功
                MessageBoxs.Show(Trans.PIS, ToString(),
                    MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.EXPORT }), MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                //消息
                var outMsg = string.Empty;
                bool exportResult = BLLCom.ExportFileByFileName(paramPictureKey, ref outMsg);
                if (exportResult == false)
                {
                    //导出失败
                    MessageBoxs.Show(Trans.PIS, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (string.IsNullOrEmpty(outMsg))
                {
                    return true;
                }
                //导出成功
                MessageBoxs.Show(Trans.PIS, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return true;
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="paramPictureKey"></param>
        /// <param name="paramAutoPartsPicture">配件图片</param>
        /// <returns></returns>
        private bool DeletePicture(string paramPictureKey, object paramAutoPartsPicture = null)
        {
            if (paramAutoPartsPicture == null)
            {
                //未保存的场合，仅清除显示图片
                //删除当前图片控件
                var deleteImage = _pictureExpandList.FirstOrDefault(x => x.PictureKey == paramPictureKey);
                if (deleteImage != null)
                {
                    flowLayoutPanelPicture.Controls.Remove(deleteImage);
                    _pictureExpandList.Remove(deleteImage);
                }
                //移除配件图片列表中数据
                var deletePicture = _curAutoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == paramPictureKey);
                if (deletePicture != null)
                {
                    _curAutoPartsPictureList.Remove(deletePicture);
                }
                return true;
            }
            //待删除的配件图片
            AutoPartsPictureUIModel argsAutoPartsPicture = paramAutoPartsPicture as AutoPartsPictureUIModel;

            if (argsAutoPartsPicture == null
                || string.IsNullOrEmpty(argsAutoPartsPicture.INVP_PictureName))
            {
                //未保存的场合，仅清除显示图片
                //删除当前图片控件
                var deletePicture = _pictureExpandList.FirstOrDefault(x => x.PictureKey == paramPictureKey);
                if (deletePicture != null)
                {
                    flowLayoutPanelPicture.Controls.Remove(deletePicture);
                    _pictureExpandList.Remove(deletePicture);
                }
                return true;
            }

            //错误消息
            var outMsg = string.Empty;
            //删除本地和文件服务器中的图片
            bool deleteResult = BLLCom.DeleteFileByFileName(paramPictureKey, ref outMsg);
            if (deleteResult == false)
            {
                //删除失败
                MessageBoxs.Show(Trans.PIS, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            #region 删除数据库中图片
            MDLPIS_InventoryPicture deleteAutoPartsPicture = new MDLPIS_InventoryPicture();
            if (!string.IsNullOrEmpty(argsAutoPartsPicture.INVP_ID))
            {
                deleteAutoPartsPicture.WHERE_INVP_ID = argsAutoPartsPicture.INVP_ID;
                deleteAutoPartsPicture.WHERE_INVP_VersionNo = argsAutoPartsPicture.INVP_VersionNo;
                try
                {
                    bool deleteInvPicture = _bll.Delete<MDLPIS_InventoryPicture>(deleteAutoPartsPicture);
                    if (!deleteInvPicture)
                    {
                        //删除失败
                        MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    //删除失败，失败原因：ex.Message
                    MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.DELETE, ex.Message }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            #endregion

            //删除成功
            MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //删除当前图片控件
            var removeImage = _pictureExpandList.FirstOrDefault(x => x.PictureKey == paramPictureKey);
            if (removeImage != null)
            {
                flowLayoutPanelPicture.Controls.Remove(removeImage);
                _pictureExpandList.Remove(removeImage);
            }
            //移除配件图片列表中数据
            var removePicture = _curAutoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == paramPictureKey);
            if (removePicture != null)
            {
                _curAutoPartsPictureList.Remove(removePicture);
            }

            return true;
        }

        #endregion

        #region 批量操作图片

        /// <summary>
        /// 全选图片
        /// </summary>
        private void SelectAllPicture()
        {
            if (_pictureExpandList.Count == 0)
            {
                return;
            }
            foreach (var loopPicture in _pictureExpandList)
            {
                if (loopPicture.IsCheckedIsVisible == false)
                {
                    continue;
                }
                loopPicture.IsChecked = ckSelectAllPicture.Checked;
            }

        }

        /// <summary>
        /// 批量上传图片
        /// </summary>
        private void BatchUploadPicture()
        {
            if (_curActiveInventoryTotal == null
                || string.IsNullOrEmpty(_curActiveInventoryTotal.INV_Barcode))
            {
                return;
            }

            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Multiselect = true;
                if (fileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                foreach (var loopFileName in fileDialog.FileNames)
                {
                    Image curImage = Image.FromStream(WebRequest.Create(loopFileName).GetResponse().GetResponseStream());

                    SkyCarPictureExpand autoPartsPicture = new SkyCarPictureExpand
                    {
                        //图片名称作为操作图片的唯一标识
                        PictureKey = Guid.NewGuid() + ".jpg",
                        //上传图片
                        ExecuteUpload = UploadPicture,
                        //导出图片
                        ExecuteExport = ExportPicture,
                        //删除图片
                        ExecuteDelete = DeletePicture,
                        //图片Image
                        PictureImage = curImage
                    };
                    var removePicture = _pictureExpandList.FirstOrDefault(x => x.PictureImage == null);
                    if (removePicture != null)
                    {
                        flowLayoutPanelPicture.Controls.Remove(removePicture);
                        _pictureExpandList.Remove(removePicture);
                    }

                    flowLayoutPanelPicture.Controls.Add(autoPartsPicture);
                    _pictureExpandList.Add(autoPartsPicture);

                    //临时保存图片
                    string tempFileName = Application.StartupPath + @"\" + autoPartsPicture.PictureKey;
                    curImage.Save(tempFileName, ImageFormat.Jpeg);

                    if ((_curAutoPartsPictureList.Count == 0
                        ||
                        _curAutoPartsPictureList.Any(x => x.INVP_PictureName != autoPartsPicture.PictureKey && x.SourceFilePath != tempFileName)))
                    {
                        //第一次上传的场合，新增待保存的配件图片
                        AutoPartsPictureUIModel newAutoPartsPicture = new AutoPartsPictureUIModel
                        {
                            INVP_Barcode = _curActiveInventoryTotal.INV_Barcode,
                            INVP_PictureName = autoPartsPicture.PictureKey,
                            SourceFilePath = tempFileName,
                        };
                        _curAutoPartsPictureList.Add(newAutoPartsPicture);
                    }
                    else
                    {
                        //更新上传的场合，更新图片文件源
                        var curUploadPicture = _curAutoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == autoPartsPicture.PictureKey);
                        if (curUploadPicture != null)
                        {
                            curUploadPicture.SourceFilePath = tempFileName;
                        }
                    }
                }

                //添加空图片控件
                AddNullPictureControl();

                //设置图片是否可见、可编辑
                SetPictureControl(_curActiveInventoryTotal);
            }
            catch (Exception ex)
            {
                //上传图片失败
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0018, new object[] { MsgParam.UPLOAD_IMAGE, ex.Message }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// 批量导出时的默认路径
        /// </summary>
        private string _defaultFilePath = string.Empty;

        /// <summary>
        /// 批量导出图片
        /// </summary>
        private void BatchExportPicture()
        {
            #region 验证

            //勾选的图片列表
            var checkedPictureExpandList = _pictureExpandList.Where(x => x.IsChecked == true && x.PictureImage != null).ToList();
            if (checkedPictureExpandList.Count == 0)
            {
                //请选择图片
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.IMAGE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #endregion

            List<string> fileNameList = new List<string>();
            //导出图片
            foreach (var loopPictureExpand in checkedPictureExpandList)
            {
                if (string.IsNullOrEmpty(loopPictureExpand.PictureKey))
                {
                    continue;
                }
                fileNameList.Add(loopPictureExpand.PictureKey);
            }

            //错误消息
            var outMsg = string.Empty;
            bool exportResult = BLLCom.ExportFileListByFileNameList(fileNameList, ref _defaultFilePath, ref outMsg);
            if (exportResult == false)
            {
                //导出失败
                MessageBoxs.Show(Trans.PIS, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //导出成功
            MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.EXPORT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 批量删除图片
        /// </summary>
        private void BatchDeletePicture()
        {
            #region 验证

            //勾选的图片列表
            var checkedPictureExpandList = _pictureExpandList.Where(x => x.IsChecked == true && x.PictureImage != null).ToList();
            if (checkedPictureExpandList.Count == 0)
            {
                //请选择图片
                MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.IMAGE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //已选checkedPictureList.Count 条数据，确定删除？\r\n单击【确定】删除，【取消】返回。
            DialogResult dialogResult = MessageBoxs.Show(Trans.PIS, ToString(), MsgHelp.GetMsg(MsgCode.W_0013, new object[] { checkedPictureExpandList.Count }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            #endregion

            //待删除的配件图片列表
            List<MDLPIS_InventoryPicture> deleteAutoPartsPictureList = new List<MDLPIS_InventoryPicture>();

            string pictureNameString = string.Empty;
            foreach (var loopPictureExpand in checkedPictureExpandList)
            {
                if (string.IsNullOrEmpty(loopPictureExpand.PictureKey))
                {
                    continue;
                }
                pictureNameString += loopPictureExpand.PictureKey + SysConst.Semicolon_DBC;
            }
            //根据图片名称查询待删除的配件图片
            _bll.QueryForList(SQLID.COMM_SQL49, new MDLPIS_InventoryPicture()
            {
                WHERE_INVP_Barcode = _curActiveInventoryTotal.INV_Barcode + SysConst.Semicolon_DBC,
                WHERE_INVP_PictureName = pictureNameString
            }, deleteAutoPartsPictureList);
            foreach (var loopPictureExpand in checkedPictureExpandList)
            {
                loopPictureExpand.PictureImage = null;

                var curAutoPartsPicture = deleteAutoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == loopPictureExpand.PictureKey);
                if (curAutoPartsPicture == null)
                {
                    //未保存到数据库的图片，仅清除
                    continue;
                }

                #region 删除本地和文件服务器中的图片

                //错误消息
                var outMsg = string.Empty;
                //删除本地和文件服务器中的图片
                bool deleteResult = BLLCom.DeleteFileByFileName(curAutoPartsPicture.INVP_PictureName, ref outMsg);
                if (deleteResult == false)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.PIS, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion
            }

            #region 删除数据库中配件图片

            if (deleteAutoPartsPictureList.Count > 0)
            {
                var outMsg = string.Empty;
                bool deleteDataResult = AutoPartsComFunction.DeleteAutoPartsPicture(deleteAutoPartsPictureList, ref outMsg);
                if (!deleteDataResult)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.PIS, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            //删除成功
            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            #endregion

            foreach (var loopPicture in deleteAutoPartsPictureList)
            {
                //删除当前图片控件
                var removeImage = _pictureExpandList.FirstOrDefault(x => x.PictureKey == loopPicture.INVP_PictureName);
                if (removeImage != null)
                {
                    flowLayoutPanelPicture.Controls.Remove(removeImage);
                    _pictureExpandList.Remove(removeImage);
                }
                //移除配件图片列表中数据
                var removePicture = _curAutoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == loopPicture.INVP_PictureName);
                if (removePicture != null)
                {
                    _curAutoPartsPictureList.Remove(removePicture);
                }
            }
        }

        #endregion

        /// <summary>
        /// 设置图片是否可见、可编辑
        /// </summary>
        /// <param name="paramActiveInventoryTotal">当前选中库存汇总</param>
        private void SetPictureControl(InventoryQueryUIModel paramActiveInventoryTotal)
        {
            if (paramActiveInventoryTotal == null
                || string.IsNullOrEmpty(paramActiveInventoryTotal.INV_Barcode)
                || _isCanEditPicture == false)
            {
                //未选中配件的场合 或者 无保存配件档案权限的场合，不可上传、导出、删除图片
                ckSelectAllPicture.Enabled = false;
                toolbarsAutoPartsPicture.Tools[SysConst.Upload].SharedProps.Enabled = false;
                toolbarsAutoPartsPicture.Tools[SysConst.Export].SharedProps.Enabled = false;
                toolbarsAutoPartsPicture.Tools[SysConst.EN_DEL].SharedProps.Enabled = false;

                flowLayoutPanelPicture.Controls.Clear();
                _pictureExpandList.Clear();
            }
            else
            {
                if (toolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[SystemActionEnum.Code.SAVE].SharedPropsInternal.Enabled)
                {
                    //配件图片可保存的场合，可上传、删除图片
                    toolbarsAutoPartsPicture.Tools[SysConst.Upload].SharedProps.Enabled = true;
                    toolbarsAutoPartsPicture.Tools[SysConst.EN_DEL].SharedProps.Enabled = true;
                    foreach (var loopPicture in _pictureExpandList)
                    {
                        loopPicture.UploadIsEnabled = true;
                        loopPicture.DeleteIsEnabled = true;
                    }
                }
                else
                {
                    //配件图片不可保存的场合，不可上传、删除图片
                    toolbarsAutoPartsPicture.Tools[SysConst.Upload].SharedProps.Enabled = false;
                    toolbarsAutoPartsPicture.Tools[SysConst.EN_DEL].SharedProps.Enabled = false;
                    foreach (var loopPicture in _pictureExpandList)
                    {
                        loopPicture.UploadIsEnabled = false;
                        loopPicture.DeleteIsEnabled = false;
                    }
                }

                //默认不勾选
                ckSelectAllPicture.Checked = false;
                if (_curAutoPartsPictureList.Count == 0)
                {
                    //无图片的场合，不可导出、删除图片
                    ckSelectAllPicture.Enabled = false;
                    toolbarsAutoPartsPicture.Tools[SysConst.Export].SharedProps.Enabled = false;
                    toolbarsAutoPartsPicture.Tools[SysConst.EN_DEL].SharedProps.Enabled = false;
                }
                else
                {
                    //有图片的场合，可导出、删除图片
                    ckSelectAllPicture.Enabled = true;
                    toolbarsAutoPartsPicture.Tools[SysConst.Export].SharedProps.Enabled = true;
                    toolbarsAutoPartsPicture.Tools[SysConst.EN_DEL].SharedProps.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 添加空图片控件
        /// </summary>
        private void AddNullPictureControl()
        {
            SkyCarPictureExpand addAutoPartsPicture = new SkyCarPictureExpand
            {
                //图片名称作为操作图片的唯一标识
                PictureKey = Guid.NewGuid() + ".jpg",
                //上传图片
                ExecuteUpload = UploadPicture,
                //导出图片
                ExecuteExport = ExportPicture,
                //删除图片
                ExecuteDelete = DeletePicture,
            };

            flowLayoutPanelPicture.Controls.Add(addAutoPartsPicture);
            _pictureExpandList.Add(addAutoPartsPicture);
        }

        #endregion

        #region 库存明细toolbar翻页事件

        /// <summary>
        /// 设置总页数和总记录条数
        /// </summary>
        /// <param name="paramTotalRecordCount">总记录条数</param>
        private void SetTotalPageCountAndTotalRecordCountForInventoryDetail(int paramTotalRecordCount)
        {
            var funcName = "SetTotalPageCountAndTotalRecordCountForInventoryDetail";
            LogHelper.WriteBussLogStart(Trans.PIS, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPagingForInvDetail != null)
            {
                if (paramTotalRecordCount > 0)
                {
                    _inventoryDetailTotalRecordCount = paramTotalRecordCount;
                    int? remainder = _inventoryDetailTotalRecordCount % PageSize;
                    if (remainder > 0)
                    {
                        _inventoryDetailTotalPageCount = _inventoryDetailTotalRecordCount / PageSize + 1;
                    }
                    else
                    {
                        _inventoryDetailTotalPageCount = _inventoryDetailTotalRecordCount / PageSize;
                    }
                }
                else
                {
                    _inventoryDetailPageIndex = 1;
                    _inventoryDetailTotalPageCount = 1;
                    _inventoryDetailTotalRecordCount = 0;
                }
                ((TextBoxTool)(ToolBarPagingForInvDetail.Tools[SysConst.EN_PAGEINDEX])).Text = _inventoryDetailPageIndex.ToString();
                ToolBarPagingForInvDetail.Tools[SysConst.EN_PAGECOUNT].SharedProps.Caption = _inventoryDetailTotalPageCount.ToString();
            }
            LogHelper.WriteBussLogEndOK(Trans.PIS, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 设置翻页按钮状态
        /// </summary>
        private void SetPageButtonStatusForInventoryDetail()
        {
            var funcName = "SetPageButtonStatusForInventoryDetail";
            LogHelper.WriteBussLogStart(Trans.PIS, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPagingForInvDetail != null)
            {
                if (_inventoryDetailPageIndex == 0 || _inventoryDetailTotalRecordCount == 0)
                {
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;

                    return;
                }
                if (_inventoryDetailPageIndex == 1 && _inventoryDetailTotalRecordCount <= PageSize)
                {
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (_inventoryDetailPageIndex == 1 && _inventoryDetailTotalRecordCount > PageSize)
                {
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (_inventoryDetailPageIndex != 1 && _inventoryDetailTotalRecordCount > PageSize * _inventoryDetailPageIndex)
                {
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
                }
                else if (_inventoryDetailPageIndex != 1 && _inventoryDetailTotalRecordCount <= PageSize * _inventoryDetailPageIndex)
                {
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPagingForInvDetail.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
                }
                else
                {
                    throw (new Exception("非预期的场合。。。"));
                }
            }
            LogHelper.WriteBussLogEndOK(Trans.PIS, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 设置翻页控件
        /// <para>窗体加载或初始化时调用</para>
        /// </summary>
        /// <param name="paramTotalRecordCount">总记录条数</param>
        public void SetBarPagingForInventoryDetail(int paramTotalRecordCount)
        {
            var funcName = "SetBarPagingForInventoryDetail";
            LogHelper.WriteBussLogStart(Trans.PIS, LoginInfoDAX.UserName, funcName, "", "", null);
            //重新计算[总页数]，设置最新[总记录条数]
            SetTotalPageCountAndTotalRecordCountForInventoryDetail(paramTotalRecordCount);
            //设置翻页按钮状态
            SetPageButtonStatusForInventoryDetail();
            LogHelper.WriteBussLogEndOK(Trans.PIS, LoginInfoDAX.UserName, funcName, "", "", null);
        }


        #endregion

        #endregion

      
    }
}
