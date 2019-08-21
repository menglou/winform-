using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.SM;
using SkyCar.Coeus.UIModel.SM.UIModel;

namespace SkyCar.Coeus.BLL.SM
{
    /// <summary>
    /// 用户管理BLL
    /// </summary>
    public class UserManagerBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.SM);
        #endregion

        #region 构造方法
        /// <summary>
        /// 用户管理BLL
        /// </summary>
        public UserManagerBLL() : base(Trans.SM)
        {

        }
        #endregion

        #region  公共方法
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramUser"></param>
        /// <param name="paramUserOrgList"></param>
        /// <returns></returns>
        public bool SaveDetailDS(UserManagerUIModel paramUser, SkyCarBindingList<UserOrgUIModel, MDLSM_UserOrg> paramUserOrgList)
        {
            #region 带事务的保存

            //将UIModel转为TBModel
            var argsUser = paramUser.ToTBModelForSaveAndDelete<MDLSM_User>();
            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存用户信息
                if (string.IsNullOrEmpty(paramUser.User_ID))
                {
                    argsUser.User_ID = Guid.NewGuid().ToString();
                    argsUser.User_CreatedBy = LoginInfoDAX.UserName;
                    argsUser.User_CreatedTime = BLLCom.GetCurStdDatetime();
                }
                argsUser.User_IsValid = true;
                argsUser.User_UpdatedBy = LoginInfoDAX.UserName;
                argsUser.User_UpdatedTime = BLLCom.GetCurStdDatetime();
                bool saveUserResult = _bll.Save(argsUser);
                if (!saveUserResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SM_User });
                    return false;
                }
                #endregion

                //保存用户组织信息
                if (paramUserOrgList != null && paramUserOrgList.InsertList != null && paramUserOrgList.InsertList.Count > 0)
                {
                    foreach (var loopUserOrg in paramUserOrgList.InsertList)
                    {
                        loopUserOrg.UO_User_ID = argsUser.User_ID ?? argsUser.WHERE_User_ID;
                        loopUserOrg.UO_CreatedBy = LoginInfoDAX.UserName;
                        loopUserOrg.UO_CreatedTime = BLLCom.GetCurStdDatetime();
                        loopUserOrg.UO_UpdatedBy = LoginInfoDAX.UserName;
                        loopUserOrg.UO_UpdatedTime = BLLCom.GetCurStdDatetime();
                    }
                    bool saveUserOrgResult = _bll.InsertByList<UserOrgUIModel, MDLSM_UserOrg>(paramUserOrgList.InsertList);
                    if (!saveUserOrgResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SM_UserOrg });
                        return false;
                    }
                }
                if (paramUserOrgList != null && paramUserOrgList.DeleteList != null && paramUserOrgList.DeleteList.Count > 0)
                {
                    foreach (var loopUserOrg in paramUserOrgList.DeleteList)
                    {
                        loopUserOrg.WHERE_UO_ID = loopUserOrg.UO_ID;
                    }
                    bool deleteUserOrgResult = _bll.DeleteByList<UserOrgUIModel, MDLSM_UserOrg>(paramUserOrgList.DeleteList);
                    if (!deleteUserOrgResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SM_UserOrg });
                        return false;
                    }
                }

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }

            //将最新数据回写给DetailDS
            _bll.CopyModel(argsUser, paramUser);

            //刷新用户缓存
            var resultUserList = CacheDAX.Get(CacheDAX.ConfigDataKey.SystemUser) as List<MDLSM_User>;
            List<MDLSM_User> newUserList = new List<MDLSM_User>();
            if (resultUserList != null)
            {
                newUserList = resultUserList;
                if (resultUserList.All(x => x.User_ID != argsUser.User_ID && x.User_Name != argsUser.User_Name))
                {
                    newUserList.Insert(0, argsUser);
                    CacheDAX.Add(CacheDAX.ConfigDataKey.SystemUser, newUserList, true);
                }
            }
            else
            {
                newUserList.Add(argsUser);
                CacheDAX.Add(CacheDAX.ConfigDataKey.SystemUser, newUserList, true);
            }
            return true;

            #endregion
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="paramUserList"></param>
        /// <returns></returns>
        public bool DeleteUserInfo(List<MDLSM_User> paramUserList)
        {
            //验证
            if (paramUserList == null || paramUserList.Count == 0)
            {
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0016, new object[] { SystemTableEnums.Name.SM_User, SystemActionEnum.Name.DELETE });
                return false;
            }

            #region 准备数据
            //待删除的[用户组织]列表
            List<MDLSM_UserOrg> deleteUserOrgList = new List<MDLSM_UserOrg>();
            //待删除的[用户菜单授权]列表
            List<MDLSM_UserMenuAuthority> deleteUserMenuAuthoryList = new List<MDLSM_UserMenuAuthority>();
            //待删除的[用户动作授权]列表
            List<MDLSM_UserActionAuthority> deleteUserActionAuthoryList = new List<MDLSM_UserActionAuthority>();

            string userIdString = SysConst.Semicolon_DBC;
            foreach (var loopUser in paramUserList)
            {
                if (string.IsNullOrEmpty(loopUser.User_ID))
                {
                    continue;
                }
                userIdString += loopUser.User_ID + SysConst.Semicolon_DBC;
            }
            //查询[用户组织]列表
            _bll.QueryForList(SQLID.SM_UserManager_SQL06, userIdString, deleteUserOrgList);
            foreach (var loopDeleteUserOrg in deleteUserOrgList)
            {
                loopDeleteUserOrg.WHERE_UO_ID = loopDeleteUserOrg.UO_ID;
            }
            //查询[用户菜单授权]列表
            _bll.QueryForList(SQLID.SM_UserManager_SQL07, userIdString, deleteUserMenuAuthoryList);
            foreach (var loopDeleteUserMenuAuthory in deleteUserMenuAuthoryList)
            {
                loopDeleteUserMenuAuthory.WHERE_UMA_ID = loopDeleteUserMenuAuthory.UMA_ID;
            }
            //查询[用户动作授权]列表
            _bll.QueryForList(SQLID.SM_UserManager_SQL08, userIdString, deleteUserActionAuthoryList);
            foreach (var loopDeleteUserActionAuthory in deleteUserActionAuthoryList)
            {
                loopDeleteUserActionAuthory.WHERE_UAA_ID = loopDeleteUserActionAuthory.UAA_ID;
            }

            #endregion

            #region 带事务的更新和删除
            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                //删除用户表
                if (paramUserList.Count > 0)
                {
                    bool deleteUserResult = _bll.DeleteByList<MDLSM_User, MDLSM_User>(paramUserList);
                    if (!deleteUserResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.SM_User });
                        return false;
                    }
                }
                //删除用户组织表【可能没有用户组织信息】
                if (deleteUserOrgList.Count > 0)
                {
                    bool deleteUserOrgResult = _bll.DeleteByList<MDLSM_UserOrg, MDLSM_UserOrg>(deleteUserOrgList);
                    if (!deleteUserOrgResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.SM_UserOrg });
                        return false;
                    }
                }
                //删除用户菜单表【可能没有用户菜单信息】
                if (deleteUserMenuAuthoryList.Count > 0)
                {
                    bool deleteUserMenuResult = _bll.DeleteByList<MDLSM_UserMenuAuthority, MDLSM_UserMenuAuthority>(deleteUserMenuAuthoryList);
                    if (!deleteUserMenuResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.SM_UserMenuAuthority });
                        return false;
                    }
                }
                //删除用户动作表【可能没有用户动作信息】
                if (deleteUserActionAuthoryList.Count > 0)
                {
                    bool deleteUserActionResult = _bll.DeleteByList<MDLSM_UserActionAuthority, MDLSM_UserActionAuthority>(deleteUserActionAuthoryList);
                    if (!deleteUserActionResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.SM_UserActionAuthority });
                        return false;
                    }
                }

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.DELETE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }
            #endregion

            //刷新用户缓存
            var resultUserList = CacheDAX.Get(CacheDAX.ConfigDataKey.SystemUser) as List<MDLSM_User>;
            if (resultUserList != null)
            {
                var newUserList = resultUserList;
                //待移除的用户
                List<MDLSM_User> removeUserList = new List<MDLSM_User>();
                foreach (var loopUser in newUserList)
                {
                    var deleteUser = paramUserList.FirstOrDefault(x => x.User_ID == loopUser.User_ID);
                    if (deleteUser != null)
                    {
                        removeUserList.Add(loopUser);
                    }
                }
                foreach (var loopUser in removeUserList)
                {
                    newUserList.Remove(loopUser);
                }
                CacheDAX.Remove(CacheDAX.ConfigDataKey.SystemUser);
                CacheDAX.Add(CacheDAX.ConfigDataKey.SystemUser, newUserList, true);
            }

            return true;
        }

        /// <summary>
        /// 判断员工工号是否存在
        /// </summary>
        /// <param name="paramUserId"></param>
        /// <param name="paramEmpNo"></param>
        /// <returns></returns>
        public bool JudgeEmpNoIsExist(string paramUserId, string paramEmpNo)
        {
            MDLSM_User argsUser = new MDLSM_User();
            if (!string.IsNullOrWhiteSpace(paramUserId))
            {
                argsUser.WHERE_User_ID = paramUserId;
            }
            if (!string.IsNullOrEmpty(paramEmpNo))
            {
                argsUser.WHERE_User_EMPNO = paramEmpNo;
            }
            int resultCount = QueryForObject<int>(SQLID.SM_UserManager_SQL01, argsUser);
            if (resultCount > 0)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
