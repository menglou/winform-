using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 系统参数Model
    /// </summary>
    public class MDLSM_Parameter
    {
        #region 公共属性
        /// <summary>
        /// 参数ID
        /// </summary>
        public String Para_ID { get; set; }
        /// <summary>
        /// 参数编码1
        /// </summary>
        public String Para_Code1 { get; set; }
        /// <summary>
        /// 参数描述1
        /// </summary>
        public String Para_Name1 { get; set; }
        /// <summary>
        /// 参数值1
        /// </summary>
        public String Para_Value1 { get; set; }
        /// <summary>
        /// 参数编码2
        /// </summary>
        public String Para_Code2 { get; set; }
        /// <summary>
        /// 参数描述2
        /// </summary>
        public String Para_Name2 { get; set; }
        /// <summary>
        /// 参数值2
        /// </summary>
        public String Para_Value2 { get; set; }
        /// <summary>
        /// 参数编码3
        /// </summary>
        public String Para_Code3 { get; set; }
        /// <summary>
        /// 参数描述3
        /// </summary>
        public String Para_Name3 { get; set; }
        /// <summary>
        /// 参数值3
        /// </summary>
        public String Para_Value3 { get; set; }
        /// <summary>
        /// 参数编码4
        /// </summary>
        public String Para_Code4 { get; set; }
        /// <summary>
        /// 参数描述4
        /// </summary>
        public String Para_Name4 { get; set; }
        /// <summary>
        /// 参数值4
        /// </summary>
        public String Para_Value4 { get; set; }
        /// <summary>
        /// 参数编码5
        /// </summary>
        public String Para_Code5 { get; set; }
        /// <summary>
        /// 参数描述5
        /// </summary>
        public String Para_Name5 { get; set; }
        /// <summary>
        /// 参数值5
        /// </summary>
        public String Para_Value5 { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? Para_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String Para_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Para_CreatedTime { get; set; }
        /// <summary>
        /// 创建时间-开始（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeStart { get; set; }
        /// <summary>
        /// 创建时间-终了（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeEnd { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String Para_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Para_UpdatedTime { get; set; }
        /// <summary>
        /// 修改时间-开始（查询条件用）
        /// </summary>
        public DateTime? _UpdatedTimeStart { get; set; }
        /// <summary>
        /// 修改时间-终了（查询条件用）
        /// </summary>
        public DateTime? _UpdatedTimeEnd { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? Para_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String Para_TransID { get; set; }
        /// <summary>
        /// 总记录条数（翻页用）
        /// </summary>
        public Int32? RecordCount { get; set; }
        /// <summary>
        /// 当前页面索引/要跳转的页码（翻页用）
        /// </summary>
        public Int32? PageIndex { get; set; }
        /// <summary>
        /// 页面大小（翻页用）
        /// </summary>
        public Int32? PageSize { get; set; }
        #endregion

        #region 公共属性-条件用
        /// <summary>
        /// 参数ID
        /// </summary>
        public String WHERE_Para_ID { get; set; }
        /// <summary>
        /// 参数编码1
        /// </summary>
        public String WHERE_Para_Code1 { get; set; }
        /// <summary>
        /// 参数描述1
        /// </summary>
        public String WHERE_Para_Name1 { get; set; }
        /// <summary>
        /// 参数值1
        /// </summary>
        public String WHERE_Para_Value1 { get; set; }
        /// <summary>
        /// 参数编码2
        /// </summary>
        public String WHERE_Para_Code2 { get; set; }
        /// <summary>
        /// 参数描述2
        /// </summary>
        public String WHERE_Para_Name2 { get; set; }
        /// <summary>
        /// 参数值2
        /// </summary>
        public String WHERE_Para_Value2 { get; set; }
        /// <summary>
        /// 参数编码3
        /// </summary>
        public String WHERE_Para_Code3 { get; set; }
        /// <summary>
        /// 参数描述3
        /// </summary>
        public String WHERE_Para_Name3 { get; set; }
        /// <summary>
        /// 参数值3
        /// </summary>
        public String WHERE_Para_Value3 { get; set; }
        /// <summary>
        /// 参数编码4
        /// </summary>
        public String WHERE_Para_Code4 { get; set; }
        /// <summary>
        /// 参数描述4
        /// </summary>
        public String WHERE_Para_Name4 { get; set; }
        /// <summary>
        /// 参数值4
        /// </summary>
        public String WHERE_Para_Value4 { get; set; }
        /// <summary>
        /// 参数编码5
        /// </summary>
        public String WHERE_Para_Code5 { get; set; }
        /// <summary>
        /// 参数描述5
        /// </summary>
        public String WHERE_Para_Name5 { get; set; }
        /// <summary>
        /// 参数值5
        /// </summary>
        public String WHERE_Para_Value5 { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_Para_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_Para_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_Para_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_Para_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_Para_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_Para_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_Para_TransID { get; set; }
        #endregion

    }
}
