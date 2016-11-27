using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmptyAllFilesInThisFolder
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("This will delete contents off all .jpg and .mp4 files in this folder. Press C to continue.");
			var r = Console.ReadKey();
			if (r.KeyChar == 'c' || r.KeyChar == 'C')
			{

				List<string> allFilesPaths = GetFilePaths(ApplicationPhysicalPath, "*.jpg");
				foreach (string filePath in allFilesPaths)
				{
					Console.WriteLine(Path.GetFileName(filePath));
					File.Delete(filePath);
					using (File.Create(filePath))
					{
					}

				}

				allFilesPaths = GetFilePaths(ApplicationPhysicalPath, "*.mp4");
				foreach (string filePath in allFilesPaths)
				{
					Console.WriteLine(Path.GetFileName(filePath));
					File.Delete(filePath);
					using (File.Create(filePath))
					{
					}

				}
				Console.WriteLine("Finished.");
				Console.ReadKey();
			}
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

	}


}
