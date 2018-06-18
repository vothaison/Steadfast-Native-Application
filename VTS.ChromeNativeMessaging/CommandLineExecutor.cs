using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VTS.ChromeNativeMessaging
{
    /// <summary>
    /// This class handles the work with cmd.exe, to run nunit-console.exe
    /// </summary>
    public class CommandLineExecutor
    {
        /// <summary>
        /// Create an instance of ProcessStartInfo which holds the information of nunit-console.exe process
        /// </summary>
        /// <returns></returns>
        //private Process CreateProcess()
        //{
        //    System.Diagnostics.Process process = new System.Diagnostics.Process();
        //    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        //    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        //    startInfo.FileName = "cmd.exe";
        //    startInfo.Arguments = "/C copy /b Image1.jpg + Archive.rar Image2.jpg";
        //    process.StartInfo = startInfo;

        //    return process;

        //    //process.Start();
        //}

        /// <summary>
        /// Execute one single test case
        /// </summary>
        /// <param name="command">Instance of CommandContent for the test</param>
        /// <param name="target">Testcase to be test</param>
        /// <returns></returns>
        public void ExecuteCommand(string commandContent, Action<string, string> callback )
        {
            string output;
            this.Callback = callback;
            RunCommand(commandContent);
        }


        void RunCommand(string commandContent)
        {
            //* Create your Process
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c " + commandContent;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            //* Set your output and error (asynchronous) handlers
            process.OutputDataReceived += OutputHandler;
            process.ErrorDataReceived += ErrorHandler;
            //* Start process and handlers
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            Callback(_sbOutput.ToString(), _sbError.ToString());
        }

        private StringBuilder _sbOutput;
        private StringBuilder _sbError;

        public CommandLineExecutor()
        {
            this._sbOutput = new StringBuilder();
            this._sbError = new StringBuilder();
        }

        void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //* Do your stuff with the output (write to console/log/StringBuilder)
            Debug.WriteLine("Output: " + outLine.Data);
            _sbOutput.AppendLine(outLine.Data);
            //
        }

        void ErrorHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //* Do your stuff with the output (write to console/log/StringBuilder)
            Debug.WriteLine("ERROR: " + outLine.Data);
            _sbError.AppendLine(outLine.Data);
            //Callback(outLine.Data);
        }

        public Action<string, string> Callback { get; set; }
    }
}