using System;
using System.Management.Automation.Host;
using System.Windows.Forms;

namespace ModuleNameSpace
{
	internal class MainModuleRawUI : PSHostRawUserInterface
	{
		public override ConsoleColor BackgroundColor
		{
			get
			{
				return this.GUIBackgroundColor;
			}
			set
			{
				this.GUIBackgroundColor = value;
			}
		}
		public override Size BufferSize
		{
			get
			{
				return new Size(120, 50);
			}
			set
			{
			}
		}
		public override Coordinates CursorPosition
		{
			get
			{
				return new Coordinates(0, 0);
			}
			set
			{
			}
		}
		public override int CursorSize
		{
			get
			{
				return 25;
			}
			set
			{
			}
		}
		public override void FlushInputBuffer()
		{
			if (this.Invisible_Form != null)
			{
				this.Invisible_Form.Close();
				this.Invisible_Form = null;
				return;
			}
			this.Invisible_Form = new Form();
			this.Invisible_Form.Opacity = 0.0;
			this.Invisible_Form.ShowInTaskbar = false;
			this.Invisible_Form.Visible = true;
		}
		public override ConsoleColor ForegroundColor
		{
			get
			{
				return this.GUIForegroundColor;
			}
			set
			{
				this.GUIForegroundColor = value;
			}
		}
		public override BufferCell[,] GetBufferContents(Rectangle rectangle)
		{
			BufferCell[,] array = new BufferCell[rectangle.Bottom - rectangle.Top + 1, rectangle.Right - rectangle.Left + 1];
			for (int i = 0; i <= rectangle.Bottom - rectangle.Top; i++)
			{
				for (int j = 0; j <= rectangle.Right - rectangle.Left; j++)
				{
					array[i, j] = new BufferCell(' ', this.GUIForegroundColor, this.GUIBackgroundColor, BufferCellType.Complete);
				}
			}
			return array;
		}
		public override bool KeyAvailable
		{
			get
			{
				return true;
			}
		}
		public override Size MaxPhysicalWindowSize
		{
			get
			{
				return new Size(240, 84);
			}
		}
		public override Size MaxWindowSize
		{
			get
			{
				return new Size(120, 84);
			}
		}
		public override KeyInfo ReadKey(ReadKeyOptions options)
		{
			if ((options & ReadKeyOptions.IncludeKeyDown) != (ReadKeyOptions)0)
			{
				return ReadKey_Box.Show("", "", true);
			}
			return ReadKey_Box.Show("", "", false);
		}
		public override void ScrollBufferContents(Rectangle source, Coordinates destination, Rectangle clip, BufferCell fill)
		{
		}
		public override void SetBufferContents(Rectangle rectangle, BufferCell fill)
		{
		}
		public override void SetBufferContents(Coordinates origin, BufferCell[,] contents)
		{
		}
		public override Coordinates WindowPosition
		{
			get
			{
				return new Coordinates
				{
					X = 0,
					Y = 0
				};
			}
			set
			{
			}
		}
		public override Size WindowSize
		{
			get
			{
				return new Size
				{
					Height = 50,
					Width = 120
				};
			}
			set
			{
			}
		}
		public override string WindowTitle
		{
			get
			{
				return AppDomain.CurrentDomain.FriendlyName;
			}
			set
			{
			}
		}
		private ConsoleColor GUIBackgroundColor = ConsoleColor.White;
		private ConsoleColor GUIForegroundColor;
		private Form Invisible_Form;
	}
}
