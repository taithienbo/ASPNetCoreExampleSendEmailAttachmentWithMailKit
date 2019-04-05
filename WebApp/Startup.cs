using System;
using ASPNetCoreExampleSendEmailAttachmentWithMailKit.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

namespace ASPNetCoreExampleSendEmailAttachmentWithMailKit
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddEmail(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }

    public static class StartupExtensions
    {
        public static IServiceCollection AddEmail(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMailKit(optionBuilder =>
            {
                var mailKitOptions = new MailKitOptions()
                {
                    // get options from secrets.json 
                    Server = configuration.GetValue<string>("Email:Server"),
                    Port = configuration.GetValue<int>("Email:Port"),
                    SenderName = configuration.GetValue<string>("Email:SenderName"),
                    SenderEmail = configuration.GetValue<string>("Email:SenderEmail"),
                    // can be optional with no authentication 
                    Account = configuration.GetValue<string>("Email:Account"),
                    Password = configuration.GetValue<string>("Email:Password"),
                    Security = configuration.GetValue<bool>("Email:Security")
                };

                if (mailKitOptions.Server == null)
                {
                    throw new InvalidOperationException("Please specify SmtpServer in appsettings");
                }
                if (mailKitOptions.Port == 0)
                {
                    throw new InvalidOperationException("Please specify Smtp port in appsettings");
                }

                if (mailKitOptions.SenderEmail == null)
                {
                    throw new InvalidOperationException("Please specify SenderEmail in appsettings");
                }
                
                optionBuilder.UseMailKit(mailKitOptions);
            });
            services.AddScoped<IAppEmailService, AppEmailService>();
            return services;
        }
    }

}
