using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Profiles;

 public class GetProfileByIdQueryCommandValidator : AbstractValidator<GetProfileByIdQueryCommandRequest>
 {
     public GetProfileByIdQueryCommandValidator()
     {
         RuleFor(request => request.ProfileId).NotEqual(0);
     }
 }

 public class GetProfileByIdQueryCommandRequest : IRequest<GetProfileByIdQueryCommandResponse> {
     public int ProfileId { get; set; }
 }

 public class GetProfileByIdQueryCommandResponse
 {
     public ProfileApiModel Profile { get; set; }
 }

 public class GetProfileByIdQueryCommandHandler : IRequestHandler<GetProfileByIdQueryCommandRequest, GetProfileByIdQueryCommandResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetProfileByIdQueryCommandHandler(IAppDbContext context) => _context = context;

     public async Task<GetProfileByIdQueryCommandResponse> Handle(GetProfileByIdQueryCommandRequest request, CancellationToken cancellationToken)
         => new GetProfileByIdQueryCommandResponse()
         {
             Profile = ProfileApiModel.FromProfile(await _context.Profiles.FindAsync(request.ProfileId))
         };
 }
