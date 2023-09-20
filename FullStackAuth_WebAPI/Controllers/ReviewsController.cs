using BookNookBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using FullStackAuth_WebAPI.Data;
using FullStackAuth_WebAPI.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookNookBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST api/<ReviewsController>
        [HttpPost, Authorize]
        public IActionResult Post([FromBody] Review review)
        {
            try
            {
                string userId = User.FindFirstValue("id");

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                review.UserId = userId;

                _context.Reviews.Add(review);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _context.SaveChanges();

                return StatusCode(201, review);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // check if it works also need to add a working code
        [HttpDelete("{id}"), Authorize]
        public IActionResult DeleteReview(int id)
        {
            try
            {
                var review = _context.Reviews.FirstOrDefault(f => f.Id == id);
                if (review is null)
                    return NotFound();

                var userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId) || review.UserId != userId)
                    return Unauthorized();

                _context.Reviews.Remove(review);
                _context.SaveChanges();

                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // something is off in here or in postman
        [HttpPut("{id}"), Authorize]
        public IActionResult Put(int id, [FromBody] Review review)
        {
            try
            {
                Review existReview = _context.Reviews.Include(o => o.User).FirstOrDefault(f => f.Id == id);
                if (existReview is null)
                    return NotFound();

                var userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId) || review.UserId != userId)
                    return Unauthorized();

                // I don't know if it make sence to add Book Id and User Id in here because if it's the wrong book than we just should delete it???
                existReview.Text = review.Text;
                existReview.Rating = review.Rating;

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return StatusCode(201, existReview);
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

}

