using System;

namespace SkyCar.Coeus.UIModel.Common.QCModel
{
    /// <summary>
    /// 库存异动日志查询QCModel
    /// </summary>
    public class InventoryTransLogQueryQCModel : BaseQCModel
    {
        /// <summary>
        /// 异动类型
        /// </summary>
        public String WHERE_ITL_TransType { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_ITL_Org_ID { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String WHERE_ITL_Barcode { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_ITL_Name { get; set; }
        /// <summary>
        /// 创建时间-开始（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeStart { get; set; }
        /// <summary>
        /// 创建时间-终了（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeEnd { get; set; }
    }
}
