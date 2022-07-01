using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Employee.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        static readonly IEmployeeRepository repository = new EmployeeRepository();
        public IEnumerable<Employee> GetAllEmployee()
        {
            return repository.GetAll();
        }
        public Employee GetEmployee(int id)
        {
            Employee item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return item;
        }
        public IEnumerable<Employee> GetEmployeeByName(string name)
        {
            return repository.GetAll().Where(
                p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
        }
        public HttpResponseMessage PostEmployee(Employee item)
        {
            item = repository.Add(item);
            var response = Request.CreateResponse<Employee>(HttpStatusCode.Created, item);
            string uri = Url.Link("DefaultApi", new { id = item.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }
        public void PutEmployee(int id, Employee employee)
        {
            employee.Id = id;
            if (!repository.Update(employee))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }
        public HttpResponseMessage DeleteEmployee(int id)
        {
            repository.Remove(id);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}