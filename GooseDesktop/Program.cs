using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace GooseDesktop
{
	internal static class Program
	{
		public const int GWL_EXSTYLE = -20;

		private const int WS_EX_LAYERED = 524288;

		private const int WS_EX_TRANSPARENT = 32;

		private const int LWA_ALPHA = 2;

		private const int LWA_COLORKEY = 1;

		private static IntPtr OriginalWindowStyle;

		private static IntPtr PassthruWindowStyle;

		private static BufferedPanel canvas;

		public static Color ColorKey;

		public static Form mainForm;

		static Program()
		{
			Program.ColorKey = Color.Coral;
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern short GetAsyncKeyState(Keys vKey);

		public static string GetPathToFileInAssembly(string relativePath)
		{
			return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), relativePath);
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

		private static void HandleApplicationIdle(object sender, EventArgs e)
		{
			while (Program.IsApplicationIdle())
			{
				Program.mainForm.TopMost = true;
				Program.canvas.BringToFront();
				Program.canvas.Invalidate();
				Thread.Sleep(8);
			}
		}

		private static bool IsApplicationIdle()
		{
			Program.NativeMessage nativeMessage;
			return Program.PeekMessage(out nativeMessage, IntPtr.Zero, 0, 0, 0) == 0;
		}

		[STAThread]
		private static void Main()
		{
			Program.mainForm = new Form()
			{
				BackColor = Program.ColorKey,
				FormBorderStyle = FormBorderStyle.None,
				Size = Screen.PrimaryScreen.WorkingArea.Size,
				StartPosition = FormStartPosition.Manual,
				Location = new Point(0, 0),
				TopMost = true,
				AllowTransparency = true
			};
			Program.mainForm.BackColor = Program.ColorKey;
			Program.mainForm.TransparencyKey = Program.ColorKey;
			Program.mainForm.ShowIcon = false;
			Program.mainForm.ShowInTaskbar = false;
			Program.OriginalWindowStyle = (IntPtr)((ulong)Program.GetWindowLong(Program.mainForm.Handle, -20));
			Program.PassthruWindowStyle = (IntPtr)((ulong)(Program.GetWindowLong(Program.mainForm.Handle, -20) | 524288 | 32));
			Program.SetWindowPassthru(true);
			Program.canvas = new BufferedPanel()
			{
				Dock = DockStyle.Fill,
				BackColor = Color.Transparent
			};
			Program.canvas.BringToFront();
			Program.canvas.Paint += new PaintEventHandler(Program.Render);
			Program.mainForm.Controls.Add(Program.canvas);
			MainGame.Init();
			Application.Idle += new EventHandler(Program.HandleApplicationIdle);
			Application.EnableVisualStyles();
			Application.Run(Program.mainForm);
		}

		public static void OpenSubform(Form f)
		{
			Program.mainForm.IsMdiContainer = true;
			f.MdiParent = Program.mainForm;
			f.Show();
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern int PeekMessage(out Program.NativeMessage message, IntPtr window, uint filterMin, uint filterMax, uint remove);

		private static void Render(object sender, PaintEventArgs e)
		{
			MainGame.Update(e.Graphics);
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

		private static void SetWindowPassthru(bool passthrough)
		{
			if (passthrough)
			{
				Program.SetWindowLong(Program.mainForm.Handle, -20, Program.PassthruWindowStyle);
				return;
			}
			Program.SetWindowLong(Program.mainForm.Handle, -20, Program.OriginalWindowStyle);
		}

		public struct NativeMessage
		{
			public IntPtr Handle;

			public uint Message;

			public IntPtr WParameter;

			public IntPtr LParameter;

			public uint Time;

			public Point Location;
		}
	}
}