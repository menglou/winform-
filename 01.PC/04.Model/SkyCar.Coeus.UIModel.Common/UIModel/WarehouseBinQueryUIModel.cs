using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 仓位查询UIModel
    /// </summary>
    public class WarehouseBinQueryUIModel : BaseUIModel
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
        public String WHB_ID { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public String WH_Name { get; set; }
        /// <summary>
        /// 仓位名称
        /// </summary>
        public String WHB_Name { get; set; }
    }
}
