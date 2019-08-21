using System;
using System.Collections.Generic;
using System.Linq;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.PIS;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using System.Reflection;
using SkyCar.Coeus.Common.Enums;

namespace SkyCar.Coeus.BLL.PIS
{
    /// <summary>
    /// 供应商管理BLL
    /// </summary>
    public class SupplierManagerBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.PIS);
        #endregion

        #region 公共属性

        #endregion

        #region 构造方法

        /// <summary>
        /// 供应商管理BLL
        /// </summary>
        public SupplierManagerBLL() : base(Trans.PIS)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        public bool SaveDetailDS(SupplierManagerUIModel paramModel)
        {
            //服务端检查
            if (!ServerCheck(paramModel))
            {
                return false;
            }

            #region 保存数据

            //将UIModel转为TBModel
            var argsSupplier = CopyModel<MDLPIS_Supplier>(paramModel);
            try
            {
                //判断主键是否被赋值
                if (string.IsNullOrEmpty(argsSupplier.SUPP_ID))
                {
                    #region 新增
                    //生成新ID
                    argsSupplier.SUPP_ID = Guid.NewGuid().ToString();
                    //主键未被赋值，则执行新增
                    bool insertSupplierResult = _bll.Insert(argsSupplier);
                    if (!insertSupplierResult)
                    {
                        //新增[供应商]信息失败！
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.ADD + SystemTableEnums.Name.PIS_Supplier });

                        return false;
                    }
                    #endregion
                }
                else
                {
                    #region 更新
                    //主键被赋值，则需要更新，更新需要设定更新条件
                    argsSupplier.WHERE_SUPP_ID = argsSupplier.SUPP_ID;
                    argsSupplier.WHERE_SUPP_VersionNo = argsSupplier.SUPP_VersionNo;
                    argsSupplier.SUPP_VersionNo++;
                    bool updateSupplierResult = _bll.Update(argsSupplier);
                    if (!updateSupplierResult)
                    {
                        //"更新[供应商]信息失败！"
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.PIS_Supplier });
                        return false;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }
            //将最新数据回写给DetailDS
            CopyModel(argsSupplier, paramModel);

            #endregion

            //刷新供应商缓存
            var resultSupplierList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsSupplier) as List<MDLPIS_Supplier>;
            List<MDLPIS_Supplier> newSupplierList = new List<MDLPIS_Supplier>();
            if (resultSupplierList != null)
            {
                newSupplierList = resultSupplierList;
                if (resultSupplierList.All(x => x.SUPP_ID != argsSupplier.SUPP_ID && x.SUPP_Name != argsSupplier.SUPP_Name))
                {
                    newSupplierList.Add(argsSupplier);
                    CacheDAX.Add(CacheDAX.ConfigDataKey.AutoPartsSupplier, newSupplierList, true);
                }
            }
            else
            {
                newSupplierList.Add(argsSupplier);
                CacheDAX.Add(CacheDAX.ConfigDataKey.AutoPartsSupplier, newSupplierList, true);
            }
            return true;
        }

        /// <summary>
        /// 删除供应商
        /// </summary>
        /// <param name="paramSupplierList"></param>
        /// <returns></returns>
        public bool DeleteSupplier(List<MDLPIS_Supplier> paramSupplierList)
        {
            if (paramSupplierList == null || paramSupplierList.Count == 0)
            {
                //请选择要删除供应商
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0006, new object[] { SystemTableEnums.Name.PIS_Supplier, SystemActionEnum.Name.DELETE });
                return false;
            }

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 删除供应商
                var deleteSupplierResult = _bll.DeleteByList<MDLPIS_Supplier, MDLPIS_Supplier>(paramSupplierList);
                if (!deleteSupplierResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    //删除[供应商]信息失败
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.PIS_Supplier });
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

            //刷新供应商缓存
            var resultSupplierList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsSupplier) as List<MDLPIS_Supplier>;
            if (resultSupplierList != null)
            {
                var newSupplierList = resultSupplierList;
                //待移除的供应商
                List<MDLPIS_Supplier> removeSupplierList = new List<MDLPIS_Supplier>();
                foreach (var loopSupplier in newSupplierList)
                {
                    var deleteSupplier = paramSupplierList.FirstOrDefault(x => x.SUPP_ID == loopSupplier.SUPP_ID);
                    if (deleteSupplier != null)
                    {
                        removeSupplierList.Add(loopSupplier);
                    }
                }
                foreach (var loopSupplier in removeSupplierList)
                {
                    newSupplierList.Remove(loopSupplier);
                }
                CacheDAX.Remove(CacheDAX.ConfigDataKey.AutoPartsSupplier);
                CacheDAX.Add(CacheDAX.ConfigDataKey.AutoPartsSupplier, newSupplierList, true);
            }

            return true;
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        private bool ServerCheck(SupplierManagerUIModel paramModel)
        {
            return true;
        }

        #endregion
    }
}
