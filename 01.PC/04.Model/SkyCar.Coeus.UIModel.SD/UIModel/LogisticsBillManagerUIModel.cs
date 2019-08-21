using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SD
{
    /// <summary>
    /// 物流单管理UIModel
    /// </summary>
    public class LogisticsBillManagerUIModel : BaseUIModel
    {
        /// <summary>
        /// 物流单号
        /// </summary>
        public String LB_No { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String LB_Org_Name { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String LB_SourceTypeName { get; set; }
        /// <summary>
        /// 物流单来源单号
        /// </summary>
        public String LB_SourceNo { get; set; }
        /// <summary>
        /// 物流人员类型
        /// </summary>
        public String LB_SourceName { get; set; }
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
        /// 赔偿金额
        /// </summary>
        public Decimal? LB_Indemnification { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public Decimal? LB_AccountReceivableAmount { get; set; }
        /// <summary>
        /// 物流费付款状态名称
        /// </summary>
        public String LB_PayStautsName { get; set; }
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
        /// 修改人
        /// </summary>
        public String LB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? LB_UpdatedTime { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String LB_SourceTypeCode { get; set; }
        /// <summary>
        /// 物流人员类型编码
        /// </summary>
        public String LB_SourceCode { get; set; }
        /// <summary>
        /// 物流费付款状态编码
        /// </summary>
        public String LB_PayStautsCode { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String LB_StatusCode { get; set; }
        /// <summary>
        /// 物流订单ID
        /// </summary>
        public String LB_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String LB_Org_ID { get; set; }
        /// <summary>
        /// 物流人员ID
        /// </summary>
        public String LB_DeliveryByID { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? LB_VersionNo { get; set; }

        #region 其他属性

        /// <summary>
        /// 选择
        /// </summary>
        public Boolean IsChecked { get; set; }

        /// <summary>
        /// 已收金额
        /// </summary>
        public Decimal? ARB_ReceivedAmount { get; set; }
        /// <summary>
        /// 未收金额
        /// </summary>
        public Decimal? ARB_UnReceiveAmount { get; set; }

        #endregion
    }
}
