﻿using System;
using System.IO;
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
        public static HttpClient HttpClient => _lazyInstance.Value;

        static Http()
        {
            HttpClient.DefaultRequestHeaders.ConnectionClose = false;
            HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
        }

        public static async Task<string> Get(string url)
        {
            //Logging.Log.Debug($"HTTP GET: {url}");
            try
            {
                var response = await HttpClient.GetAsync(url);
                var a = response.IsSuccessStatusCode;
                //response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (HttpRequestException e)
            {
                throw new SyrusException($"HTTP GET {url} throws exception.", e);
            }
        }

        public static async Task<string> Get(string url, HttpStatusCode ignoredStatusCodes)
        {
            //Logging.Log.Debug($"HTTP GET: {url}");
            HttpResponseMessage response;
            try
            {
                response = await HttpClient.GetAsync(url);
                if(!response.IsSuccessStatusCode && (response.StatusCode & ignoredStatusCodes) == 0)
                {
                    response.EnsureSuccessStatusCode(); // throw exception
                }
                return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
            }
            catch (HttpRequestException e)
            {
                throw new SyrusException($"HTTP GET {url} throws exception.", e);
            }
        }
    }
}
