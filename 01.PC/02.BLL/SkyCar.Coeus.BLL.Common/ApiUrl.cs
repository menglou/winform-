using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using SkyCar.Coeus.Common.Const;

namespace SkyCar.Coeus.BLL.Common
{
    /// <summary>
    /// 平台API请求地址
    /// </summary>
    public class ApiUrl
    {
        /// <summary>
        /// 验证商户激活码
        /// </summary>
        public static string BF0001Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0001";
        /// <summary>
        /// 获取商户组织信息
        /// </summary>
        public static string BF0002Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0002";
        /// <summary>
        /// 商户客户端授权验证
        /// </summary>
        public static string BF0003Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0003";
        /// <summary>
        /// 获取微信配置信息
        /// </summary>
        public static string BF0004Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0004";
        /// <summary>
        /// SuperAdmin密码验证
        /// </summary>
        public static string BF0005Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0005";
        /// <summary>
        /// 获取允许的最新版本号
        /// </summary>
        public static string BF0006Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0006";
        /// <summary>
        /// 授权验证（商户，组织，产品）
        /// </summary>
        public static string BF0007Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0007";
        /// <summary>
        /// 获取微信支付配置
        /// </summary>
        public static string BF0008Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0008";
        /// <summary>
        /// 获取最新有效的微信AccessToken
        /// </summary>
        public static string BF0009Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0009";
        /// <summary>
        /// 获取最新有效的微信JSApiTicket
        /// </summary>
        public static string BF0010Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0010";
        /// <summary>
        /// 获取微信信息发送服务地址
        /// </summary>
        public static string BF0011Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0011";
        /// <summary>
        /// 根据汽配供应商编码获取对应汽修商户授权信息
        /// </summary>
        public static string BF0012Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0012";
        /// <summary>
        /// 根据汽配商户编码获取汽修商户服务器信息
        /// </summary>
        public static string BF0013Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0013";
        /// <summary>
        /// 获取配件级别信息
        /// </summary>
        public static string BF0014Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0014";
        /// <summary>
        /// 获取配件档案信息
        /// </summary>
        public static string BF0017Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0017";
        /// <summary>
        /// 获取单据编号
        /// </summary>
        public static string BF0018Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0018";
        /// <summary>
        /// 获取车辆品牌车系信息
        /// </summary>
        public static string BF0020Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0020";
        /// <summary>
        /// 同步省市区
        /// </summary>
        public static string BF0030Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0030";
        /// <summary>
        /// 核实汽修商户预定单
        /// </summary>
        public static string BF0035Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0035";
        /// <summary>
        /// 同步共享库存信息到平台
        /// </summary>
        public static string BF0040Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0040";
        /// <summary>
        /// 同步车辆信息、车辆原厂件信息、车辆品牌件信息到平台
        /// </summary>
        public static string BF0043Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0043";
        /// <summary>
        /// 同步配件价格类别信息到平台
        /// </summary>
        public static string BF0044Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0044";
        /// <summary>
        /// 同步汽配端汽修商客户配件价格类别信息到平台
        /// </summary>
        public static string BF0045Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0045";
        /// <summary>
        /// 导入配件档案
        /// </summary>
        public static string BF0046Url => ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "/API/BF0046";
    }
}
