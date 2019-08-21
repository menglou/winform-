using System;
using System.Collections.Generic;
using System.Linq;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.BS;
using SkyCar.Coeus.DAL;
using System.Reflection;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.Common.Enums;

namespace SkyCar.Coeus.BLL.BS
{
    /// <summary>
    /// 配件名称BLL
    /// </summary>
    public class AutoPartsNameManagerBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.BS);
        #endregion

        #region 公共属性

        #endregion

        #region 构造方法

        /// <summary>
        /// 配件名称BLL
        /// </summary>
        public AutoPartsNameManagerBLL() : base(Trans.BS)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        public bool SaveDetailDS(AutoPartsNameManagerUIModel paramModel)
        {
            //服务端检查
            if (!ServerCheck(paramModel))
            {
                return false;
            }

            #region 保存数据

            //将UIModel转为TBModel
            var argsAutoPartsName = CopyModel<MDLBS_AutoPartsName>(paramModel);

            //判断主键是否被赋值
            if (string.IsNullOrEmpty(argsAutoPartsName.APN_ID))
            {
                #region 新增
                //主键未被赋值，则执行新增
                argsAutoPartsName.APN_ID = Guid.NewGuid().ToString().Trim();
                argsAutoPartsName.APN_CreatedBy = LoginInfoDAX.UserName;
                argsAutoPartsName.APN_CreatedTime = BLLCom.GetCurStdDatetime();
                argsAutoPartsName.APN_UpdatedBy = LoginInfoDAX.UserName;
                argsAutoPartsName.APN_UpdatedTime = BLLCom.GetCurStdDatetime();
                if (!_bll.Insert(argsAutoPartsName))
                {
                    //新增[配件名称]信息失败
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.ADD + SystemTableEnums.Name.BS_AutoPartsName });
                    return false;
                }
                #endregion
            }
            else
            {
                #region 更新
                //主键被赋值，则需要更新，更新需要设定更新条件
                argsAutoPartsName.WHERE_APN_ID = argsAutoPartsName.APN_ID;
                argsAutoPartsName.WHERE_APN_VersionNo = argsAutoPartsName.APN_VersionNo;
                argsAutoPartsName.APN_VersionNo++;
                argsAutoPartsName.APN_UpdatedBy = LoginInfoDAX.UserName;
                argsAutoPartsName.APN_UpdatedTime = BLLCom.GetCurStdDatetime();
                if (!_bll.Update(argsAutoPartsName))
                {
                    //更新[配件名称]信息失败
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.BS_AutoPartsName });
                    return false;
                }
                #endregion
            }

            #endregion

            //将最新数据回写给DetailDS
            CopyModel(argsAutoPartsName, paramModel);

            //刷新配件名称缓存
            var resultAutoPartsNameList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsName) as List<MDLBS_AutoPartsName>;
            List<MDLBS_AutoPartsName> newAutoPartsNameList = new List<MDLBS_AutoPartsName>();
            if (resultAutoPartsNameList != null)
            {
                newAutoPartsNameList = resultAutoPartsNameList;
                if (resultAutoPartsNameList.All(x => x.APN_ID != argsAutoPartsName.APN_ID && x.APN_Name != argsAutoPartsName.APN_Name))
                {
                    newAutoPartsNameList.Add(argsAutoPartsName);
                    CacheDAX.Add(CacheDAX.ConfigDataKey.AutoPartsName, newAutoPartsNameList, true);
                }
            }
            else
            {
                newAutoPartsNameList.Add(argsAutoPartsName);
                CacheDAX.Add(CacheDAX.ConfigDataKey.AutoPartsName, newAutoPartsNameList, true);
            }
            return true;
        }

        /// <summary>
        /// 删除配件名称
        /// </summary>
        /// <param name="paramAutoPartsNamelList"></param>
        /// <returns></returns>
        public bool DeleteAutoPartsName(List<MDLBS_AutoPartsName> paramAutoPartsNamelList)
        {
            if (paramAutoPartsNamelList == null || paramAutoPartsNamelList.Count == 0)
            {
                //请选择要删除的配件名称
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0006, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.BS_AutoPartsName });
                return false;
            }

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 删除配件名称
                var deleteAutoPartsNameResult = _bll.DeleteByList<MDLBS_AutoPartsName, MDLBS_AutoPartsName>(paramAutoPartsNamelList);
                if (!deleteAutoPartsNameResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    //删除[配件名称]信息失败
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.BS_AutoPartsName });
                    return false;
                }
                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                //删除[配件名称]信息发生异常
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.DELETE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }

            //刷新配件名称缓存
            var resultAutoPartsNameList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsName) as List<MDLBS_AutoPartsName>;
            if (resultAutoPartsNameList != null)
            {
                var newAutoPartsNameList = resultAutoPartsNameList;
                //待移除的配件名称
                List<MDLBS_AutoPartsName> removeAutoPartsNameList = new List<MDLBS_AutoPartsName>();
                foreach (var loopAutoPartsName in newAutoPartsNameList)
                {
                    var deleteUser = paramAutoPartsNamelList.FirstOrDefault(x => x.APN_ID == loopAutoPartsName.APN_ID);
                    if (deleteUser != null)
                    {
                        removeAutoPartsNameList.Add(loopAutoPartsName);
                    }
                }
                foreach (var loopAutoPartsName in removeAutoPartsNameList)
                {
                    newAutoPartsNameList.Remove(loopAutoPartsName);
                }
                CacheDAX.Remove(CacheDAX.ConfigDataKey.AutoPartsName);
                CacheDAX.Add(CacheDAX.ConfigDataKey.AutoPartsName, newAutoPartsNameList, true);
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
        private bool ServerCheck(AutoPartsNameManagerUIModel paramModel)
        {
            return true;
        }

        #endregion
    }
}
