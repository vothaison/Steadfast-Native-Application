
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
    public class GitActionsTests
    {
        [TestMethod]
        public void OpenGitLog()
        {
            GitActions.OpenGitLog(@"C:\Dev\boa", @"DataAccess/SQL/BOAIpLookup/README.md ", 0);
        }
        
        
    }
}
