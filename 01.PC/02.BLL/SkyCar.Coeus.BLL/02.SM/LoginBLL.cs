using SkyCar.Coeus.BLL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel;

namespace SkyCar.Coeus.BLL
{
    public class LoginBLL : BLLBase
    {
        public LoginBLL()
            : base(Trans.SM)
        {

        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="paramModel"></param>
        /// <returns></returns>
        public List<MDLSM_User> GetUserList(LoginUIModel paramModel)
        {
            List<MDLSM_User> resultList = new List<MDLSM_User>();
            QuerryForList<MDLSM_User>(SQLID.Login_SQL01, paramModel, resultList);
            return resultList;
        }

        /// <summary>
        /// 获取用户所在的组织列表
        /// </summary>
        /// <param name="paramModel"></param>
        /// <returns></returns>
        public IList<MDLSM_Organization> GetOrgListByUser(LoginUIModel paramModel)
        {
            List<MDLSM_Organization> resultList = new List<MDLSM_Organization>();
            QuerryForList<MDLSM_Organization>(SQLID.Login_SQL02, paramModel, resultList);
            return resultList;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="paramModel"></param>
        /// <returns></returns>
        public SystemConfigInfoUIModel GetSystemConfigInfo(LoginUIModel paramModel)
        {
            return QuerryForObject<SystemConfigInfoUIModel>(SQLID.Login_SQL03, paramModel);
        }

        /// <summary>
        /// 获取有效的许可证
        /// </summary>
        /// <param name="paramModel"></param>
        /// <returns></returns>
        public List<MDLSM_ClientUseLicense> GetClientUseLicenses(MDLSM_ClientUseLicense paramModel)
        {
            List<MDLSM_ClientUseLicense> resultList = new List<MDLSM_ClientUseLicense>();
            QuerryForList<MDLSM_ClientUseLicense>(SQLID.Login_SQL04, paramModel, resultList);
            return resultList;
        }
    }
}
