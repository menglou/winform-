using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SM
{
    /// <summary>
    /// 门店管理QCModel
    /// </summary>
    public class OrganizationManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 门店编码
        /// </summary>
        public String WHERE_Org_Code { get; set; }
        /// <summary>
        /// 组织全称
        /// </summary>
        public String WHERE_Org_FullName { get; set; }
        /// <summary>
        /// 组织简称
        /// </summary>
        public String WHERE_Org_ShortName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_Org_IsValid { get; set; }
    }
}
