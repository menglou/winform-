using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 系统编号Model
    /// </summary>
    public class MDLSM_SystemNo
    {
        #region 公共属性
        /// <summary>
        /// 系统编号ID
        /// </summary>
        public String SN_ID { get; set; }
        /// <summary>
        /// 规则ID
        /// </summary>
        public String SN_ER_ID { get; set; }
        /// <summary>
        /// 编码值
        /// </summary>
        public String SN_Value { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public String SN_Status { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SN_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SN_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SN_CreatedTime { get; set; }
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
        public String SN_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SN_UpdatedTime { get; set; }
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
        public Int64? SN_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String SN_TransID { get; set; }
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
        /// 系统编号ID
        /// </summary>
        public String WHERE_SN_ID { get; set; }
        /// <summary>
        /// 规则ID
        /// </summary>
        public String WHERE_SN_ER_ID { get; set; }
        /// <summary>
        /// 编码值
        /// </summary>
        public String WHERE_SN_Value { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public String WHERE_SN_Status { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_SN_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_SN_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_SN_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_SN_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_SN_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_SN_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_SN_TransID { get; set; }
        #endregion

    }
}
