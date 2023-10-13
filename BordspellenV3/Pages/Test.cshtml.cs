using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BordspellenV3.Pages
{
    public class TestModel : PageModel
    {
        public IEnumerable<Game> Games { get; set; } = Enumerable.Empty<Game>();
        public void OnGet()
        {
        }
    }
}
