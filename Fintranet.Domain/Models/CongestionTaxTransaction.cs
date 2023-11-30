using System.ComponentModel.DataAnnotations;

namespace Fintranet.Domain.Models;
public class CongestionTaxTransaction
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime EntryTime { get; set; }

    [Required]
    public string VehicleType { get; set; }

    [Required]
    public string CityName { get; set; }

    [Required]
    public decimal CongestionTaxAmount { get; set; }

    [Required]
    public string PlateAreaCode { get; set; }
    
    [Required]
    public string Regionletter { get; set; }
    
    [Required]
    public int AgeIdentifier { get; set; }
}