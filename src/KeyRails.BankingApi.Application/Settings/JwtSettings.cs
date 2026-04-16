using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyRails.BankingApi.Application.Settings
{
    public class JwtSettings
    {
        public string RefreshTokenSecret { get; set; } = null!;
        public string JwtSecret { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int RefreshTokenExpirationMinutes { get; set; }
        public int TokenExpirationMinutes { get; set; }
    }
}