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
            try
            {

                var reviews = _context.Reviews.Where(r => r.BookId == bookId).Select(r => new ReviewWithUserDto
                {
                    Id = r.Id,
                    BookId = r.BookId,
                    Text = r.Text,
                    User = new UserForDisplayDto
                    {
                        Id = r.User.Id,
                        FirstName = r.User.FirstName,
                        LastName = r.User.LastName,
                        UserName = r.User.UserName,
                    }
                }).ToList();

                return Ok(reviews);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

           
        }





    }
}

