using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testServer.Models;

namespace testServer.BOs.Interfaces
{
    interface ISearchBO
    {
        List<Map> GetMapSearch(Search search);
    }
}
