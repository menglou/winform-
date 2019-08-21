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
    /// 配件类别BLL
    /// </summary>
    public class AutoPartsTypeManagerBLL : BLLBase
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
        /// 配件类别BLL
        /// </summary>
        public AutoPartsTypeManagerBLL() : base(Trans.BS)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        public bool SaveDetailDS(AutoPartsTypeManagerUIModel paramModel)
        {
            //服务端检查
            if (!ServerCheck(paramModel))
            {
                return false;
            }

            #region 保存数据

            //将UIModel转为TBModel
            var argsAutoPartsType = CopyModel<MDLBS_AutoPartsType>(paramModel);

            //判断主键是否被赋值
            if (string.IsNullOrEmpty(argsAutoPartsType.APT_ID))
            {
                #region 新增
                //生成新ID
                argsAutoPartsType.APT_ID = Guid.NewGuid().ToString();
                argsAutoPartsType.APT_CreatedBy = LoginInfoDAX.UserName;
                argsAutoPartsType.APT_CreatedTime = BLLCom.GetCurStdDatetime();
                argsAutoPartsType.APT_UpdatedBy = LoginInfoDAX.UserName;
                argsAutoPartsType.APT_UpdatedTime = BLLCom.GetCurStdDatetime();
                //主键未被赋值，则执行新增
                if (!_bll.Insert(argsAutoPartsType))
                {
                    //新增[配件类别]信息失败
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.ADD + SystemTableEnums.Name.BS_AutoPartsType });
                    return false;
                }
                #endregion
            }
            else
            {
                #region 更新
                //主键被赋值，则需要更新，更新需要设定更新条件
                argsAutoPartsType.WHERE_APT_ID = argsAutoPartsType.APT_ID;
                argsAutoPartsType.WHERE_APT_VersionNo = argsAutoPartsType.APT_VersionNo;
                argsAutoPartsType.APT_VersionNo++;
                argsAutoPartsType.APT_UpdatedBy = LoginInfoDAX.UserName;
                argsAutoPartsType.APT_UpdatedTime = BLLCom.GetCurStdDatetime();
                if (!_bll.Update(argsAutoPartsType))
                {
                    //更新[配件类别]信息失败！
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.BS_AutoPartsType });
                    return false;
                }
                #endregion
            }

            //将最新数据回写给DetailDS
            CopyModel(argsAutoPartsType, paramModel);

            #endregion

            //刷新配件类别缓存
            var resultAutoPartsTypeList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsType) as List<MDLBS_AutoPartsType>;
            List<MDLBS_AutoPartsType> newAutoPartsTypeList = new List<MDLBS_AutoPartsType>();
            if (resultAutoPartsTypeList != null)
            {
                newAutoPartsTypeList = resultAutoPartsTypeList;
                if (resultAutoPartsTypeList.All(x => x.APT_ID != argsAutoPartsType.APT_ID && x.APT_Name != argsAutoPartsType.APT_Name))
                {
                    newAutoPartsTypeList.Add(argsAutoPartsType);
                    CacheDAX.Add(CacheDAX.ConfigDataKey.AutoPartsType, newAutoPartsTypeList, true);
                }
            }
            else
            {
                newAutoPartsTypeList.Add(argsAutoPartsType);
                CacheDAX.Add(CacheDAX.ConfigDataKey.AutoPartsType, newAutoPartsTypeList, true);
            }
            return true;
        }

        /// <summary>
        /// 删除配件类别
        /// </summary>
        /// <param name="paramAutoPartsTypeList"></param>
        /// <returns></returns>
        public bool DeleteAutoPartsType(List<MDLBS_AutoPartsType> paramAutoPartsTypeList)
        {
            if (paramAutoPartsTypeList == null || paramAutoPartsTypeList.Count == 0)
            {
                //请选择要删除的配件类别
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0006, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.BS_AutoPartsType });
                return false;
            }

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 删除配件类别
                var deleteAutoPartsTypeResult = _bll.DeleteByList<MDLBS_AutoPartsType, MDLBS_AutoPartsType>(paramAutoPartsTypeList);
                if (!deleteAutoPartsTypeResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    //删除[配件类别]信息失败
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.BS_AutoPartsType });
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

            //刷新配件类别缓存
            var resultAutoPartsTypeList = CacheDAX.Get(CacheDAX.ConfigDataKey.AutoPartsType) as List<MDLBS_AutoPartsType>;
            if (resultAutoPartsTypeList != null)
            {
                var newAutoPartsTypeList = resultAutoPartsTypeList;
                //待移除的类别名称
                List<MDLBS_AutoPartsType> removeAutoPartsTypeList = new List<MDLBS_AutoPartsType>();
                foreach (var loopAutoPartsType in newAutoPartsTypeList)
                {
                    var deleteUser = paramAutoPartsTypeList.FirstOrDefault(x => x.APT_ID == loopAutoPartsType.APT_ID);
                    if (deleteUser != null)
                    {
                        removeAutoPartsTypeList.Add(loopAutoPartsType);
                    }
                }
                foreach (var loopAutoPartsType in removeAutoPartsTypeList)
                {
                    newAutoPartsTypeList.Remove(loopAutoPartsType);
                }
                CacheDAX.Remove(CacheDAX.ConfigDataKey.AutoPartsType);
                CacheDAX.Add(CacheDAX.ConfigDataKey.AutoPartsType, newAutoPartsTypeList, true);
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
        private bool ServerCheck(AutoPartsTypeManagerUIModel paramModel)
        {
            return true;
        }

        #endregion
    }
}
