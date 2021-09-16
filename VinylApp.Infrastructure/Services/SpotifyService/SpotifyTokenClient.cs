using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VinylApp.Domain.Models.VinylAppModels.UserAggregate.Auth;

namespace VinylApp.Infrastructure.Services.SpotifyService
{
    public class SpotifyTokenClient : ISpotifyTokenClient
    {
        private readonly ILogger<SpotifyTokenClient> _logger;
        private readonly HttpClient _client;
        private readonly string _credentials;
        private readonly string _baseSpotifyUrl;

        public SpotifyTokenClient(IConfiguration config, ILogger<SpotifyTokenClient> logger)
        {
            _client = new HttpClient();
            _logger = logger;
            _credentials = $"{config.GetSection("Client_Id").Value}:{config.GetSection("Client_Secret").Value}";
            _baseSpotifyUrl = config.GetSection("Spotify_Token_URL").Value;
        }

        public async Task<SpotifyToken> RetrieveToken()
        {
            SetHeaders();
            using (_client)
            {
                _logger.LogInformation("Retrieving token!");
                var request = await _client.PostAsync(_baseSpotifyUrl, UrlEncodeRequestBody());
                var response = await request.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<SpotifyToken>(response);
                return token;
            }   
        }

        private void SetHeaders()
        {
            _logger.LogInformation("Setting headers!");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(_credentials)));
        }

        private static FormUrlEncodedContent UrlEncodeRequestBody()
        {
            List<KeyValuePair<string, string>> requestData = new()
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            };
            FormUrlEncodedContent requestBody = new(requestData);
            return requestBody;
        }
    }
}