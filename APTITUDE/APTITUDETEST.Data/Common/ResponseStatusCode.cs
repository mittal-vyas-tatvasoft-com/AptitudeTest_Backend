namespace AptitudeTest.Data.Common
{
    public static class ResponseStatusCode
    {
        public readonly static int Success = 200;
        public readonly static int OK = 200;
        public readonly static int NotFound = 404;
        public readonly static int AlreadyExist = 409;
        public readonly static int BadRequest = 400;
        public readonly static int RequestFailed = 400;
        public readonly static int InternalServerError = 500;
        public readonly static int Unauthorized = 401;
        public readonly static int Forbidden = 403;
        public readonly static int MethodNotAllowed = 405;
        public readonly static int NotAcceptable = 406;
        public readonly static int RequestTimeout = 408;
        public readonly static int Conflict = 409;
    }
}
