// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;


namespace Commitments.Api.Controllers;

[Route("")]
[ApiExplorerSettings(IgnoreApi = true)]
[ApiController]
public class HomeController
{
    [HttpGet("health")]
    public IActionResult Health()
        => new OkObjectResult(new
        {
            Status = "Healthy"
        });

    [HttpGet]
    public IActionResult Index()
        => new RedirectResult("~/swagger");
}

