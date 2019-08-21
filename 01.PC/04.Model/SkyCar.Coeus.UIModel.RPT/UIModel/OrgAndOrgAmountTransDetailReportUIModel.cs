using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.UIModel
{
    /// <summary>
    /// 组织与组织资金往来统计明细UIModel
    /// </summary>
    public class OrgAndOrgAmountTransDetailReportUIModel : BaseNotificationUIModel
    {
        #region 调拨单

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
        /// 调出组织ID
        /// </summary>
        public String TB_TransferOutOrgId { get; set; }
        /// <summary>
        /// 调出组织
        /// </summary>
        public String TB_TransferOutOrgName { get; set; }
        /// <summary>
        /// 调入组织ID
        /// </summary>
        public String TB_TransferInOrgId { get; set; }
        /// <summary>
        /// 调入组织
        /// </summary>
        public String TB_TransferInOrgName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? TB_CreatedTime { get; set; }

        #endregion

        #region 调拨明细
        /// <summary>
        /// 配件条码
        /// </summary>
        public String TBD_Barcode { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String TBD_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String TBD_OEMNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String TBD_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String TBD_Specification { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? TBD_Qty { get; set; }
        #endregion
        
        #region 配件档案

        /// <summary>
        /// 配件品牌
        /// </summary>
        public String APA_Brand { get; set; }
        #endregion

        #region 其他属性

        /// <summary>
        /// 调拨金额
        /// </summary>
        public Decimal? TransferAmount { get; set; }

        /// <summary>
        /// 数量合计
        /// </summary>
        public Decimal? TotalQty { get; set; }

        /// <summary>
        /// 金额合计
        /// </summary>
        public Decimal? TotalAmount { get; set; }

        #endregion
    }
}
