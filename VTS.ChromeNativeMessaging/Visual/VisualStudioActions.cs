using System;
using System.Diagnostics;
using System.IO;
using EnvDTE;
using MsdevManager;

namespace VTS.ChromeNativeMessaging.Visual
{
    public static class VisualStudioActions
    {
        public const string FILE_OPENFILE = "File.OpenFile";

        public static void LocateSourceFile(string solutionFolder, string currentRouteName, string locationPathName, int lineNumber)
        {
            string emberFolder = Path.Combine(solutionFolder, @"Web\Ember");
            string appName = locationPathName.Replace("/Boa/", "").Replace("/", "");

            if (String.IsNullOrEmpty(appName))
            {
                appName = "Home";
            }

            string appFolder = Path.Combine(emberFolder, appName);
            string controllerFolder = Path.Combine(appFolder, @"Index\controllers");
            string fileName = currentRouteName.Replace(".", "\\") + ".js";
            string filePath = Path.Combine(controllerFolder, fileName);

            OpenFile(solutionFolder, filePath, lineNumber);
        }

        private static void OpenFile(string solutionFolder, string filePath, int lineNumber)
        {
            string solutionPath = Path.Combine(solutionFolder, "Boa.sln");
            {
                try
                {
                    EnvDTE80.DTE2 boa = (EnvDTE80.DTE2)Msdev.GetIDEInstance(solutionPath);
                    
                    if (boa == null)
                    {
                        throw new Exception("BOA is not opened in Visual Studio");
                    }

                    boa.ExecuteCommand(FILE_OPENFILE, filePath);

                    ((EnvDTE.TextSelection)boa.ActiveDocument.Selection).GotoLine(lineNumber, true);

                    boa.ActiveWindow.Activate();

                    boa.MainWindow.WindowState = vsWindowState.vsWindowStateMinimize;
                    boa.ActiveWindow.WindowState = vsWindowState.vsWindowStateMinimize;

                    boa.ActiveWindow.Activate();
                    boa.MainWindow.Activate();

                }
                catch (Exception e)
                {
                    Debug.Write(e.Message);
                }
            }

        }

        public static void LocateSourceFileByFileIdentifier(string solutionFolder, string fileIdentifier, int lineNumber)
        {
            string filePath = Path.Combine(solutionFolder, fileIdentifier);
            OpenFile(solutionFolder, filePath, lineNumber);
        }
    }
}