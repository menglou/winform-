using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 编码规则Model
    /// </summary>
    public class MDLSM_EncodingRule
    {
        #region 公共属性
        /// <summary>
        /// 规则ID
        /// </summary>
        public String ER_ID { get; set; }
        /// <summary>
        /// 主编码
        /// </summary>
        public String ER_MasterCode { get; set; }
        /// <summary>
        /// 规则编码
        /// </summary>
        public String ER_Code { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        public String ER_Name { get; set; }
        /// <summary>
        /// 模块
        /// </summary>
        public String ER_Module { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public String ER_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? ER_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String ER_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? ER_CreatedTime { get; set; }
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
        public String ER_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ER_UpdatedTime { get; set; }
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
        public Int64? ER_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String ER_TransID { get; set; }
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
        /// 规则ID
        /// </summary>
        public String WHERE_ER_ID { get; set; }
        /// <summary>
        /// 主编码
        /// </summary>
        public String WHERE_ER_MasterCode { get; set; }
        /// <summary>
        /// 规则编码
        /// </summary>
        public String WHERE_ER_Code { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        public String WHERE_ER_Name { get; set; }
        /// <summary>
        /// 模块
        /// </summary>
        public String WHERE_ER_Module { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public String WHERE_ER_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_ER_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_ER_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_ER_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_ER_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_ER_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_ER_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_ER_TransID { get; set; }
        #endregion

    }
}
