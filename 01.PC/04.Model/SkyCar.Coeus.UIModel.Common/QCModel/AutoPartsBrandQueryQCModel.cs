using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 配件名称查询QCModel
    /// </summary>
    public class AutoPartsBrandQueryQCModel : BaseQCModel
    {
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_APA_Name { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String WHERE_APA_Brand { get; set; }
    }
}
