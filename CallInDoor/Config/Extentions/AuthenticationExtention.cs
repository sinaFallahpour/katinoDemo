﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Katino.Config.Extentions
{
    public static class AuthenticationExtention
    {
        public static IServiceCollection AddOurAuthentication(this IServiceCollection services,
            AppSettings appSettings)
        {
            //// Authorization service
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("GetAllUser",
            //        policy => policy.RequireClaim("AccessAllUser", "True"));
            //});

            // JWT Authentication
            //var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.Secret));
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(PublicHelper.SECREKEY));

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            return services;
        }
    }
}

