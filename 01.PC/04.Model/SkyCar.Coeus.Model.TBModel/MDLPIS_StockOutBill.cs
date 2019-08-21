using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 出库单Model
    /// </summary>
    public class MDLPIS_StockOutBill
    {
        #region 公共属性
        /// <summary>
        /// 出库单ID
        /// </summary>
        public String SOB_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String SOB_Org_ID { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public String SOB_No { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String SOB_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String SOB_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String SOB_SourceNo { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String SOB_SUPP_ID { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String SOB_SUPP_Name { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String SOB_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String SOB_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String SOB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String SOB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String SOB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SOB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SOB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SOB_CreatedTime { get; set; }
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
        public String SOB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SOB_UpdatedTime { get; set; }
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
        public Int64? SOB_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String SOB_TransID { get; set; }
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
        /// 出库单ID
        /// </summary>
        public String WHERE_SOB_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_SOB_Org_ID { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public String WHERE_SOB_No { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String WHERE_SOB_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_SOB_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String WHERE_SOB_SourceNo { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String WHERE_SOB_SUPP_ID { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String WHERE_SOB_SUPP_Name { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String WHERE_SOB_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String WHERE_SOB_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String WHERE_SOB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_SOB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_SOB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_SOB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_SOB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_SOB_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_SOB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_SOB_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_SOB_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_SOB_TransID { get; set; }
        #endregion

    }
}
