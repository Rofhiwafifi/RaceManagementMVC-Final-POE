using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RaceManagementMVC.Services
{
    public class apiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public apiService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        // Create an HTTP client for the Part 2 API.
        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(
                _configuration["ApiSettings:BaseUrl"]!);

            string? token = _httpContextAccessor.HttpContext?
                .Session.GetString("token");

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        token);
            }

            return client;
        }

        // Send a GET request.
        public async Task<HttpResponseMessage> GetAsync(
            string url)
        {
            var client = CreateClient();

            return await client.GetAsync(url);
        }

        // Send a POST request.
        public async Task<HttpResponseMessage> PostAsync<T>(
            string url,
            T request)
        {
            var client = CreateClient();

            string json = JsonSerializer.Serialize(request);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            return await client.PostAsync(url, content);
        }

        // Send a PUT request.
        public async Task<HttpResponseMessage> PutAsync<T>(
            string url,
            T request)
        {
            var client = CreateClient();

            string json = JsonSerializer.Serialize(request);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            return await client.PutAsync(url, content);
        }

        // Send a DELETE request.
        public async Task<HttpResponseMessage> DeleteAsync(
            string url)
        {
            var client = CreateClient();

            return await client.DeleteAsync(url);
        }

        // Send a file to the API.
        public async Task<HttpResponseMessage> PostFileAsync(
            string url,
            IFormFile file,
            string fieldName)
        {
            var client = CreateClient();

            using var content = new MultipartFormDataContent();

            using var stream =
                file.OpenReadStream();

            using var fileContent =
                new StreamContent(stream);

            fileContent.Headers.ContentType =
                new MediaTypeHeaderValue(
                    file.ContentType);

            content.Add(
                fileContent,
                fieldName,
                file.FileName);

            return await client.PostAsync(
                url,
                content);
        }
    }
}