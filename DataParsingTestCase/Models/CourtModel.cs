using System.Text.Json.Serialization;

namespace DataParsingTestCase.Models
{
    internal class CourtModel
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
