using CodeMaze.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMaze.Services
{
    public interface INotebookService
    {
        Task<List<NotebookModel>> GetAllAsync();
        Task<List<NotebookModel>> GetAllByUserAsync(string username);

        Task<NotebookModel> AddAsync(NotebookModel noteBook);

        Task<NotebookModel> UpdateAsync(string id, NotebookModel noteBook);

        Task<NotebookModel> GetByIdAsync(string id);

        Task<NotebookModel> GetByCodeAsync(string code);

        Task<bool> AnyByCodeAsync(string code);

        Task<bool> AnyByIdAsync(string code);

        Task<bool> DeleteByIdAsync(string id);
    }
}
