using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// Venus中配件名称表
    /// </summary>
    public class MDLAPM_AutoPartsName
    {
        #region 公共属性
        /// <summary>
        /// 配置名称ID
        /// </summary>
        public String APN_ID { set; get; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String APN_Name { set; get; }
        /// <summary>
        /// 配件别名
        /// </summary>
        public String APN_Alias
        { set; get; }
        /// <summary>
        /// 名称拼音简写
        /// </summary>
        public String APN_NameSpellCode
        { set; get; }
        /// <summary>
        /// 别名拼音简写
        /// </summary>
        public String APN_AliasSpellCode
        { set; get; }
        /// <summary>
        /// 配件类别ID
        /// </summary>
        public String APN_APT_ID
        { set; get; }
        /// <summary>
        /// 配件类别ID
        /// </summary>
        public String APN_APT_Name
        { set; get; }
        /// <summary>
        /// 呆滞天数
        /// </summary>
        public Int64? APN_SlackDays
        { set; get; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public String APN_UOM
        { set; get; }
        /// <summary>
        /// 固定计量单位
        /// </summary>
        public Boolean? APN_FixUOM
        { set; get; }
        /// <summary>
        /// 是否适用于APP
        /// </summary>
        public Boolean? APN_IsSuitableForApp
        { set; get; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public Boolean? APN_IsValid
        { set; get; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String APN_CreatedBy
        { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? APN_CreatedTime
        { set; get; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String APN_UpdatedBy
        { set; get; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APN_UpdatedTime
        { set; get; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? APN_VersionNo
        { set; get; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String APN_TransID
        { set; get; }
        #endregion
    }
}
