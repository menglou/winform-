using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.ComModel;
using System.Collections;
using System.ComponentModel;
using IBatisNet.DataMapper.MappedStatements;


namespace SkyCar.Coeus.DAL
{
    /// <summary>
    /// 数据记录增删改查类，根据给定的Model实例/[ID]/[条件]，自动执行插入、更新和删除操作。
    /// </summary>
    public class CRUD
    {
        #region 变量定义
        /// <summary>
        /// 消息
        /// </summary>
        public string ReturnMsg { get; set; }
        /// <summary>
        /// 数据库配置（DAL.DBCONFIG）
        /// </summary>
        private string dbConfig { get; set; }
        #endregion

        #region 构造方法
        /// <summary>
        /// 数据记录增删改查类，根据给定的Model实例/[ID]/[条件]，自动执行插入、更新和删除操作。
        /// </summary>
        /// <param name="dbConfig">数据库配置（DAL.DBCONFIG）</param>
        public CRUD(string dbConfig)
        {
            this.dbConfig = dbConfig;
        }
        #endregion

        #region 查询

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">查询结果Model类型</typeparam>
        /// <param name="paramConditionTBModel">查询条件TBModel</param>
        /// <param name="paramObject">查询结果</param>
        public object QueryForObject<T>(object paramConditionTBModel)
        {
            if (!paramConditionTBModel.GetType().FullName.Contains(DALConst.TBModel))
            {
                ReturnMsg = "the ConditionTBModel is not TBModel.";
                return null;
            }
            string argsStatementName = "COMM_SQL_" + paramConditionTBModel.GetType().Name;
            return DBManager.QueryForObject<T>(DBCONFIG.Coeus, argsStatementName, paramConditionTBModel);

        }
        /// <summary>
        /// 查询记录条数
        /// </summary>
        /// <param name="paramConditionTBModel">查询条件TBModel</param>
        /// <returns>正常：返回记录条数；异常：-1</returns>
        public int QueryForCount(object paramConditionTBModel)
        {
            if (!paramConditionTBModel.GetType().FullName.Contains(DALConst.TBModel))
            {
                ReturnMsg = "the ConditionTBModel is not TBModel.";
                return -1;
            }
            string argsStatementName = "COMM_SQL_COUNT_" + paramConditionTBModel.GetType().Name;
            var result = DBManager.QueryForObject(DBCONFIG.Coeus, argsStatementName, paramConditionTBModel);
            if (result == null)
            {
                return -1;
            }
            else
            {
                return (int)result;
            }
        }
        /// <summary>
        /// 查询（模糊查询）
        /// </summary>
        /// <typeparam name="T">查询结果Model类型</typeparam>
        /// <param name="paramConditionTBModel">查询条件TBModel</param>
        /// <param name="paramObject">查询结果</param>
        public object QueryForObjectLike<T>(object paramConditionTBModel)
        {
            if (!paramConditionTBModel.GetType().FullName.Contains(DALConst.TBModel))
            {
                ReturnMsg = "the ConditionTBModel is not TBModel.";
                return null;
            }
            string argsStatementName = "COMM_SQL_LIKE_" + paramConditionTBModel.GetType().Name;
            return DBManager.QueryForObject<T>(DBCONFIG.Coeus, argsStatementName, paramConditionTBModel);

        }
        ///// <summary>
        ///// 查询TODO
        ///// </summary>
        ///// <typeparam name="T">查询结果Model类型</typeparam>
        ///// <param name="paramConditionTBModel">查询条件TBModel</param>
        ///// <param name="paramObject">查询结果</param>
        //public void QueryForObject<S>(object paramConditionTBModel,object paramResultModel)
        //{
        //    if (!paramConditionTBModel.GetType().FullName.Contains(DALConst.TBModel))
        //    {
        //        ReturnMsg = "the ConditionTBModel is not TBModel.";
        //        return;
        //    }
        //    string argsStatementName = "COMM_SQL_" + paramConditionTBModel.GetType().Name;
        //    paramResultModel = DBManager.QueryForObject<S>(DBCONFIG.Coeus, argsStatementName, paramConditionTBModel);

        //}
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">查询结果Model类型</typeparam>
        /// <param name="paramConditionTBModel">查询条件TBModel</param>
        /// <param name="paramObjectList">查询结果</param>
        public void QueryForList<T>(object paramConditionTBModel, IList<T> paramObjectList)
        {
            if (!paramConditionTBModel.GetType().FullName.Contains(DALConst.TBModel))
            {
                ReturnMsg = "the ConditionTBModel is not TBModel.";
                return;
            }
            string argsStatementName = "COMM_SQL_" + paramConditionTBModel.GetType().Name;
            paramObjectList.Clear();
            DBManager.QueryForList<T>(DBCONFIG.Coeus, argsStatementName, paramConditionTBModel, paramObjectList);

        }
        /// <summary>
        /// 查询（模糊）
        /// </summary>
        /// <typeparam name="T">查询结果Model类型</typeparam>
        /// <param name="paramConditionTBModel">查询条件TBModel</param>
        /// <param name="paramObjectList">查询结果</param>
        public void QueryForListLike<T>(object paramConditionTBModel, IList<T> paramObjectList)
        {
            if (!paramConditionTBModel.GetType().FullName.Contains(DALConst.TBModel))
            {
                ReturnMsg = "the ConditionTBModel is not TBModel.";
                return;
            }
            string argsStatementName = "COMM_SQL_LIKE_" + paramConditionTBModel.GetType().Name;
            paramObjectList.Clear();
            DBManager.QueryForList<T>(DBCONFIG.Coeus, argsStatementName, paramConditionTBModel, paramObjectList);

        }

        #endregion

        #region 插入


        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="paramTBModel">TBModel对象</param>
        /// <param name="paramObjResult">返回新增的ID</param>
        public void InsertByTBModel(object paramTBModel, out object paramObjResult)
        {
            if (!paramTBModel.GetType().FullName.Contains(DALConst.TBModel))
            {
                ReturnMsg = "the paramTBModel is not TBModel.";
                paramObjResult = null;
                return;
            }

            CreateSQL createSQL = new CreateSQL();

            ComCRUDModel param = new ComCRUDModel();

            //SQL语句
            param.StrSQL = createSQL.CreateSQLForInsertOutIdentity(paramTBModel);
            //执行
            paramObjResult = DBManager.Insert(dbConfig, SQLID.COMM_SQL01, param);
        }
        ///// <summary>
        ///// 插入
        ///// </summary>
        ///// <param name="paramTBModel">Model实例</param>
        ///// <returns>是否插入成功（true：成功 false：失败）</returns>
        //public bool Insert(object paramTBModel)
        //{
        //    if (!paramTBModel.GetType().FullName.Contains(DALConst.TBModel))
        //    {
        //        ReturnMsg = "the paramTBModel is not TBModel.";
        //        return false;
        //    }
        //    //返回值
        //    bool result = true;

        //    CreateSQL createSQL = new CreateSQL();
        //    ComCRUDModel param = new ComCRUDModel();

        //    //SQL语句
        //    param.StrSQL = createSQL.CreateSQLForInsert(paramTBModel);
        //    //执行查询操作
        //    object objResult = DBManager.Insert(dbConfig, SQLID.COMM_SQL01, param);

        //    if (objResult != null)
        //    {
        //        result = false;
        //    }

        //    return result;
        //}

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramConditionTBModelList"></param>
        /// <returns></returns>
        public bool InsertByList<T>(IList<T> paramConditionTBModelList)
        {
            if (!paramConditionTBModelList.GetType().FullName.Contains(DALConst.TBModel))
            {
                ReturnMsg = "the ConditionTBModel is not TBModel.";
                return false;
            }
            string argsStatementName = "COMM_SQL_INSERT_" + paramConditionTBModelList[0].GetType().Name;

            var propertyCount = paramConditionTBModelList[0].GetType().GetProperties().Length;
            //ibatis限制Model属性总数不能超过2100
            var num = 2100 / propertyCount;

            List<T> modelList = new List<T>();

            for (int i = 0; i < paramConditionTBModelList.Count; i++)
            {
                if (i != 0 && i % num == 0)
                {
                    object obj = DBManager.Insert(DBCONFIG.Coeus, argsStatementName, modelList);
                    if (obj != null)
                    {
                        return false;
                    }
                    modelList.Clear();

                    //将临界值对象插入清空后的List
                    modelList.Add(paramConditionTBModelList[i]);
                }
                else
                {
                    modelList.Add(paramConditionTBModelList[i]);
                }
            }
            if (modelList.Count > 0)
            {
                object obj = DBManager.Insert(DBCONFIG.Coeus, argsStatementName, modelList);
                if (obj != null)
                {
                    return false;
                }
                modelList.Clear();
            }

            return true;
        }

        #endregion 插入End

        #region 更新
        /// <summary>
        /// 更新，根据TBModel内WHERE属性
        /// </summary>
        /// <param name="paramTBModel">model实例</param>
        /// <returns>是否更新成功（true：成功 false：失败）</returns>
        public bool Update(object paramTBModel)
        {
            if (!paramTBModel.GetType().FullName.Contains(DALConst.TBModel))
            {
                ReturnMsg = "the paramTBModel is not TBModel.";
                return false;
            }
            //返回值
            bool result = true;
            string argsStatementName = "COMM_SQL_Update" + paramTBModel.GetType().Name;
            //执行查询操作
            object objResult = DBManager.Update(dbConfig, argsStatementName, paramTBModel);
            if (objResult != null && (int)objResult < 1)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 更新,通过给定的ID
        /// </summary>
        /// <param name="paramTBModel">model实例</param>
        /// <param name="paramId">id（数据表主键）</param>
        /// <returns>是否更新成功（true：成功 false：失败）</returns>
        public bool UpdateByID(object paramTBModel, object paramId)
        {
            if (!paramTBModel.GetType().FullName.Contains(DALConst.TBModel))
            {
                ReturnMsg = "the paramTBModel is not TBModel.";
                return false;
            }
            //返回值
            bool result = true;
            CreateSQL createSQL = new CreateSQL();
            ComCRUDModel param = new ComCRUDModel();
            //Int64型ID
            if (paramId is Int64)
            {
                //SQL语句
                param.StrSQL = createSQL.CreateSQLForUpdateByInt64ID(paramTBModel, (Int64)paramId);
            }
            //String型ID
            else if (paramId is string)
            {
                //SQL语句
                param.StrSQL = createSQL.CreateSQLForUpdateByStrID(paramTBModel, paramId.ToString());
            }
            else
            {
                return false;
            }
            //执行查询操作
            object objResult = DBManager.Update(dbConfig, SQLID.COMM_SQL01, param);

            if (objResult != null && (int)objResult < 1)
            {
                result = false;
            }

            return result;
        }
        /// <summary>
        /// 更新,通过给定的WHERE条件
        /// </summary>
        /// <param name="paramTBModel">TBModel实例</param>
        /// <param name="paramCondition">条件</param>
        /// <returns>是否更新成功（true：成功 false：失败）</returns>
        public bool UpdateByCondition(object paramTBModel, string paramCondition)
        {
            if (!paramTBModel.GetType().FullName.Contains(DALConst.TBModel))
            {
                ReturnMsg = "the paramTBModel is not TBModel.";
                return false;
            }
            //返回值
            bool result = true;

            CreateSQL createSQL = new CreateSQL();
            ComCRUDModel param = new ComCRUDModel();

            //SQL语句
            param.StrSQL = createSQL.CreateSQLForUpdateByCondition(paramTBModel, paramCondition);
            //执行查询操作
            object objResult = DBManager.Update(dbConfig, SQLID.COMM_SQL01, param);

            if (objResult != null && (int)objResult < 1)
            {
                result = false;
            }

            return result;
        }
        /// <summary>
        /// 更新-有事务,通过给定的ID(Model实例列表内元素的顺序需跟ID列表内元素的顺序一致)
        /// </summary>
        /// <param name="paramTBModelList">Model实例列表</param>
        /// <param name="paramID">ID列表</param>
        /// <returns>是否更新成功（true：成功 false：失败）</returns>
        public bool UpdateByIDListHaveTrans(List<object> paramTBModelList, object[] paramID)
        {
            //返回结果
            bool result = true;
            bool resultTmp = true;

            //事务开始
            DBManager.BeginTransaction(dbConfig);

            try
            {
                int i = 0;
                foreach (object model in paramTBModelList)
                {
                    //更新
                    resultTmp = UpdateByID(model, paramID[i]);
                    //结果判定
                    if (!resultTmp)
                    {
                        result = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                ReturnMsg = ex.Message;
            }

            if (result)
            {
                //事务提交
                DBManager.CommitTransaction(dbConfig);
            }
            else
            {
                //事务回滚
                DBManager.RollBackTransaction(dbConfig);
            }

            return result;
        }
        /// <summary>
        /// 更新-无事务,通过给定的ID(Model实例列表内元素的顺序需跟ID列表内元素的顺序一致)
        /// </summary>
        /// <param name="paramTBModelList">Model实例列表</param>
        /// <param name="paramID">ID列表</param>
        /// <returns>是否更新成功（true：成功 false：失败）</returns>
        public bool UpdateByIDListNoTrans(List<object> paramTBModelList, object[] paramID)
        {
            //返回结果
            bool result = true;
            bool resultTmp = true;

            try
            {
                int i = 0;
                foreach (object model in paramTBModelList)
                {
                    //更新
                    resultTmp = UpdateByID(model, paramID[i]);
                    //结果判定
                    if (!resultTmp)
                    {
                        result = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                ReturnMsg = ex.Message;
            }

            return result;
        }
        /// <summary>
        /// 更新-有事务，通过给定的WHERE条件(Model实例列表内元素的顺序需跟条件列表内元素的顺序一致)
        /// </summary>
        /// <param name="modelList">Model实例列表</param>
        /// <param name="condition">条件列表</param>
        /// <returns>是否更新成功（true：成功 false：失败）</returns>
        public bool UpdateByConditionListHaveTrans(List<object> paramModelList, string[] paramCondition)
        {
            //返回结果
            bool result = true;
            bool resultTmp = true;

            //事务开始
            DBManager.BeginTransaction(dbConfig);

            try
            {
                int i = 0;
                foreach (object model in paramModelList)
                {
                    //更新
                    resultTmp = UpdateByCondition(model, paramCondition[i]);
                    //结果判定
                    if (!resultTmp)
                    {
                        result = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                ReturnMsg = ex.Message;
            }

            if (result)
            {
                //事务提交
                DBManager.CommitTransaction(dbConfig);
            }
            else
            {
                //事务回滚
                DBManager.RollBackTransaction(dbConfig);
            }

            return result;
        }
        /// <summary>
        /// 更新-无事务，通过给定的WHERE条件(Model实例列表内元素的顺序需跟条件列表内元素的顺序一致)
        /// </summary>
        /// <param name="modelList">Model实例列表</param>
        /// <param name="condition">条件列表</param>
        /// <returns>是否更新成功（true：成功 false：失败）</returns>
        public bool UpdateByConditionListNoTrans(List<object> paramModelList, string[] paramCondition)
        {
            //返回结果
            bool result = true;
            bool resultTmp = true;

            try
            {
                int i = 0;
                foreach (object model in paramModelList)
                {
                    //更新
                    resultTmp = UpdateByCondition(model, paramCondition[i]);
                    //结果判定
                    if (!resultTmp)
                    {
                        result = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                ReturnMsg = ex.Message;
            }

            return result;
        }

        #endregion 更新End

        #region 删除

        /// <summary>
        /// 删除，根据TBModel内已赋值的ID和版本号
        /// </summary>
        /// <param name="model">model实例</param>
        /// <returns>是否删除成功（true：成功 false：失败）</returns>
        public bool Delete(object paramModel)
        {
            //返回值
            bool result = true;

            CreateSQL createSQL = new CreateSQL();
            ComCRUDModel param = new ComCRUDModel();

            //SQL语句
            param.StrSQL = createSQL.CreateSQLForDeleteByIDAndVersionNo(paramModel);

            //执行查询操作
            object objResult = DBManager.Delete(dbConfig, SQLID.COMM_SQL01, param);

            if (objResult != null && (int)objResult < 1)
            {
                result = false;
            }

            return result;
        }
        /// <summary>
        /// 删除,通过给定的ID
        /// </summary>
        /// <param name="model">model实例</param>
        /// <param name="id">id（数据表自增项目）</param>
        /// <returns>是否删除成功（true：成功 false：失败）</returns>
        public bool DeleteByID(object paramModel, object paramId)
        {
            //返回值
            bool result = true;

            CreateSQL createSQL = new CreateSQL();
            ComCRUDModel param = new ComCRUDModel();

            //Int64型ID
            if (paramId is Int64)
            {
                //SQL语句
                param.StrSQL = createSQL.CreateSQLForDeleteByInt64ID(paramModel, (Int64)paramId);
            }
            //String型ID
            else if (paramId is string)
            {
                //SQL语句
                param.StrSQL = createSQL.CreateSQLForDeleteByStrID(paramModel, paramId.ToString());
            }
            else
            {
                return false;
            }

            //执行查询操作
            object objResult = DBManager.Delete(dbConfig, SQLID.COMM_SQL01, param);

            if (objResult != null && (int)objResult < 1)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 删除,通过给定的WHERE条件
        /// </summary>
        /// <param name="model">Model实例</param>
        /// <param name="condition">条件</param>
        /// <returns>是否删除成功（true：成功 false：失败）</returns>
        public bool DeleteByCondition(object paramModel, string paramCondition)
        {
            //返回值
            bool result = true;

            CreateSQL createSQL = new CreateSQL();
            ComCRUDModel param = new ComCRUDModel();

            //SQL语句
            param.StrSQL = createSQL.CreateSQLForDeleteByCondition(paramModel, paramCondition);
            //执行查询操作
            object objResult = DBManager.Delete(dbConfig, SQLID.COMM_SQL01, param);

            if (objResult != null && (int)objResult < 1)
            {
                result = false;
            }

            return result;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramConditionTBModelList"></param>
        /// <returns></returns>
        public bool DeleteByList<T>(IList<T> paramConditionTBModelList)
        {
            if (!paramConditionTBModelList[0].GetType().FullName.Contains(DALConst.TBModel))
            {
                ReturnMsg = "the ConditionTBModel is not TBModel.";
                return false;
            }
            string argsStatementName = "COMM_SQL_DELETE_" + paramConditionTBModelList[0].GetType().Name;

            int obj = DBManager.Delete(DBCONFIG.Coeus, argsStatementName, paramConditionTBModelList);
            if (obj < 1)
            {
                return false;
            }

            return true;
        }
        #endregion 删除End

        #region 混合
        ///// <summary>
        ///// 混合操作,通过给定的ID(Model实例列表内元素的顺序需跟ID列表内元素的顺序一致)
        ///// </summary>
        ///// <param name="insertList">新增列表</param>
        ///// <param name="updateList">更新列表</param>
        ///// <param name="deleteList">删除列表</param>
        ///// <returns></returns>
        //public bool MingleByListHaveTrans(IList<object> paramInsertList, IList<object> paramUpdateList, IList<object> paramDeleteList)
        //{
        //    //返回结果
        //    bool result = true;
        //    bool resultTmp = true;
        //    MDOperate mo = new MDOperate();

        //    //事务开始
        //    DBManager.BeginTransaction(dbConfig);

        //    try
        //    {
        //        foreach (object model in paramInsertList)
        //        {
        //            //新增
        //            resultTmp = Insert(model);
        //            //结果判定
        //            if (!resultTmp)
        //            {
        //                result = false;
        //                break;
        //            }
        //        }
        //        foreach (object model in paramUpdateList)
        //        {
        //            //更新
        //            resultTmp = UpdateByID(model, Convert.ToInt64(model.GetType().GetProperty(mo.GetPKName(model)).GetValue(model, null)));
        //            //结果判定
        //            if (!resultTmp)
        //            {
        //                result = false;
        //                break;
        //            }
        //        }
        //        foreach (object model in paramDeleteList)
        //        {
        //            //删除
        //            resultTmp = DeleteByID(model, Convert.ToInt64(model.GetType().GetProperty(mo.GetPKName(model)).GetValue(model, null)));
        //            //结果判定
        //            if (!resultTmp)
        //            {
        //                result = false;
        //                break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = false;
        //        ReturnMsg = ex.Message;
        //    }

        //    if (result)
        //    {
        //        //事务提交
        //        DBManager.CommitTransaction(dbConfig);
        //    }
        //    else
        //    {
        //        //事务回滚
        //        DBManager.RollBackTransaction(dbConfig);
        //    }

        //    return result;
        //}
        #endregion 混合End
    }
}
