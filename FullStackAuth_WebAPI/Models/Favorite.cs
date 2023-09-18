using FullStackAuth_WebAPI.Models;

namespace BookNookBackend.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public string BookId { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }


    }
}
