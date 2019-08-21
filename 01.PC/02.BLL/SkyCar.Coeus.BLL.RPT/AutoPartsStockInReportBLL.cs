using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;

namespace SkyCar.Coeus.BLL.RPT
{
    public class AutoPartsStockInReportBLL : BLLBase
    {
        #region 构造方法

        /// <summary>
        /// 配件入库汇总报表BLL
        /// </summary>
        public AutoPartsStockInReportBLL() : base(Trans.RPT)
        {

        }

        #endregion
    }
}
