using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 使用许可证Model
    /// </summary>
    public class MDLSM_ClientUseLicense
    {
        #region 公共属性
        /// <summary>
        /// 许可证ID
        /// </summary>
        public String CUL_ID { get; set; }
        /// <summary>
        /// 许可证号
        /// </summary>
        public String CUL_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String CUL_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String CUL_Org_Name { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public String CUL_Name { get; set; }
        /// <summary>
        /// 申请原因
        /// </summary>
        public String CUL_ApplyReason { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public String CUL_ContactNo { get; set; }
        /// <summary>
        /// 网卡类型1
        /// </summary>
        public String CUL_NetworkCardType1 { get; set; }
        /// <summary>
        /// 网卡地址1
        /// </summary>
        public String CUL_MACAdress1 { get; set; }
        /// <summary>
        /// 网卡类型2
        /// </summary>
        public String CUL_NetworkCardType2 { get; set; }
        /// <summary>
        /// 网卡地址2
        /// </summary>
        public String CUL_MACAdress2 { get; set; }
        /// <summary>
        /// 网卡类型3
        /// </summary>
        public String CUL_NetworkCardType3 { get; set; }
        /// <summary>
        /// 网卡地址3
        /// </summary>
        public String CUL_MACAdress3 { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public String CUL_ApproveStatus { get; set; }
        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime? CUL_InvalidDate { get; set; }
        /// <summary>
        /// 失效日期-开始（查询条件用）
        /// </summary>
        public DateTime? _InvalidDateStart { get; set; }
        /// <summary>
        /// 失效日期-终了（查询条件用）
        /// </summary>
        public DateTime? _InvalidDateEnd { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String CUL_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? CUL_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String CUL_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CUL_CreatedTime { get; set; }
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
        public String CUL_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? CUL_UpdatedTime { get; set; }
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
        public Int64? CUL_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String CUL_TransID { get; set; }
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
        /// 许可证ID
        /// </summary>
        public String WHERE_CUL_ID { get; set; }
        /// <summary>
        /// 许可证号
        /// </summary>
        public String WHERE_CUL_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_CUL_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String WHERE_CUL_Org_Name { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public String WHERE_CUL_Name { get; set; }
        /// <summary>
        /// 申请原因
        /// </summary>
        public String WHERE_CUL_ApplyReason { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public String WHERE_CUL_ContactNo { get; set; }
        /// <summary>
        /// 网卡类型1
        /// </summary>
        public String WHERE_CUL_NetworkCardType1 { get; set; }
        /// <summary>
        /// 网卡地址1
        /// </summary>
        public String WHERE_CUL_MACAdress1 { get; set; }
        /// <summary>
        /// 网卡类型2
        /// </summary>
        public String WHERE_CUL_NetworkCardType2 { get; set; }
        /// <summary>
        /// 网卡地址2
        /// </summary>
        public String WHERE_CUL_MACAdress2 { get; set; }
        /// <summary>
        /// 网卡类型3
        /// </summary>
        public String WHERE_CUL_NetworkCardType3 { get; set; }
        /// <summary>
        /// 网卡地址3
        /// </summary>
        public String WHERE_CUL_MACAdress3 { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public String WHERE_CUL_ApproveStatus { get; set; }
        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime? WHERE_CUL_InvalidDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_CUL_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_CUL_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_CUL_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_CUL_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_CUL_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_CUL_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_CUL_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_CUL_TransID { get; set; }
        #endregion

    }
}
