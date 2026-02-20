

using SeaBaas.CentralJournalPosting.Api.Infrastructure;
using CityCode.MandateSystem.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
const string CorsPolicyName = "corsapp";

// Add services to the container.
//builder.Services.AddKeyVaultIfConfigured(builder.Configuration);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();

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


// builder.Services.ConfigureApplicationCookie(options =>
// {
//     options.Events.OnRedirectToLogin = context =>
//     {
//         context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//         return Task.CompletedTask;
//     };

//     options.Events.OnRedirectToAccessDenied = context =>
//     {
//         context.Response.StatusCode = StatusCodes.Status403Forbidden;
//         return Task.CompletedTask;
//     };
// });


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

await app.InitializeSeed(CancellationToken.None);
app.UseExceptionHandler(options => { });
app.UseCustomExceptionHandler();
app.UseSerilogRequestLogging();
app.UseCors(CorsPolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.MapEndpoints();
app.UseEndpointDefinitions();

//app.UseSwagger();
//app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CityCode.MandateSystem Middleware"));

app.Run();
public partial class Program { }

