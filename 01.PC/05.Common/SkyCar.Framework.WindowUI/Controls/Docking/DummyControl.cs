using System;
using System.Windows.Forms;

namespace SkyCar.Framework.WindowUI.Controls.Docking
{
	internal class DummyControl : Control
	{
		public DummyControl()
		{
			SetStyle(ControlStyles.Selectable, false);
		}
	}
}
