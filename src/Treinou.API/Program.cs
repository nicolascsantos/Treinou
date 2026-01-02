using Treinou.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAndConfigureControllers();
builder.Services.AddUseCases();
builder.Services.AddAppConnections(builder.Configuration);

var app = builder.Build();

app.UseDocumentation();

app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
