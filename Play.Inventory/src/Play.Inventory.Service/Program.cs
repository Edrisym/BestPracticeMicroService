using System.ComponentModel.DataAnnotations;
using Play.Common.MongoDB;
using Play.Inventory.Service.Cients;
using Play.Inventory.Service.Entities;
using Polly;
using Polly.Timeout;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMongo(builder.Configuration)
                .AddMongoRepository<InventoryItem>("inventoryitems");
Random jitterer = new Random();
builder.Services.AddHttpClient<CatalogClients>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5131");
})
.AddTransientHttpErrorPolicy(build => build.Or<TimeoutRejectedException>().WaitAndRetryAsync(
5,
retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
   + TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)),
onRetry: (outcome, timespan, retryAttempt) =>
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    serviceProvider.GetService<ILogger<CatalogClients>>()?
        .LogWarning($"Delaying for {timespan.TotalSeconds} seconds, then making retry {retryAttempt}");
}
))
.AddTransientHttpErrorPolicy(build => build.Or<TimeoutRejectedException>().CircuitBreakerAsync(
3,
TimeSpan.FromSeconds(15),
onBreak: (outCome, timeSpan) =>
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    serviceProvider.GetService<ILogger<CatalogClients>>()?
    .LogWarning($"Opening the circuit breaker for {timeSpan.TotalSeconds} seconds...");
},
onReset: () =>
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    serviceProvider.GetService<ILogger<CatalogClients>>()?
    .LogWarning($"Closing the circuit...");
}
))
.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
