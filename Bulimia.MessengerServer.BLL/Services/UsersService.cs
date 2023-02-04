using Bulimia.MessengerServer.DAL.Repositories;
using Bulimia.MessengerServer.Domain.Core;

namespace Bulimia.MessengerServer.BLL.Services;

public class UsersService
{
    private readonly UserRepository _userRepository;

    public UsersService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserRecord>> GetUsers()
    {
        var userDtos = await _userRepository.GetUsers();

        return userDtos.Select(x =>
                new UserRecord
                {
                    Id = x.Id,
                    Username = x.Username
                }
            ).ToList();
    }

    public async Task<string?> GetUsernameById(int id)
    {
        var result = await _userRepository.GetUserById(id);

        return result?.Username;
    }
}