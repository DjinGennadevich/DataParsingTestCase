using DataParsingTestCase.Models;

namespace DataParsingTestCase
{
    internal interface IParsingService : IDisposable
    {
        Task<SubjectModel[]> GetSubjectsAsync();

        Task<CourtModel[]> GetCourtsBySubjectIdAsync(int subjectId);

        Task<IEnumerable<ParsingResultModel>> GetParsingResultsAsync();
    }
}
