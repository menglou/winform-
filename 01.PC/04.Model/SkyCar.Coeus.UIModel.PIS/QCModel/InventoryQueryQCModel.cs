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
    public class InventoryQueryQCModel : BaseQCModel
    {
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String WHERE_INV_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String WHERE_INV_OEMNo { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String WHERE_INV_Barcode { get; set; }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String WHERE_INV_BatchNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_INV_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String WHERE_INV_Specification { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String WHERE_INV_SUPP_Name { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public String WHERE_INV_WH_Name { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WHERE_INV_WH_ID { get; set; }
        /// <summary>
        /// 仓位名称
        /// </summary>
        public String WHERE_INV_WHB_Name { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String WHERE_INV_WHB_ID { get; set; }

        /// <summary>
        /// 0库存
        /// </summary>
        public Boolean WHERE_INV_IsZero { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String WHERE_INV_Org_ID { get; set; }

        /// <summary>
        /// 配件条形码（条形码+批次号）
        /// </summary>
        public String WHERE_BarcodeAndBatchNo { get; set; }
        /// <summary>
        /// 配件编码（原厂编码或第三方编码）
        /// </summary>
        public String WHERE_AutoPartsCode { get; set; }
        /// <summary>
        /// 其他描述（专有属性或适用范围中的任一项）
        /// </summary>
        public String WHERE_OtherDesc { get; set; }
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
