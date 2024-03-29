// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;


namespace Commitments.Core.AggregateModel.CommitmentAggregate.Queries;

public class GetCommitmentByIdValidator : AbstractValidator<GetCommitmentByIdRequest>
{
    public GetCommitmentByIdValidator()
    {
        RuleFor(request => request.CommitmentId).NotEqual(default(Guid));
    }
}

public class GetCommitmentByIdRequest : IRequest<GetCommitmentByIdResponse>
{
    public Guid CommitmentId { get; set; }
}

public class GetCommitmentByIdResponse
{
    public CommitmentDto Commitment { get; set; }
}

public class GetCommitmentByIdHandler : IRequestHandler<GetCommitmentByIdRequest, GetCommitmentByIdResponse>
{
    public ICommitmentsDbContext _context { get; set; }

    public GetCommitmentByIdHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetCommitmentByIdResponse> Handle(GetCommitmentByIdRequest request, CancellationToken cancellationToken)
        => new GetCommitmentByIdResponse()
        {
            Commitment = CommitmentDto.FromCommitment(await _context.Commitments.FindAsync(request.CommitmentId))
        };
}