using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 库存图片Model
    /// </summary>
    public class MDLPIS_InventoryPicture
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String INVP_ID { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String INVP_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String INVP_SourceTypeName { get; set; }
        /// <summary>
        /// 配件ID
        /// </summary>
        public String INVP_AutoPartsID { get; set; }
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
        /// 创建时间-开始（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeStart { get; set; }
        /// <summary>
        /// 创建时间-终了（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeEnd { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String INVP_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? INVP_UpdatedTime { get; set; }
        /// <summary>
        /// 修改时间-开始（查询条件用）
        /// </summary>
        public DateTime? _UpdatedTimeStart { get; set; }
        /// <summary>
        /// 修改时间-终了（查询条件用）
        /// </summary>
        public DateTime? _UpdatedTimeEnd { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? INVP_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String INVP_TransID { get; set; }
        /// <summary>
        /// 总记录条数（翻页用）
        /// </summary>
        public Int32? RecordCount { get; set; }
        /// <summary>
        /// 当前页面索引/要跳转的页码（翻页用）
        /// </summary>
        public Int32? PageIndex { get; set; }
        /// <summary>
        /// 页面大小（翻页用）
        /// </summary>
        public Int32? PageSize { get; set; }
        #endregion

        #region 公共属性-条件用
        /// <summary>
        /// ID
        /// </summary>
        public String WHERE_INVP_ID { get; set; }
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
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_INVP_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_INVP_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_INVP_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_INVP_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_INVP_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_INVP_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_INVP_TransID { get; set; }
        #endregion

    }
}
