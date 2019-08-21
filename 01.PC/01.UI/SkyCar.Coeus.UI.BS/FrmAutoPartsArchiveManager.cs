using System;
using System.Collections.Generic;
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
using Infragistics.Win.UltraWinEditors;
using SkyCar.Coeus.Common.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;
using Infragistics.Win;
using Infragistics.Win.UltraWinTabControl;
using SkyCar.Coeus.BLL.COM;
using SkyCar.Coeus.Common.CustomControl;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.Common.APModel;
using SkyCar.Coeus.Common.FileIO;

namespace SkyCar.Coeus.UI.BS
{
    /// <summary>
    /// 配件档案
    /// </summary >
    public partial class FrmAutoPartsArchiveManager : BaseFormCardList<AutoPartsArchiveManagerUIModel, AutoPartsArchiveManagerQCModel, MDLBS_AutoPartsArchive>
    {
        #region 全局变量

        /// <summary>
        /// 配件档案BLL
        /// </summary>
        private AutoPartsArchiveManagerBLL _bll = new AutoPartsArchiveManagerBLL();

        /// <summary>
        /// 配件价格明细数据源
        /// </summary>
        private SkyCarBindingList<AutoPartsPriceTypeUIModel, MDLBS_AutoPartsPriceType> _detailGridDS =
            new SkyCarBindingList<AutoPartsPriceTypeUIModel, MDLBS_AutoPartsPriceType>();

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

        /// <summary>
        /// 年款数据源
        /// </summary>
        List<string> _vehicleYearDS = new List<string>();

        /// <summary>
        /// 排量数据源
        /// </summary>
        List<string> _vehicleCapacityDS = new List<string>();

        /// <summary>
        /// 配件级别数据源
        /// </summary>
        ObservableCollection<CodeTableValueTextModel> _autoPartsLevelDS = new ObservableCollection<CodeTableValueTextModel>();

        /// <summary>
        /// 配件价格类别
        /// </summary>
        ObservableCollection<CodeTableValueTextModel> _autoPartsPriceTypeList = new ObservableCollection<CodeTableValueTextModel>();

        #endregion

        /// <summary>
        /// 【详情】配件图片列表
        /// </summary>
        List<AutoPartsPictureUIModel> _autoPartsPictureList = new List<AutoPartsPictureUIModel>();

        /// <summary>
        /// 【详情】配件图片控件列表
        /// </summary>
        List<SkyCarPictureExpand> _pictureExpandList = new List<SkyCarPictureExpand>();

        /// <summary>
        /// 界面属性值发生变化时不予检查的属性列表
        /// </summary>
        List<string> _skipPropertyList = new List<string>();

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmAutoPartsArchiveManager构造方法
        /// </summary>
        public FrmAutoPartsArchiveManager()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAutoPartsArchiveManager_Load(object sender, EventArgs e)
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
            AcceptUIModelChanges();
            #endregion

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            AcceptUIModelChanges();

            #region 界面发生变化时不予检查的属性值

            _skipPropertyList.Add("WH_Name");
            _skipPropertyList.Add("WHB_Name");
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
        /// 【列表】查询配件品牌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_APA_Brand_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            //先选择配件名称，才能选配件品牌
            if (string.IsNullOrEmpty(mcbWhere_APA_Name.SelectedValue)
                || string.IsNullOrEmpty(mcbWhere_APA_Name.SelectedText))
            {
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0006, new object[] { SystemTableColumnEnums.BS_AutoPartsArchive.Name.APA_Name, SystemActionEnum.Name.QUERY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //查询配件品牌
            FrmAutoPartsBrandQuery frmAutoPartsBrandQuery =
                new FrmAutoPartsBrandQuery(SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Brand,
                paramSelectedValue: this.txtWhere_APA_Brand.Text,
                paramCustomeSelectionMode: CustomEnums.CustomeSelectionMode.Multiple,
                paramItemSelectedItemParentValue: this.mcbWhere_APA_Name.SelectedText)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
            DialogResult dialogResult = frmAutoPartsBrandQuery.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                this.txtWhere_APA_Brand.Text = frmAutoPartsBrandQuery.SelectedText;

            }
        }
        #endregion

        #region 详情相关事件

        private string latestAPA_Name = string.Empty;
        /// <summary>
        /// 【详情】配件名称改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbAPA_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(mcbAPA_Name.SelectedValue) || mcbAPA_Name.SelectedText == latestAPA_Name)
            {
                return;
            }
            latestAPA_Name = mcbAPA_Name.SelectedText;

            //清空配件品牌和计量单位
            txtAPA_Brand.Clear();
            cbAPA_UOM.Clear();
            cbAPA_UOM.ReadOnly = false;
            cbAPA_UOM.Enabled = true;

            var curAutoPartsName = _autoPartsNameList.FirstOrDefault(x => x.APN_Name == mcbAPA_Name.SelectedValue);
            if (curAutoPartsName != null)
            {
                numAPA_SlackDays.Value = curAutoPartsName.APN_SlackDays;
            }
            //根据配件名称设置计量单位
            SetDetailFixedUom();
        }

        /// <summary>
        /// 【详情】查询配件品牌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAPA_Brand_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            //先选择配件名称，才能选配件品牌
            if (string.IsNullOrEmpty(mcbAPA_Name.SelectedValue)
                || string.IsNullOrEmpty(mcbAPA_Name.SelectedText))
            {
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0006, new object[] { SystemTableColumnEnums.BS_AutoPartsArchive.Name.APA_Name, SystemActionEnum.Name.QUERY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //查询配件品牌
            FrmAutoPartsBrandQuery frmAutoPartsBrandQuery =
                new FrmAutoPartsBrandQuery(SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Brand,
                paramSelectedValue: this.txtAPA_Brand.Text,
                paramCustomeSelectionMode: CustomEnums.CustomeSelectionMode.Multiple,
                paramItemSelectedItemParentValue: mcbAPA_Name.SelectedText)
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

        /// <summary>
        /// 默认仓库改变事件
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

        #endregion

        #region 配件价格明细相关事件

        /// <summary>
        /// 【详情】Tab内Grid动作按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolbarsManagerDetail_ToolClick(object sender, ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                //添加
                case SysConst.EN_ADD:
                    AddAutoPartsPriceDetail();
                    break;
                //删除
                case SysConst.EN_DEL:
                    DeleteAutoPartsPriceDetail();
                    break;
            }
        }

        /// <summary>
        /// 配件价格明细Grid的单元格下拉框选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_CellListSelect(object sender, CellEventArgs e)
        {
            gdDetail.UpdateData();
        }
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

            //控制详情是否可编辑
            SetDetailControl();

            //将最新的值Copy到初始UIModel
            SetCardCtrlsToDetailDS();
            this.AcceptUIModelChanges();
        }
        /// <summary>
        /// 保存
        /// </summary>
        public override void SaveAction()
        {
            gdDetail.UpdateData();
            //1.前端检查-保存
            if (!ClientCheckForSave())
            {
                return;
            }
            //2.将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();
            //3.执行保存（含服务端检查）
            bool saveDetailDsResult = _bll.SaveDetailDS(DetailDS, _detailGridDS, _autoPartsPictureList);
            if (!saveDetailDsResult)
            {
                //保存失败
                MessageBoxs.Show(Trans.BS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            _detailGridDS.StartMonitChanges();

            //刷新列表
            RefreshList();

            //4.将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //控制详情是否可编辑
            SetDetailControl();
            //控制[配件价格明细]Grid的单元格样式
            SetAutoPartsPriceDetailStyle();

            //为图片控件设置传入的配件图片Model
            foreach (var loopPicture in _autoPartsPictureList)
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
            //1.设置查询条件（翻页相关属性不用设置）
            base.ConditionDS = new AutoPartsArchiveManagerQCModel()
            {
                //SqlId
                SqlId = SQLID.BS_AutoPartsArchiveManager_SQL02,
                //条形码
                WHERE_APA_Barcode = txtWhere_APA_Barcode.Text.Trim(),
                //配件编码（原厂编码或第三方编码）
                WHERE_AutoPartsCode = txtWhere_AutoPartsCode.Text.Trim(),
                //配件名称
                WHERE_APA_Name = mcbWhere_APA_Name.SelectedText,
                //配件品牌
                WHERE_APA_Brand = txtWhere_APA_Brand.Text.Trim(),
                //规格型号
                WHERE_APA_Specification = txtWhere_APA_Specification.Text.Trim(),
                //配件级别
                WHERE_APA_Level = cbWhere_APA_Level.Text.Trim(),
                //默认供应商
                WHERE_SUPP_Name = mcbWhere_SUPP_Name.SelectedText,
                //车型代码
                WHERE_APA_VehicleModelCode = txtWhere_APA_VehicleModelCode.Text.Trim(),
                //互换码
                WHERE_APA_ExchangeCode = txtWhere_APA_ExchangeCode.Text.Trim()
            };
            //2.执行基类方法
            base.QueryAction();
            //3.Grid绑定数据源
            gdGrid.DataSource = base.GridDS;
            gdGrid.DataBind();
            //4.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
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
            if (ViewHasChanged(_skipPropertyList)
                || _detailGridDS.InsertList.Count > 0
                || _detailGridDS.UpdateList.Count > 0
                || _detailGridDS.DeleteList.Count > 0)
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
            paramGridName = SystemTableEnums.Name.BS_AutoPartsArchive;
            base.ExportAction(gdGrid, paramGridName);
        }
        /// <summary>
        /// 导出全部
        /// </summary>
        public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = SystemTableEnums.Name.BS_AutoPartsArchive;
            List<AutoPartsArchiveManagerUIModel> resultAllList = new List<AutoPartsArchiveManagerUIModel>();
            _bll.QueryForList(SQLID.BS_AutoPartsArchiveManager_SQL02, new AutoPartsArchiveManagerQCModel()
            {
                PageIndex = 1,
                PageSize = null,
                //条形码
                WHERE_APA_Barcode = txtWhere_APA_Barcode.Text.Trim(),
                //配件编码（原厂编码或第三方编码）
                WHERE_AutoPartsCode = txtWhere_AutoPartsCode.Text.Trim(),
                //配件名称
                WHERE_APA_Name = mcbWhere_APA_Name.SelectedText,
                //配件品牌
                WHERE_APA_Brand = txtWhere_APA_Brand.Text.Trim(),
                //规格型号
                WHERE_APA_Specification = txtWhere_APA_Specification.Text.Trim(),
                //配件级别
                WHERE_APA_Level = cbWhere_APA_Level.Text.Trim(),
                //默认供应商
                WHERE_SUPP_Name = mcbWhere_SUPP_Name.SelectedText,
                //车型代码
                WHERE_APA_VehicleModelCode = txtWhere_APA_VehicleModelCode.Text.Trim(),
                //互换码
                WHERE_APA_ExchangeCode = txtWhere_APA_ExchangeCode.Text.Trim()
            }, resultAllList);
            UltraGrid allGrid = gdGrid;
            allGrid.DataSource = resultAllList;
            allGrid.DataBind();

            base.ExportAllAction(allGrid, paramGridName);

            gdGrid.DataSource = GridDS;
            gdGrid.DataBind();
        }

        /// <summary>
        /// 导入
        /// </summary>
        public override void ImportAction()
        {
            //return;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "*.xlsx";
            ofd.Filter = FormatConst.FILE_TYPE_FILTER_EXCEL_01;

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            DataTable clientDataTable = null;
            try
            {
                clientDataTable = NPOIExcelHelper.ReadExcelToDataTable(ofd.FileName);
            }
            catch (Exception ex)
            {
                //导入失败，失败原因：ex.Message
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.IMPORT, ex.Message }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (clientDataTable == null || clientDataTable.Rows.Count == 0)
            {
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "Excel不包含有效数据" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (clientDataTable.Rows.Count > 2000)
            {
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "一次最多能导入2000条" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!clientDataTable.Columns.Contains("配件名称"))
            {
                DialogResult dialogResult = MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { "Excel表的第一行必须包含列：配件名称。\n需要下载导入模板吗？" }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                ExecuteDownloadImportTemplateCommand();
            }

            //待导入的配件档案列表
            List<ImportAutoPartsArchiveUIModel> importAutoPartsArchiveList = new List<ImportAutoPartsArchiveUIModel>();

            //第一行为列标题
            int rowIndex = 1;
            foreach (DataRow loopClientRow in clientDataTable.Rows)
            {
                ImportAutoPartsArchiveUIModel importAutoPartsArchive = new ImportAutoPartsArchiveUIModel();
                importAutoPartsArchive.InfoRowNumber = rowIndex;

                //Excel中数据从第二行开始
                rowIndex++;

                if (loopClientRow["配件名称"] == null || string.IsNullOrEmpty(loopClientRow["配件名称"].ToString().Trim()))
                {
                    MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "第" + rowIndex + "行，配件名称 不能为空。\n如果是空白行，请删除。" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                importAutoPartsArchive.APA_Name = loopClientRow["配件名称"].ToString().Trim();

                if (loopClientRow["原厂编码"] != null)
                {
                    importAutoPartsArchive.APA_OEMNo = loopClientRow["原厂编码"].ToString().Trim();
                }
                if (loopClientRow["第三方编码"] != null)
                {
                    importAutoPartsArchive.APA_ThirdNo = loopClientRow["第三方编码"].ToString().Trim();
                }
                if (loopClientRow["配件品牌"] != null)
                {
                    importAutoPartsArchive.APA_Brand = loopClientRow["配件品牌"].ToString().Trim();
                }
                if (loopClientRow["规格型号"] != null)
                {
                    importAutoPartsArchive.APA_Specification = loopClientRow["规格型号"].ToString().Trim();
                }
                if (loopClientRow["计量单位"] != null)
                {
                    importAutoPartsArchive.APA_UOM = loopClientRow["计量单位"].ToString().Trim();
                }
                if (loopClientRow["配件级别"] != null)
                {
                    importAutoPartsArchive.APA_Level = loopClientRow["配件级别"].ToString().Trim();
                }
                if (loopClientRow["车辆品牌"] != null)
                {
                    importAutoPartsArchive.APA_VehicleBrand = loopClientRow["车辆品牌"].ToString().Trim();
                }
                if (loopClientRow["车系"] != null)
                {
                    importAutoPartsArchive.APA_VehicleInspire = loopClientRow["车系"].ToString().Trim();
                }
                if (loopClientRow["排量"] != null)
                {
                    importAutoPartsArchive.APA_VehicleCapacity = loopClientRow["排量"].ToString().Trim();
                }
                if (loopClientRow["年款"] != null)
                {
                    importAutoPartsArchive.APA_VehicleYearModel = loopClientRow["年款"].ToString().Trim();
                }
                if (loopClientRow["变速类型"] != null)
                {
                    importAutoPartsArchive.APA_VehicleGearboxType = loopClientRow["变速类型"].ToString().Trim();
                }
                importAutoPartsArchive.APA_SourceType = "商户";
                importAutoPartsArchiveList.Add(importAutoPartsArchive);
            }

            //通过平台接口导入平台
            if (!_bll.ImportAutoPartsArchive(importAutoPartsArchiveList))
            {
                //导入失败
                MessageBoxs.Show(Trans.BS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //导入成功
            MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.IMPORT }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            QueryAction();
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 工具生成
            //条形码
            txtAPA_Barcode.Clear();
            //配件名称
            mcbAPA_Name.Clear();
            //配件品牌
            txtAPA_Brand.Clear();
            //配件级别
            cbAPA_Level.Items.Clear();
            //原厂编码
            txtAPA_OEMNo.Clear();
            //第三方编码
            txtAPA_ThirdNo.Clear();
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
            //安全库存是否预警
            ckAPA_IsWarningSafeStock.Checked = true;
            ckAPA_IsWarningSafeStock.CheckState = CheckState.Checked;
            //安全库存
            numAPA_SafeStockNum.Value = null;
            //呆滞件是否预警
            ckAPA_IsWarningDeadStock.Checked = true;
            ckAPA_IsWarningDeadStock.CheckState = CheckState.Checked;
            //呆滞天数
            numAPA_SlackDays.Value = null;
            //车辆品牌
            mcbAPA_VehicleBrand.Clear();
            //车系
            mcbAPA_VehicleInspire.Clear();
            //销价系数
            numAPA_SalePriceRate.Value = null;
            //销价
            numAPA_SalePrice.Value = null;
            //车型代码
            txtAPA_VehicleModelCode.Clear();
            //互换码
            txtAPA_ExchangeCode.Clear();
            //有效
            ckAPA_IsValid.Checked = true;
            ckAPA_IsValid.CheckState = CheckState.Checked;
            //配置档案ID
            txtAPA_ID.Clear();
            //组织ID
            txtAPA_Org_ID.Clear();
            //变速类型编码
            txtAPA_VehicleGearboxTypeCode.Clear();
            //版本号
            txtAPA_VersionNo.Clear();
            //创建人
            txtAPA_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtAPA_CreatedTime.Value = BLLCom.GetCurStdDatetime();
            //修改人
            txtAPA_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtAPA_UpdatedTime.Value = BLLCom.GetCurStdDatetime();
            //默认供应商
            mcbSUPP_Name.Clear();
            //默认仓库
            mcbWH_Name.Clear();
            //默认仓位
            mcbWHB_Name.Clear();
            //给 条形码 设置焦点
            lblAPA_Barcode.Focus();
            #endregion

            #region 初始化下拉框
            //配件名称
            _autoPartsNameList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsName) as List<MDLBS_AutoPartsName>;
            mcbAPA_Name.DisplayMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbAPA_Name.ValueMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbAPA_Name.DataSource = _autoPartsNameList;

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
                var curVehicleInspireList = _vehicleInspireList.Where(x => x.VBIS_Brand == mcbAPA_VehicleBrand.SelectedValue).ToList();
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

            #region 初始化配件级别
            //从码表获取
            _autoPartsLevelDS = CodeTableHelp.GetEnumForComboBoxWithValueText(CodeType.AutoPartsLevel);
            cbAPA_Level.DisplayMember = SysConst.EN_TEXT;
            cbAPA_Level.ValueMember = SysConst.Value;
            cbAPA_Level.DataSource = _autoPartsLevelDS;
            cbAPA_Level.DataBind();

            #endregion

            #region 初始化排量下拉列表

            _vehicleCapacityDS = SystemDAX.GetVehicleCapacity();
            cbAPA_VehicleCapacity.DataSource = _vehicleCapacityDS;
            cbAPA_VehicleCapacity.DataBind();

            #endregion

            #region 初始化年款下拉列表

            _vehicleYearDS = SystemDAX.GetVehicleYearMode();
            cbAPA_VehicleYearModel.DataSource = _vehicleYearDS;
            cbAPA_VehicleYearModel.DataBind();
            #endregion

            #region 初始化计量单位下拉列表
            List<MDLBS_AutoPartsArchive> resultAutoPartsUOMList = new List<MDLBS_AutoPartsArchive>();
            _bll.QueryForList(SQLID.BS_AutoPartsArchiveManager_SQL01, new MDLBS_AutoPartsArchive(), resultAutoPartsUOMList);
            cbAPA_UOM.DisplayMember = SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_UOM;
            cbAPA_UOM.ValueMember = SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_UOM;
            cbAPA_UOM.DataSource = resultAutoPartsUOMList;
            cbAPA_UOM.Value = null;
            #endregion

            #region 初始化变速类型下拉列表
            _vehicleGearboxTypeDS = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.GearboxType);
            cbAPA_VehicleGearboxTypeName.DisplayMember = SysConst.EN_TEXT;
            cbAPA_VehicleGearboxTypeName.ValueMember = SysConst.EN_Code;
            cbAPA_VehicleGearboxTypeName.DataSource = _vehicleGearboxTypeDS;
            cbAPA_VehicleGearboxTypeName.DataBind();
            #endregion

            //配件价格类别（从码表获取）
            _autoPartsPriceTypeList = CodeTableHelp.GetEnumForComboBoxWithValueText(CodeType.AutoPartsPriceType);

            #endregion

            //初始化[配件价格明细]列表
            _detailGridDS = new SkyCarBindingList<AutoPartsPriceTypeUIModel, MDLBS_AutoPartsPriceType>();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //开始监控List变化（该数据源若有其他调整，请在StartMonitChanges()方法执行前调整）
            _detailGridDS.StartMonitChanges();
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);

            //清空所有图片控件
            _autoPartsPictureList = new List<AutoPartsPictureUIModel>();
            flowLayoutPanelPicture.Controls.Clear();
            _pictureExpandList.Clear();
        }
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //条形码
            txtWhere_APA_Barcode.Clear();
            //配件名称
            mcbWhere_APA_Name.Clear();
            //配件品牌
            txtWhere_APA_Brand.Clear();
            //规格型号
            txtWhere_APA_Specification.Clear();
            //配件级别
            cbWhere_APA_Level.Clear();
            //默认供应商
            mcbWhere_SUPP_Name.Clear();
            //配件编码
            txtWhere_AutoPartsCode.Clear();
            //车型代码
            txtWhere_APA_VehicleModelCode.Clear();
            //互换码
            txtWhere_APA_ExchangeCode.Clear();
            //给 条形码 设置焦点
            lblWhere_APA_Barcode.Focus();
            #endregion

            //清空Grid
            GridDS = new BindingList<AutoPartsArchiveManagerUIModel>();
            gdGrid.DataSource = base.GridDS;
            gdGrid.DataBind();

            #endregion

            #region 初始化下拉框

            //配件名称
            mcbWhere_APA_Name.DisplayMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbWhere_APA_Name.ValueMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_ID;
            mcbWhere_APA_Name.DataSource = _autoPartsNameList;

            //默认供应商
            mcbWhere_SUPP_Name.DisplayMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_Name;
            mcbWhere_SUPP_Name.ValueMember = SystemTableColumnEnums.PIS_Supplier.Code.SUPP_ID;
            mcbWhere_SUPP_Name.DataSource = _supplierList;

            //配件级别
            //从码表获取
            cbWhere_APA_Level.DisplayMember = SysConst.EN_TEXT;
            cbWhere_APA_Level.ValueMember = SysConst.Value;
            cbWhere_APA_Level.DataSource = _autoPartsLevelDS;
            cbWhere_APA_Level.DataBind();

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
            DetailDS = GridDS.FirstOrDefault(x => x.APA_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_ID].Value);
            if (DetailDS == null || string.IsNullOrEmpty(DetailDS.APA_ID))
            {
                return;
            }

            if (txtAPA_ID.Text != DetailDS.APA_ID
                || (txtAPA_ID.Text == DetailDS.APA_ID && txtAPA_VersionNo.Text != DetailDS.APA_VersionNo?.ToString()))
            {
                if (txtAPA_ID.Text == DetailDS.APA_ID && txtAPA_VersionNo.Text != DetailDS.APA_VersionNo?.ToString())
                {
                    //数据版本已过期，将加载最新详情。
                    MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.DataHasOverdue }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else if (ViewHasChanged(_skipPropertyList)
                || _detailGridDS.InsertList.Count > 0
                || _detailGridDS.UpdateList.Count > 0
                || _detailGridDS.DeleteList.Count > 0)
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

            //查询配件价格明细
            QueryDetail();
            //控制详情是否可编辑
            SetDetailControl();
            //控制[配件价格明细]Grid的单元格样式
            SetAutoPartsPriceDetailStyle();

            //加载配件图片
            LoadAutoPartsPicture();
        }

        /// <summary>
        /// 根据配件名称获取计量单位
        /// </summary>
        private void SetDetailFixedUom()
        {
            //根据配件名称获取计量单位
            MDLBS_AutoPartsName resultAutoPartsName = new MDLBS_AutoPartsName();
            _bll.QueryForObject<MDLBS_AutoPartsName, MDLBS_AutoPartsName>(new MDLBS_AutoPartsName
            {
                WHERE_APN_Name = mcbAPA_Name.SelectedText,
                WHERE_APN_IsValid = true
            }, resultAutoPartsName);
            if (!string.IsNullOrEmpty(resultAutoPartsName.APN_ID))
            {
                cbAPA_UOM.Text = resultAutoPartsName.APN_UOM;
                if (resultAutoPartsName.APN_FixUOM == true && !string.IsNullOrEmpty(resultAutoPartsName.APN_UOM))
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
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //条形码
            txtAPA_Barcode.Text = DetailDS.APA_Barcode;
            //配件名称
            mcbAPA_Name.SelectedValue = DetailDS.APA_Name;
            //配件品牌
            txtAPA_Brand.Text = DetailDS.APA_Brand;
            //配件级别
            cbAPA_Level.Text = DetailDS.APA_Level ?? string.Empty;
            //原厂编码
            txtAPA_OEMNo.Text = DetailDS.APA_OEMNo;
            //第三方编码
            txtAPA_ThirdNo.Text = DetailDS.APA_ThirdNo;
            //规格型号
            txtAPA_Specification.Text = DetailDS.APA_Specification;
            //计量单位 
            cbAPA_UOM.Text = DetailDS.APA_UOM ?? string.Empty;
            //变速类型名称
            cbAPA_VehicleGearboxTypeName.Text = DetailDS.APA_VehicleGearboxTypeName ?? string.Empty;
            //变速类型编码
            txtAPA_VehicleGearboxTypeCode.Text = DetailDS.APA_VehicleGearboxTypeCode;
            //排量
            cbAPA_VehicleCapacity.Text = DetailDS.APA_VehicleCapacity ?? string.Empty;
            //年款
            cbAPA_VehicleYearModel.Text = DetailDS.APA_VehicleYearModel ?? string.Empty;
            //安全库存是否预警
            if (DetailDS.APA_IsWarningSafeStock != null)
            {
                ckAPA_IsWarningSafeStock.Checked = DetailDS.APA_IsWarningSafeStock.Value;
                ckAPA_IsWarningSafeStock.CheckState = ckAPA_IsWarningSafeStock.Checked ? CheckState.Checked : CheckState.Unchecked;
            }
            else
            {
                ckAPA_IsWarningSafeStock.CheckState = CheckState.Indeterminate;
            }
            //安全库存
            numAPA_SafeStockNum.Value = DetailDS.APA_SafeStockNum;
            //呆滞件是否预警
            if (DetailDS.APA_IsWarningDeadStock != null)
            {
                ckAPA_IsWarningDeadStock.Checked = DetailDS.APA_IsWarningDeadStock.Value;
                ckAPA_IsWarningDeadStock.CheckState = ckAPA_IsWarningDeadStock.Checked ? CheckState.Checked : CheckState.Unchecked;
            }
            else
            {
                ckAPA_IsWarningDeadStock.CheckState = CheckState.Indeterminate;
            }
            //呆滞天数
            numAPA_SlackDays.Value = DetailDS.APA_SlackDays;
            //车辆品牌
            mcbAPA_VehicleBrand.SelectedValue = DetailDS.APA_VehicleBrand;
            //车辆车系
            mcbAPA_VehicleInspire.SelectedValue = DetailDS.APA_VehicleInspire;
            //销价系数
            numAPA_SalePriceRate.Value = DetailDS.APA_SalePriceRate;
            //销价
            numAPA_SalePrice.Value = DetailDS.APA_SalePrice;
            //车型代码
            txtAPA_VehicleModelCode.Text = DetailDS.APA_VehicleModelCode;
            //互换码
            txtAPA_ExchangeCode.Text = DetailDS.APA_ExchangeCode;
            //供应商
            mcbSUPP_Name.SelectedValue = DetailDS.APA_SUPP_ID;
            //仓库
            mcbWH_Name.SelectedValue = DetailDS.APA_WH_ID;
            //仓位
            mcbWHB_Name.SelectedValue = DetailDS.APA_WHB_ID;
            //配置档案ID
            txtAPA_ID.Text = DetailDS.APA_ID;
            //组织ID
            txtAPA_Org_ID.Text = DetailDS.APA_Org_ID;
            //有效
            if (DetailDS.APA_IsValid != null)
            {
                ckAPA_IsValid.Checked = DetailDS.APA_IsValid.Value;
            }
            //创建人
            txtAPA_CreatedBy.Text = DetailDS.APA_CreatedBy;
            //创建时间
            dtAPA_CreatedTime.Value = DetailDS.APA_CreatedTime;
            //修改人
            txtAPA_UpdatedBy.Text = DetailDS.APA_UpdatedBy;
            //修改时间
            dtAPA_UpdatedTime.Value = DetailDS.APA_UpdatedTime;
            //版本号
            txtAPA_VersionNo.Text = DetailDS.APA_VersionNo?.ToString() ?? string.Empty;
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
            //验证配件名称
            if (string.IsNullOrEmpty(mcbAPA_Name.SelectedText))
            {
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.BS_AutoPartsArchive.Name.APA_Name }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                mcbAPA_Name.Focus();
                return false;
            }
            //验证配件级别
            if (string.IsNullOrEmpty(cbAPA_Level.Text.Trim()))
            {
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.BS_AutoPartsArchive.Name.APA_Level }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbAPA_Level.Focus();
                return false;
            }
            //验证计量单位
            if (string.IsNullOrEmpty(cbAPA_UOM.Text.Trim()))
            {
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.BS_AutoPartsArchive.Name.APA_UOM }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbAPA_UOM.Focus();
                return false;
            }

            #region 验证[配件价格明细]

            for (int i = 0; i < gdDetail.Rows.Count; i++)
            {
                //验证价格类别
                if (gdDetail.Rows[i].Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].Value == null)
                {
                    MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0027, new object[] { i + 1, MsgParam.PRICE_TYPE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //验证单价
                if (gdDetail.Rows[i].Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Price].Value == null)
                {
                    MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, SystemTableColumnEnums.BS_AutoPartsPriceType.Name.APPT_Price, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (!BLLCom.IsDecimal(gdDetail.Rows[i].Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Price].Value.ToString()))
                {
                    MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, SystemTableColumnEnums.BS_AutoPartsPriceType.Name.APPT_Price, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //一种价格类别只能有一个价格
                for (int j = i + 1; j < gdDetail.Rows.Count; j++)
                {
                    if (gdDetail.Rows[i].Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].Value.ToString()
                        == gdDetail.Rows[j].Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].Value.ToString())
                    {
                        MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { gdDetail.Rows[i].Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].Value.ToString() + "的价格只能有一个！" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            DetailDS = new AutoPartsArchiveManagerUIModel()
            {
                //条形码
                APA_Barcode = txtAPA_Barcode.Text.Trim(),
                //配件名称
                APA_Name = mcbAPA_Name.SelectedText,
                //配件品牌
                APA_Brand = txtAPA_Brand.Text.Trim(),
                //配件级别
                APA_Level = cbAPA_Level.Text.Trim(),
                //原厂编码
                APA_OEMNo = txtAPA_OEMNo.Text.Trim(),
                //第三方编码
                APA_ThirdNo = txtAPA_ThirdNo.Text.Trim(),
                //规格型号
                APA_Specification = txtAPA_Specification.Text.Trim(),
                //计量单位
                APA_UOM = cbAPA_UOM.Text.Trim(),
                //变速类型名称
                APA_VehicleGearboxTypeName = cbAPA_VehicleGearboxTypeName.Text.Trim(),
                //变速类型编码
                APA_VehicleGearboxTypeCode = txtAPA_VehicleGearboxTypeCode.Text.Trim(),
                //排量
                APA_VehicleCapacity = cbAPA_VehicleCapacity.Text.Trim(),
                //年款
                APA_VehicleYearModel = cbAPA_VehicleYearModel.Text.Trim(),
                //安全库存
                APA_SafeStockNum = Convert.ToInt32(numAPA_SafeStockNum.Value ?? 0),
                //呆滞天数
                APA_SlackDays = Convert.ToInt32(numAPA_SlackDays.Value ?? 0),
                //车辆品牌
                APA_VehicleBrand = mcbAPA_VehicleBrand.SelectedText,
                //车系
                APA_VehicleInspire = mcbAPA_VehicleInspire.SelectedText,
                //销价系数
                APA_SalePriceRate = Convert.ToDecimal(numAPA_SalePriceRate.Value ?? 0),
                //销价
                APA_SalePrice = Convert.ToDecimal(numAPA_SalePrice.Value ?? 0),
                //车型代码
                APA_VehicleModelCode = txtAPA_VehicleModelCode.Text,
                //互换码
                APA_ExchangeCode = txtAPA_ExchangeCode.Text,
                //有效
                APA_IsValid = ckAPA_IsValid.Checked,
                //配置档案ID
                APA_ID = txtAPA_ID.Text.Trim(),
                //组织ID
                APA_Org_ID = txtAPA_Org_ID.Text.Trim(),
                //默认供应商ID
                APA_SUPP_ID = mcbSUPP_Name.SelectedValue,
                //默认供应商名称
                SUPP_Name = mcbSUPP_Name.SelectedText,
                //默认仓库ID
                APA_WH_ID = mcbWH_Name.SelectedValue,
                //默认仓库名称
                WH_Name = mcbWH_Name.SelectedText,
                //默认仓位ID
                APA_WHB_ID = mcbWHB_Name.SelectedValue,
                //默认仓位名称
                WHB_Name = mcbWHB_Name.SelectedText,
                //版本号
                APA_VersionNo = Convert.ToInt64(txtAPA_VersionNo.Text.Trim() == "" ? "1" : txtAPA_VersionNo.Text.Trim()),
                //创建人
                APA_CreatedBy = txtAPA_CreatedBy.Text.Trim(),
                //创建时间
                APA_CreatedTime = (DateTime?)dtAPA_CreatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //修改人
                APA_UpdatedBy = txtAPA_UpdatedBy.Text.Trim(),
                //修改时间
                APA_UpdatedTime = (DateTime?)dtAPA_UpdatedTime.Value ?? BLLCom.GetCurStdDatetime(),
            };

            //安全库存是否预警
            if (ckAPA_IsWarningSafeStock.CheckState == CheckState.Indeterminate)
            {
                DetailDS.APA_IsWarningSafeStock = null;
            }
            else
            {
                DetailDS.APA_IsWarningSafeStock = ckAPA_IsWarningSafeStock.Checked;
            }
            //呆滞件是否预警
            if (ckAPA_IsWarningDeadStock.CheckState == CheckState.Indeterminate)
            {
                DetailDS.APA_IsWarningDeadStock = null;
            }
            else
            {
                DetailDS.APA_IsWarningDeadStock = ckAPA_IsWarningDeadStock.Checked;
            }

            //组织
            if (string.IsNullOrEmpty(DetailDS.APA_Org_ID))
            {
                DetailDS.APA_Org_ID = LoginInfoDAX.OrgID;
            }
        }

        /// <summary>
        /// 设置详情页面控件是否可编辑
        /// </summary>
        private void SetDetailControl()
        {
            if (string.IsNullOrEmpty(txtAPA_ID.Text))
            {
                #region 配件档案未保存的场合，全部可编辑

                //配件名称
                mcbAPA_Name.Enabled = true;
                //配件品牌
                txtAPA_Brand.Enabled = true;
                //规格型号
                txtAPA_Specification.Enabled = true;
                //原厂编码
                txtAPA_OEMNo.Enabled = true;
                //第三方编码
                txtAPA_ThirdNo.Enabled = true;
                //适用范围相关
                mcbAPA_VehicleBrand.IsReadOnly = false;
                mcbAPA_VehicleInspire.IsReadOnly = false;
                cbAPA_VehicleCapacity.Enabled = true;
                cbAPA_VehicleYearModel.Enabled = true;
                cbAPA_VehicleGearboxTypeName.Enabled = true;
                //配件级别
                cbAPA_Level.Enabled = true;
                //计量单位
                cbAPA_UOM.Enabled = true;

                #endregion
            }
            else
            {
                #region 配件档案已保存的场合，[配件名称]，[配件品牌]，[规格型号]，[适用范围相关]，[配件级别]，[计量单位]，[原厂编码]，[第三方编码]不可编辑

                //配件名称
                mcbAPA_Name.Enabled = false;
                //配件品牌
                txtAPA_Brand.Enabled = false;
                //规格型号
                txtAPA_Specification.Enabled = false;
                //原厂编码
                txtAPA_OEMNo.Enabled = false;
                //第三方编码
                txtAPA_ThirdNo.Enabled = false;
                //适用范围相关
                mcbAPA_VehicleBrand.IsReadOnly = true;
                mcbAPA_VehicleInspire.IsReadOnly = true;
                cbAPA_VehicleCapacity.Enabled = false;
                cbAPA_VehicleYearModel.Enabled = false;
                cbAPA_VehicleGearboxTypeName.Enabled = false;
                //配件级别
                cbAPA_Level.Enabled = false;
                //计量单位
                cbAPA_UOM.Enabled = false;

                #endregion
            }
        }

        #region 配件价格明细相关方法

        /// <summary>
        /// 添加配件价格明细
        /// </summary>
        private void AddAutoPartsPriceDetail()
        {
            AutoPartsPriceTypeUIModel newAutoPartsPriceTypeRow = new AutoPartsPriceTypeUIModel
            {
                IsChecked = false,
                Tmp_ID = Guid.NewGuid().ToString(),
                APPT_Org_ID = LoginInfoDAX.OrgID,
                APPT_Barcode = txtAPA_Barcode.Text.Trim(),
                APPT_IsValid = true,
                APPT_CreatedBy = LoginInfoDAX.UserName,
                APPT_CreatedTime = BLLCom.GetCurStdDatetime(),
                APPT_UpdatedBy = LoginInfoDAX.UserName,
                APPT_UpdatedTime = BLLCom.GetCurStdDatetime(),
            };

            _detailGridDS.Add(newAutoPartsPriceTypeRow);

            #region 价格类别下拉框数据源

            var curRowIndex = gdDetail.Rows.Count == 0 ? 0 : gdDetail.Rows.Count - 1;
            gdDetail.DisplayLayout.ValueLists.Add("List" + newAutoPartsPriceTypeRow.Tmp_ID);

            if (_autoPartsPriceTypeList.Count > 0)
            {
                for (int j = 0; j < _autoPartsPriceTypeList.Count; j++)
                {
                    gdDetail.DisplayLayout.ValueLists["List" + newAutoPartsPriceTypeRow.Tmp_ID].ValueListItems.Add(
                        _autoPartsPriceTypeList[j].Value, _autoPartsPriceTypeList[j].Text);
                }
                gdDetail.DisplayLayout.ValueLists["List" + newAutoPartsPriceTypeRow.Tmp_ID].DisplayStyle = ValueListDisplayStyle.DisplayText;
                gdDetail.DisplayLayout.Rows[curRowIndex].Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].ValueList = gdDetail.DisplayLayout.ValueLists["List" + newAutoPartsPriceTypeRow.Tmp_ID];
            }
            else
            {
                gdDetail.DisplayLayout.Rows[curRowIndex].Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].ValueList = null;
                gdDetail.Rows[curRowIndex].Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].Value = null;
            }

            #endregion

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;

            //控制[配件价格明细]Grid的单元格样式
            SetAutoPartsPriceDetailStyle();
        }

        /// <summary>
        /// 删除配件价格明细
        /// </summary>
        private void DeleteAutoPartsPriceDetail()
        {
            gdDetail.UpdateData();

            //待删除的配件价格明细
            List<AutoPartsPriceTypeUIModel> deleteAutoPartsPriceList = _detailGridDS.Where(p => p.IsChecked == true).ToList();
            if (deleteAutoPartsPriceList.Count == 0)
            {
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { MsgParam.AUTOPARTS_PRICEDETAIL, SystemActionEnum.Name.DELETE }),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //移除勾选项
            foreach (var loopAutoPartsPriceDetail in deleteAutoPartsPriceList)
            {
                _detailGridDS.Remove(loopAutoPartsPriceDetail);
            }

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 查询配件价格明细
        /// </summary>
        private void QueryDetail()
        {
            //配件价格明细列表
            _bll.QueryForList(SQLID.BS_AutoPartsArchiveManager_SQL04, new AutoPartsArchiveManagerQCModel
            {
                WHERE_APA_Barcode = txtAPA_Barcode.Text.Trim(),
            }, _detailGridDS);

            //3.开始监控List变化（该数据源若有其他调整，请在StartMonitChanges()方法执行前调整）
            _detailGridDS.StartMonitChanges();

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();

            #region 加载价格类别

            for (int i = 0; i < gdDetail.DisplayLayout.Rows.Count; i++)
            {
                var detailRow = gdDetail.DisplayLayout.Rows[i];
                var curRowTmpId = detailRow.Cells["Tmp_ID"].Text;

                gdDetail.DisplayLayout.ValueLists.Add("List" + curRowTmpId);

                if (_autoPartsPriceTypeList.Count > 0)
                {
                    //该明细价格类别在价格类别列表的位置
                    int? detailPriceTypeIndex = null;
                    for (int j = 0; j < _autoPartsPriceTypeList.Count; j++)
                    {
                        gdDetail.DisplayLayout.ValueLists["List" + curRowTmpId].ValueListItems.Add(
                        _autoPartsPriceTypeList[j].Value, _autoPartsPriceTypeList[j].Text);

                        if (_autoPartsPriceTypeList[j].Value == detailRow.Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].Value?.ToString())
                        {
                            detailPriceTypeIndex = j;
                        }
                    }
                    gdDetail.DisplayLayout.ValueLists["List" + curRowTmpId].DisplayStyle = ValueListDisplayStyle.DisplayText;
                    detailRow.Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].ValueList = gdDetail.DisplayLayout.ValueLists["List" + curRowTmpId];
                    if (detailPriceTypeIndex != null)
                    {
                        detailRow.Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].Value = gdDetail.DisplayLayout.ValueLists["List" + curRowTmpId].ValueListItems[(int)detailPriceTypeIndex].DataValue;
                    }
                }
                else
                {
                    detailRow.Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].ValueList = null;
                    detailRow.Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].Value = null;
                }
            }
            #endregion

            //设置明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 设置配件价格明细Grid的单元格样式
        /// </summary>
        private void SetAutoPartsPriceDetailStyle()
        {
            gdDetail.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;

            //[选择]始终可编辑
            gdDetail.DisplayLayout.Bands[0].Columns["IsChecked"].CellAppearance.BackColor = CustomEnums.CellBackColor.EditableColor;
            gdDetail.DisplayLayout.Bands[0].Columns["IsChecked"].CellAppearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

            //[价格]始终可编辑
            gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Price].CellActivation = Activation.AllowEdit;
            gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Price].CellAppearance.BackColor = CustomEnums.CellBackColor.EditableColor;
            gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Price].CellAppearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

            foreach (var loopGridRow in gdDetail.Rows)
            {
                if (!string.IsNullOrEmpty(txtAPA_ID.Text.Trim()))
                {
                    #region 配件档案已保存的场合

                    if (loopGridRow.Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_ID].Value == null
                        || string.IsNullOrEmpty(loopGridRow.Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_ID].Value.ToString()))
                    {
                        //未保存的配件价格明细，[价格类别]可编辑
                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].Activation =
                            Activation.AllowEdit;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].Appearance
                            .BackColor = CustomEnums.CellBackColor.EditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].Appearance
                            .BorderColor = CustomEnums.ColumnBorderColor.EditableColor;
                    }
                    else
                    {
                        //已保存的配件价格明细，[价格类别]不可编辑
                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].Activation = Activation.ActivateOnly;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;
                    }
                    #endregion
                }
                else
                {
                    #region 配件档案未保存的场合，[价格类别]可编辑

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].Activation =
                        Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].Appearance
                        .BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.BS_AutoPartsPriceType.Code.APPT_Name].Appearance
                        .BorderColor = CustomEnums.ColumnBorderColor.EditableColor;
                    #endregion
                }
            }
        }
        #endregion

        #region 配件图片相关方法

        /// <summary>
        /// 加载配件图片
        /// </summary>
        private void LoadAutoPartsPicture()
        {
            if (string.IsNullOrEmpty(txtAPA_Barcode.Text))
            {
                return;
            }

            flowLayoutPanelPicture.Controls.Clear();
            _pictureExpandList.Clear();

            #region 查询配件图片列表

            _bll.QueryForList(new MDLPIS_InventoryPicture
            {
                WHERE_INVP_Barcode = txtAPA_Barcode.Text,
                WHERE_INVP_IsValid = true,
            }, _autoPartsPictureList);

            #endregion

            if (_autoPartsPictureList.Count == 0)
            {
                //配件无图片时，加载一个扩展的图片控件
                //添加空图片控件
                AutoPartsPictureUIModel nullAutoPartsPicture = new AutoPartsPictureUIModel()
                {
                    INVP_Barcode = txtAPA_Barcode.Text,
                };
                AddNullPictureControl(nullAutoPartsPicture);
            }
            else
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
                AutoPartsPictureUIModel nullAutoPartsPicture = new AutoPartsPictureUIModel()
                {
                    INVP_Barcode = txtAPA_Barcode.Text,
                };
                //添加空图片控件
                AddNullPictureControl(nullAutoPartsPicture);
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
                if (_autoPartsPictureList.Count == 0
                    ||
                    _autoPartsPictureList.Any(
                        x => x.INVP_PictureName != paramPictureKey && x.SourceFilePath != tempFileName))
                {
                    AutoPartsPictureUIModel newAutoPartsPicture = new AutoPartsPictureUIModel
                    {
                        INVP_PictureName = paramPictureKey,
                        SourceFilePath = tempFileName,
                    };
                    if (curAutoPartsPicture != null)
                    {
                        newAutoPartsPicture.INVP_Barcode = curAutoPartsPicture.INVP_Barcode;
                    }
                    _autoPartsPictureList.Add(newAutoPartsPicture);
                }
                else
                {
                    var curUploadPicture =
                        _autoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == paramPictureKey);
                    if (curUploadPicture != null)
                    {
                        curUploadPicture.SourceFilePath = tempFileName;
                    }
                }

                var nullImagePicture = _pictureExpandList.Where(x => x.PictureImage == null).ToList();
                if (nullImagePicture.Count == 1)
                {
                    //添加空图片控件
                    AddNullPictureControl(curAutoPartsPicture);
                }

                //设置图片是否可见、可编辑
                if (curAutoPartsPicture != null)
                {
                    var tempPicture = _pictureExpandList.FirstOrDefault(x => x.PictureKey == paramPictureKey);
                    if (tempPicture != null)
                    {
                        tempPicture.PictureImage = curImage;
                    }
                    //var curDetail = _detailGridDS.FirstOrDefault(x => x.APPT_Barcode == curAutoPartsPicture.INVP_Barcode);
                    //if (curDetail != null)
                    //{
                    //    SetPictureControl(curDetail);
                    //}
                }

                return curImage;
            }
            catch (Exception ex)
            {
                //上传图片失败
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0018, new object[] { MsgParam.UPLOAD_IMAGE, ex.Message }), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (_autoPartsPictureList == null || _autoPartsPictureList.Count == 0)
            {
                isDownFromWeb = true;
            }
            else
            {
                var curPicture = _autoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == paramPictureKey);
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
                MessageBoxs.Show(Trans.BS, ToString(),
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
                    MessageBoxs.Show(Trans.BS, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (string.IsNullOrEmpty(outMsg))
                {
                    return true;
                }
                //导出成功
                MessageBoxs.Show(Trans.BS, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                var deletePicture = _autoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == paramPictureKey);
                if (deletePicture != null)
                {
                    _autoPartsPictureList.Remove(deletePicture);
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
                MessageBoxs.Show(Trans.BS, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    //删除失败，失败原因：ex.Message
                    MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.DELETE, ex.Message }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            #endregion

            //删除成功
            MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //删除当前图片控件
            var removeImage = _pictureExpandList.FirstOrDefault(x => x.PictureKey == paramPictureKey);
            if (removeImage != null)
            {
                flowLayoutPanelPicture.Controls.Remove(removeImage);
                _pictureExpandList.Remove(removeImage);
            }
            //移除配件图片列表中数据
            var removePicture = _autoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == paramPictureKey);
            if (removePicture != null)
            {
                _autoPartsPictureList.Remove(removePicture);
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
                if (loopPicture.IsCheckedIsVisible == false
                    || loopPicture.IsCheckedIsEnabled == false)
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

                    AutoPartsPictureUIModel curAutoPartsPicture = new AutoPartsPictureUIModel()
                    {
                        INVP_Barcode = txtAPA_Barcode.Text.Trim(),
                    };
                    SkyCarPictureExpand autoPartsPicture = new SkyCarPictureExpand
                    {
                        //图片名称作为操作图片的唯一标识
                        PictureKey = Guid.NewGuid() + ".jpg",
                        //当前图片Model
                        PropertyModel = curAutoPartsPicture,
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

                    if ((_autoPartsPictureList.Count == 0
                        || _autoPartsPictureList.Any(x => x.INVP_PictureName != autoPartsPicture.PictureKey && x.SourceFilePath != tempFileName)))
                    {
                        //第一次上传的场合，新增待保存的配件图片
                        AutoPartsPictureUIModel newAutoPartsPicture = new AutoPartsPictureUIModel
                        {
                            INVP_Barcode = txtAPA_Barcode.Text.Trim(),
                            INVP_PictureName = autoPartsPicture.PictureKey,
                            SourceFilePath = tempFileName,
                        };
                        _autoPartsPictureList.Add(newAutoPartsPicture);
                    }
                    else
                    {
                        //更新上传的场合，更新图片文件源
                        var curUploadPicture = _autoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == autoPartsPicture.PictureKey);
                        if (curUploadPicture != null)
                        {
                            curUploadPicture.SourceFilePath = tempFileName;
                        }
                    }
                }

                //添加空图片控件
                AutoPartsPictureUIModel nullAutoPartsPicture = new AutoPartsPictureUIModel()
                {
                    INVP_Barcode = txtAPA_Barcode.Text.Trim(),
                };
                AddNullPictureControl(nullAutoPartsPicture);

                //设置图片是否可见、可编辑
                SetPictureControl();
            }
            catch (Exception ex)
            {
                //上传图片失败
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0018, new object[] { MsgParam.UPLOAD_IMAGE, ex.Message }), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            List<AutoPartsPictureUIModel> curAutoPartsPictureList = new List<AutoPartsPictureUIModel>();
            foreach (var loopCheckedPictureExpand in checkedPictureExpandList)
            {
                var curAutoPartsPicture = loopCheckedPictureExpand.PropertyModel as AutoPartsPictureUIModel;
                if (curAutoPartsPicture == null || curAutoPartsPicture.INVP_Barcode != txtAPA_Barcode.Text.Trim())
                {
                    continue;
                }
                curAutoPartsPictureList.Add(curAutoPartsPicture);
            }
            if (curAutoPartsPictureList.Count == 0)
            {
                //请选择图片
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.IMAGE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #endregion

            List<string> fileNameList = new List<string>();
            //导出图片
            foreach (var loopPicture in curAutoPartsPictureList)
            {
                if (string.IsNullOrEmpty(loopPicture.INVP_PictureName))
                {
                    continue;
                }
                fileNameList.Add(loopPicture.INVP_PictureName);
            }

            //错误消息
            var outMsg = string.Empty;
            bool exportResult = BLLCom.ExportFileListByFileNameList(fileNameList, ref _defaultFilePath, ref outMsg);
            if (exportResult == false)
            {
                //导出失败
                MessageBoxs.Show(Trans.BS, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //导出成功
            MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.EXPORT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 批量删除图片
        /// </summary>
        private void BatchDeletePicture()
        {
            #region 验证

            //勾选的图片列表
            var checkedPictureExpandList = _pictureExpandList.Where(x => x.IsChecked == true && x.PictureImage != null).ToList();
            List<AutoPartsPictureUIModel> curAutoPartsPictureList = new List<AutoPartsPictureUIModel>();
            foreach (var loopCheckedPictureExpand in checkedPictureExpandList)
            {
                var curAutoPartsPicture = loopCheckedPictureExpand.PropertyModel as AutoPartsPictureUIModel;
                if (curAutoPartsPicture == null || curAutoPartsPicture.INVP_Barcode != txtAPA_Barcode.Text.Trim())
                {
                    continue;
                }
                curAutoPartsPictureList.Add(curAutoPartsPicture);
            }
            if (curAutoPartsPictureList.Count == 0)
            {
                //请选择图片
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.IMAGE }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //已选checkedPictureList.Count 条数据，确定删除？\r\n单击【确定】删除，【取消】返回。
            DialogResult dialogResult = MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.W_0013, new object[] { curAutoPartsPictureList.Count }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            #endregion

            //待删除的配件图片列表
            List<MDLPIS_InventoryPicture> deleteAutoPartsPictureList = new List<MDLPIS_InventoryPicture>();

            string pictureNameString = string.Empty;
            foreach (var loopPicture in curAutoPartsPictureList)
            {
                if (string.IsNullOrEmpty(loopPicture.INVP_PictureName))
                {
                    continue;
                }
                pictureNameString += loopPicture.INVP_PictureName + SysConst.Semicolon_DBC;
            }
            //根据图片名称查询待删除的配件图片
            _bll.QueryForList(SQLID.COMM_SQL49, new AutoPartsPictureQCModel
            {
                WHERE_INVP_Barcode = txtAPA_Barcode.Text.Trim() + SysConst.Semicolon_DBC,
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
                    MessageBoxs.Show(Trans.BS, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBoxs.Show(Trans.BS, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            //删除成功
            MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                var removePicture = _autoPartsPictureList.FirstOrDefault(x => x.INVP_PictureName == loopPicture.INVP_PictureName);
                if (removePicture != null)
                {
                    _autoPartsPictureList.Remove(removePicture);
                }
            }
        }

        #endregion

        /// <summary>
        /// 设置图片是否可见、可编辑
        /// </summary>
        private void SetPictureControl()
        {
            ckSelectAllPicture.Enabled = true;
            toolbarsAutoPartsPicture.Tools[SysConst.Upload].SharedProps.Enabled = true;
            toolbarsAutoPartsPicture.Tools[SysConst.Export].SharedProps.Enabled = true;
            toolbarsAutoPartsPicture.Tools[SysConst.EN_DEL].SharedProps.Enabled = true;

            if (toolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[SystemActionEnum.Code.SAVE].SharedPropsInternal.Enabled)
            {
                //配件图片可保存的场合，可上传、删除图片
                toolbarsAutoPartsPicture.Tools[SysConst.Upload].SharedProps.Enabled = true;
                toolbarsAutoPartsPicture.Tools[SysConst.EN_DEL].SharedProps.Enabled = true;
                foreach (var loopPicture in _pictureExpandList)
                {
                    loopPicture.IsCheckedIsVisible = true;
                    loopPicture.UploadIsVisible = true;
                    loopPicture.ExportIsVisible = true;
                    loopPicture.DeleteIsVisible = true;

                    loopPicture.IsCheckedIsEnabled = loopPicture.PictureImage != null;
                    loopPicture.UploadIsEnabled = true;
                    loopPicture.DeleteIsEnabled = loopPicture.PictureImage != null;
                    loopPicture.ExportIsEnabled = loopPicture.PictureImage != null;
                }
            }
            else
            {
                //配件图片不可保存的场合，不可上传、删除图片
                toolbarsAutoPartsPicture.Tools[SysConst.Upload].SharedProps.Enabled = false;
                toolbarsAutoPartsPicture.Tools[SysConst.EN_DEL].SharedProps.Enabled = false;
                foreach (var loopPicture in _pictureExpandList)
                {
                    loopPicture.IsCheckedIsVisible = false;
                    loopPicture.UploadIsVisible = false;
                    loopPicture.ExportIsVisible = false;
                    loopPicture.DeleteIsVisible = false;
                }
            }

            //默认不勾选
            ckSelectAllPicture.Checked = false;
            var curAutoPartsPictureList = _autoPartsPictureList.Where(x => x.INVP_Barcode == txtAPA_Barcode.Text.Trim()).ToList();
            if (curAutoPartsPictureList.Count == 0)
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

        /// <summary>
        /// 添加空图片控件
        /// </summary>
        /// <param name="paramAutoPartsPicture">配件图片</param>
        private void AddNullPictureControl(AutoPartsPictureUIModel paramAutoPartsPicture)
        {
            SkyCarPictureExpand addAutoPartsPicture = new SkyCarPictureExpand
            {
                //图片名称作为操作图片的唯一标识
                PictureKey = Guid.NewGuid() + ".jpg",
                //待保存的配件图片TBModel
                PropertyModel = paramAutoPartsPicture,
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

        /// <summary>
        /// 下载导入模板
        /// </summary>
        private void ExecuteDownloadImportTemplateCommand()
        {
            string templateFilePath = Directory.GetCurrentDirectory() + @"\TemplateFiles\配件档案导入模板.xlsx";
            if (!File.Exists(templateFilePath))
            {
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { templateFilePath + @"\不存在，无法下载" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = FormatConst.FILE_TYPE_FILTER_EXCEL_01, //设置保存文件类型
                FileName = "配件档案模板",                        //文件名
                AddExtension = true                             //设置添加扩展名
            };
            saveDialog.ShowDialog();
            string saveFilePath = saveDialog.FileName;
            if (saveFilePath.IndexOf(SysConst.COLON_DBC, StringComparison.Ordinal) < 0)
            {
                return; //被点了取消 
            }

            if (!string.IsNullOrEmpty(saveFilePath))
            {
                try
                {
                    if (File.Exists(saveFilePath))
                    {
                        File.Delete(saveFilePath);
                    }
                    File.Copy(templateFilePath, saveFilePath);

                    MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { "下载成功" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    //下载失败，失败原因：ex.Message
                    MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0018, new object[] { "下载", ex.Message }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <param name="paramIsDelete">是否是删除操作</param>
        private void RefreshList()
        {
            var curHead = GridDS.FirstOrDefault(x => x.APA_ID == DetailDS.APA_ID);
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
