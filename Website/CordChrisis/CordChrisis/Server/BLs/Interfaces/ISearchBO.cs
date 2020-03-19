using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CordChrisis.Shared.Models;

namespace CordChrisis.BOs.Interfaces
{
    interface ISearchBO
    {
        List<Map> GetMapSearch(Search search);
        List<Map> GetPopularMaps();
    }
}
