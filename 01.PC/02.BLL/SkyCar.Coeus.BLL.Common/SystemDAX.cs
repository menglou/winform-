using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.TBModel;


namespace SkyCar.Coeus.BLL.Common
{
    public class SystemDAX
    {
        #region 变量
        private static BLLBase baseDAX = new BLLBase(Trans.COM);
        #endregion

        /// <summary>
        /// 获取基本信息（组织信息，个人客户信息，单位客户信息等等）
        /// </summary>
        /// <typeparam name="D">结果Model类型</typeparam>
        /// <param name="paramTBModel">查询条件TBModel，基本信息对应的TBModel(其类型即系统缓存的Key)</param>
        /// <param name="paramIsGetInfoFromDBDirectly">是否直接从数据库中获取信息
        /// <para>paramIsGetInfoFromDBDirectly=true，直接从数据库中取数据。</para>
        /// <para>paramIsGetInfoFromDBDirectly=false，先从缓存取数据，未取到的场合，再从数据库中取数据。</para>
        /// </param>
        /// <returns>结果ModelList</returns>
        public static List<D> GetBaseInfor<D>(object paramTBModel, bool paramIsGetInfoFromDBDirectly)
        {
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), string.Empty, string.Empty, null);
            //结果List
            List<D> resultList = new List<D>();

            //非直接从数据库中获取信息
            if (!paramIsGetInfoFromDBDirectly)
            {
                //根据TBModel类型从缓存获取信息
                object obj = CacheDAX.GetItself(paramTBModel.GetType().FullName);
                if (obj != null)
                {
                    resultList = obj as List<D>;
                    return resultList;
                }
            }

            //检查参数是否为TBModel
            if (paramTBModel != null && !paramTBModel.GetType().FullName.Contains(SysConst.EN_TBMODEL))
            {
                LogHelper.WriteBussLogEndNG(Trans.COM, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), string.Empty, string.Empty, null);
                return resultList;
            }
            //根据TBModel对象，查询信息
            if (paramTBModel != null)
            {
                baseDAX.QueryForList<D>(paramTBModel, resultList);
                //将结果信息添加到缓存，已经存在则覆盖原来的信息
                CacheDAX.Add(paramTBModel.GetType().FullName, resultList, true);
            }

            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), string.Empty, string.Empty, null);
            return resultList;
        }
        /// <summary>
        /// 初始化系统消息
        /// </summary>
        public static void InitializeSystemMessage()
        {
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), string.Empty, string.Empty, null);

            //定义枚举对象列表
            List<MDLSM_Message> msgList = new List<MDLSM_Message>();
            //查询系统所有枚举数据
            baseDAX.QueryForList<MDLSM_Message>(new MDLSM_Message(), msgList);
            foreach (var loopMsg in msgList)
            {
                if (loopMsg == null || string.IsNullOrEmpty(loopMsg.Msg_ID) || string.IsNullOrEmpty(loopMsg.Msg_Content))
                {
                    continue;
                }
                if (loopMsg.Msg_Content.Contains("\\r\\n"))
                {
                    loopMsg.Msg_Content = loopMsg.Msg_Content.Replace("\\r\\n", "\r\n");
                }
            }
            //初始化系统消息
            MsgHelp.InitializeMsg(msgList);

            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), string.Empty, string.Empty, null);
        }

        /// <summary>
        /// 获取汽车排量列表
        /// </summary>
        /// <returns>汽车排量列表</returns>
        public static List<String> GetVehicleCapacity()
        {
            List<string> list = new List<string>();

            for (float i = 0.5f; i < 10f;)
            {
                string value = i.ToString("0.0");
                list.Add(value);
                list.Add(value + "S");
                list.Add(value + "T");
                i += 0.1f;
            }
            return list;
        }
        /// <summary>
        /// 获取汽车年款列表
        /// </summary>
        /// <returns>汽车年款</returns>
        public static List<String> GetVehicleYearMode()
        {
            List<string> list = new List<string>();
            DateTime now = BLLCom.GetCurStdDatetime();
            for (int i = -3; i <= 30; i++)
            {
                list.Add((now.Year - i).ToString());
            }
            return list;
        }
    }
}
