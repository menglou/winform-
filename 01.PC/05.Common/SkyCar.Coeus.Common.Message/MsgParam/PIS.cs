namespace SkyCar.Coeus.Common.Message
{
    /// <summary>
    /// 消息参数常量
    /// </summary>
    public partial class MsgParam
    {
        /// <summary>
        /// 单据类型
        /// </summary>
        public static string BILL_TYPE = "单据类型";

        /// <summary>
        /// 调拨类型
        /// </summary>
        public static string TRANSFER_TYPE = "调拨类型";

        /// <summary>
        /// 调出组织
        /// </summary>
        public static string TRANSFEROUT_ORG = "调出组织";

        /// <summary>
        /// 调入组织
        /// </summary>
        public static string TRANSFERIN_ORG = "调入组织";

        /// <summary>
        /// 调入仓库
        /// </summary>
        public static string TRANSFERIN_WAREHOUSE = "调入仓库";

        /// <summary>
        /// 调入仓位
        /// </summary>
        public static string TRANSFERIN_WAREHOUSEBIN = "调入仓位";

        /// <summary>
        /// 选择
        /// </summary>
        public static string SELECT = "选择";

        /// <summary>
        /// 盘点状态
        /// </summary>
        public static string STOCKTAKING_STATUS = "盘点状态";

        /// <summary>
        /// 加载零库存
        /// </summary>
        public static string LOAD_ZEROINVENTORY = "加载零库存";

        /// <summary>
        /// 当前
        /// </summary>
        public static string CURRENT = "当前";

        /// <summary>
        /// 未完成
        /// </summary>
        public static string INCOMPLETE = "未完成"; 

        /// <summary>
        /// 上传数据
        /// </summary>
        public static string UPLOAD_DATA = "上传数据";

        /// <summary>
        /// 生成损益表
        /// </summary>
        public static string GENERATE_PROFITANDLOSS = "生成损益表";

        /// <summary>
        /// 校正库存
        /// </summary>
        public static string ADJUST_INVENTORY = "校正库存";

        /// <summary>
        /// 确认损益
        /// </summary>
        public static string CONFIRM_PROFITANDLOSS = "确认损益";

        /// <summary>
        /// 检查库存，校正失败！
        /// </summary>
        public static string INVENTORYRECTIFY_FAILED = "检查库存，校正失败！";

        /// <summary>
        /// 取消确认损益
        /// </summary>
        public static string CANCLE_CONFIRM_PROFITANDLOSS = "取消确认损益";

        /// <summary>
        /// 取消盘点任务
        /// </summary>
        public static string CANCLE_STOCKTAKINGTASK = "取消盘点任务";

        /// <summary>
        /// 本次签收数量
        /// </summary>
        public static string SIGN_COUNT = "本次签收数量";

        /// <summary>
        /// 来源类型为退货出库并且已审核的
        /// </summary>
        public static string THCK_YSH = "来源类型为退货出库并且已审核的";

        /// <summary>
        /// 来源类型为手工创建并且已审核的
        /// </summary>
        public static string SGCJ_YSH = "来源类型为手工创建并且已审核的";

        /// <summary>
        /// 相同来源类型，相同供应商的采购订单
        /// </summary>
        public static string SAME_SOURCEANDSUPPLIER = "相同来源类型，相同供应商的采购订单";

        /// <summary>
        /// 至少一张
        /// </summary>
        public static string ATLEAST_APIECE = "至少一张";

        /// <summary>
        /// 库存
        /// </summary>
        public static string INVENTORY = "库存";

        /// <summary>
        /// 同一供应商的
        /// </summary>
        public static string SAME_SUPPLIER = "同一供应商的";

        /// <summary>
        /// 平台内
        /// </summary>
        public static string PLATFORM = "平台内";

    }
}
