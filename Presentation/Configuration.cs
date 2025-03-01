using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions;
using Domain;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Domain.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Presentation
{
    public static class Configuration
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );
            services.AddScoped<DataSeeder>();
            
            return services;
        }
        
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecurityKey"]!))
                };

                var jwtBearerEvents = new JwtBearerEvents();
                jwtBearerEvents.OnTokenValidated += TokenValidatedAsync;
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services) => services.AddSwaggerGen(options =>
        {
            var assemblyDate = File.GetLastWriteTimeUtc(Assembly.GetExecutingAssembly().Location);
            var buildNo = assemblyDate.DayOfYear + "." + (assemblyDate.Hour * 60 + assemblyDate.Minute);
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "InternalApp",
                Version = $"Build {buildNo} on date " + assemblyDate.ToString("dd/MM hh:mm")
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });

            options.CustomSchemaIds(type => type.FullName);

        }).AddSwaggerGenNewtonsoftSupport();

        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder app) => app.UseExceptionHandler(builder =>
        {
            builder.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    if (contextFeature.Error.InnerException is null)
                    {
                        context.Items["Exception"] = contextFeature.Error.Message;
                        context.Items["StackTrace"] = contextFeature.Error.StackTrace;
                    }
                    else
                    {
                        context.Items["Exception"] = $"{contextFeature.Error.Message}\n{contextFeature.Error.InnerException.Message}";
                        context.Items["StackTrace"] = $"{contextFeature.Error.StackTrace}\n{contextFeature.Error.InnerException.StackTrace}";
                    }

                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        BadRequestException => StatusCodes.Status400BadRequest,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    if (contextFeature.Error is AggregateException aggregateException && aggregateException.InnerExceptions.Any(e => e is BadRequestException))
                    {
                        await context.Response.WriteAsJsonAsync(new
                        {
                            message = string.Join("\n", aggregateException.InnerExceptions.Select(e => e.Message)),
                            timestamp = DateTime.UtcNow
                        });
                    }
                    else if (context.Response.StatusCode == StatusCodes.Status500InternalServerError)
                    {
                        await context.Response.WriteAsJsonAsync(new
                        {
                            message = "Internal Server Error",
                            details = contextFeature.Error.Message,
                            type = contextFeature.Error.GetType().Name,
                            timestamp = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        await context.Response.WriteAsJsonAsync(new
                        {
                            message = contextFeature.Error.Message,
                            timestamp = DateTime.UtcNow
                        });
                    }
                }
            });
        });  

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IAccountRepository, AccountRepository>()
                .AddScoped<IBeachRepository, BeachRepository>()
                .AddScoped<IReviewRepository, ReviewRepository>();
                
            return services;
        }
        
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<IJwtService, JwtService>()
                .AddScoped<IBeachService, BeachService>()
                .AddScoped<IReviewService, ReviewService>();
                
            return services;
        }
        
        private static async Task TokenValidatedAsync(TokenValidatedContext ctx)
        {
            if (!(ctx.SecurityToken is JsonWebToken accessToken))
            {
                ctx.Fail("The token is not a JWT token.");
                return;
            }

            var accountId = accessToken.Claims
                .FirstOrDefault(x => string.Equals(x.Type, "accountId"))?.Value;

            if (string.IsNullOrEmpty(accountId))
            {
                ctx.Fail("The accountId is missing.");
                return;
            }

            if (!Guid.TryParse(accountId, out var accountIdGuid))
            {
                ctx.Fail("The accountId is not GUID.");
                return;
            }

            using var scope = ctx.HttpContext.RequestServices.CreateScope();
            var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();

            if (await jwtService.CheckAccessTokenAsync(accountIdGuid))
                ctx.Success();
            else
                ctx.Fail("Invalid access token.");
        }
    }
}


