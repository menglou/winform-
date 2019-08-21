using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 配件品牌查询UIModel
    /// </summary>
    public class AutoPartsBrandQueryUIModel : BaseUIModel
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
        /// 配件名称
        /// </summary>
        public String APA_Name { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String APA_Brand { get; set; }
    }
}
