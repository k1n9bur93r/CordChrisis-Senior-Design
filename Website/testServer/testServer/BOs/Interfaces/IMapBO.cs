using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testServer.Models;

namespace testServer.BOs.Interfaces
{
    public interface IMapBO
    {
        Map GetMapData(string ID);

        void PostNewMap(Map newMap);

        void CreateNewMap(Map newMap);
        void UpdateMap(Map newMap);
       
    }
}
