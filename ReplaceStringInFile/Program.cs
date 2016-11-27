using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReplaceStringInFile
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3 || string.IsNullOrEmpty(args[0]) || string.IsNullOrEmpty(args[0]) || string.IsNullOrEmpty(args[2]))
            {
                Console.WriteLine("Syntax: ReplaceStringInFile [full file path] [old string] [new string]");
                Console.WriteLine("Note: arguments must be enclosed with double quotes.");
                Console.WriteLine(@"Example: ReplaceStringInFile ""C:\Program Files\MyFile.txt"" ""myString1"" ""myString2"" ");
            }
            else
            {
                try
                {
                    string content = File.ReadAllText(args[0]);                    
                    if (content.IndexOf(args[1]) == -1)
                    {
                        Console.WriteLine(string.Format("Not found. String searched: '{0}'", args[1]));
                    }
                    else
                    {
                        content = content.Replace(args[1], args[2]);
                        File.WriteAllText(args[0], content);
                        Console.WriteLine(string.Format("Replaced string '{0}' with string '{1}' in file '{2}' ",
                            args[1], args[2], args[0]));
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: " +exception.Message);                                
                }
            }



        }
    }
}
