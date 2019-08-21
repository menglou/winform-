using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.FM.APModel
{
    /// <summary>
    /// 收付款QCModel
    /// </summary>
    public class ReceiptAndPayQCModel : BaseUIModel
    {
        /// <summary>
        /// 来源单号
        /// </summary>
        public String Where_BillSourceNo { get; set; }
        /// <summary>
        /// 交易对象ID
        /// </summary>
        public String Where_TradeObjectID { get; set; }
    }
}
