using System;
using System.Windows.Forms;

namespace GooseDesktop
{
	// Token: 0x02000010 RID: 16
	public class BufferedPanel : Panel
	{
		// Token: 0x0600006E RID: 110 RVA: 0x00004DAE File Offset: 0x00002FAE
		public BufferedPanel()
		{
			this.DoubleBuffered = true;
		}
	}
}
