using System;


namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 物流单QCModel
    /// </summary>
    public class LogisticsBillQueryQCModel : BaseQCModel
    {
        /// <summary>
        /// 物流人员类型
        /// </summary>
        public String WHERE_LB_SourceName { get; set; }
        /// <summary>
        /// 物流人员名称
        /// </summary>
        public String WHERE_LB_DeliveryBy { get; set; }
        /// <summary>
        /// 物流人员手机号
        /// </summary>
        public String WHERE_LB_PhoneNo { get; set; }
        
    }
}
