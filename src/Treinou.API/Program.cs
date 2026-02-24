using Treinou.API.Configurations;
using Treinou.Infraestructure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAndConfigureControllers();
builder.Services.AddUseCases();
builder.Services.AddAppConnections(builder.Configuration);
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<AuthDbContext>();

var app = builder.Build();

app.UseDocumentation();

app.MapOpenApi();

app.UseHttpsRedirection();

app.UseRateLimiter();

app.MapIdentityApi<ApplicationUser>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
