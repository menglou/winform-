using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 供应商管理Model
    /// </summary>
    public class MDLPIS_Supplier
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String SUPP_ID { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public String SUPP_Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String SUPP_Name { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        public String SUPP_ShortName { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public String SUPP_Contacter { get; set; }
        /// <summary>
        /// 固定号码
        /// </summary>
        public String SUPP_Tel { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public String SUPP_Phone { get; set; }
        /// <summary>
        /// QQ号码
        /// </summary>
        public String SUPP_QQ { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public String SUPP_Territory { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public String SUPP_Prov_Code { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public String SUPP_City_Code { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        public String SUPP_Dist_Code { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public String SUPP_Address { get; set; }
        /// <summary>
        /// 评估等级
        /// </summary>
        public String SUPP_EvaluateLevel { get; set; }
        /// <summary>
        /// 最近评估日
        /// </summary>
        public DateTime? SUPP_LastEvaluateDate { get; set; }
        /// <summary>
        /// 最近评估日-开始（查询条件用）
        /// </summary>
        public DateTime? _LastEvaluateDateStart { get; set; }
        /// <summary>
        /// 最近评估日-终了（查询条件用）
        /// </summary>
        public DateTime? _LastEvaluateDateEnd { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public String SUPP_BankName { get; set; }
        /// <summary>
        /// 开户名
        /// </summary>
        public String SUPP_BankAccountName { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public String SUPP_BankAccountNo { get; set; }
        /// <summary>
        /// 主营配件
        /// </summary>
        public String SUPP_MainAutoParts { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String SUPP_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SUPP_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SUPP_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SUPP_CreatedTime { get; set; }
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
        public String SUPP_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SUPP_UpdatedTime { get; set; }
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
        public Int64? SUPP_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String SUPP_TransID { get; set; }
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
        public String WHERE_SUPP_ID { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public String WHERE_SUPP_Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String WHERE_SUPP_Name { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        public String WHERE_SUPP_ShortName { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public String WHERE_SUPP_Contacter { get; set; }
        /// <summary>
        /// 固定号码
        /// </summary>
        public String WHERE_SUPP_Tel { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public String WHERE_SUPP_Phone { get; set; }
        /// <summary>
        /// QQ号码
        /// </summary>
        public String WHERE_SUPP_QQ { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public String WHERE_SUPP_Territory { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public String WHERE_SUPP_Prov_Code { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public String WHERE_SUPP_City_Code { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        public String WHERE_SUPP_Dist_Code { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public String WHERE_SUPP_Address { get; set; }
        /// <summary>
        /// 评估等级
        /// </summary>
        public String WHERE_SUPP_EvaluateLevel { get; set; }
        /// <summary>
        /// 最近评估日
        /// </summary>
        public DateTime? WHERE_SUPP_LastEvaluateDate { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public String WHERE_SUPP_BankName { get; set; }
        /// <summary>
        /// 开户名
        /// </summary>
        public String WHERE_SUPP_BankAccountName { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public String WHERE_SUPP_BankAccountNo { get; set; }
        /// <summary>
        /// 主营配件
        /// </summary>
        public String WHERE_SUPP_MainAutoParts { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_SUPP_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_SUPP_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_SUPP_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_SUPP_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_SUPP_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_SUPP_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_SUPP_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_SUPP_TransID { get; set; }
        #endregion

    }
}
