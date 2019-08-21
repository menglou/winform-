using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.UIModel.Common
{
    public class SystemConfigInfoUIModel
    {
        #region 属性
        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public bool User_IsSuperAdmin { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public String User_ID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public String User_Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public String User_Password { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public String User_EMPNO { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public string User_RoleID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string User_RoleName { get; set; }

        /// <summary>
        /// 打印标题前缀
        /// </summary>
        public String User_PrintTitlePrefix { get; set; }

        /// <summary>
        /// 平台编码
        /// </summary>
        public string Org_PlatformCode { get; set; }

        /// <summary>
        /// 组织ID
        /// </summary>
        public String Org_ID { get; set; }

        /// <summary>
        /// 组织编码
        /// </summary>
        public string Org_Code { get; set; }

        /// <summary>
        /// 组织全称
        /// </summary>
        public string Org_FullName { get; set; }

        /// <summary>
        /// 组织简称
        /// </summary>
        public String Org_ShortName { get; set; }

        /// <summary>
        /// 组织所在省份编码
        /// </summary>
        public String Org_Prov_Code { get; set; }

        /// <summary>
        /// 组织地址
        /// </summary>
        public String Org_Address { get; set; }

        /// <summary>
        /// 组织固定电话
        /// </summary>
        public String Org_TEL { get; set; }

        /// <summary>
        /// 组织移动电话
        /// </summary>
        public String Org_PhoneNo { get; set; }

        /// <summary>
        /// 该组织是否独立核算
        /// </summary>
        public Boolean? Org_IsIndependentAccounting { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string SM_SystemName { get; set; }
        /// <summary>
        /// 系统编码
        /// </summary>
        public string SM_SystemCode { get; set; }

        /// <summary>
        /// 微信地址
        /// </summary>
        public string Org_WechatAdress { get; set; }
        #endregion
    }
}
