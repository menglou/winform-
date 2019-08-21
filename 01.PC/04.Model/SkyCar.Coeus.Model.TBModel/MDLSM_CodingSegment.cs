using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 编码段Model
    /// </summary>
    public class MDLSM_CodingSegment
    {
        #region 公共属性
        /// <summary>
        /// 编码段ID
        /// </summary>
        public String CS_ID { get; set; }
        /// <summary>
        /// 编码规则ID
        /// </summary>
        public String CS_ER_ID { get; set; }
        /// <summary>
        /// 编码段类型编码
        /// </summary>
        public String CS_TypeCode { get; set; }
        /// <summary>
        /// 编码段类型名称
        /// </summary>
        public String CS_TypeName { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public Int32? CS_Length { get; set; }
        /// <summary>
        /// 填充字符
        /// </summary>
        public String CS_PanddingChar { get; set; }
        /// <summary>
        /// 填充方式编码
        /// </summary>
        public String CS_PanddingStyleCode { get; set; }
        /// <summary>
        /// 填充方式名称
        /// </summary>
        public String CS_PanddingStyleName { get; set; }
        /// <summary>
        /// 编码段值
        /// </summary>
        public String CS_Value { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? CS_Index { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? CS_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String CS_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CS_CreatedTime { get; set; }
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
        public String CS_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? CS_UpdatedTime { get; set; }
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
        public Int64? CS_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String CS_TransID { get; set; }
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
        /// 编码段ID
        /// </summary>
        public String WHERE_CS_ID { get; set; }
        /// <summary>
        /// 编码规则ID
        /// </summary>
        public String WHERE_CS_ER_ID { get; set; }
        /// <summary>
        /// 编码段类型编码
        /// </summary>
        public String WHERE_CS_TypeCode { get; set; }
        /// <summary>
        /// 编码段类型名称
        /// </summary>
        public String WHERE_CS_TypeName { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public Int32? WHERE_CS_Length { get; set; }
        /// <summary>
        /// 填充字符
        /// </summary>
        public String WHERE_CS_PanddingChar { get; set; }
        /// <summary>
        /// 填充方式编码
        /// </summary>
        public String WHERE_CS_PanddingStyleCode { get; set; }
        /// <summary>
        /// 填充方式名称
        /// </summary>
        public String WHERE_CS_PanddingStyleName { get; set; }
        /// <summary>
        /// 编码段值
        /// </summary>
        public String WHERE_CS_Value { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? WHERE_CS_Index { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_CS_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_CS_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_CS_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_CS_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_CS_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_CS_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_CS_TransID { get; set; }
        #endregion

    }
}
