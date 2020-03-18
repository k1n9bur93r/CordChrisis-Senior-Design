using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CordChrisis.Shared.Models;

namespace CordChrisis.DAOs
{
    public class MapDA
    {
        public void Create(Map map) {
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                context.Add(map);
            }
        }
        public Map ReadSingle(string mapID)
        {
            Map data = new Map();
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                data = context.Map.Where(a => a.ID.ToString() == mapID).FirstOrDefault();
            }
            return data;
        }

        public List<Map> ReadMany(Search search)
        {
            List<Map> data = new List<Map>();
            using (var context = new ApplicationDBContext())
            {
               
                context.Database.EnsureCreated();
                if (search.Ratings == 0)
                {
                    data = context.Map.Where(n => n.Name.Contains(search.Name) && (n.Difficulty == search.Difficulty) &&(n.PublicVisible==true)).OrderBy(n => n.Rating).ToList();
                }
                else {
                    data = context.Map.Where(n => n.Name.Contains(search.Name) && (n.Rating >= (decimal)search.Ratings) && (n.Difficulty == search.Difficulty) && (n.PublicVisible == true)).OrderBy(n => n.Rating).ToList();
                }
            }
            return data;
        }

        public List<Map> PopularSearch(Search search)
        {
            List<Map> data = new List<Map>();
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                
            }
            return data;
        }

        public void Update(Map map)
        {
            using (var context = new ApplicationDBContext())
            {
                var row = context.Map.Where(a => a.ID == map.ID).FirstOrDefault();
                if (row == null) return;
                row = map;
                context.Map.Update(row);
                context.SaveChanges();
            }
        }

        public void Delete(string ID)
        {
            throw new NotImplementedException();
        }

    }
}
