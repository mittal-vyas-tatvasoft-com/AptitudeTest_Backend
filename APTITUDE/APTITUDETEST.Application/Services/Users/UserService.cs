using AptitudeTest.Core.Interfaces.Users;
using APTITUDETEST.Core.Entities.Users;

namespace AptitudeTest.Application.Services.Users
{
    public class UserService : IUserService
    {
        #region Properties
        private readonly IUsersRepository _userRepository;
        #endregion

        #region Constructor
        public UserService(IUsersRepository userepo)
        {
            _userRepository = userepo;
        }
        #endregion

        #region Methods
        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.GetById(id);

        }
        #endregion
    }
}
