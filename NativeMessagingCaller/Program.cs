using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeMessagingCaller
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = false; //required to redirect standart input/output

            // redirects on your choice
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            startInfo.FileName = @"C:\Users\S.VoT\Documents\Visual Studio 2013\Projects\Assembla\Steadfast Chrome Extension\native-client\VTS.ChromeNativeMessaging.exe";
            startInfo.Arguments = "ONE";

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            process.StandardInput.WriteLine("sample");
            process.StandardInput.WriteLine("end");
            process.StandardInput.Close();
            process.Close();
            Console.WriteLine("END");
            Console.ReadKey();
        }
    }
}
