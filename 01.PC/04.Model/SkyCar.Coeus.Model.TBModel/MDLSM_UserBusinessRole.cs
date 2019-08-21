using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 用户业务角色Model
    /// </summary>
    public class MDLSM_UserBusinessRole
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String UBR_ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String UBR_User_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String UBR_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String UBR_Org_Name { get; set; }
        /// <summary>
        /// 班组名称
        /// </summary>
        public String UBR_GroupName { get; set; }
        /// <summary>
        /// 业务角色
        /// </summary>
        public String UBR_BusinessRole { get; set; }
        /// <summary>
        /// 业务工种
        /// </summary>
        public String UBR_Jobs { get; set; }
        /// <summary>
        /// 行业证件类型
        /// </summary>
        public String UBR_CertType { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public String UBR_CertNo { get; set; }
        /// <summary>
        /// 能力级别
        /// </summary>
        public Int32? UBR_TecLevel { get; set; }
        /// <summary>
        /// 绩效系数
        /// </summary>
        public Decimal? UBR_PerformanceRatio { get; set; }
        /// <summary>
        /// 是否适用于APP
        /// </summary>
        public Boolean? UBR_IsSuitableForApp { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? UBR_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String UBR_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? UBR_CreatedTime { get; set; }
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
        public String UBR_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UBR_UpdatedTime { get; set; }
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
        public Int64? UBR_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String UBR_TransID { get; set; }
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
        public String WHERE_UBR_ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String WHERE_UBR_User_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_UBR_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String WHERE_UBR_Org_Name { get; set; }
        /// <summary>
        /// 班组名称
        /// </summary>
        public String WHERE_UBR_GroupName { get; set; }
        /// <summary>
        /// 业务角色
        /// </summary>
        public String WHERE_UBR_BusinessRole { get; set; }
        /// <summary>
        /// 业务工种
        /// </summary>
        public String WHERE_UBR_Jobs { get; set; }
        /// <summary>
        /// 行业证件类型
        /// </summary>
        public String WHERE_UBR_CertType { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public String WHERE_UBR_CertNo { get; set; }
        /// <summary>
        /// 能力级别
        /// </summary>
        public Int32? WHERE_UBR_TecLevel { get; set; }
        /// <summary>
        /// 绩效系数
        /// </summary>
        public Decimal? WHERE_UBR_PerformanceRatio { get; set; }
        /// <summary>
        /// 是否适用于APP
        /// </summary>
        public Boolean? WHERE_UBR_IsSuitableForApp { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_UBR_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_UBR_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_UBR_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_UBR_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_UBR_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_UBR_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_UBR_TransID { get; set; }
        #endregion

    }
}
