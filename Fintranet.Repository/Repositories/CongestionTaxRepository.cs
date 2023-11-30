using Repository;
using System.Linq;
using Fintranet.Domain;
using Fintranet.Domain.Models;

namespace Fintranet.Repository;

public class CongestionTaxRepository : ICongestionTaxRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    public CongestionTaxRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    public async Task<CongestionTaxTransaction> AddTransaction(CongestionTaxTransaction congestionTaxTransaction)
    {
        try
        {
            await _applicationDbContext.AddAsync(congestionTaxTransaction);
            _applicationDbContext.SaveChanges();

            return congestionTaxTransaction;
        }
        catch (Exception)
        {

            throw;
        }

    }

    public Task<IEnumerable<CongestionTaxTransaction>> GetTodayCongestionTaxesByPlate(string plateAreaCode, string regionletter, int ageIdentifier, DateTime entryDateTime)
    {

            if (_applicationDbContext is not null)
            {
                var congestionsTax = _applicationDbContext.CongestionTaxTransactions
                    .Where(w =>
                            w.PlateAreaCode == plateAreaCode 
                            && w.Regionletter == regionletter 
                            && w.AgeIdentifier == ageIdentifier 
                            && w.EntryTime.Date == entryDateTime.Date)
                    .OrderBy(o => o.Id)
                    .AsEnumerable();

                return Task.FromResult(congestionsTax);
            }
           return Task.FromResult<IEnumerable<CongestionTaxTransaction>>(null);
    }

    public Task<IEnumerable<CongestionTaxTransaction>> GetCongestionTax(string plateAreaCode, string regionletter, int ageIdentifier)
    {
        try
        {
            var congestionsTax = _applicationDbContext.CongestionTaxTransactions
                .Where(w => w.PlateAreaCode == plateAreaCode && w.Regionletter == regionletter && w.AgeIdentifier == ageIdentifier )
                .OrderByDescending(o => o.Id)
                .AsEnumerable();

            return Task.FromResult(congestionsTax);
        }
        catch (Exception)
        {
            return Task.FromResult<IEnumerable<CongestionTaxTransaction>>(null!);

            throw;
        } 
    }

}