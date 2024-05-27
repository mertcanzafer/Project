using System.Security.Claims;
using FitnessApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace MyApp.Namespace
{
    [Authorize]
    public class ChallengesModel : PageModel
    {

        [BindProperty]
        public TblTodo NewToDo { get; set; } = default!;

        public UserToDoDatabaseContext ToDo = new();
        public List<TblTodo> ToDoList { get; set; } = new List<TblTodo>();

        public List<Favorite> FavoriteList { get; set; } = new List<Favorite>();
        public Favorite favorite { get; set; } = default!;
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || NewToDo == null)
            {
                return Page();
            }
            NewToDo.IsDeleted = false;
            NewToDo.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // add this statement
            ToDo.Add(NewToDo);
            ToDo.SaveChanges();
            return RedirectToAction("Get");
        }
        public IActionResult OnPostDelete(int id)
        {
            // var itemToUpdate = ToDoList.FirstOrDefault(item => item.Id == id);
            if (ToDo.TblTodos != null)
            {
                var todo = ToDo.TblTodos.Find(id);
                if (todo != null)
                {
                    todo.IsDeleted = true;
                    ToDo.SaveChanges();
                }
            }

            return RedirectToAction("Get");
        }
        public void OnGet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // will give the logged-in user's userId

            // LINQ query to retrieve items where IsDeleted is false and UserId is the logged-in user's id
            ToDoList = (from item in ToDo.TblTodos
                        where item.IsDeleted == false
                        where item.UserId == userId
                        select item).ToList();
        }
        public IActionResult OnPostAddToList(int id)
        {
            var todo = ToDo.TblTodos.Find(id);

            if (todo != null)
            {
                var favorite = new Favorite
                {
                    Id = todo.Id,
                    ChallengeName = todo.ChallengeName,
                    Category = todo.Category,
                    DifficultyLevel = todo.DifficultyLevel,
                    Period = todo.Period,
                    UserId = todo.UserId
                };

                FavoriteList.Add(favorite);

                foreach (var favoriteItem in FavoriteList)
                {
                    ToDo.Favorites.Add(favoriteItem);
                }

                ToDo.SaveChanges();
                FavoriteList.Clear();
            }

            return RedirectToAction("Get");
        }
    }
}
