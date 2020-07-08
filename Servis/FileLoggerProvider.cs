using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Servis
{
    /// <summary>
    ////Провайдер логирования
    /// </summary>
    public class FileLoggerProvider : ILoggerProvider
    {
        
        private string path; //путь к файлу лога

        /// <summary>
        /// Конструктор иницализирует путь к файлу
        /// </summary>
        /// <param name="_path">путь к файлу лога</param>
        public FileLoggerProvider(string _path)
        {
            path = _path;
        }

        /// <summary>
        /// Cоздает и возвращает объект логгера. 
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(path);
        }

        /// <summary>
        /// Управляет освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
        }

    }
}
