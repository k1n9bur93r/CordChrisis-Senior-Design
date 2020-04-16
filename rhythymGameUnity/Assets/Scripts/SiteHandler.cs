﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/*
	> SiteHandler class

	Recieves data from website for use ingame.
*/

public class SiteHandler : MonoBehaviour
{
	[Tooltip("On: Download data from a given URL.\nOff: Read data from the Resources folder.\n\nEnable this when building for WebGL!")]
	public bool webMode;

	[Tooltip("On: Ignore inspector and wait for settings from the site.\nOff: Use user settings from the inspector.\n\nThis option is ignored when Web Mode is disabled.\nEnable this when building for WebGL!")]
	public bool waitForSettings;

	[Header("Non-Wait For Settings options")]
	[Tooltip("On: Launch game in play mode.\nOff: Launch game in editor.\n\nThis option is ignored when Wait For Settings is enabled.")]
	public bool gameMode;

	//private bool siteArgsDone = false;
	//private bool downloadersDone = false;
	private bool locationsDone;
	private bool settingsDone;

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

	// NoteSpawner vars
	[Tooltip("Note scroll speed.\n\nValues are factors of 100 BPM.\nLowest recommended value is 1.\nValue must be above 0.")]
	public float userSpeed;

	// InputController vars
		// BINDINGS GO HERE

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject); // Makes it survives scene transitions

		if (!waitForSettings)
		{
			userOffset = userOffset / 1000.0;
			locationsDone = true;
			settingsDone = true;
		}

		if (!webMode)
		{
			userOffset = userOffset / 1000.0;
			locationsDone = true;
			settingsDone = true;
		}

		Debug.Log("[SiteHandler] Downloading...");

		StartCoroutine(StartDownloads());
	}

	/*
	void Update()
	{
		// Test site-waiting co-routines
		if (Input.GetKeyDown(KeyCode.G))
		{
			GetSiteURL("https://se7enytes.github.io/Music/Lucky%20Star.ogg", "https://se7enytes.github.io/Charts/Lucky%20Star.json");
		}

		if (Input.GetKeyDown(KeyCode.H))
		{
			GetUserSettings(true, 2.0f, 0.0);
		}
	}
	*/

	IEnumerator StartDownloads()
	{
		// Get user settings
		Coroutine site = StartCoroutine(WaitForSite());
		yield return site;

		// Download the files (does not download in parallel, but the only big file will be audio anyway)
		Coroutine chart = StartCoroutine(GetChart());
		yield return chart;

		Coroutine audio = StartCoroutine(GetAudio());
		yield return audio;

		Debug.Log("[SiteHandler] Downloads finished!");

		LoadNextScene();
	}

	IEnumerator WaitForSite()
	{
		while (!locationsDone || !settingsDone)
		{
			GameObject loadingText = GameObject.Find("LoadText");
			loadingText.GetComponent<TextMeshProUGUI>().text = "Waiting for response from site...";

			yield return null;
		}

		yield return true;
	}

	public void GetSiteURL(string audio, string chart)
	{
		audioURL = audio;
		chartURL = chart;

		locationsDone = true;
	}

	public void GetUserSettings(bool mode, float speed, double offset)
	{
		gameMode = mode; // true = play, false = editor
		userSpeed = speed;
		userOffset = offset / 1000.0;

		settingsDone = true;
	}

	IEnumerator GetChart()
	{
		UnityWebRequest www = UnityWebRequest.Get(chartURL);
		
		if (webMode)
		{
			// Download the file and sit tight
			GameObject loadingText = GameObject.Find("LoadText");
			loadingText.GetComponent<TextMeshProUGUI>().text = "Loading chart: ";

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
			GameObject loadingText = GameObject.Find("LoadText");
			loadingText.GetComponent<TextMeshProUGUI>().text = "Loading music: ";

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
	}

	IEnumerator ProgressBar(UnityWebRequest www)
	{
		GameObject loadingText = GameObject.Find("LoadText");
		string originalText = loadingText.GetComponent<TextMeshProUGUI>().text;

		while (!www.isDone)
		{
			//Debug.Log("[Downloader]: Progress " + www.downloadProgress * 100.0 + "%");
			loadingText.GetComponent<TextMeshProUGUI>().text = originalText;

			int loadPercent = (int)(www.downloadProgress * 100);

			loadingText.GetComponent<TextMeshProUGUI>().text += " " + loadPercent + "%";

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
