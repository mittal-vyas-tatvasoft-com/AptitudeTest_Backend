﻿using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class ProfileService : IProfileService
    {
        #region Properties
        private readonly IProfileRepository _repository;
        #endregion

        #region Constructor
        public ProfileService(IProfileRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Methods

        public async Task<JsonResult> GetProfiles(string? searchQuery, int? filter, int? currentPageIndex, int? pageSize)
        {
            return await _repository.GetProfiles(searchQuery, filter, currentPageIndex, pageSize);
        }

        public async Task<JsonResult> Create(ProfileVM profile)
        {
            return await _repository.Create(profile);
        }

        public async Task<JsonResult> Update(ProfileVM profile)
        {
            return await _repository.Update(profile);
        }
        public async Task<JsonResult> CheckUncheckAll(bool check)
        {
            return await _repository.CheckUncheckAll(check);
        }

        public async Task<JsonResult> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<JsonResult> GetProfileById(int? id)
        {
            return await _repository.GetProfileById(id);
        }

        public async Task<JsonResult> UpdateStatus(StatusVM status)
        {
            return await _repository.UpdateStatus(status);
        }
        #endregion
    }
}