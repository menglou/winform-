using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// BaseQCModel
    /// </summary>
    public class BaseQCModel
    {
        /// <summary>
        /// 当前页面索引/要跳转的页码（翻页用）
        /// </summary>
        public Int32? PageIndex { get; set; }
        /// <summary>
        /// 页面大小（翻页用）
        /// </summary>
        public Int32? PageSize { get; set; }
        /// <summary>
        /// 查询用SqlId
        /// <para>SqlId未指定的场合：使用TBModel进行查询</para>
        /// <para>SqlId指定的场合：使用SqlId对应的SQL语句进行查询</para>
        /// </summary>
        public String SqlId { get; set; }

        private bool _onlyRetrieveRecordCount = false;
        /// <summary>
        /// 是否只返回满足条件的记录数RecordCount,默认为false；如果为true,则结果中只有RecordCount属性有值。
        /// COMMSQL_TB.xml文件中统一处理，开发人员自己写SQL语句时参照。
        /// </summary>
        public bool OnlyRetrieveRecordCount
        {
            get
            {
                return _onlyRetrieveRecordCount;
            }
            set
            {
                _onlyRetrieveRecordCount = value;
            }
        }

    }
}
