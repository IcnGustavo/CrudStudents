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
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "CPF inválido. Use o formato 000.000.000-00.")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; }
    }
}
