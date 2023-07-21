using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ModuleNameSpace
{
	public class Input_Box
	{
		[DllImport("user32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		private static extern IntPtr MB_GetString(uint strId);

		public static DialogResult Show(string strTitle, string strPrompt, ref string strVal, bool blSecure)
		{
			Form form = new Form();
			form.AutoScaleDimensions = new SizeF(6f, 13f);
			form.AutoScaleMode = AutoScaleMode.Font;
			Label label = new Label();
			TextBox textBox = new TextBox();
			Button button = new Button();
			Button button2 = new Button();
			if (string.IsNullOrEmpty(strPrompt))
			{
				if (blSecure)
				{
					label.Text = "Secure input:   ";
				}
				else
				{
					label.Text = "Input:          ";
				}
			}
			else
			{
				label.Text = strPrompt;
			}
			label.Location = new Point(9, 19);
			label.MaximumSize = new Size(Screen.FromControl(form).Bounds.Width * 5 / 8 - 18, 0);
			label.AutoSize = true;
			form.Controls.Add(label);
			if (blSecure)
			{
				textBox.UseSystemPasswordChar = true;
			}
			textBox.Text = strVal;
			textBox.SetBounds(12, label.Bottom, label.Right - 12, 20);
			string text = Marshal.PtrToStringUni(Input_Box.MB_GetString(0U));
			if (string.IsNullOrEmpty(text))
			{
				button.Text = "OK";
			}
			else
			{
				button.Text = text;
			}
			string text2 = Marshal.PtrToStringUni(Input_Box.MB_GetString(1U));
			if (string.IsNullOrEmpty(text2))
			{
				button2.Text = "Cancel";
			}
			else
			{
				button2.Text = text2;
			}
			button.DialogResult = DialogResult.OK;
			button2.DialogResult = DialogResult.Cancel;
			button.SetBounds(Math.Max(12, label.Right - 158), label.Bottom + 36, 75, 23);
			button2.SetBounds(Math.Max(93, label.Right - 77), label.Bottom + 36, 75, 23);
			if (string.IsNullOrEmpty(strTitle))
			{
				form.Text = AppDomain.CurrentDomain.FriendlyName;
			}
			else
			{
				form.Text = strTitle;
			}
			form.ClientSize = new Size(Math.Max(178, label.Right + 10), label.Bottom + 71);
			form.Controls.AddRange(new Control[]
			{
				textBox,
				button,
				button2
			});
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			try
			{
				form.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
			}
			catch
			{
			}
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = button;
			form.CancelButton = button2;
			DialogResult result = form.ShowDialog();
			strVal = textBox.Text;
			return result;
		}

		public static DialogResult Show(string strTitle, string strPrompt, ref string strVal)
		{
			return Input_Box.Show(strTitle, strPrompt, ref strVal, false);
		}
	}
}
