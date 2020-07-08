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

        //  »спользуйте этот метод дл€ добавлени€ служб в контейнер.
        public void ConfigureServices(IServiceCollection services)
        {
            string con = "Server=(localdb)\\mssqllocaldb;Database=usersdbstore;Trusted_Connection=True;";
            // устанавливаем контекст данных
            services.AddDbContext<UsersContext>(options => options.UseSqlServer(con));

            services.AddControllers(); // контролеры
            services.AddMvc();

            services.AddSignalR();// сервис работы с SignalR
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //¬ каком режиме мы находимс€
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // показываем страницу с ошибками
            }


            // пишем на консоль информацию. «апускать только в консольном исполнении
           // logger.LogInformation("Processing request {0}");
            //или так
            // logger.LogInformation($"Processing request {context.Request.Path}");

            //»спользуем фабрику логеров
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddDebug();
            });
            // создаем обьект фабрики логеров
            ILogger logger = loggerFactory.CreateLogger<Startup>();


            app.UseDefaultFiles(); //–абота с остатическими файлами
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // запус через атрибуты контроллера
                endpoints.MapHub<ChatHub>("/chat"); 
            });
        }
    }
}
