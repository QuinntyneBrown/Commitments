// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Core.AggregateModel.TagAggregate.Queries;

public class GetTagByIdValidator : AbstractValidator<GetTagByIdRequest>
{
    public GetTagByIdValidator()
    {
        RuleFor(request => request.TagId).NotEqual(default(Guid));
    }
}

public class GetTagByIdRequest : IRequest<GetTagByIdResponse>
{
    public Guid TagId { get; set; }
}

public class GetTagByIdResponse
{
    public TagDto Tag { get; set; }
}

public class GetTagByIdHandler : IRequestHandler<GetTagByIdRequest, GetTagByIdResponse>
{
    private readonly ICommimentsDbContext _context;

    public GetTagByIdHandler(ICommimentsDbContext context) => _context = context;

    public async Task<GetTagByIdResponse> Handle(GetTagByIdRequest request, CancellationToken cancellationToken)
        => new GetTagByIdResponse()
        {
            Tag = TagDto.FromTag(await _context.Tags.FindAsync(request.TagId))
        };
}

