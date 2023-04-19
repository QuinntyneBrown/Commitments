// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace NoteService.Core.AggregateModel.TagAggregate.Commands;

public class DeleteTagRequestValidator : AbstractValidator<DeleteTagRequest>
{
    public DeleteTagRequestValidator()
    {

        RuleFor(x => x.TagId).NotEqual(default(Guid));

    }

}


public class DeleteTagRequest : IRequest<DeleteTagResponse>
{
    public Guid TagId { get; set; }
}


public class DeleteTagResponse
{
    public required TagDto Tag { get; set; }
}


public class DeleteTagRequestHandler : IRequestHandler<DeleteTagRequest, DeleteTagResponse>
{
    private readonly INoteServiceDbContext _context;

    private readonly ILogger<DeleteTagRequestHandler> _logger;

    public DeleteTagRequestHandler(ILogger<DeleteTagRequestHandler> logger, INoteServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<DeleteTagResponse> Handle(DeleteTagRequest request, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags.FindAsync(request.TagId);

        _context.Tags.Remove(tag);

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            Tag = tag.ToDto()
        };
    }

}