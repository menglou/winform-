using System;
using System.Reflection;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.SM.UIModel;

namespace SkyCar.Coeus.BLL.SM
{
    /// <summary>
    /// 用户菜单管理（组织）BLL
    /// </summary>
    public class UserMenuManagerBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.SM);
        #endregion

        #region 公共属性

        #endregion

        #region 构造方法

        /// <summary>
        /// 用户菜单管理（组织）BLL
        /// </summary>
        public UserMenuManagerBLL() : base(Trans.SM)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存用户菜单和动作授权信息
        /// </summary>
        /// <param name="paramUserId">用户ID</param>
        /// <param name="paramUserMenuAuthoritiyList">用户菜单授权列表</param>
        /// <param name="paramUserActionAuthoritiyList">用户菜单动作授权列表</param>
        /// <param name="paramUserJobAuthoritiyList">用户作业授权列表</param>
        /// <returns></returns>
        public bool SaveUserMenu(string paramUserId, SkyCarBindingList<UserMenuAuthorityUIModel, MDLSM_UserMenuAuthority> paramUserMenuAuthoritiyList, SkyCarBindingList<UserActionAuthorityUIModel, MDLSM_UserActionAuthority> paramUserActionAuthoritiyList, SkyCarBindingList<UserJobAuthorityUIModel, MDLSM_UserJobAuthority> paramUserJobAuthoritiyList)
        {
            if (string.IsNullOrEmpty(paramUserId))
            {
                //没有获取到用户ID，保存失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableColumnEnums.SM_User.Name.User_ID, SystemActionEnum.Name.SAVE });
                return false;
            }

            #region 准备数据

            #region 用户菜单授权

            if (paramUserMenuAuthoritiyList != null)
            {
                //新增用户菜单授权
                if (paramUserMenuAuthoritiyList.InsertList != null &&
                paramUserMenuAuthoritiyList.InsertList.Count > 0)
                {
                    foreach (var loopUserMenuAuthoritiy in paramUserMenuAuthoritiyList.InsertList)
                    {
                        loopUserMenuAuthoritiy.UMA_User_ID = paramUserId;
                        loopUserMenuAuthoritiy.UMA_CreatedBy = LoginInfoDAX.UserName;
                        loopUserMenuAuthoritiy.UMA_CreatedTime = BLLCom.GetCurStdDatetime();
                        loopUserMenuAuthoritiy.UMA_UpdatedBy = LoginInfoDAX.UserName;
                        loopUserMenuAuthoritiy.UMA_UpdatedTime = BLLCom.GetCurStdDatetime();
                    }
                }

                //删除用户菜单授权
                if (paramUserMenuAuthoritiyList.DeleteList != null &&
                    paramUserMenuAuthoritiyList.DeleteList.Count > 0)
                {
                    foreach (var loopUserMenuAuthoritiy in paramUserMenuAuthoritiyList.DeleteList)
                    {
                        loopUserMenuAuthoritiy.WHERE_UMA_ID = loopUserMenuAuthoritiy.UMA_ID;
                    }
                }
            }
            #endregion

            #region 用户菜单动作授权

            if (paramUserActionAuthoritiyList != null)
            {
                //新增用户菜单动作授权
                if (paramUserActionAuthoritiyList.InsertList != null &&
                    paramUserActionAuthoritiyList.InsertList.Count > 0)
                {
                    foreach (var loopUserActionAuthoritiy in paramUserActionAuthoritiyList.InsertList)
                    {
                        loopUserActionAuthoritiy.UAA_User_ID = paramUserId;
                        loopUserActionAuthoritiy.UAA_CreatedBy = LoginInfoDAX.UserName;
                        loopUserActionAuthoritiy.UAA_CreatedTime = BLLCom.GetCurStdDatetime();
                        loopUserActionAuthoritiy.UAA_UpdatedBy = LoginInfoDAX.UserName;
                        loopUserActionAuthoritiy.UAA_UpdatedTime = BLLCom.GetCurStdDatetime();
                    }
                }
                //删除用户菜单动作授权
                if (paramUserActionAuthoritiyList.DeleteList != null &&
                    paramUserActionAuthoritiyList.DeleteList.Count > 0)
                {
                    foreach (var loopUserActionAuthoritiy in paramUserActionAuthoritiyList.DeleteList)
                    {
                        loopUserActionAuthoritiy.WHERE_UAA_ID = loopUserActionAuthoritiy.UAA_ID;
                    }
                }
            }

            #endregion

            #region 用户作业授权

            //新增用户菜单动作授权
            if (paramUserJobAuthoritiyList.InsertList != null)
            {
                foreach (var loopUserJobAuthoritiy in paramUserJobAuthoritiyList.InsertList)
                {
                    loopUserJobAuthoritiy.UJA_User_ID = paramUserId;
                }
            }
            //删除用户菜单动作授权
            if (paramUserJobAuthoritiyList.DeleteList != null)
            {
                foreach (var loopUserJobAuthoritiy in paramUserJobAuthoritiyList.DeleteList)
                {
                    loopUserJobAuthoritiy.WHERE_UJA_ID = loopUserJobAuthoritiy.UJA_ID;
                }
            }
            #endregion

            #endregion

            #region 带事务的保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                if (paramUserMenuAuthoritiyList != null)
                {
                    #region 新增[用户菜单授权]

                    if (paramUserMenuAuthoritiyList.InsertList != null &&
                    paramUserMenuAuthoritiyList.InsertList.Count > 0)
                    {
                        bool saveUserMenuAuthority = _bll.InsertByList<UserMenuAuthorityUIModel, MDLSM_UserMenuAuthority>(
                            paramUserMenuAuthoritiyList.InsertList);
                        if (!saveUserMenuAuthority)
                        {
                            DBManager.RollBackTransaction(DBCONFIG.Coeus);
                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.SM_UserMenuAuthority });
                            return false;
                        }
                    }
                    #endregion 

                    #region 删除[用户菜单授权]
                    if (paramUserMenuAuthoritiyList.DeleteList != null &&
                        paramUserMenuAuthoritiyList.DeleteList.Count > 0)
                    {
                        bool saveUserMenuAuthority = _bll.DeleteByList<UserMenuAuthorityUIModel, MDLSM_UserMenuAuthority>(
                            paramUserMenuAuthoritiyList.DeleteList);
                        if (!saveUserMenuAuthority)
                        {
                            DBManager.RollBackTransaction(DBCONFIG.Coeus);
                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.SM_UserMenuAuthority });
                            return false;
                        }
                    }
                    #endregion 
                }
                if (paramUserActionAuthoritiyList != null)
                {
                    #region 新增[用户菜单动作授权]
                    if (paramUserActionAuthoritiyList.InsertList != null &&
                    paramUserActionAuthoritiyList.InsertList.Count > 0)
                    {
                        bool saveUserActionAuthority = _bll.InsertByList<UserActionAuthorityUIModel, MDLSM_UserActionAuthority>(paramUserActionAuthoritiyList.InsertList);
                        if (!saveUserActionAuthority)
                        {
                            DBManager.RollBackTransaction(DBCONFIG.Coeus);
                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.SM_UserActionAuthority });
                            return false;
                        }
                    }
                    #endregion

                    #region 删除[用户菜单动作授权]
                    if (paramUserActionAuthoritiyList.DeleteList != null &&
                        paramUserActionAuthoritiyList.DeleteList.Count > 0)
                    {
                        bool saveUserActionAuthority = _bll.DeleteByList<UserActionAuthorityUIModel, MDLSM_UserActionAuthority>(paramUserActionAuthoritiyList.DeleteList);
                        if (!saveUserActionAuthority)
                        {
                            DBManager.RollBackTransaction(DBCONFIG.Coeus);
                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.SM_UserActionAuthority });
                            return false;
                        }
                    }
                    #endregion

                    #region 保存[用户作业授权]

                    bool saveUserJobAuthorityResult = _bll.UnitySave(paramUserJobAuthoritiyList);
                    if (!saveUserJobAuthorityResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SM_UserJobAuthority });
                        return false;
                    }

                    #endregion
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

            #endregion

            return true;
        }

        #endregion

        #region 私有方法

        #endregion
    }
}
