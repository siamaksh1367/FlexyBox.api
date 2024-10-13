using FlexyBox.common;
using FlexyBox.core;
using FlexyBox.core.Services.ContentStorage;
using FlexyBox.core.Shared;
using FlexyBox.dal.Generic;
using FlexyBox.dal.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace FlexyBox.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddEnvironmentVariables();
            builder.Configuration.AddUserSecrets("ac67d5f4-4690-491a-a0d9-b99948807318");
            var okta = builder.Configuration.GetSection(nameof(OktaServer)).Get<OktaServer>();
            var sql = builder.Configuration.GetSection(nameof(SQLConnectionString)).Get<SQLConnectionString>();

            builder.Services.Configure<OktaServer>(builder.Configuration.GetSection(nameof(OktaServer)));
            builder.Services.Configure<SQLConnectionString>(builder.Configuration.GetSection(nameof(SQLConnectionString)));
            builder.Services.Configure<StorageConnectionString>(builder.Configuration.GetSection(nameof(StorageConnectionString)));
            Console.WriteLine(sql.ConnectionString);
            builder.Services.AddDbContext<FlexyBoxDB>(option => option.UseSqlServer(sql.ConnectionString));
            builder.Services.AddAutoMapper(typeof(DeleteCommand).Assembly);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
                {
                    c.Authority = okta.Authority;
                    c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidAudience = okta.Audience,
                        ValidIssuer = okta.Authority
                    };
                });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdmin", policy => policy.RequireClaim("permissions", "admin"));
            });
            builder.Services.AddMediatR(config =>
                config.RegisterServicesFromAssembly(typeof(DeleteCommand).Assembly)
                      .AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
                      .AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
                      .AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>))
            );

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IContentStorage, ContentBlobStorage>();


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseCors("AllowAllOrigins");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            app.Run();
        }
    }
}
