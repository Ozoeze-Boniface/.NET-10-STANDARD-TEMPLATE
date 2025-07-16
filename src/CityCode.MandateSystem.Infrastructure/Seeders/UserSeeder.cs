using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Common.Interfaces;
using CityCode.MandateSystem.Domain.Constants;
using CityCode.MandateSystem.Domain.Entities;
using CityCode.MandateSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CityCode.MandateSystem.Infrastructure.Seeders
{
    public class UserSeeder : IDataSeeder
    {
        private readonly IApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UserSeeder(IApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task SeedAsync()
        {
            try
            {
                var email = _configuration.GetValue<string>("DefaultAdminEmail") ?? "";
                if (string.IsNullOrEmpty(email))
                {
                    //
                }
                else
                {
                    var existsing = await _context.AppUsers.AnyAsync(s => s.Email == email);
                    if (!existsing)
                    {
                        var user = new User("System", "Admin", email, "08068854789", "system", null!, true, DateTime.UtcNow, Role.Admin, true);
                        user.WithPermissions(new List<Domain.DomainDto.PermissionDto>()
                        {
                            new Domain.DomainDto.PermissionDto
                            {
                                Name = PermissionConstants.CreateMandate,
                                Action = "Approve",
                                Description = "CreatingMandate",
                                IsActive = true,
                                Resource = "User"
                            }
                        });
                        await _context.AppUsers.AddAsync(user);
                        await _context.SaveChangesAsync(CancellationToken.None);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
        public int Order => 2;
    }
}