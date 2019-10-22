using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Syrus.Shared.Http
{
    public static class Http
    {
        private const string UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36";
        private static readonly HttpClientHandler _clientHandler = new HttpClientHandler();
        
        private static readonly Lazy<HttpClient> _lazyInstance = new Lazy<HttpClient>(() => new HttpClient(_clientHandler, false));
        private static HttpClient HttpClient => _lazyInstance.Value;

        static Http()
        {
            HttpClient.DefaultRequestHeaders.ConnectionClose = false;
            HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
        }

        public static async Task<string> Get(string url)
        {
            Logging.Log.Debug($"HTTP GET: {url}");
            try
            {
                var response = await HttpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (HttpRequestException e)
            {
                throw new SyrusException($"HTTP GET {url} throws exception.", e);
            }
        }
    }
}
