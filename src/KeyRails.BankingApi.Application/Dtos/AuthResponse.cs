using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyRails.BankingApi.Application.Dtos
{
    public record AuthResponse(string accessToken, string refreshToken, User User);
}