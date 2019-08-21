using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.TBModel;
using System;
using System.Collections.Generic;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.UIModel.Common.QCModel;
using SkyCar.Coeus.UIModel.Common.UIModel;
using SkyCar.Framework.WindowUI;

namespace SkyCar.Coeus.UI.Common
{
    /// <summary>
    /// 库存异动日志查询
    /// </summary>
    public partial class FrmInventoryTransLogQuery : BaseForm
    {
        #region 全局变量
        /// <summary>
        /// 库存异动日志查询BLL
        /// </summary>
        private BLLBase _bll = new BLLBase(Trans.COM);

        /// <summary>
        /// 翻页按钮ToolBar
        /// </summary>
        private UltraToolbarsManager _toolBarPaging = new UltraToolbarsManager();

        /// <summary>
        /// Grid的数据源
        /// </summary>
        List<InventoryTransLogQueryUIModel> ListGridDS = new List<InventoryTransLogQueryUIModel>();

        /// <summary>
        /// 页面参数字典
        /// </summary>
        private Dictionary<string, object> _viewParameters = new Dictionary<string, object>();

        #region 下拉框数据源

        /// <summary>
        /// 异动类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _inventoryTransTypeList = new List<ComComboBoxDataSourceTC>();

        /// <summary>
        /// 配件名称
        /// </summary>
        List<MDLBS_AutoPartsName> _autoPartsNameList = new List<MDLBS_AutoPartsName>();
        #endregion

        #region 列表Grid翻页用

        /// <summary>
        /// 翻页ToolBar
        /// </summary>
        private UltraToolbarsManager ToolBarPaging
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
        /// 执行子类中的查询方法
        /// </summary>
        private Action ExecuteQuery;
        /// <summary>
        /// 当前页面索引值
        /// </summary>
        private int PageIndex = 1;
        /// <summary>
        /// 总记录条数
        /// </summary>
        private int TotalRecordCount = 0;
        /// <summary>
        /// 页面大小
        /// </summary>
        private int PageSize = 20;
        /// <summary>
        /// 总页面数
        /// </summary>
        private int TotalPageCount = 0;
        #endregion

        #endregion

        #region 公共属性

        /// <summary>
        /// 选中项列表
        /// </summary>
        public List<InventoryTransLogQueryUIModel> SelectedGridList = new List<InventoryTransLogQueryUIModel>();
        #endregion

        #region 系统事件

        /// <summary>
        /// 库存异动日志查询
        /// </summary>
        /// <param name="paramWindowParameters">传入参数</param>
        public FrmInventoryTransLogQuery(Dictionary<string, object> paramWindowParameters)
        {
            _viewParameters = paramWindowParameters;

            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmInventoryTransLogQuery_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（翻页）
            ToolBarPaging = toolBarManagerListTabPaging;
            //查询委托（基类控制翻页用）
            ExecuteQuery = QueryAction;
            //工具栏（翻页）单击事件
            toolBarManagerListTabPaging.ToolClick += new ToolClickEventHandler(ToolBarPaging_ToolClick);
            //工具栏（翻页）[当前页]值改变事件
            toolBarManagerListTabPaging.ToolValueChanged += new ToolEventHandler(ToolBarPaging_ToolValueChanged);
            //初始化【列表】Tab内控件
            InitializeListTabControls();
            #endregion

            #region 处理参数

            if (_viewParameters == null)
            {
                return;
            }

            #region 配件条形码

            if (_viewParameters.ContainsKey(ComViewParamKey.AutoPartsBarcode.ToString()))
            {
                string tempAutoPartsBarcode = _viewParameters[ComViewParamKey.AutoPartsBarcode.ToString()] as string;

                txtWhere_ITL_Barcode.Text = tempAutoPartsBarcode;
                txtWhere_ITL_Barcode.Enabled = false;
            }
            #endregion

            #region 配件名称

            if (_viewParameters.ContainsKey(ComViewParamKey.AutoPartsName.ToString()))
            {
                string tempAutoPartsName = _viewParameters[ComViewParamKey.AutoPartsName.ToString()] as string;

                mcbWhere_ITL_Name.SelectedValue = tempAutoPartsName;
                mcbWhere_ITL_Name.Enabled = false;
            }
            #endregion

            #region 异动类型

            if (_viewParameters.ContainsKey(PISViewParamKey.InventoryTransType.ToString()))
            {
                _inventoryTransTypeList = _viewParameters[PISViewParamKey.InventoryTransType.ToString()] as List<ComComboBoxDataSourceTC>;
                if (_inventoryTransTypeList != null && _inventoryTransTypeList.Count > 0)
                {
                    cbWhere_ITL_TransType.DisplayMember = SysConst.EN_TEXT;
                    cbWhere_ITL_TransType.ValueMember = SysConst.EN_Code;
                    cbWhere_ITL_TransType.DataSource = _inventoryTransTypeList;
                    cbWhere_ITL_TransType.DataBind();

                    cbWhere_ITL_TransType.Text = _inventoryTransTypeList[0].Text;
                    cbWhere_ITL_TransType.Value = _inventoryTransTypeList[0].Code;

                    if (_inventoryTransTypeList.Count == 1)
                    {
                        cbWhere_ITL_TransType.Enabled = false;
                    }
                    else
                    {
                        cbWhere_ITL_TransType.Enabled = true;
                    }
                }
            }
            #endregion

            #region 是否在加载画面时就进行查询

            if (_viewParameters.ContainsKey(ComViewParamKey.IsQueryAtLoadForm.ToString()))
            {
                var isQueryAtLoadForm = _viewParameters[ComViewParamKey.IsQueryAtLoadForm.ToString()] as bool?;
                if (isQueryAtLoadForm == true)
                {
                    QueryAction();
                }
            }
            #endregion

            #endregion

        }

        /// <summary>
        /// [清空]按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            #region 查询条件初始化

            //条形码
            if (txtWhere_ITL_Barcode.Enabled)
            {
                txtWhere_ITL_Barcode.Clear();
            }
            //配件名称
            if (mcbWhere_ITL_Name.Enabled)
            {
                mcbWhere_ITL_Name.Clear();
            }
            //异动类型
            if (cbWhere_ITL_TransType.Enabled)
            {
                cbWhere_ITL_TransType.Clear();
            }
            //异动时间
            dtWhere_ITL_CreatedTimeStart.Value = null;
            dtWhere_ITL_CreatedTimeEnd.Value = null;

            //给 条形码 设置焦点
            lblWhere_ITL_Barcode.Focus();

            #endregion

            #region Grid初始化

            //清空Grid
            ListGridDS = new List<InventoryTransLogQueryUIModel>();
            gdGrid.DataSource = ListGridDS;
            gdGrid.DataBind();

            #endregion
        }
        /// <summary>
        /// [查询]按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //查询
            QueryAction();
        }

        #endregion

        #region 重写基类方法

        /// <summary>
        /// 查询
        /// </summary>
        public void QueryAction()
        {
            var argsInventoryTransLogQuery = new InventoryTransLogQueryQCModel()
            {
                PageSize = PageSize,
                PageIndex = PageIndex,
                //条形码
                WHERE_ITL_Barcode = txtWhere_ITL_Barcode.Text.Trim(),
                //配件名称
                WHERE_ITL_Name = mcbWhere_ITL_Name.SelectedValue,
                //组织ID
                WHERE_ITL_Org_ID = LoginInfoDAX.OrgID,
            };
            //异动时间-开始
            if (dtWhere_ITL_CreatedTimeStart.Value != null)
            {
                argsInventoryTransLogQuery._CreatedTimeStart = dtWhere_ITL_CreatedTimeStart.DateTime;
            }
            //异动时间-终了
            if (dtWhere_ITL_CreatedTimeEnd.Value != null)
            {
                argsInventoryTransLogQuery._CreatedTimeEnd = dtWhere_ITL_CreatedTimeEnd.DateTime;
            }
            //异动类型
            string transTypeStr = string.Empty;
            if (string.IsNullOrEmpty(cbWhere_ITL_TransType.Text))
            {
                foreach (var loopTransType in _inventoryTransTypeList)
                {
                    transTypeStr += loopTransType.Text + SysConst.Semicolon_DBC;
                }
                argsInventoryTransLogQuery.WHERE_ITL_TransType = transTypeStr;
            }
            else
            {
                argsInventoryTransLogQuery.WHERE_ITL_TransType = cbWhere_ITL_TransType.Text + SysConst.Semicolon_DBC;
            }
            _bll.QueryForList<InventoryTransLogQueryUIModel>(SQLID.COMM_SQL55, argsInventoryTransLogQuery, ListGridDS);

            gdGrid.DataSource = ListGridDS;
            gdGrid.DataBind();

            if (ListGridDS.Count > 0)
            {
                dynamic subObject = ListGridDS[0];

                TotalRecordCount = subObject.RecordCount;
            }
            else
            {
                TotalRecordCount = 0;
            }
            //设置翻页控件
            SetBarPaging(TotalRecordCount);
            //设置单元格是否可以编辑
            SetGridCellStyle();
        }
        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 查询条件初始化

            //条形码
            if (txtWhere_ITL_Barcode.Enabled)
            {
                txtWhere_ITL_Barcode.Clear();
            }
            //配件名称
            if (mcbWhere_ITL_Name.Enabled)
            {
                mcbWhere_ITL_Name.Clear();
            }
            //异动类型
            if (cbWhere_ITL_TransType.Enabled)
            {
                cbWhere_ITL_TransType.Clear();
            }
            //异动时间
            dtWhere_ITL_CreatedTimeStart.Value = DateTime.Now.AddMonths(-6);
            dtWhere_ITL_CreatedTimeEnd.Value = null;

            //给 条形码 设置焦点
            lblWhere_ITL_Barcode.Focus();

            #endregion

            #region Grid初始化

            //清空Grid
            ListGridDS = new List<InventoryTransLogQueryUIModel>();
            gdGrid.DataSource = ListGridDS;
            gdGrid.DataBind();

            #endregion

            #region 初始化下拉框

            //配件名称下拉框
            _autoPartsNameList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsName) as List<MDLBS_AutoPartsName>;
            mcbWhere_ITL_Name.DisplayMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbWhere_ITL_Name.ValueMember = SystemTableColumnEnums.BS_AutoPartsName.Code.APN_Name;
            mcbWhere_ITL_Name.DataSource = _autoPartsNameList;

            //异动类型
            _inventoryTransTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.InventoryTransType);
            cbWhere_ITL_TransType.DisplayMember = SysConst.EN_TEXT;
            cbWhere_ITL_TransType.ValueMember = SysConst.EN_Code;
            cbWhere_ITL_TransType.DataSource = _inventoryTransTypeList;
            cbWhere_ITL_TransType.DataBind();

            #endregion
        }

        #region Grid翻页相关方法

        #region 公共
        /// <summary>
        /// 设置翻页控件
        /// <para>窗体加载或初始化时调用</para>
        /// </summary>
        /// <param name="paramTotalRecordCount">总记录条数</param>
        private void SetBarPaging(int paramTotalRecordCount)
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

        private void ToolBarPaging_ToolClick(object sender, ToolClickEventArgs e)
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
        private void ToolBarPaging_ToolValueChanged(object sender, ToolEventArgs e)
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

        /// <summary>
        /// 设置Grid单元格样式
        /// </summary>
        private void SetGridCellStyle()
        {
            #region 设置Grid数据颜色
            gdGrid.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;

            #endregion
        }

        #endregion
    }
}
