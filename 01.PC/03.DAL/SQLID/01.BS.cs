namespace SkyCar.Coeus.DAL
{
    public partial class SQLID
    {
        #region 车辆品牌车系管理

        /// <summary>
        /// 验证车辆品牌+车系唯一性
        /// </summary>
        public static string BS_VehicleBrandInspireSummaManager_SQL01 = "VehicleBrandInspireSummaManager_SQL01";
        /// <summary>
        /// 查询品牌车系是否被引用过
        /// </summary>
        public static string BS_VehicleBrandInspireSummaManager_SQL02 = "VehicleBrandInspireSummaManager_SQL02";
        #endregion

        #region 配件档案管理

        /// <summary>
        /// 获取配件计量单位
        /// </summary>
        public static string BS_AutoPartsArchiveManager_SQL01 = "AutoPartsArchiveManager_SQL01";
        /// <summary>
        /// 查询配件档案列表
        /// </summary>
        public static string BS_AutoPartsArchiveManager_SQL02 = "AutoPartsArchiveManager_SQL02";
        /// <summary>
        /// 查询配件档案是否被引用过
        /// </summary>
        public static string BS_AutoPartsArchiveManager_SQL03 = "AutoPartsArchiveManager_SQL03";
        /// <summary>
        /// 根据条形码查询配件价格明细
        /// </summary>
        public static string BS_AutoPartsArchiveManager_SQL04 = "AutoPartsArchiveManager_SQL04";
        /// <summary>
        /// 根据ID查询配件价格明细
        /// </summary>
        public static string BS_AutoPartsArchiveManager_SQL05 = "AutoPartsArchiveManager_SQL05";
        #endregion

        #region 配件类别管理
        /// <summary>
        /// 查询配件类别列表
        /// </summary>
        public static string BS_AutoPartsTypeManager_SQL01 = "AutoPartsTypeManager_SQL01";
        /// <summary>
        /// 查询配件类别是否已存在
        /// </summary>
        public static string BS_AutoPartsTypeManager_SQL02 = "AutoPartsTypeManager_SQL02";
        /// <summary>
        /// 获取一级类别数
        /// </summary>
        public static string BS_AutoPartsTypeManager_SQL03 = "AutoPartsTypeManager_SQL03";
        /// <summary>
        /// 查询配件类别是否被引用
        /// </summary>
        public static string BS_AutoPartsTypeManager_SQL04 = "AutoPartsTypeManager_SQL04";

        #endregion

        #region 码表管理

        /// <summary>
        /// 查询码表管理列表
        /// </summary>
        public static string BS_CodeTableManager_SQL01 = "CodeTableManager_SQL01";
        /// <summary>
        /// 验证参数类型和参数值的唯一性
        /// </summary>
        public static string BS_CodeTableManager_SQL02 = "CodeTableManager_SQL02";
        /// <summary>
        /// 查询码表是否被引用过
        /// </summary>
        public static string BS_CodeTableManager_SQL03 = "CodeTableManager_SQL03";

        #endregion

        #region 配件名称管理

        /// <summary>
        /// 查询配件名称是否已存在
        /// </summary>
        public static string BS_AutoPartsNameManager_SQL01 = "AutoPartsNameManager_SQL01";
        /// <summary>
        /// 查询配件名称列表
        /// </summary>
        public static string BS_AutoPartsNameManager_SQL02 = "AutoPartsNameManager_SQL02";
        /// <summary>
        /// 查询配件名称是否被引用
        /// </summary>
        public static string BS_AutoPartsNameManager_SQL03 = "AutoPartsNameManager_SQL03";

        #endregion

        #region 车型配件匹配管理

        /// <summary>
        /// 查询车辆信息列表
        /// </summary>
        public static string BS_VehicleModelMatchAutoPartsManager_SQL01 = "VehicleModelMatchAutoPartsManager_SQL01";
        /// <summary>
        /// 根据车架号查询车辆原厂件信息
        /// </summary>
        public static string BS_VehicleModelMatchAutoPartsManager_SQL02 = "VehicleModelMatchAutoPartsManager_SQL02";
        /// <summary>
        /// 根据车架号查询车辆品牌件信息
        /// </summary>
        public static string BS_VehicleModelMatchAutoPartsManager_SQL03 = "VehicleModelMatchAutoPartsManager_SQL03";
        /// <summary>
        /// 检查车架号是否已存在
        /// </summary>
        public static string BS_VehicleModelMatchAutoPartsManager_SQL04 = "VehicleModelMatchAutoPartsManager_SQL04";
        /// <summary>
        /// 根据ID查询车辆原厂件件信息
        /// </summary>
        public static string BS_VehicleModelMatchAutoPartsManager_SQL05 = "VehicleModelMatchAutoPartsManager_SQL05";
        /// <summary>
        /// 根据ID查询车辆品牌件信息
        /// </summary>
        public static string BS_VehicleModelMatchAutoPartsManager_SQL06 = "VehicleModelMatchAutoPartsManager_SQL06";

        #endregion
    }
}
