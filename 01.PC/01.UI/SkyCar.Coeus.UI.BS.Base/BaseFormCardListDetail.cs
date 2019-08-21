using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UI.Base
{
    /// <summary>
    /// 基础窗体（列表-详情）
    /// </summary>
    /// <typeparam name="HeadUIModelType">【列表】Tab内Grid绑定用的数据源类型</typeparam>
    /// <typeparam name="QCModelType">【新增/详情】Tab内控件绑定用的数据源类型</typeparam>
    /// <typeparam name="HeadTBModeType">单头对应的TBModel类型（QCModel不指定SQLID的场合，Base.QueryAction()方法查询TBModel对应的数据表）</typeparam>
    public class BaseFormCardListDetail<HeadUIModelType, QCModelType, HeadTBModeType> : Form
    {
        #region 私有变量

        /// <summary>
        /// BLLBase
        /// </summary>
        private BLLBase _bll = new BLLBase(Trans.COM);
        //翻页按钮ToolBar
        private UltraToolbarsManager _toolBarPaging = new UltraToolbarsManager();
        //动作按钮ToolBar
        private UltraToolbarsManager _toolBarActionAndNavigate = new UltraToolbarsManager();
        //当前画面的动作权限列表
        private List<MenuGroupActionUIModel> _menuGroupActionList = new List<MenuGroupActionUIModel>();
        #endregion

        #region 公共属性
        /// <summary>
        /// 当前界面的菜单动作列表
        /// </summary>
        public List<MenuGroupActionUIModel> MenuGroupActionList
        {
            get { return _menuGroupActionList; }
        }
        /// <summary>
        /// 翻页ToolBar
        /// </summary>
        public UltraToolbarsManager ToolBarPaging
        {
            get { return _toolBarPaging; }
            set
            {
                _toolBarPaging = value;
                //初始化翻页
                SetBarPaging(TotalRecordCount);
            }
        }

        /// <summary>
        /// 动作按钮ToolBar
        /// </summary>
        public UltraToolbarsManager ToolBarActionAndNavigate
        {
            get { return _toolBarActionAndNavigate; }
            set
            {
                _toolBarActionAndNavigate = value;
                //初始化动作按钮
                InitializeAction();
                //初始化导航按钮
                InitializeNavigate();
            }

        }

        /// <summary>
        /// 【列表】Tab内Grid绑定用的数据源
        /// </summary>
        public BindingList<HeadUIModelType> HeadGridDS = new BindingList<HeadUIModelType>();

        /// <summary>
        /// 【新增/详情】Tab内控件绑定用的数据源
        /// </summary>
        public HeadUIModelType HeadDS;

        /// <summary>
        /// 【列表】Tab内查询条件绑定用的数据源
        /// </summary>
        public QCModelType ConditionDS;

        #region 列表Grid翻页用
        /// <summary>
        /// 执行子类中的查询方法
        /// </summary>
        public Action ExecuteQuery;
        /// <summary>
        /// 当前页面索引值
        /// </summary>
        public int PageIndex = 1;
        /// <summary>
        /// 总记录条数
        /// </summary>
        public int TotalRecordCount = 0;
        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize = 40;
        /// <summary>
        /// 总页面数
        /// </summary>
        public int TotalPageCount = 0;
        #endregion

        #region 【详情】Model相关
        /// <summary>
        /// 旧UIModel
        /// </summary>
        public object OrginalUIModel;

        /// <summary>
        /// 新UIModel
        /// </summary>
        public object NewUIModel;

        private HeadUIModelType _headDsCopy;
        /// <summary>
        /// 【详情】Tab内控件绑定用的数据源备份
        /// </summary>
        public HeadUIModelType HeadDSCopy
        {
            get { return _headDsCopy; }
            set
            {
                _headDsCopy = value;
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// 基础窗体（列表-详情）
        /// </summary>
        public BaseFormCardListDetail()
        {
            //实例化ConditionDS
            ConditionDS = Activator.CreateInstance<QCModelType>();
        }

        #region 画面动作和导航相关方法

        /// <summary>
        /// 工具栏（动作-导航）单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="Exception"></exception>
        public void toolBarActionAndNavigate_ToolClick(object sender, ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                #region 动作按钮

                case SystemActionEnum.Code.NEW:
                    NewAction();
                    break;
                case SystemActionEnum.Code.SAVE:
                    SaveAction();
                    break;
                case SystemActionEnum.Code.COPY:
                    CopyAction();
                    break;
                case SystemActionEnum.Code.DELETE:
                    DeleteAction();
                    break;
                case SystemActionEnum.Code.QUERY:
                    QueryAction();
                    break;
                case SystemActionEnum.Code.CLEAR:
                    ClearAction();
                    break;
                case SystemActionEnum.Code.APPROVE:
                    ApproveAction();
                    break;
                case SystemActionEnum.Code.UNAPPROVE:
                    UnApproveAction();
                    break;
                case SystemActionEnum.Code.EXPORT:
                    ExportAction(null);
                    break;
                case SysConst.ExportCurrentPage:
                    ExportAction(null);
                    break;
                case SysConst.ExportAll:
                    ExportAllAction(null);
                    break;
                case SystemActionEnum.Code.IMPORT:
                    ImportAction();
                    break;
                case SystemActionEnum.Code.PRINT:
                    PrintAction();
                    break;
                case SystemActionEnum.Code.SENDMSG:
                    SendMsgAction();
                    break;
                case SystemActionEnum.Code.SIGNIN:
                    SignInAction();
                    break;
                case SystemActionEnum.Code.INVENTORYRECTIFY:
                    AdjustAction();
                    break;
                case SystemActionEnum.Code.STARTSTOCKTASK:
                    StartStockTaskAction();
                    break;
                case SystemActionEnum.Code.SYNCHRONIZE:
                    SynchronizeAction();
                    break;
                case SystemActionEnum.Code.CANCELCERTIFICATION:
                    CancelCertificationAction();
                    break;
                case SystemActionEnum.Code.DELIVER:
                    DeliverAction();
                    break;
                case SystemActionEnum.Code.CANCELDELIVER:
                    CancelDeliverAction();
                    break;
                case SystemActionEnum.Code.ANALYSE:
                    AnalyseAction();
                    break;
                case SystemActionEnum.Code.VERIFY:
                    VerifyAction();
                    break;
                case SystemActionEnum.Code.RECONCILIATION:
                    ReconciliationAction();
                    break;

                #endregion

                #region 导航按钮

                case SystemNavigateEnum.Code.TOSALESORDER:
                    //转销售
                    ToSalesOrderNavigate();
                    break;
                case SystemNavigateEnum.Code.TOLOGISTICSBILL:
                    //转物流
                    ToLogisticsBillNavigate();
                    break;
                case SystemNavigateEnum.Code.TOSTOCKINBILL:
                    //转入库
                    ToStockInBillNavigate();
                    break;
                case SystemNavigateEnum.Code.ONLINEPAY:
                    //在线支付
                    OnLinePayNavigate();
                    break;
                case SystemNavigateEnum.Code.PRINTBARCODE:
                    //打印条码
                    PrintBarCodeNavigate();
                    break;
                case SystemNavigateEnum.Code.TORECEIPTBILL:
                    //转收款
                    ToReceiptBillNavigate();
                    break;
                case SystemNavigateEnum.Code.TOPAYBILL:
                    //转付款
                    ToPayBillNavigate();
                    break;
                case SystemNavigateEnum.Code.TOPURCHASEORDER:
                    //转采购
                    ToPurchaseOrderNavigate();
                    break;
                case SystemNavigateEnum.Code.TOSTOCKOUTBILL:
                    //转出库
                    ToStockOutBillNavigate();
                    break;
                case SystemNavigateEnum.Code.ALLSIGN:
                    //全部签收
                    AllSignNavigate();
                    break;
                case SystemNavigateEnum.Code.TOSETTLEMENT:
                    //转结算
                    ToSettlementNavigate();
                    break;
                case SystemNavigateEnum.Code.CREATEACCOUNT:
                    //开户
                    WalletCreateAccount();
                    break;
                case SystemNavigateEnum.Code.DEPOSITMONEY:
                    //充值
                    WalletDepositMoney();
                    break;
                #endregion

                default:
                    throw new Exception("非系统导航按钮");
            }

        }

        #region 动作按钮虚方法

        /// <summary>
        ///  新增
        /// </summary>
        public virtual void NewAction() { }

        /// <summary>
        /// 推送消息
        /// </summary>
        public virtual void SendMsgAction() { }

        /// <summary>
        /// 签收
        /// </summary>
        public virtual void SignInAction() { }

        /// <summary>
        /// 校正
        /// </summary>
        public virtual void AdjustAction() { }

        /// <summary>
        /// 启动盘点
        /// </summary>
        public virtual void StartStockTaskAction() { }

        /// <summary>
        /// 同步
        /// </summary>
        public virtual void SynchronizeAction() { }

        /// <summary>
        /// 取消认证
        /// </summary>
        public virtual void CancelCertificationAction() { }

        /// <summary>
        /// 下发
        /// </summary>
        public virtual void DeliverAction() { }

        /// <summary>
        /// 取消下发
        /// </summary>
        public virtual void CancelDeliverAction() { }

        /// <summary>
        /// 损益分析
        /// </summary>
        public virtual void AnalyseAction() { }

        /// <summary>
        /// 保存
        /// </summary>
        public virtual void SaveAction() { }

        /// <summary>
        /// 复制
        /// </summary>
        public virtual void CopyAction() { }

        /// <summary>
        /// 删除
        /// </summary>
        public virtual void DeleteAction() { }

        /// <summary>
        /// 查询
        /// </summary>
        public virtual void QueryAction()
        {
            try
            {
                dynamic argsModel = ConditionDS;
                argsModel.PageSize = PageSize;
                argsModel.PageIndex = PageIndex;

                if (String.IsNullOrEmpty(argsModel.SqlId))
                {
                    _bll.QueryForListLike<HeadTBModeType, HeadUIModelType>(argsModel, HeadGridDS);
                }
                else
                {
                    _bll.QueryForList<HeadUIModelType>(argsModel.SqlId, argsModel, HeadGridDS);
                }

                if (HeadGridDS.Count > 0)
                {
                    dynamic subObject = HeadGridDS[0];

                    TotalRecordCount = subObject.RecordCount;
                }
                else
                {
                    TotalRecordCount = 0;
                }
                //设置翻页控件
                SetBarPaging(TotalRecordCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 清空查询条件
        /// </summary>
        public virtual void ClearAction() { }

        /// <summary>
        /// 审核
        /// </summary>
        public virtual void ApproveAction() { }

        /// <summary>
        /// 反审核
        /// </summary>
        public virtual void UnApproveAction() { }

        /// <summary>
        /// Grid数据导出当前页
        /// <para>此方法需要被重写后调用</para>
        /// <para>一行代码调用即可：base.ExportAction(gdGrid);</para>
        /// </summary>
        /// <param name="paramGrid">需要导出的UltraGrid对象</param>
        /// <param name="paramGridName">需要导出的Grid的名称</param>
        public virtual void ExportAction(UltraGrid paramGrid, string paramGridName = "")
        {
            if (paramGrid == null)
            {
                return;
            }
            if (paramGridName == null)
            {
                return;
            }
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    FileName = $"Export{paramGridName + DateTime.Now.ToString(FormatConst.DATE_TIME_FORMAT_03)}",
                    Filter = FormatConst.FILE_TYPE_FILTER_EXCEL_03,
                };
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                var gdGridExcelExporter = new UltraGridExcelExporter();
                gdGridExcelExporter.Export(paramGrid).Save(saveFileDialog.FileName);

                MessageBox.Show("导出成功！", SysConst.CHS_MSG_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败！", SysConst.CHS_MSG_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Grid数据导出全部
        /// <para>此方法需要被重写后调用</para>
        /// <para>一行代码调用即可：base.ExportAllAction(gdGrid);</para>
        /// </summary>
        /// <param name="paramGrid">需要导出的UltraGrid对象</param>
        /// <param name="paramGridName">需要导出的Grid的名称</param>
        public virtual void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            if (paramGrid == null)
            {
                return;
            }
            if (paramGridName == null)
            {
                return;
            }
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    FileName = $"Export{paramGridName + DateTime.Now.ToString(FormatConst.DATE_TIME_FORMAT_03)}_All",
                    Filter = FormatConst.FILE_TYPE_FILTER_EXCEL_03,
                };
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                var gdGridExcelExporter = new UltraGridExcelExporter();
                gdGridExcelExporter.Export(paramGrid).Save(saveFileDialog.FileName);

                MessageBox.Show("导出成功！", SysConst.CHS_MSG_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败！", SysConst.CHS_MSG_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 导入
        /// </summary>
        public virtual void ImportAction() { }
        /// <summary>
        /// 打印
        /// </summary>
        public virtual void PrintAction() { }

        /// <summary>
        /// 核实
        /// </summary>
        public virtual void VerifyAction() { }

        /// <summary>
        /// 对账
        /// </summary>
        public virtual void ReconciliationAction() { }

        #endregion

        #region 导航按钮虚方法

        /// <summary>
        /// 转销售
        /// </summary>
        public virtual void ToSalesOrderNavigate() { }
        /// <summary>
        /// 转物流
        /// </summary>
        public virtual void ToLogisticsBillNavigate() { }
        /// <summary>
        /// 转入库
        /// </summary>
        public virtual void ToStockInBillNavigate() { }
        /// <summary>
        /// 在线支付
        /// </summary>
        public virtual void OnLinePayNavigate() { }
        /// <summary>
        /// 打印条码
        /// </summary>
        public virtual void PrintBarCodeNavigate() { }
        /// <summary>
        /// 转收款
        /// </summary>
        public virtual void ToReceiptBillNavigate() { }
        /// <summary>
        /// 转付款
        /// </summary>
        public virtual void ToPayBillNavigate() { }
        /// <summary>
        /// 转采购
        /// </summary>
        public virtual void ToPurchaseOrderNavigate() { }
        /// <summary>
        /// 转出库
        /// </summary>
        public virtual void ToStockOutBillNavigate() { }
        /// <summary>
        /// 全部签收
        /// </summary>
        public virtual void AllSignNavigate() { }
        /// <summary>
        /// 转结算
        /// </summary>
        public virtual void ToSettlementNavigate() { }
        /// <summary>
        /// 开户
        /// </summary>
        public virtual void WalletCreateAccount() { }
        /// <summary>
        /// 充值
        /// </summary>
        public virtual void WalletDepositMoney() { }
        #endregion

        /// <summary>
        /// 初始化动作按钮
        /// </summary>
        public void InitializeAction()
        {
            if (ToolBarActionAndNavigate.Toolbars.Exists(SysConst.EN_ACTION))
            {
                foreach (var loopAction in ToolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools)
                {
                    SetActionVisiableAndEnable(loopAction.Key, false, false);
                }
            }

            #region 根据登录用户的权限设置动作按钮[是否可见]和[是否可用]（SuperAdmin拥有所有权限，登录时候数据已处理）

            var cacheObj = CacheDAX.Get(CacheDAX.ConfigDataKey.SystemUserAuthority);
            if (cacheObj == null) return;
            //根据Form类型获取对应动作权限
            _menuGroupActionList =
                new List<MenuGroupActionUIModel>(
                    ((List<MenuGroupActionUIModel>)cacheObj).Where(
                        x => x.MenuD_ClassFullName == this.GetType().FullName).ToList());
            foreach (var loopAction in _menuGroupActionList)
            {
                SetActionVisiableAndEnable(loopAction.Act_Key, true, true);
            }

            #endregion

        }

        /// <summary>
        /// 初始化导航按钮（需被重写）
        /// <para>根据各自画面业务需求初始化导航按钮</para>
        /// </summary>
        public virtual void InitializeNavigate()
        {
            if (ToolBarActionAndNavigate.Toolbars.Exists(SysConst.EN_NAVIGATE))
            {
                foreach (var loopNavigate in ToolBarActionAndNavigate.Toolbars[SysConst.EN_NAVIGATE].Tools)
                {
                    SetActionVisiableAndEnable(loopNavigate.Key, false, false);
                }
            }

            if (ToolBarActionAndNavigate.Toolbars.Exists(SysConst.EN_NAVIGATE) &&
                ToolBarActionAndNavigate.Toolbars[SysConst.EN_NAVIGATE].Tools.Count == 0)
            {
                ToolBarActionAndNavigate.Toolbars[SysConst.EN_NAVIGATE].Visible = false;
            }

        }

        /// <summary>
        /// 根据选中的Tab，设置动作按钮[是否可用]（在系统权限的基础上进行控制）
        /// <para> </para>
        /// <para>________[详情]Tab__[列表]Tab</para>
        /// <para>保存_____可用______不可用</para>
        /// <para>复制_____可用______不可用</para>
        /// <para>审核_____可用______不可用</para>
        /// <para>反审核___可用______不可用</para>
        /// <para>导出_____不可用____可用</para>
        /// <para>启动盘点_可用______不可用</para>
        /// <para> </para>
        /// <para>其他动作按钮不管当前选中[详情]或[列表]Tab，都按系统权限进行控制</para>
        /// </summary>
        /// <param name="paramTabKey"></param>
        public void SetActionEnableBySelectedTab(string paramTabKey)
        {
            //【详情】Tab
            if (paramTabKey == SysConst.EN_DETAIL)
            {
                //Query查询 不可用
                SetActionEnable(SystemActionEnum.Code.QUERY, false);
                //Save保存
                var saveEnable = _menuGroupActionList.FirstOrDefault(x => x.Act_Key == SystemActionEnum.Code.SAVE) !=
                                 null ? true : false;
                SetActionEnable(SystemActionEnum.Code.SAVE, saveEnable);
                //Copy复制
                var copyEnable = _menuGroupActionList.FirstOrDefault(x => x.Act_Key == SystemActionEnum.Code.COPY) !=
                                null ? true : false;
                SetActionEnable(SystemActionEnum.Code.COPY, copyEnable);
                //Clear清空
                SetActionEnable(SystemActionEnum.Code.CLEAR, false);
                //Approve审核
                var approveEnable = _menuGroupActionList.FirstOrDefault(x => x.Act_Key == SystemActionEnum.Code.APPROVE) !=
                               null ? true : false;
                SetActionEnable(SystemActionEnum.Code.APPROVE, approveEnable);
                //UnApprove反审核
                var unApproveEnable = _menuGroupActionList.FirstOrDefault(x => x.Act_Key == SystemActionEnum.Code.UNAPPROVE) !=
                              null ? true : false;
                SetActionEnable(SystemActionEnum.Code.UNAPPROVE, unApproveEnable);
                //Export导出
                SetActionEnable(SystemActionEnum.Code.EXPORT, false);
                //StartStockTask启动盘点
                var startStockTaskEnable = _menuGroupActionList.FirstOrDefault(x => x.Act_Key == SystemActionEnum.Code.STARTSTOCKTASK) != null ? true : false;
                SetActionEnable(SystemActionEnum.Code.STARTSTOCKTASK, startStockTaskEnable);

                //StockCountZeroInventory盘点零库存
                var stockCountZeroInventoryEnable = _menuGroupActionList.FirstOrDefault(x => x.Act_Key == SystemActionEnum.Code.STOCKCOUNTZEROINVENTORY) != null ? true : false;
                SetActionEnable(SystemActionEnum.Code.STOCKCOUNTZEROINVENTORY, stockCountZeroInventoryEnable);

                //ConfirmProfitAndLoss确认库存损益
                var confirmProfitAndLossEnable = _menuGroupActionList.FirstOrDefault(x => x.Act_Key == SystemActionEnum.Code.CONFIRMPROFITANDLOSS) != null ? true : false;
                SetActionEnable(SystemActionEnum.Code.CONFIRMPROFITANDLOSS, confirmProfitAndLossEnable);

                //InventoryRectify库存校正
                var inventoryRectifyEnable = _menuGroupActionList.FirstOrDefault(x => x.Act_Key == SystemActionEnum.Code.INVENTORYRECTIFY) != null ? true : false;
                SetActionEnable(SystemActionEnum.Code.INVENTORYRECTIFY, inventoryRectifyEnable);

                //CancelConfirmProfitAndLoss取消确认库存损益
                var cancelConfirmProfitAndLossEnable = _menuGroupActionList.FirstOrDefault(x => x.Act_Key == SystemActionEnum.Code.CANCELCONFIRMPROFITANDLOSS) != null ? true : false;
                SetActionEnable(SystemActionEnum.Code.CANCELCONFIRMPROFITANDLOSS, cancelConfirmProfitAndLossEnable);

                //Print打印
                var printEnable = _menuGroupActionList.FirstOrDefault(x => x.Act_Key == SystemActionEnum.Code.PRINT) !=
                               null ? true : false;
                SetActionEnable(SystemActionEnum.Code.PRINT, printEnable);
            }
            //【列表】Tab
            else if (paramTabKey == SysConst.EN_LIST)
            {
                //Query查询 可用
                SetActionEnable(SystemActionEnum.Code.QUERY, true);
                //Save保存
                SetActionEnable(SystemActionEnum.Code.SAVE, false);
                //Copy复制
                SetActionEnable(SystemActionEnum.Code.COPY, false);
                //Clear清空
                var clearEnable = _menuGroupActionList.FirstOrDefault(x => x.Act_Key == SystemActionEnum.Code.CLEAR) !=
                                null ? true : false;
                SetActionEnable(SystemActionEnum.Code.CLEAR, clearEnable);
                //Approve审核
                SetActionEnable(SystemActionEnum.Code.APPROVE, false);
                //UnApprove反审核
                SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);
                //Export导出
                var exportEnable = _menuGroupActionList.FirstOrDefault(x => x.Act_Key == SystemActionEnum.Code.EXPORT) !=
                    null ? true : false;
                SetActionEnable(SystemActionEnum.Code.EXPORT, exportEnable);
                //StartStockTask启动盘点
                SetActionEnable(SystemActionEnum.Code.STARTSTOCKTASK, false);
                //Verify核实
                SetActionEnable(SystemActionEnum.Code.VERIFY, false);
                //Print打印
                SetActionEnable(SystemActionEnum.Code.PRINT, false);
            }
        }

        /// <summary>
        /// 设置指定动作按钮[是否可见]和[是否可用]
        /// </summary>
        /// <param name="paramSystemAction">SystemActionEnum.Code.XXX</param>
        /// <param name="paramVisible">true:显示；false:不显示</param>
        /// <param name="paramEnabled">true:可用；false:不可用</param>
        public void SetActionVisiableAndEnable(string paramSystemAction, bool paramVisible, bool paramEnabled)
        {
            if (ToolBarActionAndNavigate.Toolbars.Exists(SysConst.EN_ACTION) && ToolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools.Exists(paramSystemAction))
            {
                ToolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[paramSystemAction].SharedPropsInternal.Enabled = paramEnabled;
                ToolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[paramSystemAction].SharedPropsInternal.Visible = paramVisible;
            }
        }
        /// <summary>
        /// 设置指定动作按钮[是否可用]
        /// </summary>
        /// <param name="paramSystemAction">SystemActionEnum.Code.XXX</param>
        /// <param name="paramEnabled">true:可用；false:不可用</param>
        public void SetActionEnable(string paramSystemAction, bool paramEnabled)
        {
            if (ToolBarActionAndNavigate.Toolbars.Exists(SysConst.EN_ACTION) && ToolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools.Exists(paramSystemAction))
            {
                ToolBarActionAndNavigate.Toolbars[SysConst.EN_ACTION].Tools[paramSystemAction].SharedPropsInternal.Enabled = paramEnabled;
            }
        }
        /// <summary>
        /// 设置指定导航按钮[是否可见]和[是否可用]
        /// </summary>
        /// <param name="paramSystemAction">SystemNavigateEnum.Code.XXX</param>
        /// <param name="paramVisible">true:显示；false:不显示</param>
        /// <param name="paramEnabled">true:可用；false:不可用</param>
        public void SetNavigateVisiableAndEnable(string paramSystemAction, bool paramVisible, bool paramEnabled)
        {
            if (ToolBarActionAndNavigate.Toolbars.Exists(SysConst.EN_NAVIGATE) && ToolBarActionAndNavigate.Toolbars[SysConst.EN_NAVIGATE].Tools.Exists(paramSystemAction))
            {
                ToolBarActionAndNavigate.Toolbars[SysConst.EN_NAVIGATE].Tools[paramSystemAction].SharedPropsInternal.Enabled = paramEnabled;
                ToolBarActionAndNavigate.Toolbars[SysConst.EN_NAVIGATE].Tools[paramSystemAction].SharedPropsInternal.Visible = paramVisible;
            }
        }
        /// <summary>
        /// 设置指定导航按钮[是否可用]
        /// </summary>
        /// <param name="paramSystemAction">SystemNavigateEnum.Code.XXX</param>
        /// <param name="paramEnabled">true:可用；false:不可用</param>
        public void SetNavigateEnable(string paramSystemAction, bool paramEnabled)
        {
            if (ToolBarActionAndNavigate.Toolbars.Exists(SysConst.EN_NAVIGATE) && ToolBarActionAndNavigate.Toolbars[SysConst.EN_NAVIGATE].Tools.Exists(paramSystemAction))
            {
                ToolBarActionAndNavigate.Toolbars[SysConst.EN_NAVIGATE].Tools[paramSystemAction].SharedPropsInternal.Enabled = paramEnabled;
            }
        }
        #endregion

        #region Grid翻页相关方法

        #region 公共
        /// <summary>
        /// 设置翻页控件
        /// <para>窗体加载或初始化时调用</para>
        /// </summary>
        /// <param name="paramTotalRecordCount">总记录条数</param>
        public void SetBarPaging(int paramTotalRecordCount)
        {
            var funcName = "SetBarPaging";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            //重新计算[总页数]，设置最新[总记录条数]
            SetTotalPageCountAndTotalRecordCount(paramTotalRecordCount);
            //设置翻页按钮状态
            SetPageButtonStatus();
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void ToolBarPaging_ToolClick(object sender, ToolClickEventArgs e)
        {
            var funcName = "ToolBarPaging_ToolClick";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPaging != null)
            {
                switch (e.Tool.Key)
                {
                    //第一页
                    case SysConst.EN_FIRSTPAGE:
                        ((TextBoxTool)(ToolBarPaging.Tools[SysConst.EN_PAGEINDEX])).Text =
                            SysConst.NUMBER_ONE.ToString();
                        break;
                    // 前一页
                    case SysConst.EN_FORWARDPAGE:
                        ((TextBoxTool)(ToolBarPaging.Tools[SysConst.EN_PAGEINDEX])).Text = (PageIndex - 1).ToString();
                        break;
                    // 下一页
                    case SysConst.EN_NEXTPAGE:
                        ((TextBoxTool)(ToolBarPaging.Tools[SysConst.EN_PAGEINDEX])).Text = (PageIndex + 1).ToString();
                        break;
                    // 最后一页
                    case SysConst.EN_LASTPAGE:
                        ((TextBoxTool)(ToolBarPaging.Tools[SysConst.EN_PAGEINDEX])).Text = TotalPageCount.ToString();
                        break;
                }
            }
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }
        /// <summary>
        /// 翻页ToolBar的值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ToolBarPaging_ToolValueChanged(object sender, ToolEventArgs e)
        {
            var funcName = "ToolBarPaging_ToolValueChanged";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            if (SysConst.EN_PAGEINDEX.Equals(e.Tool.Key))
            {
                string strValue = ((TextBoxTool)(e.Tool)).Text.Trim();
                int tmpPageIndex = 0;
                if (!int.TryParse(strValue, out tmpPageIndex) && tmpPageIndex <= 0)
                {
                    ((TextBoxTool)(e.Tool)).Text = SysConst.NUMBER_ONE.ToString();
                    ((TextBoxTool)(e.Tool)).SelectionLength = 1;
                    return;
                }
                else if (tmpPageIndex > TotalPageCount)
                {
                    ((TextBoxTool)(e.Tool)).Text = TotalPageCount.ToString();
                    ((TextBoxTool)(e.Tool)).SelectionLength = TotalPageCount.ToString().Length;
                    return;
                }

                #region 当前页码设置
                if (Convert.ToInt32(strValue) <= 0)
                {
                    ((TextBoxTool)(e.Tool)).Text = SysConst.NUMBER_ONE.ToString();
                }
                else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) >= TotalPageCount)
                {
                    ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(TotalPageCount).ToString();
                }
                else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) < TotalPageCount)
                {
                    ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(strValue).ToString();
                }
                #endregion

                PageIndex = tmpPageIndex;
                if (tmpPageIndex <= 0)
                {
                    PageIndex = 1;
                }
                ExecuteQuery?.Invoke();
            }
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }
        #endregion

        #region 私有
        /// <summary>
        /// 设置总页数和总记录条数
        /// </summary>
        /// <param name="paramTotalRecordCount">总记录条数</param>
        private void SetTotalPageCountAndTotalRecordCount(int paramTotalRecordCount)
        {
            var funcName = "SetTotalPageCountAndTotalRecordCount";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPaging != null)
            {
                if (paramTotalRecordCount > 0)
                {
                    TotalRecordCount = paramTotalRecordCount;
                    int? remainder = TotalRecordCount % PageSize;
                    if (remainder > 0)
                    {
                        TotalPageCount = TotalRecordCount / PageSize + 1;
                    }
                    else
                    {
                        TotalPageCount = TotalRecordCount / PageSize;
                    }
                }
                else
                {
                    PageIndex = 1;
                    TotalPageCount = 1;
                    TotalRecordCount = 0;
                }
                ((TextBoxTool)(ToolBarPaging.Tools[SysConst.EN_PAGEINDEX])).Text = PageIndex.ToString();
                ToolBarPaging.Tools[SysConst.EN_PAGECOUNT].SharedProps.Caption = TotalPageCount.ToString();
            }
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 设置翻页按钮状态
        /// </summary>
        private void SetPageButtonStatus()
        {
            var funcName = "SetPageButtonStatus";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPaging != null)
            {
                if (PageIndex == 0 || TotalRecordCount == 0)
                {
                    ToolBarPaging.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;

                    return;
                }
                if (PageIndex == 1 && TotalRecordCount <= PageSize)
                {
                    ToolBarPaging.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (PageIndex == 1 && TotalRecordCount > PageSize)
                {
                    ToolBarPaging.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPaging.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPaging.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (PageIndex != 1 && TotalRecordCount > PageSize * PageIndex)
                {
                    ToolBarPaging.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPaging.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPaging.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPaging.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
                }
                else if (PageIndex != 1 && TotalRecordCount <= PageSize * PageIndex)
                {
                    ToolBarPaging.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPaging.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
                }
                else
                {
                    throw (new Exception("非预期的场合。。。"));
                }
            }
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        #endregion

        #endregion

        #region Model属性值变化相关方法

        /// <summary>
        /// 接受调用此方法之前UIModel的变化
        /// </summary>
        /// <param name="paramPropertyName">UIModel中的属性名</param>
        public void AcceptUIModelChanges(string paramPropertyName)
        {
            NewUIModel = this.HeadDS;
            if (OrginalUIModel == null)
            {
                OrginalUIModel = Activator.CreateInstance<HeadUIModelType>();
            }
            _bll.CopyModelByPropertyName(this.HeadDS, OrginalUIModel, paramPropertyName);
        }

        /// <summary>
        /// 接受调用此方法之前UIModel的变化
        /// </summary>
        protected virtual void AcceptUIModelChanges()
        {
            NewUIModel = this.HeadDS;
            if (OrginalUIModel == null)
            {
                OrginalUIModel = Activator.CreateInstance<HeadUIModelType>();
            }
            if (HeadDSCopy == null)
            {
                HeadDSCopy = Activator.CreateInstance<HeadUIModelType>();
            }
            _bll.CopyModelFully(this.HeadDS, OrginalUIModel, this.HeadDSCopy);
        }

        /// <summary>
        /// 检查UIModel属性值是否发生变化
        /// </summary>
        /// <param name="paramSkipProperyList">需要跳过的属性（不予检查）</param>
        /// <returns></returns>
        public virtual bool ViewHasChanged(List<string> paramSkipProperyList = null)
        {
            bool tabViewHasChanged = false;
            //以Tab打开时是否变化
            tabViewHasChanged = OrginalUIModel != null
                && NewUIModel != null
                && CupidModelHelper.ModelHasChanged(Trans.COM, NewUIModel, OrginalUIModel, paramSkipProperyList);

            return tabViewHasChanged;
        }

        /// <summary>
        /// 关闭画面时UIModel属性值是否发生变化
        /// </summary>
        /// 注意【详情】Tab中有Grid的场合，注意检查明细List是否变化
        /// <returns></returns>
        public virtual bool ViewHasChangedWhenClose()
        {
            return false;
        }
        #endregion
    }
}
