using MediatR;
using Microsoft.AspNetCore.Mvc;
using Fintranet.Application.Core.Query;
using Fintranet.Application.Service.Command;
using Fintranet.Domain.Models;

namespace Fintranet.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ComgestionController : ControllerBase
{
    private readonly ILogger<ComgestionController> _logger;
    private readonly IMediator _mediator;

    public ComgestionController(ILogger<ComgestionController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    [HttpPost("AddCongestionTaxTransaction")]
    public CongestionTaxTransaction AddCongestionTaxTransaction(CreateCongestionTaxCommand command)
    {
        var addCongestionTaxTransaction = _mediator.Send(command);
        return addCongestionTaxTransaction.Result;
    }

    [HttpGet("GetCongestionTaxTransaction")]
    public IEnumerable<CongestionTaxTransaction> GetCongestionTaxTransaction([FromQuery]GetCongestionTaxTransactionQuery query)
    {
        var getCongestionTaxTransaction = _mediator.Send(query);
        return getCongestionTaxTransaction.Result;
    }
}