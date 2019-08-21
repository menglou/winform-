using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.Common.Log;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.DAL;
using Infragistics.Win.UltraWinEditors;
using SkyCar.Coeus.BLL.COM;
using SkyCar.Coeus.Common.Message;

namespace SkyCar.Coeus.UI.Common
{
    /// <summary>
    /// 维护配件明细
    /// </summary>
    public partial class FrmMaintainAutoPartsDetail : BaseFormDialog<MaintainAutoPartsDetailUIModel, MaintainAutoPartsDetailQCModel, MDLBS_AutoPartsArchive>
    {
        #region 全局变量

        /// <summary>
        /// 维护配件明细BLL
        /// </summary>
        private MaintainAutoPartsDetailBLL _bll = new MaintainAutoPartsDetailBLL();

        /// <summary>
        /// 翻页按钮ToolBar
        /// </summary>
        private UltraToolbarsManager _toolBarPaging = new UltraToolbarsManager();

        /// <summary>
        /// 接收SourceView的Func
        /// </summary>
        private Func<MaintainAutoPartsDetailUIModel, bool> _maintainAutoPartsDetailFunc;
        /// <summary>
        /// 页面参数字典
        /// </summary>
        private Dictionary<string, object> _viewParameters;
        /// <summary>
        /// 来源画面名称
        /// </summary>
        private ComViewParamValue.MaintainAutoPartsSourForm _sourceFormName;
        /// <summary>
        /// 维护动作（添加、更新、即进即出）
        /// </summary>
        private ComViewParamValue.MaintainAutoPartsAction _maintainAction;
        /// <summary>
        /// 配件明细ID
        /// </summary>
        private string _detailID = null;
        /// <summary>
        /// 默认供应商
        /// </summary>
        string _defaultSupplierId = null;
        /// <summary>
        /// 默认仓库
        /// </summary>
        string _defaultWarehouseId = null;
        /// <summary>
        /// 默认仓位
        /// </summary>
        string _defaultWarehouseBinId = null;
        /// <summary>
        /// 条码关键信息是否可编辑
        /// </summary>
        private bool? _barcodeKeyInfoEditable = true;
        /// <summary>
        /// 采购数量格式
        /// </summary>
        private string _purchaseQtyFormatString = "F0";

        #region 下拉框数据源

        /// <summary>
        /// 配件名称
        /// </summary>
        List<MDLBS_AutoPartsName> _autoPartsNameList = new List<MDLBS_AutoPartsName>();

        /// <summary>
        /// 车辆品牌
        /// </summary>
        List<MDLBS_VehicleBrandInspireSumma> _vehicleBrandList = new List<MDLBS_VehicleBrandInspireSumma>();

        /// <summary>
        /// 车系
        /// </summary>
        List<MDLBS_VehicleBrandInspireSumma> _vehicleInspireList = new List<MDLBS_VehicleBrandInspireSumma>();

        /// <summary>
        /// 默认供应商
        /// </summary>
        List<MDLPIS_Supplier> _supplierList = new List<MDLPIS_Supplier>();

        /// <summary>
        /// 默认仓库
        /// </summary>
        List<MDLPIS_Warehouse> _warehouseList = new List<MDLPIS_Warehouse>();

        /// <summary>
        /// 默认仓位
        /// </summary>
        List<MDLPIS_WarehouseBin> _warehouseBinList = new List<MDLPIS_WarehouseBin>();

        /// <summary>
        /// 变速类型数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _vehicleGearboxTypeDS = new List<ComComboBoxDataSourceTC>();
        #endregion

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

        #region 列表Grid翻页用
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
        private int PageSize = 20;
        /// <summary>
        /// 总页面数
        /// </summary>
        private int TotalPageCount = 0;
        #endregion

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmMaintainAutoPartsDetail构造方法
        /// </summary>
        public FrmMaintainAutoPartsDetail()
        {
            InitializeComponent();

            this.Size = new Size(SystemConfigInfo.MainWindowWidth - 100, SystemConfigInfo.MainWindowHeight - 50);
        }
        /// <summary>
        /// FrmMaintainAutoPartsDetail构造方法
        /// </summary>
        public FrmMaintainAutoPartsDetail(Dictionary<string, object> paramWindowParameters)
        {
            InitializeComponent();

            _viewParameters = paramWindowParameters;

            this.Size = new Size(SystemConfigInfo.MainWindowWidth - 100, SystemConfigInfo.MainWindowHeight - 50);
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMaintainAutoPartsDetail_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（翻页）
            ToolBarPaging = toolBarPaging;
            //查询委托（基类控制翻页用）
            ExecuteQuery = QueryAction;
            //工具栏（翻页）单击事件
            toolBarPaging.ToolClick += new ToolClickEventHandler(ToolBarPaging_ToolClick);
            //工具栏（翻页）[当前页]值改变事件
            toolBarPaging.ToolValueChanged += new ToolEventHandler(ToolBarPaging_ToolValueChanged);
            //初始化【详情】Tab内控件
            InitializeDetailTabControls();
            //初始化【列表】Tab内控件
            InitializeListTabControls();
            #endregion

            //将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();

            #region 处理参数

            if (_viewParameters == null)
            {
                return;
            }

            #region 获取SourceView
            if (_viewParameters.ContainsKey(ComViewParamKey.MaintainAutoPartsSourForm.ToString()))
            {
                if (_viewParameters[ComViewParamKey.MaintainAutoPartsSourForm.ToString()] is ComViewParamValue.MaintainAutoPartsSourForm)
                {
                    _sourceFormName = (ComViewParamValue.MaintainAutoPartsSourForm)_viewParameters[ComViewParamKey.MaintainAutoPartsSourForm.ToString()];
                }
                else
                {
                    MessageBoxs.Show(Trans.COM, this.ToString(), "请传入ComViewParamValue.MaintainAutoPartsSourView中的枚举值", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                MessageBoxs.Show(Trans.COM, this.ToString(), "请传入MaintainAutoPartsSourView", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion

            #region 获取MaintainAction
            if (_viewParameters.ContainsKey(ComViewParamKey.MaintainAutoPartsAction.ToString()))
            {
                if (_viewParameters[ComViewParamKey.MaintainAutoPartsAction.ToString()] is ComViewParamValue.MaintainAutoPartsAction)
                {
                    _maintainAction = (ComViewParamValue.MaintainAutoPartsAction)_viewParameters[ComViewParamKey.MaintainAutoPartsAction.ToString()];
                }
                else
                {
                    MessageBoxs.Show(Trans.COM, this.ToString(), "请传入ComViewParamValue.MaintainAutoPartsAction中的枚举值", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                MessageBoxs.Show(Trans.COM, this.ToString(), "请传入MaintainAutoPartsAction", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion

            #region 获取MaintainAutoPartsSourViewFunc
            if (_viewParameters.ContainsKey(ComViewParamKey.MaintainAutoPartsSourFormFunc.ToString()))
            {
                var tempFunc = _viewParameters[ComViewParamKey.MaintainAutoPartsSourFormFunc.ToString()] as Func<MaintainAutoPartsDetailUIModel, bool>;
                if (tempFunc != null)
                {
                    _maintainAutoPartsDetailFunc = tempFunc;
                }
                else
                {
                    MessageBoxs.Show(Trans.COM, this.ToString(), "请传入Func<MaintainAutoPartsDetailUIModel, bool>，以便处理配件明细", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            #endregion

            #region 获取默认供应商和仓库
            if (_viewParameters.ContainsKey(ComViewParamKey.MaintainAutoPartsDefaultSupplierID.ToString()))
            {
                if (_viewParameters[ComViewParamKey.MaintainAutoPartsDefaultSupplierID.ToString()] != null)
                {
                    _defaultSupplierId = _viewParameters[ComViewParamKey.MaintainAutoPartsDefaultSupplierID.ToString()].ToString();
                }
            }
            if (_viewParameters.ContainsKey(ComViewParamKey.MaintainAutoPartsDefaultWarehouseID.ToString()))
            {
                if (_viewParameters[ComViewParamKey.MaintainAutoPartsDefaultWarehouseID.ToString()] != null)
                {
                    _defaultWarehouseId = _viewParameters[ComViewParamKey.MaintainAutoPartsDefaultWarehouseID.ToString()].ToString();
                }
            }
            if (_viewParameters.ContainsKey(ComViewParamKey.MaintainAutoPartsDefaultWarehouseBinID.ToString()))
            {
                if (_viewParameters[ComViewParamKey.MaintainAutoPartsDefaultWarehouseBinID.ToString()] != null)
                {
                    _defaultWarehouseBinId = _viewParameters[ComViewParamKey.MaintainAutoPartsDefaultWarehouseBinID.ToString()].ToString();
                }
            }
            #endregion

            #region 获取供应商查询条件
            if (_viewParameters.ContainsKey(ComViewParamKey.SpecialSupplierName.ToString()))
            {
                var stringSupplierName = _viewParameters[ComViewParamKey.SpecialSupplierName.ToString()] as string;
                if (stringSupplierName != null)
                {
                    mcbSUPP_Name.SelectedText = Convert.ToString(_viewParameters[ComViewParamKey.SpecialSupplierName.ToString()]);
                    mcbSUPP_Name.Enabled = false;
                }
            }
            if (_viewParameters.ContainsKey(ComViewParamKey.SpecialSupplierID.ToString()))
            {
                var stringSupplierId = _viewParameters[ComViewParamKey.SpecialSupplierID.ToString()] as string;
                if (stringSupplierId != null)
                {
                    mcbSUPP_Name.SelectedValue = Convert.ToString(_viewParameters[ComViewParamKey.SpecialSupplierID.ToString()]);
                }
            }
            #endregion

            #region 获取配件信息

            if (_viewParameters.ContainsKey(ComViewParamKey.DestModel.ToString()))
            {
                // 更新配件时
                var updateAutoPartsDetail = _viewParameters[ComViewParamKey.DestModel.ToString()] as MaintainAutoPartsDetailUIModel;
                if (updateAutoPartsDetail != null)
                {
                    _bll.CopyModel(updateAutoPartsDetail, DetailDS);

                    _detailID = updateAutoPartsDetail.Detail_ID;
                    //计算入库金额
                    DetailDS.StockInAmount = CalcStockInAmount(DetailDS.PurchaseUnitPrice, DetailDS.PurchaseQuantity);

                    //将DetailDS数据赋值给【详情】Tab内的对应控件
                    SetDetailDSToCardCtrls();
                }
            }

            #endregion

            #region 应用界面参数

            btnAddAutoParts.Visible = _maintainAction == ComViewParamValue.MaintainAutoPartsAction.AddAutoParts;
            btnAddAutoPartsAndContinue.Visible = _maintainAction == ComViewParamValue.MaintainAutoPartsAction.AddAutoParts;
            btnUpdateAutoParts.Visible = _maintainAction == ComViewParamValue.MaintainAutoPartsAction.UpdateAutoParts;

            #endregion
            #endregion

        }
        private void FrmMaintainAutoPartsDetail_Activated(object sender, EventArgs e)
        {
            this.Invalidate(true);
        }

        private void FrmMaintainAutoPartsDetail_Deactivate(object sender, EventArgs e)
        {
            this.Invalidate(true);
        }

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
            //执行查询
            QueryAction();
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
        private void gdGrid_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            //将Grid中选中的数据赋值给【详情】Tab内的对应控件
            SetGridDataToCardCtrls();
        }

        #region 查询条件相关事件

        #region KeyDown事件

        /// <summary>
        /// 配件名称KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_APA_Name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //执行查询
                QueryAction();
            }
        }
        /// <summary>
        /// 配件编码（原厂编码或第三方编码）KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_AutoPartsCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //执行查询
                QueryAction();
            }
        }
        /// <summary>
        /// 其他描述（专有属性或适用范围中的任一项）KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_OtherDesc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //执行查询
                QueryAction();
            }
        }
        #endregion

        #endregion

        #region 详情相关事件

        string latestAPA_Name = string.Empty;
        /// <summary>
        /// 【详情】配件名称改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbAPA_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            //清空配件品牌和计量单位
            txtAPA_Brand.Clear();
            cbAPA_UOM.Clear();
            cbAPA_UOM.ReadOnly = false;
            cbAPA_UOM.Enabled = true;

            if (string.IsNullOrEmpty(mcbAPA_Name.SelectedText) || mcbAPA_Name.SelectedText == latestAPA_Name)
            {
                return;
            }
            latestAPA_Name = mcbAPA_Name.SelectedText;

            //根据配件名称获取计量单位
            MDLBS_AutoPartsName resultAutoPartsName = new MDLBS_AutoPartsName();
            _bll.QueryForObject<MDLBS_AutoPartsName, MDLBS_AutoPartsName>(new MDLBS_AutoPartsName
            {
                WHERE_APN_Name = mcbAPA_Name.SelectedText,
                WHERE_APN_IsValid = true
            }, resultAutoPartsName);
            if (!string.IsNullOrEmpty(resultAutoPartsName.APN_ID))
            {
                cbAPA_UOM.Text = resultAutoPartsName.APN_UOM ?? string.Empty;
                if (resultAutoPartsName.APN_FixUOM == true && !string.IsNullOrEmpty(cbAPA_UOM.Text))
                {
                    cbAPA_UOM.ReadOnly = true;
                    cbAPA_UOM.Enabled = false;
                }
                else
                {
                    cbAPA_UOM.ReadOnly = false;
                    cbAPA_UOM.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 【详情】查询配件品牌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAPA_Brand_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            //先选择配件名称，才能选配件品牌
            if (string.IsNullOrEmpty(mcbAPA_Name.SelectedValue))
            {
                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0006, new object[] { SystemTableColumnEnums.BS_AutoPartsArchive.Name.APA_Name, SystemActionEnum.Name.QUERY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //查询配件品牌
            FrmAutoPartsBrandQuery frmAutoPartsBrandQuery =
                new FrmAutoPartsBrandQuery(SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Brand,
                paramSelectedValue: this.txtAPA_Brand.Text,
                paramCustomeSelectionMode: CustomEnums.CustomeSelectionMode.Multiple,
                paramItemSelectedItemParentValue: mcbAPA_Name.SelectedValue)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
            DialogResult dialogResult = frmAutoPartsBrandQuery.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                this.txtAPA_Brand.Text = frmAutoPartsBrandQuery.SelectedText;

            }
        }

        /// <summary>
        /// 【详情】仓库改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbWH_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(mcbWH_Name.SelectedValue))
            {
                return;
            }
            mcbWHB_Name.Clear();

            if (_warehouseBinList != null)
            {
                var curWarehouseBinList = _warehouseBinList.Where(x => x.WHB_WH_ID == mcbWH_Name.SelectedValue).ToList();
                mcbWHB_Name.DataSource = curWarehouseBinList;
            }
        }
        /// <summary>
        /// 【详情】车辆品牌改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbAPA_VehicleBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(mcbAPA_VehicleBrand.SelectedValue))
            {
                return;
            }
            mcbAPA_VehicleInspire.Clear();

            if (_vehicleInspireList != null)
            {
                var curVehicleInspireList = _vehicleInspireList.Where(x => !string.IsNullOrEmpty(x.VBIS_Inspire) && mcbAPA_VehicleBrand.SelectedValue.Contains(x.VBIS_Brand)).ToList();
                mcbAPA_VehicleInspire.DataSource = curVehicleInspireList;
            }
        }

        #region 值改变事件

        decimal lastPurchaseUnitPrice = 0;
        /// <summary>
        /// 采购单价值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAPA_PurchaseUnitPrice_ValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPurchaseUnitPrice.Text)
                || txtPurchaseUnitPrice.Text == lastPurchaseUnitPrice.ToString(CultureInfo.InvariantCulture))
            {
                return;
            }

            if (DetailDS == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(txtPurchaseUnitPrice.Text.Trim()))
            {
                DetailDS.PurchaseUnitPrice = null;
            }
            else
            {
                DetailDS.PurchaseUnitPrice = Convert.ToDecimal(txtPurchaseUnitPrice.Text.Trim());
            }

            if (string.IsNullOrEmpty(txtPurchaseQuantity.Text.Trim()))
            {
                DetailDS.PurchaseQuantity = null;
            }
            else
            {
                DetailDS.PurchaseQuantity = Convert.ToDecimal(txtPurchaseQuantity.Text.Trim());
            }
            //计算入库金额
            DetailDS.StockInAmount = CalcStockInAmount(DetailDS.PurchaseUnitPrice, DetailDS.PurchaseQuantity);
            txtStockInAmount.Text = DetailDS.StockInAmount.ToString();
        }

        decimal lastPurchaseQuantity = 0;
        /// <summary>
        /// 采购数量值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPurchaseQuantity_ValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPurchaseQuantity.Text)
                || txtPurchaseQuantity.Text == lastPurchaseQuantity.ToString(CultureInfo.InvariantCulture))
            {
                return;
            }

            if (DetailDS == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(txtPurchaseUnitPrice.Text.Trim()))
            {
                DetailDS.PurchaseUnitPrice = null;
            }
            else
            {
                DetailDS.PurchaseUnitPrice = Convert.ToDecimal(txtPurchaseUnitPrice.Text.Trim());
            }

            if (string.IsNullOrEmpty(txtPurchaseQuantity.Text.Trim()))
            {
                DetailDS.PurchaseQuantity = null;
            }
            else
            {
                DetailDS.PurchaseQuantity = Convert.ToDecimal(txtPurchaseQuantity.Text.Trim());
            }
            //计算入库金额
            DetailDS.StockInAmount = CalcStockInAmount(DetailDS.PurchaseUnitPrice, DetailDS.PurchaseQuantity);
            txtStockInAmount.Text = DetailDS.StockInAmount.ToString();
        }
        #endregion

        #region KeyPress事件
        /// <summary>
        /// 采购单价KeyPress事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPurchaseUnitPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            //只能输入数字和小数点
            if (!(((e.KeyChar >= 48) && (e.KeyChar <= 57)) || e.KeyChar <= 31))
            {
                if (e.KeyChar == 46)
                {
                    if (((UltraTextEditor)sender).Text.Trim().IndexOf('.') > -1)
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (e.KeyChar <= 31)
                {
                    e.Handled = false;
                }
                else if (((UltraTextEditor)sender).Text.Trim().IndexOf('.') > -1)
                {
                    if (((UltraTextEditor)sender).Text.Trim()
                            .Substring(((UltraTextEditor)sender).Text.Trim().IndexOf('.') + 1)
                            .Length >= 4)
                    {
                        e.Handled = true;
                    }
                }
            }
        }
        /// <summary>
        /// 采购数量KeyPress事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPurchaseQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            //只能输入数字
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        #endregion

        #endregion

        #region 按钮事件

        /// <summary>
        /// 添加配件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddAutoParts_Click(object sender, EventArgs e)
        {
            if (ValidateAndSaveConfigInfo())
            {
                bool addResult = false;
                if (_maintainAutoPartsDetailFunc != null)
                {
                    addResult = _maintainAutoPartsDetailFunc(DetailDS);
                }
                if (addResult)
                {
                    DialogResult = DialogResult.OK;
                }
            }
        }
        /// <summary>
        /// 更新配件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateAutoParts_Click(object sender, EventArgs e)
        {
            if (ValidateAndSaveConfigInfo())
            {
                bool updateResult = false;
                if (_maintainAutoPartsDetailFunc != null)
                {
                    DetailDS.Detail_ID = _detailID;
                    updateResult = _maintainAutoPartsDetailFunc(DetailDS);
                }
                if (updateResult)
                {
                    DialogResult = DialogResult.OK;
                }
            }
        }
        /// <summary>
        /// 添加配件并继续
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddAutoPartsAndContinue_Click(object sender, EventArgs e)
        {
            if (ValidateAndSaveConfigInfo())
            {
                bool addResult = false;
                if (_maintainAutoPartsDetailFunc != null)
                {
                    addResult = _maintainAutoPartsDetailFunc(DetailDS);
                }
                if (addResult)
                {
                    _defaultSupplierId = DetailDS.APA_SUPP_ID;
                    DetailDS = new MaintainAutoPartsDetailUIModel();
                    InitializeDetailTabControls();
                }
            }
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseView_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        #endregion
        #endregion

        #region 重写基类方法

        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            _bll.QueryForList<MaintainAutoPartsDetailUIModel>(SQLID.COMM_SQL19, new MaintainAutoPartsDetailQCModel()
            {
                PageSize = PageSize,
                PageIndex = PageIndex,
                //配件名称
                WHERE_APA_Name = mcbWhere_APA_Name.SelectedValue,
                //配件编码（原厂编码或第三方编码）
                WHERE_AutoPartsCode = txtWhere_AutoPartsCode.Text.Trim(),
                //其他描述（专有属性或适用范围中的任一项）
                WHERE_OtherDesc = txtWhere_OtherDesc.Text.Trim(),
                //车型代码
                WHERE_APA_VehicleModelCode = txtWhere_APA_VehicleModelCode.Text.Trim(),
                //互换码
                WHERE_APA_ExchangeCode = txtWhere_APA_ExchangeCode.Text.Trim(),
                //组织ID（为查询当前组织下最近一次入库的信息）
                WHERE_Org_ID = LoginInfoDAX.OrgID,
            }, GridDS);

            //Grid绑定数据源
            gdGrid.DataSource = base.GridDS;
            gdGrid.DataBind();

            if (GridDS.Count > 0)
            {
                dynamic subObject = GridDS[0];

                TotalRecordCount = subObject.RecordCount;
            }
            else
            {
                TotalRecordCount = 0;
            }
            //设置翻页控件
            SetBarPaging(TotalRecordCount);
            //设置单元格是否可以编辑
            SetPayConfirmWindowStyle();
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 工具生成
            //配件名称
            mcbAPA_Name.Clear();
            //原厂编码
            txtAPA_OEMNo.Clear();
            //第三方编码
            txtAPA_ThirdNo.Clear();
            //配件品牌
            txtAPA_Brand.Clear();
            //规格型号
            txtAPA_Specification.Clear();
            //计量单位
            cbAPA_UOM.Items.Clear();
            //变速类型名称
            cbAPA_VehicleGearboxTypeName.Items.Clear();
            //排量
            cbAPA_VehicleCapacity.Items.Clear();
            //年款
            cbAPA_VehicleYearModel.Items.Clear();
            //配件级别
            cbAPA_Level.Items.Clear();
            //品牌
            mcbAPA_VehicleBrand.Clear();
            //车系
            mcbAPA_VehicleInspire.Clear();
            //条形码
            txtAPA_Barcode.Clear();
            //车型代码
            txtAPA_VehicleModelCode.Clear();
            //互换码
            txtAPA_ExchangeCode.Clear();
            //仓库名称
            mcbWH_Name.Clear();
            //仓位名称
            mcbWHB_Name.Clear();
            //配置档案ID
            txtAPA_ID.Clear();
            //组织ID
            txtAPA_Org_ID.Clear();
            //变速类型编码
            txtAPA_VehicleGearboxTypeCode.Clear();
            //版本号
            txtAPA_VersionNo.Clear();
            //给 配件名称 设置焦点
            lblAPA_Name.Focus();
            #endregion

            #region 初始化下拉框

            //配件名称
            _autoPartsNameList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsName) as List<MDLBS_AutoPartsName>;
            mcbAPA_Name.DisplayMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbAPA_Name.ValueMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbAPA_Name.DataSource = _autoPartsNameList;

            //配件级别：从码表获取
            ObservableCollection<CodeTableValueTextModel> autoPartsLevelList = new ObservableCollection<CodeTableValueTextModel>();
            autoPartsLevelList = CodeTableHelp.GetEnumForComboBoxWithValueText(CodeType.AutoPartsLevel);
            cbAPA_Level.DisplayMember = SysConst.EN_TEXT;
            cbAPA_Level.ValueMember = SysConst.Value;
            cbAPA_Level.DataSource = autoPartsLevelList;
            cbAPA_Level.DataBind();

            //排量
            List<string> vehicleCapacityDS = SystemDAX.GetVehicleCapacity();
            cbAPA_VehicleCapacity.DataSource = vehicleCapacityDS;
            cbAPA_VehicleCapacity.DataBind();

            //年款
            List<string> vehicleYearDS = SystemDAX.GetVehicleYearMode();
            cbAPA_VehicleYearModel.DataSource = vehicleYearDS;
            cbAPA_VehicleYearModel.DataBind();

            //计量单位
            List<MDLBS_AutoPartsArchive> resultAutoPartsUOMList = new List<MDLBS_AutoPartsArchive>();
            _bll.QueryForList<MDLBS_AutoPartsArchive>(SQLID.BS_AutoPartsArchiveManager_SQL01, new MDLBS_AutoPartsArchive(), resultAutoPartsUOMList);
            cbAPA_UOM.DisplayMember = SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_UOM;
            cbAPA_UOM.ValueMember = SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_UOM;
            cbAPA_UOM.DataSource = resultAutoPartsUOMList;
            cbAPA_UOM.Value = null;

            //变速类型
            _vehicleGearboxTypeDS = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.GearboxType);
            cbAPA_VehicleGearboxTypeName.DisplayMember = SysConst.EN_TEXT;
            cbAPA_VehicleGearboxTypeName.ValueMember = SysConst.EN_Code;
            cbAPA_VehicleGearboxTypeName.DataSource = _vehicleGearboxTypeDS;
            cbAPA_VehicleGearboxTypeName.DataBind();

            //车辆品牌
            _vehicleBrandList = CacheDAX.Get(CacheDAX.ConfigDataKey.VehicleBrand) as List<MDLBS_VehicleBrandInspireSumma>;
            mcbAPA_VehicleBrand.DisplayMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Brand;
            mcbAPA_VehicleBrand.ValueMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Brand;
            mcbAPA_VehicleBrand.DataSource = _vehicleBrandList;

            //车辆车系
            _vehicleInspireList = CacheDAX.Get(CacheDAX.ConfigDataKey.VehicleBrandInspire) as List<MDLBS_VehicleBrandInspireSumma>;
            mcbAPA_VehicleInspire.DisplayMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Inspire;
            mcbAPA_VehicleInspire.ValueMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Inspire;
            if (_vehicleInspireList != null && !string.IsNullOrEmpty(mcbAPA_VehicleBrand.SelectedValue))
            {
                var curVehicleInspireList = _vehicleInspireList.Where(x => !string.IsNullOrEmpty(x.VBIS_Inspire) && mcbAPA_VehicleBrand.SelectedValue.Contains(x.VBIS_Brand)).ToList();
                mcbAPA_VehicleInspire.DataSource = curVehicleInspireList;
            }

            //默认供应商
            _supplierList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsSupplier) as List<MDLPIS_Supplier>;
            mcbSUPP_Name.DisplayMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_Name;
            mcbSUPP_Name.ValueMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_ID;
            if (mcbSUPP_Name.Enabled)
            {
                mcbSUPP_Name.DataSource = _supplierList;
            }

            //默认仓库
            var allWarehouseList = CacheDAX.Get(CacheDAX.ConfigDataKey.Warehouse) as List<MDLPIS_Warehouse>;
            mcbWH_Name.DisplayMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_Name;
            mcbWH_Name.ValueMember = SystemTableColumnEnums.PIS_Warehouse.Code.WH_ID;
            if (allWarehouseList != null)
            {
                _warehouseList = allWarehouseList.Where(x => x.WH_Org_ID == LoginInfoDAX.OrgID).ToList();
                mcbWH_Name.DataSource = _warehouseList;
            }

            //默认仓位
            _warehouseBinList = CacheDAX.Get(CacheDAX.ConfigDataKey.WarehouseBin) as List<MDLPIS_WarehouseBin>;
            mcbWHB_Name.DisplayMember = SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_Name;
            mcbWHB_Name.ValueMember = SystemTableColumnEnums.PIS_WarehouseBin.Code.WHB_ID;
            if (_warehouseBinList != null && !string.IsNullOrEmpty(mcbWH_Name.SelectedValue))
            {
                var curWarehouseBinList = _warehouseBinList.Where(x => x.WHB_WH_ID == mcbWH_Name.SelectedValue).ToList();
                mcbWHB_Name.DataSource = curWarehouseBinList;
            }
            #endregion
        }
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //配件名称
            mcbWhere_APA_Name.Clear();
            //配件编码（原厂编码或第三方编码）
            txtWhere_AutoPartsCode.Clear();
            //其他描述（专有属性或适用范围中的任一项）
            txtWhere_OtherDesc.Clear();
            //车型代码
            txtWhere_APA_VehicleModelCode.Clear();
            //互换码
            txtWhere_APA_ExchangeCode.Clear();
            //给 配件名称 设置焦点
            lblWhere_APA_Name.Focus();
            #endregion

            #endregion

            //清空Grid
            GridDS = new BindingList<MaintainAutoPartsDetailUIModel>();
            gdGrid.DataSource = GridDS;
            gdGrid.DataBind();

            #region 初始化下拉框

            mcbWhere_APA_Name.DisplayMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbWhere_APA_Name.ValueMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbWhere_APA_Name.DataSource = _autoPartsNameList;
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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            DetailDS = base.GridDS.FirstOrDefault(x => x.APA_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_ID].Value);

            //计算入库金额
            if (DetailDS != null)
            {
                DetailDS.StockInAmount = CalcStockInAmount(DetailDS.PurchaseUnitPrice, DetailDS.PurchaseQuantity);
            }

            //将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //展开GroupBox
            gbBase.Expanded = true;
            gbExclusiveProperties.Expanded = true;
            gbApplicability.Expanded = true;
            gbStockInInfo.Expanded = true;
        }
        /// <summary>
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //配件名称
            mcbAPA_Name.SelectedValue = DetailDS.APA_Name;
            //原厂编码
            txtAPA_OEMNo.Text = DetailDS.APA_OEMNo;
            //第三方编码
            txtAPA_ThirdNo.Text = DetailDS.APA_ThirdNo;
            //配件品牌
            txtAPA_Brand.Text = DetailDS.APA_Brand;
            //规格型号
            txtAPA_Specification.Text = DetailDS.APA_Specification;
            //计量单位
            cbAPA_UOM.Text = DetailDS.APA_UOM ?? string.Empty;
            //配件级别
            cbAPA_Level.Text = DetailDS.APA_Level ?? string.Empty;
            //品牌
            mcbAPA_VehicleBrand.SelectedValue = DetailDS.APA_VehicleBrand;
            if (_vehicleInspireList != null && !string.IsNullOrEmpty(mcbAPA_VehicleBrand.SelectedValue))
            {
                var curVehicleInspireList = _vehicleInspireList.Where(x => !string.IsNullOrEmpty(x.VBIS_Inspire) && mcbAPA_VehicleBrand.SelectedValue.Contains(x.VBIS_Brand)).ToList();
                mcbAPA_VehicleInspire.DataSource = curVehicleInspireList;
            }
            //车系
            mcbAPA_VehicleInspire.SelectedValue = DetailDS.APA_VehicleInspire;
            //排量
            cbAPA_VehicleCapacity.Text = DetailDS.APA_VehicleCapacity ?? string.Empty;
            //年款
            cbAPA_VehicleYearModel.Text = DetailDS.APA_VehicleYearModel ?? string.Empty;
            //变速类型
            cbAPA_VehicleGearboxTypeName.Text = DetailDS.APA_VehicleGearboxTypeName ?? string.Empty;
            //变速类型编码
            txtAPA_VehicleGearboxTypeCode.Text = DetailDS.APA_VehicleGearboxTypeCode;
            //条形码
            txtAPA_Barcode.Text = DetailDS.APA_Barcode;
            //车型代码
            txtAPA_VehicleModelCode.Text = DetailDS.APA_VehicleModelCode;
            //互换码
            txtAPA_ExchangeCode.Text = DetailDS.APA_ExchangeCode;
            //采购单价
            txtPurchaseUnitPrice.Text = DetailDS.PurchaseUnitPrice?.ToString() ?? string.Empty;
            //采购数量
            txtPurchaseQuantity.Text = DetailDS.PurchaseQuantity?.ToString() ?? string.Empty;
            //入库金额
            txtStockInAmount.Text = DetailDS.StockInAmount?.ToString() ?? string.Empty;
            //默认仓库ID
            mcbWH_Name.SelectedValue = DetailDS.APA_WH_ID;
            //仓库名称
            mcbWH_Name.Text = DetailDS.WH_Name;
            //默认仓位ID
            mcbWHB_Name.SelectedValue = DetailDS.APA_WHB_ID;
            //仓位名称
            mcbWHB_Name.Text = DetailDS.WHB_Name;
            //配置档案ID
            txtAPA_ID.Text = DetailDS.APA_ID;
            //组织ID
            txtAPA_Org_ID.Text = DetailDS.APA_Org_ID;
            if (mcbSUPP_Name.Enabled == true)
            {
                //供应商名称
                mcbSUPP_Name.SelectedText = DetailDS.SUPP_Name;
                mcbSUPP_Name.SelectedValue = DetailDS.APA_SUPP_ID;
            }
            //版本号
            txtAPA_VersionNo.Text = DetailDS.APA_VersionNo?.ToString() ?? string.Empty;

            #region 设置计量单位
            //固定计量单位
            if (DetailDS.APN_FixUOM == true && !string.IsNullOrEmpty(DetailDS.APA_UOM))
            {
                cbAPA_UOM.ReadOnly = true;
                cbAPA_UOM.Enabled = false;
            }
            else
            {
                cbAPA_UOM.ReadOnly = false;
                cbAPA_UOM.Enabled = true;
            }
            #endregion

            #region 设置默认供应商、仓库、仓位
            //供应商
            if (!string.IsNullOrEmpty(_defaultSupplierId))
            {
                MDLPIS_Supplier resultSupplier = new MDLPIS_Supplier();
                _bll.QueryForObject<MDLPIS_Supplier, MDLPIS_Supplier>(new MDLPIS_Supplier
                {
                    WHERE_SUPP_ID = _defaultSupplierId,
                    WHERE_SUPP_IsValid = true
                }, resultSupplier);
                if (!string.IsNullOrEmpty(resultSupplier.SUPP_ID))
                {
                    mcbSUPP_Name.SelectedText = resultSupplier.SUPP_Name;
                    mcbSUPP_Name.SelectedValue = resultSupplier.SUPP_ID;
                }
            }
            //仓库
            if (!string.IsNullOrEmpty(_defaultWarehouseId))
            {
                MDLPIS_Warehouse resultWarehouse = new MDLPIS_Warehouse();
                _bll.QueryForObject<MDLPIS_Warehouse, MDLPIS_Warehouse>(new MDLPIS_Warehouse
                {
                    WHERE_WH_ID = _defaultWarehouseId,
                    WHERE_WH_IsValid = true
                }, resultWarehouse);
                if (!string.IsNullOrEmpty(resultWarehouse.WH_ID))
                {
                    mcbWH_Name.SelectedText = resultWarehouse.WH_Name;
                    mcbWH_Name.SelectedValue = resultWarehouse.WH_ID;
                }
            }
            //仓位
            if (!string.IsNullOrEmpty(_defaultWarehouseBinId))
            {
                MDLPIS_WarehouseBin resultWarehouseBin = new MDLPIS_WarehouseBin();
                _bll.QueryForObject<MDLPIS_WarehouseBin, MDLPIS_WarehouseBin>(new MDLPIS_WarehouseBin
                {
                    WHERE_WHB_ID = _defaultWarehouseBinId,
                    WHERE_WHB_IsValid = true
                }, resultWarehouseBin);
                if (!string.IsNullOrEmpty(resultWarehouseBin.WHB_ID))
                {
                    mcbWHB_Name.SelectedText = resultWarehouseBin.WHB_Name;
                    mcbWHB_Name.SelectedValue = resultWarehouseBin.WHB_ID;
                }
            }
            #endregion
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
                foreach (UltraGridColumn loopSortedColumn in gdGrid.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (loopSortedColumn.IsGroupByColumn)
                    {
                        InitializeDetailTabControls();
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            DetailDS = new MaintainAutoPartsDetailUIModel()
            {
                //配件名称
                APA_Name = mcbAPA_Name.SelectedValue,
                //原厂编码
                APA_OEMNo = txtAPA_OEMNo.Text.Trim(),
                //第三方编码
                APA_ThirdNo = txtAPA_ThirdNo.Text.Trim(),
                //配件品牌
                APA_Brand = txtAPA_Brand.Text.Trim(),
                //规格型号
                APA_Specification = txtAPA_Specification.Text.Trim(),
                //计量单位
                APA_UOM = cbAPA_UOM.Text.Trim(),
                //配件级别
                APA_Level = cbAPA_Level.Text.Trim(),
                //品牌
                APA_VehicleBrand = mcbAPA_VehicleBrand.SelectedValue,
                //车系
                APA_VehicleInspire = mcbAPA_VehicleInspire.SelectedValue,
                //排量
                APA_VehicleCapacity = cbAPA_VehicleCapacity.Text.Trim(),
                //年款
                APA_VehicleYearModel = cbAPA_VehicleYearModel.Text.Trim(),
                //变速类型
                APA_VehicleGearboxTypeName = cbAPA_VehicleGearboxTypeName.Text.Trim(),
                //变速类型编码
                APA_VehicleGearboxTypeCode = txtAPA_VehicleGearboxTypeCode.Text.Trim(),
                //条形码
                APA_Barcode = txtAPA_Barcode.Text.Trim(),
                //车型代码
                APA_VehicleModelCode = txtAPA_VehicleModelCode.Text.Trim(),
                //互换码
                APA_ExchangeCode = txtAPA_ExchangeCode.Text.Trim(),
                //采购单价
                PurchaseUnitPrice = Convert.ToDecimal(txtPurchaseUnitPrice.Text.Trim() == "" ? "0" : txtPurchaseUnitPrice.Text.Trim()),
                //采购数量
                PurchaseQuantity = Convert.ToDecimal(txtPurchaseQuantity.Text.Trim() == "" ? "0" : txtPurchaseQuantity.Text.Trim()),
                //采购金额
                StockInAmount = Convert.ToDecimal(txtStockInAmount.Text.Trim() == "" ? "0" : txtStockInAmount.Text.Trim()),
                //供应商名称
                SUPP_Name = mcbSUPP_Name.SelectedText,
                //默认供应商ID
                APA_SUPP_ID = mcbSUPP_Name.SelectedValue,
                //仓库名称
                WH_Name = mcbWH_Name.SelectedText,
                //默认仓库ID
                APA_WH_ID = mcbWH_Name.SelectedValue,
                //仓位名称
                WHB_Name = mcbWHB_Name.SelectedText,
                //默认仓位ID
                APA_WHB_ID = mcbWHB_Name.SelectedValue,
                //配置档案ID
                APA_ID = txtAPA_ID.Text.Trim(),
                //组织ID
                APA_Org_ID = txtAPA_Org_ID.Text.Trim(),
                //版本号
                APA_VersionNo = Convert.ToInt64(txtAPA_VersionNo.Text.Trim() == "" ? "1" : txtAPA_VersionNo.Text.Trim()),
            };
        }

        /// <summary>
        /// 计算入库金额
        /// </summary>
        /// <param name="paramUnitPrice">单价</param>
        /// <param name="paramQuantity">数量</param>
        private decimal CalcStockInAmount(decimal? paramUnitPrice, decimal? paramQuantity)
        {
            decimal stockInAmount = 0;

            if (paramUnitPrice == null || paramUnitPrice == 0
                || paramQuantity == null || paramQuantity == 0)
            {
                return stockInAmount;
            }

            //int dotIndex = paramQuantity.ToString().IndexOf(".", StringComparison.Ordinal);
            //if (dotIndex > 0)
            //{
            //    int qtyDecimalLen = 0;
            //    if (_purchaseQtyFormatString.Length == 2)
            //    {
            //        int.TryParse(_purchaseQtyFormatString.Substring(1), out qtyDecimalLen);
            //    }
            //    if (qtyDecimalLen == 0)
            //    {
            //        paramQuantity = (int)paramQuantity;
            //    }
            //    else
            //    {
            //        decimal tempPurchaseQuantity = 0;
            //        decimal.TryParse(paramQuantity.ToString().Substring(0, Math.Min(dotIndex + 1 + qtyDecimalLen, paramQuantity.ToString().Length)), out tempPurchaseQuantity);
            //        paramQuantity = tempPurchaseQuantity;
            //    }
            //}
            stockInAmount = Math.Round(Convert.ToDecimal(paramUnitPrice * paramQuantity), 2);

            return stockInAmount;
        }
        /// <summary>
        /// 验证以及保存配置信息
        /// </summary>
        /// <returns></returns>
        private bool ValidateAndSaveConfigInfo()
        {
            //将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();

            //验证采购单价
            if (DetailDS.PurchaseUnitPrice == null || DetailDS.PurchaseUnitPrice == 0)
            {
                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.PIS_StockInDetail.Name.SID_UnitCostPrice }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //验证采购数量
            if (DetailDS.PurchaseQuantity == null || DetailDS.PurchaseQuantity == 0)
            {
                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.PIS_StockInDetail.Name.SID_Qty }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //验证供应商
            if (string.IsNullOrEmpty(DetailDS.APA_SUPP_ID)
                || string.IsNullOrEmpty(DetailDS.SUPP_Name))
            {
                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { MsgParam.SUPPLIER }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //验证仓库
            if (string.IsNullOrEmpty(DetailDS.APA_WH_ID)
                || string.IsNullOrEmpty(DetailDS.WH_Name))
            {
                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { MsgParam.WAREHOUSE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //如果条码可编辑
            if (DetailDS.BarcodeKeyInfoEditable != false)
            {
                //验证配件信息
                if (!ValidateAutoPartsInfo(DetailDS))
                {
                    return false;
                }
                //同步配件档案
                _bll.SyncAutoPartsArchive(DetailDS, true);
            }
            //验证条形码
            if (string.IsNullOrEmpty(DetailDS.APA_Barcode))
            {
                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.BS_AutoPartsArchive.Name.APA_Barcode }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 验证配件信息
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        private bool ValidateAutoPartsInfo(MaintainAutoPartsDetailUIModel paramModel)
        {
            //验证配件名称
            if (string.IsNullOrEmpty(DetailDS.APA_Name))
            {
                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.BS_AutoPartsArchive.Name.APA_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //验证计量单位
            if (string.IsNullOrEmpty(DetailDS.APA_UOM))
            {
                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.BS_AutoPartsArchive.Name.APA_UOM }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //验证配件级别
            if (string.IsNullOrEmpty(DetailDS.APA_Level))
            {
                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.BS_AutoPartsArchive.Name.APA_Level }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //验证专有属性或适用范围
            var infoIsValid = paramModel != null && (!string.IsNullOrEmpty(paramModel.APA_Brand)
                                                    ||
                                                    !string.IsNullOrEmpty(paramModel.APA_Specification)
                                                    ||
                                                    !string.IsNullOrEmpty(paramModel.APA_VehicleBrand)
                                                    ||
                                                    !string.IsNullOrEmpty(paramModel.APA_VehicleInspire)
                                                    ||
                                                    !string.IsNullOrEmpty(paramModel.APA_VehicleYearModel)
                                                    ||
                                                    !string.IsNullOrEmpty(paramModel.APA_VehicleCapacity)
                                                    ||
                                                    !string.IsNullOrEmpty(paramModel.APA_VehicleGearboxTypeName));
            if (!infoIsValid)
            {
                //必须至少填入专有属性或适用范围中的一项
                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0021, new object[] { MsgParam.EXCLUSIVE_PROPERTY + MsgParam.OR + MsgParam.APPLICABILITY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            #region 验证原厂编码和第三方编码是否被其他配件名称使用
            if (!string.IsNullOrEmpty(paramModel.APA_Name))
            {
                if (!string.IsNullOrEmpty(paramModel.APA_OEMNo))
                {
                    //验证原厂编码是否被其他配件名称使用
                    if (!AutoPartsComFunction.ValidateOEMNoAndThirdNo(paramModel.APA_Name, paramModel.APA_OEMNo, null))
                    {
                        DialogResult dialogResult = MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0019, new object[] { SystemTableColumnEnums.BS_AutoPartsArchive.Name.APA_OEMNo, SystemTableColumnEnums.BS_AutoPartsArchive.Name.APA_Name, MsgParam.ADD }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dialogResult != DialogResult.OK)
                        {
                            return false;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(paramModel.APA_ThirdNo))
                {
                    //验证第三方编码是否被其他配件名称使用
                    if (!AutoPartsComFunction.ValidateOEMNoAndThirdNo(paramModel.APA_Name, null, paramModel.APA_ThirdNo))
                    {
                        DialogResult dialogResult = MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0019, new object[] { SystemTableColumnEnums.BS_AutoPartsArchive.Name.APA_ThirdNo, SystemTableColumnEnums.BS_AutoPartsArchive.Name.APA_Name, MsgParam.ADD }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dialogResult != DialogResult.OK)
                        {
                            return false;
                        }
                    }
                }

            }
            #endregion

            return true;
        }

        #region Grid翻页相关方法

        #region 公共
        /// <summary>
        /// 设置翻页控件
        /// <para>窗体加载或初始化时调用</para>
        /// </summary>
        /// <param name="paramTotalRecordCount">总记录条数</param>
        private new void SetBarPaging(int paramTotalRecordCount)
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

        private new void ToolBarPaging_ToolClick(object sender, ToolClickEventArgs e)
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
        private new void ToolBarPaging_ToolValueChanged(object sender, ToolEventArgs e)
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
