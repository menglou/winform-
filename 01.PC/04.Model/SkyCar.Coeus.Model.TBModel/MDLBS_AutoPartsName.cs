using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 配件名称Model
    /// </summary>
    public class MDLBS_AutoPartsName
    {
        #region 公共属性
        /// <summary>
        /// 配置名称ID
        /// </summary>
        public String APN_ID { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String APN_Name { get; set; }
        /// <summary>
        /// 配件别名
        /// </summary>
        public String APN_Alias { get; set; }
        /// <summary>
        /// 名称拼音简写
        /// </summary>
        public String APN_NameSpellCode { get; set; }
        /// <summary>
        /// 别名拼音简写
        /// </summary>
        public String APN_AliasSpellCode { get; set; }
        /// <summary>
        /// 配件类别ID
        /// </summary>
        public String APN_APT_ID { get; set; }
        /// <summary>
        /// 呆滞天数
        /// </summary>
        public Int32? APN_SlackDays { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public String APN_UOM { get; set; }
        /// <summary>
        /// 固定计量单位
        /// </summary>
        public Boolean? APN_FixUOM { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? APN_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String APN_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? APN_CreatedTime { get; set; }
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
        public String APN_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APN_UpdatedTime { get; set; }
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
        public Int64? APN_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String APN_TransID { get; set; }
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
        /// 配置名称ID
        /// </summary>
        public String WHERE_APN_ID { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_APN_Name { get; set; }
        /// <summary>
        /// 配件别名
        /// </summary>
        public String WHERE_APN_Alias { get; set; }
        /// <summary>
        /// 名称拼音简写
        /// </summary>
        public String WHERE_APN_NameSpellCode { get; set; }
        /// <summary>
        /// 别名拼音简写
        /// </summary>
        public String WHERE_APN_AliasSpellCode { get; set; }
        /// <summary>
        /// 配件类别ID
        /// </summary>
        public String WHERE_APN_APT_ID { get; set; }
        /// <summary>
        /// 呆滞天数
        /// </summary>
        public Int32? WHERE_APN_SlackDays { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public String WHERE_APN_UOM { get; set; }
        /// <summary>
        /// 固定计量单位
        /// </summary>
        public Boolean? WHERE_APN_FixUOM { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_APN_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_APN_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_APN_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_APN_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_APN_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_APN_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_APN_TransID { get; set; }
        #endregion

    }
}
