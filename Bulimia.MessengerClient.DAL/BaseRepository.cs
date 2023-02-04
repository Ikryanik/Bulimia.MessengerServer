using System.Net.Http;

namespace Bulimia.MessengerClient.DAL
{
    public static class BaseRepository
    {
        public static readonly HttpClient Client = new HttpClient();
    }
}