﻿using Microsoft.AspNetCore.Mvc;
using WebAPIDemo.Attributes;
using WebAPIDemo.Data;
using WebAPIDemo.Filters;
using WebAPIDemo.Filters.ActionFilters;
using WebAPIDemo.Filters.AuthFilters;
using WebAPIDemo.Filters.ExceptionFilters;
using WebAPIDemo.Models;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Controllers
{

	[ApiVersion("1.0")]
	[ApiController]
	[Route("api/[controller]")]
	[JwtTokenAuthFilter]
	public class ShirtsController : ControllerBase
	{
        public ApplicationDbContext Db { get; }

        public ShirtsController(ApplicationDbContext db)
        {
            Db = db;
        }

        [HttpGet]
		[RequiredClaim("read", "true")]
		public IActionResult GetShirts()
		{
			return Ok(Db.Shirts.ToList());
		}

		[HttpGet("{id}")]
		[TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [RequiredClaim("read", "true")]
        public IActionResult GetShirtById(int id)
		{
			var shirt = HttpContext.Items["shirt"];

			return Ok(shirt);

		}

		[HttpPost]
		[TypeFilter(typeof(Shirt_ValidateCreateShirtFilterAttribute))]
        [RequiredClaim("write", "true")]
        public IActionResult CreateShirt(Shirt shirt)
		{

			Db.Shirts.Add(shirt);
			Db.SaveChanges();

			return CreatedAtAction(nameof(GetShirtById),
				new { id = shirt.ShirtId },
				shirt);
		}

		[HttpPut("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [RequiredClaim("write", "true")]
        [Shirt_ValidateUpdateShirtFilter]
		[TypeFilter(typeof(Shirt_HandleUpdateExceptionsFilterAttribute))]
        public IActionResult UpdateShirt(int id, Shirt shirt)
		{
			var shirtToUpdate = HttpContext.Items["shirt"] as Shirt;
            shirtToUpdate.Brand = shirt.Brand;
            shirtToUpdate.Price = shirt.Price;
            shirtToUpdate.Size = shirt.Size;
            shirtToUpdate.Color = shirt.Color;
            shirtToUpdate.Gender = shirt.Gender;

			Db.SaveChanges();

            return NoContent();
		}

		[HttpDelete("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [RequiredClaim("delete", "true")]
        public IActionResult DeleteShirt(int id)
		{
			var shirtToDelete = HttpContext.Items["shirt"] as Shirt;

			Db.Shirts.Remove(shirtToDelete);
			Db.SaveChanges();
            return Ok(shirtToDelete);
		}
	}
}
