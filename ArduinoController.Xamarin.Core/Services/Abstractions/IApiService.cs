using System.Threading.Tasks;
using ArduinoController.Xamarin.Core.Dto;

namespace ArduinoController.Xamarin.Core.Services.Abstractions
{
    public interface IApiService
    {
        Task<TResponse> CallAsync<TResponse>(string pathToResource, string httpMethod, object body, bool authorize = true);
        Task<LoginDto> Login(string email, string password);
    }
}
