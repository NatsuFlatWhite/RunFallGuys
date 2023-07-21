using System;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Threading;

namespace ModuleNameSpace
{
	internal class MainModule : PSHost
	{
		public MainModule(MainAppInterface app, MainModuleUI ui)
		{
			this.parent = app;
			this.ui = ui;
		}
		public override PSObject PrivateData
		{
			get
			{
				if (this.ui == null)
				{
					return null;
				}
				PSObject result;
				if ((result = this._consoleColorProxy) == null)
				{
					result = (this._consoleColorProxy = PSObject.AsPSObject(new MainModule.ConsoleColorProxy(this.ui)));
				}
				return result;
			}
		}
		public override CultureInfo CurrentCulture
		{
			get
			{
				return this.originalCultureInfo;
			}
		}
		public override CultureInfo CurrentUICulture
		{
			get
			{
				return this.originalUICultureInfo;
			}
		}
		public override Guid InstanceId
		{
			get
			{
				return this.myId;
			}
		}
		public override string Name
		{
			get
			{
				return "PSRunspace-Host";
			}
		}
		public override PSHostUserInterface UI
		{
			get
			{
				return this.ui;
			}
		}
		public override Version Version
		{
			get
			{
				return new Version(0, 5, 0, 27);
			}
		}
		public override void EnterNestedPrompt()
		{
		}
		public override void ExitNestedPrompt()
		{
		}
		public override void NotifyBeginApplication()
		{
		}
		public override void NotifyEndApplication()
		{
		}
		public override void SetShouldExit(int exitCode)
		{
			this.parent.ShouldExit = true;
			this.parent.ExitCode = exitCode;
		}
		private MainAppInterface parent;
		private MainModuleUI ui;
		private CultureInfo originalCultureInfo = Thread.CurrentThread.CurrentCulture;
		private CultureInfo originalUICultureInfo = Thread.CurrentThread.CurrentUICulture;
		private Guid myId = Guid.NewGuid();
		private PSObject _consoleColorProxy;
		public class ConsoleColorProxy
		{
			public ConsoleColorProxy(MainModuleUI ui)
			{
				if (ui == null)
				{
					throw new ArgumentNullException("ui");
				}
				this._ui = ui;
			}
			public ConsoleColor ErrorForegroundColor
			{
				get
				{
					return this._ui.ErrorForegroundColor;
				}
				set
				{
					this._ui.ErrorForegroundColor = value;
				}
			}
			public ConsoleColor ErrorBackgroundColor
			{
				get
				{
					return this._ui.ErrorBackgroundColor;
				}
				set
				{
					this._ui.ErrorBackgroundColor = value;
				}
			}
			public ConsoleColor WarningForegroundColor
			{
				get
				{
					return this._ui.WarningForegroundColor;
				}
				set
				{
					this._ui.WarningForegroundColor = value;
				}
			}
			public ConsoleColor WarningBackgroundColor
			{
				get
				{
					return this._ui.WarningBackgroundColor;
				}
				set
				{
					this._ui.WarningBackgroundColor = value;
				}
			}
			public ConsoleColor DebugForegroundColor
			{
				get
				{
					return this._ui.DebugForegroundColor;
				}
				set
				{
					this._ui.DebugForegroundColor = value;
				}
			}
			public ConsoleColor DebugBackgroundColor
			{
				get
				{
					return this._ui.DebugBackgroundColor;
				}
				set
				{
					this._ui.DebugBackgroundColor = value;
				}
			}
			public ConsoleColor VerboseForegroundColor
			{
				get
				{
					return this._ui.VerboseForegroundColor;
				}
				set
				{
					this._ui.VerboseForegroundColor = value;
				}
			}
			public ConsoleColor VerboseBackgroundColor
			{
				get
				{
					return this._ui.VerboseBackgroundColor;
				}
				set
				{
					this._ui.VerboseBackgroundColor = value;
				}
			}
			public ConsoleColor ProgressForegroundColor
			{
				get
				{
					return this._ui.ProgressForegroundColor;
				}
				set
				{
					this._ui.ProgressForegroundColor = value;
				}
			}
			public ConsoleColor ProgressBackgroundColor
			{
				get
				{
					return this._ui.ProgressBackgroundColor;
				}
				set
				{
					this._ui.ProgressBackgroundColor = value;
				}
			}
			private MainModuleUI _ui;
		}
	}
}
