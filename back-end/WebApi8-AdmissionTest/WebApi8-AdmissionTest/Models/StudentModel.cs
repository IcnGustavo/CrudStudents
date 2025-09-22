using System.ComponentModel.DataAnnotations;

namespace WebApi8_AdmissionTest.Models
{
    public class StudentModel
    {
        [Key]
        public int Ra { get; set; }

        [Required(ErrorMessage = "O Nome é obrigatório.")]
        [MaxLength(150, ErrorMessage = "O Nome deve ter no máximo 150 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF inválido. Deve conter 11 dígitos.")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; }
    }
}
