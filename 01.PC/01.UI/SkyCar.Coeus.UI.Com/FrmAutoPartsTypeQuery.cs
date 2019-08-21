using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Infragistics.Win.UltraWinTree;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Framework.WindowUI;

namespace SkyCar.Coeus.UI.Common
{
    /// <summary>
    /// 配件类别查询
    /// </summary>
    public partial class FrmAutoPartsTypeQuery : BaseForm
    {
        #region 全局变量
        /// <summary>
        /// 配件类别查询BLL
        /// </summary>
        private BLLBase _bll = new BLLBase(Trans.COM);

        /// <summary>
        /// Resource对象
        /// </summary>
        ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmAutoPartsTypeQuery));

        /// <summary>
        /// 已查询过的Key
        /// </summary>
        private readonly Dictionary<string, string> _alreadySearchDic = new Dictionary<string, string>();
        #endregion

        #region 公共属性
        /// <summary>
        /// 选中项的值
        /// </summary>
        public string SelectedValue { get; set; }
        /// <summary>
        /// 选中项的描述
        /// </summary>
        public string SelectedText { get; set; }

        #endregion

        #region 系统事件

        /// <summary>
        /// 配件父类名称查询
        /// </summary>
        public FrmAutoPartsTypeQuery()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAutoPartTypeQuery_Load(object sender, EventArgs e)
        {
            //查询配件类别并创建树
            QueryListAndCreateTree();

            txtWhere_APT_Name.Focus();
        }

        /// <summary>
        /// 【查询条件】配件类别ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_APT_Name_ValueChanged(object sender, EventArgs e)
        {
            _alreadySearchDic.Clear();
        }

        /// <summary>
        /// 【查询条件】配件类别KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_APT_Name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TraversTreeToSearch();
            }
        }
        /// <summary>
        /// 选中节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeAutoPartsType_AfterSelect(object sender, SelectEventArgs e)
        {
            if (treeAutoPartsType.SelectedNodes.Count == 0)
            {
                return;
            }
            btnConfirm.Enabled = true;
        }
        /// <summary>
        /// 双击节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeAutoPartsType_DoubleClick(object sender, EventArgs e)
        {
            if (treeAutoPartsType.SelectedNodes.Count == 0)
            {
                return;
            }
            SelectedValue = treeAutoPartsType.SelectedNodes[0].Key;
            SelectedText = treeAutoPartsType.SelectedNodes[0].Text;

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// [清空]按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            //初始化【列表】Tab内控件
            InitializeListTabControls();
        }

        /// <summary>
        /// [查询]按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //查询
            TraversTreeToSearch();
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            SelectedValue = treeAutoPartsType.SelectedNodes[0].Key;
            SelectedText = treeAutoPartsType.SelectedNodes[0].Text;

            if (string.IsNullOrEmpty(SelectedValue)
                || string.IsNullOrEmpty(SelectedText))
            {
                //请选择配件类别
                MessageBoxs.Show(Trans.COM, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { SystemTableEnums.Name.BS_AutoPartsType }), MessageBoxButtons.OK, MessageBoxIcon.Information );
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 查询条件初始化
            //配件类别
            txtWhere_APT_Name.Clear();
            //给 配件类别 设置焦点
            txtWhere_APT_Name.Focus();
            #endregion
        }

        /// <summary>
        /// 查询配件类别并创建树
        /// </summary>
        private void QueryListAndCreateTree()
        {
            List<MDLBS_AutoPartsType> resultAutoPartsTypeList = new List<MDLBS_AutoPartsType>();
            _bll.QueryForList<MDLBS_AutoPartsType>(SQLID.COMM_SQL20, new MDLBS_AutoPartsType
            {
                WHERE_APT_IsValid = true
            }, resultAutoPartsTypeList);

            treeAutoPartsType.Nodes.Clear();
            //根节点列表
            var rootList = resultAutoPartsTypeList.FindAll(x => string.IsNullOrEmpty(x.APT_ParentID));
            for (int i = 0; i < rootList.Count; i++)
            {
                UltraTreeNode node = new UltraTreeNode
                {
                    //Key
                    Key = rootList[i].APT_ID,
                    //显示值
                    Text = rootList[i].APT_Name,
                    //操作
                    DataKey = string.Empty,
                    //版本号
                    Tag = rootList[i].APT_VersionNo
                };
                node.Override.NodeAppearance.ForeColor = Color.Black;
                //node.Override.NodeAppearance.Image = ((object)(resources.GetObject("Query")));
                BuildNode(node, resultAutoPartsTypeList, 2);
                treeAutoPartsType.Nodes.Insert(i, node);
            }
            //展开所有配件类别
            treeAutoPartsType.ExpandAll();
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="paramNode"></param>
        /// <param name="paramAutoPartsTypeList"></param>
        /// <param name="paramLevel"></param>
        private void BuildNode(UltraTreeNode paramNode, List<MDLBS_AutoPartsType> paramAutoPartsTypeList, int paramLevel)
        {
            //根节点列表
            var rootList = paramAutoPartsTypeList.FindAll(r => r.APT_ParentID == paramNode.Key);
            for (int i = 0; i < rootList.Count; i++)
            {
                UltraTreeNode node = new UltraTreeNode()
                {
                    //Key
                    Key = rootList[i].APT_ID,
                    //显示值
                    Text = rootList[i].APT_Name,
                    //操作
                    DataKey = string.Empty,
                    //版本号
                    Tag = rootList[i].APT_VersionNo
                };
                node.Override.NodeAppearance.ForeColor = Color.Black;
                if (paramLevel <= 9)
                {
                    //node.Override.NodeAppearance.Image = ((object)(resources.GetObject("n" + paramLevel)));
                    //node.Override.NodeAppearance.Image = ((object)(resources.GetObject("Query")));
                }
                else
                {
                    //node.Override.NodeAppearance.Image = ((object)(resources.GetObject("Query")));
                }
                BuildNode(node, paramAutoPartsTypeList, paramLevel + 1);
                paramNode.Nodes.Insert(i, node);
            }
        }

        /// <summary>
        /// 遍历Tree
        /// </summary>
        private void TraversTreeToSearch()
        {
            if (string.IsNullOrEmpty(txtWhere_APT_Name.Text.Trim()))
            {
                return;
            }
            int noodCount = 0;
            bool isContinue = false;
            foreach (UltraTreeNode loopNode in treeAutoPartsType.Nodes)
            {
                noodCount++;
                if (!TraversTreeNodeToSearch(loopNode))
                {
                    isContinue = true;
                    continue;
                }
                isContinue = false;
                break;
            }
            if (isContinue && noodCount == treeAutoPartsType.Nodes.Count)
            {
                _alreadySearchDic.Clear();
                txtWhere_APT_Name.SelectAll();
                txtWhere_APT_Name.Focus();

                MessageBoxs.Show(Trans.COM, ToString(), "找不到‘" + txtWhere_APT_Name.Text.Trim() + "’", MessageBoxButtons.OK, MessageBoxIcon.Information );
            }
        }
        /// <summary>
        /// 遍历节点
        /// </summary>
        /// <param name="paramNode"></param>
        private bool TraversTreeNodeToSearch(UltraTreeNode paramNode)
        {
            if (paramNode.Text.Contains(txtWhere_APT_Name.Text.Trim())
                && !_alreadySearchDic.ContainsKey(paramNode.Key))
            {
                paramNode.Selected = true;
                treeAutoPartsType.Focus();
                treeAutoPartsType.ActiveNode = paramNode;

                _alreadySearchDic.Add(paramNode.Key, paramNode.Text);
                return true;
            }
            foreach (UltraTreeNode loopNode in paramNode.Nodes)
            {
                if (TraversTreeNodeToSearch(loopNode))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
