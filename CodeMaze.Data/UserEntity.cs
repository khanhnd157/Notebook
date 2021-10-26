using MazeCore.MongoDb.Entities;

using MongoDB.Bson.Serialization.Attributes;

namespace CodeMaze.Data
{
    [BsonDiscriminator("Users")]
    public class UserEntity : BaseEntity
    {
        [BsonRequired]
        public string Username { get; set; }
        [BsonRequired]
        public string Password { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public bool IsActive { get; set; }
    }
}
