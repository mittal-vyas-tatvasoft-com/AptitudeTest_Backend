﻿using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IGroupService
    {
        public Task<JsonResult> GetActiveGroups();
        public Task<JsonResult> GetGroups(string? searchGroup, int? collegeId);
        public Task<JsonResult> Create(GroupsQueryVM groupToBeAdded);
        public Task<JsonResult> Update(GroupsQueryVM updatedGroup);
        public Task<JsonResult> Delete(int id);
        public Task<JsonResult> DeleteMultipleGroups(List<int> groupIds);
    }
}
