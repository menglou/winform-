using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 物流订单Model
    /// </summary>
    public class MDLSD_LogisticsBill
    {
        #region 公共属性
        /// <summary>
        /// 物流订单ID
        /// </summary>
        public String LB_ID { get; set; }
        /// <summary>
        /// 物流单号
        /// </summary>
        public String LB_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String LB_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String LB_Org_Name { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String LB_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String LB_SourceTypeName { get; set; }
        /// <summary>
        /// 物流单来源单号
        /// </summary>
        public String LB_SourceNo { get; set; }
        /// <summary>
        /// 物流人员类型编码
        /// </summary>
        public String LB_SourceCode { get; set; }
        /// <summary>
        /// 物流人员类型名称
        /// </summary>
        public String LB_SourceName { get; set; }
        /// <summary>
        /// 物流人员ID
        /// </summary>
        public String LB_DeliveryByID { get; set; }
        /// <summary>
        /// 物流人员名称
        /// </summary>
        public String LB_DeliveryBy { get; set; }
        /// <summary>
        /// 物流人员手机号
        /// </summary>
        public String LB_PhoneNo { get; set; }
        /// <summary>
        /// 物流人员接单时间
        /// </summary>
        public DateTime? LB_AcceptTime { get; set; }
        /// <summary>
        /// 物流人员接单时间-开始（查询条件用）
        /// </summary>
        public DateTime? _AcceptTimeStart { get; set; }
        /// <summary>
        /// 物流人员接单时间-终了（查询条件用）
        /// </summary>
        public DateTime? _AcceptTimeEnd { get; set; }
        /// <summary>
        /// 物流人员接单图片路径1
        /// </summary>
        public String LB_AcceptPicPath1 { get; set; }
        /// <summary>
        /// 物流人员接单图片路径2
        /// </summary>
        public String LB_AcceptPicPath2 { get; set; }
        /// <summary>
        /// 收件人姓名
        /// </summary>
        public String LB_Receiver { get; set; }
        /// <summary>
        /// 收件人地址
        /// </summary>
        public String LB_ReceiverAddress { get; set; }
        /// <summary>
        /// 收件人邮编
        /// </summary>
        public String LB_ReceiverPostcode { get; set; }
        /// <summary>
        /// 收件人手机号
        /// </summary>
        public String LB_ReceiverPhoneNo { get; set; }
        /// <summary>
        /// 签收人
        /// </summary>
        public String LB_ReceivedBy { get; set; }
        /// <summary>
        /// 签收时间
        /// </summary>
        public DateTime? LB_ReceivedTime { get; set; }
        /// <summary>
        /// 签收时间-开始（查询条件用）
        /// </summary>
        public DateTime? _ReceivedTimeStart { get; set; }
        /// <summary>
        /// 签收时间-终了（查询条件用）
        /// </summary>
        public DateTime? _ReceivedTimeEnd { get; set; }
        /// <summary>
        /// 签收拍照图片路径1
        /// </summary>
        public String LB_ReceivedPicPath1 { get; set; }
        /// <summary>
        /// 签收拍照图片路径2
        /// </summary>
        public String LB_ReceivedPicPath2 { get; set; }
        /// <summary>
        /// 物流费
        /// </summary>
        public Decimal? LB_Fee { get; set; }
        /// <summary>
        /// 赔偿金
        /// </summary>
        public Decimal? LB_Indemnification { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public Decimal? LB_AccountReceivableAmount { get; set; }
        /// <summary>
        /// 物流费付款状态编码
        /// </summary>
        public String LB_PayStautsCode { get; set; }
        /// <summary>
        /// 物流费付款状态名称
        /// </summary>
        public String LB_PayStautsName { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String LB_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String LB_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String LB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String LB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String LB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? LB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String LB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? LB_CreatedTime { get; set; }
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
        public String LB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? LB_UpdatedTime { get; set; }
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
        public Int64? LB_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String LB_TransID { get; set; }
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
        /// 物流订单ID
        /// </summary>
        public String WHERE_LB_ID { get; set; }
        /// <summary>
        /// 物流单号
        /// </summary>
        public String WHERE_LB_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_LB_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String WHERE_LB_Org_Name { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String WHERE_LB_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_LB_SourceTypeName { get; set; }
        /// <summary>
        /// 物流单来源单号
        /// </summary>
        public String WHERE_LB_SourceNo { get; set; }
        /// <summary>
        /// 物流人员类型编码
        /// </summary>
        public String WHERE_LB_SourceCode { get; set; }
        /// <summary>
        /// 物流人员类型名称
        /// </summary>
        public String WHERE_LB_SourceName { get; set; }
        /// <summary>
        /// 物流人员ID
        /// </summary>
        public String WHERE_LB_DeliveryByID { get; set; }
        /// <summary>
        /// 物流人员名称
        /// </summary>
        public String WHERE_LB_DeliveryBy { get; set; }
        /// <summary>
        /// 物流人员手机号
        /// </summary>
        public String WHERE_LB_PhoneNo { get; set; }
        /// <summary>
        /// 物流人员接单时间
        /// </summary>
        public DateTime? WHERE_LB_AcceptTime { get; set; }
        /// <summary>
        /// 物流人员接单图片路径1
        /// </summary>
        public String WHERE_LB_AcceptPicPath1 { get; set; }
        /// <summary>
        /// 物流人员接单图片路径2
        /// </summary>
        public String WHERE_LB_AcceptPicPath2 { get; set; }
        /// <summary>
        /// 收件人姓名
        /// </summary>
        public String WHERE_LB_Receiver { get; set; }
        /// <summary>
        /// 收件人地址
        /// </summary>
        public String WHERE_LB_ReceiverAddress { get; set; }
        /// <summary>
        /// 收件人邮编
        /// </summary>
        public String WHERE_LB_ReceiverPostcode { get; set; }
        /// <summary>
        /// 收件人手机号
        /// </summary>
        public String WHERE_LB_ReceiverPhoneNo { get; set; }
        /// <summary>
        /// 签收人
        /// </summary>
        public String WHERE_LB_ReceivedBy { get; set; }
        /// <summary>
        /// 签收时间
        /// </summary>
        public DateTime? WHERE_LB_ReceivedTime { get; set; }
        /// <summary>
        /// 签收拍照图片路径1
        /// </summary>
        public String WHERE_LB_ReceivedPicPath1 { get; set; }
        /// <summary>
        /// 签收拍照图片路径2
        /// </summary>
        public String WHERE_LB_ReceivedPicPath2 { get; set; }
        /// <summary>
        /// 物流费
        /// </summary>
        public Decimal? WHERE_LB_Fee { get; set; }
        /// <summary>
        /// 赔偿金
        /// </summary>
        public Decimal? WHERE_LB_Indemnification { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public Decimal? WHERE_LB_AccountReceivableAmount { get; set; }
        /// <summary>
        /// 物流费付款状态编码
        /// </summary>
        public String WHERE_LB_PayStautsCode { get; set; }
        /// <summary>
        /// 物流费付款状态名称
        /// </summary>
        public String WHERE_LB_PayStautsName { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String WHERE_LB_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String WHERE_LB_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String WHERE_LB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_LB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_LB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_LB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_LB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_LB_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_LB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_LB_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_LB_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_LB_TransID { get; set; }
        #endregion

    }
}
