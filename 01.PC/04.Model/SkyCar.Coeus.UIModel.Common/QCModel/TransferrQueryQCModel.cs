using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 销售订单查询QCModel
    /// </summary>
    public class TransferrQueryQCModel : BaseQCModel
    {
        #region 公共属性-条件用
       
        /// <summary>
        /// 单号
        /// </summary>
        public String WHERE_TB_No { get; set; }
        /// <summary>
        /// 单据类型编码
        /// </summary>
        public String WHERE_TB_TypeCode { get; set; }
        /// <summary>
        /// 单据类型名称
        /// </summary>
        public String WHERE_TB_TypeName { get; set; }
        /// <summary>
        /// 调拨类型编码
        /// </summary>
        public String WHERE_TB_TransferTypeCode { get; set; }
        /// <summary>
        /// 调拨类型名称
        /// </summary>
        public String WHERE_TB_TransferTypeName { get; set; }
        /// <summary>
        /// 调出组织ID
        /// </summary>
        public String WHERE_TB_TransferOutOrgId { get; set; }
        /// <summary>
        /// 调出组织名称
        /// </summary>
        public String WHERE_TB_TransferOutOrgName { get; set; }
        /// <summary>
        /// 调入组织ID
        /// </summary>
        public String WHERE_TB_TransferInOrgId { get; set; }
        /// <summary>
        /// 调入组织名称
        /// </summary>
        public String WHERE_TB_TransferInOrgName { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String WHERE_TB_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String WHERE_TB_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String WHERE_TB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_TB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_TB_IsValid { get; set; }
      
        #endregion
    }
}
