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
	[Tooltip("Off: Download data from a given URL.\nOn: Read data from the Resources folder.\n\nDisable this when building for WebGL!")]
	public bool localMode = false;

	private bool siteArgsDone = false;
	private bool downloadersDone = false;

	// Track vars
	[Space]
	public Track track;
	public string chartURL;
	private bool trackDone = false;

	// Metronome vars
	[Space]
	public Metronome metronome;
	public string audioURL;
	[Tooltip("Visual offset between note movement and audio.\nIncrease this if notes are coming too early,\nor decrease it if notes are coming too late.\n\nValues are factors of 1 millisecond.\nLowest possible value is -100.")]
	public double userOffset;
	private bool metronomeDone = false;

	// NoteSpawner vars
	[Space]
	public NoteSpawner noteSpawner;
	[Tooltip("Note scroll speed relative to chart-designated \"normal\" tempo.\n\nValues are factors of 100 BPM.\nLowest recommended value is 1.")]
	public float userSpeed;

	// InputController vars
		// BINDINGS GO HERE

	// Check if these ^^^^^ have been called during the Awake() loop

	void Awake()
	{
		// FIRST STEP: Recieve URLS and other small data from site.
		Debug.Log("[SiteHandler] Waiting for site to pass data (not really lol)...");
		// Do this later
		
		// TEMP SETTERS
		metronome.userOffset = userOffset / 1000.0;
		noteSpawner.userSpeed = userSpeed;

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
		if (!localMode)
		{
			UnityWebRequest www = UnityWebRequest.Get(chartURL);

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

		else
		{
			track.track_file = Resources.Load<TextAsset>(chartURL).ToString();
		}
	}

	IEnumerator InitMetronome()
	{
		if (!localMode)
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

		else
		{
			metronome.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(audioURL);
		}
	}
}
