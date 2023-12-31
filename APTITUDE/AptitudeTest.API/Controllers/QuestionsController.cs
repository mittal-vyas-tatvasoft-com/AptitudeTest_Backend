﻿using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionsController : ControllerBase
    {
        #region Properties
        private readonly IQuestionService _service;
        #endregion

        #region Constructor
        public QuestionsController(IQuestionService service)
        {
            _service = service;
        }
        #endregion

        #region Methods

        /// <summary>
        /// This method gives Question from its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id:int}")]
        public async Task<JsonResult> Get(int id)
        {
            return await _service.Get(id);
        }

        /// <summary>
        /// This method gives all questions with filter of topic and status
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetQuestions(int? topic, bool? status, int pageSize, int pageIndex)
        {
            return await _service.GetQuestions(topic, status, pageSize, pageIndex);
        }

        /// <summary>
        /// This method gives count of questions in different topics
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<JsonResult> GetQuestionCount(int? topic, bool? status)
        {
            if (ModelState.IsValid)
            {
                return await _service.GetQuestionCount(topic, status);
            }
            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        /// <summary>
        /// This method Adds new College
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<JsonResult> Create([FromForm] QuestionVM question)
        {
            if (ModelState.IsValid)
            {
                return await _service.Create(question);
            }
            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        /// <summary>
        /// This method updates Question
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> Update([FromForm] QuestionVM question)
        {
            if (ModelState.IsValid)
            {
                return await _service.Update(question);
            }
            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });

        }

        /// <summary>
        /// This method  Updates Question Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> UpdateStatus(StatusVM status)
        {
            if (ModelState.IsValid)
            {
                return await _service.UpdateStatus(status);
            }
            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });

        }

        /// <summary>
        /// This method soft deletes Question
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<JsonResult> Delete(int id)
        {
            return await _service.Delete(id);
        }

        /// <summary>
        /// This method imports Questions
        /// </summary>
        /// <param name="importQuestionVM"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<JsonResult> ImportQuestions([FromForm] ImportQuestionVM importQuestionVM)
        {
            return await _service.ImportQuestions(importQuestionVM);
        }

        /// <summary>
        /// This method updates Status of list of questions 
        /// </summary>
        /// <param name="bulkStatusUpdateVM"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> UpdateBulkStatus(BulkStatusUpdateVM bulkStatusUpdateVM)
        {
            return await _service.UpdateBulkStatus(bulkStatusUpdateVM);
        }

        /// <summary>
        /// This method deletes questions in bulk
        /// </summary>
        /// <param name="questionIdList"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> DeleteMultipleQuestions(int[] questionIdList)
        {
            return await _service.DeleteMultipleQuestions(questionIdList);
        }
        #endregion
    }
}
