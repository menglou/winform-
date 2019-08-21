using System;

namespace SkyCar.Coeus.UIModel.Common.APModel
{
    /// <summary>
    /// 配件图片UIModel
    /// </summary>
    public class AutoPartsPictureUIModel : BaseNotificationUIModel
    {
        #region 配件图片
        /// <summary>
        /// ID
        /// </summary>
        public String INVP_ID { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public String INVP_Barcode { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
        public String INVP_PictureName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? INVP_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String INVP_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? INVP_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String INVP_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? INVP_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? INVP_VersionNo { get; set; }
        #endregion

        #region 其他属性

        /// <summary>
        /// 源文件路径
        /// </summary>
        public String SourceFilePath { get; set; }
        #endregion
    }
}
