using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Core.Contracts;
using Notes.Core.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Notes.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesService notesService;

        public NotesController(INotesService notesService)
        {
            this.notesService = notesService;
        }
        
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((typeof(NoteDto)), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            NoteDto response = await notesService.GetNoteAsync(id);
            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((typeof(NoteUpsertDto)), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Create([FromBody] NoteUpsertDto request)
        {
            NoteDto response = await notesService.CreateNoteAsync(request, HttpContext);
            return Ok(response);
        }
        
        [HttpGet]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((typeof(IEnumerable<NoteDto>)), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAll()
        {
            IEnumerable<NoteDto> response = await notesService.GetAllNotesAsync(HttpContext);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            NoteDto note = await notesService.GetNoteAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            await notesService.DeleteNoteAsync(note.Id);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] NoteUpsertDto request)
        {
            NoteDto note = await notesService.GetNoteAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            await notesService.UpdateNoteAsync(id, request);
            return Ok();
        }
    }
}
