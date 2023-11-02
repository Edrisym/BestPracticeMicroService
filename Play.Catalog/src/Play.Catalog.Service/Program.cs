using MongoDB.Driver;
using Play.Catalog.Service.Repositories;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Settings;

var builder = WebApplication.CreateBuilder(args);

var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

builder.Services.AddMongo(builder.Configuration)
.AddMongoRepository<Item>("Items");

// Add services to the container.

builder.Services.AddControllers(
    option => { option.SuppressAsyncSuffixInActionNames = false; }
);
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
