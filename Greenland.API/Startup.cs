using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greenland.API.Business_Logic;
using Greenland.API.Business_Logic.GoogleCalendar;
using Greenland.API.Business_Logic.Iterations;
using Greenland.API.Business_Logic.JWT;
using Greenland.API.Business_Logic.Mail_Logic;
using Greenland.API.DB;
using Greenland.API.Models;
//using Greenland.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Greenland.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<greenlandDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("greenlandDB")));
            services.AddCors();
            services.AddControllers();
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            services.Configure<CalendarSettings>(Configuration.GetSection("CalendarSettings"));
            services.Configure<IterationSettings>(Configuration.GetSection("IterationSettings"));
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IJwtService, JwtService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder => builder
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());


            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
