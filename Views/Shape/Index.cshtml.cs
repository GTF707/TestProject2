using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TestProject2.Views.Shape
{
    public class IndexModel : PageModel
    {
        public string Name { get; set; } = "Georgy";
        public void OnGet()
        {   
            Name = "Hello";
        }
    }
}
