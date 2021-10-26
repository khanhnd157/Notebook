using AutoMapper;

using CodeMaze.Cryptography.Hash;
using CodeMaze.Data;
using CodeMaze.Models;

using MazeCore.MongoDb.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeMaze.Services
{
    public class NotebookService : INotebookService
    {
        private readonly IMongoRepository<NoteBookEntity> repositoryNotebook;
        private readonly IMapper mapper;

        public NotebookService(IMongoRepository<NoteBookEntity> repositoryNotebook, IMapper mapper)
        {
            this.repositoryNotebook = repositoryNotebook;
            this.mapper = mapper;
        }

        public async Task<NotebookModel> AddAsync(NotebookModel noteBook)
        {
            var entity = mapper.Map<NoteBookEntity>(noteBook);

            if (string.IsNullOrEmpty(entity.Username))
            {
                entity.Username = "anonymous";
            }

            entity.Username = entity.Username.Trim().ToLower();

            if (entity.Access == "protected")
            {
                if (!string.IsNullOrWhiteSpace(entity.Password))
                    entity.Password = HashPassword.Hash(entity.Password);
                else
                {
                    entity.Access = "public";
                    entity.Password = string.Empty;
                }
            }
            else
                entity.Password = string.Empty;

            var dateCreated = System.DateTime.UtcNow;
            entity.CreatedAt = dateCreated;
            entity.UpdatedAt = dateCreated;

            await repositoryNotebook.AddAsync(entity);

            return mapper.Map<NotebookModel>(entity);
        }

        public async Task<NotebookModel> GetByIdAsync(string id)
        {
            var note = await repositoryNotebook.GetFirstOrDefaultAsync(x => x.Id.Equals(id));

            return mapper.Map<NotebookModel>(note);
        }

        public async Task<NotebookModel> GetByCodeAsync(string code)
        {
            var note = await repositoryNotebook.GetFirstOrDefaultAsync(x => x.Code.Equals(code));

            return mapper.Map<NotebookModel>(note);
        }

        public Task<bool> AnyByCodeAsync(string code)
        {
            return repositoryNotebook.AnyAsync(x => x.Code.Equals(code));
        }

        public Task<bool> AnyByIdAsync(string id)
        {
            return repositoryNotebook.AnyAsync(x => x.Id.Equals(id));
        }

        public async Task<List<NotebookModel>> GetAllAsync()
        {
            var notes = await repositoryNotebook.SelectAsync(book => true);

            return notes?.Select(note => mapper.Map<NotebookModel>(note))?
                .ToList() ??
                new List<NotebookModel>();
        }

        public async Task<List<NotebookModel>> GetAllByUserAsync(string username)
        {
            var notes = await repositoryNotebook.SelectAsync(n => n.Username.Equals(username));

            return notes?.Select(note => mapper.Map<NotebookModel>(note))?
                .ToList() ??
                new List<NotebookModel>();
        }

        public async Task<NotebookModel> UpdateAsync(string id, NotebookModel noteBook)
        {
            var model = await repositoryNotebook.GetFirstOrDefaultAsync(x => x.Id.Equals(id));
            if (!string.IsNullOrEmpty(model?.Id))
            {
                var entity = mapper.Map<NoteBookEntity>(noteBook);
                entity.UpdatedAt = DateTime.UtcNow;

                if (string.IsNullOrEmpty(entity.Username))
                {
                    entity.Username = "anonymous";
                }
                entity.Username = entity.Username.Trim().ToLower();

                if (entity.Access == "protected")
                {
                    if (string.IsNullOrEmpty(entity.Password) || entity.Password.Equals("_******_"))
                        entity.Password = model.Password;
                    else
                        entity.Password = HashPassword.Hash(entity.Password);
                }
                else
                    entity.Password = string.Empty;

                var note = await repositoryNotebook.UpdateAsync(id, entity);

                return mapper.Map<NotebookModel>(note);
            }
            throw new NullReferenceException("Could not find notebook " + id);
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            await repositoryNotebook.DeleteAsync(id);

            return true;
        }
    }
}
