using System;
using System.Threading.Tasks;

namespace VTS.ChromeNativeMessaging
{
    public static class TaskHelpers
    {
        public static void Delay(TimeSpan timespan)
        {
            Task t = Task.Run<bool>(async delegate
            {
                await Task.Delay(timespan);
                return true;
            });
            t.Wait();
        }
    }
}