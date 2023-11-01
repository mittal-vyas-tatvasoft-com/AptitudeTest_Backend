using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IGroupRepository
    {
        public Task<JsonResult> GetActiveGroups();
        public Task<JsonResult> GetGroups(string? searchGroup);
        public Task<JsonResult> Create(GroupsQueryVM groupToBeAdded);
        public Task<JsonResult> Rename(GroupsQueryVM groupToBeRenamed);
        public Task<JsonResult> Delete(int id);
    }
}
