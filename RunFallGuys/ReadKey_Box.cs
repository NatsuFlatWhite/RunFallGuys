using System;
using System.Drawing;
using System.Management.Automation.Host;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ModuleNameSpace
{
		public class ReadKey_Box
	{
				[DllImport("user32.dll")]
		public static extern int ToUnicode(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pwszBuff, int cchBuff, uint wFlags);

				private static string GetCharFromKeys(Keys keys, bool blShift, bool blAltGr)
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			byte[] array = new byte[256];
			if (blShift)
			{
				array[16] = byte.MaxValue;
			}
			if (blAltGr)
			{
				array[17] = byte.MaxValue;
				array[18] = byte.MaxValue;
			}
			if (ReadKey_Box.ToUnicode((uint)keys, 0U, array, stringBuilder, 64, 0U) >= 1)
			{
				return stringBuilder.ToString();
			}
			return "\0";
		}

				public static KeyInfo Show(string strTitle, string strPrompt, bool blIncludeKeyDown)
		{
			ReadKey_Box.Keyboard_Form keyboard_Form = new ReadKey_Box.Keyboard_Form();
			Label label = new Label();
			if (string.IsNullOrEmpty(strPrompt))
			{
				label.Text = "Press a key";
			}
			else
			{
				label.Text = strPrompt;
			}
			label.Location = new Point(9, 19);
			label.MaximumSize = new System.Drawing.Size(Screen.FromControl(keyboard_Form).Bounds.Width * 5 / 8 - 18, 0);
			label.AutoSize = true;
			keyboard_Form.Controls.Add(label);
			if (string.IsNullOrEmpty(strTitle))
			{
				keyboard_Form.Text = AppDomain.CurrentDomain.FriendlyName;
			}
			else
			{
				keyboard_Form.Text = strTitle;
			}
			keyboard_Form.ClientSize = new System.Drawing.Size(Math.Max(178, label.Right + 10), label.Bottom + 55);
			keyboard_Form.FormBorderStyle = FormBorderStyle.FixedDialog;
			keyboard_Form.StartPosition = FormStartPosition.CenterScreen;
			try
			{
				keyboard_Form.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
			}
			catch
			{
			}
			keyboard_Form.MinimizeBox = false;
			keyboard_Form.MaximizeBox = false;
			keyboard_Form.checkKeyDown = blIncludeKeyDown;
			keyboard_Form.ShowDialog();
			return keyboard_Form.keyinfo;
		}

				private class Keyboard_Form : Form
		{
						public Keyboard_Form()
			{
				base.AutoScaleDimensions = new SizeF(6f, 13f);
				base.AutoScaleMode = AutoScaleMode.Font;
				base.KeyDown += this.Keyboard_Form_KeyDown;
				base.KeyUp += this.Keyboard_Form_KeyUp;
			}

						private void Keyboard_Form_KeyDown(object sender, KeyEventArgs e)
			{
				if (this.checkKeyDown)
				{
					this.keyinfo.VirtualKeyCode = e.KeyValue;
					this.keyinfo.Character = ReadKey_Box.GetCharFromKeys(e.KeyCode, e.Shift, e.Alt & e.Control)[0];
					this.keyinfo.KeyDown = false;
					this.keyinfo.ControlKeyState = (ControlKeyStates)0;
					if (e.Alt)
					{
						this.keyinfo.ControlKeyState = (ControlKeyStates.RightAltPressed | ControlKeyStates.LeftAltPressed);
					}
					if (e.Control)
					{
						this.keyinfo.ControlKeyState = (this.keyinfo.ControlKeyState | (ControlKeyStates.RightCtrlPressed | ControlKeyStates.LeftCtrlPressed));
						if (!e.Alt && e.KeyValue > 64 && e.KeyValue < 96)
						{
							this.keyinfo.Character = (char)(e.KeyValue - 64);
						}
					}
					if (e.Shift)
					{
						this.keyinfo.ControlKeyState = (this.keyinfo.ControlKeyState | ControlKeyStates.ShiftPressed);
					}
					if ((e.Modifiers & Keys.Capital) > Keys.None)
					{
						this.keyinfo.ControlKeyState = (this.keyinfo.ControlKeyState | ControlKeyStates.CapsLockOn);
					}
					if ((e.Modifiers & Keys.NumLock) > Keys.None)
					{
						this.keyinfo.ControlKeyState = (this.keyinfo.ControlKeyState | ControlKeyStates.NumLockOn);
					}
					base.Close();
				}
			}

						private void Keyboard_Form_KeyUp(object sender, KeyEventArgs e)
			{
				if (!this.checkKeyDown)
				{
					this.keyinfo.VirtualKeyCode = e.KeyValue;
					this.keyinfo.Character = ReadKey_Box.GetCharFromKeys(e.KeyCode, e.Shift, e.Alt & e.Control)[0];
					this.keyinfo.KeyDown = true;
					this.keyinfo.ControlKeyState = (ControlKeyStates)0;
					if (e.Alt)
					{
						this.keyinfo.ControlKeyState = (ControlKeyStates.RightAltPressed | ControlKeyStates.LeftAltPressed);
					}
					if (e.Control)
					{
						this.keyinfo.ControlKeyState = (this.keyinfo.ControlKeyState | (ControlKeyStates.RightCtrlPressed | ControlKeyStates.LeftCtrlPressed));
						if (!e.Alt && e.KeyValue > 64 && e.KeyValue < 96)
						{
							this.keyinfo.Character = (char)(e.KeyValue - 64);
						}
					}
					if (e.Shift)
					{
						this.keyinfo.ControlKeyState = (this.keyinfo.ControlKeyState | ControlKeyStates.ShiftPressed);
					}
					if ((e.Modifiers & Keys.Capital) > Keys.None)
					{
						this.keyinfo.ControlKeyState = (this.keyinfo.ControlKeyState | ControlKeyStates.CapsLockOn);
					}
					if ((e.Modifiers & Keys.NumLock) > Keys.None)
					{
						this.keyinfo.ControlKeyState = (this.keyinfo.ControlKeyState | ControlKeyStates.NumLockOn);
					}
					base.Close();
				}
			}

						public bool checkKeyDown = true;

						public KeyInfo keyinfo;
		}
	}
}
