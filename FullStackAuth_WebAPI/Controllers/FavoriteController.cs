using BookNookBackend.Models;
using FullStackAuth_WebAPI.Data;
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

        // Post: api/favorite/
        [HttpPost, Authorize]
        public IActionResult Post([FromBody] Favorite favorite)
        {
            try
            {
                string userId = User.FindFirstValue("id");

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                favorite.UserId = userId;

                _context.Favorites.Add(favorite);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _context.SaveChanges();

                return StatusCode(201, favorite);
            }

            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }


        // doesn't work probably we need to remove from user that its his favorite not just delete vaforite
        //[HttpDelete("{favoriteid}"), Authorize]
        //public IActionResult Deletefavorite(int favoriteid)
        //{
        //    try
        //    {
        //        var review = _context.Reviews.FirstOrDefault(r => r.Id == favoriteid);
        //        if (review is null)
        //            return NotFound();

        //        var userId = User.FindFirstValue("id");
        //        if (string.IsNullOrEmpty(userId) || review.UserId != userId)
        //            return Unauthorized();

        //        _context.Reviews.Remove(review);
        //        _context.SaveChanges();

        //        return StatusCode(204);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

    }
}
