using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApiTest.Models;
using WebApiTest.Servis;
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

            // ��������� ����������� UserService ��� ������ � ������������ 
            services.AddTransient<UserService>();
            // ���������� �����������
            services.AddMemoryCache();

            // ��������� ������ ����������. ������ ������ ��� ������� �������
            services.AddResponseCompression(options => options.EnableForHttps = true);
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });


            services.AddControllers(); // ���������� ����������� ��� �������������
            services.AddControllersWithViews();
            services.AddMvc();

            services.AddSignalR();// ������ ������ � SignalR
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
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
            //loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
           // loggerFactory.
           // var logger = loggerFactory.CreateLogger("FileLogger");

            //var loggerFactory = LoggerFactory.Create(builder =>
            //{
            //    builder.AddDebug();
            //});

            // ������� ������ ������� �������
          //  ILogger logger = loggerFactory.CreateLogger<Startup>();


            app.UseDefaultFiles();
            //app.UseStaticFiles();//������ � ������������� �������
           app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Add("Cache-Control", "public,max-age=600"); // ������������� ��� �� 10 �����
                }
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //���������� ����� ����������. ������ ������ ��� �������� �������
            app.UseResponseCompression();

            // ���������� CORS // �������� https://metanit.com/sharp/aspnet5/31.1.php
            app.UseCors(builder => builder.AllowAnyOrigin()); //� ������� ������ AllowAnyOrigin() �� ���������, ��� ���������� ����� ������������ ������� �� ���������� �� ����� �����

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // ����� ����� �������� �����������
                endpoints.MapHub<ChatHub>("/chat"); 
            });
        }
    }
}
