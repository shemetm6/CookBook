using AutoMapper;
using CookBook.Abstractions;
using CookBook.Contracts;
using Microsoft.EntityFrameworkCore;
using CookBook.Models;
using CookBook.Exceptions;

namespace CookBook.Services;

public class UserService : IUserService
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public UserService(
        IApplicationDbContext applicationDbContext,
        IMapper mapper
        )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public int AddUser(CreateUserDto dto)
    {
        var user = _mapper.Map<User>(dto);

        _applicationDbContext.Users.Add(user);

        _applicationDbContext.SaveChanges();

        return user.Id;
    }

    public void UpdateUser(int id, UpdateUserDto dto)
    {
        var user = _applicationDbContext.Users.FirstOrDefault(u => u.Id == id);

        if (user is null)
            throw new UserNotFoundException(id);

        // Переписал с примера в pdf'ке про СУБД и не понимаю с чего dto.Login может быть null.
        // В контракте мы это свойство не делали nullable.
        user.Login = dto.Login ?? user.Login;

        _applicationDbContext.SaveChanges();
    }

    public void DeleteUser(int id)
    {
        var deletedUsersCount = _applicationDbContext.Users
            .Where(u => u.Id == id)
            .ExecuteDelete();

        if (deletedUsersCount == 0)
            throw new UserNotFoundException(id);
    }

    public UserVm GetUser(int id)
    {
        var user = _applicationDbContext.Users
            .AsNoTracking()
            .Include(u => u.Recipes.OrderBy(r => r.Id))
            .FirstOrDefault(u => u.Id == id);

        if (user is null)
            throw new UserNotFoundException(id);

        return _mapper.Map<UserVm>(user);
    }

    public ListOfUsers GetUsers()
        => _mapper.Map<ListOfUsers>(_applicationDbContext.Users
            .AsNoTracking()
            .OrderBy(r => r.Id)
            .ToList());
}
