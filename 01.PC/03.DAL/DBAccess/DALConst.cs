using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.DAL
{
    public class DALConst
    {

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionStr = "data source={0};database={1};user id={2};password={3};pooling=true;min pool size=5;max pool size=512;connect timeout = 300;";
        /// <summary>
        /// "_"
        /// </summary>
        public const string ULINE = "_";
        /// <summary>
        /// ","
        /// </summary>
        public const string COM_MA = ",";
        /// <summary>
        /// 单引号
        /// </summary>
        public const string SING_LEQUOTES = "'";
        /// <summary>
        /// 一个空格
        /// </summary>
        public const string SPACE = " ";
        /// <summary>
        /// "("
        /// </summary>
        public const string LEFT_PARENTHESIS = "(";
        /// <summary>
        /// "="
        /// </summary>
        public const string EQUAL = "=";
        /// <summary>
        /// ")"
        /// </summary>
        public const string RIGHT_PARENTHESIS = ")";

        /// <summary>
        /// 版本号VersionNo
        /// </summary>
        public const string VERSION_NO = "VersionNo";
        /// <summary>
        /// TBModel
        /// </summary>
        public const string TBModel = "TBModel";
        /// <summary>
        /// "ID"
        /// </summary>
        public const string ID = "ID";
 
        #region SQL关键字

        /// <summary>
        /// "SELECT @@IDENTITY"
        /// </summary>
        public const string SQLKEY_SELECT_IDENTITY = "SELECT @@IDENTITY";
        /// <summary>
        /// "INSERT INTO "
        /// </summary>
        public const string SQLKEY_INSERT_INTO = "INSERT INTO ";
        /// <summary>
        /// "UPDATE "
        /// </summary>
        public const string SQLKEY_UPDATE = "UPDATE ";
        /// <summary>
        /// "DELETE "
        /// </summary>
        public const string SQLKEY_DELETE = "DELETE ";
        /// <summary>
        /// "FROM "
        /// </summary>
        public const string SQLKEY_FROM = "FROM ";
        /// <summary>
        /// "VALUES"
        /// </summary>
        public const string SQLKEY_VALUES = "VALUES";
        /// <summary>
        /// "SET"
        /// </summary>
        public const string SQLKEY_SET = "SET";
        /// <summary>
        /// "WHERE"
        /// </summary>
        public const string SQLKEY_WHERE = "WHERE";
        /// <summary>
        /// "AND"
        /// </summary>
        public const string SQLKEY_AND = " AND ";
        #endregion

        #region 数字

        /// <summary>
        /// 1
        /// </summary>
        public const string INT_TYPE_FLG = "1";
        /// <summary>
        /// -1
        /// </summary>
        public const int MINUS_1 = -1;

        #endregion

    }
}
