using AptitudeTest.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class GroupService : IGroupService
    {
        #region Properties
        private readonly IGroupRepository _repository;
        #endregion

        #region Constructor
        public GroupService(IGroupRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> GetActiveGroups()
        {
            return await _repository.GetActiveGroups();
        }
        #endregion
    }
}
