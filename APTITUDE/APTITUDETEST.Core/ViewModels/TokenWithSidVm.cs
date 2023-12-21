namespace AptitudeTest.Core.ViewModels
{
    public class TokenWithSidVm
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string? Sid { get; set; }
        public bool? IsSubmitted { get; set; }
    }
}
