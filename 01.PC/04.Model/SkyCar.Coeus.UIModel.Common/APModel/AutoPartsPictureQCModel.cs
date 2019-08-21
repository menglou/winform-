using System;

namespace SkyCar.Coeus.UIModel.Common.APModel
{
    /// <summary>
    /// 配件图片QCModel
    /// </summary>
    public class AutoPartsPictureQCModel : BaseNotificationUIModel
    {
        /// <summary>
        /// 库存组织ID
        /// </summary>
        public String WHERE_INV_Org_ID { get; set; }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String WHERE_INV_BatchNo { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WHERE_INV_WH_ID { get; set; }

        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String WHERE_INVP_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_INVP_SourceTypeName { get; set; }
        /// <summary>
        /// 配件ID
        /// </summary>
        public String WHERE_INVP_AutoPartsID { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public String WHERE_INVP_Barcode { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
        public String WHERE_INVP_PictureName { get; set; }
    }
}
