// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Core.AggregateModel.TagAggregate.Commands;

public class RemoveTagCommandValidator : AbstractValidator<RemoveTagRequest>
{
    public RemoveTagCommandValidator()
    {
        RuleFor(request => request.TagId).NotEqual(default(Guid));
    }
}
public class RemoveTagRequest : IRequest<RemoveTagResponse>
{
    public Guid TagId { get; set; }
}

public class RemoveTagResponse { }

public class RemoveTagCommandHandler : IRequestHandler<RemoveTagRequest, RemoveTagResponse>
{
    private readonly ICommimentsDbContext _context;

    public RemoveTagCommandHandler(ICommimentsDbContext context) => _context = context;

    public async Task<RemoveTagResponse> Handle(RemoveTagRequest request, CancellationToken cancellationToken)
    {
        _context.Tags.Remove(await _context.Tags.FindAsync(request.TagId));
        await _context.SaveChangesAsync(cancellationToken);
        return new RemoveTagResponse() { };
    }
}

