using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ArduinoController.Xamarin.Core.Dto;
using ArduinoController.Xamarin.Core.Exceptions;
using ArduinoController.Xamarin.Core.Services.Abstractions;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace ArduinoController.Xamarin.Core.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ISettings _appSettings;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _appSettings = CrossSettings.Current;
        }

        public async Task CallAsync(string pathToResource, string httpMethod, object body = null, bool authorize = true)
        {
            await CallAsync<object>(pathToResource, httpMethod, body, authorize);
        }

        public async Task<LoginDto> Login(string email, string password)
        {
            var result = await CallAsync<LoginDto>("users/login", "POST", new { email, password }, false);

            _appSettings.AddOrUpdateValue("token", result.Token);
            _appSettings.AddOrUpdateValue("refreshToken", result.RefreshToken);

            return result;
        }

        public void Logout()
        {
            _appSettings.Remove("token");
            _appSettings.Remove("refreshToken");
        }

        public bool IsLoggedIn => _appSettings.Contains("token");


        public async Task<TResponse> CallAsync<TResponse>(string pathToResource, 
            string httpMethod, object body = null, bool authorize = true)
        {
            var uri = new Uri(Configuration.ApiUrl + "/" + pathToResource);
            var content = body != null
                ? new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
                : null;

            if (authorize)
            {
                if (!IsLoggedIn)
                {
                    throw new NotLoggedInException();
                }

                var token = await GetToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage response;
            
            switch (httpMethod)
            {
                case "GET":
                    response = _httpClient.GetAsync(uri).GetAwaiter().GetResult();
                    break;
                case "POST":
                    response= _httpClient.PostAsync(uri, content).GetAwaiter().GetResult();
                    break;
                case "PUT":
                    response = _httpClient.PutAsync(uri, content).GetAwaiter().GetResult();
                    break;
                case "DELETE":
                    response = _httpClient.DeleteAsync(uri).GetAwaiter().GetResult();
                    break;
                default:
                    throw new Exception("Unsupported http method");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new UnsuccessfulStatusCodeException(response
                    .Content?
                    .ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult(), response.StatusCode, response.ReasonPhrase);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseBody = JsonConvert.DeserializeObject<TResponse>(responseContent);

            return responseBody;
        }

        private async Task<string> GetRefreshedTokenAsync(string token, string refreshToken)
        {
            var refreshResponse = await CallAsync<LoginDto>("users/refresh", "POST",
                new LoginDto { Token = token, RefreshToken = refreshToken }, false);

            _appSettings.AddOrUpdateValue("token", refreshResponse.Token);
            _appSettings.AddOrUpdateValue("refreshToken", refreshResponse.RefreshToken);

            return refreshResponse.Token;
        }

        private async Task<string> GetToken()
        {
            var savedToken = _appSettings.GetValueOrDefault("token", "");

            var claimsPart = new string(savedToken.SkipWhile(c => c != '.').Skip(1).TakeWhile(c => c != '.').ToArray());

            while (claimsPart.Length % 4 != 0)
            {
                claimsPart += "=";
            }
            var base64EncodedBytes = Convert.FromBase64String(claimsPart);
            var decodedClaims = Encoding.UTF8.GetString(base64EncodedBytes);
            var claims = JsonConvert.DeserializeObject<Dictionary<string, object>>(decodedClaims);

            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var expiryDate = unixEpoch.AddSeconds(Convert.ToDouble(claims["exp"]));

            if ((expiryDate - DateTime.UtcNow).TotalMinutes > 1)
            {
                return savedToken;
            }

            var savedRefreshToken = _appSettings.GetValueOrDefault("refreshToken", "");
            return await GetRefreshedTokenAsync(savedToken, savedRefreshToken);
        }
    }
}
