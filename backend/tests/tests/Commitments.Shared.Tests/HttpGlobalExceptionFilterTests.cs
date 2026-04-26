using Commitments.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Commitments.Shared.Tests;

public class HttpGlobalExceptionFilterTests
{
    [Fact]
    public void OnException_LogsError()
    {
        var mockLogger = new Mock<ILogger<HttpGlobalExceptionFilter>>();
        var filter = new HttpGlobalExceptionFilter(mockLogger.Object);

        var exception = new InvalidOperationException("Test error");
        var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
        var context = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = exception
        };

        filter.OnException(context);

        mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
