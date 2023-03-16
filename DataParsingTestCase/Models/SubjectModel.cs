using System.Text.Json.Serialization;

namespace DataParsingTestCase.Models
{
    internal class SubjectModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        public static SubjectModel Empty => new SubjectModel();
    }
}
