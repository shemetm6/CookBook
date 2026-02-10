using CookBook.Contracts;

namespace CookBook.Abstractions;

public interface IUserService
{
    Task UpdateUserAsync(int id, UpdateUserDto dto);
    Task DeleteUserAsync(int id);
    Task<UserVm> GetUserAsync(int id);
    Task<ListOfUsers> GetUsersAsync();
}
