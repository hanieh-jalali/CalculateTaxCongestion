using System.ComponentModel.DataAnnotations;
using Fintranet.Domain.Enums;

namespace Fintranet.Domain.Models;
public class CityTaxRule
{
    public CityTaxRule() 
    {
        ExemptVehicleTypes = new();
        ExemptDaysOfWeek = new();
        TaxTimeRanges = new();
        ExemptDates = new();
        ExemptMonth = Enums.ExemptMonth.NotSet;
    }
    public string CityName { get; set; }
    public List<string>? ExemptVehicleTypes { get; set; }
    public ExemptMonth? ExemptMonth { get; set; }
    public List<DayOfWeek>? ExemptDaysOfWeek { get; set; }
    public List<DateTime>? ExemptDates{ get; set; }
    public List<TaxTimeRange> TaxTimeRanges { get; set; }
}
