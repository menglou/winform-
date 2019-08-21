﻿#region COPYRIGHT
//
//     THIS IS GENERATED BY TEMPLATE
//     
//     AUTHOR  :     ROYE
//     DATE       :     2010
//
//     COPYRIGHT (C) 2010, TIANXIAHOTEL TECHNOLOGIES CO., LTD. ALL RIGHTS RESERVED.
//
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Framework.WindowUI.Controls
{
    public struct EditItemInformations
    {
        private string _label;
        private TreeListViewItem _item;
        private int _colindex;

        public EditItemInformations(TreeListViewItem item, int column, string label)
        {
            _item = item; 
            _colindex = column; 
            _label = label; 
            CreationTime = DateTime.Now;
        }

        public string Label
        {
            get { return _label; }
        }

        public TreeListViewItem Item
        {
            get { return _item; }
        }

        public int ColumnIndex
        {
            get { return _colindex; }
        }

        internal DateTime CreationTime;
    }
}
