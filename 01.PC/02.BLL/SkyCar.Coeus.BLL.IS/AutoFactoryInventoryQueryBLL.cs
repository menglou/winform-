using System.Collections.Generic;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.UIModel.IS.QCModel;
using SkyCar.Coeus.UIModel.IS.UIModel;

namespace SkyCar.Coeus.BLL.IS
{
    /// <summary>
    /// 汽修商库存和异动日志查询BLL
    /// </summary>
    public class AutoFactoryInventoryQueryBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.IS);
        #endregion

        #region 构造方法

        /// <summary>
        /// 汽修商库存和异动日志查询BLL
        /// </summary>
        public AutoFactoryInventoryQueryBLL() : base(Trans.IS)
        {

        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 查询汽修商户的库存异动日志
        /// </summary>
        /// <param name="paramAutoFactoryCustomerDbConfig"></param>
        /// <param name="paramInventoryTransLog"></param>
        /// <returns></returns>
        public List<ARInventoryTransLogUIModel> QueryInventoryTransLogList(string paramAutoFactoryCustomerDbConfig, ARInventoryAndTransLogQCModel paramInventoryTransLog)
        {
            if (paramInventoryTransLog == null || string.IsNullOrEmpty(paramAutoFactoryCustomerDbConfig))
            {
                return null;
            }

            List<ARInventoryTransLogUIModel> resultInventoryTransLogList = new List<ARInventoryTransLogUIModel>();
            DBManager.QueryForList<ARInventoryTransLogUIModel>(paramAutoFactoryCustomerDbConfig, SQLID.IS_AutoFactoryInventoryQuery_SQL01, paramInventoryTransLog, resultInventoryTransLogList);
            return resultInventoryTransLogList;
        }

        /// <summary>
        /// 查询汽修商户库存信息
        /// </summary>
        /// <param name="paramAutoFactoryCustomerDbConfig"></param>
        /// <param name="paramInventory"></param>
        /// <param name="paramIsDifferentiateOrg"></param>
        /// <returns></returns>
        public List<ARInventoryUIModel> QueryInventoryList(string paramAutoFactoryCustomerDbConfig,
            ARInventoryAndTransLogQCModel paramInventory, bool paramIsDifferentiateOrg)
        {
            if (paramInventory == null || string.IsNullOrEmpty(paramAutoFactoryCustomerDbConfig))
            {
                return null;
            }
            List<ARInventoryUIModel> resultInventoryList = new List<ARInventoryUIModel>();
            if (paramIsDifferentiateOrg)
            {
                //考虑汽修商户组织
                DBManager.QueryForList<ARInventoryUIModel>(paramAutoFactoryCustomerDbConfig, SQLID.IS_AutoFactoryInventoryQuery_SQL02,
                    paramInventory,
                    resultInventoryList);
            }
            else
            {
                //不考虑汽修商户组织
                DBManager.QueryForList<ARInventoryUIModel>(paramAutoFactoryCustomerDbConfig, SQLID.IS_AutoFactoryInventoryQuery_SQL03,
                    paramInventory,
                    resultInventoryList);
            }
            return resultInventoryList;
        }

        #endregion
    }
}
