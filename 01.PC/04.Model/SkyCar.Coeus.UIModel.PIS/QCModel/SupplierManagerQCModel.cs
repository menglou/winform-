using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 供应商管理QCModel
    /// </summary>
    public class SupplierManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 编码
        /// </summary>
        public String WHERE_SUPP_Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String WHERE_SUPP_Name { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        public String WHERE_SUPP_ShortName { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public String WHERE_SUPP_Contacter { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_SUPP_IsValid { get; set; }
    }
}
