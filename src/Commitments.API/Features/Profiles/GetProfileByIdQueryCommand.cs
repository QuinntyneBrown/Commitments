using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Profiles;

 public class GetProfileByIdQueryCommandValidator : AbstractValidator<GetProfileByIdQueryRequest>
 {
     public GetProfileByIdQueryCommandValidator()
     {
         RuleFor(request => request.ProfileId).NotEqual(0);
     }
 }

 public class GetProfileByIdQueryRequest : IRequest<GetProfileByIdQueryResponse> {
     public int ProfileId { get; set; }
 }

 public class GetProfileByIdQueryResponse
 {
     public ProfileDto Profile { get; set; }
 }

 public class GetProfileByIdQueryCommandHandler : IRequestHandler<GetProfileByIdQueryRequest, GetProfileByIdQueryResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetProfileByIdQueryCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetProfileByIdQueryResponse> Handle(GetProfileByIdQueryRequest request, CancellationToken cancellationToken)
         => new GetProfileByIdQueryResponse()
         {
             Profile = ProfileDto.FromProfile(await _context.Profiles.FindAsync(request.ProfileId))
         };
 }
