using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.BS
{
    /// <summary>
    /// 配件名称QCModel
    /// </summary>
    public class AutoPartsNameManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_APN_Name { get; set; }
        /// <summary>
        /// 配件类别ID
        /// </summary>
        public String WHERE_APN_APT_ID { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_APN_IsValid { get; set; }
    }
}
