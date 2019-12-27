using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;
using System;

namespace Commitments.Api.Features.DigitalAssets
{
    public class GetDigitalAssetByIdQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.DigitalAssetId).NotEqual(default(Guid));
            }
        }

        public class Request : IRequest<Response> {
            public Guid DigitalAssetId { get; set; }
        }

        public class Response
        {
            public DigitalAssetApiModel DigitalAsset { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    DigitalAsset = DigitalAssetApiModel.FromDigitalAsset(await _context.DigitalAssets.FindAsync(request.DigitalAssetId))
                };
        }
    }
}
