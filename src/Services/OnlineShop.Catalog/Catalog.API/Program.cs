using Catalog.API.Configurations;
using Catalog.Application.Contract.Dtos;
using Catalog.Application.IServices;
using Catalog.Infrastructure;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
//var builder = WebApplication.CreateBuilder(new WebApplicationOptions
//{
//    ApplicationName = typeof(Program).Assembly.FullName,
//    ContentRootPath = Path.GetFullPath(Directory.GetCurrentDirectory()),
//    WebRootPath = Path.GetFullPath(Directory.GetCurrentDirectory()),
//    Args = args
//});

// Add services to the container.
builder.Services.AddDaprClient();

builder.Services.AddInfrastructure(builder.Configuration);
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
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

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
