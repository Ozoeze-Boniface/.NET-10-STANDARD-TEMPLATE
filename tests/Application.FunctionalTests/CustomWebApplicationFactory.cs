namespace CityCode.MandateSystem.Application.FunctionalTests;
using System.Data.Common;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using CityCode.MandateSystem.Application.Common.Interfaces;
using CityCode.MandateSystem.Infrastructure.Data;

using static Testing;

public class CustomWebApplicationFactory(DbConnection connection) : WebApplicationFactory<Program>
{
    private readonly DbConnection _connection = connection;

    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.ConfigureTestServices(services =>
                                                                              {
                                                                                  services
                                                                                      .RemoveAll<IUser>()
                                                                                      .AddTransient(provider => Mock.Of<IUser>(s => s.Id == GetUserId()));

                                                                                  services
                                                                                      .RemoveAll<DbContextOptions<ApplicationDbContext>>()
                                                                                      .AddDbContext<ApplicationDbContext>((sp, options) =>
                                                                                      {
                                                                                          options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                                                                                          options.UseSqlServer(this._connection);
                                                                                      });
                                                                              });
}
