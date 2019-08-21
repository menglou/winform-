using System;
using System.Windows.Forms;
using SkyCar.Framework.WindowUI;

namespace SkyCar.Coeus.Ult.Entrance
{
    public partial class FrmProgressBar : BaseForm
    {
        /// <summary>
        /// 长时间业务处理器
        /// </summary>
        public Action LongTimeHandler = null;

        /// <summary>
        /// 关闭处理器
        /// </summary>
        public Action CloseHandler = null;

        /// <summary>
        /// 程序自动关闭
        /// </summary>
        private bool _autoClose = false;

        public FrmProgressBar()
        {
            InitializeComponent();
        }

        // 进度发生变化之后的回调方法
        private void workder_ValueChanged(object sender, ValueEventArgs e)
        {
            MethodInvoker invoker = () =>
            {
                this.ultraProgressBar1.Value = e.Value;
            };

            if (this.ultraProgressBar1.InvokeRequired)
            {
                this.ultraProgressBar1.Invoke(invoker);
            }
            else
            {
                invoker();
            }
        }

        private void FrmProgressBar_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_autoClose)
            {
                DialogResult dr = MessageBox.Show(@"加载尚未完成，确定结束？", @"结束确认", MessageBoxButtons.YesNo);
                if (dr != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private void FrmProgressBar_Load(object sender, EventArgs e)
        {
            // 实例化业务对象
            LongTimeWork workder = new LongTimeWork();
            workder.ValueChanged += new ValueChangedEventHandler(workder_ValueChanged);
            if (LongTimeHandler != null)
            {
                LongTimeHandler.BeginInvoke(new AsyncCallback(this.AsyncCallback), LongTimeHandler);
            }
        }

        // 结束异步操作
        private void AsyncCallback(IAsyncResult ar)
        {
            // 标准的处理步骤
            Action handler = ar.AsyncState as Action;

            if (handler != null)
            {
                handler.EndInvoke(ar);
            }

            _autoClose = true;
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => this.Close()));
            }
            else
            {
                this.Close();
            }

            if (CloseHandler != null)
            {
                CloseHandler();
            }

        }
    }
}
