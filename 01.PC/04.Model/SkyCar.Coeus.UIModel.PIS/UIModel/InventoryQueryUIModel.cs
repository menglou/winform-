using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 库存查询UIModel
    /// </summary>
    public class InventoryQueryUIModel : BaseNotificationUIModel
    {
        #region 库存

        /// <summary>
        /// 组织ID
        /// </summary>
        public String INV_Org_ID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String INV_WH_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String Org_ShortName { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        public String WH_Name { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String INV_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String INV_OEMNo { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String INV_Barcode { get; set; }
       
        /// <summary>
        /// 配件名称
        /// </summary>
        public String INV_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String INV_Specification { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? INV_Qty { get; set; }
        #endregion

        #region 配件档案字段
        /// <summary>
        /// 计量单位
        /// </summary>
        public String APA_UOM { get; set; }
        /// <summary>
        /// 配件级别
        /// </summary>
        public String APA_Level { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String APA_Brand { get; set; }
        /// <summary>
        /// 互换码
        /// </summary>
        public String APA_ExchangeCode { get; set; }


        #endregion

        #region 其他属性

        private Boolean _ischecked;
        /// <summary>
        /// 选择
        /// </summary>
        public Boolean IsChecked
        {
            get { return _ischecked; }
            set
            {
                _ischecked = value;
                RaisePropertyChanged(() => IsChecked);
            }
        }
        /// <summary>
        /// 采购金额
        /// </summary>
        public Decimal? PurchaseAmount { get; set; }
        
        #endregion
    }
}
