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

        //  Используйте этот метод для добавления служб в контейнер.
        public void ConfigureServices(IServiceCollection services)
        {
            string con = "Server=(localdb)\\mssqllocaldb;Database=usersdbstore;Trusted_Connection=True;";
            
            // устанавливаем контекст данных
            services.AddDbContext<UsersContext>(options => options.UseSqlServer(con));

            // внедрение зависимости UserService для работы с кэшированием 
            services.AddTransient<UserService>();
            // добавление кэширования
            services.AddMemoryCache();

            // добавляем сервис компрессии. сжатия данных для отпраки клиенту
            services.AddResponseCompression(options => options.EnableForHttps = true);
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });


            services.AddControllers(); // используем контроллеры без представлений
            services.AddControllersWithViews();
            services.AddMvc();

            services.AddSignalR();// сервис работы с SignalR
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //В каком режиме мы находимся
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // показываем страницу с ошибками
            }


            // пишем на консоль информацию. Запускать только в консольном исполнении
            // logger.LogInformation("Processing request {0}");
            //или так
            // logger.LogInformation($"Processing request {context.Request.Path}");

            //Используем фабрику логеров
            //loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
           // loggerFactory.
           // var logger = loggerFactory.CreateLogger("FileLogger");

            //var loggerFactory = LoggerFactory.Create(builder =>
            //{
            //    builder.AddDebug();
            //});

            // создаем обьект фабрики логеров
          //  ILogger logger = loggerFactory.CreateLogger<Startup>();


            app.UseDefaultFiles();
            //app.UseStaticFiles();//Работа с остатическими файлами
           app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Add("Cache-Control", "public,max-age=600"); // устанавливаем кэш на 10 минут
                }
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //Подключаем метод компрессии. сжатия данных для отправки клиенту
            app.UseResponseCompression();

            // подключаем CORS // подробно https://metanit.com/sharp/aspnet5/31.1.php
            app.UseCors(builder => builder.AllowAnyOrigin()); //С помощью метода AllowAnyOrigin() мы указываем, что приложение может обрабатывать запросы от приложений по любым адрес

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // запус через атрибуты контроллера
                endpoints.MapHub<ChatHub>("/chat"); 
            });
        }
    }
}
