using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Servis
{
    /// <summary>
    /// Класс разширение. Добавляет к объекту ILoggerFactory метод расширения AddFile, который будет добавлять наш провайдер логгирования.
    /// </summary>
    public static class FileLoggerExtensions
    {
        /// <summary>
        /// добавляет к объекту ILoggerFactory метод расширения AddFile
        /// </summary>
        /// <param name="factory">текущий экземпляр логера</param>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns></returns>
        public static ILoggerFactory AddFile(this ILoggerFactory loggerFactory, string filePath)
        {
            loggerFactory.AddProvider(new FileLoggerProvider(filePath));

            return loggerFactory;
        }

    }
}
