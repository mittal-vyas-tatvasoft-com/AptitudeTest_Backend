namespace AptitudeTest.Core.ViewModels
{
    public class ApiResponseVm<T>
    {
        public bool Result { get; set; }

        public string? Message { get; set; }

        public int StatusCode { get; set; }

        public T? Data { get; set; }
    }
}
