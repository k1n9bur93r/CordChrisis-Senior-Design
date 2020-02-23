using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace testServer.Controllers
{
    public class GameBuilderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}