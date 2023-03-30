using System.Collections.Generic;
using System.Threading.Tasks;
using Bulimia.MessengerClient.DAL.Repositories;
using Bulimia.MessengerClient.Domain.Core;

namespace Bulimia.MessengerClient.BLL
{
    public class UserClient
    {
        private readonly UserRepository _userRepository;

        public UserClient()
        {
            _userRepository = new UserRepository();
        }

        public async Task<List<UserModel>> SearchUsers()
        {
            return await _userRepository.SearchUsers();
        }
    }
}