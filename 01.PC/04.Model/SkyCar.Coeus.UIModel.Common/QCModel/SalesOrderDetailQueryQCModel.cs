using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 销售订单查询QCModel
    /// </summary>
    public class SalesOrderDetailQueryQCModel : BaseQCModel
    {
        #region 公共属性-条件用
        /// <summary>
        /// 配件条码
        /// </summary>
        public String WHERE_SOD_Barcode { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_SOD_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String WHERE_SOD_Specification { get; set; }
        /// <summary>
        /// 销售订单ID
        /// </summary>
        public String WHERE_SOD_SO_ID { get; set; }
        /// <summary>
        /// 销售订单明细ID
        /// </summary>
        public String WHERE_SOD_ID { get; set; }
       
        #endregion
    }
}
