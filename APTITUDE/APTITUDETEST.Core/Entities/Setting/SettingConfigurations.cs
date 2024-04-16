using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.Entities.Setting
{
    public class SettingConfigurations
    {
        [Key]
        public int Id { get; set; }
        public bool UserRegistration { get; set; }
        public bool Camera { get; set; }
        public bool ClearResponseButton { get; set; }
        public bool ScreenCapture { get; set; }
        public int IntervalForScreenCapture { get; set; }
        public int CutOff { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
