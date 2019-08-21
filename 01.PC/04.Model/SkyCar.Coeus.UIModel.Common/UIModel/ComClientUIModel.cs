using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.UIModel.Common.UIModel
{
    public class ComClientUIModel
    {
        /// <summary>
        /// 客户类别
        /// </summary>
        public string ClientType { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public string ClientID { get; set; }
        /// <summary>
        /// 商户编码
        /// </summary>
        public string MerchantCode { get; set; }
        /// <summary>
        /// 组织编码
        /// </summary>
        public string OrgCode { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public string OrgID { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// 价格类别
        /// </summary>
        public string AutoPartsPriceType { get; set; }
        /// <summary>
        /// 供应商优先显示
        /// </summary>
        public bool SupplierFirstlyDisplay { get; set; }
        /// <summary>
        /// 终止销售
        /// </summary>
        public bool? IsEndSales { get; set; }
    }
}
