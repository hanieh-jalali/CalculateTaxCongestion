using MediatR;
using Fintranet.Domain.Models;

namespace Fintranet.Application.Core.Query;

public class GetCongestionTaxTransactionQuery : IRequest<IEnumerable<CongestionTaxTransaction>>
{
    public string PlateAreaCode { get; set; }
    public string Regionletter { get; set; }
    public int AgeIdentifier { get; set; }
}