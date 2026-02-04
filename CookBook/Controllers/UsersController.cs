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
    public ActionResult<LogInResponse> SignUp([FromBody] SignUpDto dto)
    {
        var token = _authService.SignUp(dto);

        return Ok(token);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<LogInResponse> LogIn([FromBody] LogInDto dto)
    {
        var token = _authService.LogIn(dto);

        if (token is null)
            return NotFound();

        return Ok(token);
    }

    [HttpPost("logout")]
    public ActionResult<bool> LogOut([FromBody] int id)
    {
        var result = _authService.LogOut(id);

        if (!result)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("refresh")]
    public ActionResult<LogInResponse> Refresh([FromBody] string refreshToken)
    {
        var result = _authService.Refresh(refreshToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("revoke")]
    public ActionResult Revoke([FromBody] string refreshToken)
    {
        _authService.Revoke(refreshToken);

        return NoContent();
    }

    [HttpPut]
    public ActionResult UpdateUser(UpdateUserDto dto)
    {
        var id = HttpContext.ExtractUserIdFromClaims();

        if (id is null)
            return Unauthorized();

        _userService.UpdateUser(id.Value, dto);

        return NoContent();
    }

    [HttpDelete]
    public ActionResult DeleteUser()
    {
        var id = HttpContext.ExtractUserIdFromClaims();

        if (id is null)
            return Unauthorized();

        _userService.DeleteUser(id.Value);

        return NoContent();
    }

    [HttpGet]
    public ActionResult<ListOfUsers> GetUsers()
        => Ok(_userService.GetUsers());

    [HttpGet("{id}")]
    public ActionResult<UserVm> GetUser(int id)
        => Ok(_userService.GetUser(id));
}
