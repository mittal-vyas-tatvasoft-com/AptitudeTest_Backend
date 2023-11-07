using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class ChangePasswordVM
    {
        [Required]
        [RegularExpression(@"^\w+([\\.-]?\w+)*@\w+([\\.-]?\w+)*(\.\w{2,3})+$", ErrorMessage = "Please enter valid email.")]
        public string Email { get; set; }
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!^#%*?&])[A-Za-z\d@$!^#%*?&]{8,}$", ErrorMessage = "Your password must be atleast 8 characters long and it should contain one number, one uppercase, one lowercase letters and one special charactor.")]
        public string NewPassword { get; set; }
        [Required]
        public string confirmPassword { get; set; }
    }
}
