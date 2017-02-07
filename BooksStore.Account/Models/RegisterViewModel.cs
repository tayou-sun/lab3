using System.ComponentModel.DataAnnotations;

namespace BooksStore.Account.Models
{
    /// <summary>
    ///     Модель представления для вида Register
    /// </summary>
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}