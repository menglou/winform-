using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using SkyCar.Coeus.ComModel;

namespace SkyCar.Coeus.DAL
{
    public class CreateSQL
    {
        /// <summary>
        /// 根据Model自动创建SQL文的类,Model名必须跟数据库表名一致。
        /// </summary>
        public CreateSQL()
        {
        }

        #region public

        /// <summary>
        /// 创建InsertSQL
        /// </summary>
        /// <param name="model">Model实例（注：只根据已赋值的字段创建SQL文）</param>
        /// <returns>SQL文</returns>
        public string CreateSQLForInsert(object paramModel)
        {
            //表名
            string tableName = paramModel.GetType().Name.Substring(3);
            MDOperate mdOperate = new MDOperate();
            //取已赋值的属性作为参数,过滤掉翻页相关属性（RecordCount、PageIndex、PageSize）
            List<ComMDAttribute> param = mdOperate.GetPropertyListFormatValueFOPageProperty(paramModel);

            StringBuilder strB = new StringBuilder();
            StringBuilder strBCol = new StringBuilder();
            StringBuilder strBValue = new StringBuilder();

            foreach (ComMDAttribute attribute in param)
            {
                strBCol.AppendLine(attribute.Name + DALConst.COM_MA);
                strBValue.AppendLine(attribute.Value + DALConst.COM_MA);

                if (DALConst.VERSION_NO.Equals(attribute.Name.Substring(attribute.Name.IndexOf(DALConst.ULINE) + 1)))
                {
                    //版本号=1（返回给调用方法）
                    attribute.Value = 1;
                }
            }

            //去除最后一个逗号
            strBCol = strBCol.Remove(strBCol.Length - 3, 3);
            strBValue = strBValue.Remove(strBValue.Length - 3, 3);


            strB.AppendLine(DALConst.SQLKEY_INSERT_INTO);
            //表名
            strB.AppendLine(tableName);
            //(
            strB.AppendLine(DALConst.LEFT_PARENTHESIS);
            //列名
            strB.AppendLine(strBCol.ToString());
            //)
            strB.AppendLine(DALConst.RIGHT_PARENTHESIS);
            strB.AppendLine(DALConst.SQLKEY_VALUES);
            //(
            strB.AppendLine(DALConst.LEFT_PARENTHESIS);
            //列值
            strB.AppendLine(strBValue.ToString());
            //)
            strB.AppendLine(DALConst.RIGHT_PARENTHESIS);

            return strB.ToString();
        }

        /// <summary>
        /// 创建InsertSQL（含返回新插入的自增字段的值）
        /// </summary>
        /// <param name="model">Model实例（注：只根据已赋值的字段创建SQL文）</param>
        /// <returns>SQL文</returns>
        public string CreateSQLForInsertOutIdentity(object paramModel)
        {
            //表名
            string tableName = paramModel.GetType().Name.Substring(3);
            MDOperate mdOperate = new MDOperate();
            //取已赋值的属性作为参数，过滤掉翻页相关属性（RecordCount、PageIndex、PageSize）
            List<ComMDAttribute> param = mdOperate.GetPropertyListFormatValueFOPageProperty(paramModel);

            StringBuilder strB = new StringBuilder();
            StringBuilder strBCol = new StringBuilder();
            StringBuilder strBValue = new StringBuilder();

            foreach (ComMDAttribute ComMDAttribute in param)
            {
                strBCol.AppendLine(ComMDAttribute.Name + DALConst.COM_MA);
                strBValue.AppendLine(ComMDAttribute.Value + DALConst.COM_MA);
            }

            //去除最后一个逗号
            strBCol = strBCol.Remove(strBCol.Length - 3, 3);
            strBValue = strBValue.Remove(strBValue.Length - 3, 3);


            strB.AppendLine(DALConst.SQLKEY_INSERT_INTO);
            //表名
            strB.AppendLine(tableName);
            //(
            strB.AppendLine(DALConst.LEFT_PARENTHESIS);
            //列名
            strB.AppendLine(strBCol.ToString());
            //)
            strB.AppendLine(DALConst.RIGHT_PARENTHESIS);
            strB.AppendLine(DALConst.SQLKEY_VALUES);
            //(
            strB.AppendLine(DALConst.LEFT_PARENTHESIS);
            //列值
            strB.AppendLine(strBValue.ToString());
            //)
            strB.AppendLine(DALConst.RIGHT_PARENTHESIS);
            //SELECT @@IDENTITY
            strB.AppendLine(DALConst.SQLKEY_SELECT_IDENTITY);
            return strB.ToString();
        }

        /// <summary>
        /// 创建UpdateSQL，给定ID号
        /// </summary>
        /// <param name="model">Model实例（注：只根据已赋值的字段创建SQL文）</param>
        /// <param name="id">ID</param>
        /// <returns>SQL文</returns>
        public string CreateSQLForUpdateByInt64ID(object paramModel, Int64 paramId)
        {
            MDOperate mo = new MDOperate();

            string shortName = DALConst.SPACE + mo.GetPKName(paramModel) + DALConst.EQUAL;

            string condition = DALConst.SQLKEY_WHERE + shortName + paramId.ToString();
            return CreateSQLForUpdateByCondition(paramModel, condition);
        }
        /// <summary>
        /// 创建UpdateSQL，给定ID号
        /// </summary>
        /// <param name="model">Model实例（注：只根据已赋值的字段创建SQL文）</param>
        /// <param name="id">ID</param>
        /// <returns>SQL文</returns>
        public string CreateSQLForUpdateByStrID(object paramModel, string paramId)
        {
            MDOperate mo = new MDOperate();

            string shortName = DALConst.SPACE + mo.GetPKName(paramModel) + DALConst.EQUAL;

            string condition = DALConst.SQLKEY_WHERE + shortName + DALConst.SING_LEQUOTES + paramId.ToString() + DALConst.SING_LEQUOTES;
            return CreateSQLForUpdateByCondition(paramModel, condition);
        }

        /// <summary>
        /// 创建UpdateSQL，给定WHERE条件语句
        /// </summary>
        /// <param name="model">Model实例（注：只根据已赋值的字段创建SQL文）</param>
        /// <param name="condition">条件(例："WHERE ID=1")</param>
        /// <returns>SQL文</returns>
        public string CreateSQLForUpdateByCondition(object paramModel, string paramCondition)
        {
            //表名
            string tableName = paramModel.GetType().Name.Substring(3);
            MDOperate mdOperate = new MDOperate();
            //取已赋值的属性作为参数，过滤掉翻页相关属性（RecordCount、PageIndex、PageSize）
            List<ComMDAttribute> param = mdOperate.GetPropertyListFormatValueFOPageProperty(paramModel);

            StringBuilder strB = new StringBuilder();
            StringBuilder strBCol = new StringBuilder();
            string id = mdOperate.GetPKName(paramModel);
            foreach (ComMDAttribute ComMDAttribute in param)
            {
                if (!id.Equals(ComMDAttribute.Name))
                {
                    strBCol.AppendLine(ComMDAttribute.Name + DALConst.EQUAL + ComMDAttribute.Value + DALConst.COM_MA);
                }
                //strBCol.AppendLine(ComMDAttribute.Name + SysConst.EQUAL + ComMDAttribute.Value + SysConst.ComMA);
            }

            //去除最后一个逗号
            strBCol = strBCol.Remove(strBCol.Length - 3, 3);

            strB.AppendLine(DALConst.SQLKEY_UPDATE);
            //表名
            strB.AppendLine(tableName);
            //SET
            strB.AppendLine(DALConst.SQLKEY_SET);
            //列名
            strB.AppendLine(strBCol.ToString());
            //条件
            strB.AppendLine(paramCondition);

            return strB.ToString();
        }
        /// <summary>
        /// 根据TBModel内已赋值的ID和版本号，创建UpdateSQL
        /// </summary>
        /// <param name="model">Model实例（注：只根据已赋值的字段创建SQL文）</param>
        /// <returns>SQL文</returns>
        public string CreateSQLForUpdateByIDAndVersionNo(object paramModel)
        {
            //表名
            string tableName = paramModel.GetType().Name.Substring(3);
            MDOperate mdOperate = new MDOperate();
            //取已赋值的属性作为参数，过滤掉翻页相关属性（RecordCount、PageIndex、PageSize）
            List<ComMDAttribute> param = mdOperate.GetPropertyListFormatValueFOPageProperty(paramModel);

            StringBuilder strB = new StringBuilder();
            StringBuilder strBCol = new StringBuilder();
            StringBuilder strCondition = new StringBuilder();

            string id = mdOperate.GetPKName(paramModel);
            foreach (ComMDAttribute attribute in param)
            {
                if (!id.Equals(attribute.Name)
                    && !DALConst.VERSION_NO.Equals(attribute.Name.Substring(attribute.Name.IndexOf(DALConst.ULINE) + 1)))
                {
                    strBCol.AppendLine(attribute.Name + DALConst.EQUAL + attribute.Value + DALConst.COM_MA);
                }
                else if (id.Equals(attribute.Name))
                {
                    #region ID
                    if (!strCondition.ToString().Contains(DALConst.SQLKEY_WHERE))
                    {
                        strCondition.Append(DALConst.SQLKEY_WHERE + DALConst.SPACE + id + DALConst.EQUAL + attribute.Value);
                    }
                    else
                    {
                        strCondition.Append(DALConst.SQLKEY_AND + id + DALConst.EQUAL + attribute.Value);
                    }
                    #endregion
                }
                else if (DALConst.VERSION_NO.Equals(attribute.Name.Substring(attribute.Name.IndexOf(DALConst.ULINE) + 1)))
                {
                    #region VersionNo
                    //版本号为空的场合
                    if (attribute.Value == null)
                    {
                        //版本号=版本号+1
                        strBCol.AppendLine(attribute.Name + DALConst.EQUAL + attribute.Name + DALConst.INT_TYPE_FLG + DALConst.COM_MA);
                    }
                    //版本号不为空的场合
                    else
                    {
                        //版本号=当前值+1
                        strBCol.AppendLine(attribute.Name + DALConst.EQUAL + Convert.ToString(Convert.ToInt64(attribute.Value) + 1) + DALConst.COM_MA);

                        if (!strCondition.ToString().Contains(DALConst.SQLKEY_WHERE))
                        {
                            strCondition.Append(DALConst.SQLKEY_WHERE + DALConst.SPACE + attribute.Name + DALConst.EQUAL + attribute.Value);
                        }
                        else
                        {
                            strCondition.Append(DALConst.SQLKEY_AND + attribute.Name + DALConst.EQUAL + attribute.Value);
                        }
                        //版本号+1（返回给调用方法）
                        attribute.Value = Convert.ToInt64(attribute.Value) + 1; 
                    }
                    #endregion
                }

            }

            //去除最后一个逗号
            strBCol = strBCol.Remove(strBCol.Length - 3, 3);

            strB.AppendLine(DALConst.SQLKEY_UPDATE);
            //表名
            strB.AppendLine(tableName);
            //SET
            strB.AppendLine(DALConst.SQLKEY_SET);
            //列名
            strB.AppendLine(strBCol.ToString());
            //条件
            strB.AppendLine(strCondition.ToString());

            return strB.ToString();
        }
        /// <summary>
        /// 根据TBModel内已赋值的ID和版本号，创建DeleteSQL
        /// </summary>
        /// <param name="model">Model实例</param>
        /// <returns>SQL文</returns>
        public string CreateSQLForDeleteByIDAndVersionNo(object paramModel)
        {
            //表名
            string tableName = paramModel.GetType().Name.Substring(3);
            MDOperate mdOperate = new MDOperate();
            //取已赋值的属性作为参数，过滤掉翻页相关属性（RecordCount、PageIndex、PageSize）
            List<ComMDAttribute> param = mdOperate.GetPropertyListFormatValueFOPageProperty(paramModel);

            StringBuilder strCondition = new StringBuilder();

            string id = mdOperate.GetPKName(paramModel);
            foreach (ComMDAttribute comDAttribute in param)
            {
                if (id.Equals(comDAttribute.Name))
                {
                    #region ID
                    if (!strCondition.ToString().Contains(DALConst.SQLKEY_WHERE))
                    {
                        strCondition.Append(DALConst.SQLKEY_WHERE + DALConst.SPACE + id + DALConst.EQUAL + comDAttribute.Value);
                    }
                    else
                    {
                        strCondition.Append(DALConst.SQLKEY_AND + id + DALConst.EQUAL + comDAttribute.Value);
                    }
                    #endregion
                }
                else if (DALConst.VERSION_NO.Equals(comDAttribute.Name.Substring(comDAttribute.Name.IndexOf(DALConst.ULINE) + 1)))
                {
                    #region VersionNo

                    if (!strCondition.ToString().Contains(DALConst.SQLKEY_WHERE))
                    {
                        strCondition.Append(DALConst.SQLKEY_WHERE + DALConst.SPACE + comDAttribute.Name + DALConst.EQUAL + comDAttribute.Value);
                    }
                    else
                    {
                        strCondition.Append(DALConst.SQLKEY_AND + comDAttribute.Name + DALConst.EQUAL + comDAttribute.Value);
                    }
                    #endregion
                }
            }

            StringBuilder strB = new StringBuilder();

            strB.AppendLine(DALConst.SQLKEY_DELETE + DALConst.SQLKEY_FROM);
            //表名
            strB.AppendLine(tableName);
            //条件
            strB.AppendLine(strCondition.ToString());

            return strB.ToString();
        }
        /// <summary>
        /// 创建DeleteSQL，给定ID号
        /// </summary>
        /// <param name="model">Model实例</param>
        /// <param name="id">ID</param>
        /// <returns>SQL文</returns>
        public string CreateSQLForDeleteByStrID(object paramModel, string paramId)
        {
            MDOperate mo = new MDOperate();

            string shortName = DALConst.SPACE + mo.GetPKName(paramModel) + DALConst.EQUAL;

            string condition = DALConst.SQLKEY_WHERE + shortName + DALConst.SING_LEQUOTES + paramId.ToString() + DALConst.SING_LEQUOTES;

            return CreateSQLForDeleteByCondition(paramModel, condition);
        }
        /// <summary>
        /// 创建DeleteSQL，给定ID号
        /// </summary>
        /// <param name="model">Model实例</param>
        /// <param name="id">ID</param>
        /// <returns>SQL文</returns>
        public string CreateSQLForDeleteByInt64ID(object paramModel, Int64 paramId)
        {
            MDOperate mo = new MDOperate();

            string shortName = DALConst.SPACE + mo.GetPKName(paramModel) + DALConst.EQUAL;

            string condition = DALConst.SQLKEY_WHERE + shortName + paramId.ToString();

            return CreateSQLForDeleteByCondition(paramModel, condition);
        }
        /// <summary>
        /// 创建DeleteSQL，给定WHERE条件语句
        /// </summary>
        /// <param name="model">Model实例</param>
        /// <param name="condition">条件(例："WHERE ID=1")</param>
        /// <returns>SQL文</returns>
        public string CreateSQLForDeleteByCondition(object paramModel, string paramCondition)
        {
            //表名
            string tableName = paramModel.GetType().Name.Substring(3);

            StringBuilder strB = new StringBuilder();

            strB.AppendLine(DALConst.SQLKEY_DELETE + DALConst.SQLKEY_FROM);
            //表名
            strB.AppendLine(tableName);
            //条件
            strB.AppendLine(paramCondition);

            return strB.ToString();
        }
        #endregion


    }
}
