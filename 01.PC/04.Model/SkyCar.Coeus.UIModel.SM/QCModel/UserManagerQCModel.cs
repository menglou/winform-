using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SM
{
    /// <summary>
    /// 用户管理QCModel
    /// </summary>
    public class UserManagerQCModel : BaseQCModel
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
        /// 有效
        /// </summary>
        public Boolean? WHERE_User_IsValid { get; set; }

        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_UO_Org_ID { get; set; }
    }
}
