using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace Dmp.Stanlab.References.ReportingApi.Implementation.Registrations
{
    internal static class AuthorizationRegistration
    {
        internal static void AddJwtBearerAuthorization(this IServiceCollection services, string authority, string audience)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authority;
                    options.Audience = audience;
                });
        }
    }
}
