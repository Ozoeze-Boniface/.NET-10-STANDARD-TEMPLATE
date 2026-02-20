using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Application.Dtos
{
    public class TokenUserData
    {
        public long UserId { get; set; }
        public bool IsSuperAdmin { get; set; }
    }

    public class TokenPermissionData
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UserTokenData
    {
        public bool IsAuthenticated { get; set; }
        public TokenUserData User { get; set; } = new();
        public IEnumerable<TokenPermissionData> Permissions { get; set; } = [];
        public string? Role { get; set; }

        public UserTokenData(bool isAuthenticated, TokenUserData user, IEnumerable<TokenPermissionData> permissions, string role)
        {
            User = user;
            Permissions = permissions;
            Role = role;
            IsAuthenticated = isAuthenticated;
        }
    }
}
