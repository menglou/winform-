using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.DAL;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using SkyCar.Coeus.Common.ExtendClass;
using SkyCar.Coeus.Common.ExtendClass.Interface;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.BLL.Common
{
    /// <summary>
    /// 基本业务逻辑
    /// </summary>
    public class BLLBase
    {
        #region 变量定义
        /// <summary>
        /// 业务模块
        /// </summary>
        public string BussID = string.Empty;
        /// <summary>
        /// 返回结果编码
        /// </summary>
        public string ResultCode { get; set; }
        /// <summary>
        /// 返回结果消息
        /// </summary>
        public string ResultMsg = string.Empty;
        /// <summary>
        /// 数据库配置
        /// </summary>
        public string DBConfig = string.Empty;

        /// <summary>
        /// 主窗体
        /// </summary>
        public static Form MainFrom = new Form();
        /// <summary>
        /// 当前已打开的Form
        /// </summary>
        public static Dictionary<string, Form> DicWorkForm = new Dictionary<string, Form>();

        #endregion

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="paramBussID">业务ID</param>
        /// <param name="paramDBConfig">数据库配置（默认值：DBCONFIG.Coeus）</param>
        public BLLBase(string paramBussID, string paramDBConfig = DBCONFIG.Coeus)
        {
            //业务ID
            BussID = paramBussID;
            //数据库配置
            DBConfig = paramDBConfig;
        }

        #region 查询
        /// <summary>
        /// 查询单条信息(需要写SQL)
        /// </summary>
        /// <param name="paramSQLID">SQLID</param>
        /// <param name="paramModel">查询条件Model（TBModel或UIModel或者QCModel）</param>
        /// <returns>查询结果对象</returns>
        public object QueryForObject(string paramSQLID, object paramModel)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            //定义结果对象
            object result = new object();
            //执行查询
            result = DBManager.QueryForObject(DBConfig, paramSQLID, paramModel);
            LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            return result;
        }
        /// <summary>
        /// 查询单条信息(需要写SQL)
        /// </summary>
        /// <typeparam name="D">查询结果Model类型</typeparam>
        /// <param name="paramSQLID">SQLID</param>
        /// <param name="paramModel">查询条件Model（TBModel或UIModel或者QCModel）</param>
        /// <returns>查询结果对象</returns>
        public D QueryForObject<D>(string paramSQLID, object paramModel)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            //定义结果对象
            D result = Activator.CreateInstance<D>();
            //执行查询
            result = DBManager.QueryForObject<D>(DBConfig, paramSQLID, paramModel);
            LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            return result;
        }

        /// <summary>
        /// 查询多条信息(需要写SQL)
        /// </summary>
        /// <typeparam name="D">查询结果Model类型</typeparam>
        /// <param name="paramSQLID">SQLID</param>
        /// <param name="paramModel">查询条件Model（TBModel或UIModel或者QCModel）</param>
        /// <param name="paramResultList">查询结果List</param>
        /// <returns></returns>
        public void QueryForList<D>(string paramSQLID, object paramModel, IList<D> paramResultList)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            paramResultList.Clear();
            //执行查询
            DBManager.QueryForList<D>(DBConfig, paramSQLID, paramModel, paramResultList);
            LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
        }

        /// <summary>
        /// 查询单条信息(不用写SQL-精确条件)
        /// </summary>
        /// <typeparam name="S">TBModel类型</typeparam>
        /// <typeparam name="D">结果Model类型（TBModel或UIModel）</typeparam>
        /// <param name="paramModel">查询条件Model（TBModel或UIModel或者QCModel）</param>
        /// <param name="paramResultModel">查询结果Model</param>
        /// <returns>结果Model</returns>
        public void QueryForObject<S, D>(object paramModel, D paramResultModel)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);

            //实例化CRUD对象
            CRUD crud = new CRUD(DBConfig);
            //定义CRUD结果对象
            S tbModel = Activator.CreateInstance<S>();
            //检查paramModel是否是TBModel类型
            //是TBModel类型的场合：不用转换，执行查询
            if (paramModel.GetType().FullName.Contains(SysConst.EN_TBMODEL))
            {
                tbModel = (S)crud.QueryForObject<S>(paramModel);
            }
            //不是TBModel类型的场合：转换后，执行查询
            else
            {
                //创建S类型实例
                S argsModel = Activator.CreateInstance<S>();
                //根据属性名和类型，将给定paramModel的值复制到指定S类型的Model内
                argsModel = CopyModel<S>(paramModel);
                //执行查询
                tbModel = (S)crud.QueryForObject<S>(argsModel);
            }
            //根据属性名和类型，将给定对象的值复制到指定类型的D类型对象内
            //paramResultModel = CopyModel<D>(tbModel);
            //结果paramResultModel为null时，进行初始化操作
            if (paramResultModel == null)
            {
                paramResultModel = Activator.CreateInstance<D>();
            }
            CopyModel(tbModel, paramResultModel);
            LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);

        }
        /// <summary>
        /// 查询记录条数(不用写SQL-精确条件)
        /// </summary>
        /// <typeparam name="TBModelType">TBModel类型</typeparam>
        /// <param name="paramModel">查询条件Model（TBModel或UIModel或者QCModel）</param>
        /// <returns></returns>
        public int QueryForCount<TBModelType>(object paramModel)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);

            //实例化CRUD对象
            CRUD crud = new CRUD(DBConfig);
            int result;
            //检查paramModel是否是TBModel类型
            //是TBModel类型的场合：不用转换，执行查询
            if (paramModel.GetType().FullName.Contains(SysConst.EN_TBMODEL))
            {
                result = crud.QueryForCount(paramModel);
            }
            //不是TBModel类型的场合：转换后，执行查询
            else
            {
                //创建TBModelType类型实例
                TBModelType argsModel = Activator.CreateInstance<TBModelType>();
                //根据属性名和类型，将给定paramModel的值复制到指定S类型的Model内
                argsModel = CopyModel<TBModelType>(paramModel);
                //执行查询
                result = crud.QueryForCount(argsModel);
            }

            LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                "RecordCount=" + result, "", null);
            return result;
        }
        /// <summary>
        /// 查询单条信息(不用写SQL-模糊条件)
        /// </summary>
        /// <typeparam name="S">TBModel类型</typeparam>
        /// <typeparam name="D">结果Model类型（TBModel或UIModel）</typeparam>
        /// <param name="paramModel">查询条件Model（TBModel或UIModel或者QCModel）</param>
        /// <param name="paramResultModel">查询结果Model</param>
        /// <returns>结果Model</returns>
        public void QueryForObjectLike<S, D>(object paramModel, D paramResultModel)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);

            //实例化CRUD对象
            CRUD crud = new CRUD(DBConfig);
            //定义CRUD结果对象
            S tbModel = Activator.CreateInstance<S>();
            //检查paramModel是否是TBModel类型
            //是TBModel类型的场合：不用转换，执行查询
            if (paramModel.GetType().FullName.Contains(SysConst.EN_TBMODEL))
            {
                tbModel = (S)crud.QueryForObjectLike<S>(paramModel);
            }
            //不是TBModel类型的场合：转换后，执行查询
            else
            {
                //创建S类型实例
                S argsModel = Activator.CreateInstance<S>();
                //根据属性名和类型，将给定paramModel的值复制到指定S类型的Model内
                argsModel = CopyModel<S>(paramModel);
                //执行查询
                tbModel = (S)crud.QueryForObjectLike<S>(argsModel);
            }
            //根据属性名和类型，将给定对象的值复制到指定类型的D类型对象内
            //paramResultModel = CopyModel<D>(tbModel);
            //结果paramResultModel为null时，进行初始化操作
            if (paramResultModel == null)
            {
                paramResultModel = Activator.CreateInstance<D>();
            }
            CopyModel(tbModel, paramResultModel);
            LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);

        }
        /// <summary>
        /// 查询多条信息(不用写SQL)
        /// </summary>
        /// <typeparam name="S">TBModel类型</typeparam>
        /// <typeparam name="D">结果Model类型（TBModel或UIModel）</typeparam>
        /// <param name="paramModel">查询条件Model（TBModel或UIModel或者QCModel）</param>
        /// <returns>结果ModelList</returns>
        public void QueryForList<S, D>(object paramModel, IList<D> paramResultList)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            paramResultList.Clear();
            //实例化CRUD对象
            CRUD crud = new CRUD(DBConfig);
            //定义CRUD结果List
            List<S> tbModelList = new List<S>();
            //检查T是否是TBModel类型
            //是TBModel类型的场合：不用转换，执行查询
            if (paramModel.GetType().FullName.Contains(SysConst.EN_TBMODEL))
            {
                //执行查询
                crud.QueryForList<S>(paramModel, tbModelList);
            }
            //是TBModel类型的场合：转换后，执行查询
            else
            {
                //创建S类型实例
                S argsModel = Activator.CreateInstance<S>();
                //根据属性名和类型，将给定paramModel的值复制到指定S类型的Model内
                argsModel = CopyModel<S>(paramModel);
                //执行查询
                crud.QueryForList<S>(argsModel, tbModelList);
            }

            //根据属性名和类型，将给定List的值复制到指定类型的IList<T>内
            CopyModelList<S, D>(tbModelList, paramResultList);
            LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);

        }
        /// <summary>
        /// 查询多条信息(不用写SQL-模糊条件)
        /// </summary>
        /// <typeparam name="S">TBModel类型</typeparam>
        /// <typeparam name="D">结果Model类型（TBModel或UIModel）</typeparam>
        /// <param name="paramModel">查询条件Model（TBModel或UIModel或者QCModel）</param>
        /// <returns>结果ModelList</returns>
        public void QueryForListLike<S, D>(object paramModel, IList<D> paramResultList)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            paramResultList.Clear();
            //实例化CRUD对象
            CRUD crud = new CRUD(DBConfig);
            //定义CRUD结果List
            List<S> tbModelList = new List<S>();
            //检查T是否是TBModel类型
            //是TBModel类型的场合：不用转换，执行查询
            if (paramModel.GetType().FullName.Contains(SysConst.EN_TBMODEL))
            {
                //执行查询
                crud.QueryForListLike<S>(paramModel, tbModelList);
            }
            //是TBModel类型的场合：转换后，执行查询
            else
            {
                //创建S类型实例
                S argsModel = Activator.CreateInstance<S>();
                //根据属性名和类型，将给定paramModel的值复制到指定S类型的Model内
                argsModel = CopyModel<S>(paramModel);
                //执行查询
                crud.QueryForListLike<S>(argsModel, tbModelList);
            }

            //根据属性名和类型，将给定List的值复制到指定类型的IList<T>内
            CopyModelList<S, D>(tbModelList, paramResultList);
            LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);

        }

        /// <summary>
        /// 查询多条信息(不用写SQL)
        /// </summary>
        /// <typeparam name="D">结果Model类型（TBModel或UIModel）</typeparam>
        /// <param name="paramTBModel">查询条件TBModel</param>
        /// <returns>结果ModelList</returns>
        public void QueryForList<D>(object paramTBModel, IList<D> paramResultList)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            paramResultList.Clear();
            //检查参数是否为TBModel
            if (!paramTBModel.GetType().FullName.Contains(SysConst.EN_TBMODEL))
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
                return;
            }
            //定义CRUD结果List
            List<object> tbModelList = new List<object>();
            //实例化CRUD对象
            CRUD crud = new CRUD(DBConfig);
            //执行查询
            crud.QueryForList<object>(paramTBModel, tbModelList);
            //根据属性名和类型，将给定List的值复制到指定类型的IList<T>内
            CopyModelList<D>(tbModelList, paramResultList);
            LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);

        }

        /// <summary>
        /// 查询多条信息(不用写SQL-模糊条件)
        /// </summary>
        /// <typeparam name="D">结果Model类型（TBModel或UIModel）</typeparam>
        /// <param name="paramTBModel">查询条件TBModel</param>
        /// <param name="paramResultList">查询结果List</param>
        /// <returns></returns>
        public void QueryForListLike<D>(object paramTBModel, IList<D> paramResultList)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            paramResultList.Clear();
            //检查参数是否为TBModel
            if (!paramTBModel.GetType().FullName.Contains(SysConst.EN_TBMODEL))
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
                return;
            }
            //定义CRUD结果List
            List<object> tbModelList = new List<object>();
            //实例化CRUD对象
            CRUD crud = new CRUD(DBConfig);
            //执行查询
            crud.QueryForListLike<object>(paramTBModel, tbModelList);
            //根据属性名和类型，将给定List的值复制到指定类型的IList<T>内
            CopyModelList<D>(tbModelList, paramResultList);
            LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);

        }
        #endregion

        #region 新增

        /// <summary>
        /// 新增(不用写SQL)，根据[TBModel对象]。
        /// </summary>
        /// <param name="paramTBModel">TBModel对象</param>
        /// <returns>成功：True/失败：False</returns>
        public bool Insert<T>(T paramTBModel)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            //检查参数是否为TBModel
            if (!paramTBModel.GetType().FullName.Contains(SysConst.EN_TBMODEL))
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
                return false;
            }

            //返回值
            bool result;
            //实例化CRUD对象
            CRUD crud = new CRUD(DBConfig);
            //执行插入
            result = crud.InsertByList(new List<T> { paramTBModel });
            //输出结束日志
            if (result)
            {
                LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            }
            else
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            }
            return result;
        }

        //public bool Insert(string dbConfig, object paramTBModel)
        //{
        //    LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
        //    //检查参数是否为TBModel
        //    if (!paramTBModel.GetType().FullName.Contains(SysConst.EN_TBMODEL))
        //    {
        //        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
        //        return false;
        //    }

        //    //返回值
        //    bool result;
        //    //实例化CRUD对象
        //    CRUD crud = new CRUD(dbConfig);
        //    //执行插入
        //    result = crud.Insert(paramTBModel);
        //    //输出结束日志
        //    if (result == true)
        //    {
        //        LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
        //    }
        //    else
        //    {
        //        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
        //    }
        //    return result;
        //}

        /// <summary>
        /// 新增(不用写SQL)，根据[TBModel类型]和[UIModel对象]。
        /// 根据**_ID是否被赋值，来区分执行新增操作或者更新操作。
        /// </summary>
        /// <typeparam name="S">源Model类型</typeparam>
        /// <typeparam name="D">要新增的数据表对应的TBModel类型</typeparam>
        /// <param name="paramModelList">UIModelList对象或者TBModelList对象</param>
        /// <returns>成功：True/失败：False</returns>
        public bool InsertByList<S, D>(IList<S> paramModelList)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "",
                "", null);
            //返回值
            bool result = false;
            try
            {
                if (paramModelList.Count <= 0) return true;
                //检查T是否是TBModel类型
                if (!typeof(D).ToString().Contains(SysConst.EN_TBMODEL))
                {
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName,
                        MethodBase.GetCurrentMethod().ToString(), "", "", null);
                    return false;
                }

                #region 给ID赋值
                foreach (S obj in paramModelList)
                {
                    Type tp = obj.GetType();
                    //在[新增]参数Model中查找ID，并将新生成的GUID赋值给它
                    foreach (PropertyInfo pi in tp.GetProperties())
                    {
                        string[] tmp = pi.Name.Split(Convert.ToChar(SysConst.ULINE));
                        if (tmp.Length >= 2 && SysConst.EN_ID.Equals(tmp[1]) && typeof(D).GetProperty(pi.Name) != null)
                        {
                            //新ID
                            string tmpID;
                            var getId = pi.GetValue(obj, null);
                            if (getId != null && !string.IsNullOrEmpty(getId.ToString()))
                            {
                                tmpID = getId.ToString();
                            }
                            else
                            {
                                tmpID = Guid.NewGuid().ToString();
                            }
                            pi.SetValue(obj, tmpID, null);
                            break;
                        }
                    }
                    foreach (PropertyInfo pi in tp.GetProperties())
                    {
                        //设置初始[版本号为]1
                        if (pi.Name.Contains("_VersionNo"))
                        {
                            pi.SetValue(obj, System.Convert.ToInt64(1), null);
                        }
                        //设置[创建人]为登录用户
                        else if (pi.Name.Contains("_CreatedBy"))
                        {
                            pi.SetValue(obj, LoginInfoDAX.UserName, null);
                        }
                        //设置[修改人]为登录用户
                        else if (pi.Name.Contains("_UpdatedBy"))
                        {
                            pi.SetValue(obj, LoginInfoDAX.UserName, null);
                        }
                        //设置[有效]为TRUE
                        else if (pi.Name.Contains("_IsValid"))
                        {
                            pi.SetValue(obj, true, null);
                        }
                    }
                }
                #endregion

                //根据属性名和类型，将给定Model的值复制到指定类型的ModelList内
                List<D> paramTBModelList = new List<D>();
                //判断类型S和类型D是否相同
                if (typeof(S) != typeof(D))
                {
                    CopyModelList<S, D>(paramModelList, paramTBModelList);
                    //实例化CRUD对象
                    CRUD crud = new CRUD(DBConfig);
                    result = crud.InsertByList<D>(paramTBModelList);
                }
                else
                {
                    //实例化CRUD对象
                    CRUD crud = new CRUD(DBConfig);
                    result = crud.InsertByList<S>(paramModelList);
                }

                //输出结束日志
                if (result)
                {
                    LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName,
                        MethodBase.GetCurrentMethod().ToString(), "", "", null);
                }
                else
                {
                    #region 新增失败的场合：清空ID
                    foreach (S obj in paramModelList)
                    {
                        //新ID
                        string tmpID = string.Empty;

                        Type tp = obj.GetType();
                        //在[新增]参数Model中查找ID，并将新生成的GUID赋值给它
                        foreach (PropertyInfo pi in tp.GetProperties())
                        {
                            string[] tmp = pi.Name.Split(Convert.ToChar(SysConst.ULINE));
                            if (tmp.Length >= 2 && SysConst.EN_ID.Equals(tmp[1]))
                            {
                                pi.SetValue(obj, tmpID, null);
                                break;
                            }
                        }
                    }
                    #endregion
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName,
                        MethodBase.GetCurrentMethod().ToString(), "", "", null);
                }
            }
            catch (Exception ex)
            {
                //输出错误日志
                LogHelper.WriteErrorLog(BussID,
                    MethodBase.GetCurrentMethod().ToString() + SysConst.ENTER + ex.StackTrace, ex.Message, null, ex);
            }
            return result;
        }
        #endregion

        #region 更新

        /// <summary>
        /// 更新(不用写SQL)，根据[TBModel对象]内已赋值的ID和版本号
        /// </summary>
        /// <param name="paramTBModel">TBModel对象</param>
        /// <returns>成功：True/失败：False</returns>
        public bool Update(object paramTBModel)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            //检查参数是否为TBModel
            if (!paramTBModel.GetType().FullName.Contains(SysConst.EN_TBMODEL))
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
                return false;
            }
            //检查ID或者版本号是否为空
            if (IsWhereIdORVersionEmpty(paramTBModel))
            {
                ResultMsg = "更新条件[WHERE_XXX_ID]或者[WHERE_XXX_VersionNo]未被赋值！";
                return false;
            }

            CRUD crud = new CRUD(DBConfig);
            bool tmpBool = crud.Update(paramTBModel);
            //输出结束日志
            if (tmpBool == true)
            {
                LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            }
            else
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            }
            return tmpBool;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="paramSQLID">SQLID</param>
        /// <param name="paramModel">参数Model</param>
        /// <returns>成功：True/失败：False</returns>
        public bool Update(string paramSQLID, object paramModel)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);

            int tmpInt = DBManager.Update(DBConfig, paramSQLID, paramModel);

            //输出结束日志，并返回更新结果
            if (tmpInt > 0)
            {
                LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
                return true;
            }
            else
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
                return false;
            }
        }
        /// <summary>
        /// 更新(不用写SQL)，根据指定的WHERE条件更新TBModel
        /// </summary>
        /// <param name="paramTBModel">即将更新的参数Model（必须为TBModel）</param>
        /// <param name="paramWHERE">WHERE条件(例："WHERE ID=1")</param>
        /// <returns>成功：True/失败：False</returns>
        public bool UpdateByCondition(object paramTBModel, string paramWHERE)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            //检查参数是否为TBModel
            if (!paramTBModel.GetType().FullName.Contains(SysConst.EN_TBMODEL))
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
                return false;
            }

            CRUD crud = new CRUD(DBConfig);
            bool tmpBool = crud.UpdateByCondition(paramTBModel, paramWHERE);
            //输出结束日志
            if (tmpBool == true)
            {
                LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            }
            else
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            }
            return tmpBool;
        }
        #endregion

        #region 删除

        /// <summary>
        /// 删除(不用写SQL)，根据[TBModel类型]和[UIModel对象]。
        /// 根据WHERE_XXX_ID进行删除。
        /// </summary>
        /// <typeparam name="S">源Model类型</typeparam>
        /// <typeparam name="D">要删除的数据表对应的TBModel类型</typeparam>
        /// <param name="paramUIModelList">UIModelList对象或者TBModelList对象</param>
        /// <returns>成功：True/失败：False</returns>
        public bool DeleteByList<S, D>(IList<S> paramUIModelList)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "",
                "", null);
            //返回值
            bool result = false;
            try
            {
                if (paramUIModelList.Count <= 0) return true;
                //检查T是否是TBModel类型
                if (!typeof(D).ToString().Contains(SysConst.EN_TBMODEL))
                {
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName,
                        MethodBase.GetCurrentMethod().ToString(), "", "", null);
                    return false;
                }

                //根据属性名和类型，将给定Model的值复制到指定类型的ModelList内
                if (typeof(S) != typeof(D))
                {
                    List<D> paramTBModelList = new List<D>();
                    CopyModelList<S, D>(paramUIModelList, paramTBModelList);
                    //实例化CRUD对象
                    CRUD crud = new CRUD(DBConfig);
                    result = crud.DeleteByList<D>(paramTBModelList);
                }
                else
                {
                    //实例化CRUD对象
                    CRUD crud = new CRUD(DBConfig);
                    result = crud.DeleteByList<S>(paramUIModelList);
                }

                //输出结束日志
                if (result)
                {
                    LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName,
                        MethodBase.GetCurrentMethod().ToString(), "", "", null);
                }
                else
                {
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName,
                        MethodBase.GetCurrentMethod().ToString(), "", "", null);
                }
            }
            catch (Exception ex)
            {
                //输出错误日志
                LogHelper.WriteErrorLog(BussID,
                    MethodBase.GetCurrentMethod().ToString() + SysConst.ENTER + ex.StackTrace, ex.Message, null, ex);
            }
            return result;
        }

        /// <summary>
        /// 删除(不用写SQL)，根据[XModel对象]（根据XModel内已赋值的ID、版本号和TBModel类型）
        /// </summary>
        /// <typeparam name="T">TBModel类型</typeparam>
        /// <param name="paramModel">XModel对象（任意类型Model）</param>
        /// <returns>成功：True/失败：False</returns>
        public bool Delete<T>(object paramModel)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            //检查T是否是TBModel类型
            if (!typeof(T).ToString().Contains(SysConst.EN_TBMODEL))
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
                return false;
            }
            //根据属性名和类型，将给定Model的值复制到指定类型的Model内
            T argsT = CopyModel<T>(paramModel);

            //检查ID是否被赋值
            if (IsWhereIdEmpty(argsT))
            {
                ResultMsg = "删除条件[WHERE_XXX_ID]未被赋值！";
                return false;
            }

            CRUD crud = new CRUD(DBConfig);
            bool tmpBool = crud.DeleteByList(new List<T> { argsT });
            //输出结束日志
            if (tmpBool == true)
            {
                LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            }
            else
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            }
            return tmpBool;
        }
        /// <summary>
        /// 删除，根据TBModel内已赋值的ID和版本号
        /// </summary>
        /// <param name="paramTBModel">即将删除的参数Model（必须为TBModel）</param>
        /// <returns>成功：True/失败：False</returns>
        public bool Delete(object paramTBModel)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            //检查参数是否为TBModel
            if (!paramTBModel.GetType().FullName.Contains(SysConst.EN_TBMODEL))
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
                return false;
            }
            //检查ID是否被赋值
            if (IsWhereIdEmpty(paramTBModel))
            {
                ResultMsg = "删除条件[WHERE_XXX_ID]未被赋值！";
                return false;
            }

            CRUD crud = new CRUD(DBConfig);
            bool tmpBool = crud.DeleteByList(new List<object> { paramTBModel });
            //输出结束日志
            if (tmpBool == true)
            {
                LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            }
            else
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            }
            return tmpBool;
        }

        /// <summary>
        /// 删除，根据TBModel内已赋值的ID和版本号
        /// </summary>
        /// <param name="paramHeadTBModel">即将删除的参数Model（必须为TBModel）</param>
        /// <param name="paramDetailTBModelList">即将删除的参数Model列表（必须为TBModel）</param>
        /// <returns>成功：True/失败：False</returns>
        public bool UnityDelete<T1, T2>(T1 paramHeadTBModel, IList<T2> paramDetailTBModelList)
        {
            var funcName = "UnityDelete";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);
            //检查参数是否为TBModel
            if (!paramHeadTBModel.GetType().FullName.Contains(SysConst.EN_TBMODEL))
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, "参数[Head]不是TBModel类型！", "", null);
                return false;
            }
            //检查参数是否为TBModel
            if (paramDetailTBModelList.Count > 0 && !paramDetailTBModelList[0].GetType().FullName.Contains(SysConst.EN_TBMODEL))
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, "参数[Detal]不是TBModel类型！", "", null);
                return false;
            }

            #region 检查ID是否被赋值

            if (IsWhereIdEmpty(paramHeadTBModel))
            {
                ResultMsg = "参数[Head]中的删除条件[WHERE_XXX_ID]未被赋值！";
                return false;
            }
            foreach (var loopDetail in paramDetailTBModelList)
            {
                if (IsWhereIdEmpty(loopDetail))
                {
                    ResultMsg = "参数[Detail]中的删除条件[WHERE_XXX_ID]未被赋值！";
                    return false;
                }
            }

            #endregion

            var crud = new CRUD(DBConfig);
            bool tmpBool = crud.DeleteByList(new List<T1> { paramHeadTBModel });
            if (!tmpBool)
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, "删除[Head]失败！", "", null);
            }
            LogHelper.WriteBussLog(BussID, LoginInfoDAX.UserName, funcName, "删除[Head]成功！", "", null);

            if (paramDetailTBModelList.Count > 0)
            {
                tmpBool = crud.DeleteByList(paramDetailTBModelList);
                if (!tmpBool)
                {
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, "删除[Detail]失败！", "", null);
                }
            }

            LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, funcName, "删除[Detail]成功！", "", null);

            return tmpBool;
        }

        /// <summary>
        /// 删除(需要写SQL)
        /// </summary>
        /// <param name="paramSQLID">SQLID</param>
        /// <param name="paramModel">参数Model</param>
        /// <returns>成功：True/失败：False</returns>
        public bool Delete(string paramSQLID, object paramModel)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);

            int tmpInt = DBManager.Delete(DBConfig, paramSQLID, paramModel);
            //输出结束日志，并返回处理结果
            if (tmpInt > 0)
            {
                LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
                return true;
            }
            else
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
                return false;
            }
        }
        /// <summary>
        /// 删除(不用写SQL)，根据[WHERE条件]和[TBModel对象]
        /// </summary>
        /// <param name="paramTBModel">TBModel对象</param>
        /// <param name="paramWHERE">WHERE条件(例："WHERE ID=1")</param>
        /// <returns>成功：True/失败：False</returns>
        public bool DeleteByCondition(object paramTBModel, string paramWHERE)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            //检查参数是否为TBModel
            if (!paramTBModel.GetType().FullName.Contains(SysConst.EN_TBMODEL))
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
                return false;
            }

            CRUD crud = new CRUD(DBConfig);
            bool tmpBool = crud.DeleteByCondition(paramTBModel, paramWHERE);
            //输出结束日志
            if (tmpBool == true)
            {
                LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            }
            else
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            }
            return tmpBool;
        }
        #endregion

        #region 保存

        /// <summary>
        /// 保存
        /// <para>根据指定的[TBModel类型]，将[UIModel对象]的值保存到TBModel对应的数据表中。</para>
        /// <para>根据**_ID是否被赋值，来区分执行新增操作或者更新操作。</para>
        /// <para>保存成功后，最新的ID和版本号可在UIModel中获取。</para>
        /// </summary>
        /// <typeparam name="T">TBModel类型</typeparam>
        /// <param name="paramTBModel">TBModel对象</param>
        /// <param name="paramNewId">默认主键ID</param>
        /// <returns>成功：True/失败：False</returns>
        public virtual bool Save<T>(T paramTBModel, string paramNewId = "")
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            //返回值
            bool result = false;
            try
            {
                //检查T是否是TBModel类型
                if (!typeof(T).ToString().Contains(SysConst.EN_TBMODEL))
                {
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
                    return false;
                }

                //根据属性名和类型，将给定Model的值复制到指定类型的Model内
                T argsT = CopyModel<T>(paramTBModel);
                //实例化CRUD对象
                CRUD crud = new CRUD(DBConfig);

                //新增
                if (IsInsert(argsT))
                {
                    #region 给ID赋值

                    //新ID
                    string tmpID = paramNewId == "" ? Guid.NewGuid().ToString() : paramNewId;
                    //ID的全称
                    string tmpIDName = string.Empty;

                    //在[新增]参数Model中查找ID，并将新生成的GUID赋值给它
                    foreach (PropertyInfo pi in argsT.GetType().GetProperties())
                    {
                        string[] tmp = pi.Name.Split(Convert.ToChar(SysConst.ULINE));
                        if (tmp.Length >= 2 && SysConst.EN_ID.Equals(tmp[1]))
                        {
                            tmpIDName = pi.Name;
                            pi.SetValue(argsT, tmpID, null);
                            break;
                        }
                    }

                    //在形参Model中查找ID，将最新ID设置给形参的ID（返回给调用方法）
                    //foreach (PropertyInfo pi in paramTBModel.GetType().GetProperties())
                    //{
                    //    if (tmpIDName.Equals(pi.Name))
                    //    {
                    //        pi.SetValue(paramTBModel, tmpID, null);
                    //        break;
                    //    }
                    //}
                    #endregion
                    //执行Insert
                    result = Insert(argsT);
                }
                //更新
                else
                {
                    //找到[WHERE_XX_VersionNo]，然后将[WHERE_XX_VersionNo]赋值给[XX_VersionNo]
                    //[WHERE_XX_VersionNo]的值是最新的版本号，将其赋值给[XX_VersionNo]，以保持成功后画面用
                    //通用的Update语句内部使用的是[XX_VersionNo]=[XX_VersionNo]+1，所以
                    foreach (var pi in argsT.GetType().GetProperties())
                    {
                        var tmp = pi.Name.Split(Convert.ToChar(SysConst.ULINE));
                        if (tmp.Length == 3 && SysConst.EN_VERSION_NO.Equals(tmp[2]))
                        {
                            foreach (var pi2 in argsT.GetType().GetProperties())
                            {
                                string[] tmp2 = pi2.Name.Split(Convert.ToChar(SysConst.ULINE));
                                if (tmp2.Length >= 2 && SysConst.EN_VERSION_NO.Equals(tmp2[1]))
                                {
                                    pi2.SetValue(argsT, ((Int64)pi.GetValue(argsT, null) + 1), null);
                                    break;
                                }
                            }
                            break;
                        }
                    }

                    //执行Update
                    result = crud.Update(argsT);
                }
                //输出结束日志
                if (result == true)
                {
                    //追加复制（将最新版本号赋值给UIModel）
                    CopyModel(argsT, paramTBModel);
                    LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
                }
                else
                {
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
                }
            }
            catch (Exception ex)
            {
                ResultMsg = ex.Message + SysConst.ENTER + ex.StackTrace;
                //输出错误日志
                LogHelper.WriteErrorLog(BussID, MethodBase.GetCurrentMethod().ToString(), ResultMsg, null, ex);
                result = false;
            }
            return result;
        }
        #endregion

        #region 统一提交（包含插入，删除）
        /// <summary>
        /// 同一保存，调用该方法需要加事务处理
        /// </summary>
        /// <typeparam name="S">源Model类型</typeparam>
        /// <typeparam name="D">要保存的数据表对应的TBModel类型</typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UnitySave<S, D>(SkyCarObservableList<S, D> list) where S : IPropertyChanged
        {
            if (!this.DeleteByList<S, D>(list.DeleteList))
            {
                return false;
            }

            //先更新，后插入，因为插入后ID已经存在值
            //foreach (var item in list.UpdateList)
            //{
            //    if (!this.Update(item))
            //    {
            //        return false;
            //    }
            //}
            if (!this.InsertByList<S, D>(list.InsertList))
            {
                return false;
            }

            //list.ClearDeleteList();
            return true;
        }
        /// <summary>
        /// 统一保存
        /// <para>调用该方法前后，需要加事务处理</para>
        /// </summary>
        /// <typeparam name="TBaseNotificationUIModel">源Model类型</typeparam>
        /// <typeparam name="TBModel">要保存的数据表对应的TBModel类型</typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UnitySave<TBaseNotificationUIModel, TBModel>(SkyCarBindingList<TBaseNotificationUIModel, TBModel> list) where TBaseNotificationUIModel : BaseNotificationUIModel
        {
            //是否正在监控List变化
            if (!list.IsMonitChanges)
            {
                //继续监控List的改变
                list.ContinueMonitChanges();
            }
            //新增
            if (!this.InsertByList<TBModel, TBModel>(list.GetInsertList()))
            {
                return false;
            }
            //删除
            if (!this.DeleteByList<TBModel, TBModel>(list.GetDeleteList()))
            {
                return false;
            }
            //更新
            foreach (var item in list.GetUpdateList())
            {
                if (!this.Update(item))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region CopyModel

        /// <summary>
        /// 覆盖复制（根据属性名和类型，将[源ModelList]的值复制到[结果Model类型]的IList[D]内）
        /// </summary>
        /// <typeparam name="D">结果Model类型</typeparam>
        /// <param name="paramModelList">源ModelList</param>
        /// <returns>结果ModelList</returns>
        public void CopyModelList<D>(IList<object> paramModelList, IList<D> paramResultList)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            //清空结果List
            paramResultList.Clear();
            foreach (object obj in paramModelList)
            {
                D objT = System.Activator.CreateInstance<D>();
                objT = CopyModel<D>(obj);
                paramResultList.Add(objT);
            }
            LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
        }


        /// <summary>
        /// 覆盖复制（根据属性名和类型，将[源ModelLis]的值复制到[结果Model类型]的IList[D]内）
        /// </summary>
        /// <typeparam name="S">源Model类型</typeparam>
        /// <typeparam name="D">结果Model类型</typeparam>
        /// <param name="paramModelList">源ModelList</param>
        /// <param name="paramResultList">结果ModelList（List,BindingList,ObservableCollection）</param>
        public void CopyModelList<S, D>(IList<S> paramModelList, IList<D> paramResultList)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            //清空结果List
            paramResultList.Clear();
            foreach (S obj in paramModelList)
            {
                D objT = System.Activator.CreateInstance<D>();
                objT = CopyModel<D>(obj);
                paramResultList.Add(objT);
            }
            LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);

        }

        /// <summary>
        /// 覆盖复制（根据属性名和类型，将[源Model]的值复制到[结果Model类型]的Model内）
        /// </summary>
        /// <typeparam name="D">结果Model类型</typeparam>
        /// <param name="paramModel">源Model</param>
        /// <returns>结果Model</returns>
        public D CopyModel<D>(object paramModel)
        {
            D resultObj = System.Activator.CreateInstance<D>();
            try
            {
                Type tp = paramModel.GetType();
                foreach (PropertyInfo pi in tp.GetProperties())
                {
                    //过滤索引器
                    if (SysConst.EN_ITEM.Equals(pi.Name)) continue;

                    if (pi.GetValue(paramModel, null) != null)
                    {
                        foreach (PropertyInfo piT in resultObj.GetType().GetProperties())
                        {
                            //过滤索引器
                            if (SysConst.EN_ITEM.Equals(piT.Name)) continue;

                            //判断属性名和属性类型是否相等，且可写
                            if (piT.Name == pi.Name && piT.PropertyType.Name == pi.PropertyType.Name && piT.CanWrite)
                            {
                                piT.SetValue(resultObj, pi.GetValue(paramModel, null), null);
                                break;
                            }
                        }
                    }
                }
                return resultObj;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(BussID, MethodBase.GetCurrentMethod().ToString(), ex.Message + SysConst.ENTER + ex.StackTrace, null, ex);
                return resultObj;
            }
        }
        /// <summary>
        /// 追加复制（根据属性名和类型，将[源Model]的值追加复制到[结果Model类型]的Model内）
        /// </summary>
        /// <typeparam name="D">结果Model类型</typeparam>
        /// <param name="paramModel">源Model</param>
        /// <param name="paramResultModel">结果Model</param>
        public void CopyModel(object paramModel, object paramResultModel)
        {
            try
            {
                //若结果Model为空则退出处理
                if (paramModel == null || paramResultModel == null) return;

                Type tp = paramModel.GetType();
                foreach (PropertyInfo pi in tp.GetProperties())
                {
                    //过滤索引器
                    if (SysConst.EN_ITEM.Equals(pi.Name)) continue;

                    if (pi.GetValue(paramModel, null) != null)
                    {
                        foreach (PropertyInfo piT in paramResultModel.GetType().GetProperties())
                        {
                            //过滤索引器
                            if (SysConst.EN_ITEM.Equals(piT.Name)) continue;

                            //判断属性名和属性类型是否相等，且可写
                            if (piT.Name == pi.Name && piT.PropertyType.Name == pi.PropertyType.Name && piT.CanWrite)
                            {
                                piT.SetValue(paramResultModel, pi.GetValue(paramModel, null), null);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(BussID, MethodBase.GetCurrentMethod().ToString(), ex.Message + SysConst.ENTER + ex.StackTrace, null, ex);
            }
        }

        /// <summary>
        /// 完全复制（根据属性名和类型，将[源Model]的值完全复制到[结果Model类型]的Model内）
        /// </summary>
        /// <param name="paramModel"></param>
        /// <param name="paramResultModel1"></param>
        /// <param name="paramResultModel2"></param>
        public void CopyModelFully(object paramModel, object paramResultModel1, object paramResultModel2)
        {
            try
            {
                //若结果Model为空则退出处理
                if (paramModel == null || paramResultModel1 == null || paramResultModel2 == null) return;

                Type tp = paramModel.GetType();
                foreach (PropertyInfo pi in tp.GetProperties())
                {
                    //过滤索引器
                    if (SysConst.EN_ITEM.Equals(pi.Name)) continue;

                    foreach (PropertyInfo piT1 in paramResultModel1.GetType().GetProperties())
                    {
                        //过滤索引器
                        if (SysConst.EN_ITEM.Equals(piT1.Name)) continue;

                        //判断属性名和属性类型是否相等，且可写
                        if (piT1.Name == pi.Name && piT1.PropertyType.Name == pi.PropertyType.Name && piT1.CanWrite)
                        {
                            piT1.SetValue(paramResultModel1, pi.GetValue(paramModel, null), null);
                            break;
                        }
                    }
                    foreach (PropertyInfo piT2 in paramResultModel2.GetType().GetProperties())
                    {
                        //过滤索引器
                        if (SysConst.EN_ITEM.Equals(piT2.Name)) continue;

                        //判断属性名和属性类型是否相等，且可写
                        if (piT2.Name == pi.Name && piT2.PropertyType.Name == pi.PropertyType.Name && piT2.CanWrite)
                        {
                            piT2.SetValue(paramResultModel2, pi.GetValue(paramModel, null), null);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ResultMsg = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                LogHelper.WriteErrorLog(BussID, MethodBase.GetCurrentMethod().ToString(), ex.Message + SysConst.ENTER + ex.StackTrace, null, ex);
            }
        }

        /// <summary>
        /// 属性复制（将[源Model]的制定属性复制到[结果Model类型]的Model内）
        /// </summary>
        /// <param name="paramModel">源Model</param>
        /// <param name="paramResultModel">结果Model</param>
        /// <param name="paramPropertyName"></param>
        public void CopyModelByPropertyName(object paramModel, object paramResultModel, string paramPropertyName)
        {
            try
            {
                //若结果Model为空则退出处理
                if (paramModel == null || paramResultModel == null) return;

                Type tp = paramModel.GetType();
                foreach (PropertyInfo pi in tp.GetProperties())
                {
                    //过滤索引器
                    if (SysConst.EN_ITEM.Equals(pi.Name) || pi.Name != paramPropertyName) continue;
                    bool hasCopyProperty = false;
                    foreach (PropertyInfo piT in paramResultModel.GetType().GetProperties())
                    {
                        //过滤索引器
                        if (SysConst.EN_ITEM.Equals(piT.Name) || piT.Name != paramPropertyName) continue;

                        //判断属性名和属性类型是否相等，且可写
                        if (piT.Name == pi.Name && piT.PropertyType.Name == pi.PropertyType.Name && piT.CanWrite)
                        {

                            piT.SetValue(paramResultModel, pi.GetValue(paramModel, null), null);
                            hasCopyProperty = true;
                            break;
                        }
                    }
                    if (hasCopyProperty)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ResultMsg = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                LogHelper.WriteErrorLog(BussID, MethodBase.GetCurrentMethod().ToString(), ex.Message + SysConst.ENTER + ex.StackTrace, null, ex);
            }
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 根据[TBModel对象]，判断是否需要新增
        /// </summary>
        /// <param name="paramTBModel">TBModel对象</param>
        /// <returns></returns>
        private bool IsInsert(object paramTBModel)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            Type t = paramTBModel.GetType();
            foreach (PropertyInfo pi in t.GetProperties())
            {
                string[] tmp = pi.Name.Split(new char[] { Convert.ToChar(SysConst.ULINE) });
                //找到主键(WHERE_XX_ID)，并判断其值是否为空
                if (tmp.Length >= 2 && SysConst.EN_WHERE.Equals(tmp[0]) && SysConst.EN_ID.Equals(tmp[tmp.Length - 1]) && pi.GetValue(paramTBModel, null) != null && pi.GetValue(paramTBModel, null).ToString() != string.Empty)
                {
                    LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "主键值非空（需更新）", "", null);
                    //主键值非空（需更新）
                    return false;
                }
            }
            LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "主键值为空（需新增）", "", null);
            //主键值为空（需新增）
            return true;
        }
        /// <summary>
        /// 判断[TBModel对象]的WHERE_XX_ID属性是否为空值
        /// </summary>
        /// <param name="paramTBModel">TBModel对象</param>
        /// <returns>true:空；false:非空</returns>
        private bool IsWhereIdEmpty(object paramTBModel)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            Type t = paramTBModel.GetType();
            foreach (PropertyInfo pi in t.GetProperties())
            {
                string[] tmp = pi.Name.Split(new char[] { Convert.ToChar(SysConst.ULINE) });
                //找到主键，并判断其值是否为空
                if (tmp.Length >= 2 && SysConst.EN_WHERE.Equals(tmp[0]) && SysConst.EN_ID.Equals(tmp[tmp.Length - 1]) && pi.GetValue(paramTBModel, null) != null)
                {
                    LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "WHERE主键值非空", "", null);
                    return false;
                }
            }
            LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "WHERE主键值为空", "", null);
            return true;
        }
        /// <summary>
        /// 判断[TBModel对象]的[WHERE_XX_ID]或[WHERE_XX_Version]属性是否为空值
        /// </summary>
        /// <param name="paramTBModel">TBModel对象</param>
        /// <returns>true:空；false:非空</returns>
        private bool IsWhereIdORVersionEmpty(object paramTBModel)
        {
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "", "", null);
            Type t = paramTBModel.GetType();
            var isIdEmpty = true;
            var isVersionNoEmpty = true;
            foreach (PropertyInfo pi in t.GetProperties())
            {
                string[] tmp = pi.Name.Split(new char[] { Convert.ToChar(SysConst.ULINE) });
                //找到主键，并判断其值是否为空
                if (tmp.Length >= 2 && SysConst.EN_WHERE.Equals(tmp[0]) && SysConst.EN_ID.Equals(tmp[tmp.Length - 1]) && pi.GetValue(paramTBModel, null) != null)
                {
                    LogHelper.WriteBussLog(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "WHERE主键值非空", "", null);
                    isIdEmpty = false;
                    continue;
                }
                //找到版本号，并判断其值是否为空
                if (tmp.Length >= 2 && SysConst.EN_WHERE.Equals(tmp[0]) && SysConst.EN_VERSION_NO.Equals(tmp[tmp.Length - 1]) && pi.GetValue(paramTBModel, null) != null)
                {
                    LogHelper.WriteBussLog(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "WHERE版本号值非空", "", null);
                    isVersionNoEmpty = false;
                    continue;
                }
            }
            if (isIdEmpty || isVersionNoEmpty)
            {
                LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "WHERE主键或者版本号的值为空", "", null);
                return true;
            }
            else
            {
                LogHelper.WriteBussLogEndOK(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(), "WHERE主键和版本号的值都已赋值", "", null);
                return false;
            }
        }
        #endregion
    }
}
