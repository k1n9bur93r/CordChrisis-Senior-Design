﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CordChrisis.BOs.Interfaces;
using CordChrisis.DAOs;
using CordChrisis.Shared.Models;

namespace CordChrisis.BOs
{
    public class MapBO : IMapBO 
    {

		public Stream GetMapMusic(string ID)
		{
			try
			{
				MapMusicDA mapMusic = new MapMusicDA();
				return mapMusic.GetMapMusic(ID);
			}
			catch (Exception ex)
			{
				Trace.TraceError("Error: " + ex.Message + " Unable to post informaiton for map: ");
				throw;
			}

		}

		public Map GetMapData(string mapID) {
			try
			{
				MapDA mapda = new MapDA();
				return mapda.ReadSingle(mapID);
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
				MapDA mapDA = new MapDA();
				mapDA.Update(map);
			}
			catch (Exception ex)
			{
				Trace.TraceError("Error: " + ex.Message + "Unable to map: " + map.ID);
				throw;
			}
		}

	}
}
