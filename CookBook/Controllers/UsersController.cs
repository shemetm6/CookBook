using CookBook.Abstractions;
using CookBook.Contracts;
using CookBook.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CookBook.Controllers;

public class UsersController : BaseController
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public UsersController(
        IUserService userService,
        IAuthService authService
        )
    {
        _userService = userService;
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<ActionResult<LogInResponse>> SignUp([FromBody] SignUpDto dto)
    {
        var token = await _authService.SignUpAsync(dto);

        return Ok(token);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LogInResponse>> LogIn([FromBody] LogInDto dto)
    {
        var token = await _authService.LogInAsync(dto);

        if (token is null)
            return NotFound();

        return Ok(token);
    }

    [HttpPost("logout")]
    public async Task<ActionResult<bool>> LogOut([FromBody] int id)
    {
        var result = await _authService.LogOutAsync(id);

        if (!result)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<LogInResponse>> Refresh([FromBody] string refreshToken)
    {
        var result = await _authService.RefreshAsync(refreshToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("revoke")]
    public async Task<ActionResult> Revoke([FromBody] string refreshToken)
    {
        await _authService.RevokeAsync(refreshToken);

        return NoContent();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(UpdateUserDto dto)
    {
        var id = HttpContext.ExtractUserIdFromClaims();

        if (id is null)
            return Unauthorized();

        await _userService.UpdateUserAsync(id.Value, dto);

        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUser()
    {
        var id = HttpContext.ExtractUserIdFromClaims();

        if (id is null)
            return Unauthorized();

        await _userService.DeleteUserAsync(id.Value);

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<ListOfUsers>> GetUsers()
    {
        var users = await _userService.GetUsersAsync();

        return Ok(users);
    }
        
    [HttpGet("{id}")]
    public async Task<ActionResult<UserVm>> GetUser(int id)
    {
        var user = await _userService.GetUserAsync(id);

        return Ok(user);
    }
}
