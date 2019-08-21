using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 供应商查询QCModel
    /// </summary>
    public class SupplierQueryQCModel : BaseQCModel
    {
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
