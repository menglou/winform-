using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.BS;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.BS;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;

namespace SkyCar.Coeus.UI.BS
{
    /// <summary>
    /// 品牌车系
    /// </summary>
    public partial class FrmVehicleBrandInspireSummaManager : BaseFormCardList<VehicleBrandInspireSummaManagerUIModel, VehicleBrandInspireSummaManagerQCModel, MDLBS_VehicleBrandInspireSumma>
    {
        #region 全局变量

        /// <summary>
        /// 品牌车系BLL
        /// </summary>
        private VehicleBrandInspireSummaManagerBLL _bll = new VehicleBrandInspireSummaManagerBLL();

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
        /// 车辆类型
        /// </summary>
        ObservableCollection<CodeTableValueTextModel> _vehicleModelList = new ObservableCollection<CodeTableValueTextModel>();

        /// <summary>
        /// 车型描述
        /// </summary>
        List<MDLBS_VehicleBrandInspireSumma> _vehicleModelDescList = new List<MDLBS_VehicleBrandInspireSumma>();

        #endregion

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmVehicleBrandInspireSummaManager构造方法
        /// </summary>
        public FrmVehicleBrandInspireSummaManager()
        {
            InitializeComponent();
        }
       
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmVehicleBrandInspireSummaManager_Load(object sender, EventArgs e)
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
            #endregion

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

        /// <summary>
        /// 选中的Tab改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlFull_SelectedTabChanging(object sender, SelectedTabChangingEventArgs e)
        {
            base.SetActionEnableBySelectedTab(e.Tab.Key);
        }
       
        #region 查询条件相关事件

        /// <summary>
        /// 【列表】品牌改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbWhere_VBIS_Brand_SelectedIndexChanged(object sender, EventArgs e)
        {
            mcbWhere_VBIS_Inspire.Clear();
            mcbWhere_VBIS_ModelDesc.Clear();
            if (string.IsNullOrEmpty(mcbWhere_VBIS_Brand.SelectedValue))
            {
                mcbWhere_VBIS_Inspire.DataSource = null;
                mcbWhere_VBIS_ModelDesc.DataSource = null;
                return;
            }

            //车系
            if (_vehicleInspireList != null)
            {
                var curVehicleInspireList = _vehicleInspireList.Where(x => !string.IsNullOrEmpty(x.VBIS_Inspire) && mcbWhere_VBIS_Brand.SelectedValue.Contains(x.VBIS_Brand)).ToList();
                mcbWhere_VBIS_Inspire.DataSource = curVehicleInspireList;
            }
        }

        /// <summary>
        /// 【列表】车系改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbWhere_VBIS_Inspire_SelectedIndexChanged(object sender, EventArgs e)
        {
            mcbWhere_VBIS_ModelDesc.Clear();
            if (string.IsNullOrEmpty(mcbWhere_VBIS_Inspire.SelectedValue))
            {
                mcbWhere_VBIS_ModelDesc.DataSource = null;
                return;
            }

            //车型描述
            if (_vehicleModelDescList != null)
            {
                var curVehicleModelDescList = _vehicleModelDescList.Where(x => !string.IsNullOrEmpty(x.VBIS_ModelDesc) && mcbWhere_VBIS_Brand.SelectedValue.Contains(x.VBIS_Brand) && mcbWhere_VBIS_Inspire.SelectedValue.Contains(x.VBIS_Inspire)).ToList();
                mcbWhere_VBIS_ModelDesc.DataSource = curVehicleModelDescList;
            }
        }

        #endregion

        #region 详情相关事件

        /// <summary>
        /// 品牌值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbVBIS_Brand_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(mcbVBIS_Brand.SelectedValue))
            {
                mcbVBIS_Inspire.Clear();
                mcbVBIS_Inspire.DataSource = null;
                return;
            }
            mcbVBIS_Inspire.Clear();

            //车系
            if (_vehicleInspireList != null)
            {
                var curVehicleInspireList = _vehicleInspireList.Where(x => !string.IsNullOrEmpty(x.VBIS_Inspire) && mcbVBIS_Brand.SelectedValue.Contains(x.VBIS_Brand)).ToList();
                mcbVBIS_Inspire.DataSource = curVehicleInspireList;
            }

            txtVBIS_BrandSpellCode.Text = ChineseSpellCode.GetShortSpellCode(mcbVBIS_Brand.SelectedValue);
        }
       
        /// <summary>
        /// 车系值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbVBIS_Inspire_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtVBIS_InspireSpellCode.Text = ChineseSpellCode.GetShortSpellCode(mcbVBIS_Inspire.SelectedValue);
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
                DialogResult dialogResult = MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.W_0001), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
            }
            #endregion

            //执行基类方法
            base.NewAction();
            //初始化【详情】Tab内控件
            InitializeDetailTabControls();
            //设置【详情】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;

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
            base.ConditionDS = new VehicleBrandInspireSummaManagerQCModel()
            {
                //品牌
                WHERE_VBIS_Brand = mcbWhere_VBIS_Brand.SelectedValue,
                //车系
                WHERE_VBIS_Inspire = mcbWhere_VBIS_Inspire.SelectedValue,
                //车辆类型
                WHERE_VBIS_Model = cbWhere_VBIS_Model.Text,
                //车型描述
                WHERE_VBIS_ModelDesc = mcbWhere_VBIS_ModelDesc.SelectedValue,
            };
            //3.执行基类方法
            base.QueryAction();
            //4.Grid绑定数据源
            gdGrid.DataSource = base.GridDS;
            gdGrid.DataBind();
            //5.设置Grid自适应列宽（根据单元格内容）
            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdGrid.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
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
            var saveResult = _bll.SaveDetailDS(DetailDS);
            if (!saveResult)
            {
                MessageBoxs.Show(Trans.BS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            #region 准备数据
            //待删除的车辆品牌车系列表
            List<MDLBS_VehicleBrandInspireSumma> deleteVehicleBrandInspireList = new List<MDLBS_VehicleBrandInspireSumma>();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 详情删除
                if (string.IsNullOrEmpty(txtVBIS_ID.Text))
                {
                    MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0016, new object[] { SystemTableEnums.Name.BS_VehicleBrandInspireSumma, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //验证品牌车系是否被引用过
                List<MDLBS_AutoPartsArchive> usedBrandAndInspireList = new List<MDLBS_AutoPartsArchive>();
                _bll.QueryForList(SQLID.BS_VehicleBrandInspireSummaManager_SQL02, new MDLBS_VehicleBrandInspireSumma
                {
                    WHERE_VBIS_Brand = mcbVBIS_Brand.SelectedValue + SysConst.Semicolon_DBC,
                    WHERE_VBIS_Inspire = mcbVBIS_Inspire.SelectedValue + SysConst.Semicolon_DBC,
                }, usedBrandAndInspireList);
                if (usedBrandAndInspireList.Count > 0)
                {
                    //品牌车系已经被使用，不能删除
                    MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0007, new object[] { mcbVBIS_Brand.SelectedText + SysConst.Comma_DBC + mcbVBIS_Inspire.SelectedText, MsgParam.APPLY, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //确认删除
                DialogResult dialogResult = MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0012), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                //待删除的车辆品牌车系
                MDLBS_VehicleBrandInspireSumma vehicleBrandInspireToDelete = new MDLBS_VehicleBrandInspireSumma
                {
                    WHERE_VBIS_ID = txtVBIS_ID.Text.Trim(),
                    VBIS_Brand = mcbVBIS_Brand.SelectedValue,
                    VBIS_Inspire = mcbVBIS_Inspire.SelectedValue,
                };
                deleteVehicleBrandInspireList.Add(vehicleBrandInspireToDelete);
                #endregion
            }
            else
            {
                #region 列表删除
                gdGrid.UpdateData();
                //勾选的品牌车系列表
                List<VehicleBrandInspireSummaManagerUIModel> checkedVehicleBrandInspireList = GridDS.Where(x => x.IsChecked == true).ToList();
                if (checkedVehicleBrandInspireList.Count == 0)
                {
                    MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { SystemTableEnums.Name.BS_VehicleBrandInspireSumma, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //验证品牌车系是否被引用过
                string vehicleBrandStr = string.Empty;
                string vehicleInspireStr = string.Empty;
                foreach (var loopVehicleBrandInspire in checkedVehicleBrandInspireList)
                {
                    if (string.IsNullOrEmpty(loopVehicleBrandInspire.VBIS_Brand)
                        || string.IsNullOrEmpty(loopVehicleBrandInspire.VBIS_Inspire))
                    {
                        continue;
                    }
                    vehicleBrandStr += loopVehicleBrandInspire.VBIS_Brand + SysConst.Semicolon_DBC;
                    vehicleInspireStr += loopVehicleBrandInspire.VBIS_Inspire + SysConst.Semicolon_DBC;
                }
                List<MDLBS_AutoPartsArchive> usedBrandAndInspireList = new List<MDLBS_AutoPartsArchive>();
                _bll.QueryForList(SQLID.BS_VehicleBrandInspireSummaManager_SQL02, new MDLBS_VehicleBrandInspireSumma
                {
                    WHERE_VBIS_Brand = vehicleBrandStr,
                    WHERE_VBIS_Inspire = vehicleInspireStr,
                }, usedBrandAndInspireList);
                if (usedBrandAndInspireList.Count > 0)
                {
                    string usedBrandstring = string.Empty;
                    foreach (var loopBrandAndInspire in usedBrandAndInspireList)
                    {
                        if (string.IsNullOrEmpty(loopBrandAndInspire.APA_VehicleBrand))
                        {
                            continue;
                        }
                        usedBrandstring += loopBrandAndInspire.APA_VehicleBrand + SysConst.Comma_DBC;
                    }
                    //品牌车系已经被使用，不能删除
                    MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0007, new object[] { usedBrandstring, MsgParam.APPLY, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //确认删除
                DialogResult dialogResult = MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0013, new object[] { checkedVehicleBrandInspireList.Count }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                foreach (var loopCheckedVehicleBrandInspire in checkedVehicleBrandInspireList)
                {
                    if (string.IsNullOrEmpty(loopCheckedVehicleBrandInspire.VBIS_ID))
                    {
                        continue;
                    }

                    //待删除的车辆品牌车系
                    MDLBS_VehicleBrandInspireSumma vehicleBrandInspireToDelete = new MDLBS_VehicleBrandInspireSumma
                    {
                        WHERE_VBIS_ID = loopCheckedVehicleBrandInspire.VBIS_ID,
                        VBIS_Brand = loopCheckedVehicleBrandInspire.VBIS_Brand,
                        VBIS_Inspire = loopCheckedVehicleBrandInspire.VBIS_Inspire,
                    };
                    deleteVehicleBrandInspireList.Add(vehicleBrandInspireToDelete);
                }
                #endregion
            }
            #endregion

            #region 删除数据
            if (deleteVehicleBrandInspireList.Count > 0)
            {
                var deleteResult = _bll.DeleteVehicleBrandInspire(deleteVehicleBrandInspireList);
                if (!deleteResult)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.BS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //删除成功
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (ViewHasChanged())
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
            paramGridName = SystemTableEnums.Name.BS_VehicleBrandInspireSumma;
            base.ExportAction(gdGrid, paramGridName);
        }
       
        /// <summary>
        /// 导出全部
        /// </summary>
        public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = SystemTableEnums.Name.BS_VehicleBrandInspireSumma;
            List<VehicleBrandInspireSummaManagerUIModel> resultAllList = new List<VehicleBrandInspireSummaManagerUIModel>();
            _bll.QueryForList<MDLBS_VehicleBrandInspireSumma, VehicleBrandInspireSummaManagerUIModel>(new VehicleBrandInspireSummaManagerQCModel()
            {
                PageIndex = 1,
                PageSize = null,
                //品牌
                WHERE_VBIS_Brand = mcbWhere_VBIS_Brand.SelectedValue,
                //车系
                WHERE_VBIS_Inspire = mcbWhere_VBIS_Inspire.SelectedValue,
                //车辆类型
                WHERE_VBIS_Model = cbWhere_VBIS_Model.Text,
                //车型描述
                WHERE_VBIS_ModelDesc = mcbWhere_VBIS_ModelDesc.SelectedValue,
            }, resultAllList);
            UltraGrid allGrid = gdGrid;
            allGrid.DataSource = resultAllList;
            allGrid.DataBind();

            base.ExportAllAction(allGrid, paramGridName);

            gdGrid.DataSource = GridDS;
            gdGrid.DataBind();
        }
        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 工具生成
            //品牌
            mcbVBIS_Brand.Clear();
            //品牌拼音首字母
            txtVBIS_BrandSpellCode.Clear();
            //车系
            mcbVBIS_Inspire.Clear();
            //车系拼音首字母
            txtVBIS_InspireSpellCode.Clear();
            //车辆类型
            cbVBIS_Model.Items.Clear();
            //车型描述
            txtVBIS_ModelDesc.Clear();
            //有效
            ckVBIS_IsValid.Checked = true;
            ckVBIS_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtVBIS_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtVBIS_CreatedTime.Value = BLLCom.GetCurStdDatetime();
            //修改人
            txtVBIS_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtVBIS_UpdatedTime.Value = BLLCom.GetCurStdDatetime();
            //品牌车系ID
            txtVBIS_ID.Clear();
            //版本号
            txtVBIS_VersionNo.Clear();
            //给 品牌 设置焦点
            lblVBIS_Brand.Focus();
            #endregion

            #region 初始化下拉框
            //品牌
            _vehicleBrandList = CacheDAX.Get(CacheDAX.ConfigDataKey.VehicleBrand) as List<MDLBS_VehicleBrandInspireSumma>;
            mcbVBIS_Brand.DisplayMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Brand;
            mcbVBIS_Brand.ValueMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Brand;
            mcbVBIS_Brand.DataSource = _vehicleBrandList;

            //车系
            _vehicleInspireList = CacheDAX.Get(CacheDAX.ConfigDataKey.VehicleBrandInspire) as List<MDLBS_VehicleBrandInspireSumma>;
            mcbVBIS_Inspire.DisplayMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Inspire;
            mcbVBIS_Inspire.ValueMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Inspire;
            if (_vehicleInspireList != null && !string.IsNullOrEmpty(mcbVBIS_Brand.SelectedValue))
            {
                var curVehicleInspireList = _vehicleInspireList.Where(x => x.VBIS_Brand == mcbVBIS_Brand.SelectedValue).ToList();
                mcbVBIS_Inspire.DataSource = curVehicleInspireList;
            }

            //车辆类型：从码表获取
            _vehicleModelList = CodeTableHelp.GetEnumForComboBoxWithValueText(CodeType.VehicleModel);
            cbVBIS_Model.DisplayMember = SysConst.EN_TEXT;
            cbVBIS_Model.ValueMember = SysConst.Value;
            cbVBIS_Model.DataSource = _vehicleModelList;
            cbVBIS_Model.DataBind();

            #endregion

        }
       
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //品牌
            mcbWhere_VBIS_Brand.Clear();
            //车系
            mcbWhere_VBIS_Inspire.Clear();
            //车辆类型
            cbWhere_VBIS_Model.Clear();
            //车型描述
            mcbWhere_VBIS_ModelDesc.Clear();
            #endregion

            //清空Grid
            GridDS = new BindingList<VehicleBrandInspireSummaManagerUIModel>();
            gdGrid.DataSource = base.GridDS;
            gdGrid.DataBind();

            base.ClearAction();

            #endregion

            #region 初始化下拉框

            //品牌
            _vehicleBrandList = CacheDAX.Get(CacheDAX.ConfigDataKey.VehicleBrand) as List<MDLBS_VehicleBrandInspireSumma>;
            mcbWhere_VBIS_Brand.DisplayMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Brand;
            mcbWhere_VBIS_Brand.ValueMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Brand;
            mcbWhere_VBIS_Brand.DataSource = _vehicleBrandList;

            //车系
            _vehicleInspireList = CacheDAX.Get(CacheDAX.ConfigDataKey.VehicleBrandInspire) as List<MDLBS_VehicleBrandInspireSumma>;
            mcbWhere_VBIS_Inspire.DisplayMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Inspire;
            mcbWhere_VBIS_Inspire.ValueMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Inspire;
            if (_vehicleInspireList != null && !string.IsNullOrEmpty(mcbWhere_VBIS_Brand.SelectedValue))
            {
                var curVehicleInspireList = _vehicleInspireList.Where(x => x.VBIS_Brand == mcbWhere_VBIS_Brand.SelectedValue).ToList();
                mcbWhere_VBIS_Inspire.DataSource = curVehicleInspireList;
            }

            //车型描述
            _vehicleModelDescList = CacheDAX.Get(CacheDAX.ConfigDataKey.VehicleModelDesc) as List<MDLBS_VehicleBrandInspireSumma>;
            mcbWhere_VBIS_ModelDesc.DisplayMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_ModelDesc;
            mcbWhere_VBIS_ModelDesc.ValueMember = SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_ModelDesc;
            if (_vehicleModelDescList != null
                && !string.IsNullOrEmpty(mcbWhere_VBIS_Brand.SelectedValue)
                && !string.IsNullOrEmpty(mcbWhere_VBIS_Inspire.SelectedValue))
            {
                var curVehicleInspireList = _vehicleModelDescList.Where(x => x.VBIS_Brand == mcbWhere_VBIS_Brand.SelectedValue && x.VBIS_Inspire == mcbWhere_VBIS_Inspire.SelectedValue).ToList();
                mcbWhere_VBIS_ModelDesc.DataSource = curVehicleInspireList;
            }

            //车辆类型：从码表获取
            cbWhere_VBIS_Model.DisplayMember = SysConst.EN_TEXT;
            cbWhere_VBIS_Model.ValueMember = SysConst.Value;
            cbWhere_VBIS_Model.DataSource = _vehicleModelList;
            cbWhere_VBIS_Model.DataBind();
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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            DetailDS = GridDS.FirstOrDefault(x => x.VBIS_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_ID].Value);
            if (DetailDS == null || string.IsNullOrEmpty(DetailDS.VBIS_ID))
            {
                return;
            }
            if (txtVBIS_ID.Text != DetailDS.VBIS_ID
                || (txtVBIS_ID.Text == DetailDS.VBIS_ID && txtVBIS_VersionNo.Text != DetailDS.VBIS_VersionNo?.ToString()))
            {
                if (txtVBIS_ID.Text == DetailDS.VBIS_ID && txtVBIS_VersionNo.Text != DetailDS.VBIS_VersionNo?.ToString())
                {
                    //数据版本已过期，将加载最新详情。
                    MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.DataHasOverdue }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else if (ViewHasChanged())
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
        }
       
        /// <summary>
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //品牌
            mcbVBIS_Brand.SelectedValue = DetailDS.VBIS_Brand;
            //品牌拼音首字母
            txtVBIS_BrandSpellCode.Text = DetailDS.VBIS_BrandSpellCode;
            //车系
            mcbVBIS_Inspire.SelectedValue = DetailDS.VBIS_Inspire;
            //车系拼音首字母
            txtVBIS_InspireSpellCode.Text = DetailDS.VBIS_InspireSpellCode;
            //车辆类型
            cbVBIS_Model.Text = DetailDS.VBIS_Model ?? string.Empty;
            //车型描述
            txtVBIS_ModelDesc.Text = DetailDS.VBIS_ModelDesc;
            //品牌车系ID
            txtVBIS_ID.Text = DetailDS.VBIS_ID;
            //有效
            if (DetailDS.VBIS_IsValid != null)
            {
                ckVBIS_IsValid.Checked = DetailDS.VBIS_IsValid.Value;
            }
            //创建人
            txtVBIS_CreatedBy.Text = DetailDS.VBIS_CreatedBy;
            //创建时间
            dtVBIS_CreatedTime.Value = DetailDS.VBIS_CreatedTime;
            //修改人
            txtVBIS_UpdatedBy.Text = DetailDS.VBIS_UpdatedBy;
            //修改时间
            dtVBIS_UpdatedTime.Value = DetailDS.VBIS_UpdatedTime;
            //版本号
            txtVBIS_VersionNo.Text = DetailDS.VBIS_VersionNo?.ToString() ?? string.Empty;
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
            //验证品牌
            if (string.IsNullOrEmpty(mcbVBIS_Brand.SelectedValue))
            {
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Name.VBIS_Brand }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                mcbVBIS_Brand.Focus();
                return false;
            }

            //验证车系
            if (string.IsNullOrEmpty(mcbVBIS_Inspire.SelectedValue))
            {
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Name.VBIS_Inspire }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                mcbVBIS_Inspire.Focus();
                return false;
            }

            //验证车型描述
            if (string.IsNullOrEmpty(txtVBIS_ModelDesc.Text.Trim()))
            {
                MessageBoxs.Show(Trans.BS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Name.VBIS_ModelDesc }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtVBIS_ModelDesc.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            DetailDS = new VehicleBrandInspireSummaManagerUIModel()
            {
                //品牌
                VBIS_Brand = mcbVBIS_Brand.SelectedValue,
                //品牌拼音首字母
                VBIS_BrandSpellCode = txtVBIS_BrandSpellCode.Text.Trim(),
                //车系
                VBIS_Inspire = mcbVBIS_Inspire.SelectedValue,
                //车系
                VBIS_InspireSpellCode = txtVBIS_InspireSpellCode.Text.Trim(),
                //车辆类型
                VBIS_Model = cbVBIS_Model.Text.Trim(),
                //车型描述
                VBIS_ModelDesc = txtVBIS_ModelDesc.Text.Trim(),
                //有效
                VBIS_IsValid = ckVBIS_IsValid.Checked,
                //创建人
                VBIS_CreatedBy = txtVBIS_CreatedBy.Text.Trim(),
                //创建时间
                VBIS_CreatedTime = (DateTime?)dtVBIS_CreatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //修改人
                VBIS_UpdatedBy = txtVBIS_UpdatedBy.Text.Trim(),
                //修改时间
                VBIS_UpdatedTime = (DateTime?)dtVBIS_UpdatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //入库单ID
                VBIS_ID = txtVBIS_ID.Text.Trim(),
                //版本号
                VBIS_VersionNo = Convert.ToInt64(txtVBIS_VersionNo.Text.Trim() == "" ? "1" : txtVBIS_VersionNo.Text.Trim()),
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
                    var removeList = GridDS.Where(x => x.IsChecked == true).ToList();
                    foreach (var loopRemove in removeList)
                    {
                        GridDS.Remove(loopRemove);
                    }
                }
                else
                {
                    var curHead = GridDS.FirstOrDefault(x => x.VBIS_ID == DetailDS.VBIS_ID);
                    if (curHead != null)
                    {
                        GridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = GridDS.FirstOrDefault(x => x.VBIS_ID == DetailDS.VBIS_ID);
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
