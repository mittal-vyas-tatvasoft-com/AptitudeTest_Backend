using APTITUDETEST.Core.Entities.Users;

namespace AptitudeTest.Core.Interfaces.Users
{
    public interface IUserService
    {
        Task<User> GetUserById(int id);

    }
}
