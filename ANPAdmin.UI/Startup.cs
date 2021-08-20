using System;
using ANPAdmin.Business;
using ANPAdmin.Data;
using FluentMigrator.Runner;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ANPAdmin.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFluentMigratorCore()
                .ConfigureRunner(cfg => cfg
                    .AddSQLite() // Utilize .AddSqlServer() caso escolha SQL Server
                    .WithGlobalConnectionString(Configuration.GetConnectionString("BaseComm"))
                    .ScanIn(typeof(Startup).Assembly).For.Migrations()
                )
                .AddLogging(cfg => cfg.AddFluentMigratorConsole());

            services.AddHealthChecks();
            services.AddSession();
            
            if (!String.IsNullOrWhiteSpace(Configuration["ApplicationInsights:InstrumentationKey"]))
            {
                services.AddApplicationInsightsTelemetry(Configuration);
                services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>(
                    (module, o) =>
                    {
                        module.EnableSqlCommandTextInstrumentation = true;
                    });
            }

            services.AddRazorPages();

            services.AddScoped<IAuth, Auth>();
            services.AddSingleton<IUserRepository>(
                new UserRepositorySQLite(Configuration.GetConnectionString("BaseComm")));
            // Para utilizar SQL Server substitua a linha anterior por:
            // services.AddScoped<IUserRepository, userRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
        {
            migrationRunner.MigrateUp();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseHealthChecks("/check");

            app.UseSession();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}