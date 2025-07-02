using DevOpsQuickScan.Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace DevOpsQuickScan.UnitTests;

public static class SessionServiceCreator
{
   public static SessionService Create(
      ILogger<SessionService> logger, 
      IQuestionSender questionSender = null,
      IAnswersSender answersSender = null,
      ISessionDataRepository sessionDataRepository = null)
   {
      return new SessionService(
            questionSender ?? new Mock<IQuestionSender>().Object,
            answersSender ?? new Mock<IAnswersSender>().Object,
            sessionDataRepository ?? new Mock<ISessionDataRepository>().Object, logger);
   }
}