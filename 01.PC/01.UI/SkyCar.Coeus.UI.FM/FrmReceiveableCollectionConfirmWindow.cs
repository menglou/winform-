using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.FM;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.Common.UIModel;
using SkyCar.Coeus.UIModel.FM.APModel;
using SkyCar.Framework.WindowUI;

namespace SkyCar.Coeus.UI.FM
{
    /// <summary>
    /// 应收单确认收款弹出窗
    /// </summary>
    public partial class FrmReceiveableCollectionConfirmWindow : BaseForm
    {
        #region 全局变量

        /// <summary>
        /// 收款单
        /// </summary>
        MDLFM_ReceiptBill _receiptBill = new MDLFM_ReceiptBill();

        /// <summary>
        /// 应收管理BLL
        /// </summary>
        private ReceiveableCollectionConfirmWindowBLL _bll = new ReceiveableCollectionConfirmWindowBLL();

        /// <summary>
        /// 页面参数字典
        /// </summary>
        private Dictionary<string, object> _viewParameters = new Dictionary<string, object>();

        /// <summary>
        /// Grid的数据源
        /// </summary>
        List<ReceiveableCollectionConfirmUIModel> ListGridDS = new List<ReceiveableCollectionConfirmUIModel>();

        #region 下拉框数据源

        /// <summary>
        /// 付款对象类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _payObjectTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 收款方式
        /// </summary>
        List<ComComboBoxDataSourceTC> _receiveTypeList = new List<ComComboBoxDataSourceTC>();

        #endregion

        #endregion

        #region 系统事件

        /// <summary>
        /// 应收单确认收款弹出窗
        /// </summary>
        public FrmReceiveableCollectionConfirmWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 应收单确认收款弹出窗
        /// </summary>
        /// <param name="paramWindowParameters"></param>
        public FrmReceiveableCollectionConfirmWindow(Dictionary<string, object> paramWindowParameters)
        {
            _viewParameters = paramWindowParameters;

            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmReceiveableCollectionConfirmWindow_Load(object sender, EventArgs e)
        {
            #region 初始化下拉框

            #region 付款对象类型

            _payObjectTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.AmountTransObjectType);
            cbPayObjectTypeName.DisplayMember = SysConst.EN_TEXT;
            cbPayObjectTypeName.ValueMember = SysConst.EN_Code;
            cbPayObjectTypeName.DataSource = _payObjectTypeList;
            cbPayObjectTypeName.DataBind();
            //默认[汽修商户]
            cbPayObjectTypeName.Text = AmountTransObjectTypeEnum.Name.PLATFORMAUTOFACTORY;
            cbPayObjectTypeName.Value = AmountTransObjectTypeEnum.Code.PLATFORMAUTOFACTORY;

            #endregion

            #endregion

            #region 处理参数

            #region 确认收款

            if (_viewParameters != null && _viewParameters.ContainsKey(FMViewParamKey.ReceiveableCashierConfirm.ToString()))
            {
                ListGridDS = _viewParameters[FMViewParamKey.ReceiveableCashierConfirm.ToString()] as List<ReceiveableCollectionConfirmUIModel> ?? new List<ReceiveableCollectionConfirmUIModel>();

                if (ListGridDS.Count > 0)
                {
                    //组织
                    txtBusinessOrgID.Text = ListGridDS[0].BusinessOrgID;
                    txtBusinessOrgName.Text = ListGridDS[0].BusinessOrgName;
                    //付款对象类型
                    cbPayObjectTypeName.Text = ListGridDS[0].PayObjectTypeName;
                    cbPayObjectTypeName.Value = ListGridDS[0].PayObjectTypeCode;
                    //付款对象
                    txtPayObjectName.Text = ListGridDS[0].PayObjectName;
                    //付款对象id
                    txtPayObjectID.Text = ListGridDS[0].PayObjectID;
                    //钱包余额
                    txtWalletBalance.Text = Convert.ToString(ListGridDS[0].Wal_AvailableBalance);
                    //钱包账号
                    txtWalletNo.Text = ListGridDS[0].Wal_No;
                    gdGrid.DataSource = ListGridDS;
                    gdGrid.DataBind();
                    //设置单元格是否可以编辑
                    SetReceiveableConfirmWindowStyle();

                    #region 计算出grid中应付金额，已付金额，未付金额

                    decimal accountReceivableAmount = 0;
                    decimal receiveAmount = 0;
                    decimal unReceiveAmount = 0;
                    foreach (var loopGrid in ListGridDS)
                    {
                        accountReceivableAmount = accountReceivableAmount + (loopGrid.ReceivableTotalAmount ?? 0);
                        receiveAmount = receiveAmount + (loopGrid.ReceiveTotalAmount ?? 0);
                        unReceiveAmount = unReceiveAmount + (loopGrid.UnReceiveTotalAmount ?? 0);
                    }
                    txtReceivableTotalAmount.Text = Convert.ToString(accountReceivableAmount);
                    txtReceiveTotalAmount.Text = Convert.ToString(receiveAmount);
                    txtUnReceiveTotalAmount.Text = Convert.ToString(unReceiveAmount);
                    #endregion

                }
            }
            #endregion

            #endregion

            #region 收款方式

            _receiveTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.TradeType);
            cbReceiveTypeName.DisplayMember = SysConst.EN_TEXT;
            cbReceiveTypeName.ValueMember = SysConst.EN_Code;
            if (string.IsNullOrEmpty(txtWalletNo.Text.Trim()))
            {
                foreach (var loopReceiveType in _receiveTypeList)
                {
                    if (loopReceiveType.Text == TradeTypeEnum.Name.WALLET)
                    {
                        _receiveTypeList.Remove(loopReceiveType);
                        break;
                    }
                }
            }
            cbReceiveTypeName.DataSource = _receiveTypeList;
            cbReceiveTypeName.DataBind();
            //默认[现金]
            cbReceiveTypeName.Text = TradeTypeEnum.Name.CASH;
            cbReceiveTypeName.Value = TradeTypeEnum.Code.CASH;

            #endregion

        }

        #region 控件事件

        /// <summary>
        /// 收款方式改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbReceiveTypeName_ValueChanged(object sender, EventArgs e)
        {
            if (cbReceiveTypeName.Text == TradeTypeEnum.Name.CASH)
            {
                lblWalletBalance.Visible = false;
                txtWalletBalance.Visible = false;
                lblWalletNo.Visible = false;
                txtWalletNo.Visible = false;
            }
            else if (cbReceiveTypeName.Text == TradeTypeEnum.Name.WALLET)
            {
                lblWalletBalance.Visible = true;
                txtWalletBalance.Visible = true;
                lblWalletNo.Visible = true;
                txtWalletNo.Visible = true;
            }
        }
        /// <summary>
        /// 本次收款金额_KeyUp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numThisReceiveAmount_KeyUp(object sender, KeyEventArgs e)
        {
            //未付金额总和
            decimal _minusUnReceiveTotalAmount = 0;

            #region 清除上次[本次付款金额]([未付金额]为负数,不清楚)

            foreach (var loopGridDS in ListGridDS)
            {
                if (loopGridDS.UnReceiveTotalAmount > 0)
                {
                    loopGridDS.ThisReceiveAmount = 0;
                }
                else
                {
                    //计算出负向的未付金额
                    _minusUnReceiveTotalAmount = _minusUnReceiveTotalAmount - (loopGridDS.UnReceiveTotalAmount ?? 0);
                }
            }

            #endregion

            #region 本次付款总金额为空时，去抵消正负应付金额

            if (string.IsNullOrEmpty(numThisReceiveAmount.Text))
            {
                for (int i = 0; i < ListGridDS.Count; i++)
                {
                    //如果[未付金额]小于零，跳到下一次循环
                    if (ListGridDS[i].UnReceiveTotalAmount < 0)
                    {
                        continue;
                    }
                    //如果[负向应付金额]之和大于[未付金额]
                    if (_minusUnReceiveTotalAmount > (ListGridDS[i].UnReceiveTotalAmount ?? 0))
                    {
                        ListGridDS[i].ThisReceiveAmount = ListGridDS[i].UnReceiveTotalAmount ?? 0;
                        _minusUnReceiveTotalAmount = _minusUnReceiveTotalAmount - (ListGridDS[i].UnReceiveTotalAmount ?? 0);
                    }
                    //如果[负向应付金额]之和小于或等于[未付金额]
                    else if (_minusUnReceiveTotalAmount <= (ListGridDS[i].UnReceiveTotalAmount ?? 0))
                    {
                        ListGridDS[i].ThisReceiveAmount = _minusUnReceiveTotalAmount;
                        _minusUnReceiveTotalAmount = 0;
                        break;
                    }
                    //如果如果[负向应付金额]之和大于并且是最后一次循环
                    if (_minusUnReceiveTotalAmount > 0 && i == (ListGridDS.Count - 1))
                    {
                        ListGridDS[0].ThisReceiveAmount = ListGridDS[0].ThisReceiveAmount + _minusUnReceiveTotalAmount;
                        _minusUnReceiveTotalAmount = 0;
                    }
                }
                gdGrid.DataSource = ListGridDS;
                gdGrid.DataBind();
                return;
            }

            #endregion

            #region 本次付款总金额不为空时，负向应付金额总和加上本次付款总金额，然后在去平均分配

            decimal thisReceiveAmount = Convert.ToDecimal(numThisReceiveAmount.Value ?? 0) + _minusUnReceiveTotalAmount;
            for (int i = 0; i < ListGridDS.Count; i++)
            {
                //如果[未付金额]小于零，跳到下一次循环
                if (ListGridDS[i].UnReceiveTotalAmount < 0)
                {
                    for (int j = 0; j < ListGridDS.Count; j++)
                    {
                        if (ListGridDS[j].ReceivableTotalAmount > 0)
                        {
                            ListGridDS[j].ThisReceiveAmount = ListGridDS[j].ThisReceiveAmount + thisReceiveAmount;
                            thisReceiveAmount = 0;
                            break;
                        }
                    }
                    continue;
                }
                //如果[本次付款总金额]之和大于[未付金额]
                if (thisReceiveAmount > (ListGridDS[i].UnReceiveTotalAmount ?? 0))
                {
                    ListGridDS[i].ThisReceiveAmount = ListGridDS[i].UnReceiveTotalAmount ?? 0;
                    thisReceiveAmount = thisReceiveAmount - (ListGridDS[i].UnReceiveTotalAmount ?? 0);
                }
                //如果[本次付款总金额]之和小于或等于[未付金额]
                else if (thisReceiveAmount <= (ListGridDS[i].UnReceiveTotalAmount ?? 0))
                {
                    ListGridDS[i].ThisReceiveAmount = thisReceiveAmount;
                    thisReceiveAmount = 0;
                    break;
                }
                //如果如果[本次付款总金额]之和大于并且是最后一次循环
                if (thisReceiveAmount > 0 && i == (ListGridDS.Count - 1))
                {
                    for (int j = 0; j < ListGridDS.Count; j++)
                    {
                        if (ListGridDS[j].ReceivableTotalAmount > 0)
                        {
                            ListGridDS[j].ThisReceiveAmount = ListGridDS[j].ThisReceiveAmount + thisReceiveAmount;
                            thisReceiveAmount = 0;
                            break;
                        }
                    }
                }
            }

            #endregion

            gdGrid.DataSource = ListGridDS;
            gdGrid.DataBind();
        }
        #endregion

        #region gdGrid相关事件

        /// <summary>
        /// gdGrid单元格的值变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_CellChange(object sender, CellEventArgs e)
        {
            //验证本次收款金额格式
            if (e.Cell.Column.Key == "ThisReceiveAmount")
            {
                gdGrid.UpdateData();
                decimal thisReceiveAmount = 0;

                #region 给[本次付款总金额赋值]

                foreach (var loopGrid in ListGridDS)
                {
                    if (loopGrid.UnReceiveTotalAmount < 0)
                    {
                        loopGrid.ThisReceiveAmount = loopGrid.UnReceiveTotalAmount;
                    }
                    thisReceiveAmount = thisReceiveAmount + (loopGrid.ThisReceiveAmount ?? 0);
                }

                #endregion

                numThisReceiveAmount.Text = Convert.ToString(thisReceiveAmount);
                gdGrid.UpdateData();
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
            //将页面内控件的值赋值给基类的_receiptBill
            SetCardCtrlsToDetailDS();
            if (!_bll.SaveReceiveableCollectionConfirmData(_receiptBill, ListGridDS))
            {
                //保存失败
                MessageBoxs.Show(Trans.FM, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        #endregion

        #region 自定义方法

        /// <summary>
        /// 将页面内控件的值赋值给基类的_receiptBill
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            _receiptBill = new MDLFM_ReceiptBill
            {
                RB_ReceiveTypeName = cbReceiveTypeName.Text,
                RB_ReceiveTypeCode = cbReceiveTypeName.Value?.ToString() ?? "",
                RB_Remark = txtRemark.Text,
                RB_CertificateNo = txtReceiveCertificateNo.Text,
                RB_ReceiveTotalAmount = Convert.ToDecimal(numThisReceiveAmount.Value ?? 0),
            };
        }

        /// <summary>
        /// 前端检查-保存
        /// </summary>
        private bool ClientCheckForSave()
        {
            #region 验证

            if (string.IsNullOrEmpty(numThisReceiveAmount.Text.Trim()))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { MsgParam.THIS_PAYAMOUNT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                numThisReceiveAmount.Focus();
                return false;
            }

            if (cbReceiveTypeName.Text == TradeTypeEnum.Name.WALLET)
            {
                decimal walletBalance = Convert.ToDecimal(txtWalletBalance.Text.Trim() == "" ? "0" : txtWalletBalance.Text.Trim());
                decimal thisPayAmount = Convert.ToDecimal(numThisReceiveAmount.Value ?? 0);
                if (walletBalance < thisPayAmount)
                {
                    //本次付款总金额超出钱包金额，不能支付
                    MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "本次付款总金额超出钱包金额，不能支付" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            decimal receiveAmount = Convert.ToDecimal(numThisReceiveAmount.Value ?? 0);
            decimal unReceiveAmount = Convert.ToDecimal(txtUnReceiveTotalAmount.Text.Trim() == "" ? "0" : txtUnReceiveTotalAmount.Text.Trim());
            if (receiveAmount > unReceiveAmount)
            {
                //本次收款金额大于收付金额，是否确认支付？\r\n单击【确定】支付单据，【取消】返回。
                DialogResult isPay = MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0037, new object[] { MsgParam.THIS_RECERIVEAMOUNT, SystemTableColumnEnums.FM_AccountReceivableBill.Name.ARB_UnReceiveAmount, MsgParam.PAY }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
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
        private void SetReceiveableConfirmWindowStyle()
        {
            #region 设置Grid数据颜色
            gdGrid.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;

            for (int i = 0; i < ListGridDS.Count; i++)
            {
                if ((ListGridDS[i].UnReceiveTotalAmount ?? 0) < 0)
                {
                    //设置[本次付款金额]单元格不可编辑
                    gdGrid.DisplayLayout.Rows[i].Cells["ThisReceiveAmount"].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    gdGrid.DisplayLayout.Rows[i].Cells["ThisReceiveAmount"].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    gdGrid.DisplayLayout.Rows[i].Cells["ThisReceiveAmount"].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;
                    ListGridDS[i].ThisReceiveAmount = ListGridDS[i].UnReceiveTotalAmount ?? 0;
                }
                else
                {
                    //设置[本次付款金额]单元格不可编辑
                    gdGrid.DisplayLayout.Rows[i].Cells["ThisReceiveAmount"].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    gdGrid.DisplayLayout.Rows[i].Cells["ThisReceiveAmount"].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    gdGrid.DisplayLayout.Rows[i].Cells["ThisReceiveAmount"].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;
                }
            }

            #endregion
        }
        #endregion

    }
}
