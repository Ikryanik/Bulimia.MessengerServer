using Bulimia.MessengerServer.DAL.Dtos;
using Bulimia.MessengerServer.DAL.Models;
using Bulimia.MessengerServer.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bulimia.MessengerServer.DAL.Repositories;

public class UserRepository
{
    private readonly MessengerContext _context;

    public UserRepository(MessengerContext context)
    {
        _context = context;
    }

    public async Task<List<UserDto>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();

        return users.Select(x => Map(x)).ToList();
    }

    public async Task<UserDto?> GetUserById(int userId)
    {
        var result = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

        return result == null 
            ? null 
            : Map(result);
    }

    public async Task<UserDto?> GetUserByUsername(string username)
    {
        var result = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

        return result == null 
            ? null 
            : Map(result);
    }
    
    public async Task<UserDto> CreateUser(RegistrationRequest registrationRequest)
    {
        var result = await _context.Users.AddAsync(Map(registrationRequest));
        await _context.SaveChangesAsync();

        return Map(result);
    }

    private UserDto Map(EntityEntry<User> userModel)
    {
        var model = userModel.Entity;

        return new UserDto
        {
            Id = model.Id,
            Username = model.Username
        };
    }

    private User Map(RegistrationRequest registrationRequest)
    {
        return new User
        {
            Username = registrationRequest.Username
        };
    }

    private UserDto Map(User user)
    {
        return new UserDto
        {
            Id = user.Id, Username = user.Username
        };
    }

}