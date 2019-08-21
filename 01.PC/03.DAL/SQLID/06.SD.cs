namespace SkyCar.Coeus.DAL
{
    public partial class SQLID
    {
        #region 主动销售模板管理

        /// <summary>
        /// 判断是否存在相同名称的主动销售模板
        /// </summary>
        public static string SD_SalesTemplate_SQL01 = "SalesTemplate_SQL01";
        /// <summary>
        /// 查询目的组织下的主动销售模板
        /// </summary>
        public static string SD_SalesTemplate_SQL03 = "SalesTemplate_SQL03";
        /// <summary>
        /// 查询主动销售模板管理列表
        /// </summary>
        public static string SD_SalesTemplate_SQL02 = "SalesTemplate_SQL02";
        /// <summary>
        /// 查询某汽修商户某组织是否存在已审核的销售模板
        /// </summary>
        public static string SD_SalesTemplate_SQL05 = "SalesTemplate_SQL05";
        #endregion

        #region 销售订单管理
        /// <summary>
        /// 查询销售订单明细
        /// </summary>
        public static string SD_SalesOrder_SQL01 = "SalesOrder_SQL01";
        /// <summary>
        /// 查询汽修商户订单中配件的最新销售价格
        /// </summary>
        public static string SD_SalesOrder_SQL02 = "SalesOrder_SQL02";
        /// <summary>
        /// 根据组织编码获取Venus仓库和仓位信息
        /// </summary>
        public static string SD_SalesOrder_SQL03 = "SalesOrder_SQL03";
        /// <summary>
        /// 查询销售订单列表
        /// </summary>
        public static string SD_SalesOrder_SQL04 = "SalesOrder_SQL04";
        /// <summary>
        /// 根据销售订单ID更新物流订单列表
        /// </summary>
        public static string SD_SalesOrder_SQL05 = "SalesOrder_SQL05";
        #endregion

        #region 主动销售管理

        /// <summary>
        /// 查询Venus对应供应商信息
        /// </summary>
        public static string SD_ProactiveSales_SQL04 = "ProactiveSales_SQL04";
        /// <summary>
        /// 查询配件名称在Venus系统中是否存在
        /// </summary>
        public static string SD_ProactiveSales_SQL05 = "ProactiveSales_SQL05";
        /// <summary>
        /// 查询条码在Venus系统中是否存在
        /// </summary>
        public static string SD_ProactiveSales_SQL07 = "ProactiveSales_SQL07";
        /// <summary>
        /// 查询销售列表
        /// </summary>
        public static string SD_ProactiveSales_SQL08 = "ProactiveSales_SQL08";
        /// <summary>
        /// 查询汽修商户的配件采购订单
        /// </summary>
        public static string SD_ProactiveSales_SQL09 = "ProactiveSales_SQL09";
        /// <summary>
        /// 查询汽修商户的配件采购订单明细
        /// </summary>
        public static string SD_ProactiveSales_SQL10 = "ProactiveSales_SQL10";
        /// <summary>
        /// 删除汽修商户的配件采购订单
        /// </summary>
        public static string SD_ProactiveSales_SQL11 = "ProactiveSales_SQL11";
        /// <summary>
        /// 删除汽修商户的配件采购订单明细
        /// </summary>
        public static string SD_ProactiveSales_SQL12 = "ProactiveSales_SQL12";
        /// <summary>
        /// 根据[销售订单].[单号]查询[出库单明细]
        /// </summary>
        public static string SD_ProactiveSales_SQL13 = "ProactiveSales_SQL13";
        /// <summary>
        /// 查询Venus车辆品牌车系信息
        /// </summary>
        public static string SD_ProactiveSales_SQL14 = "ProactiveSales_SQL14";
        /// <summary>
        /// 查询Venus码表（类别为[配件级别]）信息
        /// </summary>
        public static string SD_ProactiveSales_SQL15 = "ProactiveSales_SQL15";
        /// <summary>
        /// 查询Venus配件类别信息
        /// </summary>
        public static string SD_ProactiveSales_SQL16 = "ProactiveSales_SQL16";
        /// <summary>
        /// 查询Venus系统编号信息
        /// </summary>
        public static string SD_ProactiveSales_SQL17 = "ProactiveSales_SQL17";
        /// <summary>
        /// 删除Venus配件采购订单编号对应的系统编号
        /// </summary>
        public static string SD_ProactiveSales_SQL18 = "ProactiveSales_SQL18";
        /// <summary>
        /// 反审核时更新[销售订单]
        /// </summary>
        public static string SD_ProactiveSales_SQL19 = "ProactiveSales_SQL19";
        /// <summary>
        /// 反审核时更新[销售订单明细]
        /// </summary>
        public static string SD_ProactiveSales_SQL20 = "ProactiveSales_SQL20";
        /// <summary>
        /// 查询Coeus车辆品牌车系信息
        /// </summary>
        public static string SD_ProactiveSales_SQL21 = "ProactiveSales_SQL21";
        /// <summary>
        /// 查询Coeus配件名称信息
        /// </summary>
        public static string SD_ProactiveSales_SQL22 = "ProactiveSales_SQL22";
        #endregion

        #region 销售补货建议查询

        /// <summary>
        /// 删除销售预测订单明细
        /// </summary>
        public static string SD_SalesForecastOrder_SQL01 = "SalesForecastOrder_SQL01";
        /// <summary>
        /// 删除销售预测订单
        /// </summary>
        public static string SD_SalesForecastOrder_SQL02 = "SalesForecastOrder_SQL02";
        #endregion

        #region 物流单管理
        /// <summary>
        /// 查询物流单明细
        /// </summary>
        public static string SD_LogisticsBill_SQL01 = "LogisticsBill_SQL01";
        /// <summary>
        /// 根据图片名称查询物流单
        /// </summary>
        public static string SD_LogisticsBill_SQL02 = "LogisticsBill_SQL02";
        /// <summary>
        /// 查询物流单列表
        /// </summary>
        public static string SD_LogisticsBill_SQL03 = "LogisticsBill_SQL03";
        /// <summary>
        /// 删除图片时更新物流单
        /// </summary>
        public static string SD_LogisticsBill_SQL04 = "LogisticsBill_SQL04";

        #endregion

        #region 销售退货管理

        /// <summary>
        /// 查询销售退货管理列表
        /// </summary>
        public static string SD_SalesReturnOrderManager_SQL01 = "SalesReturnOrderManager_SQL01";
        /// <summary>
        /// 
        /// </summary>
        public static string SD_SalesReturnOrderManager_SQL02 = "SalesReturnOrderManager_SQL02";
        #endregion

    }
}
