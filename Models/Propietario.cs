
using System.ComponentModel.DataAnnotations;
namespace InmoviliariaSarchioniAlfonzo.Models;

    public class Propietario
    {
        public int Id_Propietario { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [RegularExpression(@"^\d{1,10}$", ErrorMessage = "El DNI debe contener solo números y un máximo de 11 dígitos.")]
        public int Dni { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string? Apellido { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
        [RegularExpression(@"^\d{7,15}(-\d{1,15})*$", ErrorMessage = "El teléfono debe contener solo números, guiones, y tener entre 7 y 15 dígitos.")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public string? Direccion { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public int Estado_Propietario { get; set; }
    }
    