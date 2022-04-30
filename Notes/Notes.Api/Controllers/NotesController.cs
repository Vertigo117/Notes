using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Core.Contracts;
using Notes.Core.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
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

        /// <summary>
        /// Получить постраничный список заметок
        /// </summary>
        /// <param name="skip">Сколько пропустить</param>
        /// <param name="take">Сколько вывести</param>
        /// <returns>Постраничный список заметок</returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((typeof(IEnumerable<NoteDto>)), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Get([FromQuery] int skip, [FromQuery] int take)
        {
            string email = GetUserMail();
            PagedNotesDto response = await notesService.GetPagedNotesAsync(email, skip, take);
            return Ok(response);
        }

        /// <summary>
        /// Получить заметку с указанным уникальным идентификатором
        /// </summary>
        /// <param name="id">Уникальный идентификатор заметки</param>
        /// <returns>Результат выполнения операции</returns>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((typeof(NoteDto)), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            NoteDto response = await notesService.GetNoteAsync(id);
            return Ok(response);
        }

        /// <summary>
        /// Создать новую заметку
        /// </summary>
        /// <param name="noteUpsertDto">Данные заметки</param>
        /// <returns>Результат операции</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((typeof(NoteUpsertDto)), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Create([FromBody] NoteUpsertDto noteUpsertDto)
        {
            string email = GetUserMail();
            NoteDto response = await notesService.CreateNoteAsync(noteUpsertDto, email);
            return Ok(response);
        }

        private string GetUserMail()
        {
            return HttpContext.User.FindFirstValue(ClaimTypes.Email);
        }

        /// <summary>
        /// Удалить заметку с указанным уникальным идентификатором
        /// </summary>
        /// <param name="id">Уникальный идентификатор заметки</param>
        /// <returns>Результат операции</returns>
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

        /// <summary>
        /// Обновить данные заметки
        /// </summary>
        /// <param name="id">Уникальный идентификатор заметки</param>
        /// <param name="noteUpsertDto">Данные для обновления</param>
        /// <returns>Результат операции</returns>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] NoteUpsertDto noteUpsertDto)
        {
            NoteDto note = await notesService.GetNoteAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            await notesService.UpdateNoteAsync(id, noteUpsertDto);
            return Ok();
        }
    }
}
