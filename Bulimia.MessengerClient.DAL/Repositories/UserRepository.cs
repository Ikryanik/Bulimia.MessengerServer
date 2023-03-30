using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bulimia.MessengerClient.Domain.Core;

namespace Bulimia.MessengerClient.DAL.Repositories
{
    public class UserRepository
    {
        public async Task<UserDto> Authenticate(AuthenticateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);

            var response = await BaseRepository.Client.PostAsync(Api.Authenticate, new StringContent(json, Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return null;

            var userDto = JsonConvert.DeserializeObject<UserDto>(content);

            return userDto;
        }

        public async Task<UserDto> Register(RegisterRequest request)
        {
            var json = JsonConvert.SerializeObject(request);

            var response = await BaseRepository.Client.PostAsync(Api.Register, new StringContent(json, Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return null;

            var userDto = JsonConvert.DeserializeObject<UserDto>(content);

            return userDto;
        }

        public async Task<List<UserModel>> SearchUsers()
        {
            try
            {
                var response = await BaseRepository.Client.GetAsync(Api.SearchUsers);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return null;

                var users = JsonConvert.DeserializeObject<List<UserModel>>(content);

                return users;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
    }
}
