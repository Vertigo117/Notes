using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Core.Contracts
{
    /// <summary>
    /// Результат выполнения запроса на регистрацию пользователя в системе
    /// </summary>
    public class RegistrationResponse
    {
        /// <summary>
        /// Результат регистрации
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Создаёт новый экземпляр класса <seealso cref="RegistrationResponse"/>
        /// </summary>
        public RegistrationResponse()
        {
            IsSuccess = true;
        }

        /// <summary>
        /// Создаёт новый экземпляр класса <seealso cref="RegistrationRequest"/> с сообщением
        /// </summary>
        /// <param name="message">Сообщение</param>
        public RegistrationResponse(string message)
        {
            Message = message;
            IsSuccess = false;
        }
    }
}
