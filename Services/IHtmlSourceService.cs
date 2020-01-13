using System.Threading.Tasks;

namespace InfoTrack.Services
{
    public interface IHtmlSourceService
    {
        Task<string> GetSource(string keywords);
    }
}
