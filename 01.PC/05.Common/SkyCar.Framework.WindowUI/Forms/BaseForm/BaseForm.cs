using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using SkyCar.Framework.WindowUI.Controls;

namespace SkyCar.Framework.WindowUI
{
    /// <summary>
    /// 全新UI设计的窗体基类
    /// </summary>
    /// User:Ryan  CreateTime:2012-8-3 14:21.
    [ToolboxBitmap(typeof(Form))]
    public partial class BaseForm : Controls.Docking.DockContent
    {
        #region private attributes

        /// <summary>
        /// 圆角值
        /// </summary>
        private int _cornerRadius = 1;

        /// <summary>
        /// 标题栏高度
        /// </summary>
        private int _captionHeight = 27;

        /// <summary>
        /// 标题栏字体
        /// </summary>
        private Font _captionFont = SystemFonts.CaptionFont;

        /// <summary>
        /// 标题字体颜色
        /// </summary>
        private Color _captionForeColor = Color.Black;

        /// <summary>
        /// 边框宽度
        /// </summary>
        private int _borderWidth = 2;

        /// <summary>
        /// 是否可以调整大小
        /// </summary>
        private bool _resizeEnable = true;

        /// <summary>
        /// 控制按钮大小（最小化，最大化，关闭）
        /// </summary>
        private Size _controlBoxSize = new Size(32, 18);

        /// <summary>
        /// 边距
        /// </summary>
        private Point _offset = new Point(0, 0);

        /// <summary>
        /// 图标大小
        /// </summary>
        private Size _logoSize = new Size(26, 26);

        /// <summary>
        /// 内部间距
        /// </summary>
        private Padding _padding = new Padding(0, 0, 0, 0);

        /// <summary>
        /// 窗体Logo
        /// </summary>
        private Image _capitionLogo;

        private bool _inPosChanged;

        /// <summary>
        /// 窗体控制按钮绘制对象
        /// </summary>
        internal FormControlBoxRender ControlBoxRender;

        #endregion

        #region Initializes

        public BaseForm()
            : base()
        {
            this.SetStyle(ControlStyles.UseTextForAccessibility |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.DoubleBuffer |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.UserPaint, true);
            this.UpdateStyles();
            base.FormBorderStyle = FormBorderStyle.None;
            base.Padding = this.DefaultPadding;
            this.StartPosition = FormStartPosition.CenterScreen;
            base.Size = new Size(500, 350);
            this.ResetRegion();
            ////任务栏的logo
            base.Icon = Properties.Resources.logo;
            this._capitionLogo = Properties.Resources.naruto;
            this.InitializeControlBoxInfo();
            TXToolStripRenderer render = new TXToolStripRenderer();
            //this.SetToolStripRenderer(render);
            ToolStripManager.Renderer = render;
            this.KeyPreview = true;
            this.ControlBoxRender = new FormControlBoxRender();
        }
        #endregion

        #region Properties

        [Category("TXProperties")]
        [Description("标题栏的Logo")]
        public Image CapitionLogo
        {
            get { return this._capitionLogo; }
            set
            {
                this._capitionLogo = value;
                base.Invalidate(new Rectangle(0, 0, this.Width, this.CaptionHeight));
            }
        }

        [Category("TXProperties")]
        [DefaultValue(typeof(Size), "26,26")]
        [Description("图标大小")]
        public Size LogoSize
        {
            get { return this._logoSize; }
            set
            {
                this._logoSize = value;
                base.Invalidate(this.LogoRect);
            }
        }

        [Category("TXProperties")]
        [DefaultValue(3)]
        [Description("窗体圆角值")]
        public int CornerRadius
        {
            get { return this._cornerRadius; }
            set
            {
                this._cornerRadius = value > 0 ? value : 0;
                base.Invalidate();
            }
        }

        [Category("TXProperties")]
        [DefaultValue(true)]
        [Description("是否允许客户调整窗体大小")]
        public bool ResizeEnable
        {
            get { return this._resizeEnable; }
            set { this._resizeEnable = value; }
        }

        [Category("TXProperties")]
        [DefaultValue(25)]
        [Description("窗口标题高度，为0则为无标题栏窗体")]
        public int CaptionHeight
        {
            get { return this._captionHeight; }
            set
            {
                if (value == 0)
                {
                    this._captionHeight = 0;
                    this.ControlBox = false;
                }
                else
                {
                    this._captionHeight = value > this._borderWidth ? value : this._borderWidth;
                }

                base.Invalidate();
            }
        }

        [Category("TXProperties")]
        [DefaultValue(typeof(Font), "CaptionFont")]
        [Description("窗口标题字体")]
        public Font CaptionFont
        {
            get { return this._captionFont; }
            set
            {
                if (value == null)
                {
                    this._captionFont = new Font("微软雅黑", 18);
                }
                else
                {
                    this._captionFont = value;
                }

                base.Invalidate(new Rectangle(0, 0, this.Width, this.CaptionHeight));
            }
        }

        [Category("TXProperties")]
        [DefaultValue(typeof(Color), "White")]
        [Description("窗口标题字体颜色")]
        public Color CaptionForeColor
        {
            get { return this._captionForeColor; }
            set
            {
                this._captionForeColor = value;
                base.Invalidate(new Rectangle(0, 0, this.Width, this.CaptionHeight));
            }
        }

        [Category("TXProperties")]
        [DefaultValue(typeof(Size), "32, 18")]
        [Description("窗体控制按钮大小")]
        public Size ControlBoxSize
        {
            get { return this._controlBoxSize; }
            set { this._controlBoxSize = value; base.Invalidate(new Rectangle(0, 0, this.Width, this.CaptionHeight)); }
        }

        [Category("TXProperties")]
        [DefaultValue(typeof(Point), "8,0")]
        [Description("窗体标题栏内容与边框的偏移量")]
        public Point Offset
        {
            get { return this._offset; }
            set { this._offset = value; base.Invalidate(new Rectangle(0, 0, this.Width, this.CaptionHeight)); }
        }

        [Category("TXProperties")]
        [DefaultValue(3)]
        [Description("边框宽度")]
        public int BorderWidth
        {
            get { return this._borderWidth; }
            set { this._borderWidth = value > 1 ? value : 1; }
        }

        [Category("TXProperties")]
        [DefaultValue(typeof(Padding), "0")]
        public new Padding Padding
        {
            get { return this._padding; }
            set
            {
                this._padding = value;
                base.Padding = new Padding(this._borderWidth + this._padding.Left,
                    this._captionHeight + this._padding.Top,
                    this._borderWidth + this._padding.Right,
                    this._borderWidth + this._padding.Bottom);
                base.Invalidate();
            }
        }

        [Category("TXProperties")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                base.Invalidate(new Rectangle(0, 0, this.Width, this._captionHeight + 1));
            }
        }

        protected Rectangle CaptionRect
        {
            get
            {
                return new Rectangle(0, 0, this.Width - this.Offset.X, this.CaptionHeight);
            }
        }

        protected Rectangle WorkRectangle
        {
            get
            {
                return new Rectangle(this.Padding.Left,
                    this.CaptionHeight + this.Padding.Top,
                    this.Width - this.Padding.Left - this.Padding.Right,
                    this.Height - this.CaptionHeight - this.Padding.Top - this.Padding.Bottom);
            }
        }

        protected override Padding DefaultPadding
        {
            get
            {
                return new Padding(this._borderWidth, this._captionHeight, this._borderWidth, this._borderWidth);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!DesignMode)
                {
                    cp.Style |= (int)WindowStyle.WS_THICKFRAME;
                    if (ControlBox)
                    {
                        cp.Style |= (int)WindowStyle.WS_SYSMENU;
                    }

                    if (MinimizeBox)
                    {
                        cp.Style |= (int)WindowStyle.WS_MINIMIZEBOX;
                    }

                    if (!MaximizeBox)
                    {
                        cp.Style &= ~(int)WindowStyle.WS_MAXIMIZEBOX;
                    }

                    if (this._inPosChanged)
                    {
                        cp.Style &= ~((int)WindowStyle.WS_THICKFRAME |
                            (int)WindowStyle.WS_SYSMENU);
                        cp.ExStyle &= ~((int)WindowStyleEx.WS_EX_DLGMODALFRAME |
                            (int)WindowStyleEx.WS_EX_WINDOWEDGE);
                    }
                }

                return cp;
            }
        }

        #endregion

        #region Override methods

        protected override void OnCreateControl()
        {
            this.ResetRegion();
            base.OnCreateControl();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (this.Created)
            {
                this.ResetRegion();
                this.Invalidate();
                this.Refresh();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            this.ProcessMouseMove(e.Location);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.ProcessMouseDown(e.Location);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.ProcessMouseUp(e.Location);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.ProcessMouseLeave(PointToClient(MousePosition));
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (e.KeyChar == (char)27)
            {
                this.Close();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            GDIHelper.InitializeGraphics(g);
            this.DrawFormBackGround(g);
            this.DrawCaption(g);
            this.DrawFormBorder(g);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)WindowMessages.WM_NCHITTEST:
                    WmNcHitTest(ref m);
                    break;
                case (int)WindowMessages.WM_NCPAINT:
                case (int)WindowMessages.WM_NCCALCSIZE:
                    break;
                case (int)WindowMessages.WM_WINDOWPOSCHANGED:
                    _inPosChanged = true;
                    base.WndProc(ref m);
                    _inPosChanged = false;
                    break;
                case (int)WindowMessages.WM_GETMINMAXINFO:
                    WmMinMaxInfo(ref m);
                    break;
                case (int)WindowMessages.WM_LBUTTONDBLCLK:
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// 设置窗口区域
        /// </summary>
        /// User:Ryan  CreateTime:2011-07-26 14:06.
        private void ResetRegion()
        {
            if (base.Region != null)
            {
                base.Region.Dispose();
            }

            Rectangle rect = new Rectangle(0, 0, this.Size.Width, this.Size.Height);
            RoundRectangle roundRect = new RoundRectangle(rect, new CornerRadius(this._cornerRadius));
            using (GraphicsPath path = roundRect.ToGraphicsBezierPath())
            {
                path.CloseFigure();
                base.Region = new Region(path);
            }

            //这种方式设置窗口圆角，边框不好控制
            //int rgn = Win32.CreateRoundRectRgn(0, 0,
            //    this.Size.Width, this.Size.Height,
            //    this._cornerRadius + 1, this._cornerRadius);
            //Win32.SetWindowRgn(this.Handle, rgn, true);
        }

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BaseForm
            // 
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "BaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }
    }
}
