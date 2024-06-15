using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReidarJWTApi.Models;

namespace ReidarJWTApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }

        private static List<Customer> Customers = new List<Customer>
        {
            new Customer { CustomerId = 1, Name = "Tobias Landen", Email = "tobbe@qlok.se", Address = "87999 Stockholm" },
            new Customer { CustomerId = 2, Name = "Reidar Summerplace", Email = "reidar@spain.com", Address = "99765 Palma" }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            var authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();

            return Ok(Customers);
        }

        [HttpGet("{id}")]
        public ActionResult<Customer> Get(int id)
        {
            var customer = Customers.Find(c => c.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        public ActionResult<Customer> Post(Customer customer)
        {
            customer.CustomerId = Customers.Count + 1;
            Customers.Add(customer);
            return CreatedAtAction(nameof(Get), new { id = customer.CustomerId }, customer);
        }
    }
}
