using CookBook.Abstractions;
using Microsoft.AspNetCore.Mvc;
using CookBook.Contracts;

namespace CookBook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
        => _userService = userService;

    [HttpPost]
    public ActionResult<int> AddUser(CreateUserDto dto)
    {
        var id = _userService.AddUser(dto);

        return Ok(id);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateUser(int id, UpdateUserDto dto)
    {
        _userService.UpdateUser(id, dto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteUser(int id)
    {
        _userService.DeleteUser(id);

        return NoContent();
    }

    [HttpGet]
    public ActionResult<ListOfUsers> GetUsers()
        => Ok(_userService.GetUsers());

    [HttpGet("{id}")]
    public ActionResult<UserVm> GetUser(int id)
        => Ok(_userService.GetUser(id));
}
