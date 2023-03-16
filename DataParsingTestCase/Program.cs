using DataParsingTestCase;
using System.Text.Encodings.Web;
using System.Text.Json;

ParsingService parsingService = new();

var parsingResultModels = await parsingService.GetParsingResultsAsync();

JsonSerializerOptions jsonOptions = new();
jsonOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

var json = JsonSerializer.Serialize(parsingResultModels, jsonOptions);

Console.WriteLine(json);
Console.ReadLine();
    

