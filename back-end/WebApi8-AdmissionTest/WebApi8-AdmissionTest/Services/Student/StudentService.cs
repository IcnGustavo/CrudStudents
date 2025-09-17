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
                var student = await _context.Students.FirstOrDefaultAsync(studentData => studentData.Cpf == cpf);
                return RetResponse(student, null);
            }
            catch (Exception ex)
            {
                return RetResponse(null, ex);
            }

        }

        public async Task<ResponseModel<StudentModel>> SearchStudentByRa(int studentRa)
        {
            ResponseModel<StudentModel> response = new ResponseModel<StudentModel>();
            try
            {
                var student = await _context.Students.FirstOrDefaultAsync(studentData => studentData.Ra == studentRa);
                return RetResponse(student, null);
            }
            catch (Exception ex)
            {
                return RetResponse(null, ex);
            }
        }

        public async Task<ResponseModel<StudentModel>> SearchStudentByName(string name)
        {
            ResponseModel<StudentModel> response = new ResponseModel<StudentModel>();
            try
            {
                var student = await _context.Students.Where(p => EF.Functions.Like(p.Name, $"%{name}%")).FirstOrDefaultAsync();

                return RetResponse(student, null);
            }
            catch (Exception ex)
            {
                return RetResponse(null, ex);
            }
        }

        public ResponseModel<StudentModel> RetResponse(StudentModel student, Exception ex)
        {
            ResponseModel<StudentModel> response = new ResponseModel<StudentModel>();
            if (ex != null)
            {
                response.Message = ex.Message;
                response.Status = false;
            }

            if (student == null)
            {
                response.Message = "Nenhum registro localizado!";
                return response;
            }
            response.Data = student;
            response.Message = "Aluno Localizado!";

            return response;
        }

        public async Task<ResponseModel<List<StudentModel>>> ListStudents(string? name = null,string? cpf = null,int page = 1,int pageSize = 10)
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

                var Students = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                response.Data = Students;
                response.Message = "Lista de alunos retornada com sucesso!";
                response.Status = true;
                response.TotalPages = totalPages;
                response.TotalRegisters = totalRegisters;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<StudentModel>>> CreateStudent(StudentCreateDto personDto)
        {
            ResponseModel<List<StudentModel>> response = new ResponseModel<List<StudentModel>> ();

            try
            {
                var personModel = _mapper.Map<StudentModel>(personDto);

                _context.Add(personModel);
                await _context.SaveChangesAsync();

                response.Data = await _context.Students.ToListAsync();
                response.Message = "Aluno criado com sucesso";

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.InnerException?.Message ?? ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<StudentModel>>> EditStudent(StudentEditDto putPerson)
        {
            ResponseModel<List<StudentModel>> response = new ResponseModel<List<StudentModel>>();

            try
            {
                var student = await _context.Students.FirstOrDefaultAsync(studentData => studentData.Ra == putPerson.Ra);

                if (student == null)
                {
                    response.Message = "Nenhum aluno localizado!";
                    return response;
                }

                _mapper.Map(putPerson, student);
                _context.Update(student);
                await _context.SaveChangesAsync();

                response.Data = await _context.Students.ToListAsync();
                response.Message = "Aluno criado com sucesso"; 
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.InnerException?.Message ?? ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<StudentModel>>> DeleteStudent(int raPerson)
        {
            ResponseModel<List<StudentModel>> response = new ResponseModel<List<StudentModel>>();

            try
            {
                var student = await _context.Students.FirstOrDefaultAsync(personData => personData.Ra == raPerson);

                if (student == null)
                {
                    response.Message = "Nenhum aluno localizado!";
                    return response;
                }

                _context.Remove(student);
                await _context.SaveChangesAsync();

                response.Data = await _context.Students.ToListAsync();
                response.Message = "Aluno excluido com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.InnerException?.Message ?? ex.Message;
                response.Status = false;
                return response;
            }
        }
    }
}
