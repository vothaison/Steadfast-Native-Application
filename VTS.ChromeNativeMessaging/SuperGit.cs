using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;

namespace VTS.ChromeNativeMessaging
{
    public class SuperGit
    {
        string gitRoot = @"c:\dev\boa";

        /// <summary>
        /// Do we have a branch for this task
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool HasLocalBranch(string name, Action<bool> callback)
        {
            string gitCommand = String.Format(@" git rev-parse --verify ""{0}""", name);
            string cmd = String.Format(@"cd /d ""{0}"" & {1}", gitRoot, gitCommand);

            CommandLineExecutor executor = new CommandLineExecutor();
            executor.ExecuteCommand(cmd, (output, error) =>
            {
                string[] parts = output.Split('\n');

                if (parts.Length > 0)
                {
                    string branchId = parts[0].Trim();
                    if (Regex.Match(branchId, "^[0-9a-f]{40}$").Success)
                    {
                        callback(true);
                        return;
                    }
                }

                callback(false);
            });

            return true;
        }

        /// <summary>
        /// Are we in this branch right now?
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool IsCurrentBranch(string name, Action<bool> callback)
        {
            CommandLineExecutor executor = new CommandLineExecutor();
            executor.ExecuteCommand(" cd /d c:\\dev\\boa & git branch", (output, error) =>
            {
                string[] parts = output.Split('\n');
                bool found = false;

                if (parts.Length > 0)
                {
                    foreach (var part in parts)
                    {
                        if (part.StartsWith("*") && part.Contains(name))
                        {
                            found = true;
                            break;
                        }
                    }

                }

                callback(found);
            });
            return true;
        }

        public void IsClean(Action<bool> callback)
        {
            CommandLineExecutor executor = new CommandLineExecutor();
            executor.ExecuteCommand(" cd /d c:\\dev\\boa & git status", (output, error) =>
            {
                string[] parts = output.Split('\n');
                bool isClean = false;

                if (parts.Length > 3)
                {
                    isClean = output.Contains("nothing to commit, working directory clean")
                        || !output.Contains("Changes not staged for commit:");
                }

                callback(isClean);
            });
        }

        private const string RefreshStatus = "Refresh Status";

        public void SwitchToBranch(string name, Action<bool, string, string> callback)
        {
            HasLocalBranch(name, (has) =>
            {
                if (has)
                {
                    IsClean((clean) =>
                    {
                        if (clean)
                        {
                            CommandLineExecutor executor = new CommandLineExecutor();
                            string gitCommand = String.Format(@"git checkout ""{0}""", name);
                            string cmd = String.Format(@"cd /d ""{0}"" & {1}", gitRoot, gitCommand);

                            executor.ExecuteCommand(cmd, (output, error) =>
                            {
                                if (output.Contains("Switched to branch ")
                                    || error.Contains("Switched to branch ")
                                    || error.Contains("Already on "))
                                {
                                    callback(true, "Some error occurred", RefreshStatus);
                                }
                                else
                                {
                                    callback(false, "Some error occurred", RefreshStatus);
                                }
                            });
                        }
                        else
                        {
                            callback(false, "Git not clean. Please commit your changes.", RefreshStatus);
                        }
                    });
                }
                else
                {
                    callback(false, "There is no branch with name: " + name, RefreshStatus);
                }

            });
        }

        public void CheckoutBranch(string name, Action<bool, string, string> callback)
        {
            SwitchToBranch(name, callback);
        }

        public void PullCodeUpstreamMaster(Action<bool> callback)
        {
            CommandLineExecutor executor = new CommandLineExecutor();
            string gitCommand = @"git pull upstream master";
            string cmd = String.Format(@"cd /d ""{0}"" & {1}", gitRoot, gitCommand);

            executor.ExecuteCommand(cmd, (output, error) =>
            {
                callback(output.Contains("Fast-forward")
                    || output.Contains("Already up-to-date"));
            });
        }

        public void CreateCheckoutNewBranch(string name, Action<bool, string, string> callback)
        {
            SwitchToBranch("master", (switchSuccess, result, command) =>
            {
                if (switchSuccess)
                {
                    //PullCodeUpstreamMaster((pullSuccess) =>
                    //{
                    //    if (pullSuccess)
                    //    {
                            CommandLineExecutor executor = new CommandLineExecutor();
                            string gitCommand = String.Format(@"git checkout -b ""{0}""", name);
                            string cmd = String.Format(@"cd /d ""{0}"" & {1}", gitRoot, gitCommand);

                            executor.ExecuteCommand(cmd, (output, error) =>
                            {
                                if (output.Contains("Switched to ") || error.Contains("Switched to "))
                                {
                                    callback(true, "You are working on this branch right now", RefreshStatus);
                                }
                                else
                                {
                                    callback(true, "For some reason, an error occurred", RefreshStatus);
                                }
                            });
                    //    }
                    //    else
                    //    {
                    //        callback(false, "Could not pull upstream master", RefreshStatus);
                    //    }
                    //});
                }
                else
                {
                    callback(false, "Could not switch to master: ", RefreshStatus);
                }
            });

        }

        public void CanCreateCheckoutNewBranch(string name, Action<bool, string, string> callback)
        {
            HasLocalBranch(name, (has) =>
            {
                if (has)
                {
                    IsCurrentBranch(name, (current) =>
                    {
                        if (current)
                        {
                            callback(true, "You are working on this branch right now", RefreshStatus);
                        }
                        else
                        {
                            IsClean((clean) =>
                            {
                                if (clean)
                                {
                                    callback(false, "You are NOT on this branch. Your git is clean. You can switch to this branch.", "Checkout This Branch");
                                }
                                else
                                {
                                    callback(true, "You are NOT on this branch. You git is NOT clean.", RefreshStatus);
                                }
                            });
                        }
                    });
                }
                else
                {
                    IsClean((clean) =>
                    {
                        if (clean)
                        {
                            callback(true, "Git is clean. Ready to create new branch.", "Create New Branch");
                        }
                        else
                        {
                            callback(false, "Git not clean. Please commit your changes.", RefreshStatus);
                        }
                    });
                }
            });


        }

    }
}
