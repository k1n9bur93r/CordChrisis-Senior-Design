using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/*
	> Downloader class

	Downloads items from the internet.
	NOTICE: Do not use this in the actual game!
*/

public class Downloader : MonoBehaviour
{
	public string chartURL;
	public string audioURL;
	public AudioClip audioFile;
	public string jsonContent;

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject); // Makes it survives scene transitions
		StartCoroutine(StartDownloads());
	}

	IEnumerator StartDownloads()
	{
		Coroutine json = StartCoroutine(InitTrack());
		yield return json;
		Coroutine audio = StartCoroutine(InitMetronome());
		yield return audio;

		Debug.Log("doggy");

		SceneManager.LoadScene("Loader Test B", LoadSceneMode.Single);
		//Coroutine loaded = StartCoroutine("WaitForSceneLoad");
		//yield return loaded;

		/*
		GameObject target = GameObject.Find("DownloaderTester");
		if (target == null) { Debug.Log("Scene load failed!"); }

		target.GetComponent<DownloaderTester>().SetAudio(audioFile);
		*/
	}

	IEnumerator WaitForSceneLoad()
	{
		while (SceneManager.GetActiveScene().name != "Loader B")
		{
			yield return null;
		}

	}

	IEnumerator InitTrack()
	{
		UnityWebRequest www = UnityWebRequest.Get(chartURL);
		
		// Download the file and sit tight
		StartCoroutine(ProgressBar(www));
		yield return www.SendWebRequest();

		// Results of the download
		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log("[Downloader] InitTrack(): " + www.error);
		}

		else
		{
			Debug.Log("[Downloader] InitTrack(): done!");
			jsonContent = www.downloadHandler.text;
			//Debug.Log(www.downloadHandler.text);
		}
	}

	IEnumerator InitMetronome()
	{
		UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(audioURL, AudioType.OGGVORBIS); // OGG
		
		// Download the file and sit tight
		StartCoroutine(ProgressBar(www));
		yield return www.SendWebRequest();

		// Results of the download
		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log("[Downloader] InitMetronome(): " + www.error);
		}

		else
		{
			Debug.Log("[Downloader] InitMetronome(): done!");
			audioFile = DownloadHandlerAudioClip.GetContent(www);
		}
	}

	IEnumerator ProgressBar(UnityWebRequest www)
	{
		while (!www.isDone)
		{
			Debug.Log("[Downloader]: DL " + www.downloadProgress * 100.0 + "%");
			yield return new WaitForSeconds((1.0f / 30.0f));
		}
	}
}