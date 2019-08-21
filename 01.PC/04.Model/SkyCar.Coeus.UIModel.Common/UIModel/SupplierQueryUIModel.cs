using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 供应商查询UIModel
    /// </summary>
    public class SupplierQueryUIModel : BaseUIModel
    {
        /// <summary>
        /// 行标识
        /// </summary>
        public string RowID { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public String SUPP_ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String SUPP_Name { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        public String SUPP_ShortName { get; set; }
    }
}
