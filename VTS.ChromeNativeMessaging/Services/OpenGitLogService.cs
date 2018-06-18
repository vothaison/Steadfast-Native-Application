using VTS.ChromeNativeMessaging.Visual;

namespace VTS.ChromeNativeMessaging.Services
{
    class OpenGitLogService : ServiceBase<OpenGitLogServiceRq, OpenGitLogServiceRs>
    {
        protected override OpenGitLogServiceRs DoRun(OpenGitLogServiceRq rq)
        {
            GitActions.OpenGitLog(rq.BoaSolutionFolder, rq.FileIdentifier, lineNumber: rq.LineNumber);

            return new OpenGitLogServiceRs()
            {
                Status = "OK",
            };
        }
    }
}