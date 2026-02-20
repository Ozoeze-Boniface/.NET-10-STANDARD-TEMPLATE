using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Application.Dtos
{
    public class JwtAuthenticationResponse
    {
        public JwtAuthenticationResponse(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
        }

        public bool IsAuthenticated { get; set; }
        public TokenUserData User { get; set; } = new();
        public IEnumerable<TokenPermissionData> Permissions { get; set; } = [];
        public string? Role { get; set; }
    }
}
