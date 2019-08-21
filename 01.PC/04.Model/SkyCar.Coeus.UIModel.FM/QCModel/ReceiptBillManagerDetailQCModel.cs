using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.FM
{
    /// <summary>
    /// 收款单管理QCModel
    /// </summary>
    public class ReceiptBillManagerDetailQCModel : BaseQCModel
    {
        /// <summary>
        /// 收款单ID
        /// </summary>
        public String WHERE_RBD_RB_ID { get; set; }
        /// <summary>
        /// 收款单号
        /// </summary>
        public String WHERE_RBD_RB_No { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_RBD_IsValid { get; set; }
    }
}
