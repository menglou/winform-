using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.PIS;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.Common.UIModel;
using SkyCar.Framework.WindowUI;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.PIS;

namespace SkyCar.Coeus.UI.PIS
{
    /// <summary>
    /// 业务单确认付款弹出窗
    /// </summary>
    public partial class FrmPurchaseOrderToPayConfirmWindow : BaseForm
    {
        #region 全局变量

        /// <summary>
        /// 业务单确认付款弹出窗BLL
        /// </summary>
        private PurchaseOrderToPayConfirmWindowBLL _bll = new PurchaseOrderToPayConfirmWindowBLL();

        /// <summary>
        /// 页面参数字典
        /// </summary>
        private Dictionary<string, object> _viewParameters = new Dictionary<string, object>();

        /// <summary>
        /// Grid的数据源
        /// </summary>
        List<PurchaseOrderToPayConfirmWindowModel> ListGridDS = new List<PurchaseOrderToPayConfirmWindowModel>();

        /// <summary>
        /// 获取该客户所有付款方式最后的付款账户
        /// </summary>
        List<MDLFM_PayBill> _payTypeAndAccountList = new List<MDLFM_PayBill>();

        /// <summary>
        /// 获取采购单下对应的付款单
        /// </summary>
        List<MDLFM_AccountPayableBill> _accountPayableList = new List<MDLFM_AccountPayableBill>();

        /// <summary>
        /// 获取页面控件的值
        /// </summary>
        MDLFM_PayBill _payBillManager = new MDLFM_PayBill();

        #region 下拉框数据源

        /// <summary>
        /// 收款对象类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _payObjectTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 付款方式
        /// </summary>
        List<ComComboBoxDataSourceTC> _payTypeList = new List<ComComboBoxDataSourceTC>();
        #endregion

        #endregion

        #region 系统事件

        /// <summary>
        /// 业务单确认付款弹出窗
        /// </summary>
        public FrmPurchaseOrderToPayConfirmWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 业务单确认付款弹出窗
        /// </summary>
        /// <param name="paramWindowParameters"></param>
        public FrmPurchaseOrderToPayConfirmWindow(Dictionary<string, object> paramWindowParameters)
        {
            _viewParameters = paramWindowParameters;

            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmPurchaseOrderToPayConfirmWindow_Load(object sender, EventArgs e)
        {
            #region 初始化下拉框

            #region 收款对象类型

            _payObjectTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.AmountTransObjectType);
            cbReceivableObjectTypeName.DisplayMember = SysConst.EN_TEXT;
            cbReceivableObjectTypeName.ValueMember = SysConst.EN_Code;
            cbReceivableObjectTypeName.DataSource = _payObjectTypeList;
            cbReceivableObjectTypeName.DataBind();

            #endregion

            #region 付款方式
            List<ComComboBoxDataSourceTC> payTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.TradeType);
            _payTypeList = payTypeList.Where(x => x.Text != TradeTypeEnum.Name.WALLET).ToList();
            cbPayTypeName.DisplayMember = SysConst.EN_TEXT;
            cbPayTypeName.ValueMember = SysConst.EN_Code;
            cbPayTypeName.DataSource = _payTypeList;
            cbPayTypeName.DataBind();
            //默认[现金]
            cbPayTypeName.Text = TradeTypeEnum.Name.CASH;
            cbPayTypeName.Value = TradeTypeEnum.Code.CASH;

            #endregion

            #endregion

            #region 处理参数

            if (_viewParameters != null)
            {
                #region 业务单确认付款

                //业务单确认付款
                if (_viewParameters.ContainsKey(ComViewParamKey.BusinessPaymentConfirm.ToString()))
                {
                    ListGridDS = _viewParameters[ComViewParamKey.BusinessPaymentConfirm.ToString()] as List<PurchaseOrderToPayConfirmWindowModel> ?? new List<PurchaseOrderToPayConfirmWindowModel>();

                    if (ListGridDS.Count > 0)
                    {
                        //组织
                        txtBusinessOrgID.Text = ListGridDS[0].BusinessOrgID;
                        txtBusinessOrgName.Text = ListGridDS[0].BusinessOrgName;

                        //来源类型
                        txtBusinessSourceTypeName.Text = ListGridDS[0].BusinessSourceTypeName;
                        txtBusinessSourceTypeCode.Text = ListGridDS[0].BusinessSourceTypeCode;
                        if (string.IsNullOrEmpty(ListGridDS[0].ReceiveObjectTypeName))
                        {
                            cbReceivableObjectTypeName.Enabled = true;
                        }
                        else
                        {
                            cbReceivableObjectTypeName.Enabled = false;
                        }
                        //收款对象类型名称
                        cbReceivableObjectTypeName.Text = ListGridDS[0].ReceiveObjectTypeName;
                        cbReceivableObjectTypeName.Value = ListGridDS[0].ReceiveObjectTypeCode;
                        if (string.IsNullOrEmpty(ListGridDS[0].ReceiveObjectTypeName))
                        {
                            txtRecObjectName.Enabled = true;
                        }
                        else
                        {
                            txtRecObjectName.Enabled = false;
                        }

                        //收款对象
                        txtRecObjectID.Text = ListGridDS[0].ReceiveObjectID;
                        txtRecObjectName.Text = ListGridDS[0].ReceiveObjectName;

                        //Grid绑定
                        gdGrid.DataSource = ListGridDS;
                        gdGrid.DataBind();

                        //设置单元格是否可以编辑
                        SetPayConfirmWindowStyle();

                        #region 计算出grid中应付金额，已付金额，未付金额

                        decimal accountPayableAmount = 0;
                        decimal paidAmount = 0;
                        decimal unpaidAmount = 0;
                        foreach (var loopGrid in ListGridDS)
                        {
                            accountPayableAmount = accountPayableAmount + (loopGrid.PayableTotalAmount ?? 0);
                            paidAmount = paidAmount + (loopGrid.PayTotalAmount ?? 0);
                            unpaidAmount = unpaidAmount + (loopGrid.UnPayTotalAmount ?? 0);
                        }
                        txtAPB_AccountPayableAmount.Text = Convert.ToString(accountPayableAmount);
                        txtAPB_PaidAmount.Text = Convert.ToString(paidAmount);
                        txtAPB_UnpaidAmount.Text = Convert.ToString(unpaidAmount);
                        #endregion


                        #region 获取该采购的所有的应付单
                        StringBuilder accountPayableBillIDs = new StringBuilder();
                        accountPayableBillIDs.Append(SysConst.Semicolon_DBC);
                        foreach (var loopGrid in ListGridDS)
                        {
                            accountPayableBillIDs.Append(loopGrid.APB_SourceBillNo + SysConst.Semicolon_DBC);
                        }
                        //获取该采购的所有的应付单
                        _bll.QueryForList(SQLID.PIS_PurchaseOrderToPayConfirmWindow_SQL_01, new MDLFM_AccountPayableBill()
                        {
                            WHERE_APB_SourceBillNo = accountPayableBillIDs.ToString(),
                        }, _accountPayableList);
                        #endregion

                    }
                }

                #endregion
            }

            #endregion
        }

        /// <summary>
        /// 收款对象类型ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbReceivableObjectTypeName_ValueChanged(object sender, EventArgs e)
        {
            txtRecObjectName.Clear();
            txtRecObjectID.Clear();
            if (cbReceivableObjectTypeName.Text == AmountTransObjectTypeEnum.Name.OTHERS)
            {
                txtRecObjectName.ReadOnly = false;
            }
            else
            {
                txtRecObjectName.ReadOnly = true;
            }
        }

        /// <summary>
        /// 收款对象EditorButton事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPayObjectName_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(cbReceivableObjectTypeName.Text))
            {
                //请先选择收款对象类型，再选择收款对象
                MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0034, new object[]
                    { MsgParam.PAYOBJECT_TYPE, SystemTableColumnEnums.FM_PayBill.Name.PB_RecObjectName }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            switch (cbReceivableObjectTypeName.Text)
            {
                case AmountTransObjectTypeEnum.Name.ORGANIZATION:
                    #region [收款对象类型]为{组织}的场合

                    //查询组织
                    FrmOrgQuery frmOrgQuery = new FrmOrgQuery();
                    if (frmOrgQuery.ShowDialog() == DialogResult.OK)
                    {
                        if (frmOrgQuery.SelectedGridList != null && frmOrgQuery.SelectedGridList.Count == 1)
                        {
                            //收款对象ID
                            txtRecObjectID.Text = frmOrgQuery.SelectedGridList[0].Org_ID;
                            //收款对象
                            txtRecObjectName.Text = frmOrgQuery.SelectedGridList[0].Org_ShortName;
                        }
                    }
                    #endregion
                    break;
                case AmountTransObjectTypeEnum.Name.AUTOPARTSSUPPLIER:
                    #region [收款对象类型]为{供应商}的场合

                    //查询供应商
                    FrmSupplierQuery frmSupplierQuery = new FrmSupplierQuery(SystemTableColumnEnums.PIS_Supplier.Code.SUPP_ID,
                        SystemTableColumnEnums.PIS_Supplier.Code.SUPP_Name);
                    if (frmSupplierQuery.ShowDialog() == DialogResult.OK)
                    {
                        //收款对象
                        txtRecObjectName.Text = frmSupplierQuery.SelectedText;
                        //收款对象ID
                        txtRecObjectID.Text = frmSupplierQuery.SelectedValue;
                    }
                    #endregion
                    break;
                case AmountTransObjectTypeEnum.Name.GENERALAUTOFACTORY:
                    #region [收款对象类型]为{一般汽修商户}的场合

                    //查询一般汽修商客户
                    Dictionary<string, object> generalAutoFactoryQueryParameters = new Dictionary<string, object>
                    {
                        {ComViewParamKey.CustomerType.ToString(), CustomerTypeEnum.Name.YBQXSH},
                    };
                    FrmCustomerQuery frmGeneralAutoFactoryQuery = new FrmCustomerQuery(generalAutoFactoryQueryParameters);
                    if (frmGeneralAutoFactoryQuery.ShowDialog() == DialogResult.OK)
                    {
                        List<CustomerQueryUIModel> selectedGridList = frmGeneralAutoFactoryQuery.SelectedGridList;
                        if (selectedGridList != null && selectedGridList.Count == 1)
                        {
                            //收款对象ID
                            txtRecObjectID.Text = selectedGridList[0].CustomerID;
                            //收款对象
                            txtRecObjectName.Text = selectedGridList[0].CustomerName;
                        }
                    }
                    #endregion
                    break;
                case AmountTransObjectTypeEnum.Name.PLATFORMAUTOFACTORY:
                    #region [收款对象类型]为{平台内汽修商}的场合

                    //查询平台内汽修商
                    Dictionary<string, object> platformAutoFactoryParameters = new Dictionary<string, object>
                    {
                        {ComViewParamKey.CustomerType.ToString(), CustomerTypeEnum.Name.PTNQXSH},
                    };
                    FrmCustomerQuery frmPlatformAutoFactoryQuery = new FrmCustomerQuery(platformAutoFactoryParameters);
                    if (frmPlatformAutoFactoryQuery.ShowDialog() == DialogResult.OK)
                    {
                        List<CustomerQueryUIModel> selectedGridList = frmPlatformAutoFactoryQuery.SelectedGridList;
                        if (selectedGridList != null && selectedGridList.Count == 1)
                        {
                            //收款对象ID
                            txtRecObjectID.Text = selectedGridList[0].CustomerID;
                            //收款对象
                            txtRecObjectName.Text = selectedGridList[0].CustomerName;
                        }
                    }
                    #endregion
                    break;
                case AmountTransObjectTypeEnum.Name.REGULARCUSTOMER:
                    #region [收款对象类型]为{普通客户}的场合

                    //查询普通客户
                    Dictionary<string, object> regularCustomerParameters = new Dictionary<string, object>
                    {
                        {ComViewParamKey.CustomerType.ToString(), CustomerTypeEnum.Name.PTKH},
                    };
                    FrmCustomerQuery frmRegularCustomerQuery = new FrmCustomerQuery(regularCustomerParameters);
                    if (frmRegularCustomerQuery.ShowDialog() == DialogResult.OK)
                    {
                        List<CustomerQueryUIModel> selectedGridList = frmRegularCustomerQuery.SelectedGridList;
                        if (selectedGridList != null && selectedGridList.Count == 1)
                        {
                            //收款对象ID
                            txtRecObjectID.Text = selectedGridList[0].CustomerID;
                            //收款对象
                            txtRecObjectName.Text = selectedGridList[0].CustomerName;
                        }
                    }
                    #endregion
                    break;
                case AmountTransObjectTypeEnum.Name.DELIVERYMAN:
                    #region [收款对象类型]为{配送人员}的场合

                    //查询物流单
                    FrmLogisticsBillQuery logisticsBillQuery = new FrmLogisticsBillQuery()
                    {
                        StartPosition = FormStartPosition.CenterScreen
                    };
                    if (logisticsBillQuery.ShowDialog() == DialogResult.OK)
                    {
                        if (logisticsBillQuery.SelectedGridList != null && logisticsBillQuery.SelectedGridList.Count == 1)
                        {
                            //收款对象ID
                            txtRecObjectID.Text = logisticsBillQuery.SelectedGridList[0].LB_DeliveryByID;
                            //收款对象
                            txtRecObjectName.Text = logisticsBillQuery.SelectedGridList[0].LB_DeliveryBy;
                        }
                    }
                    #endregion
                    break;
                case AmountTransObjectTypeEnum.Name.OTHERS:
                    #region [收款对象类型]为{其他}的场合
                    //手动输入

                    #endregion
                    break;
            }
        }

        #region gdGrid相关事件

        /// <summary>
        /// gdGrid单元格的值变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_CellChange(object sender, CellEventArgs e)
        {
            //验证本次付款金额格式
            if (e.Cell.Column.Key == "ThisPayAmount")
            {
                decimal thisPayAmount = 0;
                gdGrid.UpdateData();
                foreach (var loopGrid in ListGridDS)
                {
                    thisPayAmount = thisPayAmount + (loopGrid.ThisPayAmount ?? 0);
                }
                numThisPayAmount.Text = Convert.ToString(thisPayAmount);
            }
        }

        #endregion

        #region 按钮事件

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            //前端检查-保存
            if (!ClientCheckForSave())
            {
                return;
            }

            foreach (var loopItem in ListGridDS)
            {
                loopItem.BusinessOrgID = txtBusinessOrgID.Text;
                loopItem.BusinessOrgName = txtBusinessOrgName.Text;
                loopItem.ReceiveObjectTypeName = cbReceivableObjectTypeName.Text;
                loopItem.ReceiveObjectTypeCode = cbReceivableObjectTypeName.Value?.ToString();
                loopItem.ReceiveObjectID = txtRecObjectID.Text;
                loopItem.ReceiveObjectName = txtRecObjectName.Text;
            }
            //将页面内控件的值赋值给基类的_payBillManager
            SetCardCtrlsToDetailDS();
            if (!_bll.SavePurchaseOrderToPayConfirmData(_payBillManager, ListGridDS, _accountPayableList))
            {
                //保存失败
                MessageBoxs.Show(Trans.PIS, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.PIS, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
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

        #region 文本框事件

        /// <summary>
        /// 本次付款总金额_KeyUp事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numThisPayAmount_KeyUp(object sender, KeyEventArgs e)
        {
            #region 改变本次付款金额

            #region 清除上次[本次付款金额]([未付金额]为负数,不清楚)

            //负向的未付金额
            decimal minusUnPayTotalAmount = 0;
            foreach (var loopGridDS in ListGridDS)
            {

                if (loopGridDS.UnPayTotalAmount > 0)
                {
                    loopGridDS.ThisPayAmount = 0;
                }
                else
                {
                    //计算出负向的未付金额
                    minusUnPayTotalAmount = minusUnPayTotalAmount - (loopGridDS.UnPayTotalAmount ?? 0);
                }
            }

            #endregion

            #region 本次付款总金额为空时，去抵消正负应付金额

            if (string.IsNullOrEmpty(numThisPayAmount.Text))
            {
                for (int i = 0; i < ListGridDS.Count; i++)
                {
                    //如果[未付金额]小于零，跳到下一次循环
                    if (ListGridDS[i].UnPayTotalAmount < 0)
                    {
                        continue;
                    }
                    //如果[负向应付金额]之和大于[未付金额]
                    if (minusUnPayTotalAmount > (ListGridDS[i].UnPayTotalAmount ?? 0))
                    {
                        ListGridDS[i].ThisPayAmount = ListGridDS[i].UnPayTotalAmount ?? 0;
                        minusUnPayTotalAmount = minusUnPayTotalAmount - (ListGridDS[i].UnPayTotalAmount ?? 0);
                    }
                    //如果[负向应付金额]之和小于或等于[未付金额]
                    else if (minusUnPayTotalAmount <= (ListGridDS[i].UnPayTotalAmount ?? 0))
                    {
                        ListGridDS[i].ThisPayAmount = minusUnPayTotalAmount;
                        minusUnPayTotalAmount = 0;
                        break;
                    }
                    //如果如果[负向应付金额]之和大于并且是最后一次循环
                    if (minusUnPayTotalAmount > 0 && i == (ListGridDS.Count - 1))
                    {
                        ListGridDS[0].ThisPayAmount = ListGridDS[0].ThisPayAmount + minusUnPayTotalAmount;
                        minusUnPayTotalAmount = 0;
                    }
                }
                gdGrid.DataSource = ListGridDS;
                gdGrid.DataBind();
                return;
            }

            #endregion

            #region 本次付款总金额不为空时，负向应付金额总和加上本次付款总金额，然后在去平均分配

            decimal thisPayAmount = Convert.ToDecimal(numThisPayAmount.Value ?? 0) + minusUnPayTotalAmount;
            for (int i = 0; i < ListGridDS.Count; i++)
            {
                //如果[未付金额]小于零，跳到下一次循环
                if (ListGridDS[i].UnPayTotalAmount < 0)
                {
                    continue;
                }
                //如果[本次付款总金额]之和大于[未付金额]
                if (thisPayAmount > (ListGridDS[i].UnPayTotalAmount ?? 0))
                {
                    ListGridDS[i].ThisPayAmount = ListGridDS[i].UnPayTotalAmount ?? 0;
                    thisPayAmount = thisPayAmount - (ListGridDS[i].UnPayTotalAmount ?? 0);
                }
                //如果[本次付款总金额]之和小于或等于[未付金额]
                else if (thisPayAmount <= (ListGridDS[i].UnPayTotalAmount ?? 0))
                {
                    ListGridDS[i].ThisPayAmount = thisPayAmount;
                    thisPayAmount = 0;
                    break;
                }
                //如果如果[本次付款总金额]之和大于并且是最后一次循环
                if (thisPayAmount > 0 && i == (ListGridDS.Count - 1))
                {
                    ListGridDS[0].ThisPayAmount = ListGridDS[0].ThisPayAmount + thisPayAmount;
                    thisPayAmount = 0;
                }
            }

            #endregion

            gdGrid.DataSource = ListGridDS;
            gdGrid.DataBind();

            #endregion
        }
        #endregion

        #endregion

        #region 自定义方法

        /// <summary>
        /// 将页面内控件的值赋值给基类的_payBillManager
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            _payBillManager = new MDLFM_PayBill()
            {
                PB_PayTypeName = cbPayTypeName.Text,
                PB_PayTypeCode = cbPayTypeName.Value?.ToString() ?? "",
                PB_Remark = txtRemark.Text,
                PB_CertificateNo = txtPayCertificateNo.Text,
                PB_PayableTotalAmount = Convert.ToDecimal(txtAPB_UnpaidAmount.Text.Trim() == "" ? "0" : txtAPB_UnpaidAmount.Text.Trim()),
                PB_RealPayableTotalAmount = Convert.ToDecimal(numThisPayAmount.Value ?? 0)
            };
        }
        /// <summary>
        /// 前端检查-保存
        /// </summary>
        private bool ClientCheckForSave()
        {
            #region 验证

            if (string.IsNullOrEmpty(numThisPayAmount.Text.Trim()))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { MsgParam.THIS_PAYAMOUNT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                numThisPayAmount.Focus();
                return false;
            }
            decimal payAmount = Convert.ToDecimal(numThisPayAmount.Value ?? 0);
            decimal unPayAmount = Convert.ToDecimal(txtAPB_UnpaidAmount.Text.Trim() == "" ? "0" : txtAPB_UnpaidAmount.Text.Trim());
            if (payAmount > unPayAmount)
            {
                //本次付款金额大于未付金额，是否确认支付？\r\n单击【确定】支付单据，【取消】返回。
                DialogResult isPay = MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0037, new object[] { MsgParam.THIS_PAYAMOUNT, SystemTableColumnEnums.FM_AccountPayableBill.Name.APB_UnpaidAmount, MsgParam.PAY }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (isPay != DialogResult.OK)
                {
                    return false;
                }
            }
            #endregion

            return true;
        }
        /// <summary>
        /// 设置单元格是否可以编辑
        /// </summary>
        private void SetPayConfirmWindowStyle()
        {
            #region 设置Grid数据颜色
            gdGrid.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;

            //设置负向的应付单不可编辑
            for (int i = 0; i < ListGridDS.Count; i++)
            {
                if ((ListGridDS[i].UnPayTotalAmount ?? 0) < 0)
                {
                    //设置[本次付款金额]单元格不可编辑
                    gdGrid.DisplayLayout.Rows[i].Cells["ThisPayAmount"].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    gdGrid.DisplayLayout.Rows[i].Cells["ThisPayAmount"].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    gdGrid.DisplayLayout.Rows[i].Cells["ThisPayAmount"].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;
                    ListGridDS[i].ThisPayAmount = ListGridDS[i].UnPayTotalAmount ?? 0;
                }
                else
                {
                    //设置[本次付款金额]单元格不可编辑
                    gdGrid.DisplayLayout.Rows[i].Cells["ThisPayAmount"].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    gdGrid.DisplayLayout.Rows[i].Cells["ThisPayAmount"].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    gdGrid.DisplayLayout.Rows[i].Cells["ThisPayAmount"].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;
                }
            }

            #endregion
        }
        #endregion

    }
}
