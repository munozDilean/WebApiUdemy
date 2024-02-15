using Microsoft.AspNetCore.Mvc;
using WebAPIUdemy.Data;
using WebAPIUdemy.Filters.ActionFilters;
using WebAPIUdemy.Filters.ExceptionFilters;
using WebAPIUdemy.Models;
using WebAPIUdemy.Models.Repositories;

namespace WebAPIUdemy.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ShirtsController : ControllerBase
    {   
        private readonly ApplicationDbContext db;

        public ShirtsController(ApplicationDbContext db) 
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult GetShirts()
        {
            return Ok(db.Shirts.ToList());
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        public IActionResult GetShirtById(int id)
        {
            return Ok(HttpContext.Items["shirt"]);
        }

        [HttpPost]
        [TypeFilter(typeof(Shirt_ValidateCreateShirtFilterAttribute))]
        public IActionResult CreateShirt([FromBody] Shirt shirt)
        {
            this.db.Shirts.Add(shirt);
            this.db.SaveChanges();
            
            return CreatedAtAction(nameof(GetShirtById),
            new { id = shirt.ShirtId },
                shirt);
        }

        [HttpPut("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [Shirt_ValidateUpdateShirtFilter]
        [TypeFilter(typeof(Shirt_HandleUpdateExceptionsFilterAttribute))]
        public IActionResult UpdateShirt(int id, Shirt shirt)
        {
            var shirtToUpdate = HttpContext.Items["shirt"] as Shirt;
            // It will never be null beacuse we check in the Filters
            shirtToUpdate.Brand = shirt.Brand;
            shirtToUpdate.Price = shirt.Price;
            shirtToUpdate.Size = shirt.Size;
            shirtToUpdate.Color = shirt.Color;
            shirtToUpdate.Gender = shirt.Gender;


            // Elements are mark for updates, but are not updated officially until  this method is called. 
            db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        public IActionResult DeleteShirt(int id)
        {
            var shirtToDelete = HttpContext.Items["shirt"] as Shirt; // will never be null
            db.Shirts.Remove(shirtToDelete);
            db.SaveChanges();

            return Ok(shirtToDelete);
        }
    }
}