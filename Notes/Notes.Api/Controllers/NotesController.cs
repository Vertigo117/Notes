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
        [ProducesResponseType((typeof(GetNoteResponse)), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            GetNoteResponse response = await notesService.GetNoteAsync(id);
            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((typeof(CreateNoteResponse)), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Create([FromBody] CreateNoteRequest request)
        {
            CreateNoteResponse response = await notesService.CreateNoteAsync(request, HttpContext);
            return Ok(response);
        }
        
        [HttpGet]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((typeof(IEnumerable<GetNoteResponse>)), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAll()
        {
            IEnumerable<GetNoteResponse> response = await notesService.GetAllNotesAsync(HttpContext);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            GetNoteResponse note = await notesService.GetNoteAsync(id);

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
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateNoteRequest request)
        {
            GetNoteResponse note = await notesService.GetNoteAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            await notesService.UpdateNoteAsync(id, request);
            return Ok();
        }
    }
}
