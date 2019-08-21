namespace SkyCar.Coeus.DAL
{
    public partial class SQLID
    {
        /// <summary>
        /// 查询用户菜单和动作权限
        /// </summary>
        public static string SM_Menu_SQL01 = "Menu_SQL01";

        #region 组织管理

        /// <summary>
        /// 查询组织信息
        /// </summary>
        public static string SM_OrganizationManager_SQL01 = "OrganizationManager_SQL01";

        /// <summary>
        /// 查询 该商户名称（唯一）下 门店编码 是否存在
        /// </summary>
        public static string SM_OrganizationManager_SQL02 = "OrganizationManager_SQL02";
        /// <summary>
        /// 更新组织
        /// </summary>
        public static string SM_OrganizationManager_SQL03 = "OrganizationManager_SQL03";
        #endregion

        #region 用户管理

        /// <summary>
        /// 判断是否存在相同工号的员工
        /// </summary>
        public static string SM_UserManager_SQL01 = "UserManager_SQL01";
        /// <summary>
        /// 删除用户菜单授权
        /// </summary>
        public static string SM_UserManager_SQL02 = "UserManager_SQL02";
        /// <summary>
        /// 删除用户组织授权
        /// </summary>
        public static string SM_UserManager_SQL03 = "UserManager_SQL03";
        /// <summary>
        /// 查询用户列表
        /// </summary>
        public static string SM_UserManager_SQL04 = "UserManager_SQL04";
        /// <summary>
        /// 查询用户是否被引用过
        /// </summary>
        public static string SM_UserManager_SQL05 = "UserManager_SQL05";
        /// <summary>
        /// 查询用户组织信息
        /// </summary>
        public static string SM_UserManager_SQL06 = "UserManager_SQL06";
        /// <summary>
        /// 查询用户菜单授权信息
        /// </summary>
        public static string SM_UserManager_SQL07 = "UserManager_SQL07";
        /// <summary>
        /// 查询用户动作授权信息
        /// </summary>
        public static string SM_UserManager_SQL08 = "UserManager_SQL08";
        #endregion

        #region 用户菜单授权

        /// <summary>
        /// 查询用户拥有权限的菜单明细
        /// </summary>
        public static string SM_UserMenuManager_SQL01 = "UserMenuManager_SQL01";
        /// <summary>
        /// 查询用户拥有权限的菜单动作
        /// </summary>
        public static string SM_UserMenuManager_SQL03 = "UserMenuManager_SQL03";
        /// <summary>
        /// 查询用户拥有权限的菜单
        /// </summary>
        public static string SM_UserMenuManager_SQL04 = "UserMenuManager_SQL04";
        /// <summary>
        /// 查询用户拥有权限的作业
        /// </summary>
        public static string SM_UserMenuManager_SQL05 = "UserMenuManager_SQL05";
        #endregion

        #region 作业管理

        /// <summary>
        /// 查询作业内容是否已存在
        /// </summary>
        public static string SM_BatchJobManage_SQL01 = "BatchJobManage_SQL01";
        #endregion
    }
}
