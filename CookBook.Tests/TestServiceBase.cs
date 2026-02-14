using AutoMapper;
using CookBook.Abstractions;
using CookBook.Configurations.Mapping;
using CookBook.Database;
using CookBook.Services;
using Microsoft.Data.Sqlite;

namespace CookBook.Tests;

public class TestServiceBase : IDisposable
{
    protected readonly ApplicationDbContext Context;
    protected readonly IMapper Mapper;
    protected readonly ITimeConverter TimeConverter;
    private readonly SqliteConnection _connection;


    protected TestServiceBase()
    {
        (Context, _connection) = FakeApplicationDbContextFactory.Create();

        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(RecipeMappingProfile).Assembly));
        Mapper = config.CreateMapper();
        TimeConverter = new TimeConverter();
    }

    public void Dispose()
    {
        FakeApplicationDbContextFactory.Destroy(Context, _connection);
    }
}
