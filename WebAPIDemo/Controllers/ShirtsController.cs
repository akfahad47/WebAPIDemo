﻿using Microsoft.AspNetCore.Mvc;
using WebAPIDemo.Filters;
using WebAPIDemo.Filters.ActionFilters;
using WebAPIDemo.Filters.ExceptionFilters;
using WebAPIDemo.Models;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ShirtsController : ControllerBase
	{

		[HttpGet]
		public IActionResult GetShirts()
		{
			return Ok(ShirtRepositorie.GetShirts());
		}

		[HttpGet("{id}")]
		[Shirt_ValidateShirtIdFilter]
		public IActionResult GetShirtById(int id)
		{
			return Ok(ShirtRepositorie.GetShirtById(id));

		}

		[HttpPost]
		[Shirt_ValidateCreateShirtFilter]
		public IActionResult CreateShirt(Shirt shirt)
		{

			ShirtRepositorie.AddShirt(shirt);

			return CreatedAtAction(nameof(GetShirtById),
				new { id = shirt.ShirtId },
				shirt);
		}

		[HttpPut("{id}")]
		[Shirt_ValidateShirtIdFilter]
		[Shirt_ValidateUpdateShirtFilter]
		[Shirt_HandleUpdateExceptionsFilter]
		public IActionResult UpdateShirt(int id, Shirt shirt)
		{

			ShirtRepositorie.UpdateShirt(shirt);
			
			return NoContent();
		}

		[HttpDelete("{id}")]
		[Shirt_ValidateShirtIdFilter]
		public IActionResult DeleteShirt(int id)
		{
			var shirt = ShirtRepositorie.GetShirtById(id);
			ShirtRepositorie.DeleteShirt(id);
			return Ok(shirt);
		}
	}
}
