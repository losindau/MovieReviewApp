using MovieReview.Infrastructure.Commons.Constants;
using Newtonsoft.Json;
using System.Text;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace MovieReview.Infrastructure.Commons.Helpers
{
    public static class JsonHelper
    {
        public static ICollection<T> LoadJsonFile<T>(string fileName)
        {
            if (!File.Exists(fileName))
            {
                StringBuilder message = new StringBuilder(JsonConstants.FilePathNotFound);
                message.Append(fileName);
                throw new FileNotFoundException(message.ToString());
            }

            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<ICollection<T>>(jsonReader)!;
            }
        }
    }
}
