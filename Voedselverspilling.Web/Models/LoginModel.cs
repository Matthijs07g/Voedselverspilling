using System.ComponentModel.DataAnnotations;

namespace Voedselverspilling.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required")]
        public required string Emailaddres { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
