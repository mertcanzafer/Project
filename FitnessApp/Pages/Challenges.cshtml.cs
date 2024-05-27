using System.Security.Claims;
using FitnessApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

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

        [BindProperty(SupportsGet = true)]
        public string CategoryFilter { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string DifficultyLevelFilter { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int? PeriodFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ChallengeNameSearch { get; set; } = default!;

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || NewToDo == null)
            {
                return Page();
            }

            NewToDo.IsDeleted = false;
            NewToDo.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ToDo.Add(NewToDo);
            ToDo.SaveChanges();
            return RedirectToAction("Get");
        }

        public IActionResult OnPostDelete(int id)
        {
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

            var query = ToDo.TblTodos.AsQueryable();

            query = query.Where(item => item.IsDeleted == false && item.UserId == userId);

            if (!string.IsNullOrEmpty(CategoryFilter))
            {
                query = query.Where(item => item.Category == CategoryFilter);
            }

            if (!string.IsNullOrEmpty(DifficultyLevelFilter))
            {
                query = query.Where(item => item.DifficultyLevel == DifficultyLevelFilter);
            }

            if (PeriodFilter.HasValue)
            {
                query = query.Where(item => item.Period == PeriodFilter.Value);
            }

            if (!string.IsNullOrEmpty(ChallengeNameSearch))
            {
                query = query.Where(item => item.ChallengeName.Contains(ChallengeNameSearch));
            }

            ToDoList = query.ToList();
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
