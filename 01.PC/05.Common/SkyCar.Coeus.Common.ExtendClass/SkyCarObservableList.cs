using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.ExtendClass.Interface;

namespace SkyCar.Coeus.Common.ExtendClass
{
    /// <summary>
    /// 云车扩展的ObservableCollection
    /// </summary>
    /// <typeparam name="TS">界面的UIModel类型</typeparam>
    /// <typeparam name="TD">对应的TBModel类型</typeparam>
    public class SkyCarObservableList<TS, TD> : ObservableCollection<TS> where TS : IPropertyChanged
    {
        /// <summary>
        /// 集合发生变化Action
        /// </summary>
        public Action CollectionChangedAction;

        private List<TS> _deleteList;
        /// <summary>
        /// 删除列表
        /// </summary>
        public IList<TS> DeleteList
        {
            get
            {
                List<TS> tempDeleteList = new List<TS>();
                foreach (var loopDeleteItem in _deleteList)
                {
                    bool existsValidItem = false;
                    foreach (var loopValidItem in this)
                    {
                        if (IdHasValue(loopValidItem) && GetIdValue(loopValidItem) == GetIdValue(loopDeleteItem))
                        {
                            existsValidItem = true;
                            break;
                        }
                    }
                    if (!existsValidItem)
                    {
                        tempDeleteList.Add(loopDeleteItem);
                    }
                }

                return tempDeleteList.AsReadOnly();
            }
        }
        /// <summary>
        /// 更新列表
        /// </summary>
        public IList<TS> UpdateList
        {
            get { return (from x in this where IdHasValue(x) && x.PropertyValueChanged select x).ToList().AsReadOnly(); }
        }
        /// <summary>
        /// 插入列表
        /// </summary>
        public IList<TS> InsertList
        {
            get { return (from x in this where !IdHasValue(x) select x).ToList().AsReadOnly(); }
        }

        private bool _hasChanges = false;
        /// <summary>
        /// 标识列表是否变化
        /// </summary>
        public bool HasChanges
        {
            get
            {
                return InsertList != null && InsertList.Count > 0 ||
                    UpdateList != null && UpdateList.Count > 0 ||
                    DeleteList != null && DeleteList.Count > 0;
            }
            private set
            {
                _hasChanges = value;
            }
        }

        public SkyCarObservableList()
            : base()
        {
            InitStateList();
        }

        public SkyCarObservableList(List<TS> list)
            : base(list)
        {
            InitStateList();
        }

        public SkyCarObservableList(IEnumerable<TS> collection)
            : base(collection)
        {
            InitStateList();
        }

        private void InitStateList()
        {
            _deleteList = new List<TS>();
            ApplyCollectionChanged();
        }

        void ApplyCollectionChanged()
        {
            if (CollectionChangedAction != null)
            {
                CollectionChangedAction();
            }
        }
        public void AcceptChanges(bool paramIsDelete = true)
        {
            if (paramIsDelete)
            {
                _deleteList.Clear();
            }
            foreach (var loopUpdateItem in this.UpdateList)
            {
                loopUpdateItem.PropertyValueChanged = false;
            }
            HasChanges = false;
        }
        /// <summary>
        /// 移除某项        
        /// </summary>
        /// <param name="item">待移除的项</param>
        /// <returns></returns>
        public new bool Remove(TS item)
        {
            var result = base.Remove(item);
            if (result)
            {
                HasChanges = true;
                if (IdHasValue(item))
                {
                    if (!_deleteList.Contains(item))
                    {
                        _deleteList.Add(item);
                    }
                }
            }
            ApplyCollectionChanged();
            return result;
        }
        /// <summary>
        /// 移除所有项（会产生待删除的列表）
        /// </summary>
        /// <returns></returns>
        public bool RemoveAll()
        {
            bool result = true;
            List<TS> tempList = new List<TS>(this);
            foreach (var item in tempList)
            {
                result = this.Remove(item);
                if (result == false)
                {
                    break;
                }
            }
            ApplyCollectionChanged();
            return result;
        }
        /// <summary>
        /// 清除列表
        /// </summary>
        public new void Clear()
        {
            _deleteList.Clear();
            base.Clear();
            ApplyCollectionChanged();
        }

        public new void Insert(int index, TS item)
        {
            HasChanges = true;
            base.Insert(index, item);
            ApplyCollectionChanged();
        }

        public new void Add(TS item)
        {
            HasChanges = true;
            base.Add(item);
            ApplyCollectionChanged();
        }


        private string GetIdValue(TS item)
        {
            bool flag = false;
            Type type = item.GetType();
            object tempIdValue = null;
            foreach (var loopProperty in type.GetProperties())
            {
                string[] propertyNameArray = !string.IsNullOrEmpty(loopProperty.Name) ? loopProperty.Name.Split(SysConst.ULINE.ToCharArray()) : null;
                if (propertyNameArray != null && propertyNameArray.Length == 2 && SysConst.EN_ID.Equals(propertyNameArray[1]) && typeof(TD).GetProperty(loopProperty.Name) != null)
                {
                    tempIdValue = loopProperty.GetValue(item, null);
                    break;
                }
            }
            return tempIdValue?.ToString() ?? string.Empty;
        }

        private bool IdHasValue(TS item)
        {
            bool flag = false;
            Type type = item.GetType();
            foreach (var loopProperty in type.GetProperties())
            {
                string[] propertyNameArray = !string.IsNullOrEmpty(loopProperty.Name) ? loopProperty.Name.Split(SysConst.ULINE.ToCharArray()) : null;
                if (propertyNameArray != null && propertyNameArray.Length == 2 && SysConst.EN_ID.Equals(propertyNameArray[1]) && typeof(TD).GetProperty(loopProperty.Name) != null)
                {
                    var vaule = loopProperty.GetValue(item, null);
                    if (vaule != null && vaule.ToString() != string.Empty)
                    {
                        flag = true;
                    }
                    break;
                }
            }
            return flag;
        }

        public void ForEach(Action<TS> action)
        {
            if (action != null)
            {
                foreach (var item in this)
                {
                    action(item);
                }
            }

        }

    }
}
