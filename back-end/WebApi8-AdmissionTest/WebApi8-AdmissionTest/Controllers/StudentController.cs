using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi8_AdmissionTest.Dto.Student;
using WebApi8_AdmissionTest.Models;
using WebApi8_AdmissionTest.Services.Student;

namespace WebApi8_AdmissionTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentInterface _studentInterface;
        public StudentController(IMapper mapper,IStudentInterface studentInterface) 
        {
            _studentInterface = studentInterface;
        }

        [HttpGet("ListStudents")]
        public async Task<ActionResult<ResponseModel<List<StudentModel>>>> ListStudents(string? name, string? cpf, int page = 1, int pageSize = 1)
        {
            var students = await _studentInterface.ListStudents(name,cpf,page,pageSize);

            return Ok(students);
        }

        [HttpGet("SearchStudentByRa/{studentRa}")]
        public async Task<ActionResult<ResponseModel<StudentModel>>> SearchStudentByRa(int studentRa)
        {
            var student = await _studentInterface.SearchStudentByRa(studentRa);
            return Ok(student);
        }

        [HttpGet("SearchStudentByCpf/{cpf}")]
        public async Task<ActionResult<ResponseModel<StudentModel>>> SearchStudentByCpf(string cpf)
        {
            var student = await _studentInterface.SearchStudentByCpf(cpf);
            return Ok(student);
        }

        [HttpGet("SearchStudentByName/{name}")]
        public async Task<ActionResult<ResponseModel<StudentModel>>> SearchStudentByNome(string name)
        {
            var student = await _studentInterface.SearchStudentByName(name);
            return Ok(student);
        }

        [HttpPost("CreateStudent")]
        public async Task<ActionResult<ResponseModel<List<StudentModel>>>> CreateStudent(StudentCreateDto studentCreateDto)
        {
            var students = await _studentInterface.CreateStudent(studentCreateDto);
            return Ok(students);
        }

        [HttpPut("EditStudent")]
        public async Task<ActionResult<ResponseModel<List<StudentModel>>>> EditStudent(StudentEditDto editStudentDto)
        {
            var students = await _studentInterface.EditStudent(editStudentDto);
            return Ok(students);
        }

        [HttpDelete("DeleteStudent")]
        public async Task<ActionResult<ResponseModel<List<StudentModel>>>> DeleteStudent(int id)
        {
            var students = await _studentInterface.DeleteStudent(id);
            return Ok(students);
        }
    }
}
