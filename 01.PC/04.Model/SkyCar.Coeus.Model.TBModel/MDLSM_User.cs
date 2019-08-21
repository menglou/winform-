using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 用户Model
    /// </summary>
    public class MDLSM_User
    {
        #region 公共属性
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
        /// 是否允许微信认证
        /// </summary>
        public Boolean? User_IsAllowWechatCertificate { get; set; }
        /// <summary>
        /// 是否已微信认证
        /// </summary>
        public Boolean? User_IsWechatCertified { get; set; }
        /// <summary>
        /// 打印标题前缀
        /// </summary>
        public String User_PrintTitlePrefix { get; set; }
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
        /// 创建时间-开始（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeStart { get; set; }
        /// <summary>
        /// 创建时间-终了（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeEnd { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String User_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? User_UpdatedTime { get; set; }
        /// <summary>
        /// 修改时间-开始（查询条件用）
        /// </summary>
        public DateTime? _UpdatedTimeStart { get; set; }
        /// <summary>
        /// 修改时间-终了（查询条件用）
        /// </summary>
        public DateTime? _UpdatedTimeEnd { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? User_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String User_TransID { get; set; }
        /// <summary>
        /// 总记录条数（翻页用）
        /// </summary>
        public Int32? RecordCount { get; set; }
        /// <summary>
        /// 当前页面索引/要跳转的页码（翻页用）
        /// </summary>
        public Int32? PageIndex { get; set; }
        /// <summary>
        /// 页面大小（翻页用）
        /// </summary>
        public Int32? PageSize { get; set; }
        #endregion

        #region 公共属性-条件用
        /// <summary>
        /// 用户ID
        /// </summary>
        public String WHERE_User_ID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public String WHERE_User_Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public String WHERE_User_Password { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public String WHERE_User_EMPNO { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public String WHERE_User_IDNo { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public String WHERE_User_Sex { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public String WHERE_User_Address { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public String WHERE_User_PhoneNo { get; set; }
        /// <summary>
        /// 是否允许微信认证
        /// </summary>
        public Boolean? WHERE_User_IsAllowWechatCertificate { get; set; }
        /// <summary>
        /// 是否已微信认证
        /// </summary>
        public Boolean? WHERE_User_IsWechatCertified { get; set; }
        /// <summary>
        /// 打印标题前缀
        /// </summary>
        public String WHERE_User_PrintTitlePrefix { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_User_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_User_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_User_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_User_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_User_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_User_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_User_TransID { get; set; }
        #endregion

    }
}
