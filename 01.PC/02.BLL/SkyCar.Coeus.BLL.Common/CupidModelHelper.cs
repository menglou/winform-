using System;
using System.Collections.Generic;
using System.Reflection;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Log;

namespace SkyCar.Coeus.BLL.Common
{
    public class CupidModelHelper
    {
        static string newTempStr = string.Empty;
        static string oldTempStr = string.Empty;

        /// <summary>
        /// 检查Model属性值是否发生变化
        /// </summary>
        /// <param name="paramBussId"></param>
        /// <param name="paramNewModel">新Model</param>
        /// <param name="paramOrginalModel">源Model</param>
        /// <param name="paramSkipProperties">需要跳过的属性（不予检查）</param>
        /// <returns></returns>
        public static bool ModelHasChanged(string paramBussId, object paramNewModel, object paramOrginalModel, List<string> paramSkipProperties = null)
        {
            try
            {
                if (paramNewModel == null && paramOrginalModel != null
                    || paramNewModel != null && paramOrginalModel == null)
                {
                    return true;
                }
                else if (paramNewModel == null && paramOrginalModel == null)
                {
                    return false;
                }

                bool propertyHasChanged = false;
                Type modelType = paramNewModel.GetType();

                foreach (PropertyInfo newPropInfo in modelType.GetProperties())
                {
                    //过滤索引器
                    if (SysConst.EN_ITEM.Equals(newPropInfo.Name))
                    {
                        continue;
                    }

                    if (newPropInfo.Name.Contains("_CreatedBy")
                        || newPropInfo.Name.Contains("_CreatedTime")
                        || newPropInfo.Name.Contains("_UpdatedBy")
                        || newPropInfo.Name.Contains("_UpdatedTime")
                        || newPropInfo.Name.Contains("RecordCount"))
                    {
                        continue;
                    }

                    //过滤不予检查的属性
                    if (paramSkipProperties != null && paramSkipProperties.Contains(newPropInfo.Name))
                    {
                        continue;
                    }

                    foreach (PropertyInfo orgPropInfo in paramOrginalModel.GetType().GetProperties())
                    {
                        //过滤索引器
                        if (SysConst.EN_ITEM.Equals(orgPropInfo.Name))
                        {
                            continue;
                        }

                        if (orgPropInfo.Name.Contains("_CreatedBy")
                            || orgPropInfo.Name.Contains("_CreatedTime")
                            || orgPropInfo.Name.Contains("_UpdatedBy")
                            || orgPropInfo.Name.Contains("_UpdatedTime")
                            || orgPropInfo.Name.Contains("RecordCount"))
                        {
                            continue;
                        }
                        //判断属性名和属性类型是否相等，且可写
                        if (orgPropInfo.Name != "PropertyValueChanged"
                            && orgPropInfo.Name == newPropInfo.Name
                            && orgPropInfo.PropertyType.Name == newPropInfo.PropertyType.Name && newPropInfo.CanWrite)
                        {

                            //当前值不为null且不为空,初始值为null或空
                            if (newPropInfo.GetValue(paramNewModel, null) != null
                                && newPropInfo.GetValue(paramNewModel, null).ToString() != string.Empty
                                && newPropInfo.GetValue(paramNewModel, null).ToString() != "0"
                                && (orgPropInfo.GetValue(paramOrginalModel, null) == null || orgPropInfo.GetValue(paramOrginalModel, null).ToString() == string.Empty || orgPropInfo.GetValue(paramOrginalModel, null).ToString() == "0")

                                //当前值为空或null,初始值不为null且不为空
                                || (newPropInfo.GetValue(paramNewModel, null) == null || newPropInfo.GetValue(paramNewModel, null).ToString() == string.Empty || newPropInfo.GetValue(paramNewModel, null).ToString() == "0")
                                && orgPropInfo.GetValue(paramOrginalModel, null) != null
                                && orgPropInfo.GetValue(paramOrginalModel, null).ToString() != string.Empty
                                && orgPropInfo.GetValue(paramOrginalModel, null).ToString() != "0"

                                //当前值不为null且不为空,初始不为null且不为空，且两者不相等
                                || newPropInfo.GetValue(paramNewModel, null) != null
                                && newPropInfo.GetValue(paramNewModel, null).ToString() != string.Empty
                                && newPropInfo.GetValue(paramNewModel, null).ToString() != "0"
                                && orgPropInfo.GetValue(paramOrginalModel, null) != null
                                && orgPropInfo.GetValue(paramOrginalModel, null).ToString() != string.Empty
                                && orgPropInfo.GetValue(paramOrginalModel, null).ToString() != "0"
                                && !newPropInfo.GetValue(paramNewModel, null).Equals(orgPropInfo.GetValue(paramOrginalModel, null))
                                )
                            {
                                //富文本控件当有图片进行的处理
                                object newTempValue = newPropInfo.GetValue(paramNewModel, null);
                                object oldTempValue = orgPropInfo.GetValue(paramOrginalModel, null);
                                newTempStr = newTempValue != null ? newTempValue.ToString() : string.Empty;
                                oldTempStr = oldTempValue != null ? oldTempValue.ToString() : string.Empty;
                                newTempStr = newTempStr.Replace("amp;", "");
                                if (!newTempStr.Equals(oldTempStr))
                                {
                                    propertyHasChanged = true;
                                    break;
                                }
                            }
                        }
                    }
                    //发现属性值发生变化，结束循环
                    if (propertyHasChanged)
                    {
                        break;
                    }
                }
                return propertyHasChanged;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(paramBussId, MethodBase.GetCurrentMethod().ToString(), ex.Message + SysConst.ENTER + ex.StackTrace, null, ex);
                ExceptionBLL.WriteLogFileAndEmail(ex);
                return false;
            }
        }
    }
}
