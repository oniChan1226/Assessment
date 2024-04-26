using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentPortal.API.Data;
using StudentPortal.API.Models;
using StudentPortal.API.Models.DTOs;
using System.Text.RegularExpressions;
using System;

namespace StudentPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentDbContext dbContext;
        public StudentController(StudentDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private bool IsValid(string name)
        {
            string pattern = @"[\d~`!@#$%^&*()+=\[\]{}\\|;:'"",.<>?\/]";
            if(name == null || Regex.IsMatch(name, pattern))
            {
                return false;
            }
            return true;
        }

        [HttpGet]
        public IActionResult GetAllStudents()
        {
            var students = dbContext.Students.ToList();
            var studentsDto = new List<GetStudentsDto>();

            foreach (var student in students)
            {
                studentsDto.Add(new GetStudentsDto()
                {
                    RollNumber = student.RollNumber,
                    Name = student.Name,
                    Class = student.Class,
                    Address = student.Address,
                });
            }
            return Ok(studentsDto);
        }

        [HttpGet("{RollNumber:Guid}")]
        public IActionResult GetStudentByRollNumber(Guid RollNumber)
        {
            //var student = dbContext.Students.FirstOrDefault(x => x.RollNumber == RollNumber);
            var student = dbContext.Students.Find(RollNumber);
            if(student == null)
            {
                return NotFound();
            }
            var studentDto = new GetStudentsDto
            {
                RollNumber = student.RollNumber,
                Name = student.Name,
                Class = student.Class,
                Address = student.Address,
            };
            return Ok(studentDto);
        }

        [HttpPost]
        public IActionResult CreateStudent([FromBody] PostStudentDto studentDto)
        {
            if (!(IsValid(studentDto.Name)))
            {
                return BadRequest("Name Contains Invalid Characters");
            }
            var studentModel = new StudentModel
            {
                Name = studentDto.Name,
                Class = studentDto.Class,
                Address = studentDto.Address,
            };
            dbContext.Students.Add(studentModel);
            dbContext.SaveChanges();
            var NewStudent = new PostStudentDto
            {
                Name = studentModel.Name,
                Class = studentModel.Class,
                Address = studentModel.Address,
            };
            return CreatedAtAction(nameof(GetStudentByRollNumber), new {RollNumber = studentModel.RollNumber}, NewStudent);
        }

        [HttpPut]
        [Route("{RollNumber:Guid}")]
        public IActionResult UpdateStudent(Guid RollNumber, [FromBody] PostStudentDto updateStudentDto)
        {
            var student = dbContext.Students.FirstOrDefault(x => x.RollNumber == RollNumber);
            if(student == null || !(IsValid(updateStudentDto.Name)))
            {
                return NotFound();
            }
            student.Name = updateStudentDto.Name;
            student.Class = updateStudentDto.Class;
            student.Address = updateStudentDto.Address;
            dbContext.SaveChanges();
            var updatedStudent = new PostStudentDto
            {
                Name = student.Name,
                Class = student.Class,
                Address = student.Address,
            };
            return Ok(updatedStudent);
        }

        [HttpDelete("{RollNumber:Guid}")]
        public IActionResult DeleteStudent(Guid RollNumber)
        {
            var student = dbContext.Students.FirstOrDefault(x =>x.RollNumber == RollNumber);
            if(student == null)
            {
                return NotFound();
            }
            dbContext.Students.Remove(student);
            dbContext.SaveChanges();

            return NoContent();
        }


    }
}
