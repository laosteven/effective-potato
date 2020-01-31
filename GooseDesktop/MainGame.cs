using SamEngine;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GooseDesktop
{
	public static class MainGame
	{
		private static float curQuitAlpha;

		private static Font showCurQuitFont;

		static MainGame()
		{
			MainGame.curQuitAlpha = 0f;
			MainGame.showCurQuitFont = new Font("Arial", 12f, FontStyle.Bold);
		}

		public static void Init()
		{
			string pathToFileInAssembly = Program.GetPathToFileInAssembly("Assets/Images/Memes/");
			try
			{
				Directory.GetFiles(pathToFileInAssembly);
			}
			catch
			{
				MessageBox.Show(string.Concat("Warning: Some assets expected at the path: \n\n'", pathToFileInAssembly, "' \n\ncannot be found. \n\nYour .exe should ideally be next to an Assets folder and config, all bundled together!\n\nPlease make sure you extracted the zip file, with the whole folder together, to a known location like Documents or Desktop- and we didn't end up somewhere random like AppData.\n\nGoose will still work, but he won't be able to use custom memes or any of that fanciness.\nHold ESC for several seconds to quit."));
			}
			GooseConfig.LoadConfig();
			Sound.Init();
			TheGoose.Init();
		}

		public static void Update(Graphics g)
		{
			Time.TickTime();
			if (Program.GetAsyncKeyState(Keys.Escape) == 0)
			{
				MainGame.curQuitAlpha -= 0.0166666675f;
			}
			else
			{
				MainGame.curQuitAlpha += 0.00216666679f;
			}
			MainGame.curQuitAlpha = SamMath.Clamp(MainGame.curQuitAlpha, 0f, 1f);
			if (MainGame.curQuitAlpha > 0.2f)
			{
				float single = (MainGame.curQuitAlpha - 0.2f) / 0.8f;
				int num = (int)SamMath.Lerp(-15f, 10f, Easings.ExponentialEaseOut(single * 2f));
				SizeF sizeF = g.MeasureString("Continue Holding ESC to evict goose", MainGame.showCurQuitFont, 2147483647);
				g.FillRectangle(Brushes.LightBlue, new Rectangle(5, num - 5, (int)sizeF.Width + 10, (int)sizeF.Height + 10));
				g.FillRectangle(Brushes.LightPink, new Rectangle(5, num - 5, (int)SamMath.Lerp(0f, sizeF.Width + 10f, single), (int)sizeF.Height + 10));
				SolidBrush solidBrush = new SolidBrush(Color.FromArgb(255, (int)(256f * MainGame.curQuitAlpha), (int)(256f * MainGame.curQuitAlpha), (int)(256f * MainGame.curQuitAlpha)));
				g.DrawString("Continue holding ESC to evict goose", MainGame.showCurQuitFont, solidBrush, 10f, (float)num);
				solidBrush.Dispose();
			}
			if (MainGame.curQuitAlpha > 0.99f)
			{
				Application.Exit();
			}
			TheGoose.Tick();
			TheGoose.Render(g);
		}
	}
}