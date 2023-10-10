namespace AptitudeTest.Core.ViewModels.Common
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}
