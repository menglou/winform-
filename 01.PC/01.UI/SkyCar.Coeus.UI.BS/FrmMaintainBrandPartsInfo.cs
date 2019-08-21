using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.BS.UIModel;
using SkyCar.Framework.WindowUI;

namespace SkyCar.Coeus.UI.BS
{
    /// <summary>
    /// 维护车辆品牌件信息
    /// </summary>
    public partial class FrmMaintainBrandPartsInfo : BaseForm
    {
        #region 全局变量

        /// <summary>
        /// 维护车辆品牌件信息BLL
        /// </summary>
        private BLLBase _bll = new BLLBase(Trans.BS);

        /// <summary>
        /// 页面参数字典
        /// </summary>
        private Dictionary<string, object> _viewParameters = new Dictionary<string, object>();

        /// <summary>
        /// 接收SourceView的Func
        /// </summary>
        private Func<VehicleThirdPartsInfoUIModel, bool> _maintainBrandPartsInfoFunc;

        /// <summary>
        /// 当前车辆品牌件信息
        /// </summary>
        VehicleThirdPartsInfoUIModel _brandPartsInfo = new VehicleThirdPartsInfoUIModel();

        /// <summary>
        /// 配件名称数据源
        /// </summary>
        List<MDLBS_AutoPartsName> _autoPartsNameList = new List<MDLBS_AutoPartsName>();

        #endregion

        #region 系统事件

        /// <summary>
        /// 维护车辆品牌件信息
        /// </summary>
        public FrmMaintainBrandPartsInfo()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 维护车辆品牌件信息
        /// </summary>
        /// <param name="paramWindowParameters">传入参数</param>
        public FrmMaintainBrandPartsInfo(Dictionary<string, object> paramWindowParameters)
        {
            _viewParameters = paramWindowParameters;

            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMaintainBrandPartsInfo_Load(object sender, EventArgs e)
        {
            //初始化控件
            InitializeDetailTabControls();

            #region 加载配件下拉列表           
            //配件名称
            _autoPartsNameList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsName) as List<MDLBS_AutoPartsName>;
            mcbVTPI_AutoPartsName.DisplayMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbVTPI_AutoPartsName.ValueMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbVTPI_AutoPartsName.DataSource = _autoPartsNameList;
            #endregion

            #region 处理参数

            if (_viewParameters != null)
            {
                //传入Model
                if (_viewParameters.ContainsKey(ComViewParamKey.DestModel.ToString()))
                {
                    _brandPartsInfo = _viewParameters[ComViewParamKey.DestModel.ToString()] as VehicleThirdPartsInfoUIModel;
                    if (_brandPartsInfo != null)
                    {
                        //第三方编码
                        txtVBPI_ThirdNo.Text = _brandPartsInfo.VTPI_ThirdNo;
                        //配件名称
                        mcbVTPI_AutoPartsName.SelectedValue = _brandPartsInfo.VTPI_AutoPartsName;
                        //配件品牌
                        txtVBPI_AutoPartsBrand.Text = _brandPartsInfo.VTPI_AutoPartsBrand;
                        //备注
                        txtVBPI_Remark.Text = _brandPartsInfo.VTPI_Remark;
                    }
                }

                //MaintainBrandPartsInfoFunc
                if (_viewParameters.ContainsKey(BSViewParamKey.MaintainBrandPartsInfo.ToString()))
                {
                    var tempFunc = _viewParameters[BSViewParamKey.MaintainBrandPartsInfo.ToString()] as Func<VehicleThirdPartsInfoUIModel, bool>;
                    if (tempFunc != null)
                    {
                        _maintainBrandPartsInfoFunc = tempFunc;
                    }
                    else
                    {
                        MessageBoxs.Show(Trans.BS, this.ToString(), "请传入Func<VehicleThirdPartsInfoUIModel, bool>，以便处理原厂件信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            //验证
            if (!CheckForAdd())
            {
                return;
            }

            bool addResult = false;
            if (_maintainBrandPartsInfoFunc != null)
            {
                addResult = _maintainBrandPartsInfoFunc(_brandPartsInfo);
            }
            if (addResult)
            {
                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// 确定并继续按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirmAndContinue_Click(object sender, EventArgs e)
        {
            //验证
            if (!CheckForAdd())
            {
                return;
            }
            bool addResult = false;
            if (_maintainBrandPartsInfoFunc != null)
            {
                addResult = _maintainBrandPartsInfoFunc(_brandPartsInfo);
            }
            if (addResult)
            {
                //初始化控件
                InitializeDetailTabControls();
                _brandPartsInfo.VTPI_ThirdNo = txtVBPI_ThirdNo.Text.Trim();
                _brandPartsInfo.VTPI_AutoPartsName = mcbVTPI_AutoPartsName.SelectedValue;
                _brandPartsInfo.VTPI_AutoPartsBrand = txtVBPI_AutoPartsBrand.Text.Trim();
                _brandPartsInfo.VTPI_Remark = txtVBPI_Remark.Text.Trim();
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

        #region 自定义方法

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            //第三方编码
            txtVBPI_ThirdNo.Clear();
            //配件名称
            mcbVTPI_AutoPartsName.Clear();
            //配件品牌
            txtVBPI_AutoPartsBrand.Clear();
            //备注
            txtVBPI_Remark.Clear();
        }

        /// <summary>
        /// 添加前检查
        /// </summary>
        private bool CheckForAdd()
        {
            //第三方编码
            string thirdNo = txtVBPI_ThirdNo.Text.Trim();
            //配件名称
            string autoPartsName = mcbVTPI_AutoPartsName.SelectedValue;
            //配件品牌
            string autoPartsBrand = txtVBPI_AutoPartsBrand.Text.Trim();
            if (string.IsNullOrEmpty(thirdNo))
            {
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, "请输入第三方编码"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (string.IsNullOrEmpty(autoPartsName))
            {
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, "请选择配件名称"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (string.IsNullOrEmpty(autoPartsBrand))
            {
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, "请选择配件品牌"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            _brandPartsInfo.VTPI_ThirdNo = thirdNo;
            _brandPartsInfo.VTPI_AutoPartsName = autoPartsName;
            _brandPartsInfo.VTPI_AutoPartsBrand = autoPartsBrand;
            _brandPartsInfo.VTPI_Remark = txtVBPI_Remark.Text.Trim();
            return true;
        }

        #endregion

    }
}
