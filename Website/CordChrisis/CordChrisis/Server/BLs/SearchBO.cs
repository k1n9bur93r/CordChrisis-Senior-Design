using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CordChrisis.BOs.Interfaces;
using CordChrisis.DAOs;
using CordChrisis.Shared.Models;

namespace CordChrisis.BOs
{ 
    public class SearchBO : ISearchBO
    {

    public List<Map> GetMapSearch(Search search)
    {
		try
		{
				MapDA mapSearch = new MapDA();
				return mapSearch.ReadMany(search);
		}
		catch (Exception ex)
		{
			Trace.TraceError("Error: " + ex.Message + "Unable to get Map search results");
			throw;
		}
	}

		public List<Map> GetPopularMaps()
		{
			try
			{
				MapDA mapSearch = new MapDA();
				return mapSearch.PopularSearch();
			}
			catch (Exception ex)
			{
				Trace.TraceError("Error: " + ex.Message + "Unable to get Map search results");
				throw;
			}
		}
	}
}


