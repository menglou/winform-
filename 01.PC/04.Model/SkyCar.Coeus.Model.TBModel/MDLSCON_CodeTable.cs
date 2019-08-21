using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 码表Model
    /// </summary>
    public class MDLSCON_CodeTable
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String CT_ID { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public String CT_Type { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public String CT_Name { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public String CT_Value { get; set; }
        /// <summary>
        /// 参数描述
        /// </summary>
        public String CT_Desc { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? CT_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String CT_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CT_CreatedTime { get; set; }
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
        public String CT_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? CT_UpdatedTime { get; set; }
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
        public Int64? CT_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String CT_TransID { get; set; }
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
        /// ID
        /// </summary>
        public String WHERE_CT_ID { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public String WHERE_CT_Type { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public String WHERE_CT_Name { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public String WHERE_CT_Value { get; set; }
        /// <summary>
        /// 参数描述
        /// </summary>
        public String WHERE_CT_Desc { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_CT_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_CT_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_CT_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_CT_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_CT_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_CT_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_CT_TransID { get; set; }
        #endregion

    }
}
