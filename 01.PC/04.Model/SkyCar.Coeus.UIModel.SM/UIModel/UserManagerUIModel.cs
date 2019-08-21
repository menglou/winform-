using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SM
{
    /// <summary>
    /// 用户管理UIModel
    /// </summary>
    public class UserManagerUIModel : BaseUIModel
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public String User_Name { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public String User_EMPNO { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public String User_IDNo { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public String User_Sex { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public String User_Address { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public String User_PhoneNo { get; set; }
        /// <summary>
        /// 打印标题前缀
        /// </summary>
        public String User_PrintTitlePrefix { get; set; }
        /// <summary>
        /// 是否允许微信认证
        /// </summary>
        public Boolean? User_IsAllowWechatCertificate { get; set; }
        /// <summary>
        /// 是否已微信认证
        /// </summary>
        public Boolean? User_IsWechatCertified { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? User_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String User_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? User_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String User_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? User_UpdatedTime { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String User_ID { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public String User_Password { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? User_VersionNo { get; set; }

        public Boolean IsChecked { get; set; }
    }
}
