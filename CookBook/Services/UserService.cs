using AutoMapper;
using AutoMapper.QueryableExtensions;
using CookBook.Abstractions;
using CookBook.Contracts;
using CookBook.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Services;

public class UserService : IUserService
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IApplicationDbContext applicationDbContext,
        IMapper mapper,
        ILogger<UserService> logger
        )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task UpdateUserAsync(int id, UpdateUserDto dto)
    {
        var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
        {
            _logger.LogError("Couldn't get user with {UserId}. Not found.", id);
            throw new UserNotFoundException(id);
        }

        user.Login = dto.Login;

        await _applicationDbContext.SaveChangesAsync();

        _logger.LogInformation("Successfully updated user with {Id}", id);
    }

    public async Task DeleteUserAsync(int id)
    {
        var deletedUsersCount = await _applicationDbContext.Users
            .Where(u => u.Id == id)
            .ExecuteDeleteAsync();

        if (deletedUsersCount == 0)
        {
            _logger.LogError("Couldn't get user with {UserId}. Not found.", id);
            throw new UserNotFoundException(id);
        }

        _logger.LogInformation("Successfully deleted user with {Id}", id);
    }

    public async Task<UserVm> GetUserAsync(int id)
    {
        var user = await _applicationDbContext.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .ProjectTo<UserVm>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            _logger.LogError("Couldn't get user with {UserId}. Not found.", id);
            throw new UserNotFoundException(id);
        }

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
