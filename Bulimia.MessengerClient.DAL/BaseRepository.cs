using System;
using System.Net.Http;
using System.Threading;

namespace Bulimia.MessengerClient.DAL
{
    public static class BaseRepository
    {
        public static readonly HttpClient Client = new HttpClient(
            new HttpClientHandler
            {
                MaxConnectionsPerServer = 16
            })
        {
            Timeout = TimeSpan.FromMinutes(1)
        };
    }
}