using System;
using System.Drawing;
using System.Windows.Forms;
using Infragistics.Win.UltraWinEditors;

namespace SkyCar.Coeus.Common.CustomControl
{
    /// <summary>
    /// 图片扩展
    /// </summary>
    public sealed partial class SkyCarPictureExpand : UserControl
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public SkyCarPictureExpand()
        {
            InitializeComponent();

            this.pictureBox.Image = null;
        }

        #region 公共属性

        private string _pictureKey;
        /// <summary>
        /// 图片唯一标识
        /// </summary>
        public string PictureKey
        {
            get { return _pictureKey; }
            set { _pictureKey = value; }
        }

        private object _pictureImage;
        /// <summary>
        /// 图片Image
        /// </summary>
        public object PictureImage
        {
            get { return _pictureImage; }
            set
            {
                pictureBox.Image = value;
                _pictureImage = value;
            }
        }

        private object _propertyModel;
        /// <summary>
        /// 属性Model
        /// </summary>
        public object PropertyModel
        {
            get { return _propertyModel; }
            set { _propertyModel = value; }
        }

        private bool _isChecked;
        /// <summary>
        /// 勾选
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                ckCheckd.Checked = value;
                _isChecked = value;
            }
        }

        #region 可见性

        private bool _isCheckedIsVisible;
        /// <summary>
        /// 勾选是否可见
        /// </summary>
        public bool IsCheckedIsVisible
        {
            get { return _isCheckedIsVisible; }
            set
            {
                ckCheckd.Visible = value;
                _isCheckedIsVisible = value;
            }
        }

        private bool _uploadIsVisible;
        /// <summary>
        /// 上传Button是否可见
        /// </summary>
        public bool UploadIsVisible
        {
            get { return _uploadIsVisible; }
            set
            {
                btnUpload.Visible = value;
                _uploadIsVisible = value;
            }
        }

        private bool _exportIsVisible;
        /// <summary>
        /// 导出Button是否可见
        /// </summary>
        public bool ExportIsVisible
        {
            get { return _exportIsVisible; }
            set
            {
                btnExport.Visible = value;
                _exportIsVisible = value;
            }
        }

        private bool _deleteIsVisible;
        /// <summary>
        /// 删除Button是否可见
        /// </summary>
        public bool DeleteIsVisible
        {
            get { return _deleteIsVisible; }
            set
            {
                btnDelete.Visible = value;
                _deleteIsVisible = value;
            }
        }

        #endregion

        #region 可编辑

        private bool _isCheckedIsEnabled;
        /// <summary>
        /// 勾选是否可编辑
        /// </summary>
        public bool IsCheckedIsEnabled
        {
            get { return _isCheckedIsEnabled; }
            set
            {
                ckCheckd.Enabled = value;
                _isCheckedIsEnabled = value;
            }
        }

        private bool _uploadIsEnabled;
        /// <summary>
        /// 上传Button是否可编辑
        /// </summary>
        public bool UploadIsEnabled
        {
            get { return _uploadIsEnabled; }
            set
            {
                btnUpload.Enabled = value;
                _uploadIsEnabled = value;
            }
        }

        private bool _exportIsEnabled;
        /// <summary>
        /// 导出Button是否可编辑
        /// </summary>
        public bool ExportIsEnabled
        {
            get { return _exportIsEnabled; }
            set
            {
                btnExport.Enabled = value;
                _exportIsEnabled = value;
            }
        }

        private bool _deleteIsEnabled;
        /// <summary>
        /// 删除Button是否可编辑
        /// </summary>
        public bool DeleteIsEnabled
        {
            get { return _deleteIsEnabled; }
            set
            {
                btnDelete.Enabled = value;
                _deleteIsEnabled = value;
            }
        }

        #endregion

        #endregion

        #region Action

        /// <summary>
        /// 执行上传方法
        /// </summary>
        public Func<string, object, object> ExecuteUpload;

        /// <summary>
        /// 执行导出方法
        /// </summary>
        public Func<string, bool> ExecuteExport;

        /// <summary>
        /// 执行删除方法
        /// </summary>
        public Func<string, object, bool> ExecuteDelete;

        #endregion

        #region 系统事件

        /// <summary>
        /// 用户控件加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SkyCarPictureExpand_Load(object sender, EventArgs e)
        {
            //控制控件状态
            SetControlsStatus();
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnUpload_Click(object sender, EventArgs e)
        {
            if (ExecuteUpload != null)
            {
                //PictureKey为图片唯一标识，返回值为Image
                object uploadResult = ExecuteUpload(PictureKey, PropertyModel);
                if (uploadResult != null)
                {
                    //上传成功，设置Image
                    this.pictureBox.Image = uploadResult;
                    PictureImage = pictureBox.Image;
                }
            }

            //控制控件状态
            SetControlsStatus();
        }

        /// <summary>
        /// 导出图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnExport_Click(object sender, EventArgs e)
        {
            if (ExecuteExport != null)
            {
                //参数为Image，返回值为bool（true：导出成功；false：导出失败；）
                ExecuteExport(PictureKey);
            }
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnDelete_Click(object sender, EventArgs e)
        {
            if (ExecuteDelete != null)
            {
                //PictureKey为图片唯一标识，返回值为bool（true：删除成功；false：删除失败；）
                bool deleteResult = ExecuteDelete(PictureKey, PropertyModel);
                if (deleteResult == true)
                {
                    //删除成功，清除图片
                    this.pictureBox.Image = null;
                }
            }
            //控制控件状态
            SetControlsStatus();
        }

        /// <summary>
        /// 双击图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void pictureBox_DoubleClick(object sender, EventArgs e)
        {
            //双击图片查看原图

            UltraPictureBox originalPictureBox = new UltraPictureBox
            {
                Image = this.pictureBox.Image,
                Width = this.Width*2 + 10,
                Height = this.Height*2 + 100
            };

            FlowLayoutPanel originalPicturePanel = new FlowLayoutPanel
            {
                Width = originalPictureBox.Width + 10,
                Height = originalPictureBox.Height + 10
            };
            originalPicturePanel.Controls.Add(originalPictureBox);

            Form originalPictureForm = new Form
            {
                Name = "放大图片",
                StartPosition = FormStartPosition.CenterScreen,
                Width = originalPicturePanel.Width + 10,
                Height = originalPicturePanel.Height + 10
            };
            originalPictureForm.Controls.Add(originalPicturePanel);
            originalPictureForm.ShowDialog();
        }

        /// <summary>
        /// 图片鼠标悬停事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_MouseHover(object sender, EventArgs e)
        {
            toolTipPanelPicture.SetToolTip(pictureBox, "双击查看原图");
        }

        /// <summary>
        /// 勾选改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckCheckd_CheckedChanged(object sender, EventArgs e)
        {
            if (ckCheckd.Checked)
            {
                IsChecked = true;
            }
            else
            {
                IsChecked = false;
            }
        }
        #endregion

        #region 公共方法

        #endregion

        #region 私有方法

        /// <summary>
        /// 控制控件状态
        /// </summary>
        private void SetControlsStatus()
        {
            if (pictureBox.Image == null)
            {
                //图片为空的场合，隐藏勾选、导出、删除
                IsCheckedIsVisible = false;
                ExportIsVisible = false;
                DeleteIsVisible = false;
            }
            else
            {
                //图片不为空的场合，显示勾选、导出、删除
                IsCheckedIsVisible = true;
                ExportIsVisible = true;
                DeleteIsVisible = true;
            }
        }
        #endregion

    }
}
