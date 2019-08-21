using System;
using System.Reflection;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;

namespace SkyCar.Coeus.BLL.SD
{
    /// <summary>
    /// 销售补货建议查询BLL
    /// </summary>
    public class SalesForecastOrderManageBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.SD);
        #endregion

        #region 公共属性

        #endregion

        #region 构造方法

        /// <summary>
        /// 销售补货建议查询BLL
        /// </summary>
        public SalesForecastOrderManageBLL() : base(Trans.SD)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 删除销售预测订单信息
        /// </summary>
        /// <param name="paramSalesForecastOrderId"></param>
        /// <returns></returns>
        public bool DeleteSalesForecastOrder(string paramSalesForecastOrderId)
        {
            if (string.IsNullOrEmpty(paramSalesForecastOrderId))
            {
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.SD_SalesForecastOrder, SystemActionEnum.Name.DELETE });
                return false;
            }
            try
            {

                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 预测销售订单明细
                MDLSD_SalesForecastOrderDetail argSalesForecastOrderDetail = new MDLSD_SalesForecastOrderDetail();
                argSalesForecastOrderDetail.SFOD_ST_ID = paramSalesForecastOrderId;
                int deleteSalesForecastOrderDetail = DBManager.Delete(DBCONFIG.Coeus, SQLID.SD_SalesForecastOrder_SQL01, argSalesForecastOrderDetail);
                if (deleteSalesForecastOrderDetail == 0)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.SD_SalesForecastOrderDetail });
                    return false;
                }
                #endregion

                #region 预测销售订单
                MDLSD_SalesForecastOrder argSalesForecastOrder = new MDLSD_SalesForecastOrder();
                argSalesForecastOrder.SFO_ID = paramSalesForecastOrderId;
                int deleteSalesForecastOrder = DBManager.Delete(DBCONFIG.Coeus, SQLID.SD_SalesForecastOrder_SQL02, argSalesForecastOrder);
                if (deleteSalesForecastOrder == 0)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.SD_SalesForecastOrder });
                    return false;
                }
                #endregion

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
            return true;
        }
        #endregion

        #region 私有方法

        #endregion
    }
}
