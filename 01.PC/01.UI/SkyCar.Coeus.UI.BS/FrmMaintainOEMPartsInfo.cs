using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.TBModel;
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
    /// 维护车辆原厂件信息
    /// </summary>
    public partial class FrmMaintainOEMPartsInfo : BaseForm
    {
        #region 全局变量

        /// <summary>
        /// 维护车辆原厂件信息BLL
        /// </summary>
        private BLLBase _bll = new BLLBase(Trans.BS);

        /// <summary>
        /// 页面参数字典
        /// </summary>
        private Dictionary<string, object> _viewParameters = new Dictionary<string, object>();

        /// <summary>
        /// 接收SourceView的Func
        /// </summary>
        private Func<VehicleOemPartsInfoUIModel, bool> _maintainOemPartsInfoFunc;

        /// <summary>
        /// 配件名称数据源
        /// </summary>
        List<MDLBS_AutoPartsName> _autoPartsNameList = new List<MDLBS_AutoPartsName>();
        
        /// <summary>
        /// 当前车辆原厂件信息
        /// </summary>
        VehicleOemPartsInfoUIModel _oemPartsInfo = new VehicleOemPartsInfoUIModel();

        #endregion

        #region 系统事件

        /// <summary>
        /// 维护车辆原厂件信息
        /// </summary>
        public FrmMaintainOEMPartsInfo()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 维护车辆原厂件信息
        /// </summary>
        /// <param name="paramWindowParameters">传入参数</param>
        public FrmMaintainOEMPartsInfo(Dictionary<string, object> paramWindowParameters)
        {
            _viewParameters = paramWindowParameters;

            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMaintainOEMPartsInfo_Load(object sender, EventArgs e)
        {
            //初始化控件
            InitializeDetailTabControls();

            #region 加载配件下拉列表           
            //配件名称
            _autoPartsNameList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsName) as List<MDLBS_AutoPartsName>;
            mcbVOPI_AutoPartsName.DisplayMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbVOPI_AutoPartsName.ValueMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbVOPI_AutoPartsName.DataSource = _autoPartsNameList;
            #endregion

            #region 处理参数

            if (_viewParameters != null)
            {
                //传入Model
                if (_viewParameters.ContainsKey(ComViewParamKey.DestModel.ToString()))
                {
                    _oemPartsInfo = _viewParameters[ComViewParamKey.DestModel.ToString()] as VehicleOemPartsInfoUIModel;
                    if (_oemPartsInfo != null)
                    {
                        //原厂编码
                        txtVOPI_OEMNo.Text = _oemPartsInfo.VOPI_OEMNo;
                        //配件名称
                        mcbVOPI_AutoPartsName.SelectedValue = _oemPartsInfo.VOPI_AutoPartsName;
                        //备注
                        txtVOPI_Remark.Text = _oemPartsInfo.VOPI_Remark;
                    }
                }
                
                //MaintainOemPartsInfoFunc
                if (_viewParameters.ContainsKey(BSViewParamKey.MaintainOemPartsInfo.ToString()))
                {
                    var tempFunc = _viewParameters[BSViewParamKey.MaintainOemPartsInfo.ToString()] as Func<VehicleOemPartsInfoUIModel, bool>;
                    if (tempFunc != null)
                    {
                        _maintainOemPartsInfoFunc = tempFunc;
                    }
                    else
                    {
                        MessageBoxs.Show(Trans.BS, this.ToString(), "请传入Func<VehicleOemPartsInfoUIModel, bool>，以便处理原厂件信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (_maintainOemPartsInfoFunc != null)
            {
                addResult = _maintainOemPartsInfoFunc(_oemPartsInfo);
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
            if (_maintainOemPartsInfoFunc != null)
            {
                addResult = _maintainOemPartsInfoFunc(_oemPartsInfo);
            }
            if (addResult)
            {
                //初始化控件
                InitializeDetailTabControls();
                _oemPartsInfo.VOPI_OEMNo = txtVOPI_OEMNo.Text.Trim();
                _oemPartsInfo.VOPI_AutoPartsName = mcbVOPI_AutoPartsName.SelectedValue;
                _oemPartsInfo.VOPI_Remark = txtVOPI_Remark.Text.Trim();
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
            //原厂编码
            txtVOPI_OEMNo.Clear();
            //配件名称
            mcbVOPI_AutoPartsName.Clear();
            //备注
            txtVOPI_Remark.Clear();
        }

        /// <summary>
        /// 添加前检查
        /// </summary>
        private bool CheckForAdd()
        {
            //原厂编码
            string oemNo = txtVOPI_OEMNo.Text.Trim();
            //配件名称
            string autoPartsName = mcbVOPI_AutoPartsName.SelectedValue;
            if (string.IsNullOrEmpty(oemNo))
            {
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, "请输入原厂编码"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (string.IsNullOrEmpty(autoPartsName))
            {
                MessageBoxs.Show(Trans.BS, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, "请选择配件名称"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            _oemPartsInfo.VOPI_OEMNo = oemNo;
            _oemPartsInfo.VOPI_AutoPartsName = autoPartsName;
            _oemPartsInfo.VOPI_Remark = txtVOPI_Remark.Text.Trim();
            return true;
        }

        #endregion

    }
}
