using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 调拨管理UIModel
    /// </summary>
    public class TransferBillManagerUIModel : BaseUIModel
    {
        #region 调拨管理属性

        /// <summary>
        /// 单号
        /// </summary>
        public String TB_No { get; set; }
        /// <summary>
        /// 单据类型
        /// </summary>
        public String TB_TypeName { get; set; }
        /// <summary>
        /// 调拨类型
        /// </summary>
        public String TB_TransferTypeName { get; set; }
        /// <summary>
        /// 调出组织
        /// </summary>
        public String TB_TransferOutOrgName { get; set; }
        /// <summary>
        /// 调入组织
        /// </summary>
        public String TB_TransferInOrgName { get; set; }
        /// <summary>
        /// 单据状态
        /// </summary>
        public String TB_StatusName { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public String TB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? TB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String TB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? TB_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String TB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? TB_UpdatedTime { get; set; }
        /// <summary>
        /// 调拨单ID
        /// </summary>
        public String TB_ID { get; set; }
        /// <summary>
        /// 单据类型编码
        /// </summary>
        public String TB_TypeCode { get; set; }
        /// <summary>
        /// 调拨类型编码
        /// </summary>
        public String TB_TransferTypeCode { get; set; }
        /// <summary>
        /// 调出组织ID
        /// </summary>
        public String TB_TransferOutOrgId { get; set; }
        /// <summary>
        /// 调入组织ID
        /// </summary>
        public String TB_TransferInOrgId { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String TB_StatusCode { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String TB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? TB_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String TB_TransID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String TB_Remark { get; set; }

        #endregion

        #region 其他属性

        /// <summary>
        /// 选择
        /// </summary>
        public bool IsChecked { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public decimal TotalQty { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        #endregion
    }
}
