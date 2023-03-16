using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.DigitalAssets;

 public class RemoveDigitalAssetCommandValidator : AbstractValidator<RemoveDigitalAssetCommandRequest>
 {
     public RemoveDigitalAssetCommandValidator()
     {
         RuleFor(request => request.DigitalAssetId).NotEqual(0);
     }
 }

 public class RemoveDigitalAssetCommandRequest : IRequest
 {
     public int DigitalAssetId { get; set; }
 }

 public class RemoveDigitalAssetCommandHandler : IRequestHandler<RemoveDigitalAssetCommandRequest>
 {
     public IAppDbContext _context { get; set; }

     public RemoveDigitalAssetCommandHandler(IAppDbContext context) => _context = context;

     public async Task Handle(RemoveDigitalAssetCommandRequest request, CancellationToken cancellationToken)
     {
         _context.DigitalAssets.Remove(await _context.DigitalAssets.FindAsync(request.DigitalAssetId));
         await _context.SaveChangesAsync(cancellationToken);
     }

 }
