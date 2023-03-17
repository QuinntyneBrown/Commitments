using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;
using System;


namespace Commitments.Api.Features.DigitalAssets;

 public class GetDigitalAssetByIdValidator : AbstractValidator<GetDigitalAssetByIdRequest>
 {
     public GetDigitalAssetByIdValidator()
     {
         RuleFor(request => request.DigitalAssetId).NotEqual(default(Guid));
     }
 }

 public class GetDigitalAssetByIdRequest : IRequest<GetDigitalAssetByIdResponse> {
     public Guid DigitalAssetId { get; set; }
 }

 public class GetDigitalAssetByIdResponse
 {
     public DigitalAssetDto DigitalAsset { get; set; }
 }

 public class GetDigitalAssetByIdHandler : IRequestHandler<GetDigitalAssetByIdRequest, GetDigitalAssetByIdResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetDigitalAssetByIdHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetDigitalAssetByIdResponse> Handle(GetDigitalAssetByIdRequest request, CancellationToken cancellationToken)
         => new GetDigitalAssetByIdResponse()
         {
             DigitalAsset = DigitalAssetDto.FromDigitalAsset(await _context.DigitalAssets.FindAsync(request.DigitalAssetId))
         };
 }
