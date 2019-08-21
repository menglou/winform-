using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.BS
{
    /// <summary>
    /// 码表管理QCModel
    /// </summary>
    public class CodeTableManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 类型
        /// </summary>
        public String WHERE_CT_Type { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public String WHERE_CT_Name { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_CT_IsValid { get; set; }
    }
}
