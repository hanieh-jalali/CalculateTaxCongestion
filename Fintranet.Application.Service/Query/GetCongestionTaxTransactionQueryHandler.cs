using MediatR;
using Fintranet.Domain;
using Fintranet.Domain.Models;

namespace Fintranet.Application.Core.Query;

public class GetCongestionTaxTransactionQueryHandler :
    IRequestHandler<GetCongestionTaxTransactionQuery, IEnumerable<CongestionTaxTransaction>>
{

    private readonly ICongestionTaxRepository _congestionTaxRepository;

    public GetCongestionTaxTransactionQueryHandler(ICongestionTaxRepository congestionTaxRepository)
    {
        _congestionTaxRepository = congestionTaxRepository;
    }

    public async Task<IEnumerable<CongestionTaxTransaction>> Handle(GetCongestionTaxTransactionQuery request, CancellationToken cancellationToken)
    {
       var congestiontaxTransactions = await _congestionTaxRepository.GetCongestionTax(request.PlateAreaCode, request.Regionletter,request.AgeIdentifier);
        return congestiontaxTransactions;
    }
}