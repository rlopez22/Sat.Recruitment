using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Services
{
    public interface ILoadFileService
    {
        Task LoadUsersFromFileAsync(string FullPath);
    }
}