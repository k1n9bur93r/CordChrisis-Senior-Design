using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CordChrisis.Shared.Models;
using CordChrisis.BOs;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;

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
            List<Map> returnData= search.GetPopularMaps();
            return returnData;
        }

        [HttpPost]
        [Route("readsingle")]
        public Map GetMapByID([FromBody]string ID)
        {
            MapBO map = new MapBO();
            Map data = new Map();
            data = map.GetMapData(ID);
            return data;
        }
        [HttpGet]
        [Route("readsinglesong/{ID}")]
        public Stream GetMapSongByID([FromRoute]string ID)
        {
            MapBO map = new MapBO();
            //HttpContent fileStreamContent = new StreamContent(map.GetMapMusic(ID));
            Stream data = map.GetMapMusic(ID);
            return data;

        }


        [HttpPost]
        [Route("update")]
        public  void UpdateMap([FromBody]string updatedMap)
        {
            MapBO map = new MapBO();
             map.UpdateMap(JsonConvert.DeserializeObject<Map>(updatedMap));
        }

        //public IActionResult GetDefaultSearchList()
        //{
        //    return View();
        //}
    }
}