using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 收款单Model
    /// </summary>
    public class MDLFM_ReceiptBill
    {
        #region 公共属性
        /// <summary>
        /// 收款ID
        /// </summary>
        public String RB_ID { get; set; }
        /// <summary>
        /// 收款单号
        /// </summary>
        public String RB_No { get; set; }
        /// <summary>
        /// 收款组织ID
        /// </summary>
        public String RB_Rec_Org_ID { get; set; }
        /// <summary>
        /// 收款组织名称
        /// </summary>
        public String RB_Rec_Org_Name { get; set; }
        /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime? RB_Date { get; set; }
        /// <summary>
        /// 收款日期-开始（查询条件用）
        /// </summary>
        public DateTime? _DateStart { get; set; }
        /// <summary>
        /// 收款日期-终了（查询条件用）
        /// </summary>
        public DateTime? _DateEnd { get; set; }
        /// <summary>
        /// 付款对象类型编码
        /// </summary>
        public String RB_PayObjectTypeCode { get; set; }
        /// <summary>
        /// 付款对象类型名称
        /// </summary>
        public String RB_PayObjectTypeName { get; set; }
        /// <summary>
        /// 付款对象ID
        /// </summary>
        public String RB_PayObjectID { get; set; }
        /// <summary>
        /// 付款对象
        /// </summary>
        public String RB_PayObjectName { get; set; }
        /// <summary>
        /// 收款通道编码
        /// </summary>
        public String RB_ReceiveTypeCode { get; set; }
        /// <summary>
        /// 收款通道名称
        /// </summary>
        public String RB_ReceiveTypeName { get; set; }
        /// <summary>
        /// 收款账号
        /// </summary>
        public String RB_ReceiveAccount { get; set; }
        /// <summary>
        /// 收款凭证编号
        /// </summary>
        public String RB_CertificateNo { get; set; }
        /// <summary>
        /// 收款凭证图片
        /// </summary>
        public String RB_CertificatePic { get; set; }
        /// <summary>
        /// 合计金额
        /// </summary>
        public Decimal? RB_ReceiveTotalAmount { get; set; }
        /// <summary>
        /// 业务状态编码
        /// </summary>
        public String RB_BusinessStatusCode { get; set; }
        /// <summary>
        /// 业务状态名称
        /// </summary>
        public String RB_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String RB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String RB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String RB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? RB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String RB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? RB_CreatedTime { get; set; }
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
        public String RB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? RB_UpdatedTime { get; set; }
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
        public Int64? RB_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String RB_TransID { get; set; }
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
        /// 收款ID
        /// </summary>
        public String WHERE_RB_ID { get; set; }
        /// <summary>
        /// 收款单号
        /// </summary>
        public String WHERE_RB_No { get; set; }
        /// <summary>
        /// 收款组织ID
        /// </summary>
        public String WHERE_RB_Rec_Org_ID { get; set; }
        /// <summary>
        /// 收款组织名称
        /// </summary>
        public String WHERE_RB_Rec_Org_Name { get; set; }
        /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime? WHERE_RB_Date { get; set; }
        /// <summary>
        /// 付款对象类型编码
        /// </summary>
        public String WHERE_RB_PayObjectTypeCode { get; set; }
        /// <summary>
        /// 付款对象类型名称
        /// </summary>
        public String WHERE_RB_PayObjectTypeName { get; set; }
        /// <summary>
        /// 付款对象ID
        /// </summary>
        public String WHERE_RB_PayObjectID { get; set; }
        /// <summary>
        /// 付款对象
        /// </summary>
        public String WHERE_RB_PayObjectName { get; set; }
        /// <summary>
        /// 收款通道编码
        /// </summary>
        public String WHERE_RB_ReceiveTypeCode { get; set; }
        /// <summary>
        /// 收款通道名称
        /// </summary>
        public String WHERE_RB_ReceiveTypeName { get; set; }
        /// <summary>
        /// 收款账号
        /// </summary>
        public String WHERE_RB_ReceiveAccount { get; set; }
        /// <summary>
        /// 收款凭证编号
        /// </summary>
        public String WHERE_RB_CertificateNo { get; set; }
        /// <summary>
        /// 收款凭证图片
        /// </summary>
        public String WHERE_RB_CertificatePic { get; set; }
        /// <summary>
        /// 合计金额
        /// </summary>
        public Decimal? WHERE_RB_ReceiveTotalAmount { get; set; }
        /// <summary>
        /// 业务状态编码
        /// </summary>
        public String WHERE_RB_BusinessStatusCode { get; set; }
        /// <summary>
        /// 业务状态名称
        /// </summary>
        public String WHERE_RB_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String WHERE_RB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_RB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_RB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_RB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_RB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_RB_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_RB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_RB_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_RB_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_RB_TransID { get; set; }
        #endregion

    }
}
