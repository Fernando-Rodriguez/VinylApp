using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VinylApp.Domain.DTOs.SpotifyDTOs;
using VinylApp.Domain.Models.VinylAppModels.AlbumAggregate;
using VinylApp.Domain.Models.VinylAppModels.UserAggregate;
using VinylApp.Domain.Models.VinylAppModels.UserAggregate.Auth;

namespace VinylApp.Infrastructure.Services.SpotifyService
{
    public class SpotifyRequestClient : ISpotifyRequestClient
    {
        private readonly ISpotifyTokenClient _tokenClient;
        private readonly ILogger<SpotifyRequestClient> _logger;
        private readonly string _spotifySearchUrl;
        private readonly HttpClient _client;
        private SpotifyToken _token;
        private HttpRequestMessage _request;

        public SpotifyRequestClient(IConfiguration config, ISpotifyTokenClient tokenClient, ILogger<SpotifyRequestClient> logger)
        {
            _spotifySearchUrl = config.GetSection("Spotify_Search_URL").Value;
            _tokenClient = tokenClient;
            _logger = logger;
            _client = new HttpClient();
        }

        public async Task<SpotifyAlbum> Search(AlbumItem album )
        {
            await GetToken();
            BuildRequestMessage(album.AlbumName);
            SetRequestHeaders();
            
            _logger.LogInformation($"Requesting album: {album.AlbumName}");
            var response = await _client.SendAsync(_request);
            var spotifyResponse = await response.Content.ReadAsStringAsync();
            var spotifyAlbum = JsonConvert.DeserializeObject<SpotifyAlbum>(spotifyResponse);
            return spotifyAlbum;
        }

        private async Task GetToken()
        {
            _token = await _tokenClient.RetrieveToken();
        }

        private string AlbumSearchUrl(string searchAlbum)
        {
            const string searchType = "album";
            var builder = new UriBuilder(_spotifySearchUrl)
            {
                Query = $"q={searchAlbum}&type={searchType}&limit=1"
            };
            return builder.ToString();
        }

        private void BuildRequestMessage(string album) => _request = new HttpRequestMessage(HttpMethod.Get, AlbumSearchUrl(album));
        private void SetRequestHeaders() => _request.Headers.Add("Authorization", $"Bearer {_token.AccessToken}");

    }
}

