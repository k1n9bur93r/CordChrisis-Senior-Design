﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using testServer.BOs.Interfaces;
using testServer.Models;

namespace testServer.BOs
{
    public class MapBO : IMapBO 
    {
		public Map GetMapData(string mapID) {
			try
			{
				throw new NotImplementedException();
			}
			catch (Exception ex)
			{
				Trace.TraceError("Error: " + ex.Message + "Unable to fetch Map details for Map: " + mapID);
				throw;
			}
        }

		public void PostNewMap(Map map)
		{
			try
			{
				throw new NotImplementedException();
			}
			catch (Exception ex)
			{
				Trace.TraceError("Error: "+ ex.Message+" Unable to post informaiton for map: " + map.Name+" "+map.ID );
				throw;
			}
		}

		public  void CreateNewMap(Map map)
		{
			try
			{
				throw new NotImplementedException();
			}
			catch (Exception ex )
			{
				Trace.TraceError("Error: " + ex.Message + "Unable to create new Map details for :" + map.ID);
				throw;
			}
		}

		public void UpdateMap(Map map)
		{
			try
			{
				throw new NotImplementedException();
			}
			catch (Exception ex)
			{
				Trace.TraceError("Error: " + ex.Message + "Unable to map: " + map.ID);
				throw;
			}
		}

	}
}