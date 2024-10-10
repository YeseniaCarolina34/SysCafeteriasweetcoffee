using System.ComponentModel.DataAnnotations;

namespace SysCafeteriasweetcoffee.ViewModel
{
    public class LoginViewModel
    {
        //Agregue esto 
        public string Usuario { get; set; }
       
        [Required(ErrorMessage = "El campo Login es obligatorio.")]
        public string Login { get; set; } = null!;

        [Required(ErrorMessage = "El campo Password es obligatorio.")]
        public string Password { get; set; } = null!;
    }
}
