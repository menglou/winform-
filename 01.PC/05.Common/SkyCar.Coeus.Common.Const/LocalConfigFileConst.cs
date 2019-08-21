using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.Common.Const
{
    public static class LocalConfigFileConst
    {
        public static class FileFolderName
        {
            /// <summary>
            /// 本地配置
            /// </summary>
            public static string LocalConfig = "LocalConfig";
            /// <summary>
            /// 系统配置数据
            /// </summary>
            public static string SysConData = "SysConData";
            /// <summary>
            /// 登录信息
            /// </summary>
            public static string LoginInfo = "LoginInfo";
            /// <summary>
            /// 本地文件库
            /// </summary>
            public static string LocalFileBase = "LocalFileBase";
            /// <summary>
            /// 录音文件
            /// </summary>
            public static string RecordFile = "RecordFile";

        }
        /// <summary>
        /// ini文件的Selection部分
        /// </summary>
        public static class SelectionName
        {
            /// <summary>
            /// 客户端信息
            /// </summary>
            public static string ClientInfo = "ClientInfo";
            /// <summary>
            /// 版本号信息
            /// </summary>
            public static string SoftInfo = "SoftInfo";
        }

        /// <summary>
        /// ini文件的Key部分
        /// </summary>
        public static class KeyName
        {

            /// <summary>
            /// 是否连接服务端
            /// </summary>
            public static string ConnectServer = "ConnectServer";
            /// <summary>
            /// 是否有微信服务号
            /// </summary>
            public static string HasWechatSvcAccount = "HasWechatSvcAccount";

            /// <summary>
            /// 打开标签式界面数
            /// </summary>
            public static string MaxTabNumberOfOpen = "MaxTabNumberOfOpen";

            /// <summary>
            /// 是否显示托盘提示
            /// </summary>
            public static string ShowNotifyIconText = "ShowNotifyIconText";

            /// <summary>
            /// 锁定等待时间
            /// </summary>
            public static string WaitMinutesOfLock = "WaitMinutesOfLock";

            /// <summary>
            /// 上次登录用户名
            /// </summary>
            public static string LastLoginUserName = "LastLoginUserName";

            /// <summary>
            /// 该产品当前的版本号
            /// </summary>
            public static string SoftVersionNo = "SoftVersionNo";
            /// <summary>
            /// 上次登录密码
            /// </summary>
            public static string LastLoginPassword = "LastLoginPassword";

            /// <summary>
            /// 记住上次登录组织ID
            /// </summary>
            public static string LastRembLoginOrgID = "LastRembLoginOrgID";

            /// <summary>
            /// 记住登录用户名
            /// </summary>
            public static string RembLoginUserNames = "RembLoginUserNames";

            /// <summary>
            /// 最后一次登录的用户名称
            /// </summary>
            public static string LastestUserName = "LastestUserName";
        }

        public static class ConfigFilePath
        {
            /// <summary>
            /// 所有用户登录信息
            /// </summary>
            public static string LocalLoginInfo = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + FileFolderName.LocalConfig + "\\" + FileFolderName.LoginInfo + "\\LocalLoginInfo.ini";
            /// <summary>
            /// 单个用户登录信息
            /// </summary>
            public static string UserLoginInfo = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + FileFolderName.LocalConfig + "\\" + FileFolderName.LoginInfo + "\\" + SysConst.PH_USERNAME + SysConst.ULINE + "LoginInfo.ini";
            /// <summary>
            /// 产品的版本号
            /// </summary>
            public static string SoftVersionNo = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + FileFolderName.LocalConfig + "\\" + FileFolderName.SysConData + "\\SoftConfig.ini";
            /// <summary>
            /// 系统配置时间戳信息
            /// </summary>
            public static string TimeStampInfo = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + FileFolderName.LocalConfig + "\\" + FileFolderName.SysConData + "\\TimeStampInfo.xml";

            /// <summary>
            /// 列表样式
            /// </summary>
            public static string SM_ListStyle = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + FileFolderName.LocalConfig + "\\" + FileFolderName.SysConData + "\\SM_ListStyle" + SysConst.ULINE + SysConst.PH_USERID + ".xml";

            /// <summary>
            /// 维修项目大全
            /// </summary>
            public static string AR_RepairItemSumma = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + FileFolderName.LocalConfig + "\\" + FileFolderName.SysConData + "\\AR_RepairItemSumma" + SysConst.ULINE + SysConst.PH_ORGID + ".xml";

            /// <summary>
            /// 维修技师
            /// </summary>
            public static string AR_RepairTechnician = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + FileFolderName.LocalConfig + "\\" + FileFolderName.SysConData + "\\AR_RepairTechnician" + SysConst.ULINE + SysConst.PH_ORGID + ".xml";

            /// <summary>
            /// 其他表文件路径
            /// </summary>
            public static string OtherTablePath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + FileFolderName.LocalConfig + "\\" + FileFolderName.SysConData + "\\" + SysConst.PH_TABLENAME + ".xml";
        }

        public static class TableName
        {
            public static string SM_Column = "SM_Column";

            public static string SM_Enum = "SM_Enum";

            public static string SM_CodeTable = "SM_CodeTable";

            public static string SM_Parameter = "SM_Parameter";

            public static string SM_ListStyle = "SM_ListStyle";

            public static string BM_InsuranceCompany = "BM_InsuranceCompany";

            public static string SCON_VehicleBrandInspireSumma = "SCON_VehicleBrandInspireSumma";

            public static string SCON_VehicleInspireSumma = "SCON_VehicleInspireSumma";

            public static string SCON_VehicleStatusSumma = "SCON_VehicleStatusSumma";

            public static string SCON_VehicleTroubleSumma = "SCON_VehicleTroubleSumma";

            public static string SCON_BusinessMsgTemplate = "SCON_BusinessMsgTemplate";

            public static string AR_RepairItemSumma = "AR_RepairItemSumma";

            public static string AR_RepairTechnician = "AR_RepairTechnician";

            public static string APM_AutoPartsName = "APM_AutoPartsName";
            public static string APM_AutoPartsArchive = "AutoPartsArchive";

            public static string APM_AutoPartsType = "APM_AutoPartsType";

            public static string APM_AutoPartsBrand = "APM_AutoPartsBrand";

            public static string APM_AutoPartsSpecification = "APM_AutoPartsSpecification";

            public static string APM_AutoPartsUOM = "APM_AutoPartsUOM";

            public static string APM_Supplier = "APM_Supplier";
            public static string APM_Warehouse = "APM_Warehouse";
            public static string APM_WarehouseBin = "APM_WarehouseBin";
            public static string WC_Product = "WC_Product";
            public static string SM_UserBusinessRole = "SM_UserBusinessRole";
        }
    }
}
