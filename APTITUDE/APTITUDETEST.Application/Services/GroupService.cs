using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
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

        public async Task<JsonResult> Create(GroupsQueryVM groupToBeAdded)
        {
            return await _repository.Create(groupToBeAdded);
        }

        public async Task<JsonResult> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<JsonResult> GetGroups(string? searchGroup, int? collegeId)
        {
            return await _repository.GetGroups(searchGroup, collegeId);
        }

        public async Task<JsonResult> Update(GroupsQueryVM groupToBeRenamed)
        {
            return await _repository.Update(groupToBeRenamed);
        }
        #endregion
    }
}
