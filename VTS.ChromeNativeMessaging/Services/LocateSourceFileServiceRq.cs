namespace VTS.ChromeNativeMessaging.Services
{
    class LocateSourceFileServiceRq: RequestBase
    {
        public string BoaSolutionFolder { get; set; }
        public string CurrentRouteName { get; set; }
        public string UrlPathName { get; set; }
    }
}