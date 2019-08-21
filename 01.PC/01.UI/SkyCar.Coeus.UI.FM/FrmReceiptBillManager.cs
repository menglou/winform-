using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.FM;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.FM;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.Common.Enums;
using System.Net;
using Infragistics.Win.UltraWinTabControl;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.Common.UIModel;

namespace SkyCar.Coeus.UI.FM
{
    /// <summary>
    /// 收款单管理
    /// </summary>
    public partial class FrmReceiptBillManager : BaseFormCardListDetail<ReceiptBillManagerUIModel, ReceiptBillManagerQCModel, MDLFM_ReceiptBill>
    {
        #region 全局变量

        /// <summary>
        /// 收款单管理BLL
        /// </summary>
        private ReceiptBillManagerBLL _bll = new ReceiptBillManagerBLL();
        /// <summary>
        /// 付款单明细
        /// </summary>
        private SkyCarBindingList<ReceiptBillManagerDetailUIModel, MDLFM_ReceiptBillDetail> _detailGridDS = new SkyCarBindingList<ReceiptBillManagerDetailUIModel, MDLFM_ReceiptBillDetail>();

        /// <summary>
        /// 图片名称与临时保存路径
        /// </summary>
        private Dictionary<string, string> _dicPictureNameAndTempPath = new Dictionary<string, string>();

        /// <summary>
        /// 页面参数字典
        /// </summary>
        private Dictionary<string, object> _viewParameters;

        #region 下拉框数据源
        /// <summary>
        /// 收款单来源类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _ReceiptBillDetailSourceTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 付款对象类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _payObjectTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 收款方式
        /// </summary>
        List<ComComboBoxDataSourceTC> _receiveTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 业务状态
        /// </summary>
        List<ComComboBoxDataSourceTC> _receiptBillStatusList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 审核状态
        /// </summary>
        List<ComComboBoxDataSourceTC> _approvalStatusList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 所有客户数据源
        /// </summary>
        List<ComClientUIModel> _tempAllClientList = new List<ComClientUIModel>();
        /// <summary>
        /// 客户数据源
        /// </summary>
        List<ComClientUIModel> _clientList = new List<ComClientUIModel>();
        #endregion

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmReceiptBillManager构造方法
        /// </summary>
        public FrmReceiptBillManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// FrmReceiptBillManager构造方法
        /// </summary>
        public FrmReceiptBillManager(Dictionary<string, object> paramWindowParameters)
        {
            InitializeComponent();
            _viewParameters = paramWindowParameters;

        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmReceiptBillManager_Load(object sender, EventArgs e)
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

            _tempAllClientList = BLLCom.GetAllCustomerList(LoginInfoDAX.OrgID);

            //初始化【详情】Tab内控件
            InitializeDetailTabControls();
            //初始化【列表】Tab内控件
            InitializeListTabControls();
            //根据选中的Tab，设置动作按钮[是否可用]（在系统权限的基础上进行控制）
            base.SetActionEnableBySelectedTab(SysConst.EN_LIST);
            #endregion

            //[列表]页不允许删除
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
            }
            //[打印]不可用
            SetActionEnable(SystemActionEnum.Code.PRINT, false);

            #region 处理参数

            if (_viewParameters == null)
            {
                return;
            }

            #region 付款单

            if (!_viewParameters.ContainsKey(ComViewParamKey.BillNo.ToString()))
            {
                return;
            }
            string billNo = _viewParameters[ComViewParamKey.BillNo.ToString()] as string;

            if (!string.IsNullOrEmpty(billNo))
            {
                #region 加载列表tab

                txtWhere_RB_No.Text = billNo;
                QueryAction();

                #endregion

                #region 加载详情tab

                if (HeadGridDS.Count == 1)
                {
                    //收款ID
                    txtRB_ID.Text = HeadGridDS[0].RB_ID;
                    //收款单号
                    txtRB_No.Text = HeadGridDS[0].RB_No;
                    //收款组织ID
                    txtRB_Rec_Org_ID.Text = HeadGridDS[0].RB_Rec_Org_ID;
                    //收款组织名称
                    txtRB_Rec_Org_Name.Text = HeadGridDS[0].RB_Rec_Org_Name;
                    //付款对象类型
                    mcbARB_PayObjectTypeName.SelectedValue = HeadGridDS[0].RB_PayObjectTypeCode;
                    //付款对象
                    mcbARB_PayObjectName.SelectedValue = HeadGridDS[0].RB_PayObjectID;
                    //收款日期
                    dtRB_Date.Value = HeadGridDS[0].RB_Date;
                    //收款通道编码
                    cbRB_ReceiveTypeName.Value = HeadGridDS[0].RB_ReceiveTypeCode;
                    //收款通道名称
                    cbRB_ReceiveTypeName.Text = HeadGridDS[0].RB_ReceiveTypeName ?? "";
                    //收款凭证编号
                    txtRB_CertificateNo.Text = HeadGridDS[0].RB_CertificateNo;
                    //收款凭证图片
                    if (!string.IsNullOrEmpty(HeadGridDS[0].RB_CertificatePic))
                    {
                        pbRB_CertificatePic.PictureKey = HeadGridDS[0].RB_CertificatePic;
                        pbRB_CertificatePic.PictureImage = BLLCom.GetBitmapImageByFileName(HeadGridDS[0].RB_CertificatePic);
                    }
                    else
                    {
                        pbRB_CertificatePic.PictureKey = Guid.NewGuid() + ".jpg";
                    }
                    //合计金额
                    numRB_ReceiveTotalAmount.Value = HeadGridDS[0].RB_ReceiveTotalAmount;
                    //业务状态名称
                    cbRB_BusinessStatusName.Text = HeadGridDS[0].RB_BusinessStatusName ?? "";
                    //业务状态编码
                    cbRB_BusinessStatusName.Value = HeadGridDS[0].RB_BusinessStatusCode;
                    //审核状态名称
                    cbRB_ApprovalStatusName.Text = HeadGridDS[0].RB_ApprovalStatusName ?? "";
                    //审核状态编码
                    cbRB_ApprovalStatusName.Value = HeadGridDS[0].RB_ApprovalStatusCode;
                    //备注
                    txtRB_Remark.Text = HeadGridDS[0].RB_Remark;
                    //有效
                    if (HeadGridDS[0].RB_IsValid != null)
                    {
                        ckRB_IsValid.Checked = HeadGridDS[0].RB_IsValid.Value;
                    }
                    //创建人
                    txtRB_CreatedBy.Text = HeadGridDS[0].RB_CreatedBy;
                    //创建时间
                    dtRB_CreatedTime.Value = HeadGridDS[0].RB_CreatedTime;
                    //修改人
                    txtRB_UpdatedBy.Text = HeadGridDS[0].RB_UpdatedBy;
                    //修改时间
                    dtRB_UpdatedTime.Value = HeadGridDS[0].RB_UpdatedTime;
                    //版本号
                    txtRB_VersionNo.Value = HeadGridDS[0].RB_VersionNo;
                    //钱包ID
                    txtWal_ID.Text = HeadGridDS[0].Wal_ID;
                    //钱包账号
                    txtWal_No.Text = HeadGridDS[0].Wal_No;
                    //钱包余额
                    txtWal_AvailableBalance.Value = HeadGridDS[0].Wal_AvailableBalance;
                    //钱包版本号
                    txtWal_VersionNo.Value = HeadGridDS[0].Wal_VersionNo;
                }
                //查询明细
                QueryDetail();

                #endregion
                //选择【详情】Tab
                tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;
                // 设置动作按钮状态
                SetActionEnableByStatus();
                //设置详情页面控件的是否可编辑
                SetDetailControl();
                //设置图片是否可编辑
                SetPictureControl();
                //设置单元格是否可以编辑
                SetSalesOrderDetailStyle();
            }

            #endregion

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
        /// 【列表】Grid的CellChange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_CellChange(object sender, CellEventArgs e)
        {
            gdGrid.UpdateData();
            decimal receiveTotalAmount = 0;

            foreach (var loopGridRow in gdGrid.Rows)
            {
                if (string.IsNullOrEmpty(loopGridRow.Cells[SystemTableColumnEnums.FM_ReceiptBill.Code.RB_No].Value?.ToString())
                    || loopGridRow.Cells["IsChecked"].Value == null
                    || (bool)loopGridRow.Cells["IsChecked"].Value == false)
                {
                    continue;
                }
                receiveTotalAmount = receiveTotalAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_ReceiptBill.Code.RB_ReceiveTotalAmount].Value?.ToString());
            }

            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtReceiveTotalAmount"])).Text = Convert.ToString(receiveTotalAmount);
        }

        /// <summary>
        /// 【列表】Grid的AfterHeaderCheckStateChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            gdGrid.UpdateData();
            decimal receiveTotalAmount = 0;

            foreach (var loopGridRow in gdGrid.Rows)
            {
                if (string.IsNullOrEmpty(loopGridRow.Cells[SystemTableColumnEnums.FM_ReceiptBill.Code.RB_No].Value?.ToString())
                    || loopGridRow.Cells["IsChecked"].Value == null
                    || (bool)loopGridRow.Cells["IsChecked"].Value == false)
                {
                    continue;
                }
                receiveTotalAmount = receiveTotalAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_ReceiptBill.Code.RB_ReceiveTotalAmount].Value?.ToString());
            }

            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtReceiveTotalAmount"])).Text = Convert.ToString(receiveTotalAmount);
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
        
        /// <summary>
        /// 选中的Tab改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlFull_SelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            //[列表]页不允许删除、打印
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                SetActionEnable(SystemActionEnum.Code.PRINT, false);
            }
            else
            {
                //设置动作按钮状态
                SetActionEnableByStatus();
            }
        }

        #endregion

        #region 查询条件相关事件

        /// <summary>
        /// 收款单号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_RB_No_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }

        /// <summary>
        /// 付款对象KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_RB_PayObjectName_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 收款通道ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_RB_ReceiveTypeName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 收款日期ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_RB_Date_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 业务状态ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_RB_BusinessStatusName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 审核状态ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_RB_ApprovalStatusName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }

        /// <summary>
        /// 列表查询条件dtWhere_RB_DateEnd_ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_RB_DateEnd_ValueChanged(object sender, EventArgs e)
        {
            if (this.dtWhere_RB_DateEnd.Value != null &&
              this.dtWhere_RB_DateEnd.DateTime.Hour == 0 &&
              this.dtWhere_RB_DateEnd.DateTime.Minute == 0 &&
              this.dtWhere_RB_DateEnd.DateTime.Second == 0)
            {
                DateTime newDateTime = new DateTime(this.dtWhere_RB_DateEnd.DateTime.Year, this.dtWhere_RB_DateEnd.DateTime.Month, this.dtWhere_RB_DateEnd.DateTime.Day, 23, 59, 59);
                this.dtWhere_RB_DateEnd.DateTime = newDateTime;
            }
        }

        #endregion

        #region 详情相关事件

        /// <summary>
        /// [付款对象]SelectedIndexChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbARB_PayObjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //根据选择的对象选中对象类型
            if (!string.IsNullOrEmpty(mcbARB_PayObjectName.SelectedTextExtra))
            {
                mcbARB_PayObjectTypeName.SelectedText = mcbARB_PayObjectName.SelectedTextExtra;
            }
            else if (!string.IsNullOrEmpty(mcbARB_PayObjectName.SelectedText))
            {
                mcbARB_PayObjectTypeName.SelectedText = "其他";
            }
        }

        /// <summary>
        /// [付款对象类型]SelectedIndexChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbARB_PayObjectTypeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curClientType = mcbARB_PayObjectName.SelectedTextExtra;
            if (curClientType != mcbARB_PayObjectTypeName.SelectedText)
            {
                _clientList.Clear();
                if (string.IsNullOrEmpty(mcbARB_PayObjectTypeName.SelectedText))
                {
                    _bll.CopyModelList(_tempAllClientList, _clientList);
                }
                else
                {
                    foreach (var loopTempClient in _tempAllClientList)
                    {
                        if (loopTempClient.ClientType == mcbARB_PayObjectTypeName.SelectedText)
                        {
                            _clientList.Add(loopTempClient);
                        }
                    }
                }
                mcbARB_PayObjectName.DataSource = _clientList;
                mcbARB_PayObjectName.Clear();
            }
            else if (string.IsNullOrEmpty(mcbARB_PayObjectTypeName.SelectedText))
            {
                _bll.CopyModelList(_tempAllClientList, _clientList);
                mcbARB_PayObjectName.DataSource = _clientList;
                mcbARB_PayObjectName.Clear();
            }
        }

        /// <summary>
        /// 审核状态ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbRB_ApprovalStatusName_ValueChanged(object sender, EventArgs e)
        {
            if (cbRB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                mcbARB_PayObjectTypeName.Enabled = false;
            }
            else
            {
                mcbARB_PayObjectTypeName.Enabled = true;
            }
        }
        
        /// <summary>
        /// 钱包ID_ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWal_ID_ValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtWal_ID.Text.Trim()))
            {
                txtWal_No.Visible = false;
                lblWal_No.Visible = false;
                txtWal_AvailableBalance.Visible = false;
                lblWal_AvailableBalance.Visible = false;
                //【收款方式】绑定数据源
                var receiveTypeList = _receiveTypeList.Where(x => x.Text != TradeTypeEnum.Name.WALLET).ToList();
                cbRB_ReceiveTypeName.DisplayMember = SysConst.EN_TEXT;
                cbRB_ReceiveTypeName.ValueMember = SysConst.EN_Code;
                cbRB_ReceiveTypeName.DataSource = receiveTypeList;
                cbRB_ReceiveTypeName.DataBind();
            }
            else
            {
                //【收款方式】绑定数据源
                cbRB_ReceiveTypeName.DisplayMember = SysConst.EN_TEXT;
                cbRB_ReceiveTypeName.ValueMember = SysConst.EN_Code;
                cbRB_ReceiveTypeName.DataSource = _receiveTypeList;
                cbRB_ReceiveTypeName.DataBind();
                if (cbRB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
                {
                    txtWal_No.Visible = false;
                    lblWal_No.Visible = false;
                    txtWal_AvailableBalance.Visible = false;
                    lblWal_AvailableBalance.Visible = false;
                }
                else
                {
                    txtWal_No.Visible = true;
                    lblWal_No.Visible = true;
                    txtWal_AvailableBalance.Visible = true;
                    lblWal_AvailableBalance.Visible = true;
                }
            }
        }
        
        /// <summary>
        /// 明细来源单号_TextChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRBD_SrcBillNo_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRBD_SrcBillNo.Text.Trim()))
            {
                txtRBD_SrcBillNo.Visible = false;
                lblRBD_SrcBillNo.Visible = false;
            }
            else
            {
                txtRBD_SrcBillNo.Visible = true;
                lblRBD_SrcBillNo.Visible = true;
            }
        }

        #endregion

        #region 明细相关事件

        /// <summary>
        /// 添加/删除明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolbarsManagerDetail_ToolClick(object sender, ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case SysConst.EN_ADD:
                    AddReceiptDetail();
                    break;

                case SysConst.EN_DEL:
                    DeleteReceiptDetail();
                    break;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// 明细Grid改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_CellChange(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.Key == SystemTableColumnEnums.FM_ReceiptBillDetail.Code.RBD_ReceiveAmount)
            {
                //计算总金额
                decimal totalPayAmount = 0;
                gdDetail.UpdateData();
                foreach (var loopDetailGrid in _detailGridDS)
                {
                    totalPayAmount = totalPayAmount + (loopDetailGrid.RBD_ReceiveAmount ?? 0);
                }
                numRB_ReceiveTotalAmount.Text = Convert.ToString(totalPayAmount);
            }
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
            if (ViewHasChanged()
                || _detailGridDS.InsertList.Count > 0
                || _detailGridDS.UpdateList.Count > 0
                || _detailGridDS.DeleteList.Count > 0)
            {
                //信息尚未保存，确定进行当前操作？
                DialogResult dialogResult = MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.W_0001), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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

            //设置动作按钮状态
            SetActionEnableByStatus();
            //控制详情是否可编辑
            SetDetailControl();
            //设置图片是否可编辑
            SetPictureControl();
            //设置单元格是否可以编辑
            SetSalesOrderDetailStyle();

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
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
            bool saveResult = _bll.SaveDetailDS(HeadDS, _detailGridDS, _dicPictureNameAndTempPath);
            if (!saveResult)
            {
                //保存失败
                MessageBoxs.Show(Trans.FM, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //开始监控List变化
            _detailGridDS.StartMonitChanges();
            //将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //控制详情是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();
            //设置图片是否可编辑
            SetPictureControl();

            //设置图片Key
            pbRB_CertificatePic.PictureKey = !string.IsNullOrEmpty(HeadDS.RB_CertificatePic) ? HeadDS.RB_CertificatePic : (Guid.NewGuid() + ".jpg");

            //清除图片临时路径
            _dicPictureNameAndTempPath.Clear();
            //设置单元格是否可以编辑
            SetSalesOrderDetailStyle();
        }
       
        /// <summary>
        /// 删除
        /// </summary>
        public override void DeleteAction()
        {
            //1.前端检查-删除
            if (!ClientCheckForDelete())
            {
                return;
            }

            var argsDetail = new List<MDLFM_ReceiptBillDetail>();
            //将HeadDS转换为TBModel对象
            var argsHead = HeadDS.ToTBModelForSaveAndDelete<MDLFM_ReceiptBill>();
            //将当前DetailGridDS转换为指定类型的TBModelList
            _detailGridDS.ToTBModelListForUpdateAndDelete<MDLFM_ReceiptBillDetail>(argsDetail);
            //过滤明细列表中未保存的数据
            argsDetail = argsDetail.Where(x => !string.IsNullOrEmpty(x.WHERE_RBD_ID)).ToList();
            //2.执行删除
            bool deleteResult = _bll.UnityDelete<MDLFM_ReceiptBill, MDLFM_ReceiptBillDetail>(argsHead, argsDetail);
            if (!deleteResult)
            {
                //删除失败
                MessageBoxs.Show(Trans.FM, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //删除成功
            MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //3.清空【详情】画面数据
            InitializeDetailTabControls();
            //刷新列表
            RefreshList(true);

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();
        }
    
        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            //2.设置查询条件（翻页相关属性不用设置）
            base.ConditionDS = new ReceiptBillManagerQCModel()
            {
                SqlId = SQLID.FM_ReceiptBillManager_SQL01,
                //收款单号
                WHERE_RB_No = txtWhere_RB_No.Text.Trim(),
                //付款对象类型
                //WHERE_RB_PayObjectTypeName = mcbWhere_ARB_PayObjectName.SelectedText,
                //付款对象
                WHERE_RB_PayObjectName = mcbWhere_ARB_PayObjectName.SelectedText,
                //收款方式
                WHERE_RB_ReceiveTypeName = cbWhere_RB_ReceiveTypeName.Text,
                //业务状态
                WHERE_RB_BusinessStatusName = cbWhere_RB_BusinessStatusName.Text,
                //审核状态
                WHERE_RB_ApprovalStatusName = cbWhere_RB_ApprovalStatusName.Text,
                //收款组织ID
                WHERE_RB_Rec_Org_ID = LoginInfoDAX.OrgID,
                //明细来源单号
                WHERE_RBD_SrcBillNo = txtWhere_RBD_SrcBillNo.Text,
            };
            if (dtWhere_RB_DateStart.Value != null)
            {
                //收款时间-开始
                ConditionDS._DateStart = dtWhere_RB_DateStart.DateTime;
            }
            if (dtWhere_RB_DateEnd.Value != null)
            {
                //收款时间-终了
                ConditionDS._DateEnd = dtWhere_RB_DateEnd.DateTime;
            }
            //3.执行基类方法（注意：基类使用模糊查询）
            base.QueryAction();
            //4.Grid绑定数据源
            gdGrid.DataSource = base.HeadGridDS;
            gdGrid.DataBind();
            //5.设置Grid自适应列宽（根据单元格内容）
            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
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
            if (ViewHasChanged()
                || _detailGridDS.InsertList.Count > 0
                || _detailGridDS.UpdateList.Count > 0
                || _detailGridDS.DeleteList.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 审核
        /// </summary>
        public override void ApproveAction()
        {
            base.ApproveAction();

            gdDetail.UpdateData();

            #region 验证数据
            //未保存,不能审核
            if (string.IsNullOrEmpty(txtRB_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.FM_ReceiptBill + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MDLFM_ReceiptBill resultReceiptBill = new MDLFM_ReceiptBill();
            _bll.QueryForObject<MDLFM_ReceiptBill, MDLFM_ReceiptBill>(new MDLFM_ReceiptBill()
            {
                WHERE_RB_IsValid = true,
                WHERE_RB_ID = txtRB_ID.Text.Trim()
            }, resultReceiptBill);
            //收款单不存在,不能审核
            if (string.IsNullOrEmpty(resultReceiptBill.RB_ID))
            {
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.FM_ReceiptBill + MsgParam.NOTEXIST, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #endregion

            //收款日期
            dtRB_Date.Value = BLLCom.GetCurStdDatetime();

            //将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();

            #region 审核收款单
            bool saveApprove = _bll.ApproveDetailDS(HeadDS, _detailGridDS);
            //审核失败
            if (!saveApprove)
            {
                MessageBoxs.Show(Trans.FM, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //审核成功
            MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.APPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            #endregion

            //刷新列表
            RefreshList();

            //开始监控List变化
            _detailGridDS.StartMonitChanges();
            //将HeadDS数据赋值给【详情】Tab内的对应控件
            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //控制详情是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();
            //设置图片是否可编辑
            SetPictureControl();
            //设置单元格是否可以编辑
            SetSalesOrderDetailStyle();
        }
        
        /// <summary>
        /// 打印
        /// </summary>
        public override void PrintAction()
        {
            base.PrintAction();
            if (HeadDS == null || string.IsNullOrWhiteSpace(HeadDS.RB_No))
            {
                //没有获取到收款单，打印失败
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.FM_ReceiptBill, SystemActionEnum.Name.PRINT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (HeadDS.RB_ApprovalStatusName != ApprovalStatusEnum.Name.YSH)
            {
                //收款单待审核，不能打印
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, SystemTableEnums.Name.FM_ReceiptBill + cbRB_ApprovalStatusName.Text, SystemActionEnum.Name.PRINT), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                //待打印的收款单
                ReceiptBillUIModelToPrint receiptBillToPrint = new ReceiptBillUIModelToPrint();
                _bll.CopyModel(HeadDS, receiptBillToPrint);
                //待打印的收款单明细
                List<ReceiptBillDetailUIModelToPtint> receiptBillDetailToPrintList = new List<ReceiptBillDetailUIModelToPtint>();
                _bll.CopyModelList(_detailGridDS, receiptBillDetailToPrintList);
                Dictionary<string, object> argsViewParams = new Dictionary<string, object>
                {
                    //收款单
                    {FMViewParamKey.ReceiptBill.ToString(), receiptBillToPrint},
                    //收款单明细
                    {FMViewParamKey.ReceiptBillDetail.ToString(), receiptBillDetailToPrintList},
                };
                FrmViewAndPrintReceiptBill frmViewAndPrintReceiptBill = new FrmViewAndPrintReceiptBill(argsViewParams)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = frmViewAndPrintReceiptBill.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBoxs.Show(Trans.FM, ToString(), ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            //收款单号
            txtRB_No.Clear();
            //付款对象类型
            mcbARB_PayObjectTypeName.Clear();
            //付款对象
            mcbARB_PayObjectName.Clear();
            //收款组织名称
            txtRB_Rec_Org_Name.Clear();
            //收款日期
            dtRB_Date.Value = DateTime.Now;
            //收款通道名称
            cbRB_ReceiveTypeName.Value = null;
            //收款凭证编号
            txtRB_CertificateNo.Clear();
            //收款凭证图片
            pbRB_CertificatePic.PictureImage = null;
            //合计金额
            numRB_ReceiveTotalAmount.Value = null;
            //业务状态名称
            cbRB_BusinessStatusName.Value = null;
            //审核状态名称
            cbRB_ApprovalStatusName.Value = null;
            //备注
            txtRB_Remark.Clear();
            //有效
            ckRB_IsValid.Checked = true;
            ckRB_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtRB_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtRB_CreatedTime.Value = DateTime.Now;
            //修改人
            txtRB_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtRB_UpdatedTime.Value = DateTime.Now;
            //版本号
            txtRB_VersionNo.Clear();
            //收款ID
            txtRB_ID.Clear();
            //收款组织ID
            txtRB_Rec_Org_ID.Clear();
            //给 收款单号 设置焦点
            lblRB_No.Focus();

            //默认收款组织为当前组织
            txtRB_Rec_Org_Name.Text = LoginInfoDAX.OrgShortName;
            txtRB_Rec_Org_ID.Text = LoginInfoDAX.OrgID;

            //钱包账号
            txtWal_No.Clear();
            //钱包余额
            txtWal_AvailableBalance.Clear();
            //钱包ID
            txtWal_ID.Clear();
            //钱包版本号
            txtWal_VersionNo.Clear();
            //明细来源单号
            txtRBD_SrcBillNo.Clear();

            #region 初始化下拉框
            //来源类型【手工收款，销售收款，其他收款（赔偿）】
            _ReceiptBillDetailSourceTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.ReceiptBillDetailSourceType);

            //付款对象
            _bll.CopyModelList(_tempAllClientList, _clientList);
            mcbARB_PayObjectName.ExtraDisplayMember = "ClientType";
            mcbARB_PayObjectName.DisplayMember = "ClientName";
            mcbARB_PayObjectName.ValueMember = "ClientID";
            mcbARB_PayObjectName.DataSource = _clientList;

            //付款对象类型
            _payObjectTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.AmountTransObjectType);
            mcbARB_PayObjectTypeName.DisplayMember = SysConst.EN_TEXT;
            mcbARB_PayObjectTypeName.ValueMember = SysConst.EN_Code;
            mcbARB_PayObjectTypeName.DataSource = _payObjectTypeList;
            //默认客户类型为汽修商户
            mcbARB_PayObjectTypeName.SelectedValue = AmountTransObjectTypeEnum.Code.PLATFORMAUTOFACTORY;

            //收款方式【银行，微信，支付宝，现金】
            _receiveTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.TradeType);
            var receiveTypeList = _receiveTypeList.Where(x => x.Text != TradeTypeEnum.Name.WALLET).ToList();
            cbRB_ReceiveTypeName.DisplayMember = SysConst.EN_TEXT;
            cbRB_ReceiveTypeName.ValueMember = SysConst.EN_Code;
            cbRB_ReceiveTypeName.DataSource = receiveTypeList;
            cbRB_ReceiveTypeName.DataBind();
            //默认[现金]
            cbRB_ReceiveTypeName.Text = TradeTypeEnum.Name.CASH;
            cbRB_ReceiveTypeName.Value = TradeTypeEnum.Code.CASH;

            //业务状态【已生成，执行中，已完成】
            _receiptBillStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.ReceiptBillStatus);
            cbRB_BusinessStatusName.DisplayMember = SysConst.EN_TEXT;
            cbRB_BusinessStatusName.ValueMember = SysConst.EN_Code;
            cbRB_BusinessStatusName.DataSource = _receiptBillStatusList;
            cbRB_BusinessStatusName.DataBind();
            //默认[已生成]
            cbRB_BusinessStatusName.Text = ReceiptBillStatusEnum.Name.YSC;
            cbRB_BusinessStatusName.Value = ReceiptBillStatusEnum.Code.YSC;

            //审核状态【待审核，已审核】
            _approvalStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.ApprovalStatus);
            cbRB_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
            cbRB_ApprovalStatusName.ValueMember = SysConst.EN_Code;
            cbRB_ApprovalStatusName.DataSource = _approvalStatusList;
            cbRB_ApprovalStatusName.DataBind();
            //默认[待审核]
            cbRB_ApprovalStatusName.Text = ApprovalStatusEnum.Name.DSH;
            cbRB_ApprovalStatusName.Value = ApprovalStatusEnum.Code.DSH;
            #endregion

            //初始化明细grid
            _detailGridDS = new SkyCarBindingList<ReceiptBillManagerDetailUIModel, MDLFM_ReceiptBillDetail>();
            //开始监控List变化
            _detailGridDS.StartMonitChanges();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();

            #region 初始化图片

            //付款凭证图片
            pbRB_CertificatePic.PictureKey = Guid.NewGuid() + ".jpg";
            pbRB_CertificatePic.ExecuteUpload = UploadPicture;
            pbRB_CertificatePic.ExecuteExport = ExportPicture;
            pbRB_CertificatePic.ExecuteDelete = DeletePicture;

            #endregion
        }
        
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 查询条件初始化
            //收款单号
            txtWhere_RB_No.Clear();
            //付款对象类型
            mcbARB_PayObjectTypeName.Clear();
            //付款对象
            mcbARB_PayObjectName.Clear();
            //收款方式
            cbWhere_RB_ReceiveTypeName.Value = null;
            //收款日期-开始
            dtWhere_RB_DateStart.Value = null;
            //收款日期-终了
            dtWhere_RB_DateEnd.Value = null;
            //业务状态
            cbWhere_RB_BusinessStatusName.Value = null;
            //审核状态
            cbWhere_RB_ApprovalStatusName.Value = null;
            //明细来源单号
            txtWhere_RBD_SrcBillNo.Clear();
            //给 收款单号 设置焦点
            lblWhere_RB_No.Focus();
            #endregion

            #region Grid初始化

            base.HeadGridDS = new BindingList<ReceiptBillManagerUIModel>();
            gdGrid.DataSource = base.HeadGridDS;
            gdGrid.DataBind();

            #endregion

            #region 初始化下拉框
            //收款方式【银行，微信，支付宝，现金】
            cbWhere_RB_ReceiveTypeName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_RB_ReceiveTypeName.ValueMember = SysConst.EN_Code;
            cbWhere_RB_ReceiveTypeName.DataSource = _receiveTypeList;
            cbWhere_RB_ReceiveTypeName.DataBind();

            //业务状态【已生成，执行中，已完成】
            cbWhere_RB_BusinessStatusName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_RB_BusinessStatusName.ValueMember = SysConst.EN_Code;
            cbWhere_RB_BusinessStatusName.DataSource = _receiptBillStatusList;
            cbWhere_RB_BusinessStatusName.DataBind();

            //审核状态【待审核，已审核】
            cbWhere_RB_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_RB_ApprovalStatusName.ValueMember = SysConst.EN_Code;
            cbWhere_RB_ApprovalStatusName.DataSource = _approvalStatusList;
            cbWhere_RB_ApprovalStatusName.DataBind();

            //条件付款对象
            mcbWhere_ARB_PayObjectName.ExtraDisplayMember = "ClientType";
            mcbWhere_ARB_PayObjectName.DisplayMember = "ClientName";
            mcbWhere_ARB_PayObjectName.ValueMember = "ClientID";
            mcbWhere_ARB_PayObjectName.DataSource = _tempAllClientList;

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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.FM_ReceiptBill.Code.RB_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.FM_ReceiptBill.Code.RB_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            HeadDS = base.HeadGridDS.FirstOrDefault(x => x.RB_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.FM_ReceiptBill.Code.RB_ID].Value);
            if (HeadDS == null || string.IsNullOrEmpty(HeadDS.RB_ID))
            {
                return;
            }

            if (txtRB_ID.Text != HeadDS.RB_ID
                || (txtRB_ID.Text == HeadDS.RB_ID && txtRB_VersionNo.Text != HeadDS.RB_VersionNo?.ToString()))
            {
                if (txtRB_ID.Text == HeadDS.RB_ID && txtRB_VersionNo.Text != HeadDS.RB_VersionNo?.ToString())
                {
                    //数据版本已过期，将加载最新详情。
                    MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.DataHasOverdue }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else if (ViewHasChanged()
                || _detailGridDS.InsertList.Count > 0
                || _detailGridDS.UpdateList.Count > 0
                || _detailGridDS.DeleteList.Count > 0)
                {
                    //将放弃之前的修改，是否继续？
                    DialogResult dialogResult = MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.ConfirmGiveUpEdit }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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
            
            //设置动作按钮状态
            SetActionEnableByStatus();
            //控制详情是否可编辑
            SetDetailControl();
            //设置图片是否可编辑
            SetPictureControl();

            //查询明细
            QueryDetail();
            //设置单元格是否可以编辑
            SetSalesOrderDetailStyle();
        }
        
        /// <summary>
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //收款ID
            txtRB_ID.Text = HeadDS.RB_ID;
            //收款单号
            txtRB_No.Text = HeadDS.RB_No;
            //收款组织ID
            txtRB_Rec_Org_ID.Text = HeadDS.RB_Rec_Org_ID;
            //收款组织名称
            txtRB_Rec_Org_Name.Text = HeadDS.RB_Rec_Org_Name;
            //付款对象类型
            mcbARB_PayObjectTypeName.SelectedValue = HeadDS.RB_PayObjectTypeCode;
            //付款对象名称
            if (!string.IsNullOrEmpty(HeadDS.RB_PayObjectID))
            {
                mcbARB_PayObjectName.SelectedValue = HeadDS.RB_PayObjectID;
            }
            else
            {
                mcbARB_PayObjectName.SelectedTextExtra = HeadDS.RB_PayObjectTypeName;
                mcbARB_PayObjectName.SelectedText = HeadDS.RB_PayObjectName;
            }
            //收款日期
            dtRB_Date.Value = HeadDS.RB_Date;
            //收款通道编码
            cbRB_ReceiveTypeName.Value = HeadDS.RB_ReceiveTypeCode;
            //收款通道名称
            cbRB_ReceiveTypeName.Text = HeadDS.RB_ReceiveTypeName ?? "";
            //收款凭证编号
            txtRB_CertificateNo.Text = HeadDS.RB_CertificateNo;
            //收款凭证图片
            if (!string.IsNullOrEmpty(HeadDS.RB_CertificatePic))
            {
                pbRB_CertificatePic.PictureKey = HeadDS.RB_CertificatePic;
                pbRB_CertificatePic.PictureImage = BLLCom.GetBitmapImageByFileName(HeadDS.RB_CertificatePic);
            }
            else
            {
                pbRB_CertificatePic.PictureKey = Guid.NewGuid() + ".jpg";
            }
            //合计金额
            numRB_ReceiveTotalAmount.Value = HeadDS.RB_ReceiveTotalAmount;
            //业务状态名称
            cbRB_BusinessStatusName.Text = HeadDS.RB_BusinessStatusName ?? "";
            //业务状态编码
            cbRB_BusinessStatusName.Value = HeadDS.RB_BusinessStatusCode;
            //审核状态名称
            cbRB_ApprovalStatusName.Text = HeadDS.RB_ApprovalStatusName ?? "";
            //审核状态编码
            cbRB_ApprovalStatusName.Value = HeadDS.RB_ApprovalStatusCode;
            //备注
            txtRB_Remark.Text = HeadDS.RB_Remark;
            //有效
            if (HeadDS.RB_IsValid != null)
            {
                ckRB_IsValid.Checked = HeadDS.RB_IsValid.Value;
            }
            //创建人
            txtRB_CreatedBy.Text = HeadDS.RB_CreatedBy;
            //创建时间
            dtRB_CreatedTime.Value = HeadDS.RB_CreatedTime;
            //修改人
            txtRB_UpdatedBy.Text = HeadDS.RB_UpdatedBy;
            //修改时间
            dtRB_UpdatedTime.Value = HeadDS.RB_UpdatedTime;
            //版本号
            txtRB_VersionNo.Value = HeadDS.RB_VersionNo;
            //钱包ID
            txtWal_ID.Text = HeadDS.Wal_ID;
            //钱包账号
            txtWal_No.Text = HeadDS.Wal_No;
            //钱包余额
            txtWal_AvailableBalance.Value = HeadDS.Wal_AvailableBalance;
            //钱包版本号
            txtWal_VersionNo.Value = HeadDS.Wal_VersionNo;
            txtRBD_SrcBillNo.Text = txtWhere_RBD_SrcBillNo.Text;
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
            #region 检查单头

            //付款对象类型不能为空
            if (string.IsNullOrEmpty(mcbARB_PayObjectTypeName.SelectedText))
            {
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[]
                {MsgParam.PAYOBJECT_TYPE}), MessageBoxButtons.OK, MessageBoxIcon.Information);
                mcbARB_PayObjectTypeName.Focus();
                return false;
            }
            //付款对象不能为空
            if (string.IsNullOrEmpty(mcbARB_PayObjectName.SelectedText))
            {
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[]
                {SystemTableColumnEnums.FM_ReceiptBill.Name.RB_PayObjectName}), MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                mcbARB_PayObjectName.Focus();
                return false;
            }
            //收款方式不能为空
            if (string.IsNullOrEmpty(cbRB_ReceiveTypeName.Text.Trim()))
            {
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[]
                {MsgParam.RECEIVE_TYPE}), MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbRB_ReceiveTypeName.Focus();
                return false;
            }
            if (cbRB_ReceiveTypeName.Text == TradeTypeEnum.Name.WALLET)
            {
                decimal receiveTotalAmount = Convert.ToDecimal(numRB_ReceiveTotalAmount.Value ?? 0);
                decimal availableBalance = Convert.ToDecimal(txtWal_AvailableBalance.Text.Trim() == "" ? "0" : txtWal_AvailableBalance.Text.Trim());
                if (receiveTotalAmount > availableBalance)
                {
                    MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "收款金额超出钱包金额，不能保存" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            //验证合计金额
            if (string.IsNullOrEmpty(numRB_ReceiveTotalAmount.Text.Trim()))
            {
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[]
                             { SystemTableColumnEnums.FM_ReceiptBill.Name.RB_ReceiveTotalAmount }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                numRB_ReceiveTotalAmount.Focus();
                return false;
            }

            #endregion

            #region 检查明细

            if (_detailGridDS.Count == 0)
            {
                //请至少添加一条收款单明细信息！
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0014, new object[] { SystemTableEnums.Name.FM_ReceiptBillDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            foreach (var loopDetailGrid in _detailGridDS)
            {
                if (loopDetailGrid.RBD_ReceiveAmount == null || loopDetailGrid.RBD_ReceiveAmount < 0)
                {
                    //收款单明细的收款金额不能为空！
                    MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[]
                            {SystemTableEnums.Name.FM_ReceiptBillDetail+MsgParam.OF+ SystemTableColumnEnums.FM_ReceiptBillDetail.Name.RBD_ReceiveAmount }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            #endregion

            return true;
        }

        /// <summary>
        /// 前端检查-删除
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForDelete()
        {
            if (string.IsNullOrEmpty(txtRB_ID.Text))
            {
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.W_0006, new object[]
                { SystemTableEnums.Name.FM_ReceiptBill, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //确认删除
            DialogResult dialogResult = MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.W_0012), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.OK)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            HeadDS = new ReceiptBillManagerUIModel()
            {
                //收款ID
                RB_ID = txtRB_ID.Text.Trim(),
                //收款组织ID
                RB_Rec_Org_ID = txtRB_Rec_Org_ID.Text.Trim(),
                //收款组织名称
                RB_Rec_Org_Name = txtRB_Rec_Org_Name.Text.Trim(),
                //收款单号
                RB_No = txtRB_No.Text.Trim(),
                //付款对象类型名称
                RB_PayObjectTypeName = mcbARB_PayObjectTypeName.SelectedText,
                //付款对象类型编码
                RB_PayObjectTypeCode = mcbARB_PayObjectTypeName.SelectedValue,
                //付款对象ID
                RB_PayObjectID = mcbARB_PayObjectName.SelectedValue,
                //付款对象
                RB_PayObjectName = mcbARB_PayObjectName.SelectedText,
                //收款日期
                RB_Date = (DateTime?)dtRB_Date.Value ?? DateTime.Now,
                //收款通道编码
                RB_ReceiveTypeCode = cbRB_ReceiveTypeName.Value?.ToString() ?? "",
                //收款通道名称
                RB_ReceiveTypeName = cbRB_ReceiveTypeName.Text,
                //收款凭证编号
                RB_CertificateNo = txtRB_CertificateNo.Text.Trim(),
                //收款凭证图片
                RB_CertificatePic = pbRB_CertificatePic.PictureImage == null ? "" : pbRB_CertificatePic.PictureKey,
                //合计金额
                RB_ReceiveTotalAmount = Convert.ToDecimal(numRB_ReceiveTotalAmount.Value ?? 0),
                //业务状态名称
                RB_BusinessStatusName = cbRB_BusinessStatusName.Text,
                //业务状态编码
                RB_BusinessStatusCode = cbRB_BusinessStatusName.Value?.ToString() ?? "",
                //审核状态名称
                RB_ApprovalStatusName = cbRB_ApprovalStatusName.Text,
                //审核状态编码
                RB_ApprovalStatusCode = cbRB_ApprovalStatusName.Value?.ToString() ?? "",
                //备注
                RB_Remark = txtRB_Remark.Text.Trim(),
                //有效
                RB_IsValid = ckRB_IsValid.Checked,
                //创建人
                RB_CreatedBy = txtRB_CreatedBy.Text.Trim(),
                //创建时间
                RB_CreatedTime = (DateTime?)dtRB_CreatedTime.Value ?? DateTime.Now,
                //修改人
                RB_UpdatedBy = txtRB_UpdatedBy.Text.Trim(),
                //修改时间
                RB_UpdatedTime = (DateTime?)dtRB_UpdatedTime.Value ?? DateTime.Now,
                //版本号
                RB_VersionNo = Convert.ToInt64(txtRB_VersionNo.Text.Trim() == "" ? "1" : txtRB_VersionNo.Text.Trim()),
                //钱包ID
                Wal_ID = txtWal_ID.Text,
                //钱包账号
                Wal_No = txtWal_No.Text,
                //钱包余额
                Wal_AvailableBalance = Convert.ToDecimal(txtWal_AvailableBalance.Text.Trim() == "" ? "0" : txtWal_AvailableBalance.Text.Trim()),
                //钱包版本号
                Wal_VersionNo = Convert.ToInt64(txtWal_VersionNo.Text.Trim() == "" ? "1" : txtWal_VersionNo.Text.Trim()),
            };
        }

        /// <summary>
        /// 设置详情页面控件是否可编辑
        /// </summary>
        private void SetDetailControl()
        {
            //根据[是否存在明细]控制单头是否可编辑
            SetDetailByIsExistDetail();
            if (string.IsNullOrEmpty(txtRB_ID.Text.Trim()))
            {
                #region 收款单未保存

                //收款方式
                cbRB_ReceiveTypeName.Enabled = true;
                //收款凭证编号
                txtRB_CertificateNo.Enabled = true;
                //备注
                txtRB_Remark.Enabled = true;
                //明细列表可添加、删除
                toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedProps.Enabled = true;
                toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = true;
                
                #endregion
            }
            else
            {
                #region 收款单已保存

                if (cbRB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
                {
                    #region 收款单已审核的场合，详情不可编辑

                    //付款对象
                    mcbARB_PayObjectName.Enabled = false;
                    //收款方式
                    cbRB_ReceiveTypeName.Enabled = false;
                    //收款凭证编号
                    txtRB_CertificateNo.Enabled = false;
                    //备注
                    txtRB_Remark.Enabled = false;
                    //明细列表不可添加、删除
                    toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedProps.Enabled = false;
                    toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = false;
                   
                    #endregion
                }
                else
                {
                    #region 收款单待审核的场合，详情可编辑

                    //付款对象
                    mcbARB_PayObjectName.Enabled = true;
                    //收款方式
                    cbRB_ReceiveTypeName.Enabled = true;
                    //收款凭证编号
                    txtRB_CertificateNo.Enabled = true;
                    //备注
                    txtRB_Remark.Enabled = true;
                    //明细列表不可添加、删除
                    toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedProps.Enabled = true;
                    toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = true;
                    
                    #endregion
                }
                #endregion
            }
        }

        /// <summary>
        /// 设置动作按钮状态
        /// </summary>
        private void SetActionEnableByStatus()
        {
            if (cbRB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                //[审核状态]为[已审核]的场合，[保存]、[删除]、[审核]不可用，[打印]可用
                SetActionEnable(SystemActionEnum.Code.SAVE, false);
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                SetActionEnable(SystemActionEnum.Code.APPROVE, false);
                SetActionEnable(SystemActionEnum.Code.PRINT, true);
            }
            else
            {
                //[审核状态]为[待审核]的场合，[打印]不可用
                //新增的场合，[删除]、[审核]不可用
                SetActionEnable(SystemActionEnum.Code.SAVE, true);
                SetActionEnable(SystemActionEnum.Code.DELETE, !string.IsNullOrEmpty(txtRB_ID.Text));
                SetActionEnable(SystemActionEnum.Code.APPROVE, !string.IsNullOrEmpty(txtRB_ID.Text));
                SetActionEnable(SystemActionEnum.Code.PRINT, false);
            }
        }

        /// <summary>
        /// 设置单元格是否可以编辑
        /// </summary>
        private void SetSalesOrderDetailStyle()
        {
            #region 设置Grid数据颜色
            gdDetail.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;

            foreach (var loopGridRow in gdDetail.Rows)
            {
                if (cbRB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
                {
                    #region 收款单已审核的场合

                    #region 收款金额

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.FM_ReceiptBillDetail.Code.RBD_ReceiveAmount].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.FM_ReceiptBillDetail.Code.RBD_ReceiveAmount].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.FM_ReceiptBillDetail.Code.RBD_ReceiveAmount].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;
                    #endregion

                    #region 备注

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.FM_ReceiptBillDetail.Code.RBD_Remark].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.FM_ReceiptBillDetail.Code.RBD_Remark].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.FM_ReceiptBillDetail.Code.RBD_Remark].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;
                    #endregion
                    #endregion
                }
                else
                {
                    #region 收款单未保存或待审核的场合

                    #region 收款金额

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.FM_ReceiptBillDetail.Code.RBD_ReceiveAmount].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.FM_ReceiptBillDetail.Code.RBD_ReceiveAmount].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.FM_ReceiptBillDetail.Code.RBD_ReceiveAmount].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;
                    #endregion

                    #region 备注

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.FM_ReceiptBillDetail.Code.RBD_Remark].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.FM_ReceiptBillDetail.Code.RBD_Remark].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.FM_ReceiptBillDetail.Code.RBD_Remark].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;
                    #endregion
                    #endregion
                }
            }
            #endregion
        }
        
        #region 明细相关事件

        /// <summary>
        /// 查询收款单明细
        /// </summary>
        private void QueryDetail()
        {
            //销售订单明细列表
            _bll.QueryForList<ReceiptBillManagerDetailUIModel>(new MDLFM_ReceiptBillDetail
            {
                WHERE_RBD_RB_ID = txtRB_ID.Text.Trim(),
                WHERE_RBD_RB_No = txtRB_No.Text.Trim()
            }, _detailGridDS);

            //3.开始监控List变化（该数据源若有其他调整，请在StartMonitChanges()方法执行前调整）
            _detailGridDS.StartMonitChanges();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();

            //设置销售订单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }
        /// <summary>
        /// 添加明细
        /// </summary>
        private void AddReceiptDetail()
        {
            #region 验证

            //提醒明细数量
            if (gdDetail.Rows.Count >= 25 && gdDetail.Rows.Count % 25 == 0)
            {
                DialogResult isAddContinueResult = MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.W_0020, new object[] { gdDetail.Rows.Count }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (isAddContinueResult != DialogResult.OK)
                {
                    return;
                }
            }
            if (string.IsNullOrEmpty(mcbARB_PayObjectName.SelectedText))
            {
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.FM_ReceiptBill.Name.RB_PayObjectName }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #endregion

            //添加一条明细
            ReceiptBillManagerDetailUIModel addAccountReceivableBillDetail = new ReceiptBillManagerDetailUIModel
            {
                IsChecked = false,
                RBD_RB_ID = txtRB_ID.Text.Trim(),
                RBD_RB_No = txtRB_No.Text.Trim(),
                RBD_SourceTypeCode = ReceiptBillDetailSourceTypeEnum.Code.SGSK,
                RBD_SourceTypeName = ReceiptBillDetailSourceTypeEnum.Name.SGSK,
                RBD_IsValid = true,
                RBD_CreatedBy = LoginInfoDAX.UserName,
                RBD_CreatedTime = BLLCom.GetCurStdDatetime(),
                RBD_UpdatedBy = LoginInfoDAX.UserName,
                RBD_UpdatedTime = BLLCom.GetCurStdDatetime(),
            };
            _detailGridDS.Add(addAccountReceivableBillDetail);

            //根据[是否存在明细]控制单头是否可编辑
            SetDetailByIsExistDetail();

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
            //设置单元格是否可以编辑
            SetSalesOrderDetailStyle();
        }
        /// <summary>
        /// 删除明细
        /// </summary>
        private void DeleteReceiptDetail()
        {
            #region 验证

            //验证付款单的审核状态，[审核状态]为[已审核]的付款单不能删除明细
            if (cbRB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableEnums.Name.FM_PayBill + ApprovalStatusEnum.Name.YSH, SystemActionEnum.Name.DELETE + SystemTableEnums.Name.FM_ReceiptBillDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gdDetail.UpdateData();
            //待删除的付款单明细列表
            var deleteAccountReceivableBillDetailList = _detailGridDS.Where(p => p.IsChecked == true).ToList();
            if (deleteAccountReceivableBillDetailList.Count == 0)
            {
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { SystemTableEnums.Name.FM_PayBill, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion

            //移除勾选项
            foreach (var loopAccountReceivableDetail in deleteAccountReceivableBillDetailList)
            {
                _detailGridDS.Remove(loopAccountReceivableDetail);
            }

            //根据[是否存在明细]控制单头是否可编辑
            SetDetailByIsExistDetail();

            //计算总金额
            decimal totalPayAmount = 0;
            foreach (var loopDetailGrid in _detailGridDS)
            {
                totalPayAmount = totalPayAmount + (loopDetailGrid.RBD_ReceiveAmount ?? 0);
            }
            numRB_ReceiveTotalAmount.Text = Convert.ToString(totalPayAmount);
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置付款单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }
        /// <summary>
        /// 根据[是否存在明细]控制单头是否可编辑
        /// </summary>
        private void SetDetailByIsExistDetail()
        {
            if (_detailGridDS.Count == 0)
            {
                mcbARB_PayObjectTypeName.Enabled = true;
                mcbARB_PayObjectName.Enabled = true;
            }
            else
            {
                mcbARB_PayObjectTypeName.Enabled = false;
                mcbARB_PayObjectName.Enabled = false;
            }
        }

        #endregion

        #region 图片相关方法

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="paramPictureKey">图片唯一标识</param>
        /// <param name="paramInventoryPicture">库存图片</param>
        /// <returns></returns>
        private object UploadPicture(string paramPictureKey, object paramInventoryPicture = null)
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

                if (!_dicPictureNameAndTempPath.ContainsKey(paramPictureKey))
                {
                    _dicPictureNameAndTempPath.Add(paramPictureKey, tempFileName);
                }
                else
                {
                    _dicPictureNameAndTempPath[paramPictureKey] = tempFileName;
                }

                //设置图片是否可见、可编辑
                SetPictureControl();

                return curImage;
            }
            catch (Exception ex)
            {
                //上传图片失败
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0018, new object[] { MsgParam.UPLOAD_IMAGE, ex.Message }), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (_dicPictureNameAndTempPath == null || _dicPictureNameAndTempPath.Count == 0)
            {
                isDownFromWeb = true;
            }
            else
            {
                tempLocalPicturePath = _dicPictureNameAndTempPath[paramPictureKey];
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
                MessageBoxs.Show(Trans.SD, ToString(),
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
                    MessageBoxs.Show(Trans.SD, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (string.IsNullOrEmpty(outMsg))
                {
                    return true;
                }
                //导出成功
                MessageBoxs.Show(Trans.SD, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return true;
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="paramPictureKey"></param>
        /// <param name="paramInventoryPicture">库存图片</param>
        /// <returns></returns>
        private bool DeletePicture(string paramPictureKey, object paramInventoryPicture = null)
        {
            //查询图片是否已保存
            MDLFM_ReceiptBill resultReceiptBill = new MDLFM_ReceiptBill();
            _bll.QueryForObject<MDLFM_ReceiptBill, MDLFM_ReceiptBill>(new MDLFM_ReceiptBill()
            {
                WHERE_RB_CertificatePic = paramPictureKey,
                WHERE_RB_IsValid = true
            }, resultReceiptBill);
            if (!string.IsNullOrEmpty(resultReceiptBill.RB_ID))
            {
                //错误消息
                var outMsg = string.Empty;
                //删除本地和文件服务器中的图片
                bool deleteResult = BLLCom.DeleteFileByFileName(paramPictureKey, ref outMsg);
                if (deleteResult == false)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.FM, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                //更新收款单
                resultReceiptBill.WHERE_RB_ID = resultReceiptBill.RB_ID;
                resultReceiptBill.WHERE_RB_VersionNo = resultReceiptBill.RB_VersionNo;
                if (resultReceiptBill.RB_CertificatePic == paramPictureKey)
                {
                    resultReceiptBill.RB_CertificatePic = null;
                    pbRB_CertificatePic.PictureKey = Guid.NewGuid() + ".jpg";
                }

                bool updateReceiptBillResult = _bll.Update(SQLID.FM_ReceiptBillManager_SQL02, resultReceiptBill);
                if (!updateReceiptBillResult)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.FM, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                //删除成功
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (_dicPictureNameAndTempPath.ContainsKey(paramPictureKey))
                {
                    _dicPictureNameAndTempPath.Remove(paramPictureKey);
                }

                //更新版本号
                txtRB_VersionNo.Text = ((resultReceiptBill.RB_VersionNo ?? 0) + 1).ToString();
            }

            return true;
        }

        /// <summary>
        /// 设置图片是否可见、可编辑
        /// </summary>
        private void SetPictureControl()
        {
            if (toolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[SystemActionEnum.Code.SAVE].SharedPropsInternal.Enabled
                || toolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[SystemActionEnum.Code.APPROVE].SharedPropsInternal.Enabled)
            {
                //付款单可保存、审核的场合，可上传图片
                //有图片的场合，删除可见
                pbRB_CertificatePic.UploadIsVisible = true;
                pbRB_CertificatePic.DeleteIsVisible = pbRB_CertificatePic.PictureImage != null;

            }
            else
            {
                //库存图片不可保存、审核的场合，不可上传、删除图片
                pbRB_CertificatePic.UploadIsVisible = false;
                pbRB_CertificatePic.DeleteIsVisible = false;
            }
            //有图片的场合，导出可见
            pbRB_CertificatePic.ExportIsVisible = pbRB_CertificatePic.PictureImage != null;
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
                    var removeList = HeadGridDS.Where(x => x.IsChecked == true).ToList();
                    foreach (var loopRemove in removeList)
                    {
                        HeadGridDS.Remove(loopRemove);
                    }
                }
                else
                {
                    var curHead = HeadGridDS.FirstOrDefault(x => x.RB_ID == HeadDS.RB_ID);
                    if (curHead != null)
                    {
                        HeadGridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = HeadGridDS.FirstOrDefault(x => x.RB_ID == HeadDS.RB_ID);
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
