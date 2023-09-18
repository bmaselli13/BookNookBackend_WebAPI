﻿using FullStackAuth_WebAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookNookBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FavoriteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/cars/myFavorites
        [HttpGet("myFavorite"), Authorize]
        public IActionResult GetAllReviews()
        {
            try
            {
                string userId = User.FindFirstValue("id");

                var favorites = _context.Favorites.Where(f => f.UserId.Equals(userId));

                return StatusCode(200, favorites);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
