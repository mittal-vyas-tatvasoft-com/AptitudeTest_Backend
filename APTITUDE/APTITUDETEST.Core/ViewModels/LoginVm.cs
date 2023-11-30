using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class LoginVm
    {
        [Required]
        [RegularExpression(@"^\w+([\\.-]?\w+)*@\w+([\\.-]?\w+)*(\.\w{2,3})+$", ErrorMessage = "Please enter valid email.")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
