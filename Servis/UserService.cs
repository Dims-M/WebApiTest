using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest.Models;

namespace WebApiTest.Servis
{
    /// <summary>
    /// Класс для работы  взаимодействия с контекстом и бд , с кешом 
    /// Инкапсулирует все объекты кэша в виде словаря Dictionary.
    /// При сохранении объекта в кэше в качестве его ключа выступает значение свойства Id
    /// </summary>
    public class UserService
    {
        //Подробно на https://metanit.com/sharp/aspnet5/14.1.php

        private UsersContext db;
        private IMemoryCache cache;


        //Конструктор. Через механизм внедрения зависимостей будет получать контекст данных
        public UserService(UsersContext context, IMemoryCache memoryCache)
        {
            db = context;
            cache = memoryCache;
        }

        //Иницализация БД. Если она не существует
        public void Initialize()
        {
            if (!db.Users.Any())
            {
                db.Users.AddRange(
                    new User { Name = "Tom",  Age = 35 },
                    new User { Name = "Alice", Age = 29 },
                    new User { Name = "Sam",  Age = 37 }
                );
                db.SaveChanges();
            }
        }


        /// <summary>
        /// Получаем всех пользователь из Бд
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await db.Users.ToListAsync();
        }


        /// <summary>
        /// Добавление новых узеров
        /// </summary>
        /// <param name="user">Заполненная модель приложения</param>
        /// <returns></returns>
        public async Task AddUser(User user)
        {
            db.Users.Add(user);
            int n = await db.SaveChangesAsync(); // при сохранеии в БД получаем интовое значени
           
            if (n > 0) // проверка.
            {                                   
                cache.Set(user.Id, user, new MemoryCacheEntryOptions // Создаем кэш и заполняем его моделями
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // время жизни кэша
                });
            }
        }

        /// <summary>
        /// Получение user по id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User> GetUser(int id)
        {
            User user = null;

            //Проверка. Если данные в кэше есть. 
            if (!cache.TryGetValue(id, out user)) //пытаемся получить элемент по ключу key(id).  Если нет то регаем из БД
            {
                user = await db.Users.FirstOrDefaultAsync(p => p.Id == id); // получаем их БД по id
               
                if (user != null) 
                {
                    cache.Set(user.Id, user,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5))); // Создание нового кэша.
                }
            }
            return user;
        }



        //[HttpPost]
        //[Route("foo/bar")]
        //[Consumes("multipart/form-data")]
        //[DisableRequestSizeLimit]
        //public async Task<IActionResult> FooBar([FromForm] User data)
        //{
        //    if (data == null)
        //    {
        //        return ContentResult "";
        //    }

        //    db.Add(data);
        //    await db.SaveChangesAsync();
        //    return Ok(data);
        //}


    }

}

