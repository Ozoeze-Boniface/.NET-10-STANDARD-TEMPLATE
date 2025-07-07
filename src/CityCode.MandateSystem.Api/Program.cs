

using SeaBaas.CentralJournalPosting.Api.Infrastructure;
using CityCode.MandateSystem.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddKeyVaultIfConfigured(builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder => builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()));


// builder.Host.UseSerilog(
//     (HostBuilderContext context, IServiceProvider serviceProvider, LoggerConfiguration config) => config.ReadFrom
//         .Configuration(context.Configuration)
//         .ReadFrom.Services(serviceProvider));

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
// await app.InitialiseDatabaseAsync();
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


app.UseExceptionHandler(options => { });
app.UseCustomExceptionHandler();
app.MapEndpoints();
app.UseSerilogRequestLogging();
app.UseEndpointDefinitions();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

//app.UseSwagger();
//app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CityCode.MandateSystem Middleware"));

app.Run();
public partial class Program { }

