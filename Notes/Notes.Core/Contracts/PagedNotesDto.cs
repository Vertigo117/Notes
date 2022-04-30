using System.Collections.Generic;

namespace Notes.Core.Contracts
{
    public class PagedNotesDto
    {
        public int Total { get; set; }
        public IEnumerable<NoteDto> Notes { get; set; }
    }
}
