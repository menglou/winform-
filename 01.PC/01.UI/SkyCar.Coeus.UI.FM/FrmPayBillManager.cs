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
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.FM;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;
using System.Net;
using Infragistics.Win.UltraWinTabControl;
using SkyCar.Coeus.UIModel.Common.UIModel;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.DAL;

namespace SkyCar.Coeus.UI.FM
{
    /// <summary>
    /// 付款单管理
    /// </summary>
    public partial class FrmPayBillManager : BaseFormCardListDetail<PayBillManagerUIModel, PayBillManagerQCModel, MDLFM_PayBill>
    {
        #region 全局变量

        /// <summary>
        /// 付款单管理BLL
        /// </summary>
        private PayBillManagerBLL _bll = new PayBillManagerBLL();
        /// <summary>
        /// 付款单明细
        /// </summary>
        private SkyCarBindingList<PayBillManagerDetailUIModel, MDLFM_PayBillDetail> _detailGridDS = new SkyCarBindingList<PayBillManagerDetailUIModel, MDLFM_PayBillDetail>();

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
        /// 收款对象类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _amountTransObjectTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 付款方式
        /// </summary>
        List<ComComboBoxDataSourceTC> _paymentChannelList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 付款单业务状态
        /// </summary>
        List<ComComboBoxDataSourceTC> _payBillStatusList = new List<ComComboBoxDataSourceTC>();
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
        /// FrmPayBillManager构造方法
        /// </summary>
        public FrmPayBillManager()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// FrmPayBillManager构造方法
        /// </summary>
        public FrmPayBillManager(Dictionary<string, object> paramWindowParameters)
        {
            InitializeComponent();

            _viewParameters = paramWindowParameters;
        }
        
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmPayBillManager_Load(object sender, EventArgs e)
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

            if (_viewParameters != null)
            {
                #region 付款单

                if (_viewParameters.ContainsKey(ComViewParamKey.BillNo.ToString()))
                {
                    string billNo = _viewParameters[ComViewParamKey.BillNo.ToString()] as string;

                    if (billNo != null)
                    {
                        #region 加载列表tab

                        txtWhere_PB_No.Text = billNo;
                        QueryAction();

                        #endregion

                        #region 加载详情tab

                        if (HeadGridDS.Count == 1)
                        {
                            //付款单号
                            txtPB_No.Text = HeadGridDS[0].PB_No;
                            //审核状态
                            cbPB_ApprovalStatusName.Text = HeadGridDS[0].PB_ApprovalStatusName ?? string.Empty;
                            cbPB_ApprovalStatusName.Value = HeadGridDS[0].PB_ApprovalStatusCode;
                            //付款组织
                            txtPB_Pay_Org_Name.Text = HeadGridDS[0].PB_Pay_Org_Name;
                            //应付合计金额
                            //txtPB_PayableTotalAmount.Value = HeadGridDS[0].PB_PayableTotalAmount;
                            //付款方式
                            cbPB_PayTypeName.Text = HeadGridDS[0].PB_PayTypeName ?? string.Empty;
                            cbPB_PayTypeName.Value = HeadGridDS[0].PB_PayTypeCode;
                            //实付合计金额
                            numPB_RealPayableTotalAmount.Value = HeadGridDS[0].PB_RealPayableTotalAmount;
                            //付款日期
                            dtPB_Date.Value = HeadGridDS[0].PB_Date;
                            //付款凭证图片
                            if (!string.IsNullOrEmpty(HeadGridDS[0].PB_CertificatePic))
                            {
                                pbPB_CertificatePic.PictureKey = HeadGridDS[0].PB_CertificatePic;
                                pbPB_CertificatePic.PictureImage = BLLCom.GetBitmapImageByFileName(HeadGridDS[0].PB_CertificatePic);
                            }
                            else
                            {
                                pbPB_CertificatePic.PictureKey = Guid.NewGuid() + ".jpg";
                            }
                            //业务状态名称
                            cbPB_BusinessStatusName.Text = HeadGridDS[0].PB_BusinessStatusName ?? string.Empty;
                            cbPB_BusinessStatusName.Value = HeadGridDS[0].PB_BusinessStatusCode;
                            //收款对象类型
                            mcbAPB_ReceiveObjectTypeName.SelectedValue = HeadGridDS[0].PB_RecObjectTypeCode;
                            //收款对象
                            if (!string.IsNullOrEmpty(HeadGridDS[0].PB_RecObjectID))
                            {
                                mcbAPB_ReceiveObjectName.SelectedValue = HeadGridDS[0].PB_RecObjectID;
                            }
                            else
                            {
                                mcbAPB_ReceiveObjectName.SelectedTextExtra = HeadGridDS[0].PB_RecObjectTypeName;
                                mcbAPB_ReceiveObjectName.SelectedText = HeadGridDS[0].PB_RecObjectName;
                            }
                            //有效
                            if (HeadGridDS[0].PB_IsValid != null)
                            {
                                ckPB_IsValid.Checked = HeadGridDS[0].PB_IsValid.Value;
                            }
                            //备注
                            txtPB_Remark.Text = HeadGridDS[0].PB_Remark;
                            //创建人
                            txtPB_CreatedBy.Text = HeadGridDS[0].PB_CreatedBy;
                            //创建时间
                            dtPB_CreatedTime.Value = HeadGridDS[0].PB_CreatedTime;
                            //修改人
                            txtPB_UpdatedBy.Text = HeadGridDS[0].PB_UpdatedBy;
                            //修改时间
                            dtPB_UpdatedTime.Value = HeadGridDS[0].PB_UpdatedTime;
                            //付款凭证编号
                            txtPB_CertificateNo.Text = HeadGridDS[0].PB_CertificateNo;
                            //付款ID
                            txtPB_ID.Text = HeadGridDS[0].PB_ID;
                            //付款组织ID
                            txtPB_Pay_Org_ID.Text = HeadGridDS[0].PB_Pay_Org_ID;
                            //收款对象ID
                            txtPB_RecObjectID.Text = HeadGridDS[0].PB_RecObjectID;
                            //版本号
                            txtPB_VersionNo.Value = HeadGridDS[0].PB_VersionNo;
                            //钱包ID
                            txtWal_ID.Text = HeadGridDS[0].Wal_ID;
                            //钱包账号
                            txtWal_No.Text = HeadGridDS[0].Wal_No;
                            //钱包版本号
                            txtWal_VersionNo.Value = HeadGridDS[0].Wal_VersionNo;
                            //钱包余额
                            txtWal_AvailableBalance.Value = HeadGridDS[0].Wal_AvailableBalance;
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
                        //设置付款单明细单元格样式
                        SetPayBillStyle();
                    }
                }
                #endregion
            }
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
            decimal realPayableTotalAmount = 0;

            foreach (var loopGridRow in gdGrid.Rows)
            {
                if (string.IsNullOrEmpty(loopGridRow.Cells[SystemTableColumnEnums.FM_PayBill.Code.PB_No].Value?.ToString())
                    || loopGridRow.Cells["IsChecked"].Value == null
                    || (bool)loopGridRow.Cells["IsChecked"].Value == false)
                {
                    continue;
                }
                realPayableTotalAmount = realPayableTotalAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_PayBill.Code.PB_RealPayableTotalAmount].Value?.ToString());
            }
            
            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtRealPayableTotalAmount"])).Text = Convert.ToString(realPayableTotalAmount);
        }
       
        /// <summary>
        /// 【列表】Grid的AfterHeaderCheckStateChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            gdGrid.UpdateData();
            decimal realPayableTotalAmount = 0;

            foreach (var loopGridRow in gdGrid.Rows)
            {
                if (string.IsNullOrEmpty(loopGridRow.Cells[SystemTableColumnEnums.FM_PayBill.Code.PB_No].Value?.ToString())
                    || loopGridRow.Cells["IsChecked"].Value == null
                    || (bool)loopGridRow.Cells["IsChecked"].Value == false)
                {
                    continue;
                }
                realPayableTotalAmount = realPayableTotalAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_PayBill.Code.PB_RealPayableTotalAmount].Value?.ToString());
            }

            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtRealPayableTotalAmount"])).Text = Convert.ToString(realPayableTotalAmount);
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
        /// 付款单号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_PB_No_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 审核状态ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_PB_ApprovalStatusName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }

        /// <summary>
        /// 收款对象KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_PB_RecObjectName_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 付款方式ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_PB_PayTypeName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 有效CheckedChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckWhere_PB_IsValid_CheckedChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }

        /// <summary>
        /// 列表查询条件dtWhere_PB_DateEnd_ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_PB_DateEnd_ValueChanged(object sender, EventArgs e)
        {
            if (this.dtWhere_PB_DateEnd.Value != null &&
              this.dtWhere_PB_DateEnd.DateTime.Hour == 0 &&
              this.dtWhere_PB_DateEnd.DateTime.Minute == 0 &&
              this.dtWhere_PB_DateEnd.DateTime.Second == 0)
            {
                DateTime newDateTime = new DateTime(this.dtWhere_PB_DateEnd.DateTime.Year, this.dtWhere_PB_DateEnd.DateTime.Month, this.dtWhere_PB_DateEnd.DateTime.Day, 23, 59, 59);
                this.dtWhere_PB_DateEnd.DateTime = newDateTime;
            }
        }
        #endregion

        #region 详情相关事件

        /// <summary>
        /// [收款对象]SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbAPB_ReceiveObjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //根据选择的对象选中对象类型
            if (!string.IsNullOrEmpty(mcbAPB_ReceiveObjectName.SelectedTextExtra))
            {
                mcbAPB_ReceiveObjectTypeName.SelectedText = mcbAPB_ReceiveObjectName.SelectedTextExtra;
            }
            else if (!string.IsNullOrEmpty(mcbAPB_ReceiveObjectName.SelectedText))
            {
                mcbAPB_ReceiveObjectTypeName.SelectedText = "其他";
            }
        }

        /// <summary>
        /// [收款对象类型]SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbAPB_ReceiveObjectTypeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curClientType = mcbAPB_ReceiveObjectName.SelectedTextExtra;
            if (curClientType != mcbAPB_ReceiveObjectTypeName.SelectedText)
            {
                _clientList.Clear();
                if (string.IsNullOrEmpty(mcbAPB_ReceiveObjectTypeName.SelectedText))
                {
                    _bll.CopyModelList(_tempAllClientList, _clientList);
                }
                else
                {
                    foreach (var loopTempClient in _tempAllClientList)
                    {
                        if (loopTempClient.ClientType == mcbAPB_ReceiveObjectTypeName.SelectedText)
                        {
                            _clientList.Add(loopTempClient);
                        }
                    }
                }
                mcbAPB_ReceiveObjectName.DataSource = null;
                mcbAPB_ReceiveObjectName.DataSource = _clientList;
                mcbAPB_ReceiveObjectName.Clear();
            }
            else if (string.IsNullOrEmpty(mcbAPB_ReceiveObjectTypeName.SelectedText))
            {
                _bll.CopyModelList(_tempAllClientList, _clientList);
                mcbAPB_ReceiveObjectName.DataSource = _clientList;
                mcbAPB_ReceiveObjectName.Clear();
            }
        }

        /// <summary>
        /// [钱包ID]ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWal_ID_ValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtWal_ID.Text.Trim()))
            {
                txtWal_No.Visible = false;
                lblWal_No.Visible = false;
                //【收款方式】绑定数据源
                var receiveTypeList = _paymentChannelList.Where(x => x.Text != TradeTypeEnum.Name.WALLET).ToList();
                cbPB_PayTypeName.DisplayMember = SysConst.EN_TEXT;
                cbPB_PayTypeName.ValueMember = SysConst.EN_Code;
                cbPB_PayTypeName.DataSource = receiveTypeList;
                cbPB_PayTypeName.DataBind();
            }
            else
            {
                //【收款方式】绑定数据源
                cbPB_PayTypeName.DisplayMember = SysConst.EN_TEXT;
                cbPB_PayTypeName.ValueMember = SysConst.EN_Code;
                cbPB_PayTypeName.DataSource = _paymentChannelList;
                cbPB_PayTypeName.DataBind();

                if (cbPB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
                {
                    txtWal_No.Visible = false;
                    lblWal_No.Visible = false;
                }
                else
                {
                    txtWal_No.Visible = true;
                    lblWal_No.Visible = true;
                }
            }
        }

        /// <summary>
        /// 明细来源单号_TextChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPBD_SrcBillNo_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPBD_SrcBillNo.Text))
            {
                lblPBD_SrcBillNo.Visible = false;
                txtPBD_SrcBillNo.Visible = false;
            }
            else
            {
                lblPBD_SrcBillNo.Visible = true;
                txtPBD_SrcBillNo.Visible = true;
            }
        }

        #endregion

        #region 明细ToolBar事件

        /// <summary>
        /// 添加/删除明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolbarsManagerDetail_ToolClick(object sender, ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                //添加
                case SysConst.EN_ADD:
                    AddPayDetail();
                    break;

                //删除
                case SysConst.EN_DEL:
                    DeletePayDetail();
                    break;
            }
        }

        #endregion

        #region 明细Grid事件

        /// <summary>
        /// 明细Grid改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_CellChange(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.Key == SystemTableColumnEnums.FM_PayBillDetail.Code.PBD_PayAmount)
            {
                //计算总金额
                decimal totalPayAmount = 0;
                gdDetail.UpdateData();
                foreach (var loopDetailGrid in _detailGridDS)
                {
                    totalPayAmount = totalPayAmount + (loopDetailGrid.PBD_PayAmount ?? 0);
                }
                numPB_RealPayableTotalAmount.Text = Convert.ToString(totalPayAmount);
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

            //6. 设置动作按钮状态
            SetActionEnableByStatus();
            //7.设置详情页面控件的是否可编辑
            SetDetailControl();
            //设置图片是否可编辑
            SetPictureControl();
            //设置付款单明细单元格样式
            SetPayBillStyle();

            //将最新的值Copy到初始UIModel
            SetCardCtrlsToDetailDS();
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

            //6. 设置动作按钮状态
            SetActionEnableByStatus();
            //7.设置详情页面控件的是否可编辑
            SetDetailControl();
            //设置图片是否可编辑
            SetPictureControl();

            //设置图片Key
            pbPB_CertificatePic.PictureKey = !string.IsNullOrEmpty(HeadDS.PB_CertificatePic) ? HeadDS.PB_CertificatePic : (Guid.NewGuid() + ".jpg");

            //清除图片临时路径
            _dicPictureNameAndTempPath.Clear();
            //设置付款单明细单元格样式
            SetPayBillStyle();
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
            var argsDetail = new List<MDLFM_PayBillDetail>();
            //将DetailDS转换为TBModel对象
            var argsHead = HeadDS.ToTBModelForSaveAndDelete<MDLFM_PayBill>();
            //将当前DetailGridDS转换为指定类型的TBModelList
            _detailGridDS.ToTBModelListForUpdateAndDelete<MDLFM_PayBillDetail>(argsDetail);
            //过滤明细列表中未保存的数据
            argsDetail = argsDetail.Where(x => !string.IsNullOrEmpty(x.WHERE_PBD_ID)).ToList();
            //2.执行删除
            bool deleteResult = _bll.UnityDelete<MDLFM_PayBill, MDLFM_PayBillDetail>(argsHead, argsDetail);
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

            //将最新的值Copy到初始UIModel
            SetCardCtrlsToDetailDS();
            this.AcceptUIModelChanges();

            #region 删除本地和文件服务器中的图片

            //错误消息
            var outMsg = string.Empty;
            //删除本地和文件服务器中的图片
            BLLCom.DeleteFileByFileName(argsHead.PB_CertificatePic, ref outMsg);

            _dicPictureNameAndTempPath.Clear();
            #endregion
        }
        
        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            //1.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
            //2.设置查询条件（翻页相关属性不用设置）
            base.ConditionDS = new PayBillManagerQCModel()
            {
                SqlId = SQLID.FM_PayBillManager_SQL02,
                //付款单号
                WHERE_PB_No = txtWhere_PB_No.Text.Trim(),
                //审核状态
                WHERE_PB_ApprovalStatusName = cbWhere_PB_ApprovalStatusName.Text.Trim(),
                //业务状态
                WHERE_PB_BusinessStatusName = cbWhere_PB_BusinessStatusName.Text.Trim(),
                //收款对象类型
                //WHERE_PB_RecObjectTypeName = mcbAPB_ReceiveObjectTypeName.SelectedText,
                //收款对象
                WHERE_PB_RecObjectName = mcbWhere_ARB_PayObjectName.SelectedText,
                //付款方式
                WHERE_PB_PayTypeName = cbWhere_PB_PayTypeName.Text.Trim(),
                //付款组织ID
                WHERE_PB_Pay_Org_ID = LoginInfoDAX.OrgID,
                //明细来源单号
                WHERE_PBD_SrcBillNo = txtWhere_PBD_SrcBillNo.Text,
            };
            if (dtWhere_PB_DateStart.Value != null)
            {
                //收款时间-开始
                ConditionDS._DateStart = dtWhere_PB_DateStart.DateTime;
            }
            if (dtWhere_PB_DateEnd.Value != null)
            {
                //收款时间-终了
                ConditionDS._DateEnd = dtWhere_PB_DateEnd.DateTime;
            }
            //3.执行基类方法（注意：基类使用模糊查询）
            base.QueryAction();
            //4.Grid绑定数据源
            gdGrid.DataSource = HeadGridDS;
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
            gdDetail.UpdateData();
            if (!ClientCheckForApprove())
            {
                return;
            }

            SetCardCtrlsToDetailDS();

            #region 审核收款单
            bool saveApprove = _bll.ApprovePayBill(HeadDS, _detailGridDS);
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
            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置动作按钮状态
            SetActionEnableByStatus();
            //设置详情页面控件的是否可编辑
            SetDetailControl();
            //设置图片是否可编辑
            SetPictureControl();

            //设置图片Key
            pbPB_CertificatePic.PictureKey = !string.IsNullOrEmpty(HeadDS.PB_CertificatePic) ? HeadDS.PB_CertificatePic : (Guid.NewGuid() + ".jpg");
            //清除图片临时路径
            _dicPictureNameAndTempPath.Clear();
            //设置付款单明细单元格样式
            SetPayBillStyle();
        }
        
        /// <summary>
        /// 打印
        /// </summary>
        public override void PrintAction()
        {
            base.PrintAction();

            if (HeadDS == null || string.IsNullOrWhiteSpace(HeadDS.PB_No))
            {
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, SystemTableEnums.Name.FM_PayBill), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (HeadDS.PB_ApprovalStatusName != ApprovalStatusEnum.Name.YSH)
            {
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, SystemTableEnums.Name.FM_PayBill + cbPB_ApprovalStatusName.Text, SystemActionEnum.Name.PRINT), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                //待打印的付款单
                PayBillUIModelToPrint payBillToPrint = new PayBillUIModelToPrint();
                _bll.CopyModel(HeadDS, payBillToPrint);
                //待打印的付款单明细
                List<PayBillDetailUIModelToPrint> payBillDetailToPrintList = new List<PayBillDetailUIModelToPrint>();
                _bll.CopyModelList(_detailGridDS, payBillDetailToPrintList);
                Dictionary<string, object> argsViewParams = new Dictionary<string, object>
                {
                    //付款单
                    {FMViewParamKey.PayBill.ToString(), payBillToPrint},
                    //付款单明细
                    {FMViewParamKey.PayBillDetail.ToString(), payBillDetailToPrintList},
                };

                FrmViewAndPrintPayBill frmViewAndPrintPayBill = new FrmViewAndPrintPayBill(argsViewParams)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = frmViewAndPrintPayBill.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBoxs.Show(Trans.FM, ToString(), ex.Message.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            //付款单号
            txtPB_No.Clear();
            //审核状态
            cbPB_ApprovalStatusName.Value = null;
            //付款组织
            txtPB_Pay_Org_Name.Clear();
            //付款方式
            cbPB_PayTypeName.Value = null;
            //实付合计金额
            numPB_RealPayableTotalAmount.Value = null;
            //付款日期
            dtPB_Date.Value = DateTime.Now;
            //付款凭证图片
            pbPB_CertificatePic.PictureImage = null;
            //业务状态名称
            cbPB_BusinessStatusName.Value = null;
            //收款对象类型
            mcbAPB_ReceiveObjectTypeName.Clear();
            //收款对象
            mcbAPB_ReceiveObjectName.Clear();
            //有效
            ckPB_IsValid.Checked = true;
            ckPB_IsValid.CheckState = CheckState.Checked;
            //备注
            txtPB_Remark.Clear();
            //创建人
            txtPB_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtPB_CreatedTime.Value = DateTime.Now;
            //修改人
            txtPB_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtPB_UpdatedTime.Value = DateTime.Now;
            //付款ID
            txtPB_ID.Clear();
            //付款凭证编号
            txtPB_CertificateNo.Clear();
            //付款组织ID
            txtPB_Pay_Org_ID.Clear();
            //收款对象ID
            txtPB_RecObjectID.Clear();
            //版本号
            txtPB_VersionNo.Clear();
            //给 付款单号 设置焦点
            lblPB_No.Focus();
            //钱包ID
            txtWal_ID.Clear();
            //钱包账号
            txtWal_No.Clear();
            //钱包版本号
            txtWal_VersionNo.Clear();
            //钱包余额
            txtWal_AvailableBalance.Clear();
            //明细来源单号
            txtPBD_SrcBillNo.Clear();

            //付款组织为当前登录组织
            txtPB_Pay_Org_ID.Text = LoginInfoDAX.OrgID;
            txtPB_Pay_Org_Name.Text = LoginInfoDAX.OrgShortName;

            //初始化明细列表
            _detailGridDS = new SkyCarBindingList<PayBillManagerDetailUIModel, MDLFM_PayBillDetail>();
            _detailGridDS.StartMonitChanges();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();

            #region 初始化图片

            //付款凭证图片
            pbPB_CertificatePic.PictureKey = Guid.NewGuid() + ".jpg";
            pbPB_CertificatePic.ExecuteUpload = UploadPicture;
            pbPB_CertificatePic.ExecuteExport = ExportPicture;
            pbPB_CertificatePic.ExecuteDelete = DeletePicture;

            #endregion

            //下拉框初始化
            SetToComboEditor();
        }
        
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 查询条件初始化
            //付款单号
            txtWhere_PB_No.Clear();
            //审核状态
            cbWhere_PB_ApprovalStatusName.Items.Clear();
            //收款对象类型
            mcbAPB_ReceiveObjectTypeName.Clear();
            //收款对象
            mcbAPB_ReceiveObjectName.Clear();
            //付款方式
            cbWhere_PB_PayTypeName.Items.Clear();
            //付款日期-开始
            dtWhere_PB_DateStart.Value = null;
            //付款日期-终了
            dtWhere_PB_DateEnd.Value = null;
            //付款单来源单号
            txtWhere_PBD_SrcBillNo.Clear();
            //给 付款单号 设置焦点
            lblWhere_PB_No.Focus();
            #endregion

            #region Grid初始化

            HeadGridDS = new BindingList<PayBillManagerUIModel>();
            gdGrid.DataSource = HeadGridDS;
            gdGrid.DataBind();

            #endregion

            //下拉框初始化
            SetToComboEditor();
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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.FM_PayBill.Code.PB_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.FM_PayBill.Code.PB_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            HeadDS = HeadGridDS.FirstOrDefault(x => x.PB_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.FM_PayBill.Code.PB_ID].Value);
            if (HeadDS == null || string.IsNullOrEmpty(HeadDS.PB_ID))
            {
                return;
            }
            
            if (txtPB_ID.Text != HeadDS.PB_ID
                || (txtPB_ID.Text == HeadDS.PB_ID && txtPB_VersionNo.Text != HeadDS.PB_VersionNo?.ToString()))
            {
                if (txtPB_ID.Text == HeadDS.PB_ID && txtPB_VersionNo.Text != HeadDS.PB_VersionNo?.ToString())
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

            // 设置动作按钮状态
            SetActionEnableByStatus();
            //设置详情页面控件的是否可编辑
            SetDetailControl();
            //设置图片是否可编辑
            SetPictureControl();
            //查询明细
            QueryDetail();
            //设置付款单明细单元格样式
            SetPayBillStyle();
        }
        
        /// <summary>
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //付款单号
            txtPB_No.Text = HeadDS.PB_No;
            //审核状态
            cbPB_ApprovalStatusName.Text = HeadDS.PB_ApprovalStatusName ?? string.Empty;
            cbPB_ApprovalStatusName.Value = HeadDS.PB_ApprovalStatusCode;
            //付款组织
            txtPB_Pay_Org_Name.Text = HeadDS.PB_Pay_Org_Name;
            //应付合计金额
            //txtPB_PayableTotalAmount.Value = HeadDS.PB_PayableTotalAmount;
            //付款方式
            cbPB_PayTypeName.Text = HeadDS.PB_PayTypeName ?? string.Empty;
            cbPB_PayTypeName.Value = HeadDS.PB_PayTypeCode;
            //实付合计金额
            numPB_RealPayableTotalAmount.Value = HeadDS.PB_RealPayableTotalAmount;
            //付款日期
            dtPB_Date.Value = HeadDS.PB_Date;
            //付款凭证图片
            if (!string.IsNullOrEmpty(HeadDS.PB_CertificatePic))
            {
                pbPB_CertificatePic.PictureKey = HeadDS.PB_CertificatePic;
                pbPB_CertificatePic.PictureImage = BLLCom.GetBitmapImageByFileName(HeadDS.PB_CertificatePic);
            }
            else
            {
                pbPB_CertificatePic.PictureKey = Guid.NewGuid() + ".jpg";
            }
            //业务状态名称
            cbPB_BusinessStatusName.Text = HeadDS.PB_BusinessStatusName ?? string.Empty;
            cbPB_BusinessStatusName.Value = HeadDS.PB_BusinessStatusCode;
            //收款对象类型
            mcbAPB_ReceiveObjectTypeName.SelectedValue = HeadDS.PB_RecObjectTypeCode;
            //收款对象
            if (!string.IsNullOrEmpty(HeadDS.PB_RecObjectID))
            {
                mcbAPB_ReceiveObjectName.SelectedValue = HeadDS.PB_RecObjectID;
            }
            else
            {
                mcbAPB_ReceiveObjectName.SelectedTextExtra = HeadDS.PB_RecObjectTypeName;
                mcbAPB_ReceiveObjectName.SelectedText = HeadDS.PB_RecObjectName;
            }

            //有效
            if (HeadDS.PB_IsValid != null)
            {
                ckPB_IsValid.Checked = HeadDS.PB_IsValid.Value;
            }
            //备注
            txtPB_Remark.Text = HeadDS.PB_Remark;
            //创建人
            txtPB_CreatedBy.Text = HeadDS.PB_CreatedBy;
            //创建时间
            dtPB_CreatedTime.Value = HeadDS.PB_CreatedTime;
            //修改人
            txtPB_UpdatedBy.Text = HeadDS.PB_UpdatedBy;
            //修改时间
            dtPB_UpdatedTime.Value = HeadDS.PB_UpdatedTime;
            //付款凭证编号
            txtPB_CertificateNo.Text = HeadDS.PB_CertificateNo;
            //付款ID
            txtPB_ID.Text = HeadDS.PB_ID;
            //付款组织ID
            txtPB_Pay_Org_ID.Text = HeadDS.PB_Pay_Org_ID;
            //收款对象ID
            txtPB_RecObjectID.Text = HeadDS.PB_RecObjectID;
            //版本号
            txtPB_VersionNo.Value = HeadDS.PB_VersionNo;
            //钱包ID
            txtWal_ID.Text = HeadDS.Wal_ID;
            //钱包账号
            txtWal_No.Text = HeadDS.Wal_No;
            //钱包版本号
            txtWal_VersionNo.Value = HeadDS.Wal_VersionNo;
            //钱包余额
            txtWal_AvailableBalance.Value = HeadDS.Wal_AvailableBalance;
            //明细来源单号
            txtPBD_SrcBillNo.Text = txtWhere_PBD_SrcBillNo.Text;
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
            #region 单头检查

            //验证收款对象是否为空
            if (string.IsNullOrEmpty(mcbAPB_ReceiveObjectName.SelectedText))
            {
                //收款对象不能为空！
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[]
                            { SystemTableColumnEnums.FM_PayBill.Name.PB_RecObjectName }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                mcbAPB_ReceiveObjectName.Focus();
                return false;
            }

            #endregion

            #region 明细检查

            if (_detailGridDS.Count == 0)
            {
                //请至少添加一条付款单明细信息！
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0014, new object[] { SystemTableEnums.Name.FM_PayBillDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            foreach (var loopDetailGrid in _detailGridDS)
            {
                if (loopDetailGrid.PBD_PayAmount == null || loopDetailGrid.PBD_PayAmount <= 0)
                {
                    //付款单明细的付款金额不能为空！
                    MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[]
                            {SystemTableEnums.Name.FM_PayBillDetail+MsgParam.OF+ SystemTableColumnEnums.FM_PayBillDetail.Name.PBD_PayAmount }), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (string.IsNullOrEmpty(txtPB_ID.Text))
            {
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.W_0006, new object[]
                { SystemTableEnums.Name.FM_PayBill, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// 前端检查—审核
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForApprove()
        {
            #region 验证数据
            //未保存,不能审核
            if (string.IsNullOrEmpty(txtPB_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.FM_PayBill + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            MDLFM_PayBill resulReceiptBill = new MDLFM_PayBill();
            _bll.QueryForObject<MDLFM_PayBill, MDLFM_PayBill>(new MDLFM_PayBill()
            {
                WHERE_PB_IsValid = true,
                WHERE_PB_ID = txtPB_ID.Text.Trim()
            }, resulReceiptBill);
            //收款单不存在,不能审核
            if (string.IsNullOrEmpty(resulReceiptBill.PB_ID))
            {
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.FM_PayBill + MsgParam.NOTEXIST, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //收款单已审核,不能审核
            if (resulReceiptBill.PB_ApprovalStatusName == ApprovalStatusEnum.Name.YSH)
            {
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.FM_PayBill + ApprovalStatusEnum.Name.YSH, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //确认审核操作
            DialogResult isConfirmApprove = MessageBoxs.Show(Trans.FM, ToString(),
                MsgHelp.GetMsg(MsgCode.W_0014),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isConfirmApprove != DialogResult.OK)
            {
                return false;
            }

            #endregion

            return true;
        }

        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            HeadDS = new PayBillManagerUIModel()
            {
                //付款单号
                PB_No = txtPB_No.Text.Trim(),
                //审核状态
                PB_ApprovalStatusName = cbPB_ApprovalStatusName.Text.Trim(),
                //付款组织
                PB_Pay_Org_Name = txtPB_Pay_Org_Name.Text.Trim(),
                //付款方式
                PB_PayTypeName = cbPB_PayTypeName.Text.Trim(),
                //实付合计金额
                PB_RealPayableTotalAmount = Convert.ToDecimal(numPB_RealPayableTotalAmount.Value ?? 0),
                //付款日期
                PB_Date = (DateTime?)dtPB_Date.Value ?? DateTime.Now,
                //付款凭证图片
                PB_CertificatePic = pbPB_CertificatePic.PictureImage == null ? "" : pbPB_CertificatePic.PictureKey,
                //业务状态名称
                PB_BusinessStatusName = cbPB_BusinessStatusName.Text.Trim(),
                //收款对象类型
                PB_RecObjectTypeName = mcbAPB_ReceiveObjectTypeName.SelectedText,
                //收款对象
                PB_RecObjectName = mcbAPB_ReceiveObjectName.SelectedText,
                //有效
                PB_IsValid = ckPB_IsValid.Checked,
                //备注
                PB_Remark = txtPB_Remark.Text.Trim(),
                //创建人
                PB_CreatedBy = txtPB_CreatedBy.Text.Trim(),
                //创建时间
                PB_CreatedTime = (DateTime?)dtPB_CreatedTime.Value ?? DateTime.Now,
                //修改人
                PB_UpdatedBy = txtPB_UpdatedBy.Text.Trim(),
                //修改时间
                PB_UpdatedTime = (DateTime?)dtPB_UpdatedTime.Value ?? DateTime.Now,
                //付款凭证编号
                PB_CertificateNo = txtPB_CertificateNo.Text.Trim(),
                //付款ID
                PB_ID = txtPB_ID.Text.Trim(),
                //付款组织ID
                PB_Pay_Org_ID = txtPB_Pay_Org_ID.Text.Trim(),
                //收款对象类型编码
                PB_RecObjectTypeCode = mcbAPB_ReceiveObjectTypeName.SelectedValue,
                //收款对象ID
                PB_RecObjectID = mcbAPB_ReceiveObjectName.SelectedValue,
                //付款方式编码
                PB_PayTypeCode = cbPB_PayTypeName.Value?.ToString() ?? "",
                //业务状态编码
                PB_BusinessStatusCode = cbPB_BusinessStatusName.Value?.ToString() ?? "",
                //审核状态编码
                PB_ApprovalStatusCode = cbPB_ApprovalStatusName.Value?.ToString() ?? "",
                //版本号
                PB_VersionNo = Convert.ToInt64(txtPB_VersionNo.Text.Trim() == "" ? "1" : txtPB_VersionNo.Text.Trim()),
                //钱包ID
                Wal_ID = txtWal_ID.Text,
                //钱包账号
                Wal_No = txtWal_No.Text,
                //钱包版本号
                Wal_VersionNo = Convert.ToInt64(txtWal_VersionNo.Text.Trim() == "" ? "1" : txtWal_VersionNo.Text.Trim()),
                //钱包余额
                Wal_AvailableBalance = Convert.ToDecimal(txtWal_AvailableBalance.Text.Trim() == "" ? "0" : txtWal_AvailableBalance.Text.Trim()),
            };
        }
        
        /// <summary>
        /// 下拉框赋值
        /// </summary>
        private void SetToComboEditor()
        {
            //收款对象
            _bll.CopyModelList(_tempAllClientList, _clientList);
            mcbAPB_ReceiveObjectName.ExtraDisplayMember = "ClientType";
            mcbAPB_ReceiveObjectName.DisplayMember = "ClientName";
            mcbAPB_ReceiveObjectName.ValueMember = "ClientID";
            mcbAPB_ReceiveObjectName.DataSource = _clientList;

            //条件收款对象
            mcbWhere_ARB_PayObjectName.ExtraDisplayMember = "ClientType";
            mcbWhere_ARB_PayObjectName.DisplayMember = "ClientName";
            mcbWhere_ARB_PayObjectName.ValueMember = "ClientID";
            mcbWhere_ARB_PayObjectName.DataSource = _tempAllClientList;

            //收款对象类型【组织,供应商,一般汽修商户,平台内汽修商,配送人员,其他】
            _amountTransObjectTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.AmountTransObjectType);
            mcbAPB_ReceiveObjectTypeName.DisplayMember = SysConst.EN_TEXT;
            mcbAPB_ReceiveObjectTypeName.ValueMember = SysConst.EN_Code;
            mcbAPB_ReceiveObjectTypeName.DataSource = _amountTransObjectTypeList;
            mcbAPB_ReceiveObjectTypeName.SelectedValue = AmountTransObjectTypeEnum.Code.AUTOPARTSSUPPLIER;

            //付款方式【银行，微信，支付宝，现金】
            _paymentChannelList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.TradeType);
            List<ComComboBoxDataSourceTC> paymentChannelList = new List<ComComboBoxDataSourceTC>();

            paymentChannelList = _paymentChannelList.Where(x => x.Text != TradeTypeEnum.Name.WALLET).ToList();
            cbWhere_PB_PayTypeName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_PB_PayTypeName.ValueMember = SysConst.EN_Code;
            cbWhere_PB_PayTypeName.DataSource = _paymentChannelList;
            cbWhere_PB_PayTypeName.DataBind();

            cbPB_PayTypeName.DisplayMember = SysConst.EN_TEXT;
            cbPB_PayTypeName.ValueMember = SysConst.EN_Code;
            cbPB_PayTypeName.DataSource = _paymentChannelList;
            cbPB_PayTypeName.Text = TradeTypeEnum.Name.CASH;
            cbPB_PayTypeName.Value = TradeTypeEnum.Code.CASH;
            cbPB_PayTypeName.DataBind();

            //业务状态【已生成，执行中，已完成】
            _payBillStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.PayBillStatus);
            cbWhere_PB_BusinessStatusName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_PB_BusinessStatusName.ValueMember = SysConst.EN_Code;
            cbWhere_PB_BusinessStatusName.DataSource = _payBillStatusList;
            cbWhere_PB_BusinessStatusName.DataBind();

            cbPB_BusinessStatusName.DisplayMember = SysConst.EN_TEXT;
            cbPB_BusinessStatusName.ValueMember = SysConst.EN_Code;
            cbPB_BusinessStatusName.DataSource = _payBillStatusList;
            cbPB_BusinessStatusName.Text = PayBillStatusEnum.Name.YSC;
            cbPB_BusinessStatusName.Value = PayBillStatusEnum.Code.YSC;
            cbPB_BusinessStatusName.DataBind();

            //审核状态【待审核，已审核】
            _approvalStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.ApprovalStatus);
            cbWhere_PB_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_PB_ApprovalStatusName.ValueMember = SysConst.EN_Code;
            cbWhere_PB_ApprovalStatusName.DataSource = _approvalStatusList;
            cbWhere_PB_ApprovalStatusName.DataBind();

            cbPB_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
            cbPB_ApprovalStatusName.ValueMember = SysConst.EN_Code;
            cbPB_ApprovalStatusName.DataSource = _approvalStatusList;
            cbPB_ApprovalStatusName.Text = ApprovalStatusEnum.Name.DSH;
            cbPB_ApprovalStatusName.Value = ApprovalStatusEnum.Code.DSH;
            cbPB_ApprovalStatusName.DataBind();
        }

        /// <summary>
        /// 设置动作按钮状态
        /// </summary>
        private void SetActionEnableByStatus()
        {
            if (cbPB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                //[审核状态]为[已审核]的场合，[保存]、[删除]、[审核]不可用
                SetActionEnable(SystemActionEnum.Code.SAVE, false);
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                SetActionEnable(SystemActionEnum.Code.APPROVE, false);
                SetActionEnable(SystemActionEnum.Code.PRINT, true);
                SetActionEnable(SystemActionEnum.Code.SIGNIN, true);
            }
            else
            {
                //[审核状态]为[待审核]的场合，[打印]不可用
                //新增的场合，[删除]、[审核]不可用
                SetActionEnable(SystemActionEnum.Code.SAVE, true);
                SetActionEnable(SystemActionEnum.Code.DELETE, !string.IsNullOrEmpty(txtPB_ID.Text));
                SetActionEnable(SystemActionEnum.Code.APPROVE, !string.IsNullOrEmpty(txtPB_ID.Text));
                SetActionEnable(SystemActionEnum.Code.PRINT, false);
                SetActionEnable(SystemActionEnum.Code.SIGNIN, false);
            }
        }

        /// <summary>
        /// 根据[是否存在明细]控制单头是否可编辑
        /// </summary>
        private void SetDetailByIsExistDetail()
        {
            if (_detailGridDS.Count == 0)
            {
                mcbAPB_ReceiveObjectTypeName.Enabled = true;
                mcbAPB_ReceiveObjectName.Enabled = true;
            }
            else
            {
                mcbAPB_ReceiveObjectTypeName.Enabled = false;
                mcbAPB_ReceiveObjectName.Enabled = false;
            }
        }
       
        /// <summary>
        /// 设置详情页面控件的是否可编辑
        /// </summary>
        private void SetDetailControl()
        {
            if (cbPB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                #region 付款单.[审核状态]为[已审核] 的场合，详情不可编辑

                //单头
                dtPB_Date.Enabled = false;
                cbPB_PayTypeName.Enabled = false;
                mcbAPB_ReceiveObjectTypeName.Enabled = false;
                mcbAPB_ReceiveObjectName.Enabled = false;
                txtPB_CertificateNo.Enabled = false;
                ckPB_IsValid.Enabled = false;
                txtPB_Remark.Enabled = false;
                //明细列表不可添加、删除
                toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedProps.Enabled = false;
                toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = false;
               
                #endregion

            }
            else
            {
                #region 付款单未保存 或 付款单.[审核状态]为[待审核]的场合，详情可编辑

                //根据[是否存在明细]控制单头是否可编辑
                SetDetailByIsExistDetail();
                //单头
                dtPB_Date.Enabled = true;
                cbPB_PayTypeName.Enabled = true;
                txtPB_CertificateNo.Enabled = true;
                ckPB_IsValid.Enabled = true;
                txtPB_Remark.Enabled = true;
                //明细列表可添加、删除
                toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedProps.Enabled = true;
                toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = true;
               
                #endregion

            }
        }

        /// <summary>
        /// 设置单元格是否可以编辑
        /// </summary>
        private void SetPayBillStyle()
        {
            #region 设置Grid数据颜色
            gdDetail.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;

            foreach (var loopGridRow in gdDetail.Rows)
            {
                if (cbPB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
                {
                    #region 付款金额

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.FM_PayBillDetail.Code.PBD_PayAmount].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.FM_PayBillDetail.Code.PBD_PayAmount].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.FM_PayBillDetail.Code.PBD_PayAmount].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion

                    #region 备注

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.FM_PayBillDetail.Code.PBD_Remark].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.FM_PayBillDetail.Code.PBD_Remark].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.FM_PayBillDetail.Code.PBD_Remark].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion
                }
                else
                {
                    #region 付款金额

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.FM_PayBillDetail.Code.PBD_PayAmount].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.FM_PayBillDetail.Code.PBD_PayAmount].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.FM_PayBillDetail.Code.PBD_PayAmount].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #region 备注

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.FM_PayBillDetail.Code.PBD_Remark].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.FM_PayBillDetail.Code.PBD_Remark].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.FM_PayBillDetail.Code.PBD_Remark].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion
                }
            }
            #endregion
        }
        
        #region 明细Grid相关方法

        /// <summary>
        /// 查询付款单明细
        /// </summary>
        private void QueryDetail()
        {
            //销售订单明细列表
            _bll.QueryForList<PayBillManagerDetailUIModel>(new MDLFM_PayBillDetail
            {
                WHERE_PBD_PB_ID = txtPB_ID.Text.Trim(),
                WHERE_PBD_PB_No = txtPB_No.Text.Trim()
            }, _detailGridDS);

            //3.开始监控List变化（该数据源若有其他调整，请在StartMonitChanges()方法执行前调整）
            _detailGridDS.StartMonitChanges();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();

            //设置销售订单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }
     
        /// <summary>
        /// 添加明细
        /// </summary>
        private void AddPayDetail()
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
            if (string.IsNullOrEmpty(mcbAPB_ReceiveObjectName.SelectedText))
            {
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.FM_PayBill.Name.PB_RecObjectName }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #endregion

            //添加一条明细
            PayBillManagerDetailUIModel addAccountReceivableBillDetail = new PayBillManagerDetailUIModel
            {
                IsChecked = false,
                PBD_PB_ID = txtPB_ID.Text.Trim(),
                PBD_PB_No = txtPB_No.Text.Trim(),
                PBD_SourceTypeCode = PayBillDetailSourceTypeEnum.Code.SGFK,
                PBD_SourceTypeName = PayBillDetailSourceTypeEnum.Name.SGFK,
                PBD_IsValid = true,
                PBD_CreatedBy = LoginInfoDAX.UserName,
                PBD_CreatedTime = BLLCom.GetCurStdDatetime(),
                PBD_UpdatedBy = LoginInfoDAX.UserName,
                PBD_UpdatedTime = BLLCom.GetCurStdDatetime(),
            };
            _detailGridDS.Add(addAccountReceivableBillDetail);

            //根据[是否存在明细]控制单头是否可编辑
            SetDetailByIsExistDetail();

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
            //设置付款单明细单元格样式
            SetPayBillStyle();
        }
        
        /// <summary>
        /// 删除明细
        /// </summary>
        private void DeletePayDetail()
        {
            #region 验证

            //验证付款单的审核状态，[审核状态]为[已审核]的付款单不能删除明细
            if (cbPB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableEnums.Name.FM_PayBill + ApprovalStatusEnum.Name.YSH, SystemActionEnum.Name.DELETE + SystemTableEnums.Name.FM_PayBillDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                totalPayAmount = totalPayAmount + (loopDetailGrid.PBD_PayAmount ?? 0);
            }
            numPB_RealPayableTotalAmount.Text = Convert.ToString(totalPayAmount);
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置付款单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
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
                MessageBoxs.Show(Trans.FM, ToString(),
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
                    MessageBoxs.Show(Trans.FM, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (string.IsNullOrEmpty(outMsg))
                {
                    return true;
                }
                //导出成功
                MessageBoxs.Show(Trans.FM, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            MDLFM_PayBill resultPayBill = new MDLFM_PayBill();
            _bll.QueryForObject<MDLFM_PayBill, MDLFM_PayBill>(new MDLFM_PayBill()
            {
                WHERE_PB_CertificatePic = paramPictureKey,
                WHERE_PB_IsValid = true
            }, resultPayBill);
            if (!string.IsNullOrEmpty(resultPayBill.PB_ID))
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

                //更新付款单
                resultPayBill.WHERE_PB_ID = resultPayBill.PB_ID;
                resultPayBill.WHERE_PB_VersionNo = resultPayBill.PB_VersionNo;
                if (resultPayBill.PB_CertificatePic == paramPictureKey)
                {
                    resultPayBill.PB_CertificatePic = null;
                    pbPB_CertificatePic.PictureKey = Guid.NewGuid() + ".jpg";
                }

                bool updatePayBillResult = _bll.Update(SQLID.FM_PayBillManager_SQL01, resultPayBill);
                if (!updatePayBillResult)
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
                txtPB_VersionNo.Text = ((resultPayBill.PB_VersionNo ?? 0) + 1).ToString();
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
                pbPB_CertificatePic.UploadIsVisible = true;
                pbPB_CertificatePic.DeleteIsVisible = pbPB_CertificatePic.PictureImage != null;

            }
            else
            {
                //库存图片不可保存、审核的场合，不可上传、删除图片
                pbPB_CertificatePic.UploadIsVisible = false;
                pbPB_CertificatePic.DeleteIsVisible = false;
            }
            //有图片的场合，导出可见
            pbPB_CertificatePic.ExportIsVisible = pbPB_CertificatePic.PictureImage != null;
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
                    var curHead = HeadGridDS.FirstOrDefault(x => x.PB_ID == HeadDS.PB_ID);
                    if (curHead != null)
                    {
                        HeadGridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = HeadGridDS.FirstOrDefault(x => x.PB_ID == HeadDS.PB_ID);
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
