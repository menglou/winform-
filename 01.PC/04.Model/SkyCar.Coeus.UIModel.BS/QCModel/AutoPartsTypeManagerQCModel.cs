using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.BS
{
    /// <summary>
    /// 配件类别QCModel
    /// </summary>
    public class AutoPartsTypeManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 配件类别名称
        /// </summary>
        public String WHERE_APT_Name { get; set; }
        /// <summary>
        /// 父级类别ID
        /// </summary>
        public String WHERE_APT_ParentID { get; set; }
        /// <summary>
        /// 父级类别名称
        /// </summary>
        public String WHERE_APT_ParentName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_APT_IsValid { get; set; }
    }
}
