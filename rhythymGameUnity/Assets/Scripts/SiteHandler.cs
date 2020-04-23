using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/*
	> SiteHandler class

	Recieves data from website for use ingame.
	Do not add this object to any other scene besides Loader. It transfers itself via scene transitions.
*/

public class ArgumentsContainer
{
	public string audioLocation;
	//public string chartLocation;
	public bool gameMode;
	//public float userSpeed;
	//public double userOffset;
}

public class SiteHandler : MonoBehaviour
{
	[Tooltip("On: Download data from a given URL.\nOff: Read data from the Resources folder.\n\nEnable this when building for WebGL!")]
	public bool webMode;

	//[Tooltip("On: Ignore inspector and wait for settings from the site.\nOff: Use user settings from the inspector.\n\nThis option is ignored when Web Mode is disabled.\nEnable this when building for WebGL!")]
	//public bool waitForSettings;

	[Header("These options are ignored when Web Mode is enabled!")]
	[Tooltip("On: Launch game in play mode.\nOff: Launch game in editor.\n\nThis option is ignored when Wait For Settings is enabled.")]
	public bool gameMode;

	private bool infoDone;
	private bool chartDone;

	// Track vars
	public string chartLocation;
	[HideInInspector]
	public string chartFile;

	// Metronome vars
	public string audioLocation;
	[HideInInspector]
	public AudioClip audioFile;
	//[Tooltip("Visual offset between note movement and audio.\nIncrease this if notes are coming too early,\nor decrease it if notes are coming too late.\n\nValues are factors of 1 millisecond.\nLowest possible value is -100.")]
	[HideInInspector]
	public double userOffset;// = 0.0;

	// NoteSpawner vars
	//[Tooltip("Note scroll speed.\n\nValues are factors of 100 BPM.\nLowest recommended value is 1.\nValue must be above 0.")]
	[HideInInspector]
	public float userSpeed;//= 1.0f;

	// InputController vars
		// BINDINGS GO HERE

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject); // Makes it survives scene transitions

		// GROSS PUBLIC HACK
		userSpeed = 1.0f;
		userOffset = 0.0;

		GameObject loadingText = GameObject.Find("LoadText");
		loadingText.GetComponent<TextMeshProUGUI>().text = "";

		/*
		if (!webMode) //(!waitForSettings || !webMode)
		{
			userOffset = userOffset / 1000.0;
		}
		*/

		Debug.Log("[SiteHandler] Downloading...");

		StartCoroutine(StartDownloads());
	}

	void Update()
	{
		// Test site-waiting co-routines
		if (Input.GetKeyDown(KeyCode.G))
		{
			string mySettings =
			"{\"audioLocation\": \"https://se7enytes.github.io/Music/Lucky%20Star.mp3\", \"gameMode\": \"true\" }";
			//"{\"audioLocation\": \"https://se7enytes.github.io/Music/Lucky%20Star.mp3\", \"gameMode\": \"true\", \"userSpeed\": 1.0, \"userOffset\": 0.0 }";

			string myChart =
			"{\"title\": \"none\",\"artist\": \"none\",\"genre\": \"none\",\"tempo_normal\": 150.0,\"tempo_change_amount\": [150.0],\"tempo_change_beat\": [0.0],\"offset\": 0.0,\"difficulty\": \"none\",\"beats\": [2.0],\"notes\": [1]}";

			GetSiteInfo(mySettings);
			GetChartFromSite(myChart);
		}
	}

	IEnumerator StartDownloads()
	{
		// Get chart and user settings via argument
		if (webMode)
		{
			Coroutine site = StartCoroutine(WaitForSite());
			yield return site;
		}

		// Download audio
		Coroutine audio = StartCoroutine(GetAudio());
		yield return audio;

		Debug.Log("[SiteHandler] Downloads finished!");

		LoadNextScene();
	}

	IEnumerator WaitForSite()
	{
		while (!infoDone || !chartDone)
		{
			GameObject loadingText = GameObject.Find("LoadText");
			loadingText.GetComponent<TextMeshProUGUI>().text = "Waiting for response from site...";

			yield return null;
		}

		yield return true;
	}

	/*
		Recieves JSON stringified data.
		To be called by website.
	*/

	public void GetSiteInfo(string data)
	{
		ArgumentsContainer settings = JsonUtility.FromJson<ArgumentsContainer>(data);

		audioLocation = settings.audioLocation;
		//chartLocation = settings.chartLocation;
		gameMode = settings.gameMode;
		//userSpeed = (float)settings.userSpeed;
		//userOffset = settings.userOffset / 1000.0;

		infoDone = true;
	}

	public void GetChartFromSite(string data)
	{
		chartFile = data;

		chartDone = true;
	}

	/*
	IEnumerator GetChart()
	{
		UnityWebRequest www = UnityWebRequest.Get(chartLocation);
		
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
	*/

	IEnumerator GetAudio()
	{
		UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(audioLocation, AudioType.MPEG); // MP3
		
		if (webMode)
		{
			// Download the file and sit tight
			GameObject loadingText = GameObject.Find("LoadText");
			loadingText.GetComponent<TextMeshProUGUI>().text = "Get Ready!\n";

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
			int loadPercent = (int)(www.downloadProgress * 100);

			loadingText.GetComponent<TextMeshProUGUI>().text = originalText;
			loadingText.GetComponent<TextMeshProUGUI>().text += " " + loadPercent + "%";

			yield return new WaitForSeconds((1.0f / 30.0f));
		}
	}

	public void SetOptionsIngame(float speed, double offset)
	{
		userSpeed = speed;
		userOffset = offset / 1000.0;
	}

	private void LoadNextScene()
	{
		/*
		if (gameMode)
		{
			Initiate.Fade("Main Game", Color.black, 2.5f);
			//SceneManager.LoadScene("Main Game", LoadSceneMode.Single);
		}

		else
		{
			Initiate.Fade("NoteEditor", Color.black, 2.5f);
			//SceneManager.LoadScene("NoteEditor", LoadSceneMode.Single);
		}
		*/

		SceneManager.LoadScene("Options", LoadSceneMode.Single);
	}
}
