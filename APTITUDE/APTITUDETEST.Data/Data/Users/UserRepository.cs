using AptitudeTest.Core.Interfaces.Users;
using APTITUDETEST.Common.Data;
using APTITUDETEST.Core.Entities.Users;

namespace AptitudeTest.Data.Data.Users
{
    public class UserRepository : RepositoryBase<User>, IUsersRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }
    }
}
