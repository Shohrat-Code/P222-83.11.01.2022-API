using APIIntro.Data;
using APIIntro.Models;
using APIIntro.Models.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace APIIntro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public EmployeeController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {
            List<DtoEmployee> dtoEmployees = new List<DtoEmployee>();
            foreach (var item in _context.Employees.ToList())
            {
                DtoEmployee dtoEmployee = new DtoEmployee();

                //dtoEmployee.Id = item.Id;
                //dtoEmployee.Name = item.Name;
                //dtoEmployee.Surname = item.Surname;
                //dtoEmployee.Age = item.Age;
                //dtoEmployee.IsMarried = item.IsMarried;
                //dtoEmployee.PositinId = item.PositinId;

                dtoEmployee = _mapper.Map<Employee, DtoEmployee>(item);
                dtoEmployee.Position = _context.Positions.Select(p => new { p.Id, p.Name })
                    .FirstOrDefault(pa => pa.Id == item.PositinId);

                if (item.Image!=null)
                {
                    string path = Path.Combine("wwwroot", "Uploads", item.Image);
                    byte[] bytes = System.IO.File.ReadAllBytes(path);
                    dtoEmployee.ImageBase64 = Convert.ToBase64String(bytes);
                }

                dtoEmployees.Add(dtoEmployee);
            };

            return Ok(dtoEmployees);
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployee(int? id)
        {
            if (id == null)
            {
                //return BadRequest();

                //ModelState.AddModelError("", "Error nese oluf!");
                //return StatusCode(StatusCodes.Status400BadRequest, ModelState);

                return StatusCode(StatusCodes.Status400BadRequest, "Aee naqardin?!");
            }

            Employee employee = _context.Employees.Find(id);
            if (employee == null)
            {
                ModelState.AddModelError("", "Error, nese oluf!");
                return StatusCode(StatusCodes.Status404NotFound, ModelState);

                //return StatusCode(StatusCodes.Status400BadRequest, "Aee naqardin?!");
            }

            return Ok(employee);
        }

        [HttpPost]
        public IActionResult CreateEmployee(Employee model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;

                byte[] bytes = Convert.FromBase64String(model.ImageBase64);
                MemoryStream stream = new MemoryStream(bytes);

                IFormFile file = new FormFile(stream, 0, bytes.Length, "image", "image.png");

                string filename = Guid.NewGuid() + "-" + file.FileName;
                string filePath = Path.Combine("wwwroot", "Uploads", filename);

                using (var stream2 = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream2);
                }

                model.Image = filename;

                _context.Employees.Add(model);
                _context.SaveChanges();
                return Ok();
            }

            return BadRequest();
        }

        [HttpPatch]
        public IActionResult UpdateEmployee(Employee model)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(model).State = EntityState.Modified;

                _context.Entry(model).Property(p => p.CreatedDate).IsModified = false;
                //_context.Entry(model).Property(p => p.Surname).IsModified = false;


                //_context.Employees.Update(model);
                _context.SaveChanges();
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int? id)
        {
            if (id==null)
            {
                return BadRequest();
            }

            Employee employee = _context.Employees.Find(id);
            if (employee==null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            return Ok();
        }
    }
}
