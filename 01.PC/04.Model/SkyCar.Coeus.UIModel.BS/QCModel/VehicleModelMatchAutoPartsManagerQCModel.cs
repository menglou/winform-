using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.BS.QCModel
{
    /// <summary>
    /// 车型配件匹配管理QCModel
    /// </summary>
    public class VehicleModelMatchAutoPartsManagerQCModel : BaseQCModel
    {
        #region 车辆信息
        /// <summary>
        /// ID
        /// </summary>
        public String WHERE_VC_ID { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        public String WHERE_VC_VIN { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public String WHERE_VC_PlateNumber { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public String WHERE_VC_Brand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String WHERE_VC_Inspire { get; set; }
        /// <summary>
        /// 车型描述
        /// </summary>
        public String WHERE_VC_BrandDesc { get; set; }
        /// <summary>
        /// 排量
        /// </summary>
        public String WHERE_VC_Capacity { get; set; }
        /// <summary>
        /// 发动机型号
        /// </summary>
        public String WHERE_VC_EngineType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_VC_Remark { get; set; }
        #endregion

        /// <summary>
        /// 是否用于删除
        /// </summary>
        public bool IsUsedDelete { get; set; }
    }
}
