// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using NoteService.Core.AggregateModel.NoteAggregate;

namespace NoteService.Core.AggregateModel.TagAggregate.Commands;

public class CreateTagRequestValidator : AbstractValidator<CreateTagRequest>
{
    public CreateTagRequestValidator()
    {

        RuleFor(x => x.Name).NotEqual(default(Guid));
        RuleFor(x => x.Slug).NotNull();
        RuleFor(x => x.Notes).NotNull();

    }

}


public class CreateTagRequest : IRequest<CreateTagResponse>
{
    public Guid Name { get; set; }
    public string Slug { get; set; }
    public List<Note> Notes { get; set; }
}


public class CreateTagResponse
{
    public required TagDto Tag { get; set; }
}


public class CreateTagRequestHandler : IRequestHandler<CreateTagRequest, CreateTagResponse>
{
    private readonly INoteServiceDbContext _context;

    private readonly ILogger<CreateTagRequestHandler> _logger;

    public CreateTagRequestHandler(ILogger<CreateTagRequestHandler> logger, INoteServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<CreateTagResponse> Handle(CreateTagRequest request, CancellationToken cancellationToken)
    {
        var tag = new Tag();

        _context.Tags.Add(tag);

        tag.Name = request.Name;
        tag.Slug = request.Slug;
        tag.Notes = request.Notes;

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            Tag = tag.ToDto()
        };

    }

}