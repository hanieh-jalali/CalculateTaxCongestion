using MediatR;
using Microsoft.EntityFrameworkCore;
using Repository;
using Fintranet.Application.Service.Command;
using Fintranet.Domain;
using Fintranet.Domain.Enums;
using Fintranet.Domain.Models;
using Fintranet.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(CreateCongestionTaxCommand).Assembly);
});
builder.Services.AddScoped<ICongestionTaxRepository, CongestionTaxRepository>();
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseInMemoryDatabase("CongestionTax"));
builder.Services.AddScoped<CityTaxRule>((serviceProvider) => {
    var config = serviceProvider.GetService<IConfiguration>();
    
    var cityTaxRule = new CityTaxRule()
    { 
        CityName = config.GetValue<string>("CityTaxRules:CityName"),
        ExemptDates = config.GetSection("CityTaxRules:ExemptDates").Get<List<DateTime>>(),
        ExemptDaysOfWeek = config.GetSection("CityTaxRules:ExemptDaysOfWeek").Get<List<DayOfWeek>>(),
        ExemptMonth = config.GetValue<ExemptMonth>("CityTaxRules:ExemptMonth"),
        ExemptVehicleTypes = config.GetSection("CityTaxRules:ExemptVehicleTypes").Get<List<string>>(),
        TaxTimeRanges = config.GetSection("CityTaxRules:TaxTimeRanges").Get<List<TaxTimeRange>>(),
    };
    return cityTaxRule;
} );
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();