using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApiTest.Models;
using WebApiTest.SignaR;

namespace WebApiTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //  ����������� ���� ����� ��� ���������� ����� � ���������.
        public void ConfigureServices(IServiceCollection services)
        {
            string con = "Server=(localdb)\\mssqllocaldb;Database=usersdbstore;Trusted_Connection=True;";
            // ������������� �������� ������
            services.AddDbContext<UsersContext>(options => options.UseSqlServer(con));

            services.AddControllers(); // ����������
            services.AddMvc();

            services.AddSignalR();// ������ ������ � SignalR
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //� ����� ������ �� ���������
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // ���������� �������� � ��������
            }


            // ����� �� ������� ����������. ��������� ������ � ���������� ����������
           // logger.LogInformation("Processing request {0}");
            //��� ���
            // logger.LogInformation($"Processing request {context.Request.Path}");

            //���������� ������� �������
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddDebug();
            });
            // ������� ������ ������� �������
            ILogger logger = loggerFactory.CreateLogger<Startup>();


            app.UseDefaultFiles(); //������ � ������������� �������
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // ����� ����� �������� �����������
                endpoints.MapHub<ChatHub>("/chat"); 
            });
        }
    }
}
