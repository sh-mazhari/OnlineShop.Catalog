using Catalog.API.Configurations;
using Catalog.Application.Contract.Dtos;
using Catalog.Application.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterDI(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.MapGet("/features/{id}", async (IFeatureService featureService, Guid id) =>
{
    var result = await featureService.GetById(id, Guid.NewGuid());
    Results.Ok(result);
}).WithDisplayName("Get Feature with Id");

app.MapPost("/features", async (FeatureDto model, IFeatureService featureService) =>
{
    var result = await featureService.Add(model);
    Results.Ok(result);
}).WithName("Add new Feature");


app.Run();
