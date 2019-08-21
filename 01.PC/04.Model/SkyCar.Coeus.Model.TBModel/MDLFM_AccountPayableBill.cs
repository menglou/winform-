using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 应付单Model
    /// </summary>
    public class MDLFM_AccountPayableBill
    {
        #region 公共属性
        /// <summary>
        /// 应付单ID
        /// </summary>
        public String APB_ID { get; set; }
        /// <summary>
        /// 应付单号
        /// </summary>
        public String APB_No { get; set; }
        /// <summary>
        /// 单据方向编码
        /// </summary>
        public String APB_BillDirectCode { get; set; }
        /// <summary>
        /// 单据方向名称
        /// </summary>
        public String APB_BillDirectName { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String APB_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String APB_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单据号
        /// </summary>
        public String APB_SourceBillNo { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String APB_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String APB_Org_Name { get; set; }
        /// <summary>
        /// 收款对象类型编码
        /// </summary>
        public String APB_ReceiveObjectTypeCode { get; set; }
        /// <summary>
        /// 收款对象类型名称
        /// </summary>
        public String APB_ReceiveObjectTypeName { get; set; }
        /// <summary>
        /// 收款对象ID
        /// </summary>
        public String APB_ReceiveObjectID { get; set; }
        /// <summary>
        /// 收款对象名称
        /// </summary>
        public String APB_ReceiveObjectName { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public Decimal? APB_AccountPayableAmount { get; set; }
        /// <summary>
        /// 已付金额
        /// </summary>
        public Decimal? APB_PaidAmount { get; set; }
        /// <summary>
        /// 未付金额
        /// </summary>
        public Decimal? APB_UnpaidAmount { get; set; }
        /// <summary>
        /// 业务状态编码
        /// </summary>
        public String APB_BusinessStatusCode { get; set; }
        /// <summary>
        /// 业务状态名称
        /// </summary>
        public String APB_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String APB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String APB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 对账时间
        /// </summary>
        public DateTime? APB_ReconciliationTime { get; set; }
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
        public String APB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? APB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String APB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? APB_CreatedTime { get; set; }
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
        public String APB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APB_UpdatedTime { get; set; }
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
        public Int64? APB_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String APB_TransID { get; set; }
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
        /// 应付单ID
        /// </summary>
        public String WHERE_APB_ID { get; set; }
        /// <summary>
        /// 应付单号
        /// </summary>
        public String WHERE_APB_No { get; set; }
        /// <summary>
        /// 单据方向编码
        /// </summary>
        public String WHERE_APB_BillDirectCode { get; set; }
        /// <summary>
        /// 单据方向名称
        /// </summary>
        public String WHERE_APB_BillDirectName { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String WHERE_APB_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String WHERE_APB_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单据号
        /// </summary>
        public String WHERE_APB_SourceBillNo { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_APB_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String WHERE_APB_Org_Name { get; set; }
        /// <summary>
        /// 收款对象类型编码
        /// </summary>
        public String WHERE_APB_ReceiveObjectTypeCode { get; set; }
        /// <summary>
        /// 收款对象类型名称
        /// </summary>
        public String WHERE_APB_ReceiveObjectTypeName { get; set; }
        /// <summary>
        /// 收款对象ID
        /// </summary>
        public String WHERE_APB_ReceiveObjectID { get; set; }
        /// <summary>
        /// 收款对象名称
        /// </summary>
        public String WHERE_APB_ReceiveObjectName { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public Decimal? WHERE_APB_AccountPayableAmount { get; set; }
        /// <summary>
        /// 已付金额
        /// </summary>
        public Decimal? WHERE_APB_PaidAmount { get; set; }
        /// <summary>
        /// 未付金额
        /// </summary>
        public Decimal? WHERE_APB_UnpaidAmount { get; set; }
        /// <summary>
        /// 业务状态编码
        /// </summary>
        public String WHERE_APB_BusinessStatusCode { get; set; }
        /// <summary>
        /// 业务状态名称
        /// </summary>
        public String WHERE_APB_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String WHERE_APB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_APB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 对账时间
        /// </summary>
        public DateTime? WHERE_APB_ReconciliationTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_APB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_APB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_APB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_APB_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_APB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_APB_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_APB_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_APB_TransID { get; set; }
        #endregion

    }
}
