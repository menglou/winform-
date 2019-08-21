using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.SD;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.SD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinToolbars;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using SkyCar.Coeus.UIModel.Common.UIModel;

namespace SkyCar.Coeus.UI.SD
{
    /// <summary>
    /// 物流单管理
    /// </summary>
    public partial class FrmLogisticsBillManager : BaseFormCardListDetail<LogisticsBillManagerUIModel, LogisticsBillManagerQCModel, MDLSD_LogisticsBill>
    {
        #region 全局变量

        /// <summary>
        /// 物流单管理BLL
        /// </summary>
        private LogisticsBillManagerBLL _bll = new LogisticsBillManagerBLL();

        #region Grid数据源

        /// <summary>
        /// 物流单明细数据源
        /// </summary>
        SkyCarBindingList<LogisticsBillDetailManagerUIModel, MDLSD_LogisticsBillDetail> _detailGridDS =
            new SkyCarBindingList<LogisticsBillDetailManagerUIModel, MDLSD_LogisticsBillDetail>();

        #endregion

        #region 下拉框数据源

        /// <summary>
        /// 审核状态数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _approveStatusList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 物流单来源类型数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _logisticsBillSourceTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 物流人员类型数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _deliveryTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 所有客户数据源
        /// </summary>
        List<ComClientUIModel> _tempAllClientList = new List<ComClientUIModel>();
        /// <summary>
        /// 客户数据源
        /// </summary>
        List<ComClientUIModel> _clientList = new List<ComClientUIModel>();
        #endregion

        #region 图片路径相关

        /// <summary>
        /// 图片名称与临时保存路径
        /// </summary>
        private Dictionary<string, string> _dicPictureNameAndTempPath = new Dictionary<string, string>();
        #endregion

        /// <summary>
        /// 页面参数字典
        /// </summary>
        private Dictionary<string, object> _viewParameters;

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmLogisticsBillManager构造方法
        /// </summary>
        public FrmLogisticsBillManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// FrmLogisticsBillManager构造方法
        /// </summary>
        public FrmLogisticsBillManager(Dictionary<string, object> paramWindowParameters)
        {
            InitializeComponent();

            _viewParameters = paramWindowParameters;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmLogisticsBillManager_Load(object sender, EventArgs e)
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

            _tempAllClientList = BLLCom.GetAllLogisticsList(LoginInfoDAX.OrgID);

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
                #region 物流单

                if (_viewParameters.ContainsKey(SDViewParamKey.LogisticsBill.ToString()))
                {
                    MDLSD_LogisticsBill resultLogisticsBill = _viewParameters[SDViewParamKey.LogisticsBill.ToString()] as MDLSD_LogisticsBill;

                    if (resultLogisticsBill != null)
                    {
                        //物流单号
                        txtLB_No.Text = resultLogisticsBill.LB_No;
                        //组织
                        txtLB_Org_ID.Text = resultLogisticsBill.LB_Org_ID;
                        txtLB_Org_Name.Text = resultLogisticsBill.LB_Org_Name;
                        //来源类型
                        cbLB_SourceTypeName.Text = resultLogisticsBill.LB_SourceTypeName;
                        cbLB_SourceTypeName.Value = resultLogisticsBill.LB_SourceTypeCode;
                        //来源单号
                        txtLB_SourceNo.Text = resultLogisticsBill.LB_SourceNo;
                        //物流人员类型
                        cbLB_SourceName.Text = resultLogisticsBill.LB_SourceName;
                        cbLB_SourceName.Value = resultLogisticsBill.LB_SourceCode;
                        //物流人员
                        mcbLogisticsName.SelectedValue = resultLogisticsBill.LB_DeliveryByID + "|" + resultLogisticsBill.LB_PhoneNo + "|" + resultLogisticsBill.LB_Org_ID;
                        //物流人员手机号
                        txtLB_PhoneNo.Text = resultLogisticsBill.LB_PhoneNo;
                        //物流人员接单时间
                        dtLB_AcceptTime.Value = resultLogisticsBill.LB_AcceptTime;
                        //物流人员接单图片路径1
                        if (!string.IsNullOrEmpty(resultLogisticsBill.LB_AcceptPicPath1))
                        {
                            Image myImage1 =
                                Image.FromStream(WebRequest.Create(resultLogisticsBill.LB_AcceptPicPath1).GetResponse().GetResponseStream());
                            pbLB_AcceptPicPath1.PictureImage = myImage1;
                            pbLB_AcceptPicPath1.PictureKey = resultLogisticsBill.LB_AcceptPicPath1;
                        }
                        else
                        {
                            pbLB_AcceptPicPath1.PictureKey = Guid.NewGuid() + ".jpg";
                        }

                        //物流人员接单图片路径2
                        if (!string.IsNullOrEmpty(resultLogisticsBill.LB_AcceptPicPath2))
                        {
                            Image myImage1 = Image.FromStream(WebRequest.Create(resultLogisticsBill.LB_AcceptPicPath2).GetResponse().GetResponseStream());
                            pbLB_AcceptPicPath2.PictureImage = myImage1;
                            pbLB_AcceptPicPath2.PictureKey = resultLogisticsBill.LB_AcceptPicPath2;
                        }
                        else
                        {
                            pbLB_AcceptPicPath2.PictureKey = Guid.NewGuid() + ".jpg";
                        }

                        //收件人姓名
                        txtLB_Receiver.Text = resultLogisticsBill.LB_Receiver;
                        //收件人地址
                        txtLB_ReceiverAddress.Text = resultLogisticsBill.LB_ReceiverAddress;
                        //收件人邮编
                        txtLB_ReceiverPostcode.Text = resultLogisticsBill.LB_ReceiverPostcode;
                        //收件人手机号
                        txtLB_ReceiverPhoneNo.Text = resultLogisticsBill.LB_ReceiverPhoneNo;
                        //签收人
                        txtLB_ReceivedBy.Text = resultLogisticsBill.LB_ReceivedBy;
                        //签收时间
                        dtLB_ReceivedTime.Value = resultLogisticsBill.LB_ReceivedTime;
                        //签收拍照图片路径1
                        if (!string.IsNullOrEmpty(resultLogisticsBill.LB_ReceivedPicPath1))
                        {
                            Image myImage3 = Image.FromStream(WebRequest.Create(resultLogisticsBill.LB_ReceivedPicPath1).GetResponse().GetResponseStream());
                            pbLB_ReceivedPicPath1.PictureImage = myImage3;
                            pbLB_ReceivedPicPath1.PictureKey = resultLogisticsBill.LB_ReceivedPicPath1;
                        }
                        else
                        {
                            pbLB_ReceivedPicPath1.PictureKey = Guid.NewGuid() + ".jpg";
                        }
                        //签收拍照图片路径2
                        if (!string.IsNullOrEmpty(resultLogisticsBill.LB_ReceivedPicPath2))
                        {
                            Image myImage4 = Image.FromStream(WebRequest.Create(resultLogisticsBill.LB_ReceivedPicPath2).GetResponse().GetResponseStream());
                            pbLB_ReceivedPicPath2.PictureImage = myImage4;
                            pbLB_ReceivedPicPath2.PictureKey = resultLogisticsBill.LB_ReceivedPicPath2;
                        }
                        else
                        {
                            pbLB_ReceivedPicPath2.PictureKey = Guid.NewGuid() + ".jpg";
                        }
                        //物流费
                        numLB_Fee.Text = resultLogisticsBill.LB_Fee?.ToString();
                        //应收金额
                        txtLB_AccountReceivableAmount.Text = resultLogisticsBill.LB_AccountReceivableAmount?.ToString();
                        //赔偿金额
                        txtLB_Indemnification.Text = resultLogisticsBill.LB_Indemnification?.ToString();
                        //物流费付款状态名称
                        cbLB_PayStautsName.Text = resultLogisticsBill.LB_PayStautsName;
                        //物流费付款状态编码
                        cbLB_PayStautsName.Value = resultLogisticsBill.LB_PayStautsCode;
                        //单据状态名称
                        cbLB_StatusName.Text = resultLogisticsBill.LB_StatusName;
                        //单据状态编码
                        cbLB_StatusName.Value = resultLogisticsBill.LB_StatusCode;
                        //审核状态名称
                        cbLB_ApprovalStatusName.Text = resultLogisticsBill.LB_ApprovalStatusName;
                        //审核状态编码
                        cbLB_ApprovalStatusName.Value = resultLogisticsBill.LB_ApprovalStatusCode;
                        //备注
                        txtLB_Remark.Text = resultLogisticsBill.LB_Remark;
                        //有效
                        if (resultLogisticsBill.LB_IsValid != null)
                        {
                            ckLB_IsValid.Checked = resultLogisticsBill.LB_IsValid.Value;
                        }
                        //创建人
                        txtLB_CreatedBy.Text = resultLogisticsBill.LB_CreatedBy;
                        //创建时间
                        dtLB_CreatedTime.Value = resultLogisticsBill.LB_CreatedTime;
                        //修改人
                        txtLB_UpdatedBy.Text = resultLogisticsBill.LB_UpdatedBy;
                        //修改时间
                        dtLB_UpdatedTime.Value = resultLogisticsBill.LB_UpdatedTime;
                        //物流订单ID
                        txtLB_ID.Text = resultLogisticsBill.LB_ID;
                        //版本号
                        txtLB_VersionNo.Value = resultLogisticsBill.LB_VersionNo;

                    }
                    //选择【详情】Tab
                    tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;
                }
                #endregion

                #region 物流单明细

                if (_viewParameters.ContainsKey(SDViewParamKey.LogisticsBillDetail.ToString()))
                {
                    List<MDLSD_LogisticsBillDetail> resultLogisticsBillDetailList = _viewParameters[SDViewParamKey.LogisticsBillDetail.ToString()] as List<MDLSD_LogisticsBillDetail>;

                    if (resultLogisticsBillDetailList != null)
                    {
                        _detailGridDS.StartMonitChanges();
                        _bll.CopyModelList(resultLogisticsBillDetailList, _detailGridDS);
                        gdDetail.DataSource = _detailGridDS;
                        gdDetail.DataBind();
                        gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
                    }
                }
                #endregion
            }
            #endregion

            //设置详情页面控件以及导航按钮是否可编辑
            SetDetailControl();
            //7.设置动作按钮状态
            SetActionEnableByStatus();
            //设置单元格是否可以编辑
            SetLogisticsDetailStyle();

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
        /// 【列表】Grid的CellChange事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_CellChange(object sender, CellEventArgs e)
        {
            gdGrid.UpdateData();
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
        #endregion

        #region Tab改变事件

        /// <summary>
        /// 选中的Tab改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlFull_SelectedTabChanging(object sender, SelectedTabChangingEventArgs e)
        {
            SetActionEnableBySelectedTab(e.Tab.Key);
        }
        /// <summary>
        /// 选中的Tab改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlFull_SelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                //[列表]页不允许删除
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
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
        /// 物流单号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_LB_No_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //执行查询
                //QueryAction();
            }
        }
        /// <summary>
        /// 物流单来源单号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_LB_SourceNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //执行查询
                //QueryAction();
            }
        }
        /// <summary>
        /// 收件人姓名KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_LB_Receiver_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //执行查询
                //QueryAction();
            }
        }
        /// <summary>
        /// 审核状态名称ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_LB_ApprovalStatusName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 来源类型ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_LB_SourceTypeName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            QueryAction();
        }
        /// <summary>
        /// 来源单号EditorButtonClick事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_LB_SourceNo_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(cbWhere_LB_SourceTypeName.Text))
            {
                //请先选择来源类型，再选择来源单号
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.W_0034, new object[] { MsgParam.SOURCE_TYPE, SystemTableColumnEnums.SD_LogisticsBill.Name.LB_SourceNo }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #region 根据物流单.[来源类型]设置销售订单.[来源类型]

            //获取销售订单所有来源类型
            var salesOrderAllSourceTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.SalesOrderSourceType);
            string salesOrderSourceTypeName = string.Empty;
            switch (cbWhere_LB_SourceTypeName.Text)
            {
                case DeliveryBillSourceTypeEnum.Name.ZDXS:
                    //主动销售
                    salesOrderSourceTypeName = SalesOrderSourceTypeEnum.Name.ZDXS;
                    break;
                case DeliveryBillSourceTypeEnum.Name.ZJXS:
                    //手工创建
                    salesOrderSourceTypeName = SalesOrderSourceTypeEnum.Name.SGCJ;
                    break;
                case DeliveryBillSourceTypeEnum.Name.ZXXS:
                    //在线销售
                    salesOrderSourceTypeName = SalesOrderSourceTypeEnum.Name.ZXXS;
                    break;
            }
            #endregion

            //仅查询[单据状态]为{待发货}，[审核状态]为{已审核}的销售订单
            var paramApprovetatusList = _approveStatusList.Where(x => x.Text == ApprovalStatusEnum.Name.YSH).ToList();
            var paramSourceTypeList = salesOrderAllSourceTypeList.Where(x => x.Text == salesOrderSourceTypeName).ToList();

            //获取销售订单所有单据状态
            var salesOrderAllStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.SalesOrderStatus);
            var paramSalesOrderStatusList = salesOrderAllStatusList.Where(x => x.Text != SalesOrderStatusEnum.Name.YSC).ToList();

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //单据状态
                {ComViewParamKey.DocumentStatus.ToString(), paramSalesOrderStatusList},
                //审核状态
                {ComViewParamKey.ApprovalStatus.ToString(), paramApprovetatusList},
                //来源类型
                {ComViewParamKey.SourceType.ToString(), paramSourceTypeList}
            };

            FrmSalesOrderQuery frmSalesOrderQuery = new FrmSalesOrderQuery(paramViewParameters)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            DialogResult dialogResult = frmSalesOrderQuery.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            if (frmSalesOrderQuery.SelectedGridList != null && frmSalesOrderQuery.SelectedGridList.Count == 1)
            {
                //[物流单].[来源单号]为[销售订单].[单号]
                txtWhere_LB_SourceNo.Text = frmSalesOrderQuery.SelectedGridList[0].SO_No;
            }
        }
        /// <summary>
        /// 单据状态ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_LB_StatusName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 有效CheckedChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckWhere_LB_IsValid_CheckedChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        #endregion

        #region 单头相关事件

        /// <summary>
        /// 【详情】来源类型改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbLB_SourceTypeName_ValueChanged(object sender, EventArgs e)
        {
            txtLB_SourceNo.Clear();
            txtLB_Receiver.Clear();
            txtLB_ReceiverPhoneNo.Clear();
            txtLB_ReceiverAddress.Clear();
            txtLB_ReceiverPostcode.Clear();
            txtLB_AccountReceivableAmount.Clear();
            txtARB_ReceivedAmount.Clear();
            txtARB_UnReceiveAmount.Clear();

            _detailGridDS = new SkyCarBindingList<LogisticsBillDetailManagerUIModel, MDLSD_LogisticsBillDetail>();
            _detailGridDS.StartMonitChanges();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }

        /// <summary>
        /// 【详情】查询销售订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLB_SourceNo_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(cbLB_SourceTypeName.Text))
            {
                //请先选择来源类型，再选择来源单号
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.W_0034, new object[] { MsgParam.SOURCE_TYPE, SystemTableColumnEnums.SD_LogisticsBill.Name.LB_SourceNo }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cbLB_SourceTypeName.Text == DeliveryBillSourceTypeEnum.Name.ZZDB)
            {
                #region 查询调拨单

                FrmTransferQuery frmTransferQuery = new FrmTransferQuery()
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = frmTransferQuery.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                if (frmTransferQuery.SelectedGridList != null && frmTransferQuery.SelectedGridList.Count == 1)
                {
                    //比较这次选择的和上次选择的单号一样吗，如果一样
                    if (!string.IsNullOrEmpty(txtLB_SourceNo.Text) && txtLB_SourceNo.Text == frmTransferQuery.SelectedGridList[0].TB_No)
                    {
                        return;
                    }
                    //[物流单].[来源单号]为[销售订单].[单号]
                    txtLB_SourceNo.Text = frmTransferQuery.SelectedGridList[0].TB_No;
                    txtLB_Receiver.Clear();
                    txtLB_ReceiverAddress.Clear();
                    txtLB_ReceiverPhoneNo.Clear();
                    txtLB_ReceiverPostcode.Clear();
                    txtLB_AccountReceivableAmount.Clear();
                    txtARB_ReceivedAmount.Clear();
                    txtARB_UnReceiveAmount.Clear();

                    _detailGridDS = new SkyCarBindingList<LogisticsBillDetailManagerUIModel, MDLSD_LogisticsBillDetail>();
                    _detailGridDS.StartMonitChanges();
                    gdDetail.DataSource = _detailGridDS;
                    gdDetail.DataBind();
                    gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
                    //设置单元格是否可以编辑
                    SetLogisticsDetailStyle();


                    #region 查询该销售订单是否已存在物流单

                    var logisticsBillCount = _bll.QueryForCount<int>(new MDLSD_LogisticsBill
                    {
                        WHERE_LB_SourceNo = txtLB_SourceNo.Text,
                        WHERE_LB_IsValid = true
                    });
                    if (logisticsBillCount > 0)
                    {
                        //txtLB_SourceNo.Text对应的物流订单已存在，不能重复添加
                        MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0006, new object[] { txtLB_SourceNo.Text + MsgParam.CORRESPONDENCE + MsgParam.OF + SystemTableEnums.Name.SD_LogisticsBill }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtLB_SourceNo.Clear();
                        return;
                    }
                    #endregion

                    #region 查询收件人姓名、地址和手机号、默认物流人员相关信息

                    //获取客户信息
                    List<MDLSM_Organization> resultOrganizationList = new List<MDLSM_Organization>();
                    _bll.QueryForList(new MDLSM_Organization()
                    {
                        WHERE_Org_ID = frmTransferQuery.SelectedGridList[0].TB_TransferInOrgId,
                    }, resultOrganizationList);
                    if (resultOrganizationList.Count == 1)
                    {
                        //收件人
                        txtLB_Receiver.Text = resultOrganizationList[0].Org_ShortName;
                        //收件人地址
                        txtLB_ReceiverAddress.Text = resultOrganizationList[0].Org_Addr;
                        //收件人手机号
                        txtLB_ReceiverPhoneNo.Text = resultOrganizationList[0].Org_PhoneNo;
                    }

                    #endregion

                    #region 根据来源单号加载[物流单明细]

                    //查询调拨明细列表
                    List<MDLPIS_TransferBillDetail> transferBillDetailList = new List<MDLPIS_TransferBillDetail>();
                    _bll.QueryForList<MDLPIS_TransferBillDetail, MDLPIS_TransferBillDetail>(new MDLPIS_TransferBillDetail()
                    {
                        WHERE_TBD_TB_ID = frmTransferQuery.SelectedGridList[0].TB_ID,
                        WHERE_TBD_IsValid = true
                    }, transferBillDetailList);
                    decimal amount = 0;
                    foreach (var loopSalesOrderDetail in transferBillDetailList)
                    {
                        //定义一个物流单明细
                        LogisticsBillDetailManagerUIModel addLogisticsBillDetail = new LogisticsBillDetailManagerUIModel
                        {
                            LBD_Barcode = loopSalesOrderDetail.TBD_Barcode,
                            LBD_BatchNoNew = loopSalesOrderDetail.TBD_TransInBatchNo,
                            LBD_Name = loopSalesOrderDetail.TBD_Name,
                            LBD_Specification = loopSalesOrderDetail.TBD_Specification,
                            LBD_UOM = loopSalesOrderDetail.TBD_UOM,
                            LBD_DeliveryQty = loopSalesOrderDetail.TBD_Qty,
                            LBD_AccountReceivableAmount = (loopSalesOrderDetail.TBD_Qty ?? 0) * (loopSalesOrderDetail.TBD_DestUnitPrice ?? 0),
                            //默认物流明细为{已生成}
                            LBD_StatusName = LogisticsBillDetailStatusEnum.Name.YSC,
                            LBD_StatusCode = LogisticsBillDetailStatusEnum.Code.YSC,
                            LBD_IsValid = true,
                            LBD_CreatedBy = LoginInfoDAX.UserName,
                            LBD_UpdatedBy = LoginInfoDAX.UserName,
                            LBD_CreatedTime = BLLCom.GetCurStdDatetime(),
                            LBD_UpdatedTime = BLLCom.GetCurStdDatetime(),
                            LBD_VersionNo = 1
                        };
                        addLogisticsBillDetail.LBD_CreatedBy = addLogisticsBillDetail.LBD_UpdatedBy = LoginInfoDAX.UserName;
                        addLogisticsBillDetail.LBD_CreatedTime = addLogisticsBillDetail.LBD_UpdatedTime = BLLCom.GetCurStdDatetime();
                        amount = amount + (addLogisticsBillDetail.LBD_AccountReceivableAmount ?? 0);
                        _detailGridDS.Add(addLogisticsBillDetail);
                    }
                    gdDetail.DataSource = _detailGridDS;
                    gdDetail.DataBind();
                    gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
                    txtLB_AccountReceivableAmount.Text = Convert.ToString(amount);
                    #endregion
                }

                #endregion
            }
            else
            {
                #region 查询销售单

                #region 根据物流单.[来源类型]设置销售订单.[来源类型]

                //获取销售订单所有来源类型
                var salesOrderAllSourceTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.SalesOrderSourceType);
                string salesOrderSourceTypeName = string.Empty;
                switch (cbLB_SourceTypeName.Text)
                {
                    case DeliveryBillSourceTypeEnum.Name.ZDXS:
                        //主动销售
                        salesOrderSourceTypeName = SalesOrderSourceTypeEnum.Name.ZDXS;
                        break;
                    case DeliveryBillSourceTypeEnum.Name.ZJXS:
                        //手工创建
                        salesOrderSourceTypeName = SalesOrderSourceTypeEnum.Name.SGCJ;
                        break;
                    case DeliveryBillSourceTypeEnum.Name.ZXXS:
                        //在线销售
                        salesOrderSourceTypeName = SalesOrderSourceTypeEnum.Name.ZXXS;
                        break;
                }
                #endregion

                //仅查询[单据状态]为{待发货}，[审核状态]为{已审核}的销售订单
                var paramApprovetatusList = _approveStatusList.Where(x => x.Text == ApprovalStatusEnum.Name.YSH).ToList();
                var paramSourceTypeList = salesOrderAllSourceTypeList.Where(x => x.Text == salesOrderSourceTypeName).ToList();

                //获取销售订单所有单据状态
                var salesOrderAllStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.SalesOrderStatus);
                var paramSalesOrderStatusList = salesOrderAllStatusList.Where(x => x.Text == SalesOrderStatusEnum.Name.DFH).ToList();

                Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
                {
                    //单据状态
                    {ComViewParamKey.DocumentStatus.ToString(), paramSalesOrderStatusList},
                    //审核状态
                    {ComViewParamKey.ApprovalStatus.ToString(), paramApprovetatusList},
                    //来源类型
                    {ComViewParamKey.SourceType.ToString(), paramSourceTypeList}
                };

                FrmSalesOrderQuery frmSalesOrderQuery = new FrmSalesOrderQuery(paramViewParameters)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = frmSalesOrderQuery.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                if (frmSalesOrderQuery.SelectedGridList != null && frmSalesOrderQuery.SelectedGridList.Count == 1)
                {
                    //比较这次选择的和上次选择的单号一样吗，如果一样
                    if (!string.IsNullOrEmpty(txtLB_SourceNo.Text) && txtLB_SourceNo.Text == frmSalesOrderQuery.SelectedGridList[0].SO_No)
                    {
                        return;
                    }
                    //[物流单].[来源单号]为[销售订单].[单号]
                    txtLB_SourceNo.Text = frmSalesOrderQuery.SelectedGridList[0].SO_No;
                    txtLB_Receiver.Clear();
                    txtLB_ReceiverAddress.Clear();
                    txtLB_ReceiverPhoneNo.Clear();
                    txtLB_ReceiverPostcode.Clear();
                    txtLB_AccountReceivableAmount.Clear();
                    txtARB_ReceivedAmount.Clear();
                    txtARB_UnReceiveAmount.Clear();

                    _detailGridDS = new SkyCarBindingList<LogisticsBillDetailManagerUIModel, MDLSD_LogisticsBillDetail>();
                    _detailGridDS.StartMonitChanges();
                    gdDetail.DataSource = _detailGridDS;
                    gdDetail.DataBind();
                    gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
                    //设置单元格是否可以编辑
                    SetLogisticsDetailStyle();

                    #region 查询该销售订单是否已存在物流单

                    var logisticsBillCount = _bll.QueryForCount<int>(new MDLSD_LogisticsBill
                    {
                        WHERE_LB_SourceNo = txtLB_SourceNo.Text,
                        WHERE_LB_IsValid = true
                    });
                    if (logisticsBillCount > 0)
                    {
                        //txtLB_SourceNo.Text对应的物流订单已存在，不能重复添加
                        MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0006, new object[] { txtLB_SourceNo.Text + MsgParam.CORRESPONDENCE + MsgParam.OF + SystemTableEnums.Name.SD_LogisticsBill }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtLB_SourceNo.Clear();
                        return;
                    }
                    #endregion

                    #region 查询收件人姓名、地址和手机号、默认物流人员相关信息

                    List<CustomerQueryUIModel> resultCustomerList = new List<CustomerQueryUIModel>();
                    resultCustomerList = BLLCom.GetCustomerInfo(frmSalesOrderQuery.SelectedGridList[0].SO_CustomerTypeName, frmSalesOrderQuery.SelectedGridList[0].SO_CustomerID);
                    if (resultCustomerList.Count == 1)
                    {
                        //收件人
                        txtLB_Receiver.Text = resultCustomerList[0].CustomerName;
                        //收件人地址
                        txtLB_ReceiverAddress.Text = resultCustomerList[0].CustomerAddress;
                        //收件人手机号
                        txtLB_ReceiverPhoneNo.Text = resultCustomerList[0].CustomerPhoneNo;
                        //物流人员类型名称
                        cbLB_SourceName.Text = resultCustomerList[0].DeliveryTypeName;
                        //物流人员类型编码
                        cbLB_SourceName.Value = resultCustomerList[0].DeliveryTypeCode;
                        //物流人员ID
                        mcbLogisticsName.SelectedValue = resultCustomerList[0].DeliveryByID + "|" + resultCustomerList[0].DeliveryByPhone + "|" + LoginInfoDAX.OrgID;
                        //物流人员手机号
                        txtLB_PhoneNo.Text = resultCustomerList[0].DeliveryByPhone;
                    }
                    #endregion

                    #region 查询销售订单对应的应收单
                    List<MDLFM_AccountReceivableBill> resultAccountReceivableBillList = new List<MDLFM_AccountReceivableBill>();
                    _bll.QueryForList<MDLFM_AccountReceivableBill, MDLFM_AccountReceivableBill>(new MDLFM_AccountReceivableBill
                    {
                        WHERE_ARB_SrcBillNo = txtLB_SourceNo.Text,
                        WHERE_ARB_BillDirectName = BillDirectionEnum.Name.PLUS,
                        WHERE_ARB_IsValid = true,
                    }, resultAccountReceivableBillList);
                    //销售应收的应收单
                    var tempSalesAccountReceivableBill = resultAccountReceivableBillList.FirstOrDefault(
                            x => x.ARB_SourceTypeName == AccountReceivableBillSourceTypeEnum.Name.XSYS);
                    if (tempSalesAccountReceivableBill != null)
                    {
                        //[物流单].[应收金额]为[销售应收的应收单].[应收金额]
                        txtLB_AccountReceivableAmount.Text = tempSalesAccountReceivableBill.ARB_AccountReceivableAmount?.ToString();
                        //[销售应收的应收单].已收金额
                        txtARB_ReceivedAmount.Text = tempSalesAccountReceivableBill.ARB_ReceivedAmount?.ToString();
                        //[销售应收的应收单].未收金额
                        txtARB_UnReceiveAmount.Text = tempSalesAccountReceivableBill.ARB_UnReceiveAmount?.ToString();
                    }
                    //其他应收（赔偿）的应收单
                    var tempPCAccountReceivableBill = resultAccountReceivableBillList.FirstOrDefault(
                            x => x.ARB_SourceTypeName == AccountReceivableBillSourceTypeEnum.Name.QTYS);
                    if (tempPCAccountReceivableBill != null)
                    {
                        //[物流单].[赔偿金额]为[其他应收（赔偿）的应收单].[应收金额]
                        txtLB_Indemnification.Text = tempPCAccountReceivableBill.ARB_AccountReceivableAmount?.ToString();
                    }
                    #endregion

                    #region 查询销售订单对应的应收单明细

                    List<MDLFM_AccountReceivableBillDetail> resultAccountReceivableBillDetailList = new List<MDLFM_AccountReceivableBillDetail>();
                    _bll.QueryForList<MDLFM_AccountReceivableBillDetail, MDLFM_AccountReceivableBillDetail>(new MDLFM_AccountReceivableBillDetail
                    {
                        WHERE_ARBD_SrcBillNo = txtLB_SourceNo.Text,
                        WHERE_ARBD_IsMinusDetail = false,
                        WHERE_ARBD_IsValid = true,
                    }, resultAccountReceivableBillDetailList);
                    //销售应收对应的应收单明细
                    List<MDLFM_AccountReceivableBillDetail> tempSalesAccountReceivableDetailList = new List<MDLFM_AccountReceivableBillDetail>();
                    if (tempSalesAccountReceivableBill != null)
                    {
                        tempSalesAccountReceivableDetailList = resultAccountReceivableBillDetailList.Where(x => x.ARBD_ARB_ID == tempSalesAccountReceivableBill.ARB_ID).ToList();
                    }
                    //其他应收（赔偿）对应的应收单明细
                    List<MDLFM_AccountReceivableBillDetail> tempPCAccountReceivableDetailList = new List<MDLFM_AccountReceivableBillDetail>();
                    if (tempPCAccountReceivableBill != null)
                    {
                        tempPCAccountReceivableDetailList = resultAccountReceivableBillDetailList.Where(x => x.ARBD_ARB_ID == tempPCAccountReceivableBill.ARB_ID).ToList();
                    }
                    #endregion

                    #region 根据来源单号加载[物流单明细]

                    //查询销售明细列表
                    List<MDLSD_SalesOrderDetail> salesOrderDetailList = new List<MDLSD_SalesOrderDetail>();
                    _bll.QueryForList<MDLSD_SalesOrderDetail, MDLSD_SalesOrderDetail>(new MDLSD_SalesOrderDetail()
                    {
                        WHERE_SOD_SO_ID = frmSalesOrderQuery.SelectedGridList[0].SO_ID,
                        WHERE_SOD_IsValid = true
                    }, salesOrderDetailList);

                    foreach (var loopSalesOrderDetail in salesOrderDetailList)
                    {
                        //定义一个物流单明细
                        LogisticsBillDetailManagerUIModel addLogisticsBillDetail = new LogisticsBillDetailManagerUIModel
                        {
                            LBD_LB_ID = txtLB_ID.Text,
                            LBD_LB_No = txtLB_No.Text,
                            LBD_Barcode = loopSalesOrderDetail.SOD_Barcode,
                            LBD_BatchNo = loopSalesOrderDetail.SOD_BatchNo,
                            LBD_BatchNoNew = loopSalesOrderDetail.SOD_BatchNoNew,
                            LBD_Name = loopSalesOrderDetail.SOD_Name,
                            LBD_Specification = loopSalesOrderDetail.SOD_Specification,
                            LBD_UOM = loopSalesOrderDetail.SOD_UOM,
                            LBD_DeliveryQty = loopSalesOrderDetail.SOD_Qty,
                            LBD_StatusCode = LogisticsBillDetailStatusEnum.Code.YSC,
                            LBD_StatusName = LogisticsBillDetailStatusEnum.Name.YSC,
                            LBD_Remark = loopSalesOrderDetail.SOD_Remark,
                            LBD_IsValid = loopSalesOrderDetail.SOD_IsValid
                        };
                        addLogisticsBillDetail.LBD_CreatedBy = addLogisticsBillDetail.LBD_UpdatedBy = LoginInfoDAX.UserName;
                        addLogisticsBillDetail.LBD_CreatedTime = addLogisticsBillDetail.LBD_UpdatedTime = BLLCom.GetCurStdDatetime();
                        //该销售明细对应的销售应收的应收单明细
                        var tempSalesAccountReceivableDetail = tempSalesAccountReceivableDetailList.FirstOrDefault(x => x.ARBD_SrcBillDetailID == loopSalesOrderDetail.SOD_ID);
                        if (tempSalesAccountReceivableDetail != null)
                        {
                            addLogisticsBillDetail.LBD_AccountReceivableAmount = tempSalesAccountReceivableDetail.ARBD_AccountReceivableAmount;
                        }
                        //该销售明细对应的其他应收（赔偿）的应收单明细
                        var tempPCAccountReceivableDetail = tempPCAccountReceivableDetailList.FirstOrDefault(x => x.ARBD_SrcBillDetailID == loopSalesOrderDetail.SOD_ID);
                        if (tempPCAccountReceivableDetail != null)
                        {
                            addLogisticsBillDetail.LBD_Indemnification = tempPCAccountReceivableDetail.ARBD_AccountReceivableAmount;
                        }

                        _detailGridDS.Add(addLogisticsBillDetail);
                    }
                    gdDetail.DataSource = _detailGridDS;
                    gdDetail.DataBind();
                    gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);

                    #endregion
                }
                #endregion
            }
        }

        /// <summary>
        /// 物流人员发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbLogisticsName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.mcbLogisticsName.SelectedText))
            {
                this.cbLB_SourceName.Clear();
                this.txtLB_PhoneNo.Clear();
            }
            else
            {
                this.cbLB_SourceName.Text = this.mcbLogisticsName.SelectedTextExtra;
                string[] infoArray = this.mcbLogisticsName.SelectedText.Split('|');
                if (infoArray.Length >= 2)
                {
                    this.txtLB_PhoneNo.Text = infoArray[1];
                }
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
                DialogResult dialogResult = MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.W_0001), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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
            //4.给下拉框赋值
            SetToComboEditor();

            //6.设置详情页面控件以及导航按钮是否可编辑
            SetDetailControl();
            //7.设置动作按钮状态
            SetActionEnableByStatus();
            //设置图片是否可编辑
            SetPictureControl();
            //设置单元格是否可以编辑
            SetLogisticsDetailStyle();

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();
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
            bool saveResult = _bll.SaveDetailDS(HeadDS, _detailGridDS, _dicPictureNameAndTempPath);
            if (!saveResult)
            {
                //保存失败
                MessageBoxs.Show(Trans.SD, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //4.开始监控List变化
            _detailGridDS.StartMonitChanges();
            //4.将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //6.设置详情页面控件以及导航按钮是否可编辑
            SetDetailControl();
            //7.设置动作按钮状态
            SetActionEnableByStatus();

            //设置图片Key
            pbLB_AcceptPicPath1.PictureKey = !string.IsNullOrEmpty(HeadDS.LB_AcceptPicPath1) ? HeadDS.LB_AcceptPicPath1 : (Guid.NewGuid() + ".jpg");
            pbLB_AcceptPicPath2.PictureKey = !string.IsNullOrEmpty(HeadDS.LB_AcceptPicPath2) ? HeadDS.LB_AcceptPicPath2 : (Guid.NewGuid() + ".jpg");
            pbLB_ReceivedPicPath1.PictureKey = !string.IsNullOrEmpty(HeadDS.LB_ReceivedPicPath1) ? HeadDS.LB_ReceivedPicPath1 : (Guid.NewGuid() + ".jpg");
            pbLB_ReceivedPicPath2.PictureKey = !string.IsNullOrEmpty(HeadDS.LB_ReceivedPicPath2) ? HeadDS.LB_ReceivedPicPath2 : (Guid.NewGuid() + ".jpg");

            //清除图片临时路径
            _dicPictureNameAndTempPath.Clear();
            //设置单元格是否可以编辑
            SetLogisticsDetailStyle();
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
            var argsDetail = new List<MDLSD_LogisticsBillDetail>();
            //将HeadDS转换为TBModel对象
            var argsHead = base.HeadDS.ToTBModelForSaveAndDelete<MDLSD_LogisticsBill>();
            //将当前DetailGridDS转换为指定类型的TBModelList
            _detailGridDS.ToTBModelListForUpdateAndDelete<MDLSD_LogisticsBillDetail>(argsDetail);
            //过滤明细列表中未保存的数据
            argsDetail = argsDetail.Where(x => !string.IsNullOrEmpty(x.WHERE_LBD_ID)).ToList();
            //2.执行删除
            bool deleteResult = _bll.UnityDelete<MDLSD_LogisticsBill, MDLSD_LogisticsBillDetail>(argsHead, argsDetail);
            if (!deleteResult)
            {
                //删除失败
                MessageBoxs.Show(Trans.SD, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //删除成功
            MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            //3.清空【详情】画面数据
            InitializeDetailTabControls();
            //刷新列表
            RefreshList(true);

            //4.将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            #region 删除本地和文件服务器中的图片

            //错误消息
            var outMsg = string.Empty;
            //删除本地和文件服务器中的图片
            BLLCom.DeleteFileByFileName(argsHead.LB_AcceptPicPath1, ref outMsg);
            BLLCom.DeleteFileByFileName(argsHead.LB_AcceptPicPath2, ref outMsg);
            BLLCom.DeleteFileByFileName(argsHead.LB_ReceivedPicPath1, ref outMsg);
            BLLCom.DeleteFileByFileName(argsHead.LB_ReceivedPicPath2, ref outMsg);

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
            base.ConditionDS = new LogisticsBillManagerQCModel()
            {
                //SqlId
                SqlId = SQLID.SD_LogisticsBill_SQL03,
                //物流单号
                WHERE_LB_No = txtWhere_LB_No.Text.Trim(),
                //来源类型
                WHERE_LB_SourceTypeName = cbWhere_LB_SourceTypeName.Text,
                //来源单号
                WHERE_LB_SourceNo = txtWhere_LB_SourceNo.Text.Trim(),
                //物流人员
                WHERE_LB_DeliveryBy = txtWhere_LB_DeliveryBy.Text.Trim(),
                //收件人姓名
                WHERE_LB_Receiver = txtWhere_LB_Receiver.Text.Trim(),
                //单据状态
                WHERE_LB_StatusName = cbWhere_LB_StatusName.Text,
                //审核状态
                WHERE_LB_ApprovalStatusName = cbWhere_LB_ApprovalStatusName.Text,
                //有效
                WHERE_LB_IsValid = ckWhere_LB_IsValid.Checked,
                //组织ID
                WHERE_LB_Org_ID = LoginInfoDAX.OrgID,
            };
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

            //1.前端检查-审核
            if (!ClientCheckForApprove())
            {
                return;
            }

            //将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();

            #region 物流单审核
            bool saveApproveResult = _bll.ApproveDetailDS(HeadDS, _detailGridDS, _dicPictureNameAndTempPath);
            //审核失败
            if (!saveApproveResult)
            {
                MessageBoxs.Show(Trans.SD, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //审核成功
            MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.APPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            #endregion

            //刷新列表
            RefreshList();

            //开始监控List变化
            _detailGridDS.StartMonitChanges();
            //将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置详情页面控件以及导航按钮是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();

            //更新明细列表
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);

            //设置图片Key
            pbLB_AcceptPicPath1.PictureKey = !string.IsNullOrEmpty(HeadDS.LB_AcceptPicPath1) ? HeadDS.LB_AcceptPicPath1 : (Guid.NewGuid() + ".jpg");
            pbLB_AcceptPicPath2.PictureKey = !string.IsNullOrEmpty(HeadDS.LB_AcceptPicPath2) ? HeadDS.LB_AcceptPicPath2 : (Guid.NewGuid() + ".jpg");
            pbLB_ReceivedPicPath1.PictureKey = !string.IsNullOrEmpty(HeadDS.LB_ReceivedPicPath1) ? HeadDS.LB_ReceivedPicPath1 : (Guid.NewGuid() + ".jpg");
            pbLB_ReceivedPicPath2.PictureKey = !string.IsNullOrEmpty(HeadDS.LB_ReceivedPicPath2) ? HeadDS.LB_ReceivedPicPath2 : (Guid.NewGuid() + ".jpg");

            //清除图片临时路径
            _dicPictureNameAndTempPath.Clear();
            //设置图片是否可编辑
            SetPictureControl();
            //设置单元格是否可以编辑
            SetLogisticsDetailStyle();
        }

        /// <summary>
        /// 反审核
        /// </summary>
        public override void UnApproveAction()
        {
            base.UnApproveAction();

            //1.前端检查-删除
            if (!ClientCheckForUnApprove())
            {
                return;
            }

            SetCardCtrlsToDetailDS();

            #region 物流单反审核
            bool saveUnApprove = _bll.UnApproveDetailDS(HeadDS, _detailGridDS);
            //反审核失败
            if (!saveUnApprove)
            {
                MessageBoxs.Show(Trans.SD, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //反审核成功
            MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.UNAPPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            #endregion

            //刷新列表
            RefreshList();

            //开始监控List变化
            _detailGridDS.StartMonitChanges();
            //将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置详情页面控件以及导航按钮是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();

            //更新明细列表
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);

            //设置图片是否可编辑
            SetPictureControl();
            //设置单元格是否可以编辑
            SetLogisticsDetailStyle();
        }

        /// <summary>
        /// 打印
        /// </summary>
        public override void PrintAction()
        {
            base.PrintAction();
            Print();
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 工具生成
            //物流单号
            txtLB_No.Clear();
            //组织名称
            txtLB_Org_Name.Clear();
            //来源类型
            cbLB_SourceTypeName.Items.Clear();
            //物流单来源单号
            txtLB_SourceNo.Clear();
            //物流人员类型
            cbLB_SourceName.Items.Clear();
            //物流人员名称
            mcbLogisticsName.Clear();
            //物流人员手机号
            txtLB_PhoneNo.Clear();
            //物流人员接单时间
            dtLB_AcceptTime.Value = DateTime.Now;
            //物流人员接单图片路径1
            pbLB_AcceptPicPath1.PictureImage = null;
            //物流人员接单图片路径2
            pbLB_AcceptPicPath2.PictureImage = null;
            //收件人姓名
            txtLB_Receiver.Clear();
            //收件人地址
            txtLB_ReceiverAddress.Clear();
            //收件人邮编
            txtLB_ReceiverPostcode.Clear();
            //收件人手机号
            txtLB_ReceiverPhoneNo.Clear();
            //签收人
            txtLB_ReceivedBy.Clear();
            //签收时间
            dtLB_ReceivedTime.Value = null;
            //签收拍照图片路径1
            pbLB_ReceivedPicPath1.PictureImage = null;
            //签收拍照图片路径2
            pbLB_ReceivedPicPath2.PictureImage = null;
            //物流费
            numLB_Fee.Value = null;
            //应收金额
            txtLB_AccountReceivableAmount.Clear();
            //赔偿金额
            txtLB_Indemnification.Clear();
            //物流费付款状态名称
            cbLB_PayStautsName.Items.Clear();
            //单据状态名称
            cbLB_StatusName.Items.Clear();
            //审核状态名称
            cbLB_ApprovalStatusName.Items.Clear();
            //备注
            txtLB_Remark.Clear();
            //有效
            ckLB_IsValid.Checked = true;
            ckLB_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtLB_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtLB_CreatedTime.Value = DateTime.Now;
            //修改人
            txtLB_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtLB_UpdatedTime.Value = DateTime.Now;
            //物流订单ID
            txtLB_ID.Clear();
            //版本号
            txtLB_VersionNo.Clear();
            //给 物流单号 设置焦点
            lblLB_No.Focus();
            #endregion

            #region 初始化物流单明细列表

            _detailGridDS = new SkyCarBindingList<LogisticsBillDetailManagerUIModel, MDLSD_LogisticsBillDetail>();
            //开始监控物流明细List变化
            _detailGridDS.StartMonitChanges();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            #endregion

            #region 初始化图片

            //物流人员接单图片1
            pbLB_AcceptPicPath1.PictureKey = Guid.NewGuid() + ".jpg";
            pbLB_AcceptPicPath1.ExecuteUpload = UploadPicture;
            pbLB_AcceptPicPath1.ExecuteExport = ExportPicture;
            pbLB_AcceptPicPath1.ExecuteDelete = DeletePicture;

            //物流人员接单图片2
            pbLB_AcceptPicPath2.PictureKey = Guid.NewGuid() + ".jpg";
            pbLB_AcceptPicPath2.ExecuteUpload = UploadPicture;
            pbLB_AcceptPicPath2.ExecuteExport = ExportPicture;
            pbLB_AcceptPicPath2.ExecuteDelete = DeletePicture;

            //签收拍照图片1
            pbLB_ReceivedPicPath1.PictureKey = Guid.NewGuid() + ".jpg";
            pbLB_ReceivedPicPath1.ExecuteUpload = UploadPicture;
            pbLB_ReceivedPicPath1.ExecuteExport = ExportPicture;
            pbLB_ReceivedPicPath1.ExecuteDelete = DeletePicture;

            //签收拍照图片2
            pbLB_ReceivedPicPath2.PictureKey = Guid.NewGuid() + ".jpg";
            pbLB_ReceivedPicPath2.ExecuteUpload = UploadPicture;
            pbLB_ReceivedPicPath2.ExecuteExport = ExportPicture;
            pbLB_ReceivedPicPath2.ExecuteDelete = DeletePicture;

            #endregion
        }
     
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 查询条件初始化
            //物流单号
            txtWhere_LB_No.Clear();
            //物流单来源单号
            txtWhere_LB_SourceNo.Clear();
            //收件人姓名
            txtWhere_LB_Receiver.Clear();
            //审核状态名称
            cbWhere_LB_ApprovalStatusName.Items.Clear();
            //来源类型
            cbWhere_LB_SourceTypeName.Items.Clear();
            //物流人员
            txtWhere_LB_DeliveryBy.Clear();
            //单据状态
            cbWhere_LB_StatusName.Items.Clear();
            //有效
            ckWhere_LB_IsValid.Checked = true;
            ckWhere_LB_IsValid.CheckState = CheckState.Checked;
            //给 物流单号 设置焦点
            lblWhere_LB_No.Focus();

            //清空grid数据
            HeadDS = new LogisticsBillManagerUIModel();
            //给下拉框复制
            SetToComboEditor();
            gdGrid.DataSource = HeadDS;
            gdGrid.DataBind();
            base.ClearAction();
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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SD_LogisticsBill.Code.LB_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SD_LogisticsBill.Code.LB_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            HeadDS = HeadGridDS.FirstOrDefault(x => x.LB_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SD_LogisticsBill.Code.LB_ID].Value);

            if (HeadDS == null || string.IsNullOrEmpty(HeadDS.LB_ID))
            {
                return;
            }

            if (txtLB_ID.Text != HeadDS.LB_ID
                || (txtLB_ID.Text == HeadDS.LB_ID && txtLB_VersionNo.Text != HeadDS.LB_VersionNo?.ToString()))
            {
                if (txtLB_ID.Text == HeadDS.LB_ID && txtLB_VersionNo.Text != HeadDS.LB_VersionNo?.ToString())
                {
                    //数据版本已过期，将加载最新详情。
                    MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.DataHasOverdue }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else if (ViewHasChanged()
                || _detailGridDS.InsertList.Count > 0
                || _detailGridDS.UpdateList.Count > 0
                || _detailGridDS.DeleteList.Count > 0)
                {
                    //将放弃之前的修改，是否继续？
                    DialogResult dialogResult = MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.ConfirmGiveUpEdit }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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

            //设置详情页面控件以及导航按钮是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();

            //查询物流单明细
            QueryDetail();

            //设置图片是否可编辑
            SetPictureControl();
            //设置单元格是否可以编辑
            SetLogisticsDetailStyle();
        }
      
        /// <summary>
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //物流单号
            txtLB_No.Text = HeadDS.LB_No;
            //组织名称
            txtLB_Org_Name.Text = HeadDS.LB_Org_Name;
            //来源类型
            cbLB_SourceTypeName.Text = HeadDS.LB_SourceTypeName ?? "";
            //来源类型编码
            cbLB_SourceTypeName.Value = HeadDS.LB_SourceTypeCode ?? "";
            //物流单来源单号
            txtLB_SourceNo.Text = HeadDS.LB_SourceNo;
            //物流人员类型
            cbLB_SourceName.Text = HeadDS.LB_SourceName ?? "";
            //物流人员类型编码
            cbLB_SourceName.Value = HeadDS.LB_SourceCode ?? "";
            //物流人员名称
            mcbLogisticsName.SelectedValue = HeadDS.LB_DeliveryByID + "|" + HeadDS.LB_PhoneNo + "|" + HeadDS.LB_Org_ID;
            //物流人员手机号
            txtLB_PhoneNo.Text = HeadDS.LB_PhoneNo;
            //物流人员接单时间
            dtLB_AcceptTime.Value = HeadDS.LB_AcceptTime;
            //物流人员接单图片路径1
            if (!string.IsNullOrEmpty(HeadDS.LB_AcceptPicPath1))
            {
                pbLB_AcceptPicPath1.PictureKey = HeadDS.LB_AcceptPicPath1;
                pbLB_AcceptPicPath1.PictureImage = BLLCom.GetBitmapImageByFileName(HeadDS.LB_AcceptPicPath1);
            }
            else
            {
                pbLB_AcceptPicPath1.PictureKey = Guid.NewGuid() + ".jpg";
            }
            //物流人员接单图片路径2
            if (!string.IsNullOrEmpty(HeadDS.LB_AcceptPicPath2))
            {
                pbLB_AcceptPicPath2.PictureKey = HeadDS.LB_AcceptPicPath2;
                pbLB_AcceptPicPath2.PictureImage = BLLCom.GetBitmapImageByFileName(HeadDS.LB_AcceptPicPath2);
            }
            else
            {
                pbLB_AcceptPicPath2.PictureKey = Guid.NewGuid() + ".jpg";
            }
            //收件人姓名
            txtLB_Receiver.Text = HeadDS.LB_Receiver;
            //收件人地址
            txtLB_ReceiverAddress.Text = HeadDS.LB_ReceiverAddress;
            //收件人邮编
            txtLB_ReceiverPostcode.Text = HeadDS.LB_ReceiverPostcode;
            //收件人手机号
            txtLB_ReceiverPhoneNo.Text = HeadDS.LB_ReceiverPhoneNo;
            //签收人
            txtLB_ReceivedBy.Text = HeadDS.LB_ReceivedBy;
            //签收时间
            dtLB_ReceivedTime.Value = HeadDS.LB_ReceivedTime;
            //签收拍照图片路径1
            if (!string.IsNullOrEmpty(HeadDS.LB_ReceivedPicPath1))
            {
                pbLB_ReceivedPicPath1.PictureKey = HeadDS.LB_ReceivedPicPath1;
                pbLB_ReceivedPicPath1.PictureImage = BLLCom.GetBitmapImageByFileName(HeadDS.LB_ReceivedPicPath1);
            }
            else
            {
                pbLB_ReceivedPicPath1.PictureKey = Guid.NewGuid() + ".jpg";
            }
            //签收拍照图片路径2
            if (!string.IsNullOrEmpty(HeadDS.LB_ReceivedPicPath2))
            {
                pbLB_ReceivedPicPath2.PictureKey = HeadDS.LB_ReceivedPicPath2;
                pbLB_ReceivedPicPath2.PictureImage = BLLCom.GetBitmapImageByFileName(HeadDS.LB_ReceivedPicPath2);
            }
            else
            {
                pbLB_ReceivedPicPath2.PictureKey = Guid.NewGuid() + ".jpg";
            }
            //物流费
            numLB_Fee.Text = HeadDS.LB_Fee?.ToString();
            //应收金额
            txtLB_AccountReceivableAmount.Text = HeadDS.LB_AccountReceivableAmount?.ToString();
            //赔偿金额
            txtLB_Indemnification.Text = HeadDS.LB_Indemnification?.ToString();
            //物流费付款状态名称
            cbLB_PayStautsName.Text = HeadDS.LB_PayStautsName ?? "";
            //物流费付款状态编码
            cbLB_PayStautsName.Value = HeadDS.LB_PayStautsCode ?? "";
            //单据状态名称
            cbLB_StatusName.Text = HeadDS.LB_StatusName ?? "";
            //单据状态编码
            cbLB_StatusName.Value = HeadDS.LB_StatusCode ?? "";
            //审核状态名称
            cbLB_ApprovalStatusName.Text = HeadDS.LB_ApprovalStatusName ?? "";
            //审核状态编码
            cbLB_ApprovalStatusName.Value = HeadDS.LB_ApprovalStatusCode ?? "";
            //备注
            txtLB_Remark.Text = HeadDS.LB_Remark;
            //有效
            if (HeadDS.LB_IsValid != null)
            {
                ckLB_IsValid.Checked = HeadDS.LB_IsValid.Value;
            }
            //创建人
            txtLB_CreatedBy.Text = HeadDS.LB_CreatedBy;
            //创建时间
            dtLB_CreatedTime.Value = HeadDS.LB_CreatedTime;
            //修改人
            txtLB_UpdatedBy.Text = HeadDS.LB_UpdatedBy;
            //修改时间
            dtLB_UpdatedTime.Value = HeadDS.LB_UpdatedTime;
            //物流订单ID
            txtLB_ID.Text = HeadDS.LB_ID;
            //版本号
            txtLB_VersionNo.Text = HeadDS.LB_VersionNo?.ToString();

            //已收金额
            txtARB_ReceivedAmount.Text = HeadDS.ARB_ReceivedAmount?.ToString();
            //未收金额
            txtARB_UnReceiveAmount.Text = HeadDS.ARB_UnReceiveAmount?.ToString();
        }

        /// <summary>
        /// 查询物流单明细
        /// </summary>
        private void QueryDetail()
        {
            #region 查询出对应的物流单明细数据并绑定到grid中
            if (string.IsNullOrEmpty(txtLB_ID.Text.Trim()))
            {
                return;
            }
            _bll.QueryForList(SQLID.SD_LogisticsBill_SQL01, new MDLSD_LogisticsBillDetail()
            {
                WHERE_LBD_LB_ID = txtLB_ID.Text.Trim()
            }, _detailGridDS);
            _detailGridDS.StartMonitChanges();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();

            //设置明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            #endregion
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
                foreach (UltraGridColumn ugc in gdGrid.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (ugc.IsGroupByColumn)
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
            #region 验证单头
            //请选择来源类型
            if (string.IsNullOrEmpty(cbLB_SourceTypeName.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[]
                    { MsgParam.SOURCE_TYPE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbLB_SourceTypeName.Focus();
                return false;
            }
            //请选择来源单号
            if (string.IsNullOrEmpty(txtLB_SourceNo.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[]
                    { SystemTableColumnEnums.SD_LogisticsBill.Name.LB_SourceNo }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLB_SourceNo.Focus();
                return false;
            }
            //请选择物流人员类型
            if (string.IsNullOrEmpty(cbLB_SourceName.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[]
                    { SystemTableColumnEnums.SD_LogisticsBill.Name.LB_SourceName }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbLB_SourceName.Focus();
                return false;
            }
            //请选择物流人员
            if (string.IsNullOrEmpty(mcbLogisticsName.SelectedText))
            {
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[]
                    { MsgParam.DELIVERYBY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                mcbLogisticsName.Focus();
                return false;
            }
            //请输入有效的物流人员手机号
            if (string.IsNullOrEmpty(txtLB_PhoneNo.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[]
                    { SystemTableColumnEnums.SD_LogisticsBill.Name.LB_PhoneNo }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLB_PhoneNo.Focus();
                txtLB_PhoneNo.SelectAll();
                return false;
            }
            //请选择物流人员接单时间
            if (dtLB_AcceptTime.Value == null)
            {
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[]
                {SystemTableColumnEnums.SD_LogisticsBill.Name.LB_AcceptTime}), MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                dtLB_AcceptTime.Focus();
                return false;
            }
            //收件人不能为空
            if (string.IsNullOrEmpty(txtLB_Receiver.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.SD_LogisticsBill.Name.LB_Receiver }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLB_Receiver.Focus();
                return false;
            }
            //请输入有效的收件人手机号
            if (string.IsNullOrEmpty(txtLB_ReceiverPhoneNo.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.SD_LogisticsBill.Name.LB_ReceiverPhoneNo }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLB_ReceiverPhoneNo.Focus();
                return false;
            }
            //收件人地址不能为空
            if (string.IsNullOrEmpty(txtLB_ReceiverAddress.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.SD_LogisticsBill.Name.LB_ReceiverAddress }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLB_ReceiverAddress.Focus();
                return false;
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
            if (string.IsNullOrEmpty(txtLB_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.W_0016, new object[] { SystemTableEnums.Name.SD_LogisticsBill, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //确认删除操作
            DialogResult dialogResult = MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0012), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.OK)
            {
                return false;
            }
            return true;
        }
   
        /// <summary>
        /// 前端检查-审核
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForApprove()
        {
            #region 验证数据
            //物流单未保存,不能审核
            if (string.IsNullOrEmpty(txtLB_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.SD_LogisticsBill + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //物流单未保存,不能审核
            if (cbLB_StatusName.Text.Trim() == LogisticsBillStatusEnum.Name.YGB
                || cbLB_StatusName.Text.Trim() == LogisticsBillStatusEnum.Name.YWC)
            {
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.SD_LogisticsBill + cbLB_StatusName.Text.Trim(), SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //查询出对应物流单
            MDLSD_LogisticsBill resultLogisticsBill = new MDLSD_LogisticsBill();
            _bll.QueryForObject<MDLSD_LogisticsBill, MDLSD_LogisticsBill>(new MDLSD_LogisticsBill()
            {
                WHERE_LB_IsValid = true,
                WHERE_LB_ID = txtLB_ID.Text.Trim()
            }, resultLogisticsBill);
            //物流单不存在,不能审核
            if (string.IsNullOrEmpty(resultLogisticsBill.LB_ID))
            {
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.SD_LogisticsBill + MsgParam.NOTEXIST, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //确认审核操作
            DialogResult isConfirmApprove = MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.W_0014), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isConfirmApprove != DialogResult.OK)
            {
                return false;
            }

            #endregion

            return true;
        }

        /// <summary>
        /// 前端检查-反审核
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForUnApprove()
        {
            #region 验证数据
            //物流单未保存,不能反审核
            if (string.IsNullOrEmpty(txtLB_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.SD_LogisticsBill + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            MDLSD_LogisticsBill resulLogisticsBill = new MDLSD_LogisticsBill();
            _bll.QueryForObject<MDLSD_LogisticsBill, MDLSD_LogisticsBill>(new MDLSD_LogisticsBill()
            {
                WHERE_LB_IsValid = true,
                WHERE_LB_ID = txtLB_ID.Text.Trim()
            }, resulLogisticsBill);
            //物流单不存在,不能反审核
            if (string.IsNullOrEmpty(resulLogisticsBill.LB_ID))
            {
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.SD_LogisticsBill + MsgParam.NOTEXIST, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //验证配件是否已被签收
            if (_detailGridDS.Any(x => x.LBD_SignQty > 0))
            {
                //配件已签收，不能反审核
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { MsgParam.AUTOPARTS + MsgParam.ALREADY_SIGN, SystemActionEnum.Name.UNAPPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //确认反审核操作
            DialogResult isConfirmUnApprove = MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.W_0018), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isConfirmUnApprove != DialogResult.OK)
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
            HeadDS = new LogisticsBillManagerUIModel()
            {
                //物流单号
                LB_No = txtLB_No.Text.Trim(),
                //组织名称
                LB_Org_Name = txtLB_Org_Name.Text.Trim(),
                //来源类型
                LB_SourceTypeName = cbLB_SourceTypeName.Text,
                //物流单来源单号
                LB_SourceNo = txtLB_SourceNo.Text.Trim(),
                //物流人员类型
                LB_SourceName = cbLB_SourceName.Text,
                //物流人员手机号
                LB_PhoneNo = txtLB_PhoneNo.Text.Trim(),
                //物流人员接单时间
                LB_AcceptTime = (DateTime?)dtLB_AcceptTime.Value ?? DateTime.Now,
                //物流人员接单图片路径1
                LB_AcceptPicPath1 = pbLB_AcceptPicPath1.PictureImage == null ? "" : pbLB_AcceptPicPath1.PictureKey,
                //物流人员接单图片路径2
                LB_AcceptPicPath2 = pbLB_AcceptPicPath2.PictureImage == null ? "" : pbLB_AcceptPicPath2.PictureKey,
                //收件人姓名
                LB_Receiver = txtLB_Receiver.Text.Trim(),
                //收件人地址
                LB_ReceiverAddress = txtLB_ReceiverAddress.Text.Trim(),
                //收件人邮编
                LB_ReceiverPostcode = txtLB_ReceiverPostcode.Text.Trim(),
                //收件人手机号
                LB_ReceiverPhoneNo = txtLB_ReceiverPhoneNo.Text.Trim(),
                //签收人
                LB_ReceivedBy = txtLB_ReceivedBy.Text.Trim(),
                //签收时间
                LB_ReceivedTime = (DateTime?)dtLB_ReceivedTime.Value ?? DateTime.Now,
                //签收拍照图片路径1
                LB_ReceivedPicPath1 = pbLB_ReceivedPicPath1.PictureImage == null ? "" : pbLB_ReceivedPicPath1.PictureKey,
                //签收拍照图片路径2
                LB_ReceivedPicPath2 = pbLB_ReceivedPicPath2.PictureImage == null ? "" : pbLB_ReceivedPicPath2.PictureKey,
                //物流费
                LB_Fee = Convert.ToDecimal(numLB_Fee.Value ?? 0),
                //应收金额
                LB_AccountReceivableAmount = Convert.ToDecimal(txtLB_AccountReceivableAmount.Text.Trim() == "" ? "0" : txtLB_AccountReceivableAmount.Text.Trim()),
                //赔偿金额
                LB_Indemnification = Convert.ToDecimal(txtLB_Indemnification.Text.Trim() == "" ? "0" : txtLB_Indemnification.Text.Trim()),
                //物流费付款状态名称
                LB_PayStautsName = cbLB_PayStautsName.Text,
                //单据状态名称
                LB_StatusName = cbLB_StatusName.Text,
                //审核状态名称
                LB_ApprovalStatusName = cbLB_ApprovalStatusName.Text,
                //备注
                LB_Remark = txtLB_Remark.Text.Trim(),
                //有效
                LB_IsValid = ckLB_IsValid.Checked,
                //创建人
                LB_CreatedBy = txtLB_CreatedBy.Text.Trim(),
                //创建时间
                LB_CreatedTime = (DateTime?)dtLB_CreatedTime.Value ?? DateTime.Now,
                //修改人
                LB_UpdatedBy = txtLB_UpdatedBy.Text.Trim(),
                //修改时间
                LB_UpdatedTime = (DateTime?)dtLB_UpdatedTime.Value ?? DateTime.Now,
                //来源类型编码
                LB_SourceTypeCode = cbLB_SourceTypeName.Value?.ToString(),
                //物流人员类型编码
                LB_SourceCode = cbLB_SourceName.Value?.ToString(),
                //物流费付款状态编码
                LB_PayStautsCode = cbLB_PayStautsName.Value?.ToString(),
                //单据状态编码
                LB_StatusCode = cbLB_StatusName.Value?.ToString(),
                //单据状态编码
                LB_ApprovalStatusCode = cbLB_ApprovalStatusName.Value?.ToString(),
                //物流订单ID
                LB_ID = txtLB_ID.Text.Trim(),
                //组织ID
                LB_Org_ID = txtLB_Org_ID.Text.Trim(),
                //版本号
                LB_VersionNo = Convert.ToInt64(txtLB_VersionNo.Text.Trim() == "" ? "1" : txtLB_VersionNo.Text.Trim()),

                //已收金额
                ARB_ReceivedAmount = Convert.ToDecimal(txtARB_ReceivedAmount.Text.Trim() == "" ? "0" : txtARB_ReceivedAmount.Text.Trim()),
                //未收金额
                ARB_UnReceiveAmount = Convert.ToDecimal(txtARB_UnReceiveAmount.Text.Trim() == "" ? "0" : txtARB_UnReceiveAmount.Text.Trim()),
            };
            //物流人员名称
            string[] dispalyInfoArray = this.mcbLogisticsName.SelectedText.Split('|');
            if (dispalyInfoArray.Length >= 1)
            {
                HeadDS.LB_DeliveryBy = dispalyInfoArray[0];
            }
            //物流人员ID
            string[] valueInfoArray = this.mcbLogisticsName.SelectedValue.Split('|');
            if (valueInfoArray.Length >= 1)
            {
                HeadDS.LB_DeliveryByID = valueInfoArray[0];
            }
        }

        /// <summary>
        /// 给下拉框赋值
        /// </summary>
        private void SetToComboEditor()
        {
            //来源类型【新增时只考虑主动销售，在线销售，直接销售】
            _logisticsBillSourceTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.DeliveryBillSourceType);
            cbWhere_LB_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_LB_SourceTypeName.ValueMember = SysConst.EN_Code;
            cbWhere_LB_SourceTypeName.DataSource = _logisticsBillSourceTypeList;
            cbWhere_LB_SourceTypeName.DataBind();

            cbLB_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
            cbLB_SourceTypeName.ValueMember = SysConst.EN_Code;
            cbLB_SourceTypeName.DataSource = _logisticsBillSourceTypeList;
            cbLB_SourceTypeName.Text = DeliveryBillSourceTypeEnum.Name.ZJXS;
            cbLB_SourceTypeName.Value = DeliveryBillSourceTypeEnum.Code.ZJXS;
            cbLB_SourceTypeName.DataBind();

            //支付状态【已支付，未支付】
            List<ComComboBoxDataSourceTC> resultPayStautsList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.PaymentStatus);
            cbLB_PayStautsName.DisplayMember = SysConst.EN_TEXT;
            cbLB_PayStautsName.ValueMember = SysConst.EN_Code;
            cbLB_PayStautsName.DataSource = resultPayStautsList;
            cbLB_PayStautsName.Text = PaymentStatusEnum.Name.WZF;
            cbLB_PayStautsName.Value = PaymentStatusEnum.Code.WZF;
            cbLB_PayStautsName.DataBind();

            //单据类型【已生成，配送中，已签收，部分签收，已拒收，已关闭，已完成】
            List<ComComboBoxDataSourceTC> resultStautsList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.LogisticsBillStatus);
            cbWhere_LB_StatusName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_LB_StatusName.ValueMember = SysConst.EN_Code;
            cbWhere_LB_StatusName.DataSource = resultStautsList;
            cbWhere_LB_StatusName.DataBind();

            cbLB_StatusName.DisplayMember = SysConst.EN_TEXT;
            cbLB_StatusName.ValueMember = SysConst.EN_Code;
            cbLB_StatusName.DataSource = resultStautsList;
            cbLB_StatusName.Text = LogisticsBillStatusEnum.Name.YSC;
            cbLB_StatusName.Value = LogisticsBillStatusEnum.Code.YSC;
            cbLB_StatusName.DataBind();

            //物流人员
            _bll.CopyModelList(_tempAllClientList, _clientList);
            mcbLogisticsName.ExtraDisplayMember = "ClientType";
            mcbLogisticsName.DisplayMember = "ClientName";
            mcbLogisticsName.ValueMember = "ClientID";
            mcbLogisticsName.DataSource = _clientList;

            //物流人员类型【员工，第三方个人，第三方公司】
            _deliveryTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.DeliveryType);
            cbLB_SourceName.DisplayMember = SysConst.EN_TEXT;
            cbLB_SourceName.ValueMember = SysConst.EN_Code;
            cbLB_SourceName.DataSource = _deliveryTypeList;
            cbLB_SourceName.DataBind();

            //审核状态名称【待审核，已审核】
            _approveStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.ApprovalStatus);
            cbWhere_LB_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_LB_ApprovalStatusName.ValueMember = SysConst.EN_Code;
            cbWhere_LB_ApprovalStatusName.DataSource = _approveStatusList;
            cbWhere_LB_ApprovalStatusName.DataBind();

            cbLB_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
            cbLB_ApprovalStatusName.ValueMember = SysConst.EN_Code;
            cbLB_ApprovalStatusName.DataSource = _approveStatusList;
            cbLB_ApprovalStatusName.Text = ApprovalStatusEnum.Name.DSH;
            cbLB_ApprovalStatusName.Value = ApprovalStatusEnum.Code.DSH;
            cbLB_ApprovalStatusName.DataBind();

            //获取当前组织的id和名称
            txtLB_Org_ID.Text = LoginInfoDAX.OrgID;
            txtLB_Org_Name.Text = LoginInfoDAX.OrgShortName;
        }

        /// <summary>
        /// 打印
        /// </summary>
        public void Print()
        {
            if (HeadDS == null || string.IsNullOrEmpty(HeadDS.LB_No))
            {
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, SystemTableEnums.Name.SD_LogisticsBill), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //物流单待审核，不能打印
            if (HeadDS.LB_ApprovalStatusName != ApprovalStatusEnum.Name.YSH)
            {
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, SystemTableEnums.Name.SD_LogisticsBill + cbLB_ApprovalStatusName.Text, SystemActionEnum.Name.PRINT), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                //待打印的物流单
                LogisticsBillUIModelToPrint logisticsBillToPrint = new LogisticsBillUIModelToPrint();
                _bll.CopyModel(HeadDS, logisticsBillToPrint);
                //待打印的物流单明细
                List<LogisticsBillDetailUIModelToPrint> logisticsBillDetailToPrintsList = new List<LogisticsBillDetailUIModelToPrint>();
                _bll.CopyModelList(_detailGridDS, logisticsBillDetailToPrintsList);
                Dictionary<string, object> argsViewParams = new Dictionary<string, object>
                {
                    //物流单
                    {SDViewParamKey.LogisticsBill.ToString(), logisticsBillToPrint},
                    //物流单明细
                    {SDViewParamKey.LogisticsBillDetail.ToString(), logisticsBillDetailToPrintsList},
                };
                FrmViewAndPrintLogisticsBill frmViewAndPrintLogisticsBill = new FrmViewAndPrintLogisticsBill(argsViewParams);
                DialogResult dialogResult = frmViewAndPrintLogisticsBill.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBoxs.Show(Trans.SD, ToString(), ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// 设置详情页面控件的是否可编辑
        /// </summary>
        private void SetDetailControl()
        {
            if (cbLB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                #region 物流单为[已审核]的场合

                #region 单头相关

                //单头不可编辑
                cbLB_SourceTypeName.Enabled = false;
                txtLB_SourceNo.Enabled = false;

                cbLB_SourceName.Enabled = false;
                mcbLogisticsName.Enabled = false;
                txtLB_PhoneNo.Enabled = false;
                dtLB_AcceptTime.Enabled = false;

                txtLB_ReceiverPhoneNo.Enabled = false;
                txtLB_ReceiverAddress.Enabled = false;
                txtLB_ReceiverPostcode.Enabled = false;

                txtLB_ReceivedBy.Enabled = false;
                dtLB_ReceivedTime.Enabled = false;

                cbLB_PayStautsName.Enabled = false;
                numLB_Fee.Enabled = false;
                txtLB_Remark.Enabled = false;

                #endregion

                //物流单{已审核}的场合，显示[签收数量]、[拒收数量]、[丢失数量]
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_SignQty].Hidden = false;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_RejectQty].Hidden = false;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_LoseQty].Hidden = false;

                #endregion
            }
            else
            {
                #region 物流单未保存 或 待审核的场合

                #region 单头相关

                //单头可编辑
                cbLB_SourceTypeName.Enabled = true;
                txtLB_SourceNo.Enabled = true;

                cbLB_SourceName.Enabled = true;
                mcbLogisticsName.Enabled = true;
                txtLB_PhoneNo.Enabled = true;
                dtLB_AcceptTime.Enabled = true;

                txtLB_ReceiverPhoneNo.Enabled = true;
                txtLB_ReceiverAddress.Enabled = true;
                txtLB_ReceiverPostcode.Enabled = true;

                numLB_Fee.Enabled = true;
                cbLB_PayStautsName.Enabled = true;
                txtLB_Remark.Enabled = true;

                if (string.IsNullOrEmpty(txtLB_ID.Text))
                {
                    //物流单未保存的场合
                    //签收人和签收时间不可编辑
                    txtLB_ReceivedBy.Enabled = false;
                    dtLB_ReceivedTime.Enabled = false;
                }
                else
                {
                    //物流单{待审核}的场合
                    //签收人和签收时间可编辑
                    txtLB_ReceivedBy.Enabled = true;
                    dtLB_ReceivedTime.Enabled = true;
                }
                #endregion

                #region 物流单明细列表相关

                //明细列表不显示[签收数量]、[拒收数量]、[丢失数量]
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_SignQty].Hidden = true;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_RejectQty].Hidden = true;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_LoseQty].Hidden = true;
                #endregion

                #endregion
            }
        }

        /// <summary>
        /// 设置动作按钮状态
        /// </summary>
        private void SetActionEnableByStatus()
        {
            if (cbLB_StatusName.Text == LogisticsBillStatusEnum.Name.YWC ||
                cbLB_StatusName.Text == LogisticsBillStatusEnum.Name.YGB)
            {
                #region [物流单].[单据状态]为{已完成}、{已关闭}的场合
                //[保存]、[审核]、[反审核]不可用
                SetActionEnable(SystemActionEnum.Code.SAVE, false);
                SetActionEnable(SystemActionEnum.Code.APPROVE, false);
                SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);

                //{已关闭}的场合，[删除]可用，否则不可用
                SetActionEnable(SystemActionEnum.Code.DELETE, cbLB_StatusName.Text == LogisticsBillStatusEnum.Name.YGB);

                #endregion
            }
            else
            {
                #region [物流单].[单据状态]不是{已完成}、{已关闭}的场合

                if (cbLB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
                {
                    //[审核状态]为[已审核]的场合，[保存]、[删除]、[审核]不可用，[反审核]、[打印]可用
                    SetActionEnable(SystemActionEnum.Code.SAVE, false);
                    SetActionEnable(SystemActionEnum.Code.DELETE, false);
                    SetActionEnable(SystemActionEnum.Code.APPROVE, false);

                    SetActionEnable(SystemActionEnum.Code.UNAPPROVE, true);
                    SetActionEnable(SystemActionEnum.Code.PRINT, true);
                }
                else
                {
                    //新增或[审核状态]为[待审核]的场合，[保存]、[删除]、[审核]可用，[反审核]、[打印]不可用
                    SetActionEnable(SystemActionEnum.Code.SAVE, true);
                    SetActionEnable(SystemActionEnum.Code.DELETE, !string.IsNullOrEmpty(txtLB_ID.Text));
                    SetActionEnable(SystemActionEnum.Code.APPROVE, !string.IsNullOrEmpty(txtLB_ID.Text));

                    SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);
                    SetActionEnable(SystemActionEnum.Code.PRINT, false);
                }
                #endregion
            }
        }

        /// <summary>
        /// 设置明细Grid单元格是否可以编辑
        /// </summary>
        private void SetLogisticsDetailStyle()
        {
            #region 设置Grid数据颜色
            gdDetail.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;

            foreach (var loopGridRow in gdDetail.Rows)
            {
                if (cbLB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
                {
                    #region 物流单为[已审核]的场合


                    if (cbLB_StatusName.Text == LogisticsBillStatusEnum.Name.YWC
                        || cbLB_StatusName.Text == LogisticsBillStatusEnum.Name.YGB)
                    {
                        #region 物流单.[单据状态]是{已完成}、{已关闭}的场合

                        #region 签收数量

                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_SignQty].Activation = Activation.ActivateOnly;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_SignQty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_SignQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                        #endregion

                        #region 拒收数量

                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_RejectQty].Activation = Activation.ActivateOnly;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_RejectQty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_RejectQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                        #endregion

                        #region 丢失数量

                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_LoseQty].Activation = Activation.ActivateOnly;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_LoseQty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_LoseQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                        #endregion

                        #region 备注

                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_Remark].Activation = Activation.ActivateOnly;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_Remark].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_Remark].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                        #endregion

                        #endregion

                    }
                    else
                    {
                        #region 物流单.[单据状态]不是{已完成}、{已关闭}的场合

                        #region 签收数量

                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_SignQty].Activation = Activation.AllowEdit;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_SignQty].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_SignQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                        #endregion

                        #region 拒收数量

                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_RejectQty].Activation = Activation.AllowEdit;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_RejectQty].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_RejectQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                        #endregion

                        #region 丢失数量

                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_LoseQty].Activation = Activation.AllowEdit;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_LoseQty].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_LoseQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                        #endregion

                        #region 备注

                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_Remark].Activation = Activation.AllowEdit;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_Remark].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_Remark].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                        #endregion

                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region 物流单未保存 或 待审核的场合

                    #region 备注

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_Remark].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_Remark].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_LogisticsBillDetail.Code.LBD_Remark].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #endregion
                }
            }
            #endregion
        }

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
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0018, new object[] { MsgParam.UPLOAD_IMAGE, ex.Message }), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            MDLSD_LogisticsBill resultLogisticsBill = new MDLSD_LogisticsBill();
            resultLogisticsBill = _bll.QueryForObject<MDLSD_LogisticsBill>(SQLID.SD_LogisticsBill_SQL02, paramPictureKey);
            if (resultLogisticsBill != null
                && !string.IsNullOrEmpty(resultLogisticsBill.LB_ID))
            {
                //错误消息
                var outMsg = string.Empty;
                //删除本地和文件服务器中的图片
                bool deleteResult = BLLCom.DeleteFileByFileName(paramPictureKey, ref outMsg);
                if (deleteResult == false)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.SD, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                //更新物流单
                resultLogisticsBill.WHERE_LB_ID = resultLogisticsBill.LB_ID;
                resultLogisticsBill.WHERE_LB_VersionNo = resultLogisticsBill.LB_VersionNo;
                if (resultLogisticsBill.LB_AcceptPicPath1 == paramPictureKey)
                {
                    resultLogisticsBill.LB_AcceptPicPath1 = null;
                    pbLB_AcceptPicPath1.PictureKey = Guid.NewGuid() + ".jpg";
                }
                else if (resultLogisticsBill.LB_AcceptPicPath2 == paramPictureKey)
                {
                    resultLogisticsBill.LB_AcceptPicPath2 = null;
                    pbLB_AcceptPicPath2.PictureKey = Guid.NewGuid() + ".jpg";
                }
                else if (resultLogisticsBill.LB_ReceivedPicPath1 == paramPictureKey)
                {
                    resultLogisticsBill.LB_ReceivedPicPath1 = null;
                    pbLB_ReceivedPicPath1.PictureKey = Guid.NewGuid() + ".jpg";
                }
                else if (resultLogisticsBill.LB_ReceivedPicPath2 == paramPictureKey)
                {
                    resultLogisticsBill.LB_ReceivedPicPath2 = null;
                    pbLB_ReceivedPicPath2.PictureKey = Guid.NewGuid() + ".jpg";
                }

                bool updateLogisticDetailResult = _bll.Update(SQLID.SD_LogisticsBill_SQL04, resultLogisticsBill);
                if (!updateLogisticDetailResult)
                {
                    //删除失败
                    MessageBoxs.Show(Trans.SD, ToString(), outMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                //删除成功
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (_dicPictureNameAndTempPath.ContainsKey(paramPictureKey))
                {
                    _dicPictureNameAndTempPath.Remove(paramPictureKey);
                }

                //更新版本号
                txtLB_VersionNo.Text = ((resultLogisticsBill.LB_VersionNo ?? 0) + 1).ToString();
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
                //物流单可保存、审核的场合，可上传图片
                //有图片的场合，删除可见
                pbLB_AcceptPicPath1.UploadIsVisible = true;
                pbLB_AcceptPicPath1.DeleteIsVisible = pbLB_AcceptPicPath1.PictureImage != null;

                pbLB_AcceptPicPath2.UploadIsVisible = true;
                pbLB_AcceptPicPath2.DeleteIsVisible = pbLB_AcceptPicPath2.PictureImage != null;

                pbLB_ReceivedPicPath1.UploadIsVisible = true;
                pbLB_ReceivedPicPath1.DeleteIsVisible = pbLB_ReceivedPicPath1.PictureImage != null;

                pbLB_ReceivedPicPath2.UploadIsVisible = true;
                pbLB_ReceivedPicPath2.DeleteIsVisible = pbLB_ReceivedPicPath2.PictureImage != null;

            }
            else
            {
                //库存图片不可保存、审核的场合，不可上传、删除图片
                pbLB_AcceptPicPath1.UploadIsVisible = false;
                pbLB_AcceptPicPath1.DeleteIsVisible = false;

                pbLB_AcceptPicPath2.UploadIsVisible = false;
                pbLB_AcceptPicPath2.DeleteIsVisible = false;

                pbLB_ReceivedPicPath1.UploadIsVisible = false;
                pbLB_ReceivedPicPath1.DeleteIsVisible = false;

                pbLB_ReceivedPicPath2.UploadIsVisible = false;
                pbLB_ReceivedPicPath2.DeleteIsVisible = false;
            }
            //有图片的场合，导出可见
            pbLB_AcceptPicPath1.ExportIsVisible = pbLB_AcceptPicPath1.PictureImage != null;
            pbLB_AcceptPicPath2.ExportIsVisible = pbLB_AcceptPicPath2.PictureImage != null;
            pbLB_ReceivedPicPath1.ExportIsVisible = pbLB_ReceivedPicPath1.PictureImage != null;
            pbLB_ReceivedPicPath2.ExportIsVisible = pbLB_ReceivedPicPath2.PictureImage != null;
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
                    var curHead = HeadGridDS.FirstOrDefault(x => x.LB_ID == HeadDS.LB_ID);
                    if (curHead != null)
                    {
                        HeadGridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = HeadGridDS.FirstOrDefault(x => x.LB_ID == HeadDS.LB_ID);
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
