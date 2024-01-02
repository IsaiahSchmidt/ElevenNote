using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElevenNote.Data;
using ElevenNote.Data.Entities;
using ElevenNote.Models.Note;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElevenNote.Services.Note
{
    public class NoteService : INoteService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly int _userId;

        public NoteService(UserManager<UserEntity> userManager,
                            SignInManager<UserEntity> signInManager,
                            ApplicationDbContext dbContext)
        {
            var currentUser = signInManager.Context.User;
            var userIdClaim = userManager.GetUserId(currentUser);
            var hasValidId = int.TryParse(userIdClaim, out _userId);
            if (!hasValidId)
            {
                throw new Exception("Attempted to build NoteService without Id claim");
            }

            _dbContext = dbContext;
        }

        public async Task<NoteListItem?> CreateNoteAsync(NoteCreate request)
        {
            NoteEntity entity = new()
            {
                Title = request.Title,
                Content = request.Content,
                OwnerId = _userId,
                CreatedUtc = DateTimeOffset.Now
            };
            _dbContext.Notes.Add(entity);
            var numberOfChanges = await _dbContext.SaveChangesAsync();

            if (numberOfChanges != 1)
            {
                return null;
            }

            NoteListItem response = new()
            {
                Id = entity.Id,
                Title = entity.Title,
                CreatedUtc = entity.CreatedUtc
            };
            return response;
        }

        public async Task<IEnumerable<NoteListItem>> GetAllNotesAsync()
        {
            List<NoteListItem> notes = await _dbContext.Notes
                .Where(entity => entity.OwnerId == _userId)
                .Select(entity => new NoteListItem
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    CreatedUtc = entity.CreatedUtc 
                })
                .ToListAsync();
                return notes;
        }

        public async Task<NoteDetail?> GetNoteByIdAsync(int noteId)
        {
            NoteEntity? entity = await _dbContext.Notes
                .FirstOrDefaultAsync(e =>
                e.Id == noteId && e.OwnerId == _userId
                );
            return entity is null ? null : new NoteDetail
            {
                Id = entity.Id,
                Title = entity.Title,
                Content = entity.Content,
                CreatedUtc = entity.CreatedUtc,
                ModifiedUtc = (DateTimeOffset)entity.ModifiedUtc
            };
        }

        public async Task<bool> UpdateNoteAsync(NoteUpdate request)
        {
            NoteEntity? entity = await _dbContext.Notes.FindAsync(request.Id);
            if (entity?.OwnerId != _userId)
            {
                return false;
            }

            entity.Title = request.Title;
            entity.Content = request.Content;
            entity.ModifiedUtc = DateTimeOffset.Now;

            int numberOfChanges = await _dbContext.SaveChangesAsync();
            return numberOfChanges == 1;
        }

        public async Task<bool> DeleteNoteAsync(int noteId)
        {
            var noteEntity = await _dbContext.Notes.FindAsync(noteId);
            if (noteEntity?.OwnerId != _userId)
            {
                return false;
            }
            _dbContext.Notes.Remove(noteEntity);
            return await _dbContext.SaveChangesAsync() == 1;
        }
    }
}