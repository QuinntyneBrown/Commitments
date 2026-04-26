using Commitments.Shared;
using Dashboard.Data;
using MediatR;

namespace Dashboard.Features.CardLayout;

public class GetCardLayoutByIdRequest : IRequest<GetCardLayoutByIdResponse>
{
    public Guid CardLayoutId { get; set; }
}

public class GetCardLayoutByIdResponse : ResponseBase
{
    public CardLayoutDto? CardLayout { get; set; }
}

public class GetCardLayoutByIdRequestHandler : IRequestHandler<GetCardLayoutByIdRequest, GetCardLayoutByIdResponse>
{
    private readonly IDashboardDbContext _context;

    public GetCardLayoutByIdRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<GetCardLayoutByIdResponse> Handle(GetCardLayoutByIdRequest request, CancellationToken cancellationToken)
    {
        var cardLayout = await _context.CardLayouts.FindAsync(request.CardLayoutId);
        return new GetCardLayoutByIdResponse { CardLayout = cardLayout?.ToDto() };
    }
}
