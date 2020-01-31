using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GooseDesktop
{
	public class Form1 : Form
	{
		private IContainer components;

		public Form1()
		{
			this.InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
		}

		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(8f, 16f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(282, 253);
			base.Name = "Form1";
			this.Text = "Form1";
			base.Load += new EventHandler(this.Form1_Load);
			base.ResumeLayout(false);
		}
	}
}