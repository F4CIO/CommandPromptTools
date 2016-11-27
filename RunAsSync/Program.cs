using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Security;

namespace RunAsSync
{
    class Program
    {
        static int Main(string[] args)
        {
            string error = string.Empty;
            int errorcode = 0;

            try
            {
                if (args.Length != 5 && args.Length != 6)
                {                    
                    errorcode = 1;
                }
                else
                {
                    string domain = args[0];
                    string username = args[1];
                    string password = args[2];
                    int timeOut = 1000 * int.Parse(args[3]);
                    string command = args[4];
                    string arguments = (args.Length == 6) ? args[5] : string.Empty;

                    Process p = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.Domain = domain;
                    startInfo.UserName = username;
                    SecureString secureString = new SecureString();
                    foreach (char c in password.ToCharArray())
                    {
                        secureString.AppendChar(c);
                    }
                    startInfo.Password = secureString;
                    startInfo.FileName = command;
                    if (!string.IsNullOrEmpty(arguments))
                    {
                        startInfo.Arguments = arguments;
                    }
                    p.StartInfo = startInfo;
                    p.StartInfo.UseShellExecute = false;
                    p.Start();
                    p.WaitForExit(timeOut);
                    if (p != null && !p.HasExited)
                    {
                        p.Kill();
                        errorcode = 2;
                    }
                    else
                    {
                        errorcode = p.ExitCode;
                    }
                }
            }
            catch (Exception exception)
            {
                errorcode = 3;
                error = exception.ToString();
            }

            try
            {
                if (errorcode != 0)
                {
                    Console.WriteLine("Runs specified command under specified user account and waits for it to finish. Returns 0 in case of success.");
                    Console.WriteLine("I command is not finished in specified timeout time proccess is killed.");
                    Console.WriteLine("Syntax: RunAsSync domain username password timeoutInSeconds command [arguments]");
                }
                Console.WriteLine("RunAsSync exiting with errorcode: {0}", errorcode);
                Console.WriteLine(error);
            }
            catch { };

            return errorcode;
        }
    }
}
