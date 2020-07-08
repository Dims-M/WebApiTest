using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.SignaR
{
    /// <summary>
    /// Хаб для обработки запросов SignaR
    /// </summary>
    public class ChatHub : Hub
    {
        ///// <summary>
        ///// Оправление сообщение клиентам(потребителя)
        ///// </summary>
        ///// <param name="message">само тело сообщения</param>
        ///// <returns></returns>
        //public async Task Send(string message)
        //{
        //    await this.Clients.All.SendAsync("Send", message);
        //}

        /// <summary>
        /// Метод для работы с 2 параметрами. Тело сообщния и имя отправиеля
        /// </summary>
        /// <param name="message">Тело сообщения</param>
        /// <param name="userName">Имя отправителя</param>
        /// <returns></returns>
        public async Task Send(string message, string userName)
        {
            await Clients.All.SendAsync("Send", message, userName);
        }
    }
}
