using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyCar.Coeus.Common.Const
{
    /// <summary>
    /// Api参数格式化字符串
    /// </summary>
    public class ApiParameter
    {
        /// <summary>
        /// 验证商户激活码
        /// </summary>
        public const string BF0001 = "MCT_ActivationCode={0}&SP_Code={1}";
        /// <summary>
        /// 获取商户组织信息
        /// </summary>
        public const string BF0002 = "MCT_Code={0}&SP_Code={1}";
        /// <summary>
        /// 商户客户端授权验证
        /// </summary>
        public const string BF0003 = "";
        /// <summary>
        /// 获取微信配置信息
        /// </summary>
        public const string BF0004 = "";
        /// <summary>
        /// SuperAdmin密码验证
        /// </summary>
        public const string BF0005 = "MCT_Code={0}&SP_Code={1}&SuperAdminPassword={2}";
        /// <summary>
        /// 获取允许的最新版本号
        /// </summary>
        public const string BF0006 = "MCT_Code={0}&SP_Code={1}";
        /// <summary>
        /// 授权验证（商户，组织，产品）
        /// </summary>
        public const string BF0007 = "MCT_Code={0}&SP_Code={1}&Org_Code={2}";
        /// <summary>
        /// 获取微信支付配置
        /// </summary>
        public const string BF0008 = "";
        /// <summary>
        /// 获取最新有效的微信AccessToken
        /// </summary>
        public const string BF0009 = "";
        /// <summary>
        /// 获取最新有效的微信JSApiTicket
        /// </summary>
        public const string BF0010 = "";
        /// <summary>
        /// 获取微信信息发送服务地址
        /// </summary>
        public const string BF0011 = "";
        /// <summary>
        /// 根据汽配供应商编码获取对应汽修商户授权信息
        /// </summary>
        public const string BF0012 = "MCT_Code={0}&MCT_ActivationCode={1}&SP_Code={2}";
        /// <summary>
        /// 根据汽配商户编码获取汽修商户服务器信息
        /// </summary>
        public const string BF0013 = "";
        /// <summary>
        /// 获取配件级别信息
        /// </summary>
        public const string BF0014 = "MCT_Code={0}&APL_Name={1}";
        /// <summary>
        /// 获取配件档案信息
        /// </summary>
        public const string BF0017 = "MCT_Code={0}&APA_SourceType={1}&APA_Name={2}&APA_Brand={3}&APA_Specification={4}&APA_UOM={5}&APA_Level={6}&APA_VehicleBrand={7}&APA_VehicleInspire={8}&APA_VehicleCapacity={9}&APA_VehicleYearModel={10}&APA_VehicleGearboxType={11}&APA_OEMNo={12}&APA_ThirdNo={13}&APA_Barcode={14}&APA_IsValid={15}";
        /// <summary>
        /// 获取单据编号
        /// </summary>
        public const string BF0018 = "MCT_Code={0}&Type={1}&Qty={2}&DocumentType={3}";
        /// <summary>
        /// 获取车辆品牌车系信息
        /// </summary>
        public const string BF0020 = "VBIS_Brand={0}&VBIS_Inspire={1}&MCT_Code={2}";
        /// <summary>
        /// 同步省市区
        /// </summary>
        public const string BF0030 = "";

        /// <summary>
        /// 核实汽修商户预定单
        /// </summary>
        public const string BF0035 = "SupMCT_Code={0}&SP_Code={1}&ARMCT_Code={2}&PO_No={3}";
        /// <summary>
        /// 同步共享库存信息到平台
        /// </summary>
        public const string BF0040 = "MCT_Code={0}&Org_Code={1}&ShareInventoryList={2}";
        /// <summary>
        /// 同步车辆信息、车辆原厂件信息、车辆品牌件信息到平台
        /// </summary>
        public const string BF0043 = "MCT_Code={0}&VehicleInfoList={1}&VehicleOemPartsInfoList={2}&VehicleThirdPartsInfoList={3}";
        /// <summary>
        /// 同步配件价格类别信息到平台
        /// </summary>
        public const string BF0044 = "MCT_Code={0}&Org_Code={1}&AutoPartsPriceTypeInfoList={2}";
        /// <summary>
        /// 同步汽配端汽修商客户配件价格类别信息到平台
        /// </summary>
        public const string BF0045 = "MCT_Code={0}&Org_Code={1}&ARCustomerAutoPartsPriceTypeInfoList={2}";
        /// <summary>
        /// 导入配件档案
        /// </summary>
        public const string BF0046 = "MCT_Code={0}&ImportAutoPartsArchiveList={1}";
    }
}
