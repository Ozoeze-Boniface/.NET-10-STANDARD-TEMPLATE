using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Domain.DomainDto;

namespace CityCode.MandateSystem.Domain.Entities
{
    public class User : BaseAuditableEntity
    {
        public long UserId { get; set; }
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string FullName => FirstName + " " + LastName;
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public string Username { get; private set; } = string.Empty;
        public string? PasswordHash { get; private set; }
        public bool IsSuperAdmin { get; set; } = false;
        public bool IsActive { get; private set; } = true;
        public string Role { get; set; }
        public DateTime? LastLogin { get; private set; }
        public string? Otp { get; set; }
        public virtual List<Permission>? Permission { get; private set; }


        public User(
            string firstName,
            string lastName,
            string email,
            string phoneNumber,
            string username,
            string passwordHash,
            bool isActive,
            DateTime? lastLogin,
            string role,
            bool isSuperAdmin,
            string? createdBy)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Username = username;
            PasswordHash = passwordHash;
            IsActive = isActive;
            LastLogin = lastLogin;
            Role = role;
            IsSuperAdmin = isSuperAdmin;
            CreatedBy = createdBy;
        }

        public void WithPermissions(List<PermissionDto>? permissions)
        {
            Permission = permissions?.Select(s => new Permission
            {
                Description = s.Description ?? "Creation",
                Action = s.Action ?? "User",
                Name = s.Name,
                IsActive = true,
                Resource = s.Resource ?? s.Name
            }).ToList() ?? null!;
        }

        public void SetPassword(string password)
        {
            PasswordHash = password;
        }

        public void ChangeUserStatus(bool status)
        {
            IsActive = status;
        }

        public void SetOtp(string otp)
        {
            Otp = otp;
        }

        public void Update(
            string? firstName = null,
            string? lastName = null,
            string? email = null,
            string? phoneNumber = null,
            string? username = null,
            bool? isActive = null,
            DateTime? lastLogin = null,
            string? role = null,
            bool? isSuperAdmin = null,
            List<PermissionDto>? permissions = null
        )
        {
            if (!string.IsNullOrWhiteSpace(firstName))
                FirstName = firstName;

            if (!string.IsNullOrWhiteSpace(lastName))
                LastName = lastName;

            if (!string.IsNullOrWhiteSpace(email))
                Email = email;

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                PhoneNumber = phoneNumber;

            if (!string.IsNullOrWhiteSpace(username))
                Username = username;

            if (isActive.HasValue)
                IsActive = isActive.Value;

            if (lastLogin.HasValue)
                LastLogin = lastLogin.Value;

            if (role != null)
                Role = role;

            if (isSuperAdmin.HasValue)
                IsSuperAdmin = isSuperAdmin.Value;

            if (permissions != null)
            {
                Permission = permissions.Select(s => new Permission
                {
                    Description = s.Description ?? "Creation",
                    Action = s.Action ?? "User",
                    Name = s.Name,
                    IsActive = true,
                    Resource = s.Resource ?? s.Name
                }).ToList();
            }
        }

    }



}