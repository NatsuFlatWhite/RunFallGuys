using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Reflection;
using System.Security;
using System.Windows.Forms;

namespace ModuleNameSpace
{
	internal class MainModuleUI : PSHostUserInterface
	{
		public MainModuleUI()
		{
			this.rawUI = new MainModuleRawUI();
		}

		public override Dictionary<string, PSObject> Prompt(string caption, string message, Collection<FieldDescription> descriptions)
		{
			if (!string.IsNullOrEmpty(caption) || !string.IsNullOrEmpty(message))
			{
				string caption2 = AppDomain.CurrentDomain.FriendlyName;
				string text = "";
				if (!string.IsNullOrEmpty(caption))
				{
					caption2 = caption;
				}
				if (!string.IsNullOrEmpty(message))
				{
					text = message;
				}
				MessageBox.Show(text, caption2);
			}
			this.ib_caption = "";
			this.ib_message = "";
			Dictionary<string, PSObject> dictionary = new Dictionary<string, PSObject>();
			foreach (FieldDescription fieldDescription in descriptions)
			{
				Type type = null;
				if (string.IsNullOrEmpty(fieldDescription.ParameterAssemblyFullName))
				{
					type = typeof(string);
				}
				else
				{
					type = Type.GetType(fieldDescription.ParameterAssemblyFullName);
				}
				if (type.IsArray)
				{
					Type elementType = type.GetElementType();
					Type type2 = Type.GetType("System.Collections.Generic.List" + '`'.ToString() + "1");
					type2 = type2.MakeGenericType(new Type[]
					{
						elementType
					});
					ConstructorInfo constructor = type2.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, Type.EmptyTypes, null);
					object target = constructor.Invoke(null);
					int num = 0;
					for (;;)
					{
						try
						{
							if (!string.IsNullOrEmpty(fieldDescription.Name))
							{
								this.ib_message = string.Format("{0}[{1}]: ", fieldDescription.Name, num);
							}
							string value = this.ReadLine();
							if (string.IsNullOrEmpty(value))
							{
								break;
							}
							object obj = Convert.ChangeType(value, elementType);
							type2.InvokeMember("Add", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, null, target, new object[]
							{
								obj
							});
						}
						catch (Exception ex)
						{
							throw ex;
						}
						num++;
					}
					Array obj2 = (Array)type2.InvokeMember("ToArray", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, null, target, null);
					dictionary.Add(fieldDescription.Name, new PSObject(obj2));
				}
				else
				{
					object obj3 = null;
					string text2 = null;
					try
					{
						if (type != typeof(SecureString))
						{
							if (type != typeof(PSCredential))
							{
								if (!string.IsNullOrEmpty(fieldDescription.Name))
								{
									this.ib_message = string.Format("{0}: ", fieldDescription.Name);
								}
								if (!string.IsNullOrEmpty(fieldDescription.HelpMessage))
								{
									this.ib_message += "\n(Type !? for help.)";
								}
								do
								{
									text2 = this.ReadLine();
									if (text2 == "!?")
									{
										this.WriteLine(fieldDescription.HelpMessage);
									}
									else
									{
										if (string.IsNullOrEmpty(text2))
										{
											obj3 = fieldDescription.DefaultValue;
										}
										if (obj3 == null)
										{
											try
											{
												obj3 = Convert.ChangeType(text2, type);
											}
											catch
											{
												this.Write("Wrong format, please repeat input: ");
												text2 = "!?";
											}
										}
									}
								}
								while (text2 == "!?");
							}
							else
							{
								PSCredential pscredential = this.PromptForCredential("", "", "", "");
								obj3 = pscredential;
							}
						}
						else
						{
							if (!string.IsNullOrEmpty(fieldDescription.Name))
							{
								this.ib_message = string.Format("{0}: ", fieldDescription.Name);
							}
							SecureString secureString = this.ReadLineAsSecureString();
							obj3 = secureString;
						}
						dictionary.Add(fieldDescription.Name, new PSObject(obj3));
					}
					catch (Exception ex2)
					{
						throw ex2;
					}
				}
			}
			this.ib_caption = "";
			this.ib_message = "";
			return dictionary;
		}
		public override int PromptForChoice(string caption, string message, Collection<ChoiceDescription> choices, int defaultChoice)
		{
			int num = Choice_Box.Show(choices, defaultChoice, caption, message);
			if (num == -1)
			{
				num = defaultChoice;
			}
			return num;
		}
		public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName, PSCredentialTypes allowedCredentialTypes, PSCredentialUIOptions options)
		{
			Credential_Form.User_Pwd user_Pwd = Credential_Form.PromptForPassword(caption, message, targetName, userName, allowedCredentialTypes, options);
			if (user_Pwd != null)
			{
				SecureString secureString = new SecureString();
				foreach (char c in user_Pwd.Password.ToCharArray())
				{
					secureString.AppendChar(c);
				}
				return new PSCredential(user_Pwd.User, secureString);
			}
			return null;
		}
		public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName)
		{
			Credential_Form.User_Pwd user_Pwd = Credential_Form.PromptForPassword(caption, message, targetName, userName, PSCredentialTypes.Default, PSCredentialUIOptions.Default);
			if (user_Pwd != null)
			{
				SecureString secureString = new SecureString();
				foreach (char c in user_Pwd.Password.ToCharArray())
				{
					secureString.AppendChar(c);
				}
				return new PSCredential(user_Pwd.User, secureString);
			}
			return null;
		}
		public override PSHostRawUserInterface RawUI
		{
			get
			{
				return this.rawUI;
			}
		}
		public override string ReadLine()
		{
			string result = "";
			if (Input_Box.Show(this.ib_caption, this.ib_message, ref result) == DialogResult.OK)
			{
				return result;
			}
			return "";
		}
		private SecureString getPassword()
		{
			SecureString secureString = new SecureString();
			for (;;)
			{
				ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
				if (consoleKeyInfo.Key == ConsoleKey.Enter)
				{
					break;
				}
				if (consoleKeyInfo.Key == ConsoleKey.Backspace)
				{
					if (secureString.Length > 0)
					{
						secureString.RemoveAt(secureString.Length - 1);
						Console.Write("\b \b");
					}
				}
				else if (consoleKeyInfo.KeyChar != '\0')
				{
					secureString.AppendChar(consoleKeyInfo.KeyChar);
					Console.Write("*");
				}
			}
			Console.WriteLine();
			return secureString;
		}
		public override SecureString ReadLineAsSecureString()
		{
			SecureString secureString = new SecureString();
			string text = "";
			if (Input_Box.Show(this.ib_caption, this.ib_message, ref text, true) == DialogResult.OK)
			{
				foreach (char c in text)
				{
					secureString.AppendChar(c);
				}
			}
			return secureString;
		}
		public override void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
		{
		}
		public override void Write(string value)
		{
		}
		public override void WriteDebugLine(string message)
		{
		}
		public override void WriteErrorLine(string value)
		{
		}
		public override void WriteLine()
		{
		}
		public override void WriteLine(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
		{
		}
		public override void WriteLine(string value)
		{
		}
		public override void WriteProgress(long sourceId, ProgressRecord record)
		{
			if (this.pf == null)
			{
				if (record.RecordType == ProgressRecordType.Completed)
				{
					return;
				}
				this.pf = new Progress_Form(this.ProgressForegroundColor);
				this.pf.Show();
			}
			this.pf.Update(record);
			if (record.RecordType == ProgressRecordType.Completed && this.pf.GetCount() == 0)
			{
				this.pf = null;
			}
		}
		public override void WriteVerboseLine(string message)
		{
		}
		public override void WriteWarningLine(string message)
		{
		}
		private MainModuleRawUI rawUI;
		public ConsoleColor ErrorForegroundColor = ConsoleColor.Red;
		public ConsoleColor ErrorBackgroundColor;
		public ConsoleColor WarningForegroundColor = ConsoleColor.Yellow;
		public ConsoleColor WarningBackgroundColor;
		public ConsoleColor DebugForegroundColor = ConsoleColor.Yellow;
		public ConsoleColor DebugBackgroundColor;
		public ConsoleColor VerboseForegroundColor = ConsoleColor.Yellow;
		public ConsoleColor VerboseBackgroundColor;
		public ConsoleColor ProgressForegroundColor = ConsoleColor.DarkCyan;
		public ConsoleColor ProgressBackgroundColor = ConsoleColor.DarkCyan;
		private string ib_caption;
		private string ib_message;
		public Progress_Form pf;
	}
}
