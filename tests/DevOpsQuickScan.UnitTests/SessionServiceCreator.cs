using DevOpsQuickScan.Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace DevOpsQuickScan.UnitTests;

public static class SessionServiceCreator
{
   public static SessionService Create(ILogger<SessionService> logger, ISessionDataRepository sessionDataRepository = null)
   {
      if(sessionDataRepository == null)
         return new SessionService(new Mock<ISessionDataRepository>().Object, logger);

      return new SessionService(sessionDataRepository, logger);
   }
}