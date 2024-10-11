using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BudgetTracker.Web.SignalR;
using BudgetTracker.Web.Data;
using BudgetTracker.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using BudgetTracker.Web.Repositories;

namespace BudgetTracker.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Register repositories
            services.AddScoped<IBudgetRepository, BudgetRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IIncomeRepository, IncomeRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            // Register services
            services.AddScoped<BudgetService>();
            services.AddScoped<IncomeService>();
            services.AddScoped<CategoryService>();
            // Register the BudgetNotificationService as a singleton
            services.AddHostedService<BudgetNotificationService>();

            var key = Encoding.UTF8.GetBytes("YourSuperSecretKeyHereYourSuperSecretKeyHereYourSuperSecretKeyHereYourSuperSecretKeyHere");
            // JWT Authentication configuration
            Console.WriteLine($"JWT Validation Key: {Convert.ToBase64String(key)}");

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.UseSecurityTokenValidators = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };

                    /*options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                            Console.WriteLine($"Token being validated: {token}");  // Log the token being validated
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                            return Task.CompletedTask;
                        }
                    };*/
                });

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                });

            //services.AddControllers();
            services.AddDbContext<BudgetTrackerContext>(options =>
                options.UseInMemoryDatabase("BudgetTrackerDB"));

            services.AddSignalR();
            services.AddHostedService<BudgetNotificationService>();
            services.AddSwaggerGen(); // For Swagger API testing
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Add Authentication and Authorization
            app.UseAuthentication(); // Authentication middleware must come before authorization
            app.UseAuthorization();  // This ensures that [Authorize] attributes work


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/notificationHub");
            });

            // Swagger middleware
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BudgetTracker API V1");
            });
        }
    }
}
