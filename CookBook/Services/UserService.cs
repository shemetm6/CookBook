using CookBook.Contracts;
using CookBook.Abstractions;
using CookBook.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

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

    public async Task UpdateUserAsync(int id, UpdateUserDto dto)
    {
        var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            throw new UserNotFoundException(id);

        user.Login = dto.Login;

        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var deletedUsersCount = await _applicationDbContext.Users
            .Where(u => u.Id == id)
            .ExecuteDeleteAsync();

        if (deletedUsersCount == 0)
            throw new UserNotFoundException(id);
    }

    public async Task<UserVm> GetUserAsync(int id)
    {
        var user = await _applicationDbContext.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .ProjectTo<UserVm>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (user is null)
            throw new UserNotFoundException(id);

        return user;
    }

    public async Task<ListOfUsers> GetUsersAsync()
    {
        var users = await _applicationDbContext.Users
            .AsNoTracking()
            .OrderBy(r => r.Id)
            .ToListAsync();

        return _mapper.Map<ListOfUsers>(users);
    }
}
