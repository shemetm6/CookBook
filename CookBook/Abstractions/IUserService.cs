using CookBook.Contracts;

namespace CookBook.Abstractions;

public interface IUserService
{
    int AddUser(CreateUserDto dto);
    void UpdateUser(int id, UpdateUserDto dto);
    void DeleteUser(int id);
    UserVm GetUser(int id);
    ListOfUsers GetUsers();
}
