using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT150Site.Models;

namespace MSIT150Site.Controllers
{
    public class HomeWorkController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Homework0807()
        {
            return View();
        }

        public IActionResult Travel()
        {
            return View();
        }

        

    }
}
