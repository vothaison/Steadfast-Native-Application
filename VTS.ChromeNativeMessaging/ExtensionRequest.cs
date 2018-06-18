namespace VTS.ChromeNativeMessaging
{
    class ExtensionRequest
    {
        public string BranchName { get; set; }
        public string Request { get; set; }

        public string FilePath { get; set; }
        public int? LineNumber { get; set; }

    }
}