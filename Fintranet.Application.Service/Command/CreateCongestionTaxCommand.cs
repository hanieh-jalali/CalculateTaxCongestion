using MediatR;
using Fintranet.Domain.Models;

namespace Fintranet.Application.Service.Command;

public class CreateCongestionTaxCommand : IRequest<CongestionTaxTransaction>
{
    public DateTime EntryTime { get; set; }
    public string VehicleType { get; set; }
    public string PlateAreaCode { get; set; }
    public string Regionletter { get; set; }
    public int AgeIdentifier { get; set; }
}