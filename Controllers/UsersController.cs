using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApiTest.Models;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

       private UsersContext db;
       private readonly ILogger _logger;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">контекст для работы с БД типа UsersContext</param>
        public UsersController(UsersContext context, ILogger<UsersController> logger)
        {
            db = context; 

            if (!db.Users.Any()) // проверяем. Если бд не существует. Создаем новую
            {
                db.Users.Add(new User { Name = "Alice", Age = 31, DataAddBD = DateTime.Now });
                db.Users.Add(new User { Name = "Tom", Age = 26, DataAddBD = DateTime.Now });
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Получаем все данные из Бд по таблице узер
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
           // _logger.LogInformation("Test логирования");
            return await db.Users.ToListAsync();
            
        }

        // Тестовые методы
      //  [ApiController]
       // [Route("api/[AddFile]")]
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                //using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                //{
                //    await uploadedFile.CopyToAsync(fileStream);
                //}
                FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };
               // _context.Files.Add(file);
               // _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }




        /// <summary>
        /// Получаем пользователя по id
        /// </summary>
        /// <param name="id">id нужного пользователя.</param>
        /// <returns></returns>
        // GET api/users/5 https://localhost:44325/api/users/1
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            User user = await db.Users.FirstOrDefaultAsync(x => x.Id == id); //Ищем по id. Есть ли такой пользователь в БД

            if (user == null) // Проверяем на null
                return NotFound(); //Если обькт пользователя пустой(незаполненый). Ошибку
            return new ObjectResult(user); // отправляем  готовый результат
        }

        /// <summary>
        ///Добавляем пользователя. 
        /// </summary>
        /// <param name="user">Модель типа User user</param>
        /// <returns>Возрат результата. Если без ошибок то ОК -200</returns>
        // POST api/users
        [HttpPost]
        public async Task<ActionResult<User>> Post(User user) //
      //  public async Task<ActionResult<User>> Post(IFormFile uploadedFile) //
        {
           //string temp = uploadedFile.ToString();
            //if (user.Age == 99)
            //    ModelState.AddModelError("Age", "Возраст не должен быть равен 99");

            //if (user.Name == "admin")
            //    if (user.Name == "admin")
            //{
            //    ModelState.AddModelError("Name", "Недопустимое имя пользователя - admin");
            //}
            //// если есть лшибки - возвращаем ошибку 400
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            //db.Users.Add(user);
            //await db.SaveChangesAsync(); // сохранение в Бд
           // return Ok(user);
            return Ok();
        }


        //[HttpPost]
        //public ActionResult Index(HttpPostedFileBase upload)
        //{
        //    //Вот здесь я хочу принять файл и записать его
        //    return Content("ура- файл записан");

        //}

        /// <summary>
        /// Обновление текущего пользователя.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        // PUT api/users/
        [HttpPut]
        public async Task<ActionResult<User>> Put(User user)
        {

            if (user == null)
            {
                return BadRequest();
            }
            if (!db.Users.Any(x => x.Id == user.Id)) // проверка. Если такого пользователя с заданой id нет. То ошибка
            {
                return NotFound();
            }

            db.Update(user); // обновлние 
            await db.SaveChangesAsync();
            return Ok(user);
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int id)
        {
            User user = db.Users.FirstOrDefault(x => x.Id == id); //ищем по id в Бд

            if (user == null) // проверяем на nuul
            {
                return NotFound();
            }

            db.Users.Remove(user); // Запись на очистку Бд
            await db.SaveChangesAsync(); //Сохранеия состояния БД
            return Ok(user); // возврат статуса
        }

        [HttpPost]
      //  [Route("foo/bar")]
        [Consumes("multipart/form-data")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> FooBar([FromForm] User data)
        {
            if (data == null)
            {
                return BadRequest ();
            }

            db.Add(data);
            await db.SaveChangesAsync();
            return Ok(data);
        }


    }
}
