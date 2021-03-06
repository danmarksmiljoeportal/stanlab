﻿using IdentityModel;

namespace Dmp.Stanlab.References.ReportingApi
{
    public static class DmpClaimTypes
    {
        public const string UserId = JwtClaimTypes.Subject;
        public const string Name = JwtClaimTypes.Name;
        public const string Email = JwtClaimTypes.Email;
        public const string Company = "vat";
        public const string Role = JwtClaimTypes.Role;
    }
}
