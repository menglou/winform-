using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 配件档案查询QCModel
    /// </summary>
    public class AutoPartsArchiveQueryQCModel : BaseQCModel
    {
        #region 公共属性-条件用
        /// <summary>
        /// 条形码
        /// </summary>
        public String WHERE_APA_Barcode { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String WHERE_APA_OEMNo { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String WHERE_APA_ThirdNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_APA_Name { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String WHERE_APA_Brand { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String WHERE_APA_Specification { get; set; }
        #endregion
    }
}
