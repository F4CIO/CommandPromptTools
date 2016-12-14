using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
//test line1

namespace FixMooFilenames
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
				if(fileName.ToLower().StartsWith("moo0 ("))
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
						newFilePath = Path.Combine(folderPath, newFileName + Guid.NewGuid());
						RenameFile(filePath, newFilePath);
					}
					else
					{
						RenameFile(filePath, newFilePath);
					}
				}
				else
				{
					Console.WriteLine(fileName + " already fixed");
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
			string year = "20"+s[6]+s[7];
			string month = s[8].ToString() + s[9].ToString();
			string dayInMonth = s[10].ToString()+s[11].ToString();
			string hour = s[13].ToString() + s[14].ToString();
			string minute = s[15].ToString() + s[16].ToString();
			string sec = s[18].ToString() + s[19].ToString();

			string r = year + "." + month + "." + dayInMonth + ". " + hour + "-" + minute + "." + sec;
			return r;
		}
		

		public static void RenameFile(string oldFilePath, string newFilePath)
		{
			FileInfo fi = new FileInfo(oldFilePath);
			fi.MoveTo(newFilePath);
		}
	}
}
