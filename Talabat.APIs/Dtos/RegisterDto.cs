using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string DisplayName{ get; set; }
    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
    [Required]
    //[RegularExpression("/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/\r\n")]
    public string Password { get; set; }
}
