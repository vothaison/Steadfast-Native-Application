using Newtonsoft.Json;

namespace VTS.ChromeNativeMessaging.Support
{
    public static class ApiDeserializer
    {
        public static T Deserialize<T>(object o)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(o));
        }
    }
}