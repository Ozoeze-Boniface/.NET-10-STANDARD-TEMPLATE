namespace CityCode.MandateSystem.Domain.Entities;
using System;

public class GapUser
{
    public string? FullName { get; set; }
    public string? CustomerId { get; set; }
    public string? BVN { get; set; }
    public bool? BvnConfirmed { get; set; } = false;
    public string? PrimaryAccountNO { get; set; }
    public string? NIN { get; set; }
    public bool? NinConfirmed { get; set; } = false;
    public string? IntPassportNo { get; set; }
    public bool? IntPassportNoConfirmed { get; set; } = false;
    public string? DrivingLicenseNo { get; set; }
    public bool? DrivingLicenseNoConfirmed { get; set; } = false;
    public string? VotersCardNo { get; set; }
    public bool? VotersCardConfirmed { get; set; } = false;
    public string? PinCodeHash { get; set; }
    public List<string>? Devices { get; set; }
    public DateTime? Birthday { get; set; }
    public string? FullAddress { get; set; }
    public bool? FullAddressConfirmed { get; set; } = false;
    public string? EmployerName { get; set; }
    public string? EmployerAddress { get; set; }
    public string? ReferredBy { get; set; }
    public string? NOKFullName { get; set; }
    public string? NOKResidentialAddress { get; set; }
    public string? NOKEmailAddress { get; set; }
    public string? NOKMobileNo { get; set; }
    public string? UpdatedBy { get; set; }
    public string? LastUpdated { get; set; }
}
