using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Framework.WindowUI;

namespace SkyCar.Coeus.Ult.Entrance
{

    public partial class FrmClientUseLicense : BaseForm
    {

        #region 变量
        LoginInfoDAX dax = new LoginInfoDAX();
        public string CopyRightDesc = string.Empty;
        private MDLSM_ClientUseLicense argsLicense = new MDLSM_ClientUseLicense();
        #endregion

        public FrmClientUseLicense()
        {
            InitializeComponent();
            CopyRightDesc = string.IsNullOrEmpty(SystemConfigInfo.CopyRightDesc) ? "Copyright@2017 无锡云车物联网科技有限公司版权所有  客服热线：0510-68566886" : SystemConfigInfo.CopyRightDesc;
        
        }

        public FrmClientUseLicense(string paramCulID)
        {
            InitializeComponent();
            argsLicense.CUL_ID = paramCulID;
        }

        private void btnApplyClientUseLicense_Click(object sender, EventArgs e)
        {
            string name = this.txtCULName.Text.Trim();
            string contactNo = this.txtCULContactNo.Text.Trim();
            string applyReason = this.txtCULApplyReason.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("请输入用户名", "用户使用许可证验证", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(contactNo))
            {
                MessageBox.Show("请输入联系方式", "用户使用许可证验证", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(applyReason))
            {
                MessageBox.Show("请输入申请原因", "用户使用许可证验证", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int netCardIndex = 0;
            NetworkInterface[] fNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in fNetworkInterfaces)
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet
                    || adapter.NetworkInterfaceType.ToString().StartsWith(SysConst.WLAN_NAME))
                {
                    netCardIndex++;
                    if (netCardIndex == 1)
                    {
                        if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                        {
                            argsLicense.CUL_NetworkCardType1 = NetworkCardTypeEnum.Name.LOCAL;
                        }
                        else if (adapter.NetworkInterfaceType.ToString().Contains(SysConst.WLAN_NAME))
                        {
                            argsLicense.CUL_NetworkCardType1 = NetworkCardTypeEnum.Name.WIRELESSLAN;
                        }
                        argsLicense.CUL_MACAdress1 = adapter.GetPhysicalAddress().ToString();
                    }
                    if (netCardIndex == 2)
                    {
                        if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                        {
                            argsLicense.CUL_NetworkCardType2 = NetworkCardTypeEnum.Name.LOCAL;
                        }
                        else if (adapter.NetworkInterfaceType.ToString().Contains(SysConst.WLAN_NAME))
                        {
                            argsLicense.CUL_NetworkCardType2 = NetworkCardTypeEnum.Name.WIRELESSLAN;
                        }
                        argsLicense.CUL_MACAdress2 = adapter.GetPhysicalAddress().ToString();
                    }
                    if (netCardIndex == 3)
                    {
                        if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                        {
                            argsLicense.CUL_NetworkCardType3 = NetworkCardTypeEnum.Name.LOCAL;
                        }
                        else if (adapter.NetworkInterfaceType.ToString().Contains(SysConst.WLAN_NAME))
                        {
                            argsLicense.CUL_NetworkCardType3 = NetworkCardTypeEnum.Name.WIRELESSLAN;
                        }
                        argsLicense.CUL_MACAdress3 = adapter.GetPhysicalAddress().ToString();
                    }
                }
            }
            //TODO
            List<MDLSM_ClientUseLicense> resultLicence =new List<MDLSM_ClientUseLicense>(); // dax.GetClientUseLicenses(argsLicense);
            if (resultLicence.Count != 0 && resultLicence[0].CUL_ApproveStatus == ApproveStatusEnum.Name.APPROVEFAILED)
            {
                argsLicense.CUL_ApproveStatus = ApproveStatusEnum.Name.TOAPPROVE;
                argsLicense.CUL_Remark = null;
              //  bool boolTmp = dax.Update<MDLSM_ClientUseLicense>(argsLicense);
                if (!dax.Update(argsLicense))//更新已处理
                {
                    //回滚事务
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    MessageBox.Show("保存失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("修改申请成功，请等待管理员审核！", "提交成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    return;
                }
            }
            else
            {
                argsLicense.CUL_No = BLLCom.GetCurStdDatetime().ToString(FormatConst.DATE_TIME_FORMAT_03);
                argsLicense.CUL_ApproveStatus = ApproveStatusEnum.Name.TOAPPROVE;
                argsLicense.CUL_CreatedBy = LoginInfoDAX.UserName;
                bool boolTmp = dax.Save<MDLSM_ClientUseLicense>(argsLicense);
                if (!boolTmp)
                {
                    //回滚事务
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    MessageBox.Show("保存失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("提交申请成功，请等待管理员审核！", "提交成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    return;
                }
            }
        }
    }
}
