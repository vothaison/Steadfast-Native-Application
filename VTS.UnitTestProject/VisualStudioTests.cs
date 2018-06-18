
using System;
using System.Diagnostics;
using System.IO;
using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsdevManager;
using Newtonsoft.Json;
using VTS.ChromeNativeMessaging;
using VTS.ChromeNativeMessaging.Visual;

namespace VTS.UnitTestProject
{
    [TestClass]
    public class VisualStudioTests
    {
        [TestMethod]
        public void LocateVisualStudioSourceFile()
        {
            string urlPathName = "/Boa/Security";
            string currentRouteName = "processing-permissions-group.tasks";


            
        }

        [TestMethod]
        public void Two()
        {
            new GithubRush().OpenBoaSolution();

        }

        
    }
}
