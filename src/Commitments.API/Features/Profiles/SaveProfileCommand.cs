using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.Profiles;

 public class SaveProfileCommandValidator: AbstractValidator<SaveProfileCommandRequest> {
     public SaveProfileCommandValidator()
     {
         RuleFor(request => request.Profile.ProfileId).NotNull();
     }
 }

 public class SaveProfileCommandRequest : IRequest<SaveProfileCommandResponse> {
     public ProfileDto Profile { get; set; }
 }

 public class SaveProfileCommandResponse
 {            
     public int ProfileId { get; set; }
 }

 public class SaveProfileCommandHandler : IRequestHandler<SaveProfileCommandRequest, SaveProfileCommandResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public SaveProfileCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<SaveProfileCommandResponse> Handle(SaveProfileCommandRequest request, CancellationToken cancellationToken)
     {
         var profile = await _context.Profiles.FindAsync(request.Profile.ProfileId);

         if (profile == null) _context.Profiles.Add(profile = new Profile());

         profile.Name = request.Profile.Name;


         await _context.SaveChangesAsync(cancellationToken);

         return new SaveProfileCommandResponse() { ProfileId = profile.ProfileId };
     }
 }
