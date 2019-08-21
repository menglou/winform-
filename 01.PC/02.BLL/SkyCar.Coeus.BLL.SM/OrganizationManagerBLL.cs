using System;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.SM;

namespace SkyCar.Coeus.BLL.SM
{
    /// <summary>
    /// 门店管理BLL
    /// </summary>
    public class OrganizationManagerBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll =new BLLBase(Trans.SM);

        #endregion

        #region 公共属性

        #endregion

        #region 构造方法

        /// <summary>
        /// 门店管理BLL
        /// </summary>
        public OrganizationManagerBLL() : base(Trans.SM)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        public bool SaveDetailDS(OrganizationManagerUIModel paramModel)
        {
            //服务端检查
            if (!ServerCheck(paramModel))
            {
                return false;
            }

            //将UIModel转为TBModel
            var argsOrganization = CopyModel<MDLSM_Organization>(paramModel);

            //判断主键是否被赋值
            if (string.IsNullOrEmpty(argsOrganization.Org_ID))
            {
                //生成新ID
                argsOrganization.Org_ID = Guid.NewGuid().ToString();
                argsOrganization.Org_CreatedBy = LoginInfoDAX.UserName;
                argsOrganization.Org_CreatedTime = BLLCom.GetCurStdDatetime();
                argsOrganization.Org_UpdatedBy = LoginInfoDAX.UserName;
                argsOrganization.Org_UpdatedTime = BLLCom.GetCurStdDatetime();
                //主键未被赋值，则执行新增
                if (!_bll.Insert(argsOrganization))
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + MsgParam.ORGNIZATION });
                    return false;
                }
            }
            else
            {
                //主键被赋值，则需要更新，更新需要设定更新条件
                argsOrganization.WHERE_Org_ID = argsOrganization.Org_ID;
                argsOrganization.WHERE_Org_VersionNo = argsOrganization.Org_VersionNo;
                argsOrganization.Org_VersionNo++;
                argsOrganization.Org_UpdatedBy = LoginInfoDAX.UserName;
                argsOrganization.Org_UpdatedTime = BLLCom.GetCurStdDatetime();
                if (!_bll.Update(argsOrganization))
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + MsgParam.ORGNIZATION });
                    return false;
                }
            }

            //将最新数据回写给DetailDS
            CopyModel(argsOrganization, paramModel);

            return true;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        private bool ServerCheck(OrganizationManagerUIModel paramModel)
        {
            return true;
        }

        #endregion
    }
}
