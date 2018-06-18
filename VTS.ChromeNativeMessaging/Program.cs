using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VTS.ChromeNativeMessaging.Services;
using VTS.ChromeNativeMessaging.Support;

namespace VTS.ChromeNativeMessaging
{
    public static class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            // Start
            var linkTimeLocal = Assembly.GetExecutingAssembly().GetLinkerDateTime();
            log.Info("Started ChromeNativeMessaging. Build at: " + linkTimeLocal);

            // Error handling
            AppDomain.CurrentDomain.UnhandledException
                += delegate (object sender, UnhandledExceptionEventArgs _args)
                {
                    Exception e = (Exception)_args.ExceptionObject;
                    log.Error(e.Message, e);
                    Environment.Exit(1);
                };

            // Read input
            string input = OpenStandardStreamIn();
            log.Info("Standard Stream In: " + input + ' ' + (string.IsNullOrEmpty(input) ? "(EMPTY)" : "(NOT EMPTY)"));

            if (string.IsNullOrEmpty(input))
            {
                WriteResponse(JsonConvert.SerializeObject(new { Error = "Expecting none-empty input from stdin." }));
            }
            else
            {
                RequestRaw raw = JsonConvert.DeserializeObject<RequestRaw>(input);
                Process(raw);
            }

            Environment.Exit(0);

        }

        private static void Process(RequestRaw raw)
        {
            if (raw.ServiceName == typeof(LocateSourceFileService).Name)
            {
                LocateSourceFileServiceRq rq = ApiDeserializer.Deserialize<LocateSourceFileServiceRq>(raw.Request);
                LocateSourceFileServiceRs rs = (new LocateSourceFileService()).Run(rq);
                WriteResponse(JsonConvert.SerializeObject(rs));
            }
            else if (raw.ServiceName == typeof(LocateSourceFileByFileIdentifierService).Name)
            {
                LocateSourceFileByFileIdentifierServiceRq rq = ApiDeserializer.Deserialize<LocateSourceFileByFileIdentifierServiceRq>(raw.Request);
                LocateSourceFileByFileIdentifierServiceRs rs = (new LocateSourceFileByFileIdentifierService()).Run(rq);
                WriteResponse(JsonConvert.SerializeObject(rs));
            }
            else if (raw.ServiceName == typeof(OpenGitLogService).Name)
            {
                OpenGitLogServiceRq rq = ApiDeserializer.Deserialize<OpenGitLogServiceRq>(raw.Request);
                OpenGitLogServiceRs rs = (new OpenGitLogService()).Run(rq);
                WriteResponse(JsonConvert.SerializeObject(rs));
            }
            else
            {
                WriteResponse(JsonConvert.SerializeObject(new { Error = "Service " + raw.ServiceName + " is not implemented." }));
            }
        }

        
        private static string OpenStandardStreamIn()
        {
            log.Info("OpenStandardStreamIn BEGIN");
            //// We need to read first 4 bytes for length information
            Stream stdin = Console.OpenStandardInput();
            int length = 0;
            byte[] bytes = new byte[4];
            stdin.Read(bytes, 0, 4);
            length = System.BitConverter.ToInt32(bytes, 0);
            log.Info("OpenStandardStreamIn length = " + length);
            string input = "";
            for (int i = 0; i < length; i++)
            {
                input += (char)stdin.ReadByte();
            }

            log.Info("OpenStandardStreamIn END");
            return input;
        }

        public static void WriteResponse(JToken data)
        {
            var json = new JObject();
            json["data"] = data;

            var bytes = System.Text.Encoding.UTF8.GetBytes(json.ToString(Formatting.None));

            var stdout = Console.OpenStandardOutput();
            stdout.WriteByte((byte)((bytes.Length >> 0) & 0xFF));
            stdout.WriteByte((byte)((bytes.Length >> 8) & 0xFF));
            stdout.WriteByte((byte)((bytes.Length >> 16) & 0xFF));
            stdout.WriteByte((byte)((bytes.Length >> 24) & 0xFF));

            stdout.Write(bytes, 0, bytes.Length);

            stdout.Flush();
        }


        private static void OpenStandardStreamOut(string stringData, string[] args)
        {
            string text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff",
                                CultureInfo.InvariantCulture);
            String str = text + Environment.NewLine + "{\"length\": \"" + args.Count() + "\"}";
            str += " " + args[0] + Environment.NewLine + args[1] + Environment.NewLine;
            str += stringData;
            //String str = stringData;
            Stream stdout = Console.OpenStandardOutput();
            Stream stdin = Console.OpenStandardInput();

            stdout.WriteByte((byte)str.Length);
            stdout.WriteByte((byte)'\0');
            stdout.WriteByte((byte)'\0');
            stdout.WriteByte((byte)'\0');
            File.WriteAllText(@"C:\ChromeNativeMessaging_dump.txt", str + Environment.NewLine + stdin);
            Console.Write(str);
        }
    }
}
