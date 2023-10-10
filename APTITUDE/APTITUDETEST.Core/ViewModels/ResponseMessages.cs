namespace AptitudeTest.Core.ViewModels
{
    public static class ResponseMessages
    {
        #region UserAuthentication
        public static string LoginSuccess = "Login done successfully";
        public static string SessionRefresh = "Session has been refressed successfully";
        public static string InvalidCredetials = "Please enter valid credetials";
        public static string MailSentForForgetPassword = "Your password reset request has been granted and we sent you mail for new password!!";
        #endregion

        #region Common
        public static string BadRequest = "Invalid payload data";
        public static string InternalServerError = "Internal server error";
        public static string TokenExpired = "Token has been expired please login again";
        public static string TokenInvalid = "token is invalid";
        #endregion
    }
}
