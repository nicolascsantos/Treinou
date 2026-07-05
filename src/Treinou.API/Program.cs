using Treinou.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200/")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAndConfigureControllers();
builder.Services.AddUseCases();
builder.Services.AddAppConnections(builder.Configuration);

var app = builder.Build();

app.UseCors("AllowAngularDev");

app.UseDocumentation();

app.MapOpenApi();

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
