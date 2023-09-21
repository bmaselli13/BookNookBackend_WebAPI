using Azure;
using BookNookBackend.DataTransferObjects;
using BookNookBackend.Models;
using FullStackAuth_WebAPI.Data;
using FullStackAuth_WebAPI.DataTransferObjects;
using FullStackAuth_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Drawing.Text;
using System.Linq.Expressions;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace BookNookBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookDetailsController(ApplicationDbContext context, UserManager<User> user)
        {
            _context = context;
        }

        // GET: api/cars/myFavorites
        [HttpGet("{bookId}")]
        public IActionResult GetAllReviews(string bookId)
        {
            try
            {
                // This is for isfavorited . I think it would be written with ternarry operator but it is what it is.

                bool isFavoriteResultBool = false;
                var userId = User.FindFirstValue("id");

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var isFavoriteResult = _context.Favorites.Where(f => f.UserId == userId);
                if (isFavoriteResult != null)
                {
                    isFavoriteResultBool = true;
                }

                // This is for reviews
                var reviews = _context.Reviews.Include(r => r.User).Where(r => r.BookId == bookId).ToList();

                var bookDetails = new BookDetailsDto
                {
                    Reviews = reviews.Select(r => new ReviewWithUserDto
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

                    }).ToList(),
                    AverageRating = _context.Reviews.Where(b=>b.BookId == bookId).Average(r=>r.Rating),
                    IsFavorited = isFavoriteResultBool
                };

                return Ok(bookDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            };
        }
    }
}

// For my memories
//var result = _context.Reviews.Where(r => r.BookId == bookId).Select(r => new BookDetailsDto
//{

//}).FirstOrDefault();





