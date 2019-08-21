using System;

namespace SkyCar.Coeus.UIModel.SD.QCModel
{
    /// <summary>
    /// 查询汽修商户最近订单中配件的销售单价QCModel
    /// </summary>
    public class QueryAutoPartUnitPriceQCModel
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public String WHERE_SO_CustomerID { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String WHERE_SOD_Barcode { get; set; }
        /// <summary>
        /// 平台内汽修商户组织ID
        /// </summary>
        public String WHERE_AutoFactoryOrgId { get; set; }
    }
}
