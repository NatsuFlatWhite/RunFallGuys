using System;
using System.Collections.Generic;
using System.Drawing;
using System.Management.Automation;
using System.Timers;
using System.Windows.Forms;

namespace ModuleNameSpace
{
		public partial class Progress_Form : Form
	{
				private Color DrawingColor(ConsoleColor color)
		{
			switch (color)
			{
			case ConsoleColor.Black:
				return Color.Black;
			case ConsoleColor.DarkBlue:
				return ColorTranslator.FromHtml("#000080");
			case ConsoleColor.DarkGreen:
				return ColorTranslator.FromHtml("#008000");
			case ConsoleColor.DarkCyan:
				return ColorTranslator.FromHtml("#008080");
			case ConsoleColor.DarkRed:
				return ColorTranslator.FromHtml("#800000");
			case ConsoleColor.DarkMagenta:
				return ColorTranslator.FromHtml("#800080");
			case ConsoleColor.DarkYellow:
				return ColorTranslator.FromHtml("#808000");
			case ConsoleColor.Gray:
				return ColorTranslator.FromHtml("#C0C0C0");
			case ConsoleColor.DarkGray:
				return ColorTranslator.FromHtml("#808080");
			case ConsoleColor.Blue:
				return Color.Blue;
			case ConsoleColor.Green:
				return ColorTranslator.FromHtml("#00FF00");
			case ConsoleColor.Cyan:
				return Color.Cyan;
			case ConsoleColor.Red:
				return Color.Red;
			case ConsoleColor.Magenta:
				return Color.Magenta;
			case ConsoleColor.White:
				return Color.White;
			}
			return Color.Yellow;
		}

				private void TimeTick(object source, ElapsedEventArgs e)
		{
			if (this.inTick)
			{
				return;
			}
			this.inTick = true;
			if (this.barNumber >= 0)
			{
				if (this.barValue >= 0)
				{
					this.progressDataList[this.barNumber].objProgressBar.Value = this.barValue;
					this.barValue = -1;
				}
				this.progressDataList[this.barNumber].objProgressBar.Refresh();
			}
			this.inTick = false;
		}

				private void AddBar(ref Progress_Form.Progress_Data pd, int position)
		{
			pd.lbActivity = new Label();
			pd.lbActivity.Left = 5;
			pd.lbActivity.Top = 104 * position + 10;
			pd.lbActivity.Width = 780;
			pd.lbActivity.Height = 16;
			pd.lbActivity.Font = new Font(pd.lbActivity.Font, FontStyle.Bold);
			pd.lbActivity.Text = "";
			base.Controls.Add(pd.lbActivity);
			pd.lbStatus = new Label();
			pd.lbStatus.Left = 25;
			pd.lbStatus.Top = 104 * position + 26;
			pd.lbStatus.Width = 760;
			pd.lbStatus.Height = 16;
			pd.lbStatus.Text = "";
			base.Controls.Add(pd.lbStatus);
			pd.objProgressBar = new ProgressBar();
			pd.objProgressBar.Value = 0;
			pd.objProgressBar.Style = ProgressBarStyle.Blocks;
			pd.objProgressBar.ForeColor = this.DrawingColor(this.ProgressBarColor);
			if (pd.Depth < 15)
			{
				pd.objProgressBar.Size = new Size(740 - 30 * pd.Depth, 20);
				pd.objProgressBar.Left = 25 + 30 * pd.Depth;
			}
			else
			{
				pd.objProgressBar.Size = new Size(290, 20);
				pd.objProgressBar.Left = 475;
			}
			pd.objProgressBar.Top = 104 * position + 47;
			base.Controls.Add(pd.objProgressBar);
			pd.lbRemainingTime = new Label();
			pd.lbRemainingTime.Left = 5;
			pd.lbRemainingTime.Top = 104 * position + 72;
			pd.lbRemainingTime.Width = 780;
			pd.lbRemainingTime.Height = 16;
			pd.lbRemainingTime.Text = "";
			base.Controls.Add(pd.lbRemainingTime);
			pd.lbOperation = new Label();
			pd.lbOperation.Left = 25;
			pd.lbOperation.Top = 104 * position + 88;
			pd.lbOperation.Width = 760;
			pd.lbOperation.Height = 16;
			pd.lbOperation.Text = "";
			base.Controls.Add(pd.lbOperation);
		}

				public int GetCount()
		{
			return this.progressDataList.Count;
		}

				public Progress_Form()
		{
			this.InitializeComponent();
		}

				public Progress_Form(ConsoleColor BarColor)
		{
			this.ProgressBarColor = BarColor;
			this.InitializeComponent();
		}

				public void Update(ProgressRecord objRecord)
		{
			if (objRecord == null)
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < this.progressDataList.Count; i++)
			{
				if (this.progressDataList[i].ActivityId == objRecord.ActivityId)
				{
					num = i;
					break;
				}
			}
			if (objRecord.RecordType != ProgressRecordType.Completed)
			{
				if (num < 0)
				{
					Progress_Form.Progress_Data item = default(Progress_Form.Progress_Data);
					item.ActivityId = objRecord.ActivityId;
					item.ParentActivityId = objRecord.ParentActivityId;
					item.Depth = 0;
					int num2 = -1;
					int num3 = -1;
					if (item.ParentActivityId >= 0)
					{
						for (int j = 0; j < this.progressDataList.Count; j++)
						{
							if (this.progressDataList[j].ActivityId == item.ParentActivityId)
							{
								num3 = j;
								break;
							}
						}
					}
					if (num3 >= 0)
					{
						item.Depth = this.progressDataList[num3].Depth + 1;
						for (int k = num3 + 1; k < this.progressDataList.Count; k++)
						{
							if (this.progressDataList[k].Depth < item.Depth || (this.progressDataList[k].Depth == item.Depth && this.progressDataList[k].ParentActivityId != item.ParentActivityId))
							{
								num2 = k;
								break;
							}
						}
					}
					if (num2 == -1)
					{
						this.AddBar(ref item, this.progressDataList.Count);
						num = this.progressDataList.Count;
						this.progressDataList.Add(item);
					}
					else
					{
						this.AddBar(ref item, num2);
						num = num2;
						this.progressDataList.Insert(num2, item);
						for (int l = num + 1; l < this.progressDataList.Count; l++)
						{
							this.progressDataList[l].lbActivity.Top = 104 * l + 10;
							this.progressDataList[l].lbStatus.Top = 104 * l + 26;
							this.progressDataList[l].objProgressBar.Top = 104 * l + 47;
							this.progressDataList[l].lbRemainingTime.Top = 104 * l + 72;
							this.progressDataList[l].lbOperation.Top = 104 * l + 88;
						}
					}
					if (104 * this.progressDataList.Count + 43 <= Screen.FromControl(this).Bounds.Height)
					{
						base.Height = 104 * this.progressDataList.Count + 43;
						base.Location = new Point((Screen.FromControl(this).Bounds.Width - base.Width) / 2, (Screen.FromControl(this).Bounds.Height - base.Height) / 2);
					}
					else
					{
						base.Height = Screen.FromControl(this).Bounds.Height;
						base.Location = new Point((Screen.FromControl(this).Bounds.Width - base.Width) / 2, 0);
					}
				}
				if (!string.IsNullOrEmpty(objRecord.Activity))
				{
					this.progressDataList[num].lbActivity.Text = objRecord.Activity;
				}
				else
				{
					this.progressDataList[num].lbActivity.Text = "";
				}
				if (!string.IsNullOrEmpty(objRecord.StatusDescription))
				{
					this.progressDataList[num].lbStatus.Text = objRecord.StatusDescription;
				}
				else
				{
					this.progressDataList[num].lbStatus.Text = "";
				}
				if (objRecord.PercentComplete >= 0 && objRecord.PercentComplete <= 100)
				{
					if (objRecord.PercentComplete < 100)
					{
						this.progressDataList[num].objProgressBar.Value = objRecord.PercentComplete + 1;
					}
					else
					{
						this.progressDataList[num].objProgressBar.Value = 99;
					}
					this.progressDataList[num].objProgressBar.Visible = true;
					this.barNumber = num;
					this.barValue = objRecord.PercentComplete;
				}
				else if (objRecord.PercentComplete > 100)
				{
					this.progressDataList[num].objProgressBar.Value = 0;
					this.progressDataList[num].objProgressBar.Visible = true;
					this.barNumber = num;
					this.barValue = 0;
				}
				else
				{
					this.progressDataList[num].objProgressBar.Visible = false;
					if (this.barNumber == num)
					{
						this.barNumber = -1;
					}
				}
				if (objRecord.SecondsRemaining >= 0)
				{
					TimeSpan timeSpan = new TimeSpan(0, 0, objRecord.SecondsRemaining);
					this.progressDataList[num].lbRemainingTime.Text = "Remaining time: " + string.Format("{0:00}:{1:00}:{2:00}", (int)timeSpan.TotalHours, timeSpan.Minutes, timeSpan.Seconds);
				}
				else
				{
					this.progressDataList[num].lbRemainingTime.Text = "";
				}
				if (!string.IsNullOrEmpty(objRecord.CurrentOperation))
				{
					this.progressDataList[num].lbOperation.Text = objRecord.CurrentOperation;
				}
				else
				{
					this.progressDataList[num].lbOperation.Text = "";
				}
				Application.DoEvents();
				return;
			}
			if (num >= 0)
			{
				if (this.barNumber == num)
				{
					this.barNumber = -1;
				}
				base.Controls.Remove(this.progressDataList[num].lbActivity);
				base.Controls.Remove(this.progressDataList[num].lbStatus);
				base.Controls.Remove(this.progressDataList[num].objProgressBar);
				base.Controls.Remove(this.progressDataList[num].lbRemainingTime);
				base.Controls.Remove(this.progressDataList[num].lbOperation);
				this.progressDataList[num].lbActivity.Dispose();
				this.progressDataList[num].lbStatus.Dispose();
				this.progressDataList[num].objProgressBar.Dispose();
				this.progressDataList[num].lbRemainingTime.Dispose();
				this.progressDataList[num].lbOperation.Dispose();
				this.progressDataList.RemoveAt(num);
			}
			if (this.progressDataList.Count == 0)
			{
				this.timer.Stop();
				this.timer.Dispose();
				base.Close();
				return;
			}
			if (num < 0)
			{
				return;
			}
			for (int m = num; m < this.progressDataList.Count; m++)
			{
				this.progressDataList[m].lbActivity.Top = 104 * m + 10;
				this.progressDataList[m].lbStatus.Top = 104 * m + 26;
				this.progressDataList[m].objProgressBar.Top = 104 * m + 47;
				this.progressDataList[m].lbRemainingTime.Top = 104 * m + 72;
				this.progressDataList[m].lbOperation.Top = 104 * m + 88;
			}
			if (104 * this.progressDataList.Count + 43 <= Screen.FromControl(this).Bounds.Height)
			{
				base.Height = 104 * this.progressDataList.Count + 43;
				base.Location = new Point((Screen.FromControl(this).Bounds.Width - base.Width) / 2, (Screen.FromControl(this).Bounds.Height - base.Height) / 2);
				return;
			}
			base.Height = Screen.FromControl(this).Bounds.Height;
			base.Location = new Point((Screen.FromControl(this).Bounds.Width - base.Width) / 2, 0);
		}

				private ConsoleColor ProgressBarColor = ConsoleColor.DarkCyan;

				private int barNumber = -1;

				private int barValue = -1;

				private bool inTick;

				private List<Progress_Form.Progress_Data> progressDataList = new List<Progress_Form.Progress_Data>();

				private struct Progress_Data
		{
						internal Label lbActivity;

						internal Label lbStatus;

						internal ProgressBar objProgressBar;

						internal Label lbRemainingTime;

						internal Label lbOperation;

						internal int ActivityId;

						internal int ParentActivityId;

						internal int Depth;
		}
	}
}
