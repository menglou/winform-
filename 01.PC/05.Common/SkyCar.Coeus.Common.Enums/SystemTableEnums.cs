/************************************************************************
* 文件名: XXX.SystemTableEnums.cs
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
    /// <summary>
    /// 系统表枚举
    /// </summary>
    public class SystemTableEnums
    {
        #region 系统数据表

        public static Dictionary<string, string> Tables = new Dictionary<string, string>
        {
            {"BS_AutoPartsArchive", "配件档案"},
            {"BS_AutoPartsName", "配件名称"},
            {"BS_VehicleBrandInspireSumma", "车辆品牌车系"},
            {"BS_AutoPartsLevel", "配件级别"},
            {"BS_AutoPartsType", "配件类别"},
            {"BS_AutoPartsPriceType", "配件价格类别"},
            {"BS_VehicleInfo", "车辆信息"},
            {"BS_VehicleOemPartsInfo", "车辆原厂件信息"},
            {"BS_VehicleThirdPartsInfo", "车辆品牌件信息"},
            {"BS_VehicleBrandPartsInfo", "车辆原厂品牌关联信息"},
            {"SM_CodingSegment", "编码段"},
            {"SM_EncodingRule", "编码规则"},
            {"SM_SystemNo", "系统编号"},
            {"SM_Organization", "组织管理"},
            {"SM_Menu", "菜单"},
            {"SM_MenuGroup", "菜单分组"},
            {"SM_MenuDetail", "菜单明细"},
            {"SM_Action", "系统动作"},
            {"SM_MenuDetailAction", "菜单明细动作"},
            {"SM_UserMenuAuthority", "用户菜单"},
            {"SM_UserActionAuthority", "用户菜单动作"},
            {"SM_User", "用户"},
            {"SM_UserLoginLog", "用户登录日志"},
            {"SM_UserOrg", "用户组织"},
            {"SM_UserBusinessRole", "用户业务角色"},
            {"SM_UserJobAuthority", "用户作业权限"},
            {"SM_AROrgSupMerchantAuthority", "汽配汽修商户授权"},
            {"SM_AROrgSupOrgAuthority", "汽配汽修组织授权"},
            {"SM_ClientUseLicense", "使用许可证"},
            {"SM_Parameter", "系统参数"},
            {"SM_CodeTable", "码表"},
            {"SM_Enum", "系统枚举"},
            {"SM_Message", "系统消息"},
            {"SM_PushMesage", "消息推送"},
            {"SM_ChineseRegion", "中国大区"},
            {"SM_ChineseProvince", "中国省份"},
            {"SM_ProvinceCity", "省份城市"},
            {"SM_CityDistrict", "城市区域"},
            {"FM_PayBill", "付款单"},
            {"FM_PayBillDetail", "付款单明细"},
            {"FM_ReceiptBill", "收款单"},
            {"FM_ReceiptBillDetail", "收款单明细"},
            {"FM_AccountPayableBill", "应付单"},
            {"FM_AccountPayableBillDetail", "应付单明细"},
            {"FM_AccountPayableBillLog", "应付单日志"},
            {"FM_AccountReceivableBill", "应收单"},
            {"FM_AccountReceivableBillDetail", "应收单明细"},
            {"FM_AccountReceivableBillLog", "应收单日志"},
            {"PIS_PurchaseOrder", "采购订单"},
            {"PIS_PurchaseOrderDetail", "采购订单明细"},
            {"PIS_PurchaseForecastOrder", "采购预测订单"},
            {"PIS_PurchaseForecastOrderDetail", "采购预测订单明细"},
            {"PIS_Warehouse", "仓库"},
            {"PIS_WarehouseBin", "仓位"},
            {"PIS_StockOutBill", "出库单"},
            {"PIS_StockOutBillDetail", "出库单明细"},
            {"PIS_Supplier", "供应商管理"},
            {"PIS_Inventory", "库存"},
            {"PIS_InventoryPicture", "库存图片"},
            {"PIS_InventoryTransLog", "库存异动日志"},
            {"PIS_ShareInventory", "共享库存"},
            {"PIS_StocktakingTask", "盘点任务"},
            {"PIS_StocktakingTaskDetail", "盘点任务明细"},
            {"PIS_GeneralCustomer", "普通客户"},
            {"PIS_AutoFactoryCustomer", "汽修商客户"},
            {"PIS_TransferBill", "调拨单"},
            {"PIS_TransferBillDetail", "调拨单明细"},
            {"PIS_StockInBill", "入库单"},
            {"PIS_StockInDetail", "入库单明细"},
            {"WC_Menu", "微信菜单"},
            {"WC_Message", "微信消息"},
            {"WC_User", "微信用户"},
            {"WC_UserDetail", "微信用户明细"},
            {"WC_UserDetailTrans", "微信用户明细异动"},
            {"WC_UserSubscribeTrans", "微信用户关注明细"},
            {"SD_LogisticsBill", "物流订单"},
            {"SD_LogisticsBillDetail", "物流订单明细"},
            {"SD_LogisticsBillTrans", "物流订单异动"},
            {"SD_SalesOrder", "销售订单"},
            {"SD_SalesOrderDetail", "销售订单明细"},
            {"SD_DistributePath", "下发路径"},
            {"SD_SalesTemplate", "销售模板"},
            {"SD_SalesTemplateDetail", "销售模板明细"},
            {"SD_SalesForecastOrder", "销售预测订单"},
            {"SD_SalesForecastOrderDetail", "销售预测订单明细"},
            {"CSM_BatchJob", "系统作业"},
            {"CSM_BatchJobLog", "系统作业日志"},
            {"CSM_BusinessRemindLog", "业务提醒日志"},
            {"CSM_PushMesageLog", "消息推送接收日志"},
            {"EWM_Wallet", "电子钱包"},
            {"EWM_WalletTrans", "电子钱包异动"},
        };

        #endregion

        /// <summary>
        /// 数据表中文名
        /// </summary>
        public class Name
        {
            /// <summary>
            /// 配件档案
            /// </summary>
            public const string BS_AutoPartsArchive = "配件档案";

            /// <summary>
            /// 配件名称
            /// </summary>
            public const string BS_AutoPartsName = "配件名称";

            /// <summary>
            /// 车辆品牌车系
            /// </summary>
            public const string BS_VehicleBrandInspireSumma = "车辆品牌车系";

            /// <summary>
            /// 配件级别
            /// </summary>
            public const string BS_AutoPartsLevel = "配件级别";

            /// <summary>
            /// 配件类别
            /// </summary>
            public const string BS_AutoPartsType = "配件类别";

            /// <summary>
            /// 配件价格类别
            /// </summary>
            public const string BS_AutoPartsPriceType = "配件价格类别";

            /// <summary>
            /// 车辆信息
            /// </summary>
            public const string BS_VehicleInfo = "车辆信息";

            /// <summary>
            /// 车辆原厂件信息
            /// </summary>
            public const string BS_VehicleOemPartsInfo = "车辆原厂件信息";

            /// <summary>
            /// 车辆品牌件信息
            /// </summary>
            public const string BS_VehicleThirdPartsInfo = "车辆品牌件信息";

            /// <summary>
            /// 车辆原厂品牌关联信息
            /// </summary>
            public const string BS_VehicleBrandPartsInfo = "车辆原厂品牌关联信息";

            /// <summary>
            /// 编码段
            /// </summary>
            public const string SM_CodingSegment = "编码段";

            /// <summary>
            /// 编码规则
            /// </summary>
            public const string SM_EncodingRule = "编码规则";

            /// <summary>
            /// 系统编号
            /// </summary>
            public const string SM_SystemNo = "系统编号";

            /// <summary>
            /// 组织管理
            /// </summary>
            public const string SM_Organization = "组织管理";

            /// <summary>
            /// 菜单
            /// </summary>
            public const string SM_Menu = "菜单";

            /// <summary>
            /// 菜单分组
            /// </summary>
            public const string SM_MenuGroup = "菜单分组";

            /// <summary>
            /// 菜单明细
            /// </summary>
            public const string SM_MenuDetail = "菜单明细";

            /// <summary>
            /// 系统动作
            /// </summary>
            public const string SM_Action = "系统动作";

            /// <summary>
            /// 菜单明细动作
            /// </summary>
            public const string SM_MenuDetailAction = "菜单明细动作";

            /// <summary>
            /// 用户菜单
            /// </summary>
            public const string SM_UserMenuAuthority = "用户菜单";

            /// <summary>
            /// 用户菜单动作
            /// </summary>
            public const string SM_UserActionAuthority = "用户菜单动作";

            /// <summary>
            /// 用户
            /// </summary>
            public const string SM_User = "用户";

            /// <summary>
            /// 用户登录日志
            /// </summary>
            public const string SM_UserLoginLog = "用户登录日志";

            /// <summary>
            /// 用户组织
            /// </summary>
            public const string SM_UserOrg = "用户组织";

            /// <summary>
            /// 用户业务角色
            /// </summary>
            public const string SM_UserBusinessRole = "用户业务角色";

            /// <summary>
            /// 用户作业权限
            /// </summary>
            public const string SM_UserJobAuthority = "用户作业权限";

            /// <summary>
            /// 汽配汽修商户授权
            /// </summary>
            public const string SM_AROrgSupMerchantAuthority = "汽配汽修商户授权";

            /// <summary>
            /// 汽配汽修组织授权
            /// </summary>
            public const string SM_AROrgSupOrgAuthority = "汽配汽修组织授权";

            /// <summary>
            /// 使用许可证
            /// </summary>
            public const string SM_ClientUseLicense = "使用许可证";

            /// <summary>
            /// 系统参数
            /// </summary>
            public const string SM_Parameter = "系统参数";

            /// <summary>
            /// 码表
            /// </summary>
            public const string SM_CodeTable = "码表";

            /// <summary>
            /// 系统枚举
            /// </summary>
            public const string SM_Enum = "系统枚举";

            /// <summary>
            /// 系统消息
            /// </summary>
            public const string SM_Message = "系统消息";

            /// <summary>
            /// 消息推送
            /// </summary>
            public const string SM_PushMesage = "消息推送";

            /// <summary>
            /// 中国大区
            /// </summary>
            public const string SM_ChineseRegion = "中国大区";

            /// <summary>
            /// 中国省份
            /// </summary>
            public const string SM_ChineseProvince = "中国省份";

            /// <summary>
            /// 省份城市
            /// </summary>
            public const string SM_ProvinceCity = "省份城市";

            /// <summary>
            /// 城市区域
            /// </summary>
            public const string SM_CityDistrict = "城市区域";

            /// <summary>
            /// 付款单
            /// </summary>
            public const string FM_PayBill = "付款单";

            /// <summary>
            /// 付款单明细
            /// </summary>
            public const string FM_PayBillDetail = "付款单明细";

            /// <summary>
            /// 收款单
            /// </summary>
            public const string FM_ReceiptBill = "收款单";

            /// <summary>
            /// 收款单明细
            /// </summary>
            public const string FM_ReceiptBillDetail = "收款单明细";

            /// <summary>
            /// 应付单
            /// </summary>
            public const string FM_AccountPayableBill = "应付单";

            /// <summary>
            /// 应付单明细
            /// </summary>
            public const string FM_AccountPayableBillDetail = "应付单明细";

            /// <summary>
            /// 应付单日志
            /// </summary>
            public const string FM_AccountPayableBillLog = "应付单日志";

            /// <summary>
            /// 应收单
            /// </summary>
            public const string FM_AccountReceivableBill = "应收单";

            /// <summary>
            /// 应收单明细
            /// </summary>
            public const string FM_AccountReceivableBillDetail = "应收单明细";

            /// <summary>
            /// 应收单日志
            /// </summary>
            public const string FM_AccountReceivableBillLog = "应收单日志";

            /// <summary>
            /// 采购订单
            /// </summary>
            public const string PIS_PurchaseOrder = "采购订单";

            /// <summary>
            /// 采购订单明细
            /// </summary>
            public const string PIS_PurchaseOrderDetail = "采购订单明细";

            /// <summary>
            /// 采购预测订单
            /// </summary>
            public const string PIS_PurchaseForecastOrder = "采购预测订单";

            /// <summary>
            /// 采购预测订单明细
            /// </summary>
            public const string PIS_PurchaseForecastOrderDetail = "采购预测订单明细";

            /// <summary>
            /// 仓库
            /// </summary>
            public const string PIS_Warehouse = "仓库";

            /// <summary>
            /// 仓位
            /// </summary>
            public const string PIS_WarehouseBin = "仓位";

            /// <summary>
            /// 出库单
            /// </summary>
            public const string PIS_StockOutBill = "出库单";

            /// <summary>
            /// 出库单明细
            /// </summary>
            public const string PIS_StockOutBillDetail = "出库单明细";

            /// <summary>
            /// 供应商管理
            /// </summary>
            public const string PIS_Supplier = "供应商管理";

            /// <summary>
            /// 库存
            /// </summary>
            public const string PIS_Inventory = "库存";

            /// <summary>
            /// 库存图片
            /// </summary>
            public const string PIS_InventoryPicture = "库存图片";

            /// <summary>
            /// 库存异动日志
            /// </summary>
            public const string PIS_InventoryTransLog = "库存异动日志";

            /// <summary>
            /// 共享库存
            /// </summary>
            public const string PIS_ShareInventory = "共享库存";

            /// <summary>
            /// 盘点任务
            /// </summary>
            public const string PIS_StocktakingTask = "盘点任务";

            /// <summary>
            /// 盘点任务明细
            /// </summary>
            public const string PIS_StocktakingTaskDetail = "盘点任务明细";

            /// <summary>
            /// 普通客户
            /// </summary>
            public const string PIS_GeneralCustomer = "普通客户";

            /// <summary>
            /// 汽修商客户
            /// </summary>
            public const string PIS_AutoFactoryCustomer = "汽修商客户";

            /// <summary>
            /// 调拨单
            /// </summary>
            public const string PIS_TransferBill = "调拨单";

            /// <summary>
            /// 调拨单明细
            /// </summary>
            public const string PIS_TransferBillDetail = "调拨单明细";

            /// <summary>
            /// 入库单
            /// </summary>
            public const string PIS_StockInBill = "入库单";

            /// <summary>
            /// 入库单明细
            /// </summary>
            public const string PIS_StockInDetail = "入库单明细";

            /// <summary>
            /// 微信菜单
            /// </summary>
            public const string WC_Menu = "微信菜单";

            /// <summary>
            /// 微信消息
            /// </summary>
            public const string WC_Message = "微信消息";

            /// <summary>
            /// 微信用户
            /// </summary>
            public const string WC_User = "微信用户";

            /// <summary>
            /// 微信用户明细
            /// </summary>
            public const string WC_UserDetail = "微信用户明细";

            /// <summary>
            /// 微信用户明细异动
            /// </summary>
            public const string WC_UserDetailTrans = "微信用户明细异动";

            /// <summary>
            /// 微信用户关注明细
            /// </summary>
            public const string WC_UserSubscribeTrans = "微信用户关注明细";

            /// <summary>
            /// 物流订单
            /// </summary>
            public const string SD_LogisticsBill = "物流订单";

            /// <summary>
            /// 物流订单明细
            /// </summary>
            public const string SD_LogisticsBillDetail = "物流订单明细";

            /// <summary>
            /// 物流订单异动
            /// </summary>
            public const string SD_LogisticsBillTrans = "物流订单异动";

            /// <summary>
            /// 销售订单
            /// </summary>
            public const string SD_SalesOrder = "销售订单";

            /// <summary>
            /// 销售订单明细
            /// </summary>
            public const string SD_SalesOrderDetail = "销售订单明细";

            /// <summary>
            /// 下发路径
            /// </summary>
            public const string SD_DistributePath = "下发路径";

            /// <summary>
            /// 销售模板
            /// </summary>
            public const string SD_SalesTemplate = "销售模板";

            /// <summary>
            /// 销售模板明细
            /// </summary>
            public const string SD_SalesTemplateDetail = "销售模板明细";

            /// <summary>
            /// 销售预测订单
            /// </summary>
            public const string SD_SalesForecastOrder = "销售预测订单";

            /// <summary>
            /// 销售预测订单明细
            /// </summary>
            public const string SD_SalesForecastOrderDetail = "销售预测订单明细";

            /// <summary>
            /// 系统作业
            /// </summary>
            public const string CSM_BatchJob = "系统作业";

            /// <summary>
            /// 系统作业日志
            /// </summary>
            public const string CSM_BatchJobLog = "系统作业日志";

            /// <summary>
            /// 业务提醒日志
            /// </summary>
            public const string CSM_BusinessRemindLog = "业务提醒日志";

            /// <summary>
            /// 消息推送接收日志
            /// </summary>
            public const string CSM_PushMesageLog = "消息推送接收日志";

            /// <summary>
            /// 电子钱包
            /// </summary>
            public const string EWM_Wallet = "电子钱包";

            /// <summary>
            /// 电子钱包异动
            /// </summary>
            public const string EWM_WalletTrans = "电子钱包异动";

        }

        /// <summary>
        /// 数据表英文名
        /// </summary>
        public class Code
        {
            /// <summary>
            /// 配件档案
            /// </summary>
            public const string BS_AutoPartsArchive = "BS_AutoPartsArchive";

            /// <summary>
            /// 配件名称
            /// </summary>
            public const string BS_AutoPartsName = "BS_AutoPartsName";

            /// <summary>
            /// 车辆品牌车系
            /// </summary>
            public const string BS_VehicleBrandInspireSumma = "BS_VehicleBrandInspireSumma";

            /// <summary>
            /// 配件级别
            /// </summary>
            public const string BS_AutoPartsLevel = "BS_AutoPartsLevel";

            /// <summary>
            /// 配件类别
            /// </summary>
            public const string BS_AutoPartsType = "BS_AutoPartsType";

            /// <summary>
            /// 配件价格类别
            /// </summary>
            public const string BS_AutoPartsPriceType = "BS_AutoPartsPriceType";

            /// <summary>
            /// 车辆信息
            /// </summary>
            public const string BS_VehicleInfo = "BS_VehicleInfo";

            /// <summary>
            /// 车辆原厂件信息
            /// </summary>
            public const string BS_VehicleOemPartsInfo = "BS_VehicleOemPartsInfo";

            /// <summary>
            /// 车辆品牌件信息
            /// </summary>
            public const string BS_VehicleThirdPartsInfo = "BS_VehicleThirdPartsInfo";

            /// <summary>
            /// 车辆原厂品牌关联信息
            /// </summary>
            public const string BS_VehicleBrandPartsInfo = "BS_VehicleBrandPartsInfo";

            /// <summary>
            /// 编码段
            /// </summary>
            public const string SM_CodingSegment = "SM_CodingSegment";

            /// <summary>
            /// 编码规则
            /// </summary>
            public const string SM_EncodingRule = "SM_EncodingRule";

            /// <summary>
            /// 系统编号
            /// </summary>
            public const string SM_SystemNo = "SM_SystemNo";

            /// <summary>
            /// 组织管理
            /// </summary>
            public const string SM_Organization = "SM_Organization";

            /// <summary>
            /// 菜单
            /// </summary>
            public const string SM_Menu = "SM_Menu";

            /// <summary>
            /// 菜单分组
            /// </summary>
            public const string SM_MenuGroup = "SM_MenuGroup";

            /// <summary>
            /// 菜单明细
            /// </summary>
            public const string SM_MenuDetail = "SM_MenuDetail";

            /// <summary>
            /// 系统动作
            /// </summary>
            public const string SM_Action = "SM_Action";

            /// <summary>
            /// 菜单明细动作
            /// </summary>
            public const string SM_MenuDetailAction = "SM_MenuDetailAction";

            /// <summary>
            /// 用户菜单
            /// </summary>
            public const string SM_UserMenuAuthority = "SM_UserMenuAuthority";

            /// <summary>
            /// 用户菜单动作
            /// </summary>
            public const string SM_UserActionAuthority = "SM_UserActionAuthority";

            /// <summary>
            /// 用户
            /// </summary>
            public const string SM_User = "SM_User";

            /// <summary>
            /// 用户登录日志
            /// </summary>
            public const string SM_UserLoginLog = "SM_UserLoginLog";

            /// <summary>
            /// 用户组织
            /// </summary>
            public const string SM_UserOrg = "SM_UserOrg";

            /// <summary>
            /// 用户业务角色
            /// </summary>
            public const string SM_UserBusinessRole = "SM_UserBusinessRole";

            /// <summary>
            /// 用户作业权限
            /// </summary>
            public const string SM_UserJobAuthority = "SM_UserJobAuthority";

            /// <summary>
            /// 汽配汽修商户授权
            /// </summary>
            public const string SM_AROrgSupMerchantAuthority = "SM_AROrgSupMerchantAuthority";

            /// <summary>
            /// 汽配汽修组织授权
            /// </summary>
            public const string SM_AROrgSupOrgAuthority = "SM_AROrgSupOrgAuthority";

            /// <summary>
            /// 使用许可证
            /// </summary>
            public const string SM_ClientUseLicense = "SM_ClientUseLicense";

            /// <summary>
            /// 系统参数
            /// </summary>
            public const string SM_Parameter = "SM_Parameter";

            /// <summary>
            /// 码表
            /// </summary>
            public const string SM_CodeTable = "SM_CodeTable";

            /// <summary>
            /// 系统枚举
            /// </summary>
            public const string SM_Enum = "SM_Enum";

            /// <summary>
            /// 系统消息
            /// </summary>
            public const string SM_Message = "SM_Message";

            /// <summary>
            /// 消息推送
            /// </summary>
            public const string SM_PushMesage = "SM_PushMesage";

            /// <summary>
            /// 中国大区
            /// </summary>
            public const string SM_ChineseRegion = "SM_ChineseRegion";

            /// <summary>
            /// 中国省份
            /// </summary>
            public const string SM_ChineseProvince = "SM_ChineseProvince";

            /// <summary>
            /// 省份城市
            /// </summary>
            public const string SM_ProvinceCity = "SM_ProvinceCity";

            /// <summary>
            /// 城市区域
            /// </summary>
            public const string SM_CityDistrict = "SM_CityDistrict";

            /// <summary>
            /// 付款单
            /// </summary>
            public const string FM_PayBill = "FM_PayBill";

            /// <summary>
            /// 付款单明细
            /// </summary>
            public const string FM_PayBillDetail = "FM_PayBillDetail";

            /// <summary>
            /// 收款单
            /// </summary>
            public const string FM_ReceiptBill = "FM_ReceiptBill";

            /// <summary>
            /// 收款单明细
            /// </summary>
            public const string FM_ReceiptBillDetail = "FM_ReceiptBillDetail";

            /// <summary>
            /// 应付单
            /// </summary>
            public const string FM_AccountPayableBill = "FM_AccountPayableBill";

            /// <summary>
            /// 应付单明细
            /// </summary>
            public const string FM_AccountPayableBillDetail = "FM_AccountPayableBillDetail";

            /// <summary>
            /// 应付单日志
            /// </summary>
            public const string FM_AccountPayableBillLog = "FM_AccountPayableBillLog";

            /// <summary>
            /// 应收单
            /// </summary>
            public const string FM_AccountReceivableBill = "FM_AccountReceivableBill";

            /// <summary>
            /// 应收单明细
            /// </summary>
            public const string FM_AccountReceivableBillDetail = "FM_AccountReceivableBillDetail";

            /// <summary>
            /// 应收单日志
            /// </summary>
            public const string FM_AccountReceivableBillLog = "FM_AccountReceivableBillLog";

            /// <summary>
            /// 采购订单
            /// </summary>
            public const string PIS_PurchaseOrder = "PIS_PurchaseOrder";

            /// <summary>
            /// 采购订单明细
            /// </summary>
            public const string PIS_PurchaseOrderDetail = "PIS_PurchaseOrderDetail";

            /// <summary>
            /// 采购预测订单
            /// </summary>
            public const string PIS_PurchaseForecastOrder = "PIS_PurchaseForecastOrder";

            /// <summary>
            /// 采购预测订单明细
            /// </summary>
            public const string PIS_PurchaseForecastOrderDetail = "PIS_PurchaseForecastOrderDetail";

            /// <summary>
            /// 仓库
            /// </summary>
            public const string PIS_Warehouse = "PIS_Warehouse";

            /// <summary>
            /// 仓位
            /// </summary>
            public const string PIS_WarehouseBin = "PIS_WarehouseBin";

            /// <summary>
            /// 出库单
            /// </summary>
            public const string PIS_StockOutBill = "PIS_StockOutBill";

            /// <summary>
            /// 出库单明细
            /// </summary>
            public const string PIS_StockOutBillDetail = "PIS_StockOutBillDetail";

            /// <summary>
            /// 供应商管理
            /// </summary>
            public const string PIS_Supplier = "PIS_Supplier";

            /// <summary>
            /// 库存
            /// </summary>
            public const string PIS_Inventory = "PIS_Inventory";

            /// <summary>
            /// 库存图片
            /// </summary>
            public const string PIS_InventoryPicture = "PIS_InventoryPicture";

            /// <summary>
            /// 库存异动日志
            /// </summary>
            public const string PIS_InventoryTransLog = "PIS_InventoryTransLog";

            /// <summary>
            /// 共享库存
            /// </summary>
            public const string PIS_ShareInventory = "PIS_ShareInventory";

            /// <summary>
            /// 盘点任务
            /// </summary>
            public const string PIS_StocktakingTask = "PIS_StocktakingTask";

            /// <summary>
            /// 盘点任务明细
            /// </summary>
            public const string PIS_StocktakingTaskDetail = "PIS_StocktakingTaskDetail";

            /// <summary>
            /// 普通客户
            /// </summary>
            public const string PIS_GeneralCustomer = "PIS_GeneralCustomer";

            /// <summary>
            /// 汽修商客户
            /// </summary>
            public const string PIS_AutoFactoryCustomer = "PIS_AutoFactoryCustomer";

            /// <summary>
            /// 调拨单
            /// </summary>
            public const string PIS_TransferBill = "PIS_TransferBill";

            /// <summary>
            /// 调拨单明细
            /// </summary>
            public const string PIS_TransferBillDetail = "PIS_TransferBillDetail";

            /// <summary>
            /// 入库单
            /// </summary>
            public const string PIS_StockInBill = "PIS_StockInBill";

            /// <summary>
            /// 入库单明细
            /// </summary>
            public const string PIS_StockInDetail = "PIS_StockInDetail";

            /// <summary>
            /// 微信菜单
            /// </summary>
            public const string WC_Menu = "WC_Menu";

            /// <summary>
            /// 微信消息
            /// </summary>
            public const string WC_Message = "WC_Message";

            /// <summary>
            /// 微信用户
            /// </summary>
            public const string WC_User = "WC_User";

            /// <summary>
            /// 微信用户明细
            /// </summary>
            public const string WC_UserDetail = "WC_UserDetail";

            /// <summary>
            /// 微信用户明细异动
            /// </summary>
            public const string WC_UserDetailTrans = "WC_UserDetailTrans";

            /// <summary>
            /// 微信用户关注明细
            /// </summary>
            public const string WC_UserSubscribeTrans = "WC_UserSubscribeTrans";

            /// <summary>
            /// 物流订单
            /// </summary>
            public const string SD_LogisticsBill = "SD_LogisticsBill";

            /// <summary>
            /// 物流订单明细
            /// </summary>
            public const string SD_LogisticsBillDetail = "SD_LogisticsBillDetail";

            /// <summary>
            /// 物流订单异动
            /// </summary>
            public const string SD_LogisticsBillTrans = "SD_LogisticsBillTrans";

            /// <summary>
            /// 销售订单
            /// </summary>
            public const string SD_SalesOrder = "SD_SalesOrder";

            /// <summary>
            /// 销售订单明细
            /// </summary>
            public const string SD_SalesOrderDetail = "SD_SalesOrderDetail";

            /// <summary>
            /// 下发路径
            /// </summary>
            public const string SD_DistributePath = "SD_DistributePath";

            /// <summary>
            /// 销售模板
            /// </summary>
            public const string SD_SalesTemplate = "SD_SalesTemplate";

            /// <summary>
            /// 销售模板明细
            /// </summary>
            public const string SD_SalesTemplateDetail = "SD_SalesTemplateDetail";

            /// <summary>
            /// 销售预测订单
            /// </summary>
            public const string SD_SalesForecastOrder = "SD_SalesForecastOrder";

            /// <summary>
            /// 销售预测订单明细
            /// </summary>
            public const string SD_SalesForecastOrderDetail = "SD_SalesForecastOrderDetail";

            /// <summary>
            /// 系统作业
            /// </summary>
            public const string CSM_BatchJob = "CSM_BatchJob";

            /// <summary>
            /// 系统作业日志
            /// </summary>
            public const string CSM_BatchJobLog = "CSM_BatchJobLog";

            /// <summary>
            /// 业务提醒日志
            /// </summary>
            public const string CSM_BusinessRemindLog = "CSM_BusinessRemindLog";

            /// <summary>
            /// 消息推送接收日志
            /// </summary>
            public const string CSM_PushMesageLog = "CSM_PushMesageLog";

            /// <summary>
            /// 电子钱包
            /// </summary>
            public const string EWM_Wallet = "EWM_Wallet";

            /// <summary>
            /// 电子钱包异动
            /// </summary>
            public const string EWM_WalletTrans = "EWM_WalletTrans";

        }

    }

}
