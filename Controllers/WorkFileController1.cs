using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiTest.Models;


namespace WebApiTest.Controllers
{
    /// <summary>
    /// Работа с избражением
    /// </summary>
    /// <returns></returns>
    public class WorkFileController : Controller
    {
        UsersContext _context; // работа с БД
        IWebHostEnvironment _appEnvironment; //  информация о хостинге и среде выполнения текущего приложения

        public WorkFileController(UsersContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        /// <summary>
        /// Выводим все данные из БД
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View(_context.FileModels.ToList());
        }

        /// <summary>
        /// Отправка файла на сервер.
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName; // путь к месту сохранения + имя самого файла
                
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream); // загрузка файла
                }
                FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };
                _context.FileModels.Add(file); // добавляем текущий файл
                _context.SaveChanges(); // сохранение 
            }

            return RedirectToAction("Index"); // редикерт
        }


    }
}
