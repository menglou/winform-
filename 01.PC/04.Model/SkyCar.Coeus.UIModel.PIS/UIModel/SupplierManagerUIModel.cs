using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 供应商管理UIModel
    /// </summary>
    public class SupplierManagerUIModel : BaseUIModel
    {
        private bool _isChecked = false;
        /// <summary>
        /// 选择
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    PropertyValueChanged = true;
                }
                _isChecked = value;
            }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public String SUPP_Name { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        public String SUPP_ShortName { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public String SUPP_Code { get; set; }
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
        /// 修改人
        /// </summary>
        public String SUPP_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SUPP_UpdatedTime { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public String SUPP_ID { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? SUPP_VersionNo { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public String Prov_Name { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public String City_Name { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public String Dist_Name { get; set; }
    }
}
