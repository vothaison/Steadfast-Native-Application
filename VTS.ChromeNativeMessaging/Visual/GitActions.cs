using System;
using System.IO;

namespace VTS.ChromeNativeMessaging.Visual
{
    public static class GitActions
    {
        public static void OpenGitLog(string solutionFolder, string fileIdentifier, int lineNumber = 0)
        {
            string filePath = Path.Combine(solutionFolder, fileIdentifier);

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "TortoiseGitProc";
            startInfo.Arguments = "/command:log /path:" + filePath;
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}