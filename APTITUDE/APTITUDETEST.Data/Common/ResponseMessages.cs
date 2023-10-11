﻿namespace AptitudeTest.Data.Common
{
    public class ResponseMessages
    {

        #region Common
        public static string InternalError = "Internal Server Error";
        public static string Success = "Operation Success";
        public static string BadRequest = "Invalid Request";
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

        #region Master
        #region College
        public static string CollegeNotFound = "College does not exists";
        public static string CollegeAlreadyExists = "College Or Abbreviation already exists";
        public static string CollegeAddSuccess = "College Added successfully";
        public static string CollegeUpdateSuccess = "College updated successfully";
        public static string CollegeDeleteSuccess = "College deleted successfully";
        #endregion


        #region Location
        public static string LocationNotFound = "Location does not exists";
        public static string LocationAlreadyExists = "Location already exists";
        public static string LocationAddSuccess = "Location Added successfully";
        public static string LocationUpdateSuccess = "Location updated successfully";
        public static string LocationDeleteSuccess = "Location deleted successfully";
        #endregion

        #region Degree
        public static string DegreeNotFound = "Degree does not exists";
        public static string DegreeAlreadyExists = "Degree already exists";
        public static string DegreeAddSuccess = "Degree Added successfully";
        public static string DegreeUpdateSuccess = "Degree updated successfully";
        public static string DegreeDeleteSuccess = "Degree deleted successfully";
        public static string DegreeNotEditable = "Degree not editable";
        #endregion

        #region Stream
        public static string StreamNotFound = "Stream does not exists";
        public static string StreamAlreadyExists = "Stream already exists";
        public static string StreamAddSuccess = "Stream Added successfully";
        public static string StreamUpdateSuccess = "Stream updated successfully";
        public static string StreamDeleteSuccess = "Stream deleted successfully";
        public static string StreamNotEditable = "Stream not editable";
        #endregion

        #region Technology
        public static string TechnologyNotFound = "Technology does not exists";
        public static string TechnologyAlreadyExists = "Technology already exists";
        public static string TechnologyAddSuccess = "Technology Added successfully";
        public static string TechnologyUpdateSuccess = "Technology updated successfully";
        public static string TechnologyDeleteSuccess = "Technology deleted successfully";
        public static string TechnologyNotEditable = "Technology not editable";
        #endregion

        #endregion

    }
}
