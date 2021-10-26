using System;

namespace CodeMaze.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public bool IsActive { get; set; }
    }
}
