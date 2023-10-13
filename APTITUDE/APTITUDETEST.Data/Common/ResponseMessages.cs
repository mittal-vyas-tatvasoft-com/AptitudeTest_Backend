namespace AptitudeTest.Data.Common
{
    public class ResponseMessages
    {

        #region Common
        public static string InternalError = "Internal Server Error";
        public static string Success = "Operation Success";
        public static string BadRequest = "Invalid Request";

        public static string NotFound = "{0} does not exists";
        public static string AlreadyExists = "{0} already exists";
        public static string AddSuccess = "{0} Added successfully";
        public static string UpdateSuccess = "{0} updated successfully";
        public static string DeleteSuccess = "{0} deleted successfully";
        public static string NotEditable = "{0}  not editable";
        public static string Addfailed = "{0}  add failed";
        #endregion

        #region UserAuthentication
        public static string LoginSuccess = "Login done successfully";
        public static string SessionRefresh = "Session has been refressed successfully";
        public static string PasswordUpdatedSuccess = "Password has been changed successfully";
        public static string InvalidCredetials = "Please enter valid credetials";
        public static string MailSentForForgetPassword = "Your password reset request has been granted and we sent you mail for new password!!";
        public static string TokenExpired = "Token has been expired please login again";
        public static string TokenInvalid = "token is invalid";
        #endregion


    }
}
