using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 配件类别查询QCModel
    /// </summary>
    public class AutoPartsTypeQueryQCModel : BaseQCModel
    {
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_APT_Name { get; set; }
        /// <summary>
        /// 配件ID
        /// </summary>
        public String WHERE_APT_ID { get; set; }
    }
}
