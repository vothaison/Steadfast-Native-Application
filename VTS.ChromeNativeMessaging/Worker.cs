using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;

namespace VTS.ChromeNativeMessaging
{
    public class Input
    {
        
    }

    public class Worker
    {
        private WebClient client;
        private string input;

        public Worker(string input)
        {
            this.input = input;
        }

        public void Work(Func<string, bool> callback)
        {
            this.Callback = callback;   
            string downloadLink =
                "https://copy.com/yC42r9dpWFTWLHTS/10%20Minions%20Run%20Amok.m4a";
            string downloadLocation = @"C:\Song.mp3";
            
            client = new WebClient();
            Uri uri = new Uri(downloadLink);


            client.DownloadFileCompleted += DownloadFileCallback;
            TaskHelpers.Delay(TimeSpan.FromSeconds(0.5));
            client.DownloadFileAsync(uri, downloadLocation);
            TaskHelpers.Delay(TimeSpan.FromSeconds(17));
        }

        public void Ping(Func<object, bool> callback)
        {
            CommandLineExecutor executor = new CommandLineExecutor();
            executor.ExecuteCommand("ping google.com", (output, error) =>
            {
                Debug.WriteLine("Full ouput: " + output);
                Debug.WriteLine("Full error: " + error);
                callback( new 
                {
                    output,
                    error
                });
            });
        }

        private void DownloadFileCallback(object sender, AsyncCompletedEventArgs e)
        {
            Callback("");
        }

        public Func<string, bool> Callback { get; set; }
    }
}
