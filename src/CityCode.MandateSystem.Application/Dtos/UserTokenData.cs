using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Application.Dtos
{
    public class UserTokenData
    {
        public bool IsAuthenticated { get; set; }
        public User User { get; set; } = null!;
        public IEnumerable<Permission> Permissions { get; set; }
        public string? Role { get; set; }
        public UserTokenData(bool isAuthenticated, User user, List<Permission> permissions, string role)
        {
            User = user;
            Permissions = permissions;
            Role = role;
            IsAuthenticated = isAuthenticated;
        }
    }
}