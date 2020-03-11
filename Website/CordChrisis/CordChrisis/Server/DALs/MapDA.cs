using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CordChrisis.Models;

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
                data = context.Map.Where(a => a.ID == mapID).FirstOrDefault();
            }
            return data;
        }

        public List<Map> ReadMany(Search search)
        {
            throw new NotImplementedException();
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
