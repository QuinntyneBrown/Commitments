using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Commitments.Core.Extensions;


namespace Commitments.Api.Features.Users;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMediator _mediator;

    public UsersController(IHttpContextAccessor httpContextAccessor, IMediator mediator) {
        _httpContextAccessor = httpContextAccessor;
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("token")]
    public async Task<ActionResult<AuthenticateResponse>> SignIn(AuthenticateRequest request)
        => await _mediator.Send(request);

    [HttpPost("changePassword")]
    public async Task<ActionResult<ChangePasswordResponse>> ChangePassword(ChangePasswordRequest request) {
        request.ProfileId = _httpContextAccessor.GetProfileIdFromClaims();
        return await _mediator.Send(request);
    }
}
