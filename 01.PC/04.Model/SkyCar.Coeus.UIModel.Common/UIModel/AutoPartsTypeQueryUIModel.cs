using System;
using System.Collections.Generic;
using Microsoft.Practices.Prism.ViewModel;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 配件类别查询UIModel
    /// </summary>
    public class AutoPartsTypeQueryUIModel : NotificationObject
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public AutoPartsTypeQueryUIModel()
        {
            this.Nodes = new List<AutoPartsTypeQueryUIModel>();
        }

        private List<AutoPartsTypeQueryUIModel> _nodes;

        /// <summary>
        /// 此节点拥有的子级列表
        /// </summary>
        public List<AutoPartsTypeQueryUIModel> Nodes
        {
            get { return _nodes; }
            set
            {
                _nodes = value;
                RaisePropertyChanged(() => Nodes);
            }
        }
        private string _apt_id;
        /// <summary>
        /// 配件类别ID
        /// </summary>
        public string APT_ID
        {
            get { return _apt_id; }
            set
            {
                _apt_id = value;
                RaisePropertyChanged(() => APT_ID);
            }
        }

        private string _apt_name;
        /// <summary>
        /// 配件类别名称
        /// </summary>
        public string APT_Name
        {
            get { return _apt_name; }
            set
            {
                _apt_name = value;
                RaisePropertyChanged(() => APT_Name);
            }
        }

        private string _apt_parentid;
        /// <summary>
        /// 父级类别ID
        /// </summary>
        public string APT_ParentID
        {
            get { return _apt_parentid; }
            set
            {
                _apt_parentid = value;
                RaisePropertyChanged(() => APT_ParentID);
            }
        }
        ///// <summary>
        ///// 行标识
        ///// </summary>
        //public string RowID { get; set; }
        ///// <summary>
        ///// 是否选中
        ///// </summary>
        //public bool IsChecked { get; set; }
        ///// <summary>
        ///// 类别名称
        ///// </summary>
        //public String APT_Name { get; set; }
        ///// <summary>
        ///// 类别ID
        ///// </summary>
        //public String APT_ID { get; set; }
        ///// <summary>
        ///// 父级类别ID
        ///// </summary>
        //public String APT_ParentID { get; set; }

        private bool _isSelected = false;
        /// <summary>
        /// 判断节点是否被选择
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
                if (IsSelected)
                {
                    SelectedItem = this;
                }
            }
        }

        private static AutoPartsTypeQueryUIModel _selectedItem = null;
        /// <summary>
        /// 被选择的节点
        /// </summary>
        public static AutoPartsTypeQueryUIModel SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; }
        }
        private bool _isCanSelect = true;
        /// <summary>
        /// 判断节点是否可被选择
        /// </summary>
        public bool IsCanSelect
        {
            get { return _isCanSelect; }
            set
            {
                _isCanSelect = value;
                RaisePropertyChanged(() => IsCanSelect);
            }
        }
    }
}
