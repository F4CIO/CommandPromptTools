using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FixAthtekFilenames
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
			List<string> monthNames = new List<string>(){"january","february","march","april","may","june","july","august","september","october","november","december"};
			foreach (string filePath in allFilesPaths)
			{
				string folderPath = Path.GetDirectoryName(filePath);
				string fileName = Path.GetFileNameWithoutExtension(filePath);
				string fileExtension = Path.GetExtension(filePath).TrimStart('.');
				if (monthNames.Any(mn => fileName.ToLower().Contains(mn)))
				{
					string newFileName = FixFileName(fileName) + "." + fileExtension;
					Console.WriteLine(fileName);
					string newFilePath = Path.Combine(folderPath, newFileName);
					if (File.Exists(newFilePath) && new FileInfo(newFileName).Length == new FileInfo(filePath).Length)
					{
						//already fixed
						File.Delete(filePath);
					}
					else if (File.Exists(newFilePath) && new FileInfo(newFileName).Length != new FileInfo(filePath).Length)
					{
						//somethings wrong -dump with guid
						newFilePath = Path.Combine(folderPath, newFileName+Guid.NewGuid());
						RenameFile(filePath, newFilePath);
					}
					else
					{
						RenameFile(filePath, newFilePath);
					}
				}
				else
				{
					Console.WriteLine(fileName+" already fixed");
				}

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

	
		public static string FixFileName(string s)
		{
			string dayInMonth = s.Split()[2].Trim().Trim('.').Trim(',').Trim().PadLeft(2, '0');
			string month = GetMonthNumberByName(s.Split()[1].Trim()).ToString().PadLeft(2, '0');
			string year = s.Split()[3].Trim();
			string time = s.Split()[4].Trim().Replace('.','-');
			
			string contact = string.Empty;
			s.Split().Skip(5).ToList().ForEach(c => contact = contact + " " + c);
			contact = contact.Trim();
			
			string r  = String.Format(year+"."+month+"."+dayInMonth+". "+time+" "+contact).Trim();
			return r;
		}

		public static int GetMonthNumberByName(string m)
		{
			int r = -1;
			switch (m.ToLower().Trim())
			{
				case "january":r = 1;break;
				case "february":r = 2;break;
				case "march":r = 3;break;
				case "april":r = 4;break;
				case "may":r = 5;break;
				case "june":r = 6;break;
				case "july":r = 7;break;
				case "august":r = 8;break;
				case "september":r = 9;break;
				case "october":r = 10;break;
				case "november":r = 11;break;
				case "december":r = 12;break;
			}
			return r;
		}

		public static void RenameFile(string oldFilePath, string newFilePath)
		{
			FileInfo fi = new FileInfo(oldFilePath);
			fi.MoveTo(newFilePath);
		}
		
	}
}
