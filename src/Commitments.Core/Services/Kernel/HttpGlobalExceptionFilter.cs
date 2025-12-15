// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Commitments.Core.Services.Kernel;

public class HttpGlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<HttpGlobalExceptionFilter> _logger;

    public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Unhandled exception occurred.");
    }
}
