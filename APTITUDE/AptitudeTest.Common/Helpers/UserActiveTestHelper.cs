using AptitudeTest.Core.Entities.Test;
using APTITUDETEST.Common.Data;

namespace AptitudeTest.Common.Helpers
{
    public class UserActiveTestHelper
    {
        #region
        public enum TestStatus
        {
            Draft = 1,
            Active = 2,
            Completed = 3
        }
        #endregion

        #region Properties
        private readonly AppDbContext _context;
        #endregion

        #region Constructor
        public UserActiveTestHelper(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public Test? GetTestOfUser(int userId)
        {
            int? groupId = _context.Users.Where(x => x.Id == userId && x.IsDeleted == false).Select(x => x.GroupId).FirstOrDefault();
            if (groupId != null)
            {
                Test? test = _context.Tests.Where(x => x.GroupId == groupId && x.Status == (int)TestStatus.Active && x.IsDeleted == false).FirstOrDefault();
                if (test != null && Convert.ToDateTime(test?.EndTime) >= DateTime.Now && Convert.ToDateTime(test?.StartTime) <= DateTime.Now)
                {
                    return test;
                }
            }
            return null;
        }
        #endregion
    }
}
