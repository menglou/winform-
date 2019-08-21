using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.FM
{
    /// <summary>
    /// 付款单明细QCModel
    /// </summary>
    public class PayBillManagerDetailQCModel : BaseQCModel
    {
        /// <summary>
        /// 付款单ID
        /// </summary>
        public String WHERE_PBD_PB_ID { get; set; }
        /// <summary>
        /// 付款单号
        /// </summary>
        public String WHERE_PBD_PB_No { get; set; }
    }
}
