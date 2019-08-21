using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.UIModel.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.CustomControl;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common.APModel;
using SkyCar.Framework.WindowUI;

namespace SkyCar.Coeus.UI.Common
{
    /// <summary>
    /// 领料查询
    /// </summary>
    public partial class FrmPickPartsQuery : BaseForm
    {
        #region 全局变量
        /// <summary>
        /// 领料查询BLL
        /// </summary>
        private BLLBase _bll = new BLLBase(Trans.COM);

        /// <summary>
        /// 翻页按钮ToolBar
        /// </summary>
        private UltraToolbarsManager _toolBarPaging = new UltraToolbarsManager();

        /// <summary>
        /// 接收SourceView的Func
        /// </summary>
        private Func<List<PickPartsQueryUIModel>, bool> _pickPartsDetailFunc;

        /// <summary>
        /// 页面参数字典
        /// </summary>
        private Dictionary<string, object> _viewParameters = new Dictionary<string, object>();

        /// <summary>
        /// Grid的数据源
        /// </summary>
        List<PickPartsQueryUIModel> ListGridDS = new List<PickPartsQueryUIModel>();

        /// <summary>
        /// 列表选择模式，默认单选，多选时可以选择多项。
        /// </summary>
        private CustomEnums.CustomeSelectionMode ListSelectionMode { get; set; }

        #region 下拉框数据源

        /// <summary>
        /// 配件名称
        /// </summary>
        List<MDLBS_AutoPartsName> _autoPartsNameList = new List<MDLBS_AutoPartsName>();

        /// <summary>
        /// 供应商
        /// </summary>
        List<MDLPIS_Supplier> _supplierList = new List<MDLPIS_Supplier>();

        /// <summary>
        /// 车辆品牌
        /// </summary>
        List<MDLBS_VehicleBrandInspireSumma> _vehicleBrandList = new List<MDLBS_VehicleBrandInspireSumma>();

        /// <summary>
        /// 车系
        /// </summary>
        List<MDLBS_VehicleBrandInspireSumma> _vehicleInspireList = new List<MDLBS_VehicleBrandInspireSumma>();

        /// <summary>
        /// 仓库
        /// </summary>
        List<MDLPIS_Warehouse> _warehouseList = new List<MDLPIS_Warehouse>();

        /// <summary>
        /// 仓位
        /// </summary>
        List<MDLPIS_WarehouseBin> _warehouseBinList = new List<MDLPIS_WarehouseBin>();

        #endregion

        #region 列表Grid翻页用

        /// <summary>
        /// 翻页ToolBar
        /// </summary>
        private UltraToolbarsManager ToolBarPaging
        {
            get { return _toolBarPaging; }
            set
            {
                _toolBarPaging = value;
                //初始化翻页
                SetBarPaging(TotalRecordCount);
            }
        }

        /// <summary>
        /// 执行子类中的查询方法
        /// </summary>
        private Action ExecuteQuery;
        /// <summary>
        /// 当前页面索引值
        /// </summary>
        private int PageIndex = 1;
        /// <summary>
        /// 总记录条数
        /// </summary>
        private int TotalRecordCount = 0;
        /// <summary>
        /// 页面大小
        /// </summary>
        private int PageSize = 30;
        /// <summary>
        /// 总页面数
        /// </summary>
        private int TotalPageCount = 0;
        #endregion

        #region 配件图片相关

        /// <summary>
        /// 当前选中的库存（仅用于配件图片）
        /// </summary>
        private PickPartsQueryUIModel _curInventory;

        /// <summary>
        /// 配件图片列表
        /// </summary>
        List<AutoPartsPictureUIModel> _autoPartsPictureList = new List<AutoPartsPictureUIModel>();

        /// <summary>
        /// 配件图片控件列表
        /// </summary>
        List<SkyCarPictureExpand> _pictureExpandList = new List<SkyCarPictureExpand>();

        #endregion

        #endregion

        #region 公共属性

        /// <summary>
        /// 选中项列表
        /// </summary>
        public List<PickPartsQueryUIModel> SelectedGridList = new List<PickPartsQueryUIModel>();
        #endregion

        #region 系统事件

        /// <summary>
        /// 领料查询
        /// </summary>
        /// <param name="paramCustomeSelectionMode">选择模式，默认单选</param>
        public FrmPickPartsQuery(CustomEnums.CustomeSelectionMode paramCustomeSelectionMode = CustomEnums.CustomeSelectionMode.Single)
        {
            ListSelectionMode = paramCustomeSelectionMode;

            InitializeComponent();

            this.Size = new Size(SystemConfigInfo.MainWindowWidth - 100, SystemConfigInfo.MainWindowHeight - 50);
        }

        /// <summary>
        /// 领料查询
        /// </summary>
        /// <param name="paramWindowParameters"></param>
        /// <param name="paramCustomeSelectionMode">选择模式，默认单选</param>
        public FrmPickPartsQuery(Dictionary<string, object> paramWindowParameters, CustomEnums.CustomeSelectionMode paramCustomeSelectionMode = CustomEnums.CustomeSelectionMode.Single)
        {
            ListSelectionMode = paramCustomeSelectionMode;
            _viewParameters = paramWindowParameters;

            InitializeComponent();

            this.Size = new Size(SystemConfigInfo.MainWindowWidth - 100, SystemConfigInfo.MainWindowHeight - 50);
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmPickPartsQuery_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（翻页）
            ToolBarPaging = toolBarManagerListTabPaging;
            //查询委托（基类控制翻页用）
            ExecuteQuery = QueryAction;
            //工具栏（翻页）单击事件
            toolBarManagerListTabPaging.ToolClick += new ToolClickEventHandler(ToolBarPaging_ToolClick);
            //工具栏（翻页）[当前页]值改变事件
            toolBarManagerListTabPaging.ToolValueChanged += new ToolEventHandler(ToolBarPaging_ToolValueChanged);
            //初始化【列表】Tab内控件
            InitializeListTabControls();
            #endregion

            #region 处理参数

            #region 获取PickAutoPartsFunc

            if (_viewParameters.ContainsKey(ComViewParamKey.PickAutoPartsFunc.ToString()))
            {
                var tempFunc = _viewParameters[ComViewParamKey.PickAutoPartsFunc.ToString()] as Func<List<PickPartsQueryUIModel>, bool>;
                if (tempFunc != null)
                {
                    _pickPartsDetailFunc = tempFunc;
                }
                else
                {
                    MessageBoxs.Show(Trans.COM, this.ToString(), "请传入Func<PickPartsQueryUIModel, bool>，以便处理配件明细", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            #endregion

            #region 获取供应商查询条件
            if (_viewParameters.ContainsKey(ComViewParamKey.SpecialSupplierName.ToString()))
            {
                var stringSupplierName = _viewParameters[ComViewParamKey.SpecialSupplierName.ToString()] as string;
                if (stringSupplierName != null)
                {
                    mcbWhere_Supplier.SelectedText = Convert.ToString(_viewParameters[ComViewParamKey.SpecialSupplierName.ToString()]);
                    mcbWhere_Supplier.Enabled = false;
                }
            }
            if (_viewParameters.ContainsKey(ComViewParamKey.SpecialSupplierID.ToString()))
            {
                var stringSupplierId = _viewParameters[ComViewParamKey.SpecialSupplierID.ToString()] as string;
                if (stringSupplierId != null)
                {
                    mcbWhere_Supplier.SelectedValue = Convert.ToString(_viewParameters[ComViewParamKey.SpecialSupplierID.ToString()]);
                }
            }
            #endregion

            #endregion

            if (ListSelectionMode == CustomEnums.CustomeSelectionMode.Multiple)
            {
                //可多选
                gdGrid.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
            }
            else
            {
                //只能单选
                gdGrid.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Never;
            }
        }

        #region 条件相关事件

        /// <summary>
        /// 仓库改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbWhere_Warehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(mcbWhere_Warehouse.SelectedValue))
            {
                return;
            }
            mcbWhere_WarehouseBin.Clear();

            if (_warehouseBinList != null)
            {
                var curWarehouseBinList = _warehouseBinList.Where(x => x.WHB_WH_ID == mcbWhere_Warehouse.SelectedValue).ToList();
                mcbWhere_WarehouseBin.DataSource = curWarehouseBinList;
            }
        }

        /// <summary>
        /// 车辆品牌改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbWhere_APA_VehicleBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(mcbWhere_APA_VehicleBrand.SelectedValue))
            {
                return;
            }
            mcbWhere_APA_VehicleInspire.Clear();

            if (_vehicleInspireList != null)
            {
                var curVehicleInspireList = _vehicleInspireList.Where(x => x.VBIS_Brand == mcbWhere_APA_VehicleBrand.SelectedValue).ToList();
                mcbWhere_APA_VehicleInspire.DataSource = curVehicleInspireList;
            }
        }
        #endregion

        #region 查询结果相关事件

        /// <summary>
        /// 单元格的值变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_CellChange(object sender, CellEventArgs e)
        {
            //提交数据到数据源
            gdGrid.UpdateData();
            if (e.Cell != null && e.Cell.Row != null)
            {
                bool curRowIsChecked = Convert.ToBoolean(e.Cell.Row.Cells[SysConst.IsChecked].Value);

                if (curRowIsChecked)
                {
                    //选中当前行，且为单选模式时，取消选中其他行
                    if (ListSelectionMode == CustomEnums.CustomeSelectionMode.Single)
                    {
                        string curRowID = e.Cell.Row.Cells[SysConst.RowID].Value != null ? e.Cell.Row.Cells[SysConst.RowID].Value.ToString() : string.Empty;

                        foreach (var loopSourceItem in ListGridDS)
                        {
                            if (loopSourceItem.RowID != curRowID)
                            {
                                if (loopSourceItem.IsChecked)
                                {
                                    loopSourceItem.IsChecked = false;
                                }
                            }
                        }
                        this.gdGrid.Refresh();
                    }
                }
                GenerateSelectedValueAndText();
            }
        }

        /// <summary>
        /// 双击单元格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            //是否允许获取Grid数据
            if (!IsAllowSetGridDataToCard())
            {
                return;
            }
            if (e.Cell.Column.Key == "IsChecked")
            {
                return;
            }
            string tempRowID = e.Cell.Row.Cells[SysConst.RowID].Value != null ? e.Cell.Row.Cells[SysConst.RowID].Value.ToString() : string.Empty;
            foreach (var loopSourceItem in ListGridDS)
            {
                if (loopSourceItem.RowID == tempRowID)
                {
                    if (!loopSourceItem.IsChecked)
                    {
                        loopSourceItem.IsChecked = true;
                        this.gdGrid.Refresh();
                    }
                }
                else
                {
                    //双击当前行，且为单选模式时，取消选中其他行
                    if (ListSelectionMode == CustomEnums.CustomeSelectionMode.Single)
                    {
                        if (loopSourceItem.IsChecked)
                        {
                            loopSourceItem.IsChecked = false;
                        }
                    }
                }
            }
            GenerateSelectedValueAndText();

            if (_pickPartsDetailFunc != null)
            {
                var pickPartsResult = _pickPartsDetailFunc(SelectedGridList);
                if (!pickPartsResult)
                {
                    //领取配件失败
                    MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.PICK + MsgParam.AUTOPARTS }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //领料成功，取消选中
                var tempSelectedList = ListGridDS.Where(x => x.IsChecked).ToList();
                foreach (var loopSelected in tempSelectedList)
                {
                    loopSelected.IsChecked = false;
                }
            }
            this.DialogResult = DialogResult.OK;

            MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { MsgParam.PICK + MsgParam.AUTOPARTS }), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Grid的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_DoubleClick(object sender, EventArgs e)
        {
            //是否允许获取Grid数据
            if (!IsAllowSetGridDataToCard())
            {
                return;
            }
            if (_pickPartsDetailFunc != null)
            {
                var pickPartsResult = _pickPartsDetailFunc(SelectedGridList);
                if (!pickPartsResult)
                {
                    //领取配件失败
                    MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.PICK + MsgParam.AUTOPARTS }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //领料成功，取消选中
                var tempSelectedList = ListGridDS.Where(x => x.IsChecked).ToList();
                foreach (var loopSelected in tempSelectedList)
                {
                    loopSelected.IsChecked = false;
                }
            }
            this.DialogResult = DialogResult.OK;

            MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { MsgParam.PICK + MsgParam.AUTOPARTS }), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Grid单击行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_Click(object sender, EventArgs e)
        {
            var curActiveRow = gdGrid.ActiveRow;

            if (curActiveRow != null)
            {
                //当前领料明细
                _curInventory = ListGridDS.FirstOrDefault(x => x.INV_Barcode == curActiveRow.Cells[SystemTableColumnEnums.PIS_Inventory.Code.INV_Barcode].Value.ToString());
            }

            //加载配件图片
            LoadAutoPartsPicture(_curInventory);

            //展开【配件图片】GroupBox
            gbPicture.Expanded = true;
        }

        /// <summary>
        /// Grid单击单元格事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_ClickCell(object sender, ClickCellEventArgs e)
        {
            var curActiveRow = gdGrid.Rows[e.Cell.Row.Index];

            if (curActiveRow != null)
            {
                //当前领料明细
                _curInventory = ListGridDS.FirstOrDefault(x => x.INV_Barcode == curActiveRow.Cells[SystemTableColumnEnums.PIS_Inventory.Code.INV_Barcode].Value.ToString());
            }

            //加载配件图片
            LoadAutoPartsPicture(_curInventory);

            //展开【配件图片】GroupBox
            gbPicture.Expanded = true;
        }

        /// <summary>
        /// Grid勾选HeaderCheckBox事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            gdGrid.UpdateData();
            GenerateSelectedValueAndText();
        }
        #endregion

        #region Button事件

        /// <summary>
        /// [清空]按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            //初始化【列表】Tab内控件
            InitializeListTabControls();
        }
        /// <summary>
        /// [查询]按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //查询
            QueryAction();
        }

        /// <summary>
        /// 清除已选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearSelectedItem_Click(object sender, EventArgs e)
        {
            foreach (var loopSourceItem in ListGridDS)
            {
                loopSourceItem.IsChecked = false;
            }
            this.gdGrid.Refresh();
            SelectedGridList = new List<PickPartsQueryUIModel>();
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (_pickPartsDetailFunc != null)
            {
                var pickPartsResult = _pickPartsDetailFunc(SelectedGridList);
                if (!pickPartsResult)
                {
                    //领取配件失败
                    MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.PICK + MsgParam.AUTOPARTS }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //领料成功，取消选中
                var tempSelectedList = ListGridDS.Where(x => x.IsChecked).ToList();
                foreach (var loopSelected in tempSelectedList)
                {
                    loopSelected.IsChecked = false;
                }
            }
            if (SelectedGridList.Count == 0)
            {
                //请至少勾选一条数据
                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0038, new object[] { }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.DialogResult = DialogResult.OK;

            //领取配件成功
            MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { MsgParam.PICK + MsgParam.AUTOPARTS }), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 确定并继续
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirmAndContinue_Click(object sender, EventArgs e)
        {
            gdGrid.UpdateData();
            if (_pickPartsDetailFunc != null)
            {
                var pickPartsResult = _pickPartsDetailFunc(SelectedGridList);
                if (!pickPartsResult)
                {
                    //领取配件失败
                    MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.PICK + MsgParam.AUTOPARTS }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //领料成功，取消选中
                var tempSelectedList = ListGridDS.Where(x => x.IsChecked).ToList();
                foreach (var loopSelected in tempSelectedList)
                {
                    loopSelected.IsChecked = false;
                }

                gdGrid.Refresh();
            }
            if (SelectedGridList.Count == 0)
            {
                //请至少勾选一条数据
                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0038, new object[] { }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #endregion

        #region 重写基类方法

        /// <summary>
        /// 查询
        /// </summary>
        public void QueryAction()
        {
            PickPartsQueryQCModel pickPartsQuery = new PickPartsQueryQCModel
            {
                PageSize = PageSize,
                PageIndex = PageIndex,
                //组织
                WHERE_INV_Org_ID = LoginInfoDAX.OrgID,
                //配件名称
                WHERE_INV_Name = mcbWhere_APA_Name.Text.Trim(),
                //配件条码（配件条码+批次号）
                WHERE_AutoPartsBarcode = txtWhere_Barcode.Text.Trim(),
                //配件编码（原厂编码或第三方编码）
                WHERE_AutoPartsCode = txtWhere_AutoPartsCode.Text.Trim(),
                //其他描述（专有属性或适用范围中的任一项）
                WHERE_OtherDesc = txtWhere_OtherDesc.Text.Trim(),
                //车型代码
                WHERE_APA_VehicleModelCode = txtWhere_APA_VehicleModelCode.Text.Trim(),
                //供应商ID
                WHERE_INV_SUPP_ID = mcbWhere_Supplier.SelectedValue,
                //供应商名称
                WHERE_SUPP_Name = mcbWhere_Supplier.SelectedText,
                //仓库
                WHERE_WH_Name = mcbWhere_Warehouse.SelectedText,
                //仓位
                WHERE_WHB_Name = mcbWhere_WarehouseBin.SelectedText
            };
            //入库时间-开始
            if (dtWhere_StockInTimeStart.Value != null)
            {
                pickPartsQuery._CreatedTimeStart = dtWhere_StockInTimeStart.DateTime;
            }
            //入库时间-终了
            if (dtWhere_StockInTimeEnd.Value != null)
            {
                pickPartsQuery._CreatedTimeEnd = dtWhere_StockInTimeEnd.DateTime;
            }
            //配件品牌
            pickPartsQuery.WHERE_APA_Brand = txtWhere_APA_Brand.Text.Trim();
            //车辆品牌
            pickPartsQuery.WHERE_APA_VehicleBrand = mcbWhere_APA_VehicleBrand.SelectedText;
            //车系
            pickPartsQuery.WHERE_APA_VehicleInspire = mcbWhere_APA_VehicleInspire.SelectedText;
            _bll.QueryForList<PickPartsQueryUIModel>(SQLID.COMM_SQL32, pickPartsQuery, ListGridDS);

            gdGrid.DataSource = ListGridDS;
            gdGrid.DataBind();

            if (ListGridDS.Count > 0)
            {
                dynamic subObject = ListGridDS[0];

                TotalRecordCount = subObject.RecordCount;
            }
            else
            {
                TotalRecordCount = 0;
            }
            //设置翻页控件
            SetBarPaging(TotalRecordCount);

            //收起【配件图片】GroupBox
            gbPicture.Expanded = false;
            //设置单元格是否可以编辑
            SetPayConfirmWindowStyle();
        }
        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 查询条件初始化
            //配件名称
            mcbWhere_APA_Name.Clear();
            if (mcbWhere_Supplier.Enabled)
            {
                //供应商
                mcbWhere_Supplier.Clear();
            }
            //配件条码（配件条形码+批次号）
            txtWhere_Barcode.Clear();
            //配件编码（原厂编码或第三方编码）
            txtWhere_AutoPartsCode.Clear();
            //其他描述（专有属性或适用范围中的任一项）
            txtWhere_OtherDesc.Clear();
            //车型代码
            txtWhere_APA_VehicleModelCode.Clear();
            //仓库
            mcbWhere_Warehouse.Clear();
            //仓位
            mcbWhere_WarehouseBin.Clear();
            //入库时间
            dtWhere_StockInTimeStart.Value = null;
            dtWhere_StockInTimeEnd.Value = null;

            //配件品牌
            txtWhere_APA_Brand.Clear();
            //车辆品牌
            mcbWhere_APA_VehicleBrand.Clear();
            //车系
            mcbWhere_APA_VehicleInspire.Clear();
            //给 配件名称 设置焦点
            lblWhere_APA_Name.Focus();
            #endregion

            #region 初始化下拉框
            //配件名称
            _autoPartsNameList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsName) as List<MDLBS_AutoPartsName>;
            mcbWhere_APA_Name.DisplayMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbWhere_APA_Name.ValueMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_ID;
            mcbWhere_APA_Name.DataSource = _autoPartsNameList;

            //供应商
            _supplierList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsSupplier) as List<MDLPIS_Supplier>;
            mcbWhere_Supplier.DisplayMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_Name;
            mcbWhere_Supplier.ValueMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_ID;
            if (mcbWhere_Supplier.Enabled)
            {
                mcbWhere_Supplier.DataSource = _supplierList;
            }

            //车辆品牌
            _vehicleBrandList = CacheDAX.Get(CacheDAX.ConfigDataKey.VehicleBrand) as List<MDLBS_VehicleBrandInspireSumma>;
            mcbWhere_APA_VehicleBrand.DisplayMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Brand;
            mcbWhere_APA_VehicleBrand.ValueMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Brand;
            mcbWhere_APA_VehicleBrand.DataSource = _vehicleBrandList;

            //车辆车系
            _vehicleInspireList = CacheDAX.Get(CacheDAX.ConfigDataKey.VehicleBrandInspire) as List<MDLBS_VehicleBrandInspireSumma>;
            mcbWhere_APA_VehicleInspire.DisplayMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Inspire;
            mcbWhere_APA_VehicleInspire.ValueMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Inspire;
            mcbWhere_APA_VehicleInspire.DataSource = _vehicleInspireList;

            //仓库
            var allWarehouseList = CacheDAX.Get(CacheDAX.ConfigDataKey.Warehouse) as List<MDLPIS_Warehouse>;
            mcbWhere_Warehouse.DisplayMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name;
            mcbWhere_Warehouse.ValueMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_ID;
            if (allWarehouseList != null)
            {
                _warehouseList = allWarehouseList.Where(x => x.WH_Org_ID == LoginInfoDAX.OrgID).ToList();
                mcbWhere_Warehouse.DataSource = _warehouseList;
            }

            //仓位
            _warehouseBinList = CacheDAX.Get(CacheDAX.ConfigDataKey.WarehouseBin) as List<MDLPIS_WarehouseBin>;
            mcbWhere_WarehouseBin.DisplayMember = SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name;
            mcbWhere_WarehouseBin.ValueMember = SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_ID;
            if (_warehouseBinList != null && !string.IsNullOrEmpty(mcbWhere_Warehouse.SelectedValue))
            {
                var curWarehouseBinList = _warehouseBinList.Where(x => x.WHB_WH_ID == mcbWhere_Warehouse.SelectedValue).ToList();
                mcbWhere_WarehouseBin.DataSource = curWarehouseBinList;
            }
            #endregion

            //清空Grid
            ListGridDS = new List<PickPartsQueryUIModel>();
            gdGrid.DataSource = ListGridDS;
            gdGrid.DataBind();
        }

        /// <summary>
        /// 生成选中项的值和描述
        /// </summary>
        void GenerateSelectedValueAndText()
        {
            SelectedGridList = new List<PickPartsQueryUIModel>();
            foreach (var loopSourceItem in ListGridDS)
            {
                if (!loopSourceItem.IsChecked)
                {
                    continue;
                }
                PickPartsQueryUIModel argsPickPartsQuery = new PickPartsQueryUIModel
                {
                    INV_ID = loopSourceItem.INV_ID,
                    INV_Org_ID = loopSourceItem.INV_Org_ID,
                    INV_SUPP_ID = loopSourceItem.INV_SUPP_ID,
                    INV_WH_ID = loopSourceItem.INV_WH_ID,
                    INV_WHB_ID = loopSourceItem.INV_WHB_ID,
                    INV_ThirdNo = loopSourceItem.INV_ThirdNo,
                    INV_OEMNo = loopSourceItem.INV_OEMNo,
                    INV_Barcode = loopSourceItem.INV_Barcode,
                    INV_BatchNo = loopSourceItem.INV_BatchNo,
                    INV_Name = loopSourceItem.INV_Name,
                    INV_Specification = loopSourceItem.INV_Specification,
                    INV_Qty = loopSourceItem.INV_Qty,
                    INV_PurchaseUnitPrice = loopSourceItem.INV_PurchaseUnitPrice,
                    INV_IsValid = loopSourceItem.INV_IsValid,
                    INV_CreatedBy = loopSourceItem.INV_CreatedBy,
                    INV_CreatedTime = loopSourceItem.INV_CreatedTime,
                    INV_UpdatedBy = loopSourceItem.INV_UpdatedBy,
                    INV_UpdatedTime = loopSourceItem.INV_UpdatedTime,

                    APA_Brand = loopSourceItem.APA_Brand,
                    APA_UOM = loopSourceItem.APA_UOM,
                    APA_Level = loopSourceItem.APA_Level,
                    APA_VehicleBrand = loopSourceItem.APA_VehicleBrand,
                    APA_VehicleInspire = loopSourceItem.APA_VehicleInspire,
                    APA_VehicleCapacity = loopSourceItem.APA_VehicleCapacity,
                    APA_VehicleYearModel = loopSourceItem.APA_VehicleYearModel,
                    APA_VehicleGearboxTypeName = loopSourceItem.APA_VehicleGearboxTypeName,
                    APA_VehicleGearboxTypeCode = loopSourceItem.APA_VehicleGearboxTypeCode,

                    Org_ShortName = loopSourceItem.Org_ShortName,
                    SUPP_Name = loopSourceItem.SUPP_Name,
                    WH_Name = loopSourceItem.WH_Name,
                    WHB_Name = loopSourceItem.WHB_Name,

                    APN_FixUOM = loopSourceItem.APN_FixUOM,
                    QtyFormatString = loopSourceItem.QtyFormatString,
                };

                SelectedGridList.Add(argsPickPartsQuery);
            }
        }
        /// <summary>
        /// 是否允许获取Grid数据
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

        #region 配件图片相关方法

        private string _latestBarcode = string.Empty;
        /// <summary>
        /// 加载配件图片
        /// </summary>
        private void LoadAutoPartsPicture(PickPartsQueryUIModel paramInventory)
        {
            if (paramInventory == null
                || string.IsNullOrEmpty(paramInventory.INV_Barcode)
                || paramInventory.INV_Barcode == _latestBarcode)
            {
                return;
            }
            _latestBarcode = paramInventory.INV_Barcode;

            //查询配件图片
            _bll.QueryForList(new MDLPIS_InventoryPicture
            {
                WHERE_INVP_Barcode = paramInventory.INV_Barcode,
                WHERE_INVP_IsValid = true,
            }, _autoPartsPictureList);

            flowLayoutPanelPicture.Controls.Clear();
            _pictureExpandList.Clear();

            if (_autoPartsPictureList.Count > 0)
            {
                //配件有图片时，加载实际数量的扩展的图片控件以及图片
                foreach (var loopPicture in _autoPartsPictureList)
                {
                    if (string.IsNullOrEmpty(loopPicture.INVP_ID)
                        || string.IsNullOrEmpty(loopPicture.INVP_PictureName))
                    {
                        continue;
                    }

                    Dictionary<string, AutoPartsPictureUIModel> pictureDictionary = new Dictionary<string, AutoPartsPictureUIModel>();
                    if (!pictureDictionary.ContainsKey(loopPicture.INVP_PictureName))
                    {
                        pictureDictionary.Add(loopPicture.INVP_PictureName, loopPicture);
                    }
                    ThreadPool.QueueUserWorkItem(new WaitCallback(LoadImage), pictureDictionary);
                }
            }
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
                        };
                        if (autoPartsPicture.PictureImage != null)
                        {
                            flowLayoutPanelPicture.Controls.Add(autoPartsPicture);
                            autoPartsPicture.IsCheckedIsVisible = false;
                            autoPartsPicture.UploadIsVisible = false;
                            autoPartsPicture.ExportIsVisible = false;
                            autoPartsPicture.DeleteIsVisible = false;
                            _pictureExpandList.Add(autoPartsPicture);
                        }
                        if (autoPartsPicture.PictureImage != null)
                        {
                            flowLayoutPanelPicture.Controls.Add(autoPartsPicture);
                            _pictureExpandList.Add(autoPartsPicture);
                        }
                    }));
                }
            }
        }
        #endregion

        #region Grid翻页相关方法

        #region 公共
        /// <summary>
        /// 设置翻页控件
        /// <para>窗体加载或初始化时调用</para>
        /// </summary>
        /// <param name="paramTotalRecordCount">总记录条数</param>
        private void SetBarPaging(int paramTotalRecordCount)
        {
            var funcName = "SetBarPaging";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            //重新计算[总页数]，设置最新[总记录条数]
            SetTotalPageCountAndTotalRecordCount(paramTotalRecordCount);
            //设置翻页按钮状态
            SetPageButtonStatus();
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void ToolBarPaging_ToolClick(object sender, ToolClickEventArgs e)
        {
            var funcName = "ToolBarPaging_ToolClick";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPaging != null)
            {
                switch (e.Tool.Key)
                {
                    //第一页
                    case SysConst.EN_FIRSTPAGE:
                        ((TextBoxTool)(ToolBarPaging.Tools[SysConst.EN_PAGEINDEX])).Text =
                            SysConst.NUMBER_ONE.ToString();
                        break;
                    // 前一页
                    case SysConst.EN_FORWARDPAGE:
                        ((TextBoxTool)(ToolBarPaging.Tools[SysConst.EN_PAGEINDEX])).Text = (PageIndex - 1).ToString();
                        break;
                    // 下一页
                    case SysConst.EN_NEXTPAGE:
                        ((TextBoxTool)(ToolBarPaging.Tools[SysConst.EN_PAGEINDEX])).Text = (PageIndex + 1).ToString();
                        break;
                    // 最后一页
                    case SysConst.EN_LASTPAGE:
                        ((TextBoxTool)(ToolBarPaging.Tools[SysConst.EN_PAGEINDEX])).Text = TotalPageCount.ToString();
                        break;
                }
            }
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }
        /// <summary>
        /// 翻页ToolBar的值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolBarPaging_ToolValueChanged(object sender, ToolEventArgs e)
        {
            var funcName = "ToolBarPaging_ToolValueChanged";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
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
                else if (tmpPageIndex > TotalPageCount)
                {
                    ((TextBoxTool)(e.Tool)).Text = TotalPageCount.ToString();
                    ((TextBoxTool)(e.Tool)).SelectionLength = TotalPageCount.ToString().Length;
                    return;
                }

                #region 当前页码设置
                if (Convert.ToInt32(strValue) <= 0)
                {
                    ((TextBoxTool)(e.Tool)).Text = SysConst.NUMBER_ONE.ToString();
                }
                else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) >= TotalPageCount)
                {
                    ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(TotalPageCount).ToString();
                }
                else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) < TotalPageCount)
                {
                    ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(strValue).ToString();
                }
                #endregion

                PageIndex = tmpPageIndex;
                if (tmpPageIndex <= 0)
                {
                    PageIndex = 1;
                }
                ExecuteQuery?.Invoke();
            }
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }
        #endregion

        #region 私有
        /// <summary>
        /// 设置总页数和总记录条数
        /// </summary>
        /// <param name="paramTotalRecordCount">总记录条数</param>
        private void SetTotalPageCountAndTotalRecordCount(int paramTotalRecordCount)
        {
            var funcName = "SetTotalPageCountAndTotalRecordCount";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPaging != null)
            {
                if (paramTotalRecordCount > 0)
                {
                    TotalRecordCount = paramTotalRecordCount;
                    int? remainder = TotalRecordCount % PageSize;
                    if (remainder > 0)
                    {
                        TotalPageCount = TotalRecordCount / PageSize + 1;
                    }
                    else
                    {
                        TotalPageCount = TotalRecordCount / PageSize;
                    }
                }
                else
                {
                    PageIndex = 1;
                    TotalPageCount = 1;
                    TotalRecordCount = 0;
                }
                ((TextBoxTool)(ToolBarPaging.Tools[SysConst.EN_PAGEINDEX])).Text = PageIndex.ToString();
                ToolBarPaging.Tools[SysConst.EN_PAGECOUNT].SharedProps.Caption = TotalPageCount.ToString();
            }
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 设置翻页按钮状态
        /// </summary>
        private void SetPageButtonStatus()
        {
            var funcName = "SetPageButtonStatus";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPaging != null)
            {
                if (PageIndex == 0 || TotalRecordCount == 0)
                {
                    ToolBarPaging.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;

                    return;
                }
                if (PageIndex == 1 && TotalRecordCount <= PageSize)
                {
                    ToolBarPaging.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (PageIndex == 1 && TotalRecordCount > PageSize)
                {
                    ToolBarPaging.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPaging.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPaging.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (PageIndex != 1 && TotalRecordCount > PageSize * PageIndex)
                {
                    ToolBarPaging.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPaging.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPaging.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPaging.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
                }
                else if (PageIndex != 1 && TotalRecordCount <= PageSize * PageIndex)
                {
                    ToolBarPaging.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPaging.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
                }
                else
                {
                    throw (new Exception("非预期的场合。。。"));
                }
            }
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }
        #endregion

        #endregion

        /// <summary>
        /// 设置单元格是否可以编辑
        /// </summary>
        private void SetPayConfirmWindowStyle()
        {
            #region 设置Grid数据颜色
            gdGrid.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;

            #endregion
        }

        #endregion

    }
}
