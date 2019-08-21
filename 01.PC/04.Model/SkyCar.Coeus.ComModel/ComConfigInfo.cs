using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.TBModel;

namespace SkyCar.Coeus.ComModel
{
    [Serializable]
    public static class LocalConfigInfo
    {
        public static class DaysNum
        {
            private static int _delayNoSettled = 30;
            public static int DelayNoSettled
            {
                get
                {
                    return _delayNoSettled;
                }
                set
                {
                    _delayNoSettled = value;
                }
            }

            private static int _delayNoPosted = 1;
            public static int DelayNoPosted
            {
                get
                {
                    return _delayNoPosted;
                }
                set
                {
                    _delayNoPosted = value;
                }
            }

            private static int _delayReceivable = 1;
            public static int DelayReceivable
            {
                get
                {
                    return _delayReceivable;
                }
                set
                {
                    _delayReceivable = value;
                }
            }
        }

        private static string _currentSoftVersion;
        public static string CurrentSoftVersion
        {
            get
            {
                return _currentSoftVersion;
            }
            set
            {
                _currentSoftVersion = value;
            }
        }

        private static bool _isLockScreen = false;
        public static bool IsLockScreen
        {
            get
            {
                return _isLockScreen;
            }
            set
            {
                _isLockScreen = value;
            }
        }

        private static string _localMacAddress;
        public static string LocalMacAddress
        {
            get
            {
                return _localMacAddress;
            }
            set
            {
                _localMacAddress = value;
            }
        }

        private static string _recordingIP;
        public static string RecordingIP
        {
            get
            {
                return _recordingIP;
            }
            set
            {
                _recordingIP = value;
            }
        }

        private static string _recordingPort;
        public static string RecordingPort
        {
            get
            {
                return _recordingPort;
            }
            set
            {
                _recordingPort = value;
            }
        }

        private static string _fixedPhoneNo;
        public static string FixedPhoneNo
        {
            get
            {
                return _fixedPhoneNo;
            }
            set
            {
                _fixedPhoneNo = value;
            }
        }

        private static string _userPosition;
        public static string UserPosition
        {
            get
            {
                return _userPosition;
            }
            set
            {
                _userPosition = value;
            }
        }

        private static bool _autoStart = false;
        public static bool AutoStart
        {
            get
            {
                return _autoStart;
            }
            set
            {
                _autoStart = value;
            }
        }

        private static bool _isServer = false;
        public static bool IsServer
        {
            get
            {
                return _isServer;
            }
            set
            {
                _isServer = value;
            }
        }

        private static List<DBTableTimeStampUIModel> _DBTimeStampList;

        /// <summary>
        /// 数据表更新时间
        /// </summary>
        public static List<DBTableTimeStampUIModel> DBTimeStampList
        {
            get { return _DBTimeStampList; }
            set { _DBTimeStampList = value; }
        }
    }
    /// <summary>
    /// 登陆信息
    /// </summary>
    //public static class LoginConfigInfo
    //{
    //    #region 静态属性
    //    /// <summary>
    //    /// 是否超级管理员
    //    /// </summary>
    //    public static bool IsSuperAdmin { get; set; }
    //    /// <summary>
    //    /// 用户名
    //    /// </summary>
    //    public static string UserName { get; set; }
    //    /// <summary>
    //    /// 用户ID（测试ID=1）
    //    /// </summary>
    //    public static string UserID { get; set; }
    //    // <summary>
    //    /// 用户ID（测试ID=1）
    //    /// </summary>
    //    public static string Password { get; set; }
    //    /// <summary>
    //    /// 当前登录的商户编码
    //    /// </summary>
    //    public static string MCTCode { get; set; }
    //    /// <summary>
    //    /// 当前登录的产品编码
    //    /// </summary>
    //    public static string SPCode { get; set; }
    //    /// <summary>
    //    /// 当前登录的组织ID
    //    /// </summary>
    //    public static string OrgID { get; set; }
    //    /// <summary>
    //    /// 组织编码
    //    /// </summary>
    //    public static string OrgCode { get; set; }
    //    /// <summary>
    //    /// 组织全称
    //    /// </summary>
    //    public static string OrgFullName { get; set; }
    //    /// <summary>
    //    /// 组织简称
    //    /// </summary>
    //    public static string OrgShortName { get; set; }
    //    /// <summary>
    //    /// 登录组织的省份编码
    //    /// </summary>
    //    public static string OrgProvCode { get; set; }
    //    /// <summary>
    //    /// 组织地址
    //    /// </summary>
    //    public static string OrgAddress { get; set; }
    //    /// <summary>
    //    /// 组织固定电话
    //    /// </summary>
    //    public static string OrgTEL { get; set; }

    //    /// <summary>
    //    /// 组织移动电话
    //    /// </summary>
    //    public static string OrgPhoneNo { get; set; }

    //    /// <summary>
    //    /// 该组织是否独立核算
    //    /// </summary>
    //    public static Boolean? OrgIsIndependentAccounting { get; set; }
    //    /// <summary>
    //    /// 用户工号
    //    /// </summary>
    //    public static string UserEMPNO { get; set; }
    //    /// <summary>
    //    /// 角色ID,多个用“;”分隔
    //    /// </summary>
    //    public static string RoleIDs { get; set; }
    //    /// <summary>
    //    /// 角色名称,多个用“;”分隔
    //    /// </summary>
    //    public static string RoleNames { get; set; }
    //    /// <summary>
    //    /// 用户绑定的组织列表
    //    /// </summary>
    //    public static List<MDLSM_Organization> OrgList { get; set; }
    //    #endregion
    //}

    public static class SystemConfigInfo
    {
        /// <summary>
        /// 主窗体宽度
        /// </summary>
        public static int MainWindowWidth { get; set; }
        /// <summary>
        /// 主窗体高度
        /// </summary>
        public static int MainWindowHeight { get; set; }

        /// <summary>
        /// 云车研发Email地址
        /// </summary>
        public static string SkyCarDevEmailAdress { get; set; }

        /// <summary>
        /// 云车研发Email密码
        /// </summary>
        public static string SkyCarDevEmailPwd { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public static string SystemName { get; set; }

        /// <summary>
        /// 系统版权描述
        /// </summary>
        public static string CopyRightDesc { get; set; }

        /// <summary>
        /// 登录界面顶部图片名称
        /// </summary>
        public static string PicNameOfLoginFormTop { get; set; }

        /// <summary>
        /// 系统编码
        /// </summary>
        public static string SystemCode { get; set; }

        /// <summary>
        /// 平台编码
        /// </summary>
        public static string OrgPlatformCode { get; set; }

        /// <summary>
        /// 登录组织所在城市车牌前缀
        /// </summary>
        public static string LocalPlatePrefix { get; set; }

        /// <summary>
        /// 微信地址
        /// </summary>
        public static string Org_WechatAdress { get; set; }

        /// <summary>
        /// 是否连接服务端
        /// </summary>
        public static bool ConnectServer { get; set; }

        /// <summary>
        /// 是否有微信服务号
        /// </summary>
        public static bool HasWechatSvcAccount { get; set; }

        /// <summary>
        /// 文件服务器地址
        /// </summary>
        public static string FileSeverAdress { get; set; }

        /// <summary>
        /// 文件服务器地址
        /// </summary>
        public static string FileUserName { get; set; }

        /// <summary>
        /// 文件服务器地址
        /// </summary>
        public static string FilePassword { get; set; }

        /// <summary>
        /// 本地固定电话号码
        /// </summary>
        public static string FixedPhoneNo { get; set; }

        /// <summary>
        /// 充值基数
        /// </summary>
        public static int DepositTimesValue { get; set; }

        /// <summary>
        /// 数据表更新时间
        /// </summary>
        public static List<DBTableTimeStampUIModel> DBTimeStampList { get; set; }

    }

    /// <summary>
    /// 本地电脑配置信息
    /// </summary>
    public static class PCConfigInfo
    {
        /// <summary>
        /// 物理内存值(单位G)
        /// </summary>
        public static decimal PhisicalMemorySize { get; set; }
    }
}
