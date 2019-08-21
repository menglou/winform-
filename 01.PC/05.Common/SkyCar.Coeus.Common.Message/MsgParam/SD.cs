namespace SkyCar.Coeus.Common.Message
{
    /// <summary>
    /// 消息参数常量
    /// </summary>
    public partial class MsgParam
    {
        /// <summary>
        /// 客户类型
        /// </summary>
        public static string CUST_TYPE = "客户类型";
        /// <summary>
        /// 客户ID
        /// </summary>
        public static string CUST_ID = "客户ID";
        /// <summary>
        /// 客户名称
        /// </summary>
        public static string CUST_NAME = "客户名称";
        /// <summary>
        /// 平台内商户
        /// </summary>
        public static string PLATFORM_MERCHANT = "平台内商户";
        /// <summary>
        /// 转销售
        /// </summary>
        public static string TO_SALESORDER = "转销售";
        /// <summary>
        /// 汽修商户
        /// </summary>
        public static string AUTOFACTORY = "汽修商户";
        /// <summary>
        /// 汽修商组织
        /// </summary>
        public static string AUTOFACTORY_ORG = "汽修商组织";
        /// <summary>
        /// 服务器
        /// </summary>
        public static string SERVER = "服务器";
        /// <summary>
        /// 连接
        /// </summary>
        public static string CONNECTION = "连接";
        /// <summary>
        /// 数据库
        /// </summary>
        public static string DATABASE = "数据库";
        /// <summary>
        /// 配置
        /// </summary>
        public static string CONFIHURATION = "配置";
        /// <summary>
        /// 信息
        /// </summary>
        public static string INFORMATION = "信息";
        /// <summary>
        /// 目的组织
        /// </summary>
        public static string PURPOSEORGNIZATION = "目的组织";
        /// <summary>
        /// 配件已被汽修商户签收
        /// </summary>
        public static string AUTOPARTS_SIGNIN = "配件已被汽修商户签收";
        /// <summary>
        /// 相同来源类型，相同客户的销售订单
        /// </summary>
        public static string SAME_SOURCEANDCUSTOMER = "相同来源类型，相同客户的销售订单";
        /// <summary>
        /// 退货数量
        /// </summary>
        public static string RETURN_QTY = "退货数量";
        /// <summary>
        /// 退货单价
        /// </summary>
        public static string RETURN_PRICE = "退货单价";
        /// <summary>
        /// 已审核并且不是退货的
        /// </summary>
        public static string YSH_AND_NOTRETURN = "已审核并且不是退货的";
        /// <summary>
        /// 交易成功并且不是退货的
        /// </summary>
        public static string JYCG_AND_NOTRETURN = "交易成功并且不是退货的";
        /// <summary>
        /// 赔偿金额
        /// </summary>
        public static string COMPENSATION_AMOUNT = "赔偿金额";
        /// <summary>
        /// 销售金额
        /// </summary>
        public static string SALES_AMOUT = "销售金额";
        /// <summary>
        /// 已签收
        /// </summary>
        public static string ALREADY_SIGN = "已签收";
        /// <summary>
        /// 未签收
        /// </summary>
        public static string NOT_SIGN = "未签收";
        /// <summary>
        /// 物流人员类型
        /// </summary>
        public static string DELIVERYBY_TYPE = "物流人员类型";
        /// <summary>
        /// 物流人员
        /// </summary>
        public static string DELIVERYBY = "物流人员";
        /// <summary>
        /// 签收数量、拒收数量和丢失数量之和
        /// </summary>
        public static string SIGN_REJECT_LOSE_SUM = "签收数量、拒收数量和丢失数量之和";
        /// <summary>
        /// 拒收数量或丢失数量
        /// </summary>
        public static string REJECTORLOSE = "拒收数量或丢失数量";
        /// <summary>
        /// 签收数量、拒收数量或丢失数量
        /// </summary>
        public static string SIGNORREJECTORLOSE = "签收数量、拒收数量或丢失数量";

        #region Venus数据表
        /// <summary>
        /// 配件采购订单
        /// </summary>
        public static string APM_PurchaseOrder = "配件采购订单";
        /// <summary>
        /// 配件采购订单明细
        /// </summary>
        public static string APM_PurchaseOrderDetail = "配件采购订单明细";
        /// <summary>
        /// 车辆品牌车系
        /// </summary>
        public static string SCON_VehicleBrandInspireSumma = "车辆品牌车系";
        /// <summary>
        /// 码表
        /// </summary>
        public static string SCON_CodeTable = "码表";
        /// <summary>
        /// 配件类别
        /// </summary>
        public static string APM_AutoPartsType = "配件类别";
        #endregion
    }

}
