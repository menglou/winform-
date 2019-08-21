using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.ComModel;
using System.ComponentModel;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.DAL;

namespace SkyCar.Coeus.BLL.Common
{
    /// <summary>
    /// 登陆信息
    /// </summary>
    public class LoginInfoDAX : BLLBase
    {
        #region 静态属性
        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public static bool IsSuperAdmin { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName { get; set; }
        /// <summary>
        /// 用户ID（测试ID=1）
        /// </summary>
        public static string UserID { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public static string UserPassword { get; set; }
        /// <summary>
        /// 用户工号
        /// </summary>
        public static string UserEMPNO { get; set; }

        /// <summary>
        /// 打印标题前缀
        /// </summary>
        public static string User_PrintTitlePrefix { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public static string SplName { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public static string UserType { get; set; }
        // <summary>
        /// 用户ID（测试ID=1）
        /// </summary>
        public static string Password { get; set; }
        /// <summary>
        /// 当前登录的组织ID
        /// </summary>
        public static string OrgID { get; set; }
        /// <summary>
        /// 组织编码
        /// </summary>
        public static string OrgCode { get; set; }
        /// <summary>
        /// 组织全称
        /// </summary>
        public static string OrgFullName { get; set; }
        /// <summary>
        /// 组织简称
        /// </summary>
        public static string OrgShortName { get; set; }
        /// <summary>
        /// 组织地址
        /// </summary>
        public static string OrgAddress { get; set; }
        /// <summary>
        /// 组织固定电话
        /// </summary>
        public static string OrgTEL { get; set; }

        /// <summary>
        /// 组织移动电话
        /// </summary>
        public static string OrgPhoneNo { get; set; }
        /// <summary>
        /// 用户绑定的组织列表
        /// </summary>
        public static List<MDLSM_Organization> OrgList { get; set; }
        /// <summary>
        /// 当前登录的商户编码
        /// </summary>
        public static string MCTCode { get; set; }
        /// <summary>
        /// 当前登录的商户名称
        /// </summary>
        public static string MCTName { get; set; }
        /// <summary>
        /// 当前登录的产品编码
        /// </summary>
        public static string SPCode { get; set; }
        #endregion

        public LoginInfoDAX()
            : base(Trans.SM)
        {

        }

        #region 自定义方法

        public void InitSystemConfigData()
        {
            //初始化系统枚举
            EnumDAX.InitializeEnum();
            //初始化CodeTable
            CodeTableHelp.InitializeCode();
            //初始化系统缓存
            //CacheDAX.InitSystemParameter();
            //初始化用户习惯
            //CacheDAX.InitUserPractice(LoginInfoDAX.UserID);
            //初始化车辆的品牌车系信息
            //CacheDAX.InitVehicleBrandInspireList();

            CacheDAX.InitializeCache();
            //初始化系统消息
            SystemDAX.InitializeSystemMessage();

            //初始化汽修商户到缓存
            BLLCom.InitializeARMerchantToCache();

            //初始化汽修商户数据库配置信息
            BLLCom.InitializeARMerchantDBConfigInfo();

            //初始化系统用户
            CacheDAX.InitSystemUser();
            //初始化配件名称
            CacheDAX.InitAutoPartsName();
            //初始化配件类别
            CacheDAX.InitAutoPartsType();
            //初始化供应商
            CacheDAX.InitAutoPartsSupplier();
            //初始化仓库
            CacheDAX.InitWarehouse();
            //初始化仓位
            CacheDAX.InitWarehouseBin();
            //初始化车辆品牌
            CacheDAX.InitVehicleBrand();
            //初始化车辆车系
            CacheDAX.InitVehicleInspire();
            //初始化客户
            CacheDAX.InitCustomer();

            //初始化系统数据表和字段信息
            //SystemDAX.InitializeSystemTableAndColumnInfo();
        }

        #endregion

    }
}
