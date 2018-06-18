using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTS.ChromeNativeMessaging.Visual;

namespace VTS.ChromeNativeMessaging.Services
{
    class LocateSourceFileService: ServiceBase<LocateSourceFileServiceRq, LocateSourceFileServiceRs>
    {
        protected override LocateSourceFileServiceRs DoRun(LocateSourceFileServiceRq rq)
        {
            VisualStudioActions.LocateSourceFile(rq.BoaSolutionFolder, currentRouteName: rq.CurrentRouteName, locationPathName: rq.UrlPathName, lineNumber: 180);

            return new LocateSourceFileServiceRs()
            {
                Status = "OK",
                Href = ""
            };
        }
    }
}
