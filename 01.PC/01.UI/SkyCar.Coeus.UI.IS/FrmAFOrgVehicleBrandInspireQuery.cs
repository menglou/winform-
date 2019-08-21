using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.IS;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.UIModel.IS.QCModel;
using SkyCar.Coeus.UIModel.Common.UIModel;
using SkyCar.Coeus.UIModel.IS.UIModel;

namespace SkyCar.Coeus.UI.IS
{
    /// <summary>
    /// 汽修组织车辆品牌车系查询
    /// </summary>
    public partial class FrmAFOrgVehicleBrandInspireQuery : BaseFormCardList<AFOrgVehicleBrandInspireQueryUIModel, AFOrgVehicleBrandInspireQueryQCModel, AFOrgVehicleBrandInspireQueryUIModel>
    {
        #region 全局变量

        /// <summary>
        /// 汽修组织车辆品牌车系查询BLL
        /// </summary>
        private AFOrgVehicleBrandInspireQueryBLL _bll = new AFOrgVehicleBrandInspireQueryBLL();

        #region 下拉框数据源

        /// <summary>
        /// 汽修商户
        /// </summary>
        List<MDLPIS_AutoFactoryCustomer> _autoFactoryList = new List<MDLPIS_AutoFactoryCustomer>();
        /// <summary>
        /// 汽修商户组织
        /// </summary>
        List<CustomerQueryUIModel> _autoFactoryOrgList = new List<CustomerQueryUIModel>();

        #endregion

        /// <summary>
        /// 汽修商户编码
        /// </summary>
        private string _arMerchantCode;

        /// <summary>
        /// 当前页合计
        /// </summary>
        string _sumCurPageDesc = "当前页合计：";

        /// <summary>
        /// 合计
        /// </summary>
        string _sumAllPageDesc = "合计：";

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmAFOrgVehicleBrandInspireQuery构造方法
        /// </summary>
        public FrmAFOrgVehicleBrandInspireQuery()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAFOrgVehicleBrandInspireQuery_Load(object sender, EventArgs e)
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

            //初始化
            InitializeListTabControls();
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

            PageSize = tmpPageSize;

            QueryAction();
        }

        /// <summary>
        /// 【查询条件】汽修商户改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbWhere_InvARMerchant_SelectedIndexChanged(object sender, EventArgs e)
        {
            mcbWhere_InvAROrgName.Clear();

            if (string.IsNullOrEmpty(mcbWhere_InvARMerchant.SelectedText)
                || string.IsNullOrEmpty(mcbWhere_InvARMerchant.SelectedValue))
            {
                mcbWhere_InvAROrgName.DataSource = null;
                return;
            }
            if (mcbWhere_InvARMerchant.SelectedValue != _arMerchantCode)
            {
                //获取汽修商户相关信息
                bool getARMerchantInfoResult = GetARMerchantInfo(mcbWhere_InvARMerchant.SelectedValue);
                if (!getARMerchantInfoResult)
                {
                    return;
                }
            }

            if (_autoFactoryOrgList != null)
            {
                var tempAutoFactoryOrgList =
                    _autoFactoryOrgList.Where(x => x.AutoFactoryCode == mcbWhere_InvARMerchant.SelectedValue).ToList();
                mcbWhere_InvAROrgName.DataSource = tempAutoFactoryOrgList;
            }
        }

        /// <summary>
        /// 【Grid】Grid的CellChange事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdInventory_CellChange(object sender, CellEventArgs e)
        {
            gdInventory.UpdateData();
        }

        #endregion

        #region 重写基类方法

        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            //汽修商户为必选项
            if (string.IsNullOrEmpty(mcbWhere_InvARMerchant.SelectedText)
                || string.IsNullOrEmpty(mcbWhere_InvARMerchant.SelectedValue))
            {
                MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.AUTOFACTORY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //判断汽修商的数据库链接字符串是否存在
            if (string.IsNullOrEmpty(_arMerchantCode) || !DBManager.CheckConnectin(_arMerchantCode))
            {
                MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.AUTOFACTORY + MsgParam.SERVER + MsgParam.CONNECTION }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ConditionDS = new AFOrgVehicleBrandInspireQueryQCModel
            {
                PageSize = PageSize,
                PageIndex = PageIndex,
                ARMerchantCode = mcbWhere_InvARMerchant.SelectedValue,
                ARMerchantName = mcbWhere_InvARMerchant.SelectedText,
                AROrgName = mcbWhere_InvAROrgName.SelectedText,
            };

            if (rbVehicleBrand.Checked)
            {
                //按品牌统计
                ConditionDS.StatisticsMode = "Brand";
            }
            else if (rbVehicleInspire.Checked)
            {
                //按车系统计
                ConditionDS.StatisticsMode = "Inspire";
            }
            GridDS = new BindingList<AFOrgVehicleBrandInspireQueryUIModel>();

            DBManager.QueryForList<AFOrgVehicleBrandInspireQueryUIModel>(_arMerchantCode, SQLID.IS_AFOrgVehicleBrandInspireQuery_SQL01, ConditionDS, GridDS);

            int totalVehicleCountOfCurPage = 0;
            int totalVehicleCountOfAllPage = 0;

            if (GridDS.Count > 0)
            {
                AFOrgVehicleBrandInspireQueryUIModel subObject = GridDS[0];
                TotalRecordCount = subObject.RecordCount ?? 0;
                totalVehicleCountOfAllPage = subObject.TotalVehicleCount;
            }
            else
            {
                TotalRecordCount = 0;
            }

            foreach (var loopItem in GridDS)
            {
                totalVehicleCountOfCurPage += loopItem.VC_Count;
            }
            AFOrgVehicleBrandInspireQueryUIModel curPageTotal = new AFOrgVehicleBrandInspireQueryUIModel
            {
                VC_Count = totalVehicleCountOfCurPage
            };
            GridDS.Add(curPageTotal);

            AFOrgVehicleBrandInspireQueryUIModel allPageTotal = new AFOrgVehicleBrandInspireQueryUIModel
            {
                VC_Count = totalVehicleCountOfAllPage
            };

            if (rbVehicleBrand.Checked)
            {
                //隐藏[车系]列
                gdInventory.DisplayLayout.Bands[0].Columns["VC_Inspire"].Hidden = true;
                curPageTotal.VC_Brand = _sumCurPageDesc;
                allPageTotal.VC_Brand = _sumAllPageDesc;
            }
            else if (rbVehicleInspire.Checked)
            {
                //显示[车系]列
                gdInventory.DisplayLayout.Bands[0].Columns["VC_Inspire"].Hidden = false;
                curPageTotal.VC_Inspire = _sumCurPageDesc;
                allPageTotal.VC_Inspire = _sumAllPageDesc;
            }
            GridDS.Add(allPageTotal);

            //设置翻页控件
            base.SetBarPaging(TotalRecordCount);

            //4.Grid绑定数据源
            gdInventory.DataSource = base.GridDS;
            gdInventory.DataBind();
            //5.设置Grid自适应列宽（根据单元格内容）
            gdInventory.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdInventory.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 清空
        /// </summary>
        public override void ClearAction()
        {
            lblWhere_InvAROrgName.Visible = true;
            mcbWhere_InvAROrgName.Visible = true;
            mcbWhere_InvARMerchant.Clear();
            mcbWhere_InvAROrgName.Clear();
            rbVehicleBrand.Checked = true;
            rbVehicleInspire.Checked = false;

            GridDS = new BindingList<AFOrgVehicleBrandInspireQueryUIModel>();
            gdInventory.DataSource = GridDS;
            gdInventory.DataBind();
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitializeListTabControls()
        {
            lblWhere_InvAROrgName.Visible = true;
            mcbWhere_InvAROrgName.Visible = true;
            mcbWhere_InvARMerchant.Clear();
            mcbWhere_InvAROrgName.Clear();
            rbVehicleBrand.Checked = true;
            rbVehicleInspire.Checked = false;

            GridDS = new BindingList<AFOrgVehicleBrandInspireQueryUIModel>();
            gdInventory.DataSource = GridDS;
            gdInventory.DataBind();

            #region 初始化下拉框

            //汽修商户名称
            _autoFactoryList = CacheDAX.Get(CacheDAX.ConfigDataKey.ARMerchant) as List<MDLPIS_AutoFactoryCustomer>;
            if (_autoFactoryList != null)
            {
                var autoFactoryCustomerList = _autoFactoryList.Where(x => x.AFC_IsPlatform != null && x.AFC_IsPlatform.Value == true).ToList();

                mcbWhere_InvARMerchant.DisplayMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Name;
                mcbWhere_InvARMerchant.ValueMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Code;
                mcbWhere_InvARMerchant.DataSource = autoFactoryCustomerList;
            }

            //汽修商户组织
            var allCustomerList = CacheDAX.Get(CacheDAX.ConfigDataKey.Customer) as List<CustomerQueryUIModel>;
            if (allCustomerList != null)
            {
                _autoFactoryOrgList = allCustomerList.Where(x => x.CustomerType == CustomerTypeEnum.Name.PTNQXSH).ToList();
            }
            mcbWhere_InvAROrgName.DisplayMember = "CustomerName";
            mcbWhere_InvAROrgName.ValueMember = "AutoFactoryOrgInfo";
            if (!string.IsNullOrEmpty(mcbWhere_InvARMerchant.SelectedValue))
            {
                var tempAutoFactoryOrgList =
                    _autoFactoryOrgList.Where(x => x.AutoFactoryCode == mcbWhere_InvARMerchant.SelectedValue);
                mcbWhere_InvAROrgName.DataSource = tempAutoFactoryOrgList;
            }
            #endregion
        }

        /// <summary>
        /// 获取汽修商户相关信息
        /// </summary>
        /// <param name="paramMerchantCode">汽修商客户编码</param>
        private bool GetARMerchantInfo(string paramMerchantCode)
        {
            if (string.IsNullOrEmpty(paramMerchantCode))
            {
                //汽修商客户ID为空，汽修商户信息获取失败
                MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0016, SystemTableColumnEnums.PIS_AutoFactoryCustomer.Name.AFC_ID, MsgParam.AUTOFACTORY + MsgParam.INFORMATION + MsgParam.GET), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //从缓存中获取汽修商户信息
            List<MDLPIS_AutoFactoryCustomer> resultAutoFactoryCustomerList = CacheDAX.Get(CacheDAX.ConfigDataKey.ARMerchant) as List<MDLPIS_AutoFactoryCustomer>;
            if (resultAutoFactoryCustomerList == null)
            {
                //汽修商户信息获取失败
                MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0002, MsgParam.AUTOFACTORY + MsgParam.INFORMATION + MsgParam.GET), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //获取该汽修商Code
            string tempAutoFactoryCode = string.Empty;
            List<MDLPIS_AutoFactoryCustomer> tempAutoFactoryCustomerList = resultAutoFactoryCustomerList.Where(p => p.AFC_Code == paramMerchantCode).ToList();
            if (tempAutoFactoryCustomerList.Count == 1)
            {
                tempAutoFactoryCode = tempAutoFactoryCustomerList[0].AFC_Code;
            }

            #region 获取商户数据库配置Key

            _arMerchantCode = BLLCom.GetMerchantDbConfigKey(tempAutoFactoryCode);

            if (string.IsNullOrEmpty(_arMerchantCode))
            {
                //请选择有效的汽修商户
                MessageBoxs.Show(Trans.IS, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, MsgParam.VALID + MsgParam.OF + MsgParam.AUTOFACTORY), MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return false;
            }

            #endregion

            return true;
        }

        #endregion
    }
}
