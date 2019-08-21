namespace SkyCar.Coeus.Common.Enums
{
    /// <summary>
    /// 公共
    /// </summary>
    public static class ComViewParamValue
    {
        /// <summary>
        /// 维护配件明细源Form
        /// </summary>
        public enum MaintainAutoPartsSourForm
        {
            /// <summary>
            /// 配件预定
            /// </summary>
            PIS_FrmPurchaseOrderManager,
            /// <summary>
            /// 配件入库
            /// </summary>
            PIS_FrmStockInBillManager,
        }
        /// <summary>
        /// 维护配件明细动作
        /// </summary>
        public enum MaintainAutoPartsAction
        {
            /// <summary>
            /// 添加普通配件
            /// </summary>
            AddAutoParts,
            /// <summary>
            /// 更新普通配件
            /// </summary>
            UpdateAutoParts,
            /// <summary>
            /// 即进即出普通配件
            /// </summary>
            InOutComAutoParts,
            /// <summary>
            /// 即进即出630配件
            /// </summary>
            InOutDiscountAutoParts,
        }

        /// <summary>
        /// 客户查询源Form
        /// </summary>
        public enum CustomerQuerySourForm
        {
            /// <summary>
            /// 销售订单管理
            /// </summary>
            SD_FrmSalesOrderManager,
            /// <summary>
            /// 主动销售管理
            /// </summary>
            SD_FrmProactiveSalesManager,
        }
    }

    /// <summary>
    /// 基础设置
    /// </summary>
    public static class BSViewParamValue
    {

    }

    /// <summary>
    /// 财务管理
    /// </summary>
    public static class FMViewParamValue
    {

    }

    /// <summary>
    /// 进销存管理
    /// </summary>
    public static class PISViewParamValue
    {
        
    }

    /// <summary>
    /// 统计报表
    /// </summary>
    public static class RPTViewParamValue
    {

    }

    /// <summary>
    /// 销售管理
    /// </summary>
    public static class SDViewParamValue
    {
        /// <summary>
        /// 确认退货画面的来源方式
        /// </summary>
        public enum ConfirmReturnSourType
        {
            /// <summary>
            /// 核实销售订单
            /// </summary>
            VerifySalesOrder,
            /// <summary>
            /// 审核退货的销售订单
            /// </summary>
            ApproveReturnSalesOrder,
        }
    }

    /// <summary>
    /// 系统管理
    /// </summary>
    public static class SMViewParamValue
    {

    }

    /// <summary>
    /// 微信管理
    /// </summary>
    public static class WCViewParamValue
    {

    }
}
