using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Core.Contracts
{
    /// <summary>
    /// Результат запроса на получение всех заметок
    /// </summary>
    public class GetAllNotesResponse
    {
        public IEnumerable<GetNoteResponse> Notes { get; set; }
    }
}
