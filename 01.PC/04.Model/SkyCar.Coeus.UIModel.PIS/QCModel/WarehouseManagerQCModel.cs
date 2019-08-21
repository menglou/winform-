using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 仓库管理QCModel
    /// </summary>
    public class WarehouseManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 仓库名称
        /// </summary>
        public String WHERE_WH_Name { get; set; }
        /// <summary>
        /// 仓库编号
        /// </summary>
        public String WHERE_WH_No { get; set; }
        /// <summary>
        /// 仓库地址
        /// </summary>
        public String WHERE_WH_Address { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_WH_Org_ID { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_WH_IsValid { get; set; }
    }
}
