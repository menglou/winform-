using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 应收单Model
    /// </summary>
    public class MDLFM_AccountReceivableBill
    {
        #region 公共属性
        /// <summary>
        /// 应收单ID
        /// </summary>
        public String ARB_ID { get; set; }
        /// <summary>
        /// 应收单号
        /// </summary>
        public String ARB_No { get; set; }
        /// <summary>
        /// 单据方向编码
        /// </summary>
        public String ARB_BillDirectCode { get; set; }
        /// <summary>
        /// 单据方向名称
        /// </summary>
        public String ARB_BillDirectName { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String ARB_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String ARB_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单据号
        /// </summary>
        public String ARB_SrcBillNo { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String ARB_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String ARB_Org_Name { get; set; }
        /// <summary>
        /// 付款对象类型编码
        /// </summary>
        public String ARB_PayObjectTypeCode { get; set; }
        /// <summary>
        /// 付款对象类型名称
        /// </summary>
        public String ARB_PayObjectTypeName { get; set; }
        /// <summary>
        /// 付款对象ID
        /// </summary>
        public String ARB_PayObjectID { get; set; }
        /// <summary>
        /// 付款对象名称
        /// </summary>
        public String ARB_PayObjectName { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public Decimal? ARB_AccountReceivableAmount { get; set; }
        /// <summary>
        /// 已收金额
        /// </summary>
        public Decimal? ARB_ReceivedAmount { get; set; }
        /// <summary>
        /// 未收金额
        /// </summary>
        public Decimal? ARB_UnReceiveAmount { get; set; }
        /// <summary>
        /// 业务状态编码
        /// </summary>
        public String ARB_BusinessStatusCode { get; set; }
        /// <summary>
        /// 业务状态名称
        /// </summary>
        public String ARB_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String ARB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String ARB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 对账时间
        /// </summary>
        public DateTime? ARB_ReconciliationTime { get; set; }
        /// <summary>
        /// 对账时间-开始（查询条件用）
        /// </summary>
        public DateTime? _ReconciliationTimeStart { get; set; }
        /// <summary>
        /// 对账时间-终了（查询条件用）
        /// </summary>
        public DateTime? _ReconciliationTimeEnd { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String ARB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? ARB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String ARB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? ARB_CreatedTime { get; set; }
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
        public String ARB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ARB_UpdatedTime { get; set; }
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
        public Int64? ARB_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String ARB_TransID { get; set; }
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
        /// 应收单ID
        /// </summary>
        public String WHERE_ARB_ID { get; set; }
        /// <summary>
        /// 应收单号
        /// </summary>
        public String WHERE_ARB_No { get; set; }
        /// <summary>
        /// 单据方向编码
        /// </summary>
        public String WHERE_ARB_BillDirectCode { get; set; }
        /// <summary>
        /// 单据方向名称
        /// </summary>
        public String WHERE_ARB_BillDirectName { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String WHERE_ARB_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String WHERE_ARB_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单据号
        /// </summary>
        public String WHERE_ARB_SrcBillNo { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_ARB_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String WHERE_ARB_Org_Name { get; set; }
        /// <summary>
        /// 付款对象类型编码
        /// </summary>
        public String WHERE_ARB_PayObjectTypeCode { get; set; }
        /// <summary>
        /// 付款对象类型名称
        /// </summary>
        public String WHERE_ARB_PayObjectTypeName { get; set; }
        /// <summary>
        /// 付款对象ID
        /// </summary>
        public String WHERE_ARB_PayObjectID { get; set; }
        /// <summary>
        /// 付款对象名称
        /// </summary>
        public String WHERE_ARB_PayObjectName { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public Decimal? WHERE_ARB_AccountReceivableAmount { get; set; }
        /// <summary>
        /// 已收金额
        /// </summary>
        public Decimal? WHERE_ARB_ReceivedAmount { get; set; }
        /// <summary>
        /// 未收金额
        /// </summary>
        public Decimal? WHERE_ARB_UnReceiveAmount { get; set; }
        /// <summary>
        /// 业务状态编码
        /// </summary>
        public String WHERE_ARB_BusinessStatusCode { get; set; }
        /// <summary>
        /// 业务状态名称
        /// </summary>
        public String WHERE_ARB_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String WHERE_ARB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_ARB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 对账时间
        /// </summary>
        public DateTime? WHERE_ARB_ReconciliationTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_ARB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_ARB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_ARB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_ARB_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_ARB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_ARB_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_ARB_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_ARB_TransID { get; set; }
        #endregion

    }
}
