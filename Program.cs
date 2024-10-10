using FlexyBox.common;
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
            var okta = builder.Configuration.GetSection(nameof(Okta)).Get<Okta>();
            var sql = builder.Configuration.GetSection(nameof(SQLConnectionString)).Get<SQLConnectionString>();

            builder.Services.Configure<Okta>(builder.Configuration.GetSection(nameof(Okta)));
            builder.Services.Configure<SQLConnectionString>(builder.Configuration.GetSection(nameof(SQLConnectionString)));
            builder.Services.Configure<StorageConnectionString>(builder.Configuration.GetSection(nameof(StorageConnectionString)));

            builder.Services.AddDbContext<FlexyBoxDB>(option => option.UseSqlServer(sql.ConnectionString));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = okta.Authority;
                options.Audience = okta.Audience;
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
