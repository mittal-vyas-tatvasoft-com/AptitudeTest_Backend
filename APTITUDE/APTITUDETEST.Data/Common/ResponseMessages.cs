namespace AptitudeTest.Data.Common
{
    public static class ResponseMessages
    {

        #region Common
        public readonly static string InternalError = "Internal Server Error";
        public readonly static string Success = "Operation Success";
        public readonly static string BadRequest = "Invalid Request";
        public readonly static string InsertProperData = "Please insert proper data according requirement";
        public readonly static string InsertSomeData = "Please insert some data";
        public readonly static string InvalidAnswerSelection = "Invalid Answers";
        public readonly static string InvalidTopics = "Invalid Topics";
        public readonly static string NoChanges = "No Changes were made";
        public readonly static string InvalidFormat = "Please insert data in valid format";
        public readonly static string InvalidOptions = "Two options can not be same";

        public readonly static string NotFound = "{0} does not exists";
        public readonly static string AlreadyExists = "{0} already exists";
        public readonly static string AddSuccess = "{0} Added successfully";
        public readonly static string RegisterSuccess = "{0} Registerd successfully, Password has been sent to your registerd email";
        public readonly static string UpdateSuccess = "{0} updated successfully";
        public readonly static string StatusUpdateSuccess = "{0} status updated successfully";
        public readonly static string passwordNotMatched = "New Password and Confirm Password doesn't match";
        public readonly static string currentAndNewSame = "Current Password and New Password can't be same";
        public readonly static string DeleteSuccess = "{0} deleted successfully";
        public readonly static string NotEditable = "{0} not editable";
        public readonly static string NoRecordsFound = "No Record Found";
        #endregion

        #region UserAuthentication
        public readonly static string LoginSuccess = "Login done successfully";
        public readonly static string SessionRefresh = "Session has been refressed successfully";
        public readonly static string PasswordUpdatedSuccess = "Password has been changed successfully";
        public readonly static string InvalidCredentials = "Please enter valid credentials";
        public readonly static string UserDoesNotExist = "User does not exist";
        public readonly static string InActiveAccount = "This Acoount is InActive";
        public readonly static string MailSentForForgetPassword = "Your password reset request has been granted and we sent you mail for new password!!";
        public readonly static string TokenExpired = "Token has been expired please login again";
        public readonly static string TokenInvalid = "token is invalid";
        #endregion

        #region Test
        public readonly static string NotEnoughQuestion = "There is not enough {0} mark questions of {1} in database";
        public readonly static string TestTopicAlreadyExists = "Questions for this topic already exists";
        public readonly static string NoOfQuestions = "Please enter correct no of questions";
        public readonly static string TestTopicQuestionsNotFound = "Questions for this topic does not found for this test";
        public readonly static string TestWithSameName = "Test with same name exists";
        public readonly static string TestAlreadySubmitted = "Your test is already submitted";
        public readonly static string CantChangeStatusBecauseActive = "Your can't change status of the test because it is currently active...!";
        public readonly static string CantChangeGroupBecauseActive = "Your can't change group of the test because it is currently active...!";
        public readonly static string CantUpdateTestBecauseActive = "Your can't update data of the test because it is currently active...!";
        public readonly static string CantAddQuestionsBecauseActive = "Your can't add questions of the test because it is currently active...!";
        public readonly static string CanDeleteQuestionsBecauseActive = "Your can't delete questions of the test because it is currently active...!";
        public readonly static string CanDeleteTestBecauseActive = "Your can't test questions of the test because it is currently active...!";
        #endregion

        #region AdminAuthentication
        public readonly static string SuperAdminRequestFail = "You can't perform {0} for this user";
        #endregion

        #region Candidate
        public readonly static string InternalErrorForAddingQuestionsToTest = "Internal error for adding questions to test";
        public readonly static string EndTest = "End test successfully";
        #endregion

    }
}
