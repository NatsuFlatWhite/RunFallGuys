namespace ModuleNameSpace
{
	public partial class Progress_Form : global::System.Windows.Forms.Form
	{
		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Text = global::System.AppDomain.CurrentDomain.FriendlyName;
			base.Height = 147;
			base.Width = 800;
			this.BackColor = global::System.Drawing.Color.White;
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MinimizeBox = false;
			base.MaximizeBox = false;
			base.ControlBox = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			base.ResumeLayout();
			this.timer.Elapsed += new global::System.Timers.ElapsedEventHandler(this.TimeTick);
			this.timer.Interval = 50.0;
			this.timer.AutoReset = true;
			this.timer.Start();
		}
		private global::System.Timers.Timer timer = new global::System.Timers.Timer();
	}
}
