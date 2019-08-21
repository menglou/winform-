using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.IS.QCModel
{
    /// <summary>
    /// 汽修商库存和异动日志查询QCModel
    /// </summary>
    public class ARInventoryAndTransLogQCModel : BaseQCModel
    {
        //汽修商户，是否分组织，汽修商户组织，配件条码，第三方编码，配件名称，配件品牌，规格型号，出入库时间（开始-终了）
        /// <summary>
        /// 是否分组织
        /// </summary>
        public Boolean? IsDifferentiateOrg { get; set; }
        /// <summary>
        /// 汽修商户组织名称
        /// </summary>
        public String INV_Org_ShortName { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String INV_Barcode { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String INV_ThirdNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String APA_Name { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String APA_Brand { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String APA_Specification { get; set; }
        /// <summary>
        /// 创建时间-开始（查询条件用）
        /// </summary>
        public DateTime? INV_CreatedTime_Start { get; set; }
        /// <summary>
        /// 创建时间-终了（查询条件用）
        /// </summary>
        public DateTime? INV_CreatedTime_End { get; set; }

        /// <summary>
        /// 汽修商户组织名称
        /// </summary>
        public String ITL_Org_Name { get; set; }
        /// <summary>
        /// 库存异动类型
        /// </summary>
        public String ITL_TransType { get; set; }
        /// <summary>
        /// 汽配商户编码
        /// </summary>
        public String MerchantCode { get; set; }
    }
}
