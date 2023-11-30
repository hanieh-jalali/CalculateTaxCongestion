using MediatR;
using Fintranet.Domain;
using Fintranet.Domain.Models;

namespace Fintranet.Application.Service.Command;

public class CreateCongestionTaxCommandHandler :
    IRequestHandler<CreateCongestionTaxCommand, CongestionTaxTransaction>
{
    private readonly ICongestionTaxRepository _congestionTaxRepository;
    private readonly CityTaxRule _taxRule;

    public CreateCongestionTaxCommandHandler(ICongestionTaxRepository congestionTaxRepository, CityTaxRule taxRule)
    {
        _congestionTaxRepository = congestionTaxRepository;
        _taxRule = taxRule;
    }
    public Task<decimal> CalculateTaxCongestionAmount(CreateCongestionTaxCommand command)
    {
        if (_taxRule.ExemptVehicleTypes!.Contains(command.VehicleType)) return Task.FromResult(0M);
        if ((int)_taxRule.ExemptMonth!.Value == command.EntryTime.Month) return Task.FromResult(0M);
        if (_taxRule.ExemptDaysOfWeek!.Where(w => (int)w == (int)command.EntryTime.DayOfWeek).Count() > 0) return Task.FromResult(0M);
        if (_taxRule.ExemptDates!.FirstOrDefault(w => w == command.EntryTime.Date) != new DateTime()) return Task.FromResult(0M);

        var entryTimeOnly = TimeOnly.FromDateTime(command.EntryTime);
        var taxRange = _taxRule.TaxTimeRanges.FirstOrDefault(w => w.StartTime <= entryTimeOnly && w.EndTime >= entryTimeOnly);
        var crossAmount = 0M;
        if (taxRange is not null)
        {
            crossAmount = taxRange.Amount;
        }
        else 
        {
            var passedDateTimeRange = _taxRule.TaxTimeRanges.FirstOrDefault(w => w.StartTime > w.EndTime);
            if (passedDateTimeRange is null) return Task.FromResult(0M);

            var lastStartTimeRange = new DateTime(command.EntryTime.Year
                                , command.EntryTime.Month
                                , command.EntryTime.Day
                                , passedDateTimeRange.StartTime.Hour
                                , passedDateTimeRange.StartTime.Minute
                                , 0);
            var lastEndTimeRange = new DateTime(command.EntryTime.AddDays(1).Year
                                , command.EntryTime.AddDays(1).Month
                                , command.EntryTime.AddDays(1).Day
                                , passedDateTimeRange.EndTime.Hour
                                , passedDateTimeRange.EndTime.Minute
                                , 0);

            var foundTimeRange = lastStartTimeRange <= command.EntryTime && lastEndTimeRange >= command.EntryTime;
            if (foundTimeRange)
                crossAmount = passedDateTimeRange.Amount;
        }

        if (crossAmount == 0) return Task.FromResult(0M);

        var todayCongestionTaxesByPlate = _congestionTaxRepository
            .GetTodayCongestionTaxesByPlate(command.PlateAreaCode, command.Regionletter, command.AgeIdentifier,command.EntryTime).Result.ToList();

        if (todayCongestionTaxesByPlate is not null &&  todayCongestionTaxesByPlate.Count > 0)
        {
            var dailyAggregateTax = todayCongestionTaxesByPlate.Sum(s => s.CongestionTaxAmount);
            if (dailyAggregateTax == 60) return Task.FromResult(0M);

            var lastCongestionTax = todayCongestionTaxesByPlate.LastOrDefault();
            var diffrentiateToTalHours = command.EntryTime.Subtract(lastCongestionTax!.EntryTime).TotalHours;
            if (diffrentiateToTalHours < 1)
            {
                if (crossAmount >= lastCongestionTax.CongestionTaxAmount)
                    crossAmount = crossAmount - lastCongestionTax.CongestionTaxAmount;
                else return Task.FromResult(0M);
            }

            if (crossAmount + dailyAggregateTax > 60)
                crossAmount = 60 - dailyAggregateTax;
        }

        return Task.FromResult(crossAmount);
    }

    public Task<CongestionTaxTransaction> Handle(CreateCongestionTaxCommand request, CancellationToken cancellationToken)
    {
        var newCongestionTaxTransaction = new CongestionTaxTransaction
        {
            EntryTime = request.EntryTime,
            AgeIdentifier = request.AgeIdentifier,
            Regionletter = request.Regionletter,
            CityName = _taxRule.CityName,
            CongestionTaxAmount = CalculateTaxCongestionAmount(request).Result,
            PlateAreaCode = request.PlateAreaCode,
            VehicleType = request.VehicleType,
        };

        if (newCongestionTaxTransaction.CongestionTaxAmount > 0)
        {
            var addedTransaction = _congestionTaxRepository.AddTransaction(newCongestionTaxTransaction).Result;
            return Task.FromResult(addedTransaction);
        }

        return Task.FromResult(newCongestionTaxTransaction);
    }
}