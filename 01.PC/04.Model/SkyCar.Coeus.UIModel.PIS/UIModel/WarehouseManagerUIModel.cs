using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 仓库管理UIModel
    /// </summary>
    public class WarehouseManagerUIModel : BaseUIModel
    {
        /// <summary>
        /// 仓库名称
        /// </summary>
        public String WH_Name { get; set; }
        /// <summary>
        /// 仓库编号
        /// </summary>
        public String WH_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WH_Org_ID { get; set; }
        /// <summary>
        /// 仓库地址
        /// </summary>
        public String WH_Address { get; set; }
        /// <summary>
        /// 仓库描述
        /// </summary>
        public String WH_Description { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WH_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WH_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WH_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WH_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WH_UpdatedTime { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WH_ID { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WH_VersionNo { get; set; }

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
        /// 组织名称
        /// </summary>
        public String OrgName { get; set; }
        
    }
}
