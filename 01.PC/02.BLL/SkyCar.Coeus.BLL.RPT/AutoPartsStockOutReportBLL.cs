using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;

namespace SkyCar.Coeus.BLL.RPT
{
    public class AutoPartsStockOutReportBLL : BLLBase
    {
        #region 构造方法

        /// <summary>
        /// 配件出库汇总报表BLL
        /// </summary>
        public AutoPartsStockOutReportBLL() : base(Trans.RPT)
        {

        }

        #endregion
    }
}
