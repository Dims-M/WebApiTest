using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiTest.Models;
using WebApiTest.Servis;

namespace WebApiTest.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        ////Работа с кэшем
        /// </summary>
        UserService userService;

        //Конструктор. Через механизм внедрения зависимостей будет получать контекст данных
        public HomeController(UserService service) 
        {
            userService = service;
            userService.Initialize();
        }
        public async Task<IActionResult> Index(int id)
        {
            User user = await userService.GetUser(id);
            if (user != null)
                return Content($"User: {user.Name}");
            return Content("User not found");
        }
    }
}
