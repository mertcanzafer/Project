using System.Security.Claims;
using FitnessApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

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
            // will give the logged-in user's userId

            FavoriteList = ToDo.Favorites.ToList();
        }
    }
}
