using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.PIS;

namespace SkyCar.Coeus.BLL.PIS
{
    /// <summary>
    /// 采购补货建议查询BLL
    /// </summary>
    public class PurchaseForecastOrderQueryBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll =new BLLBase(Trans.PIS);
        #endregion

        #region 公共属性

        #endregion

        #region 构造方法

        /// <summary>
        /// 采购补货建议查询BLL
        /// </summary>
        public PurchaseForecastOrderQueryBLL() : base(Trans.PIS)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramHead">单头UIModel</param>
        /// <param name="paramDetailList">明细UIModel列表</param>
        /// <returns></returns>
        public bool SaveDetailDS(PurchaseForecastOrderQueryUIModel paramHead,SkyCarBindingList<PurchaseForecastOrderQueryDetailUIModel,MDLPIS_PurchaseForecastOrderDetail> paramDetailList )
        {
            var funcName = "SaveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);
            //服务端检查
            if (!ServerCheck(paramHead,paramDetailList))
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }

            #region 事务，多数据表操作

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存单头

                //将UIModel转为TBModel
                var argsHead = paramHead.ToTBModelForSaveAndDelete<MDLPIS_PurchaseForecastOrder>();
                //判断主键是否被赋值
                if (string.IsNullOrEmpty(paramHead.PFO_ID))
                {
                    argsHead.PFO_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.PFO);
                }
                //执行保存
                if (!_bll.Save(argsHead))
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "保存[XX]信息失败！" });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存明细

                //执行保存
                if (!_bll.UnitySave(paramDetailList))
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "保存[XX]信息失败！" });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                //将最新数据回写给DetailDS
                CopyModel(argsHead, paramHead);

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "保存失败，失败原因：" + ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ex.Message, "", null);
                return false;
            }

            #endregion

            return true;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        private bool ServerCheck(PurchaseForecastOrderQueryUIModel paramModel, SkyCarBindingList<PurchaseForecastOrderQueryDetailUIModel, MDLPIS_PurchaseForecastOrderDetail> paramDetailList)
        {
            return true;
        }

        #endregion
    }
}
