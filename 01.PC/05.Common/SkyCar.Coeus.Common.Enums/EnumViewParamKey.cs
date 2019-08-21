namespace SkyCar.Coeus.Common.Enums
{
    /// <summary>
    /// 公共
    /// </summary>
    public enum ComViewParamKey
    {
        /// <summary>
        /// OrgInfo
        /// </summary>
        OrgInfo,
        /// <summary>
        /// 界面呈现方式
        /// </summary>
        PageDisplayMode,
        /// <summary>
        /// 操作参数Key
        /// </summary>
        EnumKeyOperation,
        /// <summary>
        /// 业务大类
        /// </summary>
        BusinessType,
        /// <summary>
        /// 新打开界面的Model
        /// </summary>
        DestModel,
        /// <summary>
        /// 用APModel传参
        /// </summary>
        APModel,
        /// <summary>
        /// 新打开界面的List
        /// </summary>
        DestList,
        /// <summary>
        /// AddAction
        /// </summary>
        AddAction,
        /// <summary>
        /// UpdateAction
        /// </summary>
        UpdateAction,
        /// <summary>
        /// ViewCostAction
        /// </summary>
        ViewCostAction,
        /// <summary>
        /// 单据所属组织ID
        /// </summary>
        BillsOrgID,
        /// <summary>
        /// 窗体名称
        /// </summary>
        WindowName,
        /// <summary>
        /// 维护配件明细的源界面
        /// </summary>
        MaintainAutoPartsSourForm,
        /// <summary>
        /// 维护配件明细的源界面Func
        /// </summary>
        MaintainAutoPartsSourFormFunc,
        /// <summary>
        /// 维护配件明细的默认供应商
        /// </summary>
        MaintainAutoPartsDefaultSupplierID,
        /// <summary>
        /// 维护配件明细的默认仓库
        /// </summary>
        MaintainAutoPartsDefaultWarehouseID,
        /// <summary>
        /// 维护配件明细的默认仓库
        /// </summary>
        MaintainAutoPartsDefaultWarehouseBinID,
        /// <summary>
        /// 维护配件明细的动作
        /// </summary>
        MaintainAutoPartsAction,
        /// <summary>
        /// 签收配件明细的源界面Func
        /// </summary>
        SignAutoPartsToWarehouseSourFormFunc,
        /// <summary>
        /// 确认收银Func
        /// </summary>
        CashierConfirmFunc,
        /// <summary>
        /// 确认挂单Func
        /// </summary>
        PendingOrderConfirmFunc,
        /// <summary>
        /// 确认付款Func
        /// </summary>
        PaymentConfirmFunc,
        /// <summary>
        /// 确认收款Func
        /// </summary>
        CollectionConfirmFunc,
        /// <summary>
        /// 用户的业务角色
        /// </summary>
        UserBusinessRole,
        /// <summary>
        /// 配件查询来源画面
        /// </summary>
        PartsSearchDialogSourForm,
        /// <summary>
        /// 配件条码List
        /// </summary>
        BarCodeList,
        /// <summary>
        /// 配件领料Func
        /// </summary>
        PickAutoPartsFunc,
        /// <summary>
        /// 来源类型
        /// </summary>
        SourceType,
        /// <summary>
        /// 单据状态
        /// </summary>
        DocumentStatus,
        /// <summary>
        /// 审核状态
        /// </summary>
        ApprovalStatus,
        /// <summary>
        /// 指定的供应商ID
        /// </summary>
        SpecialSupplierID,
        /// <summary>
        /// 指定的供应商名称
        /// </summary>
        SpecialSupplierName,
        /// <summary>
        /// 是否弹出二维码扫描窗体
        /// </summary>
        ShowQrCodeWindow,
        /// <summary>
        /// 支付凭证信息
        /// </summary>
        SettlementBillInfo,
        /// <summary>
        /// 微信支付成功Func
        /// </summary>
        PaySuccessConfirmFunc,
        /// <summary>
        /// 取消微信支付Func
        /// </summary>
        CancelConfirmFunc,
        /// <summary>
        /// 选择配件档案Func
        /// </summary>
        SelectAutoPartsArchive,
        /// <summary>
        /// 套餐信息
        /// </summary>
        PackageInfo,
        /// <summary>
        /// 客户信息
        /// </summary>
        CustomerInfo,
        /// <summary>
        /// 客户类型
        /// </summary>
        CustomerType,
        /// <summary>
        /// 客户ID
        /// </summary>
        CustomerId,
        /// <summary>
        /// 客户名称
        /// </summary>
        CustomerName,
        /// <summary>
        /// 业务单确认收款
        /// </summary>
        BusinessCollectionConfirm,
        /// <summary>
        /// 业务单确认付款
        /// </summary>
        BusinessPaymentConfirm,
        /// <summary>
        /// 汽修商户编码
        /// </summary>
        AutoFactoryCode,
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        AutoFactoryName,
        /// <summary>
        /// 汽修商户组织ID
        /// </summary>
        AutoFactoryOrgId,
        /// <summary>
        /// 汽修商户组织名称
        /// </summary>
        AutoFactoryOrgName,
        /// <summary>
        /// 客户查询来源窗体
        /// </summary>
        CustomerQuerySourForm,
        /// <summary>
        /// UserID
        /// </summary>
        UserID,
        /// <summary>
        /// 单号
        /// </summary>
        BillNo,
        /// <summary>
        /// 配件条形码
        /// </summary>
        AutoPartsBarcode,
        /// <summary>
        /// 配件名称
        /// </summary>
        AutoPartsName,
        /// <summary>
        /// 是否在加载画面时就进行查询
        /// </summary>
        IsQueryAtLoadForm,
    }

    /// <summary>
    /// 销售
    /// </summary>
    public enum SDViewParamKey
    {
        /// <summary>
        /// 物流单
        /// </summary>
        LogisticsBill,
        /// <summary>
        /// 物流单明细
        /// </summary>
        LogisticsBillDetail,
        /// <summary>
        /// 销售订单
        /// </summary>
        SalesOrder,
        /// <summary>
        /// 销售订单明细
        /// </summary>
        SalesOrderDetail,
        /// <summary>
        /// 销售预测订单
        /// </summary>
        SalesForecastOrder,
        /// <summary>
        /// 销售预测订单明细
        /// </summary>
        SalesForecastOrderDetail,
        /// <summary>
        /// 销售退货明细
        /// </summary>
        SalesOrderReturnDetail,
        /// <summary>
        /// 退货入库明细
        /// </summary>
        ReturnStockInDetail,
        /// <summary>
        /// 物流人员类型
        /// </summary>
        DeliveryType,
        /// <summary>
        /// 确认退货画面的来源方式
        /// </summary>
        ConfirmReturnSourType,
    }

    /// <summary>
    /// 进销存管理
    /// </summary>
    public enum PISViewParamKey
    {
        /// <summary>
        /// 采购单
        /// </summary>
        PurchaseOrder,
        /// <summary>
        /// 采购单明细
        /// </summary>
        PurchaseOrderDetail,
        /// <summary>
        /// 调拨单
        /// </summary>
        TransferBill,
        /// <summary>
        /// 调拨明细
        /// </summary>
        TransferBillDetail,
        /// <summary>
        /// 入库单
        /// </summary>
        StockInBill,
        /// <summary>
        /// 入库单明细
        /// </summary>
        StockInBillDetail,
        /// <summary>
        /// 出库单
        /// </summary>
        StockOutBill,
        /// <summary>
        /// 入库单明细
        /// </summary>
        StockOutBillDetail,
        /// <summary>
        /// 盘库单
        /// </summary>
        StocktakingBill,
        /// <summary>
        /// 盘库明细
        /// </summary>
        StocktakingDetailList,
        /// <summary>
        /// 库存查询
        /// </summary>
        InventoryQuery,
        /// <summary>
        /// 库存异动日志
        /// </summary>
        InventoryTransLog,
        /// <summary>
        /// 库存异动类型
        /// </summary>
        InventoryTransType,
    }

    /// <summary>
    /// 财务
    /// </summary>
    public enum FMViewParamKey
    {
        /// <summary>
        /// 收款单
        /// </summary>
        ReceiptBill,
        /// <summary>
        /// 收款单明细
        /// </summary>
        ReceiptBillDetail,
        /// <summary>
        /// 收款单列表
        /// </summary>
        ReceiptBillList,
        /// <summary>
        /// 收款单（其他收款）
        /// </summary>
        ReceiptBillQTSK,
        /// <summary>
        /// 付款单
        /// </summary>
        PayBill,
        /// <summary>
        /// 付款单明细
        /// </summary>
        PayBillDetail,
        /// <summary>
        /// 应付单
        /// </summary>
        AccountPayableBill,
        /// <summary>
        /// 应付单明细
        /// </summary>
        AccountPayableBillDetail,
        /// <summary>
        /// 应收单
        /// </summary>
        AccountReceivableBill,
        /// <summary>
        /// 应收单明细
        /// </summary>
        AccountReceivableBillDetail,
        /// <summary>
        /// 应收单确认收款
        /// </summary>
        ReceiveableCashierConfirm,
        /// <summary>
        /// 应付单确认付款
        /// </summary>
        PayableCashierConfirm,
    }

    /// <summary>
    /// 预收管理
    /// </summary>
    public enum RIAViewParamKey
    {
        /// <summary>
        /// 钱包相关信息
        /// </summary>
        WalletInfo,
    }

    /// <summary>
    /// 基础设置
    /// </summary>
    public enum BSViewParamKey
    {
        /// <summary>
        /// 维护车辆原厂件信息
        /// </summary>
        MaintainOemPartsInfo,
        /// <summary>
        /// 维护车辆品牌件信息
        /// </summary>
        MaintainBrandPartsInfo,
    }
}
