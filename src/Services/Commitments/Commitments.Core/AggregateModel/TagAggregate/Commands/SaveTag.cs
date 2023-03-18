// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using Commitments.Core.Interfaces;
using Commitments.Core.AggregateModel;
using Commitments.Core.Extensions;


namespace Commitments.Core.AggregateModel.TagAggregate.Commands;

public class SaveTagCommandValidator : AbstractValidator<SaveTagRequest>
{
    public SaveTagCommandValidator()
    {
        RuleFor(request => request.Tag.TagId).NotNull();
    }
}

public class SaveTagRequest : IRequest<SaveTagResponse>
{
    public TagDto Tag { get; set; }
}

public class SaveTagResponse
{
    public Guid TagId { get; set; }
}

public class SaveTagCommandHandler : IRequestHandler<SaveTagRequest, SaveTagResponse>
{
    private readonly ICommimentsDbContext _context;

    public SaveTagCommandHandler(ICommimentsDbContext context) => _context = context;

    public async Task<SaveTagResponse> Handle(SaveTagRequest request, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags.FindAsync(request.Tag.TagId);

        if (tag == null) _context.Tags.Add(tag = new Tag());

        tag.Name = request.Tag.Name;

        tag.Slug = request.Tag.Name.GenerateSlug();

        await _context.SaveChangesAsync(cancellationToken);

        return new SaveTagResponse() { TagId = tag.TagId };
    }
}

