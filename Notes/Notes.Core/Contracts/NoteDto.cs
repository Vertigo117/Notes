using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Core.Contracts
{
    /// <summary>
    /// Результат выполнения запроса на получение записи
    /// </summary>
    public class NoteDto
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название заметки
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Текст заметки
        /// </summary>
        public string Text { get; set; }
    }
}
