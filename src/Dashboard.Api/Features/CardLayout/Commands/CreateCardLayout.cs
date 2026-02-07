using Commitments.Shared;
using Dashboard.Core;
using CardLayoutModel = Dashboard.Core.Model.CardLayoutAggregate.CardLayout;
using MediatR;

namespace Dashboard.Api.Features.CardLayout;

public class CreateCardLayoutRequest : IRequest<CreateCardLayoutResponse>
{
    public CardLayoutDto CardLayout { get; set; }
}

public class CreateCardLayoutResponse : ResponseBase
{
    public CardLayoutDto CardLayout { get; set; }
}

public class CreateCardLayoutRequestHandler : IRequestHandler<CreateCardLayoutRequest, CreateCardLayoutResponse>
{
    private readonly IDashboardDbContext _context;

    public CreateCardLayoutRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<CreateCardLayoutResponse> Handle(CreateCardLayoutRequest request, CancellationToken cancellationToken)
    {
        var cardLayout = new CardLayoutModel(
            request.CardLayout.Name,
            request.CardLayout.Description);

        _context.CardLayouts.Add(cardLayout);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateCardLayoutResponse { CardLayout = cardLayout.ToDto() };
    }
}
