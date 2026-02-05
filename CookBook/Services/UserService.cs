using CookBook.Contracts;
using CookBook.Abstractions;
using CookBook.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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

    public void UpdateUser(int id, UpdateUserDto dto)
    {
        var user = _applicationDbContext.Users.FirstOrDefault(u => u.Id == id);

        if (user is null)
            throw new UserNotFoundException(id);

        user.Login = dto.Login;

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
            .ThenInclude(r => r.Ratings)
            .Include(u => u.Ratings)
            .ThenInclude(rating => rating.Recipe)
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
