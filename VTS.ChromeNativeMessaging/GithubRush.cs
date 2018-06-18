using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using MsdevManager;

namespace VTS.ChromeNativeMessaging
{
    public class GithubRush
    {
        private readonly string _solutionPath;
        private DTE2 _solution;

        public GithubRush(string solutionPath = @"C:\Dev\Boa\Boa.sln")
        {
            _solutionPath = solutionPath;
        }

        public void OpenFileInVisualStudio(string filename, int lineNumber)
        {
            try
            {
                EnvDTE80.DTE2 boa = (EnvDTE80.DTE2)Msdev.GetIDEInstance(_solutionPath);

                if (boa == null)
                {
                   boa = OpenBoaSolution();
                }

                File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "filename.txt"), filename);

                boa.ExecuteCommand("File.OpenFile", filename);

                ((EnvDTE.TextSelection)boa.ActiveDocument.Selection).GotoLine(lineNumber, true);
                boa.ActiveWindow.Activate();

                boa.MainWindow.WindowState = vsWindowState.vsWindowStateMinimize;
                boa.ActiveWindow.WindowState = vsWindowState.vsWindowStateMinimize;

                boa.ActiveWindow.Activate();
                boa.MainWindow.Activate();
                
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

        public bool IsBoaOpened()
        {
            var boa = Msdev.GetIDEInstance(_solutionPath);
            return boa != null;
        }

        public DTE2 OpenBoaSolution()
        {
            System.Type type = System.Type.GetTypeFromProgID
    ("VisualStudio.DTE.12.0");
            Object obj = System.Activator.CreateInstance(type, true);
            EnvDTE80.DTE2 dte8Obj = (EnvDTE80.DTE2)obj;
            dte8Obj.MainWindow.Activate();
            dte8Obj.Solution.Open(_solutionPath);
            
            _solution = dte8Obj;

            return dte8Obj;
        }


    }
}
