using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.UIModel.Common.UIModel;
using SkyCar.Framework.WindowUI;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.TBModel;

namespace SkyCar.Coeus.UI.Common
{
    /// <summary>
    /// 业务单确认付款弹出窗
    /// </summary>
    public partial class FrmBusinessPayConfirmWindow : BaseForm
    {
        #region 全局变量

        /// <summary>
        /// 付款单
        /// </summary>
        private MDLFM_PayBill _payBillManager = new MDLFM_PayBill();

        /// <summary>
        /// 业务单确认付款弹出窗BLL
        /// </summary>
        private BusinessPayConfirmWindowBLL _bll = new BusinessPayConfirmWindowBLL();

        /// <summary>
        /// 页面参数字典
        /// </summary>
        private Dictionary<string, object> _viewParameters = new Dictionary<string, object>();

        /// <summary>
        /// Grid的数据源
        /// </summary>
        List<BusinessPayConfirmWindowModel> ListGridDS = new List<BusinessPayConfirmWindowModel>();

        #region 下拉框数据源

        /// <summary>
        /// 收款对象类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _receivableTypeList = new List<ComComboBoxDataSourceTC>();
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
        public FrmBusinessPayConfirmWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 业务单确认付款弹出窗
        /// </summary>
        /// <param name="paramWindowParameters"></param>
        public FrmBusinessPayConfirmWindow(Dictionary<string, object> paramWindowParameters)
        {
            _viewParameters = paramWindowParameters;

            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmBusinessPayConfirmWindow_Load(object sender, EventArgs e)
        {
            #region 收款对象类型

            _receivableTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.AmountTransObjectType);
            cbReceivableObjectTypeName.DisplayMember = SysConst.EN_TEXT;
            cbReceivableObjectTypeName.ValueMember = SysConst.EN_Code;
            cbReceivableObjectTypeName.DataSource = _receivableTypeList;
            cbReceivableObjectTypeName.DataBind();

            #endregion

            #region 处理参数

            if (_viewParameters != null)
            {
                #region 业务单确认付款

                //业务单确认付款
                if (_viewParameters.ContainsKey(ComViewParamKey.BusinessPaymentConfirm.ToString()))
                {
                    ListGridDS = _viewParameters[ComViewParamKey.BusinessPaymentConfirm.ToString()] as List<BusinessPayConfirmWindowModel> ?? new List<BusinessPayConfirmWindowModel>();

                    if (ListGridDS.Count > 0)
                    {
                        //组织
                        txtBusinessOrgID.Text = ListGridDS[0].BusinessOrgID;
                        txtBusinessOrgName.Text = ListGridDS[0].BusinessOrgName;

                        //收款对象类型名称
                        cbReceivableObjectTypeName.Text = ListGridDS[0].ReceiveObjectTypeName;
                        cbReceivableObjectTypeName.Value = ListGridDS[0].ReceiveObjectTypeCode;

                        //收款对象
                        txtRecObjectID.Text = ListGridDS[0].ReceiveObjectID;
                        txtRecObjectName.Text = ListGridDS[0].ReceiveObjectName;
                        //钱包余额
                        txtWalletBalance.Text = Convert.ToString(ListGridDS[0].Wal_AvailableBalance);
                        //钱包账号
                        txtWalletNo.Text = ListGridDS[0].Wal_No;

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
                        txtPayableTotalAmount.Text = Convert.ToString(accountPayableAmount);
                        txtPayTotalAmount.Text = Convert.ToString(paidAmount);
                        txtUnPayTotalAmount.Text = Convert.ToString(unpaidAmount);

                        #endregion

                    }
                }

                #endregion
            }

            #endregion

            #region 初始化下拉框

            #region 付款方式

            _payTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.TradeType);
            cbPayTypeName.DisplayMember = SysConst.EN_TEXT;
            cbPayTypeName.ValueMember = SysConst.EN_Code;
            if (string.IsNullOrEmpty(txtWalletNo.Text.Trim()))
            {
                foreach (var loopPayType in _payTypeList)
                {
                    if (loopPayType.Text == TradeTypeEnum.Name.WALLET)
                    {
                        _payTypeList.Remove(loopPayType);
                        break;
                    }
                }
            }
            cbPayTypeName.DataSource = _payTypeList;
            cbPayTypeName.DataBind();
            //默认[现金]
            cbPayTypeName.Text = TradeTypeEnum.Name.CASH;
            cbPayTypeName.Value = TradeTypeEnum.Code.CASH;

            #endregion

            #endregion
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
            //将页面内控件的值赋值给基类的_payBillManager
            SetCardCtrlsToDetailDS();
            if (!_bll.SaveBusinessPayConfirmData(_payBillManager, ListGridDS))
            {
                //保存失败
                MessageBoxs.Show(Trans.COM, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

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
        /// 付款方式改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbPayTypeName_ValueChanged(object sender, EventArgs e)
        {
            if (cbPayTypeName.Text == TradeTypeEnum.Name.CASH)
            {
                lblWalletBalance.Visible = false;
                txtWalletBalance.Visible = false;
                txtWalletNo.Visible = false;
                lblWalletNo.Visible = false;
            }
            else if (cbPayTypeName.Text == TradeTypeEnum.Name.WALLET)
            {
                lblWalletBalance.Visible = true;
                txtWalletBalance.Visible = true;
                txtWalletNo.Visible = true;
                lblWalletNo.Visible = true;
            }
        }
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
                PB_PayableTotalAmount = Convert.ToDecimal(txtPayableTotalAmount.Text.Trim() == "" ? "0" : txtPayableTotalAmount.Text.Trim()),
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

            if (cbPayTypeName.Text == TradeTypeEnum.Name.WALLET)
            {
                decimal walletBalance = Convert.ToDecimal(txtWalletBalance.Text.Trim() == "" ? "0" : txtWalletBalance.Text.Trim());

                decimal thisPayAmount = Convert.ToDecimal(numThisPayAmount.Value ?? 0);
                if (walletBalance < thisPayAmount)
                {
                    //本次付款总金额超出钱包金额，不能支付
                    MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "本次付款总金额超出钱包金额，不能支付" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            decimal payAmount = Convert.ToDecimal(numThisPayAmount.Value ?? 0);
            decimal unPayAmount = Convert.ToDecimal(txtUnPayTotalAmount.Text.Trim() == "" ? "0" : txtUnPayTotalAmount.Text.Trim());
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
            } //设置负向的应付单不可编辑

            #endregion
        }

        #endregion

    }
}
