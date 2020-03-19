using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CordChrisis.Shared.Models;
using CordChrisis.BOs;

namespace CordChrisis.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SearchPageController : Controller
    {
        [HttpPost]
        [Route("post")]
        public List<Map> PostSearch([FromBody] Search postData)
        {
            SearchBO search = new SearchBO();
            return search.GetMapSearch(postData);
        }

        [HttpGet]
        [Route("getpopular")]
        public List<Map> PopularMaps()
        {
            SearchBO search = new SearchBO();
            return search.GetPopularMaps();
        }

        //public IActionResult GetDefaultSearchList()
        //{
        //    return View();
        //}
    }
}