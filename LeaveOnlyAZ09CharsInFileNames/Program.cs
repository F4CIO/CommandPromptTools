using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LeaveOnlyAZ09CharsInFileNames
{
	class Program
	{
		static void Main(string[] args)
		{
			
			//Console.WriteLine("This will rename all files in current folder so that all of them contain A to Z and 0 to 1 and _ chars in their names. Press C to continue.");
			//var r = Console.ReadKey();
			//if (r.KeyChar == 'c' || r.KeyChar == 'C')
			//{

				List<string> allFilesPaths = GetFilePaths(ApplicationPhysicalPath, "*.*");
				string fileNameToExclude = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
				allFilesPaths = allFilesPaths.Where(f => !Path.GetFileNameWithoutExtension(f).ToLower().StartsWith(fileNameToExclude.ToLower())).ToList();
				foreach (string filePath in allFilesPaths)
				{
					string folderPath = Path.GetDirectoryName(filePath);
					string fileName = Path.GetFileNameWithoutExtension(filePath);
					string fileExtension = Path.GetExtension(filePath).TrimStart('.');
					string newFileName = ReplaceNonAlphaNumericCharacters(fileName, "_") + "." + ReplaceNonAlphaNumericCharacters(fileExtension, "_");
					Console.WriteLine(fileName);
					string newFilePath = Path.Combine(folderPath, newFileName);
					RenameFile(filePath, newFilePath);

				}

				Console.WriteLine("Finished.");
				//Console.ReadKey();
			//}
		}

		/// <summary>
		/// Gets list of strings where each is full path to file including filename (for example: <example>c:\dir\filename.ext</example>.
		/// </summary>
		/// <param name="folder">Full path of folder that should be searched. For example: <example>c:\dir</example>.</param>
		/// <param name="searchPatern">Filter that should be used. For example: <example>*.txt</example></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">Thrown when parameter is null or empty.</exception>
		public static List<string> GetFilePaths(string folderPath, string searchPatern)
		{
			if (string.IsNullOrEmpty(folderPath)) throw new ArgumentException("Value must be non-empty string.", "folderPath");
			if (string.IsNullOrEmpty(searchPatern)) throw new ArgumentException("Value must be non-empty string.", "searchPatern");

			List<string> filePaths = new List<string>();
			string[] filePathStrings = Directory.GetFiles(folderPath, searchPatern);
			if (filePathStrings != null)
			{
				filePaths.AddRange(filePathStrings);
			}

			return filePaths;
		}

		/// <summary>
		/// Example: C:\App1
		/// </summary>
		public static string ApplicationPhysicalPath
		{
			get
			{
				return Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", string.Empty);
			}

		}

		/// <summary>
		/// All except these chars are replaced:
		/// A-Z,a-Z,0-9
		/// </summary>
		/// <param name="s"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		public static string ReplaceNonAlphaNumericCharacters(string s, string r)
		{
			StringBuilder sb1 = new StringBuilder(s.Length);
			foreach (char ch in s)
			{
				if (Char.IsLetterOrDigit(ch))
				{
					sb1.Append(ch);
				}
				else
				{
					sb1.Append(r);
				}
			}
			return sb1.ToString();
		}

		public static void RenameFile(string oldFilePath, string newFilePath)
		{
			FileInfo fi = new FileInfo(oldFilePath);
			fi.MoveTo(newFilePath);
		}
		
	}
}
