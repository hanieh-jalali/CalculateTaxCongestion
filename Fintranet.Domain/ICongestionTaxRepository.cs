using System.Linq.Expressions;
using Fintranet.Domain.Models;

namespace Fintranet.Domain;

public interface ICongestionTaxRepository
{
    public Task<CongestionTaxTransaction> AddTransaction(CongestionTaxTransaction congestionTaxTransaction);
    public Task<IEnumerable<CongestionTaxTransaction>> GetTodayCongestionTaxesByPlate(string plateAreaCode, string regionletter, int ageIdentifier, DateTime entryDateTime);
    public Task<IEnumerable<CongestionTaxTransaction>> GetCongestionTax(string plateAreaCode, string regionletter, int ageIdentifier);
}

                                                          