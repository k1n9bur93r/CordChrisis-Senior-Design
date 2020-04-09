using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
	> SiteHandler class

	Recieves audio, chart, and user options from the site for use in the game.
*/

public class SiteHandler : MonoBehaviour
{
	/*
	[Header("Check this while testing, uncheck before building")]
	public bool bypassWait = false;
	[Space]
	*/

	bool siteArgsDone = false;
	bool downloadersDone = false;

	// Track vars
	public Track track;
	public string jsonURL;
	private bool trackDone = false;

	// Metronome vars
	public Metronome metronome;
	public string audioURL;
	public double userOffset;
	private bool metronomeDone = false;

	// NoteSpawner vars
	public NoteSpawner noteSpawner;
	public float speedMod;

	// InputController vars
		// BINDINGS GO HERE

	// Check if these ^^^^^ have been called during the Awake() loop

	void Awake()
	{
		// FIRST STEP: 
		Debug.Log("[SiteHandler] Waiting for site to pass data (not really lol)...");
		// Do this later
		
		// TEMP SETTERS
		metronome.userOffset = userOffset;
		noteSpawner.speedMod = speedMod;

		siteArgsDone = true; // may or may not actually do anything

		// SECOND STEP: Start downloader co-routines.
		Debug.Log("[SiteHandler] Downloading...");

		StartCoroutine(InitTrack()); // JSON downloader
		StartCoroutine(InitMetronome()); // Audio downloader		
		
		/*
		do
		{
			if (metronomeDone && trackDone)
			{
				downloadersDone = true; // this goes here?
			}

		} while (!downloadersDone);
		*/

		Debug.Log("[SiteHandler] Done!");
	}

	IEnumerator InitTrack()
	{
		UnityWebRequest www = UnityWebRequest.Get(jsonURL);

		yield return www.SendWebRequest(); // Request and sit tight

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log("[SiteHandler] InitTrack(): " + www.error);
		}

		else
		{
			track.track_file = www.downloadHandler.text;
			trackDone = true;
		}
	}

	IEnumerator InitMetronome()
	{
		UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(audioURL, AudioType.MPEG); // MP3

		yield return www.Send(); //SendWebRequest(); // 

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log("[SiteHandler] InitMetronome(): " + www.error);
		}

		else
		{
			metronome.GetComponent<AudioSource>().clip = DownloadHandlerAudioClip.GetContent(www);
		}
	}
}
