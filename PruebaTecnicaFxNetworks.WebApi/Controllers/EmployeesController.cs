using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PruebaTecnicaFxNetworks.Application;
using PruebaTecnicaFxNetworks.Entities;
using PruebaTecnicaFxNetworks.WebApi.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTecnicaFxNetworks.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme) ]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        IApplication<Employee> _employee;
        public EmployeesController(IApplication<Employee> Employee) 
        {
            _employee = Employee;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _employee.GetAllAsync());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employee.GetByIdAsync(id);

            if (employee != null)
            {
                return Ok(employee);
            }
            else
            {
                return NoContent();
            }
        }


        [HttpPost]
        public async Task<IActionResult> Save(EmployeeDTO employeeDTO) 
        {
            var user = new Employee()
            {
                Name = employeeDTO.Name,
                Age = employeeDTO.Age,
                Salary = employeeDTO.Salary,
                Position = employeeDTO.Position
            };

            await _employee.SaveAsync(user);
            return Ok(user);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, EmployeeDTO employeeDTO)
        {
            if (id == 0 || employeeDTO == null) return NotFound();

            var tmp = _employee.GetById(id);
            if (tmp != null)
            {
                tmp.Name = employeeDTO.Name;
                tmp.Age = employeeDTO.Age;
                tmp.Salary = employeeDTO.Salary;
                tmp.Position = employeeDTO.Position;
            }
            await _employee.SaveAsync(tmp);
            return Ok(tmp);

        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();

            _employee.DeleteAsync(id);
            return Ok();
        }
    }
}
