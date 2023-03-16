using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using DataParsingTestCase.Models;

namespace DataParsingTestCase
{
    internal class ParsingService : IParsingService
    {
        private const string ElementTagOption = "option";
        private const string BaseUri = "https://sudrf.ru/index.php?id=300&var=true#sp";
        private const string CourtUri = "https://sudrf.ru/index.php?id=300&act=ajax_search&searchtype=sp&court_subj={0}&suds_subj=&var=true";

        private readonly HttpClient _httpClient;

        public ParsingService()
        {
            HttpClientHandler httpClientHandler = new() { MaxConnectionsPerServer = int.MaxValue };
            _httpClient = new HttpClient(httpClientHandler);
        }

        public async Task<SubjectModel[]> GetSubjectsAsync()
        {
            using var response = await _httpClient.GetAsync(BaseUri);

            var htmlContent = await HtmlHelper.GetSourceContentAsync(response.Content);
            var htmlDocument = new HtmlParser().ParseDocument(htmlContent);

            const string ElementIdSubMs = "sub_ms";
            var trTag = htmlDocument.GetElementById(ElementIdSubMs);

            if (trTag == null)
                return await Task.FromResult(Array.Empty<SubjectModel>());

            var subjects = trTag.GetElementsByTagName(ElementTagOption)
                .Select(x =>
                {
                    var element = (IHtmlOptionElement)x;

                    if (int.TryParse(element.Value, out int id))
                    {
                        return new SubjectModel { Id = id, Name = element.Text };
                    }

                    return SubjectModel.Empty;
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .ToArray();

            return subjects;
        }

        public async Task<CourtModel[]> GetCourtsBySubjectIdAsync(int subjectId)
        {
            var uri = string.Format(CourtUri, subjectId);
            using var response = await _httpClient.GetAsync(uri);

            var htmlContent = await HtmlHelper.GetSourceContentAsync(response.Content);
            var htmlDocument = new HtmlParser().ParseDocument(htmlContent);

            var courts = htmlDocument.GetElementsByTagName(ElementTagOption)
                .Select(x =>
                {
                    var element = (IHtmlOptionElement)x;

                    return new CourtModel { Code = element.Value, Name = element.Text };
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .ToArray();

            return courts;
        }

        public async Task<IEnumerable<ParsingResultModel>> GetParsingResultsAsync()
        {
            var subjects = await GetSubjectsAsync();

            using SemaphoreSlim semaphore = new(3);

            var resultTasks = subjects.Select(async subject =>
            {
                await semaphore.WaitAsync();

                try
                {
                    var courts = await GetCourtsBySubjectIdAsync(subject.Id);
                    return new ParsingResultModel { Subject = subject, Courts = courts };
                }
                finally
                {
                    semaphore.Release();
                }
            });

            var combinedParsingModels = await Task.WhenAll(resultTasks);

            return combinedParsingModels;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
