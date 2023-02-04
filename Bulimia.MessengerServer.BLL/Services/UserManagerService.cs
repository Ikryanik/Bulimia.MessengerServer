using Bulimia.MessengerServer.DAL.Dtos;
using Bulimia.MessengerServer.DAL.Repositories;
using Bulimia.MessengerServer.Domain.Core;

namespace Bulimia.MessengerServer.BLL.Services;

public class UserManagerService
{
    private readonly UserRepository _userRepository;

    public UserManagerService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest authenticateRequest)
    {
        var result = await _userRepository.GetUserByUsername(authenticateRequest.Username);

        if (result == null)
            throw new Exception("Пользователь не найден");

        return Map(result);
    }
    public async Task<RegistrationResponse> Register(RegistrationRequest registrationRequest)
    {
        var resultOfSearch = await _userRepository.GetUserByUsername(registrationRequest.Username);

        if (resultOfSearch != null)
            throw new Exception("Пользователь с таким именем уже существует");

        var resultOfCreating = await _userRepository.CreateUser(registrationRequest);

        return MapRegistrationResponce(resultOfCreating);
    }

    public AuthenticateResponse Map(UserDto userDto)
    {
        return new AuthenticateResponse
        {
            Id = userDto.Id
        };
    }
    public RegistrationResponse MapRegistrationResponce(UserDto userDto)
    {
        return new RegistrationResponse
        {
            Id = userDto.Id
        };
    }

 
}