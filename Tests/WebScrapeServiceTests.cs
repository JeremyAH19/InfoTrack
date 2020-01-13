using InfoTrack.Models;
using InfoTrack.Services;
using Moq;
using Xunit;

namespace InfoTrack.Tests
{
    public class WebScrapeServiceTests
    {
        [Fact]
        public async void When_GetSearchResultsCalled_CorrectResultsReturned()
        {
            var testSource = "<div class=\"BNeawe UPmit AP7Wnd\">www.infotrack.com</div><div class=\"BNeawe UPmit AP7Wnd\">Test</div><div class=\"BNeawe UPmit AP7Wnd\">www.infotrack.com.au</div>";
            var htmlSourceService = new Mock<IHtmlSourceService>();

            htmlSourceService.Setup(m => m.GetSource(It.IsAny<string>())).ReturnsAsync(testSource);

            var webScrapeService = new WebScrapeService(htmlSourceService.Object);
            var searchCriteria = new SearchCriteria() { Keywords = "", TargetUrl = "www.infotrack.com" };
            var actual = await webScrapeService.GetSearchResults(searchCriteria);

            Assert.Collection(actual, 
                e1 => {
                    Assert.Equal(1, e1.Number);
                    Assert.Equal("www.infotrack.com", e1.Url);
                },
                e2 => {
                    Assert.Equal(3, e2.Number);
                    Assert.Equal("www.infotrack.com.au", e2.Url);
                });
        }
    }
}