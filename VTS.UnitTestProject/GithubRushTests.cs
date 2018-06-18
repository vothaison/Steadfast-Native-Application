
using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using VTS.ChromeNativeMessaging;

namespace VTS.UnitTestProject
{
    [TestClass]
    public class GithubRushTests
    {
        [TestMethod]
       public void One ()
        {
            new GithubRush().OpenFileInVisualStudio(@"C:\Dev\boa\Web\Content\boa-utils.less", 80);
        }

        [TestMethod]
        public void Two ()
        {
            new GithubRush().OpenBoaSolution();
            
        }
    }
}
