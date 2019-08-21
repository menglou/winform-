using System;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.BS;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.Common.Const;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SkyCar.Common.Utility;
using System.Configuration;
using System.Collections.Generic;
using System.Reflection;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;

namespace SkyCar.Coeus.BLL.BS
{
    /// <summary>
    /// 码表管理BLL
    /// </summary>
    public class CodeTableManagerBLL : BLLBase
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
        /// 码表管理BLL
        /// </summary>
        public CodeTableManagerBLL() : base(Trans.BS)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        public bool SaveDetailDS(CodeTableManagerUIModel paramModel)
        {
            //服务端检查
            if (!ServerCheck(paramModel))
            {
                return false;
            }

            #region 保存码表

            //将UIModel转为TBModel
            var argsCodeTable = CopyModel<MDLSM_CodeTable>(paramModel);
            argsCodeTable.CT_Value = argsCodeTable.CT_Name;
            argsCodeTable.CT_Desc = argsCodeTable.CT_Name;

            #region 新增
            //判断主键是否被赋值
            if (string.IsNullOrEmpty(argsCodeTable.CT_ID))
            {
                #region 同步配件级别
                if (argsCodeTable.CT_Type == CodeTypeEnum.Code.AUTOPARTSLEVEL)
                {
                    if (!string.IsNullOrEmpty(argsCodeTable.CT_Name))
                    {
                        string argsPostData = string.Format(ApiParameter.BF0014,
                        ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE], argsCodeTable.CT_Name);
                        string strApiData = APIDataHelper.GetAPIData(ApiUrl.BF0014Url, argsPostData);
                        var jsonResult = (JObject)JsonConvert.DeserializeObject(strApiData);

                        if (jsonResult == null || (jsonResult[SysConst.EN_RESULTCODE] != null && !jsonResult[SysConst.EN_RESULTCODE].ToString().Equals(SysConst.EN_I0001)))
                        {
                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SYNCHRONIZE + SystemTableColumnEnums.BS_AutoPartsArchive.Name.APA_Level });
                        }
                    }
                }
                #endregion

                //生成新ID
                argsCodeTable.CT_ID = Guid.NewGuid().ToString();
                argsCodeTable.CT_CreatedBy = LoginInfoDAX.UserName;
                argsCodeTable.CT_CreatedTime = BLLCom.GetCurStdDatetime();
                argsCodeTable.CT_UpdatedBy = LoginInfoDAX.UserName;
                argsCodeTable.CT_UpdatedTime = BLLCom.GetCurStdDatetime();
                //主键未被赋值，则执行新增
                if (!_bll.Insert(argsCodeTable))
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.SM_CodeTable });
                    return false;
                }
            }
            #endregion

            #region 更新
            else
            {
                //主键被赋值，则需要更新，更新需要设定更新条件
                argsCodeTable.WHERE_CT_ID = argsCodeTable.CT_ID;
                argsCodeTable.WHERE_CT_VersionNo = argsCodeTable.CT_VersionNo;
                argsCodeTable.CT_VersionNo++;
                argsCodeTable.CT_UpdatedBy = LoginInfoDAX.UserName;
                argsCodeTable.CT_UpdatedTime = BLLCom.GetCurStdDatetime();
                if (!_bll.Update(argsCodeTable))
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.SM_CodeTable });
                    return false;
                }
            }
            #endregion

            //将最新数据回写给DetailDS
            CopyModel(argsCodeTable, paramModel);

            #endregion

            return true;
        }

        /// <summary>
        /// 删除码表
        /// </summary>
        /// <param name="paramCodeTableList">待删除的码表List</param>
        /// <returns></returns>
        public bool DeleteCodeTable(List<MDLSM_CodeTable> paramCodeTableList)
        {
            try
            {

                DBManager.BeginTransaction(DBCONFIG.Coeus);

                var deleteCodeTableResult = _bll.DeleteByList<MDLSM_CodeTable, MDLSM_CodeTable>(paramCodeTableList);
                if (!deleteCodeTableResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE, SystemTableEnums.Name.SM_CodeTable });
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
            return true;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        private bool ServerCheck(CodeTableManagerUIModel paramModel)
        {
            //新增时检查参数类型下是否存在相同的参数值
            if (string.IsNullOrEmpty(paramModel.CT_ID))
            {
                var resultSameCodeTable = QueryForObject<Int32>(SQLID.BS_CodeTableManager_SQL02, new MDLSM_CodeTable
                {
                    WHERE_CT_ID = paramModel.CT_ID,
                    WHERE_CT_Type = paramModel.CT_Type,
                    WHERE_CT_Name = paramModel.CT_Name
                });
                if (resultSameCodeTable > 0)
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0006, new object[] { SystemTableColumnEnums.SM_CodeTable.Name.CT_Name });
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}
