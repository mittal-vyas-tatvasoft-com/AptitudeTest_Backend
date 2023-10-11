namespace AptitudeTest.Data.Common
{
    public class ResponseStatusCode
    {
        public static int Success = 200;
        public static int OK = 200;
        public static int NotFound = 404;
        public static int AlreadyExist = 409;
        public static int BadRequest = 400;
        public static int RequestFailed = 400;
        public static int InternalServerError = 500;
        public static int Unauthorized = 401;
        public static int Forbidden = 403;
        public static int MethodNotAllowed = 405;
        public static int NotAcceptable = 406;
        public static int RequestTimeout = 408;
        public static int Conflict = 409;
    }
}
