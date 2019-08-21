using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 菜单分组动作UIModel
    /// </summary>
    public class MenuGroupActionUIModel : BaseUIModel
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public String Menu_ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Menu_Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public String Menu_Code { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? Menu_Index { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        public Boolean? Menu_IsVisible { get; set; }

        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? Menu_IsValid { get; set; }

        /// <summary>
        /// 菜单分组ID
        /// </summary>
        public String MenuG_ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String MenuG_Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public String MenuG_Code { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? MenuG_Index { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        public Boolean? MenuG_IsVisible { get; set; }

        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? MenuG_IsValid { get; set; }

        /// <summary>
        /// 菜单明细ID
        /// </summary>
        public String MenuD_ID { get; set; }


        /// <summary>
        /// 名称
        /// </summary>
        public String MenuD_Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public String MenuD_Code { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public String MenuD_Picture { get; set; }

        /// <summary>
        /// 图标Key
        /// </summary>
        public String MenuD_ImgListKey { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? MenuD_Index { get; set; }

        /// <summary>
        /// 类全名
        /// </summary>
        public String MenuD_ClassFullName { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        public Boolean? MenuD_IsVisible { get; set; }

        /// <summary>
        /// Grid页面大小
        /// </summary>
        public Int32? MenuD_GridPageSize { get; set; }

        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? MenuD_IsValid { get; set; }

        /// <summary>
        /// 菜单明细动作ID
        /// </summary>
        public String MDA_ID { get; set; }


        /// <summary>
        /// 用户菜单权限ID
        /// </summary>
        public String UMA_ID { get; set; }

        /// <summary>
        /// 用户菜单动作权限ID
        /// </summary>
        public String UAA_ID { get; set; }

        /// <summary>
        /// 动作ID
        /// </summary>
        public String Act_ID { get; set; }

        /// <summary>
        /// 动作排序索引
        /// </summary>
        public Int32? Act_Index { get; set; }

        /// <summary>
        /// 动作名称
        /// </summary>
        public String Act_Name { get; set; }

        /// <summary>
        /// 动作Key
        /// </summary>
        public String Act_Key { get; set; }
    }
}
