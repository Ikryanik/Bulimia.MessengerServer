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
            return await ExecutionService.Execute(() => _userRepository.SearchUsers());
        }

        public async Task<string> GetUsernameById(int id)
        {
            return await _userRepository.GetUsernameById(id);
        }
    }
}