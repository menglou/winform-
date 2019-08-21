using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 采购订单Model
    /// </summary>
    public class MDLPIS_PurchaseOrder
    {
        #region 公共属性
        /// <summary>
        /// 采购订单ID
        /// </summary>
        public String PO_ID { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public String PO_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String PO_Org_ID { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String PO_SUPP_ID { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String PO_SUPP_Name { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String PO_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String PO_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String PO_SourceNo { get; set; }
        /// <summary>
        /// 订单总额
        /// </summary>
        public Decimal? PO_TotalAmount { get; set; }
        /// <summary>
        /// 物流费
        /// </summary>
        public Decimal? PO_LogisticFee { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String PO_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String PO_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String PO_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String PO_ApprovalStatusName { get; set; }
        /// <summary>
        /// 到货时间
        /// </summary>
        public DateTime? PO_ReceivedTime { get; set; }
        /// <summary>
        /// 到货时间-开始（查询条件用）
        /// </summary>
        public DateTime? _ReceivedTimeStart { get; set; }
        /// <summary>
        /// 到货时间-终了（查询条件用）
        /// </summary>
        public DateTime? _ReceivedTimeEnd { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? PO_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String PO_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? PO_CreatedTime { get; set; }
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
        public String PO_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? PO_UpdatedTime { get; set; }
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
        public Int64? PO_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String PO_TransID { get; set; }
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
        /// 采购订单ID
        /// </summary>
        public String WHERE_PO_ID { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public String WHERE_PO_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_PO_Org_ID { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String WHERE_PO_SUPP_ID { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String WHERE_PO_SUPP_Name { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String WHERE_PO_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_PO_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String WHERE_PO_SourceNo { get; set; }
        /// <summary>
        /// 订单总额
        /// </summary>
        public Decimal? WHERE_PO_TotalAmount { get; set; }
        /// <summary>
        /// 物流费
        /// </summary>
        public Decimal? WHERE_PO_LogisticFee { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String WHERE_PO_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String WHERE_PO_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String WHERE_PO_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_PO_ApprovalStatusName { get; set; }
        /// <summary>
        /// 到货时间
        /// </summary>
        public DateTime? WHERE_PO_ReceivedTime { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_PO_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_PO_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_PO_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_PO_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_PO_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_PO_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_PO_TransID { get; set; }
        #endregion

    }
}
