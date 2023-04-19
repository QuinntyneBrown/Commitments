// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.CardAggregate.Commands;

public class CreateCardRequestValidator : AbstractValidator<CreateCardRequest>
{
    public CreateCardRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().NotNull();
        RuleFor(x => x.Description).NotEmpty().NotNull();
    }
}

public class CreateCardRequest : IRequest<CreateCardResponse>
{
    public Guid CardId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}


public class CreateCardResponse : ResponseBase
{
    public CardDto Card { get; set; }
}

public class CreateCardRequestHandler : IRequestHandler<CreateCardRequest, CreateCardResponse>
{
    private readonly ILogger<CreateCardRequestHandler> _logger;

    private readonly IDashboardServiceDbContext _context;

    public CreateCardRequestHandler(ILogger<CreateCardRequestHandler> logger, IDashboardServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<CreateCardResponse> Handle(CreateCardRequest request, CancellationToken cancellationToken)
    {
        var card = new Card();
        _context.Cards.Add(card);
        card.Name = request.Name;
        card.Name = request.Description;
        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            Card = card.ToDto()
        };
    }
}