using System;

namespace SkyCar.Coeus.UIModel.Common.QCModel
{
    /// <summary>
    /// 用户查询QCModel
    /// </summary>
    public class UserQueryQCModel : BaseQCModel
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public String WHERE_User_Name { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public String WHERE_User_EMPNO { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_Org_ID { get; set; }
        /// <summary>
        /// 组织简称
        /// </summary>
        public String WHERE_Org_ShortName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_User_IsValid { get; set; }
    }
}
