using CleanApp.Application.Auth.Commands;
using CleanApp.Presentation.Auth.Contracts;
using CleanApp.Presentation.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanApp.Presentation.Auth;

// Overrides the [Authorize] inherited from ApiControllerBase - registering and logging in
// obviously can't require you to already be logged in.
[AllowAnonymous]
[Route("api/auth")]
public sealed class AuthController(ISender sender) : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RegisterCommand(request.Email, request.Password), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new LoginCommand(request.Email, request.Password), cancellationToken);
        return HandleResult(result);
    }
}
