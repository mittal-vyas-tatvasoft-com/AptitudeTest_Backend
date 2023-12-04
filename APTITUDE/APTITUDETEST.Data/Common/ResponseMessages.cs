namespace AptitudeTest.Data.Common
{
    public class ResponseMessages
    {

        #region Common
        public static string InternalError = "Internal Server Error";
        public static string Success = "Operation Success";
        public static string BadRequest = "Invalid Request";
        public static string InsertProperData = "Please insert proper data according requirement";
        public static string InsertSomeData = "Please insert some data";
        public static string InvalidAnswerSelection = "Invalid Answers";
        public static string InvalidTopics = "Invalid Topics";
        public static string NoChanges = "No Changes were made";

        public static string NotFound = "{0} does not exists";
        public static string AlreadyExists = "{0} already exists";
        public static string AddSuccess = "{0} Added successfully";
        public static string RegisterSuccess = "{0} Registerd successfully, Password has been sent to your registerd email";
        public static string UpdateSuccess = "{0} updated successfully";
        public static string StatusUpdateSuccess = "{0} status updated successfully";
        public static string passwordNotMatched = "New Password and Confirm Password doesn't match";
        public static string currentAndNewSame = "Current Password and New Password can't be same";
        public static string DeleteSuccess = "{0} deleted successfully";
        public static string NotEditable = "{0} not editable";
        public static string NoRecordsFound = "No Record Found";
        #endregion

        #region UserAuthentication
        public static string LoginSuccess = "Login done successfully";
        public static string SessionRefresh = "Session has been refressed successfully";
        public static string PasswordUpdatedSuccess = "Password has been changed successfully";
        public static string InvalidCredentials = "Please enter valid credentials";
        public static string MailSentForForgetPassword = "Your password reset request has been granted and we sent you mail for new password!!";
        public static string TokenExpired = "Token has been expired please login again";
        public static string TokenInvalid = "token is invalid";
        #endregion

        #region Test
        public static string NotEnoughQuestion = "There is not enough {0} mark questions of {1} in database";
        public static string TestTopicAlreadyExists = "Questions for this topic already exists";
        public static string NoOfQuestions = "Please enter correct no of questions";
        public static string TestTopicQuestionsNotFound = "Questions for this topic does not found for this test";
        #endregion

        #region AdminAuthentication
        public static string SuperAdminRequestFail = "You can't perform {0} for this user";
        #endregion

        #region Candidate
        public static string InternalErrorForAddingQuestionsToTest = "Internal error for adding questions to test";
        public static string EndTest = "End test successfully";
        #endregion

    }
}
