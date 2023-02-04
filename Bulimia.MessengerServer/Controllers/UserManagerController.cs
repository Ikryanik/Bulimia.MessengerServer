using Bulimia.MessengerServer.BLL.Services;
using Bulimia.MessengerServer.Domain.Core;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Bulimia.MessengerServer.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserManagerController
{
    private readonly UserManagerService _userManagerService;

    public UserManagerController(UserManagerService userManagerService)
    {
        _userManagerService = userManagerService;
    }

    [HttpPost(Name = "Authenticate")]
    public Task<AuthenticateResponse> Authenticate(AuthenticateRequest authenticateRequest)
    {
        return _userManagerService.Authenticate(authenticateRequest);
    }

    [HttpPost(Name = "Registration")]
    public Task<RegistrationResponse> Register(RegistrationRequest registrationRequest)
    {
        return _userManagerService.Register(registrationRequest);
    }
}