using System;

namespace CodeMaze.Models
{
    public class NotebookModel
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Access { get; set; }
        public string Password { get; set; }
        public string Note { get; set; }
        public string Username { get; set; }
    }
}
