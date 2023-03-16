using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;
using System;


namespace Commitments.Api.Features.DigitalAssets;

 public class GetDigitalAssetByIdQueryCommandValidator : AbstractValidator<GetDigitalAssetByIdQueryCommandRequest>
 {
     public GetDigitalAssetByIdQueryCommandValidator()
     {
         RuleFor(request => request.DigitalAssetId).NotEqual(default(Guid));
     }
 }

 public class GetDigitalAssetByIdQueryCommandRequest : IRequest<GetDigitalAssetByIdQueryCommandResponse> {
     public Guid DigitalAssetId { get; set; }
 }

 public class GetDigitalAssetByIdQueryCommandResponse
 {
     public DigitalAssetApiModel DigitalAsset { get; set; }
 }

 public class GetDigitalAssetByIdQueryCommandHandler : IRequestHandler<GetDigitalAssetByIdQueryCommandRequest, GetDigitalAssetByIdQueryCommandResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetDigitalAssetByIdQueryCommandHandler(IAppDbContext context) => _context = context;

     public async Task<GetDigitalAssetByIdQueryCommandResponse> Handle(GetDigitalAssetByIdQueryCommandRequest request, CancellationToken cancellationToken)
         => new GetDigitalAssetByIdQueryCommandResponse()
         {
             DigitalAsset = DigitalAssetApiModel.FromDigitalAsset(await _context.DigitalAssets.FindAsync(request.DigitalAssetId))
         };
 }
