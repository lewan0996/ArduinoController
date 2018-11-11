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

        public async Task<LoginDto> Login(string email, string password)
        {
            var result = await CallAsync<LoginDto>("users/login", "POST", new {email, password}, false);

            _appSettings.AddOrUpdateValue("token", result.Token);
            _appSettings.AddOrUpdateValue("refreshToken", result.RefreshToken);

            return result;
        }

        //public void Logout()
        //{
        //    _appSettings.Remove("token");
        //    _appSettings.Remove("refreshToken");
        //}

        //public async Task Register(string email, string password) // do poprawy - powtorzony kod
        //{
        //    var uri = new Uri(Url + "/register");
        //    var body = new StringContent(JsonConvert.SerializeObject(new { email, password }), Encoding.UTF8,
        //        "application/json");

        //    var response = await _httpClient.PostAsync(uri, body);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new UnsuccessfulStatusCodeException(response.ReasonPhrase);
        //    }
        //}

        //public async Task RefreshToken(string token, string refreshToken) // do poprawy - powtorzony kod
        //{
        //    var uri = new Uri(Url + "/refresh");
        //    var body = new StringContent(JsonConvert.SerializeObject(new { token, refreshToken }), Encoding.UTF8,
        //        "application/json");

        //    var response = await _httpClient.PostAsync(uri, body);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new UnsuccessfulStatusCodeException(response.ReasonPhrase);
        //    }

        //    var result = JsonConvert.DeserializeObject<LoginDto>(await response.Content.ReadAsStringAsync());

        //    _appSettings.AddOrUpdateValue("token", result.Token);
        //    _appSettings.AddOrUpdateValue("refreshToken", result.RefreshToken);
        //}

        public bool IsLoggedIn => _appSettings.Contains("token");
        

        public async Task<TResponse> CallAsync<TResponse>(string pathToResource, string httpMethod, object body,
            bool authorize = true)
        {
            var uri = new Uri(Configuration.ApiUrl + "/" + pathToResource);
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

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
                    response = await _httpClient.GetAsync(uri);
                    break;
                case "POST":
                    response = await _httpClient.PostAsync(uri, content);
                    break;
                case "PUT":
                    response = await _httpClient.PutAsync(uri, content);
                    break;
                case "DELETE":
                    response = await _httpClient.DeleteAsync(uri);
                    break;
                default:
                    throw new Exception("Unsupported http method");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new UnsuccessfulStatusCodeException(response.ReasonPhrase);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseBody = JsonConvert.DeserializeObject<TResponse>(responseContent);

            return responseBody;
        }

        private async Task<string> GetRefreshedTokenAsync(string token, string refreshToken)
        {
            var refreshResponse = await CallAsync<LoginDto>("users/refresh", "POST",
                new LoginDto {Token = token, RefreshToken = refreshToken}, false);

            _appSettings.AddOrUpdateValue("token", refreshResponse.Token);
            _appSettings.AddOrUpdateValue("refreshToken", refreshResponse.RefreshToken);

            return refreshResponse.Token;
        }

        private async Task<string> GetToken()
        {
            var savedToken = _appSettings.GetValueOrDefault("token", "");
            var base64EncodedBytes = Convert.FromBase64String(savedToken);
            var decodedToken = Encoding.UTF8.GetString(base64EncodedBytes);
            var claimsPart = decodedToken
                .SkipWhile(c => c != '.')
                .TakeWhile(c => c != '.')
                .ToString();
            var claims = JsonConvert.DeserializeObject<Dictionary<string,string>>(claimsPart);

            var expiryDate = DateTime.Parse(claims["exp"]);

            if ((DateTime.Now - expiryDate).TotalMinutes > 1)
            {
                return savedToken;
            }

            var savedRefreshToken = _appSettings.GetValueOrDefault("refreshToken","");
            return await GetRefreshedTokenAsync(savedToken, savedRefreshToken);
        }
    }
}
