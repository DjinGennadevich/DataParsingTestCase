using System.Text;

namespace DataParsingTestCase
{
    public static class HtmlHelper
    {
        public static async Task<string> GetSourceContentAsync(HttpContent content)
        {
            var byteContent = await content.ReadAsByteArrayAsync();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encoding = Encoding.GetEncoding("windows-1251");

            var htmlContent = encoding.GetString(byteContent);

            return htmlContent;
        }
    }
}
