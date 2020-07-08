using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Servis
{
    /// <summary>
    /// Класс логирования. Запись в файл
    /// </summary>
    public class FileLogger : ILogger
    {
        private string filePath;
        private static object _lock = new object();

        public FileLogger(string path)
        {
            filePath = path;
        }

        /// <summary>
        /// Этот метод возвращает объект IDisposable, который представляет некоторую область видимости для логгера.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }


        /// <summary>
        /// Метод которые указыват, доступен ли логгер для использования. 
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns>возвращает значения true или false</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            //return logLevel == LogLevel.Trace;
            return true;
        }

        /// <summary>
        /// метод предназначен для выполнения логгирования. Он принимает пять параметров:
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel">уровень детализации текущего сообщения</param>
        /// <param name="eventId">идентификатор события</param>
        /// <param name="state">некоторый объект состояния, который хранит сообщение</param>
        /// <param name="exception">информация об исключении</param>
        /// <param name="formatter"> функция форматирвания, которая с помощью двух предыдущих параметов позволяет получить собственно сообщение для логгирования</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {

                lock (_lock) // Блокировку обьекта для ожидание завиршения выполнения 
                {
                    File.AppendAllText(filePath, formatter(state, exception) + Environment.NewLine); // запись в файл
                }
            }
        }
    }
}
}
