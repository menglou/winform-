using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.PIS;
using SkyCar.Coeus.Common.Enums;

namespace SkyCar.Coeus.BLL.PIS
{
    /// <summary>
    /// 仓位管理BLL
    /// </summary>
    public class WarehouseBinManagerBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll =new BLLBase(Trans.PIS);
        #endregion

        #region 公共属性

        #endregion

        #region 构造方法

        /// <summary>
        /// 仓位管理BLL
        /// </summary>
        public WarehouseBinManagerBLL() : base(Trans.PIS)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        public bool SaveDetailDS(WarehouseBinManagerUIModel paramModel)
        {
            //服务端检查
            if (!ServerCheck(paramModel))
            {
                return false;
            }

            #region 事务，多数据表操作

            //将UIModel转为TBModel
            var argsWarehouseBin = CopyModel<MDLPIS_WarehouseBin>(paramModel);

            #region 新增
            //判断主键是否被赋值
            if (string.IsNullOrEmpty(argsWarehouseBin.WHB_ID))
            {
                //生成新ID
                argsWarehouseBin.WHB_ID = Guid.NewGuid().ToString();
                argsWarehouseBin.WHB_CreatedBy = LoginInfoDAX.UserName;
                argsWarehouseBin.WHB_CreatedTime = BLLCom.GetCurStdDatetime();
                argsWarehouseBin.WHB_UpdatedBy = LoginInfoDAX.UserName;
                argsWarehouseBin.WHB_UpdatedTime = BLLCom.GetCurStdDatetime();
                //主键未被赋值，则执行新增
                if (!_bll.Insert(argsWarehouseBin))
                {
                    //新增[仓位]信息失败
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.ADD + SystemTableEnums.Name.PIS_WarehouseBin });
                    return false;
                }
            }
            #endregion

            #region 更新
            else
            {
                //主键被赋值，则需要更新，更新需要设定更新条件
                argsWarehouseBin.WHERE_WHB_ID = argsWarehouseBin.WHB_ID;
                argsWarehouseBin.WHERE_WHB_VersionNo = argsWarehouseBin.WHB_VersionNo;
                argsWarehouseBin.WHB_VersionNo++;
                argsWarehouseBin.WHB_UpdatedBy = LoginInfoDAX.UserName;
                argsWarehouseBin.WHB_UpdatedTime = BLLCom.GetCurStdDatetime();
                if (!_bll.Update(argsWarehouseBin))
                {
                    //更新[仓位]信息失败
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.PIS_WarehouseBin });
                    return false;
                }
            }
            #endregion

            //将最新数据回写给DetailDS
            CopyModel(argsWarehouseBin, paramModel);

            #endregion

            return true;
        }

       
        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        private bool ServerCheck(WarehouseBinManagerUIModel paramModel)
        {
            return true;
        }

        #endregion
    }
}
