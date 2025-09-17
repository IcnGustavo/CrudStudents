using System.ComponentModel.DataAnnotations;

namespace WebApi8_AdmissionTest.Dto.Student
{
    public class StudentEditDto
    {
        public int Ra { get; set; }

        [StringLength(150, ErrorMessage = "O Nome Completo deve ter no máximo 150 caracteres.")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "O Email não é válido.")]
        public string Email { get; set; }

    }
}
