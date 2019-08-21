using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 菜单分组动作UIModel
    /// </summary>
    public class MenuGroupActionQCModel : BaseUIModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public String WHERE_User_ID { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public String WHERE_User_EMPNO { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public String WHERE_Org_ID { get; set; }
    }
}
