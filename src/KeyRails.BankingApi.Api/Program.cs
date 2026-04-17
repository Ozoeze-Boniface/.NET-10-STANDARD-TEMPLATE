using KeyRails.BankingApi.Api.Infrastructure;
using KeyRails.BankingApi.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
const string CorsPolicyName = "corsapp";

// Add services to the container.
//builder.Services.AddKeyVaultIfConfigured(builder.Configuration);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiServices();

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, corsPolicy =>
    {
        if (allowedOrigins.Length == 0 || allowedOrigins.Contains("*"))
        {
            corsPolicy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            return;
        }

        corsPolicy.WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
var runMigration = builder.Configuration.GetValue<bool>("RunMigrations");

if(runMigration)
    await app.InitialiseDatabaseAsync();

if (app.Environment.IsDevelopment())
{
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

// await app.InitializeSeed(CancellationToken.None);
app.UseExceptionHandler();
app.UseCustomExceptionHandler();
app.UseSerilogRequestLogging();
app.UseCors(CorsPolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KeyRails.BankingApi API"));
app.MapControllers();

app.Run();
public partial class Program { }

