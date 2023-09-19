using Azure;
using BookNookBackend.DataTransferObjects;
using BookNookBackend.Models;
using FullStackAuth_WebAPI.Data;
using FullStackAuth_WebAPI.DataTransferObjects;
using FullStackAuth_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Drawing.Text;
using System.Net;
using System.Security.Claims;

namespace BookNookBackend.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class BookDetailsController : ControllerBase
        {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<User> _userManager;

            public BookDetailsController(ApplicationDbContext context, UserManager<User> user)
            {
                _context = context;
                _userManager = user;
            }

            // GET: api/cars/myFavorites
            [HttpGet("{bookId}")]
            public IActionResult GetAllReviews(string bookId)
            {
            //var reviews = _context.Reviews.Where(r => r.BookId == bookId).Select(r => new ReviewWithUserDto
            //{
            // User = r.User.UserName,
            // Rating = r.Rating,
            // Review = r.
            //} )                                                


            var reviews = _context.Reviews.Where(r => r.BookId == bookId).Select(r => new ReviewWithUserDto
            {
                Id = r.Id,
                BookId = r.BookId,
                Text = r.Text,
                Rating = r.Rating,
                User = new UserForDisplayDto
                {
                    Id = r.User.Id,
                    FirstName = r.User.FirstName,
                    LastName = r.User.LastName,
                    UserName = r.User.UserName,
                }
            }).ToList();

            int x = 0;

            return Ok(reviews);


            //Accepts a value from the request’s URL  (The bookId of the Book I am trying to get Details for). 


            //Responds with a BookDetailsDto (Map through details defined in instructions
            // All Reviews from the database that are related to the BookId sent in the URL. This should be a List<ReviewWithUserDto> so that Reviews can be displayed with Usernames–sensitive User properties should NOT be included in response.
            // An average of all User ratings.
            // A true / false property as to whether the logged -in user has favorited this book.

            
            
            //Returns a 200 status code.



            //try
            //{
            //        string userId = User.FindFirstValue("id");

            //        var reviews = _context.Reviews.Where(r => r.UserId.Equals(userId));

            //        return StatusCode(200, reviews);
            //    }
            //    catch (Exception ex)
            //    {
            //        return StatusCode(500, ex.Message);
            //    }
            }
        }

}

