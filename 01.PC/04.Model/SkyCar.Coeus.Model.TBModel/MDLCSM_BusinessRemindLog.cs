using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 业务提醒日志Model
    /// </summary>
    public class MDLCSM_BusinessRemindLog
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String BRL_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String BRL_Org_ID { get; set; }
        /// <summary>
        /// 组织
        /// </summary>
        public String BRL_Org_Name { get; set; }
        /// <summary>
        /// 业务类别
        /// </summary>
        public String BRL_BJ_BusinessType { get; set; }
        /// <summary>
        /// 被提醒对象类别
        /// </summary>
        public String BRL_RemindObjectType { get; set; }
        /// <summary>
        /// 被提醒对象
        /// </summary>
        public String BRL_RemindObject { get; set; }
        /// <summary>
        /// 交强险保险公司
        /// </summary>
        public String BRL_ComInsuranceCompany { get; set; }
        /// <summary>
        /// 商业险保险公司
        /// </summary>
        public String BRL_BusInsuranceCompany { get; set; }
        /// <summary>
        /// 相关日期
        /// </summary>
        public DateTime? BRL_RelateDate { get; set; }
        /// <summary>
        /// 相关日期-开始（查询条件用）
        /// </summary>
        public DateTime? _RelateDateStart { get; set; }
        /// <summary>
        /// 相关日期-终了（查询条件用）
        /// </summary>
        public DateTime? _RelateDateEnd { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String BRL_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? BRL_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String BRL_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? BRL_CreatedTime { get; set; }
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
        public String BRL_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? BRL_UpdatedTime { get; set; }
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
        public Int64? BRL_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String BRL_TransID { get; set; }
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
        public String WHERE_BRL_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_BRL_Org_ID { get; set; }
        /// <summary>
        /// 组织
        /// </summary>
        public String WHERE_BRL_Org_Name { get; set; }
        /// <summary>
        /// 业务类别
        /// </summary>
        public String WHERE_BRL_BJ_BusinessType { get; set; }
        /// <summary>
        /// 被提醒对象类别
        /// </summary>
        public String WHERE_BRL_RemindObjectType { get; set; }
        /// <summary>
        /// 被提醒对象
        /// </summary>
        public String WHERE_BRL_RemindObject { get; set; }
        /// <summary>
        /// 交强险保险公司
        /// </summary>
        public String WHERE_BRL_ComInsuranceCompany { get; set; }
        /// <summary>
        /// 商业险保险公司
        /// </summary>
        public String WHERE_BRL_BusInsuranceCompany { get; set; }
        /// <summary>
        /// 相关日期
        /// </summary>
        public DateTime? WHERE_BRL_RelateDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_BRL_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_BRL_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_BRL_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_BRL_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_BRL_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_BRL_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_BRL_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_BRL_TransID { get; set; }
        #endregion

    }
}
