using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.Common.Enums
{
    /// <summary>
    /// 参数操作枚举
    /// </summary>
    public enum Operation
    {
        /// <summary>
        /// 什么都不做
        /// </summary>
        None,
        /// <summary>
        /// 显示
        /// <para>构造方法内需要做以下处理：</para>
        /// <para>①Tab定位到[详情]</para>
        /// <para>②实参Model属性复制到形参Model属性</para>
        /// </summary>
        Show,
        /// <summary>
        /// 新建
        /// <para>构造方法内需要做以下处理：</para>
        /// <para>①Tab定位到[新建]</para>
        /// <para>②保存后，需要将保存信息赋值给公开属性（ResultModel）</para>
        /// </summary>
        New,
        /// <summary>
        /// 查询
        /// <para>构造方法内需要做以下处理：</para>
        /// <para>①实参Model属性复制到形参Model（QCModel）属性</para>
        /// <para>②根据QCModel进行查询</para>
        /// <para>③结果不存在的场合，Tab定位到[新建]，实参Model属性复制到形参Model（UIModel）属性</para>
        /// <para>④结果存在的场合，Tab定位到[列表]，Grid内显示查询结果</para>
        /// </summary>
        Query
    }
}
