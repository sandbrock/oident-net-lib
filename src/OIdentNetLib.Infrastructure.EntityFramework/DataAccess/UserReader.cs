using OIdentNetLib.Infrastructure.Database;
using OIdentNetLib.Infrastructure.Database.Contracts;

namespace OIdentNetLib.Infrastructure.EntityFramework.DataAccess;

public class UserReader : IUserReader
{
    public async Task<User?> ReadById(Guid userId)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task<User?> ReadByEmail(string email)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task<User?> ReadByUsername(string username)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}