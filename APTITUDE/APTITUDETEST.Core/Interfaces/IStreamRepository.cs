﻿using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IStreamRepository
    {
        public Task<JsonResult> GetAllActiveStreams();
        public Task<JsonResult> Getstreams(string? searchQuery, int? filter, List<int>? degreelist, int? currentPageIndex, int? pageSize);
        public Task<JsonResult> Create(StreamVM stream);
        public Task<JsonResult> Update(StreamVM stream);

        public Task<JsonResult> CheckUncheckAll(bool check);

        public Task<JsonResult> Delete(int id);
    }
}
