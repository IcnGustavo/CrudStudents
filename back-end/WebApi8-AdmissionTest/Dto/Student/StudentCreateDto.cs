using System.ComponentModel.DataAnnotations;

namespace WebApi8_AdmissionTest.Dto.Student
{
    public class StudentCreateDto
    {
        [Required(ErrorMessage = "O Nome Completo é obrigatório.")]
        [StringLength(150, ErrorMessage = "O Nome Completo deve ter no máximo 150 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter 11 dígitos.")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O Email não é válido.")]
        public string Email { get; set; }
    }
}
