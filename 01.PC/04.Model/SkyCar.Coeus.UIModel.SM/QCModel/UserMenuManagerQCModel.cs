using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SM
{
    /// <summary>
    /// 用户菜单管理（组织）QCModel
    /// </summary>
    public class UserMenuManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 用户工号
        /// </summary>
        public String WHERE_UMA_User_EMPNO { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public String WHERE_UMA_User_Name { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public String WHERE_UMA_Menu_Name { get; set; }
    }
}
