using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
  public static class IdentityServiceExtentions
  {
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
     {
       options.TokenValidationParameters = new TokenValidationParameters
       {
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
         ValidateIssuerSigningKey = true,
         ValidateIssuer = false,
         ValidateAudience = false
       };
     });
      return services;
    }
  }
}