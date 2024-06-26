﻿using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class CandidateService : ICandidateService
    {
        #region Properties
        private readonly ICandidateRepository _candidateRepository;
        #endregion

        #region Constructor
        public CandidateService(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }
        #endregion

        #region Methods


        public async Task<JsonResult> GetTempUserTest(int userId)
        {
            return await _candidateRepository.GetTempUserTest(userId);
        }

        public async Task<JsonResult> GetCandidateTestQuestion(int questionId, int userId)
        {
            return await _candidateRepository.GetCandidateTestQuestion(questionId, userId);
        }

        public async Task<JsonResult> GetQuestionsStatus(int userId, bool isRefresh)
        {
            return await _candidateRepository.GetQuestionsStatus(userId, isRefresh);
        }

        public async Task<JsonResult> SaveTestQuestionAnswer(UpdateTestQuestionAnswerVM userTestQuestionAnswer)
        {
            return await _candidateRepository.SaveTestQuestionAnswer(userTestQuestionAnswer);
        }

        public async Task<JsonResult> GetInstructionsOfTheTestForUser(int userId, string testStatus)
        {
            return await _candidateRepository.GetInstructionsOfTheTestForUser(userId, testStatus);
        }

        public async Task<JsonResult> EndTest(int userId)
        {
            return await _candidateRepository.EndTest(userId);
        }

        public async Task<JsonResult> UpdateRemainingTime(UpdateTestTimeVM updateTestTimeVM)
        {
            return await _candidateRepository.UpdateRemainingTime(updateTestTimeVM);
        }

        public async Task<JsonResult> UpdateQuestionTimer(QuestionTimerVM questionTimerDetails)
        {
            return await _candidateRepository.UpdateQuestionTimer(questionTimerDetails);
        }

        public async Task<JsonResult> UpdateUserTestStatus(UpdateUserTestStatusVM updateUserTestStatusVM)
        {
            return await _candidateRepository.UpdateUserTestStatus(updateUserTestStatusVM);
        }
        #endregion
    }
}
