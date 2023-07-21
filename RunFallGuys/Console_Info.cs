using System;
using System.Runtime.InteropServices;

namespace ModuleNameSpace
{
	public class Console_Info
	{		
		[DllImport("Kernel32.dll")]
		private static extern UIntPtr GetStdHandle(Console_Info.STDHandle stdHandle);

		[DllImport("Kernel32.dll")]
		private static extern Console_Info.FileType GetFileType(UIntPtr hFile);

		public static bool IsInputRedirected()
		{
			UIntPtr stdHandle = Console_Info.GetStdHandle((Console_Info.STDHandle)4294967286U);
			Console_Info.FileType fileType = Console_Info.GetFileType(stdHandle);
			return fileType != Console_Info.FileType.FILE_TYPE_CHAR && fileType != Console_Info.FileType.FILE_TYPE_UNKNOWN;
		}

		public static bool IsOutputRedirected()
		{
			UIntPtr stdHandle = Console_Info.GetStdHandle((Console_Info.STDHandle)4294967285U);
			Console_Info.FileType fileType = Console_Info.GetFileType(stdHandle);
			return fileType != Console_Info.FileType.FILE_TYPE_CHAR && fileType != Console_Info.FileType.FILE_TYPE_UNKNOWN;
		}

		public static bool IsErrorRedirected()
		{
			UIntPtr stdHandle = Console_Info.GetStdHandle((Console_Info.STDHandle)4294967284U);
			Console_Info.FileType fileType = Console_Info.GetFileType(stdHandle);
			return fileType != Console_Info.FileType.FILE_TYPE_CHAR && fileType != Console_Info.FileType.FILE_TYPE_UNKNOWN;
		}

		private enum FileType : uint
		{
			FILE_TYPE_UNKNOWN,
			FILE_TYPE_DISK,
			FILE_TYPE_CHAR,
			FILE_TYPE_PIPE,
			FILE_TYPE_REMOTE = 32768U
		}
		private enum STDHandle : uint
		{
			STD_INPUT_HANDLE = 4294967286U,
			STD_OUTPUT_HANDLE = 4294967285U,
			STD_ERROR_HANDLE = 4294967284U
		}
	}
}
