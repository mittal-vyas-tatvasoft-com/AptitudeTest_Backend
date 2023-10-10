namespace AptitudeTest.Core.ViewModels
{
    public class TokenVm
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
