using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 应收单明细Model
    /// </summary>
    public class MDLFM_AccountReceivableBillDetail
    {
        #region 公共属性
        /// <summary>
        /// 应收单明细ID
        /// </summary>
        public String ARBD_ID { get; set; }
        /// <summary>
        /// 应收单ID
        /// </summary>
        public String ARBD_ARB_ID { get; set; }
        /// <summary>
        /// 是否负向明细
        /// </summary>
        public Boolean? ARBD_IsMinusDetail { get; set; }
        /// <summary>
        /// 来源单据号
        /// </summary>
        public String ARBD_SrcBillNo { get; set; }
        /// <summary>
        /// 来源单据明细ID
        /// </summary>
        public String ARBD_SrcBillDetailID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String ARBD_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String ARBD_Org_Name { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public Decimal? ARBD_AccountReceivableAmount { get; set; }
        /// <summary>
        /// 已收金额
        /// </summary>
        public Decimal? ARBD_ReceivedAmount { get; set; }
        /// <summary>
        /// 未收金额
        /// </summary>
        public Decimal? ARBD_UnReceiveAmount { get; set; }
        /// <summary>
        /// 业务状态编码
        /// </summary>
        public String ARBD_BusinessStatusCode { get; set; }
        /// <summary>
        /// 业务状态名称
        /// </summary>
        public String ARBD_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String ARBD_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String ARBD_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String ARBD_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? ARBD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String ARBD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? ARBD_CreatedTime { get; set; }
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
        public String ARBD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ARBD_UpdatedTime { get; set; }
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
        public Int64? ARBD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String ARBD_TransID { get; set; }
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
        /// 应收单明细ID
        /// </summary>
        public String WHERE_ARBD_ID { get; set; }
        /// <summary>
        /// 应收单ID
        /// </summary>
        public String WHERE_ARBD_ARB_ID { get; set; }
        /// <summary>
        /// 是否负向明细
        /// </summary>
        public Boolean? WHERE_ARBD_IsMinusDetail { get; set; }
        /// <summary>
        /// 来源单据号
        /// </summary>
        public String WHERE_ARBD_SrcBillNo { get; set; }
        /// <summary>
        /// 来源单据明细ID
        /// </summary>
        public String WHERE_ARBD_SrcBillDetailID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_ARBD_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String WHERE_ARBD_Org_Name { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public Decimal? WHERE_ARBD_AccountReceivableAmount { get; set; }
        /// <summary>
        /// 已收金额
        /// </summary>
        public Decimal? WHERE_ARBD_ReceivedAmount { get; set; }
        /// <summary>
        /// 未收金额
        /// </summary>
        public Decimal? WHERE_ARBD_UnReceiveAmount { get; set; }
        /// <summary>
        /// 业务状态编码
        /// </summary>
        public String WHERE_ARBD_BusinessStatusCode { get; set; }
        /// <summary>
        /// 业务状态名称
        /// </summary>
        public String WHERE_ARBD_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String WHERE_ARBD_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_ARBD_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_ARBD_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_ARBD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_ARBD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_ARBD_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_ARBD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_ARBD_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_ARBD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_ARBD_TransID { get; set; }
        #endregion

    }
}
