using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Management.Automation.Host;
using System.Reflection;
using System.Windows.Forms;

namespace ModuleNameSpace
{
	public class Choice_Box
	{
		public static int Show(Collection<ChoiceDescription> arrChoice, int intDefault, string strTitle, string strPrompt)
		{
			if (arrChoice == null)
			{
				return -1;
			}
			if (arrChoice.Count < 1)
			{
				return -1;
			}
			Form form = new Form();
			form.AutoScaleDimensions = new SizeF(6f, 13f);
			form.AutoScaleMode = AutoScaleMode.Font;
			RadioButton[] array = new RadioButton[arrChoice.Count];
			ToolTip toolTip = new ToolTip();
			Button button = new Button();
			int num = 19;
			int num2 = 0;
			if (!string.IsNullOrEmpty(strPrompt))
			{
				Label label = new Label();
				label.Text = strPrompt;
				label.Location = new Point(9, 19);
				label.MaximumSize = new System.Drawing.Size(Screen.FromControl(form).Bounds.Width * 5 / 8 - 18, 0);
				label.AutoSize = true;
				form.Controls.Add(label);
				num = label.Bottom;
				num2 = label.Right;
			}
			int i = 0;
			int num3 = Screen.FromControl(form).Bounds.Width * 5 / 8 - 18;
			foreach (ChoiceDescription choiceDescription in arrChoice)
			{
				array[i] = new RadioButton();
				array[i].Text = choiceDescription.Label;
				if (i == intDefault)
				{
					array[i].Checked = true;
				}
				array[i].Location = new Point(9, num);
				array[i].AutoSize = true;
				form.Controls.Add(array[i]);
				if (array[i].Width > num3)
				{
					int height = array[i].Height;
					array[i].Height = height * (1 + (array[i].Width - 1) / num3);
					array[i].Width = num3;
					array[i].AutoSize = false;
				}
				num = array[i].Bottom;
				if (array[i].Right > num2)
				{
					num2 = array[i].Right;
				}
				if (!string.IsNullOrEmpty(choiceDescription.HelpMessage))
				{
					toolTip.SetToolTip(array[i], choiceDescription.HelpMessage);
				}
				i++;
			}
			toolTip.ShowAlways = true;
			button.Text = "OK";
			button.DialogResult = DialogResult.OK;
			button.SetBounds(Math.Max(12, num2 - 77), num + 36, 75, 23);
			if (string.IsNullOrEmpty(strTitle))
			{
				form.Text = AppDomain.CurrentDomain.FriendlyName;
			}
			else
			{
				form.Text = strTitle;
			}
			form.ClientSize = new System.Drawing.Size(Math.Max(178, num2 + 10), num + 71);
			form.Controls.Add(button);
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
			if (form.ShowDialog() == DialogResult.OK)
			{
				int result = -1;
				for (i = 0; i < arrChoice.Count; i++)
				{
					if (array[i].Checked)
					{
						result = i;
					}
				}
				return result;
			}
			return -1;
		}
	}
}
