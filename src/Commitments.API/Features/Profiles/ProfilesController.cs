using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Commitments.API.Features.Profiles
{
    [Authorize]
    [ApiController]
    [Route("api/profiles")]
    public class ProfilesController
    {
        private readonly IMediator _mediator;

        public ProfilesController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveProfileCommand.Response>> Save(SaveProfileCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{Profile.ProfileId}")]
        public async Task Remove(RemoveProfileCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{ProfileId}")]
        public async Task<ActionResult<GetProfileByIdQuery.Response>> GetById([FromRoute]GetProfileByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetProfilesQuery.Response>> Get()
            => await _mediator.Send(new GetProfilesQuery.Request());
    }
}
