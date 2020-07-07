﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiTest.Models;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

       private UsersContext db;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">контекст для работы с БД типа UsersContext</param>
        public UsersController(UsersContext context)
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
            return await db.Users.ToListAsync();
        }

        /// <summary>
        /// Получаем пользователя по id
        /// </summary>
        /// <param name="id">id нужного пользователя.</param>
        /// <returns></returns>
        // GET api/users/5
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
        public async Task<ActionResult<User>> Post(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            db.Users.Add(user);
            await db.SaveChangesAsync(); // сохранение в Бд
            return Ok(user);
        }

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
            User user = db.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }
    



}
}