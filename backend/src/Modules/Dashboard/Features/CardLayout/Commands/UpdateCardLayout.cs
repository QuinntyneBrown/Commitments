using Commitments.Shared;
using Dashboard.Data;
using MediatR;

namespace Dashboard.Features.CardLayout;

public class UpdateCardLayoutRequest : IRequest<UpdateCardLayoutResponse>
{
    public CardLayoutDto CardLayout { get; set; }
}

public class UpdateCardLayoutResponse : ResponseBase
{
    public CardLayoutDto CardLayout { get; set; }
}

public class UpdateCardLayoutRequestHandler : IRequestHandler<UpdateCardLayoutRequest, UpdateCardLayoutResponse>
{
    private readonly IDashboardDbContext _context;

    public UpdateCardLayoutRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<UpdateCardLayoutResponse> Handle(UpdateCardLayoutRequest request, CancellationToken cancellationToken)
    {
        var cardLayout = await _context.CardLayouts.FindAsync(request.CardLayout.CardLayoutId);
        if (cardLayout == null) return new UpdateCardLayoutResponse();

        cardLayout.Name = request.CardLayout.Name;
        cardLayout.Description = request.CardLayout.Description;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateCardLayoutResponse { CardLayout = cardLayout.ToDto() };
    }
}
