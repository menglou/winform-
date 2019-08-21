using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.BS;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.BS.QCModel;
using SkyCar.Coeus.UIModel.BS.UIModel;

namespace SkyCar.Coeus.UI.BS
{
    /// <summary>
    /// 车型配件匹配管理
    /// </summary>
    public partial class FrmVehicleModelMatchAutoPartsManager : BaseFormCardListDetail<VehicleModelMatchAutoPartsManagerUIModel, VehicleModelMatchAutoPartsManagerQCModel, MDLBS_VehicleInfo>
    {
        #region 全局变量

        /// <summary>
        /// 车型配件匹配管理BLL
        /// </summary>
        private VehicleModelMatchAutoPartsManagerBLL _bll = new VehicleModelMatchAutoPartsManagerBLL();

        /// <summary>
        /// 是否合并当前车架号的信息
        /// </summary>
        private bool _isJoinVinInfo;

        /// <summary>
        /// 是否询问用户加载数据
        /// </summary>
        private bool _isAskLoadData;

        #region Grid数据源

        /// <summary>
        /// 原厂件数据源
        /// </summary>
        private SkyCarBindingList<VehicleOemPartsInfoUIModel, MDLBS_VehicleOemPartsInfo> _oemDetailGridDS = new SkyCarBindingList<VehicleOemPartsInfoUIModel, MDLBS_VehicleOemPartsInfo>();
        /// <summary>
        /// 品牌件数据源
        /// </summary>
        private SkyCarBindingList<VehicleThirdPartsInfoUIModel, MDLBS_VehicleThirdPartsInfo> _brandDetailGridDS = new SkyCarBindingList<VehicleThirdPartsInfoUIModel, MDLBS_VehicleThirdPartsInfo>();
        #endregion

        #region 下拉框数据源

        /// <summary>
        /// 车辆品牌
        /// </summary>
        List<MDLBS_VehicleBrandInspireSumma> _vehicleBrandList = new List<MDLBS_VehicleBrandInspireSumma>();

        /// <summary>
        /// 车系
        /// </summary>
        List<MDLBS_VehicleBrandInspireSumma> _vehicleInspireList = new List<MDLBS_VehicleBrandInspireSumma>();

        /// <summary>
        /// 车型描述
        /// </summary>
        List<MDLBS_VehicleBrandInspireSumma> _vehicleModelDescList = new List<MDLBS_VehicleBrandInspireSumma>();

        /// <summary>
        /// 排量
        /// </summary>
        ObservableCollection<CodeTableValueTextModel> _vehicleCapacityList = new ObservableCollection<CodeTableValueTextModel>();
        #endregion

        /// <summary>
        /// 添加维护车辆原厂件信息Func
        /// </summary>
        private Func<VehicleOemPartsInfoUIModel, bool> _maintainOemPartsInfoFunc;

        /// <summary>
        /// 添加维护车辆品牌件信息Func
        /// </summary>
        private Func<VehicleThirdPartsInfoUIModel, bool> _maintainBrandPartsInfoFunc;

        /// <summary>
        /// 界面属性值发生变化时不予检查的属性列表
        /// </summary>
        List<string> _skipPropertyList = new List<string>();

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmVehicleModelMatchAutoPartsManager构造方法
        /// </summary>
        public FrmVehicleModelMatchAutoPartsManager()
        {
            InitializeComponent();

            _maintainOemPartsInfoFunc = MaintainOemPartsInfo;
            _maintainBrandPartsInfoFunc = MaintainBrandPartsInfo;
        }
       
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmVehicleModelMatchAutoPartsManager_Load(object sender, EventArgs e)
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
            #endregion

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            AcceptUIModelChanges();

            #region 界面发生变化时不予检查的属性值

            _skipPropertyList.Add("WHERE_VC_ID");
            _skipPropertyList.Add("VC_OperateType"); 
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

        #region 查询条件相关事件

        /// <summary>
        /// 【列表】品牌SelectedIndexChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbWHERE_VC_Brand_SelectedIndexChanged(object sender, EventArgs e)
        {
            mcbWHERE_VC_Inspire.Clear();
            mcbWHERE_VC_BrandDesc.Clear();
            if (string.IsNullOrEmpty(mcbWHERE_VC_Brand.SelectedValue))
            {
                mcbWHERE_VC_Inspire.DataSource = null;
                mcbWHERE_VC_BrandDesc.DataSource = null;
                return;
            }

            //车系
            if (_vehicleInspireList != null)
            {
                var curVehicleInspireList = _vehicleInspireList.Where(x => !string.IsNullOrEmpty(x.VBIS_Inspire) && mcbWHERE_VC_Brand.SelectedValue.Contains(x.VBIS_Brand)).ToList();
                mcbWHERE_VC_Inspire.DataSource = curVehicleInspireList;
            }
        }
        /// <summary>
        /// 【列表】车系SelectedIndexChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbWHERE_VC_Inspire_SelectedIndexChanged(object sender, EventArgs e)
        {
            mcbWHERE_VC_BrandDesc.Clear();
            if (string.IsNullOrEmpty(mcbWHERE_VC_Inspire.SelectedValue))
            {
                mcbWHERE_VC_BrandDesc.DataSource = null;
                return;
            }

            //车型描述
            if (_vehicleModelDescList != null)
            {
                var curVehicleModelDescList = _vehicleModelDescList.Where(x => !string.IsNullOrEmpty(x.VBIS_ModelDesc) && mcbWHERE_VC_Brand.SelectedValue.Contains(x.VBIS_Brand) && mcbWHERE_VC_Inspire.SelectedValue.Contains(x.VBIS_Inspire)).ToList();
                mcbWHERE_VC_BrandDesc.DataSource = curVehicleModelDescList;
            }
        }
        #endregion

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

        #region 单头相关事件

        private string _latestVin = string.Empty;
        /// <summary>
        /// 【详情】车架号ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVC_VIN_ValueChanged(object sender, EventArgs e)
        {
            //全角转半角
            txtVC_VIN.Text = BLLCom.ToDBC(txtVC_VIN.Text.Trim());
            //小写转大写
            txtVC_VIN.Text = txtVC_VIN.Text.ToUpper();
            //设置光标位置为末尾
            txtVC_VIN.Select(txtVC_VIN.Text.Length, 0);

            if (txtVC_VIN.Text.Trim().Length != 17
                || txtVC_VIN.Text.Trim() == _latestVin)
            {
                return;
            }

            _latestVin = txtVC_VIN.Text.Trim();

            if (!_isAskLoadData)
            {
                return;
            }

            HeadDS = new VehicleModelMatchAutoPartsManagerUIModel();
            //查询该车架号对应的车辆信息、原厂件信息、品牌件信息
            _bll.QueryForObject<MDLBS_VehicleInfo, VehicleModelMatchAutoPartsManagerUIModel>(new MDLBS_VehicleInfo
            {
                WHERE_VC_VIN = txtVC_VIN.Text.Trim(),
                WHERE_VC_IsValid = true,
            }, HeadDS);

            if (HeadDS != null
                && !string.IsNullOrEmpty(HeadDS.VC_VIN))
            {
                #region 车架号已存在的场合

                //是否加载数据库中的数据
                var isLoadData = MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.W_0000, new object[] { "车架号已存在，是否加载对应信息？" }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (isLoadData != DialogResult.OK)
                {
                    return;
                }
                //品牌
                mcbVC_Brand.SelectedValue = HeadDS.VC_Brand;
                //车系
                mcbVC_Inspire.SelectedValue = HeadDS.VC_Inspire;
                //车型描述
                mcbVC_BrandDesc.SelectedValue = HeadDS.VC_BrandDesc;
                //排量
                mcbVC_Capacity.SelectedValue = HeadDS.VC_Capacity;
                //发动机型号
                txtVC_EngineType.Text = HeadDS.VC_EngineType;
                //备注
                txtVC_Remark.Text = HeadDS.VC_Remark;
                //有效
                if (HeadDS.VC_IsValid != null)
                {
                    ckVC_IsValid.Checked = HeadDS.VC_IsValid.Value;
                }
                //创建人
                txtVC_CreatedBy.Text = HeadDS.VC_CreatedBy;
                //创建时间
                dtVC_CreatedTime.Value = HeadDS.VC_CreatedTime;
                //修改人
                txtVC_UpdatedBy.Text = HeadDS.VC_UpdatedBy;
                //修改时间
                dtVC_UpdatedTime.Value = HeadDS.VC_UpdatedTime;
                //车辆信息ID
                txtVC_ID.Text = HeadDS.VC_ID;
                //版本号
                txtVC_VersionNo.Value = HeadDS.VC_VersionNo;
                //车牌号
                txtVC_PlateNumber.Text = HeadDS.VC_PlateNumber;

                //查询[原厂件信息]Grid数据并绑定
                QueryOemDetail();
                //查询[品牌件信息]Grid数据并绑定
                QueryBrandDetail();

                #endregion
            }
            else
            {
                #region 车架号不存在的场合

                //有效
                ckVC_IsValid.Checked = true;
                ckVC_IsValid.CheckState = CheckState.Checked;
                //创建人
                txtVC_CreatedBy.Text = LoginInfoDAX.UserName;
                //创建时间
                dtVC_CreatedTime.Value = DateTime.Now;
                //修改人
                txtVC_UpdatedBy.Text = LoginInfoDAX.UserName;
                //修改时间
                dtVC_UpdatedTime.Value = DateTime.Now;

                //刷新[原厂件信息]中的车架号
                foreach (var loopOemDetail in _oemDetailGridDS)
                {
                    loopOemDetail.VOPI_VC_VIN = txtVC_VIN.Text.Trim();
                }
                gdOEMDetail.DataSource = _oemDetailGridDS;
                gdOEMDetail.DataBind();
                //刷新[品牌件信息]中的车架号
                foreach (var loopBrandDetail in _brandDetailGridDS)
                {
                    loopBrandDetail.VTPI_VC_VIN = txtVC_VIN.Text.Trim();
                }
                gdBrandDetail.DataSource = _brandDetailGridDS;
                gdBrandDetail.DataBind();

                #endregion
            }
        }

        /// <summary>
        /// 【详情】品牌SelectedIndexChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbVC_Brand_SelectedIndexChanged(object sender, EventArgs e)
        {
            mcbVC_Inspire.Clear();
            mcbVC_BrandDesc.Clear();
            if (string.IsNullOrEmpty(mcbVC_Brand.SelectedValue))
            {
                mcbVC_Inspire.DataSource = null;
                mcbVC_BrandDesc.DataSource = null;
                return;
            }

            //车系
            if (_vehicleInspireList != null)
            {
                var curVehicleInspireList = _vehicleInspireList.Where(x => !string.IsNullOrEmpty(x.VBIS_Inspire) && mcbVC_Brand.SelectedValue.Contains(x.VBIS_Brand)).ToList();
                mcbVC_Inspire.DataSource = curVehicleInspireList;
            }
        }

        /// <summary>
        /// 【详情】车系SelectedIndexChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbVC_Inspire_SelectedIndexChanged(object sender, EventArgs e)
        {
            mcbVC_BrandDesc.Clear();
            if (string.IsNullOrEmpty(mcbVC_Inspire.SelectedValue))
            {
                mcbVC_BrandDesc.DataSource = null;
                return;
            }

            //车型描述
            if (_vehicleModelDescList != null)
            {
                var curVehicleModelDescList = _vehicleModelDescList.Where(x => !string.IsNullOrEmpty(x.VBIS_ModelDesc) && mcbVC_Brand.SelectedValue.Contains(x.VBIS_Brand) && mcbVC_Inspire.SelectedValue.Contains(x.VBIS_Inspire)).ToList();
                mcbVC_BrandDesc.DataSource = curVehicleModelDescList;
            }
        }
        #endregion

        #region [原厂件信息]相关事件

        /// <summary>
        /// 单击原厂件工具栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolbarsManagerOEMDetail_ToolClick(object sender, ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case SysConst.EN_ADD:
                    //添加原厂件
                    AddOEMAutoPartsDetail();
                    break;

                case SysConst.EN_DEL:
                    //删除原厂件
                    DeleteOEMAutoPartsDetail();
                    break;
            }
        }

        /// <summary>
        /// 原厂件信息Grid单元格值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdOEMDetail_CellChange(object sender, CellEventArgs e)
        {
            gdOEMDetail.UpdateData();
        }
        #endregion

        #region [品牌件信息]相关事件

        /// <summary>
        /// 单击品牌件工具栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolbarsManagerBrandDetail_ToolClick(object sender, ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case SysConst.EN_ADD:
                    //添加品牌件
                    AddBrandAutoPartsDetail();
                    break;

                case SysConst.EN_DEL:
                    //删除品牌件
                    DeleteBrandAutoPartsDetail();
                    break;
            }
        }

        /// <summary>
        /// 品牌件信息Grid单元格值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdBrandDetail_CellChange(object sender, CellEventArgs e)
        {
            gdBrandDetail.UpdateData();
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
            if (ViewHasChanged(_skipPropertyList)
                || _oemDetailGridDS.InsertList.Count > 0
                || _oemDetailGridDS.UpdateList.Count > 0
                || _oemDetailGridDS.DeleteList.Count > 0
                || _brandDetailGridDS.InsertList.Count > 0
                || _brandDetailGridDS.UpdateList.Count > 0
                || _brandDetailGridDS.DeleteList.Count > 0)
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
            gdOEMDetail.UpdateData();
            gdBrandDetail.UpdateData();
            //1.前端检查-保存
            if (!ClientCheckForSave())
            {
                return;
            }
            //2.将【详情】Tab内控件的值赋值给基类的HeadDS
            SetCardCtrlsToDetailDS();
            //3.执行保存（含服务端检查）
            bool saveResult = _bll.SaveDetailDS(HeadDS, _oemDetailGridDS, _brandDetailGridDS, _isJoinVinInfo);
            if (!saveResult)
            {
                //保存失败
                MessageBoxs.Show(Trans.BS, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            _isAskLoadData = false;
            //5.将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            if (_isJoinVinInfo)
            {
                //合并数据的场合，查询数据库中数据
                QueryOemDetail();
                QueryBrandDetail();
            }

            //4.开始监控List变化
            _oemDetailGridDS.StartMonitChanges();
            _brandDetailGridDS.StartMonitChanges();

            gdOEMDetail.DataSource = _oemDetailGridDS;
            gdOEMDetail.DataBind();
            //设置Grid自适应列宽（根据单元格内容）
            gdOEMDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdOEMDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;

            gdBrandDetail.DataSource = _brandDetailGridDS;
            gdBrandDetail.DataBind();
            //设置Grid自适应列宽（根据单元格内容）
            gdBrandDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdBrandDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;

            _isAskLoadData = true;
        }

        /// <summary>
        /// 复制
        /// </summary>
        public override void CopyAction()
        {
            #region 检查详情是否已保存

            SetCardCtrlsToDetailDS();
            base.NewUIModel = HeadDS;
            if (ViewHasChanged(_skipPropertyList)
                || _oemDetailGridDS.InsertList.Count > 0
                || _oemDetailGridDS.UpdateList.Count > 0
                || _oemDetailGridDS.DeleteList.Count > 0
                || _brandDetailGridDS.InsertList.Count > 0
                || _brandDetailGridDS.UpdateList.Count > 0
                || _brandDetailGridDS.DeleteList.Count > 0)
            {
                //信息尚未保存，确定进行当前操作？
                DialogResult dialogResult = MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.W_0001), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
            }
            #endregion

            base.CopyAction();
            //ID
            txtVC_ID.Clear();
            //版本号
            txtVC_VersionNo.Clear();
            //车架号
            txtVC_VIN.Clear();
            //有效
            ckVC_IsValid.Checked = true;
            ckVC_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtVC_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtVC_CreatedTime.Value = DateTime.Now;
            //修改人
            txtVC_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtVC_UpdatedTime.Value = DateTime.Now;

            //原厂件信息
            var tempOemDetailGridDS = new SkyCarBindingList<VehicleOemPartsInfoUIModel, MDLBS_VehicleOemPartsInfo>();
            _bll.CopyModelList(_oemDetailGridDS, tempOemDetailGridDS);
            _oemDetailGridDS = new SkyCarBindingList<VehicleOemPartsInfoUIModel, MDLBS_VehicleOemPartsInfo>();
            _oemDetailGridDS.StartMonitChanges();
            _bll.CopyModelList(tempOemDetailGridDS, _oemDetailGridDS);
            _oemDetailGridDS.ForEach(x => x.VOPI_VC_VIN = null);
            gdOEMDetail.DataSource = _oemDetailGridDS;
            gdOEMDetail.DataBind();

            //品牌件信息
            var tempBrandDetailGridDS = new SkyCarBindingList<VehicleThirdPartsInfoUIModel, MDLBS_VehicleThirdPartsInfo>();
            _bll.CopyModelList(_brandDetailGridDS, tempBrandDetailGridDS);
            _brandDetailGridDS = new SkyCarBindingList<VehicleThirdPartsInfoUIModel, MDLBS_VehicleThirdPartsInfo>();
            _brandDetailGridDS.StartMonitChanges();
            _bll.CopyModelList(tempBrandDetailGridDS, _brandDetailGridDS);
            _brandDetailGridDS.ForEach(x => x.VTPI_VC_VIN = null);
            gdBrandDetail.DataSource = _brandDetailGridDS;
            gdBrandDetail.DataBind();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public override void DeleteAction()
        {
            #region 准备数据

            //待删除的[车辆信息]列表
            List<VehicleModelMatchAutoPartsManagerUIModel> deleteVehicleInfoList = new List<VehicleModelMatchAutoPartsManagerUIModel>();
            //待删除的[车辆原厂件信息]列表
            List<VehicleOemPartsInfoUIModel> deleteVehicleOemPartsInfoList = new List<VehicleOemPartsInfoUIModel>();
            //待删除的[车辆品牌件信息]列表
            List<VehicleThirdPartsInfoUIModel> deleteVehicleBrandPartsInfoList = new List<VehicleThirdPartsInfoUIModel>();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 详情删除

                if (string.IsNullOrEmpty(txtVC_ID.Text.Trim())
                    || string.IsNullOrEmpty(txtVC_VIN.Text.Trim()))
                {
                    //车辆信息为空，不能删除
                    MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.W_0016, new object[] { MsgParam.VEHICLE, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //确认删除操作
                DialogResult dialogResult = MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.W_0000, new object[] { "车架号对应[原厂件信息]和[品牌件信息]将一起删除，确定删除？" }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                //待删除的车辆信息
                VehicleModelMatchAutoPartsManagerUIModel deleteVehicleInfo = new VehicleModelMatchAutoPartsManagerUIModel
                {
                    WHERE_VC_ID = txtVC_ID.Text.Trim(),
                    VC_VIN = txtVC_VIN.Text.Trim(),
                    VC_Brand = mcbVC_Brand.SelectedValue,
                    VC_Inspire = mcbVC_Inspire.SelectedValue,
                    VC_BrandDesc = mcbVC_BrandDesc.SelectedValue,
                    VC_Capacity = mcbVC_Capacity.SelectedValue,
                    VC_EngineType = txtVC_EngineType.Text.Trim(),
                    VC_OperateType = "Delete",
                };
                deleteVehicleInfoList.Add(deleteVehicleInfo);

                //查询当前车架号对应的所有[原厂件信息]
                _bll.QueryForList(SQLID.BS_VehicleModelMatchAutoPartsManager_SQL02, new VehicleModelMatchAutoPartsManagerQCModel()
                {
                    WHERE_VC_VIN = txtVC_VIN.Text.Trim() + SysConst.Semicolon_DBC,
                    IsUsedDelete = true,
                }, deleteVehicleOemPartsInfoList);

                //查询当前车架号对应的所有[品牌件信息]
                _bll.QueryForList(SQLID.BS_VehicleModelMatchAutoPartsManager_SQL03, new VehicleModelMatchAutoPartsManagerQCModel()
                {
                    WHERE_VC_VIN = txtVC_VIN.Text.Trim() + SysConst.Semicolon_DBC,
                    IsUsedDelete = true,
                }, deleteVehicleBrandPartsInfoList);

                #endregion
            }
            else
            {
                #region 列表删除
                gdGrid.UpdateData();
                //勾选的车辆信息列表
                List<VehicleModelMatchAutoPartsManagerUIModel> checkedVehicleInfoList = HeadGridDS.Where(x => x.IsChecked).ToList();
                if (checkedVehicleInfoList.Count == 0)
                {
                    MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { MsgParam.VEHICLE, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //确认删除操作
                DialogResult dialogResult = MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.W_0000, new object[] { "车架号对应[原厂件信息]和[品牌件信息]将一起删除，确定删除？" }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                //车架号字符串
                string vinStr = string.Empty;

                foreach (var loopVehicleInfo in checkedVehicleInfoList)
                {
                    if (string.IsNullOrEmpty(loopVehicleInfo.VC_ID)
                        || string.IsNullOrEmpty(loopVehicleInfo.VC_VIN))
                    {
                        continue;
                    }
                    vinStr += loopVehicleInfo.VC_VIN + SysConst.Semicolon_DBC;

                    loopVehicleInfo.WHERE_VC_ID = loopVehicleInfo.VC_ID;
                    loopVehicleInfo.VC_OperateType = "Delete";
                    deleteVehicleInfoList.Add(loopVehicleInfo);
                }
                //查询勾选的车架号对应的[原厂件信息]
                _bll.QueryForList(SQLID.BS_VehicleModelMatchAutoPartsManager_SQL02, new VehicleModelMatchAutoPartsManagerQCModel()
                {
                    WHERE_VC_VIN = vinStr,
                    IsUsedDelete = true,
                }, deleteVehicleOemPartsInfoList);
                //查询勾选的车架号对应的[品牌件信息]
                _bll.QueryForList(SQLID.BS_VehicleModelMatchAutoPartsManager_SQL03, new VehicleModelMatchAutoPartsManagerQCModel()
                {
                    WHERE_VC_VIN = vinStr,
                    IsUsedDelete = true,
                }, deleteVehicleBrandPartsInfoList);
                #endregion
            }
            #endregion

            #region 删除数据
            if (deleteVehicleInfoList.Count > 0)
            {
                var deleteCodeTableResult = _bll.DeleteDetailDS(deleteVehicleInfoList, deleteVehicleOemPartsInfoList, deleteVehicleBrandPartsInfoList);
                if (!deleteCodeTableResult)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.BS, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //删除成功
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            base.ConditionDS = new VehicleModelMatchAutoPartsManagerQCModel()
            {
                //查询用SqlId 
                SqlId = SQLID.BS_VehicleModelMatchAutoPartsManager_SQL01,
                //车架号
                WHERE_VC_VIN = txtWHERE_VC_VIN.Text.Trim(),
                //品牌
                WHERE_VC_Brand = mcbWHERE_VC_Brand.SelectedValue,
                //车系
                WHERE_VC_Inspire = mcbWHERE_VC_Inspire.SelectedValue,
                //车型描述
                WHERE_VC_BrandDesc = mcbWHERE_VC_BrandDesc.SelectedValue,
                //排量
                WHERE_VC_Capacity = mcbWHERE_VC_Capacity.SelectedValue,
                //发动机型号
                WHERE_VC_EngineType = txtWHERE_VC_EngineType.Text.Trim(),
                //备注
                WHERE_VC_Remark = txtWHERE_VC_Remark.Text.Trim(),
            };
            //3.执行基类方法（注意：基类使用模糊查询）
            base.QueryAction();
            //4.Grid绑定数据源
            gdGrid.DataSource = base.HeadGridDS;
            gdGrid.DataBind();
            //5.设置Grid自适应列宽（根据单元格内容）
            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdGrid.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
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

        /// <summary>
        /// 关闭画面时检查画面值是否发生变化
        /// </summary>
        /// <returns></returns>
        public override bool ViewHasChangedWhenClose()
        {
            SetCardCtrlsToDetailDS();
            base.NewUIModel = HeadDS;
            if (ViewHasChanged(_skipPropertyList)
                || _oemDetailGridDS.InsertList.Count > 0
                || _oemDetailGridDS.UpdateList.Count > 0
                || _oemDetailGridDS.DeleteList.Count > 0
                || _brandDetailGridDS.InsertList.Count > 0
                || _brandDetailGridDS.UpdateList.Count > 0
                || _brandDetailGridDS.DeleteList.Count > 0)
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
            //车架号
            txtVC_VIN.Clear();
            //品牌
            mcbVC_Brand.Clear();
            //车系
            mcbVC_Inspire.Clear();
            //车型描述
            mcbVC_BrandDesc.Clear();
            //排量
            mcbVC_Capacity.Clear();
            //发动机型号
            txtVC_EngineType.Clear();
            //备注
            txtVC_Remark.Clear();
            //车辆信息ID
            txtVC_ID.Clear();
            //版本号
            txtVC_VersionNo.Clear();
            //车牌号
            txtVC_PlateNumber.Clear();
            //有效
            ckVC_IsValid.Checked = true;
            ckVC_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtVC_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtVC_CreatedTime.Value = DateTime.Now;
            //修改人
            txtVC_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtVC_UpdatedTime.Value = DateTime.Now;

            #endregion

            #region 初始化下拉框
            //品牌
            _vehicleBrandList = CacheDAX.Get(CacheDAX.ConfigDataKey.VehicleBrand) as List<MDLBS_VehicleBrandInspireSumma>;
            mcbVC_Brand.DisplayMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Brand;
            mcbVC_Brand.ValueMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Brand;
            mcbVC_Brand.DataSource = _vehicleBrandList;

            //车系
            _vehicleInspireList = CacheDAX.Get(CacheDAX.ConfigDataKey.VehicleBrandInspire) as List<MDLBS_VehicleBrandInspireSumma>;
            mcbVC_Inspire.DisplayMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Inspire;
            mcbVC_Inspire.ValueMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Inspire;
            if (_vehicleInspireList != null && !string.IsNullOrEmpty(mcbVC_Brand.SelectedValue))
            {
                var curVehicleInspireList = _vehicleInspireList.Where(x => x.VBIS_Brand == mcbVC_Brand.SelectedValue).ToList();
                mcbVC_Inspire.DataSource = curVehicleInspireList;
            }

            //车型描述
            _vehicleModelDescList = CacheDAX.Get(CacheDAX.ConfigDataKey.VehicleModelDesc) as List<MDLBS_VehicleBrandInspireSumma>;
            mcbVC_BrandDesc.DisplayMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_ModelDesc;
            mcbVC_BrandDesc.ValueMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_ModelDesc;
            if (_vehicleModelDescList != null && !string.IsNullOrEmpty(mcbVC_Brand.SelectedValue))
            {
                var curVehicleModelDescList = _vehicleModelDescList.Where(x => x.VBIS_Brand == mcbVC_Brand.SelectedValue).ToList();
                mcbVC_BrandDesc.DataSource = curVehicleModelDescList;
            }

            //排量：从码表获取
            _vehicleCapacityList = CodeTableHelp.GetEnumForComboBoxWithValueText(CodeType.VehicleCapacity);
            mcbVC_Capacity.DisplayMember = SysConst.EN_TEXT;
            mcbVC_Capacity.ValueMember = SysConst.EN_TEXT;
            mcbVC_Capacity.DataSource = _vehicleCapacityList;

            #endregion

            #region 初始化[原厂件信息]Grid和[品牌件信息]Grid
            //原厂件
            _oemDetailGridDS = new SkyCarBindingList<VehicleOemPartsInfoUIModel, MDLBS_VehicleOemPartsInfo>();
            _oemDetailGridDS.StartMonitChanges();
            gdOEMDetail.DataSource = _oemDetailGridDS;
            gdOEMDetail.DataBind();
            //品牌件
            _brandDetailGridDS = new SkyCarBindingList<VehicleThirdPartsInfoUIModel, MDLBS_VehicleThirdPartsInfo>();
            _brandDetailGridDS.StartMonitChanges();
            gdBrandDetail.DataSource = _brandDetailGridDS;
            gdBrandDetail.DataBind();
            #endregion

            _latestVin = string.Empty;
        }
       
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //车架号
            txtWHERE_VC_VIN.Clear();
            //品牌
            mcbWHERE_VC_Brand.Clear();
            //车系
            mcbWHERE_VC_Inspire.Clear();
            //车型描述
            mcbWHERE_VC_BrandDesc.Clear();
            //排量
            mcbWHERE_VC_Capacity.Clear();
            //发动机型号
            txtWHERE_VC_EngineType.Clear();
            //备注
            txtWHERE_VC_Remark.Clear();
            //给 车架号 设置焦点
            lblWHERE_VC_VIN.Focus();

            #endregion

            #region Grid初始化

            //清空Grid
            HeadGridDS = new BindingList<VehicleModelMatchAutoPartsManagerUIModel>();
            gdGrid.DataSource = HeadGridDS;
            gdGrid.DataBind();

            #endregion

            #endregion

            #region 初始化下拉框
            //品牌
            mcbWHERE_VC_Brand.DisplayMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Brand;
            mcbWHERE_VC_Brand.ValueMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Brand;
            mcbWHERE_VC_Brand.DataSource = _vehicleBrandList;

            //车系
            mcbWHERE_VC_Inspire.DisplayMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Inspire;
            mcbWHERE_VC_Inspire.ValueMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Inspire;
            mcbWHERE_VC_Inspire.DataSource = _vehicleInspireList;
            if (_vehicleInspireList != null && !string.IsNullOrEmpty(mcbWHERE_VC_Brand.SelectedValue))
            {
                var curVehicleInspireList = _vehicleInspireList.Where(x => x.VBIS_Brand == mcbWHERE_VC_Brand.SelectedValue).ToList();
                mcbWHERE_VC_Inspire.DataSource = curVehicleInspireList;
            }

            //车型描述
            mcbWHERE_VC_BrandDesc.DisplayMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_ModelDesc;
            mcbWHERE_VC_BrandDesc.ValueMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_ModelDesc;
            if (_vehicleModelDescList != null
                && !string.IsNullOrEmpty(mcbWHERE_VC_Brand.SelectedValue)
                && !string.IsNullOrEmpty(mcbWHERE_VC_Inspire.SelectedValue))
            {
                var curVehicleModelDescList = _vehicleModelDescList.Where(x => x.VBIS_Brand == mcbWHERE_VC_Brand.SelectedValue && x.VBIS_Inspire == mcbWHERE_VC_Inspire.SelectedValue).ToList();
                mcbWHERE_VC_BrandDesc.DataSource = curVehicleModelDescList;
            }

            //排量
            mcbWHERE_VC_Capacity.DisplayMember = SysConst.EN_TEXT;
            mcbWHERE_VC_Capacity.ValueMember = SysConst.EN_TEXT;
            mcbWHERE_VC_Capacity.DataSource = _vehicleCapacityList;
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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.BS_VehicleInfo.Code.VC_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.BS_VehicleInfo.Code.VC_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[HeadDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            HeadDS = base.HeadGridDS.FirstOrDefault(x => x.VC_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.BS_VehicleInfo.Code.VC_ID].Value);
            if (HeadDS == null || string.IsNullOrEmpty(HeadDS.VC_ID))
            {
                return;
            }

            //将HeadDS数据赋值给【详情】Tab内的对应控件
            _isAskLoadData = false;

            if (txtVC_ID.Text != HeadDS.VC_ID
                || (txtVC_ID.Text == HeadDS.VC_ID && txtVC_VersionNo.Text != HeadDS.VC_VersionNo?.ToString()))
            {
                if (txtVC_ID.Text == HeadDS.VC_ID && txtVC_VersionNo.Text != HeadDS.VC_VersionNo?.ToString())
                {
                    //数据版本已过期，将加载最新详情。
                    MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.DataHasOverdue }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else if (ViewHasChanged(_skipPropertyList)
                || _oemDetailGridDS.InsertList.Count > 0
                || _oemDetailGridDS.UpdateList.Count > 0
                || _oemDetailGridDS.DeleteList.Count > 0
                || _brandDetailGridDS.InsertList.Count > 0
                || _brandDetailGridDS.UpdateList.Count > 0
                || _brandDetailGridDS.DeleteList.Count > 0)
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

            //查询明细Grid数据并绑定
            QueryOemDetail();
            QueryBrandDetail();

            _isAskLoadData = true;
        }

        /// <summary>
        /// 将HeadDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //车架号
            txtVC_VIN.Text = HeadDS.VC_VIN;
            //品牌
            mcbVC_Brand.SelectedValue = HeadDS.VC_Brand;
            //车系
            mcbVC_Inspire.SelectedValue = HeadDS.VC_Inspire;
            //车型描述
            mcbVC_BrandDesc.SelectedValue = HeadDS.VC_BrandDesc;
            //排量
            mcbVC_Capacity.SelectedValue = HeadDS.VC_Capacity ?? string.Empty;
            //发动机型号
            txtVC_EngineType.Text = HeadDS.VC_EngineType;
            //备注
            txtVC_Remark.Text = HeadDS.VC_Remark;
            //有效
            if (HeadDS.VC_IsValid != null)
            {
                ckVC_IsValid.Checked = HeadDS.VC_IsValid.Value;
            }
            //创建人
            txtVC_CreatedBy.Text = HeadDS.VC_CreatedBy;
            //创建时间
            dtVC_CreatedTime.Value = HeadDS.VC_CreatedTime;
            //修改人
            txtVC_UpdatedBy.Text = HeadDS.VC_UpdatedBy;
            //修改时间
            dtVC_UpdatedTime.Value = HeadDS.VC_UpdatedTime;
            //车辆信息ID
            txtVC_ID.Text = HeadDS.VC_ID;
            //版本号
            txtVC_VersionNo.Value = HeadDS.VC_VersionNo;
            //车牌号
            txtVC_PlateNumber.Text = HeadDS.VC_PlateNumber;
        }

        /// <summary>
        /// 查询[原厂件信息]Grid数据并绑定
        /// </summary>
        private void QueryOemDetail()
        {
            //1.设置查询条件
            var argsCondition = new VehicleModelMatchAutoPartsManagerQCModel()
            {
                //查询用SqlId 
                SqlId = SQLID.BS_VehicleModelMatchAutoPartsManager_SQL02,
                WHERE_VC_VIN = txtVC_VIN.Text.Trim() + SysConst.Semicolon_DBC,
            };
            //2.执行查询方法
            _bll.QueryForList(argsCondition.SqlId, argsCondition, _oemDetailGridDS);
            //3.开始监控List变化（该数据源若有其他调整，请在StartMonitChanges()方法执行前调整）
            _oemDetailGridDS.StartMonitChanges();
            //4.Grid绑定数据源
            gdOEMDetail.DataSource = _oemDetailGridDS;
            gdOEMDetail.DataBind();
            //5.设置Grid自适应列宽（根据单元格内容）
            gdOEMDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdOEMDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 查询[品牌件信息]Grid数据并绑定
        /// </summary>
        private void QueryBrandDetail()
        {
            //1.设置查询条件
            var argsCondition = new VehicleModelMatchAutoPartsManagerQCModel()
            {
                //查询用SqlId 
                SqlId = SQLID.BS_VehicleModelMatchAutoPartsManager_SQL03,
                WHERE_VC_VIN = txtVC_VIN.Text.Trim() + SysConst.Semicolon_DBC,
            };
            //2.执行查询方法
            _bll.QueryForList(argsCondition.SqlId, argsCondition, _brandDetailGridDS);
            //3.开始监控List变化（该数据源若有其他调整，请在StartMonitChanges()方法执行前调整）
            _brandDetailGridDS.StartMonitChanges();
            //4.Grid绑定数据源
            gdBrandDetail.DataSource = _brandDetailGridDS;
            gdBrandDetail.DataBind();
            //5.设置Grid自适应列宽（根据单元格内容）
            gdBrandDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdBrandDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
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
            //验证车架号是否为空
            if (string.IsNullOrEmpty(txtVC_VIN.Text.Trim()))
            {
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "请先输入17位的车架号" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //验证车架号是否为17位
            if (txtVC_VIN.Text.Trim().Length != 17)
            {
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "请输入17位的车架号" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //检查车架号是否已存在
            var curVinIsExist = _bll.QueryForObject<int>(SQLID.BS_VehicleModelMatchAutoPartsManager_SQL04, new MDLBS_VehicleInfo
            {
                WHERE_VC_ID = txtVC_ID.Text.Trim(),
                WHERE_VC_VIN = txtVC_VIN.Text.Trim(),
            });
            if (curVinIsExist > 0)
            {
                //车架号已存在
                var isJoin = MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "车架号已存在，是否合并原厂件和品牌件信息？单击【确定】将合并数据，【取消】返回。" }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (isJoin != DialogResult.OK)
                {
                    _isJoinVinInfo = false;
                    return false;
                }
                _isJoinVinInfo = true;
            }
            return true;
        }

        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的HeadDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            HeadDS = new VehicleModelMatchAutoPartsManagerUIModel()
            {
                //车架号
                VC_VIN = txtVC_VIN.Text.Trim(),
                //品牌
                VC_Brand = mcbVC_Brand.SelectedValue,
                //车系
                VC_Inspire = mcbVC_Inspire.SelectedValue,
                //车型描述
                VC_BrandDesc = mcbVC_BrandDesc.SelectedValue,
                //排量
                VC_Capacity = mcbVC_Capacity.SelectedValue,
                //发动机型号
                VC_EngineType = txtVC_EngineType.Text.Trim(),
                //备注
                VC_Remark = txtVC_Remark.Text.Trim(),
                //有效
                VC_IsValid = ckVC_IsValid.Checked,
                //创建人
                VC_CreatedBy = txtVC_CreatedBy.Text.Trim(),
                //创建时间
                VC_CreatedTime = (DateTime?)dtVC_CreatedTime.Value ?? DateTime.Now,
                //修改人
                VC_UpdatedBy = txtVC_UpdatedBy.Text.Trim(),
                //修改时间
                VC_UpdatedTime = (DateTime?)dtVC_UpdatedTime.Value ?? DateTime.Now,
                //车辆信息ID
                VC_ID = txtVC_ID.Text.Trim(),
                //版本号
                VC_VersionNo = Convert.ToInt64(txtVC_VersionNo.Text.Trim() == "" ? "1" : txtVC_VersionNo.Text.Trim()),
                //车牌号
                VC_PlateNumber = txtVC_PlateNumber.Text.Trim(),
            };
        }

        #region [原厂件信息]相关

        /// <summary>
        /// 添加原厂件
        /// </summary>
        private void AddOEMAutoPartsDetail()
        {
            #region 验证
            //验证车架号是否为空
            if (string.IsNullOrEmpty(txtVC_VIN.Text.Trim()))
            {
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "请先输入17位的车架号" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //验证车架号是否为17位
            if (txtVC_VIN.Text.Trim().Length != 17)
            {
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "请输入17位的车架号" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion

            #region 添加原厂件信息
            gdOEMDetail.UpdateData();
            VehicleOemPartsInfoUIModel newOemPartsInfo = new VehicleOemPartsInfoUIModel
            {
                VOPI_VC_VIN = txtVC_VIN.Text.Trim()
            };
            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //参数Model
                {ComViewParamKey.DestModel.ToString(), newOemPartsInfo},
                //维护原厂件信息Func
                {BSViewParamKey.MaintainOemPartsInfo.ToString(),_maintainOemPartsInfoFunc},
            };

            FrmMaintainOEMPartsInfo frmMaintainOemPartsInfo = new FrmMaintainOEMPartsInfo(paramViewParameters)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            DialogResult dialogResult = frmMaintainOemPartsInfo.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            #endregion

            gdOEMDetail.DataSource = _oemDetailGridDS;
            gdOEMDetail.DataBind();
            //设置Grid自适应列宽（根据单元格内容）
            gdOEMDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdOEMDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 删除原厂件
        /// </summary>
        private void DeleteOEMAutoPartsDetail()
        {
            gdOEMDetail.UpdateData();
            //待移除的原厂件
            var removeOemInfoList = _oemDetailGridDS.Where(x => x.IsChecked).ToList();
            if (removeOemInfoList.Count == 0)
            {
                //请勾选至少一条原厂件信息进行删除！
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { MsgParam.OEM_AUTOPARTS, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //移除勾选的原厂件
            foreach (var loopOemDetail in removeOemInfoList)
            {
                _oemDetailGridDS.Remove(loopOemDetail);
            }

            gdOEMDetail.DataSource = _oemDetailGridDS;
            gdOEMDetail.DataBind();
        }

        /// <summary>
        /// 维护车辆原厂件信息Func
        /// </summary>
        /// <param name="paramOemPartsInfo"></param>
        /// <returns></returns>
        private bool MaintainOemPartsInfo(VehicleOemPartsInfoUIModel paramOemPartsInfo)
        {
            if (paramOemPartsInfo == null)
            {
                //原厂件信息为空，添加失败
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0016, new object[] { MsgParam.OEM_AUTOPARTS, MsgParam.ADD }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (string.IsNullOrEmpty(paramOemPartsInfo.VOPI_OEMNo))
            {
                //原厂编码为空，添加失败
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0016, new object[] { SystemTableColumnEnums.BS_VehicleOemPartsInfo.Name.VOPI_OEMNo, MsgParam.ADD }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (string.IsNullOrEmpty(paramOemPartsInfo.VOPI_AutoPartsName))
            {
                //配件名称为空，添加失败
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0016, new object[] { SystemTableColumnEnums.BS_VehicleOemPartsInfo.Name.VOPI_AutoPartsName, MsgParam.ADD }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            foreach (var loopOemPartsInfo in _oemDetailGridDS)
            {
                if (loopOemPartsInfo.VOPI_OEMNo == paramOemPartsInfo.VOPI_OEMNo)
                {
                    MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, "相同的原厂编码已存在"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            //验证原厂编码是否已存在
            MDLBS_VehicleOemPartsInfo resultOemPartsInfo = new MDLBS_VehicleOemPartsInfo();
            _bll.QueryForObject<MDLBS_VehicleOemPartsInfo, MDLBS_VehicleOemPartsInfo>(new MDLBS_VehicleOemPartsInfo
            {
                WHERE_VOPI_VC_VIN = paramOemPartsInfo.VOPI_VC_VIN,
                WHERE_VOPI_OEMNo = paramOemPartsInfo.VOPI_OEMNo,
                WHERE_VOPI_IsValid = true
            }, resultOemPartsInfo);
            if (!string.IsNullOrEmpty(resultOemPartsInfo.VOPI_ID))
            {
                _bll.CopyModel(resultOemPartsInfo, paramOemPartsInfo);
            }
            //待添加的原厂件信息
            VehicleOemPartsInfoUIModel newOemPartsInfo = new VehicleOemPartsInfoUIModel()
            {
                VOPI_VC_VIN = paramOemPartsInfo.VOPI_VC_VIN,
                VOPI_OEMNo = paramOemPartsInfo.VOPI_OEMNo,
                VOPI_AutoPartsName = paramOemPartsInfo.VOPI_AutoPartsName,
                VOPI_Remark = paramOemPartsInfo.VOPI_Remark,
            };
            _oemDetailGridDS.Add(newOemPartsInfo);
            gdOEMDetail.DataSource = _oemDetailGridDS;
            gdOEMDetail.DataBind();
            //设置Grid自适应列宽（根据单元格内容）
            gdOEMDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdOEMDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;

            return true;
        }
        #endregion

        #region [品牌件信息]相关

        /// <summary>
        /// 添加品牌件
        /// </summary>
        private void AddBrandAutoPartsDetail()
        {
            #region 验证

            //验证车架号是否为空
            if (string.IsNullOrEmpty(txtVC_VIN.Text.Trim()))
            {
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "请先输入17位的车架号" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //验证车架号是否为17位
            if (txtVC_VIN.Text.Trim().Length != 17)
            {
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "请输入17位的车架号" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #endregion

            #region 添加品牌件信息
            VehicleThirdPartsInfoUIModel newBrandPartsInfo = new VehicleThirdPartsInfoUIModel
            {
                VTPI_VC_VIN = txtVC_VIN.Text.Trim(),
            };
            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //参数Model
                {ComViewParamKey.DestModel.ToString(), newBrandPartsInfo},
                //维护品牌件信息Func
                {BSViewParamKey.MaintainBrandPartsInfo.ToString(),_maintainBrandPartsInfoFunc},
            };

            FrmMaintainBrandPartsInfo frmMaintainBrandPartsInfo = new FrmMaintainBrandPartsInfo(paramViewParameters)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            DialogResult dialogResult = frmMaintainBrandPartsInfo.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            #endregion

            gdBrandDetail.DataSource = _brandDetailGridDS;
            gdBrandDetail.DataBind();
            //设置Grid自适应列宽（根据单元格内容）
            gdBrandDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdBrandDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 删除品牌件
        /// </summary>
        private void DeleteBrandAutoPartsDetail()
        {
            gdBrandDetail.UpdateData();
            //待移除的品牌件
            var removeBrandInfoList = _brandDetailGridDS.Where(x => x.IsChecked).ToList();
            if (removeBrandInfoList.Count == 0)
            {
                //请勾选至少一条品牌件信息进行删除！
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { MsgParam.BRAND_AUTOPARTS, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //移除勾选的品牌件
            foreach (var loopBrandDetail in removeBrandInfoList)
            {
                _brandDetailGridDS.Remove(loopBrandDetail);
            }

            gdBrandDetail.DataSource = _brandDetailGridDS;
            gdBrandDetail.DataBind();
        }

        /// <summary>
        /// 维护车辆品牌件信息Func
        /// </summary>
        /// <param name="paramBrandPartsInfo"></param>
        /// <returns></returns>
        private bool MaintainBrandPartsInfo(VehicleThirdPartsInfoUIModel paramBrandPartsInfo)
        {
            if (paramBrandPartsInfo == null)
            {
                //品牌件信息为空，添加失败
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0016, new object[] { MsgParam.BRAND_AUTOPARTS, MsgParam.ADD }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (string.IsNullOrEmpty(paramBrandPartsInfo.VTPI_ThirdNo))
            {
                //第三方编码为空，添加失败
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0016, new object[] { SystemTableColumnEnums.BS_VehicleThirdPartsInfo.Name.VTPI_ThirdNo, MsgParam.ADD }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (string.IsNullOrEmpty(paramBrandPartsInfo.VTPI_AutoPartsName))
            {
                //配件名称为空，添加失败
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0016, new object[] { SystemTableColumnEnums.BS_VehicleThirdPartsInfo.Name.VTPI_AutoPartsName, MsgParam.ADD }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (string.IsNullOrEmpty(paramBrandPartsInfo.VTPI_AutoPartsBrand))
            {
                //配件品牌为空，添加失败
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0016, new object[] { SystemTableColumnEnums.BS_VehicleThirdPartsInfo.Name.VTPI_AutoPartsBrand, MsgParam.ADD }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            foreach (var loopBrandPartsInfo in _brandDetailGridDS)
            {
                if (loopBrandPartsInfo.VTPI_ThirdNo == paramBrandPartsInfo.VTPI_ThirdNo)
                {
                    MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, "相同的第三方编码已存在"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            //验证第三方编码是否已存在
            MDLBS_VehicleThirdPartsInfo resultBrandPartsInfo = new MDLBS_VehicleThirdPartsInfo();
            _bll.QueryForObject<MDLBS_VehicleThirdPartsInfo, MDLBS_VehicleThirdPartsInfo>(new MDLBS_VehicleThirdPartsInfo
            {
                WHERE_VTPI_VC_VIN = paramBrandPartsInfo.VTPI_VC_VIN,
                WHERE_VTPI_ThirdNo = paramBrandPartsInfo.VTPI_ThirdNo,
                WHERE_VTPI_IsValid = true
            }, resultBrandPartsInfo);
            if (!string.IsNullOrEmpty(resultBrandPartsInfo.VTPI_ID))
            {
                _bll.CopyModel(resultBrandPartsInfo, paramBrandPartsInfo);
            }
            //待添加的品牌件信息
            VehicleThirdPartsInfoUIModel newBrandPartsInfo = new VehicleThirdPartsInfoUIModel()
            {
                VTPI_VC_VIN = paramBrandPartsInfo.VTPI_VC_VIN,
                VTPI_ThirdNo = paramBrandPartsInfo.VTPI_ThirdNo,
                VTPI_AutoPartsName = paramBrandPartsInfo.VTPI_AutoPartsName,
                VTPI_AutoPartsBrand = paramBrandPartsInfo.VTPI_AutoPartsBrand,
                VTPI_Remark = paramBrandPartsInfo.VTPI_Remark,
            };
            _brandDetailGridDS.Add(newBrandPartsInfo);
            gdBrandDetail.DataSource = _brandDetailGridDS;
            gdBrandDetail.DataBind();
            //设置Grid自适应列宽（根据单元格内容）
            gdBrandDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdBrandDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;

            return true;
        }
        #endregion

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
                    var removeList = HeadGridDS.Where(x => x.IsChecked).ToList();
                    foreach (var loopRemove in removeList)
                    {
                        HeadGridDS.Remove(loopRemove);
                    }
                }
                else
                {
                    var curHead = HeadGridDS.FirstOrDefault(x => x.VC_ID == HeadDS.VC_ID);
                    if (curHead != null)
                    {
                        HeadGridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = HeadGridDS.FirstOrDefault(x => x.VC_ID == HeadDS.VC_ID);
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
