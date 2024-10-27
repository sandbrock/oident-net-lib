namespace OIdentNetLib.Infrastructure.Database.Contracts;

public interface IUserReader
{
    Task<User?> ReadById(Guid userId);
    Task<User?> ReadByEmail(string email);
    Task<User?> ReadByUsername(string username);
}