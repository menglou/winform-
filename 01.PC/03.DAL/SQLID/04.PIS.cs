namespace SkyCar.Coeus.DAL
{
    public partial class SQLID
    {
        #region 入库管理

        /// <summary>
        /// 查询入库单列表
        /// </summary>
        public static string PIS_StockInBillManager_SQL01 = "StockInBillManager_SQL01";
        /// <summary>
        /// 查询入库单明细列表
        /// </summary>
        public static string PIS_StockInBillManager_SQL02 = "StockInBillManager_SQL02";
        #endregion

        #region 仓库管理

        /// <summary>
        /// 查询仓库有是否被使用过
        /// </summary>
        public static string PIS_WarehouseManager_SQL01 = "WarehouseManager_SQL01";
        /// <summary>
        /// 查询仓库列表
        /// </summary>
        public static string PIS_WarehouseManager_SQL02 = "WarehouseManager_SQL02";
        /// <summary>
        /// 根据仓库ID查询仓库列表
        /// </summary>
        public static string PIS_WarehouseManager_SQL03 = "WarehouseManager_SQL03";
        /// <summary>
        /// 查询仓库名称是否已存在
        /// </summary>
        public static string PIS_WarehouseManager_SQL04 = "WarehouseManager_SQL04";
        /// <summary>
        /// 查询仓库编码是否已存在
        /// </summary>
        public static string PIS_WarehouseManager_SQL05 = "WarehouseManager_SQL05";

        #endregion

        #region 仓位管理

        /// <summary>
        /// 查询仓位有没有被用过
        /// </summary>
        public static string PIS_WarehouseBinManager_SQL01 = "WarehouseBinManager_SQL01";
        #endregion

        #region 汽修商管理

        /// <summary>
        /// 验证汽修商唯一性
        /// </summary>
        public static string PIS_AutoFactoryCustomerManager_SQL01 = "AutoFactoryCustomerManager_SQL01";
        /// <summary>
        /// 查询汽修商是否被引用
        /// </summary>
        public static string PIS_AutoFactoryCustomerManager_SQL02 = "AutoFactoryCustomerManager_SQL02";
        /// <summary>
        /// 查询汽修商客户列表
        ///  </summary>
        public static string PIS_AutoFactoryCustomerManager_SQL03 = "AutoFactoryCustomerManager_SQL03";
        /// <summary>
        /// 根据ID查询汽修商客户
        ///  </summary>
        public static string PIS_AutoFactoryCustomerManager_SQL04 = "AutoFactoryCustomerManager_SQL04";
        #endregion

        #region 普通客户管理

        /// <summary>
        /// 验证普通客户唯一性
        /// </summary>
        public static string PIS_GeneralCustomerManager_SQL01 = "GeneralCustomerManager_SQL01";
        /// <summary>
        /// 查询普通客户是否被引用
        /// </summary>
        public static string PIS_GeneralCustomerManager_SQL02 = "GeneralCustomerManager_SQL02";
        #endregion

        #region 供应商管理

        /// <summary>
        /// 查询供应商信息
        /// </summary>
        public static string PIS_SupplierManager_SQL01 = "SupplierManager_SQL01";

        /// <summary>
        /// 查询供应商被使用的个数
        /// </summary>
        public static string PIS_SupplierManager_SQL02 = "SupplierManager_SQL02";
        #endregion

        #region 出库单管理

        /// <summary>
        /// 查询出库单列表
        /// </summary>
        public static string PIS_StockOutBillManager_SQL_01 = "StockOutBillManager_SQL_01";

        /// <summary>
        /// 查询出库单明细列表
        /// </summary>
        public static string PIS_StockOutBillManager_SQL_02 = "StockOutBillManager_SQL_02";

        /// <summary>
        /// 根据采购订单获取出库单明细列表
        /// </summary>
        public static string PIS_StockOutBillManager_SQL_03 = "StockOutBillManager_SQL_03";

        #endregion

        #region 调拨管理

        /// <summary>
        /// 查询调拨单列表
        /// </summary>
        public static string PIS_TransferBillManager_SQL_01 = "TransferBillManager_SQL_01";

        /// <summary>
        /// 查询调拨单明细列表
        /// </summary>
        public static string PIS_TransferBillManager_SQL_02 = "TransferBillManager_SQL_02";

        /// <summary>
        /// 反审核时，更新调拨单明细
        /// </summary>
        public static string PIS_TransferBillManager_SQL_03 = "TransferBillManager_SQL_03";
        #endregion

        #region 库存查询
        /// <summary>
        /// 库存明细查询
        /// </summary>
        public static string PIS_InventoryQuery_SQL_01 = "InventoryQuery_SQL_01";

        /// <summary>
        /// 库存异动查询
        /// </summary>
        public static string PIS_InventoryQuery_SQL_02 = "InventoryQuery_SQL_02";

        /// <summary>
        /// 库存汇总查询
        /// </summary>
        public static string PIS_InventoryQuery_SQL_03 = "InventoryQuery_SQL_03";

        #endregion

        #region 盘点管理

        /// <summary>
        /// 查询[盘点管理]【列表】信息
        /// </summary>
        public static string PIS_StocktakingTaskManager_SQL_01 = "StocktakingTaskManager_SQL_01";

        /// <summary>
        /// 查询[盘点管理]【明细】信息
        /// </summary>
        public static string PIS_StocktakingTaskManager_SQL_02 = "StocktakingTaskManager_SQL_02";

        /// <summary>
        /// 根据组织、仓库、仓位查询库存
        /// </summary>
        public static string PIS_StocktakingTaskManager_SQL_03 = "StocktakingTaskManager_SQL_03";

        /// <summary>
        /// 查询零库存的配件
        /// </summary>
        public static string PIS_StocktakingTaskManager_SQL_04 = "StocktakingTaskManager_SQL_04";

        /// <summary>
        /// 查询该仓库是否存在未完成的盘点任务
        /// </summary>
        public static string PIS_StocktakingTaskManager_SQL_05 = "StocktakingTaskManager_SQL_05";

        #endregion

        #region 采购补货建议查询
        /// <summary>
        /// 采购补货建议查询
        /// </summary>
        public static string PIS_PurchaseForecastOrderQuery_SQL_01 = "PurchaseForecastOrderQuery_SQL_01";

        /// <summary>
        /// 采购补货建议明细查询
        /// </summary>
        public static string PIS_PurchaseForecastOrderQuery_SQL_02 = "PurchaseForecastOrderQuery_SQL_02";
        #endregion

        #region 采购管理查询
        /// <summary>
        /// 采购管理查询
        /// </summary>
        public static string PIS_PurchaseOrderManager_SQL_01 = "PurchaseOrderManager_SQL_01";

        /// <summary>
        /// 采购管理明细查询
        /// </summary>
        public static string PIS_PurchaseOrderManager_SQL_02 = "PurchaseOrderManager_SQL_02";
        #endregion
        
        #region 确定收款

        /// <summary>
        /// 获取采购单下对应的应付单
        /// </summary>
        public static string PIS_PurchaseOrderToPayConfirmWindow_SQL_01 = "PurchaseOrderToPayConfirmWindow_01";

        #endregion

        #region 采购退货出库管理

        /// <summary>
        /// 查询采购退货出库列表
        /// </summary>
        public static string PIS_PurchaseReturnManager_SQL_01 = "PurchaseReturnManager_SQL_01";
        /// <summary>
        /// 查询采购退货出库明细列表
        /// </summary>
        public static string PIS_PurchaseReturnManager_SQL_02 = "PurchaseReturnManager_SQL_02";
        #endregion
    }
}
