// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;
using System;


namespace Commitments.Api.Features.DigitalAssets;

 public class SaveDigitalAssetCommandValidator: AbstractValidator<SaveDigitalAssetRequest> {
     public SaveDigitalAssetCommandValidator()
     {
         RuleFor(request => request.DigitalAsset.DigitalAssetId).NotNull();
     }
 }

 public class SaveDigitalAssetRequest : IRequest<SaveDigitalAssetResponse> {
     public DigitalAssetDto DigitalAsset { get; set; }
 }

 public class SaveDigitalAssetResponse
 {            
     public Guid DigitalAssetId { get; set; }
 }

 public class SaveDigitalAssetCommandHandler : IRequestHandler<SaveDigitalAssetRequest, SaveDigitalAssetResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public SaveDigitalAssetCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<SaveDigitalAssetResponse> Handle(SaveDigitalAssetRequest request, CancellationToken cancellationToken)
     {
         var digitalAsset = await _context.DigitalAssets.FindAsync(request.DigitalAsset.DigitalAssetId);

         if (digitalAsset == null) _context.DigitalAssets.Add(digitalAsset = new DigitalAsset());

         digitalAsset.Name = request.DigitalAsset.Name;

         await _context.SaveChangesAsync(cancellationToken);

         return new SaveDigitalAssetResponse() { DigitalAssetId = digitalAsset.DigitalAssetId };
     }
 }

