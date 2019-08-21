using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections;
using IBatisNet.DataMapper.Configuration;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.Common;
using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper.Exceptions;
using SkyCar.Coeus.Common.Const;
using SkyCar.Common;
using SkyCar.Common.Utility;


namespace SkyCar.Coeus.DAL
{
    /// <summary>
    /// 数据操作类2015-10-15
    /// </summary>
    public class DBManager
    {
        #region 常量定义
        private const string Sql_Excuting = "";
        private const string Sql_Excuted = "";
        //程序集名
        private const string AssemblyName = "SkyCar.Coeus.DAL";
        #endregion

        #region 变量定义

        public static Dictionary<string, ISqlMapper> dicSqlMap = new Dictionary<string, ISqlMapper>();

        #endregion

        #region DB初始化

        /// <summary>
        /// DB初始化（多数据库配置初始化）
        /// </summary>
        public static void DBInit(string paramDBConfig)
        {

            //如果数据库配置对象已经存在，则不再进行初始化
            if (dicSqlMap.ContainsKey(paramDBConfig)) return;

            #region 加载sqlmap.config配置文件

            XmlDocument xmlDoc = new XmlDocument();
            string xmlPath = System.AppDomain.CurrentDomain.BaseDirectory +
                             ConfigurationManager.AppSettings[AppSettingKey.SQLMAP_PATH];
            if (!File.Exists(xmlPath))
            {
                throw new Exception("Can not find the session config  xml file " + xmlPath);
            }
            xmlDoc.Load(xmlPath);

            if (null == xmlDoc)
                throw new Exception("sqlmap.config load faild");

            #endregion

            var builder = new DomSqlMapBuilder();
            try
            {
                //数据库配置SkyCar.Coeus
                if (DBCONFIG.Coeus.Equals(paramDBConfig))
                {
                    #region 解析AppSetting内配置的数据库连接字符串

                    var connectString = ConfigurationManager.AppSettings[AppSettingKey.CONNECTION_STRING];
                    try
                    {
                        var plainString = CryptoHelp.Decode(connectString);
                        if (!string.IsNullOrEmpty(plainString))
                        {
                            connectString = plainString;
                        }
                    }
                    catch
                    {
                    }
                    var dbInfos = connectString.Split(';');
                    if (dbInfos.Length != 4)
                    {
                        throw new Exception("数据库连接字符串配置错误");
                    }
                    #endregion

                    var newDBConfig = string.Format("{0}.Coeus.config,{0}", AssemblyName);
                    xmlDoc.ChildNodes[1].ChildNodes[1].Attributes[0].Value = newDBConfig;
                    var sqlMap = builder.Configure(xmlDoc);
                    sqlMap.DataSource.ConnectionString = string.Format(DALConst.ConnectionStr, dbInfos);
                    dicSqlMap[paramDBConfig] = sqlMap;
                }
                //数据库配置(其他数据库配置)
                else
                {
                    var newDBConfig = string.Format("{0}.Coeus.config,{0}", AssemblyName);
                    xmlDoc.ChildNodes[1].ChildNodes[1].Attributes[0].Value = newDBConfig;
                    var sqlMap = builder.Configure(xmlDoc);
                    sqlMap.DataSource.ConnectionString = DBCONFIG.Configs[paramDBConfig];
                    dicSqlMap[paramDBConfig] = sqlMap;
                }
            }
            catch (Exception e)
            {
                throw new DataMapperException("Error init DBConfig.  Cause: " + e.Message, e);
            }
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="paramDBConfigKey">数据库配置</param>
        /// <returns></returns>
        public static string GetConnectionString(string paramDBConfigKey)
        {
            string connStr = string.Empty;
            if (dicSqlMap.ContainsKey(paramDBConfigKey))
            {
                connStr = dicSqlMap[paramDBConfigKey].DataSource.ConnectionString;
            }
            return connStr;
        }
        #endregion

        #region beginTransaction
        /// <summary>
        /// 事务开始
        /// </summary>
        /// <returns></returns>
        public static IDalSession BeginTransaction(string paramDBConfig)
        {
            return dicSqlMap[paramDBConfig].BeginTransaction();
        }

        public static IDalSession BeginTransaction(string paramDBConfig, bool paramOpenConnection)
        {
            return dicSqlMap[paramDBConfig].BeginTransaction(paramOpenConnection);
        }

        public static IDalSession BeginTransaction(string paramDBConfig, string paramConnectionString)
        {
            return dicSqlMap[paramDBConfig].BeginTransaction(paramConnectionString);
        }

        public static IDalSession Transaction(string paramDBConfig, IsolationLevel paramIsolationLevel)
        {
            return dicSqlMap[paramDBConfig].BeginTransaction(paramIsolationLevel);
        }

        public static IDalSession BeginTransaction(string paramDBConfig, bool paramOpenNewConnection, IsolationLevel paramIsolationLevel)
        {
            return dicSqlMap[paramDBConfig].BeginTransaction(paramOpenNewConnection, paramIsolationLevel);
        }

        public static IDalSession BeginTransaction(string paramDBConfig, string paramConnectionString, IsolationLevel paramIsolationLevel)
        {
            return dicSqlMap[paramDBConfig].BeginTransaction(paramConnectionString, paramIsolationLevel);
        }

        public static IDalSession BeginTransaction(string paramDBConfig, string paramConnectionString, bool paramOpenNewConnection, IsolationLevel paramIsolationLevel)
        {
            return dicSqlMap[paramDBConfig].BeginTransaction(paramConnectionString, paramOpenNewConnection, paramIsolationLevel);
        }
        #endregion

        public static bool IsSessionStarted(string paramDBConfig)
        { return dicSqlMap[paramDBConfig].IsSessionStarted; }

        public static IDalSession LocalSession(string paramDBConfig)
        {
            if (dicSqlMap[paramDBConfig].LocalSession == null) dicSqlMap[paramDBConfig].OpenConnection();
            return dicSqlMap[paramDBConfig].LocalSession;
        }
        /// <summary>
        /// 检查数据库连接是否正常
        /// </summary>
        /// <param name="paramDBConfig">数据库配置参数(DAL.DBCONFIG)</param>
        /// <returns></returns>
        public static bool CheckConnectin(string paramDBConfig)
        {
            bool resultCode = false;

            try
            {
                //初始化数据库
                DBInit(paramDBConfig);
                //临时保存数据库连接字符串
                string tmpConnectionString = dicSqlMap[paramDBConfig].DataSource.ConnectionString;

                if (dicSqlMap[paramDBConfig].LocalSession == null)
                {
                    //修改数据库连接超时为1
                    dicSqlMap[paramDBConfig].DataSource.ConnectionString = tmpConnectionString.Substring(0, tmpConnectionString.LastIndexOf("connect timeout =") + 17) + 1;
                    dicSqlMap[paramDBConfig].OpenConnection();
                    dicSqlMap[paramDBConfig].LocalSession.Connection.Open();
                    if (dicSqlMap[paramDBConfig].LocalSession.Connection.State == ConnectionState.Open)
                    {
                        resultCode = true;
                        dicSqlMap[paramDBConfig].LocalSession.Connection.Close();
                    }
                    dicSqlMap[paramDBConfig].CloseConnection();
                    //恢复数据库连接字符串
                    dicSqlMap[paramDBConfig].DataSource.ConnectionString = tmpConnectionString;
                }
                else
                {
                    dicSqlMap[paramDBConfig].LocalSession.Connection.Open();
                    if (dicSqlMap[paramDBConfig].LocalSession.Connection.State == ConnectionState.Open)
                    {
                        resultCode = true;
                        dicSqlMap[paramDBConfig].LocalSession.Connection.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                resultCode = false;
            }
            return resultCode;
        }
        /// <summary>
        /// 打开连接
        /// </summary>
        /// <returns></returns>
        public static IDalSession OpenConnection(string paramDBConfig)
        {
            return dicSqlMap[paramDBConfig].OpenConnection();
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public static void CloseConnection(string paramDBConfig)
        {
            dicSqlMap[paramDBConfig].CloseConnection();
        }

        #region CommitTransaction
        /// <summary>
        /// 事务提交
        /// </summary>
        public static void CommitTransaction(string paramDBConfig)
        {
            dicSqlMap[paramDBConfig].CommitTransaction();
        }

        public static void CommitTransaction(string paramDBConfig, bool paramCloseConnection)
        {
            dicSqlMap[paramDBConfig].CommitTransaction(paramCloseConnection);
        }
        #endregion

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="paramStatementName">SqlID</param>
        /// <param name="parameterObject">参数Model对象</param>
        /// <returns></returns>
        public static int Delete(string paramDBConfig, string paramStatementName, object parameterObject)
        {
            try
            {
                return dicSqlMap[paramDBConfig].Delete(paramStatementName, parameterObject);
            }
            catch (Exception e)
            {

                throw new DataMapperException("Error executing query '" + paramStatementName + "' for delete.  Cause: " + e.Message, e);

            }
        }

        public static void FlushCaches(string paramDBConfig)
        {
            dicSqlMap[paramDBConfig].FlushCaches();
        }

        public static string GetDataCacheStats(string paramDBConfig)
        {
            return dicSqlMap[paramDBConfig].GetDataCacheStats();
        }

        public static IMappedStatement GetMappedStatement(string paramDBConfig, string paramId)
        {
            return dicSqlMap[paramDBConfig].GetMappedStatement(paramId);
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="paramStatementName">SqlID</param>
        /// <param name="parameterObject">参数Model对象</param>
        /// <returns></returns>
        public static object Insert(string paramDBConfig, string paramStatementName, object parameterObject)
        {
            try
            {
                return dicSqlMap[paramDBConfig].Insert(paramStatementName, parameterObject);
            }
            catch (Exception e)
            {

                throw new DataMapperException("Error executing query '" + paramStatementName + "' for insert.  Cause: " + e.Message, e);

            }
        }

        public static IDalSession OpenConnection(string paramDBConfig, string paramConnectionString)
        {
            return dicSqlMap[paramDBConfig].OpenConnection(paramConnectionString);
        }

        public static IDictionary QueryForDictionary(string paramDBConfig, string paramStatementName, object parameterObject, string paramKeyProperty)
        {
            try
            {
                return dicSqlMap[paramDBConfig].QueryForDictionary(paramStatementName, parameterObject, paramKeyProperty);
            }
            catch (Exception e)
            {

                throw new DataMapperException("Error executing query '" + paramStatementName + "' for dictionary.  Cause: " + e.Message, e);
            }
        }

        public static System.Collections.IDictionary QueryForDictionary(string paramDBConfig, string paramStatementName, object parameterObject, string paramKeyProperty, string valueProperty)
        {
            try
            {
                return dicSqlMap[paramDBConfig].QueryForDictionary(paramStatementName, parameterObject, paramKeyProperty, valueProperty);
            }
            catch (Exception e)
            {

                throw new DataMapperException("Error executing query '" + paramStatementName + "' for dictionary.  Cause: " + e.Message, e);

            }
        }

        #region QueryForList
        /// <summary>
        /// 查询（返回多条记录）
        /// </summary>
        /// <param name="paramStatementName">SqlID</param>
        /// <param name="parameterObject">参数Model对象</param>
        /// <returns>结果列表</returns>
        public static IList QueryForList(string paramDBConfig, string paramStatementName, object parameterObject)
        {
            try
            {
                return dicSqlMap[paramDBConfig].QueryForList(paramStatementName, parameterObject);
            }
            catch (Exception e)
            {

                throw new DataMapperException("Error executing query '" + paramStatementName + "' for list.  Cause: " + e.Message, e);

            }
        }
        /// <summary>
        /// 查询（返回多条记录）
        /// </summary>
        /// <param name="paramStatementName">SqlID</param>
        /// <param name="parameterObject">参数Model对象</param>
        /// <param name="paramResultObject">结果对象</param>
        public static void QueryForList(string paramDBConfig, string paramStatementName, object parameterObject, IList paramResultObject)
        {
            try
            {
                dicSqlMap[paramDBConfig].QueryForList(paramStatementName, parameterObject, paramResultObject);
            }
            catch (Exception e)
            {

                throw new DataMapperException("Error executing query '" + paramStatementName + "' for list.  Cause: " + e.Message, e);

            }
        }
        /// <summary>
        /// 查询（返回多条记录）
        /// </summary>
        /// <typeparam name="T">返回值Model类型</typeparam>
        /// <param name="paramStatementName">SqlID</param>
        /// <param name="parameterObject">参数Model对象</param>
        /// <param name="paramResultObject">返回值Model类型的结果对象</param>
        public static void QueryForList<T>(string paramDBConfig, string paramStatementName, object parameterObject, IList<T> paramResultObject)
        {
            try
            {
                dicSqlMap[paramDBConfig].QueryForList(paramStatementName, parameterObject, paramResultObject);
            }
            catch (Exception e)
            {

                throw new DataMapperException("Error executing query '" + paramStatementName + "' for list.  Cause: " + e.Message, e);

            }
        }
        /// <summary>
        /// 查询（返回多条记录）
        /// </summary>
        /// <param name="paramStatementName">SqlID</param>
        /// <param name="parameterObject">参数Model对象</param>
        /// <param name="paramResultObject">结果对象</param>
        /// <param name="paramSkipResults">跳过记录数</param>
        /// <param name="paramMaxResults">最大返回记录数</param>
        public static void QueryForList(string paramDBConfig, string paramStatementName, object parameterObject, IList paramResultObject, int paramSkipResults, int paramMaxResults)
        {
            try
            {
                dicSqlMap[paramDBConfig].QueryForList(paramStatementName, parameterObject, paramSkipResults, paramMaxResults);
            }
            catch (Exception e)
            {

                throw new DataMapperException("Error executing query '" + paramStatementName + "' for list.  Cause: " + e.Message, e);

            }
        }

        #endregion

        public static IDictionary QueryForMap(string paramDBConfig, string paramStatementName, object parameterObject, string paramKeyProperty)
        {
            try
            {
                return dicSqlMap[paramDBConfig].QueryForMap(paramStatementName, parameterObject, paramKeyProperty);
            }
            catch (Exception e)
            {

                throw new DataMapperException("Error executing query '" + paramStatementName + "' for map.  Cause: " + e.Message, e);

            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="paramStatementName">SqlID</param>
        /// <param name="parameterObject">参数Model对象</param>
        /// <returns></returns>
        public static object QueryForObject(string paramDBConfig, string paramStatementName, object parameterObject)
        {
            try
            {
                return dicSqlMap[paramDBConfig].QueryForObject(paramStatementName, parameterObject);
            }
            catch (Exception e)
            {

                throw new DataMapperException("Error executing query '" + paramStatementName + "' for object.  Cause: " + e.Message, e);

            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">返回值Model类型</typeparam>
        /// <param name="paramStatementName">SqlID</param>
        /// <param name="parameterObject">参数Model对象</param>
        /// <returns></returns>
        public static T QueryForObject<T>(string paramDBConfig, string paramStatementName, object parameterObject)
        {
            try
            {
                return dicSqlMap[paramDBConfig].QueryForObject<T>(paramStatementName, parameterObject);
            }
            catch (Exception e)
            {

                throw new DataMapperException("Error executing query '" + paramStatementName + "' for object.  Cause: " + e.Message, e);

            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">返回值Model类型</typeparam>
        /// <param name="paramStatementName">SqlID</param>
        /// <param name="parameterObject">参数Model对象</param>
        /// <param name="paramInstanceObject">返回值Model对象</param>
        /// <returns></returns>
        public static object QueryForObject<T>(string paramDBConfig, string paramStatementName, object parameterObject, T paramInstanceObject)
        {
            try
            {
                return dicSqlMap[paramDBConfig].QueryForObject<T>(paramStatementName, parameterObject, paramInstanceObject);
            }
            catch (Exception e)
            {

                throw new DataMapperException("Error executing query '" + paramStatementName + "' for object.  Cause: " + e.Message, e);

            }
        }
        ///// <summary>
        ///// 查询分页列表
        ///// </summary>
        ///// <param name="paramStatementName">SqlID</param>
        ///// <param name="parameterObject">参数Model对象</param>
        ///// <param name="pageSize">每页记录条数</param>
        ///// <returns></returns>
        //public static PaginatedList QueryForPaginatedList(string paramDBConfig, string paramStatementName, object parameterObject, int pageSize)
        //{
        //    try
        //    {
        //        return dicSqlMap[paramDBConfig].QueryForPaginatedList(paramStatementName, parameterObject, pageSize);
        //    }
        //    catch (Exception e)
        //    {

        //        throw new DataMapperException("Error executing query '" + paramStatementName + "' for paginated list.  Cause: " + e.Message, e);

        //    }

        //}

        /// <summary>
        /// 事务回滚
        /// </summary>
        public static void RollBackTransaction(string paramDBConfig)
        {
            dicSqlMap[paramDBConfig].RollBackTransaction();
        }

        public static void RollBackTransaction(string paramDBConfig, bool paramCloseConnection)
        {
            dicSqlMap[paramDBConfig].RollBackTransaction(paramCloseConnection);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="paramStatementName">SqlID</param>
        /// <param name="parameterObject">参数Model对象</param>
        /// <returns></returns>
        public static int Update(string paramDBConfig, string paramStatementName, object parameterObject)
        {
            try
            {
                return dicSqlMap[paramDBConfig].Update(paramStatementName, parameterObject);
            }
            catch (Exception e)
            {
                throw new DataMapperException("Error executing query '" + paramStatementName + "' for update.  Cause: " + e.Message, e);

            }
        }
    }

    /// <summary>
    /// 数据库配置
    /// </summary>
    public struct DBCONFIG
    {
        /// <summary>
        /// 数据库配置(其他数据库配置以外)
        /// <para>key:商户编码</para>
        /// <para>value:数据库连接字符串</para>
        /// </summary>
        public static Dictionary<string, string> Configs { get; set; }
        /// <summary>
        /// 数据库配置Coeus
        /// </summary>
        public const string Coeus = "Coeus";

    }
}
