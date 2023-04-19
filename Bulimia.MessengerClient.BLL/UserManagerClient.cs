using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulimia.MessengerClient.DAL.Repositories;
using Bulimia.MessengerClient.Domain.Core;

namespace Bulimia.MessengerClient.BLL
{
    public class UserManagerClient
    {
        private readonly UserRepository _userRepository;

        public UserManagerClient()
        {
            _userRepository = new UserRepository();
        }

        public async Task<AuthenticateResponce> Authenticate(string username)
        {
            var request = new AuthenticateRequest
            {
                Username = username
            };

            var result = await ExecutionService.Execute(() => _userRepository.Authenticate(request));

            return result == null ? null : Map(result);
        }

        public async Task<RegisterResponce> Register(string username)
        {
            var request = new RegisterRequest
            {
                Username = username
            };

            var result = await ExecutionService.Execute(() => _userRepository.Register(request));

            return result == null ? null : MapResponse(result);
        }

        public AuthenticateResponce Map(UserDto userDto)
        {
            return new AuthenticateResponce
            {
                Id = userDto.Id
            };
        }

        public RegisterResponce MapResponse(UserDto userDto)
        {
            return new RegisterResponce
            {
                Id = userDto.Id
            };
        }
    }
}

