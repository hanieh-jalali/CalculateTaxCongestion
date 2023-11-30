namespace Fintranet.Domain.Models
{
    public class TaxTimeRange
    {
        public TimeOnly StartTime{ get; set; }
        public TimeOnly EndTime{ get; set; }
        public decimal Amount { get; set;}
    }
}