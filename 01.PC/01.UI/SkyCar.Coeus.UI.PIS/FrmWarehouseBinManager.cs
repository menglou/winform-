using System;
using System.Windows.Forms;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.PIS;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UIModel.PIS;
using System.Collections.Generic;

namespace SkyCar.Coeus.UI.PIS
{
    /// <summary>
    /// 仓位管理
    /// </summary>
    public partial class FrmWarehouseBinManager : BaseFormCardList<WarehouseBinManagerUIModel, WarehouseBinManagerQCModel, MDLPIS_Warehouse>
    {
        #region 全局变量
        /// <summary>
        /// 仓位管理BLL
        /// </summary>
        private WarehouseBinManagerBLL _bll = new WarehouseBinManagerBLL();
        /// <summary>
        /// 页面参数字典
        /// </summary>
        private Dictionary<string, object> _viewParameters = new Dictionary<string, object>();
        #endregion

        #region 公共属性

        public WarehouseBinManagerUIModel warehouseBinManagerUIModel = new WarehouseBinManagerUIModel();

        #endregion

        #region 系统事件
        /// <summary>
        /// 构造方法
        /// </summary>
        public FrmWarehouseBinManager()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 领料查询
        /// </summary>
        /// <param name="paramWindowParameters"></param>
        public FrmWarehouseBinManager(Dictionary<string, object> paramWindowParameters)
        {
            _viewParameters = paramWindowParameters;
            InitializeComponent();
        }
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmWarehouseBinManager_Load(object sender, EventArgs e)
        {
            GetGridDataValue();
        }
        /// <summary>
        /// 【保存】点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            warehouseBinManagerUIModel.WHB_Name = txtWHB_Name.Text.Trim();
            warehouseBinManagerUIModel.WHB_Description = txtWHB_Description.Text.Trim();
            warehouseBinManagerUIModel.WHB_IsValid = ckWHB_IsValid.Checked;
            warehouseBinManagerUIModel.Tmp_SID_ID = txtTmp_SID_ID.Value == null ? System.Guid.NewGuid().ToString() : txtTmp_SID_ID.Text.Trim();
            warehouseBinManagerUIModel.WHB_VersionNo = txtWHB_VersionNo.Value == null ? 1 : Convert.ToInt64(txtWHB_VersionNo.Text.Trim());

            warehouseBinManagerUIModel.WHB_CreatedBy = txtWHB_CreatedBy.Value == null ? LoginInfoDAX.UserName : txtWHB_CreatedBy.Text.Trim();
            warehouseBinManagerUIModel.WHB_CreatedTime = dtWHB_CreatedTime.Value == null ? DateTime.Now : Convert.ToDateTime(dtWHB_CreatedTime.Value.ToString());
            warehouseBinManagerUIModel.WHB_UpdatedBy = txtWHB_UpdatedBy.Value == null ? LoginInfoDAX.UserName : txtWHB_UpdatedBy.Text.Trim();
            warehouseBinManagerUIModel.WHB_UpdatedTime = dtWHB_UpdatedTime.Value == null ? DateTime.Now : Convert.ToDateTime(dtWHB_UpdatedTime.Value.ToString());
            this.DialogResult = DialogResult.OK;
        }
        /// <summary>
        /// 【取消】点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            FrmWarehouseManager warehouseManager = new FrmWarehouseManager();
            warehouseManager.Activate();
            this.Close();
        }
        #endregion

        #region 重写基类方法

        #endregion

        #region 自定义方法
        /// <summary>
        /// 接受【仓位管理】页面传来的属性
        /// </summary>
        private void GetGridDataValue()
        {
            txtWHB_Name.Text = warehouseBinManagerUIModel.WHB_Name;
            txtWHB_Description.Text = warehouseBinManagerUIModel.WHB_Description;

            if (warehouseBinManagerUIModel.WHB_IsValid != null)
            {
                ckWHB_IsValid.Checked = (bool)warehouseBinManagerUIModel.WHB_IsValid;
            }
            txtWHB_WH_ID.Text = warehouseBinManagerUIModel.WHB_WH_ID;
            txtWHB_ID.Text = warehouseBinManagerUIModel.WHB_ID;
            txtWHB_VersionNo.Text = Convert.ToString(warehouseBinManagerUIModel.WHB_VersionNo);
            txtTmp_SID_ID.Text = warehouseBinManagerUIModel.Tmp_SID_ID;
            txtWHB_CreatedBy.Text = warehouseBinManagerUIModel.WHB_CreatedBy;
            dtWHB_CreatedTime.Value = warehouseBinManagerUIModel.WHB_CreatedTime;
            txtWHB_UpdatedBy.Text = warehouseBinManagerUIModel.WHB_UpdatedBy;
            dtWHB_UpdatedTime.Value = warehouseBinManagerUIModel.WHB_UpdatedTime;

            //已保存的仓位名称不可编辑
            if (!string.IsNullOrEmpty(txtWHB_ID.Text))
            {
                txtWHB_Name.Enabled = false;
            }
            else
            {
                txtWHB_Name.Enabled = true;
            }
        }
        #endregion
    }
}
