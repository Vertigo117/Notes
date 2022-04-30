using AutoFixture.Xunit2;
using AutoMapper;
using Moq;
using Notes.Core.Contracts;
using Notes.Core.Services;
using Notes.Core.Tests.Attributes;
using Notes.Data.Entities;
using Notes.Data.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Notes.Core.Tests
{
    public class NotesServiceTests
    {
        [Theory]
        [AutoMoqData]
        public async Task CreateNoteAsync_MapperMapNoteMethodCalled(
            [Frozen] Mock<IMapper> mapperMock,
            string email,
            NotesService notesService,
            Note note,
            NoteUpsertDto noteDto)
        {
            //Arrange
            mapperMock.Setup(mock => mock.Map<Note>(It.IsAny<NoteUpsertDto>())).Returns(note);

            //Act
            await notesService.CreateNoteAsync(noteDto, email);

            //Assert
            mapperMock.Verify(mock => mock.Map<Note>(It.IsAny<NoteUpsertDto>()), Times.Once());
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateNoteAsync_CreationDateAssignedProperly(
            [Frozen] Mock<IMapper> mapperMock,
            string email,
            NotesService notesService,
            Note note,
            NoteUpsertDto noteUpsertDto,
            NoteDto noteDto)
        {
            //Arrange
            mapperMock.Setup(mock => mock.Map<Note>(It.IsAny<NoteUpsertDto>())).Returns(note);
            mapperMock.Setup(mock => mock.Map<NoteDto>(It.IsAny<Note>())).Returns(noteDto);

            //Act
            NoteDto actual = await notesService.CreateNoteAsync(noteUpsertDto, email);

            //Assert
            Assert.Equal(actual.CreationDate, noteDto.CreationDate);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateNoteAsync_RepositoryUsersGetAsyncMethodCalled(
            [Frozen] Mock<IMapper> mapperMock,
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            string email,
            User user,
            NotesService notesService,
            Note note,
            NoteUpsertDto noteUpsertDto)
        {
            //Arrange
            mapperMock.Setup(mock => mock.Map<Note>(It.IsAny<NoteUpsertDto>())).Returns(note);
            unitOfWorkMock.Setup(mock => mock.Users.GetAsync(It.IsAny<string>())).ReturnsAsync(user);

            //Act
            await notesService.CreateNoteAsync(noteUpsertDto, email);

            //Assert
            unitOfWorkMock.Verify(mock => mock.Users.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateNoteAsync_RepositoryNotesAddMethodCalled(
            [Frozen] Mock<IMapper> mapperMock,
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            string email,
            User user,
            NotesService notesService,
            Note note,
            NoteUpsertDto noteUpsertDto)
        {
            //Arrange
            mapperMock.Setup(mock => mock.Map<Note>(It.IsAny<NoteUpsertDto>())).Returns(note);
            unitOfWorkMock.Setup(mock => mock.Users.GetAsync(It.IsAny<string>())).ReturnsAsync(user);

            //Act
            await notesService.CreateNoteAsync(noteUpsertDto, email);

            //Assert
            unitOfWorkMock.Verify(mock => mock.Notes.Add(It.IsAny<Note>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateNoteAsync_RepositoryNotesSaveChangesAsyncMethodCalled(
            [Frozen] Mock<IMapper> mapperMock,
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            string email,
            User user,
            NotesService notesService,
            Note note,
            NoteUpsertDto noteUpsertDto)
        {
            //Arrange
            mapperMock.Setup(mock => mock.Map<Note>(It.IsAny<NoteUpsertDto>())).Returns(note);
            unitOfWorkMock.Setup(mock => mock.Users.GetAsync(It.IsAny<string>())).ReturnsAsync(user);

            //Act
            await notesService.CreateNoteAsync(noteUpsertDto, email);

            //Assert
            unitOfWorkMock.Verify(mock => mock.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateNoteAsync_MapperMapNoteDtoMethodCalled(
            [Frozen] Mock<IMapper> mapperMock,
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            string email,
            User user,
            NotesService notesService,
            Note note,
            NoteUpsertDto noteUpsertDto,
            NoteDto noteDto)
        {
            //Arrange
            mapperMock.Setup(mock => mock.Map<Note>(It.IsAny<NoteUpsertDto>())).Returns(note);
            mapperMock.Setup(mock => mock.Map<NoteDto>(It.IsAny<Note>())).Returns(noteDto);
            unitOfWorkMock.Setup(mock => mock.Users.GetAsync(It.IsAny<string>())).ReturnsAsync(user);

            //Act
            await notesService.CreateNoteAsync(noteUpsertDto, email);

            //Assert
            mapperMock.Verify(mock => mock.Map<NoteDto>(It.IsAny<Note>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetNoteAsync_RepositoryNotesGetAsyncMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            Note note,
            NotesService notesService)
        {
            //Arrange
            unitOfWorkMock.Setup(mock => mock.Notes.GetAsync(It.IsAny<int>())).ReturnsAsync(note);

            //Act
            await notesService.GetNoteAsync(note.Id);

            //Assert
            unitOfWorkMock.Verify(mock => mock.Notes.GetAsync(It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetNoteAsync_MapperMapNoteDtoMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IMapper> mapperMock,
            Note note,
            NotesService notesService,
            NoteDto noteDto)
        {
            //Arrange
            unitOfWorkMock.Setup(mock => mock.Notes.GetAsync(It.IsAny<int>())).ReturnsAsync(note);
            mapperMock.Setup(mock => mock.Map<NoteDto>(It.IsAny<Note>())).Returns(noteDto);

            //Act
            await notesService.GetNoteAsync(note.Id);

            //Assert
            mapperMock.Verify(mock => mock.Map<NoteDto>(It.IsAny<Note>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetPagedNotesAsync_GetTotalAsyncMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            int total,
            int take,
            int skip,
            string email,
            NotesService notesService)
        {
            //Arrange
            unitOfWorkMock.Setup(mock => mock.Notes.GetTotalAsync(It.IsAny<Expression<Func<Note, bool>>>()))
                .ReturnsAsync(total);

            //Act
            await notesService.GetPagedNotesAsync(email, skip, take);

            //Assert
            unitOfWorkMock.Verify(mock => 
                mock.Notes.GetTotalAsync(It.IsAny<Expression<Func<Note, bool>>>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetPagedNotesAsync_GetPagedAsyncMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            int total,
            int take,
            int skip,
            string email,
            IEnumerable<Note> notes,
            NotesService notesService)
        {
            //Arrange
            unitOfWorkMock.Setup(mock => mock.Notes.GetTotalAsync(It.IsAny<Expression<Func<Note, bool>>>()))
                .ReturnsAsync(total);
            unitOfWorkMock.Setup(mock => mock.Notes.GetPagedAsync(
                It.IsAny<Expression<Func<Note, bool>>>(),
                It.IsAny<int>(),
                It.IsAny<int>())).ReturnsAsync(notes);

            //Act
            await notesService.GetPagedNotesAsync(email, skip, take);

            //Assert
            unitOfWorkMock.Verify(mock => mock.Notes.GetPagedAsync(
                    It.IsAny<Expression<Func<Note, bool>>>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetPagedNotesAsync_MapperMapMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IMapper> mapperMock,
            int total,
            int take,
            int skip,
            string email,
            IEnumerable<Note> notes,
            IEnumerable<NoteDto> notesDto,
            NotesService notesService)
        {
            //Arrange
            unitOfWorkMock.Setup(mock => mock.Notes.GetTotalAsync(It.IsAny<Expression<Func<Note, bool>>>()))
                .ReturnsAsync(total);
            unitOfWorkMock.Setup(mock => mock.Notes.GetPagedAsync(
                It.IsAny<Expression<Func<Note, bool>>>(),
                It.IsAny<int>(),
                It.IsAny<int>())).ReturnsAsync(notes);
            mapperMock.Setup(mock => mock.Map<IEnumerable<NoteDto>>(It.IsAny<IEnumerable<Note>>())).Returns(notesDto);

            //Act
            await notesService.GetPagedNotesAsync(email, skip, take);

            //Assert
            mapperMock.Verify(mock => mock.Map<IEnumerable<NoteDto>>(It.IsAny<IEnumerable<Note>>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetPagedNotesAsync_ReturnsPagedNotesDto(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IMapper> mapperMock,
            int total,
            int take,
            int skip,
            string email,
            IEnumerable<Note> notes,
            IEnumerable<NoteDto> notesDto,
            NotesService notesService)
        {
            //Arrange
            unitOfWorkMock.Setup(mock => mock.Notes.GetTotalAsync(It.IsAny<Expression<Func<Note, bool>>>()))
                .ReturnsAsync(total);
            unitOfWorkMock.Setup(mock => mock.Notes.GetPagedAsync(
                It.IsAny<Expression<Func<Note, bool>>>(),
                It.IsAny<int>(),
                It.IsAny<int>())).ReturnsAsync(notes);
            mapperMock.Setup(mock => mock.Map<IEnumerable<NoteDto>>(It.IsAny<IEnumerable<Note>>())).Returns(notesDto);

            //Act
            PagedNotesDto actual = await notesService.GetPagedNotesAsync(email, skip, take);

            //Assert
            Assert.NotNull(actual);
            Assert.IsType<PagedNotesDto>(actual);
            Assert.Equal(total, actual.Total);
            Assert.NotEmpty(actual.Notes);
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteNoteAsync_RepositoryNotesGetAsyncMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            Note note,
            NotesService notesService)
        {
            //Arrange
            unitOfWorkMock.Setup(mock => mock.Notes.GetAsync(It.IsAny<int>())).ReturnsAsync(note);

            //Act
            await notesService.DeleteNoteAsync(note.Id);

            //Assert
            unitOfWorkMock.Verify(mock => mock.Notes.GetAsync(It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteNoteAsync_RepositoryNotesRemoveMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            Note note,
            NotesService notesService)
        {
            //Arrange
            unitOfWorkMock.Setup(mock => mock.Notes.GetAsync(It.IsAny<int>())).ReturnsAsync(note);
            unitOfWorkMock.Setup(mock => mock.Notes.Remove(It.IsAny<Note>())).Verifiable();

            //Act
            await notesService.DeleteNoteAsync(note.Id);

            //Assert
            unitOfWorkMock.Verify(mock => mock.Notes.Remove(It.IsAny<Note>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteNoteAsync_RepositorySaveChangesAsyncMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            Note note,
            NotesService notesService)
        {
            //Arrange
            unitOfWorkMock.Setup(mock => mock.Notes.GetAsync(It.IsAny<int>())).ReturnsAsync(note);

            //Act
            await notesService.DeleteNoteAsync(note.Id);

            //Assert
            unitOfWorkMock.Verify(mock => mock.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task UpdateNoteAsync_RepositoryNotesGetAsyncMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            Note note,
            NotesService notesService,
            NoteUpsertDto noteUpsertDto)
        {
            //Arrange
            unitOfWorkMock.Setup(mock => mock.Notes.GetAsync(It.IsAny<int>())).ReturnsAsync(note);

            //Act
            await notesService.UpdateNoteAsync(note.Id, noteUpsertDto);

            //Assert
            unitOfWorkMock.Verify(mock => mock.Notes.GetAsync(It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task UpdateNoteAsync_MapMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IMapper> mapperMock,
            Note note,
            NotesService notesService,
            NoteUpsertDto noteUpsertDto)
        {
            //Arrange
            unitOfWorkMock.Setup(mock => mock.Notes.GetAsync(It.IsAny<int>())).ReturnsAsync(note);
            mapperMock.Setup(mock => mock.Map(It.IsAny<NoteUpsertDto>(), It.IsAny<Note>())).Verifiable();

            //Act
            await notesService.UpdateNoteAsync(note.Id, noteUpsertDto);

            //Assert
            mapperMock.Verify(mock => mock.Map(It.IsAny<NoteUpsertDto>(), It.IsAny<Note>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task UpdateNoteAsync_RepositoryNotesUpdateMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            Note note,
            NotesService notesService,
            NoteUpsertDto noteUpsertDto)
        {
            //Arrange
            unitOfWorkMock.Setup(mock => mock.Notes.GetAsync(It.IsAny<int>())).ReturnsAsync(note);
            unitOfWorkMock.Setup(mock => mock.Notes.Update(It.IsAny<Note>())).Verifiable();

            //Act
            await notesService.UpdateNoteAsync(note.Id, noteUpsertDto);

            //Assert
            unitOfWorkMock.Verify(mock => mock.Notes.Update(It.IsAny<Note>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task UpdateNoteAsync_RepositorySaveChangesAsyncMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            Note note,
            NotesService notesService,
            NoteUpsertDto noteUpsertDto)
        {
            //Arrange
            unitOfWorkMock.Setup(mock => mock.Notes.GetAsync(It.IsAny<int>())).ReturnsAsync(note);

            //Act
            await notesService.UpdateNoteAsync(note.Id, noteUpsertDto);

            //Assert
            unitOfWorkMock.Verify(mock => mock.SaveChangesAsync(), Times.Once);
        }
    }
}
