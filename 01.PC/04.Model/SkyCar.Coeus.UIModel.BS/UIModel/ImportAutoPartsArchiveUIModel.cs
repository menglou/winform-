using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.BS
{
    /// <summary>
    /// 导入配件档案UIModel
    /// </summary>
    public class ImportAutoPartsArchiveUIModel : BaseUIModel
    {
        #region 配件档案
        /// <summary>
        /// 条形码
        /// </summary>
        public String APA_Barcode { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String APA_Name { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String APA_OEMNo { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String APA_ThirdNo { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String APA_Brand { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String APA_Specification { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public String APA_UOM { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public String APA_Level { get; set; }
        /// <summary>
        /// 车辆品牌
        /// </summary>
        public String APA_VehicleBrand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String APA_VehicleInspire { get; set; }
        /// <summary>
        /// 排量
        /// </summary>
        public String APA_VehicleCapacity { get; set; }
        /// <summary>
        /// 年款
        /// </summary>
        public String APA_VehicleYearModel { get; set; }
        /// <summary>
        /// 变速类型名称
        /// </summary>
        public String APA_VehicleGearboxType { get; set; }
        #endregion

        /// <summary>
        /// 来源类型
        /// </summary>
        public String APA_SourceType { get; set; }
        /// <summary>
        /// 行号
        /// </summary>
        public long InfoRowNumber { get; set; }
    }
}
