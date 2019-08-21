using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 仓位管理QCModel
    /// </summary>
    public class WarehouseBinManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 仓位名称
        /// </summary>
        public String WHERE_WHB_Name { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_WH_IsValid { get; set; }
    }
}
