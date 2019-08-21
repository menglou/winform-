using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 入库单Model
    /// </summary>
    public class MDLPIS_StockInBill
    {
        #region 公共属性
        /// <summary>
        /// 入库单ID
        /// </summary>
        public String SIB_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String SIB_Org_ID { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public String SIB_No { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String SIB_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String SIB_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String SIB_SourceNo { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String SIB_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String SIB_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String SIB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String SIB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String SIB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SIB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SIB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SIB_CreatedTime { get; set; }
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
        public String SIB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SIB_UpdatedTime { get; set; }
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
        public Int64? SIB_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String SIB_TransID { get; set; }
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
        /// 入库单ID
        /// </summary>
        public String WHERE_SIB_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_SIB_Org_ID { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public String WHERE_SIB_No { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String WHERE_SIB_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_SIB_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String WHERE_SIB_SourceNo { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String WHERE_SIB_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String WHERE_SIB_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String WHERE_SIB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_SIB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_SIB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_SIB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_SIB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_SIB_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_SIB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_SIB_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_SIB_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_SIB_TransID { get; set; }
        #endregion

    }
}
