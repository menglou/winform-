using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.IS.UIModel
{
    /// <summary>
    /// 汽修商库存异动UIModel
    /// </summary>
    public class ARInventoryTransLogUIModel : BaseUIModel
    {
        #region 公共属性
        /// <summary>
        /// 组织ID
        /// </summary>
        public String ITL_Org_ID { get; set; }
        /// <summary>
        /// 异动类型
        /// </summary>
        public String ITL_TransType { get; set; }
        /// <summary>
        /// 业务单号
        /// </summary>
        public String ITL_BusinessNo { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String ITL_Barcode { get; set; }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String ITL_BatchNo { get; set; }

        /// <summary>
        /// 异动数量
        /// </summary>
        public Decimal? ITL_TransQty { get; set; }
        /// <summary>
        /// 异动后数量
        /// </summary>
        public Decimal? ITL_AfterTransQty { get; set; }
        /// <summary>
        /// 出发点
        /// </summary>
        public String ITL_Source { get; set; }
        /// <summary>
        /// 目的地
        /// </summary>
        public String ITL_Destination { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public String ITL_CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? ITL_CreatedTime { get; set; }

        #endregion

        /// <summary>
        /// 组织名称
        /// </summary>
        public String Org_ShortName { get; set; }

        /// <summary>
        /// 第三方编码
        /// </summary>
        public String APA_ThirdNo { get; set; }

        /// <summary>
        /// 配件名称
        /// </summary>
        public String APA_Name { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public String APA_Specification { get; set; }

        /// <summary>
        /// 总记录条数（翻页用）
        /// </summary>
        public Int32? RecordCount { get; set; }
    }
}
