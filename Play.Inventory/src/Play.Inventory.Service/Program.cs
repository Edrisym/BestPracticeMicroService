using Play.Common.MongoDB;
using Play.Inventory.Service.Cients;
using Play.Inventory.Service.Entities;
using SharpCompress.Archives.SevenZip;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMongo(builder.Configuration)
                .AddMongoRepository<InventoryItem>("inventoryitems");

builder.Services.AddHttpClient<CatalogClients>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5131");
});

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
