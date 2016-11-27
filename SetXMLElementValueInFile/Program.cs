using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace SetXMLElementValueInFile
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3 || string.IsNullOrEmpty(args[0]) || string.IsNullOrEmpty(args[0]) || string.IsNullOrEmpty(args[2]))
            {
                Console.WriteLine("Sets value of xml element in file.");
                Console.WriteLine("Syntax: SetXMLElementValueInFile [full file path] [XPath to element] [new value for element]");
                Console.WriteLine("Note: arguments must be enclosed with double quotes.");
                Console.WriteLine(@"Example: SetXMLElementValueInFile ""C:\Program Files\MyFile.xml"" ""/myElementParent/MyElement"" ""newValue"" ");
            }
            else
            {
                try
                {
                   string content = File.ReadAllText(args[0]);
                   XmlDocument doc = new XmlDocument();
                   doc.LoadXml(content);
                   XmlNode e = doc.SelectSingleNode(args[1]);
                   e.InnerText = args[2];
                   doc.Save(args[0]);
                   Console.WriteLine(string.Format("XML value '{0}' set in file '{1}' on element '{2}'",
                       args[2],args[0],args[1]));
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: " + exception.Message);
                }
            }
        }
    }
}
