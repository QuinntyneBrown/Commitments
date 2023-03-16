using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;
using System;


namespace Commitments.Api.Features.DigitalAssets;

 public class SaveDigitalAssetCommandValidator: AbstractValidator<SaveDigitalAssetCommandRequest> {
     public SaveDigitalAssetCommandValidator()
     {
         RuleFor(request => request.DigitalAsset.DigitalAssetId).NotNull();
     }
 }

 public class SaveDigitalAssetCommandRequest : IRequest<SaveDigitalAssetCommandResponse> {
     public DigitalAssetApiModel DigitalAsset { get; set; }
 }

 public class SaveDigitalAssetCommandResponse
 {            
     public Guid DigitalAssetId { get; set; }
 }

 public class SaveDigitalAssetCommandHandler : IRequestHandler<SaveDigitalAssetCommandRequest, SaveDigitalAssetCommandResponse>
 {
     public IAppDbContext _context { get; set; }

     public SaveDigitalAssetCommandHandler(IAppDbContext context) => _context = context;

     public async Task<SaveDigitalAssetCommandResponse> Handle(SaveDigitalAssetCommandRequest request, CancellationToken cancellationToken)
     {
         var digitalAsset = await _context.DigitalAssets.FindAsync(request.DigitalAsset.DigitalAssetId);

         if (digitalAsset == null) _context.DigitalAssets.Add(digitalAsset = new DigitalAsset());

         digitalAsset.Name = request.DigitalAsset.Name;

         await _context.SaveChangesAsync(cancellationToken);

         return new SaveDigitalAssetCommandResponse() { DigitalAssetId = digitalAsset.DigitalAssetId };
     }
 }
