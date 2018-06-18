using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using VTS.ChromeNativeMessaging;

namespace VTS.UnitTestProject
{
    [TestClass]
    public class WorkerTest
    {
        [TestMethod]
        public void TestWorker()
        {
            Worker worker = new Worker(null);
            worker.Work((output) =>
            {
                Debug.WriteLine("Downloaded " + output);
                return true;
            });
        }
        
        [TestMethod]
        public void TestExecuteCommand()
        {
            Worker worker = new Worker(null);
            worker.Ping((obj) =>
            {
                Debug.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
                return true;
            });
        }
    }

    [TestClass]
    public class GitTest
    {
        [TestMethod]
        public void Test_IsClean()
        {
            SuperGit superGit = new SuperGit();
            superGit.IsClean((output) =>
            {
                Debug.WriteLine("Git clean : " + output);
            });
        }

        [TestMethod]
        public void Test_IsCurrentBranch()
        {
            string name = "14609_Policy_Wizard_Renewal_Questions_Should_Be_In_The_Middle_Of_The_Wizard_";

            SuperGit superGit = new SuperGit();
            superGit.IsCurrentBranch(name, (output) =>
            {
                Debug.WriteLine("is current branch : " + output);
            });
        }
        
        [TestMethod]
        public void Test_HasLocalBranch()
        {
            string name = "14609_Policy_Wizard_Renewal_Questions_Should_Be_In_The_Middle_Of_The_Wizard";

            SuperGit superGit = new SuperGit();
            superGit.HasLocalBranch(name, (output) =>
            {
                Debug.WriteLine("Has Local Branch : " + output);
            });
        }

        [TestMethod]
        public void Test_SwitchToBranch()
        {
            //string name = "14609_Policy_Wizard_Renewal_Questions_Should_Be_In_The_Middle_Of_The_Wizard";
            string name = "master";

            SuperGit superGit = new SuperGit();
            superGit.SwitchToBranch(name, (success, output, command) =>
            {
                if (output == null)
                {
                    Debug.WriteLine("Switched to : " + name);
                }
                
            });
        }



        [TestMethod]
        public void Test_CanCreateCheckoutNewBranch()
        {
            //string name = "14609_Policy_Wizard_Renewal_Questions_Should_Be_In_The_Middle_Of_The_Wizard";
            string name = "9743_Can_Exit_Wizards_By_Clicking_Left_hand_Menu_Buttons";

            SuperGit superGit = new SuperGit();
            superGit.CanCreateCheckoutNewBranch(name, (success, output, command) =>
            {
                if (output == null)
                {
                    Debug.WriteLine("Switched to : " + name);
                }
                else
                {
                    Debug.WriteLine(output);
                }

            });
        }
        
        [TestMethod]
        public void Test_CreateCheckoutNewBranch()
        {
            //string name = "14609_Policy_Wizard_Renewal_Questions_Should_Be_In_The_Middle_Of_The_Wizard";
            string name = "Create_Checkout_NewBranch_"+DateTime.Now.Ticks;

            SuperGit superGit = new SuperGit();
            superGit.CreateCheckoutNewBranch(name, (success, output, command) =>
            {
                if (output == null)
                {
                    Debug.WriteLine("Switched to : " + name);
                }
                else
                {
                    Debug.WriteLine(output);
                }
                
            });
        }
    }
}
