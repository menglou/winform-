using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 库存查询QCModel
    /// </summary>
    public class InventoryTransLogQCModel : BaseQCModel
    {
        // SkyCar.Coeus.UIModel.PIS.InventoryTransLogQueryQCModel

        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_ITL_Org_ID { get; set; }
        /// <summary>
        /// 业务单号
        /// </summary>
        public String WHERE_ITL_BusinessNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_ITL_Name { get; set; }
        /// <summary>
        /// 异动类型
        /// </summary>
        public String WHERE_ITL_TransType { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WHERE_ITL_WH_ID { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String WHERE_ITL_Barcode { get; set; }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String WHERE_ITL_BatchNo { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String WHERE_ITL_Specification { get; set; }
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
