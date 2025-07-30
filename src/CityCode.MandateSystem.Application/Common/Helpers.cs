using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Application.Common
{
    public static class Helpers
    {
        public static string GenerateUniqueOtp()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] randomNumber = new byte[4]; // 4 bytes = 32-bit integer
                rng.GetBytes(randomNumber);
                int value = Math.Abs(BitConverter.ToInt32(randomNumber, 0));

                // Generate a 6-digit number
                int otp = value % 1000000;

                // Pad with leading zeros if needed (e.g., "000123")
                return otp.ToString("D6");
            }
        }
    }
}