using VTS.ChromeNativeMessaging.Visual;

namespace VTS.ChromeNativeMessaging.Services
{
    class LocateSourceFileByFileIdentifierService : ServiceBase<LocateSourceFileByFileIdentifierServiceRq, LocateSourceFileByFileIdentifierServiceRs>
    {
        protected override LocateSourceFileByFileIdentifierServiceRs DoRun(LocateSourceFileByFileIdentifierServiceRq rq)
        {
            VisualStudioActions.LocateSourceFileByFileIdentifier(rq.BoaSolutionFolder, rq.FileIdentifier, lineNumber: rq.LineNumber);

            return new LocateSourceFileByFileIdentifierServiceRs()
            {
                Status = "OK",
            };
        }
    }
}