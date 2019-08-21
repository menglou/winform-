using System;
using System.Collections.Generic;
using System.Reflection;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.PIS;

namespace SkyCar.Coeus.BLL.PIS
{
    /// <summary>
    /// 普通客户管理BLL
    /// </summary>
    public class GeneralCustomerManagerBLL : BLLBase
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
        /// 普通客户管理BLL
        /// </summary>
        public GeneralCustomerManagerBLL() : base(Trans.PIS)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        public bool SaveDetailDS(GeneralCustomerManagerUIModel paramModel)
        {
            //服务端检查
            if (!ServerCheck(paramModel))
            {
                return false;
            }

            #region 保存数据

            //将UIModel转为TBModel
            var argsGeneralCustomer = CopyModel<MDLPIS_GeneralCustomer>(paramModel);

            try
            {

                #region 新增
                //判断主键是否被赋值
                if (string.IsNullOrEmpty(argsGeneralCustomer.GC_ID))
                {
                    //生成新ID
                    argsGeneralCustomer.GC_ID = Guid.NewGuid().ToString();
                    argsGeneralCustomer.GC_CreatedBy = LoginInfoDAX.UserName;
                    argsGeneralCustomer.GC_CreatedTime = BLLCom.GetCurStdDatetime();
                    argsGeneralCustomer.GC_UpdatedBy = LoginInfoDAX.UserName;
                    argsGeneralCustomer.GC_UpdatedTime = BLLCom.GetCurStdDatetime();
                    //主键未被赋值，则执行新增
                    if (!_bll.Insert(argsGeneralCustomer))
                    {
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.PIS_GeneralCustomer });
                        return false;
                    }
                }
                #endregion

                #region 更新
                else
                {
                    //主键被赋值，则需要更新，更新需要设定更新条件
                    argsGeneralCustomer.WHERE_GC_ID = argsGeneralCustomer.GC_ID;
                    argsGeneralCustomer.WHERE_GC_VersionNo = argsGeneralCustomer.GC_VersionNo;
                    argsGeneralCustomer.GC_VersionNo++;
                    argsGeneralCustomer.GC_UpdatedBy = LoginInfoDAX.UserName;
                    argsGeneralCustomer.GC_UpdatedTime = BLLCom.GetCurStdDatetime();
                    if (!_bll.Update(argsGeneralCustomer))
                    {
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.PIS_GeneralCustomer });
                        return false;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }

            //将最新数据回写给DetailDS
            CopyModel(argsGeneralCustomer, paramModel);

            #endregion

            return true;
        }

        /// <summary>
        /// 删除普通客户
        /// </summary>
        /// <param name="paramGeneralCustomerList">待删除普通客户列表</param>
        /// <returns></returns>
        public bool DeleteGeneralCustomer(List<MDLPIS_GeneralCustomer> paramGeneralCustomerList)
        {
            if (paramGeneralCustomerList == null || paramGeneralCustomerList.Count == 0)
            {
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0006, new object[] { SystemTableEnums.Name.PIS_GeneralCustomer, SystemActionEnum.Name.DELETE });
                return false;
            }

            #region 带事务的删除

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                var deleteGeneralCustomerResult = _bll.DeleteByList<MDLPIS_GeneralCustomer, MDLPIS_GeneralCustomer>(paramGeneralCustomerList);
                if (!deleteGeneralCustomerResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE, SystemTableEnums.Name.PIS_GeneralCustomer });
                    return false;
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

            return true;
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        private bool ServerCheck(GeneralCustomerManagerUIModel paramModel)
        {
            //验证普通客户编码唯一性
            var sameGeneralCustomer = _bll.QueryForObject<Int32>(SQLID.PIS_GeneralCustomerManager_SQL01, new MDLPIS_GeneralCustomer
            {
                WHERE_GC_ID = paramModel.GC_ID,
                WHERE_GC_Name = paramModel.GC_Name,
                WHERE_GC_PhoneNo = paramModel.GC_PhoneNo
            });
            if (sameGeneralCustomer > 0)
            {
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0006, new object[] { MsgParam.SAME + MsgParam.OF + SystemTableColumnEnums.PIS_GeneralCustomer.Name.GC_Name + MsgParam.WITH + SystemTableColumnEnums.PIS_GeneralCustomer.Name.GC_PhoneNo });
                return false;
            }

            return true;
        }

        #endregion
    }
}
