using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Commitments.API.Features.Achievements
{
    [Authorize]
    [ApiController]
    [Route("api/achievements")]
    public class AchievementsController
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AchievementsController(IHttpContextAccessor httpContextAccessor, IMediator mediator) {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<GetAchievementsQuery.Response>> Get() {
            var profileClaim = _httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type =="ProfileId");
            var profileId = Convert.ToInt16(profileClaim.Value);
            return await _mediator.Send(new GetAchievementsQuery.Request() {
                ProfileId = profileId
            });
        }
    }
}
