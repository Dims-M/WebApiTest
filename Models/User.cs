using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Models
{
    /// <summary>
    /// Пользователь программы
    /// Пользователь программы
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
      
        /// <summary>
        /// дата добавления в БД
        /// </summary>
        public DateTime DataAddBD { get; set; }

    }
}
