using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;
using System;


namespace Commitments.Api.Features.DigitalAssets;

 public class GetDigitalAssetByIdQueryCommandValidator : AbstractValidator<GetDigitalAssetByIdQueryRequest>
 {
     public GetDigitalAssetByIdQueryCommandValidator()
     {
         RuleFor(request => request.DigitalAssetId).NotEqual(default(Guid));
     }
 }

 public class GetDigitalAssetByIdQueryRequest : IRequest<GetDigitalAssetByIdQueryResponse> {
     public Guid DigitalAssetId { get; set; }
 }

 public class GetDigitalAssetByIdQueryResponse
 {
     public DigitalAssetDto DigitalAsset { get; set; }
 }

 public class GetDigitalAssetByIdQueryCommandHandler : IRequestHandler<GetDigitalAssetByIdQueryRequest, GetDigitalAssetByIdQueryResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetDigitalAssetByIdQueryCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetDigitalAssetByIdQueryResponse> Handle(GetDigitalAssetByIdQueryRequest request, CancellationToken cancellationToken)
         => new GetDigitalAssetByIdQueryResponse()
         {
             DigitalAsset = DigitalAssetDto.FromDigitalAsset(await _context.DigitalAssets.FindAsync(request.DigitalAssetId))
         };
 }
