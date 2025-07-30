using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Dtos;

namespace CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string toEmail, MailContent mailContent);
    }
}