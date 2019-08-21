using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.IS.QCModel
{
    /// <summary>
    /// 汽修组织车辆品牌车系查询QCModel
    /// </summary>
    public class AFOrgVehicleBrandInspireQueryQCModel : BaseQCModel
    {
        /// <summary>
        /// 汽修商户编码
        /// </summary>
        public String ARMerchantCode { get; set; }
        /// <summary>
        /// 汽修商户编码
        /// </summary>
        public String ARMerchantName { get; set; }
        /// <summary>
        /// 汽修商户组织名称
        /// </summary>
        public String AROrgName { get; set; }
        /// <summary>
        /// 统计方式
        /// </summary>
        public String StatisticsMode { get; set; }
    }
}
