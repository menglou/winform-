/************************************************************************
* 文件名: XXX.Enums.cs
* ----------------------------------------------------------------------
* 文件概要:系统枚举
*
*     本文件由工具自动生成，请勿手动修改此文件！
*
* 详细:
*  [系统名]     : SkyCar
*  [子系统名]   : 无
*  [功能概要]   : 系统枚举定义
*
* 履历:
* No.   日期  　     姓名  　　内容
* 1.       Tool      自动生成，请勿修改；
* 
************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.Common.Enums
{

    #region 枚举编码
    /// <summary>
    /// 枚举编码（Key）
    /// </summary>
    public enum EnumKey
    {
        /// <summary>
        /// 性别
        /// </summary>
        Sex,
        /// <summary>
        /// 消息推送的方式
        /// </summary>
        PushMesageType,
        /// <summary>
        /// 编码段类型
        /// </summary>
        CodingSegmentType,
        /// <summary>
        /// 编码段填充方式
        /// </summary>
        CodingSegmentPanddingStyle,
        /// <summary>
        /// 系统编号状态
        /// </summary>
        SystemNoStatus,
        /// <summary>
        /// 客户类型
        /// </summary>
        CustomerType,
        /// <summary>
        /// 销售订单来源类型
        /// </summary>
        SalesOrderSourceType,
        /// <summary>
        /// 入库单来源类型
        /// </summary>
        StockInBillSourceType,
        /// <summary>
        /// 入库单状态
        /// </summary>
        StockInBillStatus,
        /// <summary>
        /// 出库单来源类型
        /// </summary>
        StockOutBillSourceType,
        /// <summary>
        /// 出库单状态
        /// </summary>
        StockOutBillStatus,
        /// <summary>
        /// 销售订单状态
        /// </summary>
        SalesOrderStatus,
        /// <summary>
        /// 下发数据类型
        /// </summary>
        SendDataType,
        /// <summary>
        /// 销售预测订单状态
        /// </summary>
        SalesForecastOrderStatus,
        /// <summary>
        /// 销售预测订单来源类型
        /// </summary>
        SalesForecastOrderSourceType,
        /// <summary>
        /// 采购订单来源类型
        /// </summary>
        PurchaseOrderSourceType,
        /// <summary>
        /// 采购订单状态
        /// </summary>
        PurchaseOrderStatus,
        /// <summary>
        /// 采购订单明细状态
        /// </summary>
        PurchaseOrderDetailStatus,
        /// <summary>
        /// 采购预测订单来源类型
        /// </summary>
        PurchaseForecastOrderSourceType,
        /// <summary>
        /// 采购预测订单状态
        /// </summary>
        PurchaseForecastOrderStatus,
        /// <summary>
        /// 交易方式
        /// </summary>
        TradeType,
        /// <summary>
        /// 付款单明细来源类型
        /// </summary>
        PayBillDetailSourceType,
        /// <summary>
        /// 单据审核状态
        /// </summary>
        ApprovalStatus,
        /// <summary>
        /// 付款单状态
        /// </summary>
        PayBillStatus,
        /// <summary>
        /// 收款单明细来源类型
        /// </summary>
        ReceiptBillDetailSourceType,
        /// <summary>
        /// 收款单状态
        /// </summary>
        ReceiptBillStatus,
        /// <summary>
        /// 应付单状态
        /// </summary>
        AccountPayableBillStatus,
        /// <summary>
        /// 应收单状态
        /// </summary>
        AccountReceivableBillStatus,
        /// <summary>
        /// 资金流转对象类型
        /// </summary>
        AmountTransObjectType,
        /// <summary>
        /// 应付单来源类型
        /// </summary>
        AccountPayableBillSourceType,
        /// <summary>
        /// 单据方向
        /// </summary>
        BillDirection,
        /// <summary>
        /// 应付单操作类型
        /// </summary>
        AccountPayableBillOperateType,
        /// <summary>
        /// 应收单来源类型
        /// </summary>
        AccountReceivableBillSourceType,
        /// <summary>
        /// 应收单操作类型
        /// </summary>
        AccountReceivableBillOperateType,
        /// <summary>
        /// 微信用户认证类型
        /// </summary>
        WechatUserType,
        /// <summary>
        /// 微信用户异动类型
        /// </summary>
        UserDetailChangeType,
        /// <summary>
        /// 网卡地址类型
        /// </summary>
        NetworkCardType,
        /// <summary>
        /// 审核状态
        /// </summary>
        ApproveStatus,
        /// <summary>
        /// 库存异动类型
        /// </summary>
        InventoryTransType,
        /// <summary>
        /// 物流人员类型
        /// </summary>
        DeliveryType,
        /// <summary>
        /// 物流单状态
        /// </summary>
        LogisticsBillStatus,
        /// <summary>
        /// 支付状态
        /// </summary>
        PaymentStatus,
        /// <summary>
        /// 物流单来源类型
        /// </summary>
        DeliveryBillSourceType,
        /// <summary>
        /// 物流单明细状态
        /// </summary>
        LogisticsBillDetailStatus,
        /// <summary>
        /// 调拨单类型
        /// </summary>
        TransferBillType,
        /// <summary>
        /// 调拨类型
        /// </summary>
        TransferType,
        /// <summary>
        /// 调拨状态
        /// </summary>
        TransfeStatus,
        /// <summary>
        /// 盘点单状态
        /// </summary>
        StocktakingBillStatus,
        /// <summary>
        /// 盘点结果
        /// </summary>
        StocktakingBillCheckResult,
        /// <summary>
        /// 系统动作
        /// </summary>
        SystemAction,
        /// <summary>
        /// 系统导航
        /// </summary>
        SystemNavigate,
        /// <summary>
        /// 编码类型
        /// </summary>
        CodeType,
        /// <summary>
        /// 变速类型
        /// </summary>
        GearboxType,
        /// <summary>
        /// 作业类型
        /// </summary>
        BatchJobType,
        /// <summary>
        /// 执行类别
        /// </summary>
        ExecutionType,
        /// <summary>
        /// 作业执行方式
        /// </summary>
        BatchJobExectueMode,
        /// <summary>
        /// 信息推送契机
        /// </summary>
        MessageSendOpportunity,
        /// <summary>
        /// 推送信息的方式
        /// </summary>
        SendMsgMode,
        /// <summary>
        /// 推送信息的状态
        /// </summary>
        SendMsgStatus,
        /// <summary>
        /// 消息推送状态
        /// </summary>
        PushMessageStatus,
        /// <summary>
        /// 作业执行频率
        /// </summary>
        ExecutionFrequency,
        /// <summary>
        /// 日频率执行类别
        /// </summary>
        ExecutionDayType,
        /// <summary>
        /// 日执行间隔类型
        /// </summary>
        ExecutionDayIntervalType,
        /// <summary>
        /// 钱包来源类型
        /// </summary>
        WalletSourceType,
        /// <summary>
        /// 电子钱包状态
        /// </summary>
        WalletStatus,
        /// <summary>
        /// 钱包异动类型
        /// </summary>
        WalTransType,
        /// <summary>
        /// 登录终端
        /// </summary>
        LoginTerminal,
        /// <summary>
        /// 开票类型
        /// </summary>
        BillingType,
        /// <summary>
        /// 库存图片来源类型
        /// </summary>
        InventoryPictureSourceType
    }
    #endregion


    #region 枚举值明细
    /// <summary>
    /// 性别
    /// </summary>
    public class SexEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 性别-男性
            /// </summary>
            public const string MALE = "男性";

            /// <summary>
            /// 性别-女性
            /// </summary>
            public const string FEMALE = "女性";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 性别-男性
            /// </summary>
            public const string MALE = "Male";

            /// <summary>
            /// 性别-女性
            /// </summary>
            public const string FEMALE = "Female";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 性别-男性
            /// </summary>
            public const string MALE = "1";

            /// <summary>
            /// 性别-女性
            /// </summary>
            public const string FEMALE = "2";

        }

    }
    /// <summary>
    /// 消息推送的方式
    /// </summary>
    public class PushMesageTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 消息推送的方式-微信
            /// </summary>
            public const string WECHAT = "微信";

            /// <summary>
            /// 消息推送的方式-电脑
            /// </summary>
            public const string PC = "电脑";

            /// <summary>
            /// 消息推送的方式-APP
            /// </summary>
            public const string APP = "APP";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 消息推送的方式-微信
            /// </summary>
            public const string WECHAT = "Wechat";

            /// <summary>
            /// 消息推送的方式-电脑
            /// </summary>
            public const string PC = "PC";

            /// <summary>
            /// 消息推送的方式-APP
            /// </summary>
            public const string APP = "APP";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 消息推送的方式-微信
            /// </summary>
            public const string WECHAT = "1";

            /// <summary>
            /// 消息推送的方式-电脑
            /// </summary>
            public const string PC = "2";

            /// <summary>
            /// 消息推送的方式-APP
            /// </summary>
            public const string APP = "3";

        }

    }
    /// <summary>
    /// 编码段类型
    /// </summary>
    public class CodingSegmentTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 编码段类型-固定字符
            /// </summary>
            public const string CONSTANT = "固定字符";

            /// <summary>
            /// 编码段类型-组织
            /// </summary>
            public const string ORG = "组织";

            /// <summary>
            /// 编码段类型-日期
            /// </summary>
            public const string DATE = "日期";

            /// <summary>
            /// 编码段类型-流水
            /// </summary>
            public const string SERIALNO = "流水";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 编码段类型-固定字符
            /// </summary>
            public const string CONSTANT = "Constant";

            /// <summary>
            /// 编码段类型-组织
            /// </summary>
            public const string ORG = "Org";

            /// <summary>
            /// 编码段类型-日期
            /// </summary>
            public const string DATE = "Date";

            /// <summary>
            /// 编码段类型-流水
            /// </summary>
            public const string SERIALNO = "SerialNo";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 编码段类型-固定字符
            /// </summary>
            public const string CONSTANT = "1";

            /// <summary>
            /// 编码段类型-组织
            /// </summary>
            public const string ORG = "2";

            /// <summary>
            /// 编码段类型-日期
            /// </summary>
            public const string DATE = "3";

            /// <summary>
            /// 编码段类型-流水
            /// </summary>
            public const string SERIALNO = "4";

        }

    }
    /// <summary>
    /// 编码段填充方式
    /// </summary>
    public class CodingSegmentPanddingStyleEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 编码段填充方式-左填充
            /// </summary>
            public const string PANDDINGLEFT = "左填充";

            /// <summary>
            /// 编码段填充方式-右填充
            /// </summary>
            public const string PANDDINGRIGHT = "右填充";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 编码段填充方式-左填充
            /// </summary>
            public const string PANDDINGLEFT = "PanddingLeft";

            /// <summary>
            /// 编码段填充方式-右填充
            /// </summary>
            public const string PANDDINGRIGHT = "PanddingRight";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 编码段填充方式-左填充
            /// </summary>
            public const string PANDDINGLEFT = "1";

            /// <summary>
            /// 编码段填充方式-右填充
            /// </summary>
            public const string PANDDINGRIGHT = "2";

        }

    }
    /// <summary>
    /// 系统编号状态
    /// </summary>
    public class SystemNoStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 系统编号状态-未使用
            /// </summary>
            public const string USED = "未使用";

            /// <summary>
            /// 系统编号状态-已使用
            /// </summary>
            public const string UNUSED = "已使用";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 系统编号状态-未使用
            /// </summary>
            public const string USED = "Used";

            /// <summary>
            /// 系统编号状态-已使用
            /// </summary>
            public const string UNUSED = "Unused";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 系统编号状态-未使用
            /// </summary>
            public const string USED = "1";

            /// <summary>
            /// 系统编号状态-已使用
            /// </summary>
            public const string UNUSED = "2";

        }

    }
    /// <summary>
    /// 客户类型
    /// </summary>
    public class CustomerTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 客户类型-普通客户
            /// </summary>
            public const string PTKH = "普通客户";

            /// <summary>
            /// 客户类型-一般汽修商户
            /// </summary>
            public const string YBQXSH = "一般汽修商户";

            /// <summary>
            /// 客户类型-平台内汽修商
            /// </summary>
            public const string PTNQXSH = "平台内汽修商";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 客户类型-普通客户
            /// </summary>
            public const string PTKH = "PTKH";

            /// <summary>
            /// 客户类型-一般汽修商户
            /// </summary>
            public const string YBQXSH = "YBQXSH";

            /// <summary>
            /// 客户类型-平台内汽修商
            /// </summary>
            public const string PTNQXSH = "PTNQXSH";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 客户类型-普通客户
            /// </summary>
            public const string PTKH = "1";

            /// <summary>
            /// 客户类型-一般汽修商户
            /// </summary>
            public const string YBQXSH = "2";

            /// <summary>
            /// 客户类型-平台内汽修商
            /// </summary>
            public const string PTNQXSH = "3";

        }

    }
    /// <summary>
    /// 销售订单来源类型
    /// </summary>
    public class SalesOrderSourceTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 销售订单来源类型-手工创建
            /// </summary>
            public const string SGCJ = "手工创建";

            /// <summary>
            /// 销售订单来源类型-主动销售
            /// </summary>
            public const string ZDXS = "主动销售";

            /// <summary>
            /// 销售订单来源类型-在线销售
            /// </summary>
            public const string ZXXS = "在线销售";

            /// <summary>
            /// 销售订单来源类型-销售预测
            /// </summary>
            public const string XSYC = "销售预测";

            /// <summary>
            /// 销售订单来源类型-主动销售退货
            /// </summary>
            public const string ZDXSTH = "主动销售退货";

            /// <summary>
            /// 销售订单来源类型-在线销售退货
            /// </summary>
            public const string ZXXSTH = "在线销售退货";

            /// <summary>
            /// 销售订单来源类型-手工创建退货
            /// </summary>
            public const string SGCJTH = "手工创建退货";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 销售订单来源类型-手工创建
            /// </summary>
            public const string SGCJ = "SGCJ";

            /// <summary>
            /// 销售订单来源类型-主动销售
            /// </summary>
            public const string ZDXS = "ZDXS";

            /// <summary>
            /// 销售订单来源类型-在线销售
            /// </summary>
            public const string ZXXS = "ZXXS";

            /// <summary>
            /// 销售订单来源类型-销售预测
            /// </summary>
            public const string XSYC = "XSYC";

            /// <summary>
            /// 销售订单来源类型-主动销售退货
            /// </summary>
            public const string ZDXSTH = "ZDXSTH";

            /// <summary>
            /// 销售订单来源类型-在线销售退货
            /// </summary>
            public const string ZXXSTH = "ZXXSTH";

            /// <summary>
            /// 销售订单来源类型-手工创建退货
            /// </summary>
            public const string SGCJTH = "SGCJTH";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 销售订单来源类型-手工创建
            /// </summary>
            public const string SGCJ = "1";

            /// <summary>
            /// 销售订单来源类型-主动销售
            /// </summary>
            public const string ZDXS = "2";

            /// <summary>
            /// 销售订单来源类型-在线销售
            /// </summary>
            public const string ZXXS = "3";

            /// <summary>
            /// 销售订单来源类型-销售预测
            /// </summary>
            public const string XSYC = "4";

            /// <summary>
            /// 销售订单来源类型-主动销售退货
            /// </summary>
            public const string ZDXSTH = "5";

            /// <summary>
            /// 销售订单来源类型-在线销售退货
            /// </summary>
            public const string ZXXSTH = "6";

            /// <summary>
            /// 销售订单来源类型-手工创建退货
            /// </summary>
            public const string SGCJTH = "7";

        }

    }
    /// <summary>
    /// 入库单来源类型
    /// </summary>
    public class StockInBillSourceTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 入库单来源类型-手工创建
            /// </summary>
            public const string SGCJ = "手工创建";

            /// <summary>
            /// 入库单来源类型-采购入库
            /// </summary>
            public const string CGRK = "采购入库";

            /// <summary>
            /// 入库单来源类型-销售退货
            /// </summary>
            public const string SSTH = "销售退货";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 入库单来源类型-手工创建
            /// </summary>
            public const string SGCJ = "SGCJ";

            /// <summary>
            /// 入库单来源类型-采购入库
            /// </summary>
            public const string CGRK = "CGRK";

            /// <summary>
            /// 入库单来源类型-销售退货
            /// </summary>
            public const string SSTH = "SSTH";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 入库单来源类型-手工创建
            /// </summary>
            public const string SGCJ = "1";

            /// <summary>
            /// 入库单来源类型-采购入库
            /// </summary>
            public const string CGRK = "2";

            /// <summary>
            /// 入库单来源类型-销售退货
            /// </summary>
            public const string SSTH = "3";

        }

    }
    /// <summary>
    /// 入库单状态
    /// </summary>
    public class StockInBillStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 入库单状态-已生成
            /// </summary>
            public const string YSC = "已生成";

            /// <summary>
            /// 入库单状态-已完成
            /// </summary>
            public const string YWC = "已完成";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 入库单状态-已生成
            /// </summary>
            public const string YSC = "YSC";

            /// <summary>
            /// 入库单状态-已完成
            /// </summary>
            public const string YWC = "YWC";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 入库单状态-已生成
            /// </summary>
            public const string YSC = "1";

            /// <summary>
            /// 入库单状态-已完成
            /// </summary>
            public const string YWC = "2";

        }

    }
    /// <summary>
    /// 出库单来源类型
    /// </summary>
    public class StockOutBillSourceTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 出库单来源类型-销售出库
            /// </summary>
            public const string XSCK = "销售出库";

            /// <summary>
            /// 出库单来源类型-退货出库
            /// </summary>
            public const string THCK = "退货出库";

            /// <summary>
            /// 出库单来源类型-手工创建
            /// </summary>
            public const string SGCJ = "手工创建";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 出库单来源类型-销售出库
            /// </summary>
            public const string XSCK = "XSCK";

            /// <summary>
            /// 出库单来源类型-退货出库
            /// </summary>
            public const string THCK = "THCK";

            /// <summary>
            /// 出库单来源类型-手工创建
            /// </summary>
            public const string SGCJ = "SGCJ";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 出库单来源类型-销售出库
            /// </summary>
            public const string XSCK = "1";

            /// <summary>
            /// 出库单来源类型-退货出库
            /// </summary>
            public const string THCK = "2";

            /// <summary>
            /// 出库单来源类型-手工创建
            /// </summary>
            public const string SGCJ = "3";

        }

    }
    /// <summary>
    /// 出库单状态
    /// </summary>
    public class StockOutBillStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 出库单状态-已生成
            /// </summary>
            public const string YSC = "已生成";

            /// <summary>
            /// 出库单状态-已完成
            /// </summary>
            public const string YWC = "已完成";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 出库单状态-已生成
            /// </summary>
            public const string YSC = "YSC";

            /// <summary>
            /// 出库单状态-已完成
            /// </summary>
            public const string YWC = "YWC";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 出库单状态-已生成
            /// </summary>
            public const string YSC = "1";

            /// <summary>
            /// 出库单状态-已完成
            /// </summary>
            public const string YWC = "2";

        }

    }
    /// <summary>
    /// 销售订单状态
    /// </summary>
    public class SalesOrderStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 销售订单状态-已生成
            /// </summary>
            public const string YSC = "已生成";

            /// <summary>
            /// 销售订单状态-待付款
            /// </summary>
            public const string DFK = "待付款";

            /// <summary>
            /// 销售订单状态-已付款
            /// </summary>
            public const string YFK = "已付款";

            /// <summary>
            /// 销售订单状态-待发货
            /// </summary>
            public const string DFH = "待发货";

            /// <summary>
            /// 销售订单状态-已发货
            /// </summary>
            public const string YFH = "已发货";

            /// <summary>
            /// 销售订单状态-交易成功
            /// </summary>
            public const string JYCG = "交易成功";

            /// <summary>
            /// 销售订单状态-已关闭
            /// </summary>
            public const string YGB = "已关闭";

            /// <summary>
            /// 销售订单状态-部分签收
            /// </summary>
            public const string BFQS = "部分签收";

            /// <summary>
            /// 销售订单状态-已签收
            /// </summary>
            public const string YQS = "已签收";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 销售订单状态-已生成
            /// </summary>
            public const string YSC = "YSC";

            /// <summary>
            /// 销售订单状态-待付款
            /// </summary>
            public const string DFK = "DFK";

            /// <summary>
            /// 销售订单状态-已付款
            /// </summary>
            public const string YFK = "YFK";

            /// <summary>
            /// 销售订单状态-待发货
            /// </summary>
            public const string DFH = "DFH";

            /// <summary>
            /// 销售订单状态-已发货
            /// </summary>
            public const string YFH = "YFH";

            /// <summary>
            /// 销售订单状态-交易成功
            /// </summary>
            public const string JYCG = "JYCG";

            /// <summary>
            /// 销售订单状态-已关闭
            /// </summary>
            public const string YGB = "YGB";

            /// <summary>
            /// 销售订单状态-部分签收
            /// </summary>
            public const string BFQS = "BFQS";

            /// <summary>
            /// 销售订单状态-已签收
            /// </summary>
            public const string YQS = "YQS";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 销售订单状态-已生成
            /// </summary>
            public const string YSC = "1";

            /// <summary>
            /// 销售订单状态-待付款
            /// </summary>
            public const string DFK = "2";

            /// <summary>
            /// 销售订单状态-已付款
            /// </summary>
            public const string YFK = "3";

            /// <summary>
            /// 销售订单状态-待发货
            /// </summary>
            public const string DFH = "4";

            /// <summary>
            /// 销售订单状态-已发货
            /// </summary>
            public const string YFH = "5";

            /// <summary>
            /// 销售订单状态-交易成功
            /// </summary>
            public const string JYCG = "6";

            /// <summary>
            /// 销售订单状态-已关闭
            /// </summary>
            public const string YGB = "7";

            /// <summary>
            /// 销售订单状态-部分签收
            /// </summary>
            public const string BFQS = "8";

            /// <summary>
            /// 销售订单状态-已签收
            /// </summary>
            public const string YQS = "9";

        }

    }
    /// <summary>
    /// 下发数据类型
    /// </summary>
    public class SendDataTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 下发数据类型-销售模板
            /// </summary>
            public const string XSMB = "销售模板";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 下发数据类型-销售模板
            /// </summary>
            public const string XSMB = "XSMB";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 下发数据类型-销售模板
            /// </summary>
            public const string XSMB = "1";

        }

    }
    /// <summary>
    /// 销售预测订单状态
    /// </summary>
    public class SalesForecastOrderStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 销售预测订单状态-已生成
            /// </summary>
            public const string YSC = "已生成";

            /// <summary>
            /// 销售预测订单状态-已转销售
            /// </summary>
            public const string YZXS = "已转销售";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 销售预测订单状态-已生成
            /// </summary>
            public const string YSC = "YSC";

            /// <summary>
            /// 销售预测订单状态-已转销售
            /// </summary>
            public const string YZXS = "YZXS";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 销售预测订单状态-已生成
            /// </summary>
            public const string YSC = "1";

            /// <summary>
            /// 销售预测订单状态-已转销售
            /// </summary>
            public const string YZXS = "2";

        }

    }
    /// <summary>
    /// 销售预测订单来源类型
    /// </summary>
    public class SalesForecastOrderSourceTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 销售预测订单来源类型-销售模板
            /// </summary>
            public const string XSMB = "销售模板";

            /// <summary>
            /// 销售预测订单来源类型-汽修安全库存
            /// </summary>
            public const string QXAQKC = "汽修安全库存";

            /// <summary>
            /// 销售预测订单来源类型-汽修商采购
            /// </summary>
            public const string QXSCG = "汽修商采购";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 销售预测订单来源类型-销售模板
            /// </summary>
            public const string XSMB = "XSMB";

            /// <summary>
            /// 销售预测订单来源类型-汽修安全库存
            /// </summary>
            public const string QXAQKC = "QXAQKC";

            /// <summary>
            /// 销售预测订单来源类型-汽修商采购
            /// </summary>
            public const string QXSCG = "QXSCG";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 销售预测订单来源类型-销售模板
            /// </summary>
            public const string XSMB = "1";

            /// <summary>
            /// 销售预测订单来源类型-汽修安全库存
            /// </summary>
            public const string QXAQKC = "2";

            /// <summary>
            /// 销售预测订单来源类型-汽修商采购
            /// </summary>
            public const string QXSCG = "3";

        }

    }
    /// <summary>
    /// 采购订单来源类型
    /// </summary>
    public class PurchaseOrderSourceTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 采购订单来源类型-手工创建
            /// </summary>
            public const string SGCJ = "手工创建";

            /// <summary>
            /// 采购订单来源类型-采购预测
            /// </summary>
            public const string CGYC = "采购预测";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 采购订单来源类型-手工创建
            /// </summary>
            public const string SGCJ = "SGCJ";

            /// <summary>
            /// 采购订单来源类型-采购预测
            /// </summary>
            public const string CGYC = "CGYC";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 采购订单来源类型-手工创建
            /// </summary>
            public const string SGCJ = "1";

            /// <summary>
            /// 采购订单来源类型-采购预测
            /// </summary>
            public const string CGYC = "2";

        }

    }
    /// <summary>
    /// 采购订单状态
    /// </summary>
    public class PurchaseOrderStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 采购订单状态-已生成
            /// </summary>
            public const string YSC = "已生成";

            /// <summary>
            /// 采购订单状态-已下单
            /// </summary>
            public const string YXD = "已下单";

            /// <summary>
            /// 采购订单状态-全部签收
            /// </summary>
            public const string QBQS = "全部签收";

            /// <summary>
            /// 采购订单状态-部分签收
            /// </summary>
            public const string BFQS = "部分签收";

            /// <summary>
            /// 采购订单状态-已完成
            /// </summary>
            public const string YWC = "已完成";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 采购订单状态-已生成
            /// </summary>
            public const string YSC = "YSC";

            /// <summary>
            /// 采购订单状态-已下单
            /// </summary>
            public const string YXD = "YXD";

            /// <summary>
            /// 采购订单状态-全部签收
            /// </summary>
            public const string QBQS = "QBQS";

            /// <summary>
            /// 采购订单状态-部分签收
            /// </summary>
            public const string BFQS = "BFQS";

            /// <summary>
            /// 采购订单状态-已完成
            /// </summary>
            public const string YWC = "YWC";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 采购订单状态-已生成
            /// </summary>
            public const string YSC = "1";

            /// <summary>
            /// 采购订单状态-已下单
            /// </summary>
            public const string YXD = "2";

            /// <summary>
            /// 采购订单状态-全部签收
            /// </summary>
            public const string QBQS = "3";

            /// <summary>
            /// 采购订单状态-部分签收
            /// </summary>
            public const string BFQS = "4";

            /// <summary>
            /// 采购订单状态-已完成
            /// </summary>
            public const string YWC = "5";

        }

    }
    /// <summary>
    /// 采购订单明细状态
    /// </summary>
    public class PurchaseOrderDetailStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 采购订单明细状态-已生成
            /// </summary>
            public const string YSC = "已生成";

            /// <summary>
            /// 采购订单明细状态-已下单
            /// </summary>
            public const string YXD = "已下单";

            /// <summary>
            /// 采购订单明细状态-已签收
            /// </summary>
            public const string YQS = "已签收";

            /// <summary>
            /// 采购订单明细状态-已完成
            /// </summary>
            public const string YWC = "已完成";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 采购订单明细状态-已生成
            /// </summary>
            public const string YSC = "YSC";

            /// <summary>
            /// 采购订单明细状态-已下单
            /// </summary>
            public const string YXD = "YXD";

            /// <summary>
            /// 采购订单明细状态-已签收
            /// </summary>
            public const string YQS = "YQS";

            /// <summary>
            /// 采购订单明细状态-已完成
            /// </summary>
            public const string YWC = "YWC";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 采购订单明细状态-已生成
            /// </summary>
            public const string YSC = "1";

            /// <summary>
            /// 采购订单明细状态-已下单
            /// </summary>
            public const string YXD = "2";

            /// <summary>
            /// 采购订单明细状态-已签收
            /// </summary>
            public const string YQS = "3";

            /// <summary>
            /// 采购订单明细状态-已完成
            /// </summary>
            public const string YWC = "4";

        }

    }
    /// <summary>
    /// 采购预测订单来源类型
    /// </summary>
    public class PurchaseForecastOrderSourceTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 采购预测订单来源类型-安全库存备货
            /// </summary>
            public const string AQKCBH = "安全库存备货";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 采购预测订单来源类型-安全库存备货
            /// </summary>
            public const string AQKCBH = "AQKCBH";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 采购预测订单来源类型-安全库存备货
            /// </summary>
            public const string AQKCBH = "1";

        }

    }
    /// <summary>
    /// 采购预测订单状态
    /// </summary>
    public class PurchaseForecastOrderStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 采购预测订单状态-已生成
            /// </summary>
            public const string YSC = "已生成";

            /// <summary>
            /// 采购预测订单状态-已转采购
            /// </summary>
            public const string YZCG = "已转采购";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 采购预测订单状态-已生成
            /// </summary>
            public const string YSC = "YSC";

            /// <summary>
            /// 采购预测订单状态-已转采购
            /// </summary>
            public const string YZCG = "YZCG";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 采购预测订单状态-已生成
            /// </summary>
            public const string YSC = "1";

            /// <summary>
            /// 采购预测订单状态-已转采购
            /// </summary>
            public const string YZCG = "2";

        }

    }
    /// <summary>
    /// 交易方式
    /// </summary>
    public class TradeTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 交易方式-现金
            /// </summary>
            public const string CASH = "现金";

            /// <summary>
            /// 交易方式-网银转账
            /// </summary>
            public const string NETBANK = "网银转账";

            /// <summary>
            /// 交易方式-POS
            /// </summary>
            public const string POS = "POS";

            /// <summary>
            /// 交易方式-微信
            /// </summary>
            public const string WECHAT = "微信";

            /// <summary>
            /// 交易方式-支付宝
            /// </summary>
            public const string ALIPAY = "支付宝";

            /// <summary>
            /// 交易方式-钱包
            /// </summary>
            public const string WALLET = "钱包";

            /// <summary>
            /// 交易方式-支票
            /// </summary>
            public const string CHECK = "支票";

            /// <summary>
            /// 交易方式-挂账
            /// </summary>
            public const string ONACCOUNT = "挂账";

            /// <summary>
            /// 交易方式-其他
            /// </summary>
            public const string OTHERS = "其他";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 交易方式-现金
            /// </summary>
            public const string CASH = "Cash";

            /// <summary>
            /// 交易方式-网银转账
            /// </summary>
            public const string NETBANK = "NetBank";

            /// <summary>
            /// 交易方式-POS
            /// </summary>
            public const string POS = "POS";

            /// <summary>
            /// 交易方式-微信
            /// </summary>
            public const string WECHAT = "Wechat";

            /// <summary>
            /// 交易方式-支付宝
            /// </summary>
            public const string ALIPAY = "AliPay";

            /// <summary>
            /// 交易方式-钱包
            /// </summary>
            public const string WALLET = "Wallet";

            /// <summary>
            /// 交易方式-支票
            /// </summary>
            public const string CHECK = "Check";

            /// <summary>
            /// 交易方式-挂账
            /// </summary>
            public const string ONACCOUNT = "OnAccount";

            /// <summary>
            /// 交易方式-其他
            /// </summary>
            public const string OTHERS = "Others";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 交易方式-现金
            /// </summary>
            public const string CASH = "1";

            /// <summary>
            /// 交易方式-网银转账
            /// </summary>
            public const string NETBANK = "2";

            /// <summary>
            /// 交易方式-POS
            /// </summary>
            public const string POS = "3";

            /// <summary>
            /// 交易方式-微信
            /// </summary>
            public const string WECHAT = "4";

            /// <summary>
            /// 交易方式-支付宝
            /// </summary>
            public const string ALIPAY = "5";

            /// <summary>
            /// 交易方式-钱包
            /// </summary>
            public const string WALLET = "6";

            /// <summary>
            /// 交易方式-支票
            /// </summary>
            public const string CHECK = "7";

            /// <summary>
            /// 交易方式-挂账
            /// </summary>
            public const string ONACCOUNT = "8";

            /// <summary>
            /// 交易方式-其他
            /// </summary>
            public const string OTHERS = "9";

        }

    }
    /// <summary>
    /// 付款单明细来源类型
    /// </summary>
    public class PayBillDetailSourceTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 付款单明细来源类型-手工付款
            /// </summary>
            public const string SGFK = "手工付款";

            /// <summary>
            /// 付款单明细来源类型-销售退货
            /// </summary>
            public const string XSTH = "销售退货";

            /// <summary>
            /// 付款单明细来源类型-销售返点
            /// </summary>
            public const string XSTFD = "销售返点";

            /// <summary>
            /// 付款单明细来源类型-标准采购
            /// </summary>
            public const string BZCG = "标准采购";

            /// <summary>
            /// 付款单明细来源类型-入库付款
            /// </summary>
            public const string RKFK = "入库付款";

            /// <summary>
            /// 付款单明细来源类型-销售收款
            /// </summary>
            public const string XSSK = "销售收款";

            /// <summary>
            /// 付款单明细来源类型-其他收款（赔偿）
            /// </summary>
            public const string QTSK = "其他收款（赔偿）";

            /// <summary>
            /// 付款单明细来源类型-退货收款
            /// </summary>
            public const string THSK = "退货收款";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 付款单明细来源类型-手工付款
            /// </summary>
            public const string SGFK = "SGFK";

            /// <summary>
            /// 付款单明细来源类型-销售退货
            /// </summary>
            public const string XSTH = "XSTH";

            /// <summary>
            /// 付款单明细来源类型-销售返点
            /// </summary>
            public const string XSTFD = "XSTFD";

            /// <summary>
            /// 付款单明细来源类型-标准采购
            /// </summary>
            public const string BZCG = "BZCG";

            /// <summary>
            /// 付款单明细来源类型-入库付款
            /// </summary>
            public const string RKFK = "RKFK";

            /// <summary>
            /// 付款单明细来源类型-销售收款
            /// </summary>
            public const string XSSK = "XSSK";

            /// <summary>
            /// 付款单明细来源类型-其他收款（赔偿）
            /// </summary>
            public const string QTSK = "QTSK";

            /// <summary>
            /// 付款单明细来源类型-退货收款
            /// </summary>
            public const string THSK = "THSK";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 付款单明细来源类型-手工付款
            /// </summary>
            public const string SGFK = "1";

            /// <summary>
            /// 付款单明细来源类型-销售退货
            /// </summary>
            public const string XSTH = "2";

            /// <summary>
            /// 付款单明细来源类型-销售返点
            /// </summary>
            public const string XSTFD = "3";

            /// <summary>
            /// 付款单明细来源类型-标准采购
            /// </summary>
            public const string BZCG = "4";

            /// <summary>
            /// 付款单明细来源类型-入库付款
            /// </summary>
            public const string RKFK = "5";

            /// <summary>
            /// 付款单明细来源类型-销售收款
            /// </summary>
            public const string XSSK = "6";

            /// <summary>
            /// 付款单明细来源类型-其他收款（赔偿）
            /// </summary>
            public const string QTSK = "7";

            /// <summary>
            /// 付款单明细来源类型-退货收款
            /// </summary>
            public const string THSK = "8";

        }

    }
    /// <summary>
    /// 单据审核状态
    /// </summary>
    public class ApprovalStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 单据审核状态-待审核
            /// </summary>
            public const string DSH = "待审核";

            /// <summary>
            /// 单据审核状态-已审核
            /// </summary>
            public const string YSH = "已审核";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 单据审核状态-待审核
            /// </summary>
            public const string DSH = "DSH";

            /// <summary>
            /// 单据审核状态-已审核
            /// </summary>
            public const string YSH = "YSH";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 单据审核状态-待审核
            /// </summary>
            public const string DSH = "1";

            /// <summary>
            /// 单据审核状态-已审核
            /// </summary>
            public const string YSH = "2";

        }

    }
    /// <summary>
    /// 付款单状态
    /// </summary>
    public class PayBillStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 付款单状态-已生成
            /// </summary>
            public const string YSC = "已生成";

            /// <summary>
            /// 付款单状态-执行中
            /// </summary>
            public const string ZXZ = "执行中";

            /// <summary>
            /// 付款单状态-已完成
            /// </summary>
            public const string YWC = "已完成";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 付款单状态-已生成
            /// </summary>
            public const string YSC = "YSC";

            /// <summary>
            /// 付款单状态-执行中
            /// </summary>
            public const string ZXZ = "ZXZ";

            /// <summary>
            /// 付款单状态-已完成
            /// </summary>
            public const string YWC = "YWC";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 付款单状态-已生成
            /// </summary>
            public const string YSC = "1";

            /// <summary>
            /// 付款单状态-执行中
            /// </summary>
            public const string ZXZ = "2";

            /// <summary>
            /// 付款单状态-已完成
            /// </summary>
            public const string YWC = "3";

        }

    }
    /// <summary>
    /// 收款单明细来源类型
    /// </summary>
    public class ReceiptBillDetailSourceTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 收款单明细来源类型-手工收款
            /// </summary>
            public const string SGSK = "手工收款";

            /// <summary>
            /// 收款单明细来源类型-销售收款
            /// </summary>
            public const string XSSK = "销售收款";

            /// <summary>
            /// 收款单明细来源类型-其他收款（赔偿）
            /// </summary>
            public const string QTSK = "其他收款（赔偿）";

            /// <summary>
            /// 收款单明细来源类型-退货收款
            /// </summary>
            public const string THSK = "退货收款";

            /// <summary>
            /// 收款单明细来源类型-销售退货
            /// </summary>
            public const string XSTH = "销售退货";

            /// <summary>
            /// 收款单明细来源类型-销售返点
            /// </summary>
            public const string XSTFD = "销售返点";

            /// <summary>
            /// 收款单明细来源类型-标准采购
            /// </summary>
            public const string BZCG = "标准采购";

            /// <summary>
            /// 收款单明细来源类型-入库付款
            /// </summary>
            public const string RKFK = "入库付款";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 收款单明细来源类型-手工收款
            /// </summary>
            public const string SGSK = "SGSK";

            /// <summary>
            /// 收款单明细来源类型-销售收款
            /// </summary>
            public const string XSSK = "XSSK";

            /// <summary>
            /// 收款单明细来源类型-其他收款（赔偿）
            /// </summary>
            public const string QTSK = "QTSK";

            /// <summary>
            /// 收款单明细来源类型-退货收款
            /// </summary>
            public const string THSK = "THSK";

            /// <summary>
            /// 收款单明细来源类型-销售退货
            /// </summary>
            public const string XSTH = "XSTH";

            /// <summary>
            /// 收款单明细来源类型-销售返点
            /// </summary>
            public const string XSTFD = "XSTFD";

            /// <summary>
            /// 收款单明细来源类型-标准采购
            /// </summary>
            public const string BZCG = "BZCG";

            /// <summary>
            /// 收款单明细来源类型-入库付款
            /// </summary>
            public const string RKFK = "RKFK";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 收款单明细来源类型-手工收款
            /// </summary>
            public const string SGSK = "1";

            /// <summary>
            /// 收款单明细来源类型-销售收款
            /// </summary>
            public const string XSSK = "2";

            /// <summary>
            /// 收款单明细来源类型-其他收款（赔偿）
            /// </summary>
            public const string QTSK = "3";

            /// <summary>
            /// 收款单明细来源类型-退货收款
            /// </summary>
            public const string THSK = "4";

            /// <summary>
            /// 收款单明细来源类型-销售退货
            /// </summary>
            public const string XSTH = "5";

            /// <summary>
            /// 收款单明细来源类型-销售返点
            /// </summary>
            public const string XSTFD = "6";

            /// <summary>
            /// 收款单明细来源类型-标准采购
            /// </summary>
            public const string BZCG = "7";

            /// <summary>
            /// 收款单明细来源类型-入库付款
            /// </summary>
            public const string RKFK = "8";

        }

    }
    /// <summary>
    /// 收款单状态
    /// </summary>
    public class ReceiptBillStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 收款单状态-已生成
            /// </summary>
            public const string YSC = "已生成";

            /// <summary>
            /// 收款单状态-执行中
            /// </summary>
            public const string ZXZ = "执行中";

            /// <summary>
            /// 收款单状态-已完成
            /// </summary>
            public const string YWC = "已完成";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 收款单状态-已生成
            /// </summary>
            public const string YSC = "YSC";

            /// <summary>
            /// 收款单状态-执行中
            /// </summary>
            public const string ZXZ = "ZXZ";

            /// <summary>
            /// 收款单状态-已完成
            /// </summary>
            public const string YWC = "YWC";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 收款单状态-已生成
            /// </summary>
            public const string YSC = "1";

            /// <summary>
            /// 收款单状态-执行中
            /// </summary>
            public const string ZXZ = "2";

            /// <summary>
            /// 收款单状态-已完成
            /// </summary>
            public const string YWC = "3";

        }

    }
    /// <summary>
    /// 应付单状态
    /// </summary>
    public class AccountPayableBillStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 应付单状态-已生成
            /// </summary>
            public const string YSC = "已生成";

            /// <summary>
            /// 应付单状态-执行中
            /// </summary>
            public const string ZXZ = "执行中";

            /// <summary>
            /// 应付单状态-已完成
            /// </summary>
            public const string YWC = "已完成";

            /// <summary>
            /// 应付单状态-已关闭
            /// </summary>
            public const string YGB = "已关闭";

            /// <summary>
            /// 应付单状态-已对账
            /// </summary>
            public const string YDZ = "已对账";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 应付单状态-已生成
            /// </summary>
            public const string YSC = "YSC";

            /// <summary>
            /// 应付单状态-执行中
            /// </summary>
            public const string ZXZ = "ZXZ";

            /// <summary>
            /// 应付单状态-已完成
            /// </summary>
            public const string YWC = "YWC";

            /// <summary>
            /// 应付单状态-已关闭
            /// </summary>
            public const string YGB = "YGB";

            /// <summary>
            /// 应付单状态-已对账
            /// </summary>
            public const string YDZ = "YDZ";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 应付单状态-已生成
            /// </summary>
            public const string YSC = "1";

            /// <summary>
            /// 应付单状态-执行中
            /// </summary>
            public const string ZXZ = "2";

            /// <summary>
            /// 应付单状态-已完成
            /// </summary>
            public const string YWC = "3";

            /// <summary>
            /// 应付单状态-已关闭
            /// </summary>
            public const string YGB = "4";

            /// <summary>
            /// 应付单状态-已对账
            /// </summary>
            public const string YDZ = "5";

        }

    }
    /// <summary>
    /// 应收单状态
    /// </summary>
    public class AccountReceivableBillStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 应收单状态-已生成
            /// </summary>
            public const string YSC = "已生成";

            /// <summary>
            /// 应收单状态-执行中
            /// </summary>
            public const string ZXZ = "执行中";

            /// <summary>
            /// 应收单状态-已完成
            /// </summary>
            public const string YWC = "已完成";

            /// <summary>
            /// 应收单状态-已关闭
            /// </summary>
            public const string YGB = "已关闭";

            /// <summary>
            /// 应收单状态-已对账
            /// </summary>
            public const string YDZ = "已对账";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 应收单状态-已生成
            /// </summary>
            public const string YSC = "YSC";

            /// <summary>
            /// 应收单状态-执行中
            /// </summary>
            public const string ZXZ = "ZXZ";

            /// <summary>
            /// 应收单状态-已完成
            /// </summary>
            public const string YWC = "YWC";

            /// <summary>
            /// 应收单状态-已关闭
            /// </summary>
            public const string YGB = "YGB";

            /// <summary>
            /// 应收单状态-已对账
            /// </summary>
            public const string YDZ = "YDZ";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 应收单状态-已生成
            /// </summary>
            public const string YSC = "1";

            /// <summary>
            /// 应收单状态-执行中
            /// </summary>
            public const string ZXZ = "2";

            /// <summary>
            /// 应收单状态-已完成
            /// </summary>
            public const string YWC = "3";

            /// <summary>
            /// 应收单状态-已关闭
            /// </summary>
            public const string YGB = "4";

            /// <summary>
            /// 应收单状态-已对账
            /// </summary>
            public const string YDZ = "5";

        }

    }
    /// <summary>
    /// 资金流转对象类型
    /// </summary>
    public class AmountTransObjectTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 资金流转对象类型-组织
            /// </summary>
            public const string ORGANIZATION = "组织";

            /// <summary>
            /// 资金流转对象类型-供应商
            /// </summary>
            public const string AUTOPARTSSUPPLIER = "供应商";

            /// <summary>
            /// 资金流转对象类型-一般汽修商户
            /// </summary>
            public const string GENERALAUTOFACTORY = "一般汽修商户";

            /// <summary>
            /// 资金流转对象类型-平台内汽修商
            /// </summary>
            public const string PLATFORMAUTOFACTORY = "平台内汽修商";

            /// <summary>
            /// 资金流转对象类型-普通客户
            /// </summary>
            public const string REGULARCUSTOMER = "普通客户";

            /// <summary>
            /// 资金流转对象类型-配送人员
            /// </summary>
            public const string DELIVERYMAN = "配送人员";

            /// <summary>
            /// 资金流转对象类型-其他
            /// </summary>
            public const string OTHERS = "其他";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 资金流转对象类型-组织
            /// </summary>
            public const string ORGANIZATION = "Organization";

            /// <summary>
            /// 资金流转对象类型-供应商
            /// </summary>
            public const string AUTOPARTSSUPPLIER = "AutoPartsSupplier";

            /// <summary>
            /// 资金流转对象类型-一般汽修商户
            /// </summary>
            public const string GENERALAUTOFACTORY = "GeneralAutoFactory";

            /// <summary>
            /// 资金流转对象类型-平台内汽修商
            /// </summary>
            public const string PLATFORMAUTOFACTORY = "PlatformAutoFactory";

            /// <summary>
            /// 资金流转对象类型-普通客户
            /// </summary>
            public const string REGULARCUSTOMER = "RegularCustomer";

            /// <summary>
            /// 资金流转对象类型-配送人员
            /// </summary>
            public const string DELIVERYMAN = "DeliveryMan";

            /// <summary>
            /// 资金流转对象类型-其他
            /// </summary>
            public const string OTHERS = "Others";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 资金流转对象类型-组织
            /// </summary>
            public const string ORGANIZATION = "1";

            /// <summary>
            /// 资金流转对象类型-供应商
            /// </summary>
            public const string AUTOPARTSSUPPLIER = "2";

            /// <summary>
            /// 资金流转对象类型-一般汽修商户
            /// </summary>
            public const string GENERALAUTOFACTORY = "3";

            /// <summary>
            /// 资金流转对象类型-平台内汽修商
            /// </summary>
            public const string PLATFORMAUTOFACTORY = "4";

            /// <summary>
            /// 资金流转对象类型-普通客户
            /// </summary>
            public const string REGULARCUSTOMER = "5";

            /// <summary>
            /// 资金流转对象类型-配送人员
            /// </summary>
            public const string DELIVERYMAN = "6";

            /// <summary>
            /// 资金流转对象类型-其他
            /// </summary>
            public const string OTHERS = "7";

        }

    }
    /// <summary>
    /// 应付单来源类型
    /// </summary>
    public class AccountPayableBillSourceTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 应付单来源类型-收货应付
            /// </summary>
            public const string SHYF = "收货应付";

            /// <summary>
            /// 应付单来源类型-出库应付
            /// </summary>
            public const string CKYF = "出库应付";

            /// <summary>
            /// 应付单来源类型-手工创建
            /// </summary>
            public const string SGCJ = "手工创建";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 应付单来源类型-收货应付
            /// </summary>
            public const string SHYF = "SHYF";

            /// <summary>
            /// 应付单来源类型-出库应付
            /// </summary>
            public const string CKYF = "CKYF";

            /// <summary>
            /// 应付单来源类型-手工创建
            /// </summary>
            public const string SGCJ = "SGCJ";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 应付单来源类型-收货应付
            /// </summary>
            public const string SHYF = "1";

            /// <summary>
            /// 应付单来源类型-出库应付
            /// </summary>
            public const string CKYF = "2";

            /// <summary>
            /// 应付单来源类型-手工创建
            /// </summary>
            public const string SGCJ = "3";

        }

    }
    /// <summary>
    /// 单据方向
    /// </summary>
    public class BillDirectionEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 单据方向-正向
            /// </summary>
            public const string PLUS = "正向";

            /// <summary>
            /// 单据方向-负向
            /// </summary>
            public const string MINUS = "负向";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 单据方向-正向
            /// </summary>
            public const string PLUS = "Plus";

            /// <summary>
            /// 单据方向-负向
            /// </summary>
            public const string MINUS = "Minus";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 单据方向-正向
            /// </summary>
            public const string PLUS = "1";

            /// <summary>
            /// 单据方向-负向
            /// </summary>
            public const string MINUS = "2";

        }

    }
    /// <summary>
    /// 应付单操作类型
    /// </summary>
    public class AccountPayableBillOperateTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 应付单操作类型-生成
            /// </summary>
            public const string SC = "生成";

            /// <summary>
            /// 应付单操作类型-应付调整
            /// </summary>
            public const string YFTZ = "应付调整";

            /// <summary>
            /// 应付单操作类型-应付核销
            /// </summary>
            public const string YFHX = "应付核销";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 应付单操作类型-生成
            /// </summary>
            public const string SC = "SC";

            /// <summary>
            /// 应付单操作类型-应付调整
            /// </summary>
            public const string YFTZ = "YFTZ";

            /// <summary>
            /// 应付单操作类型-应付核销
            /// </summary>
            public const string YFHX = "YFHX";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 应付单操作类型-生成
            /// </summary>
            public const string SC = "1";

            /// <summary>
            /// 应付单操作类型-应付调整
            /// </summary>
            public const string YFTZ = "2";

            /// <summary>
            /// 应付单操作类型-应付核销
            /// </summary>
            public const string YFHX = "3";

        }

    }
    /// <summary>
    /// 应收单来源类型
    /// </summary>
    public class AccountReceivableBillSourceTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 应收单来源类型-销售应收
            /// </summary>
            public const string XSYS = "销售应收";

            /// <summary>
            /// 应收单来源类型-其他应收（赔偿）
            /// </summary>
            public const string QTYS = "其他应收（赔偿）";

            /// <summary>
            /// 应收单来源类型-手工创建
            /// </summary>
            public const string SGCJ = "手工创建";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 应收单来源类型-销售应收
            /// </summary>
            public const string XSYS = "XSYS";

            /// <summary>
            /// 应收单来源类型-其他应收（赔偿）
            /// </summary>
            public const string QTYS = "QTYS";

            /// <summary>
            /// 应收单来源类型-手工创建
            /// </summary>
            public const string SGCJ = "SGCJ";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 应收单来源类型-销售应收
            /// </summary>
            public const string XSYS = "1";

            /// <summary>
            /// 应收单来源类型-其他应收（赔偿）
            /// </summary>
            public const string QTYS = "2";

            /// <summary>
            /// 应收单来源类型-手工创建
            /// </summary>
            public const string SGCJ = "3";

        }

    }
    /// <summary>
    /// 应收单操作类型
    /// </summary>
    public class AccountReceivableBillOperateTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 应收单操作类型-生成
            /// </summary>
            public const string SC = "生成";

            /// <summary>
            /// 应收单操作类型-应收调整
            /// </summary>
            public const string YSTZ = "应收调整";

            /// <summary>
            /// 应收单操作类型-应收核销
            /// </summary>
            public const string YSHX = "应收核销";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 应收单操作类型-生成
            /// </summary>
            public const string SC = "SC";

            /// <summary>
            /// 应收单操作类型-应收调整
            /// </summary>
            public const string YSTZ = "YSTZ";

            /// <summary>
            /// 应收单操作类型-应收核销
            /// </summary>
            public const string YSHX = "YSHX";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 应收单操作类型-生成
            /// </summary>
            public const string SC = "1";

            /// <summary>
            /// 应收单操作类型-应收调整
            /// </summary>
            public const string YSTZ = "2";

            /// <summary>
            /// 应收单操作类型-应收核销
            /// </summary>
            public const string YSHX = "3";

        }

    }
    /// <summary>
    /// 微信用户认证类型
    /// </summary>
    public class WechatUserTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 微信用户认证类型-普通客户
            /// </summary>
            public const string PT = "普通客户";

            /// <summary>
            /// 微信用户认证类型-非平台汽修商户
            /// </summary>
            public const string FPTQXSH = "非平台汽修商户";

            /// <summary>
            /// 微信用户认证类型-平台内汽修商户
            /// </summary>
            public const string PTNQXSH = "平台内汽修商户";

            /// <summary>
            /// 微信用户认证类型-员工
            /// </summary>
            public const string YG = "员工";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 微信用户认证类型-普通客户
            /// </summary>
            public const string PT = "PT";

            /// <summary>
            /// 微信用户认证类型-非平台汽修商户
            /// </summary>
            public const string FPTQXSH = "FPTQXSH";

            /// <summary>
            /// 微信用户认证类型-平台内汽修商户
            /// </summary>
            public const string PTNQXSH = "PTNQXSH";

            /// <summary>
            /// 微信用户认证类型-员工
            /// </summary>
            public const string YG = "YG";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 微信用户认证类型-普通客户
            /// </summary>
            public const string PT = "1";

            /// <summary>
            /// 微信用户认证类型-非平台汽修商户
            /// </summary>
            public const string FPTQXSH = "2";

            /// <summary>
            /// 微信用户认证类型-平台内汽修商户
            /// </summary>
            public const string PTNQXSH = "3";

            /// <summary>
            /// 微信用户认证类型-员工
            /// </summary>
            public const string YG = "4";

        }

    }
    /// <summary>
    /// 微信用户异动类型
    /// </summary>
    public class UserDetailChangeTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 微信用户异动类型-认证
            /// </summary>
            public const string RZ = "认证";

            /// <summary>
            /// 微信用户异动类型-取消认证
            /// </summary>
            public const string QXRZ = "取消认证";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 微信用户异动类型-认证
            /// </summary>
            public const string RZ = "RZ";

            /// <summary>
            /// 微信用户异动类型-取消认证
            /// </summary>
            public const string QXRZ = "QXRZ";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 微信用户异动类型-认证
            /// </summary>
            public const string RZ = "1";

            /// <summary>
            /// 微信用户异动类型-取消认证
            /// </summary>
            public const string QXRZ = "2";

        }

    }
    /// <summary>
    /// 网卡地址类型
    /// </summary>
    public class NetworkCardTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 网卡地址类型-本地网卡
            /// </summary>
            public const string LOCAL = "本地网卡";

            /// <summary>
            /// 网卡地址类型-无线网卡
            /// </summary>
            public const string WIRELESSLAN = "无线网卡";

            /// <summary>
            /// 网卡地址类型-虚拟网卡
            /// </summary>
            public const string VIRTUAL = "虚拟网卡";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 网卡地址类型-本地网卡
            /// </summary>
            public const string LOCAL = "Local";

            /// <summary>
            /// 网卡地址类型-无线网卡
            /// </summary>
            public const string WIRELESSLAN = "WirelessLAN";

            /// <summary>
            /// 网卡地址类型-虚拟网卡
            /// </summary>
            public const string VIRTUAL = "Virtual";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 网卡地址类型-本地网卡
            /// </summary>
            public const string LOCAL = "1";

            /// <summary>
            /// 网卡地址类型-无线网卡
            /// </summary>
            public const string WIRELESSLAN = "2";

            /// <summary>
            /// 网卡地址类型-虚拟网卡
            /// </summary>
            public const string VIRTUAL = "3";

        }

    }
    /// <summary>
    /// 审核状态
    /// </summary>
    public class ApproveStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 审核状态-未审核
            /// </summary>
            public const string TOAPPROVE = "未审核";

            /// <summary>
            /// 审核状态-审核失败
            /// </summary>
            public const string APPROVEFAILED = "审核失败";

            /// <summary>
            /// 审核状态-审核通过
            /// </summary>
            public const string APPROVEPASS = "审核通过";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 审核状态-未审核
            /// </summary>
            public const string TOAPPROVE = "ToApprove";

            /// <summary>
            /// 审核状态-审核失败
            /// </summary>
            public const string APPROVEFAILED = "ApproveFailed";

            /// <summary>
            /// 审核状态-审核通过
            /// </summary>
            public const string APPROVEPASS = "ApprovePass";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 审核状态-未审核
            /// </summary>
            public const string TOAPPROVE = "1";

            /// <summary>
            /// 审核状态-审核失败
            /// </summary>
            public const string APPROVEFAILED = "2";

            /// <summary>
            /// 审核状态-审核通过
            /// </summary>
            public const string APPROVEPASS = "3";

        }

    }
    /// <summary>
    /// 库存异动类型
    /// </summary>
    public class InventoryTransTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 库存异动类型-直接入库
            /// </summary>
            public const string ZJRK = "直接入库";

            /// <summary>
            /// 库存异动类型-销售出库
            /// </summary>
            public const string XSCK = "销售出库";

            /// <summary>
            /// 库存异动类型-销售退货
            /// </summary>
            public const string XSTH = "销售退货";

            /// <summary>
            /// 库存异动类型-调拨出库
            /// </summary>
            public const string DBCK = "调拨出库";

            /// <summary>
            /// 库存异动类型-调拨入库
            /// </summary>
            public const string DBRK = "调拨入库";

            /// <summary>
            /// 库存异动类型-调拨退库
            /// </summary>
            public const string DBTK = "调拨退库";

            /// <summary>
            /// 库存异动类型-采购入库
            /// </summary>
            public const string CGRK = "采购入库";

            /// <summary>
            /// 库存异动类型-退货出库
            /// </summary>
            public const string THCK = "退货出库";

            /// <summary>
            /// 库存异动类型-盘点调整
            /// </summary>
            public const string PDTZ = "盘点调整";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 库存异动类型-直接入库
            /// </summary>
            public const string ZJRK = "ZJRK";

            /// <summary>
            /// 库存异动类型-销售出库
            /// </summary>
            public const string XSCK = "XSCK";

            /// <summary>
            /// 库存异动类型-销售退货
            /// </summary>
            public const string XSTH = "XSTH";

            /// <summary>
            /// 库存异动类型-调拨出库
            /// </summary>
            public const string DBCK = "DBCK";

            /// <summary>
            /// 库存异动类型-调拨入库
            /// </summary>
            public const string DBRK = "DBRK";

            /// <summary>
            /// 库存异动类型-调拨退库
            /// </summary>
            public const string DBTK = "DBTK";

            /// <summary>
            /// 库存异动类型-采购入库
            /// </summary>
            public const string CGRK = "CGRK";

            /// <summary>
            /// 库存异动类型-退货出库
            /// </summary>
            public const string THCK = "THCK";

            /// <summary>
            /// 库存异动类型-盘点调整
            /// </summary>
            public const string PDTZ = "PDTZ";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 库存异动类型-直接入库
            /// </summary>
            public const string ZJRK = "1";

            /// <summary>
            /// 库存异动类型-销售出库
            /// </summary>
            public const string XSCK = "2";

            /// <summary>
            /// 库存异动类型-销售退货
            /// </summary>
            public const string XSTH = "3";

            /// <summary>
            /// 库存异动类型-调拨出库
            /// </summary>
            public const string DBCK = "4";

            /// <summary>
            /// 库存异动类型-调拨入库
            /// </summary>
            public const string DBRK = "5";

            /// <summary>
            /// 库存异动类型-调拨退库
            /// </summary>
            public const string DBTK = "6";

            /// <summary>
            /// 库存异动类型-采购入库
            /// </summary>
            public const string CGRK = "7";

            /// <summary>
            /// 库存异动类型-退货出库
            /// </summary>
            public const string THCK = "8";

            /// <summary>
            /// 库存异动类型-盘点调整
            /// </summary>
            public const string PDTZ = "9";

        }

    }
    /// <summary>
    /// 物流人员类型
    /// </summary>
    public class DeliveryTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 物流人员类型-员工
            /// </summary>
            public const string YG = "员工";

            /// <summary>
            /// 物流人员类型-第三方个人
            /// </summary>
            public const string DSFGR = "第三方个人";

            /// <summary>
            /// 物流人员类型-第三方公司
            /// </summary>
            public const string DSFGS = "第三方公司";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 物流人员类型-员工
            /// </summary>
            public const string YG = "YG";

            /// <summary>
            /// 物流人员类型-第三方个人
            /// </summary>
            public const string DSFGR = "DSFGR";

            /// <summary>
            /// 物流人员类型-第三方公司
            /// </summary>
            public const string DSFGS = "DSFGS";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 物流人员类型-员工
            /// </summary>
            public const string YG = "1";

            /// <summary>
            /// 物流人员类型-第三方个人
            /// </summary>
            public const string DSFGR = "2";

            /// <summary>
            /// 物流人员类型-第三方公司
            /// </summary>
            public const string DSFGS = "3";

        }

    }
    /// <summary>
    /// 物流单状态
    /// </summary>
    public class LogisticsBillStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 物流单状态-已生成
            /// </summary>
            public const string YSC = "已生成";

            /// <summary>
            /// 物流单状态-配送中
            /// </summary>
            public const string PSZ = "配送中";

            /// <summary>
            /// 物流单状态-已签收
            /// </summary>
            public const string YQS = "已签收";

            /// <summary>
            /// 物流单状态-部分签收
            /// </summary>
            public const string BFQS = "部分签收";

            /// <summary>
            /// 物流单状态-已拒收
            /// </summary>
            public const string YJS = "已拒收";

            /// <summary>
            /// 物流单状态-已丢失
            /// </summary>
            public const string YDS = "已丢失";

            /// <summary>
            /// 物流单状态-已关闭
            /// </summary>
            public const string YGB = "已关闭";

            /// <summary>
            /// 物流单状态-已完成
            /// </summary>
            public const string YWC = "已完成";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 物流单状态-已生成
            /// </summary>
            public const string YSC = "YSC";

            /// <summary>
            /// 物流单状态-配送中
            /// </summary>
            public const string PSZ = "PSZ";

            /// <summary>
            /// 物流单状态-已签收
            /// </summary>
            public const string YQS = "YQS";

            /// <summary>
            /// 物流单状态-部分签收
            /// </summary>
            public const string BFQS = "BFQS";

            /// <summary>
            /// 物流单状态-已拒收
            /// </summary>
            public const string YJS = "YJS";

            /// <summary>
            /// 物流单状态-已丢失
            /// </summary>
            public const string YDS = "YDS";

            /// <summary>
            /// 物流单状态-已关闭
            /// </summary>
            public const string YGB = "YGB";

            /// <summary>
            /// 物流单状态-已完成
            /// </summary>
            public const string YWC = "YWC";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 物流单状态-已生成
            /// </summary>
            public const string YSC = "1";

            /// <summary>
            /// 物流单状态-配送中
            /// </summary>
            public const string PSZ = "2";

            /// <summary>
            /// 物流单状态-已签收
            /// </summary>
            public const string YQS = "3";

            /// <summary>
            /// 物流单状态-部分签收
            /// </summary>
            public const string BFQS = "4";

            /// <summary>
            /// 物流单状态-已拒收
            /// </summary>
            public const string YJS = "5";

            /// <summary>
            /// 物流单状态-已丢失
            /// </summary>
            public const string YDS = "6";

            /// <summary>
            /// 物流单状态-已关闭
            /// </summary>
            public const string YGB = "7";

            /// <summary>
            /// 物流单状态-已完成
            /// </summary>
            public const string YWC = "8";

        }

    }
    /// <summary>
    /// 支付状态
    /// </summary>
    public class PaymentStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 支付状态-未支付
            /// </summary>
            public const string WZF = "未支付";

            /// <summary>
            /// 支付状态-已支付
            /// </summary>
            public const string YZF = "已支付";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 支付状态-未支付
            /// </summary>
            public const string WZF = "WZF";

            /// <summary>
            /// 支付状态-已支付
            /// </summary>
            public const string YZF = "YZF";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 支付状态-未支付
            /// </summary>
            public const string WZF = "1";

            /// <summary>
            /// 支付状态-已支付
            /// </summary>
            public const string YZF = "2";

        }

    }
    /// <summary>
    /// 物流单来源类型
    /// </summary>
    public class DeliveryBillSourceTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 物流单来源类型-主动销售
            /// </summary>
            public const string ZDXS = "主动销售";

            /// <summary>
            /// 物流单来源类型-在线销售
            /// </summary>
            public const string ZXXS = "在线销售";

            /// <summary>
            /// 物流单来源类型-直接销售
            /// </summary>
            public const string ZJXS = "直接销售";

            /// <summary>
            /// 物流单来源类型-组织调拨
            /// </summary>
            public const string ZZDB = "组织调拨";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 物流单来源类型-主动销售
            /// </summary>
            public const string ZDXS = "ZDXS";

            /// <summary>
            /// 物流单来源类型-在线销售
            /// </summary>
            public const string ZXXS = "ZXXS";

            /// <summary>
            /// 物流单来源类型-直接销售
            /// </summary>
            public const string ZJXS = "ZJXS";

            /// <summary>
            /// 物流单来源类型-组织调拨
            /// </summary>
            public const string ZZDB = "ZZDB";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 物流单来源类型-主动销售
            /// </summary>
            public const string ZDXS = "1";

            /// <summary>
            /// 物流单来源类型-在线销售
            /// </summary>
            public const string ZXXS = "2";

            /// <summary>
            /// 物流单来源类型-直接销售
            /// </summary>
            public const string ZJXS = "3";

            /// <summary>
            /// 物流单来源类型-组织调拨
            /// </summary>
            public const string ZZDB = "4";

        }

    }
    /// <summary>
    /// 物流单明细状态
    /// </summary>
    public class LogisticsBillDetailStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 物流单明细状态-已生成
            /// </summary>
            public const string YSC = "已生成";

            /// <summary>
            /// 物流单明细状态-配送中
            /// </summary>
            public const string PSZ = "配送中";

            /// <summary>
            /// 物流单明细状态-已签收
            /// </summary>
            public const string YQS = "已签收";

            /// <summary>
            /// 物流单明细状态-已拒收
            /// </summary>
            public const string YJS = "已拒收";

            /// <summary>
            /// 物流单明细状态-已丢失
            /// </summary>
            public const string YDS = "已丢失";

            /// <summary>
            /// 物流单明细状态-已关闭
            /// </summary>
            public const string YGB = "已关闭";

            /// <summary>
            /// 物流单明细状态-已完成
            /// </summary>
            public const string YWC = "已完成";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 物流单明细状态-已生成
            /// </summary>
            public const string YSC = "YSC";

            /// <summary>
            /// 物流单明细状态-配送中
            /// </summary>
            public const string PSZ = "PSZ";

            /// <summary>
            /// 物流单明细状态-已签收
            /// </summary>
            public const string YQS = "YQS";

            /// <summary>
            /// 物流单明细状态-已拒收
            /// </summary>
            public const string YJS = "YJS";

            /// <summary>
            /// 物流单明细状态-已丢失
            /// </summary>
            public const string YDS = "YDS";

            /// <summary>
            /// 物流单明细状态-已关闭
            /// </summary>
            public const string YGB = "YGB";

            /// <summary>
            /// 物流单明细状态-已完成
            /// </summary>
            public const string YWC = "YWC";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 物流单明细状态-已生成
            /// </summary>
            public const string YSC = "1";

            /// <summary>
            /// 物流单明细状态-配送中
            /// </summary>
            public const string PSZ = "2";

            /// <summary>
            /// 物流单明细状态-已签收
            /// </summary>
            public const string YQS = "3";

            /// <summary>
            /// 物流单明细状态-已拒收
            /// </summary>
            public const string YJS = "4";

            /// <summary>
            /// 物流单明细状态-已丢失
            /// </summary>
            public const string YDS = "5";

            /// <summary>
            /// 物流单明细状态-已关闭
            /// </summary>
            public const string YGB = "6";

            /// <summary>
            /// 物流单明细状态-已完成
            /// </summary>
            public const string YWC = "7";

        }

    }
    /// <summary>
    /// 调拨单类型
    /// </summary>
    public class TransferBillTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 调拨单类型-一步式调拨
            /// </summary>
            public const string YBS = "一步式调拨";

            /// <summary>
            /// 调拨单类型-两步式调拨
            /// </summary>
            public const string LBS = "两步式调拨";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 调拨单类型-一步式调拨
            /// </summary>
            public const string YBS = "YBS";

            /// <summary>
            /// 调拨单类型-两步式调拨
            /// </summary>
            public const string LBS = "LBS";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 调拨单类型-一步式调拨
            /// </summary>
            public const string YBS = "1";

            /// <summary>
            /// 调拨单类型-两步式调拨
            /// </summary>
            public const string LBS = "2";

        }

    }
    /// <summary>
    /// 调拨类型
    /// </summary>
    public class TransferTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 调拨类型-仓库转储
            /// </summary>
            public const string CKZC = "仓库转储";

            /// <summary>
            /// 调拨类型-库位转储
            /// </summary>
            public const string KWZC = "库位转储";

            /// <summary>
            /// 调拨类型-组织间调拨
            /// </summary>
            public const string ZZJDB = "组织间调拨";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 调拨类型-仓库转储
            /// </summary>
            public const string CKZC = "CKZC";

            /// <summary>
            /// 调拨类型-库位转储
            /// </summary>
            public const string KWZC = "KWZC";

            /// <summary>
            /// 调拨类型-组织间调拨
            /// </summary>
            public const string ZZJDB = "ZZJDB";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 调拨类型-仓库转储
            /// </summary>
            public const string CKZC = "1";

            /// <summary>
            /// 调拨类型-库位转储
            /// </summary>
            public const string KWZC = "2";

            /// <summary>
            /// 调拨类型-组织间调拨
            /// </summary>
            public const string ZZJDB = "3";

        }

    }
    /// <summary>
    /// 调拨状态
    /// </summary>
    public class TransfeStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 调拨状态-已生成
            /// </summary>
            public const string YSC = "已生成";

            /// <summary>
            /// 调拨状态-待发货
            /// </summary>
            public const string DFH = "待发货";

            /// <summary>
            /// 调拨状态-已完成
            /// </summary>
            public const string YWC = "已完成";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 调拨状态-已生成
            /// </summary>
            public const string YSC = "YSC";

            /// <summary>
            /// 调拨状态-待发货
            /// </summary>
            public const string DFH = "DFH";

            /// <summary>
            /// 调拨状态-已完成
            /// </summary>
            public const string YWC = "YWC";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 调拨状态-已生成
            /// </summary>
            public const string YSC = "1";

            /// <summary>
            /// 调拨状态-待发货
            /// </summary>
            public const string DFH = "2";

            /// <summary>
            /// 调拨状态-已完成
            /// </summary>
            public const string YWC = "3";

        }

    }
    /// <summary>
    /// 盘点单状态
    /// </summary>
    public class StocktakingBillStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 盘点单状态-正在盘库
            /// </summary>
            public const string ZZPK = "正在盘库";

            /// <summary>
            /// 盘点单状态-等待确认
            /// </summary>
            public const string DDQR = "等待确认";

            /// <summary>
            /// 盘点单状态-已经确认
            /// </summary>
            public const string YJQR = "已经确认";

            /// <summary>
            /// 盘点单状态-校正完成
            /// </summary>
            public const string JZWC = "校正完成";

            /// <summary>
            /// 盘点单状态-已经取消
            /// </summary>
            public const string YJQX = "已经取消";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 盘点单状态-正在盘库
            /// </summary>
            public const string ZZPK = "ZZPK";

            /// <summary>
            /// 盘点单状态-等待确认
            /// </summary>
            public const string DDQR = "DDQR";

            /// <summary>
            /// 盘点单状态-已经确认
            /// </summary>
            public const string YJQR = "YJQR";

            /// <summary>
            /// 盘点单状态-校正完成
            /// </summary>
            public const string JZWC = "JZWC";

            /// <summary>
            /// 盘点单状态-已经取消
            /// </summary>
            public const string YJQX = "YJQX";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 盘点单状态-正在盘库
            /// </summary>
            public const string ZZPK = "1";

            /// <summary>
            /// 盘点单状态-等待确认
            /// </summary>
            public const string DDQR = "2";

            /// <summary>
            /// 盘点单状态-已经确认
            /// </summary>
            public const string YJQR = "3";

            /// <summary>
            /// 盘点单状态-校正完成
            /// </summary>
            public const string JZWC = "2";

            /// <summary>
            /// 盘点单状态-已经取消
            /// </summary>
            public const string YJQX = "3";

        }

    }
    /// <summary>
    /// 盘点结果
    /// </summary>
    public class StocktakingBillCheckResultEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 盘点结果-盘盈
            /// </summary>
            public const string PY = "盘盈";

            /// <summary>
            /// 盘点结果-盘亏
            /// </summary>
            public const string PK = "盘亏";

            /// <summary>
            /// 盘点结果-账实相符
            /// </summary>
            public const string ZSXF = "账实相符";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 盘点结果-盘盈
            /// </summary>
            public const string PY = "PY";

            /// <summary>
            /// 盘点结果-盘亏
            /// </summary>
            public const string PK = "PK";

            /// <summary>
            /// 盘点结果-账实相符
            /// </summary>
            public const string ZSXF = "ZSXF";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 盘点结果-盘盈
            /// </summary>
            public const string PY = "1";

            /// <summary>
            /// 盘点结果-盘亏
            /// </summary>
            public const string PK = "2";

            /// <summary>
            /// 盘点结果-账实相符
            /// </summary>
            public const string ZSXF = "3";

        }

    }
    /// <summary>
    /// 系统动作
    /// </summary>
    public class SystemActionEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 系统动作-新增
            /// </summary>
            public const string NEW = "新增";

            /// <summary>
            /// 系统动作-保存
            /// </summary>
            public const string SAVE = "保存";

            /// <summary>
            /// 系统动作-复制
            /// </summary>
            public const string COPY = "复制";

            /// <summary>
            /// 系统动作-删除
            /// </summary>
            public const string DELETE = "删除";

            /// <summary>
            /// 系统动作-查询
            /// </summary>
            public const string QUERY = "查询";

            /// <summary>
            /// 系统动作-清空
            /// </summary>
            public const string CLEAR = "清空";

            /// <summary>
            /// 系统动作-审核
            /// </summary>
            public const string APPROVE = "审核";

            /// <summary>
            /// 系统动作-反审核
            /// </summary>
            public const string UNAPPROVE = "反审核";

            /// <summary>
            /// 系统动作-导出
            /// </summary>
            public const string EXPORT = "导出";

            /// <summary>
            /// 系统动作-导入
            /// </summary>
            public const string IMPORT = "导入";

            /// <summary>
            /// 系统动作-打印
            /// </summary>
            public const string PRINT = "打印";

            /// <summary>
            /// 系统动作-推送消息
            /// </summary>
            public const string SENDMSG = "推送消息";

            /// <summary>
            /// 系统动作-签收
            /// </summary>
            public const string SIGNIN = "签收";

            /// <summary>
            /// 系统动作-库存校正
            /// </summary>
            public const string INVENTORYRECTIFY = "库存校正";

            /// <summary>
            /// 系统动作-启动盘点
            /// </summary>
            public const string STARTSTOCKTASK = "启动盘点";

            /// <summary>
            /// 系统动作-同步
            /// </summary>
            public const string SYNCHRONIZE = "同步";

            /// <summary>
            /// 系统动作-取消认证
            /// </summary>
            public const string CANCELCERTIFICATION = "取消认证";

            /// <summary>
            /// 系统动作-下发
            /// </summary>
            public const string DELIVER = "下发";

            /// <summary>
            /// 系统动作-取消下发
            /// </summary>
            public const string CANCELDELIVER = "取消下发";

            /// <summary>
            /// 系统动作-损益分析
            /// </summary>
            public const string ANALYSE = "损益分析";

            /// <summary>
            /// 系统动作-盘点零库存
            /// </summary>
            public const string STOCKCOUNTZEROINVENTORY = "盘点零库存";

            /// <summary>
            /// 系统动作-确认库存损益
            /// </summary>
            public const string CONFIRMPROFITANDLOSS = "确认库存损益";

            /// <summary>
            /// 系统动作-取消确认库存损益
            /// </summary>
            public const string CANCELCONFIRMPROFITANDLOSS = "取消确认库存损益";

            /// <summary>
            /// 系统动作-核实
            /// </summary>
            public const string VERIFY = "核实";

            /// <summary>
            /// 系统动作-充值
            /// </summary>
            public const string RECHARGE = "充值";

            /// <summary>
            /// 系统动作-对账
            /// </summary>
            public const string RECONCILIATION = "对账";

            /// <summary>
            /// 系统动作-提现
            /// </summary>
            public const string WITHDRAWCASH = "提现";

            /// <summary>
            /// 系统动作-销户
            /// </summary>
            public const string CLOSEACCOUNT = "销户";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 系统动作-新增
            /// </summary>
            public const string NEW = "New";

            /// <summary>
            /// 系统动作-保存
            /// </summary>
            public const string SAVE = "Save";

            /// <summary>
            /// 系统动作-复制
            /// </summary>
            public const string COPY = "Copy";

            /// <summary>
            /// 系统动作-删除
            /// </summary>
            public const string DELETE = "Delete";

            /// <summary>
            /// 系统动作-查询
            /// </summary>
            public const string QUERY = "Query";

            /// <summary>
            /// 系统动作-清空
            /// </summary>
            public const string CLEAR = "Clear";

            /// <summary>
            /// 系统动作-审核
            /// </summary>
            public const string APPROVE = "Approve";

            /// <summary>
            /// 系统动作-反审核
            /// </summary>
            public const string UNAPPROVE = "UnApprove";

            /// <summary>
            /// 系统动作-导出
            /// </summary>
            public const string EXPORT = "Export";

            /// <summary>
            /// 系统动作-导入
            /// </summary>
            public const string IMPORT = "Import";

            /// <summary>
            /// 系统动作-打印
            /// </summary>
            public const string PRINT = "Print";

            /// <summary>
            /// 系统动作-推送消息
            /// </summary>
            public const string SENDMSG = "SendMsg";

            /// <summary>
            /// 系统动作-签收
            /// </summary>
            public const string SIGNIN = "SignIn";

            /// <summary>
            /// 系统动作-库存校正
            /// </summary>
            public const string INVENTORYRECTIFY = "InventoryRectify";

            /// <summary>
            /// 系统动作-启动盘点
            /// </summary>
            public const string STARTSTOCKTASK = "StartStockTask";

            /// <summary>
            /// 系统动作-同步
            /// </summary>
            public const string SYNCHRONIZE = "Synchronize";

            /// <summary>
            /// 系统动作-取消认证
            /// </summary>
            public const string CANCELCERTIFICATION = "CancelCertification";

            /// <summary>
            /// 系统动作-下发
            /// </summary>
            public const string DELIVER = "Deliver";

            /// <summary>
            /// 系统动作-取消下发
            /// </summary>
            public const string CANCELDELIVER = "CancelDeliver";

            /// <summary>
            /// 系统动作-损益分析
            /// </summary>
            public const string ANALYSE = "Analyse";

            /// <summary>
            /// 系统动作-盘点零库存
            /// </summary>
            public const string STOCKCOUNTZEROINVENTORY = "StockCountZeroInventory";

            /// <summary>
            /// 系统动作-确认库存损益
            /// </summary>
            public const string CONFIRMPROFITANDLOSS = "ConfirmProfitAndLoss";

            /// <summary>
            /// 系统动作-取消确认库存损益
            /// </summary>
            public const string CANCELCONFIRMPROFITANDLOSS = "CancelConfirmProfitAndLoss";

            /// <summary>
            /// 系统动作-核实
            /// </summary>
            public const string VERIFY = "Verify";

            /// <summary>
            /// 系统动作-充值
            /// </summary>
            public const string RECHARGE = "Recharge";

            /// <summary>
            /// 系统动作-对账
            /// </summary>
            public const string RECONCILIATION = "Reconciliation";

            /// <summary>
            /// 系统动作-提现
            /// </summary>
            public const string WITHDRAWCASH = "WithdrawCash";

            /// <summary>
            /// 系统动作-销户
            /// </summary>
            public const string CLOSEACCOUNT = "CloseAccount";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 系统动作-新增
            /// </summary>
            public const string NEW = "1";

            /// <summary>
            /// 系统动作-保存
            /// </summary>
            public const string SAVE = "2";

            /// <summary>
            /// 系统动作-复制
            /// </summary>
            public const string COPY = "3";

            /// <summary>
            /// 系统动作-删除
            /// </summary>
            public const string DELETE = "4";

            /// <summary>
            /// 系统动作-查询
            /// </summary>
            public const string QUERY = "5";

            /// <summary>
            /// 系统动作-清空
            /// </summary>
            public const string CLEAR = "6";

            /// <summary>
            /// 系统动作-审核
            /// </summary>
            public const string APPROVE = "7";

            /// <summary>
            /// 系统动作-反审核
            /// </summary>
            public const string UNAPPROVE = "8";

            /// <summary>
            /// 系统动作-导出
            /// </summary>
            public const string EXPORT = "9";

            /// <summary>
            /// 系统动作-导入
            /// </summary>
            public const string IMPORT = "10";

            /// <summary>
            /// 系统动作-打印
            /// </summary>
            public const string PRINT = "11";

            /// <summary>
            /// 系统动作-推送消息
            /// </summary>
            public const string SENDMSG = "12";

            /// <summary>
            /// 系统动作-签收
            /// </summary>
            public const string SIGNIN = "13";

            /// <summary>
            /// 系统动作-库存校正
            /// </summary>
            public const string INVENTORYRECTIFY = "14";

            /// <summary>
            /// 系统动作-启动盘点
            /// </summary>
            public const string STARTSTOCKTASK = "15";

            /// <summary>
            /// 系统动作-同步
            /// </summary>
            public const string SYNCHRONIZE = "16";

            /// <summary>
            /// 系统动作-取消认证
            /// </summary>
            public const string CANCELCERTIFICATION = "17";

            /// <summary>
            /// 系统动作-下发
            /// </summary>
            public const string DELIVER = "18";

            /// <summary>
            /// 系统动作-取消下发
            /// </summary>
            public const string CANCELDELIVER = "19";

            /// <summary>
            /// 系统动作-损益分析
            /// </summary>
            public const string ANALYSE = "20";

            /// <summary>
            /// 系统动作-盘点零库存
            /// </summary>
            public const string STOCKCOUNTZEROINVENTORY = "21";

            /// <summary>
            /// 系统动作-确认库存损益
            /// </summary>
            public const string CONFIRMPROFITANDLOSS = "22";

            /// <summary>
            /// 系统动作-取消确认库存损益
            /// </summary>
            public const string CANCELCONFIRMPROFITANDLOSS = "23";

            /// <summary>
            /// 系统动作-核实
            /// </summary>
            public const string VERIFY = "24";

            /// <summary>
            /// 系统动作-充值
            /// </summary>
            public const string RECHARGE = "25";

            /// <summary>
            /// 系统动作-对账
            /// </summary>
            public const string RECONCILIATION = "26";

            /// <summary>
            /// 系统动作-提现
            /// </summary>
            public const string WITHDRAWCASH = "27";

            /// <summary>
            /// 系统动作-销户
            /// </summary>
            public const string CLOSEACCOUNT = "28";

        }

    }
    /// <summary>
    /// 系统导航
    /// </summary>
    public class SystemNavigateEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 系统导航-转销售
            /// </summary>
            public const string TOSALESORDER = "转销售";

            /// <summary>
            /// 系统导航-转物流
            /// </summary>
            public const string TOLOGISTICSBILL = "转物流";

            /// <summary>
            /// 系统导航-转入库
            /// </summary>
            public const string TOSTOCKINBILL = "转入库";

            /// <summary>
            /// 系统导航-在线支付
            /// </summary>
            public const string ONLINEPAY = "在线支付";

            /// <summary>
            /// 系统导航-打印条码
            /// </summary>
            public const string PRINTBARCODE = "打印条码";

            /// <summary>
            /// 系统导航-转收款
            /// </summary>
            public const string TORECEIPTBILL = "转收款";

            /// <summary>
            /// 系统导航-转付款
            /// </summary>
            public const string TOPAYBILL = "转付款";

            /// <summary>
            /// 系统导航-转采购
            /// </summary>
            public const string TOPURCHASEORDER = "转采购";

            /// <summary>
            /// 系统导航-转出库
            /// </summary>
            public const string TOSTOCKOUTBILL = "转出库";

            /// <summary>
            /// 系统导航-全部签收
            /// </summary>
            public const string ALLSIGN = "全部签收";

            /// <summary>
            /// 系统导航-转结算
            /// </summary>
            public const string TOSETTLEMENT = "转结算";

            /// <summary>
            /// 系统导航-开户
            /// </summary>
            public const string CREATEACCOUNT = "开户";

            /// <summary>
            /// 系统导航-充值
            /// </summary>
            public const string DEPOSITMONEY = "充值";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 系统导航-转销售
            /// </summary>
            public const string TOSALESORDER = "ToSalesOrder";

            /// <summary>
            /// 系统导航-转物流
            /// </summary>
            public const string TOLOGISTICSBILL = "ToLogisticsBill";

            /// <summary>
            /// 系统导航-转入库
            /// </summary>
            public const string TOSTOCKINBILL = "ToStockInBill";

            /// <summary>
            /// 系统导航-在线支付
            /// </summary>
            public const string ONLINEPAY = "OnLinePay";

            /// <summary>
            /// 系统导航-打印条码
            /// </summary>
            public const string PRINTBARCODE = "PrintBarCode";

            /// <summary>
            /// 系统导航-转收款
            /// </summary>
            public const string TORECEIPTBILL = "ToReceiptBill";

            /// <summary>
            /// 系统导航-转付款
            /// </summary>
            public const string TOPAYBILL = "ToPayBill";

            /// <summary>
            /// 系统导航-转采购
            /// </summary>
            public const string TOPURCHASEORDER = "ToPurchaseOrder";

            /// <summary>
            /// 系统导航-转出库
            /// </summary>
            public const string TOSTOCKOUTBILL = "ToStockOutBill";

            /// <summary>
            /// 系统导航-全部签收
            /// </summary>
            public const string ALLSIGN = "AllSign";

            /// <summary>
            /// 系统导航-转结算
            /// </summary>
            public const string TOSETTLEMENT = "ToSettlement";

            /// <summary>
            /// 系统导航-开户
            /// </summary>
            public const string CREATEACCOUNT = "CreateAccount";

            /// <summary>
            /// 系统导航-充值
            /// </summary>
            public const string DEPOSITMONEY = "DepositMoney";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 系统导航-转销售
            /// </summary>
            public const string TOSALESORDER = "1";

            /// <summary>
            /// 系统导航-转物流
            /// </summary>
            public const string TOLOGISTICSBILL = "2";

            /// <summary>
            /// 系统导航-转入库
            /// </summary>
            public const string TOSTOCKINBILL = "3";

            /// <summary>
            /// 系统导航-在线支付
            /// </summary>
            public const string ONLINEPAY = "4";

            /// <summary>
            /// 系统导航-打印条码
            /// </summary>
            public const string PRINTBARCODE = "5";

            /// <summary>
            /// 系统导航-转收款
            /// </summary>
            public const string TORECEIPTBILL = "6";

            /// <summary>
            /// 系统导航-转付款
            /// </summary>
            public const string TOPAYBILL = "7";

            /// <summary>
            /// 系统导航-转采购
            /// </summary>
            public const string TOPURCHASEORDER = "8";

            /// <summary>
            /// 系统导航-转出库
            /// </summary>
            public const string TOSTOCKOUTBILL = "9";

            /// <summary>
            /// 系统导航-全部签收
            /// </summary>
            public const string ALLSIGN = "10";

            /// <summary>
            /// 系统导航-转结算
            /// </summary>
            public const string TOSETTLEMENT = "11";

            /// <summary>
            /// 系统导航-开户
            /// </summary>
            public const string CREATEACCOUNT = "12";

            /// <summary>
            /// 系统导航-充值
            /// </summary>
            public const string DEPOSITMONEY = "13";

        }

    }
    /// <summary>
    /// 编码类型
    /// </summary>
    public class CodeTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 编码类型-车辆类型
            /// </summary>
            public const string VEHICLEMODEL = "车辆类型";

            /// <summary>
            /// 编码类型-配件级别
            /// </summary>
            public const string AUTOPARTSLEVEL = "配件级别";

            /// <summary>
            /// 编码类型-配件价格类别
            /// </summary>
            public const string AUTOPARTSPRICETYPE = "配件价格类别";

            /// <summary>
            /// 编码类型-车辆排量
            /// </summary>
            public const string VEHICLECAPACITY = "车辆排量";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 编码类型-车辆类型
            /// </summary>
            public const string VEHICLEMODEL = "VehicleModel";

            /// <summary>
            /// 编码类型-配件级别
            /// </summary>
            public const string AUTOPARTSLEVEL = "AutoPartsLevel";

            /// <summary>
            /// 编码类型-配件价格类别
            /// </summary>
            public const string AUTOPARTSPRICETYPE = "AutoPartsPriceType";

            /// <summary>
            /// 编码类型-车辆排量
            /// </summary>
            public const string VEHICLECAPACITY = "VehicleCapacity";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 编码类型-车辆类型
            /// </summary>
            public const string VEHICLEMODEL = "1";

            /// <summary>
            /// 编码类型-配件级别
            /// </summary>
            public const string AUTOPARTSLEVEL = "2";

            /// <summary>
            /// 编码类型-配件价格类别
            /// </summary>
            public const string AUTOPARTSPRICETYPE = "3";

            /// <summary>
            /// 编码类型-车辆排量
            /// </summary>
            public const string VEHICLECAPACITY = "4";

        }

    }
    /// <summary>
    /// 变速类型
    /// </summary>
    public class GearboxTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 变速类型-手动变速器(MT)
            /// </summary>
            public const string MT = "手动变速器(MT)";

            /// <summary>
            /// 变速类型-自动变速器(AT)
            /// </summary>
            public const string AT = "自动变速器(AT)";

            /// <summary>
            /// 变速类型-手动/自动变速器
            /// </summary>
            public const string MTAT = "手动/自动变速器";

            /// <summary>
            /// 变速类型-无级变速器(CVT)
            /// </summary>
            public const string CVT = "无级变速器(CVT)";

            /// <summary>
            /// 变速类型-自动离合变速器(AMT)
            /// </summary>
            public const string AMT = "自动离合变速器(AMT)";

            /// <summary>
            /// 变速类型-双离合变速器(DSG/DCT)
            /// </summary>
            public const string DSGDCT = "双离合变速器(DSG/DCT)";

            /// <summary>
            /// 变速类型-电动车单速变速箱
            /// </summary>
            public const string EVAT = "电动车单速变速箱";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 变速类型-手动变速器(MT)
            /// </summary>
            public const string MT = "MT";

            /// <summary>
            /// 变速类型-自动变速器(AT)
            /// </summary>
            public const string AT = "AT";

            /// <summary>
            /// 变速类型-手动/自动变速器
            /// </summary>
            public const string MTAT = "MTAT";

            /// <summary>
            /// 变速类型-无级变速器(CVT)
            /// </summary>
            public const string CVT = "CVT";

            /// <summary>
            /// 变速类型-自动离合变速器(AMT)
            /// </summary>
            public const string AMT = "AMT";

            /// <summary>
            /// 变速类型-双离合变速器(DSG/DCT)
            /// </summary>
            public const string DSGDCT = "DSGDCT";

            /// <summary>
            /// 变速类型-电动车单速变速箱
            /// </summary>
            public const string EVAT = "EVAT";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 变速类型-手动变速器(MT)
            /// </summary>
            public const string MT = "1";

            /// <summary>
            /// 变速类型-自动变速器(AT)
            /// </summary>
            public const string AT = "2";

            /// <summary>
            /// 变速类型-手动/自动变速器
            /// </summary>
            public const string MTAT = "3";

            /// <summary>
            /// 变速类型-无级变速器(CVT)
            /// </summary>
            public const string CVT = "4";

            /// <summary>
            /// 变速类型-自动离合变速器(AMT)
            /// </summary>
            public const string AMT = "5";

            /// <summary>
            /// 变速类型-双离合变速器(DSG/DCT)
            /// </summary>
            public const string DSGDCT = "6";

            /// <summary>
            /// 变速类型-电动车单速变速箱
            /// </summary>
            public const string EVAT = "7";

        }

    }
    /// <summary>
    /// 作业类型
    /// </summary>
    public class BatchJobTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 作业类型-消息推送
            /// </summary>
            public const string XXTS = "消息推送";

            /// <summary>
            /// 作业类型-调度执行
            /// </summary>
            public const string DDZX = "调度执行";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 作业类型-消息推送
            /// </summary>
            public const string XXTS = "XXTS";

            /// <summary>
            /// 作业类型-调度执行
            /// </summary>
            public const string DDZX = "DDZX";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 作业类型-消息推送
            /// </summary>
            public const string XXTS = "1";

            /// <summary>
            /// 作业类型-调度执行
            /// </summary>
            public const string DDZX = "2";

        }

    }
    /// <summary>
    /// 执行类别
    /// </summary>
    public class ExecutionTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 执行类别-执行一次
            /// </summary>
            public const string ZXYC = "执行一次";

            /// <summary>
            /// 执行类别-重复执行
            /// </summary>
            public const string CFZX = "重复执行";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 执行类别-执行一次
            /// </summary>
            public const string ZXYC = "ZXYC";

            /// <summary>
            /// 执行类别-重复执行
            /// </summary>
            public const string CFZX = "CFZX";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 执行类别-执行一次
            /// </summary>
            public const string ZXYC = "1";

            /// <summary>
            /// 执行类别-重复执行
            /// </summary>
            public const string CFZX = "2";

        }

    }
    /// <summary>
    /// 作业执行方式
    /// </summary>
    public class BatchJobExectueModeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 作业执行方式-自动
            /// </summary>
            public const string ZD = "自动";

            /// <summary>
            /// 作业执行方式-手动
            /// </summary>
            public const string SD = "手动";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 作业执行方式-自动
            /// </summary>
            public const string ZD = "ZD";

            /// <summary>
            /// 作业执行方式-手动
            /// </summary>
            public const string SD = "SD";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 作业执行方式-自动
            /// </summary>
            public const string ZD = "1";

            /// <summary>
            /// 作业执行方式-手动
            /// </summary>
            public const string SD = "2";

        }

    }
    /// <summary>
    /// 信息推送契机
    /// </summary>
    public class MessageSendOpportunityEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 信息推送契机-立即推送
            /// </summary>
            public const string LJTS = "立即推送";

            /// <summary>
            /// 信息推送契机-暂不推送
            /// </summary>
            public const string ZBTS = "暂不推送";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 信息推送契机-立即推送
            /// </summary>
            public const string LJTS = "LJTS";

            /// <summary>
            /// 信息推送契机-暂不推送
            /// </summary>
            public const string ZBTS = "ZBTS";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 信息推送契机-立即推送
            /// </summary>
            public const string LJTS = "1";

            /// <summary>
            /// 信息推送契机-暂不推送
            /// </summary>
            public const string ZBTS = "2";

        }

    }
    /// <summary>
    /// 推送信息的方式
    /// </summary>
    public class SendMsgModeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 推送信息的方式-微信
            /// </summary>
            public const string WECHAT = "微信";

            /// <summary>
            /// 推送信息的方式-电脑
            /// </summary>
            public const string PC = "电脑";

            /// <summary>
            /// 推送信息的方式-APP
            /// </summary>
            public const string APP = "APP";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 推送信息的方式-微信
            /// </summary>
            public const string WECHAT = "Wechat";

            /// <summary>
            /// 推送信息的方式-电脑
            /// </summary>
            public const string PC = "PC";

            /// <summary>
            /// 推送信息的方式-APP
            /// </summary>
            public const string APP = "APP";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 推送信息的方式-微信
            /// </summary>
            public const string WECHAT = "1";

            /// <summary>
            /// 推送信息的方式-电脑
            /// </summary>
            public const string PC = "2";

            /// <summary>
            /// 推送信息的方式-APP
            /// </summary>
            public const string APP = "3";

        }

    }
    /// <summary>
    /// 推送信息的状态
    /// </summary>
    public class SendMsgStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 推送信息的状态-已推送
            /// </summary>
            public const string YTS = "已推送";

            /// <summary>
            /// 推送信息的状态-未推送
            /// </summary>
            public const string WTS = "未推送";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 推送信息的状态-已推送
            /// </summary>
            public const string YTS = "YTS";

            /// <summary>
            /// 推送信息的状态-未推送
            /// </summary>
            public const string WTS = "WTS";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 推送信息的状态-已推送
            /// </summary>
            public const string YTS = "1";

            /// <summary>
            /// 推送信息的状态-未推送
            /// </summary>
            public const string WTS = "2";

        }

    }
    /// <summary>
    /// 消息推送状态
    /// </summary>
    public class PushMessageStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 消息推送状态-未推送
            /// </summary>
            public const string WTS = "未推送";

            /// <summary>
            /// 消息推送状态-已推送
            /// </summary>
            public const string YTS = "已推送";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 消息推送状态-未推送
            /// </summary>
            public const string WTS = "WTS";

            /// <summary>
            /// 消息推送状态-已推送
            /// </summary>
            public const string YTS = "YTS";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 消息推送状态-未推送
            /// </summary>
            public const string WTS = "1";

            /// <summary>
            /// 消息推送状态-已推送
            /// </summary>
            public const string YTS = "2";

        }

    }
    /// <summary>
    /// 作业执行频率
    /// </summary>
    public class ExecutionFrequencyEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 作业执行频率-日
            /// </summary>
            public const string DAY = "日";

            /// <summary>
            /// 作业执行频率-周
            /// </summary>
            public const string WEEK = "周";

            /// <summary>
            /// 作业执行频率-月
            /// </summary>
            public const string MONTH = "月";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 作业执行频率-日
            /// </summary>
            public const string DAY = "DAY";

            /// <summary>
            /// 作业执行频率-周
            /// </summary>
            public const string WEEK = "WEEK";

            /// <summary>
            /// 作业执行频率-月
            /// </summary>
            public const string MONTH = "MONTH";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 作业执行频率-日
            /// </summary>
            public const string DAY = "1";

            /// <summary>
            /// 作业执行频率-周
            /// </summary>
            public const string WEEK = "2";

            /// <summary>
            /// 作业执行频率-月
            /// </summary>
            public const string MONTH = "3";

        }

    }
    /// <summary>
    /// 日频率执行类别
    /// </summary>
    public class ExecutionDayTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 日频率执行类别-执行一次
            /// </summary>
            public const string ONCE = "执行一次";

            /// <summary>
            /// 日频率执行类别-间隔执行
            /// </summary>
            public const string REPEAT = "间隔执行";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 日频率执行类别-执行一次
            /// </summary>
            public const string ONCE = "ONCE";

            /// <summary>
            /// 日频率执行类别-间隔执行
            /// </summary>
            public const string REPEAT = "REPEAT";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 日频率执行类别-执行一次
            /// </summary>
            public const string ONCE = "1";

            /// <summary>
            /// 日频率执行类别-间隔执行
            /// </summary>
            public const string REPEAT = "2";

        }

    }
    /// <summary>
    /// 日执行间隔类型
    /// </summary>
    public class ExecutionDayIntervalTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 日执行间隔类型-小时
            /// </summary>
            public const string HOUR = "小时";

            /// <summary>
            /// 日执行间隔类型-分钟
            /// </summary>
            public const string MIN = "分钟";

            /// <summary>
            /// 日执行间隔类型-秒
            /// </summary>
            public const string SECOND = "秒";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 日执行间隔类型-小时
            /// </summary>
            public const string HOUR = "HOUR";

            /// <summary>
            /// 日执行间隔类型-分钟
            /// </summary>
            public const string MIN = "MIN";

            /// <summary>
            /// 日执行间隔类型-秒
            /// </summary>
            public const string SECOND = "SECOND";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 日执行间隔类型-小时
            /// </summary>
            public const string HOUR = "1";

            /// <summary>
            /// 日执行间隔类型-分钟
            /// </summary>
            public const string MIN = "2";

            /// <summary>
            /// 日执行间隔类型-秒
            /// </summary>
            public const string SECOND = "3";

        }

    }
    /// <summary>
    /// 钱包来源类型
    /// </summary>
    public class WalletSourceTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 钱包来源类型-新增
            /// </summary>
            public const string XZ = "新增";

            /// <summary>
            /// 钱包来源类型-补办
            /// </summary>
            public const string BB = "补办";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 钱包来源类型-新增
            /// </summary>
            public const string XZ = "XZ";

            /// <summary>
            /// 钱包来源类型-补办
            /// </summary>
            public const string BB = "BB";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 钱包来源类型-新增
            /// </summary>
            public const string XZ = "1";

            /// <summary>
            /// 钱包来源类型-补办
            /// </summary>
            public const string BB = "2";

        }

    }
    /// <summary>
    /// 电子钱包状态
    /// </summary>
    public class WalletStatusEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 电子钱包状态-已挂失
            /// </summary>
            public const string YGS = "已挂失";

            /// <summary>
            /// 电子钱包状态-已补办
            /// </summary>
            public const string YBB = "已补办";

            /// <summary>
            /// 电子钱包状态-正常
            /// </summary>
            public const string ZC = "正常";

            /// <summary>
            /// 电子钱包状态-已销户
            /// </summary>
            public const string YXH = "已销户";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 电子钱包状态-已挂失
            /// </summary>
            public const string YGS = "YGS";

            /// <summary>
            /// 电子钱包状态-已补办
            /// </summary>
            public const string YBB = "YBB";

            /// <summary>
            /// 电子钱包状态-正常
            /// </summary>
            public const string ZC = "ZC";

            /// <summary>
            /// 电子钱包状态-已销户
            /// </summary>
            public const string YXH = "YXH";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 电子钱包状态-已挂失
            /// </summary>
            public const string YGS = "1";

            /// <summary>
            /// 电子钱包状态-已补办
            /// </summary>
            public const string YBB = "2";

            /// <summary>
            /// 电子钱包状态-正常
            /// </summary>
            public const string ZC = "3";

            /// <summary>
            /// 电子钱包状态-已销户
            /// </summary>
            public const string YXH = "4";

        }

    }
    /// <summary>
    /// 钱包异动类型
    /// </summary>
    public class WalTransTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 钱包异动类型-开户
            /// </summary>
            public const string KH = "开户";

            /// <summary>
            /// 钱包异动类型-销户
            /// </summary>
            public const string XH = "销户";

            /// <summary>
            /// 钱包异动类型-挂失
            /// </summary>
            public const string GS = "挂失";

            /// <summary>
            /// 钱包异动类型-解挂
            /// </summary>
            public const string JG = "解挂";

            /// <summary>
            /// 钱包异动类型-冻结
            /// </summary>
            public const string DJ = "冻结";

            /// <summary>
            /// 钱包异动类型-解冻
            /// </summary>
            public const string JD = "解冻";

            /// <summary>
            /// 钱包异动类型-消费
            /// </summary>
            public const string XF = "消费";

            /// <summary>
            /// 钱包异动类型-补办
            /// </summary>
            public const string BB = "补办";

            /// <summary>
            /// 钱包异动类型-充值
            /// </summary>
            public const string CZ = "充值";

            /// <summary>
            /// 钱包异动类型-退款
            /// </summary>
            public const string TK = "退款";

            /// <summary>
            /// 钱包异动类型-提现
            /// </summary>
            public const string TX = "提现";

            /// <summary>
            /// 钱包异动类型-修改密码
            /// </summary>
            public const string XG = "修改密码";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 钱包异动类型-开户
            /// </summary>
            public const string KH = "KH";

            /// <summary>
            /// 钱包异动类型-销户
            /// </summary>
            public const string XH = "XH";

            /// <summary>
            /// 钱包异动类型-挂失
            /// </summary>
            public const string GS = "GS";

            /// <summary>
            /// 钱包异动类型-解挂
            /// </summary>
            public const string JG = "JG";

            /// <summary>
            /// 钱包异动类型-冻结
            /// </summary>
            public const string DJ = "DJ";

            /// <summary>
            /// 钱包异动类型-解冻
            /// </summary>
            public const string JD = "JD";

            /// <summary>
            /// 钱包异动类型-消费
            /// </summary>
            public const string XF = "XF";

            /// <summary>
            /// 钱包异动类型-补办
            /// </summary>
            public const string BB = "BB";

            /// <summary>
            /// 钱包异动类型-充值
            /// </summary>
            public const string CZ = "CZ";

            /// <summary>
            /// 钱包异动类型-退款
            /// </summary>
            public const string TK = "TK";

            /// <summary>
            /// 钱包异动类型-提现
            /// </summary>
            public const string TX = "TX";

            /// <summary>
            /// 钱包异动类型-修改密码
            /// </summary>
            public const string XG = "XG";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 钱包异动类型-开户
            /// </summary>
            public const string KH = "1";

            /// <summary>
            /// 钱包异动类型-销户
            /// </summary>
            public const string XH = "2";

            /// <summary>
            /// 钱包异动类型-挂失
            /// </summary>
            public const string GS = "3";

            /// <summary>
            /// 钱包异动类型-解挂
            /// </summary>
            public const string JG = "4";

            /// <summary>
            /// 钱包异动类型-冻结
            /// </summary>
            public const string DJ = "5";

            /// <summary>
            /// 钱包异动类型-解冻
            /// </summary>
            public const string JD = "6";

            /// <summary>
            /// 钱包异动类型-消费
            /// </summary>
            public const string XF = "7";

            /// <summary>
            /// 钱包异动类型-补办
            /// </summary>
            public const string BB = "8";

            /// <summary>
            /// 钱包异动类型-充值
            /// </summary>
            public const string CZ = "9";

            /// <summary>
            /// 钱包异动类型-退款
            /// </summary>
            public const string TK = "10";

            /// <summary>
            /// 钱包异动类型-提现
            /// </summary>
            public const string TX = "11";

            /// <summary>
            /// 钱包异动类型-修改密码
            /// </summary>
            public const string XG = "12";

        }

    }
    /// <summary>
    /// 登录终端
    /// </summary>
    public class LoginTerminalEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 登录终端-PC
            /// </summary>
            public const string PC = "PC";

            /// <summary>
            /// 登录终端-平板
            /// </summary>
            public const string WINTABLET = "平板";

            /// <summary>
            /// 登录终端-微信
            /// </summary>
            public const string WECHAT = "微信";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 登录终端-PC
            /// </summary>
            public const string PC = "PC";

            /// <summary>
            /// 登录终端-平板
            /// </summary>
            public const string WINTABLET = "WinTablet";

            /// <summary>
            /// 登录终端-微信
            /// </summary>
            public const string WECHAT = "Wechat";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 登录终端-PC
            /// </summary>
            public const string PC = "1";

            /// <summary>
            /// 登录终端-平板
            /// </summary>
            public const string WINTABLET = "2";

            /// <summary>
            /// 登录终端-微信
            /// </summary>
            public const string WECHAT = "3";

        }

    }
    /// <summary>
    /// 开票类型
    /// </summary>
    public class BillingTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 开票类型-普通票
            /// </summary>
            public const string GENERAL = "普通票";

            /// <summary>
            /// 开票类型-增值税发票
            /// </summary>
            public const string VAT = "增值税发票";

            /// <summary>
            /// 开票类型-收据
            /// </summary>
            public const string RECEIPT = "收据";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 开票类型-普通票
            /// </summary>
            public const string GENERAL = "General";

            /// <summary>
            /// 开票类型-增值税发票
            /// </summary>
            public const string VAT = "VAT";

            /// <summary>
            /// 开票类型-收据
            /// </summary>
            public const string RECEIPT = "Receipt";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 开票类型-普通票
            /// </summary>
            public const string GENERAL = "1";

            /// <summary>
            /// 开票类型-增值税发票
            /// </summary>
            public const string VAT = "2";

            /// <summary>
            /// 开票类型-收据
            /// </summary>
            public const string RECEIPT = "3";

        }

    }
    /// <summary>
    /// 库存图片来源类型
    /// </summary>
    public class InventoryPictureSourceTypeEnum
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 库存图片来源类型-库存
            /// </summary>
            public const string INVENTORY = "库存";

            /// <summary>
            /// 库存图片来源类型-入库明细
            /// </summary>
            public const string STOCKINDETAIL = "入库明细";

            /// <summary>
            /// 库存图片来源类型-采购明细
            /// </summary>
            public const string PURCHASEDETAIL = "采购明细";

        }

        /// <summary>
        /// 枚举编码
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 库存图片来源类型-库存
            /// </summary>
            public const string INVENTORY = "Inventory";

            /// <summary>
            /// 库存图片来源类型-入库明细
            /// </summary>
            public const string STOCKINDETAIL = "StockInDetail";

            /// <summary>
            /// 库存图片来源类型-采购明细
            /// </summary>
            public const string PURCHASEDETAIL = "PurchaseDetail";

        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 库存图片来源类型-库存
            /// </summary>
            public const string INVENTORY = "1";

            /// <summary>
            /// 库存图片来源类型-入库明细
            /// </summary>
            public const string STOCKINDETAIL = "2";

            /// <summary>
            /// 库存图片来源类型-采购明细
            /// </summary>
            public const string PURCHASEDETAIL = "3";

        }

    }
    #endregion

}
