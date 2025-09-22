using WebApi8_AdmissionTest.Dto.Student;
using WebApi8_AdmissionTest.Models;

namespace WebApi8_AdmissionTest.Services.Student
{
    public interface IStudentInterface
    {
        Task<ResponseModel<List<StudentModel>>> ListStudents(string? name = null, string? cpf = null, int page = 1, int pageSize = 10);
        Task<ResponseModel<StudentModel>> SearchStudentByRa(int idPessoa);
        Task<ResponseModel<StudentModel>> SearchStudentByCpf(string cpf);
        Task<ResponseModel<List<StudentModel>>> SearchStudentByName(string name); // <-- agora retorna lista

        Task<ResponseModel<List<StudentModel>>> CreateStudent(StudentCreateDto studentCreateDto);
        Task<ResponseModel<List<StudentModel>>> EditStudent(StudentEditDto student);
        Task<ResponseModel<List<StudentModel>>> DeleteStudent(int studentRa);
    }
}
