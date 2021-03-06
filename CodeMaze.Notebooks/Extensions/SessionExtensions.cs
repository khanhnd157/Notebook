using Microsoft.AspNetCore.Http;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;

using System.Text;

namespace CodeMaze.Notebooks.Extensions
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, value.ToJson());
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : BsonSerializer.Deserialize<T>(value);
        }
    }
}
