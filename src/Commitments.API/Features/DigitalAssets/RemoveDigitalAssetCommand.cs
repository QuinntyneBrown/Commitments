using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
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
     public ICommimentsDbContext _context { get; set; }

     public RemoveDigitalAssetCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task Handle(RemoveDigitalAssetCommandRequest request, CancellationToken cancellationToken)
     {
         _context.DigitalAssets.Remove(await _context.DigitalAssets.FindAsync(request.DigitalAssetId));
         await _context.SaveChangesAsync(cancellationToken);
     }

 }
