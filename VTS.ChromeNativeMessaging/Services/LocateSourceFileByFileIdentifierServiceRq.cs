namespace VTS.ChromeNativeMessaging.Services
{
    class LocateSourceFileByFileIdentifierServiceRq : RequestBase
    {
        public string FileIdentifier { get; set; }
        public string BoaSolutionFolder { get; set; }
        public int LineNumber { get; set; }
    }
}