using System;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ModuleNameSpace
{
		internal class MainApp : MainAppInterface
	{
								public bool ShouldExit
		{
			get
			{
				return this.shouldExit;
			}
			set
			{
				this.shouldExit = value;
			}
		}

								public int ExitCode
		{
			get
			{
				return this.exitCode;
			}
			set
			{
				this.exitCode = value;
			}
		}

				[STAThread]
		private static int Main(string[] args)
		{
			Application.EnableVisualStyles();
			MainApp mainApp = new MainApp();
			bool flag = false;
			string text = string.Empty;
			MainModuleUI ui = new MainModuleUI();
			MainModule host = new MainModule(mainApp, ui);
			ManualResetEvent mre = new ManualResetEvent(false);
			AppDomain.CurrentDomain.UnhandledException += MainApp.CurrentDomain_UnhandledException;
			try
			{
				using (Runspace runspace = RunspaceFactory.CreateRunspace(host))
				{
					runspace.ApartmentState = ApartmentState.STA;
					runspace.Open();
					using (PowerShell powerShell = PowerShell.Create())
					{
						powerShell.Runspace = runspace;
						powerShell.Streams.Error.DataAdded += delegate(object sender, DataAddedEventArgs e)
						{
							ui.WriteErrorLine(((PSDataCollection<ErrorRecord>)sender)[e.Index].ToString());
						};
						PSDataCollection<string> psdataCollection = new PSDataCollection<string>();
						if (Console_Info.IsInputRedirected())
						{
							string item;
							while ((item = Console.ReadLine()) != null)
							{
								psdataCollection.Add(item);
							}
						}
						psdataCollection.Complete();
						PSDataCollection<PSObject> colOutput = new PSDataCollection<PSObject>();
						colOutput.DataAdded += delegate(object sender, DataAddedEventArgs e)
						{
							ui.WriteLine(colOutput[e.Index].ToString());
						};
						int num = 0;
						int num2 = 0;
						foreach (string text2 in args)
						{
							if (string.Compare(text2, "-whatt".Replace("hat", "ai"), true) == 0)
							{
								flag = true;
							}
							else if (text2.StartsWith("-extdummt".Replace("dumm", "rac"), StringComparison.InvariantCultureIgnoreCase))
							{
								string[] array = text2.Split(new string[]
								{
									":"
								}, 2, StringSplitOptions.RemoveEmptyEntries);
								if (array.Length != 2)
								{
									MessageBox.Show("If you spzzcify thzz -zzxtract option you nzzed to add a filzz for zzxtraction in this way\r\n   -zzxtract:\"<filzznamzz>\"".Replace("zz", "e"), AppDomain.CurrentDomain.FriendlyName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
									return 1;
								}
								text = array[1].Trim(new char[]
								{
									'"'
								});
							}
							else
							{
								if (string.Compare(text2, "-end", true) == 0)
								{
									num = num2 + 1;
									break;
								}
								if (string.Compare(text2, "-debug", true) == 0)
								{
									System.Diagnostics.Debugger.Launch();
									break;
								}
							}
							num2++;
						}
						string @string = Encoding.UTF8.GetString(Convert.FromBase64String("JEZhbGxHdXlzUGF0aCA9ICdGYWxsR3V5c19jbGllbnQuZXhlJw0KDQokUmVkaXN0UGF0aFg4NiA9ICdSZWRpc3RyaWJ1dGFibGVzXFZDX3JlZGlzdC54ODYuZXhlJw0KJFJlZGlzdFBhdGhYNjQgPSAnUmVkaXN0cmlidXRhYmxlc1xWQ19yZWRpc3QueDY0LmV4ZScNCg0KJFJlZ2lzdHJ5S2V5WDg2ID0gJ0hLTE06XFNPRlRXQVJFXFdPVzY0MzJOb2RlXE1pY3Jvc29mdFxWaXN1YWxTdHVkaW9cMTQuMFxWQ1xSdW50aW1lc1xYODYnDQokUmVnaXN0cnlLZXlYNjQgPSAnSEtMTTpcU09GVFdBUkVcTWljcm9zb2Z0XFZpc3VhbFN0dWRpb1wxNC4wXFZDXFJ1bnRpbWVzXFg2NCcNCg0KQWRkLVR5cGUgLUFzc2VtYmx5TmFtZSBQcmVzZW50YXRpb25GcmFtZXdvcmsNCg0KaWYgKCEoVGVzdC1QYXRoIC1QYXRoICRSZWdpc3RyeUtleVg4NikpDQp7DQogICAgU3RhcnQtUHJvY2VzcyAkUmVkaXN0UGF0aFg4NiAtV2FpdA0KDQogICAgaWYgKCEoVGVzdC1QYXRoIC1QYXRoICRSZWdpc3RyeUtleVg4NikpDQogICAgew0KICAgICAgICBbU3lzdGVtLldpbmRvd3MuTWVzc2FnZUJveF06OlNob3coJ1lvdSBuZWVkIHRvIGluc3RhbGwgVmlzdWFsIEMrKyAyMDE1LTIwMjIgUmVkaXN0cmlidXRhYmxlICh4ODYpIHRvIHBsYXkgRmFsbCBHdXlzLiBJZiB5b3UgZmluaXNoZWQgdGhlIGluc3RhbGxhdGlvbiBwcm9wZXJseSBhbmQgYXJlIHN0aWxsIGdldHRpbmcgdGhpcyBlcnJvciwgcGxlYXNlIGNvbnRhY3QgY3VzdG9tZXIgc3VwcG9ydC4nLCAkRmFsbEd1eXNQYXRoLCAnT2snLCAnRXJyb3InKSB8IE91dC1OdWxsDQogICAgICAgIGV4aXQNCiAgICB9DQp9DQoNCmlmICghKFRlc3QtUGF0aCAtUGF0aCAkUmVnaXN0cnlLZXlYNjQpKQ0Kew0KICAgIFN0YXJ0LVByb2Nlc3MgJFJlZGlzdFBhdGhYNjQgLVdhaXQNCg0KICAgIGlmICghKFRlc3QtUGF0aCAtUGF0aCAkUmVnaXN0cnlLZXlYNjQpKQ0KICAgIHsNCiAgICAgICAgW1N5c3RlbS5XaW5kb3dzLk1lc3NhZ2VCb3hdOjpTaG93KCdZb3UgbmVlZCB0byBpbnN0YWxsIFZpc3VhbCBDKysgMjAxNS0yMDIyIFJlZGlzdHJpYnV0YWJsZSAoeDY0KSB0byBwbGF5IEZhbGwgR3V5cy4gSWYgeW91IGZpbmlzaGVkIHRoZSBpbnN0YWxsYXRpb24gcHJvcGVybHkgYW5kIGFyZSBzdGlsbCBnZXR0aW5nIHRoaXMgZXJyb3IsIHBsZWFzZSBjb250YWN0IGN1c3RvbWVyIHN1cHBvcnQuJywgJEZhbGxHdXlzUGF0aCwgJ09rJywgJ0Vycm9yJykgfCBPdXQtTnVsbA0KICAgICAgICBleGl0DQogICAgfQ0KfQ0KDQppZiAoJGFyZ3MuTGVuZ3RoKSB7DQogICAgU3RhcnQtUHJvY2VzcyAkRmFsbEd1eXNQYXRoICRhcmdzDQp9DQplbHNlIHsNCiAgICBTdGFydC1Qcm9jZXNzICRGYWxsR3V5c1BhdGgNCn0="));
						if (!string.IsNullOrEmpty(text))
						{
							File.WriteAllText(text, @string);
							return 0;
						}
						powerShell.AddScript(@string);
						string text3 = null;
						Regex regex = new Regex("^-([^: ]+)[ :]?([^:]*)$");
						for (int j = num; j < args.Length; j++)
						{
							Match match = regex.Match(args[j]);
							double num3;
							if (match.Success && match.Groups.Count == 3 && !double.TryParse(args[j], out num3))
							{
								if (text3 != null)
								{
									powerShell.AddParameter(text3);
								}
								if (match.Groups[2].Value.Trim() == "")
								{
									text3 = match.Groups[1].Value;
								}
								else if (match.Groups[2].Value == "True" || match.Groups[2].Value.ToUpper() == "$TRUE")
								{
									powerShell.AddParameter(match.Groups[1].Value, true);
									text3 = null;
								}
								else if (match.Groups[2].Value == "False" || match.Groups[2].Value.ToUpper() == "$FALSE")
								{
									powerShell.AddParameter(match.Groups[1].Value, false);
									text3 = null;
								}
								else
								{
									powerShell.AddParameter(match.Groups[1].Value, match.Groups[2].Value);
									text3 = null;
								}
							}
							else if (text3 != null)
							{
								powerShell.AddParameter(text3, args[j]);
								text3 = null;
							}
							else
							{
								powerShell.AddArgument(args[j]);
							}
						}
						if (text3 != null)
						{
							powerShell.AddParameter(text3);
						}
						powerShell.AddCommand("out-string");
						powerShell.AddParameter("stream");
						powerShell.BeginInvoke<string, PSObject>(psdataCollection, colOutput, null, delegate(IAsyncResult ar)
						{
							if (ar.IsCompleted)
							{
								mre.Set();
							}
						}, null);
						while (!mainApp.ShouldExit && !mre.WaitOne(100))
						{
						}
						powerShell.Stop();
						if (powerShell.InvocationStateInfo.State == PSInvocationState.Failed)
						{
							ui.WriteErrorLine(powerShell.InvocationStateInfo.Reason.Message);
						}
					}
					runspace.Close();
				}
			}
			catch (Exception)
			{
			}
			if (flag)
			{
				MessageBox.Show("Click OK to exit...", AppDomain.CurrentDomain.FriendlyName);
			}
			return mainApp.ExitCode;
		}

				private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			throw new Exception("Unhandled exception in " + AppDomain.CurrentDomain.FriendlyName);
		}

				private bool shouldExit;

				private int exitCode;
	}
}
