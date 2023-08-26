using System;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Text;

namespace ModuleNameSpace
{
		internal class Credential_Form
	{
				[DllImport("credui", CharSet = CharSet.Unicode)]
		private static extern Credential_Form.CredUI_ReturnCodes CredUIPromptForCredentials(ref Credential_Form.CREDUI_INFO credinfo, string targetName, IntPtr reserved1, int iError, StringBuilder userName, int maxUserName, StringBuilder password, int maxPassword, [MarshalAs(UnmanagedType.Bool)] ref bool pfSave, Credential_Form.CREDUI_FLAGS flags);

				internal static Credential_Form.User_Pwd PromptForPassword(string caption, string message, string target, string user, PSCredentialTypes credTypes, PSCredentialUIOptions options)
		{
			StringBuilder stringBuilder = new StringBuilder("", 128);
			StringBuilder stringBuilder2 = new StringBuilder(user, 128);
			Credential_Form.CREDUI_INFO structure = default(Credential_Form.CREDUI_INFO);
			if (!string.IsNullOrEmpty(message))
			{
				structure.pszMessageText = message;
			}
			if (!string.IsNullOrEmpty(caption))
			{
				structure.pszCaptionText = caption;
			}
			structure.cbSize = Marshal.SizeOf<Credential_Form.CREDUI_INFO>(structure);
			bool flag = false;
			Credential_Form.CREDUI_FLAGS credui_FLAGS = Credential_Form.CREDUI_FLAGS.DO_NOT_PERSIST;
			if ((credTypes & PSCredentialTypes.Generic) == PSCredentialTypes.Generic)
			{
				credui_FLAGS |= Credential_Form.CREDUI_FLAGS.GENERIC_CREDENTIALS;
				if ((options & PSCredentialUIOptions.AlwaysPrompt) == PSCredentialUIOptions.AlwaysPrompt)
				{
					credui_FLAGS |= Credential_Form.CREDUI_FLAGS.ALWAYS_SHOW_UI;
				}
			}
			if (Credential_Form.CredUIPromptForCredentials(ref structure, target, IntPtr.Zero, 0, stringBuilder2, 128, stringBuilder, 128, ref flag, credui_FLAGS) == Credential_Form.CredUI_ReturnCodes.NO_ERROR)
			{
				return new Credential_Form.User_Pwd
				{
					User = stringBuilder2.ToString(),
					Password = stringBuilder.ToString(),
					Domain = ""
				};
			}
			return null;
		}

				[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct CREDUI_INFO
		{
						public int cbSize;

						public IntPtr hwndParent;

						public string pszMessageText;

						public string pszCaptionText;

						public IntPtr hbmBanner;
		}

				[Flags]
		private enum CREDUI_FLAGS
		{
						INCORRECT_PASSWORD = 1,
						DO_NOT_PERSIST = 2,
						REQUEST_ADMINISTRATOR = 4,
						EXCLUDE_CERTIFICATES = 8,
						REQUIRE_CERTIFICATE = 16,
						SHOW_SAVE_CHECK_BOX = 64,
						ALWAYS_SHOW_UI = 128,
						REQUIRE_SMARTCARD = 256,
						PASSWORD_ONLY_OK = 512,
						VALIDATE_USERNAME = 1024,
						COMPLETE_USERNAME = 2048,
						PERSIST = 4096,
						SERVER_CREDENTIAL = 16384,
						EXPECT_CONFIRMATION = 131072,
						GENERIC_CREDENTIALS = 262144,
						USERNAME_TARGET_CREDENTIALS = 524288,
						KEEP_USERNAME = 1048576
		}

				public enum CredUI_ReturnCodes
		{
						NO_ERROR,
						ERROR_CANCELLED = 1223,
						ERROR_NO_SUCH_LOGON_SESSION = 1312,
						ERROR_NOT_FOUND = 1168,
						ERROR_INVALID_ACCOUNT_NAME = 1315,
						ERROR_INSUFFICIENT_BUFFER = 122,
						ERROR_INVALID_PARAMETER = 87,
						ERROR_INVALID_FLAGS = 1004
		}

				public class User_Pwd
		{
						public string User = string.Empty;

						public string Password = string.Empty;

						public string Domain = string.Empty;
		}
	}
}
