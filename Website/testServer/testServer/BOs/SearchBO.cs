﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using testServer.BOs.Interfaces;
using testServer.Models;

namespace testServer.BOs
{ 
    public class SearchBO : ISearchBO
    {

    public List<Map> GetMapSearch(Search search)
    {
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			Trace.TraceError("Error: " + ex.Message + "Unable to get Map search results");
			throw;
		}
	}
    }
}