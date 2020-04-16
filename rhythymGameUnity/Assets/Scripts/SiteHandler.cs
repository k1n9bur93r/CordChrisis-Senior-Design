using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/*
	> SiteHandler class

	Recieves data from website for use ingame.
*/

public class SiteHandler : MonoBehaviour
{
	[Tooltip("On: Download data from a given URL.\nOff: Read data from the Resources folder.\n\nEnable this when building for WebGL!")]
	public bool webMode = true;

	[Tooltip("On: Launch game in play mode.\nOff: Launch game in editor.\n\nThis option is ignored on a real WebGL build due to the site setting this.")]
	public bool gameMode = true;

	//private bool siteArgsDone = false;
	//private bool downloadersDone = false;

	// Track vars
	public string chartURL;
	[HideInInspector]
	public string chartFile;

	// Metronome vars
	public string audioURL;
	[HideInInspector]
	public AudioClip audioFile;
	[Tooltip("Visual offset between note movement and audio.\nIncrease this if notes are coming too early,\nor decrease it if notes are coming too late.\n\nValues are factors of 1 millisecond.\nLowest possible value is -100.")]
	public double userOffset;
	private bool metronomeDone = false;

	// NoteSpawner vars
	[Tooltip("Note scroll speed relative to chart-designated \"normal\" tempo.\n\nValues are factors of 100 BPM.\nLowest recommended value is 1.\nValue must be above 0.")]
	public float userSpeed;

	// InputController vars
		// BINDINGS GO HERE

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject); // Makes it survives scene transitions

		// FIRST STEP: Recieve URLS and other small data from site
		Debug.Log("[SiteHandler] Waiting for site to pass data (not really lol)...");

		// TEMP SETTERS
		//metronome.userOffset = userOffset / 1000.0;
		//noteSpawner.userSpeed = userSpeed;

		//siteArgsDone = true; // may or may not actually do anything

		// SECOND STEP: Start downloader co-routines
		Debug.Log("[SiteHandler] Downloading...");

		StartCoroutine(StartDownloads());

		//Debug.Log("[SiteHandler] All done!");
	}

	IEnumerator StartDownloads()
	{
		// Download the files (not parallel, but the only big file will be audio anyway)
		Coroutine chart = StartCoroutine(GetChart());
		yield return chart;
		Coroutine audio = StartCoroutine(GetAudio());
		yield return audio;

		Debug.Log("[SiteHandler] Downloads finished!");

		LoadNextScene();
	}

	IEnumerator GetChart()
	{
		UnityWebRequest www = UnityWebRequest.Get(chartURL);
		
		if (webMode)
		{
			// Download the file and sit tight
			StartCoroutine(ProgressBar(www));
			yield return www.SendWebRequest();

			// Results of the download
			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log("[Downloader] GetChart(): " + www.error);
			}

			else
			{
				Debug.Log("[Downloader] GetChart(): done!");
				chartFile = www.downloadHandler.text;
			}
		}
	}

	IEnumerator GetAudio()
	{
		UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(audioURL, AudioType.OGGVORBIS); // OGG
		
		if (webMode)
		{
			// Download the file and sit tight
			StartCoroutine(ProgressBar(www));
			yield return www.SendWebRequest();

			// Results of the download
			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log("[Downloader] GetAudio(): " + www.error);
			}

			else
			{
				Debug.Log("[Downloader] GetAudio(): done!");
				audioFile = DownloadHandlerAudioClip.GetContent(www);
			}
		}

		else
		{
			// NO.
			//metronome.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(audioURL);
		}
	}

	IEnumerator ProgressBar(UnityWebRequest www)
	{
		while (!www.isDone)
		{
			Debug.Log("[Downloader]: Progress " + www.downloadProgress * 100.0 + "%");
			yield return new WaitForSeconds((1.0f / 30.0f));
		}
	}

	private void LoadNextScene()
	{
		if (gameMode)
		{
			SceneManager.LoadScene("Main Game", LoadSceneMode.Single);
		}

		else
		{
			SceneManager.LoadScene("NoteEditor", LoadSceneMode.Single);
		}
	}
}
