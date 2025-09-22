using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApi8_AdmissionTest.Data;
using WebApi8_AdmissionTest.Dto.Student;
using WebApi8_AdmissionTest.Models;

namespace WebApi8_AdmissionTest.Services.Student
{
    public class StudentService : IStudentInterface
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public StudentService(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ResponseModel<StudentModel>> SearchStudentByCpf(string cpf)
        {
            try
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(studentData => studentData.Cpf == cpf);

                return RetResponse(student, null, "Aluno localizado!", "Nenhum registro localizado!");
            }
            catch (Exception ex)
            {
                return RetResponse<StudentModel>(null, ex);
            }
        }

        public async Task<ResponseModel<StudentModel>> SearchStudentByRa(int studentRa)
        {
            try
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(studentData => studentData.Ra == studentRa);

                return RetResponse(student, null, "Aluno localizado!", "Nenhum registro localizado!");
            }
            catch (Exception ex)
            {
                return RetResponse<StudentModel>(null, ex);
            }
        }

        public async Task<ResponseModel<List<StudentModel>>> SearchStudentByName(string name)
        {
            try
            {
                var students = await _context.Students
                    .AsNoTracking()
                    .Where(p => EF.Functions.Like(p.Name, $"%{name}%"))
                    .ToListAsync();

                return RetResponse(students, null, "Alunos localizados!", "Nenhum registro localizado!");
            }
            catch (Exception ex)
            {
                return RetResponse<List<StudentModel>>(null, ex);
            }
        }

        private ResponseModel<T> RetResponse<T>(T data, Exception ex, string successMsg = "Sucesso", string notFoundMsg = "Nenhum registro localizado!")
        {
            ResponseModel<T> response = new ResponseModel<T>();

            if (ex != null)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }

            if (data == null || (data is IEnumerable<object> list && !list.Any()))
            {
                response.Message = notFoundMsg;
                response.Status = false;
                return response;
            }

            response.Data = data;
            response.Message = successMsg;
            response.Status = true;
            return response;
        }

        public async Task<ResponseModel<List<StudentModel>>> ListStudents(string? name = null, string? cpf = null, int page = 1, int pageSize = 10)
        {
            ResponseModel<List<StudentModel>> response = new ResponseModel<List<StudentModel>>();

            try
            {
                var query = _context.Students.AsQueryable();

                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(p => p.Name.ToLower().Contains(name.ToLower()));

                if (!string.IsNullOrWhiteSpace(cpf))
                    query = query.Where(p => p.Cpf == cpf);

                int totalRegisters = await query.CountAsync();
                int totalPages = (int)Math.Ceiling(totalRegisters / (double)pageSize);

                var students = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                response.Data = students;
                response.Message = "Lista de alunos retornada com sucesso!";
                response.Status = true;
                response.TotalPages = totalPages;
                response.TotalRegisters = totalRegisters;

                return response;
            }
            catch (Exception ex)
            {
                return RetResponse<List<StudentModel>>(null, ex);
            }
        }

        public async Task<ResponseModel<List<StudentModel>>> CreateStudent(StudentCreateDto studentDto)
        {
            try
            {
                var cpfExistente = await _context.Students.AnyAsync(s => s.Cpf == studentDto.Cpf);
                if (cpfExistente)
                {
                    return new ResponseModel<List<StudentModel>>
                    {
                        Status = false,
                        Message = "CPF já cadastrado!"
                    };
                } 
                var studentModel = _mapper.Map<StudentModel>(studentDto);

                _context.Add(studentModel);
                await _context.SaveChangesAsync();

                var students = await _context.Students.ToListAsync();
                return RetResponse(students, null, "Aluno criado com sucesso!", "Nenhum aluno localizado!");
            }
            catch (Exception ex)
            {
                return RetResponse<List<StudentModel>>(null, ex);
            }
        }

        public async Task<ResponseModel<List<StudentModel>>> EditStudent(StudentEditDto putstudent)
        {
            try
            {
                var student = await _context.Students.FirstOrDefaultAsync(studentData => studentData.Ra == putstudent.Ra);

                if (student == null)
                    return RetResponse<List<StudentModel>>(null, null, "", "Nenhum aluno localizado!");

                _mapper.Map(putstudent, student);
                _context.Update(student);
                await _context.SaveChangesAsync();

                var students = await _context.Students.ToListAsync();
                return RetResponse(students, null, "Aluno editado com sucesso!", "Nenhum aluno localizado!");
            }
            catch (Exception ex)
            {
                return RetResponse<List<StudentModel>>(null, ex);
            }
        }

        public async Task<ResponseModel<List<StudentModel>>> DeleteStudent(int rastudent)
        {
            try
            {
                var student = await _context.Students.FirstOrDefaultAsync(studentData => studentData.Ra == rastudent);

                if (student == null)
                    return RetResponse<List<StudentModel>>(null, null, "", "Nenhum aluno localizado!");

                _context.Remove(student);
                await _context.SaveChangesAsync();

                var students = await _context.Students.ToListAsync();
                return RetResponse(students, null, "Aluno excluído com sucesso!", "Nenhum aluno localizado!");
            }
            catch (Exception ex)
            {
                return RetResponse<List<StudentModel>>(null, ex);
            }
        }
    }
}
