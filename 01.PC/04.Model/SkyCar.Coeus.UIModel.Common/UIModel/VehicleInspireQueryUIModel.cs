using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 车辆品牌查询UIModel
    /// </summary>
    public class VehicleInspireQueryUIModel : BaseUIModel
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
        /// 品牌
        /// </summary>
        public String VBIS_Brand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String VBIS_Inspire { get; set; }
    }
}
