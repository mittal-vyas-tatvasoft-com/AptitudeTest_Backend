﻿
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface ICandidateRepository
    {
        Task<JsonResult> CreateUserTest(CreateUserTestVM userTest);
        Task<JsonResult> CreateTempUserTest(CreateTempUserTestVM tempUserTest);
    }
}