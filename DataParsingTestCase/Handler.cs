using System.Text.Encodings.Web;
using System.Text.Json;

namespace DataParsingTestCase
{
    public class Handler
    { 
        public async Task<Response> FuctionHandler(Request request)
        {
            ParsingService parsingService = new();

            var parsingResultModels = await parsingService.GetParsingResultsAsync();
             
            JsonSerializerOptions jsonOptions = new();
            jsonOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            var json = JsonSerializer.Serialize(parsingResultModels, jsonOptions);

            return new Response { StatusCode = 200, Body = json };
        }

        public static void Main() { }
    }
}
