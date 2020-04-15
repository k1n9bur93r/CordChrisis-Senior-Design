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
	[Tooltip("On: Download data from a given URL.\nOff: Read data from the Resources folder.\n\nEnable this when building for WebGL!")]
	public bool webMode = false;

	[Tooltip("On: Launch the main game.\nOff: Launch the editor.\n\nThis option is ignored on a real WebGL build.")]
	public bool gameMode = false;

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
	[Tooltip("Note scroll speed relative to chart-designated \"normal\" tempo.\n\nValues are factors of 100 BPM.\nLowest recommended value is 1.\nValue must be above 0.")]
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
		if (webMode)
		{
			UnityWebRequest www = UnityWebRequest.Get(chartURL);
			www.SendWebRequest();
			
			// Download the file and sit tight
			while (!www.isDone)
			{
				Debug.Log("[SiteHandler]: JSON DL " + www.downloadProgress * 100.0 + "%");
				yield return null;
			}

			// Results of the download
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
		if (webMode)
		{
			UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(audioURL, AudioType.OGGVORBIS); // OGG

			Debug.Log("[SiteHandler]: Audio DL " + www.downloadProgress + "%");
			www.Send();

			// Download the file and sit tight
			while (!www.isDone)
			{
				Debug.Log("[SiteHandler]: JSON DL " + www.downloadProgress * 100.0 + "%");
				yield return null;
			}

			// Results of the download
			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log("[SiteHandler] InitMetronome(): " + www.error);
			}

			else
			{
				metronome.GetComponent<AudioSource>().clip = DownloadHandlerAudioClip.GetContent(www);
				metronomeDone = true;
			}
		}

		else
		{
			metronome.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(audioURL);
		}
	}
}
