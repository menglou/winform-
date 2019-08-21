using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 仓库查询UIModel
    /// </summary>
    public class WarehouseQueryUIModel : BaseUIModel
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
        public String WH_ID { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public String WH_Name { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WH_Org_ID { get; set; }
        /// <summary>
        /// 组织简称
        /// </summary>
        public String Org_ShortName { get; set; }
    }
}
