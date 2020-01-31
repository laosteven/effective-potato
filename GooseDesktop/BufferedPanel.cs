using System;
using System.Windows.Forms;

namespace GooseDesktop
{
	public class BufferedPanel : Panel
	{
		public BufferedPanel()
		{
			this.DoubleBuffered = true;
		}
	}
}