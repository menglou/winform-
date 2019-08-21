using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 配件名称查询UIModel
    /// </summary>
    public class AutoPartsNameQueryUIModel : BaseUIModel
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
        public String APN_Name { get; set; }
        /// <summary>
        /// 配置名称ID
        /// </summary>
        public String APN_ID { get; set; }
    }
}
