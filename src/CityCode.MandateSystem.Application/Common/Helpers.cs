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

        public static string GenerateTransactionId(string bankCode)
        {
            // Get current date in yyMMdd format
            var datePart = DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyMMdd");
            // Compose base string
            var baseString = bankCode + datePart;
            // Calculate remaining length for random digits
            int randomLength = 30 - baseString.Length;
            // Generate random digits
            var random = new Random();
            var randomDigits = new string(Enumerable.Range(0, randomLength)
            .Select(_ => random.Next(0, 10).ToString()[0]).ToArray());
            // Combine all parts
            return baseString + randomDigits;
        }
    }
}