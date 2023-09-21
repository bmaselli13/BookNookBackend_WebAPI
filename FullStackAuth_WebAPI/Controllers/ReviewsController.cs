using BookNookBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using FullStackAuth_WebAPI.Data;
using FullStackAuth_WebAPI.Models;
using BookNookBackend.DataTransferObjects;
using FullStackAuth_WebAPI.DataTransferObjects;


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

        [HttpPut("{reviewid}"), Authorize]
        public IActionResult Put(int reviewid, [FromBody] Review review)
        {
            try
            {
                Review existReview = _context.Reviews.Include(o => o.User).FirstOrDefault(f => f.Id == reviewid);
                if (existReview is null)
                    return NotFound();
                
                string userId = User.FindFirstValue("id");

                if (string.IsNullOrEmpty(userId) || existReview.UserId != userId)
                    return Unauthorized();

                existReview.Text = review.Text;
                existReview.Rating = review.Rating;

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _context.SaveChanges();

                var userDto = new UserForDisplayDto
                {
                    Id = existReview.User.Id,
                    FirstName = existReview.User.FirstName,
                    LastName = existReview.User.LastName,
                    UserName = existReview.User.UserName
                };

                var existringReviewDto = new ReviewWithUserDto
                {
                    Id = existReview.Id,
                    BookId = existReview.BookId,
                    Text = existReview.Text,
                    Rating = existReview.Rating,
                    User = userDto
                    // at first i wanted to create with one but i had an  erro so u pull this outside...userDto... 
                    //User = 
                    //{
                    //   Id = existReview.User.Id,
                    //   FirstName = existReview.User.FirstName,
                    //   LastName = existReview.User.LastName,
                    //   UserName = existReview.User.UserName
                    //},

                };

                return StatusCode(201, existringReviewDto);
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

