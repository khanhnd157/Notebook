using MazeCore.MongoDb.Entities;

using MongoDB.Bson.Serialization.Attributes;

namespace CodeMaze.Data
{
    [BsonDiscriminator("Notes")]
    public class NoteBookEntity : BaseEntity
    {

        [BsonRequired]
        public string Code { get; set; }
        public string Title { get; set; }
        public string Access { get; set; }
        public string Password { get; set; }
        [BsonRequired]
        public string Note { get; set; }
        public string Username { get; set; }
    }
}
