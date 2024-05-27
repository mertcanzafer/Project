using System.Security.Claims;
using FitnessApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace MyApp.Namespace
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        public UserToDoDatabaseContext ToDo = new();
        public List<Favorite> FavoriteList { get; set; } = new List<Favorite>();

        public void OnGet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Get the logged-in user's userId

            // Retrieve only the favorites for the logged-in user
            FavoriteList = ToDo.Favorites
                .Where(favorite => favorite.UserId == userId)
                .ToList();
        }
    }
}
