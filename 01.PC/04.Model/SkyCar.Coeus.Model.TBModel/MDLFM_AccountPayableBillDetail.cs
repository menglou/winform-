using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 应付单明细Model
    /// </summary>
    public class MDLFM_AccountPayableBillDetail
    {
        #region 公共属性
        /// <summary>
        /// 应付单明细ID
        /// </summary>
        public String APBD_ID { get; set; }
        /// <summary>
        /// 应付单ID
        /// </summary>
        public String APBD_APB_ID { get; set; }
        /// <summary>
        /// 是否负向明细
        /// </summary>
        public Boolean? APBD_IsMinusDetail { get; set; }
        /// <summary>
        /// 来源单据号
        /// </summary>
        public String APBD_SourceBillNo { get; set; }
        /// <summary>
        /// 来源单据明细ID
        /// </summary>
        public String APBD_SourceBillDetailID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String APBD_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String APBD_Org_Name { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public Decimal? APBD_AccountPayableAmount { get; set; }
        /// <summary>
        /// 已付金额
        /// </summary>
        public Decimal? APBD_PaidAmount { get; set; }
        /// <summary>
        /// 未付金额
        /// </summary>
        public Decimal? APBD_UnpaidAmount { get; set; }
        /// <summary>
        /// 业务状态编码
        /// </summary>
        public String APBD_BusinessStatusCode { get; set; }
        /// <summary>
        /// 业务状态名称
        /// </summary>
        public String APBD_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String APBD_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String APBD_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String APBD_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? APBD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String APBD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? APBD_CreatedTime { get; set; }
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
        public String APBD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APBD_UpdatedTime { get; set; }
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
        public Int64? APBD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String APBD_TransID { get; set; }
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
        /// 应付单明细ID
        /// </summary>
        public String WHERE_APBD_ID { get; set; }
        /// <summary>
        /// 应付单ID
        /// </summary>
        public String WHERE_APBD_APB_ID { get; set; }
        /// <summary>
        /// 是否负向明细
        /// </summary>
        public Boolean? WHERE_APBD_IsMinusDetail { get; set; }
        /// <summary>
        /// 来源单据号
        /// </summary>
        public String WHERE_APBD_SourceBillNo { get; set; }
        /// <summary>
        /// 来源单据明细ID
        /// </summary>
        public String WHERE_APBD_SourceBillDetailID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_APBD_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String WHERE_APBD_Org_Name { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public Decimal? WHERE_APBD_AccountPayableAmount { get; set; }
        /// <summary>
        /// 已付金额
        /// </summary>
        public Decimal? WHERE_APBD_PaidAmount { get; set; }
        /// <summary>
        /// 未付金额
        /// </summary>
        public Decimal? WHERE_APBD_UnpaidAmount { get; set; }
        /// <summary>
        /// 业务状态编码
        /// </summary>
        public String WHERE_APBD_BusinessStatusCode { get; set; }
        /// <summary>
        /// 业务状态名称
        /// </summary>
        public String WHERE_APBD_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String WHERE_APBD_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_APBD_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_APBD_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_APBD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_APBD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_APBD_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_APBD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_APBD_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_APBD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_APBD_TransID { get; set; }
        #endregion

    }
}
