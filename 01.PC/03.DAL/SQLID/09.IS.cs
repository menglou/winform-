namespace SkyCar.Coeus.DAL
{
    /// <summary>
    /// 信息共享
    /// </summary>
    public partial class SQLID
    {
        #region 汽配库存共享管理

        /// <summary>
        /// 查询汽配共享库存列表
        /// </summary>
        public static string IS_AutoPartsShareInventoryManager_SQL01 = "AutoPartsShareInventoryManager_SQL01";
        /// <summary>
        /// 根据ID查询共享库存
        /// </summary>
        public static string IS_AutoPartsShareInventoryManager_SQL02 = "AutoPartsShareInventoryManager_SQL02";
        #endregion

        #region 汽修库存和库存异动日志查询

        /// <summary>
        /// 查询汽修库存异动日志
        /// </summary>
        public static string IS_AutoFactoryInventoryQuery_SQL01 = "AutoFactoryInventoryQuery_SQL01";
        /// <summary>
        /// 查询汽修库存【考虑汽修组织】
        /// </summary>
        public static string IS_AutoFactoryInventoryQuery_SQL02 = "AutoFactoryInventoryQuery_SQL02";
        /// <summary>
        /// 查询汽修库存【不考虑汽修组织】
        /// </summary>
        public static string IS_AutoFactoryInventoryQuery_SQL03 = "AutoFactoryInventoryQuery_SQL03";
        #endregion

        #region 汽修组织车辆品牌车系查询

        /// <summary>
        /// 根据汽修组织车辆品牌车系统计车辆数
        /// </summary>
        public static string IS_AFOrgVehicleBrandInspireQuery_SQL01 = "AFOrgVehicleBrandInspireQuery_SQL01";
        #endregion

    }
}
