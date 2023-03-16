using System.Text.Json.Serialization;

namespace DataParsingTestCase.Models
{
    internal class ParsingResultModel
    {
        [JsonPropertyName("subject")]
        public SubjectModel Subject { get; set; }

        [JsonPropertyName("child_courts")]
        public CourtModel[] Courts { get; set; }
    }
}
