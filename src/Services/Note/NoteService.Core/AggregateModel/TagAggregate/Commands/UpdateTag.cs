// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using NoteService.Core.AggregateModel.NoteAggregate;

namespace NoteService.Core.AggregateModel.TagAggregate.Commands;

public class UpdateTagRequestValidator : AbstractValidator<UpdateTagRequest>
{
    public UpdateTagRequestValidator()
    {

        RuleFor(x => x.TagId).NotEqual(default(Guid));
        RuleFor(x => x.Name).NotEqual(default(Guid));
        RuleFor(x => x.Slug).NotNull();
        RuleFor(x => x.Notes).NotNull();

    }

}


public class UpdateTagRequest : IRequest<UpdateTagResponse>
{
    public Guid TagId { get; set; }
    public Guid Name { get; set; }
    public string Slug { get; set; }
    public List<NoteDto> Notes { get; set; }
}


public class UpdateTagResponse
{
    public required TagDto Tag { get; set; }
}


public class UpdateTagRequestHandler : IRequestHandler<UpdateTagRequest, UpdateTagResponse>
{
    private readonly INoteServiceDbContext _context;

    private readonly ILogger<UpdateTagRequestHandler> _logger;

    public UpdateTagRequestHandler(ILogger<UpdateTagRequestHandler> logger, INoteServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<UpdateTagResponse> Handle(UpdateTagRequest request, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags.SingleAsync(x => x.TagId == request.TagId);

        tag.TagId = request.TagId;
        tag.Name = request.Name;
        tag.Slug = request.Slug;

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            Tag = tag.ToDto()
        };

    }

}



