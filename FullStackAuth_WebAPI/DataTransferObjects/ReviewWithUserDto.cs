using BookNookBackend.Models;
using FullStackAuth_WebAPI.DataTransferObjects;
using FullStackAuth_WebAPI.Models;

namespace BookNookBackend.DataTransferObjects
{
    public class ReviewWithUserDto
    {
        public int Id { get; set; }
        public double Rating { get; set; }

        public UserForDisplayDto User { get; set; }
    }
}
