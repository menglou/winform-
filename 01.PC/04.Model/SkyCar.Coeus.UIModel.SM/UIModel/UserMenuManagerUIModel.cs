using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SM
{
    /// <summary>
    /// 用户菜单管理（组织）UIModel
    /// </summary>
    public class UserMenuManagerUIModel : BaseUIModel
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public String Menu_ID { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public String Menu_Name { get; set; }
        /// <summary>
        /// 菜单明细ID
        /// </summary>
        public String UMA_MenuD_ID { get; set; }
        /// <summary>
        /// 菜单明细名称
        /// </summary>
        public String MenuD_Name { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? UMA_CreatedTime { get; set; }
        
    }
}
