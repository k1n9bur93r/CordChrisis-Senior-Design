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
                if (String.IsNullOrWhiteSpace(search.Name))
                {   
                    data = context.Map.Where(n => n.Rating >=(decimal)search.Ratings && (n.Difficulty == search.Difficulty) &&(n.PublicVisible==true)).OrderByDescending(n => n.Rating).ToList();
                }
                else if(search.Name!=String.Empty) {
                    data = context.Map.Where(n => n.Name.Contains(search.Name) && (n.Rating >= (decimal)search.Ratings) && (n.Difficulty == search.Difficulty) && (n.PublicVisible == true)).OrderByDescending(n => n.Rating).ToList();
                }
            }
            return data;
        }

        public List<Map> PopularSearch()
        {
            List<Map> data = new List<Map>();
            using (var context = new ApplicationDBContext())
            {
                context.Database.EnsureCreated();
                data = context.Map.Where(n => n.CreatedDate <= DateTime.Now && (n.CreatedDate > DateTime.Now.AddMonths(-1)) && (n.Rating > (decimal)3.0)).OrderBy(n => n.Rating).ToList();
            }
            return data;
        }

        public void Update(Map map)
        {
            using (var context = new ApplicationDBContext())
            {
                var row = context.Map.Where(a => a.ID == map.ID).FirstOrDefault();
                if (row == null) return;
                row.Rating = map.Rating;
                row.NumOfRatings = map.NumOfRatings;
                row.Plays = map.Plays;
                row.PublicVisible = map.PublicVisible;
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
